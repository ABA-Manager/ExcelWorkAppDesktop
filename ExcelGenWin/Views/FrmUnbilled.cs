using ABABillingAndClaim.Models;
using ABABillingAndClaim.Services;
using ClinicDOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABABillingAndClaim.Views
{
    public partial class FrmUnbilled : Form
    {
        private readonly MemoryService _memory;
        private readonly DashboardSetting _dashboardSetting;
        private Company currentCompany = null;
        private Period currentPeriod = null;
        public FrmUnbilled(MemoryService memory, DashboardSetting dashboardSetting)
        {
            _memory = memory;
            _dashboardSetting = dashboardSetting;
            InitializeComponent();
        }

        public async Task<IEnumerable<ManagerBiller>> GetBilledPatients(int period, int company) => await ManagerService.Instance.GetServiceLogsBilled(period, company);


        private void billedPatientDG_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == billedUserDG.Columns["Unbilled"].Index)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = (Properties.Resources.undo.Width + 4) / 2;
                var h = (Properties.Resources.undo.Height + 4) / 2;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.undo, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private async void FrmUnbilled_Load(object sender, EventArgs e)
        {
            var periods = await DashboardService.Instance.GetPeriods();
            periodCb.DataSource = new BindingSource(periods, null);
            periodCb.DisplayMember = "PayPeriod";
            periodCb.ValueMember = "Id";
            periodCb.SelectedValue = _dashboardSetting.Period.Id;

            var companies = await DashboardService.Instance.GetCompanies();
            companyCb.DataSource = new BindingSource(companies, null);
            companyCb.DisplayMember = "Name";
            companyCb.ValueMember = "Id";
            companyCb.SelectedValue = _dashboardSetting.Company.Id;

            currentCompany = companyCb.SelectedItem as Company;
            currentPeriod = periodCb.SelectedItem as Period;

            refreshDataBinding(await GetBilledPatients(_dashboardSetting.Period.Id, _dashboardSetting.Company.Id));
        }

        public void refreshDataBinding(IEnumerable<ManagerBiller> list) => managerBillerBindingSource.DataSource = list;

        private async void billedUserDG_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == billedUserDG.Columns["Unbilled"].Index)
                {
                    if (MessageBox.Show("You are trying unbilled this service log, Are you sure?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var billed = billedUserDG.Rows[e.RowIndex].DataBoundItem as ManagerBiller;
                        var result = await ManagerService.Instance.UpdateBilling(billed.Id);
                        refreshDataBinding(await GetBilledPatients(currentPeriod.Id, currentCompany.Id));
                        MessageBox.Show(result.ToString());
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private async void companyCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentCompany != null && currentPeriod != null)
            {
                
                refreshDataBinding(await GetBilledPatients(currentPeriod.Id, currentCompany.Id));
            }

        }

        private async void periodCb_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (currentCompany != null && currentPeriod != null)
            {
                var company = companyCb.SelectedItem as Company;
                var period = periodCb.SelectedItem as Period;
                refreshDataBinding(await GetBilledPatients(period.Id, company.Id));
            }
        }
    }
}
