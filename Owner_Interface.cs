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
    public partial class Owner_Interface : Form
    {
        public Owner_Interface()
        {
            InitializeComponent();
        }

        private void mEMBERSHIPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Owner_GymRegistration owner_GymRegistration = new Owner_GymRegistration();
            owner_GymRegistration.ShowDialog();
        }

        private void tRAINERSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OwnerAddTrainer form = new OwnerAddTrainer();
            form.ShowDialog();
        }

        private void lOGOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Owner_Interface_Load(object sender, EventArgs e)
        {

        }

        private void mEMBERSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberReports form = new MemberReports();
            form.ShowDialog();
        }

        private void pERFORMANCEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrainerReports form = new TrainerReports();
            form.ShowDialog();
        }

        private void addMemberToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OwnerAddMember form = new OwnerAddMember();   
            form.ShowDialog();  
        }
    }
}
