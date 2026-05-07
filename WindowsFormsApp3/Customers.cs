using MySql.Data.MySqlClient;
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
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
        }
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=carrental");
        //close button
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //back button
        private void button4_Click(object sender, EventArgs e)
        {
            MainForm main_form= new MainForm();
            main_form.Show();
            this.Hide();
        }
        private void populate()
        {
            con.Open();
            string query = "select * from customertb";
            MySqlDataAdapter da = new MySqlDataAdapter(query, con);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            CustomerDGV.DataSource = ds.Tables[0];



            con.Close();
        }
        //add button
        private void button1_Click(object sender, EventArgs e)
        {
            if (CID.Text == "" || Cname.Text == "" || Caddress.Text == "" || Cphone.Text==""|| Cnid.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    con.Open();
                    //string query = "insert into usertb1 values(" + CID.Text + ",'" + Cname.Text + "','" + Caddress.Text + "')";
                    string query = "Insert Into customertb(custid,custname,custaddress,phone,nid) values(@Custid, @Custname, @custaddress, @phone,@nid) ";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Custid",CID.Text);
                    cmd.Parameters.AddWithValue("@Custname", Cname.Text);
                    cmd.Parameters.AddWithValue("@Custaddress", Caddress.Text);
                    cmd.Parameters.AddWithValue("@phone", Cphone.Text);
                    cmd.Parameters.AddWithValue("@nid", Cnid.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Successfully Added");

                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //datagrid view
        private void UserDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CID.Text = CustomerDGV.SelectedRows[0].Cells[0].Value.ToString();
            Cname.Text = CustomerDGV.SelectedRows[0].Cells[1].Value.ToString();
            Caddress.Text = CustomerDGV.SelectedRows[0].Cells[2].Value.ToString();
            Cphone.Text = CustomerDGV.SelectedRows[0].Cells[3].Value.ToString();
            Cnid.Text = CustomerDGV.SelectedRows[0].Cells[4].Value.ToString();
        }
        //delete button
        private void button3_Click(object sender, EventArgs e)
        {
            if (CID.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    con.Open();
                    //string query = "delete from cartb1 where Regno='" + RegnoTb.Text + "';";
                    string query = "DELETE FROM customertb WHERE custid = @Custid;";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Custid", CID.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer deleted successfully!");

                    con.Close();
                    populate();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        //data show
        private void Customers_Load(object sender, EventArgs e)
        {
            populate();
        }
        //update data
        private void button2_Click(object sender, EventArgs e)
        {
            if(CID.Text == "" || Cname.Text == "" || Caddress.Text == "" || Cphone.Text == ""|| Cnid.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    con.Open();
                    // string query = "update usertb1 set Uname='" + Uname.Text + "',Upass='" + Upassword.Text + "' where id=" + UID.Text + ";";
                    string query = ("update customertb set Custname=@Custname, Custaddress=@Custaddress, phone=@Phone, nid=@Nid where custid=@Custid");
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Custname", Cname.Text);
                    cmd.Parameters.AddWithValue("@Custaddress", Caddress.Text);
                    cmd.Parameters.AddWithValue("@Phone", Cphone.Text);
                    cmd.Parameters.AddWithValue("@Nid", Cnid.Text);
                    cmd.Parameters.AddWithValue("@Custid", CID.Text);


                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Successfully Updated");

                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
