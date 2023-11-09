using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace GramPanchayat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // LOGIN BUTTON
        private void btn_login_Click(object sender, EventArgs e)
        {
            string username = txt_username.Text;
            string password = txt_password.Text;

            if (username == "system" && password == "system")
            {

                // MessageBox.Show("Correct Id And Password.");

                this.Hide();

                Dashboard ds = new Dashboard();
                ds.Show();

            }

            else
            {
                MessageBox.Show("Incorrect Id And Password. \nPlease Try Again.");
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void check_showPassword_CheckedChanged(object sender, EventArgs e)
        {
            txt_password.UseSystemPasswordChar = check_showPassword.Checked;
        }

    }
}
