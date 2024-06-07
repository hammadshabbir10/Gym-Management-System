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
using System.Text.RegularExpressions;
using static Member_Forms.Trainer_Login;
namespace Member_Forms
{
    public partial class AddMember : Form
    {
        public AddMember()
        {
            InitializeComponent();

        }

        private void AddMember_Load(object sender, EventArgs e)
        {
            int trainerId = SessionManager.TrainerId;
            LoadGetTrainedMembers(trainerId);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_Member();
        }

        private void Add_Member()
        {
            if (comboBoxAllMembers.SelectedValue == null)
            {
                MessageBox.Show("Please complete all fields before submitting.");
                return;
            }

            int memberId = Convert.ToInt32(comboBoxAllMembers.SelectedValue);
            Finally(memberId);
        }
        private void Finally(int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            int trainerId = SessionManager.TrainerId;

            if (MemberAlreadyAssigned(trainerId, memberId))
            {
                MessageBox.Show("This member is already assigned to this trainer.");
                return;
            }
            if (!TrainerAndMemberInSameGym(trainerId, memberId))
            {
                MessageBox.Show("This trainer and member are not in the same gym.");
                return;
            }

            string query = "INSERT INTO TrainerMembers (TrainerID, MemberID) VALUES (@TrainerID, @MemberID)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                        cmd.Parameters.AddWithValue("@MemberID", memberId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Member added successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add Member: " + ex.Message);
            }
        }

        private bool TrainerAndMemberInSameGym(int trainerId, int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string gymCheckQuery = @"
        SELECT COUNT(*) FROM GymTrainers GT
        JOIN GymMembers GM ON GT.GymID = GM.GymID
        WHERE GT.TrainerID = @TrainerID AND GM.MemberID = @MemberID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(gymCheckQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    conn.Open();
                    int result = (int)cmd.ExecuteScalar();
                    return result > 0;
                }
            }
        }

        private bool MemberAlreadyAssigned(int trainerId, int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string checkQuery = @"SELECT COUNT(*) FROM TrainerMembers WHERE TrainerID = @TrainerID AND MemberID = @MemberID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    conn.Open();
                    int result = (int)cmd.ExecuteScalar();
                    return result > 0;
                }
            }
        }

        private void LoadGetTrainedMembers(int trainerId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"SELECT Member.ID,Member.FIRSTNAME FROM Member
            JOIN MemberGetTrained ON Member.ID = MemberGetTrained.MemberID
            WHERE MemberGetTrained.TrainerID = @TrainerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> members = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = int.Parse(reader["ID"].ToString());
                        string name = reader["FIRSTNAME"].ToString();
                        members.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }
                    reader.Close();
                    comboBoxAllMembers.DataSource = new BindingSource(members, null);
                    comboBoxAllMembers.DisplayMember = "Value";
                    comboBoxAllMembers.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

    }


}
