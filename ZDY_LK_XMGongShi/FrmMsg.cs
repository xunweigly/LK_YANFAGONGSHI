

namespace LKU8.shoukuan
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class FrmMsg : Form
    {
        private IContainer components = null;
        private TextBox textBox2;
    

        public FrmMsg()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmMsg_Load(object sender, EventArgs e)
        {
            this.textBox2.Text = this.msg;
        }

        private void InitializeComponent()
        {
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 12);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(409, 338);
            this.textBox2.TabIndex = 0;
            // 
            // FrmMsg
            // 
            this.ClientSize = new System.Drawing.Size(443, 362);
            this.Controls.Add(this.textBox2);
            this.Name = "FrmMsg";
            this.Text = "消息提示";
            this.Load += new System.EventHandler(this.FrmMsg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public string msg { get; set; }

        private void FrmMsg_Load_1(object sender, EventArgs e)
        {
        
        }
    }
}

