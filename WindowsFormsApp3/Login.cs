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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=carrental");
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }
        //close button
        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //login button
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserIDTb.Text) || string.IsNullOrEmpty(PasswordTb.Text))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // Query to retrieve the hashed password for the provided username
                string query = "SELECT Upass FROM usertb1 WHERE Uname = @Uname";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Uname", UserIDTb.Text);

                // Fetch the result from the database
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string storedHashedPassword = result.ToString();
                    string enteredHashedPassword = HashPassword(PasswordTb.Text); // Hash the entered password

                    // Debugging: Show the stored and entered hashed passwords for comparison
                   // MessageBox.Show($"Stored: {storedHashedPassword}\nEntered: {enteredHashedPassword}");

                    // Compare the entered hashed password with the stored hashed password
                    if (storedHashedPassword == enteredHashedPassword)
                    {
                        //MessageBox.Show("Login successful!");
                        MainForm MAIN = new MainForm();
                        MAIN.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect username or password.");
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect username or password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }



        //method for hash password SHA 256
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


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Set the PasswordChar to '*', or use another character of your choice
            PasswordTb.PasswordChar = '*';
        }

        private void PasswordTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            CreateAcc create_acc= new CreateAcc();
            create_acc.Show();
            this.Hide();
        }
    }
}
