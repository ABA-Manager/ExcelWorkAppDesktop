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
    public partial class FrmMain : Form
    {
        private readonly AuthService _authService;
        private readonly MemoryService _memoryService;

        // Merge with Mijail y
        public Clinic_AppContext db;

        public FrmMain()
        {
            // Dependency Injection
            _memoryService = new MemoryService();
            _authService = new AuthService(_memoryService);

            InitializeComponent();
            Text = "Analyst Manage and Billing for Windows";
            db = new Clinic_AppContext("name=Clinic_AppContext");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Close
            //if (MessageBox.Show("You are closing this application, Are you sure?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            Application.Exit();
        }

        private void excelGenerationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FrmExcelGen(db);
            frm.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FrmSettings();
            frm.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FrmAbout();
            frm.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var frm = new FrmLogin(_authService);
            if (frm.ShowDialog() != DialogResult.OK)
                Application.Exit();
        }

        private void webScrappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FrmMedicaidScrap(db, _memoryService);
            frm.ShowDialog();
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

            var frm = new FrmLogin(_authService);
            if (frm.ShowDialog() != DialogResult.OK)
                Application.Exit();
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FrmListUser(_authService);
            frm.ShowDialog();
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FrmCreateUser(_authService);
            frm.ShowDialog();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            e.Cancel = _memoryService.LoggedOndUser != null && MessageBox.Show("You are closing this application, Are you sure?", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes;
        }
    }
}
