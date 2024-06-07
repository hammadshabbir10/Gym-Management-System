using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Member_Forms.Member_Login;
using System.Data.SqlClient;

namespace Member_Forms
{
    public partial class MemberWorkoutForm : Form
    {
        public MemberWorkoutForm()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(530, 80);
            pictureBox.Size = new Size(250, 300);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\workout.jpg");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Back to the Layout");
            MemberLayout form = new MemberLayout();
            form.ShowDialog();
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

        private void MemberWorkoutForm_Load(object sender, EventArgs e)
        {
            int memberId = SessionManager.MemberId;
            LoadTrainers(memberId);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Exiting the Page");
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (
           
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please complete all required fields.");
                return;
            }

            int trainerId = Convert.ToInt32(comboBoxTrainers.SelectedValue);
            string title = textBox2.Text;
            string goal = textBox3.Text;
            string experience = textBox4.Text;
            string schedule = textBox5.Text;

            int memberId = SessionManager.MemberId;
            if (CheckIfWorkoutPlanExists(memberId))
            {
                UpdateMemberWorkoutPlan(memberId, trainerId, title, goal, experience, schedule);
            }
            else
            {
                InsertMemberWorkout(memberId, trainerId, title, goal, experience, schedule);
            }

           
        }
        private bool CheckIfWorkoutPlanExists(int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT COUNT(*) FROM MemberWorkout WHERE MemberID = @MemberID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void UpdateMemberWorkoutPlan(int memberId,int  trainerId,string title,string goal,string experience,string schedule)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
            UPDATE MemberWorkout SET
            TrainerID = @TrainerID,Title = @Title,Goal = @Goal,Experience = @Experience,
            Schedule = @Schedule WHERE MemberID = @MemberID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    cmd.Parameters.AddWithValue("@Title", title ?? (object)DBNull.Value);  // Handle nullable fields
                    cmd.Parameters.AddWithValue("@Goal", goal);
                    cmd.Parameters.AddWithValue("@Experience", experience);
                    cmd.Parameters.AddWithValue("@Schedule", schedule);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Member workout plan updated successfully.");
                }
            }
        }

        private void InsertMemberWorkout(int memberId, int trainerId, string title, string goal, string experience, string schedule)
{
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
        INSERT INTO MemberWorkout (MemberID, TrainerID, Title, Goal, Experience, Schedule)
        VALUES (@MemberID, @TrainerID, @Title, @Goal, @Experience, @Schedule)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@MemberID", memberId);
                        cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                        cmd.Parameters.AddWithValue("@Title", title ?? (object)DBNull.Value);  // Handle nullable fields
                        cmd.Parameters.AddWithValue("@Goal", goal);
                        cmd.Parameters.AddWithValue("@Experience", experience);
                        cmd.Parameters.AddWithValue("@Schedule", schedule);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Workout plan added successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to submit workout plan: " + ex.Message);
            }
        }
    }
}
