using ABABillingAndClaim.Utils;
using CefSharp.WinForms;
using CefSharp;
using ClinicDOM;
using ExcelGenLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Data.SqlClient;
using ABABillingAndClaim.Services;
using CefSharp.Structs;

namespace ABABillingAndClaim.Views
{
    public partial class FrmMedicaidScrap : Form
    { 
        private NavigatorAccessBase navAccess;
        MemoryService _memory;

        public FrmMedicaidScrap(MemoryService memory)
        {
            InitializeComponent();

            this._memory = memory;

            //navAccess = new IeBrowserAccess(webBrowser2, lbCurrentPage);

            navAccess = new ChromiumBrowserAccess(WebBrowser1, lbCurrentPage);
        }

        public Details details { get; set; }
        public FrmMedicaidScrap(Details detail)
        {
            details = detail;
        }

        private async Task LoadAsync()
        {
            var periods = await BillingService.Instance.GetPeriodsAsync();
            cbPeriods.DataSource = new BindingSource(periods, null);
            cbPeriods.DisplayMember = "Formated";
            cbPeriods.ValueMember = "Id";
            cbPeriods.SelectedValue = periods[0].Id;

            var companies = await BillingService.Instance.GetCompaniesAsync();
            cbCompany.DataSource = new BindingSource(companies, null);
            cbCompany.DisplayMember = "Name";
            cbCompany.ValueMember = "Acronym";
            cbCompany.SelectedValue = companies[0].Acronym;

            await loadContractorAndClientInfoAsync(
                cbCompany.SelectedValue.ToString(),
                int.Parse(cbPeriods.SelectedValue.ToString())
            );
            // Load

            await navAccess.LoadUrlAsync(ConfigurationManager.AppSettings["web.URL"]);
        }

        private async void FrmMedicaidScrap_Load(object sender, EventArgs e)
        {
            //CefSettings settings = new CefSettings();
            //Cef.Initialize(settings);
            //WebBrowser1.LoadUrl(ConfigurationManager.AppSettings["web.URL"]));
            await LoadAsync();

        }



        public async Task loadContractorAndClientInfoAsync(string CompanyCode, int PeriodId)
        {
            var clientList = await BillingService.Instance.GetContractorAndClientsAsync(CompanyCode, PeriodId);

            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            var root = treeView1.Nodes.Add("Period");
            foreach (var cl in clientList)
            {
                var ncl = root.Nodes.Add(cl.Id.ToString(), cl.Name, cl.Status, cl.Status);
                ncl.Tag = cl;
                foreach (var ct in cl.Contractors)
                {
                    var nct = ncl.Nodes.Add(ct.Id.ToString(), ct.Name, ct.Status, ct.Status);
                    nct.Tag = ct;
                    foreach (var sl in ct.ServiceLogs)
                    {
                        var nsl = nct.Nodes.Add(sl.Id.ToString(), sl.Name, sl.Status, sl.Status);
                        nsl.Tag = sl;
                    }
                }
            }
            root.Expand();
            root.EnsureVisible();
            treeView1.EndUpdate();

            if (navAccess.isLoaded)
            {
                await navAccess.LoadLoginInfo(CompanyCode);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node.Tag != null
                && (e.Node.Tag is TvContractor || e.Node.Tag is TvServiceLog))
            {
                treeView1.SelectedNode = e.Node;
                var sl = treeView1.SelectedNode.Tag as TvObject;
                loadToFormToolStripMenuItem.Enabled = sl.Status != "billed" &&
                    navAccess.isLoaded &&
                    navAccess.getTitle().Contains("Professional");
                pendingToolStripMenuItem.Enabled = sl.Status != "billed";
                billedToolStripMenuItem.Enabled = sl.Status != "billed";
                contextMenuStrip1.Show(this.PointToScreen(e.Location));
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left
                && e.Node.Tag != null
                && (e.Node.Tag is TvContractor || e.Node.Tag is TvServiceLog))
                doDetails(e.Node.Tag as TvObject);
        }
        private async Task<bool> doLoad(TvObject obj)
        {
            int periodID = int.Parse(cbPeriods.SelectedValue.ToString());
            string companyCode = cbCompany.SelectedValue.ToString();

            if (obj is TvContractor)
            {
                var ct = obj as TvContractor;
                navAccess.details = new Details(ct.Client.Id, int.Parse(ct.Id), periodID, companyCode);
            }
            else if (obj is TvServiceLog)
            {
                var sl = obj as TvServiceLog;
                navAccess.details = new Details(int.Parse(sl.Id), companyCode, sl.Contractor.Client.Id.Split('_')[1]);
            }

            if (navAccess.getTitle().Contains("Professional") &&
                navAccess.isLoaded)
            {
                try
                {
                    await navAccess.LoadClientAndContractorInfo();
                    await navAccess.LoadDiagnosisInfo();
                    await navAccess.LoadServiceLogInfo();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Message");
                }
            }
            else MessageBox.Show("Error: The Professional Claims Medicaid Page is not loaded...");
            return false;
        }

