using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Member_Forms.Member_Login;

namespace Member_Forms
{ 
    public partial class MemberGetTrained : Form
    {
        public MemberGetTrained()
        {
            InitializeComponent();
        }

        private void LoadTrainers()
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            int memberId = SessionManager.MemberId;  
            string query = @"
        SELECT t.ID, t.FIRSTNAME 
        FROM Trainer t
        LEFT JOIN MemberGetTrained mgt ON t.ID = mgt.TrainerID AND mgt.MemberID = @MemberID
        WHERE mgt.TrainerID IS NULL";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MemberID", memberId); 
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> trainers = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = int.Parse(reader["ID"].ToString());
                        string name = reader["FIRSTNAME"].ToString();
                        trainers.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }

                    comboBoxTrainers.DataSource = new BindingSource(trainers, null);
                    comboBoxTrainers.DisplayMember = "Value";
                    comboBoxTrainers.ValueMember = "Key";
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading trainers: " + ex.Message);
            }
        }

        private void MemberGetTrained_Load(object sender, EventArgs e)
        { 
            LoadTrainers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxTrainers.SelectedItem != null)
            {
                string selectedMember = comboBoxTrainers.SelectedItem.ToString();
                var match = Regex.Match(selectedMember, @"\d+");
                if (match.Success && int.TryParse(match.Value, out int selectedTrainerId))
                {
                    int member_Id = SessionManager.MemberId;

                    string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";

                    string gymCheckQuery = @"
                SELECT COUNT(*) FROM GymMembers GM
                JOIN GymTrainers GT ON GM.GymID = GT.GymID
                WHERE GM.MemberID = @MemberID AND GT.TrainerID = @TrainerID";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand gymCheckCmd = new SqlCommand(gymCheckQuery, conn);
                        gymCheckCmd.Parameters.AddWithValue("@TrainerID", selectedTrainerId);
                        gymCheckCmd.Parameters.AddWithValue("@MemberID", member_Id);
                        conn.Open();
                        int sameGymCount = (int)gymCheckCmd.ExecuteScalar();
                        if (sameGymCount == 0)
                        {
                            MessageBox.Show("This trainer is not in the same gym as you.");
                            return;
                        }

                        string checkQuery = "SELECT COUNT(*) FROM MemberGetTrained WHERE MemberID = @MemberID and TrainerID = @TrainerID";
                        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@MemberID", member_Id);
                        checkCmd.Parameters.AddWithValue("@TrainerID", selectedTrainerId);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("You have already requested to this trainer and cannot request another.");
                            return;
                        }

                        string query = "INSERT INTO MemberGetTrained (MemberID, TrainerID) VALUES (@MemberID, @TrainerID)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@TrainerID", selectedTrainerId);
                        cmd.Parameters.AddWithValue("@MemberID", member_Id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Request sent to the trainer.");
                    }
                }
                else
                {
                    MessageBox.Show("Failed to parse trainer ID.");
                }
            }
            else
            {
                MessageBox.Show("Please select a trainer to get trained.");
            }
        }

    }
}
