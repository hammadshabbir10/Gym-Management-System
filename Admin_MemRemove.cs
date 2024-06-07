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
    public partial class Admin_MemRemove : Form
    {
        public Admin_MemRemove()
        {
            InitializeComponent();
        }

        private void Admin_MemRemove_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            conn.Open();

            SqlCommand cmd;

            string query = "SELECT ID, FIRSTNAME, LASTNAME, DOB, GENDER FROM MEMBER WHERE 1 = 1";

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

            string query = "SELECT ID, FIRSTNAME, LASTNAME, DOB, GENDER, EMAIL FROM MEMBER WHERE 1 = 1";

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
                query += " AND DOB = '" + textBox4.Text + "' ";
            }
            
            if (!string.IsNullOrEmpty(textBox5.Text))
            {
                query += " AND GENDER = '" + textBox5.Text + "' ";
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
                textBox4.Text = selectedRow.Cells["DOB"].Value.ToString();
                textBox5.Text = selectedRow.Cells["GENDER"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1 && rowindex != -1)
            {
                using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False"))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            string memberId = textBox1.Text;
                            List<string> deleteStatements = new List<string>()
                            {
                                "DELETE FROM MemberGetTrained WHERE MemberID = @MemberID",
                                "DELETE FROM MemberFeedback WHERE MemberID = @MemberID",
                                "DELETE FROM MemberDietPlan WHERE MemberID = @MemberID",
                                "DELETE FROM MemberWorkout WHERE MemberID = @MemberID",
                                "DELETE FROM MemberExercise WHERE MemberID = @MemberID",
                                "DELETE FROM MemberGetAddGym WHERE MemberID = @MemberID",
                                "DELETE FROM MEMBERSHIP_TYPE WHERE MEMBERID = @MemberID",
                                "DELETE FROM GymMembers WHERE MemberID = @MemberID",
                                "DELETE FROM TrainerMembers WHERE MemberID = @MemberID",
                                "DELETE FROM TrainerSession WHERE MemberID = @MemberID",
                                "DELETE FROM AuditTrailMember WHERE MemberID = @MemberID",
                                "DELETE FROM AuditTrailAppointment WHERE MemberID = @MemberID",
                                "DELETE FROM AuditTrailFeedback WHERE MemberID = @MemberID",
                                
                            };

                            foreach (var deleteQuery in deleteStatements)
                            {
                                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            string deleteTrainer = "DELETE FROM Member WHERE ID = @MemberID";
                            SqlCommand deleteCmd = new SqlCommand(deleteTrainer, conn, tran);
                            deleteCmd.Parameters.AddWithValue("@MemberID", memberId);
                            int result = deleteCmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show("Mmeber deleted successfully.");
                            }
                            else
                            {
                                MessageBox.Show("No Mmeber found with the given ID.");
                            }
                            tran.Commit();
                           
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show("Failed to delete member: " + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }

                    textBox1.Clear();
                    textBox2.Text = "First Name";
                    textBox2.ForeColor = SystemColors.ScrollBar;
                    textBox3.Text = "Last Name";
                    textBox3.ForeColor = SystemColors.ScrollBar;
                    textBox4.Clear();
                    textBox5.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please select a member to delete.");
            }
        }
    }
}
