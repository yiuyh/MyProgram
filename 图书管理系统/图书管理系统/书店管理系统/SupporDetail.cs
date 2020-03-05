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
    public partial class SupporDetail : Form
    {
        public SupporDetail()
        {
            InitializeComponent();

            label2.Text = search_supinfo.GetName();

            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接
                    //select Sup_no '供应商编号', Sup_name '供应商名字', Sup_address '地址', Sup_tel '电话' from Supporter
                    String sql = "select  Sup_no '供应商编号', Sup_name '供应商名字',Book_no '供应书籍编号', name '供应书籍名字' from [sup_Serch] where Sup_no = '" + search_supinfo.GetName() + "'";// 查询语句

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

        private void back_Click(object sender, EventArgs e)
        {
            this.Hide();
            /*CusView cusinfo_form = new CusView();
            cusinfo_form.ShowDialog(this);*/
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SupporDetail_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
