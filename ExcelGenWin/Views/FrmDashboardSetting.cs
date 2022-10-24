using ABABillingAndClaim.Models;
using ABABillingAndClaim.Services;
using ClinicDOM;
using System.Windows.Forms;

namespace ABABillingAndClaim.Views
{
    public partial class FrmDashboardSetting : Form
    {
        private readonly Clinic_AppContext _db;
        public DashboardSetting _dashboardSetting;

        public FrmDashboardSetting(Clinic_AppContext db, DashboardSetting dashboardSetting)
        {
            _db = db;
            _dashboardSetting = dashboardSetting;
            InitializeComponent();
        }

        private void AceptBtn_Click(object sender, System.EventArgs e)
        {
            _dashboardSetting.Company = (Company)companyCb.SelectedItem;
            _dashboardSetting.Period = (Period)periodCb.SelectedItem;
            DialogResult = DialogResult.OK;
        }

        private async void FrmDashboardSetting_Load(object sender, System.EventArgs e)
        {
            var service = new Dashboard(_db);
            var periods = await service.GetPeriods();
            periodCb.DataSource = new BindingSource(periods, null);
            periodCb.DisplayMember = "PayPeriod";
            periodCb.ValueMember = "Id";
            periodCb.SelectedValue = _dashboardSetting.Period.Id;

            var companies = await service.GetCompanies();
            companyCb.DataSource = new BindingSource(companies, null);
            companyCb.DisplayMember = "Name";
            companyCb.ValueMember = "Id";
            companyCb.SelectedValue = _dashboardSetting.Company.Id; ;
        }
    }
}
