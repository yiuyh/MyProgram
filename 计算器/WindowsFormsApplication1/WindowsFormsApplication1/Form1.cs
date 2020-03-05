using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        double num1, num2, result;
        bool symbleFlag = false;
        string myOperator;

        

        public void NumClick(int myNumber) //输入数字
        {
            if (symbleFlag)
            {
                textBox1.Text = "" + myNumber;
                symbleFlag = false;
            }
            else
            {
                if (textBox1.Text == "0") //输入第一个数
                {
                    
                    textBox1.Text = "" + myNumber;
                }else
                {
                    textBox1.Text = textBox1.Text + myNumber;
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            textBox1.TextAlign = HorizontalAlignment.Right;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Num1(object sender, EventArgs e)
        {
           // textBox1.Text = textBox1.Text + "1";
           NumClick(1);
        }

        private void Num2(object sender, EventArgs e)
        {
            NumClick(2);
        }

        private void Num3(object sender, EventArgs e)
        {
            NumClick(3);
        }

        private void Num4(object sender, EventArgs e)
        {
            NumClick(4);
        }

        private void Num5(object sender, EventArgs e)
        {
            NumClick(5);
        }

        private void Num6(object sender, EventArgs e)
        {
            NumClick(6);
        }

        private void Num7(object sender, EventArgs e)
        {
            NumClick(7);
        }

        private void Num8(object sender, EventArgs e)
        {
            NumClick(8);
        }

        private void Num9(object sender, EventArgs e)
        {
            NumClick(9);
        }

        private void Num0(object sender, EventArgs e)
        {
            NumClick(0);
        }

        private void Point(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + ".";
        }

        private void Mod(object sender, EventArgs e)
        {

        }

        private void Add(object sender, EventArgs e)
        {
            myOperator = "add";
            num1 = double.Parse(textBox1.Text);
            textBox1.Text = "+";
            symbleFlag = true;
        }

        private void Sub(object sender, EventArgs e)
        {
            myOperator = "Sub";
            num1 = double.Parse(textBox1.Text);
            textBox1.Text = "-";
            symbleFlag = true;
        } 

        private void Mul(object sender, EventArgs e)
        {
            myOperator = "Mul";
            num1 = double.Parse(textBox1.Text);
            textBox1.Text = "*";
            symbleFlag = true;
        }

        private void Div(object sender, EventArgs e)
        {
            myOperator = "Div";
            num1 = double.Parse(textBox1.Text);
            textBox1.Text = "÷";
            symbleFlag = true;
        }

        private void Equal(object sender, EventArgs e)
        {
            num2 = double.Parse(textBox1.Text);
            switch (myOperator)
            {
                case "add":
                    result = num1 + num2;
                    textBox1.Text = result.ToString();
                    break;
                case "Sub":
                    result = num1 - num2;
                    textBox1.Text = result.ToString();
                    break;
                case "Mul":
                    result = num1 * num2;
                    textBox1.Text = result.ToString();
                    break;
                case "Div":
                    if (num2 == 0)
                    {
                        textBox1.Text = "∞";
                    }
                    else
                    {
                        result = num1 / num2;
                        textBox1.Text = result.ToString();
                    }
                    break;
            }
        }
        
        private void btnC(object sender, EventArgs e)
        {
            num1 = 0;
            num2 = 0;
            textBox1.Text = "0";
            symbleFlag = false;
        }
    }
}
