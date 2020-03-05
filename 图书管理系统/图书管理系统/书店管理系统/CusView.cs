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
    public partial class CusView : Form
    {
        public CusView()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Book_Search_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)//书名
            {
                String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
                try
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = connsql;
                        conn.Open(); // 打开数据库连接

                        String sql = "select * from [cus_bookSerch] where 书名 like '%" + textBox1.Text.Trim() + "%'";//这个表名一定要加上[]; // 查询语句

                        SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                        DataTable dt = new DataTable(); // 实例化数据表
                        myda.Fill(dt); // 保存数据 

                        dataGridView1.DataSource = dt; // 设置到DataGridView中

                        conn.Close(); // 关闭数据库连接
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误信息：" + ex.Message, "出现错误");
                }
            }
            else if (comboBox1.SelectedIndex == 1)//作者
            {
                String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
                try
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = connsql;
                        conn.Open(); // 打开数据库连接

                        String sql = "select * from [cus_bookSerch] where 作者 like '%" + textBox1.Text.Trim() + "%'";//这个表名一定要加上[]; // 查询语句

                        SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                        DataTable dt = new DataTable(); // 实例化数据表
                        myda.Fill(dt); // 保存数据 

                        dataGridView1.DataSource = dt; // 设置到DataGridView中

                        conn.Close(); // 关闭数据库连接
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误信息：" + ex.Message, "出现错误");
                }
            }
            else//出版社
            {
                String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
                try
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = connsql;
                        conn.Open(); // 打开数据库连接

                        String sql = "select * from [cus_bookSerch] where 出版社 like '%" + textBox1.Text.Trim() + "%'";//这个表名一定要加上[]; // 查询语句

                        SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                        DataTable dt = new DataTable(); // 实例化数据表
                        myda.Fill(dt); // 保存数据 

                        dataGridView1.DataSource = dt; // 设置到DataGridView中

                        conn.Close(); // 关闭数据库连接
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误信息：" + ex.Message, "出现错误");
                }
            }
        }

        private void Shehuikexue_Btn_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    String sql = "select * from [cus_bookSerch] where 书籍类别 like '%社会科学%'";//这个表名一定要加上[]; // 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView1.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }

        private void Kexuejishu_Btn_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    String sql = "select * from [cus_bookSerch] where 书籍类别 like '%科学技术%'";//这个表名一定要加上[]; // 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView1.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }

        private void Wenxue_Btn_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    String sql = "select * from [cus_bookSerch] where 书籍类别 like '%文学%'";//这个表名一定要加上[]; // 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView1.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }

        private void Yishu_Btn_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    String sql = "select *  from [cus_bookSerch] where 书籍类别 like '%艺术%'";//这个表名一定要加上[]; // 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView1.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }

        private void Home_Btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main main_form = new Main();
            main_form.ShowDialog(this);//
            this.Close();
        }

        private void Person_Btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            CusInfo cusinfo_form = new CusInfo();
            cusinfo_form.ShowDialog(this);//
            this.Close();
        }

        private void OrderSearch_Btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            CusOrder cusorder_form = new CusOrder();
            cusorder_form.ShowDialog(this);//
            this.Close();
        }

        private void Add_ShopCar_Click(object sender, EventArgs e)
        {
           
            if (MessageBox.Show("确实要订购该书籍吗?", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SqlConnection conn = new SqlConnection("Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True");
                conn.Open();
                SqlCommand cmd;
                SqlDataReader sdr;
                string book_no = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();//书籍编号

                //查询购买书籍的库存
                cmd = new SqlCommand("select Stock from Book where Book_no =" + book_no, conn);
                sdr = cmd.ExecuteReader();
                sdr.Read();
                int Stock = int.Parse(sdr[0].ToString().Trim());
                try
                {
                    if (Stock <= 0)
                    {
                        MessageBox.Show("书籍库存不足");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("书籍库存不足");
                    return;
                }

                //更新库存;
                Stock = Stock - 1;
                cmd = new SqlCommand("update Book set Stock= " + Stock + " where Book_no =" + book_no, conn);//这个表名一定要加上[]
                sdr.Close();
                sdr = cmd.ExecuteReader();

                //增加到订单表中
                string datetime = DateTime.Now.ToLocalTime().ToString();  //产生日期
                cmd = new SqlCommand("select Order_no from [Order] where Book_no = '" + book_no + "' and Cus_uername = '" + common.GetName() + "' and status = " + 0, conn);
                sdr.Close();
                sdr = cmd.ExecuteReader();
                sdr.Read();
                if (sdr.HasRows)//存在购买相同的书  数量+1
                {
                    string no = sdr[0].ToString().Trim();
                    cmd = new SqlCommand("update [Order] set sellnum = sellnum+1 where Order_no = '"+ no + "'", conn);
                    sdr.Close();
                    sdr = cmd.ExecuteReader();
                    cmd = new SqlCommand("update [Order] set date = '" + datetime + "'where Order_no = '" + no + "'", conn);
                    sdr.Close();
                    sdr = cmd.ExecuteReader();
                }
                else
                {
                    string Order_no;
                    while (true)//产生订单编号
                    {
                        Random r = new Random();
                        int a = r.Next(1, 1000);
                        Order_no = "#00" + a;
                        cmd = new SqlCommand("select * from [Order] where Order_no = '" + Order_no + "'", conn);//这个表名一定要加上[]
                        sdr.Close();
                        sdr = cmd.ExecuteReader();
                        sdr.Read();
                        if (!sdr.HasRows) break;
                    }
                    //"insert into Order(Order_no, Cus_uername, date, status, Book_no, sellnum) values('" + Order_no +" ','" + common.GetName() + "','" +datetime + "', 0, '"+ book_no + "', 1)"
                    string str = "insert into [Order](Order_no, Cus_uername, date, status, Book_no, sellnum) values('" + Order_no + " ','" + common.GetName() + "','" + datetime + "', 0, '" + book_no + "', 1)";
                    cmd = new SqlCommand(str, conn);
                    sdr.Close();
                    sdr = cmd.ExecuteReader();

                    //insert into Purchase(Book_no,Cus_uername) values(1, 'xh')
                    str = "insert into Purchase(Book_no,Cus_uername) values("+ book_no +", '"+ common.GetName() + "')";
                    cmd = new SqlCommand(str, conn);
                    sdr.Close();
                    sdr = cmd.ExecuteReader();

                }

                MessageBox.Show("购买成功");

            }
           
        }

        private void label7_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    String sql = "select * from [cus_bookSerch] where 书籍类别 like '%文学%'";//这个表名一定要加上[]; // 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView1.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    String sql = "select *  from [cus_bookSerch] where 书籍类别 like '%艺术%'";//这个表名一定要加上[]; // 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView1.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }

        private void CusView_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    String sql = "select * from [cus_bookSerch] where 书籍类别 like '%科学技术%'";//这个表名一定要加上[]; // 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView1.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    String sql = "select * from [cus_bookSerch] where 书籍类别 like '%社会科学%'";//这个表名一定要加上[]; // 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView1.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }
    }
}
