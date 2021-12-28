using System;
using System.Windows.Forms;
using org.mariuszgromada.math.mxparser;

namespace Lab5
{
    public partial class Form3 : Form
    {
        public Form1 Form1;
        public Form3(Form1 form1)
        {
            Form1 = form1;
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;  // Вывод формы по центру экрана
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            int length = textBox2.Text.Length;
            if (length == 0 && ch == ',' && ch == '-')  // Исключаем запятую в начале и минус
            {
                e.Handled = true;
            }
            if (!Char.IsDigit(ch) && ch != 8 && (ch != ',' || textBox2.Text.Contains(",")) && ((ch != '-' || textBox2.Text.Contains("-")))) // Если число, BACKSPACE запятая или минус, то вводим
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            int length = textBox3.Text.Length;
            if (length == 0 && ch == ',' && ch == '-')  // Исключаем запятую в начале и минус
            {
                e.Handled = true;
            }
            if (!Char.IsDigit(ch) && ch != 8 && (ch != ',' || textBox3.Text.Contains(",")) && ((ch != '-' || textBox3.Text.Contains("-")))) // Если число, BACKSPACE запятая или минус, то вводим
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            int length = textBox4.Text.Length;
            if (length == 0 && ch == ',' && ch == '-')  // Исключаем запятую в начале и минус
            {
                e.Handled = true;
            }
            if (!Char.IsDigit(ch) && ch != 8 && (ch != ',' || textBox4.Text.Contains(",")) && ((ch != '-' || textBox4.Text.Contains("-")))) // Если число, BACKSPACE запятая или минус, то вводим
            {
                e.Handled = true;
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Поле ввода функции пусто", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (Convert.ToDouble(textBox2.Text) >= Convert.ToDouble(textBox3.Text))
                {
                    MessageBox.Show("Граница A не может быть равна границе B или быть больше", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string function = "f(x)=" + textBox1.Text;
                    double a = Convert.ToDouble(textBox2.Text);
                    double b = Convert.ToDouble(textBox3.Text);
                    double eps = Convert.ToDouble(textBox4.Text);

                    /*double a = (double)numericUpDown1.Value;
                    double b = (double)numericUpDown2.Value;
                    double eps = (double)numericUpDown3.Value;*/
                    Function Function = new Function(function);
                    if (double.IsNaN(Function.calculate(a)))
                    {
                        MessageBox.Show("Неверный формат функции", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Form1.function = function;
                        await Form1.Wolf(textBox1.Text);
                        Form1.a = a;
                        Form1.b = b;
                        Form1.eps = eps;
                        Form1.ChartCl();
                        Form1.ChartDob();
                        Close();
                    }
                }
            }
        }
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.Enabled = true;
        }
    }
}
