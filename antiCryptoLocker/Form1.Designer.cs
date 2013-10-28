namespace antiCryptoLocker
{
    partial class Form1
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
            this.btnLockdown = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLockdown
            // 
            this.btnLockdown.Enabled = false;
            this.btnLockdown.Location = new System.Drawing.Point(12, 12);
            this.btnLockdown.Name = "btnLockdown";
            this.btnLockdown.Size = new System.Drawing.Size(75, 23);
            this.btnLockdown.TabIndex = 0;
            this.btnLockdown.Text = "lockdown";
            this.btnLockdown.UseVisualStyleBackColor = true;
            this.btnLockdown.Click += new System.EventHandler(this.btnLockdown_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Enabled = false;
            this.btnUnlock.Location = new System.Drawing.Point(93, 12);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(75, 23);
            this.btnUnlock.TabIndex = 1;
            this.btnUnlock.Text = "unlock";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(176, 45);
            this.Controls.Add(this.btnUnlock);
            this.Controls.Add(this.btnLockdown);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(192, 83);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(192, 83);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "antiCryptoLocker";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLockdown;
        private System.Windows.Forms.Button btnUnlock;

    }
}

