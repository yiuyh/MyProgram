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
    public partial class Manager : Form
    {
        public Manager()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void Manager_Load(object sender, EventArgs e)
        {

        }

        private void ManagerInfo_Btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManagerInfo managerinfo_form = new ManagerInfo();
            managerinfo_form.ShowDialog(this);//
            this.Close();
        }

        private void Cus_Delete_Click(object sender, EventArgs e)
        {
            //select nickname'顾客昵称',username'用户名',passwd'密码',vip'会员',tel '电话',address '地址',cus_sex'性别',cus_age'年龄' from [Customer]", conn);

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True");
            conn.Open();
            SqlDataAdapter daAuthors = new SqlDataAdapter("select Cus_nickname'顾客昵称',Cus_uername'用户名',Cus_password'密码',vip'会员',Cus_tel '电话',Cus_address '地址',Cus_sex'性别',Cus_age'年龄' from [Customer]", conn);
            DataSet dsPubs = new DataSet("Pubs12");

            daAuthors.FillSchema(dsPubs, SchemaType.Source, "Customer");
            //FillSchema加载表的架构和数据,有了架构，表就知道哪个列是它的主键，同时 Rows 集合的 Find 方法也就可用了。


            daAuthors.Fill(dsPubs, "Customer");
            DataTable tblAuthors;
            tblAuthors = dsPubs.Tables["Customer"];


            if (MessageBox.Show("确定删除该顾客吗?", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataRow drCurrent;
                string Row_zhi = dataGridView3.SelectedRows[0].Cells[1].Value.ToString();//获取第一个单元格的值            
               // MessageBox.Show(Row_zhi);            
                drCurrent = tblAuthors.Rows.Find(Row_zhi);
                drCurrent.Delete();

                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(daAuthors);
                //SqlCommandBuilder 提供自动生成单表命令的一种方式，这些命令用于协调使用关联的 SQL Server 数据库对 DataSet 执行的更改。              
                daAuthors.Update(dsPubs, "Customer"); //数据适配器.Update()方法                
                //MessageBox.Show("数据库更新成功！");

                //-------重新绑定dataGridView的数据源，以便重新显示-------
                daAuthors.Fill(dsPubs, "Customer");
                DataTable tblAuthors1;
                tblAuthors1 = dsPubs.Tables["Customer"];
                dataGridView2.DataSource = tblAuthors1;
                
                this.Hide();
                Manager manager_form = new Manager();
                manager_form.ShowDialog(this);//
                this.Close();
            }

            conn.Close();
            conn.Dispose();
        }


        private void Cus_Search_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接


                    String sql = " select Cus_nickname'顾客昵称',Cus_uername'用户名',Cus_password'密码',vip,Cus_tel'电话',Cus_address '地址',Cus_sex'性别',Cus_age'年龄' from [Customer] where Cus_nickname like '%"
                        + cus_name.Text.Trim()
                        + "%'" + "or Cus_uername like '%" + cus_name.Text.Trim() + "%'";//这个表名一定要加上[]; // 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView3.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }


        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接
                    string strcolumn = dataGridView3.Columns[e.ColumnIndex].HeaderText;//获取列标题
                    string strrow = dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();//获取焦点触发行的第一个值
                    string value = dataGridView3.CurrentCell.Value.ToString();//获取当前点击的活动单元格的值

                    if (String.Compare(strcolumn, "顾客昵称") == 0)
                        strcolumn = "nickname";
                    if (String.Compare(strcolumn, "密码") == 0)
                        strcolumn = "passwd";
                    if (String.Compare(strcolumn, "电话") == 0)
                        strcolumn = "tel";
                    if (String.Compare(strcolumn, "地址") == 0)
                        strcolumn = "address";
                    if (String.Compare(strcolumn, "性别") == 0)
                        strcolumn = "cus_sex";
                    if (String.Compare(strcolumn, "年龄") == 0)
                        strcolumn = "cus_age";

                    string strcomm = "update Customer set " + strcolumn + "='" + value + "'where username = '" + strrow + "'";

                    SqlCommand comm = new SqlCommand(strcomm, conn);
                    comm.ExecuteNonQuery();
                    MessageBox.Show("信息修改成功");
                    conn.Close();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {

            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    String sql = "select Order_no '订单编号',Cus_uername'用户名',Book_no'订购书籍编号',sellnum'订购数量',status'是否处理',date'订单时间' from [Order] ";// 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView2.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True");

            conn.Open();
            SqlDataAdapter daAuthors = new SqlDataAdapter("select Order_no '订单编号',Cus_uername'用户名',Book_no'订购书籍编号',sellnum'订购数量',status'是否处理',date'订单时间' from [Order]", conn);
            DataSet dsPubs = new DataSet("Pubs11");

            daAuthors.FillSchema(dsPubs, SchemaType.Source, "OrderInfo");
            //FillSchema加载表的架构和数据,有了架构，表就知道哪个列是它的主键，同时 Rows 集合的 Find 方法也就可用了。


            daAuthors.Fill(dsPubs, "OrderInfo");
            DataTable tblAuthors;
            tblAuthors = dsPubs.Tables["OrderInfo"];


            if (MessageBox.Show("确定处理该订单吗?", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataRow drCurrent;
                string Row_zhi = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();//获取第一个单元格的值            
                //MessageBox.Show(Row_zhi);            
                drCurrent = tblAuthors.Rows.Find(Row_zhi);
                //drCurrent.Delete();
                //string temp = drCurrent[3].ToString();
                //MessageBox.Show(temp);
                drCurrent[4] = "1";

                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(daAuthors);
                //SqlCommandBuilder 提供自动生成单表命令的一种方式，这些命令用于协调使用关联的 SQL Server 数据库对 DataSet 执行的更改。              
                daAuthors.Update(dsPubs, "OrderInfo"); //数据适配器.Update()方法                
                //MessageBox.Show("数据库更新成功！");

                //-------重新绑定dataGridView的数据源，以便重新显示-------
                daAuthors.Fill(dsPubs, "OrderInfo");
                DataTable tblAuthors1;
                tblAuthors1 = dsPubs.Tables["OrderInfo"];
                dataGridView2.DataSource = tblAuthors1;
            }

            conn.Close();
            conn.Dispose();

        }

        private void Book_Search_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接


                    String sql = " select Book_no'书籍编号',name'书籍名称',author'作者',publish'出版社',origin_price '原价',price'售价',Stock'数量',Book_category'书籍类别' from [Book] "; // 查询语句

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

        private void Book_Add_Click(object sender, EventArgs e)
        {
            this.Hide();
            BookAdd bookadd_form = new BookAdd();
            bookadd_form.ShowDialog(this);//
        }

        private void Book_Delete_Click(object sender, EventArgs e)
        {
            //select nickname'顾客昵称',username'用户名',passwd'密码',vip'会员',tel '电话',address '地址',cus_sex'性别',cus_age'年龄' from [Customer]", conn);

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True");
            conn.Open();
            SqlDataAdapter daAuthors = new SqlDataAdapter(" select Book_no'书籍编号',name'书籍名称',author'作者',publish'出版社',origin_price '原价',price'售价',Stock'数量',Book_category'书籍类别' from [Book]", conn);
            DataSet dsPubs = new DataSet("Pubs12");

            daAuthors.FillSchema(dsPubs, SchemaType.Source, "Book");
            //FillSchema加载表的架构和数据,有了架构，表就知道哪个列是它的主键，同时 Rows 集合的 Find 方法也就可用了。


            daAuthors.Fill(dsPubs, "Book");
            DataTable tblAuthors;
            tblAuthors = dsPubs.Tables["Book"];


            if (MessageBox.Show("确定删除该书籍吗?", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataRow drCurrent;
                string Row_zhi = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();//获取第一个单元格的值            
                MessageBox.Show("删除成功");
                drCurrent = tblAuthors.Rows.Find(Row_zhi);
                drCurrent.Delete();

                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(daAuthors);
                //SqlCommandBuilder 提供自动生成单表命令的一种方式，这些命令用于协调使用关联的 SQL Server 数据库对 DataSet 执行的更改。              
                daAuthors.Update(dsPubs, "Book"); //数据适配器.Update()方法                
                //MessageBox.Show("数据库更新成功！");

                //-------重新绑定dataGridView的数据源，以便重新显示-------
                daAuthors.Fill(dsPubs, "Book");
                DataTable tblAuthors1;
                tblAuthors1 = dsPubs.Tables["Book"];
                dataGridView1.DataSource = tblAuthors1;
            }

            conn.Close();
            conn.Dispose();
        }
/*
        private void rep_search_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接


                    String sql = " select Book_no'书籍编号',name'书籍名称',Book_num'库存量' from [Repository_manager] "; // 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView4.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }

        }

        private void dataGridView4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接
                    string strcolumn = dataGridView4.Columns[e.ColumnIndex].HeaderText;//获取列标题
                    string strrow = dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString();//获取焦点触发行的第一个值
                    string value = dataGridView4.CurrentCell.Value.ToString();//获取当前点击的活动单元格的值
                   // MessageBox.Show(strrow);
                    if (String.Compare(strcolumn, "书籍名称") == 0)
                        strcolumn = "name";
                    if (String.Compare(strcolumn, "库存量") == 0)
                        strcolumn = "Book_num";
                    string strcomm = "update Repository_manager set " + strcolumn + "='" + value + "'where Book_no = '" + strrow + "'";

                    SqlCommand comm = new SqlCommand(strcomm, conn);
                    comm.ExecuteNonQuery();
                    MessageBox.Show("信息修改成功");
                    conn.Close();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
        }*/

        private void back_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main main_form = new Main();
            main_form.ShowDialog(this);//
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接
                    //select Sup_no '供应商编号', Sup_name '供应商名字', Sup_address '地址', Sup_tel '电话' from Supporter
                    String sql = "select Sup_no '供应商编号', Sup_name '供应商名字', Sup_address '地址', Sup_tel '电话' from [Supporter]";// 查询语句

                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    dataGridView4.DataSource = dt; // 设置到DataGridView中

                    conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string bianhao = dataGridView4.SelectedRows[0].Cells[0].Value.ToString();
            search_supinfo.SetName(bianhao);
            SupporDetail SupporDetail_form = new SupporDetail();
            SupporDetail_form.ShowDialog(this);

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}