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
using static Member_Forms.Trainer_Login;
using System.Text.RegularExpressions;

namespace Member_Forms
{
    public partial class Trainer_EP : Form
    {
        public Trainer_EP()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(550, 100);
            pictureBox.Size = new Size(250, 300);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\training_exercise.jpg");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Trainer_Layout form = new Trainer_Layout();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LoadMemberIDs(int trainerid)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"SELECT Member.ID,Member.FIRSTNAME FROM Member 
            JOIN MemberExercise ON Member.ID = MemberExercise.MemberID
            WHERE MemberExercise.TrainerID = @TrainerID";
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerid);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    comboBoxMemberID.Items.Clear();

                    List<KeyValuePair<int, string>> members = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = int.Parse(reader["ID"].ToString());
                        string name = reader["FIRSTNAME"].ToString();
                        members.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }
                    reader.Close();
                    comboBoxMemberID.DataSource = new BindingSource(members, null);
                    comboBoxMemberID.DisplayMember = "Value";
                    comboBoxMemberID.ValueMember = "Key";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load member IDs: " + ex.Message);
            }
        }

        private void Trainer_EP_Load(object sender, EventArgs e)
        {
            int trainerid = SessionManager.TrainerId;
            LoadMemberIDs(trainerid);

        }

        private void comboBoxMemberID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMemberID.SelectedItem != null)
            {
                string selectedMember = comboBoxMemberID.SelectedItem.ToString();
                var match = Regex.Match(selectedMember, @"\d+");
                if (match.Success && int.TryParse(match.Value, out int selectedMemberId))
                {
                    CheckAndLoadWorkout(selectedMemberId);
                }
                else
                {
                    MessageBox.Show("Invalid member ID format.");
                }
            }
        }
        private void CheckAndLoadWorkout(int memberId)
        {
            int trainerid = SessionManager.TrainerId;
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT * FROM MemberExercise WHERE MemberID = '" + memberId.ToString() + "'and TrainerID = '" + trainerid.ToString() + "'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox2.Text = reader["ExerciseName"].ToString();
                        textBox3.Text = reader["ESets"].ToString();
                        textBox4.Text = reader["EReps"].ToString();
                        textBox5.Text = reader["TargetMuscle"].ToString();
                        textBox6.Text = reader["Machine"].ToString();
                        textBox7.Text = reader["RestInterval"].ToString();
                        MessageBox.Show("Exercise plan loaded for editing.");
                    }
                    else
                    {
                        MessageBox.Show("No exercise plan found for this member.");
                        ClearExerciseFields();
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Exercise Plan: " + ex.Message);
            }
        }

        private void ClearExerciseFields()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }

        private void UpdateMemberExerciseplan(int memberId,int trainerId,string exname,int eset,int erep,string muscle,string machine,string rest)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
            UPDATE MemberExercise SET 
            ExerciseName = @ExerciseName,ESets =@ESets,EReps = @EReps,
            TargetMuscle = @TargetMuscle,Machine = @Machine,RestInterval = @RestInterval
            WHERE MemberID = @MemberID AND TrainerID = @TrainerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberId);
                        cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                        cmd.Parameters.AddWithValue("@ExerciseName", exname);
                        cmd.Parameters.AddWithValue("@ESets", eset);
                        cmd.Parameters.AddWithValue("@EReps", erep);
                        cmd.Parameters.AddWithValue("@TargetMuscle", muscle ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Machine", machine);
                        cmd.Parameters.AddWithValue("@RestInterval", rest);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Exercise plan updated successfully.");
                        else
                            MessageBox.Show("No Esxercise plan was updated. Please check the member ID and trainer ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update diet plan: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int trainerId = SessionManager.TrainerId;
            string selectedMember = comboBoxMemberID.SelectedItem.ToString();
            var match = Regex.Match(selectedMember, @"\d+");

            if (match.Success && int.TryParse(match.Value, out int selectedMemberId))
            {
                string exname = textBox2.Text;
                int eset = Convert.ToInt32(textBox3.Text);
                int erep = Convert.ToInt32(textBox4.Text);
                string muscle = textBox5.Text;
                string machine = textBox6.Text;
                string rest = textBox7.Text;

                UpdateMemberExerciseplan(selectedMemberId, trainerId, exname, eset, erep, muscle, machine, rest);

            }
            else
            {
                MessageBox.Show("Invalid member ID format.");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

  
    }
}
