﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ClinicDOM;
using System.ComponentModel.Design;
using System.Data;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Globalization;
using System.Configuration;
using System.Security.Principal;
using ABABillingAndClaim.Services;

namespace ABABillingAndClaim.Utils
{
    public class Details
    {
        //private readonly Clinic_AppContext db;
        public DataTable table;

        public string ClientName;
        public string RecipientID;
        public string ReferringPhysician;
        public string RecipientNUmber;
        public string Contractor;
        public string AuthorizationNUmber;
        public string Diagnosis;
        public string RBTUnitRate;
        public string AnalystUnitRate;
        public string RBTprovider;
        public string Analystprovider;
        public string Period;
        public string rango;
        public int WeeklyApprovedRBT;
        public int WeeklyApprovedAnalyst;
        public double tUnits;
        public string tHRS;
        public string tPaidRBT;
        public string tlPaidAnalyst;
        public double rates;
        public string tBilled;
        public string amountPaid;
        public string contractorType;
        public string RenderingProvider;
        public string Pending;
        public DateTime? BilledDate;
        public string Status;

        public Details(string theClientName, string theRecipientID, string theReferringPhysician, string theRecipientNUmber, string theContractor, string theAuthorizationNUmber, string theDiagnosis, string theRBTUnitRate,
                string theAnalystUnitRate, string theRBTprovider, string theAnalystprovider, string thePeriod, string therango, int theWeeklyApprovedRBT, int theWeeklyApprovedAnalyst, double thetUnits,
                string thetHRS, string thetPaidRBT, string thetlPaidAnalyst, double therates, string thetBilled, string theamountPaid, string thecontractorType, string therenderingProvider)
        {
            //db = vDb;

            ClientName = theClientName;
            RecipientID = theRecipientID;
            ReferringPhysician = theReferringPhysician;
            RecipientNUmber = theRecipientNUmber;
            Contractor = theContractor;
            AuthorizationNUmber = theAuthorizationNUmber;
            Diagnosis = theDiagnosis;
            RBTUnitRate = theRBTUnitRate;
            AnalystUnitRate = theAnalystUnitRate;
            Period = thePeriod;
            rango = therango;
            WeeklyApprovedRBT = theWeeklyApprovedRBT;
            WeeklyApprovedAnalyst = theWeeklyApprovedAnalyst;
            tUnits = thetUnits;
            tHRS = thetHRS;
            tPaidRBT = thetPaidRBT;
            tlPaidAnalyst = thetlPaidAnalyst;
            rates = therates;
            tBilled = thetBilled;
            amountPaid = theamountPaid;
            contractorType = thecontractorType;
            RenderingProvider = therenderingProvider;
        }
        public Details(string compClientID, int contractorID, int periodID, string companyCode)
        {
            var decSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            var otherSep = (decSep == "." ? "," : ".");
            var clientID = int.Parse(compClientID.Split('_')[0]);
            var pAccount = compClientID.Split('_')[1];

            //db = vDb;

            var period = BillingService.Instance.GetPeriodAsync(periodID).Result;

            var info = BillingService.Instance.GetAgreementAsync(companyCode, periodID, contractorID, clientID).Result;

            if (info != null)
            {
                ClientName = info.Client.Name;
                RecipientID = info.Client.RecipientID;
                ReferringPhysician = info.Client.ReferringProvider;
                RecipientNUmber = info.Client.PatientAccount;
                Contractor = info.Payroll.Contractor.Name;
                AuthorizationNUmber = pAccount; //info.Client.AuthorizationNUmber;
                Diagnosis = info.Client.Diagnosis.Name;
                WeeklyApprovedRBT = info.Client.WeeklyApprovedRBT;
                WeeklyApprovedAnalyst = info.Client.WeeklyApprovedAnalyst;
                contractorType = info.Payroll.ContractorType.Name;
                RBTprovider = info.Payroll.Contractor.RenderingProvider;
                Period = period.PayPeriod + " " + info.Company.Acronym;
                rango = $"{period.StartDate:MM/dd/yy} to {period.EndDate:MM/dd/yy}";
                RBTUnitRate = info.Payroll.Procedure.Rate.ToString();
                RenderingProvider = info.Payroll.Contractor.RenderingProvider;

                table = new DataTable();
                table.Columns.Add("Day");
                table.Columns.Add("DailyUnits", typeof(double));
                table.Columns.Add("Unit", typeof(int));
                table.Columns.Add("PosId", typeof(int));
                table.Columns.Add("POS");
                table.Columns.Add("Procedure");
                table.Columns.Add("Rate", typeof(double));

                var total = 0.0;

                var sufixList = ConfigurationManager.AppSettings["extra.procedure.list"].ToString() + ";";

                int billed = 0, pendent = 0, empty = 0;

                var task = BillingService.Instance.GetExUnitDetailsAsync(periodID, contractorID, clientID, pAccount, sufixList);

                foreach (var unitDet in task.Result)
                {
                    rates = unitDet.SubProcedure.Name.Contains("XP") ? 0 : info.RateEmployees;
                    var calc = (unitDet.SubProcedureId == 1) ? info.Payroll.Procedure.Rate : unitDet.SubProcedure.Rate;//double.Parse(unitDet.SubProcedure.Rate.Contains(otherSep) ? unitDet.SubProcedure.Rate.Replace(otherSep, decSep): unitDet.SubProcedure.Rate);
                    if (table.Rows.Count > 0 && 
                        (table.Rows[table.Rows.Count - 1]["Day"].ToString() == unitDet.DateOfService.ToString("MM/dd/yy")) &&
                        (table.Rows[table.Rows.Count - 1]["Procedure"].ToString() == ((unitDet.SubProcedureId == 1) ? info.Payroll.Procedure.Name : unitDet.SubProcedure.Name)))
                    {
                        table.Rows[table.Rows.Count - 1]["DailyUnits"] = (double)(table.Rows[table.Rows.Count - 1]["DailyUnits"]) + (unitDet.Unit / 4);
                        table.Rows[table.Rows.Count - 1]["Unit"] = (int)(table.Rows[table.Rows.Count - 1]["Unit"]) + unitDet.Unit;
                        //table.Rows[table.Rows.Count - 1]["Rate"] = (double)(table.Rows[table.Rows.Count - 1]["Rate"]) + calc;
                        if ((int)(table.Rows[table.Rows.Count - 1]["PosId"]) > unitDet.PlaceOfService.Id)
                        {
                            table.Rows[table.Rows.Count - 1]["PosId"] = unitDet.PlaceOfService.Id;
                            table.Rows[table.Rows.Count - 1]["POS"] = unitDet.PlaceOfService.Value;
                        }
                    }
                    else
                        table.Rows.Add(unitDet.DateOfService.ToString("MM/dd/yy"), unitDet.Unit / 4, unitDet.Unit, unitDet.PlaceOfService.Id, unitDet.PlaceOfService.Value, (unitDet.SubProcedureId == 1) ? info.Payroll.Procedure.Name : unitDet.SubProcedure.Name, calc);
                    
                    tUnits = tUnits + unitDet.Unit;
                    total += unitDet.Unit * calc;

                    if (unitDet.ServiceLog.BilledDate != null) billed++;
                    if (unitDet.ServiceLog.Pending != null &&
                        unitDet.ServiceLog.Pending != "") pendent++;
                    if (unitDet.ServiceLog.Pending == null ||
                        unitDet.ServiceLog.Pending == "") empty++;

                }

                if (billed > 0 && pendent == 0 && empty == 0) Status = "Billed completly";
                else if (billed == 0 && (pendent > 0 || empty > 0)) Status = "Pending completly";
                else Status = "Mixed Status";

                tHRS = "" + tUnits / 4;
                tlPaidAnalyst = "$" + total;
            }
        }

