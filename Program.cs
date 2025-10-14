using System;

namespace Geometry
{
    public class Triangle
    {
        private double _sideA, _sideB, _sideC, _angleAlpha, _angleBeta, _angleGamma;
        
        protected double SideA { get => _sideA; set => _sideA = value; }
        protected double SideB { get => _sideB; set => _sideB = value; }
        protected double SideC { get => _sideC; set => _sideC = value; }

        public static Triangle FromThreeSides(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c <= 0) throw new ArgumentException("Сторони мають бути додатні");
            if (a + b <= c || a + c <= b || b + c <= a) throw new ArgumentException("Не трикутник");
            
            var t = new Triangle();
            t._sideA = a; t._sideB = b; t._sideC = c;
            t.CalculateAngles();
            return t;
        }

        protected virtual void CalculateAngles()
        {
            if (2 * _sideB * _sideC == 0) return;
            
            double cosA = (_sideB*_sideB + _sideC*_sideC - _sideA*_sideA) / (2*_sideB*_sideC);
            double cosB = (_sideA*_sideA + _sideC*_sideC - _sideB*_sideB) / (2*_sideA*_sideC);
            double cosC = (_sideA*_sideA + _sideB*_sideB - _sideC*_sideC) / (2*_sideA*_sideB);
            
            _angleAlpha = Math.Acos(Math.Max(-1, Math.Min(1, cosA))) * 180 / Math.PI;
            _angleBeta = Math.Acos(Math.Max(-1, Math.Min(1, cosB))) * 180 / Math.PI;
            _angleGamma = Math.Acos(Math.Max(-1, Math.Min(1, cosC))) * 180 / Math.PI;
        }

        public virtual void Print()
        {
            Console.WriteLine($"Трикутник: сторони {_sideA:F1}, {_sideB:F1}, {_sideC:F1}");
        }
    }

    public class RightTriangle : Triangle
    {
        public RightTriangle(double leg1, double leg2)
        {
            if (leg1 <= 0 || leg2 <= 0) throw new ArgumentException("Катети мають бути додатні");
            
            SideA = leg1; SideB = leg2;
            CalculateThirdSide();
            CalculateAngles();
        }

        protected void CalculateThirdSide()
        {
            SideC = Math.Sqrt(SideA * SideA + SideB * SideB);
        }

        protected override void CalculateAngles()
        {
            if (SideB != 0)
                base.CalculateAngles();
        }

        public override void Print()
        {
            Console.WriteLine($"Прямокутний: {SideA:F1}, {SideB:F1}, {SideC:F1}");
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                new RightTriangle(3, 4).Print();
                Triangle.FromThreeSides(5, 5, 5).Print();
            }
            catch (Exception ex) { Console.WriteLine($"Помилка: {ex.Message}"); }
        }
    }
}
