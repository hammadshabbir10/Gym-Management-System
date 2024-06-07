using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Member_Forms
{
    public partial class MemberLayout : Form
    {
        public MemberLayout()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(330, 80);
            pictureBox.Size = new Size(450, 350);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\layout.jpg");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox); 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Workout Plan");
            MemberWorkoutForm form = new MemberWorkoutForm();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Diet Plan");
            MemberDietForm form = new MemberDietForm();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Exercise Plan");
            MemberExerciseForm form = new MemberExerciseForm();
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Session Meeting");
            MemberSessionForm form = new MemberSessionForm();
            form.ShowDialog();
        }

        private void MemberLayout_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            MemberFeedBack form = new MemberFeedBack();
            form.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MemberGetTrained form = new MemberGetTrained();
            form.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MemberAddGym form = new MemberAddGym();
            form.ShowDialog();
        }
    }
}
