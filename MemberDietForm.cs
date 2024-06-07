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
    public partial class MemberDietForm : Form
    {
        public MemberDietForm()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(530, 80);
            pictureBox.Size = new Size(250, 300);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\diet.jfif");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);
        }

        private void button1_Click(object sender, EventArgs e)
        {
       
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

        private void MemberDietForm_Load(object sender, EventArgs e)
        {
            int memberId = SessionManager.MemberId;
            LoadTrainers(memberId);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please complete all required fields.");
                return;
            }

            int trainerId = Convert.ToInt32(comboBoxTrainers.SelectedValue);
            int? peanuts = string.IsNullOrWhiteSpace(textBox5.Text) ? (int?)0 : Convert.ToInt32(textBox5.Text);
            int? gluten = string.IsNullOrWhiteSpace(textBox6.Text) ? (int?)0 : Convert.ToInt32(textBox6.Text);
            int? lactose = string.IsNullOrWhiteSpace(textBox7.Text) ? (int?)0 : Convert.ToInt32(textBox7.Text);
            int? carbs = string.IsNullOrWhiteSpace(textBox8.Text) ? (int?)0 : Convert.ToInt32(textBox8.Text);
            int? protein = string.IsNullOrWhiteSpace(textBox9.Text) ? (int?)0 : Convert.ToInt32(textBox9.Text);
            int? fats = string.IsNullOrWhiteSpace(textBox10.Text) ? (int?)0 : Convert.ToInt32(textBox10.Text);
            int? fibre = string.IsNullOrWhiteSpace(textBox11.Text) ? (int?)0 : Convert.ToInt32(textBox11.Text);


            string typeofdiet = textBox2.Text;
            string nutrition = textBox3.Text;
            string purpose = textBox4.Text;
            int memberId = SessionManager.MemberId;
            if (CheckIfDietPlanExists(memberId))
            {
                UpdateMemberDietPlan(memberId, trainerId, typeofdiet, nutrition, purpose, peanuts, gluten, lactose, carbs, protein, fats, fibre);
            }
            else
            {
                InsertMemberDietPlan(memberId, trainerId, typeofdiet, nutrition, purpose, peanuts, gluten, lactose, carbs, protein, fats, fibre);
            }
          
        }
        private bool CheckIfDietPlanExists(int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT COUNT(*) FROM MemberDietPlan WHERE MemberID = @MemberID";
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

        private void UpdateMemberDietPlan(int memberId, int trainerId, string typeofdiet, string nutrition, string purpose, int? peanuts, int? gluten, int? lactose, int? carbs, int? protein, int? fats, int? fibre)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
            UPDATE MemberDietPlan SET
            TrainerID = @TrainerID,Typeofdiet = @Typeofdiet, Nutrition = @Nutrition,
            Purpose = @Purpose,Peanuts = @Peanuts, Gluten = @Gluten,Lactose = @Lactose,
            Carbs = @Carbs,Protein = @Protein,Fat = @Fat,Fibre = @Fibre
            WHERE MemberID = @MemberID";
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    cmd.Parameters.AddWithValue("@Typeofdiet", typeofdiet);
                    cmd.Parameters.AddWithValue("@Nutrition", nutrition);
                    cmd.Parameters.AddWithValue("@Purpose", purpose);
                    cmd.Parameters.AddWithValue("@Peanuts", peanuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gluten", gluten ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Lactose", lactose ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Carbs", carbs ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Protein", protein ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Fat", fats ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Fibre", fibre ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Member diet plan updated successfully.");
                }
            }
        }


        private void InsertMemberDietPlan(int memberid,int trainerid,string typeofdiet, string nutrition, string purpose,
            int? peanuts, int? lactose, int? gluten, int? carbs, int? protein, int? fats, int? fibre)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
                  INSERT INTO MemberDietPlan (MemberID, TrainerID,
                   Typeofdiet, Nutrition,Purpose,Peanuts, Gluten,Lactose, Carbs, Protein, Fat, Fibre)
                  VALUES (@MemberID,@TrainerID, @Typeofdiet,
                   @Nutrition, @Purpose,@Peanuts, @Gluten,@Lactose, @Carbs, @Protein, @Fat, @Fibre)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@MemberID", memberid);
                        cmd.Parameters.AddWithValue("@TrainerID", trainerid);
                        cmd.Parameters.AddWithValue("@Typeofdiet", typeofdiet);
                        cmd.Parameters.AddWithValue("@Nutrition", nutrition);
                        cmd.Parameters.AddWithValue("@Purpose", purpose);
                        cmd.Parameters.AddWithValue("@Peanuts", peanuts ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Gluten", gluten ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Lactose", lactose ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Carbs", carbs ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Protein", protein ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Fat", fats ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Fibre", fibre ?? (object)DBNull.Value);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Member diet plan added successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to submit diet plan: " + ex.Message);
            }
        }

    }
}
