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
    public partial class FrmListUser : Form
    {
        private AuthService _authService;
        private bool showed = false;
        public FrmListUser(AuthService authService)
        {
            _authService = authService;
            InitializeComponent();
        }
        private void FrmListUser_Load(object sender, EventArgs e) => refreshBindingSource();

        private void refreshBindingSource() => userBindingSource.DataSource = _authService.GetAllUsers();
        private void ListUserDG_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == ListUserDG.Columns["ResetPassword"].Index)
            {
                if (MessageBox.Show("You are trying to reset the password for this user, Are you sure?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var user = ListUserDG.SelectedRows[e.RowIndex].DataBoundItem as User;
                    var result = _authService.ChangePassword(new ResetPasswordViewModel()
                    {
                        NewPassword = Properties.Settings.Default.DEFAULT_PASSWORD,
                        Username = user.username
                    });
                    if (result.IsSuccess)
                    {
                        MessageBox.Show(result.Message.ToString());
                    }
                    else
                    {
                        MessageBox.Show(result.Message.ToString());
                    }
                }
            }
        }

        private void ListUserDG_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == ListUserDG.Columns["ResetPassword"].Index)
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

        private void ListUserDG_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
           

        }
        private void ListUserDG_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //if (showed)
            //{
                
            //}
        }

        private void FrmListUser_Shown(object sender, EventArgs e) => showed = true;

        private void ListUserDG_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("You are trying to delete this user, Are you sure?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
            else 
            {
                var user = ListUserDG.Rows[e.Row.Index].DataBoundItem as User;

                var result = _authService.DeleteUser(user.id);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message.ToString());
                }
                else
                {
                    MessageBox.Show(result.Message.ToString());
                }
            }
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            FrmCreateUser frm = new FrmCreateUser(_authService);
            if (frm.ShowDialog() == DialogResult.OK)
                refreshBindingSource();
        }
    }
}
