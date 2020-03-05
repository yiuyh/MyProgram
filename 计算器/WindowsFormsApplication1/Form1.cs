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
    public partial class 计算器 : Form
    {
        public 计算器()
        {
            InitializeComponent();
        }

        double num1, num2, result;
        bool symbleFlag = false;
        bool sqrtFlag = false;
        int operatorTime = 0;
        string myOperator;

        public double Calculate(double num1, double num2)
        {
            double res = 0;
            switch (myOperator)
            {
                case "Add":
                    res = num1 + num2;
                    break;
                case "Sub":
                    res = num1 - num2;
                    break;
                case "Mul":
                    res = num1 * num2;
                    break;
                case "Sqrt":
                    res = Math.Sqrt(num2);
                    sqrtFlag = false;
                    break;
                case "Div":
                    res = num1 / num2;
                    break;
            }
            return res;
        }

        public void NumClick(int myNumber) //输入数字
        {
            if (symbleFlag)
            {
                textBox1.Text = "" + myNumber;
                symbleFlag = false;
            }else if (sqrtFlag)
            {
                textBox1.Text = textBox1.Text + myNumber;
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

        private void Add(object sender, EventArgs e)
        {
            operatorTime++;
            if(operatorTime >= 2)
            {
                if (sqrtFlag)
                {
                    num2 = Math.Sqrt(double.Parse(textBox1.Text.Substring(1, textBox1.Text.Length - 1)));
                    sqrtFlag = false;
                }
                else num2 = double.Parse(textBox1.Text);
                num1 = Calculate(num1, num2);
                myOperator = "Add";
                textBox1.Text = "+";
                symbleFlag = true;
            }
            else{
                myOperator = "Add";
                if (sqrtFlag){
                    num1 = Math.Sqrt(double.Parse(textBox1.Text.Substring(1, textBox1.Text.Length - 1)));
                    sqrtFlag = false;
                }else num1 = double.Parse(textBox1.Text);
                textBox1.Text = "+"; 
                symbleFlag = true;
            }  
        }

        private void Sub(object sender, EventArgs e)
        {
            operatorTime++;
            if (operatorTime >= 2)
            {
                if (sqrtFlag)
                {
                    num2 = Math.Sqrt(double.Parse(textBox1.Text.Substring(1, textBox1.Text.Length - 1)));
                    sqrtFlag = false;
                }
                else num2 = double.Parse(textBox1.Text);
                num1 = Calculate(num1, num2);
                myOperator = "Sub";
                textBox1.Text = "-";
                symbleFlag = true;
            }
            else
            {
                myOperator = "Sub";
                if (sqrtFlag)
                {
                    num1 = Math.Sqrt(double.Parse(textBox1.Text.Substring(1, textBox1.Text.Length - 1)));
                    sqrtFlag = false;
                }
                else num1 = double.Parse(textBox1.Text);
                textBox1.Text = "-";
                symbleFlag = true;
            }
        } 

        private void Mul(object sender, EventArgs e)
        {
            operatorTime++;
            if (operatorTime >= 2)
            {
                if (sqrtFlag)
                {
                    num2 = Math.Sqrt(double.Parse(textBox1.Text.Substring(1, textBox1.Text.Length - 1)));
                    sqrtFlag = false;
                }
                else num2 = double.Parse(textBox1.Text);
                num1 = Calculate(num1, num2);
                myOperator = "Mul";
                textBox1.Text = "*";
                symbleFlag = true;
            }
            else
            {
                myOperator = "Mul";
                if (sqrtFlag)
                {
                    num1 = Math.Sqrt(double.Parse(textBox1.Text.Substring(1, textBox1.Text.Length - 1)));
                    sqrtFlag = false;
                }
                else num1 = double.Parse(textBox1.Text);
                textBox1.Text = "*";
                symbleFlag = true;
            }
        }

        private void Div(object sender, EventArgs e)
        {
            operatorTime++;
            if (operatorTime >= 2)
            {
                if (sqrtFlag)
                {
                    num2 = Math.Sqrt(double.Parse(textBox1.Text.Substring(1, textBox1.Text.Length - 1)));
                    sqrtFlag = false;
                }
                else num2 = double.Parse(textBox1.Text);
                num1 = Calculate(num1, num2);
                myOperator = "Div";
                textBox1.Text = "÷";
                symbleFlag = true;
            }
            else
            {
                myOperator = "Div";
                if (sqrtFlag)
                {
                    num1 = Math.Sqrt(double.Parse(textBox1.Text.Substring(1, textBox1.Text.Length - 1)));
                    sqrtFlag = false;
                }
                else num1 = double.Parse(textBox1.Text);
                textBox1.Text = "÷";
                symbleFlag = true;
            }
        }

        private void Sqrt(object sender, EventArgs e)
        {
            if (operatorTime < 1) myOperator = "Sqrt";
            textBox1.Text = "√";
            symbleFlag = false;
            sqrtFlag = true;
        }

        private void Equal(object sender, EventArgs e)
        {
            operatorTime = 0;
            if (sqrtFlag) {
                num2 = Math.Sqrt(double.Parse(textBox1.Text.Substring(1, textBox1.Text.Length - 1)));
                sqrtFlag = false;
            }
            else num2 = double.Parse(textBox1.Text);
            switch (myOperator)
            {
                case "Add":
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
                case "Sqrt":
                    result = num2;
                    textBox1.Text = result.ToString();
                    sqrtFlag = false;
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
            sqrtFlag = false;
            operatorTime = 0;
        }
    }
}
