using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Transaction;
using PrjUpassDAL.Authentication;

namespace PrjUpassPl.Transaction
{
    public partial class HomeCash : System.Web.UI.Page
    {
        string username = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Home";
            if (Session["username"] != null)
            {
                username = Convert.ToString(Session["username"]);
                Session["RightsKey"] = "Y";
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                Session["pagenos"] = "1";
            }
        }


    }
}