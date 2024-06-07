using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Member_Forms
{
    public partial class AdminSignup : Form
    {
        public AdminSignup()
        {
            InitializeComponent();
        }

        private void AdminSignup_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "INSERT INTO ADMIN (USERNAME, PASSWORD, EMAIL) VALUES (@Username, @Password, @Email)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@USERNAME", textBox1.Text);
                        cmd.Parameters.AddWithValue("@PASSWORD", textBox3.Text);  
                        cmd.Parameters.AddWithValue("@EMAIL", textBox2.Text);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result < 1)
                            MessageBox.Show("Signup failed.");
                        else
                        {
                            MessageBox.Show("Signup successful.");
                            this.Close();
                            Admin_Login form = new Admin_Login();
                            form.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            Admin_Login form = new Admin_Login();
            form.ShowDialog();

        }
    }
}
