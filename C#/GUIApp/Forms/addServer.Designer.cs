namespace GUIApp
{
    partial class addServer
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
            this.label1 = new System.Windows.Forms.Label();
            this.ipLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.IPTxt = new System.Windows.Forms.TextBox();
            this.portTxt = new System.Windows.Forms.TextBox();
            this.showIPButton = new System.Windows.Forms.PictureBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.passwordTxt = new System.Windows.Forms.TextBox();
            this.showPassword = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.showIPButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.showPassword)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(159, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add server";
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipLabel.Location = new System.Drawing.Point(13, 50);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(36, 25);
            this.ipLabel.TabIndex = 2;
            this.ipLabel.Text = "IP:";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portLabel.Location = new System.Drawing.Point(327, 50);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(73, 25);
            this.portLabel.TabIndex = 3;
            this.portLabel.Text = "PORT:";
            // 
            // IPTxt
            // 
            this.IPTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPTxt.Location = new System.Drawing.Point(18, 78);
            this.IPTxt.Name = "IPTxt";
            this.IPTxt.Size = new System.Drawing.Size(271, 30);
            this.IPTxt.TabIndex = 3;
            this.IPTxt.Tag = "3";
            this.IPTxt.UseSystemPasswordChar = true;
            // 
            // portTxt
            // 
            this.portTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portTxt.Location = new System.Drawing.Point(332, 78);
            this.portTxt.Name = "portTxt";
            this.portTxt.Size = new System.Drawing.Size(100, 30);
            this.portTxt.TabIndex = 4;
            this.portTxt.Tag = "4";
            // 
            // showIPButton
            // 
            this.showIPButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.showIPButton.Location = new System.Drawing.Point(296, 78);
            this.showIPButton.Name = "showIPButton";
            this.showIPButton.Size = new System.Drawing.Size(30, 30);
            this.showIPButton.TabIndex = 11;
            this.showIPButton.TabStop = false;
            this.showIPButton.Click += new System.EventHandler(this.showIPButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(232, 199);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(117, 47);
            this.cancelButton.TabIndex = 22;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // addButton
            // 
            this.addButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addButton.Location = new System.Drawing.Point(95, 199);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(117, 47);
            this.addButton.TabIndex = 21;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 25);
            this.label2.TabIndex = 23;
            this.label2.Text = "Password:";
            // 
            // passwordTxt
            // 
            this.passwordTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTxt.Location = new System.Drawing.Point(18, 145);
            this.passwordTxt.Name = "passwordTxt";
            this.passwordTxt.Size = new System.Drawing.Size(378, 30);
            this.passwordTxt.TabIndex = 24;
            this.passwordTxt.Tag = "3";
            this.passwordTxt.UseSystemPasswordChar = true;
            // 
            // showPassword
            // 
            this.showPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.showPassword.Location = new System.Drawing.Point(402, 145);
            this.showPassword.Name = "showPassword";
            this.showPassword.Size = new System.Drawing.Size(30, 30);
            this.showPassword.TabIndex = 25;
            this.showPassword.TabStop = false;
            this.showPassword.Click += new System.EventHandler(this.showPassword_Click);
            // 
            // addServer
            // 
            this.AcceptButton = this.addButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(445, 258);
            this.Controls.Add(this.showPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.passwordTxt);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.IPTxt);
            this.Controls.Add(this.portTxt);
            this.Controls.Add(this.showIPButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "addServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.showIPButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.showPassword)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox IPTxt;
        private System.Windows.Forms.TextBox portTxt;
        private System.Windows.Forms.PictureBox showIPButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox passwordTxt;
        private System.Windows.Forms.PictureBox showPassword;
    }
}