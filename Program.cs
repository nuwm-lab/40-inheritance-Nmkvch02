using System;

namespace Geometry
{
    // Базовий клас "трикутник"
    public class Triangle
    {
        protected double _sideA;
        protected double _sideB;
        protected double _sideC;
        protected double _angleAlpha;
        protected double _angleBeta;
        protected double _angleGamma;

        public double SideA => _sideA;
        public double SideB => _sideB;
        public double SideC => _sideC;
        public double AngleAlpha => _angleAlpha;
        public double AngleBeta => _angleBeta;
        public double AngleGamma => _angleGamma;

        // Захищений конструктор для внутрішнього використання
        protected Triangle() { }

        // Фабричні методи замість конструкторів з однаковими сигнатурами
        public static Triangle FromThreeSides(double sideA, double sideB, double sideC)
        {
            var triangle = new Triangle();
            triangle.InitializeFromThreeSides(sideA, sideB, sideC);
            return triangle;
        }

        public static Triangle FromTwoSidesAndAngle(double side1, double side2, double angleBetween)
        {
            var triangle = new Triangle();
            triangle.InitializeFromTwoSidesAndAngle(side1, side2, angleBetween);
            return triangle;
        }

        public static Triangle FromTwoAnglesAndSide(double side, double angle1, double angle2)
        {
            var triangle = new Triangle();
            triangle.InitializeFromTwoAnglesAndSide(side, angle1, angle2);
            return triangle;
        }

        private void InitializeFromThreeSides(double sideA, double sideB, double sideC)
        {
            if (sideA <= 0 || sideB <= 0 || sideC <= 0)
                throw new ArgumentException("Довжини сторін повинні бути додатними числами");

            if (!IsValidTriangle(sideA, sideB, sideC))
                throw new ArgumentException("Сторони не утворюють трикутник (порушена трикутникова нерівність)");

            _sideA = sideA;
            _sideB = sideB;
            _sideC = sideC;
            CalculateAngles();
        }

        private void InitializeFromTwoSidesAndAngle(double side1, double side2, double angleBetween)
        {
            if (side1 <= 0 || side2 <= 0)
                throw new ArgumentException("Довжини сторін повинні бути додатними числами");

            if (angleBetween <= 0 || angleBetween >= 180)
                throw new ArgumentException("Кут між сторонами повинен бути в межах (0, 180) градусів");

            _sideA = side1;
            _sideB = side2;
            _angleGamma = angleBetween;
            CalculateThirdSide();
            
            if (!IsValidTriangle(_sideA, _sideB, _sideC))
                throw new ArgumentException("Задані параметри не утворюють трикутник");
                
            CalculateAngles();
        }

        private void InitializeFromTwoAnglesAndSide(double side, double angle1, double angle2)
        {
            if (side <= 0)
                throw new ArgumentException("Довжина сторони повинна бути додатним числом");
            
            if (angle1 <= 0 || angle2 <= 0 || angle1 + angle2 >= 180)
                throw new ArgumentException("Сума двох кутів повинна бути менше 180 градусів");

            _sideA = side;
            _angleAlpha = angle1;
            _angleBeta = angle2;
            _angleGamma = 180 - angle1 - angle2;
            
            CalculateSidesFromAngles();
            
            if (!IsValidTriangle(_sideA, _sideB, _sideC))
                throw new ArgumentException("Задані параметри не утворюють трикутник");
        }

        // Перевірка трикутникової нерівності
        protected virtual bool IsValidTriangle(double a, double b, double c)
        {
            return a + b > c && a + c > b && b + c > a;
        }

        // Обчислення третьої сторони за теоремою косинусів
        protected virtual void CalculateThirdSide()
        {
            double angleRad = _angleGamma * Math.PI / 180;
            _sideC = Math.Sqrt(_sideA * _sideA + _sideB * _sideB - 2 * _sideA * _sideB * Math.Cos(angleRad));
        }

        // Обчислення сторін за теоремою синусів
        protected virtual void CalculateSidesFromAngles()
        {
            // Теорема синусів: a/sinA = b/sinB = c/sinC
            double ratio = _sideA / Math.Sin(_angleAlpha * Math.PI / 180);
            _sideB = ratio * Math.Sin(_angleBeta * Math.PI / 180);
            _sideC = ratio * Math.Sin(_angleGamma * Math.PI / 180);
        }

        // Обчислення кутів за теоремою косинусів
        protected virtual void CalculateAngles()
        {
            // Обмежуємо значення для арккосинуса
            double cosAlpha = (_sideB * _sideB + _sideC * _sideC - _sideA * _sideA) / (2 * _sideB * _sideC);
            cosAlpha = Math.Max(-1, Math.Min(1, cosAlpha));
            
            double cosBeta = (_sideA * _sideA + _sideC * _sideC - _sideB * _sideB) / (2 * _sideA * _sideC);
            cosBeta = Math.Max(-1, Math.Min(1, cosBeta));
            
            double cosGamma = (_sideA * _sideA + _sideB * _sideB - _sideC * _sideC) / (2 * _sideA * _sideB);
            cosGamma = Math.Max(-1, Math.Min(1, cosGamma));

            _angleAlpha = Math.Acos(cosAlpha) * (180 / Math.PI);
            _angleBeta = Math.Acos(cosBeta) * (180 / Math.PI);
            _angleGamma = Math.Acos(cosGamma) * (180 / Math.PI);
        }

        // Метод обчислення периметра
        public virtual double CalculatePerimeter()
        {
            return _sideA + _sideB + _sideC;
        }

