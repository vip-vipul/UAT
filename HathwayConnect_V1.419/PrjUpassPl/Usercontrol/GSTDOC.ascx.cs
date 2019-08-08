using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjUpassPl.Usercontrol
{
    public partial class GSTDOC : System.Web.UI.UserControl
    {
        public string _lblLCONameHead
        {
            get { return lblLCONameHead.Text; }
            set { lblLCONameHead.Text = value; }

        }

        public string _lblSYSDATE
        {
            get { return lblSYSDATE.Text; }
            set { lblSYSDATE.Text = value; }


        }

        public string _lblCompanyName
        {
            get { return lblCompanyName.Text; }
            set { lblCompanyName.Text = value; }
        }
        public string _lblCompanyAddress
        {
            get { return lblCompanyAddress.Text; }
            set { lblCompanyAddress.Text = value; }
        }
        public string _lblLCOName
        {
            get { return lblLCOName.Text; }
            set { lblLCOName.Text = value; }
        }
        public string _lblLCOCode
        {
            get { return lblLCOCode.Text; }
            set { lblLCOCode.Text = value; }
        }
        public string _lblSYSDATETIME
        {
            get { return lblSYSDATETIME.Text; }
            set { lblSYSDATETIME.Text = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}