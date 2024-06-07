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
    public partial class OwnerAddMember : Form
    {
        public OwnerAddMember()
        {
            InitializeComponent();
        }

        private void OwnerAddMember_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            try
            {
                int ownerID = SessionManager.OwnerId;
                conn.Open();


                string query = @"
            SELECT M.ID, M.FIRSTNAME, G.GymID, G.GYMNAME
            FROM Member M
            INNER JOIN MemberGetAddGym TG ON M.ID = TG.MemberID
            INNER JOIN GYM G ON TG.GymID = G.GYMID
            WHERE G.OWNERID = @OWNERID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OWNERID", ownerID);
                SqlDataReader reader = cmd.ExecuteReader();

                Dictionary<int, List<string>> MemberGyms = new Dictionary<int, List<string>>();
                while (reader.Read())
                {
                    int id = int.Parse(reader["ID"].ToString());
                    string name = reader["FIRSTNAME"].ToString();
                    string gymId = reader["GymID"].ToString();
                    string gymName = reader["GYMNAME"].ToString();

                    string displayValue = $"{id} - {name} (Gym: {gymId} - {gymName})";

                    if (!MemberGyms.ContainsKey(id))
                    {
                        MemberGyms[id] = new List<string>();
                    }
                    MemberGyms[id].Add(displayValue);
                }
                reader.Close();

                comboBoxMemberID.DataSource = new BindingSource(MemberGyms, null);
                comboBoxMemberID.DisplayMember = "Value";
                comboBoxMemberID.ValueMember = "Key";
                if (comboBoxMemberID.Items.Count > 0)
                {
                    comboBoxMemberID.SelectedIndex = 0;
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

        private void comboBoxMemberID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMemberID.SelectedValue != null)
            {
                string selectedMember = comboBoxMemberID.SelectedItem.ToString();
                var match = Regex.Match(selectedMember, @"\d+");
                if (match.Success && int.TryParse(match.Value, out int selectedMemberId))
                {
                    LoadGymIDsForMember(selectedMemberId);
                }
                else
                {
                    MessageBox.Show("Invalid trainer ID format.");
                }
            }

        }

        private void LoadGymIDsForMember(int memberId)
        {
            int ownerID = SessionManager.OwnerId;
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
            SELECT M.GymID
            FROM MemberGetAddGym M
            JOIN GYM G ON M.GymID = G.GYMID
            WHERE M.MemberID = @MemberID AND G.OWNERID = @OWNERID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    cmd.Parameters.AddWithValue("@OWNERID", ownerID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<int> gymIds = new List<int>();
                    while (reader.Read())
                    {
                        gymIds.Add(reader.GetInt32(0));  
                    }
                    reader.Close();
                    comboBoxGyms.DataSource = gymIds;
                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Gym IDs for member: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int memberId = Convert.ToInt32(comboBoxMemberID.SelectedValue);
            int gymId; 

            if (comboBoxGyms.SelectedItem == null)
            {
                MessageBox.Show("Please select a gym.");
                return; 
            }
            else
            {
                gymId = Convert.ToInt32(comboBoxGyms.SelectedItem);
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
                        MessageBox.Show("This gym is not active. Members can only be assigned to active gyms.");
                        return;
                    }
                    string checkQuery = "SELECT GymID FROM GymMembers WHERE MemberID = @MemberID";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@MemberID", memberId);
                    object result = checkCmd.ExecuteScalar();

                    if (result != null)
                    {
                        int existingGymId = Convert.ToInt32(result);
                        if (existingGymId == gymId)
                        {
                            MessageBox.Show($"This member is already assigned to the selected gym (Gym ID: {gymId}).");
                        }
                        else
                        {
                            MessageBox.Show("This member is already assigned to another gym and cannot be assigned to a different gym.");
                        }
                        return;
                    }
                    string insertQuery = "INSERT INTO GymMembers (MemberID, GymID, RegistrationDate) VALUES (@MemberID, @GymID, @RegistrationDate)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@MemberID", memberId);
                    insertCmd.Parameters.AddWithValue("@GymID", gymId);
                    insertCmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now); 
                    insertCmd.ExecuteNonQuery();

                    MessageBox.Show("Member assigned to gym successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when assigning member to gym: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }

}
