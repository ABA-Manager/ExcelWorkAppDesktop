using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABABillingAndClaim.Views
{
    public partial class FrmSettings : Form
    {
        public static string ExcelPassword = ConfigurationManager.AppSettings["excel.password"];
        public FrmSettings()
        {
            InitializeComponent();
            txPassword.Text = ExcelPassword;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExcelPassword = txPassword.Text;

            try
            {
                //Save AppSettings
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                SetSetting(settings, "excel.password", ExcelPassword);

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show($"Error writing Application Settings {ex.Message}");
            }
        }

        private static void SetSetting(KeyValueConfigurationCollection settings, string key, string value)
        {
            if (settings[key] == null)
                settings.Add(key, value);
            else
                settings[key].Value = value;
        }
    }
}
