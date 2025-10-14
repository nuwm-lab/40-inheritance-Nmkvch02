using System;

namespace Geometry
{
    public class Triangle
    {
        private double _sideA, _sideB, _sideC, _angleAlpha, _angleBeta, _angleGamma;
        private const double Epsilon = 1e-10;
        
        public double SideA => _sideA;
        public double SideB => _sideB;
        public double SideC => _sideC;
        public double AngleAlpha => _angleAlpha;
        public double AngleBeta => _angleBeta;
        public double AngleGamma => _angleGamma;

        protected Triangle() { }

        public static Triangle FromThreeSides(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c <= 0) 
                throw new ArgumentException("Сторони мають бути додатні");
            if (a + b <= c || a + c <= b || b + c <= a) 
                throw new ArgumentException("Не трикутник");
            
            var t = new Triangle();
            t._sideA = a; 
            t._sideB = b; 
            t._sideC = c;
            t.CalculateAngles();
            return t;
        }

        protected virtual void CalculateAngles()
        {
            if (Math.Abs(2 * _sideB * _sideC) < Epsilon ||
                Math.Abs(2 * _sideA * _sideC) < Epsilon ||
                Math.Abs(2 * _sideA * _sideB) < Epsilon)
                throw new InvalidOperationException("Неможливо обчислити кути");
            
            double cosA = (_sideB*_sideB + _sideC*_sideC - _sideA*_sideA) / (2*_sideB*_sideC);
            double cosB = (_sideA*_sideA + _sideC*_sideC - _sideB*_sideB) / (2*_sideA*_sideC);
            double cosC = (_sideA*_sideA + _sideB*_sideB - _sideC*_sideC) / (2*_sideA*_sideB);
            
            _angleAlpha = Math.Acos(Math.Max(-1, Math.Min(1, cosA))) * 180 / Math.PI;
            _angleBeta = Math.Acos(Math.Max(-1, Math.Min(1, cosB))) * 180 / Math.PI;
            _angleGamma = Math.Acos(Math.Max(-1, Math.Min(1, cosC))) * 180 / Math.PI;
        }

        public override string ToString()
        {
            return $"Трикутник: сторони {_sideA:F2}, {_sideB:F2}, {_sideC:F2}; кути {_angleAlpha:F2}°, {_angleBeta:F2}°, {_angleGamma:F2}°";
        }
    }

    public class RightTriangle : Triangle
    {
        public RightTriangle(double leg1, double leg2)
        {
            if (leg1 <= 0 || leg2 <= 0) 
                throw new ArgumentException("Катети мають бути додатні");
            
            _sideA = leg1; 
            _sideB = leg2;
            CalculateThirdSide();
            _angleGamma = 90;
            CalculateAngles();
        }

        private void CalculateThirdSide()
        {
            _sideC = Math.Sqrt(_sideA * _sideA + _sideB * _sideB);
        }

        protected override void CalculateAngles()
        {
            if (Math.Abs(_sideB) < Epsilon)
                throw new InvalidOperationException("Неможливо обчислити кути");
                
            _angleAlpha = Math.Atan(_sideA / _sideB) * 180 / Math.PI;
            _angleBeta = 90 - _angleAlpha;
        }

        public override string ToString()
        {
            return $"Прямокутний: катети {_sideA:F2}, {_sideB:F2}; гіпотенуза {_sideC:F2}; кути {_angleAlpha:F2}°, {_angleBeta:F2}°, {_angleGamma:F2}°";
        }
    }

    public class IsoscelesTriangle : Triangle
    {
        public IsoscelesTriangle(double baseLength, double leg)
        {
            if (baseLength <= 0 || leg <= 0)
                throw new ArgumentException("Сторони мають бути додатні");
            if (2 * leg <= baseLength)
                throw new ArgumentException("Не трикутник");
            
            _sideA = leg;
            _sideB = baseLength;
            _sideC = leg;
            CalculateAngles();
        }

        protected override void CalculateAngles()
        {
            if (Math.Abs(2 * _sideA * _sideB) < Epsilon)
                throw new InvalidOperationException("Неможливо обчислити кути");
            
            double cosBase = (_sideA * _sideA + _sideA * _sideA - _sideB * _sideB) / (2 * _sideA * _sideA);
            double cosLeg = (_sideB * _sideB + _sideA * _sideA - _sideA * _sideA) / (2 * _sideB * _sideA);
            
            _angleAlpha = Math.Acos(Math.Max(-1, Math.Min(1, cosLeg))) * 180 / Math.PI;
            _angleBeta = Math.Acos(Math.Max(-1, Math.Min(1, cosBase))) * 180 / Math.PI;
            _angleGamma = _angleAlpha;
        }

        public bool IsApexAngleObtuse()
        {
            return _angleBeta > 90;
        }

        public double GetHeight()
        {
            return Math.Sqrt(_sideA * _sideA - (_sideB / 2) * (_sideB / 2));
        }

        public override string ToString()
        {
            return $"Рівнобедрений: основа {_sideB:F2}, бічні {_sideA:F2}; кути {_angleAlpha:F2}°, {_angleBeta:F2}°, {_angleGamma:F2}°";
        }
    }

    public class EquilateralTriangle : Triangle
    {
        public EquilateralTriangle(double side)
        {
            if (side <= 0)
                throw new ArgumentException("Сторона має бути додатна");
            
            _sideA = side;
            _sideB = side;
            _sideC = side;
            _angleAlpha = 60;
            _angleBeta = 60;
            _angleGamma = 60;
        }

        public double GetHeight()
        {
            return _sideA * Math.Sqrt(3) / 2;
        }

        public override string ToString()
        {
            return $"Рівносторонній: сторона {_sideA:F2}; кути 60.00°, 60.00°, 60.00°";
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine(new RightTriangle(3, 4));
                Console.WriteLine(Triangle.FromThreeSides(5, 5, 5));
                Console.WriteLine(new IsoscelesTriangle(6, 5));
                Console.WriteLine(new EquilateralTriangle(4));
                
                var isosceles = new IsoscelesTriangle(8, 5);
                Console.WriteLine($"Висота: {isosceles.GetHeight():F2}");
                Console.WriteLine($"Тупий кут при вершині: {isosceles.IsApexAngleObtuse()}");
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"Помилка: {ex.Message}"); 
            }
        }
    }
}
