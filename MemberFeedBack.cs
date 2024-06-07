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
using static Member_Forms.Member_Login;

namespace Member_Forms
{
    public partial class MemberFeedBack : Form
    {
        public MemberFeedBack()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SubmitFeedback();
        }

        private void LoadTrainers(int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"SELECT Trainer.ID,Trainer.FIRSTNAME from Trainer
                JOIN TrainerMembers ON Trainer.ID = TrainerMembers.TrainerID
                WHERE TrainerMembers.MemberID = @MemberID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<KeyValuePair<int, string>> trainers = new List<KeyValuePair<int, string>>();
                    comboBoxTrainers.Items.Clear();

                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["ID"]);
                        string name = reader["FIRSTNAME"].ToString();
                        trainers.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }

                    comboBoxTrainers.DataSource = new BindingSource(trainers, null);
                    comboBoxTrainers.DisplayMember = "Value";
                    comboBoxTrainers.ValueMember = "Key";
                    reader.Close();

                    if (trainers.Count == 0)
                        MessageBox.Show("No trainers found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading trainers: " + ex.Message);
            }
        }

        private void MemberFeedBack_Load(object sender, EventArgs e)
        {
            int memberId = SessionManager.MemberId;
            LoadTrainers(memberId);
        }

        private void SubmitFeedback()
        {
            if (comboBoxTrainers.SelectedValue == null || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please complete all fields before submitting.");
                return;
            }

            int trainerId = Convert.ToInt32(comboBoxTrainers.SelectedValue);
            string rating = textBox3.Text; 
            string comment = textBox4.Text;
            if (FeedbackExists(trainerId))
            {
                UpdateFeedback(trainerId, rating, comment);
            }
            else
            {
                InsertFeedback(trainerId, rating, comment);
            }

        }
        private bool FeedbackExists(int trainerId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT COUNT(1) FROM MemberFeedback WHERE TrainerID = @TrainerID AND MemberID = @MemberID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int memberId = SessionManager.MemberId;
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void UpdateFeedback(int trainerId, string rating, string comment)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "UPDATE MemberFeedback SET Rating = @Rating, Comment = @Comment WHERE TrainerID = @TrainerID AND MemberID = @MemberID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int memberId = SessionManager.MemberId;
                        cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                        cmd.Parameters.AddWithValue("@MemberID", memberId);
                        cmd.Parameters.AddWithValue("@Rating", rating);
                        cmd.Parameters.AddWithValue("@Comment", comment);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Feedback updated successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update feedback: " + ex.Message);
            }
        }

        private void InsertFeedback(int trainerId, string rating, string comment)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "INSERT INTO MemberFeedback (TrainerID, MemberID, Rating, Comment) VALUES (@TrainerID, @MemberID, @Rating, @Comment)";
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                      
                        int memberId = SessionManager.MemberId;

                        cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                        cmd.Parameters.AddWithValue("@MemberID", memberId);
                        cmd.Parameters.AddWithValue("@Rating", rating);
                        cmd.Parameters.AddWithValue("@Comment", comment);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Feedback submitted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to submit feedback: " + ex.Message);
            }
        }
    }
}