        public Details(/*Clinic_AppContext vDb, */int serviceLogId, string companyCode, string pAccount)
        {
            var decSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            var otherSep = (decSep == "." ? "," : ".");

            //db = vDb;

            var info = BillingService.Instance.GetExServiceLogAsync(companyCode, serviceLogId).Result;

            if (info != null)
            {
                ClientName = info.client.Name;
                RecipientID = info.client.RecipientID;
                ReferringPhysician = info.client.ReferringProvider;
                RecipientNUmber = info.client.PatientAccount;
                Contractor = info.contractor.Name;
                AuthorizationNUmber = pAccount;//info.cl.AuthorizationNUmber; //info.Client.AuthorizationNUmber;
                Diagnosis = info.diagnosis.Name;
                WeeklyApprovedRBT = info.client.WeeklyApprovedRBT;
                WeeklyApprovedAnalyst = info.client.WeeklyApprovedAnalyst;
                contractorType = info.contractorType.Name;
                RBTprovider = info.contractor.RenderingProvider;
                Period = info.period.PayPeriod + " " + companyCode;
                rango = $"{info.period.StartDate:MM/dd/yy} to {info.period.EndDate:MM/dd/yy}";
                RBTUnitRate = info.procedure.Rate.ToString();
                RenderingProvider = info.contractor.RenderingProvider;
                BilledDate = info.serviceLog.BilledDate;
                Pending = info.serviceLog.Pending;

                table = new DataTable();
                table.Columns.Add("Day");
                table.Columns.Add("DailyUnits", typeof(double));
                table.Columns.Add("Unit", typeof(int));
                table.Columns.Add("PosId", typeof(int));
                table.Columns.Add("POS");
                table.Columns.Add("Procedure");
                table.Columns.Add("Rate", typeof(double));

                var total = 0.0;

                var sufixList = ConfigurationManager.AppSettings["extra.procedure.list"].ToString() + ";";

                var tsk = BillingService.Instance.GetExUnitDetailsAsync(serviceLogId, pAccount, sufixList);

                tsk.Wait();

                foreach (var unitDet in tsk.Result)
                {
                    rates = unitDet.SubProcedure.Name.Contains("XP") ? 0 : info.agreement.RateEmployees;
                    var calc = (unitDet.SubProcedureId == 1) ? info.payroll.Procedure.Rate : unitDet.SubProcedure.Rate;//double.Parse(unitDet.SubProcedure.Rate.Contains(otherSep) ? unitDet.SubProcedure.Rate.Replace(otherSep, decSep): unitDet.SubProcedure.Rate);
                    if (table.Rows.Count > 0 &&
                        (table.Rows[table.Rows.Count - 1]["Day"].ToString() == unitDet.DateOfService.ToString("MM/dd/yy")) &&
                        (table.Rows[table.Rows.Count - 1]["Procedure"].ToString() == ((unitDet.SubProcedureId == 1) ? info.procedure.Name : unitDet.SubProcedure.Name)))
                    {
                        table.Rows[table.Rows.Count - 1]["DailyUnits"] = (double)(table.Rows[table.Rows.Count - 1]["DailyUnits"]) + (unitDet.Unit / 4);
                        table.Rows[table.Rows.Count - 1]["Unit"] = (int)(table.Rows[table.Rows.Count - 1]["Unit"]) + unitDet.Unit;
                        //table.Rows[table.Rows.Count - 1]["Rate"] = (double)(table.Rows[table.Rows.Count - 1]["Rate"]) + calc;
                        if ((int)(table.Rows[table.Rows.Count - 1]["PosId"]) > unitDet.PlaceOfService.Id)
                        {
                            table.Rows[table.Rows.Count - 1]["PosId"] = unitDet.PlaceOfService.Id;
                            table.Rows[table.Rows.Count - 1]["POS"] = unitDet.PlaceOfService.Value;
                        }
                    }
                    else
                        table.Rows.Add(unitDet.DateOfService.ToString("MM/dd/yy"), unitDet.Unit / 4, unitDet.Unit, unitDet.PlaceOfService.Id, unitDet.PlaceOfService.Value, (unitDet.SubProcedureId == 1) ? info.procedure.Name : unitDet.SubProcedure.Name, calc);

                    tUnits = tUnits + unitDet.Unit;
                    total += unitDet.Unit * calc;
                }

                if (BilledDate != null) Status = $"Billed on {BilledDate:MM/dd/yy HH:nn}";
                else if (Pending != null && Pending != "") Status = $"Pending for: {Pending}";
                else Status = "Pending.";

                tHRS = "" + tUnits / 4;
                tlPaidAnalyst = "$" + total;
            }
        }
    }
}
