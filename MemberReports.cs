using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Member_Forms.Owner_Login;
using System.Windows.Forms;

namespace Member_Forms
{
    public partial class MemberReports : Form
    {
        private bool isFormLoading = true;

        public MemberReports()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Owner_Interface form = new Owner_Interface();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MemberReports_Load(object sender, EventArgs e)
        {
            LoadGyms();
            comboBoxALLMembers.DataSource = null;
            comboBoxALLTrainers.DataSource = null;
            textBox5.Clear();
            textBox4.Clear();
            isFormLoading = false;
        }

        private void LoadGyms()
        {
            int ownerId = SessionManager.OwnerId;
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT GYMID, GYMNAME FROM GYM WHERE OWNERID = '" + ownerId+ "'"; // Query to fetch all gyms
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@OWNERID", ownerId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    comboBoxGyms.Items.Clear();

                    List<KeyValuePair<int, string>> gyms = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        gyms.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }
                    reader.Close();
                    comboBoxGyms.DataSource = new BindingSource(gyms, null);
                    comboBoxGyms.DisplayMember = "Value";
                    comboBoxGyms.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load gyms: " + ex.Message);
            }
        }
        private void comboBoxGyms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFormLoading && comboBoxGyms.SelectedValue != null)
            {
                comboBoxALLMembers.DataSource = null;
                comboBoxALLTrainers.DataSource = null;
                textBox5.Clear();
                textBox4.Clear();
                int selectedGymId = ((KeyValuePair<int, string>)comboBoxGyms.SelectedItem).Key;
                LoadMembers(selectedGymId);
            }
        }

        private void LoadMembers(int gymId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
            SELECT Member.ID, Member.FIRSTNAME  FROM Member
            JOIN GymMembers ON Member.ID = GymMembers.MemberID
            WHERE GymMembers.GymID = @GymID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@GymID", gymId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> members = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        members.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }
                    reader.Close();

                    comboBoxALLMembers.DataSource = new BindingSource(members, null);
                    comboBoxALLMembers.DisplayMember = "Value";
                    comboBoxALLMembers.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void comboBoxALLMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxALLMembers.SelectedItem != null && comboBoxGyms.SelectedValue != null)
            {
                comboBoxALLTrainers.DataSource = null;
                textBox5.Clear();
                textBox4.Clear();
                int selectedMemberId = ((KeyValuePair<int, string>)comboBoxALLMembers.SelectedItem).Key;
                int gymId = ((KeyValuePair<int, string>)comboBoxGyms.SelectedItem).Key;
                LoadTrainersForMemberAndGym(selectedMemberId, gymId);
            }
        }

        private void LoadTrainersForMemberAndGym(int memberId, int gymId)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            try
            {
                conn.Open();
                string query = @"
            SELECT Trainer.ID, Trainer.FIRSTNAME
            FROM Trainer
            JOIN TrainerMembers ON Trainer.ID = TrainerMembers.TrainerID
            JOIN GymMembers ON TrainerMembers.MemberID = GymMembers.MemberID
            WHERE TrainerMembers.MemberID = @MemberID AND GymMembers.GymID = @GymID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MemberID", memberId);
                cmd.Parameters.AddWithValue("@GymID", gymId);
                SqlDataReader reader = cmd.ExecuteReader();

                List<KeyValuePair<int, string>> trainers = new List<KeyValuePair<int, string>>();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    trainers.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                }
                comboBoxALLTrainers.DataSource = new BindingSource(trainers, null);
                comboBoxALLTrainers.DisplayMember = "Value";
                comboBoxALLTrainers.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load trainers: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void comboBoxALLTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxALLTrainers.SelectedItem != null && comboBoxALLMembers.SelectedItem != null)
            {
                var selectedTrainer = (KeyValuePair<int, string>)comboBoxALLTrainers.SelectedItem;
                int selectedTrainerId = selectedTrainer.Key;
                textBox5.Clear();
                textBox4.Clear();

                FetchAndDisplayReport();
            }
        }




        private void FetchAndDisplayReport()
        {
            if (isFormLoading) return;

            if (comboBoxALLTrainers.SelectedValue != null && comboBoxALLMembers.SelectedValue != null)
            {
                var selectedTrainer = (KeyValuePair<int, string>)comboBoxALLTrainers.SelectedItem;
                int selectedTrainerId = selectedTrainer.Key;

                var selectedMember = (KeyValuePair<int, string>)comboBoxALLMembers.SelectedItem;
                int selectedMemberId = selectedMember.Key;

                string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
                string query = @"SELECT Appointmentstatus, Duration FROM TrainerSession WHERE MemberID = @MemberID AND TrainerID = @TrainerID";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MemberID", selectedMemberId);
                        cmd.Parameters.AddWithValue("@TrainerID", selectedTrainerId);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            textBox5.Text = reader["Appointmentstatus"].ToString();
                            textBox4.Text = reader["Duration"].ToString();
                        }
                        else
                        {
                            textBox5.Clear();
                            textBox4.Clear();
                            MessageBox.Show("No Report found for this member from the selected trainer.");
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load Report: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
