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
    public partial class Admin_Login : Form
    {
        public Admin_Login()
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\admin_login.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox1);

            pictureBox2.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\user.png");
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox2);

            pictureBox3.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\password.png");
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox3);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
          
            conn.Open();

            SqlCommand cmd;

            string usr = textBox1.Text;
            string pwd = textBox2.Text;
            string query = "SELECT ADMIN_ID FROM ADMIN WHERE  USERNAME = '" + usr + "' AND  PASSWORD = '" + pwd + "' ";
            
            cmd = new SqlCommand(query, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Admin_Interface admin_Interface = new Admin_Interface();
                admin_Interface.ShowDialog();
            }
            else
            {
                MessageBox.Show("Login Un-Successful!! Invalid Credentials");
            }
            reader.Close();
            conn.Close();
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Admin_Login_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Text = "Password";
            }

        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.SelectAll();
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text = "Username/Email";
            }

        }

        private void linklabe1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            AdminSignup form = new AdminSignup();
            form.ShowDialog();
            
        }
    }
}
