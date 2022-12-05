﻿using ABABillingAndClaim.Models;
using ABABillingAndClaim.Services;
using ClinicDOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ABABillingAndClaim.Views
{
    public partial class FrmMain : Form
    {
        private readonly AuthService _authService;
        private readonly MemoryService _memoryService;
        private DashboardSetting _dashboardSetting;

        // Merge with Mijail y
        public Clinic_AppContext db;

        public FrmMain()
        {
            // Dependency Injection
            _memoryService = new MemoryService();
            _authService = new AuthService(_memoryService);

            InitializeComponent();
            Text = "Analyst Manage and Billing for Windows";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Close
            //if (MessageBox.Show("You are closing this application, Are you sure?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            Application.Exit();
        }

        private void excelGenerationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FrmExcelGen(db);
                frm.ShowDialog();
            }
            catch (System.Reflection.TargetInvocationException tix)
            {
                MessageBox.Show($"We are still working. Wait a few minutes \nApp message: {tix.Message}", "Open dialog not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (NotSupportedException ex)
            {

                MessageBox.Show($"We are still working. \nApp message: {ex.Message}", "Refresh not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.Data.Entity.Core.EntityException efx)
            {
                MessageBox.Show($"Network error or too slow connection, wait a few minutes \nApp message: {efx.Message}", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FrmSettings();
                frm.ShowDialog();
            }
            catch (System.Reflection.TargetInvocationException tix)
            {
                MessageBox.Show($"We are still working. Wait a few minutes \nApp message: {tix.Message}", "Open dialog not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (NotSupportedException ex)
            {

                MessageBox.Show($"We are still working. \nApp message: {ex.Message}", "Refresh not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.Data.Entity.Core.EntityException efx)
            {
                MessageBox.Show($"Network error or too slow connection, wait a few minutes \nApp message: {efx.Message}", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FrmAbout();
            frm.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var frm = new FrmLogin(_authService, _memoryService);
            if (frm.ShowDialog() != DialogResult.OK)
                Application.Exit();
            else
            {
                db = new Clinic_AppContext($"name={_memoryService.DataBaseEndPoint}");
                // Load dashboard async
                loadDashboard(db);
            }
        }

        private void webScrappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FrmMedicaidScrap(db, _memoryService);
                frm.ShowDialog();
            }
            catch (System.Reflection.TargetInvocationException tix)
            {
                MessageBox.Show($"We are still working. Wait a few minutes \nApp message: {tix.Message}", "Open dialog not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (NotSupportedException ex)
            {

                MessageBox.Show($"We are still working. \nApp message: {ex.Message}", "Refresh not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.Data.Entity.Core.EntityException efx)
            {
                MessageBox.Show($"Network error or too slow connection, wait a few minutes \nApp message: {efx.Message}", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var changePass = new ChangePassword(_memoryService, _authService);
            changePass.ShowDialog();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            _memoryService.Token = "";
            _memoryService.Connected = false;
            _memoryService.LoggedOndUser = null;

            var frm = new FrmLogin(_authService, _memoryService);
            if (frm.ShowDialog() != DialogResult.OK)
                Application.Exit();
            else
            {
                db = new Clinic_AppContext($"name={_memoryService.DataBaseEndPoint}");
                loadDashboard(db);
            }
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FrmListUser(_authService);
                frm.ShowDialog();
            }
            catch (System.Reflection.TargetInvocationException tix)
            {
                MessageBox.Show($"We are still working. Wait a few minutes \nApp message: {tix.Message}", "Open dialog not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (NotSupportedException ex)
            {

                MessageBox.Show($"We are still working. \nApp message: {ex.Message}", "Refresh not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.Data.Entity.Core.EntityException efx)
            {
                MessageBox.Show($"Network error or too slow connection, wait a few minutes \nApp message: {efx.Message}", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FrmCreateUser(_authService);
                frm.ShowDialog();
            }
            catch (System.Reflection.TargetInvocationException tix)
            {
                MessageBox.Show($"We are still working. Wait a few minutes \nApp message: {tix.Message}", "Open dialog not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (NotSupportedException ex)
            {

                MessageBox.Show($"We are still working. \nApp message: {ex.Message}", "Refresh not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.Data.Entity.Core.EntityException efx)
            {
                MessageBox.Show($"Network error or too slow connection, wait a few minutes \nApp message: {efx.Message}", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            e.Cancel = _memoryService.LoggedOndUser != null && MessageBox.Show("You are closing this application, Are you sure?", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes;
        }

        // Building DashBoard
        private async void loadDashboard(Clinic_AppContext _db)
        {
            Dashboard service = new Dashboard(_db);
            try
            {
                if (_dashboardSetting == null)
                    await FillDasboardSettings(service);

                Parallel.Invoke(
                           () =>  HistoryProfit(service, _dashboardSetting.Company.Id),
                           () => StatusServicesLog(service, _dashboardSetting.Company.Id, _dashboardSetting.Period.Id)
                           );

                ServiceLogWithoutPatientAccount(service, _dashboardSetting.Company.Id, _dashboardSetting.Period.Id);
               
                GeneralData(service, _dashboardSetting.Company.Id, _dashboardSetting.Period.Id);

                toolStripStatusLabel1.Text = $"Company {_dashboardSetting.Company.Name}";
                toolStripStatusLabel2.Text = $"Period {_dashboardSetting.Period.PayPeriod}";
            }
            catch (NotSupportedException ex)
            {

                MessageBox.Show($"We are still working. \nApp message: {ex.Message}", "Refresh not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.Data.Entity.Core.EntityException efx)
            {
                MessageBox.Show($"Network error or too slow connection, wait a few minutes \nApp message: {efx.Message}", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task FillDasboardSettings(Dashboard service)
        {
            _dashboardSetting = new DashboardSetting();
            var companies = await service.GetCompanies();
            var periods = await service.GetPeriods();
            _dashboardSetting.Company = companies.FirstOrDefault();
            _dashboardSetting.Period = periods.FirstOrDefault();
        }

        private void HistoryProfit(Dashboard _service, int company_id = 1)
        {
            profitHistoryChart.Invoke((MethodInvoker)(() =>
            {
                var historyBindingSource = _service.GetProfit(company_id: company_id);

                var objChart = profitHistoryChart.ChartAreas[0];

                // PayPerdiod
                // objChart.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
                List<string> labels = historyBindingSource.Select(x => x.PayPeriod).ToList<string>();
                double startOffset = 0;
                double endOffset = 1;
                foreach (string labelName in labels)
                {
                    CustomLabel label = new CustomLabel(startOffset, endOffset, labelName, 0, LabelMarkStyle.None);
                    objChart.AxisX.CustomLabels.Add(label);
                    startOffset++;
                    endOffset++;
                }

                objChart.AxisX.Minimum = 0;
                objChart.AxisX.Maximum = historyBindingSource.Count();


                // Clear graphic
                profitHistoryChart.Series.Clear();

                // Random Color
                Random random = new Random();
                List<string> series = new List<string>() { "Profit", "Billed", "Payment" };

                foreach (var item in series)
                {
                    profitHistoryChart.Series.Add(item);
                    profitHistoryChart.Series[item].Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                    profitHistoryChart.Series[item].Legend = "Legend1";
                    profitHistoryChart.Series[item].ChartArea = "ChartArea1";
                    profitHistoryChart.Series[item].ChartType = SeriesChartType.Line;
                    profitHistoryChart.Series[item].ToolTip = $"{item}: {"#VALY{F2}"}"; // "#VALY{F2}"

                    // Add data
                    for (int i = 0; i < historyBindingSource.Count(); i++)
                    {
                        profitHistoryChart.Series[item].Points.AddXY(i, historyBindingSource.ToArray()[i][item]);
                    }
                }
            }));
        }

        private void StatusServicesLog(Dashboard _service, int company_id = 1, int period_id = 20)
        {
            profitHistoryChart.Invoke((MethodInvoker)(() =>
            {
                var result = _service.GetServicesLgStatus(company_id, period_id);
                StatusBillingChart.Series.Clear();

                List<string> data = new List<string>() { "Pending", "Billed", "NotBilled" };

                StatusBillingChart.Series.Add("statusBillingSerie");
                StatusBillingChart.Series["statusBillingSerie"].ChartType = SeriesChartType.Pie;
                StatusBillingChart.Series["statusBillingSerie"].ToolTip = $"{"#VALY{F2}"}";

                // Add data
                foreach (string item in data)
                {
                    StatusBillingChart.Series["statusBillingSerie"].Points.AddXY(item, result[item]);
                }
            }));
        }

        private void ServiceLogWithoutPatientAccount(Dashboard _service, int company_id = 1, int period_id = 20)//test
        {
            errorPADataGrid.Invoke((MethodInvoker)(delegate
            {
                serviceLogWithoutPatientAccountBindingSource.Clear();
                serviceLogWithoutPatientAccountBindingSource.DataSource = _service.GetServiceLogWithoutPatientAccount(company_id, period_id);
            }));
        }

        private void GeneralData(Dashboard _service, int company_id = 1, int period_id = 20)
        {
            var result = _service.GetGeneralData(company_id, period_id);

            patient.Text = $"{result.Client}";

            physician.Text = $"{result.Contractor}";

            serviceLog.Text = $"{result.ServiceLog}";

        }
        private void refreshDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadDashboard(db);
        }

        private void dashboardSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FrmDashboardSetting(db, _dashboardSetting);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _dashboardSetting = frm._dashboardSetting;
                    loadDashboard(db);
                }
            }
            catch (System.Reflection.TargetInvocationException tix)
            {
                MessageBox.Show($"We are still working. Wait a few minutes \nApp message: {tix.Message}", "Open dialog not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (NotSupportedException ex)
            {

                MessageBox.Show($"We are still working. \nApp message: {ex.Message}", "Refresh not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.Data.Entity.Core.EntityException efx)
            {
                MessageBox.Show($"Network error or too slow connection, wait a few minutes \nApp message: {efx.Message}", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void unbilledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FrmUnbilled(db, _memoryService, _dashboardSetting);
                frm.ShowDialog();
            }
            catch (System.Reflection.TargetInvocationException tix)
            {
                MessageBox.Show($"We are still working. Wait a few minutes \nApp message: {tix.Message}", "Open dialog not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (NotSupportedException ex)
            {

                MessageBox.Show($"We are still working. \nApp message: {ex.Message}", "Refresh not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.Data.Entity.Core.EntityException efx)
            {
                MessageBox.Show($"Network error or too slow connection, wait a few minutes \nApp message: {efx.Message}", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
