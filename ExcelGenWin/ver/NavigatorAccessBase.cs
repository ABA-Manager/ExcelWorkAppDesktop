using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Configuration;
using ClinicDOM;
using System.Xml;

namespace ABABillingAndClaim
{
    public abstract class NavigatorAccessBase
    {
        public readonly Dictionary<string, string> ElementsID = new Dictionary<string, string>() {
                //Client
                { "RecipientID", "dnn_ClaimPhysicianInformation_ctl02_Datapanel_ctl00_ctl05_ClaimRecipient_ds_ClaimRecipient_mb_ds_ClaimRecipient"},
                { "PatientAccount", "dnn_ClaimPhysicianInformation_ctl02_Datapanel_ctl00_ctl09_PatientAccountNumber_mb_PatientAccountNumber"},
                { "ProviderID", "dnn_ClaimPhysicianInformation_ctl02_Datapanel_ctl00_ctl10_ReferringProvider1_ProviderID_mb_ReferringProvider1_ProviderID"},
                { "PANumber", "dnn_ClaimPhysicianInformation_ctl02_Datapanel_ctl00_ctl09_PANumber_mb_PANumber"},               
                //Diagnosis
                { "Sequence", "dnn_ClaimPhysicianInformation_ctl02_Diagnosis_Datapanel_ctl00_ctl00_SequenceCode"},
                { "Diagnosis", "dnn_ClaimPhysicianInformation_ctl02_Diagnosis_Datapanel_ctl00_ctl00_Diagnosis_ds_Diagnosis_mb_ds_Diagnosis"},
                 { "AddDiagnosis", "dnn_ClaimPhysicianInformation_ctl02_Diagnosis_Datapanel_ctl00_ctl01_NewButton"},
                //Details
                { "AddServiceLog","dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl18_NewButton"},
                { "Provider", "dnn$ClaimPhysicianInformation$ctl02$Detail$Datapanel$ctl00$ctl02$PerformingProvider_ProviderID$mb_PerformingProvider_ProviderID"},
                { "FromDOS", "dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl03_FirstServiceDate_mb_FirstServiceDate"},
                { "ToDOS", "dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl04_LastServiceDate_mb_LastServiceDate"},
                { "POS", "dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl05_PlaceOfService_ds_PlaceOfService_mb_ds_PlaceOfService"},
                { "Procedure", "dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl06_Procedure_ds_Procedure_mb_ds_Procedure"},
                { "Modifier1","dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl07_Modifier1_ds_Modifier1_mb_ds_Modifier1"},
                { "DiagnosisPainter", "dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl09_DiagnosisIndicator1"},
                { "Units", "dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl10_BilledQuantity_mb_BilledQuantity"},
                { "Charges", "dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl11_BilledAmount_mb_BilledAmount"},
                { "CancelBtn","dnn_ClaimPhysicianInformation_NavFooter_CancelButton"},
                { "tableUnitDetails","dnn_ClaimPhysicianInformation_ctl02_Detail_Datalist"},
                //login
                { "login","userNameInput"},
                { "pass","passwordInput"},
            };

        public Details details { get; set; }
        public NavigatorAccessBase(Details detail)
        {
            details = detail;
        }

        public NavigatorAccessBase()
        {
            details = null;
        }

        public abstract void LoadUrl(string URL);

        public abstract Task LoadUrlAsync(string URL);

        public abstract Task DoRefreshAsync();

        public abstract Task DoForwardAsync();

        public abstract Task GoBackAsync();

        public abstract bool isLoaded { get; protected set; }

        public abstract string getTitle();

        public abstract void SetElement(string codeElem, string value, bool changeFocus);

        protected abstract void Invoke(string codeElem, string function);

        protected abstract void SetFocus(string codeElem);

        protected abstract void SetEnabled(string codeElem, string enabled);

        public virtual void LoadClientInfo()
        {
            //limpiando valores
            //Invoke("CancelBtn", "click");
            //Thread.Sleep(3000);

            if (details != null)
            {
                //Recipiente ID
                SetElement("RecipientID", details.RecipientID.ToString(), true);
                SetFocus("RecipientID");
                SetFocus("PANumber");
                //Patient Account #
                SetElement("PatientAccount", details.RecipientNUmber.ToString(), false);
                //PA Number
                SetElement("PANumber", details.AuthorizationNUmber.ToString(), false);
            }
        }

        public virtual void LoadContractorInfo()
        {
            if (details != null)
            {
                //Refering Provider
                SetElement("ProviderID", details.ReferringPhysician.ToString(), true);
            }
        }
        public virtual void LoadServiceLogInfo()
        {
            if (details != null)
            {
                foreach (DataRow it in details.table.Rows)
                {
                    Invoke("AddServiceLog", "click");
                    SetElement("Provider", details.RenderingProvider, true);
                    SetElement("FromDOS", it["Day"].ToString(), true);
                    SetElement("ToDOS", it["Day"].ToString(), true);
                    SetElement("POS", it["POS"].ToString(), true);
                    SetElement("Procedure", it["Procedure"].ToString().Substring(0, 5), true);
                    if (it["Procedure"].ToString().Length > 5)
                    {
                        SetElement("Modifier1", it["Procedure"].ToString().Substring(5, 2), true);
                    }
                    SetElement("DiagnosisPainter", "1", false);
                    SetElement("Units", it["Unit"].ToString(), true);
                    SetElement("Charges", it["Procedure"].ToString().Contains("XP")
                                           ? "0.01"
                                           : it["Rate"].ToString().Replace(",", "."), true);
                    SetFocus("Charges");
                    SetFocus("PANumber");

                }
            }
        }
        public virtual void LoadDiagnosisInfo()
        {
            if (details != null)
            {
                //Add Diagnosis
                Invoke("AddDiagnosis", "click");                
                //Sequence 
                SetElement("Sequence", "1", false);
                //Diagnosis 
                //SetEnabled("Diagnosis", "");
                SetElement("Diagnosis", details.Diagnosis.ToString().Replace(".", ""), true);
                SetFocus("Diagnosis");
                SetFocus("PANumber");
            }
        }

    }
}
