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
    public partial class QueryReports : Form
    {
        public QueryReports()
        {
            InitializeComponent();
        }

        private void QueryReports_Load(object sender, EventArgs e)
        {
            InitializeReportOptions();
        }
    private void InitializeReportOptions()
    {
    var reports = new Dictionary<int, string>
    {
        { 1, "Details of members trained by a specific trainer in a specific gym" },
        { 2, "Details of members in a specific gym following a specific diet" },
        { 3, "Details of members across all gyms of a specific trainer on a diet" },
        { 4, "Count of members using a specific machine on a given day in a specific gym" },
        { 5, "List of Diet plans with < 500 calorie breakfast" },
        { 6, "Diet plans with < 300 grams of carbohydrates" },
        { 7, "Workout plans that don’t require a specific machine" },
        { 8, "Diet plans without peanuts as allergens" },
        { 9, "New membership data in the last 3 months" },
        { 10, "Comparison of total members in multiple gyms in the past 6 months" },
        { 11, "List all members trained by each trainer" },
        { 12, "List all diets followed by members under each trainer" },
        { 13, "List all members attending sessions with each trainer" },
        { 14, "List trainers and the members who provided feedback" },
        { 15, "List all members who have a premium membership type in a gym" },
        { 16, "List all owners of specific gyms" },
        { 17, "List all gym requests created by owners" },
        { 18, "List of gyms where status is pending" },
        { 19, "List all members whose gender is male" },
        { 20, "Find the oldest and youngest members" },
         { 21, "List all members along with their complete details." },
        { 22, "List all trainers along with their complete details including qualifications and specialties." },
        { 23, "List all gym owners along with their personal details." },
        { 24, "List all admins along with their usernames and email addresses." },
        { 25, "List all gyms along with their statuses and associated owner and location IDs." }
    };

            comboBoxALLQueries.DataSource = new BindingSource(reports, null);
            comboBoxALLQueries.DisplayMember = "Value";
            comboBoxALLQueries.ValueMember = "Key";
    }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBoxALLQueries_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewResults.DataSource = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedReportId = ((KeyValuePair<int, string>)comboBoxALLQueries.SelectedItem).Key;
            ExecuteSelectedQuery(selectedReportId);
        }
        private void ExecuteSelectedQuery(int reportId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = GetQueryByReportId(reportId);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridViewResults.DataSource = dt;
            }
        }

        private string GetQueryByReportId(int reportId)
        {
            switch (reportId)
            {
                case 1:
                    // Details of members of one specific gym that get training from one specific trainer
                    return @"
                SELECT m.ID, m.FirstName, m.LastName, m.Email
                FROM Member m
                JOIN MemberGetTrained mgt ON m.ID = mgt.MemberID
                WHERE mgt.TrainerID = 10 AND m.ID IN 
                (
                    SELECT gm.MemberID FROM GymMembers gm WHERE gm.GymID = 1
                );";

                case 2:
                    // Details of members from one specific gym that follow a specific diet plan
                    return @"
                SELECT m.ID, m.FirstName, m.LastName, m.Email
                FROM Member m
                JOIN MemberDietPlan mdp ON m.ID = mdp.MemberID
                WHERE mdp.Typeofdiet = 'low' AND m.ID IN 
                (
                    SELECT gm.MemberID FROM GymMembers gm WHERE gm.GymID = 7
                );";

                case 3:
                    // Details of members across all gyms of a specific trainer that follow a specific diet plan
                    return @"
                SELECT m.ID, m.FirstName, m.LastName, m.Email
                FROM Member m
                JOIN MemberDietPlan mdp ON m.ID = mdp.MemberID
                JOIN MemberGetTrained mgt ON m.ID = mgt.MemberID
                WHERE mdp.Typeofdiet = 'low' AND mgt.TrainerID = 10;";

                case 4:
                    // Count of members who will be using specific machines on a given day in a specific gym
                    return @"
                    SELECT COUNT(DISTINCT m.ID) AS UserCount
                    FROM Member m
                    JOIN MemberExercise me ON m.ID = me.MemberID
                    JOIN TrainerSession ts ON m.ID = ts.MemberID
                    WHERE me.Machine = 'NULS'
                    AND me.MemberID IN 
                    (
                        SELECT gm.MemberID FROM GymMembers gm WHERE gm.GymID = 7
                    )
                    AND ts.AppointmentStatus = 'Sechdeuled'";
                   
                case 5:
                    // List of Diet plans that have less than 500 calorie meals as breakfast
                    return @"
                SELECT MemberID, Typeofdiet
                FROM MemberDietPlan
                WHERE (Carbs + Protein) < 500;";

                case 6:
                    // List of diet plans in which total carbohydrate intake is less than 300 grams
                    return @"
                SELECT MemberID, Typeofdiet
                FROM MemberDietPlan
                WHERE Carbs < 300;";

                case 7:
                    // List of workout plans that don’t require using a specific machine
                    return @"
                SELECT MemberID, ExerciseName
                FROM MemberExercise
                WHERE Machine IS NULL OR Machine = 'None';";

                case 8:
                    // List of diet plans which doesn’t have peanuts as allergens
                    return @"
                SELECT MemberID, Typeofdiet
                FROM MemberDietPlan
                WHERE Peanuts = 0;";

                case 9:
                    // New membership data in the last 3 months (Gym Owner)
                    return @"
                SELECT go.FIRSTNAME AS OwnerFirstName, go.LASTNAME AS OwnerLastName, COUNT(gm.MemberID) AS TotalNewMembers
                FROM GYMOWNER go
                JOIN GYM g ON go.OWNER_ID = g.OWNERID
                JOIN GymMembers gm ON g.GYMID = gm.GymID
                WHERE gm.RegistrationDate >= DATEADD(MONTH, -3, GETDATE())
                GROUP BY go.FIRSTNAME, go.LASTNAME;";

                case 10:
                    // Comparison of total members in multiple gyms, in the past 6 months
                    return @"
                SELECT g.GYMNAME AS GymName, COUNT(DISTINCT gm.MemberID) AS TotalMembers
                FROM GYM g
                JOIN GymMembers gm ON g.GYMID = gm.GymID
                WHERE gm.RegistrationDate >= DATEADD(MONTH, -6, GETDATE())
                GROUP BY g.GYMNAME;";

                case 11:
                    return @"SELECT m.ID as MemberID, m.FIRSTNAME, m.LASTNAME, m.EMAIL, t.ID as TrainerID
                     FROM Member m
                     JOIN MemberGetTrained mt on m.ID = mt.MemberID
                     Join Trainer t on t.ID = mt.TrainerID
                     WHERE mt.TrainerID = t.ID
                     Order by m.ID;";

                case 12:
                    return @"SELECT  md.MemberID, md.Typeofdiet, t.ID as TrainerID
                     FROM MemberDietPlan md
                     JOIN MemberGetTrained mt ON md.MemberID = mt.MemberID
                     Join Trainer t on t.ID = mt.TrainerID
                     WHERE mt.TrainerID = t.ID
                     Order by md.MemberID;";

                case 13:
                    return @"SELECT  m.ID, m.FIRSTNAME, m.LASTNAME, m.EMAIL, t.ID as TrainerID
                     FROM Member m
                     JOIN TrainerSession ts ON m.ID = ts.MemberID
                     Join Trainer t on t.ID = ts.TrainerID
                     WHERE ts.TrainerID = t.ID
                     Order By m.ID Asc;";

                case 14:
                    return @"SELECT t.ID as TrainerID, t.FIRSTNAME, t.LASTNAME, m.ID as MemberID
                     FROM Trainer t
                     JOIN MemberFeedback mf ON t.ID = mf.TrainerID
                     Join Member m on m.ID = mf.MemberID
                     WHERE mf.MemberID = m.ID
                     ORDER BY t.ID ASC;";

                case 15:
                    return @"
                SELECT m.ID, m.FirstName, m.LastName, m.Email
                FROM Member m
                JOIN GymMembers gm ON m.ID = gm.MemberID
                WHERE gm.GymID = 1 AND m.MEMBERSHIPTYPE = 'Premium Membership';";

                case 16:
                    return @"
                SELECT g.GYMID, g.GYMNAME, go.FIRSTNAME, go.LASTNAME
                FROM GYM g
                JOIN GYMOWNER go ON g.OWNERID = go.OWNER_ID
                WHERE g.GYMID = 2;";

                case 17:
                    return @"
                SELECT g.GYMID, g.GYMNAME, go.FIRSTNAME, go.LASTNAME, gr.REQ_DATE
                FROM GYM_REQUEST gr
                JOIN GYM g ON gr.GYM_ID = g.GYMID
                JOIN GYMOWNER go ON g.OWNERID = go.OWNER_ID
                ORDER BY gr.REQ_DATE DESC;";

                case 18:
                    return @"
                SELECT g.GYMID, g.GYMNAME, g.STATUS
                FROM GYM g
                WHERE g.STATUS = 'Pending';";

                case 19:
                    return @"
                SELECT m.ID, m.FirstName, m.LastName, m.Email
                FROM Member m
                WHERE m.Gender = 'Male';";

                case 20:
                    return @"
                SELECT MAX(m.DOB) AS Youngest, MIN(m.DOB) AS Oldest
                FROM Member m;";
                
                case 21:
                    return "SELECT ID, FIRSTNAME, LASTNAME, DOB, EMAIL, GENDER, MEMBERSHIPTYPE, REGISTRATIONDATE, Status FROM MEMBER;";
                case 22:
                    return "SELECT ID, FIRSTNAME, LASTNAME, DOB, EMAIL, GENDER, Qualification, Experience, Status, Speciality FROM Trainer;";
                case 23:
                    return "SELECT OWNER_ID, FIRSTNAME, LASTNAME, EMAIL, DOB, GENDER FROM GYMOWNER;";
                case 24:
                    return "SELECT ADMIN_ID, USERNAME, EMAIL, PASSWORD FROM ADMIN;";
                case 25:
                    return "SELECT GYMID, GYMNAME, OWNERID, LOCATION_ID, STATUS FROM GYM;";

                default:
                    return "";  // Default case to handle unexpected report IDs
            }
        }

    }
}
