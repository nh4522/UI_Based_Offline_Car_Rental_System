using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Paddings;

namespace WindowsFormsApp3
{
    public partial class Return : Form
    {
        public Return()
        {
            InitializeComponent();
        }
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=carrental");
        private void populate()
        {
            con.Open();
            string query = "select * from rentaltb";
            MySqlDataAdapter da = new MySqlDataAdapter(query, con);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            CarRentDGV.DataSource = ds.Tables[0];



            con.Close();
        }
        // private void populate1()
        //{
        //con.Open();
        // string query = "select * from returntb";
        // MySqlDataAdapter da = new MySqlDataAdapter(query, con);
        // MySqlCommandBuilder builder = new MySqlCommandBuilder(da);
        // var ds = new DataSet();
        // da.Fill(ds);
        // CarReturnDGV.DataSource = ds.Tables[0];



        //  con.Close();
        // }
        private void updateonRent()
        {
            try
            {
                if (string.IsNullOrEmpty(CarRegTb.Text))
                {
                    MessageBox.Show("Please enter a valid car registration number.");
                    return;
                }

                if (con.State == ConnectionState.Open)
                {
                    con.Close(); // Close if already open
                }

                con.Open(); // Open the connection
                string query = "UPDATE cartb1 SET Available = 'YES' WHERE Regno = @regno;";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@regno", CarRegTb.Text);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Car availability updated successfully!");
                }
                else
                {
                    MessageBox.Show("No rows affected. Check if the Regno exists.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating car availability: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close(); // Ensure the connection is closed
                }
            }
        }

        private void populate1()
        {
            try
            {
                con.Open();
                string query = "SELECT rentid, carreg, custname, DATE_FORMAT(returndate, '%Y-%m-%d') AS returndate, delay, fine FROM returntb";
                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                CarReturnDGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during populate1(): " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void deleteonreturn()
        {
            try
            {
                con.Open();
                string query = "DELETE FROM rentaltb WHERE rentid = @rentid";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@rentid", RentalTb.Text);

                cmd.ExecuteNonQuery();

                con.Close();
                populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting rental record: " + ex.Message);
            }
        }

        private void Return_Load(object sender, EventArgs e)
        {
            populate();
            populate1 ();
        }
        //exit button
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

        private void CarRentDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // Ensure a valid row is selected
                {
                    DataGridViewRow row = CarRentDGV.Rows[e.RowIndex];
                    RentalTb.Text = row.Cells[0].Value.ToString(); // RentID
                    CarRegTb.Text = row.Cells[1].Value.ToString(); // Car Registration
                    NameTb.Text = row.Cells[2].Value.ToString();   // Customer Name
                    string returnDateString = row.Cells[5].Value.ToString(); // Return Date

                    DateTime returnDate;
                    if (DateTime.TryParse(returnDateString, out returnDate))
                    {
                        ReturndateTB.Text = returnDate.ToString("yyyy-MM-dd");
                        DateTime currentDate = DateTime.Now;

                        TimeSpan t = currentDate - returnDate;
                        int number_of_days = Convert.ToInt32(t.Days);

                        if (number_of_days <= 0)
                        {
                            DelayTb.Text = "No Delay";
                            FineTb.Text = "0";
                        }
                        else
                        {
                            DelayTb.Text = number_of_days + " Days";
                            FineTb.Text = (number_of_days * 250).ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid return date format.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching rental details: " + ex.Message);
            }
        }

        //add button
        private void button1_Click(object sender, EventArgs e)
        {
            if (RentalTb.Text == "" || NameTb.Text == "" || DelayTb.Text == "" || FineTb.Text == "")
            {
                MessageBox.Show("Missing Information");
                return;
            }

            try
            {
                con.Open();

                // Insert into returntb
                string query = "INSERT INTO returntb (rentid, carreg, custname, returndate, delay, fine) " +
                               "VALUES (@rentid, @carreg, @custname, @returndate, @delay, @fine)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@rentid", RentalTb.Text);
                cmd.Parameters.AddWithValue("@carreg", CarRegTb.Text);
                cmd.Parameters.AddWithValue("@custname", NameTb.Text);
                cmd.Parameters.AddWithValue("@returndate", ReturndateTB.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@delay", DelayTb.Text);
                cmd.Parameters.AddWithValue("@fine", FineTb.Text);

                cmd.ExecuteNonQuery();

                

                MessageBox.Show("Car returned.");

                con.Close();

                // Refresh grids
                updateonRent();
                populate1();
                deleteonreturn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error returning car: " + ex.Message);
            }
        }


        private void CarReturnDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
    }
}
