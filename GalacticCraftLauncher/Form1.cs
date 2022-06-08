using CmlLib.Core.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GalacticCraftLauncher
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        public string usernames;
        MLogin login = new MLogin();
        public static string accountname;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UpdateSession(MSession session)
        {
            accountname = txtUsername.Text;
            var mainForm = new MainForm(session);
            mainForm.FormClosed += (s, e) => this.Close();
            mainForm.Show();
            this.Hide();          
        }

        private void btnOfflineLogin_Click(object sender, EventArgs e)
        {
           
        }

        private void bunifuLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                MessageBox.Show("You Need To Set A Username", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUsername.Text == "Fuck")
            {
                MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUsername.Text == "Cum")
            {
                MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUsername.Text == "Dick")
            {
                MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUsername.Text == "fuck")
            {
                MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUsername.Text == "cum")
            {
                MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUsername.Text == "dick")
            {
                MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                UpdateSession(MSession.GetOfflineSession(txtUsername.Text));
                
            }
            
        }
    }
}
