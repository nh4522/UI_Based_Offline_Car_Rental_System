using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
namespace WindowsFormsApp3
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
        }
        //SqlConnection con=new SqlConnection(@"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\pc\\Documents\\CarRentalDB.mdf;Integrated Security=True;Connect Timeout=30");
        //SqlConnection con = new SqlConnection(@"Server=(LocalDB)\MSSQLLocalDB;Database=CarRentalDB;Integrated Security=True;");
        // SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\pc\Documents\CarRentalDB.mdf;Integrated Security=True;Connect Timeout=30");
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=carrental");
        private void label2_Click(object sender, EventArgs e)
        {

        }
        //Exit button
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        //show userlist
        private void populate()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                string query = "SELECT * FROM usertb1";
                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                UserDGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        //Add button
        private void button1_Click(object sender, EventArgs e)
        {
            if (Uname.Text == "" || Upassword.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM usertb1 WHERE Uname = @Uname";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@Uname", Uname.Text);

                    int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (userExists > 0)
                    {
                        MessageBox.Show("Username already exists. Please choose a different username.");
                    }
                    else
                    {
                        // Hash the password before storing
                        string hashedPassword = HashPassword(Upassword.Text);

                        // Generate a random unique ID
                        Random rand = new Random();
                        int randomID = rand.Next(100000, 999999); // Generates a number between 100000 and 999999

                        string query = "INSERT INTO usertb1 (Id, Uname, Upass) VALUES (@Id, @Uname, @Upass)";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Id", randomID);
                        cmd.Parameters.AddWithValue("@Uname", Uname.Text);
                        cmd.Parameters.AddWithValue("@Upass", hashedPassword); // Store the hashed password

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User Successfully Added");
                        populate();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
        }

        //method for Hash password with SHA 256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Converts byte to hex string
                }
                return builder.ToString();
            }
        }
        //datagrid
        private void Users_Load(object sender, EventArgs e)
        {
            populate();
        }
        //delete button
        private void button3_Click(object sender, EventArgs e)
        {
            if (UID.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    string query = "DELETE FROM usertb1 WHERE Id = @Id";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", UID.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User deleted successfully!");
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
        }
        //data selection
        private void UserDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UID.Text = UserDGV.SelectedRows[0].Cells[0].Value.ToString();
            Uname.Text = UserDGV.SelectedRows[0].Cells[1].Value.ToString();
            Upassword.Text = UserDGV.SelectedRows[0].Cells[2].Value.ToString();
        }
        //edit button
        private void button2_Click(object sender, EventArgs e)
        {
            if (UID.Text == "" || Uname.Text == "" || Upassword.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    // Hash the password before updating
                    string hashedPassword = HashPassword(Upassword.Text);

                    string query = "UPDATE usertb1 SET Uname = @Uname, Upass = @Upass WHERE Id = @Id";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", UID.Text);
                    cmd.Parameters.AddWithValue("@Uname", Uname.Text);
                    cmd.Parameters.AddWithValue("@Upass", hashedPassword); // Store the hashed password

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Successfully Updated");
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
        }

        //back button
        private void button4_Click(object sender, EventArgs e)
        {
            MainForm main_form=new MainForm();
            main_form.Show();
            this.Hide();
        }
    }
}
