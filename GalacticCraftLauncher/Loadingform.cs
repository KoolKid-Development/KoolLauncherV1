using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GalacticCraftLauncher
{
    public partial class Loadingform : Form
    {
        public Loadingform()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel2.Width += 3;

            if (panel2.Width >= 900)
            {
                timer1.Stop();
                Form1 login = new Form1();
                login.Show();
                this.Hide();
            }
        }
    }
}
