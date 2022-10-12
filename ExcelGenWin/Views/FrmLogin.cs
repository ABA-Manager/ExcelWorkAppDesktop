using ABABillingAndClaim.Models;
using ABABillingAndClaim.Services;
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
    public partial class FrmLogin : Form
    {
        private readonly AuthService _authService;
        public FrmLogin(AuthService authService)
        {
            _authService = authService;
            InitializeComponent();
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
    }
}
