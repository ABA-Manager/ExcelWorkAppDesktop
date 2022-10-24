using System;
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

namespace ABABillingAndClaim.Utils
{
    public class Details
    {
        private readonly Clinic_AppContext db;
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

        public Details(Clinic_AppContext vDb, string theClientName, string theRecipientID, string theReferringPhysician, string theRecipientNUmber, string theContractor, string theAuthorizationNUmber, string theDiagnosis, string theRBTUnitRate,
                string theAnalystUnitRate, string theRBTprovider, string theAnalystprovider, string thePeriod, string therango, int theWeeklyApprovedRBT, int theWeeklyApprovedAnalyst, double thetUnits,
                string thetHRS, string thetPaidRBT, string thetlPaidAnalyst, double therates, string thetBilled, string theamountPaid, string thecontractorType, string therenderingProvider)
        {
            db = vDb;

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
        public Details(Clinic_AppContext vDb, string compClientID, int contractorID, int periodID, string companyCode)
        {
            var decSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            var otherSep = (decSep == "." ? "," : ".");
            var clientID = int.Parse(compClientID.Split('_')[0]);
            var pAccount = compClientID.Split('_')[1];

            db = vDb;

            var infoPeriod = from p in db.Period
                             where p.Id == periodID
                             select p;
            var period = infoPeriod.First();

            db.Configuration.LazyLoadingEnabled = false;

            var infoQuery = (from ag in db.Agreement
                             where ag.Payroll.Contractor.ServiceLog.Any(x => x.PeriodId == periodID) &&
                                    ag.Company.Acronym == companyCode &&
                                    ag.Payroll.ContractorId == contractorID &&
                                    ag.ClientId == clientID
                             select ag)
                                .Include(y => y.Company)
                                .Include(y => y.Client.Diagnosis)
                                .Include(y => y.Payroll.Procedure)
                                .Include(y => y.Payroll.Contractor)
                                .Include(y => y.Payroll.ContractorType);

            var info = infoQuery.First();

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

                var infoUnitDet = from ud in db.UnitDetail
                                  join slo in db.ServiceLog on ud.ServiceLogId equals slo.Id
                                  join sp in db.SubProcedure on ud.SubProcedureId equals sp.Id
                                  join ps in db.PlaceOfService on ud.PlaceOfServiceId equals ps.Id
                                  join pa in db.PatientAccount on slo.ClientId equals pa.ClientId
                                  where pa.CreateDate <= ud.DateOfService && pa.ExpireDate >= ud.DateOfService
                                     && (pAccount == pa.Auxiliar ? sufixList.Contains(sp.Name.Substring(3) + ";") : false
                                      || pAccount == pa.LicenseNumber ? !sufixList.Contains(sp.Name.Substring(3) + ";") : false)
                                     && (from sl in db.ServiceLog
                                         where sl.ClientId == clientID
                                            && sl.ContractorId == contractorID
                                            && sl.PeriodId == periodID
                                            && sl.Id == ud.ServiceLogId
                                         select 1).Any()
                                  orderby new { ud.DateOfService, ud.SubProcedureId } 
                                  select new { ud, slo, sp, ps, pa };

                int billed = 0, pendent = 0, empty = 0;

                foreach (var unitDet in infoUnitDet.ToList())
                {
                    rates = unitDet.sp.Name.Contains("XP") ? 0 : info.RateEmployees;
                    var calc = (unitDet.ud.SubProcedureId == 1) ? info.Payroll.Procedure.Rate : unitDet.sp.Rate;//double.Parse(unitDet.SubProcedure.Rate.Contains(otherSep) ? unitDet.SubProcedure.Rate.Replace(otherSep, decSep): unitDet.SubProcedure.Rate);
                    if (table.Rows.Count > 0 && 
                        (table.Rows[table.Rows.Count - 1]["Day"].ToString() == unitDet.ud.DateOfService.ToString("MM/dd/yy")) &&
                        (table.Rows[table.Rows.Count - 1]["Procedure"].ToString() == ((unitDet.ud.SubProcedureId == 1) ? info.Payroll.Procedure.Name : unitDet.sp.Name)))
                    {
                        table.Rows[table.Rows.Count - 1]["DailyUnits"] = (double)(table.Rows[table.Rows.Count - 1]["DailyUnits"]) + (unitDet.ud.Unit / 4);
                        table.Rows[table.Rows.Count - 1]["Unit"] = (int)(table.Rows[table.Rows.Count - 1]["Unit"]) + unitDet.ud.Unit;
                        //table.Rows[table.Rows.Count - 1]["Rate"] = (double)(table.Rows[table.Rows.Count - 1]["Rate"]) + calc;
                        if ((int)(table.Rows[table.Rows.Count - 1]["PosId"]) > unitDet.ps.Id)
                        {
                            table.Rows[table.Rows.Count - 1]["PosId"] = unitDet.ps.Id;
                            table.Rows[table.Rows.Count - 1]["POS"] = unitDet.ps.Value;
                        }
                    }
                    else
                        table.Rows.Add(unitDet.ud.DateOfService.ToString("MM/dd/yy"), unitDet.ud.Unit / 4, unitDet.ud.Unit, unitDet.ps.Id, unitDet.ps.Value, (unitDet.ud.SubProcedureId == 1) ? info.Payroll.Procedure.Name : unitDet.sp.Name, calc);
                    
                    tUnits = tUnits + unitDet.ud.Unit;
                    total += unitDet.ud.Unit * calc;

                    if (unitDet.slo.BilledDate != null) billed++;
                    if (unitDet.slo.Pending != null &&
                        unitDet.slo.Pending != "") pendent++;
                    if (unitDet.slo.BilledDate == null &&
                        (unitDet.slo.Pending == null ||
                        unitDet.slo.Pending == "")) empty++;

                }

                if (billed > 0 && pendent == 0 && empty == 0) Status = "Billed completly";
                else if (billed == 0 && (pendent > 0 || empty > 0)) Status = "Pending completly";
                else Status = "Mixed Status";

                tHRS = "" + tUnits / 4;
                tlPaidAnalyst = "$" + total;
            }

            db.Configuration.LazyLoadingEnabled = true;
        }

