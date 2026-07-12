using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace GUIApp
{
    public partial class messageBoxUnclosable : Form
    {

        public Form1 mainWindow;
        int currentStep = 0;
        string IP;
        string PORT;
        string PASSWORD;

        public messageBoxUnclosable(Form1 mW, string IP, string PORT, string PASSWORD)
        {
            InitializeComponent();
            this.mainWindow = mW;
            this.IP = IP;
            this.PORT = PORT;
            this.PASSWORD = PASSWORD;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(currentStep == 0)
            {
                if(!mainWindow.namedPipe.sendInstruction("Check hook"))
                {

                    if (mainWindow.hookToCemu() != 4)
                    {
                        if (hookingLabel.Text == "   Hooking to CEMU...")
                        {
                            hookingLabel.Text = "Hooking to CEMU";
                        }
                        else
                        {
                            //hookingLabel.Text = " " + hookingLabel.Text + ".";
                            hookingLabel.Text = "Hook problem.";
                        }
                    }
                    else
                    {
                        currentStep++;
                    }
                }else
                {
                    currentStep++;
                }

            }else if(currentStep == 1)
            {
                if(!mainWindow.namedPipe.isConnected)
                {
                    if(!mainWindow.namedPipe.isConnecting)
                    {
                        Thread thread1 = new Thread(mainWindow.namedPipe.WaitForConnection);
                        thread1.Start();
                    }

                        if (hookingLabel.Text == "   Hooking to CEMU...")
                        {
                            hookingLabel.Text = "Hooking to CEMU";
                        }
                        else
                        {
                        //hookingLabel.Text = " " + hookingLabel.Text + ".";
                        hookingLabel.Text = "Connection problem";
                        }
                }
                else
                {
                    hookingLabel.Text = "Hooked to CEMU.";
                    currentStep++;
                    if(PASSWORD == "")
                    {
                        PASSWORD = "NoPassword";
                    }
                    mainWindow.namedPipe.sendInstruction("!connect;" + IP + ";" + PORT + ";" + PASSWORD + ";" + Properties.Settings.Default.playerName + ";");
                    connectingLabel.Text = "Connecting to server...";
                    this.Cursor = Cursors.AppStarting;

                }

            }
            else if(currentStep == 2)
            {

                if (mainWindow.namedPipe.receiveResponse().Contains("Succeeded"))
                {
                    this.Cursor = Cursors.Default;
                    timer1.Enabled = false;
                    connectingLabel.Text = "Connected to server.";
                    this.DialogResult = DialogResult.OK;
                }
            }

        }

        private void messageBoxUnclosable_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}
