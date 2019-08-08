using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjUpassPl
{
    public partial class forgotpass : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnreset_Click(object sender, EventArgs e)
        {
            if (txtname.Text.Trim() == "")
            {
                msgbox("Please Enter UserName", txtname);
                return;
            }
        }

        public void msgbox(string message, Control ctrl)
        {
            string msg = "<script type=\"text/javascript\">alert(\"" + message + "\");</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", msg);
            ctrl.Focus();
        }
    }
}