using System;

// Базовий клас "прямокутний трикутник"
public class RightTriangle
{
    protected double a, b; // катети
    protected double c;    // гіпотенуза

    public RightTriangle(double leg1, double leg2)
    {
        if (leg1 <= 0 || leg2 <= 0)
            throw new ArgumentException("Довжини катетів повинні бути додатними числами");
        
        a = leg1;
        b = leg2;
        c = Math.Sqrt(a * a + b * b);
    }

    public virtual double CalculatePerimeter()
    {
        return a + b + c;
    }

    public virtual void PrintCharacteristics()
    {
        Console.WriteLine("Прямокутний трикутник:");
        Console.WriteLine($"Катет a = {a:F2}");
        Console.WriteLine($"Катет b = {b:F2}");
        Console.WriteLine($"Гіпотенуза c = {c:F2}");
        Console.WriteLine($"Кут α (проти катета a) = {CalculateAngleAlpha():F2}°");
        Console.WriteLine($"Кут β (проти катета b) = {CalculateAngleBeta():F2}°");
        Console.WriteLine($"Периметр = {CalculatePerimeter():F2}");
        Console.WriteLine();
    }

    protected double CalculateAngleAlpha()
    {
        return Math.Atan(a / b) * (180 / Math.PI);
    }

    protected double CalculateAngleBeta()
    {
        return Math.Atan(b / a) * (180 / Math.PI);
    }
}

public class Triangle : RightTriangle
{
    private double angleGamma; // кут між сторонами a та b (в градусах)

    public Triangle(double side1, double side2, double angleBetween) 
        : base(side1, side2)
    {
        if (angleBetween <= 0 || angleBetween >= 180)
            throw new ArgumentException("Кут між сторонами повинен бути в межах (0, 180) градусів");
        
        angleGamma = angleBetween;
        RecalculateSides();
    }

    public void SetValues(double side1, double side2, double angle1, double angle2)
    {
        if (side1 <= 0 || side2 <= 0)
            throw new ArgumentException("Довжини сторін повинні бути додатними числами");
        
        if (angle1 <= 0 || angle2 <= 0 || angle1 + angle2 >= 180)
            throw new ArgumentException("Сума двох кутів повинна бути менше 180 градусів");

        a = side1;
        b = side2;
        angleGamma = 180 - angle1 - angle2;
        RecalculateSides();
    }

    public override double CalculatePerimeter()
    {
        return a + b + c;
    }

    public override void PrintCharacteristics()
    {
        Console.WriteLine("Трикутник:");
        Console.WriteLine($"Сторона a = {a:F2}");
        Console.WriteLine($"Сторона b = {b:F2}");
        Console.WriteLine($"Сторона c = {c:F2}");
        Console.WriteLine($"Кут α = {CalculateAngleAlpha():F2}°");
        Console.WriteLine($"Кут β = {CalculateAngleBeta():F2}°");
        Console.WriteLine($"Кут γ = {angleGamma:F2}°");
        Console.WriteLine($"Периметр = {CalculatePerimeter():F2}");
        Console.WriteLine();
    }

    protected new double CalculateAngleAlpha()
    {
        double cosAlpha = (b * b + c * c - a * a) / (2 * b * c);
        return Math.Acos(cosAlpha) * (180 / Math.PI);
    }

    protected new double CalculateAngleBeta()
    {
        double cosBeta = (a * a + c * c - b * b) / (2 * a * c);
        return Math.Acos(cosBeta) * (180 / Math.PI);
    }

    private void RecalculateSides()
    {
        double angleRad = angleGamma * Math.PI / 180;
        c = Math.Sqrt(a * a + b * b - 2 * a * b * Math.Cos(angleRad));
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

            Console.WriteLine("=== Довільний трикутник ===");
            Triangle triangle = new Triangle(5, 6, 60); // сторони 5, 6 та кут 60° між ними
            triangle.PrintCharacteristics();

            Console.WriteLine("=== Трикутник (перевантажений метод) ===");
            triangle.SetValues(7, 8, 45, 60); // сторони 7, 8 та кути 45° і 60°
            triangle.PrintCharacteristics();

            Console.WriteLine("=== Ще один трикутник ===");
            Triangle triangle2 = new Triangle(10, 12, 30);
            triangle2.PrintCharacteristics();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }
}
