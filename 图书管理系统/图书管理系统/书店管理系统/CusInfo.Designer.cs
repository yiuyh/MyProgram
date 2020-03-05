namespace 书店管理系统
{
    partial class CusInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CusInfo));
            this.nickname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.sex = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.age = new System.Windows.Forms.TextBox();
            this.tel = new System.Windows.Forms.TextBox();
            this.address = new System.Windows.Forms.TextBox();
            this.myinfo = new System.Windows.Forms.TextBox();
            this.Cus_OK = new System.Windows.Forms.Button();
            this.back = new System.Windows.Forms.PictureBox();
            this.vip = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.back)).BeginInit();
            this.SuspendLayout();
            // 
            // nickname
            // 
            this.nickname.Location = new System.Drawing.Point(188, 81);
            this.nickname.Margin = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.nickname.Name = "nickname";
            this.nickname.Size = new System.Drawing.Size(143, 47);
            this.nickname.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("楷体", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(50, 90);
            this.label1.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "昵称：";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("楷体", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(50, 169);
            this.label2.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "用户名：";
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.BackColor = System.Drawing.Color.Transparent;
            this.username.Font = new System.Drawing.Font("微软雅黑", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.username.ForeColor = System.Drawing.Color.White;
            this.username.Location = new System.Drawing.Point(226, 169);
            this.username.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(50, 40);
            this.username.TabIndex = 3;
            this.username.Text = "xz";
            this.username.Click += new System.EventHandler(this.username_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("楷体", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(50, 243);
            this.label4.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 30);
            this.label4.TabIndex = 4;
            this.label4.Text = "性别：";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // sex
            // 
            this.sex.AutoCompleteCustomSource.AddRange(new string[] {
            "男",
            "女"});
            this.sex.FormattingEnabled = true;
            this.sex.Items.AddRange(new object[] {
            "男",
            "女"});
            this.sex.Location = new System.Drawing.Point(177, 234);
            this.sex.Margin = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.sex.Name = "sex";
            this.sex.Size = new System.Drawing.Size(197, 48);
            this.sex.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("楷体", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(50, 322);
            this.label5.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 30);
            this.label5.TabIndex = 6;
            this.label5.Text = "会员：";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("楷体", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(50, 388);
            this.label6.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 30);
            this.label6.TabIndex = 7;
            this.label6.Text = "年龄：";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("楷体", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(47, 452);
            this.label7.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 30);
            this.label7.TabIndex = 8;
            this.label7.Text = "电话：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("楷体", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(50, 523);
            this.label8.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 30);
            this.label8.TabIndex = 9;
            this.label8.Text = "地址：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("楷体", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(12, 589);
            this.label9.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(168, 30);
            this.label9.TabIndex = 10;
            this.label9.Text = "个人说明：";
            // 
            // age
            // 
            this.age.Location = new System.Drawing.Point(177, 379);
            this.age.Margin = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.age.Name = "age";
            this.age.Size = new System.Drawing.Size(252, 47);
            this.age.TabIndex = 11;
            // 
            // tel
            // 
            this.tel.Location = new System.Drawing.Point(177, 452);
            this.tel.Margin = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.tel.Name = "tel";
            this.tel.Size = new System.Drawing.Size(252, 47);
            this.tel.TabIndex = 12;
            // 
            // address
            // 
            this.address.Location = new System.Drawing.Point(177, 523);
            this.address.Margin = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.address.Name = "address";
            this.address.Size = new System.Drawing.Size(252, 47);
            this.address.TabIndex = 13;
            // 
            // myinfo
            // 
            this.myinfo.Location = new System.Drawing.Point(177, 589);
            this.myinfo.Margin = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.myinfo.Multiline = true;
            this.myinfo.Name = "myinfo";
            this.myinfo.Size = new System.Drawing.Size(361, 88);
            this.myinfo.TabIndex = 14;
            this.myinfo.TextChanged += new System.EventHandler(this.myinfo_TextChanged);
            // 
            // Cus_OK
            // 
            this.Cus_OK.Location = new System.Drawing.Point(165, 695);
            this.Cus_OK.Margin = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.Cus_OK.Name = "Cus_OK";
            this.Cus_OK.Size = new System.Drawing.Size(233, 53);
            this.Cus_OK.TabIndex = 15;
            this.Cus_OK.Text = "确定";
            this.Cus_OK.UseVisualStyleBackColor = true;
            this.Cus_OK.Click += new System.EventHandler(this.Cus_OK_Click);
            // 
            // back
            // 
            this.back.BackColor = System.Drawing.Color.Transparent;
            this.back.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("back.BackgroundImage")));
            this.back.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.back.Location = new System.Drawing.Point(2, 1);
            this.back.Margin = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(71, 76);
            this.back.TabIndex = 16;
            this.back.TabStop = false;
            this.back.Click += new System.EventHandler(this.back_Click);
            // 
            // vip
            // 
            this.vip.AutoSize = true;
            this.vip.BackColor = System.Drawing.Color.Transparent;
            this.vip.ForeColor = System.Drawing.Color.White;
            this.vip.Location = new System.Drawing.Point(226, 322);
            this.vip.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.vip.Name = "vip";
            this.vip.Size = new System.Drawing.Size(36, 40);
            this.vip.TabIndex = 17;
            this.vip.Text = "0";
            this.vip.Click += new System.EventHandler(this.vip_Click);
            // 
            // CusInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(19F, 40F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(572, 749);
            this.Controls.Add(this.vip);
            this.Controls.Add(this.back);
            this.Controls.Add(this.Cus_OK);
            this.Controls.Add(this.myinfo);
            this.Controls.Add(this.address);
            this.Controls.Add(this.tel);
            this.Controls.Add(this.age);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.sex);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nickname);
            this.Font = new System.Drawing.Font("微软雅黑", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.Name = "CusInfo";
            this.Text = "个人信息";
            this.Load += new System.EventHandler(this.CusInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.back)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nickname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label username;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox sex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox age;
        private System.Windows.Forms.TextBox tel;
        private System.Windows.Forms.TextBox address;
        private System.Windows.Forms.TextBox myinfo;
        private System.Windows.Forms.Button Cus_OK;
        private System.Windows.Forms.PictureBox back;
        private System.Windows.Forms.Label vip;
    }
}