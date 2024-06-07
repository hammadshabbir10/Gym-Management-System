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
    public partial class Admin_Interface : Form
    {
        public Admin_Interface()
        {
            InitializeComponent();
        }

        private void mEMBERSHIPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin_MemRevoke admin_MemRevoke = new Admin_MemRevoke();
            admin_MemRevoke.ShowDialog();
        }

        private void tRAINERSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin_TraineRemove admin_TraineRemove = new Admin_TraineRemove();
            admin_TraineRemove.ShowDialog();
        }

        private void mEMBERSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin_MemRemove admin_MemRemove = new Admin_MemRemove();
            admin_MemRemove.ShowDialog();
        }

        private void pERFORMANCEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GymPerformance form = new GymPerformance();
            form.ShowDialog();  
        }

        private void rEQUESTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin_MemRequests admin_MemRequests = new Admin_MemRequests();
            admin_MemRequests.ShowDialog();
        }

        private void lOGOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Admin_Interface_Load(object sender, EventArgs e)
        {

        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueryReports form = new QueryReports();
            form.ShowDialog();
        }
    }
}
