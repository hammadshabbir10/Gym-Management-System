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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Member_Forms
{
    public partial class OwnerSignup : Form
    {
        public OwnerSignup()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Owner_Login form = new Owner_Login();
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fn = textBox2.Text;
            string ln = textBox3.Text;
            string em = textBox8.Text;
            string pw = textBox6.Text;
            string dob = dateTimePicker1.Value.ToString("yyyy-MM-dd"); 
            string gender = comboBox1.Text;
          

            if (string.IsNullOrWhiteSpace(fn) || string.IsNullOrWhiteSpace(ln) ||
                string.IsNullOrWhiteSpace(em) || string.IsNullOrWhiteSpace(pw) ||
                string.IsNullOrWhiteSpace(dob) || string.IsNullOrWhiteSpace(gender))
                
            {
                MessageBox.Show("Please complete all required fields.");
            }
            else
            {
                string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
                string query = "INSERT INTO GYMOWNER (FirstName, LastName, DOB, Email, Gender, Password) " +
                               "VALUES (@FirstName, @LastName, @DOB, @Email, @Gender, @Password); " +
                               "SELECT SCOPE_IDENTITY();";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                       
                        cmd.Parameters.AddWithValue("@FirstName", fn);
                        cmd.Parameters.AddWithValue("@LastName", ln);
                        cmd.Parameters.AddWithValue("@DOB", dob);
                        cmd.Parameters.AddWithValue("@Email", em);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Password", pw);
                      
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int ownerId = Convert.ToInt32(result);
                            MessageBox.Show($"Account successfully created. Owner ID: {ownerId}");

                            this.Close();
                            Owner_Login form = new Owner_Login();
                            form.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Failed to create account.");
                        }
                    }
                }
            }
        }

        private void OwnerSignup_Load(object sender, EventArgs e)
        {

        }
    }
}