        private void doDetails(object obj)
        {
            string companyCode = cbCompany.SelectedValue.ToString();
            int periodID = int.Parse(cbPeriods.SelectedValue.ToString());
            FrmLoadDetails frm = null;
            if (obj is TvContractor)
            {
                var ct = obj as TvContractor;
                frm = new FrmLoadDetails(ct, periodID, companyCode);
            }
            else //if (obj is TvServiceLog)
            {
                var sl = obj as TvServiceLog;
                frm = new FrmLoadDetails(sl, periodID, companyCode);
            }
            frm.ShowDialog();
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doDetails(treeView1.SelectedNode.Tag);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void cbCompany_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (navAccess.isLoaded)
            {
                string title = navAccess.getTitle();
                if (title.Contains("Iniciar") || title.Contains("Sign") || title.Contains("Professional"))
                {
                    await loadContractorAndClientInfoAsync(
                        cbCompany.SelectedValue.ToString(),
                        int.Parse(cbPeriods.SelectedValue.ToString())
                    );
                }
                else
                {
                    int index = cbCompany.SelectedIndex;

                    if (index == 1) cbCompany.SelectedIndex = 0;
                    else cbCompany.SelectedIndex = 1;

                    MessageBox.Show("Error: The Login Page is not loaded...");
                }
            }
        }

        private async void toolStripButton5_Click(object sender, EventArgs e)
        {
            await navAccess.DoRefreshAsync();
        }

        private async void toolStripButton4_Click(object sender, EventArgs e)
        {
            await navAccess.DoForwardAsync();
        }

        private async void toolStripButton3_Click(object sender, EventArgs e)
        {
            await navAccess.GoBackAsync();

        }

        private async void toolStripButton2_Click(object sender, EventArgs e)
        {
            await navAccess.LoadUrlAsync(ConfigurationManager.AppSettings["web.URL"]);
        }

        public void SetNodeStatus(TreeNode node, string status)
        {
            node.ImageKey = status;
            node.SelectedImageKey = status;
            if (node.Tag is TvServiceLog)
                (node.Tag as TvServiceLog).Status = status;
            else if (node.Tag is TvContractor)
                foreach (var it in (node.Tag as TvContractor).ServiceLogs)
                    it.Status = status;
        }

        private async void loadToFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetNodeStatus(treeView1.SelectedNode, "loading");
            var startAt = DateTime.Now;
            var res = await doLoad(treeView1.SelectedNode.Tag as TvObject);
            if (res)
            {
                var endAt = DateTime.Now;
                var dif = endAt.Subtract(startAt);
                SetNodeStatus(treeView1.SelectedNode, "loaded");
                foreach (var node in treeView1.SelectedNode.Nodes)
                    SetNodeStatus(((TreeNode)node), "loaded");
                MessageBox.Show($"Loaded to Form in {dif.Minutes} minutes and {dif.Seconds} seconds", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void billedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to mark this user as billed?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                if (treeView1.SelectedNode != null)
                {
                    if (await doBilled(treeView1.SelectedNode.Tag))
                    {
                        SetNodeStatus(treeView1.SelectedNode, "billed");
                        foreach (var node in treeView1.SelectedNode.Nodes)
                        {
                            SetNodeStatus(((TreeNode)node), "billed");
                        }
                    }
                }
        }

        private async Task<bool> doBilled(object obj)
        {
            if (navAccess.isLoaded)
            {
                string title = navAccess.getTitle();
                if (title.Contains("Professional"))
                {
                    try
                    {
                        int periodID = int.Parse(cbPeriods.SelectedValue.ToString());

                        string commandText;
                        SqlParameter[] sqlParams;

                        if (obj is TvServiceLog)
                        {
                            var sl = obj as TvServiceLog;
                            commandText = "UPDATE Servicelog SET BilledDate = @BilledDate, Biller = @user, Pending = null WHERE Id = @serviceLogId";
                            sqlParams = new[] {
                            new SqlParameter("@BilledDate", DateTime.Now),
                            new SqlParameter("@user", _memory.LoggedOndUser.id),
                            new SqlParameter("@serviceLogId", sl.Id)
                        };
                            sl.Status = "billed";
                        }
                        else
                        {
                            var ct = obj as TvContractor;
                            commandText = "UPDATE Servicelog SET BilledDate = @BilledDate, Biller = @user, Pending = null WHERE periodid = @period AND contractorId = @contractor AND clientId = @client";
                            sqlParams = new[] {
                            new SqlParameter("@BilledDate", DateTime.Now),
                            new SqlParameter("@user", _memory.LoggedOndUser.id),
                            new SqlParameter("@period", periodID),
                            new SqlParameter("@contractor", ct.Id),
                            new SqlParameter("@client", ct.Client.Id.Split('_')[0])
                        };
                            foreach (var it in ct.ServiceLogs) it.Status = "billed";
                        }
                        await db.Database.ExecuteSqlCommandAsync(commandText, sqlParams);
                        MessageBox.Show("Professional Claim Billed");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: Connectivity Error [{ex.Message}]");
                    }
                }
                else MessageBox.Show("Error: The Professional Claims Medicaid Page is not loaded...");
            }
            return false;
        }


        private void FrmMedicaidScrap_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = MessageBox.Show("You are closing the Professional Claims Form, Are you sure?", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes;
        }

        private void pendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && (treeView1.SelectedNode.Tag is TvServiceLog || treeView1.SelectedNode.Tag is TvContractor))
            {
                var frm = new FrmPending(db, treeView1.SelectedNode.Tag as TvObject);
                if (frm.ShowDialog() == DialogResult.OK)
                    SetNodeStatus(treeView1.SelectedNode, "pending");
            }
        }
    }
}
