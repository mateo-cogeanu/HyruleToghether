using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIApp
{
    public partial class addServer : Form
    {

        public string[] results;

        public addServer()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {

            foreach(Control ctrl in this.Controls)
            {
                if(ctrl.GetType() == typeof(Label))
                {
                    ctrl.ForeColor = Color.Black;
                }
            }

            if(!checkButtons())
            {
                return;
            }

            this.results = new string[] { IPTxt.Text, portTxt.Text, passwordTxt.Text };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool checkButtons()
        {
            if(IPTxt.Text == "")
            {
                MessageBox.Show("The IP box cannot be empty.");
                ipLabel.ForeColor = Color.IndianRed;
                return false;
            }else if(!int.TryParse(portTxt.Text, out int port))
            {
                portLabel.ForeColor = Color.IndianRed;
                MessageBox.Show("The port box cannot be empty and must be an integer.");
                return false;
            }

            return true;
        }

        private void showIPButton_Click(object sender, EventArgs e)
        {

            IPTxt.UseSystemPasswordChar = !IPTxt.UseSystemPasswordChar;

        }

        private void showPassword_Click(object sender, EventArgs e)
        {
            passwordTxt.UseSystemPasswordChar = !passwordTxt.UseSystemPasswordChar;
        }
    }
}
