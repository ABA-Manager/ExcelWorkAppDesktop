using System;
using System.Collections.Generic;
using System.Linq;
using ClinicDOM;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Data.Entity;
using System.Threading.Tasks;
using Action = System.Action;
using System.Runtime.CompilerServices;
//using DocumentFormat.OpenXML;

namespace ExcelGenLib
{
    public class ExcelGenerator
    {
        public enum OutputType
        {
            Web,
            Console,
            WinForm
        }
        //[DllImport("user32.dll")]
        //static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);

        //private void CloseExcelProcess(Application excelApp)
        //{
        //    int id;
        //    GetWindowThreadProcessId(excelApp.Hwnd,out id);
        //    Process.GetProcessById(id).Kill();
        //}

        //private readonly Clinic_AppContext db;
        private ExtendedPeriod period;
        private string TempDirectory;
        private Application app;
        private System.Windows.Forms.TextBox tbProcessLog;
        private System.Windows.Forms.ToolStripProgressBar pbProcessLog;
        public List<Company> CompanyData;
        public Dictionary<string, Dictionary<string, SummaryItem>> summaries;
        public Dictionary<string, SortedDictionary<string, double>> fullPayroll;

        public async Task SetPeriod(int PeriodId = -1)
        {
            period = await ExcelGenService.Instance.GetPeriodAsync(PeriodId);

            if (period == null)
                throw new Exception("Period not found or found more than one");            
        }

        public int GetPeriodId()
        {
            return period.Id;
        }

        public ExcelGenerator()
        {
            CompanyData = ExcelGenService.Instance.GetCompanies().Result;
        }

        public async Task GetExcelApp()
        {
            await Task.Run(() => app = new Application());
        }

        public ExcelGenerator(System.Windows.Forms.TextBox tbLog = null, System.Windows.Forms.ToolStripProgressBar pbProgress = null) : this()
        {
            tbProcessLog = tbLog;
            pbProcessLog = pbProgress;

            string path = Path.GetRandomFileName();
            Directory.CreateDirectory(TempDirectory = Path.Combine(Path.GetTempPath(), path));
        }



        public void CloseAll()
        {
            //db.Dispose();
            app?.Quit();
            //Delete Temporary Folder: Must be cleaned before
            //CloseExcelProcess(app);
        }

        public async Task<ExcelResponse> GenBillingAsync(string zipFile, string company = "", string password = "", OutputType outputType = OutputType.Console, bool verbose = true)
        {
            var response = new ExcelResponse(zipFile);
            ExcelFileData analyst;
            //var hasData = false;
            summaries = new Dictionary<string, Dictionary<string, SummaryItem>>();
            fullPayroll = new Dictionary<string, SortedDictionary<string, double>>();

            var contList = await ExcelGenService.Instance.GetExContractorsAsync(company);

            var pos = 0;
            
            if (pbProcessLog != null)
            {
                pbProcessLog.Maximum = contList.Count() + ((company == "" ? CompanyData.Count : 1) * 2);
                pbProcessLog.Value = 0;
            }

            foreach (var item in contList)
            {
                if ((analyst = await GenBillingAsync(item.contractorId, item.companyId, password, outputType, verbose)).Name != "")
                    response.AddAnalyst(analyst.Name, analyst.fileInfo);
                pos++;
                if (pbProcessLog != null) pbProcessLog.PerformStep();
            }

            foreach (var summary in summaries)
            {
                PrintLogOutput(verbose, outputType, "Generating {0} Summary for {1} analyst", summary.Key, summary.Value.Count);
                if (summary.Value.Count > 0)
                {
                    response.AddAnalyst(GenSummary(summary.Key, summary.Value, password, false));
                    //hasData = true;
                }
                if (pbProcessLog != null) pbProcessLog.PerformStep();
            }

            foreach (var payroll in fullPayroll)
            {
                PrintLogOutput(verbose, outputType, "Generating {0} Payroll for {1} contractors", payroll.Key, payroll.Value.Count);
                if (payroll.Value.Count > 0)
                {
                    response.AddAnalyst(GenPayroll(payroll.Key, payroll.Value, password, false));
                    //hasData = true;
                }
                if (pbProcessLog != null) pbProcessLog.PerformStep();
            }
            response.SavePack();
            return response;
        }

