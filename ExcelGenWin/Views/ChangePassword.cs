using ABABillingAndClaim.Models;
using ABABillingAndClaim.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ABABillingAndClaim.Views
{
    public partial class ChangePassword : Form
    {
        private MemoryService _memoryService;
        private AuthService _authService;
        public ChangePassword(MemoryService memoryService, AuthService authService)
        {
            _memoryService = memoryService;
            _authService = authService;
            InitializeComponent();
        }

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            /** Fields Emptys **/
            if ((newPassTb.Text != null && newPassTb.Text != string.Empty) && (confirmPassTb.Text != null && confirmPassTb.Text != string.Empty))
            {
                /** Password match **/
                if (newPassTb.Text.Equals(confirmPassTb.Text))
                {
                    /** Length required is 5 characters **/
                    if (newPassTb.Text.Count() > 5)
                    {
                        /** The password must have numbers, capital letters and special characters **/
                        if (Regex.IsMatch(newPassTb.Text, @"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W)"))
                        {
                            var result = _authService.ChangePassword(new ResetPasswordViewModel() { NewPassword = newPassTb.Text.ToString(), Username = _memoryService.LoggedOndUser.username.ToString() });
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
                            MessageBox.Show("The password must have numbers, capital letters and special characters");
                        }
                    }
                    else
                    {
                        MessageBox.Show("The password length must be more than 5 characters.");
                    }
                }
                else
                {
                    MessageBox.Show("The passwords do not match.");
                }
            }
            else
            {
                MessageBox.Show("The new password and the password confirmation field are required");
            }
        }
    }
}
