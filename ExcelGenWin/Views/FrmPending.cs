﻿using ABABillingAndClaim.Services;
using ABABillingAndClaim.Utils;
using ClinicDOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABABillingAndClaim.Views
{
    public partial class FrmPending : Form
    {
        public TvObject nodeObj { get; set; }

        //public Clinic_AppContext db { get; set; }
        public FrmPending(TvObject nObj)
        {
            nodeObj = nObj;
            //db = vDb;
            InitializeComponent();
        }

        protected async Task<bool> setPendingServLog(TvServiceLog node)
        {
            try
            {
                await BillingService.Instance.SetServiceLogPendingReason(int.Parse(node.Id), textBox1.Text);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: Connectivity Error [{ex.Message}]");
                return false;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Update
            if (nodeObj is TvServiceLog)
            {
                if (!await setPendingServLog(nodeObj as TvServiceLog))
                {
                    DialogResult = DialogResult.None;
                }
            }
            else if (nodeObj is TvContractor)
            {
                foreach (var it in (nodeObj as TvContractor).ServiceLogs)
                {
                    if (!await setPendingServLog(it))
                    {
                        DialogResult = DialogResult.None;
                        break;
                    }
                }
            }
        }
    }
}
