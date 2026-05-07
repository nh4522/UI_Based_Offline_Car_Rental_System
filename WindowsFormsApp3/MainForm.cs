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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //car menu
        private void button1_Click(object sender, EventArgs e)
        {
            Cars cars = new Cars();
            cars.Show();
            this.Hide();
        }
        //user menu
        private void button5_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.Show();
            this.Hide();
        }
        //customer menu
        private void button2_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Hide();
        }
        //rental menu
        private void button3_Click(object sender, EventArgs e)
        {
            Rental rental= new Rental();
            rental.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Return ret = new Return();
            ret.Show();
            this.Hide();
        }
        //logout button
        private void button6_Click(object sender, EventArgs e)
        {
            Login login= new Login();
            login.Show();
            this.Hide();
            MessageBox.Show("Logged Out Successfully!");
        }
    }
}
