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
    public partial class serverInterface : Form
    {

        public Form1 mainWindow;
        public string serverName = "Placeholder text";
        public string serverDescription = "Placeholder text";

        public serverInterface(Form1 mW)
        {
            InitializeComponent();
            this.mainWindow = mW;
        }

        private void serverInterface_Load(object sender, EventArgs e)
        {
            serverNameLabel.Text = serverName;
            serverDescriptionLabel.Text = serverDescription;
            AppendLog("Welcome to Breath of the Wild Multiplayer | Mod created by AlexMangue and Sweet", Color.FromArgb(93, 80, 104), "First");
            if(mainWindow.isHost)
            {
                mainWindow.serverInterfaceForm.AppendLog("[SERVER] Server listening on " + mainWindow.Server.NetworkInterfaceName + ".", Color.Black);
            }

            mainWindow.namedPipe.sendInstruction("!startServerLoop");

            mainWindow.debugButton.Visible = mainWindow.isHost && mainWindow.isDevBuild;
        }

        public void AppendLog(string Text, Color color, string Type = "Normal")
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AppendLog(Text, color, Type)));
                return;
            }

            if (Type == "First")
            {
                serverLog.SelectionStart = serverLog.TextLength;
                serverLog.SelectionLength = 0;

                serverLog.SelectionColor = color;

                serverLog.AppendText(Text + "\n\n");

                serverLog.SelectionColor = serverLog.ForeColor;

            }else if(Type== "Normal")
            {
                serverLog.SelectionStart = serverLog.TextLength;
                serverLog.SelectionLength = 0;

                serverLog.SelectionColor = color;

                serverLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + " | " + Text + "\n");

                serverLog.SelectionColor = serverLog.ForeColor;
            }

        }

        private void serverLog_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
