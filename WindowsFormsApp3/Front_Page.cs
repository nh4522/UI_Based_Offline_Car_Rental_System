using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Front_Page : Form
    {
        public Front_Page()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        
        

        private void MyProgress_ValueChanged(object sender, EventArgs e)
        {
            //timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login log=new Login();
            log.Show();
            this.Hide(); 
        }
        //close button
        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
