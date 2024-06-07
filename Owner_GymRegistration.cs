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
using static Member_Forms.Owner_Login;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Member_Forms
{
    public partial class Owner_GymRegistration : Form
    {
        public Owner_GymRegistration()
        {
            InitializeComponent();
        }

        private void Owner_GymRegistration_Load(object sender, EventArgs e)
        {
           
            LoadGyms();
        }

        private void LoadGyms()
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT GYMID, GYMNAME, OwnerID, STATUS,LOCATION_ID FROM GYM WHERE OwnerID = @OwnerID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                int ownerId = SessionManager.OwnerId;
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OwnerID", ownerId);  // You need to specify how you get this OwnerID

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void button2_Click(object sender, EventArgs e)  // Assuming this is your 'Register' button
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string gymName = textBox2.Text;  // Assuming textBox2 is for GymName
            string ownerID = textBox3.Text;  // Assuming textBox3 is for OwnerID
            string areaCode = textBox7.Text;  // Assuming textBox4 is for AreaCode
            string streetNo = textBox6.Text;  // Assuming textBox5 is for StreetNo
            string city = textBox5.Text;  // Assuming textBox6 is for City

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Insert into LOCATION table first
                string insertLocation = "INSERT INTO LOCATION (AreaCode, StreetNo, City) VALUES (@AreaCode, @StreetNo, @City)";
                SqlCommand cmdLocation = new SqlCommand(insertLocation, conn);
                cmdLocation.Parameters.AddWithValue("@AreaCode", areaCode);
                cmdLocation.Parameters.AddWithValue("@StreetNo", streetNo);
                cmdLocation.Parameters.AddWithValue("@City", city);
                cmdLocation.ExecuteNonQuery();

                // Get the last inserted Location ID
                string query = "SELECT @@IDENTITY AS 'Identity';";
                SqlCommand cmdIdentity = new SqlCommand(query, conn);
                int locationID = Convert.ToInt32(cmdIdentity.ExecuteScalar());

                string insertGym = "INSERT INTO GYM (GYMNAME, OWNERID, LOCATION_ID, STATUS) OUTPUT INSERTED.GYMID VALUES (@GYMNAME, @OWNERID, @LOCATION_ID, 'Pending')";
                SqlCommand cmdGym = new SqlCommand(insertGym, conn);
                cmdGym.Parameters.AddWithValue("@GYMNAME", gymName);
                cmdGym.Parameters.AddWithValue("@OWNERID", ownerID);
                cmdGym.Parameters.AddWithValue("@LOCATION_ID", locationID);
                int gymID = (int)cmdGym.ExecuteScalar(); // Execute and get the gym ID directly

                // Insert into GYM_REQUEST table
                string insertRequest = "INSERT INTO GYM_REQUEST (GYM_ID, OWNER_ID, REQ_DATE, STATUS) VALUES (@GYM_ID, @OWNER_ID, GETDATE(), 'Pending')";
                SqlCommand cmdRequest = new SqlCommand(insertRequest, conn);
                cmdRequest.Parameters.AddWithValue("@GYM_ID", gymID);
                cmdRequest.Parameters.AddWithValue("@OWNER_ID", ownerID);
                cmdRequest.ExecuteNonQuery();


            }

            MessageBox.Show("Gym registered successfully! Request is now pending.");

            // Optionally, reload the DataGridView to show new entry
            LoadGyms();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
