/*
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
namespace Member_Forms
{
    public partial class GymPerformance : Form
    {
        private bool isFormLoading = true;

        public GymPerformance()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GymPerformance_Load(object sender, EventArgs e)
        {
            LoadGyms();
            InitializeMembershipTypes();
            isFormLoading = false;
        }

        private void InitializeMembershipTypes()
        {
            List<string> membershipTypes = new List<string> { "Basic Membership", "Standard Membership", "Premium Membership" };
            comboBoxMembershipType.DataSource = membershipTypes;
        }
        private void LoadGyms()
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT GYMID, GYMNAME FROM GYM";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

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
            if (!isFormLoading && comboBoxGyms.SelectedValue != null && comboBoxMembershipType.SelectedItem != null)
            {
                int selectedGymId = ((KeyValuePair<int, string>)comboBoxGyms.SelectedItem).Key;
                string selectedMembershipType = comboBoxMembershipType.SelectedItem.ToString();
                LoadPerformances(selectedGymId, selectedMembershipType);
            }
        }
        private void comboBoxMembershipType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFormLoading && comboBoxGyms.SelectedValue != null && comboBoxMembershipType.SelectedItem != null)
            {
                int selectedGymId = ((KeyValuePair<int, string>)comboBoxGyms.SelectedItem).Key;
                string selectedMembershipType = comboBoxMembershipType.SelectedItem.ToString();
                LoadPerformances(selectedGymId, selectedMembershipType);
            }
        }
        private void LoadPerformances(int gymId, string membershipType)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False"))
            {
                connection.Open();

                // Get the maximum year of registration for the specific gym and membership type
                int maxYear = GetMaxYear(gymId, membershipType, connection);
                
                // Get the maximum month of the maximum year of registration for the specific gym and membership type
                int maxMonth = GetMaxMonth(gymId, membershipType, maxYear, connection);

                // Get the current and previous count of members
                int currentCount = GetMemberCount(gymId, membershipType, maxYear, maxMonth, connection);
                int previousCount = GetMemberCount(gymId, membershipType, maxYear, maxMonth - 1, connection);
                int memberDifference = currentCount - previousCount;

                // Calculate revenue for the current and previous period
                double membershipCost = GetMembershipCost(membershipType, connection);
                double currentRevenue = currentCount * membershipCost;
                double previousRevenue = previousCount * membershipCost;
                double revenueDifference = currentRevenue - previousRevenue;

                // Update textboxes with the calculated differences
                textBox5.Text = $"Member Growth: {memberDifference}";
                textBox4.Text = $"Revenue Growth: ${revenueDifference:F2}";

            }
        }


        // Helper method to get maximum year
        private int GetMaxYear(int gymId, string membershipType, SqlConnection connection)
        {
            string query = @"
        SELECT MAX(YEAR(M.RegistrationDate)) 
        FROM MEMBER M
        JOIN GymMembers GM ON M.ID = GM.MemberID
        WHERE GM.GymID = @GymID AND M.MembershipType = @MembershipType";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@GymID", gymId);
            command.Parameters.AddWithValue("@MembershipType", membershipType);
            object result = command.ExecuteScalar();
            return (result != DBNull.Value) ? Convert.ToInt32(result) : 0; // Handle case where no registrations exist
        }

        // Helper method to get maximum month
        private int GetMaxMonth(int gymId, string membershipType, int year, SqlConnection connection)
        {
            string query = @"
        SELECT MAX(MONTH(M.RegistrationDate))
        FROM MEMBER M
        JOIN GymMembers GM ON M.ID = GM.MemberID
        WHERE GM.GymID = @GymID AND M.MembershipType = @MembershipType AND YEAR(M.RegistrationDate) = @Year";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@GymID", gymId);
            command.Parameters.AddWithValue("@MembershipType", membershipType);
            command.Parameters.AddWithValue("@Year", year);
            object result = command.ExecuteScalar();
            return (result != DBNull.Value) ? Convert.ToInt32(result) : 0; // Handle case where no registrations exist
        }

        // Helper method to get member count
        private int GetMemberCount(int gymId, string membershipType, int year, int month, SqlConnection connection)
        {
            string query = @"
        SELECT COUNT(M.ID) 
        FROM MEMBER M
        JOIN GymMembers GM ON M.ID = GM.MemberID
        WHERE GM.GymID = @GymID AND M.MembershipType = @MembershipType AND YEAR(M.RegistrationDate) = @Year AND MONTH(M.RegistrationDate) = @Month";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@GymID", gymId);
            command.Parameters.AddWithValue("@MembershipType", membershipType);
            command.Parameters.AddWithValue("@Year", year);
            command.Parameters.AddWithValue("@Month", month);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        // Helper method to get membership cost
        private double GetMembershipCost(string membershipType, SqlConnection connection)
        {
            string query = "SELECT COST FROM MEMBERSHIP_TYPE WHERE NAME = @MembershipType";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MembershipType", membershipType);
            return Convert.ToDouble(command.ExecuteScalar());
        }


    }
}
*/

//////////////////////////////////////////////////////////////////////
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

namespace Member_Forms
{
    public partial class GymPerformance : Form
    {
        private bool isFormLoading = true;

        public GymPerformance()
        {
            InitializeComponent();
        }

