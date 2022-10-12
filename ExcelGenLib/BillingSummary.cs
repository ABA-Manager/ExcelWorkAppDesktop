using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelGenLib
{
    public class ClientData
    {
        public string AmountFormule;
        public string HoursFormule;
        public ClientData(string vAmount, string vHours)
        {
            AmountFormule = vAmount;
            HoursFormule = vHours;
        }
    }
    public class BillingSummary
    {
        public Dictionary<string, string> contractorSummary;
        public Dictionary<string, ClientData> clientSummary;
        public string BilledMedicaidFormule;
        public string PaidContractorFormule;
        public string Analyst;
        public string AnalystExtra;

        public BillingSummary()
        {
            BilledMedicaidFormule = "";
            PaidContractorFormule = "";
            contractorSummary = new Dictionary<string, string>();
            clientSummary = new Dictionary<string, ClientData>();
        }
    }
}
