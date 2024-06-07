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
    public partial class Admin_MemRequests : Form
    {
        public Admin_MemRequests()
        {
            InitializeComponent();
        }

        private void Admin_MemRequests_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            conn.Open();

            SqlCommand cmd;

            string query = "SELECT REQ_ID, GYM_ID, OWNER_ID, REQ_DATE FROM GYM_REQUEST WHERE STATUS = 'PENDING'";

            cmd = new SqlCommand(query, conn);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            conn.Open();

            SqlCommand cmd;

            string query = "SELECT REQ_ID, GYM_ID, OWNER_ID, REQ_DATE FROM GYM_REQUEST WHERE STATUS = 'PENDING'";

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                query += " AND REQ_ID = '" + textBox1.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                query += " AND GYM_ID = '" + textBox2.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                query += " AND OWNER_ID = '" + textBox3.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                query += " AND REQ_DATE = '" + textBox4.Text + "' ";
            }

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

                textBox1.Text = selectedRow.Cells["REQ_ID"].Value.ToString();
                textBox2.Text = selectedRow.Cells["GYM_ID"].Value.ToString();
                textBox3.Text = selectedRow.Cells["OWNER_ID"].Value.ToString();
                textBox4.Text = selectedRow.Cells["REQ_DATE"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1 && rowindex != -1)
            {
                SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
                conn.Open();

                SqlCommand cmd;

                string query = "DELETE FROM GYM_REQUEST WHERE STATUS = 'PENDING'";

                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    query += " AND REQ_ID = '" + textBox1.Text + "' ";
                }

                if (!string.IsNullOrEmpty(textBox2.Text))
                {
                    query += " AND GYM_ID = '" + textBox2.Text + "' ";
                }

                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    query += " AND OWNER_ID = '" + textBox3.Text + "' ";
                }

                if (!string.IsNullOrEmpty(textBox4.Text))
                {
                    query += " AND REQ_DATE = '" + textBox4.Text + "' ";
                }

                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                query = "DELETE FROM GYM WHERE STATUS = 'PENDING'";

                if (!string.IsNullOrEmpty(textBox2.Text))
                {
                    query += " AND GYMID = '" + textBox2.Text + "' ";
                }

                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    query += " AND OWNERID = '" + textBox3.Text + "' ";
                }

                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        int admin_id = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1 && rowindex != -1)
            {
                SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
                conn.Open();

                SqlCommand cmd;

                string query = "UPDATE GYM_REQUEST SET STATUS = 'APPROVED' WHERE STATUS = 'PENDING'";

                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    query += " AND REQ_ID = '" + textBox1.Text + "' ";
                }

                if (!string.IsNullOrEmpty(textBox2.Text))
                {
                    query += " AND GYM_ID = '" + textBox2.Text + "' ";
                }

                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    query += " AND OWNER_ID = '" + textBox3.Text + "' ";
                }

                if (!string.IsNullOrEmpty(textBox4.Text))
                {
                    query += " AND REQ_DATE = '" + textBox4.Text + "' ";
                }

                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                query = "UPDATE GYM SET STATUS = 'ACTIVE' WHERE STATUS = 'PENDING'";

                if (!string.IsNullOrEmpty(textBox2.Text))
                {
                    query += " AND GYMID = '" + textBox2.Text + "' ";
                }

                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    query += " AND OWNERID = '" + textBox3.Text + "' ";
                }

                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}