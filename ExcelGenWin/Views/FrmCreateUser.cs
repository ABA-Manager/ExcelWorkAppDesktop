using ABABillingAndClaim.Models;
using ABABillingAndClaim.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABABillingAndClaim.Views
{
    public partial class FrmCreateUser : Form
    {
        private AuthService _authService;
        public FrmCreateUser(AuthService authService)
        {
            _authService = authService;
            InitializeComponent();
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            if ((emailTb.Text != null && emailTb.Text != string.Empty) && (usernameTb.Text != null && usernameTb.Text != string.Empty))
            {
                if (Regex.IsMatch(emailTb.Text, "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$"))
                {
                    var rol = (Rol)rolCb.SelectedItem;
                    var result = _authService.CreateUser(new RegisterViewModel
                    {
                        Username = usernameTb.Text,
                        Email = emailTb.Text,
                        Rol = new List<string> { rol.name },
                        Password = Properties.Settings.Default.DEFAULT_PASSWORD,
                        ConfirmPassword = Properties.Settings.Default.DEFAULT_PASSWORD
                    });
                    if (result.IsSuccess)
                    {
                        MessageBox.Show(result.Message.ToString());
                        DialogResult = DialogResult.OK;
                    }
                    else 
                    {
                        MessageBox.Show(result.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("The email field is incorrectly formatted.");
                }
            }
            else 
            {
                MessageBox.Show("The username and the email fields are required.");
            }
        }

        private void FrmCreateUser_Load(object sender, EventArgs e)
        {
            rolBindingSource.DataSource = _authService.GetAllRoles();
        }
    }
}