        public void PrintLogOutput(bool verbose, OutputType outputType, string format, params object[] arg)
        {
            if (verbose)
                switch (outputType)
                {
                    case OutputType.Console:
                        Console.WriteLine(format, arg);
                        break;
                    case OutputType.WinForm:
                        if (tbProcessLog != null)
                            tbProcessLog.AppendText(string.Format(format + "\r\n", arg));
                        break;
                    default:
                        break;
                }
        }

        public async Task<ExcelFileData> GenBillingAsync(int ContractorId, int CompanyId, string password = "", OutputType outputType = OutputType.Console, bool verbose = false)
        {
            var AnalystProc = new Dictionary<string, List<string>>();

            var billingSummary = new BillingSummary();

            var agreements = await ExcelGenService.Instance.GetAgreementsAsync(ContractorId, CompanyId);

            var analyst = agreements.First().Payroll;

            billingSummary.Analyst = analyst.Contractor.Name;
            billingSummary.AnalystExtra = analyst.Contractor.Name + ((analyst.Contractor.Extra != "") ? " (" + analyst.Contractor.Extra + ")" : "");

            var wb = app.Workbooks.Open(Path.Combine(outputType == OutputType.Web ? AppDomain.CurrentDomain.BaseDirectory : Directory.GetCurrentDirectory(), "BillingTemplate.xlsx"), 0, false);

            wb.Worksheets[1].Range["B1:C1"] = $"{period.StartDate:MM/dd/yy} to {period.EndDate:MM/dd/yy}";
            wb.Worksheets[1].Range["B2:C2"] = $"{period.EndDate:MM/dd/yy}";

            var lastSheet = wb.Worksheets[2];

            if (analyst.ContractorTypeId != 1) 
                throw new Exception($"Analyst Contractor not found ({analyst.Contractor.Name})");

            //Aqui se crea el Excel con el nombre asociado al nombre del Analista
            //wb.Worksheets.Add(lastSheet);
            //wb.Worksheets[1].Name = "Summary";
            PrintLogOutput(verbose, outputType, "Source Analyst: {0}\n\tfrom: {1:MM/dd/yy} to: {2:MM/dd/yy}", analyst.Contractor.Name, period.StartDate, period.EndDate);
            int CurrentSheetValue = 0, CurrentRow = 3, SheetPos = 1;
            Company co = null;
            bool hasData = false;

            foreach (var agreement in agreements)
            {
                string LastClient = "", LastContractor = "", LastSubProc = "", LastExtra = "";
                int LastSubProcId = -1;
                co = agreement.Company;

                if (!summaries.ContainsKey(co.Acronym))
                {
                    summaries.Add(co.Acronym, new Dictionary<string, SummaryItem>());
                    fullPayroll.Add(co.Acronym, new SortedDictionary<string, double>());
                }

                var unitDetList = await ExcelGenService.Instance.GetExAgrUnitDetails(agreement.ClientId, analyst.ContractorId, CompanyId, period.Id);
                dynamic ws = null;
                foreach (var unitDet in unitDetList)
                {

                    if (unitDet.serviceLog.Client.Name != LastClient)
                    {
                        //Aqui se define el nombre de la siguiente camada de worksheet asociada al Cliente 
                        CurrentSheetValue = 0;
                        LastClient = unitDet.serviceLog.Client.Name;
                        PrintLogOutput(verbose, outputType, "\tClient: {0}", LastClient);

                        //if (!billingSummary.clientSummary.ContainsKey(LastClient)) 
                        //    billingSummary.clientSummary.Add(LastClient, new ClientData("",""));
                    }
                    if (unitDet.serviceLog.Contractor.Name + ((unitDet.unitDetail.SubProcedureId == 1)?"":"-"+ unitDet.unitDetail.SubProcedure.Name) != 
                        LastContractor + ((LastSubProcId == 1) ? "" : "-" + LastSubProc))
                    {
                        if (LastContractor != "")
                        {
                            if (!fullPayroll[co.Acronym].ContainsKey(LastContractor + "|" + LastExtra))
                                fullPayroll[co.Acronym].Add(LastContractor + "|" + LastExtra, (double)wb.Worksheets[SheetPos].Names("TotalPaid").RefersToRange.Value);
                            else
                                fullPayroll[co.Acronym][LastContractor + "|" + LastExtra] += (double)wb.Worksheets[SheetPos].Names("TotalPaid").RefersToRange.Value;
                        }
                        
                        //Aqui se crea el worksheet con el nombre del cliente mas un contador
                        CurrentRow = 11;
                        SheetPos++;
                        lastSheet.Copy(lastSheet);
                        ws = wb.Worksheets[SheetPos];
                        CurrentSheetValue++;
                        ws.Name = LastClient + (CurrentSheetValue > 1 ? " (" + CurrentSheetValue.ToString().Trim() + ")" : "");
                        if (unitDet.agreement.Payroll.ContractorTypeId == 1)
                        {
                            if (billingSummary.clientSummary.ContainsKey(LastClient))
                            {
                                billingSummary.clientSummary[LastClient].AmountFormule += string.Format("+'{0}'!PaidAnalyst", ws.Name);
                                billingSummary.clientSummary[LastClient].HoursFormule += string.Format("+'{0}'!HoursAnalyst", ws.Name);
                            }
                            else
                                billingSummary.clientSummary.Add(LastClient, new ClientData(string.Format("'{0}'!PaidAnalyst", ws.Name), string.Format("'{0}'!HoursAnalyst", wb.Worksheets[SheetPos].Name)));
                        }
                        FillBillingSheet(ws, unitDet.serviceLog, unitDet.agreement, unitDet.unitDetail);

                        LastContractor = unitDet.serviceLog.Contractor.Name;
                        LastSubProc = unitDet.unitDetail.SubProcedure.Name;
                        LastSubProcId = unitDet.unitDetail.SubProcedureId;
                        LastExtra = unitDet.serviceLog.Contractor.Extra;
                        PrintLogOutput(verbose, outputType, "\t\tContractor: {0} ({1})", LastContractor, unitDet.agreement.Payroll.ContractorType.Name);
                        var fullContractor = LastContractor + ((unitDet.serviceLog.Contractor.Extra != "") ? " (" + unitDet.serviceLog.Contractor.Extra + ")" : "");
                        if (unitDet.agreement.Payroll.ContractorTypeId != 1)
                        {
                            if (billingSummary.contractorSummary.ContainsKey(fullContractor))
                                billingSummary.contractorSummary[fullContractor] += string.Format("+'{0}'!PaidRBT", ws.Name);
                            else
                                billingSummary.contractorSummary.Add(fullContractor, string.Format("'{0}'!PaidRBT", ws.Name));
                        }
                        billingSummary.BilledMedicaidFormule += ((billingSummary.BilledMedicaidFormule == "") ? "" : "+") + string.Format("'{0}'!TotalBilled", ws.Name);
                        billingSummary.PaidContractorFormule += ((billingSummary.PaidContractorFormule == "") ? "" : "+") + string.Format("'{0}'!TotalPaid", ws.Name);
                    }
                    //Aqui se adicionan las filas en el worksheet
                    ws.Cells[CurrentRow, "A"] = $"{unitDet.unitDetail.DateOfService:MM/dd/yy}";
                    ws.Cells[CurrentRow, (unitDet.agreement.Payroll.ContractorTypeId == 1 ? "D" : "B")] = unitDet.unitDetail.Unit;
                    ws.Cells[CurrentRow, "G"] = unitDet.unitDetail.PlaceOfService.Value;
                    //ws.Cells[CurrentRow, "H"] = unitDet.unitDetail.SubProcedure.Name;
                    CurrentRow++;
                    ws.Rows[CurrentRow].Insert();
                    ws.Cells[CurrentRow, "C"] = $"=B{CurrentRow}*$C$7";
                    ws.Cells[CurrentRow, "E"] = $"=$F$7*D{CurrentRow}";
                    ws.Cells[CurrentRow, "F"] = $"=D{CurrentRow}+B{CurrentRow}";

                    PrintLogOutput(verbose, outputType, "\t\t\t{0} \t{1} \t{2}", unitDet.unitDetail.DateOfService.ToShortDateString(), unitDet.unitDetail.Unit, unitDet.unitDetail.PlaceOfService.Name);
                }

                if (unitDetList.Count() > 0)
                {
                    hasData = true;
                    if (!fullPayroll[co.Acronym].ContainsKey($"{LastContractor}|{LastExtra}"))
                        fullPayroll[co.Acronym].Add($"{LastContractor}|{LastExtra}", (double)wb.Worksheets[SheetPos].Names("TotalPaid").RefersToRange.Value);
                    else
                        fullPayroll[co.Acronym][$"{LastContractor}|{LastExtra}"] += (double)wb.Worksheets[SheetPos].Names("TotalPaid").RefersToRange.Value;
                }
            }

            if (co != null && hasData)
            {
                wb.Worksheets[1].Range["A1:A1"] = $"{period.PayPeriod} {co.Acronym}";
                summaries[co.Acronym].Add($"{analyst.Contractor.Name} ({co.Acronym})", LoadAndGetSummary(wb.Worksheets[1], billingSummary));
            }

            app.DisplayAlerts = false;
            lastSheet.Delete();
            var filename = $"{TempDirectory}\\ABA Billing-{analyst.Contractor.Name}-{period.PayPeriod} {co.Acronym}-{period.StartDate:MM.dd.yy} to {period.EndDate:MM.dd.yy}.xlsx";
            if (hasData)
                wb.SaveAs(filename, Type.Missing, password);
            wb.Close(0);
            app.DisplayAlerts = true;

            return new ExcelFileData(analyst.Contractor.Name, (hasData ? new FileInfo(filename) : null));
        }

