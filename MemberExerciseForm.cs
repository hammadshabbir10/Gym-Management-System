using System;
using System.CodeDom;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;


namespace Member_Forms
{
    public partial class MemberExerciseForm : Form
    {
        public MemberExerciseForm()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(550, 100);
            pictureBox.Size = new Size(250, 300);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\exercise.jpg");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);
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

        private void MemberExerciseForm_Load(object sender, EventArgs e)
        {
            int memberId = SessionManager.MemberId;
            LoadTrainers(memberId);
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            MemberLayout form = new MemberLayout();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Exiting the page!");
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (
                comboBoxTrainers.SelectedValue == null ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Please complete all required fields.");
                return;
            }
            int? eSets = string.IsNullOrWhiteSpace(textBox3.Text) ? (int?)0 : int.TryParse(textBox3.Text, out int tempSets) ? tempSets : (int?)null;
            int? eReps = string.IsNullOrWhiteSpace(textBox4.Text) ? (int?)0 : int.TryParse(textBox4.Text, out int tempReps) ? tempReps : (int?)null;

            int trainerId = Convert.ToInt32(comboBoxTrainers.SelectedValue);
            string exerciseName = textBox2.Text;
            string targetMuscle = string.IsNullOrWhiteSpace(textBox5.Text) ? null : textBox5.Text;
            string machine = string.IsNullOrWhiteSpace(textBox6.Text) ? null : textBox6.Text;
            string restInterval = textBox7.Text;
            int memberId = SessionManager.MemberId;
            if (CheckIfExercisePlanExists(memberId))
            {
                UpdateMemberExercisePlan(memberId,trainerId, exerciseName, eSets, eReps, targetMuscle, machine, restInterval);
               
            }
            else
            {
                InsertMemberExercise(memberId,trainerId, exerciseName, eSets, eReps, targetMuscle, machine, restInterval);
            }
          
        }
        private bool CheckIfExercisePlanExists(int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT COUNT(*) FROM MemberExercise WHERE MemberID = @MemberID";
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

        private void UpdateMemberExercisePlan(int memberId, int trainerId,string exerciseName,int? eSets,int? eReps,string targetMuscle,string machine,string restInterval)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
            UPDATE MemberExercise SET
             MemberID=@MemberID, TrainerID=@TrainerID,ExerciseName= @ExerciseName, ESets=@ESets,EReps= @EReps, 
             TargetMuscle= @TargetMuscle,Machine= @Machine,RestInterval =@RestInterval
            WHERE MemberID = @MemberID";
           

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    cmd.Parameters.AddWithValue("@ExerciseName", exerciseName);
                    cmd.Parameters.AddWithValue("@ESets", (object)eSets ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EReps", (object)eReps ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetMuscle", (object)targetMuscle ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Machine", (object)machine ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@RestInterval", restInterval);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Member exercise plan updated successfully.");
                }
            }
        }
        private void InsertMemberExercise(int memberId,int trainerId, string exerciseName, int? eSets, int? eReps, string targetMuscle, string machine, string restInterval)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
        INSERT INTO MemberExercise (MemberID, TrainerID, ExerciseName, ESets, EReps, TargetMuscle, Machine, RestInterval)
        VALUES (@MemberID, @TrainerID, @ExerciseName, @ESets, @EReps, @TargetMuscle, @Machine, @RestInterval)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                       
                        cmd.Parameters.AddWithValue("@MemberID", memberId);
                        cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                        cmd.Parameters.AddWithValue("@ExerciseName", exerciseName);
                        cmd.Parameters.AddWithValue("@ESets", (object)eSets ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@EReps", (object)eReps ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TargetMuscle", (object)targetMuscle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Machine", machine);
                        cmd.Parameters.AddWithValue("@RestInterval", restInterval);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Exercise added successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add exercise: " + ex.Message);
            }
        }
    }
}