        // Перевантажений метод для задання значень через довжини двох сторін та два кути
        public virtual void SetValues(double side1, double side2, double angle1, double angle2)
        {
            if (side1 <= 0 || side2 <= 0)
                throw new ArgumentException("Довжини сторін повинні бути додатними числами");
            
            if (angle1 <= 0 || angle2 <= 0 || angle1 + angle2 >= 180)
                throw new ArgumentException("Сума двох кутів повинна бути менше 180 градусів");

            _sideA = side1;
            _sideB = side2;
            _angleAlpha = angle1;
            _angleBeta = angle2;
            _angleGamma = 180 - angle1 - angle2;
            
            CalculateThirdSide();
            RecalculateAllAngles();
        }

        protected virtual void RecalculateAllAngles()
        {
            CalculateAngles();
        }

        // Метод для виводу характеристик
        public virtual void PrintCharacteristics()
        {
            Console.WriteLine("Трикутник:");
            Console.WriteLine($"Сторона a = {_sideA:F2}");
            Console.WriteLine($"Сторона b = {_sideB:F2}");
            Console.WriteLine($"Сторона c = {_sideC:F2}");
            Console.WriteLine($"Кут α = {_angleAlpha:F2}°");
            Console.WriteLine($"Кут β = {_angleBeta:F2}°");
            Console.WriteLine($"Кут γ = {_angleGamma:F2}°");
            Console.WriteLine($"Периметр = {CalculatePerimeter():F2}");
            Console.WriteLine();
        }
    }

    // Похідний клас "прямокутний трикутник"
    public class RightTriangle : Triangle
    {
        // Конструктор для прямокутного трикутника
        public RightTriangle(double leg1, double leg2)
        {
            if (leg1 <= 0 || leg2 <= 0)
                throw new ArgumentException("Довжини катетів повинні бути додатними числами");
            
            _sideA = leg1;
            _sideB = leg2;
            CalculateThirdSide();
            _angleGamma = 90;
            CalculateAnglesForRightTriangle();
        }

        // Специфічне обчислення кутів для прямокутного трикутника
        protected virtual void CalculateAnglesForRightTriangle()
        {
            _angleAlpha = Math.Atan(_sideA / _sideB) * (180 / Math.PI);
            _angleBeta = 90 - _angleAlpha;
        }

        protected override void CalculateThirdSide()
        {
            // Для прямокутного трикутника використовуємо теорему Піфагора
            _sideC = Math.Sqrt(_sideA * _sideA + _sideB * _sideB);
        }

        protected override void CalculateAngles()
        {
            CalculateAnglesForRightTriangle();
        }

        protected override void RecalculateAllAngles()
        {
            CalculateAnglesForRightTriangle();
        }

        public override void PrintCharacteristics()
        {
            Console.WriteLine("Прямокутний трикутник:");
            Console.WriteLine($"Катет a = {_sideA:F2}");
            Console.WriteLine($"Катет b = {_sideB:F2}");
            Console.WriteLine($"Гіпотенуза c = {_sideC:F2}");
            Console.WriteLine($"Кут α (проти катета a) = {_angleAlpha:F2}°");
            Console.WriteLine($"Кут β (проти катета b) = {_angleBeta:F2}°");
            Console.WriteLine($"Кут γ (прямий кут) = {_angleGamma:F2}°");
            Console.WriteLine($"Периметр = {CalculatePerimeter():F2}");
            Console.WriteLine();
        }

        // Перевизначення методу SetValues для прямокутного трикутника
        public override void SetValues(double side1, double side2, double angle1, double angle2)
        {
            if (Math.Abs(90 - (angle1 + angle2)) > 0.001)
                throw new ArgumentException("Для прямокутного трикутника сума двох кутів повинна дорівнювати 90 градусів");

            _sideA = side1;
            _sideB = side2;
            _angleAlpha = angle1;
            _angleBeta = angle2;
            _angleGamma = 90;
            
            CalculateThirdSide();
            CalculateAnglesForRightTriangle();
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("=== Прямокутний трикутник ===");
                RightTriangle rightTriangle = new RightTriangle(3, 4);
                rightTriangle.PrintCharacteristics();

                Console.WriteLine("=== Довільний трикутник (дві сторони і кут) ===");
                Triangle triangle1 = Triangle.FromTwoSidesAndAngle(5, 6, 60);
                triangle1.PrintCharacteristics();

                Console.WriteLine("=== Довільний трикутник (три сторони) ===");
                Triangle triangle2 = Triangle.FromThreeSides(7, 8, 9);
                triangle2.PrintCharacteristics();

                Console.WriteLine("=== Трикутник (два кути і сторона) ===");
                Triangle triangle3 = Triangle.FromTwoAnglesAndSide(10, 45, 60);
                triangle3.PrintCharacteristics();

                Console.WriteLine("=== Трикутник (перевантажений метод) ===");
                Triangle triangle4 = Triangle.FromTwoSidesAndAngle(10, 12, 50);
                triangle4.SetValues(7, 8, 45, 60);
                triangle4.PrintCharacteristics();

                // Демонстрація поліморфізму
                Console.WriteLine("=== Поліморфізм ===");
                Triangle[] triangles = new Triangle[]
                {
                    new RightTriangle(6, 8),
                    Triangle.FromThreeSides(5, 5, 5),
                    Triangle.FromTwoSidesAndAngle(4, 7, 45)
                };

                foreach (var triangle in triangles)
                {
                    triangle.PrintCharacteristics();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
