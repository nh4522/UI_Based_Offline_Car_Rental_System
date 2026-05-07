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

namespace WindowsFormsApp3
{
    public partial class CreateAcc : Form
    {
        public CreateAcc()
        {
            InitializeComponent();
        }
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=carrental");
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        //back button
        private void button4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
        //create account button
        private void button1_Click(object sender, EventArgs e)
        {
            if (UnameRegister.Text == "" || UpasswordRegister.Text == "")
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    // Check if the username already exists
                    string checkQuery = "SELECT COUNT(*) FROM usertb1 WHERE Uname = @Uname";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@Uname", UnameRegister.Text);

                    int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (userExists > 0)
                    {
                        MessageBox.Show("Username already exists. Please choose a different username.");
                    }
                    else
                    {
                        // Generate a random ID
                        Random rnd = new Random();
                        int randomID = rnd.Next(100000, 999999); // Generates a number between 100000 and 999999

                        // Hash the password before saving
                        string hashedPassword = HashPassword(UpasswordRegister.Text);

                        // Insert the new user with the hashed password
                        string query = "INSERT INTO usertb1 (Id, Uname, Upass) VALUES (@Id, @Uname, @Upass)";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Id", randomID);
                        cmd.Parameters.AddWithValue("@Uname", UnameRegister.Text);
                        cmd.Parameters.AddWithValue("@Upass", hashedPassword);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User Successfully Added");
                        this.Hide();
                        Login login = new Login();
                        login.Show();
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

    }
}
