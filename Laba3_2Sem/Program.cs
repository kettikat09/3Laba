//Скласти опис класу для трикутника. Зберігає координати вершин трикутника на площині.
//Методи: чи рівні два трикутники, площа, периметр, висоти, медіани, бісектриси, радіус вписаного, радіус описаного кола, тип трикутника
//(рівносторонній, рівнобедрений, прямокутний, гострокутний, тупокутний), поворот на заданий кут відносно (однієї з вершин, центру описаного кола).
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
public class Program
{
    public static void Main()
    {
        double[] A = { 1, 2, };
        double[] B = { 4, 3 };
        double[] C = { 2, 5 };
        Triangle tr1 = new Triangle(A, B, C);
        Triangle tr2 = new Triangle(A, B, C);

        tr1.InfoWithRotate();

        Console.WriteLine(tr1.Compare(tr2));

        string jsonString = File.ReadAllText("data.json");

        Triangle tr = JsonConvert.DeserializeObject<Triangle>(jsonString);



    }
}
public class Triangle
{
    // (Y,X)
    public double[] A;
    public double[] B;
    public double[] C;

    public double CA_Length;
    public double AB_Length;
    public double BC_Length;

    public double Perimetr;
    public double Square;

    public double A_Hight;
    public double B_Hight;
    public double C_Hight;

    public double[] A_Bisekt;
    public double[] B_Bisekt;
    public double[] C_Bisekt;

    public double[] A_Median;
    public double[] B_Median;
    public double[] C_Median;

    public double Inside_Circle;
    public double Outside_Circle;

