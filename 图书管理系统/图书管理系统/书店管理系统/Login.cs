using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using System.Data.OleDb;//新建引用
using System.Data.SqlClient;//新建引用

namespace 书店管理系统
{
    
    public partial class Login : CCSkinMain
    {
        int status = -1;//1 顾客 0 管理员
        public Login()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) status = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) status = 0;
        }

        private void Login_Btn_Click(object sender, EventArgs e)
        {
            if (status != 0 && status != 1)
            {
                MessageBox.Show("请选择登录身份！","警告");
                return;
            }
            if (username.Text == "" || password.Text == "")
            {
                MessageBox.Show("请输入用户名和密码！", "警告");//提示
            }
            else
            {
                SqlConnection conn = new SqlConnection("Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True");//建立连接
                conn.Open();
                //MessageBox.Show("连接成功！");
                if (status == 0)
                {
                    SqlCommand cmd = new SqlCommand("select * from [Manager] where M_username='" + username.Text.Trim() + "' and M_password='" + password.Text.Trim() + "'", conn);//这个表名一定要加上[]
                    SqlDataReader sdr = cmd.ExecuteReader();
                    sdr.Read();
                    if (sdr.HasRows)
                    {
                        common.SetName(username.Text.Trim().ToString());
                        conn.Close();
                        this.Hide();
                        Manager manager_form = new Manager();
                        manager_form.ShowDialog(this);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("账号或密码错误");
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("select * from [Customer] where Cus_uername='" + username.Text.Trim() + "' and Cus_password='" + password.Text.Trim() + "'", conn);//这个表名一定要加上[]
                    SqlDataReader sdr = cmd.ExecuteReader();
                    sdr.Read();
                    if (sdr.HasRows)//登录成功
                    {
                        common.SetName(username.Text.Trim().ToString());
                        //MessageBox.Show(common.GetName());
                        conn.Close();
                        this.Hide();
                        CusView cusview_form = new CusView();
                        cusview_form.ShowDialog(this);//
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("账号或密码错误");
                    }
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main main_form = new Main();
            main_form.ShowDialog(this);//
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void username_TextChanged(object sender, EventArgs e)
        {

        }

        private void skinTextBox1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
