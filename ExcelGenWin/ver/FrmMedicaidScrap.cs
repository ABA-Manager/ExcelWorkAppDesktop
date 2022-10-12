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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Remoting.Contexts;
using CefSharp.DevTools.CSS;
using System.Xml.Linq;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using CefSharp.DevTools.Audits;

namespace ABABillingAndClaim
{
    public partial class FrmMedicaidScrap : Form
    {
        private readonly Clinic_AppContext db;

        private NavigatorAccessBase navAccess;

        public FrmMedicaidScrap(Clinic_AppContext db)
        {
            InitializeComponent();

            this.db = db;

            navAccess = new IeBrowserAccess(webBrowser2);
        }

        public Details details { get; set; }
        public FrmMedicaidScrap(Details detail)
        {
            details = detail;
        }

        private async Task LoadAsync()
        {
            var periods = await GetPeriods();
            cbPeriods.DataSource = new BindingSource(periods, null);
            cbPeriods.DisplayMember = "Formated";
            cbPeriods.ValueMember = "Id";
            cbPeriods.SelectedValue = periods[0].Id;

            var companies = await GetCompanies();
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

        public async Task<List<ExtendedPeriod>> GetPeriods()
        {
            //var periodQry = from p in db.Period
            //                where (p.StartDate < DateTime.Now)
            //                orderby p.StartDate descending
            //                select new ExtendedPeriod { Id = p.Id, StartDate = p.StartDate, EndDate = p.EndDate, PayPeriod = p.PayPeriod };


            var periodQry = db.Period
                .Where(p => p.StartDate < DateTime.Now)
                .OrderByDescending(p => p.StartDate)
                .Select(p => new ExtendedPeriod { Id = p.Id, StartDate = p.StartDate, EndDate = p.EndDate, PayPeriod = p.PayPeriod });

            return await periodQry.ToListAsync();
        }

        public async Task<List<Company>> GetCompanies()
        {
            var companyQry = from c in db.Company
                             select c;

            return await companyQry.ToListAsync();
        }

        public async Task loadContractorAndClientInfoAsync(string CompanyCode, int PeriodId)
        {
            var clientList = await getContractorAndClientsAsync(CompanyCode, PeriodId);
            string user;
            string pass;

            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            var root = treeView1.Nodes.Add("Period");
            foreach (var cl in clientList)
            {
                var ncl = root.Nodes.Add(cl.Id.ToString(), cl.Name, cl.status, cl.status);
                ncl.Tag = cl;
                foreach (var ct in cl.Contractors)
                {
                    var nct = ncl.Nodes.Add(ct.Id.ToString(), ct.Name, ct.status, ct.status);
                    nct.Tag = ct;
                }
            }
            root.ExpandAll();
            treeView1.EndUpdate();

            if (this.webBrowser2.ReadyState == WebBrowserReadyState.Complete)
            {
                string title = webBrowser2.DocumentTitle;
                if (title.Contains("Iniciar") || title.Contains("Sign"))
                {
                    if (CompanyCode == "VL")
                    {
                        user = ConfigurationManager.AppSettings["web.VL.user"].ToString();
                        pass = ConfigurationManager.AppSettings["web.VL.password"].ToString();
                    }
                    else
                    {
                        user = ConfigurationManager.AppSettings["web.EP.user"].ToString();
                        pass = ConfigurationManager.AppSettings["web.EP.password"].ToString();
                    }
                    navAccess.SetElement("login", user, false);
                    navAccess.SetElement("pass", pass, false);
                }
            }
        }

        public async Task<List<TvClient>> getContractorAndClientsAsync(string CompanyCode, int PeriodId)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var queryRes = (from ag in db.Agreement
                            join co in db.Company on new { ag.CompanyId, CompanyCode } equals new { CompanyId = co.Id, CompanyCode = co.Acronym }
                            join pr in db.Payroll on ag.PayrollId equals pr.Id
                            join ctt in db.ContractorType on pr.ContractorTypeId equals ctt.Id
                            join ct in db.Contractor on pr.ContractorId equals ct.Id
                            join cl in db.Client on ag.ClientId equals cl.Id
                            join sl in db.ServiceLog on new { ag.ClientId, pr.ContractorId, PeriodId } equals new { sl.ClientId, sl.ContractorId, sl.PeriodId }
                            //join ud in db.UnitDetail on sl.Id equals ud.ServiceLogId
                            orderby new { ClientId = ag.Client.Id, ContractorId = ag.Payroll.Contractor.Id }
                            select ag).Include(x => x.Client).Include(y => y.Payroll.Contractor).Include(y => y.Payroll.ContractorType);

            TvClient lastClient = null;
            var clientList = new List<TvClient>();
            foreach (var it in await queryRes.ToListAsync())
            {
                if (it.ClientId != lastClient?.Id) clientList.Add(lastClient = new TvClient() { Id = it.ClientId, Name = it.Client.Name, status = "empty" });
                lastClient.Contractors.Add(new TvContractor()
                {
                    Id = it.Payroll.ContractorId,
                    Name = it.Payroll.Contractor.Name,
                    ContratorType = it.Payroll.ContractorType.Name,
                    parent = lastClient,
                    status = "empty"
                });
            }
            db.Configuration.LazyLoadingEnabled = true;
            return clientList;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node.Tag != null && e.Node.Tag is TvContractor)
            {
                treeView1.SelectedNode = e.Node;
                contextMenuStrip1.Show(this.PointToScreen(e.Location));
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.Tag != null && e.Node.Tag is TvContractor)
                doDetails((TvContractor)(e.Node.Tag));
        }
        private void doLoad(TvContractor ct)
        {
            int periodID = int.Parse(cbPeriods.SelectedValue.ToString());
            string companyCode = cbCompany.SelectedValue.ToString();

            navAccess.details = new Details(ct.parent.Id, ct.Id, periodID, companyCode);

            if (this.webBrowser2.ReadyState == WebBrowserReadyState.Complete)
            {
                string title = webBrowser2.DocumentTitle;

                if (title.Contains("Professional"))
                {
                    navAccess.LoadClientInfo();
                    navAccess.LoadContractorInfo();             
                    navAccess.LoadDiagnosisInfo();
                    navAccess.LoadServiceLogInfo();           
                }
                else MessageBox.Show("Wait the Claims page load");
            }
        }

