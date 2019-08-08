using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace PrjUpassPl.Transaction
{
    public partial class OnlineReqform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = "";
            try
            {
                if (Request.QueryString["formrequestdetails"].ToString() != "")
                {
                    str = Convert.ToString(Request.QueryString["formrequestdetails"]);
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "hwa", "document.getElementById('accnumber').value ='" + str + "'; ", true);
                    string str2 = "document.getElementById('accnumber').value ='" + str + "'; ";
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "hwa1", "document.getElementById('onlineformrequest').submit(); ", true);
                    // ClientScript.RegisterStartupScript(GetType(), "hwa", "document.getElementById('accnumber').value =" + Convert.ToString(Request.QueryString["formrequestdetails"])+ "; ", true);

                }
                else
                {
                    //ClientScript.RegisterStartupScript(GetType(), "hwa", "document.getElementById('accnumber').value =" + Convert.ToString(Session["vc_id"]) + "|" + Convert.ToString(Session["customer_no"]) + "|" + Convert.ToString(Session["custemail"]) + "|" + Convert.ToString(Session["custmob"]) + "; ", true);
                    str = Convert.ToString(Session["StateName"]) + "|" + Convert.ToString(Session["vc_id"]) + "|" + Convert.ToString(Session["customer_name"]) + "|" + Convert.ToString(Session["custemail"]) + "|" + Convert.ToString(Session["custmob"]);
                }

            }
            catch (Exception ex)
            {
                str = Convert.ToString(Session["StateName"]) + "|" + Convert.ToString(Session["vc_id"]) + "|" + Convert.ToString(Session["customer_name"]) + "|" + Convert.ToString(Session["custemail"]) + "|" + Convert.ToString(Session["custmob"]);
                //ClientScript.RegisterStartupScript(GetType(), "hwa", "document.getElementById('accnumber').value =" + Convert.ToString(Session["vc_id"]) + "|" + Convert.ToString(Session["customer_no"]) + "|" + Convert.ToString(Session["custemail"]) + "|" + Convert.ToString(Session["custmob"]) + "; ", true);
                //ClientScript.RegisterStartupScript(GetType(), "hwa1", "document.getElementById('form1').submit(); ", true);
            }
            if (str != "")
            {
                Response.Clear();

                StringBuilder sb = new StringBuilder();

                sb.Append("<html>");
                sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
                sb.AppendFormat("<form name='form' action='{0}' method='post' >", "http://www.hathway.com/Digital/OnlinePlanRequestForm");
                sb.AppendFormat("<input type='hidden' name='formrequestdetails' value='{0}'>", str);
                sb.Append("</form>");
                sb.Append("</body>");
                sb.Append("</html>");
                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
}