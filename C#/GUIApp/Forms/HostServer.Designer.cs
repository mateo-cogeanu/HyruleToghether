namespace GUIApp
{
    partial class HostServer
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ipLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.IPTxt = new System.Windows.Forms.TextBox();
            this.portTxt = new System.Windows.Forms.TextBox();
            this.serverNameTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.descriptionTxt = new System.Windows.Forms.TextBox();
            this.passwordTxt = new System.Windows.Forms.TextBox();
            this.seePasswordButton = new System.Windows.Forms.PictureBox();
            this.showIPButton = new System.Windows.Forms.PictureBox();
            this.enemySyncPB = new System.Windows.Forms.PictureBox();
            this.glyphSyncPB = new System.Windows.Forms.PictureBox();
            this.hostButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.serverConfigPanel = new System.Windows.Forms.Panel();
            this.serverSettingsButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.questSyncPB = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.seePasswordButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.showIPButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemySyncPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glyphSyncPB)).BeginInit();
            this.serverConfigPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.questSyncPB)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(208, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "Server name:";
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipLabel.Location = new System.Drawing.Point(4, 11);
            this.ipLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(43, 29);
            this.ipLabel.TabIndex = 2;
            this.ipLabel.Text = "IP:";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portLabel.Location = new System.Drawing.Point(423, 11);
            this.portLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(89, 29);
            this.portLabel.TabIndex = 3;
            this.portLabel.Text = "PORT:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(20, 433);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 29);
            this.label5.TabIndex = 4;
            this.label5.Text = "Settings:";
            // 
            // IPTxt
            // 
            this.IPTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPTxt.Location = new System.Drawing.Point(11, 46);
            this.IPTxt.Margin = new System.Windows.Forms.Padding(4);
            this.IPTxt.Name = "IPTxt";
            this.IPTxt.Size = new System.Drawing.Size(360, 36);
            this.IPTxt.TabIndex = 3;
            this.IPTxt.Tag = "3";
            this.toolTip1.SetToolTip(this.IPTxt, "Sets the IP the server will be opened on.\r\nIf set to localhost it will loop throu" +
        "gh the IPv4 IPs on your computer and open the server on the first one that succe" +
        "ds.\r\n");
            this.IPTxt.UseSystemPasswordChar = true;
            // 
            // portTxt
            // 
            this.portTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portTxt.Location = new System.Drawing.Point(429, 46);
            this.portTxt.Margin = new System.Windows.Forms.Padding(4);
            this.portTxt.Name = "portTxt";
            this.portTxt.Size = new System.Drawing.Size(132, 36);
            this.portTxt.TabIndex = 4;
            this.portTxt.Tag = "4";
            // 
            // serverNameTxt
            // 
            this.serverNameTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverNameTxt.Location = new System.Drawing.Point(24, 82);
            this.serverNameTxt.Margin = new System.Windows.Forms.Padding(4);
            this.serverNameTxt.Name = "serverNameTxt";
            this.serverNameTxt.Size = new System.Drawing.Size(551, 36);
            this.serverNameTxt.TabIndex = 0;
            this.serverNameTxt.Tag = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 130);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 29);
            this.label6.TabIndex = 8;
            this.label6.Text = "Description:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(17, 213);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 29);
            this.label7.TabIndex = 9;
            this.label7.Text = "Password:";
            // 
            // descriptionTxt
            // 
            this.descriptionTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionTxt.Location = new System.Drawing.Point(24, 165);
            this.descriptionTxt.Margin = new System.Windows.Forms.Padding(4);
            this.descriptionTxt.Name = "descriptionTxt";
            this.descriptionTxt.Size = new System.Drawing.Size(551, 36);
            this.descriptionTxt.TabIndex = 1;
            this.descriptionTxt.Tag = "1";
            // 
            // passwordTxt
            // 
            this.passwordTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTxt.Location = new System.Drawing.Point(24, 247);
            this.passwordTxt.Margin = new System.Windows.Forms.Padding(4);
            this.passwordTxt.Name = "passwordTxt";
            this.passwordTxt.Size = new System.Drawing.Size(503, 36);
            this.passwordTxt.TabIndex = 2;
            this.passwordTxt.Tag = "2";
            this.passwordTxt.UseSystemPasswordChar = true;
            // 
            // seePasswordButton
            // 
            this.seePasswordButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.seePasswordButton.Location = new System.Drawing.Point(536, 247);
            this.seePasswordButton.Margin = new System.Windows.Forms.Padding(4);
            this.seePasswordButton.Name = "seePasswordButton";
            this.seePasswordButton.Size = new System.Drawing.Size(39, 36);
            this.seePasswordButton.TabIndex = 10;
            this.seePasswordButton.TabStop = false;
            this.seePasswordButton.Click += new System.EventHandler(this.seePasswordButton_Click);
            // 
            // showIPButton
            // 
            this.showIPButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.showIPButton.Location = new System.Drawing.Point(381, 46);
            this.showIPButton.Margin = new System.Windows.Forms.Padding(4);
            this.showIPButton.Name = "showIPButton";
            this.showIPButton.Size = new System.Drawing.Size(39, 36);
            this.showIPButton.TabIndex = 11;
            this.showIPButton.TabStop = false;
            this.showIPButton.Click += new System.EventHandler(this.showIPButton_Click);
            // 
            // enemySyncPB
            // 
            this.enemySyncPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.enemySyncPB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.enemySyncPB.Location = new System.Drawing.Point(27, 480);
            this.enemySyncPB.Margin = new System.Windows.Forms.Padding(4);
            this.enemySyncPB.Name = "enemySyncPB";
            this.enemySyncPB.Size = new System.Drawing.Size(79, 73);
            this.enemySyncPB.TabIndex = 12;
            this.enemySyncPB.TabStop = false;
            this.enemySyncPB.Click += new System.EventHandler(this.enemySyncPB_Click);
            // 
            // glyphSyncPB
            // 
            this.glyphSyncPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.glyphSyncPB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.glyphSyncPB.Location = new System.Drawing.Point(133, 480);
            this.glyphSyncPB.Margin = new System.Windows.Forms.Padding(4);
            this.glyphSyncPB.Name = "glyphSyncPB";
            this.glyphSyncPB.Size = new System.Drawing.Size(79, 73);
            this.glyphSyncPB.TabIndex = 13;
            this.glyphSyncPB.TabStop = false;
            this.glyphSyncPB.Click += new System.EventHandler(this.glyphSyncPB_Click);
            // 
            // hostButton
            // 
            this.hostButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hostButton.Location = new System.Drawing.Point(127, 587);
            this.hostButton.Margin = new System.Windows.Forms.Padding(4);
            this.hostButton.Name = "hostButton";
            this.hostButton.Size = new System.Drawing.Size(156, 58);
            this.hostButton.TabIndex = 15;
            this.hostButton.Text = "Host";
            this.hostButton.UseVisualStyleBackColor = true;
            this.hostButton.Click += new System.EventHandler(this.hostButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(309, 587);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(156, 58);
            this.cancelButton.TabIndex = 16;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // serverConfigPanel
            // 
            this.serverConfigPanel.Controls.Add(this.ipLabel);
            this.serverConfigPanel.Controls.Add(this.portLabel);
            this.serverConfigPanel.Controls.Add(this.IPTxt);
            this.serverConfigPanel.Controls.Add(this.portTxt);
            this.serverConfigPanel.Controls.Add(this.showIPButton);
            this.serverConfigPanel.Location = new System.Drawing.Point(16, 329);
            this.serverConfigPanel.Margin = new System.Windows.Forms.Padding(4);
            this.serverConfigPanel.Name = "serverConfigPanel";
            this.serverConfigPanel.Size = new System.Drawing.Size(568, 0);
            this.serverConfigPanel.TabIndex = 17;
            // 
            // serverSettingsButton
            // 
            this.serverSettingsButton.FlatAppearance.BorderSize = 0;
            this.serverSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.serverSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverSettingsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.serverSettingsButton.Location = new System.Drawing.Point(16, 294);
            this.serverSettingsButton.Margin = new System.Windows.Forms.Padding(4);
            this.serverSettingsButton.Name = "serverSettingsButton";
            this.serverSettingsButton.Size = new System.Drawing.Size(552, 42);
            this.serverSettingsButton.TabIndex = 19;
            this.serverSettingsButton.Text = "Server settings ˅";
            this.serverSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.serverSettingsButton, "Click to change the IP and Port.\r\nDefault values can be changed on the settings p" +
        "anel.");
            this.serverSettingsButton.UseVisualStyleBackColor = true;
            this.serverSettingsButton.Click += new System.EventHandler(this.serverSettingsButton_Click);
            // 
            // questSyncPB
            // 
            this.questSyncPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.questSyncPB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.questSyncPB.Location = new System.Drawing.Point(239, 480);
            this.questSyncPB.Margin = new System.Windows.Forms.Padding(4);
            this.questSyncPB.Name = "questSyncPB";
            this.questSyncPB.Size = new System.Drawing.Size(79, 73);
            this.questSyncPB.TabIndex = 20;
            this.questSyncPB.TabStop = false;
            this.questSyncPB.Click += new System.EventHandler(this.questSyncPB_Click);
            // 
            // HostServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(593, 663);
            this.Controls.Add(this.questSyncPB);
            this.Controls.Add(this.serverSettingsButton);
            this.Controls.Add(this.serverConfigPanel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.hostButton);
            this.Controls.Add(this.glyphSyncPB);
            this.Controls.Add(this.enemySyncPB);
            this.Controls.Add(this.seePasswordButton);
            this.Controls.Add(this.passwordTxt);
            this.Controls.Add(this.descriptionTxt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.serverNameTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HostServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.HostServer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.seePasswordButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.showIPButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemySyncPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glyphSyncPB)).EndInit();
            this.serverConfigPanel.ResumeLayout(false);
            this.serverConfigPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.questSyncPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox IPTxt;
        private System.Windows.Forms.TextBox portTxt;
        private System.Windows.Forms.TextBox serverNameTxt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox descriptionTxt;
        private System.Windows.Forms.TextBox passwordTxt;
        private System.Windows.Forms.PictureBox seePasswordButton;
        private System.Windows.Forms.PictureBox showIPButton;
        private System.Windows.Forms.PictureBox enemySyncPB;
        private System.Windows.Forms.PictureBox glyphSyncPB;
        private System.Windows.Forms.Button hostButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel serverConfigPanel;
        private System.Windows.Forms.Button serverSettingsButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox questSyncPB;
    }
}