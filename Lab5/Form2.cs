using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab5
{
    public partial class Form2 : Form
    {
        List<Thread> Thread;
        public Form1 Form1;
        public Chart Chart1;
        public Chart Chart2;
        public Chart Chart3;
        private readonly RichTextBox TextBox;

        int rectType = 0;
        public Form2(Form1 form1, Chart chart1, Chart chart2, Chart chart3, RichTextBox textBox)
        {
            Form1 = form1;
            Chart1 = chart1;
            Chart2 = chart2;
            TextBox = textBox;
            Chart3 = chart3;
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;  // Вывод формы по центру экрана
        }
        private void button1_Click(object sender, EventArgs e) // Кнопка начать
        {
            TextBox.Invoke((MethodInvoker)delegate
            {
                TextBox.Clear();
            });

            Thread = new List<Thread>();

            string f = Form1.function;
            double a = Form1.a;
            double b = Form1.b;
            double eps = Form1.eps;

            if (rectType != 0)
            {
                Integral RectIntegral = new RectangleIntegral(f, a, b, eps, Chart1, rectType);
                Thread thread = new Thread(() => 
                { 
                    CalculateIntegral(RectIntegral, "Метод прямоугольников"); 
                });
                Thread.Add(thread);
                thread.Start();
            }
            foreach (var item in checkedListBox1.CheckedItems)
            {
                Integral Rast;
                switch (item)
                {
                    case "Метод трапеций":
                        Rast = new TrapezeIntegral(f, a, b, eps, Chart2);
                        break;
                    case "Метод Симпсона":
                        Rast = new SympsonIntegral(f, a, b, eps, Chart3);
                        break;
                    default:
                        Rast = new SympsonIntegral(f, a, b, eps, Chart3);
                        break;

                }
                Thread thread = new Thread(() => 
                { 
                    CalculateIntegral(Rast, item.ToString());
                });
                Thread.Add(thread);
                thread.Start();
            }
        }
        private void button2_Click(object sender, EventArgs e) // Остатовка потоков при нажатии на кнопку
        {
            if (Thread != null)
            {
                foreach (var thread in Thread)
                {
                    if (thread.IsAlive)
                    {
                        thread.Abort();
                    }
                }
            }
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e) // Остатовка потоков при закрытии формы
        {
            if (Thread != null)
            {
                foreach (var thread in Thread)
                {
                    if (thread.IsAlive)
                    {
                        thread.Abort();
                    }
                }
            }
        }
        public void CalculateIntegral(Integral integral, string methodName) 
        {
            double result = integral.Calculate();
            Console.WriteLine($"{methodName} \n Значение интеграла: {result} \nЧисло разбиений: {integral.n}");
            TextBox.Invoke((MethodInvoker)delegate
            {
                TextBox.AppendText($"{methodName} \n", Color.Green);
                TextBox.AppendText($"{result} \nЧисло разбиений: {integral.n} \n");
                TextBox.Update();
            });
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e) // Левые, правые, центральные
        {
            RadioButton radioButton = (RadioButton)sender;
            switch (radioButton.Text)
            {
                case "Левых":
                    rectType = 1;
                    break;
                case "Правых":
                    rectType = 2;
                    break;
                case "Центральных":
                    rectType = 3;
                    break;
            }
        }
    }
}
