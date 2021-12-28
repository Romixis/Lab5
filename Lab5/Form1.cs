using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using org.mariuszgromada.math.mxparser;
using Wolfram.Alpha;
using Wolfram.Alpha.Models;

namespace Lab5
{
    public partial class Form1 : Form
    {
        public string function;
        public double a;
        public double b;
        public double eps;
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;  // Вывод формы по центру экрана
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e) // Кнопка выхода
        {
            Close();
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e) // Кнопка очистить
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();

            chart1.Series[1].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart3.Series[1].Points.Clear();
            richTextBox1.Clear();
        }

        public async Task Wolf(string function)
        {
            WolframAlphaService service = new WolframAlphaService("E6JG63-6UYTPQPJGG");
            WolframAlphaRequest request = new WolframAlphaRequest($"Integral {function}");
            WolframAlphaResult result = await service.Compute(request);
            string integral = result.QueryResult.Pods[0].SubPods[0].Plaintext;
            richTextBox1.AppendText($"{integral} \n");
        }
        public void ChartDob()
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            Function Function = new Function(function);
            for (double i = a; i <= b; i += 0.1)
            {
                for (int j = 0; j < chart1.Series.Count; j += 2)
                {
                    chart1.Series[0].Points.AddXY(i, Function.calculate(i));
                    chart2.Series[0].Points.AddXY(i, Function.calculate(i));
                    chart3.Series[0].Points.AddXY(i, Function.calculate(i));
                }
            }
        }
        public void ChartCl()
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
        }

        private void выбратьМетодыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 Select = new Form2(this, chart1, chart2, chart3, richTextBox1);
            Select.Show();
        }
        private void функцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart3.Series[1].Points.Clear();
            richTextBox1.Clear();

            Form3 Function = new Form3(this);
            Enabled = false;
            Function.Show();
        }
    }
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
