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
using ABABillingAndClaim.Models;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;

namespace ABABillingAndClaim.Utils
{
    public abstract class NavigatorAccessBase
    {
        public readonly Dictionary<string, string> ElementsID = new Dictionary<string, string>() {
            //Client
            { "RecipientID", "dnn_ClaimPhysicianInformation_ctl02_Datapanel_ctl00_ctl05_ClaimRecipient_ds_ClaimRecipient_mb_ds_ClaimRecipient"},
            { "RecipientID_Search", "dnn_ClaimPhysicianInformation_ctl02_Datapanel_ctl00_ctl05_ClaimRecipient_DataSearchButton"},
            { "PatientAccount", "dnn_ClaimPhysicianInformation_ctl02_Datapanel_ctl00_ctl09_PatientAccountNumber_mb_PatientAccountNumber"},
            { "ProviderID", "dnn_ClaimPhysicianInformation_ctl02_Datapanel_ctl00_ctl10_ReferringProvider1_ProviderID_mb_ReferringProvider1_ProviderID"},
            { "ProviderID_Search", "dnn_ClaimPhysicianInformation_ctl02_Datapanel_ctl00_ctl10_ReferringProvider1_ProviderIdentifierType_DataSearchButton"},
            { "PANumber", "dnn_ClaimPhysicianInformation_ctl02_Datapanel_ctl00_ctl09_PANumber_mb_PANumber"},               
            //Diagnosis
            { "Sequence", "dnn_ClaimPhysicianInformation_ctl02_Diagnosis_Datapanel_ctl00_ctl00_SequenceCode"},
            { "Diagnosis", "dnn_ClaimPhysicianInformation_ctl02_Diagnosis_Datapanel_ctl00_ctl00_Diagnosis_ds_Diagnosis_mb_ds_Diagnosis"},
            { "Diagnosis_Search", "dnn_ClaimPhysicianInformation_ctl02_Diagnosis_Datapanel_ctl00_ctl00_Diagnosis_DataSearchButton"},
            { "AddDiagnosis", "dnn_ClaimPhysicianInformation_ctl02_Diagnosis_Datapanel_ctl00_ctl01_NewButton"},
            //Details
            { "AddServiceLog","dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl18_NewButton"},
            { "DelServiceLog","dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl18_DeleteButton"},
            { "Provider", "dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl02_PerformingProvider_ProviderID_mb_PerformingProvider_ProviderID"},
            { "Provider_Search", "dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl02_PerformingProvider_DataSearchButton"},
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
            { "AllowedAmount", "dnn_ClaimPhysicianInformation_ctl02_Detail_Datapanel_ctl00_ctl14_AllowedAmount_mb_AllowedAmount"},
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

        public abstract Task<bool> UntilPageLoad(int TimeOut);

        public abstract Task LoadUrlAsync(string URL);

        public abstract Task DoRefreshAsync();

        public abstract Task DoForwardAsync();

        public abstract Task GoBackAsync();

        public abstract bool isLoaded { get; protected set; }

        public abstract string getTitle();

        protected abstract Task SetElement(string codeElem, string value, bool changeFocus = false, bool previousChangeFocus = false);

        protected abstract Task SetFocus(string codeElem, bool wait = false);

        protected abstract Task Invoke(string codeElem, string function);

        public virtual async Task LoadLoginInfo(string CompanyCode)
        {
            string title = getTitle();
            if (title.Contains("Iniciar") || title.Contains("Sign"))
            {
                var user = ConfigurationManager.AppSettings[$"web.{CompanyCode}.user"].ToString();
                var pass = ConfigurationManager.AppSettings[$"web.{CompanyCode}.password"].ToString();

                await SetElement("login", user, false, false);
                await SetElement("pass", pass, false, false);
            }
        }

        //public abstract Task InvokeChangeField(string codeElem);

        public virtual async Task LoadClientAndContractorInfo()
        {
            //limpiando valores
            //Invoke("CancelBtn", "click");
            //Thread.Sleep(3000);

            if (details != null)
            {
                //Recipiente ID
                await SetElement("RecipientID", details.RecipientID.ToString().Trim(), true);
                await SetFocus("RecipientID_Search", true);
                //Patient Account #
                await SetElement("PatientAccount", Regex.Replace(details.RecipientNUmber.ToString().Trim(), "[^a-zA-Z0-9]", String.Empty));
                //PA Number
                await SetElement("PANumber", details.AuthorizationNUmber.ToString().Trim());
                //Refering Provider
                await SetElement("ProviderID", details.ReferringPhysician.ToString().Trim(), true);
                await SetFocus("ProviderID_Search", true);

            }
        }

        public virtual async Task LoadDiagnosisInfo()
        {
            if (details != null)
            {
                //Add Diagnosis
                await Invoke("AddDiagnosis", "click()");
                //Sequence 
                await SetElement("Sequence", "1");
                //Diagnosis 
                //SetEnabled("Diagnosis", "");
                await SetElement("Diagnosis", Regex.Replace(details.Diagnosis.ToString().Trim(), "[^a-zA-Z0-9]", String.Empty), true);
                await SetFocus("Diagnosis_Search", true);
                //SetFocus("PANumber");
            }
        }

        public virtual async Task LoadServiceLogInfo()
        {
            if (details != null)
            {
                var first = true;
                foreach (DataRow it in details.table.Rows)
                {
                    if (!first)
                    {
                        await Invoke("AddServiceLog", "click()");
                    }
                    else first = false;
                    await SetElement("Provider", details.RenderingProvider.Trim(), true);
                    await SetFocus("Provider_Search", true);
                    await Task.Delay(100);
                    await SetElement("FromDOS", it["Day"].ToString().Trim(), false, true);
                    await SetElement("ToDOS", it["Day"].ToString().Trim());
                    await SetElement("POS", it["POS"].ToString().Trim());
                    await SetElement("Procedure", it["Procedure"].ToString().Trim().Substring(0, 5));
                    if (it["Procedure"].ToString().Length > 5)
                    {
                        await SetElement("Modifier1", it["Procedure"].ToString().Trim().Substring(5, 2));
                        await SetElement("DiagnosisPainter", "1");
                    }
                    else
                        await SetElement("DiagnosisPainter", "1");

                    await SetElement("Units", it["Unit"].ToString());
                    await SetElement("AllowedAmount", it["Procedure"].ToString().Trim().Contains("XP")
                                           ? "0.01"
                                           : ((int)it["Unit"] * (double)it["Rate"]).ToString().Trim().Replace(",", "."));
                    await SetElement("Charges", it["Procedure"].ToString().Trim().Contains("XP")
                                           ? "0.01"
                                           : ((int)it["Unit"] * (double)it["Rate"]).ToString().Trim().Replace(",", "."), true);

                    //SetFocus("Charges");
                    //SetFocus("PANumber");

                }
                if (!first)
                {
                    await Invoke("AddServiceLog", "click()");
                    //await Invoke("DelServiceLog", "click");
                }
            }
        }
    }
}
