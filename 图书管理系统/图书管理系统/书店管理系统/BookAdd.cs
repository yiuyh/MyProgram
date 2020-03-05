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
    public partial class BookAdd : Form
    {
        public BookAdd()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Add_Click(object sender, EventArgs e)
        {



            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True");//建立连接
            conn.Open();

            SqlCommand cmd = new SqlCommand("select * from [Book] where name='" + name.Text.Trim() + "' and author='" + author.Text.Trim() + "' and publish='" + publish.Text.Trim() + "'", conn);//这个表名一定要加上[]
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            if (sdr.HasRows)//存在相同书籍  则只加库存
            {
                int intBook_no = sdr.GetOrdinal("Book_no");
                //update Book set Stock = Stock+1 where Book_no = 1
                cmd = new SqlCommand("update Book set Stock = Stock + " + Stock.Text.Trim() + " where Book_no = " + sdr[intBook_no], conn);//这个表名一定要加上[]
                sdr.Close();
                sdr = cmd.ExecuteReader();
            }
            else//不存在相同  加入书籍 并在suport表中插入一条信息
            {

                cmd = new SqlCommand("INSERT INTO Book(name,author,publish,origin_price,price,Book_category,Stock)VALUES('"
                                + name.Text.Trim() + "','" + author.Text.Trim() + "','" + publish.Text.Trim() + "'," + oPrice.Text.Trim() + ","
                               + price.Text.Trim() + ",'" + comboBox1.SelectedItem + "','" + Stock.Text.Trim() + "')", conn);//这个表名一定要加上[]
                sdr.Close();
                sdr = cmd.ExecuteReader();


                cmd = new SqlCommand("select * from [Book] where name='" + name.Text.Trim() + "' and author='" + author.Text.Trim() + "' and publish='" + publish.Text.Trim() + "'", conn);//这个表名一定要加上[]
                sdr.Close();
                sdr = cmd.ExecuteReader();
                sdr.Read();
                int intBook_no = sdr.GetOrdinal("Book_no");

                cmd = new SqlCommand("INSERT INTO Support(Sup_no,Book_no)VALUES('"
                                + supporter.Text.Trim() + "','" + sdr[intBook_no] + "')", conn);
                sdr.Close();
                sdr = cmd.ExecuteReader();
                sdr.Read();


            }


            conn.Close();
            MessageBox.Show("书籍添加成功");


            this.Hide();
            Manager manager_form = new Manager();
            manager_form.ShowDialog(this);//
            this.Close();


        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manager manager_form = new Manager();
            manager_form.ShowDialog(this);//
        }

        private void BookAdd_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void author_TextChanged(object sender, EventArgs e)
        {

        }

        private void publish_TextChanged(object sender, EventArgs e)
        {

        }

        private void oPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void supporter_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Stock_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
