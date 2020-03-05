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
    public partial class CusOrder : Form
    {
        public CusOrder()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Hide();
            CusView cusinfo_form = new CusView();
            cusinfo_form.ShowDialog(this);//
            this.Close();
        }

        private void CusOrder_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)//未处理订单
            {
                String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
                try
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = connsql;
                        conn.Open(); // 打开数据库连接
                        //select name'书名',time'下单时间' from cus_orderSerch where status='N' and username = 
                        String sql = "select name'书名', sellnum '购买数量',date'下单时间' from cus_orderSerch where status= 0 and Cus_uername = '" + common.GetName() +"'";//这个表名一定要加上[]; // 查询语句

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
            else//已处理订单
            {
                String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
                try
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = connsql;
                        conn.Open(); // 打开数据库连接
                        //select name'书名',time'下单时间' from cus_orderSerch where status='N' and username = 
                        String sql = "select name'书名', sellnum '购买数量',date'下单时间' from cus_orderSerch where status= 1 and Cus_uername = '" + common.GetName() + "'";//这个表名一定要加上[]; // 查询语句

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


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True");
                con.Open();
                SqlCommand cmd;
                SqlDataReader sdr;
                string bname = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                int stock = int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim());
                int bianhao;
                //select Book_no from Book where name = ''
                cmd = new SqlCommand("select Book_no from Book where name = '" + bname +"'", con);//这个表名一定要加上[]
                sdr = cmd.ExecuteReader();
                sdr.Read();
                bianhao = int.Parse(sdr[0].ToString().Trim());

                //select Order_no from "Order" where Book_no = '' and Cus_uername = ''
                cmd = new SqlCommand("select Order_no from [Order] where Book_no = " + bianhao +" and Cus_uername = '" + common.GetName()+"'", con);//这个表名一定要加上[]
                sdr.Close();
                sdr = cmd.ExecuteReader();
                sdr.Read();
                string no = sdr[0].ToString();


                //update Book set Stock = Stock+1 where Book_no = 0
                cmd = new SqlCommand("update Book set Stock = Stock+1 where Book_no = " +bianhao, con);//这个表名一定要加上[]
                sdr.Close();
                sdr = cmd.ExecuteReader();

                if (stock != 1)
                {
                    cmd = new SqlCommand("update [Order] set sellnum = sellnum-1 where Order_no = '" + no + "'and Cus_uername = '" + common.GetName() + "'", con);//这个表名一定要加上[]
                    sdr.Close();
                    sdr = cmd.ExecuteReader();
                }
                else
                {
                    //delete from Order where Order_no = ''
                    cmd = new SqlCommand("delete from [Order] where Order_no = '" + no + "'and Cus_uername = '" + common.GetName() + "'", con);//这个表名一定要加上[]
                    sdr.Close();
                    sdr = cmd.ExecuteReader();
                }



                con.Close();


                try
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = connsql;
                        conn.Open(); // 打开数据库连接
                        //select name'书名',time'下单时间' from cus_orderSerch where status='N' and username = 
                        String sql = "select name'书名', sellnum '购买数量',date'下单时间' from [cus_orderSerch] where status= 0 and Cus_uername = '" + common.GetName() + "'";//这个表名一定要加上[]; // 查询语句

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
            else
            {
                MessageBox.Show("已完成订单无法修改！", "出现错误");
            }
        }
    }
}
