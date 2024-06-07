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
    public partial class Admin_MemRevoke : Form
    {
        public Admin_MemRevoke()
        {
            InitializeComponent();
        }

        private void Admin_MemRevoke_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            conn.Open();

            SqlCommand cmd;

            string query = "SELECT* FROM GYM WHERE 1 = 1";

            cmd = new SqlCommand(query, conn);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False"))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM GYM WHERE 1 = 1";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    if (!string.IsNullOrEmpty(textBox1.Text))
                    {
                        query += " AND GYMID = @GymID";
                        cmd.Parameters.AddWithValue("@GymID", textBox1.Text);
                    }

                    if (!string.IsNullOrEmpty(textBox2.Text))
                    {
                        query += " AND GYMNAME = @GymName";
                        cmd.Parameters.AddWithValue("@GymName", textBox2.Text);
                    }

                    if (!string.IsNullOrEmpty(textBox3.Text))
                    {
                        query += " AND OWNERID = @OwnerID";
                        cmd.Parameters.AddWithValue("@OwnerID", textBox3.Text);
                    }

                    if (!string.IsNullOrEmpty(textBox4.Text))
                    {
                        query += " AND LOCATION_ID = @LocationID";
                        cmd.Parameters.AddWithValue("@LocationID", textBox4.Text);
                    }

                    cmd.CommandText = query; // Update the command text after adding parameters

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sqlDataAdapter.Fill(ds);

                    dataGridView1.DataSource = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load data: " + ex.Message);
                }
            } // The connection is automatically closed here
        }
        int rowindex = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                rowindex = selectedRow.Index;

                textBox1.Text = selectedRow.Cells["GYMID"].Value.ToString();
                textBox2.Text = selectedRow.Cells["GYMNAME"].Value.ToString();
                textBox3.Text = selectedRow.Cells["OWNERID"].Value.ToString();
                textBox4.Text = selectedRow.Cells["LOCATION_ID"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False"))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            int gymId;
                            if (!int.TryParse(textBox1.Text, out gymId))
                            {
                                MessageBox.Show("Please enter a valid Gym ID.");
                                return;
                            }
                            List<string> queries = new List<string>
                            {
                                "DELETE FROM TrainerGetAddGym WHERE GymID = @GymID",
                                "DELETE FROM GymTrainers WHERE GymID = @GymID",
                                "DELETE FROM GYM_REQUEST WHERE GYM_ID = @GymID",
                                "DELETE FROM MemberGetAddGym WHERE GYMID = @GYMID",
                                "DELETE FROM GymMembers WHERE GYMID = @GYMID",
                            };

                            foreach (string query in queries)
                            {
                                SqlCommand cmd = new SqlCommand(query, conn, tran);
                                cmd.Parameters.AddWithValue("@GymID", gymId);
                                cmd.ExecuteNonQuery();
                            }
                            string deleteGym = "DELETE FROM GYM WHERE GYMID = @GymID";
                            SqlCommand deleteCmd = new SqlCommand(deleteGym, conn, tran);
                            deleteCmd.Parameters.AddWithValue("@GymID", gymId);
                            int result = deleteCmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show("Gym deleted successfully.");
                            }
                            else
                            {
                                MessageBox.Show("No gym found with the given ID.");
                            }

                            tran.Commit(); 
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show("Failed to delete gym: " + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }
    }
}
