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
using System.Text.RegularExpressions;

namespace Member_Forms
{
    public partial class MemberAddGym : Form
    {
        public MemberAddGym()
        {
            InitializeComponent();
        }

        private void MemberAddGym_Load(object sender, EventArgs e)
        {
            LoadAvailableGyms(SessionManager.MemberId);
        }
        private void LoadAvailableGyms(int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
            SELECT GYM.GYMID, GYM.GYMNAME  FROM GYM
            LEFT JOIN MemberGetAddGym ON GYM.GYMID = MemberGetAddGym.GymID AND MemberGetAddGym.MemberID = @MemberID
            WHERE MemberGetAddGym.GymID IS NULL";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> gyms = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = int.Parse(reader["GYMID"].ToString());
                        string name = reader["GYMNAME"].ToString();
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
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxGyms.SelectedItem != null)
            {
                string selectedGym = comboBoxGyms.SelectedItem.ToString();
                var match = Regex.Match(selectedGym, @"\d+");
                if (match.Success && int.TryParse(match.Value, out int selectedGymId))
                {
                    int member_Id = SessionManager.MemberId;

                    string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";


                    string checkQuery = "SELECT COUNT(*) FROM MemberGetAddGym WHERE MemberID = @MemberID AND GymID = @GymID";
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@MemberID", member_Id);
                        checkCmd.Parameters.AddWithValue("@GymID", selectedGymId);
                        conn.Open();
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("You are already request to this gym and cannot request another.");
                            return;
                        }
                    }


                    string query = "INSERT INTO MemberGetAddGym (MemberID, GymID) VALUES (@MemberID, @GymID)";
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@GymID", selectedGymId);
                        cmd.Parameters.AddWithValue("@MemberID", member_Id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Request sent.");
                    }
                }
                else
                {
                    MessageBox.Show("Failed to parse Gym ID.");
                }
            }
            else
            {
                MessageBox.Show("Please select a Gym.");
            }
        }
    }
}
