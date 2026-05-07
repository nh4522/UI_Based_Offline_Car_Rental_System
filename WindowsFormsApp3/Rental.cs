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
using MySql.Data.MySqlClient;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
namespace WindowsFormsApp3
{
    public partial class Rental : Form
    {
        public Rental()
        {
            InitializeComponent();
        }
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=carrental");
       

        private void populate()
        {
            try
            {
                con.Open();
                string query="Select * from rentaltb"; 
                //string query = "SELECT rentid, carreg, custname, DATE_FORMAT(rentdate, '%Y-%m-%d') AS rentdate, DATE_FORMAT(returnDate, '%Y-%m-%d') AS returnDate, rentfee FROM rentaltb";
                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                RentalDGV.AutoGenerateColumns = true;
                RentalDGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        private void updateonRent()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close(); // Close if already open
                }

                con.Open(); // Open the connection
                string query = "UPDATE cartb1 SET Available = 'NO' WHERE Regno = @regno;";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@regno", CarRegCb.SelectedValue?.ToString());

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


        private void updateonRentDelete(string carRegNo)
        {
            try
            {
                con.Open();
                string query = "UPDATE cartb1 SET Available = 'YES' WHERE Regno = @regno;";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@regno", carRegNo);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating car availability: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void Fillcombo()
        {
            try
            {
                con.Open();
                string query = "SELECT Regno FROM cartb1 WHERE Available = 'YES';";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader rde = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rde);

                CarRegCb.DataSource = dt;
                CarRegCb.DisplayMember = "Regno";
                CarRegCb.ValueMember = "Regno";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching available cars: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void Fillcustomercombo()
        {
            con.Open();
            string query = "select custid from customertb";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader rde;
            rde = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("custid", typeof(string));
            dt.Load(rde);
            CustIdCb.ValueMember = "custid";
            CustIdCb.DataSource = dt;
            con.Close();
        }

        //exit button
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        //filling up the customer name from combo box
        private void fetchCustName()
        {
            con.Open();
            string query="select * from customertb where custid="+CustIdCb.SelectedValue.ToString()+";";
            MySqlCommand cmd= new MySqlCommand(query, con);
            DataTable dt= new DataTable();
            MySqlDataAdapter da= new MySqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                CnameTb.Text = dr["CustName"].ToString();
            }

            con.Close() ;
        }
        //fill up the price of the car from combo box
        private void fetchCarPrice()
        {
            try
            {
                if (CarRegCb.SelectedValue == null)
                {
                    MessageBox.Show("Please select a car.");
                    return;
                }

                if (con.State == ConnectionState.Closed)
                    con.Open();

                string query = "SELECT price FROM cartb1 WHERE regno = @regno;";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@regno", CarRegCb.SelectedValue);

                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    FeesTb.Text = dt.Rows[0]["price"].ToString();
                }
                else
                {
                    FeesTb.Text = "Not Found";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void Rental_Load(object sender, EventArgs e)
        {
            Fillcombo();
            Fillcustomercombo();
            populate();
        }

        private void CarRegCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            fetchCarPrice();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CustIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            fetchCustName();
        }
        //add button
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RentalTb.Text) || string.IsNullOrEmpty(CnameTb.Text) || string.IsNullOrEmpty(FeesTb.Text))
            {
                MessageBox.Show("Missing Information");
            }
            if (ReturndateB.Value < RentDateB.Value)
            {
                MessageBox.Show("Return date cannot be earlier than the rental date.");
                return; // Exit the method
            }
            else
            {
                try
                {
                    int rentday = (ReturndateB.Value - RentDateB.Value).Days; //Counting total rental days
                    rentday = rentday == 0 ? 1 : rentday;
                    decimal rentfee = decimal.Parse(FeesTb.Text);
                    decimal total = rentday * rentfee;
                    con.Open();
                    string query = "INSERT INTO rentaltb(rentid, carreg, custname, rentdate, returnDate, rentfee, RentDay, Total) " +
                                   "VALUES(@rentid, @carreg, @custname, @rentdate, @returndate, @rentfee, @RentDay, @Total);";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@rentid", RentalTb.Text);
                    cmd.Parameters.AddWithValue("@carreg", CarRegCb.SelectedValue);
                    cmd.Parameters.AddWithValue("@custname", CnameTb.Text);
                    cmd.Parameters.AddWithValue("@rentfee", FeesTb.Text);
                    cmd.Parameters.AddWithValue("@rentdate", RentDateB.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@returndate", ReturndateB.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@RentDay", rentday);
                    cmd.Parameters.AddWithValue("@Total", total);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Rental Successfully Added");
                    // Create an instance of the InvoiceGenerator class
                    InvoiceGenerator generator = new InvoiceGenerator();
                    string rentalId = RentalTb.Text;
                    string carReg = CarRegCb.SelectedValue.ToString();
                    string custName = CnameTb.Text;
                    DateTime rentDate = RentDateB.Value;
                    DateTime returnDate = ReturndateB.Value;
                    string rentFee = FeesTb.Text;
                    int rentalday = rentday;
                    decimal totalamt = total;

                    // Call the GenerateInvoicePDF method
                   generator.GenerateInvoicePDF(rentalId, carReg, custName, rentDate, returnDate, rentFee, rentalday,totalamt);

                    updateonRent(); // Mark car as rented
                    populate();     // Refresh DataGridView
                    Fillcombo();    // Refresh ComboBox
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        
        //delete button
        private void button3_Click(object sender, EventArgs e)
        {
            if (RentalTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    // Fetch car registration number
                    string carRegNo = "";
                    con.Open();
                    string fetchCarQuery = "SELECT carreg FROM rentaltb WHERE rentid = @rentid;";
                    MySqlCommand fetchCarCmd = new MySqlCommand(fetchCarQuery, con);
                    fetchCarCmd.Parameters.AddWithValue("@rentid", RentalTb.Text);
                    MySqlDataReader reader = fetchCarCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        carRegNo = reader["carreg"].ToString();
                    }
                    con.Close();

                    if (string.IsNullOrEmpty(carRegNo))
                    {
                        MessageBox.Show("Car not found for the selected rental.");
                        return;
                    }

                    // Delete rental record
                    con.Open();
                    string deleteRentalQuery = "DELETE FROM rentaltb WHERE rentid = @rentid;";
                    MySqlCommand deleteRentalCmd = new MySqlCommand(deleteRentalQuery, con);
                    deleteRentalCmd.Parameters.AddWithValue("@rentid", RentalTb.Text);
                    deleteRentalCmd.ExecuteNonQuery();
                    con.Close();

                    // Mark car as available
                    updateonRentDelete(carRegNo);

                    MessageBox.Show("Rental deleted successfully!");

                    populate(); // Refresh DataGridView
                    Fillcombo(); // Refresh ComboBox
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        //edit button
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close(); // Close if already open
                }
                con.Open();

                int rentday = (ReturndateB.Value - RentDateB.Value).Days;
                rentday = rentday == 0 ? 1 : rentday;

                decimal rentfee = decimal.Parse(FeesTb.Text);
                decimal total = rentday * rentfee;

                string query = "UPDATE rentaltb SET CustName = @custname, RentDay = @RentDay, Total = @total " +
                               "WHERE RentID = @rentid;";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@custname", CnameTb.Text);
                cmd.Parameters.AddWithValue("@RentDay", rentday);
                cmd.Parameters.AddWithValue("@Total", total);
                //cmd.Parameters.AddWithValue("@rentid", RentalTb.Text);
                cmd.Parameters.AddWithValue("@rentid", int.Parse(RentalTb.Text));


                // Log parameter values for debugging
                Console.WriteLine($"Executing Query: {query}");
                Console.WriteLine($"@custname: {CnameTb.Text}, @RentDay: {rentday}, @Total: {total}, @rentid: {RentalTb.Text}");

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Rental updated successfully!");
                }
                else
                {
                    MessageBox.Show("No rows affected. Please check the Rental ID or ensure the data is correct.");
                }

                updateonRent();
                populate();
                Fillcombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating rental: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }







        //back button
        private void button4_Click(object sender, EventArgs e)
        {
            MainForm main_form = new MainForm();
            main_form.Show();
            this.Hide();
        }

        private void RentalDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (RentalDGV.SelectedRows.Count > 0) // Ensure a row is selected
            {
                RentalTb.Text = RentalDGV.SelectedRows[0].Cells[0].Value.ToString();
                CarRegCb.SelectedValue = RentalDGV.SelectedRows[0].Cells[1].Value.ToString();
                CnameTb.Text = RentalDGV.SelectedRows[0].Cells[2].Value.ToString(); // Adjusted column index
                FeesTb.Text = RentalDGV.SelectedRows[0].Cells[4].Value.ToString();
                //RentDateB.Value = Convert.ToDateTime(RentalDGV.SelectedRows[0].Cells[5].Value.ToString());
               // ReturndateB.Value = Convert.ToDateTime(RentalDGV.SelectedRows[0].Cells[6].Value.ToString());
            }
            else
            {
                MessageBox.Show("No row selected.");
            }
        }

        private void CnameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void RentDateB_ValueChanged(object sender, EventArgs e)
        {
           // RentDateB.Value = DateTime.Now;  // Set to current date and time
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