        private void GymPerformance_Load(object sender, EventArgs e)
        {
            EnsureMembershipTypesExist();
            LoadGyms();
            InitializeMembershipTypes();
            isFormLoading = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void EnsureMembershipTypesExist()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False"))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM MEMBERSHIP_TYPE", conn);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        InsertPredefinedMembershipTypes(conn);
                    }
                }
            }
        }

        private void InsertPredefinedMembershipTypes(SqlConnection conn)
        {
            List<Tuple<string, decimal>> membershipTypes = new List<Tuple<string, decimal>>()
            {
                Tuple.Create("Basic Membership", 3m),
                Tuple.Create("Premium Membership", 8m),
                Tuple.Create("Standard Membership", 6m)
            };

            string query = "INSERT INTO MEMBERSHIP_TYPE (NAME, COST) VALUES (@Name, @Cost)";
            foreach (var type in membershipTypes)
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", type.Item1);
                    cmd.Parameters.AddWithValue("@Cost", type.Item2);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void InitializeMembershipTypes()
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT NAME FROM MEMBERSHIP_TYPE", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<string> membershipTypes = new List<string>();
                while (reader.Read())
                {
                    membershipTypes.Add(reader.GetString(0));
                }
                reader.Close();

                comboBoxMembershipType.DataSource = membershipTypes;
            }
        }

        private void LoadGyms()
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT GYMID, GYMNAME FROM GYM";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

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
            if (!isFormLoading && comboBoxGyms.SelectedValue != null && comboBoxMembershipType.SelectedItem != null)
            {
                int selectedGymId = ((KeyValuePair<int, string>)comboBoxGyms.SelectedItem).Key;
                string selectedMembershipType = comboBoxMembershipType.SelectedItem.ToString();
                LoadPerformances(selectedGymId, selectedMembershipType);
            }
        }

        private void comboBoxMembershipType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFormLoading && comboBoxGyms.SelectedValue != null && comboBoxMembershipType.SelectedItem != null)
            {
                int selectedGymId = ((KeyValuePair<int, string>)comboBoxGyms.SelectedItem).Key;
                string selectedMembershipType = comboBoxMembershipType.SelectedItem.ToString();
                LoadPerformances(selectedGymId, selectedMembershipType);
            }
        }

        private void LoadPerformances(int gymId, string membershipType)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False"))
            {
                connection.Open();

                int maxYear = GetMaxYear(gymId, membershipType, connection);
                int maxMonth = GetMaxMonth(gymId, membershipType, maxYear, connection);
                int currentCount = GetMemberCount(gymId, membershipType, maxYear, maxMonth, connection);
                int previousCount = GetMemberCount(gymId, membershipType, maxYear, maxMonth - 1, connection);
                int memberDifference = currentCount - previousCount;

                double membershipCost = GetMembershipCost(membershipType, connection);
                double currentRevenue = currentCount * membershipCost;
                double previousRevenue = previousCount * membershipCost;
                double revenueDifference = currentRevenue - previousRevenue;

                textBox5.Text = $"Member Growth: {memberDifference}";
                textBox4.Text = $"Revenue Growth: ${revenueDifference:F2}";
            }
        }

        // Helper method to get maximum year
        private int GetMaxYear(int gymId, string membershipType, SqlConnection connection)
        {
            string query = @"
        SELECT MAX(YEAR(M.RegistrationDate)) 
        FROM MEMBER M
        JOIN GymMembers GM ON M.ID = GM.MemberID
        WHERE GM.GymID = @GymID AND M.MembershipType = @MembershipType";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@GymID", gymId);
            command.Parameters.AddWithValue("@MembershipType", membershipType);
            object result = command.ExecuteScalar();
            return (result != DBNull.Value) ? Convert.ToInt32(result) : 0; // Handle case where no registrations exist
        }

        // Helper method to get maximum month
        private int GetMaxMonth(int gymId, string membershipType, int year, SqlConnection connection)
        {
            string query = @"
        SELECT MAX(MONTH(M.RegistrationDate))
        FROM MEMBER M
        JOIN GymMembers GM ON M.ID = GM.MemberID
        WHERE GM.GymID = @GymID AND M.MembershipType = @MembershipType AND YEAR(M.RegistrationDate) = @Year";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@GymID", gymId);
            command.Parameters.AddWithValue("@MembershipType", membershipType);
            command.Parameters.AddWithValue("@Year", year);
            object result = command.ExecuteScalar();
            return (result != DBNull.Value) ? Convert.ToInt32(result) : 0; // Handle case where no registrations exist
        }

        // Helper method to get member count
        private int GetMemberCount(int gymId, string membershipType, int year, int month, SqlConnection connection)
        {
            string query = @"
        SELECT COUNT(M.ID) 
        FROM MEMBER M
        JOIN GymMembers GM ON M.ID = GM.MemberID
        WHERE GM.GymID = @GymID AND M.MembershipType = @MembershipType AND YEAR(M.RegistrationDate) = @Year AND MONTH(M.RegistrationDate) = @Month";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@GymID", gymId);
            command.Parameters.AddWithValue("@MembershipType", membershipType);
            command.Parameters.AddWithValue("@Year", year);
            command.Parameters.AddWithValue("@Month", month);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        // Helper method to get membership cost
        private double GetMembershipCost(string membershipType, SqlConnection connection)
        {
            string query = "SELECT COST FROM MEMBERSHIP_TYPE WHERE NAME = @MembershipType";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MembershipType", membershipType);
            return Convert.ToDouble(command.ExecuteScalar());
        }

        // Helper methods are as defined in previous discussions, including GetMaxYear, GetMaxMonth, GetMemberCount, and GetMembershipCost.
    }
}