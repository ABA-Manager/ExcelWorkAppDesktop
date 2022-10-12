namespace ABABillingAndClaim.Views
{
    partial class FrmCreateUser
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
            this.usernameLb = new System.Windows.Forms.Label();
            this.emailLb = new System.Windows.Forms.Label();
            this.RolesLb = new System.Windows.Forms.Label();
            this.usernameTb = new System.Windows.Forms.TextBox();
            this.emailTb = new System.Windows.Forms.TextBox();
            this.rolCb = new System.Windows.Forms.ComboBox();
            this.rolBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CreateBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.rolBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // usernameLb
            // 
            this.usernameLb.AutoSize = true;
            this.usernameLb.Location = new System.Drawing.Point(12, 12);
            this.usernameLb.Name = "usernameLb";
            this.usernameLb.Size = new System.Drawing.Size(63, 13);
            this.usernameLb.TabIndex = 0;
            this.usernameLb.Text = "User Name:";
            // 
            // emailLb
            // 
            this.emailLb.AutoSize = true;
            this.emailLb.Location = new System.Drawing.Point(12, 42);
            this.emailLb.Name = "emailLb";
            this.emailLb.Size = new System.Drawing.Size(35, 13);
            this.emailLb.TabIndex = 1;
            this.emailLb.Text = "Email:";
            // 
            // RolesLb
            // 
            this.RolesLb.AutoSize = true;
            this.RolesLb.Location = new System.Drawing.Point(12, 75);
            this.RolesLb.Name = "RolesLb";
            this.RolesLb.Size = new System.Drawing.Size(37, 13);
            this.RolesLb.TabIndex = 2;
            this.RolesLb.Text = "Roles:";
            // 
            // usernameTb
            // 
            this.usernameTb.Location = new System.Drawing.Point(81, 9);
            this.usernameTb.Name = "usernameTb";
            this.usernameTb.Size = new System.Drawing.Size(203, 20);
            this.usernameTb.TabIndex = 3;
            // 
            // emailTb
            // 
            this.emailTb.Location = new System.Drawing.Point(81, 39);
            this.emailTb.Name = "emailTb";
            this.emailTb.Size = new System.Drawing.Size(203, 20);
            this.emailTb.TabIndex = 4;
            // 
            // rolCb
            // 
            this.rolCb.DataSource = this.rolBindingSource;
            this.rolCb.DisplayMember = "name";
            this.rolCb.FormattingEnabled = true;
            this.rolCb.Location = new System.Drawing.Point(81, 72);
            this.rolCb.Name = "rolCb";
            this.rolCb.Size = new System.Drawing.Size(203, 21);
            this.rolCb.TabIndex = 5;
            this.rolCb.ValueMember = "name";
            // 
            // rolBindingSource
            // 
            this.rolBindingSource.DataSource = typeof(ABABillingAndClaim.Models.Rol);
            // 
            // CreateBtn
            // 
            this.CreateBtn.Location = new System.Drawing.Point(150, 109);
            this.CreateBtn.Name = "CreateBtn";
            this.CreateBtn.Size = new System.Drawing.Size(75, 23);
            this.CreateBtn.TabIndex = 7;
            this.CreateBtn.Text = "Create";
            this.CreateBtn.UseVisualStyleBackColor = true;
            this.CreateBtn.Click += new System.EventHandler(this.CreateBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(231, 110);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // FrmCreateUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 145);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.CreateBtn);
            this.Controls.Add(this.rolCb);
            this.Controls.Add(this.emailTb);
            this.Controls.Add(this.usernameTb);
            this.Controls.Add(this.RolesLb);
            this.Controls.Add(this.emailLb);
            this.Controls.Add(this.usernameLb);
            this.MaximizeBox = false;
            this.Name = "FrmCreateUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmCreateUser";
            this.Load += new System.EventHandler(this.FrmCreateUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rolBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label usernameLb;
        private System.Windows.Forms.Label emailLb;
        private System.Windows.Forms.Label RolesLb;
        private System.Windows.Forms.TextBox usernameTb;
        private System.Windows.Forms.TextBox emailTb;
        private System.Windows.Forms.ComboBox rolCb;
        private System.Windows.Forms.BindingSource rolBindingSource;
        private System.Windows.Forms.Button CreateBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}