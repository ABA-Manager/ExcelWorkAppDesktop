using ClinicDOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABABillingAndClaim.Views
{
    public partial class FrmPending : Form
    {
        public int ServiceLogId { get; set; }

        public Clinic_AppContext db { get; set; }
        public FrmPending(Clinic_AppContext vDb, int vServiceLogId)
        {
            ServiceLogId = vServiceLogId;
            db = vDb;
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Update
            var commandText = "UPDATE Servicelog SET Pending = @reason WHERE Id = @serviceLogId";
            var serviceLogPr = new SqlParameter("@serviceLogId", ServiceLogId);
            var reasonPr = new SqlParameter("@reason", textBox1.Text);

            await db.Database.ExecuteSqlCommandAsync(commandText, new[] { serviceLogPr, reasonPr });
        }
    }
}
