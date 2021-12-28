using org.mariuszgromada.math.mxparser;
using System;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab5
{
    public abstract class Integral
    {
        public Function Func { get; set; }
        public double a { get; set; }
        public double b { get; set; }
        public int n { get; set; }
        public double eps { get; set; }
        public Chart Chart { get; set; }
        public Integral(string function, double a, double b, double eps, Chart chart)
        {
            Func = new Function(function);
            this.a = a;
            this.b = b;
            this.eps = eps;
            n = 1;
            Chart = chart;
        }
        public double Function(double x)
        {
            return Func.calculate(x);
        }
        public abstract double FindIntegral();
        public double Calculate()
        {
            double s1 = FindIntegral();
            Thread.Sleep(200);
            double s;
            do
            {
                s = s1; 
                n = 2 * n;
                s1 = FindIntegral();
                Thread.Sleep(200);
            }
            while (Math.Abs(s1 - s) > eps);
            return s1;
        }
        public void UpdateChart(Series series, double h, int type, double k)
        {
            if (Chart.InvokeRequired)
            {
                Chart.Invoke(new Action(() => {
                    UpdateChart(series, h, type, k);
                }));
            }
            else
            {
                series.Points.Clear();
                for (double i = a; i < b; i = i + h)
                {
                    if (type == 1)
                    {
                        series.Points.AddXY(i, 0);
                        series.Points.AddXY(i, Function(i + k));
                        series.Points.AddXY(i + h, Function(i + k));
                    }
                    if (type == 2)
                    {
                        series.Points.AddXY(i, 0);
                        series.Points.AddXY(i, Function(i));
                        series.Points.AddXY(i + h, Function(i + h));
                    }
                    if (type == 3)
                    {
                        series.Points.AddXY(i, 0);
                        double[] coefs = FindParabCoef(i, i + h / 2, i + h, Function(i), Function(i + h / 2), Function(i + h));
                        for (double j = i; j < i + h; j += 0.1)
                        {
                            double y = coefs[0] * j * j + coefs[1] * j + coefs[2];
                            series.Points.AddXY(j, y);
                        }
                        series.Points.AddXY(i + h, Function(i + h));
                    }
                }
            }
        }
        public double[] FindParabCoef(double x1, double x2, double x3, double y1, double y2, double y3)
        {
            double a = (y3 - (x3 * (y2 - y1) + x2 * y1 - x1 * y2) / (x2 - x1)) / (x3 * (x3 - x1 - x2) + x1 * x2);
            double b = ((y2 - y1) / (x2 - x1)) - a * (x1 + x2);
            double c = ((x2 * y1 - x1 * y2) / (x2 - x1)) + a * x1 * x2;
            double[] coefs = new double[] { a, b, c };
            return coefs;
        }
    }
    class RectangleIntegral : Integral
    {
        public int RectType { get; set; }
        public RectangleIntegral(string function, double a, double b, double eps, Chart chart, int rectType) : base(function, a, b, eps, chart)
        {
            RectType = rectType;
        }
        private double LeftRect()
        {
            double h = (b - a) / n;
            UpdateChart(Chart.Series[1], h, 1, 0);
            return CalculateSum(0, n, 0.0, h);
        }
        private double RightRect()
        {
            double h = (b - a) / n;
            UpdateChart(Chart.Series[1], h, 1, h);
            return CalculateSum(1, n + 1, 0.0, h);
        }
        private double CentralRect()
        {
            double h = (b - a) / n;
            UpdateChart(Chart.Series[1], h, 1, h / 2);
            double sum = (Function(a) + Function(b)) / 2;
            return CalculateSum(1, n, sum, h);
        }
        public double CalculateSum(int start, int end, double sum, double h)
        {
            for (int i = start; i < end; ++i)
            {
                sum += Function(a + i * h);
            }
            return h * sum;
        }
        public override double FindIntegral()
        {
            switch (RectType)
            {
                case 1:
                    return LeftRect();
                case 2:
                    return RightRect();
                case 3:
                    return CentralRect();
                default:
                    return LeftRect();
            }
        }
    }
    class TrapezeIntegral : Integral
    {
        public TrapezeIntegral(string function, double a, double b, double eps, Chart chart) : base(function, a, b, eps, chart) { }
        public override double FindIntegral()
        {
            double h = (b - a) / n;
            double sum = Function(a) + Function(b);
            for (int i = 1; i <= n - 1; ++i)
            {
                sum += 2 * Function(a + i * h);
            }
            UpdateChart(Chart.Series[1], h, 2, 0);
            return sum * h / 2;
        }
    }
    class SympsonIntegral : Integral
    {
        public SympsonIntegral(string function, double a, double b, double eps, Chart chart) : base(function, a, b, eps, chart) { }
        public override double FindIntegral()
        {
            double h = (b - a) / n;
            double sum = Function(a) + Function(b);
            int k;
            for (int i = 1; i <= n - 1; ++i)
            {
                k = 2 + 2 * (i % 2);
                sum += k * Function(a + i * h);
            }
            UpdateChart(Chart.Series[1], h, 3, 0);
            return sum * h / 3;
        }
    }
}