        public Details(Clinic_AppContext vDb, int serviceLogId, string companyCode, string pAccount)
        {
            var decSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            var otherSep = (decSep == "." ? "," : ".");

            db = vDb;

            db.Configuration.LazyLoadingEnabled = false;

            var infoQuery = (from sl in db.ServiceLog
                             join ag in db.Agreement on new { sl.ClientId, sl.ContractorId, companyCode } equals new { ag.ClientId, ag.Payroll.ContractorId, companyCode = ag.Payroll.Company.Acronym }
                             join cl in db.Client on ag.ClientId equals cl.Id
                             join d in db.Diagnosis on cl.DiagnosisId equals d.Id
                             join p in db.Period on sl.PeriodId equals p.Id
                             join pr in db.Payroll on ag.PayrollId equals pr.Id
                             join pd in db.Procedure on pr.ProcedureId equals pd.Id
                             join ct in db.Contractor on pr.ContractorId equals ct.Id
                             join ctt in db.ContractorType on pr.ContractorTypeId equals ctt.Id
                             where sl.Id == serviceLogId
                             select new { sl, ag, cl, d, p, pr, pd, ct, ctt });

            var info = infoQuery.First();

            if (info != null)
            {
                ClientName = info.cl.Name;
                RecipientID = info.cl.RecipientID;
                ReferringPhysician = info.cl.ReferringProvider;
                RecipientNUmber = info.cl.PatientAccount;
                Contractor = info.ct.Name;
                AuthorizationNUmber = pAccount;//info.cl.AuthorizationNUmber; //info.Client.AuthorizationNUmber;
                Diagnosis = info.d.Name;
                WeeklyApprovedRBT = info.cl.WeeklyApprovedRBT;
                WeeklyApprovedAnalyst = info.cl.WeeklyApprovedAnalyst;
                contractorType = info.ctt.Name;
                RBTprovider = info.ct.RenderingProvider;
                Period = info.p.PayPeriod + " " + companyCode;
                rango = $"{info.p.StartDate:MM/dd/yy} to {info.p.EndDate:MM/dd/yy}";
                RBTUnitRate = info.pd.Rate.ToString();
                RenderingProvider = info.ct.RenderingProvider;
                BilledDate = info.sl.BilledDate;
                Pending = info.sl.Pending;

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

                var infoUnitDet = (from ud in db.UnitDetail
                                   join slo in db.ServiceLog on ud.ServiceLogId equals slo.Id
                                   join sp in db.SubProcedure on ud.SubProcedureId equals sp.Id
                                   join ps in db.PlaceOfService on ud.PlaceOfServiceId equals ps.Id
                                   join pa in db.PatientAccount on slo.ClientId equals pa.ClientId
                                   where pa.CreateDate <= ud.DateOfService && pa.ExpireDate >= ud.DateOfService
                                      && (pAccount == pa.Auxiliar ? sufixList.Contains(sp.Name.Substring(3) + ";") : false
                                       || pAccount == pa.LicenseNumber ? !sufixList.Contains(sp.Name.Substring(3) + ";") : false)
                                      && slo.Id == serviceLogId
                                   orderby ud.DateOfService
                                   select new { ud, slo, sp, ps, pa });

                foreach (var unitDet in infoUnitDet.ToList())
                {
                    rates = unitDet.sp.Name.Contains("XP") ? 0 : info.ag.RateEmployees;
                    var calc = (unitDet.ud.SubProcedureId == 1) ? info.pr.Procedure.Rate : unitDet.sp.Rate;//double.Parse(unitDet.SubProcedure.Rate.Contains(otherSep) ? unitDet.SubProcedure.Rate.Replace(otherSep, decSep): unitDet.SubProcedure.Rate);
                    if (table.Rows.Count > 0 &&
                        (table.Rows[table.Rows.Count - 1]["Day"].ToString() == unitDet.ud.DateOfService.ToString("MM/dd/yy")) &&
                        (table.Rows[table.Rows.Count - 1]["Procedure"].ToString() == ((unitDet.ud.SubProcedureId == 1) ? info.pd.Name : unitDet.sp.Name)))
                    {
                        table.Rows[table.Rows.Count - 1]["DailyUnits"] = (double)(table.Rows[table.Rows.Count - 1]["DailyUnits"]) + (unitDet.ud.Unit / 4);
                        table.Rows[table.Rows.Count - 1]["Unit"] = (int)(table.Rows[table.Rows.Count - 1]["Unit"]) + unitDet.ud.Unit;
                        //table.Rows[table.Rows.Count - 1]["Rate"] = (double)(table.Rows[table.Rows.Count - 1]["Rate"]) + calc;
                        if ((int)(table.Rows[table.Rows.Count - 1]["PosId"]) > unitDet.ps.Id)
                        {
                            table.Rows[table.Rows.Count - 1]["PosId"] = unitDet.ps.Id;
                            table.Rows[table.Rows.Count - 1]["POS"] = unitDet.ps.Value;
                        }
                    }
                    else
                        table.Rows.Add(unitDet.ud.DateOfService.ToString("MM/dd/yy"), unitDet.ud.Unit / 4, unitDet.ud.Unit, unitDet.ps.Id, unitDet.ps.Value, (unitDet.ud.SubProcedureId == 1) ? info.pd.Name : unitDet.sp.Name, calc);

                    tUnits = tUnits + unitDet.ud.Unit;
                    total += unitDet.ud.Unit * calc;
                }

                if (BilledDate != null) Status = $"Billed on {BilledDate:MM/dd/yy HH:nn}";
                else if (Pending != null && Pending != "") Status = $"Pending for: {Pending}";
                else Status = "Pending.";

                tHRS = "" + tUnits / 4;
                tlPaidAnalyst = "$" + total;
            }

            db.Configuration.LazyLoadingEnabled = true;
        }
    }
}
