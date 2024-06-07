using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Member_Forms.Owner_Login;
namespace Member_Forms
{
    public partial class OwnerAddTrainer : Form
    {
        public OwnerAddTrainer()
        {
            InitializeComponent();
        }

        private void OwnerAddTrainer_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            try
            {
                int ownerID = SessionManager.OwnerId;
                conn.Open();
                string query = @"
            SELECT T.ID, T.FIRSTNAME, G.GymID, G.GYMNAME
            FROM TRAINER T
            INNER JOIN TrainerGetAddGym TG ON T.ID = TG.TrainerID
            INNER JOIN GYM G ON TG.GymID = G.GYMID
             WHERE G.OWNERID = @OWNERID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OWNERID", ownerID);
                SqlDataReader reader = cmd.ExecuteReader();

                Dictionary<int, List<string>> trainerGyms = new Dictionary<int, List<string>>();
                while (reader.Read())
                { 
                    int id = int.Parse(reader["ID"].ToString());
                    string name = reader["FIRSTNAME"].ToString();
                    string gymId = reader["GymID"].ToString();
                    string gymName = reader["GYMNAME"].ToString();

                    string displayValue = $"{id} - {name} (Gym: {gymId} - {gymName})";

                    if (!trainerGyms.ContainsKey(id))
                    {
                        trainerGyms[id] = new List<string>();
                    }
                    trainerGyms[id].Add(displayValue);
                }
                reader.Close();

                comboBoxTrainerID.DataSource = new BindingSource(trainerGyms, null);
                comboBoxTrainerID.DisplayMember = "Value";
                comboBoxTrainerID.ValueMember = "Key";

                if (comboBoxTrainerID.Items.Count > 0)
                {
                    comboBoxTrainerID.SelectedIndex = 0;  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load trainers and gyms: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void LoadDetails(int trainerId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT T.FIRSTNAME, T.LASTNAME, T.DOB, T.GENDER, T.QUALIFICATION, T.EXPERIENCE, T.SPECIALITY, TGA.GymID
                FROM TRAINER T
                JOIN TrainerGetAddGym TGA ON T.ID = TGA.TrainerID
                WHERE T.ID = @TrainerID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        textBox2.Text = reader["FIRSTNAME"].ToString();
                        textBox3.Text = reader["LASTNAME"].ToString();
                        dateTimePicker1.Value = Convert.ToDateTime(reader["DOB"]);
                        comboBox1.Text = reader["GENDER"].ToString();
                        textBox5.Text = reader["QUALIFICATION"].ToString();
                        textBox7.Text = reader["EXPERIENCE"].ToString();
                        textBox9.Text = reader["SPECIALITY"].ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to fetch trainer details: " + ex.Message);
            }
        }
        private void comboBoxTrainerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTrainerID.SelectedValue != null)
            {
                string selectedTrainer = comboBoxTrainerID.SelectedItem.ToString();
                var match = Regex.Match(selectedTrainer, @"\d+");
                if (match.Success && int.TryParse(match.Value, out int selectedTrainerId))
                {
                    LoadDetails(selectedTrainerId);
                    LoadGymIDsForTrainer(selectedTrainerId);
                }
                else
                {
                    MessageBox.Show("Invalid trainer ID format.");
                }
            }
            
        }
        private void LoadGymIDsForTrainer(int trainerId)
        {
            int ownerID = SessionManager.OwnerId;
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                    SELECT TGA.GymID FROM TrainerGetAddGym TGA
                    INNER JOIN GYM G ON TGA.GymID = G.GYMID
                    WHERE TGA.TrainerID = @TrainerID AND G.OWNERID = @OWNERID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@OWNERID", ownerID);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<int> gymIds = new List<int>();
                    while (reader.Read())
                    {
                        gymIds.Add(reader.GetInt32(0)); 
                    }
                    reader.Close();

                    comboBox2.DataSource = gymIds; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Gym IDs: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int trainerId = Convert.ToInt32(comboBoxTrainerID.SelectedValue);
            int gymId;  
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select a gym.");
                return;
            }
            else
            {
                gymId = Convert.ToInt32(comboBox2.SelectedItem);
            }

            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string statusCheckQuery = "SELECT STATUS FROM GYM WHERE GYMID = @GymID";
                    SqlCommand statusCmd = new SqlCommand(statusCheckQuery, conn);
                    statusCmd.Parameters.AddWithValue("@GymID", gymId);
                    string gymStatus = (string)statusCmd.ExecuteScalar();
                    
                    if (gymStatus != "ACTIVE")
                    {
                        MessageBox.Show("This gym is not active. Trainers can only be assigned to active gyms.");
                        return;
                    }

                    string checkQuery = "SELECT COUNT(*) FROM GymTrainers WHERE TrainerID = @TrainerID AND GymID = @GymID";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    checkCmd.Parameters.AddWithValue("@GymID", gymId);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("This trainer is already assigned to the selected gym.");
                        return;
                    }

                    string insertQuery = "INSERT INTO GymTrainers (TrainerID, GymID) VALUES (@TrainerID, @GymID)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    insertCmd.Parameters.AddWithValue("@GymID", gymId);
                    insertCmd.ExecuteNonQuery();
                    string updateStatusQuery = "UPDATE Trainer SET STATUS = 'Active' WHERE ID = @TrainerID";
                    SqlCommand updateStatusCmd = new SqlCommand(updateStatusQuery, conn);
                    updateStatusCmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    updateStatusCmd.ExecuteNonQuery();

                    MessageBox.Show("Trainer assigned and activated successfully.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when assigning trainer to gym: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

