namespace ABABillingAndClaim.Views
{
    partial class FrmUnbilled
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cancelbtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.periodCb = new System.Windows.Forms.ComboBox();
            this.periodLb = new System.Windows.Forms.Label();
            this.companyCb = new System.Windows.Forms.ComboBox();
            this.comapnyLb = new System.Windows.Forms.Label();
            this.billedUserDG = new System.Windows.Forms.DataGridView();
            this.clientNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contractorNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnBilled = new System.Windows.Forms.DataGridViewButtonColumn();
            this.managerBillerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.billedUserDG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.managerBillerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 617);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.cancelbtn);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 558);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(578, 56);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // cancelbtn
            // 
            this.cancelbtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbtn.Location = new System.Drawing.Point(509, 3);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(66, 34);
            this.cancelbtn.TabIndex = 0;
            this.cancelbtn.Text = "Cancel";
            this.cancelbtn.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.billedUserDG, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(578, 549);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.periodCb);
            this.panel1.Controls.Add(this.periodLb);
            this.panel1.Controls.Add(this.companyCb);
            this.panel1.Controls.Add(this.comapnyLb);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(572, 44);
            this.panel1.TabIndex = 1;
            // 
            // periodCb
            // 
            this.periodCb.FormattingEnabled = true;
            this.periodCb.Location = new System.Drawing.Point(283, 17);
            this.periodCb.Name = "periodCb";
            this.periodCb.Size = new System.Drawing.Size(142, 21);
            this.periodCb.TabIndex = 3;
            this.periodCb.SelectedIndexChanged += new System.EventHandler(this.periodCb_SelectedIndexChanged);
            // 
            // periodLb
            // 
            this.periodLb.AutoSize = true;
            this.periodLb.Location = new System.Drawing.Point(226, 20);
            this.periodLb.Name = "periodLb";
            this.periodLb.Size = new System.Drawing.Size(37, 13);
            this.periodLb.TabIndex = 2;
            this.periodLb.Text = "Period";
            // 
            // companyCb
            // 
            this.companyCb.FormattingEnabled = true;
            this.companyCb.Location = new System.Drawing.Point(65, 17);
            this.companyCb.Name = "companyCb";
            this.companyCb.Size = new System.Drawing.Size(131, 21);
            this.companyCb.TabIndex = 1;
            this.companyCb.SelectedIndexChanged += new System.EventHandler(this.companyCb_SelectedIndexChanged);
            // 
            // comapnyLb
            // 
            this.comapnyLb.AutoSize = true;
            this.comapnyLb.Location = new System.Drawing.Point(8, 20);
            this.comapnyLb.Name = "comapnyLb";
            this.comapnyLb.Size = new System.Drawing.Size(51, 13);
            this.comapnyLb.TabIndex = 0;
            this.comapnyLb.Text = "Company";
            // 
            // billedUserDG
            // 
            this.billedUserDG.AllowUserToAddRows = false;
            this.billedUserDG.AllowUserToDeleteRows = false;
            this.billedUserDG.AutoGenerateColumns = false;
            this.billedUserDG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.billedUserDG.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clientNameDataGridViewTextBoxColumn,
            this.contractorNameDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.billerDataGridViewTextBoxColumn,
            this.billedDateDataGridViewTextBoxColumn,
            this.UnBilled});
            this.billedUserDG.DataSource = this.managerBillerBindingSource;
            this.billedUserDG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.billedUserDG.Location = new System.Drawing.Point(3, 53);
            this.billedUserDG.MultiSelect = false;
            this.billedUserDG.Name = "billedUserDG";
            this.billedUserDG.ReadOnly = true;
            this.billedUserDG.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.billedUserDG.Size = new System.Drawing.Size(572, 493);
            this.billedUserDG.TabIndex = 2;
            this.billedUserDG.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.billedUserDG_CellClick);
            this.billedUserDG.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.billedPatientDG_CellPainting);
            // 
            // clientNameDataGridViewTextBoxColumn
            // 
            this.clientNameDataGridViewTextBoxColumn.DataPropertyName = "ClientName";
            this.clientNameDataGridViewTextBoxColumn.HeaderText = "ClientName";
            this.clientNameDataGridViewTextBoxColumn.Name = "clientNameDataGridViewTextBoxColumn";
            this.clientNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // contractorNameDataGridViewTextBoxColumn
            // 
            this.contractorNameDataGridViewTextBoxColumn.DataPropertyName = "ContractorName";
            this.contractorNameDataGridViewTextBoxColumn.HeaderText = "ContractorName";
            this.contractorNameDataGridViewTextBoxColumn.Name = "contractorNameDataGridViewTextBoxColumn";
            this.contractorNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // billerDataGridViewTextBoxColumn
            // 
            this.billerDataGridViewTextBoxColumn.DataPropertyName = "Biller";
            this.billerDataGridViewTextBoxColumn.HeaderText = "Biller";
            this.billerDataGridViewTextBoxColumn.Name = "billerDataGridViewTextBoxColumn";
            this.billerDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // billedDateDataGridViewTextBoxColumn
            // 
            this.billedDateDataGridViewTextBoxColumn.DataPropertyName = "BilledDate";
            this.billedDateDataGridViewTextBoxColumn.HeaderText = "BilledDate";
            this.billedDateDataGridViewTextBoxColumn.Name = "billedDateDataGridViewTextBoxColumn";
            this.billedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // UnBilled
            // 
            this.UnBilled.HeaderText = "Unbilled";
            this.UnBilled.Name = "UnBilled";
            this.UnBilled.ReadOnly = true;
            this.UnBilled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // managerBillerBindingSource
            // 
            this.managerBillerBindingSource.DataSource = typeof(ABABillingAndClaim.Models.ManagerBiller);
            // 
            // FrmUnbilled
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 617);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(600, 656);
            this.Name = "FrmUnbilled";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unbilled";
            this.Load += new System.EventHandler(this.FrmUnbilled_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.billedUserDG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.managerBillerBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button cancelbtn;
        private System.Windows.Forms.BindingSource managerBillerBindingSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox companyCb;
        private System.Windows.Forms.Label comapnyLb;
        private System.Windows.Forms.ComboBox periodCb;
        private System.Windows.Forms.Label periodLb;
        private System.Windows.Forms.DataGridView billedUserDG;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contractorNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn UnBilled;
    }
}