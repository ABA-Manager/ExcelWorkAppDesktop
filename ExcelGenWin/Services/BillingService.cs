using ABABillingAndClaim.Models;
using ABABillingAndClaim.Utils;
using CefSharp.DevTools.DOM;
using ClinicDOM;
using ClinicDOM.DAO;
using ExcelGenLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ABABillingAndClaim.Services
{
    public class BillingService
    {
        public static BillingService Instance { get; set; }

        private Clinic_AppContext db;
        public BillingService(Clinic_AppContext db)
        {
            this.db = db;
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public async Task<List<ExtendedPeriod>> GetPeriodsAsync()
        {
            var periodQry = db.Period
                .Where(p => p.StartDate < DateTime.Now)
                .OrderByDescending(p => p.StartDate)
                .Select(p => new ExtendedPeriod { Id = p.Id, StartDate = p.StartDate, EndDate = p.EndDate, PayPeriod = p.PayPeriod });

            return await periodQry.ToListAsync();
        }

        public async Task<Period> GetPeriodAsync(int periodID)
        {
            var infoPeriod = (from p in db.Period
                             where p.Id == periodID
                             select p).Take(1);
            return await infoPeriod.SingleOrDefaultAsync();
        }


        public async Task<List<Company>> GetCompaniesAsync()
        {
            var companyQry = from c in db.Company
                             select c;

            return await companyQry.ToListAsync();
        }

        public async Task<List<TvClient>> GetContractorAndClientsAsync(string CompanyCode, int PeriodId)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var sufixList = ConfigurationManager.AppSettings["extra.procedure.list"].ToString() + ";";

            //SELECT*
            //FROM Agreement ag
            //INNER JOIN Client cl ON cl.Id = ag.ClientId
            //INNER JOIN PatientAccount pa ON pa.ClientId = cl.Id
            //INNER JOIN Payroll pr ON pr.Id = ag.PayrollId
            //INNER JOIN Contractor ct ON ct.Id = pr.ContractorId
            //INNER JOIN Company c ON c.Id = ag.CompanyId
            //INNER JOIN ServiceLog sl ON sl.ClientId = cl.Id AND sl.ContractorId = ct.Id AND sl.PeriodId = 21
            //INNER JOIN UnitDetail ud ON sl.Id = ud.ServiceLogId AND ud.DateOfService BETWEEN pa.CreateDate AND pa.ExpireDate
            //INNER JOIN SubProcedure sp ON sp.Id = ud.SubProcedureId AND
            //            ISNULL(CASE WHEN(sp.Name LIKE '%51' OR sp.Name LIKE '%51TS') 
            //                        THEN pa.Auxiliar ELSE pa.LicenseNumber END, 'DOES NOT APPLY') <> 'DOES NOT APPLY'


            var queryRes = await (from ag in db.Agreement
                                  join co in db.Company on new { ag.CompanyId, CompanyCode } equals new { CompanyId = co.Id, CompanyCode = co.Acronym }
                                  join pr in db.Payroll on ag.PayrollId equals pr.Id
                                  join ctt in db.ContractorType on pr.ContractorTypeId equals ctt.Id
                                  join ct in db.Contractor on pr.ContractorId equals ct.Id
                                  join cl in db.Client on ag.ClientId equals cl.Id
                                  join pa in db.PatientAccount on ag.ClientId equals pa.ClientId
                                  join sl in db.ServiceLog on new { ag.ClientId, pr.ContractorId, PeriodId } equals new { sl.ClientId, sl.ContractorId, sl.PeriodId }
                                  join ud in db.UnitDetail on sl.Id equals ud.ServiceLogId
                                  join sp in db.SubProcedure on ud.SubProcedureId equals sp.Id
                                  where pa.CreateDate <= ud.DateOfService && pa.ExpireDate >= ud.DateOfService
                                  && (((sufixList.Contains(sp.Name.Substring(3) + ";") ? pa.Auxiliar : pa.LicenseNumber) ?? "DOES NOT APPLY") != "DOES NOT APPLY")
                                  select new { cl, ct, ctt, pa, sl, ud, sp })
                                      .Distinct()
                                      .OrderBy(it => it.cl.Name.Trim())
                                      .ThenBy(it => it.cl.Id)
                                      .ThenBy(it => it.pa.Auxiliar != null ? it.pa.Auxiliar : it.pa.LicenseNumber)
                                      .ThenBy(it => it.ct.Name)
                                      .ToListAsync();

            TvClient lastClient = null;
            TvContractor lastContractor = null;
            TvServiceLog lastServiceLog = null;

            var clientList = new List<TvClient>();
            foreach (var it in queryRes)
            {
                if (it.cl != null && it.ct != null && it.sl != null)
                {
                    var paNum = it.pa.Auxiliar != null ? it.pa.Auxiliar : it.pa.LicenseNumber; //it.pa != null ? (sufixList.Contains(it.sp.Name.Substring(3) + ";") ? it.pa.Auxiliar : it.pa.LicenseNumber) : it.cl.AuthorizationNUmber;
                    if (it.cl.Id.ToString() + $"_{paNum}" != lastClient?.Id)
                    {
                        clientList.Add(lastClient = new TvClient()
                        {
                            Id = it.cl.Id.ToString() + $"_{paNum}",
                            Name = it.cl.Name.Trim() + $" ({paNum})",
                        });
                        lastContractor = null; lastServiceLog = null;
                    }
                    if (lastContractor == null || int.Parse(lastContractor.Id) != it.ct.Id)
                    {
                        lastClient.Contractors.Add(lastContractor = new TvContractor()
                        {
                            Id = it.ct.Id.ToString(),
                            Name = it.ct.Name.Trim(),
                            ContratorType = it.ctt.Name,
                            Client = lastClient
                        });
                        lastServiceLog = null;
                    }

                    if (lastServiceLog == null || int.Parse(lastServiceLog.Id) != it.sl.Id)
                        lastContractor.ServiceLogs.Add(lastServiceLog = new TvServiceLog()
                        {
                            Id = it.sl.Id.ToString(),
                            CreatedDate = it.sl.CreatedDate,
                            Status = (it.sl.BilledDate != null) ? "billed" : "empty",
                            Contractor = lastContractor
                        });
                }
            }
            db.Configuration.LazyLoadingEnabled = true;
            return clientList;
        }

        public async Task<Agreement> GetAgreementAsync(string companyCode, int periodID, int contractorID, int clientID) 
        {
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
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
                        .Include(y => y.Payroll.ContractorType).Take(1);

                return await infoQuery.SingleOrDefaultAsync();
            }
            finally { db.Configuration.LazyLoadingEnabled = true; }
        }

        public async Task<List<ExtendedUnitDetail>> GetExUnitDetailsAsync(int periodID, int contractorID, int clientID, string pAccount, string sufixList)
        {
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
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
                                  select new ExtendedUnitDetail(){ unitDetail = ud, serviceLog = slo, subProcedure = sp, placeOfService = ps, patientAccount = pa };

                return await infoUnitDet.ToListAsync();
            }
            finally { db.Configuration.LazyLoadingEnabled = true; }
        }

        public async Task<List<ExtendedUnitDetail>> GetExUnitDetailsAsync(int serviceLogId, string pAccount, string sufixList)
        {
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
                var infoUnitDet = from ud in db.UnitDetail
                                  join slo in db.ServiceLog on ud.ServiceLogId equals slo.Id
                                  join sp in db.SubProcedure on ud.SubProcedureId equals sp.Id
                                  join ps in db.PlaceOfService on ud.PlaceOfServiceId equals ps.Id
                                  join pa in db.PatientAccount on slo.ClientId equals pa.ClientId
                                  where pa.CreateDate <= ud.DateOfService && pa.ExpireDate >= ud.DateOfService
                                     && (pAccount == pa.Auxiliar ? sufixList.Contains(sp.Name.Substring(3) + ";") : false
                                      || pAccount == pa.LicenseNumber ? !sufixList.Contains(sp.Name.Substring(3) + ";") : false)
                                     && slo.Id == serviceLogId
                                  orderby ud.DateOfService
                                  select new ExtendedUnitDetail() { unitDetail = ud, serviceLog = slo, subProcedure = sp, placeOfService = ps, patientAccount = pa };

                return await infoUnitDet.ToListAsync();
            }
            finally { db.Configuration.LazyLoadingEnabled = true; }
        }

        public async Task<ExtendedServiceLog> GetExServiceLogAsync(string companyCode, int serviceLogId)
        {
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
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
                                 select new ExtendedServiceLog { serviceLog = sl, agreement = ag, client = cl, diagnosis = d, period = p, payroll = pr, procedure = pd, contractor = ct, contractorType = ctt }).Take(1);

                return await infoQuery.SingleOrDefaultAsync();
            } finally { db.Configuration.LazyLoadingEnabled = true; }
        }

        public async Task SetServiceLogBilled(string serviceLogId, string userId)
        {
            var servLog = await db.ServiceLog.SingleOrDefaultAsync(x => x.Id == serviceLogId);

            if (servLog == null)
            {
                servLog.BilledDate = DateTime.Now;
                servLog.Biller = userId;
                servLog.Pending = null;
                db.SaveChanges();
            }
        }

        public async Task SetServiceLogBilled(int periodId, int contratorId, int clientId, string userId)
        {
            var servLog = await db.ServiceLog.SingleOrDefaultAsync(x => (x.PeriodId == periodId && x.ContractorId == contratorId && x.ClientId == clientId));

            if (servLog == null)
            {
                servLog.BilledDate = DateTime.Now;
                servLog.Biller = userId;
                servLog.Pending = null;
                db.SaveChanges();
            }
        }

        public async Task SetServiceLogPendingReason(int serviceLogId, string reason)
        {
            var servLog = await db.ServiceLog.SingleOrDefaultAsync(x => x.Id == serviceLogId);

            if (servLog == null)
            {
                servLog.Pending = reason;
                db.SaveChanges();
            }
        }
    }
}
