using ABABillingAndClaim.Models;
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

namespace ABABillingAndClaim.Views
{
    public partial class FrmLogin : Form
    {
        private readonly AuthService _authService;
        private readonly MemoryService _memoryService;

        private List<string> companies;
        public FrmLogin(AuthService authService, MemoryService memoryService)
        {
            _authService = authService;
            _memoryService = memoryService;
            InitializeComponent();
            load_company();
            companiesCb_SelectedIndexChanged(null, null);
        }

        private void load_company()
        {
            this.companies = ConfigurationManager.AppSettings["manage.company"].ToString().Split(';').ToList();
            int count = 1;
            this.companies.ForEach((item) =>
            {
                this.companyCbBindingSource.Add(new CompanyCb { key = count, value = item });
                count++;
                // this.companiesCb.Items.Add(new CompanyCb { key = count, value = item });
            });

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var response = _authService.Login(new LoginViewModel
            {
                Username = usernameTb.Text,
                Password = passwordTb.Text
            });

            if (response.IsSuccess)
            {
                DialogResult = DialogResult.OK;
            }
            else if (!response.IsSuccess && !response.Message.Contains("Some properties are not valid"))
            {
                MessageBox.Show("Incorrect username or password");
            }
            else
            {
                MessageBox.Show("Some properties are not valid");
            }
        }

        private void companiesCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._memoryService.Company = (CompanyCb)companiesCb.SelectedItem;
            switch (this._memoryService.Company.key)
            {
                case 1:
                    {
                        this._memoryService.BaseEndPoint = Properties.Settings.Default.VL_BASE_ENDPOINT;
                        this._memoryService.DataBaseEndPoint = "vl";
                    }
                    break;
                case 2:
                    {
                        this._memoryService.BaseEndPoint = Properties.Settings.Default.EP_BASE_ENDPOINT;
                        this._memoryService.DataBaseEndPoint = "ep";
                    }
                    break;
                case 3:
                    {
                        this._memoryService.BaseEndPoint = Properties.Settings.Default.BBI_BASE_ENDPOINT;
                        this._memoryService.DataBaseEndPoint = "bbi";
                    }; break;
                case 4:
                    {
                        this._memoryService.BaseEndPoint = Properties.Settings.Default.PL_BASE_ENDPOINT;
                        this._memoryService.DataBaseEndPoint = "pl";
                    }; break;
                default:
                    break;
            }
        }
    }
}
