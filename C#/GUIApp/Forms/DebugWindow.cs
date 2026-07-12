using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIApp.Forms
{
    public partial class DebugWindow : Form
    {

        Form1 mainWindow;

        public DebugWindow(Form1 mW)
        {
            InitializeComponent();
            //timer1.Enabled = true;
            this.mainWindow = mW;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            UpdateData();

        }

        private void UpdateData()
        {
            List<Dictionary<string, object>> playerData = new List<Dictionary<string, object>>(mainWindow.Server.serverData);
            Dictionary<string, int> enemyData = new Dictionary<string, int>(mainWindow.Server.serverEnemyData);
            List<string> questData = new List<string>(mainWindow.Server.serverQuestData);
            List<Dictionary<string, object>> extraData = new List<Dictionary<string, object>>(mainWindow.Server.extraData);

            for (int i = 1; i < 5; i++)
            {

                foreach (KeyValuePair<string, object> kvp in playerData[i - 1])
                {

                    if (!treeView1.Nodes["playerData"].Nodes["player" + i.ToString()].Nodes.ContainsKey(kvp.Key))
                    {

                        if (kvp.Value.GetType().ToString() != "System.Collections.Generic.List`1[System.Object]")
                        {

                            treeView1.Nodes["playerData"].Nodes["player" + i.ToString()].Nodes.Add(kvp.Key, kvp.Key + ": " + kvp.Value.ToString());

                        }
                        else
                        {

                            treeView1.Nodes["playerData"].Nodes["player" + i.ToString()].Nodes.Add(kvp.Key, kvp.Key);

                            List<object> value = (List<object>)kvp.Value;
                            for (int j = 0; j < value.Count(); j++)
                            {

                                treeView1.Nodes["playerData"].Nodes["player" + i.ToString()].Nodes[kvp.Key].Nodes.Add(j.ToString(), value[j].ToString());

                            }

                        }

                    }

                    if (kvp.Value.GetType().ToString() != "System.Collections.Generic.List`1[System.Object]")
                    {

                        treeView1.Nodes["playerData"].Nodes["player" + i.ToString()].Nodes[kvp.Key].Text = kvp.Key + ": " + kvp.Value.ToString();

                    }
                    else
                    {

                        List<object> value = (List<object>)kvp.Value;
                        for (int j = 0; j < value.Count(); j++)
                        {

                            treeView1.Nodes["playerData"].Nodes["player" + i.ToString()].Nodes[kvp.Key].Nodes[j.ToString()].Text = value[j].ToString();

                        }

                    }

                }

                foreach (KeyValuePair<string, object> kvp in extraData[i - 1])
                {

                    if (!treeView1.Nodes["extraData"].Nodes["player" + i.ToString()].Nodes.ContainsKey(kvp.Key))
                    {

                        treeView1.Nodes["extraData"].Nodes["player" + i.ToString()].Nodes.Add(kvp.Key, kvp.Key + ": " + kvp.Value.ToString());

                    }
                    else
                    {

                        treeView1.Nodes["extraData"].Nodes["player" + i.ToString()].Nodes[kvp.Key].Text = kvp.Key + ": " + kvp.Value.ToString();

                    }

                }

            }

            foreach (KeyValuePair<string, int> kvp in enemyData)
            {

                if (!treeView1.Nodes["enemyData"].Nodes.ContainsKey(kvp.Key))
                {

                    treeView1.Nodes["enemyData"].Nodes.Add(kvp.Key, kvp.Key + ": " + kvp.Value.ToString());

                }
                else
                {

                    treeView1.Nodes["enemyData"].Nodes[kvp.Key].Text = kvp.Key + ": " + kvp.Value.ToString();

                }

            }

            for (int i = 0; i < questData.Count; i++)
            {

                if (!treeView1.Nodes["questData"].Nodes.ContainsKey(questData[i]))
                {

                    treeView1.Nodes["questData"].Nodes.Add(questData[i], questData[i]);

                }

            }
        }

        private void Update_Click(object sender, EventArgs e)
        {

            mainWindow.Server.SerializationRate = Int32.Parse(SRTextbox.Text);
            mainWindow.Server.TargetFPS = Int32.Parse(FPSTextbox.Text);
            mainWindow.Server.SleepMultiplier = Int32.Parse(SMTextbox.Text);
            mainWindow.Server.isLocalTest = islocalCB.Checked ? 1 : 0;
            mainWindow.Server.ischaracterSpawn = isSpawnActorsCB.Checked ? 1 : 0;
            UpdateData();

        }

        private void SRTextbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}