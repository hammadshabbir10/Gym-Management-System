using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Member_Forms.Trainer_Login;
namespace Member_Forms
{
    public partial class Trainer_FB : Form
    {
        public Trainer_FB()
        {
            InitializeComponent();
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void LoadMemberIDs(int trainerid)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"SELECT Member.ID,Member.FIRSTNAME FROM Member 
            JOIN MemberFeedback ON Member.ID = MemberFeedback.MemberID
            WHERE MemberFeedback.TrainerID = @TrainerID";

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


        private void button1_Click(object sender, EventArgs e)
        {
            Trainer_Layout form = new Trainer_Layout();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Trainer_FB_Load(object sender, EventArgs e)
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
                    LoadFeedbackForSelectedMember(selectedMemberId);
                }
                else
                {
                    MessageBox.Show("Invalid member ID format.");
                }
            }
        }

        private void LoadFeedbackForSelectedMember(int memberId)
        {
            int trainerid = SessionManager.TrainerId;
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT Rating, Comment FROM MemberFeedback WHERE MemberID = '" + memberId.ToString() + "'and TrainerID = '" + trainerid.ToString() + "'";

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
                        textBox3.Text = reader["Rating"].ToString();
                        textBox4.Text = reader["Comment"].ToString();
                    }
                    else
                    {
                        textBox3.Clear();
                        textBox4.Clear();
                        MessageBox.Show("No feedback found for this member.");
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load feedback: " + ex.Message);
            }
        }

    }
}