        private void doDetails(TvContractor ct)
        {
            int periodID = int.Parse(cbPeriods.SelectedValue.ToString());
            string companyCode = cbCompany.SelectedValue.ToString();
            var frm = new FrmLoadDetails(ct, periodID, companyCode);
            frm.ShowDialog();
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doDetails((TvContractor)(treeView1.SelectedNode.Tag));
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void cbCompany_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.webBrowser2.ReadyState == WebBrowserReadyState.Complete)
            {
                string title = webBrowser2.DocumentTitle;
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

                    MessageBox.Show("Wait the Sign page load");
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

        private void loadToFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doLoad((TvContractor)(treeView1.SelectedNode.Tag));
        }

        private void billedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doBilled((TvContractor)(treeView1.SelectedNode.Tag));
        }

        private void doBilled(TvContractor ct)
        {
            if (this.webBrowser2.ReadyState == WebBrowserReadyState.Complete)
            {
                string title = webBrowser2.DocumentTitle;
                if (title.Contains("Professional"))
                {
                    int periodID = int.Parse(cbPeriods.SelectedValue.ToString());
                    var periodQry = from p in db.ServiceLog
                                    where (p.ClientId == ct.parent.Id && p.ContractorId == ct.Id && p.PeriodId == periodID)
                                    select p;

                    var a = periodQry.First();
                    var current = db.ServiceLog.Find(a.Id);

                    var commandText = "UPDATE Servicelog SET BilledDate = @BilledDate, Biller = 1 WHERE Id = @Id";
                    var BilledDate = new SqlParameter("@BilledDate", DateTime.Now);
                    var Id = new SqlParameter("@Id", a.Id);

                    db.Database.ExecuteSqlCommand(commandText, new[] { BilledDate, Id });

                    MessageBox.Show("Billed Executed");
                }
                else
                {
                    MessageBox.Show("Wait the Claims page load");
                }
            }
        }
    }
}

