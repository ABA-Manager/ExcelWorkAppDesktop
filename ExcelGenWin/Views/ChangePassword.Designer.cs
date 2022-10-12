namespace ABABillingAndClaim.Views
{
    partial class ChangePassword
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
            this.confirmPassLb = new System.Windows.Forms.Label();
            this.newPassTb = new System.Windows.Forms.TextBox();
            this.confirmPassTb = new System.Windows.Forms.TextBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.AcceptBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "New Password:";
            // 
            // confirmPassLb
            // 
            this.confirmPassLb.AutoSize = true;
            this.confirmPassLb.Location = new System.Drawing.Point(12, 43);
            this.confirmPassLb.Name = "confirmPassLb";
            this.confirmPassLb.Size = new System.Drawing.Size(94, 13);
            this.confirmPassLb.TabIndex = 1;
            this.confirmPassLb.Text = "Confirm Password:";
            // 
            // newPassTb
            // 
            this.newPassTb.Location = new System.Drawing.Point(134, 11);
            this.newPassTb.Name = "newPassTb";
            this.newPassTb.PasswordChar = '*';
            this.newPassTb.Size = new System.Drawing.Size(149, 20);
            this.newPassTb.TabIndex = 2;
            // 
            // confirmPassTb
            // 
            this.confirmPassTb.Location = new System.Drawing.Point(134, 40);
            this.confirmPassTb.Name = "confirmPassTb";
            this.confirmPassTb.PasswordChar = '*';
            this.confirmPassTb.Size = new System.Drawing.Size(149, 20);
            this.confirmPassTb.TabIndex = 3;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(208, 72);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // AcceptBtn
            // 
            this.AcceptBtn.Location = new System.Drawing.Point(127, 72);
            this.AcceptBtn.Name = "AcceptBtn";
            this.AcceptBtn.Size = new System.Drawing.Size(75, 23);
            this.AcceptBtn.TabIndex = 5;
            this.AcceptBtn.Text = "Ok";
            this.AcceptBtn.UseVisualStyleBackColor = true;
            this.AcceptBtn.Click += new System.EventHandler(this.AcceptBtn_Click);
            // 
            // ChangePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(295, 107);
            this.Controls.Add(this.AcceptBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.confirmPassTb);
            this.Controls.Add(this.newPassTb);
            this.Controls.Add(this.confirmPassLb);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ChangePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label confirmPassLb;
        private System.Windows.Forms.TextBox newPassTb;
        private System.Windows.Forms.TextBox confirmPassTb;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button AcceptBtn;
    }
}