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
    public partial class CusInfo : Form
    {
        public CusInfo()
        {
            InitializeComponent();

            //MessageBox.Show(common.GetName());
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True");
            conn.Open();
            SqlDataAdapter daAuthors = new SqlDataAdapter("select * From Customer", conn);
            DataSet dsPubs = new DataSet("Pubs");

            daAuthors.FillSchema(dsPubs, SchemaType.Source, "Customer");
            //FillSchema加载表的架构和数据,有了架构，表就知道哪个列是它的主键，同时 Rows 集合的 Find 方法也就可用了。


            daAuthors.Fill(dsPubs, "Customer");
            DataTable tblAuthors;
            tblAuthors = dsPubs.Tables["Customer"];
            DataRow drCurrent;
            drCurrent = tblAuthors.Rows.Find(common.GetName());
            nickname.Text = drCurrent[2].ToString();
            username.Text = drCurrent[0].ToString();
            vip.Text = drCurrent[6].ToString();
            tel.Text = drCurrent[5].ToString();
            address.Text = drCurrent[8].ToString();
            myinfo.Text = drCurrent[7].ToString();
            age.Text = drCurrent[4].ToString();
            sex.Text = drCurrent[3].ToString();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Cus_OK_Click(object sender, EventArgs e)
        {
            String connsql = "Data Source=DESKTOP-05CV8JV;Initial Catalog=bs;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接
                    //update Customer set nickname = 'lijiarui',cus_age='17'where username = cherry
                    string strcomm = "update Customer set " + "Cus_nickname='" + nickname.Text.Trim() + "'," + "Cus_sex='" + sex.Text.Trim() + "'," + "Cus_age='" + age.Text.Trim() + "'," + "Cus_tel='" + tel.Text.Trim() + "'," + "Cus_address='" + address.Text.Trim() + "'," + "Cus_remark='" + myinfo.Text.Trim() + "'where Cus_uername = '" + common.GetName() + "'";
                    //update FilTer set 列名 = value where id = 3
                    SqlCommand comm = new SqlCommand(strcomm, conn);
                    comm.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("信息修改成功！");
                    this.Hide();
                    CusView cusinfo_form = new CusView();
                    cusinfo_form.ShowDialog(this);//
                    this.Close();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Hide();
            CusView cusinfo_form = new CusView();
            cusinfo_form.ShowDialog(this);//
            this.Close();
        }

        private void CusInfo_Load(object sender, EventArgs e)
        {

        }

        private void username_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void vip_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void myinfo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
