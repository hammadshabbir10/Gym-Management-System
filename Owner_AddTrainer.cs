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
using static Member_Forms.Owner_Login;
namespace Member_Forms
{
    public partial class Owner_AddTrainer : Form
    {
        public Owner_AddTrainer()
        {
            InitializeComponent();
        }

        private void Owner_AddTrainer_Load(object sender, EventArgs e)
        {

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            conn.Open();

            SqlCommand cmd;

            string query = "SELECT ID, FIRSTNAME, LASTNAME, DOB, GENDER, QUALIFICATION, EXPERIENCE, SPECIALITY FROM TRAINER WHERE 1 = 1";

            cmd = new SqlCommand(query, conn);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];

            conn.Close();
        }

        int rowindex = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                rowindex = selectedRow.Index;

                textBox1.Text = selectedRow.Cells["ID"].Value.ToString();
                textBox2.Text = selectedRow.Cells["FIRSTNAME"].Value.ToString();
                textBox3.Text = selectedRow.Cells["LASTNAME"].Value.ToString();
                textBox4.Text = selectedRow.Cells["DOB"].Value.ToString();
                textBox5.Text = selectedRow.Cells["GENDER"].Value.ToString();
                textBox6.Text = selectedRow.Cells["QUALIFICATION"].Value.ToString();
                textBox7.Text = selectedRow.Cells["EXPERIENCE"].Value.ToString();
                textBox8.Text = selectedRow.Cells["SPECIALITY"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            conn.Open();

            SqlCommand cmd;

            string query = "UPDATE TRAINER SET STATUS = 'ACTIVE' WHERE 1 = 1";

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                query += " AND ID = '" + textBox1.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                query += " AND FIRSTNAME = '" + textBox2.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                query += " AND LASTNAME = '" + textBox3.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                query += " AND DOB = '" + textBox4.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox5.Text))
            {
                query += " AND GENDER = '" + textBox5.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox6.Text))
            {
                query += " AND QUALIFICATION = '" + textBox6.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox7.Text))
            {
                query += " AND EXPERIENCE = '" + textBox7.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox8.Text))
            {
                query += " AND SPECIALITY = '" + textBox8.Text + "' ";
            }

            cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
