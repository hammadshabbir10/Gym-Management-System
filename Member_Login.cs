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
    public partial class Member_Login : Form
    {
        public Member_Login()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(550, 100);  
            pictureBox.Size = new Size(250, 300); 
           
            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\flex.jpg"); 
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;  
            this.Controls.Add(pictureBox);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void username_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        public static class SessionManager
        {
            public static int MemberId { get; set; }  
        }

        public void Login(string idText, string password)
        {
            int id;
            if (!int.TryParse(idText, out id))
            {
                MessageBox.Show("Please enter a valid ID.");
                return;
            }

            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT ID FROM Member WHERE ID = @ID AND Password = @Password";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Password", password);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        SessionManager.MemberId = id;
                        MemberLayout form = new MemberLayout();
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Login failed. Please check ID and password.");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string idText = textBox1.Text;
            string password = textBox2.Text;
            Login(idText, password);
        }

            private void linklabe1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {   
                   MemberForm form = new MemberForm();
                  form.ShowDialog();
             }

    }
}