        private SummaryItem LoadAndGetSummary(dynamic ws, BillingSummary billingSummary)
        {
            var clientInsertPos = 15;
            var contractorInsertPos = 8;

            ws.Cells[7, "A"] = billingSummary.AnalystExtra;
            ws.Cells[13, "A"] = billingSummary.Analyst;

            ws.Cells[5, "C"] = "=" + billingSummary.BilledMedicaidFormule;
            ws.Cells[5, "D"] = "=" + billingSummary.PaidContractorFormule;

            foreach (var it in billingSummary.clientSummary)
            {
                ws.Rows[clientInsertPos++].Insert();
                ws.Cells[clientInsertPos - 2, "A"] = it.Key;
                ws.Cells[clientInsertPos - 2, "B"] = "=" + it.Value.AmountFormule;
                ws.Cells[clientInsertPos - 2, "C"] = "=" + it.Value.HoursFormule;
            }
            foreach (var it in billingSummary.contractorSummary)
            {
                ws.Rows[contractorInsertPos++].Insert();
                ws.Cells[contractorInsertPos - 1, "A"] = it.Key;
                ws.Cells[contractorInsertPos - 1, "B"] = "=" + it.Value;
            }
            return new SummaryItem(ws.Cells[5, "C"].Value, ws.Cells[5, "D"].Value);
        }

