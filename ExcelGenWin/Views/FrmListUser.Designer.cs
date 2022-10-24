namespace ABABillingAndClaim.Views
{
    partial class FrmListUser
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
            this.ListUserDG = new System.Windows.Forms.DataGridView();
            this.usernameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResetPassword = new System.Windows.Forms.DataGridViewButtonColumn();
            this.userBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.closeBtn = new System.Windows.Forms.Button();
            this.CreateBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ListUserDG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ListUserDG
            // 
            this.ListUserDG.AllowUserToAddRows = false;
            this.ListUserDG.AllowUserToOrderColumns = true;
            this.ListUserDG.AutoGenerateColumns = false;
            this.ListUserDG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListUserDG.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.usernameDataGridViewTextBoxColumn,
            this.emailDataGridViewTextBoxColumn,
            this.id,
            this.ResetPassword});
            this.ListUserDG.DataSource = this.userBindingSource;
            this.ListUserDG.Dock = System.Windows.Forms.DockStyle.Top;
            this.ListUserDG.Location = new System.Drawing.Point(0, 0);
            this.ListUserDG.Name = "ListUserDG";
            this.ListUserDG.ReadOnly = true;
            this.ListUserDG.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ListUserDG.Size = new System.Drawing.Size(438, 236);
            this.ListUserDG.TabIndex = 0;
            this.ListUserDG.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListUserDG_CellClick);
            this.ListUserDG.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.ListUserDG_CellPainting);
            this.ListUserDG.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.ListUserDG_RowsRemoved);
            this.ListUserDG.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.ListUserDG_UserDeletedRow);
            this.ListUserDG.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.ListUserDG_UserDeletingRow);
            // 
            // usernameDataGridViewTextBoxColumn
            // 
            this.usernameDataGridViewTextBoxColumn.DataPropertyName = "username";
            this.usernameDataGridViewTextBoxColumn.HeaderText = "User Name";
            this.usernameDataGridViewTextBoxColumn.Name = "usernameDataGridViewTextBoxColumn";
            this.usernameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // emailDataGridViewTextBoxColumn
            // 
            this.emailDataGridViewTextBoxColumn.DataPropertyName = "email";
            this.emailDataGridViewTextBoxColumn.HeaderText = "Email";
            this.emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            this.emailDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // ResetPassword
            // 
            this.ResetPassword.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ResetPassword.HeaderText = "Reset Password";
            this.ResetPassword.Name = "ResetPassword";
            this.ResetPassword.ReadOnly = true;
            this.ResetPassword.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ResetPassword.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // userBindingSource
            // 
            this.userBindingSource.DataSource = typeof(ABABillingAndClaim.Models.User);
            // 
            // closeBtn
            // 
            this.closeBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeBtn.Location = new System.Drawing.Point(355, 242);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 2;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            // 
            // CreateBtn
            // 
            this.CreateBtn.Location = new System.Drawing.Point(274, 242);
            this.CreateBtn.Name = "CreateBtn";
            this.CreateBtn.Size = new System.Drawing.Size(75, 23);
            this.CreateBtn.TabIndex = 3;
            this.CreateBtn.Text = "Create...";
            this.CreateBtn.UseVisualStyleBackColor = true;
            this.CreateBtn.Click += new System.EventHandler(this.CreateBtn_Click);
            // 
            // FrmListUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 273);
            this.Controls.Add(this.CreateBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.ListUserDG);
            this.MinimumSize = new System.Drawing.Size(454, 312);
            this.Name = "FrmListUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmListUser";
            this.Load += new System.EventHandler(this.FrmListUser_Load);
            this.Shown += new System.EventHandler(this.FrmListUser_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ListUserDG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ListUserDG;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.BindingSource userBindingSource;
        private System.Windows.Forms.DataGridViewButtonColumn ResetPassword;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.Button CreateBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usernameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
    }
}