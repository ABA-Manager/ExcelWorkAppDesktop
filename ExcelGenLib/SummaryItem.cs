using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelGenLib
{
    public class SummaryItem
    {
        public double Medicaid;
        public double Paid;

        public SummaryItem(decimal vMedicaid, decimal vPaid)
        {
            Medicaid = decimal.ToDouble(vMedicaid);
            Paid = decimal.ToDouble(vPaid);
        }
        public SummaryItem(double vMedicaid, double vPaid)
        {
            Medicaid = vMedicaid;
            Paid = vPaid;
        }
    }
}
