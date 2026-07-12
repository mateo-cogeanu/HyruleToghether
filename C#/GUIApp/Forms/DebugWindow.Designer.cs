namespace GUIApp.Forms
{
    partial class DebugWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Player 1");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Player 2");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Player 3");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Player 4");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Player data", new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode14,
            treeNode15,
            treeNode16});
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Enemy data");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Quest data");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Player 1");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Player 2");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Player 3");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Player 4");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Extra data", new System.Windows.Forms.TreeNode[] {
            treeNode20,
            treeNode21,
            treeNode22,
            treeNode23});
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Update = new System.Windows.Forms.Button();
            this.SRTextbox = new System.Windows.Forms.TextBox();
            this.FPSTextbox = new System.Windows.Forms.TextBox();
            this.SMTextbox = new System.Windows.Forms.TextBox();
            this.islocalCB = new System.Windows.Forms.CheckBox();
            this.isSpawnActorsCB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.treeView1.Name = "treeView1";
            treeNode13.Name = "player1";
            treeNode13.Text = "Player 1";
            treeNode14.Name = "player2";
            treeNode14.Text = "Player 2";
            treeNode15.Name = "player3";
            treeNode15.Text = "Player 3";
            treeNode16.Name = "player4";
            treeNode16.Text = "Player 4";
            treeNode17.Name = "playerData";
            treeNode17.Text = "Player data";
            treeNode18.Name = "enemyData";
            treeNode18.Text = "Enemy data";
            treeNode19.Name = "questData";
            treeNode19.Text = "Quest data";
            treeNode20.Name = "player1";
            treeNode20.Text = "Player 1";
            treeNode21.Name = "player2";
            treeNode21.Text = "Player 2";
            treeNode22.Name = "player3";
            treeNode22.Text = "Player 3";
            treeNode23.Name = "player4";
            treeNode23.Text = "Player 4";
            treeNode24.Name = "extraData";
            treeNode24.Text = "Extra data";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode24});
            this.treeView1.Size = new System.Drawing.Size(583, 708);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 729);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "Serialization rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(219, 729);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target FPS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(383, 729);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "Sleep Multiplier";
            // 
            // Update
            // 
            this.Update.Font = new System.Drawing.Font("Curlz MT", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Update.Location = new System.Drawing.Point(405, 811);
            this.Update.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(123, 47);
            this.Update.TabIndex = 1;
            this.Update.Text = "Update";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // SRTextbox
            // 
            this.SRTextbox.BackColor = System.Drawing.Color.RosyBrown;
            this.SRTextbox.Font = new System.Drawing.Font("Century Schoolbook", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SRTextbox.Location = new System.Drawing.Point(12, 761);
            this.SRTextbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SRTextbox.Name = "SRTextbox";
            this.SRTextbox.Size = new System.Drawing.Size(187, 35);
            this.SRTextbox.TabIndex = 2;
            this.SRTextbox.Text = "60";
            this.SRTextbox.TextChanged += new System.EventHandler(this.SRTextbox_TextChanged);
            // 
            // FPSTextbox
            // 
            this.FPSTextbox.BackColor = System.Drawing.SystemColors.Info;
            this.FPSTextbox.Font = new System.Drawing.Font("Century Schoolbook", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FPSTextbox.Location = new System.Drawing.Point(224, 761);
            this.FPSTextbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FPSTextbox.Name = "FPSTextbox";
            this.FPSTextbox.Size = new System.Drawing.Size(132, 35);
            this.FPSTextbox.TabIndex = 6;
            this.FPSTextbox.Text = "60";
            // 
            // SMTextbox
            // 
            this.SMTextbox.BackColor = System.Drawing.SystemColors.Highlight;
            this.SMTextbox.Font = new System.Drawing.Font("Century Schoolbook", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SMTextbox.Location = new System.Drawing.Point(388, 761);
            this.SMTextbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SMTextbox.Name = "SMTextbox";
            this.SMTextbox.Size = new System.Drawing.Size(177, 35);
            this.SMTextbox.TabIndex = 7;
            this.SMTextbox.Text = "1";
            // 
            // islocalCB
            // 
            this.islocalCB.AutoSize = true;
            this.islocalCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 4.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.islocalCB.Location = new System.Drawing.Point(36, 826);
            this.islocalCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.islocalCB.Name = "islocalCB";
            this.islocalCB.Size = new System.Drawing.Size(65, 17);
            this.islocalCB.TabIndex = 8;
            this.islocalCB.Text = "Is local test";
            this.islocalCB.UseVisualStyleBackColor = true;
            // 
            // isSpawnActorsCB
            // 
            this.isSpawnActorsCB.AutoSize = true;
            this.isSpawnActorsCB.Checked = true;
            this.isSpawnActorsCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isSpawnActorsCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isSpawnActorsCB.Location = new System.Drawing.Point(107, 811);
            this.isSpawnActorsCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.isSpawnActorsCB.Name = "isSpawnActorsCB";
            this.isSpawnActorsCB.Size = new System.Drawing.Size(261, 46);
            this.isSpawnActorsCB.TabIndex = 9;
            this.isSpawnActorsCB.Text = "spawn actors";
            this.isSpawnActorsCB.UseVisualStyleBackColor = true;
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(583, 870);
            this.Controls.Add(this.isSpawnActorsCB);
            this.Controls.Add(this.islocalCB);
            this.Controls.Add(this.SMTextbox);
            this.Controls.Add(this.FPSTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SRTextbox);
            this.Controls.Add(this.Update);
            this.Controls.Add(this.treeView1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DebugWindow";
            this.Text = "DebugWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Update;
        private System.Windows.Forms.TextBox SRTextbox;
        private System.Windows.Forms.TextBox FPSTextbox;
        private System.Windows.Forms.TextBox SMTextbox;
        private System.Windows.Forms.CheckBox islocalCB;
        private System.Windows.Forms.CheckBox isSpawnActorsCB;
    }
}