    public string Triangle_Type;
    public Triangle(double[] a, double[] b, double[] c)
    {
        A_Bisekt = new double[2];
        B_Bisekt = new double[2];
        C_Bisekt = new double[2];

        A_Median = new double[2];
        B_Median = new double[2];
        C_Median = new double[2];

        A = a;
        B = b;
        C = c;
        AB_Length = Math.Sqrt(Math.Pow((A[0] - B[0]), 2) + Math.Pow((A[1] - B[1]), 2));
        BC_Length = Math.Sqrt(Math.Pow((C[0] - B[0]), 2) + Math.Pow((C[1] - B[1]), 2));
        CA_Length = Math.Sqrt(Math.Pow((A[0] - C[0]), 2) + Math.Pow((A[1] - C[1]), 2));

        Perimetr = (AB_Length + BC_Length + CA_Length);
        double p = Perimetr / 2;
        Square = Math.Sqrt((p * (p - AB_Length) * (p - BC_Length) * (p - CA_Length)));

        A_Hight = 2 * Square / BC_Length;
        B_Hight = 2 * Square / CA_Length;
        C_Hight = 2 * Square / AB_Length;

        A_Bisekt[1] = (B[1] + C[1]) / ((CA_Length / (AB_Length + CA_Length)) + (AB_Length / (AB_Length + CA_Length)));
        A_Bisekt[0] = (B[0] + C[0]) / ((CA_Length / (AB_Length + CA_Length)) + (AB_Length / (AB_Length + CA_Length)));

        B_Bisekt[1] = (A[1] + C[1]) / ((BC_Length / (AB_Length + BC_Length)) + (AB_Length / (AB_Length + BC_Length)));
        B_Bisekt[0] = (A[0] + C[0]) / ((BC_Length / (AB_Length + BC_Length)) + (AB_Length / (AB_Length + BC_Length)));

        C_Bisekt[1] = (A[1] + B[1]) / ((BC_Length / (BC_Length + CA_Length)) + (AB_Length / (BC_Length + CA_Length)));
        C_Bisekt[0] = (A[0] + B[0]) / ((BC_Length / (BC_Length + CA_Length)) + (AB_Length / (BC_Length + CA_Length)));

        A_Median[1] = (B[1] + C[1]) / 2;
        A_Median[0] = (B[0] + C[0]) / 2;

        B_Median[1] = (A[1] + C[1]) / 2;
        B_Median[0] = (A[0] + C[0]) / 2;

        C_Median[1] = (A[1] + B[1]) / 2;
        C_Median[0] = (A[0] + B[0]) / 2;

        Inside_Circle = Square / (Perimetr / 2);
        Outside_Circle = (AB_Length * CA_Length * BC_Length) / (4 * Square);

        Triangle_Type = TriangleType();
    }
    public string Info() => $"Вершини трикутника: А({A[1]}, {A[0]}), В({B[1]}, {B[0]}), С({C[1]}, {C[0]})\n" +
        $"Площа: {Square}\nПериметр: {Perimetr}\nКоординати бісектриси:\nA - ({A_Bisekt[1]}, {A_Bisekt[0]})\nB - ({B_Bisekt[1]}, {B_Bisekt[0]})\nC - ({C_Bisekt[1]}, {C_Bisekt[0]})\n" +
        $"Висота з точки:\nA - {A_Hight}\nB - {B_Hight}\nC - {C_Hight}\n" +
        $"Координати медіани: \nA - ({A_Median[1]}, {A_Median[0]})\nB - ({B_Median[1]}, {B_Median[0]})\nC - ({C_Median[1]}, {C_Median[0]})\n" +
        $"Радиус вписаного кола: {Inside_Circle}\nРадиус oписаного кола: {Outside_Circle}\n{Triangle_Type}";
    public void InfoWithRotate()
    {
        Console.WriteLine(Info());
        Rotate();
    }
    void Rotate()
    {
        Console.WriteLine("Поворот відносно заданої вершини.");

        Console.Write("Введіть вершинy (A, B або C): ");
        string sw = Console.ReadLine();
        double x, y;
        switch (sw)
        {
            case "A":
                x = A[1];
                y = A[0];
                break;
            case "B":
                x = B[1];
                y = B[0];
                break;
            case "C":
                x = C[1];
                y = C[0];
                break;
            default:
                Console.WriteLine("Некоректний номер вершини.");
                Console.ReadKey();
                return;
        }

        Console.WriteLine("Введіть кут повороту в градусах:");
        double angle = double.Parse(Console.ReadLine());

        double angleInRadians = angle * Math.PI / 180;
        double newX1 = x + (A[1] - x) * Math.Cos(angleInRadians) - (A[0] - y) * Math.Sin(angleInRadians);
        double newY1 = y + (A[1] - x) * Math.Sin(angleInRadians) + (A[0] - y) * Math.Cos(angleInRadians);
        double newX2 = x + (B[1] - x) * Math.Cos(angleInRadians) - (B[0] - y) * Math.Sin(angleInRadians);
        double newY2 = y + (B[1] - x) * Math.Sin(angleInRadians) + (B[0] - y) * Math.Cos(angleInRadians);
        double newX3 = x + (C[1] - x) * Math.Cos(angleInRadians) - (C[0] - y) * Math.Sin(angleInRadians);
        double newY3 = y + (C[1] - x) * Math.Sin(angleInRadians) + (C[0] - y) * Math.Cos(angleInRadians);

        Console.WriteLine("Нові координати вершин трикутника:");
        Console.WriteLine($"({newX1}, {newY1})");
        Console.WriteLine($"({newX2}, {newY2})");
        Console.WriteLine($"({newX3}, {newY3})");

        Console.WriteLine("Поворот відносно центра описаного кола:");
        double centerX = (A[1] + B[1] + C[1]) / 3;
        double centerY = (A[0] + B[0] + C[0]) / 3;

        double radius = Math.Sqrt(Math.Pow(A[1] - centerX, 2) + Math.Pow(A[0] - centerY, 2));

        double newX1Center = centerX + (A[1] - centerX) * Math.Cos(angleInRadians) - (A[0] - centerY) * Math.Sin(angleInRadians);
        double newY1Center = centerY + (A[1] - centerX) * Math.Sin(angleInRadians) + (A[0] - centerY) * Math.Cos(angleInRadians);
        double newX2Center = centerX + (B[1] - centerX) * Math.Cos(angleInRadians) - (B[0] - centerY) * Math.Sin(angleInRadians);
        double newY2Center = centerY + (B[1] - centerX) * Math.Sin(angleInRadians) + (B[0] - centerY) * Math.Cos(angleInRadians);
        double newX3Center = centerX + (C[1] - centerX) * Math.Cos(angleInRadians) - (C[0] - centerY) * Math.Sin(angleInRadians);
        double newY3Center = centerY + (C[1] - centerX) * Math.Sin(angleInRadians) + (C[0] - centerY) * Math.Cos(angleInRadians);

        Console.WriteLine("Нові координати вершин трикутника:");
        Console.WriteLine($"({newX1Center}, {newY1Center})");
        Console.WriteLine($"({newX2Center}, {newY2Center})");
        Console.WriteLine($"({newX3Center}, {newY3Center})");

    }
    string TriangleType()
    {
        string type = "";
        double a = AB_Length;
        double b = BC_Length;
        double c = CA_Length;

        if (a + b > c && b + c > a && c + a > b)
        {
            if (a == b && b == c)
            {

                type = "Трикутник - рівносторонній";
            }
            else if (a == b || b == c || c == a)
            {

                type = "Трикутник - рівнобедрений";
            }
            else
            {

                type = "Трикутник - різносторонній";
            }
            type += ", ";
            double cosA = (b * b + c * c - a * a) / (2 * b * c);
            double cosB = (c * c + a * a - b * b) / (2 * c * a);
            double cosC = (a * a + b * b - c * c) / (2 * a * b);

            if (cosA == 0 || cosB == 0 || cosC == 0)
            {
                type += "прямокутний";
            }
            else if (cosA > 0 && cosB > 0 && cosC > 0)
            {
                type += "гострокутний";
            }
            else
            {
                type += "тупокутний";
            }
        }
        else
        {
            type = "Трикутник не існує";
        }
        return type;
    }

    public string Compare(Triangle trr)
    {
        string ret = "";
        if (Inside_Circle == trr.Inside_Circle && Outside_Circle == trr.Outside_Circle)
        {
            ret = "Трикутники рівні";
        }
        else
            ret = "Трикутники не рівні";
        return ret;
    }
}
