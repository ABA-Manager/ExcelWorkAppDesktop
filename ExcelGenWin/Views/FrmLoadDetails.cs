using ABABillingAndClaim.Utils;
using ClinicDOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.Expressions;
using System.Windows.Forms;

namespace ABABillingAndClaim.Views
{
    public partial class FrmLoadDetails : Form
    {
        private readonly Details details;
        public FrmLoadDetails(TvContractor ct, int periodID, string companyCode)
        {
            InitializeComponent();

            details = new Details(ct.Client.Id, int.Parse(ct.Id), periodID, companyCode);

            clientname.Text = details.ClientName;
            recipientid.Text = details.RecipientID;
            referring.Text = details.ReferringPhysician;
            recipient.Text = details.RecipientNUmber;
            analRBT.Text = details.contractorType;
            authorization.Text =  details.AuthorizationNUmber;
            diagnosis.Text = details.Diagnosis;
            RBTUnitRate.Text = details.RBTUnitRate;            
            PP.Text = details.Period;
            rango.Text = details.rango;
            hrsRBT.Text = details.WeeklyApprovedRBT.ToString();
            hrsAnalyst.Text = details.WeeklyApprovedAnalyst.ToString();
            tUnits.Text = details.tUnits.ToString();
            tHRS.Text = details.tHRS;
            tlPaidAnalyst.Text = details.tlPaidAnalyst;
            tBilled.Text = tHRS.Text;
            amountPaid.Text = tlPaidAnalyst.Text;
            RBTprovider.Text = details.RBTprovider;
            providerID.Text = $" {details.contractorType} Provider ID";
            totalPaid.Text = $"Total Paid to {details.contractorType}";
            unitRate.Text = $"{details.contractorType} Unit Rate";
            analyst.Text = details.Contractor;
            dataGridView1.DataSource = details.table;
            tStatus.Text = details.Status;
        }

        public FrmLoadDetails(TvServiceLog sl, int periodID, string companyCode)
        {
            InitializeComponent();

            details = new Details(int.Parse(sl.Id), companyCode);

            clientname.Text = details.ClientName;
            recipientid.Text = details.RecipientID;
            referring.Text = details.ReferringPhysician;
            recipient.Text = details.RecipientNUmber;
            analRBT.Text = details.contractorType;
            authorization.Text = details.AuthorizationNUmber;
            diagnosis.Text = details.Diagnosis;
            RBTUnitRate.Text = details.RBTUnitRate;
            PP.Text = details.Period;
            rango.Text = details.rango;
            hrsRBT.Text = details.WeeklyApprovedRBT.ToString();
            hrsAnalyst.Text = details.WeeklyApprovedAnalyst.ToString();
            tUnits.Text = details.tUnits.ToString();
            tHRS.Text = details.tHRS;
            tlPaidAnalyst.Text = details.tlPaidAnalyst;
            tBilled.Text = tHRS.Text;
            amountPaid.Text = tlPaidAnalyst.Text;
            RBTprovider.Text = details.RBTprovider;
            providerID.Text = $" {details.contractorType} Provider ID";
            totalPaid.Text = $"Total Paid to {details.contractorType}";
            unitRate.Text = $"{details.contractorType} Unit Rate";
            analyst.Text = details.Contractor;
            dataGridView1.DataSource = details.table;
            tStatus.Text = details.Status;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          Close();
        }
    }
}
