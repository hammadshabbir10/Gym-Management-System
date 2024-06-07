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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
namespace Member_Forms
{
    public partial class Owner_Login : Form
    {
        public Owner_Login()
        {
            InitializeComponent();
        }
        public static class SessionManager
        {
            public static int OwnerId { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            conn.Open();

            SqlCommand cmd;

            string usr = textBox1.Text;
            string pwd = textBox2.Text;
            string query = "SELECT OWNER_ID FROM GYMOWNER WHERE  OWNER_ID = '" + usr + "' AND PASSWORD = '" + pwd + "' ";

            cmd = new SqlCommand(query, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    SessionManager.OwnerId = Convert.ToInt32(reader["OWNER_ID"]);
                    Owner_Interface owner_Interface = new Owner_Interface();
                    owner_Interface.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Login Un-Successful!! Invalid Credentials");
            }
            reader.Close();
            conn.Close();
        }

        private void Owner_Login_Load(object sender, EventArgs e)
        {

        }

        private void linklabe1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OwnerSignup form = new OwnerSignup();
            form.ShowDialog();
        }
    }
}
