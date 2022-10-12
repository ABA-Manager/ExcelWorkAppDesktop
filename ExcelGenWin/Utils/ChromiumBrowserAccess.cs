using CefSharp;
using CefSharp.DevTools.CSS;
using CefSharp.DevTools.Profiler;
using CefSharp.Internals.Wcf;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;
using ClinicDOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ABABillingAndClaim.Utils
{
    public class ChromiumBrowserAccess : NavigatorAccessBase
    {
        public ChromiumWebBrowser browser { get; set; }

        public string title { get; set; } = "";

        public string address { get; set; } = "";

        public string loadingField { get; set; } = "";

        public string loadingValue { get; set; } = "";

        public ToolStripStatusLabel lbCurrentPage { get; set; }

        public ChromiumBrowserAccess(ChromiumWebBrowser webBrowser, ToolStripStatusLabel currentPage) : base()
        {
            browser = webBrowser;
            lbCurrentPage = currentPage;
            browser.LoadingStateChanged += OnLoadingStateChange;
            browser.AddressChanged += OnAddressChange;
            browser.TitleChanged += OnTitleChange;
        }

        private void OnTitleChange(object sender, TitleChangedEventArgs e)
        {
            title = e.Title;
        }

        private void OnAddressChange(object sender, AddressChangedEventArgs e)
        {
            address = e.Address;
        }

        private void OnLoadingStateChange(object sender, LoadingStateChangedEventArgs e)
        {
            isLoaded = !e.IsLoading;
            //    && address.Contains("/tabid/66/");
            loadingField = "";
            loadingValue = "";
            updateStatusBar();
        }

        public void updateStatusBar()
        {
            string str = "";
            if (loadingField != "" && loadingValue.Contains("()"))
                str = $"loading ([{loadingField}].{loadingValue}), ";
            else if (loadingField != "")
                str = $"loading ([{loadingField}]: {loadingValue}), ";
            lbCurrentPage.Text = $"Completed: {isLoaded}, {str}Url: {address}";
        }

        public ChromiumBrowserAccess(ChromiumWebBrowser webBrowser, Details detail) : base(detail)
        {
            browser = webBrowser;
        }

        public override bool isLoaded { get; protected set; }

        public override void LoadUrl(string Url)
        {
            //isLoaded = false;
            browser.LoadUrl(Url);
        }

        public override async Task LoadUrlAsync(string Url)
        {
            await Task.Run(() =>
            {
                //isLoaded = false;
                browser.LoadUrl(Url);
                //browser.ShowDevTools();
            });
        }

        public override async Task<bool> UntilPageLoad(int TimeOut)
        {
            int TimeElapsed = 0;
            do
            {
                await Task.Delay(100);
                TimeElapsed += 100;
                if (TimeElapsed >= TimeOut * 1000)
                    return false;
            } while (!isLoaded);
            return true;
        }

        public override async Task DoRefreshAsync()
        {
            await Task.Run(() =>
            {
                //isLoaded = false;
                browser.Refresh();
            });
        }

        public override async Task DoForwardAsync()
        {
            await Task.Run(() =>
            {
                //isLoaded = false;
                if (browser.CanGoForward) browser.Forward();
            });
        }

        public override async Task GoBackAsync()
        {
            await Task.Run(() =>
            {
                //isLoaded = false;
                if (browser.CanGoBack) browser.Back(); ;
            });

        }

        public override string getTitle()
        {
            return title;
        }

        protected async Task ExecElemFunction(string codeElem, string function, bool wait = false)
        {
            //if (wait) 
            //    isLoaded = false;
            loadingField = codeElem;
            loadingValue = function;
            updateStatusBar();
            //await Task.Run(() =>
            //{
            //    browser.ExecuteScriptAsync($"document.getElementById('{ElementsID[codeElem]}').{function}");
            //});            
            await browser.EvaluateScriptAsync($"document.getElementById('{ElementsID[codeElem]}').{function}",new TimeSpan(0,0,20));
            if (wait) 
                await UntilPageLoad(10);
        }

        protected async Task SetElemValue(string codeElem, string value, bool wait = false)
        {
            //if (wait) 
            //    isLoaded = false;
            loadingField = codeElem;
            loadingValue = value;
            updateStatusBar();
            //await Task.Run(() =>
            //{
            //    browser.ExecuteScriptAsync($"document.getElementById('{ElementsID[codeElem]}').value = '{value}'");
            //});
            await browser.EvaluateScriptAsync($"document.getElementById('{ElementsID[codeElem]}').value = '{value}'", new TimeSpan(0, 0, 20));
            if (wait) 
                await UntilPageLoad(10);
        }

        protected override async Task SetElement(string codeElem, string value, bool changeFocus = false, bool previousChangeFocus = false)
        {
            await SetFocus(codeElem);
            await Task.Delay(50);
            await SetElemValue(codeElem, value);
            await Task.Delay(50);
            //await ExecElemFunction(codeElem, "onchange()");
            await ExecElemFunction(codeElem, "onblur()", changeFocus);
            await Task.Delay(50);
        }

        protected override async Task Invoke(string codeElem, string function)
        {
            //isLoaded = false;
            await ExecElemFunction(codeElem, function, true);
            await Task.Delay(50);
            //await UntilPageLoad(5);
        }

        protected override async Task SetFocus(string codeElem, bool wait = false)
        {
            //if (wait) isLoaded = false;
            browser.Focus();
            //await ExecElemFunction(codeElem, "onfocus()");
            await ExecElemFunction(codeElem, "focus()");
            await Task.Delay(50);
            //if (wait) await UntilPageLoad(5);
        }
    }
}
