using CefSharp;
using CefSharp.WinForms;
using ClinicDOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ABABillingAndClaim
{
    public class IeBrowserAccess : NavigatorAccessBase
    {
        public WebBrowser browser { get; set; }

        public IeBrowserAccess(WebBrowser webBrowser) : base()
        {
            browser = webBrowser;
            browser.DocumentCompleted += DocumentComplete;
        }

        private void DocumentComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            isLoaded = true;
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
            var elems = browser.Document.GetElementById("Head")?.GetElementsByTagName("Title");
            if (elems != null && elems.Count > 0) 
                return elems[0].InnerText;
            else 
                return "";
        }

        public override void SetElement(string codeElem, string value, bool changeFocus)
        {
            browser.Document.GetElementById(ElementsID[codeElem])?.SetAttribute("value", value);
        }

        protected override void Invoke(string codeElem, string function)
        {
            browser.Document.GetElementById(ElementsID[codeElem])?.InvokeMember(function);
        }

        protected override void SetFocus(string codeElem)
        {
            browser.Document.GetElementById(ElementsID[codeElem])?.Focus(); 
        }

    }
}
