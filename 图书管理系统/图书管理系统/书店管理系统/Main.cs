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

namespace 书店管理系统
{
    public partial class Main : CCSkinMain
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login_form = new Login();
            login_form.ShowDialog(this);//
            this.Close();
        }

        private void Register_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register register_form = new Register();
            register_form.ShowDialog(this);//
            this.Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
