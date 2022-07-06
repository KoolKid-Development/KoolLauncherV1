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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace KoolLauncher
{
    public partial class Form1 : Form
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        public Form1()
        {
            InitializeComponent();
            con = new MySqlConnection("Server=0.0.0.0;Database=yourdatabasename;user=yourphpmyadminusername;Pwd=yourpasswordfromphpmyadmin;SslMode=none");
        }

        public string usernames;
        MLogin login = new MLogin();
        public static string accountname;
        private void Form1_Load(object sender, EventArgs e)
        {
            admpanel.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UpdateSession(MSession session)
        {
            accountname = txtusername.Text;
            var mainForm = new MainForm(session);
            mainForm.FormClosed += (s, e) => this.Close();
            mainForm.Show();
            this.Hide();          
        }

        private void bunifuLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (txtusername.Text == "")
            {
                MessageBox.Show("You Need To Enter Your Username Here!");
            }
            else
            {
                if (txtpassword.Text == "")
                {
                    MessageBox.Show("You need to enter a password!");
                }
                else
                {
                    string user = txtusername.Text;
                    string pass = txtpassword.Text;
                    cmd = new MySqlCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM users where username='" + txtusername.Text + "' AND password='" + txtpassword.Text + "'";
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        if (txtusername.Text == "")
                        {
                            MessageBox.Show("You Need To Set A Username", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (txtusername.Text == "Fuck")
                        {
                            MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (txtusername.Text == "Cum")
                        {
                            MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (txtusername.Text == "Dick")
                        {
                            MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (txtusername.Text == "fuck")
                        {
                            MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (txtusername.Text == "cum")
                        {
                            MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (txtusername.Text == "dick")
                        {
                            MessageBox.Show("This Username Is Banned", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else
                        {
                            UpdateSession(MSession.GetOfflineSession(txtusername.Text));

                        }

                    }
                    else
                    {
                        MessageBox.Show("This account dose not exists!", "Login Faild");
                    }
                    con.Close();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(myweblauncherurl.Text);
        }
    }
}
