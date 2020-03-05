using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;//新建引用
using System.Data.SqlClient;//新建引用

namespace 书店管理系统
{
    public partial class Register : Form
    {
        string sex = "男";//0 男 1女
        public Register()
        {
            InitializeComponent();
            radioButton1.Select();
        }

        private void RegisterOK_Click(object sender, EventArgs e)
        {

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True");//建立连接
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from [Customer] where Cus_uername='" + username_tb.Text.Trim() + "'", conn);//这个表名一定要加上[]
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            if (sdr.HasRows)
            {
                conn.Close();
                MessageBox.Show("用户名已存在");
                username_tb.Text = "";
                 
            }
            else
            {
                conn.Close();
                conn.Open();
                cmd = new SqlCommand("INSERT INTO Customer(Cus_nickname,Cus_uername,Cus_password,vip,Cus_tel,Cus_address,Cus_remark,Cus_age,Cus_sex)VALUES('" + nickename_tb.Text.Trim() + "','" + username_tb.Text.Trim() + "','" + password_tb.Text.Trim() + "','"+0+"','" + tel_tb.Text.Trim() + "','" + address_tb.Text.Trim() + "','"+ person_tb.Text.Trim() + "','"+ age_tb.Text.Trim() + "','"+ sex.Trim() +"')", conn);//这个表名一定要加上[]
                sdr = cmd.ExecuteReader();
                conn.Close();
                MessageBox.Show("注册成功");
                this.Hide();
                Main main_form = new Main();
                main_form.ShowDialog(this);//
                this.Close();
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main main_form = new Main();
            main_form.ShowDialog(this);//
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) sex = "男";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) sex = "女";
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
