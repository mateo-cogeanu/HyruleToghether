namespace GUIApp
{
    partial class messageBoxUnclosable
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
            this.hookingLabel = new System.Windows.Forms.Label();
            this.connectingLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hookingLabel
            // 
            this.hookingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hookingLabel.Location = new System.Drawing.Point(1, 14);
            this.hookingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.hookingLabel.Name = "hookingLabel";
            this.hookingLabel.Size = new System.Drawing.Size(301, 30);
            this.hookingLabel.TabIndex = 0;
            this.hookingLabel.Text = "   Hooking to CEMU...";
            this.hookingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // connectingLabel
            // 
            this.connectingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectingLabel.Location = new System.Drawing.Point(1, 59);
            this.connectingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.connectingLabel.Name = "connectingLabel";
            this.connectingLabel.Size = new System.Drawing.Size(301, 30);
            this.connectingLabel.TabIndex = 1;
            this.connectingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(85, 105);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(136, 43);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cancelButton);
            this.panel1.Controls.Add(this.connectingLabel);
            this.panel1.Controls.Add(this.hookingLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(307, 172);
            this.panel1.TabIndex = 3;
            // 
            // messageBoxUnclosable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(307, 172);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "messageBoxUnclosable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "messageBoxUnclosable";
            this.Load += new System.EventHandler(this.messageBoxUnclosable_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label hookingLabel;
        private System.Windows.Forms.Label connectingLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
    }
}