        private void FillBillingSheet(dynamic ws, ServiceLog sl, Agreement ag, UnitDetail ud)
        {
            //Encabezado
            ws.Range["B1"] = sl.Client.Name;
            ws.Range["B2"] = sl.Client.RecipientID;
            ws.Range["B3"] = sl.Client.ReferringProvider;
            ws.Range["B4"] = sl.Client.PatientAccount;
            ws.Range["B5"] = (ag.Payroll.ContractorTypeId == 1 ? sl.Contractor.Name : "");
            ws.Range["B6"] = (ag.Payroll.ContractorTypeId == 2 ? sl.Contractor.Name : "");
            ws.Range["D9"] = sl.Client.WeeklyApprovedAnalyst;
            ws.Range["B9"] = sl.Client.WeeklyApprovedRBT;
            ws.Range["E2"] = period.PayPeriod + " " + ag.Company.Acronym;
            ws.Range["F4"] = sl.Client.AuthorizationNUmber;
            ws.Range["F5"] = sl.Client.Diagnosis.Name;

            if (ag.Payroll.ContractorTypeId == 1)
            {
                ws.Range["F6"] = (ud.SubProcedureId != 1) ? ud.SubProcedure.Name : ag.Payroll.Procedure.Name;
                ws.Range["F7"] = (ud.SubProcedureId != 1) ? ud.SubProcedure.Rate : ag.Payroll.Procedure.Rate;
                ws.Range["F8"] = ag.Payroll.Contractor.RenderingProvider;
                ws.Names("RatesEmployeesAnalyst").RefersToRange = ud.SubProcedure.Name.Contains("XP") ? 0 : ag.RateEmployees;
            }
            else
            {
                ws.Range["C6"] = (ud.SubProcedureId != 1) ? ud.SubProcedure.Name : ag.Payroll.Procedure.Name;
                ws.Range["C7"] = (ud.SubProcedureId != 1) ? ud.SubProcedure.Rate : ag.Payroll.Procedure.Rate;
                ws.Range["C8"] = ag.Payroll.Contractor.RenderingProvider;
                ws.Names("RatesEmployeesRBT").RefersToRange = ud.SubProcedure.Name.Contains("XP") ? 0 : ag.RateEmployees;
            }
            //Periodo Completo
            //for (var i = 0; i < 14; i++)
            //{
            //    var fecha = period.StartDate.AddDays(i);
            //    ws.Cells[11 + i, "A"] = fecha.ToString("yyyy/MM/dd");
            //}
        }

