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

        public Details(string theClientName, string theRecipientID, string theReferringPhysician, string theRecipientNUmber, string theContractor, string theAuthorizationNUmber, string theDiagnosis, string theRBTUnitRate,
                string theAnalystUnitRate, string theRBTprovider, string theAnalystprovider, string thePeriod, string therango, int theWeeklyApprovedRBT, int theWeeklyApprovedAnalyst, double thetUnits,
                string thetHRS, string thetPaidRBT, string thetlPaidAnalyst, double therates, string thetBilled, string theamountPaid, string thecontractorType, string therenderingProvider)
        {
            db = new Clinic_AppContext();

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

            db = new Clinic_AppContext();

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
                table.Columns.Add("DailyUnits");
                table.Columns.Add("Unit", typeof(int));
                table.Columns.Add("POS");
                table.Columns.Add("Procedure");
                table.Columns.Add("Rate", typeof(double));

                var total = 0.0;

                var infoUnitDet = (from ud in db.UnitDetail
                                   join slo in db.ServiceLog on ud.ServiceLogId equals slo.Id
                                   //join sp in db.SubProcedure on ud.SubProcedureId equals sp.Id
                                   //join ps in db.PlaceOfService on ud.PlaceOfServiceId equals ps.Id
                                   from pa in db.PatientAccount.Where(x => slo.ClientId == x.ClientId).Where(x => x.CreateDate <= ud.DateOfService && x.ExpireDate >= ud.DateOfService).DefaultIfEmpty()
                                   where (from sl in db.ServiceLog
                                          where sl.ClientId == clientID
                                             && sl.ContractorId == contractorID
                                             && sl.PeriodId == periodID
                                             && sl.Id == ud.ServiceLogId
                                          select 1).Any()
                                   orderby ud.DateOfService
                                   select ud)
                                  .Include(x => x.ServiceLog)
                                  .Include(x => x.SubProcedure)
                                  .Include(x => x.PlaceOfService);

                int billed = 0, pendent = 0, empty = 0;

                foreach (var unitDet in infoUnitDet.ToList())
                {
                    rates = unitDet.SubProcedure.Name.Contains("XP") ? 0 : info.RateEmployees;
                    var calc = (unitDet.SubProcedureId == 1) ? info.Payroll.Procedure.Rate : unitDet.SubProcedure.Rate;//double.Parse(unitDet.SubProcedure.Rate.Contains(otherSep) ? unitDet.SubProcedure.Rate.Replace(otherSep, decSep): unitDet.SubProcedure.Rate);
                    table.Rows.Add(unitDet.DateOfService.ToString("MM/dd/yy"), unitDet.Unit / 4, unitDet.Unit, unitDet.PlaceOfService.Value, (unitDet.SubProcedureId == 1) ? info.Payroll.Procedure.Name : unitDet.SubProcedure.Name, calc);
                    tUnits = tUnits + unitDet.Unit;
                    total += unitDet.Unit * calc;

                    if (unitDet.ServiceLog.BilledDate != null) billed++;
                    if (unitDet.ServiceLog.Pending != null && 
                        unitDet.ServiceLog.Pending != "") pendent++;
                    if (unitDet.ServiceLog.BilledDate == null &&
                        (unitDet.ServiceLog.Pending == null ||
                        unitDet.ServiceLog.Pending == "")) empty++;

                }

                if (billed > 0 && pendent == 0 && empty == 0) Status = "Billed completly";
                else if (billed == 0 && (pendent > 0 || empty > 0)) Status = "Pending completly";
                else Status = "Mixed Status";

                tHRS = "" + tUnits / 4;
                tlPaidAnalyst = "$" + total;
            }

            db.Configuration.LazyLoadingEnabled = true;
        }

        public Details(int serviceLogId, string companyCode)
        {
            var decSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            var otherSep = (decSep == "." ? "," : ".");

            db = new Clinic_AppContext();

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
                AuthorizationNUmber = info.cl.AuthorizationNUmber; //info.Client.AuthorizationNUmber;
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
                table.Columns.Add("DailyUnits");
                table.Columns.Add("Unit", typeof(int));
                table.Columns.Add("POS");
                table.Columns.Add("Procedure");
                table.Columns.Add("Rate", typeof(double));

                var total = 0.0;

                var infoUnitDet = (from ud in db.UnitDetail
                                   join slo in db.ServiceLog on ud.ServiceLogId equals slo.Id
                                   from pa in db.PatientAccount.Where(x => slo.ClientId == x.ClientId).Where(x => x.CreateDate <= ud.DateOfService && x.ExpireDate >= ud.DateOfService).DefaultIfEmpty()
                                   where slo.Id == serviceLogId
                                   orderby ud.DateOfService
                                   select ud)
                                  .Include(x => x.SubProcedure)
                                  .Include(x => x.PlaceOfService);

                foreach (var unitDet in infoUnitDet.ToList())
                {
                    rates = unitDet.SubProcedure.Name.Contains("XP") ? 0 : info.ag.RateEmployees;
                    var calc = (unitDet.SubProcedureId == 1) ? info.pr.Procedure.Rate : unitDet.SubProcedure.Rate;//double.Parse(unitDet.SubProcedure.Rate.Contains(otherSep) ? unitDet.SubProcedure.Rate.Replace(otherSep, decSep): unitDet.SubProcedure.Rate);
                    table.Rows.Add(unitDet.DateOfService.ToString("MM/dd/yy"), unitDet.Unit / 4, unitDet.Unit, unitDet.PlaceOfService.Value, (unitDet.SubProcedureId == 1) ? info.pd.Name : unitDet.SubProcedure.Name, calc);
                    tUnits = tUnits + unitDet.Unit;
                    total += unitDet.Unit * calc;
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
