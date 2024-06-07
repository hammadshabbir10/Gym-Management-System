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
    public partial class MemberForm : Form
    {
        public MemberForm()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(520, 80);
            pictureBox.Size = new Size(250, 250);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\member1.jpg");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string type = comboBox2.Text;
            string fn = textBox2.Text;
            string ln = textBox3.Text;
            string em = textBox8.Text;
            string pw = textBox6.Text;
            string dob = dateTimePicker1.Value.ToString("yyyy-MM-dd"); 
            string gender = comboBox1.Text;
            string date = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(fn) || string.IsNullOrWhiteSpace(ln) ||
                string.IsNullOrWhiteSpace(em) || string.IsNullOrWhiteSpace(pw) || string.IsNullOrWhiteSpace(dob) ||
                string.IsNullOrWhiteSpace(gender) || string.IsNullOrWhiteSpace(date))
            {
                MessageBox.Show("Please complete all required fields.");
            }
            else
            {
                string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
                string query = "INSERT INTO MEMBER (FirstName, LastName, DOB, Email, Gender, MEMBERSHIPTYPE, REGISTRATIONDATE, Password) " +
                               "VALUES (@FirstName, @LastName, @DOB, @Email, @Gender, @MEMBERSHIPTYPE, @REGISTRATIONDATE, @Password); " +
                               "SELECT SCOPE_IDENTITY();";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@FirstName", fn);
                        cmd.Parameters.AddWithValue("@LastName", ln);
                        cmd.Parameters.AddWithValue("@DOB", dob);
                        cmd.Parameters.AddWithValue("@Email", em);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@MEMBERSHIPTYPE", type);
                        cmd.Parameters.AddWithValue("@REGISTRATIONDATE", date);
                        cmd.Parameters.AddWithValue("@Password", pw);

                        object result = cmd.ExecuteScalar(); // Executes query and returns the newly created ID
                        if (result != null)
                        {
                            int memberId = Convert.ToInt32(result);
                            MessageBox.Show($"Account successfully created. Member ID: {memberId}");

                            this.Close();
                            Member_Login form = new Member_Login();
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

        private void MemberForm_Load(object sender, EventArgs e)
        {
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            Member_Login form = new Member_Login();
            form.ShowDialog();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