        public ExcelFileData GenSummary(string co, Dictionary<string, SummaryItem> summary, string password, bool verbose)
        {
            var filename = Path.Combine(TempDirectory, $"Summary-{period.PayPeriod} {co}-{period.StartDate:MM.dd.yy} to {period.EndDate:MM.dd.yy}.xlsx");
            var wb = app.Workbooks.Add();
            var ws = wb.Worksheets[1];
            ws.Name = "Summary";

            ws.Range["A2"] = period.PayPeriod + " " + co;
            ws.Range["A2"].Font.Bold = true;
            ws.Range["A2"].Font.Size = 16;

            ws.Columns["A:A"].ColumnWidth = 32;
            ws.Columns["B:D"].ColumnWidth = 13;
            ws.Rows["2:2"].RowHeight = 21;
            ws.Rows["4:4"].RowHeight = 32;

            ws.Cells[4, "A"] = "Analyst / Contractor";
            ws.Cells[4, "B"] = "Billed To Medicaid";
            ws.Cells[4, "C"] = "Amount Paid to Contractor";
            ws.Cells[4, "D"] = "Profit";
            ws.Range["A4:D4"].Font.Bold = true;
            ws.Range["A4:D4"].Interior.Color = XlRgbColor.rgbLightSteelBlue;
            ws.Range["A4:D4"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            ws.Range["A4:D4"].VerticalAlignment = XlVAlign.xlVAlignCenter;
            ws.Range["A4:D4"].WrapText = true;
            ws.Range["A4:D4"].BorderAround(Type.Missing, XlBorderWeight.xlMedium);

            int row = 5;
            foreach (var item in summary)
            {
                ws.Cells[row, "A"] = item.Key;
                ws.Cells[row, "B"] = item.Value.Medicaid;
                ws.Cells[row, "C"] = item.Value.Paid;
                ws.Cells[row, "D"] = "=B" + row.ToString().Trim() + "-C" + row.ToString().Trim();
                row++;
            }
            row++;
            ws.Cells[row, "A"] = "Assessment";
            ws.Cells[row, "D"] = "=B" + row.ToString().Trim() + "-C" + row.ToString().Trim();
            row++;
            row++;
            ws.Cells[row, "A"] = "Adjustment (-)";
            row++;
            ws.Cells[row, "D"] = "=B" + row.ToString().Trim() + "-C" + row.ToString().Trim();
            row++;
            ws.Cells[row, "A"] = "TOTAL";
            ws.Cells[row, "B"] = "=SUM(B5:B" + (row - 1).ToString().Trim() + ")";
            ws.Cells[row, "C"] = "=SUM(C5:C" + (row - 1).ToString().Trim() + ")";
            ws.Cells[row, "D"] = "=SUM(D5:D" + (row - 1).ToString().Trim() + ")";
            row++;
            ws.Cells[row, "D"] = "=D" + (row - 1).ToString().Trim() + "/B" + (row - 1).ToString().Trim();


            ws.Range["A4:A" + (row - 1).ToString().Trim()].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            ws.Range["B4:B" + (row - 1).ToString().Trim()].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            ws.Range["C4:C" + (row - 1).ToString().Trim()].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            ws.Range["D4:D" + (row - 1).ToString().Trim()].BorderAround(Type.Missing, XlBorderWeight.xlMedium);


            ws.Range["A4:D" + row.ToString().Trim()].Font.Size = 12;
            ws.Range["B5:D" + (row - 1).ToString().Trim()].Style = "Currency";
            ws.Range["A" + (row - 1).ToString().Trim() + ":D" + (row - 1).ToString().Trim()].Interior.Color = XlRgbColor.rgbLightGreen;
            ws.Range["A" + (row - 1).ToString().Trim() + ":D" + (row - 1).ToString().Trim()].Font.Bold = true;
            ws.Range["A" + (row - 1).ToString().Trim() + ":D" + (row - 1).ToString().Trim()].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            ws.Cells[row, "D"].Style = "Percent";
            ws.Cells[row, "D"].NumberFormat = "0.00%";
            ws.Cells[row, "D"].Interior.Color = XlRgbColor.rgbRed;
            ws.Cells[row, "D"].BorderAround(Type.Missing, XlBorderWeight.xlMedium);

            //wb.Worksheets[2].Range["B3"].Font.Bold = true;
            //wb.Worksheets[2].Range["C2", "D2"].Merge();
            //wb.Worksheets[2].Range["B2", "D4"].Borders.Weight = XlBorderWeight.xlThin;
            //wb.Worksheets[2].Range["B2", "D4"].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            //wb.Worksheets[2].Range["B2", "D4"].Interior.Color = XlRgbColor.rgbLightGray;

            wb.SaveAs(filename, Type.Missing, password);
            wb.Close();

            return new ExcelFileData("Summary (" + co + ")", new FileInfo(filename));
        }
        public ExcelFileData GenPayroll(string co, SortedDictionary<string, double> payroll, string password, bool verbose)
        {
            var filename = Path.Combine(TempDirectory, $"Payroll-{period.PayPeriod} {co}-{period.StartDate:MM.dd.yy} to {period.EndDate:MM.dd.yy}.xlsx");
            var wb = app.Workbooks.Add();
            var ws = wb.Worksheets[1];
            ws.Name = "Payroll";

            ws.Range["A1", "C1"].Merge();
            ws.Range["A1"] = $"{CompanyData.Where(x => x.Acronym == co).Take(1).Single().Name.ToUpper()} PAYROLL from {period.StartDate:MM.dd.yy} to {period.EndDate:MM.dd.yy} {period.PayPeriod}";
            ws.Range["A1"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            ws.Range["A1"].Font.Bold = true;
            ws.Range["A1"].Font.Size = 12;
            ws.Range["A1:C1"].Interior.Color = XlRgbColor.rgbPaleGreen;

            ws.Columns["A:A"].ColumnWidth = 26;
            ws.Columns["B:B"].ColumnWidth = 43;
            ws.Columns["C:C"].ColumnWidth = 22;


            ws.Cells[2, "A"] = "Employee Name";
            ws.Cells[2, "B"] = "Direct Deposit To";
            ws.Cells[2, "C"] = "Check Amount";
            ws.Range["A2:C2"].Interior.Color = XlRgbColor.rgbLightGrey;

            int row = 3;
            foreach (var item in payroll)
            {
                ws.Cells[row, "A"] = item.Key.Split('|')[0];
                ws.Cells[row, "B"] = item.Key.Split('|')[1];
                ws.Cells[row, "C"] = item.Value;
                row++;
            }
            ws.Cells[row, "B"] = "Total Paid";
            ws.Cells[row, "B"].HorizontalAlignment = XlHAlign.xlHAlignRight;
            ws.Cells[row, "C"] = "=SUM(C3:C" + (row - 1).ToString().Trim() + ")";
            ws.Range["C3:C" + row.ToString().Trim()].Style = "Currency";
            ws.Range["A2:C" + row.ToString().Trim()].Font.Bold = true;
            ws.Range["A1:C" + (row - 1).ToString().Trim()].Borders.Weight = XlBorderWeight.xlThin;
            ws.Range["A2:A" + (row - 1).ToString().Trim()].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            ws.Range["B2:B" + (row).ToString().Trim()].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            ws.Range["C2:C" + (row).ToString().Trim()].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            ws.Range["B" + (row).ToString().Trim() + ":C" + (row).ToString().Trim()].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            ws.Range["A1:C1"].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            ws.Range["A2:C2"].BorderAround(Type.Missing, XlBorderWeight.xlMedium);
            ws.Range["B" + (row).ToString().Trim() + ":C" + (row).ToString().Trim()].Interior.Color = XlRgbColor.rgbPaleGreen;

            wb.SaveAs(filename, Type.Missing, password);
            wb.Close();

            return new ExcelFileData($"Payroll ({co})", new FileInfo(filename));
        }
    }
}
