using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace WindowsFormsApp3
{
    public partial class Cars : Form
    {
        public Cars()
        {
            InitializeComponent();
        }
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=carrental");
        //exit button
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //show list
        private void populate()
        {
            con.Open();
            string query = "select * from cartb1";
            MySqlDataAdapter da = new MySqlDataAdapter(query, con);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            CarDGV.DataSource = ds.Tables[0];



            con.Close();
        }
       
        
        //add button
        private void button1_Click(object sender, EventArgs e)
        {

            if (RegnoTb.Text == "" || BrandTb.Text == "" || ModelTb.Text == ""|| PriceTb.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    con.Open();
                    // string query = "INSERT INTO cartb1 VALUES (" +"'" + RegnoTb.Text + "'," +"'" + BrandTb.Text + "'," +"'" + ModelTb.Text + "'," +"'" + AvailableCb.SelectedItem.ToString() + "'," + PriceTb.Text + ")";
                    string query = "INSERT INTO cartb1 (Regno, Brand, Model, Available, Price) VALUES (@Regno, @Brand, @Model, @Available, @Price)";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Regno", RegnoTb.Text);
                    cmd.Parameters.AddWithValue("@Brand", BrandTb.Text);
                    cmd.Parameters.AddWithValue("@Model", ModelTb.Text);
                    cmd.Parameters.AddWithValue("@Available", AvailableCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Price", PriceTb.Text);
                   // MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Successfully Added");

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

        private void Cars_Load(object sender, EventArgs e)
        {
            populate();
            LoadSearchOptions();
            
        }
        private void LoadSearchOptions()
        {
            Search.Items.Clear();
            Search.Items.Add("Available");
            Search.Items.Add("Rented");
        }
        //delete button
        private void button3_Click(object sender, EventArgs e)
        {

            if (RegnoTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    con.Open();
                    //string query = "delete from cartb1 where Regno='" + RegnoTb.Text + "';";
                    string query = "DELETE FROM cartb1 WHERE Regno = @Regno;";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Regno" , RegnoTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Car deleted successfully!");

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
        private void CarDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            RegnoTb.Text = CarDGV.SelectedRows[0].Cells[0].Value.ToString();
            BrandTb.Text = CarDGV.SelectedRows[0].Cells[1].Value.ToString();
            ModelTb.Text = CarDGV.SelectedRows[0].Cells[2].Value.ToString();
            AvailableCb.SelectedItem = CarDGV.SelectedRows[0].Cells[3].Value.ToString();
            PriceTb.Text = CarDGV.SelectedRows[0].Cells[4].Value.ToString();
        }
        //back button
        private void button4_Click(object sender, EventArgs e)
        {
            MainForm main_form = new MainForm();
            main_form.Show();
            this.Hide();
        }
        //edit button
        private void button2_Click(object sender, EventArgs e)
        {
            if (RegnoTb.Text == "" || BrandTb.Text == "" || ModelTb.Text == ""|| PriceTb.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    con.Open();
                    // string query = "update usertb1 set Uname='" + Uname.Text + "',Upass='" + Upassword.Text + "' where id=" + UID.Text + ";";
                    string query = ("update cartb1 set brand=@Brand, Model=@Model, Available=@Available, price=@Price where Regno=@Regno");
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Brand",BrandTb.Text);
                    cmd.Parameters.AddWithValue("@Model",ModelTb.Text);
                    cmd.Parameters.AddWithValue("@Available", AvailableCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Price", PriceTb.Text);
                    cmd.Parameters.AddWithValue("@Regno", RegnoTb.Text);


                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Car Successfully Updated");

                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Search_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string availabilityFilter = "";
            if (Search.SelectedItem.ToString() == "Available")
            {
                availabilityFilter = "YES";
            }
            else if (Search.SelectedItem.ToString() == "Rented")
            {
                availabilityFilter = "NO";
            }

            try
            {
                con.Open();
                string query = "SELECT * FROM cartb1 WHERE available = @Available;";
                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@Available", availabilityFilter);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CarDGV.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Search_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        

    }
}
