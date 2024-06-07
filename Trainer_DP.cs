using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static Member_Forms.Trainer_Login;
using System.Text.RegularExpressions;
namespace Member_Forms
{
    public partial class Trainer_DP : Form
    {
        public Trainer_DP()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(550, 100);
            pictureBox.Size = new Size(250, 300);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\dietplan.jpg");
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
            JOIN TrainerMembers ON Member.ID = TrainerMembers.MemberID
            WHERE TrainerMembers.TrainerID = @TrainerID";

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

        private void Trainer_DP_Load(object sender, EventArgs e)
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
                    CheckAndLoadDietPlan(selectedMemberId);
                }
                else
                {
                    MessageBox.Show("Invalid member ID format.");
                }
            }
        }
        private void CheckAndLoadDietPlan(int memberId)
        {
            int trainerid = SessionManager.TrainerId;
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT * FROM MemberDietPlan WHERE MemberID = '" + memberId.ToString() + "'and TrainerID = '" + trainerid.ToString() + "'";

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
                        textBox2.Text = reader["Typeofdiet"].ToString();
                        textBox3.Text = reader["Nutrition"].ToString();
                        textBox4.Text = reader["Purpose"].ToString();
                        textBox5.Text = reader["Peanuts"].ToString();
                        textBox6.Text = reader["Gluten"].ToString();
                        textBox7.Text = reader["Lactose"].ToString();
                        textBox8.Text = reader["Carbs"].ToString();
                        textBox9.Text = reader["Protein"].ToString();
                        textBox10.Text = reader["Fat"].ToString();
                        textBox11.Text = reader["Fibre"].ToString();

                        MessageBox.Show("Diet plan loaded for editing.");
                    }
                    else
                    {
                        MessageBox.Show("No diet plan found for this member.");
                        ClearDietPlanFields();
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Diet Plan: " + ex.Message);
            }
        }

        private void ClearDietPlanFields()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();

        }

        private void UpdateMemberDietPlan(int memberId, int trainerId, string typeofdiet, string nutrition, string purpose,
                                  int peanuts, int gluten, int lactose, int carbs, int protein, int fats, int fibre)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
        UPDATE MemberDietPlan
        SET Typeofdiet = @Typeofdiet, Nutrition = @Nutrition, Purpose = @Purpose, 
            Peanuts = @Peanuts, Gluten = @Gluten, Lactose = @Lactose, 
            Carbs = @Carbs, Protein = @Protein, Fat = @Fats, Fibre = @Fibre
        WHERE MemberID = @MemberID AND TrainerID = @TrainerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberId);
                        cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                        cmd.Parameters.AddWithValue("@Typeofdiet", typeofdiet);
                        cmd.Parameters.AddWithValue("@Nutrition", nutrition);
                        cmd.Parameters.AddWithValue("@Purpose", purpose);
                        cmd.Parameters.AddWithValue("@Peanuts", peanuts);
                        cmd.Parameters.AddWithValue("@Gluten", gluten);
                        cmd.Parameters.AddWithValue("@Lactose", lactose);
                        cmd.Parameters.AddWithValue("@Carbs", carbs);
                        cmd.Parameters.AddWithValue("@Protein", protein);
                        cmd.Parameters.AddWithValue("@Fats", fats);
                        cmd.Parameters.AddWithValue("@Fibre", fibre);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Diet plan updated successfully.");
                        else
                            MessageBox.Show("No diet plan was updated. Please check the member ID and trainer ID.");
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
           
                string typeofdiet = textBox2.Text;
                string nutrition = textBox3.Text;
                string purpose = textBox4.Text;
                int peanuts = Convert.ToInt32(textBox5.Text);
                int gluten = Convert.ToInt32(textBox6.Text);
                int lactose = Convert.ToInt32(textBox7.Text);
                int carbs = Convert.ToInt32(textBox8.Text);
                int protein = Convert.ToInt32(textBox9.Text);
                int fats = Convert.ToInt32(textBox10.Text);
                int fibre = Convert.ToInt32(textBox11.Text);

                UpdateMemberDietPlan(selectedMemberId, trainerId, typeofdiet, nutrition, purpose,
                                     peanuts, gluten, lactose, carbs, protein, fats, fibre);
            }
            else
            {
                MessageBox.Show("Invalid member ID format.");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
