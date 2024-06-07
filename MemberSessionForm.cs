using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static Member_Forms.Member_Login;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Member_Forms
{
    public partial class MemberSessionForm : Form
    {
        public MemberSessionForm()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(400, 70);
            pictureBox.Size = new Size(350, 350);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\training.jpg");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Back to the Layout");
            MemberLayout form = new MemberLayout();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Exiting the page!");
            this.Close();
        }

        private void LoadTrainerIDs(int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"SELECT Trainer.ID,Trainer.FIRSTNAME FROM Trainer 
            JOIN TrainerSession ON Trainer.ID = TrainerSession.TrainerID
            WHERE TrainerSession.MemberID = @MemberID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    comboBoxALLTrainers.Items.Clear();

                    List<KeyValuePair<int, string>> members = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = int.Parse(reader["ID"].ToString());
                        string name = reader["FIRSTNAME"].ToString();
                        members.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }
                    reader.Close();
                    comboBoxALLTrainers.DataSource = new BindingSource(members, null);
                    comboBoxALLTrainers.DisplayMember = "Value";
                    comboBoxALLTrainers.ValueMember = "Key";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load member IDs: " + ex.Message);
            }
        }

        private void MemberSessionForm_Load(object sender, EventArgs e)
        {
            int memberId = SessionManager.MemberId;
            LoadTrainerIDs(memberId);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxALLTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxALLTrainers.SelectedItem != null)
            {
                string selectedTrainer = comboBoxALLTrainers.SelectedItem.ToString();
                var match = Regex.Match(selectedTrainer, @"\d+");
                if (match.Success && int.TryParse(match.Value, out int selectedTrainerId))
                {
                    LoadAppointmentForSelectedTrainer(selectedTrainerId);
                }
                else
                {
                    MessageBox.Show("Invalid trainer ID format.");
                }
            }
        }
        private void LoadAppointmentForSelectedTrainer(int trainerId)
        {
            int memberid = SessionManager.MemberId;
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT Appointmentstatus, Duration FROM TrainerSession WHERE MemberID = '" + memberid.ToString() + "'and TrainerID = '" + trainerId.ToString() + "'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        textBox3.Text = reader["Appointmentstatus"].ToString();
                        textBox2.Text = reader["Duration"].ToString();
                    }
                    else
                    {
                        textBox3.Clear();
                        textBox2.Clear();
                        MessageBox.Show("No Appointment found for this member.");
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Appointment: " + ex.Message);
            }
        }
    }
}
