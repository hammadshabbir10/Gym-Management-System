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
    public partial class Admin_TraineRemove : Form
    {
        public Admin_TraineRemove()
        {
            InitializeComponent();
        }

        private void Admin_TraineRemove_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            conn.Open();

            SqlCommand cmd;

            string query = "SELECT ID, FIRSTNAME, LASTNAME, SPECIALITY, EXPERIENCE FROM TRAINER WHERE 1 = 1";

            cmd = new SqlCommand(query, conn);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];

            conn.Close();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.SelectAll();
            textBox2.ForeColor = SystemColors.WindowText;
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                textBox3.ForeColor = SystemColors.ScrollBar;
                textBox3.Text = "Last Name";
            }
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.SelectAll();
            textBox3.ForeColor = SystemColors.WindowText;
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                textBox2.ForeColor = SystemColors.ScrollBar;
                textBox2.Text = "First Name";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            conn.Open();

            SqlCommand cmd;

            string query = "SELECT ID, FIRSTNAME, LASTNAME, SPECIALITY, EXPERIENCE FROM TRAINER WHERE 1 = 1";

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                query += " AND ID = '" + textBox1.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox2.Text) && textBox2.Text != "First Name")
            {
                query += " AND FIRSTNAME = '" + textBox2.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox3.Text) && textBox3.Text != "Last Name")
            {
                query += " AND LASTNAME = '" + textBox3.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                query += " AND SPECIALITY = '" + textBox4.Text + "' ";
            }

            if (!string.IsNullOrEmpty(textBox5.Text))
            {
                query += " AND EXPERIENCE = '" + textBox5.Text + "' ";
            }

            cmd = new SqlCommand(query, conn);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];

            conn.Close();

            textBox1.Clear();
            textBox2.ForeColor = SystemColors.ScrollBar;
            textBox2.Text = "First Name";
            textBox3.ForeColor = SystemColors.ScrollBar;
            textBox3.Text = "Last Name";
            textBox4.Clear();
            textBox5.Clear();
        }

        int rowindex = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                rowindex = selectedRow.Index;

                textBox1.Text = selectedRow.Cells["ID"].Value.ToString();
                textBox2.ForeColor = SystemColors.WindowText;
                textBox2.Text = selectedRow.Cells["FIRSTNAME"].Value.ToString();
                textBox3.ForeColor = SystemColors.WindowText;
                textBox3.Text = selectedRow.Cells["LASTNAME"].Value.ToString();
                textBox4.Text = selectedRow.Cells["SPECIALITY"].Value.ToString();
                textBox5.Text = selectedRow.Cells["EXPERIENCE"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False"))
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();

                    try
                    {
                        int trainerId = Convert.ToInt32(textBox1.Text);
                        List<string> queries = new List<string>
                        {
                            "DELETE FROM MemberGetTrained WHERE TrainerID = @TrainerID",
                            "DELETE FROM MemberFeedback WHERE TrainerID = @TrainerID",
                            "DELETE FROM MemberDietPlan WHERE TrainerID = @TrainerID",
                            "DELETE FROM MemberWorkout WHERE TrainerID = @TrainerID",
                            "DELETE FROM MemberExercise WHERE TrainerID = @TrainerID",
                            "DELETE FROM TrainerMembers WHERE TrainerID = @TrainerID",
                            "DELETE FROM TrainerSession WHERE TrainerID = @TrainerID",
                            "DELETE FROM TrainerGetAddGym WHERE TrainerID = @TrainerID",
                            "DELETE FROM GymTrainers WHERE TrainerID = @TrainerID",
                            "DELETE FROM AuditTrailMember WHERE TrainerID = @TrainerID",
                            "DELETE FROM AuditTrailAppointment WHERE TrainerID = @TrainerID",
                            "DELETE FROM AuditTrailFeedback WHERE TrainerID = @TrainerID",
                        };

                        foreach (string query in queries)
                        {
                            SqlCommand cmd = new SqlCommand(query, conn, tran);
                            cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                            cmd.ExecuteNonQuery();
                        }

                        string deleteTrainer = "DELETE FROM Trainer WHERE ID = @TrainerID";
                        SqlCommand deleteCmd = new SqlCommand(deleteTrainer, conn, tran);
                        deleteCmd.Parameters.AddWithValue("@TrainerID", trainerId);
                        int result = deleteCmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Trainer deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No trainer found with the given ID.");
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Failed to delete trainer: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a trainer to delete.");
            }
        }
    }
}
