using System;

namespace ExcelGenLib
{
    public class ExtendedPeriod
    {
        public int Id { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public string PayPeriod { get; set; }

        public string Formated => ToString();
        public override string ToString() { 
            return $"{PayPeriod} - {StartDate:MM/dd/yy} to {EndDate:MM/dd/yy}";  
        }
    }
}