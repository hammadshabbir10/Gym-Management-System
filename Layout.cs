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
    public partial class Layout : Form
    {
        public Layout()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Admin_Login admin_Login = new Admin_Login();
            admin_Login.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Owner_Login owner_Login = new Owner_Login();    
            owner_Login.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Trainer_Login trainer_Login = new Trainer_Login();
            trainer_Login.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Member_Login member_Login = new Member_Login();
            member_Login.ShowDialog();
        }

        private void Layout_Load(object sender, EventArgs e)
        {

        }
    }
}
