using ABABillingAndClaim.Services;
using ClinicDOM;
using ExcelGenLib;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABABillingAndClaim.Views
{
    public partial class FrmExcelGen : Form
    {
        public ExcelGenerator ExcelGen;
        public bool FilePathExist;
        public bool IsValidName;
        public FrmExcelGen()
        {
            InitializeComponent();
            tbPackFile.Text = ConfigurationManager.AppSettings["excel.packFile"];
            FileInfo fi;
            try
            {
                fi = new FileInfo(tbPackFile.Text);
                FilePathExist = Directory.Exists(fi.DirectoryName);
                IsValidName = true;
            }
            catch (Exception)
            {
                IsValidName = false;
                FilePathExist = false;
            }

            //this.db = db;

            ExcelGen = new ExcelGenerator(tbProcessLog, pbProgressBar);
            var periods = BillingService.Instance.GetPeriodsAsync().Result;

            ExcelGen.SetPeriod().Wait();
            cbPeriods.DataSource = new BindingSource(periods, null);
            cbPeriods.DisplayMember = "formated";
            cbPeriods.ValueMember = "Id";
            cbPeriods.SelectedValue = ExcelGen.GetPeriodId();
            cbCompany.DataSource = new BindingSource(ExcelGen.CompanyData, null);
            cbCompany.DisplayMember = "Name";
            cbCompany.ValueMember = "Acronym";
            // cbCompany.SelectedItem = null;
            cbCompany.SelectedItem = ExcelGen.CompanyData[0];
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if ((FilePathExist && IsValidName) || (saveFileDialog1.ShowDialog() == DialogResult.OK))
            { //Do process
                Control.CheckForIllegalCrossThreadCalls = false;
                btnGenerate.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string zipFile = saveFileDialog1.FileName;
            string password = FrmSettings.ExcelPassword;
            try
            {
                if ((int)cbPeriods.SelectedValue != ExcelGen.GetPeriodId())
                    ExcelGen.SetPeriod((int)cbPeriods.SelectedValue).Wait();
                tbProcessLog.AppendText(string.Format("Start Process: {0}\r\n", DateTime.Now));
                tbProcessLog.AppendText(string.Format("Genering Zip File: {0}\r\n", zipFile));

                var resp = await ExcelGen.GenBillingAsync(zipFile, (cbCompany.SelectedItem == null ? "" : cbCompany.SelectedValue.ToString()), password, ExcelGenerator.OutputType.WinForm);

                tbProcessLog.AppendText(string.Format("End Process: {0}\r\n", DateTime.Now));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnGenerate.Enabled = true;
        }

        private void ExcelGenFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ExcelGen.CloseAll();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cbCompany.Enabled = checkBox1.Checked;
            cbCompany.SelectedItem = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            tbPackFile.Text = saveFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void tbPackFile_TextChanged(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = tbPackFile.Text;
        }

        private async void FrmExcelGen_Shown(object sender, EventArgs e)
        {
            await ExcelGen.GetExcelApp();
        }
    }
}
