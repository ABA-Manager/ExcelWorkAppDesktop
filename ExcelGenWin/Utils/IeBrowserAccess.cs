using CefSharp;
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
    public class IeBrowserAccess : NavigatorAccessBase
    {
        public WebBrowser browser { get; set; }

        public ToolStripStatusLabel lbCurrentPage { get; set; }

        public IeBrowserAccess(WebBrowser webBrowser, ToolStripStatusLabel currentPage) : base()
        {
            browser = webBrowser;
            lbCurrentPage = currentPage;
            browser.DocumentCompleted += DocumentComplete;
            browser.Navigated += Navigated;
            browser.Navigating += Navigating;
        }

        private void Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string str = e.Url == null ? "null" : e.Url.ToString();
            lbCurrentPage.Text = $"Navigating Active: {browser.IsBusy} Url: {str}";
        }

        private void Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string str = e.Url == null ? "null" : e.Url.ToString();
            lbCurrentPage.Text = $"Navigated Active: {browser.IsBusy} Url: {str}";
        }

        private void DocumentComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            isLoaded = (browser.ReadyState == WebBrowserReadyState.Complete) 
                && !browser.IsBusy 
                && e.Url.ToString().Contains("/tabid/66/");

            string str = e.Url == null ? "null" : e.Url.ToString();
            lbCurrentPage.Text = $"Completed Active: {browser.IsBusy} Url: {str}";
        }

        public IeBrowserAccess(WebBrowser webBrowser, Details detail) : base(detail)
        {
            browser = webBrowser;
        }

        public override bool isLoaded { get; protected set; }

        public override void LoadUrl(string Url)
        {
            isLoaded = false;
            browser.Navigate(new Uri(Url));
        }

        public override async Task LoadUrlAsync(string Url)
        {
            await Task.Run(() =>
            {
                isLoaded = false;
                browser.Navigate(new Uri(Url));
            });
        }

        public override async Task<bool> UntilPageLoad(int TimeOut)
        {
            int TimeElapsed = 0;
            while (!isLoaded)
            {
                await Task.Delay(50);
                TimeElapsed += 50;
                if (TimeElapsed >= TimeOut * 1000) 
                    return false;
            }
            return true;
        }

        public override async Task DoRefreshAsync()
        {
            await Task.Run(() =>
            {
                isLoaded = false;
                browser.Refresh();
            });
        }

        public override async Task DoForwardAsync()
        {
            await Task.Run(() =>
            {
                isLoaded = false;
                if (browser.CanGoForward) browser.GoForward();
            });
        }

        public override async Task GoBackAsync()
        {
            await Task.Run(() =>
            {
                isLoaded = false;
                if (browser.CanGoBack) browser.GoBack(); ;
            });

        }

        public override string getTitle() 
        {
            return browser.Document.Title??"";
        }

        protected async Task<HtmlElement> getElemAsync(string codeElem, int Timeout)
        {
            HtmlElement elem;
            int passed = 0;
            do {
                await Task.Delay(100);
                if ((passed += 100) >= Timeout) return null;
            } while (null == (elem = browser.Document.GetElementById(ElementsID[codeElem])));
            return elem;
        }
        protected override async Task SetElement(string codeElem, string value, bool changeFocus = false, bool previousChangeFocus = false)
        {
            if (changeFocus) isLoaded = false;
            var elem = await getElemAsync(codeElem, 30000);
            if (elem == null)
            {
                throw new Exception("Error: There are connection issues... You most begin the process again");
            }
            elem.Focus();
            if (previousChangeFocus) elem = await getElemAsync(codeElem, 30000);
            if (elem == null)
            { 
                throw new Exception("Error: There are connection issues... You most begin the process again");
            }
            //elem.InvokeMember("onfocus");
            elem.SetAttribute("value", value);
            //elem.InvokeMember("onblur");
            if (changeFocus)
            {
                //await UntilPageLoad(30);
                //elem.InvokeMember("onblur");
            }
        }

        protected override async Task Invoke(string codeElem, string function)
        {
            isLoaded = false;
            var elem = await getElemAsync(codeElem, 30000);
            if (elem == null)
            {
                throw new Exception("Error: There are connection issues... You most begin the process again");
            }
            elem.Focus();
            elem = await getElemAsync(codeElem, 30000);
            if (elem == null)
            {
                throw new Exception("Error: There are connection issues... You most begin the process again");
            }
            elem.InvokeMember(function);
            await UntilPageLoad(30);
        }

        protected override async Task SetFocus(string codeElem, bool wait = false)
        {
            var elem = await getElemAsync(codeElem, 30000);
            if (elem == null)
            {
                throw new Exception("Error: There are connection issues... You most begin the process again");
            }
            browser.Focus();
            elem.Focus();
        }

        //public override async Task InvokeChangeField(string codeElem)
        //{
        //    isLoaded = false;
        //    var elem = browser.Document.GetElementById(ElementsID[codeElem]);
        //    var objs = new object[] { strOnblur };//$"iCFieldChanged(document.getElementById('{ElementsID[codeElem]}'));" };
        //    browser.Document.InvokeScript("eval", objs);
        //    await UntilPageLoad(8);
        //}
    }
}
