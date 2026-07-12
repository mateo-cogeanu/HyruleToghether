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
    public partial class HostServer : Form
    {

        string[] placeholderTexts = { Properties.Settings.Default.playerName + "'s server", "Breath of the Wild Multiplayer server", "", "localhost", "5050" };
        string[] lastTexts = { "lastServerName", "lastDescription", "lastPassword", "lastIP", "lastPort" };

        public bool[] lastSettings = { Properties.Settings.Default.lastEnemySync, Properties.Settings.Default.lastGlyphSync, Properties.Settings.Default.lastQuestSync };

        public string[] results = { "", "", "", "", ""};
        Form1 mainWindow;

        public HostServer(Form1 mW)
        {
            InitializeComponent();
            mainWindow = mW;
        }
        
        private void HostServer_Load(object sender, EventArgs e)
        {
            foreach(Control ctrl in this.Controls)
            {
                if(ctrl.GetType() == typeof(TextBox))
                {

                    if(Properties.Settings.Default[lastTexts[Int32.Parse(ctrl.Tag.ToString())]].ToString() != "")
                    {
                        ctrl.Text = Properties.Settings.Default[lastTexts[Int32.Parse(ctrl.Tag.ToString())]].ToString();
                        ctrl.ForeColor = Color.Black;
                    }
                    else
                    {
                        ctrl.Text = placeholderTexts[Int32.Parse(ctrl.Tag.ToString())];
                        ctrl.ForeColor = Color.Gray;
                    }

                    ctrl.GotFocus += new EventHandler(TxtGotFocus);
                    ctrl.LostFocus += new EventHandler(TxtLostFocus);
                }else if(ctrl.GetType() == typeof(Panel))
                {
                    foreach(Control ctrlChild in ctrl.Controls)
                    {
                        if(ctrlChild.GetType() == typeof(TextBox))
                        {
                            if (Properties.Settings.Default[lastTexts[Int32.Parse(ctrlChild.Tag.ToString())]].ToString() != "")
                            {
                                ctrlChild.Text = Properties.Settings.Default[lastTexts[Int32.Parse(ctrlChild.Tag.ToString())]].ToString();
                                ctrlChild.ForeColor = Color.Black;
                            }
                            else
                            {
                                ctrlChild.Text = placeholderTexts[Int32.Parse(ctrlChild.Tag.ToString())];
                                ctrlChild.ForeColor = Color.Gray;
                            }

                            ctrlChild.GotFocus += new EventHandler(TxtGotFocus);
                            ctrlChild.LostFocus += new EventHandler(TxtLostFocus);
                        }
                    }
                }
            }

            updatePB(enemySyncPB, lastSettings[0]);
            updatePB(glyphSyncPB, lastSettings[1]);
            updatePB(questSyncPB, lastSettings[2]);
            
        }

        public void TxtGotFocus(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Text == placeholderTexts[Int32.Parse(txt.Tag.ToString())])
            {
                txt.Text = "";
                txt.ForeColor = Color.Black;
            }
        }

        public void TxtLostFocus(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if(txt.Text == "")
            {
                txt.Text = placeholderTexts[Int32.Parse(txt.Tag.ToString())];
                txt.ForeColor = Color.Gray;
            }
        }

        private void seePasswordButton_Click(object sender, EventArgs e)
        {
            passwordTxt.UseSystemPasswordChar = !passwordTxt.UseSystemPasswordChar;
        }

        private void showIPButton_Click(object sender, EventArgs e)
        {
            IPTxt.UseSystemPasswordChar = !IPTxt.UseSystemPasswordChar;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hostButton_Click(object sender, EventArgs e)
        {

            foreach(Control ctrl in this.Controls)
            {
                if(ctrl.GetType() == typeof(Label))
                {
                    ctrl.ForeColor = Color.Black;
                }
                else if(ctrl.GetType() == typeof(Panel))
                {
                    foreach(Control ctrlChild in ctrl.Controls)
                    {
                        if(ctrlChild.GetType() == typeof(Label))
                        {
                            ctrlChild.ForeColor = Color.Black;
                        }
                    }
                }
            }

            serverSettingsButton.ForeColor = Color.Black;

            if (!int.TryParse(portTxt.Text, out int port))
            {
                MessageBox.Show("The port must be an integer.");
                portLabel.ForeColor = Color.IndianRed;
                serverSettingsButton.ForeColor = Color.IndianRed;
                return;
            }

            if (!mainWindow.Server.serverStart(serverNameTxt.Text, IPTxt.Text, port, passwordTxt.Text, lastSettings, Properties.Settings.Default.EnemiesSynced))
            {
                MessageBox.Show("Server start failed, check your IP and PORT");
                ipLabel.ForeColor = Color.IndianRed;
                portLabel.ForeColor = Color.IndianRed;
                serverSettingsButton.ForeColor = Color.IndianRed;
                return;
            }

            int textboxID;

            foreach(Control ctrl in this.Controls)
            {
                if(ctrl.GetType() == typeof(TextBox))
                {

                    textboxID = Int32.Parse(ctrl.Tag.ToString());

                    if (ctrl.Text != "" && ctrl.Text != placeholderTexts[textboxID])
                    {
                        Properties.Settings.Default[lastTexts[textboxID]] = ctrl.Text;
                        results[textboxID] = ctrl.Text;
                    }
                    else
                    {
                        Properties.Settings.Default[lastTexts[textboxID]] = "";
                        results[textboxID] = placeholderTexts[textboxID];
                    }
                }else if(ctrl.GetType() == typeof(Panel))
                {
                    foreach(Control ctrlChild in ctrl.Controls)
                    {
                        if(ctrlChild.GetType() == typeof(TextBox))
                        {

                            textboxID = Int32.Parse(ctrlChild.Tag.ToString());

                            if (ctrlChild.Text != "" && ctrlChild.Text != placeholderTexts[textboxID])
                            {
                                Properties.Settings.Default[lastTexts[textboxID]] = ctrlChild.Text;
                                results[textboxID] = ctrlChild.Text;
                            }
                            else
                            {
                                Properties.Settings.Default[lastTexts[textboxID]] = "";
                                results[textboxID] = placeholderTexts[textboxID];
                            }
                        }
                    }
                }
            }

            Properties.Settings.Default.lastEnemySync = lastSettings[0];
            Properties.Settings.Default.lastGlyphSync = lastSettings[1];
            Properties.Settings.Default.lastQuestSync = lastSettings[2];

            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;

            this.Close();

        }

        private void serverSettingsButton_Click(object sender, EventArgs e)
        {
            if (serverConfigPanel.Height == 75)
            {
                serverConfigPanel.Height = 0;
                serverSettingsButton.Text = "Server settings ˅";
            }
            else
            {
                serverConfigPanel.Height = 75;
                serverSettingsButton.Text = "Server settings ˄";
            }
        }

        private void enemySyncPB_Click(object sender, EventArgs e)
        {

            lastSettings[0] = !lastSettings[0];

            updatePB(enemySyncPB, lastSettings[0]);

        }

        private void glyphSyncPB_Click(object sender, EventArgs e)
        {

            lastSettings[1] = !lastSettings[1];

            updatePB(glyphSyncPB, lastSettings[1]);

        }

        private void updatePB(PictureBox pbToChange, bool state)
        {

            if(state)
            {
                pbToChange.Image = Properties.Resources.check_placeholder;
            }
            else
            {
                pbToChange.Image = Properties.Resources.x_placeholder;
            }

        }

        private void questSyncPB_Click(object sender, EventArgs e)
        {

            lastSettings[2] = !lastSettings[2];

            updatePB(questSyncPB, lastSettings[2]);

        }
    }
}
