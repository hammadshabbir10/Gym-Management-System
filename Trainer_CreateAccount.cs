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
    public partial class Trainer_CreateAccount : Form
    {
        public Trainer_CreateAccount()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(550, 100);
            pictureBox.Size = new Size(250, 300);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\trainer_Register.jpg");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Trainer_Login form = new Trainer_Login();
            form.ShowDialog();
        }

        private void Trainer_CreateAccount_Load(object sender, EventArgs e)
        {
            LoadGyms();
        }
        private void LoadGyms()
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT GYMID, GYMNAME FROM GYM";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    comboBox2.Items.Clear();

                    List<KeyValuePair<int, string>> gyms = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0); 
                        string name = reader.GetString(1);
                        gyms.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }
                    reader.Close();

                    comboBox2.DataSource = new BindingSource(gyms, null);
                    comboBox2.DisplayMember = "Value";
                    comboBox2.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load gyms: " + ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string fn = textBox2.Text;
            string ln = textBox3.Text;
            string em = textBox8.Text;
            string pw = textBox6.Text;
            string dob = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string gender = comboBox1.Text;
            string qualification = textBox7.Text;
            string experience = textBox5.Text;
            string speciality = textBox9.Text;

            if (string.IsNullOrWhiteSpace(fn) || string.IsNullOrWhiteSpace(ln) ||
                string.IsNullOrWhiteSpace(em) || string.IsNullOrWhiteSpace(pw) ||
                string.IsNullOrWhiteSpace(dob) || string.IsNullOrWhiteSpace(gender) ||
                string.IsNullOrWhiteSpace(qualification) || string.IsNullOrWhiteSpace(experience) ||
                string.IsNullOrWhiteSpace(speciality))
            {
                MessageBox.Show("Please complete all required fields.");
            }
            else
            {
                string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
                string query = "INSERT INTO Trainer (FirstName, LastName, DOB, Email, Gender, Password, Qualification, Experience, Speciality, Status) " +
                               "VALUES (@FirstName, @LastName, @DOB, @Email, @Gender, @Password, @Qualification, @Experience, @Speciality, 'Inactive'); " +
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
                        cmd.Parameters.AddWithValue("@Qualification", qualification);
                        cmd.Parameters.AddWithValue("@Experience", experience);
                        cmd.Parameters.AddWithValue("@Speciality", speciality);

                        object result = cmd.ExecuteScalar(); 
                        if (result != null)
                        {
                            int trainerId = Convert.ToInt32(result);
                            MessageBox.Show($"Account successfully created. Trainer ID: {trainerId}");

                            this.Close();
                            Trainer_Login form = new Trainer_Login();
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

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
