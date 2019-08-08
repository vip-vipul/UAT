using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjUpassPl.Master
{
    public partial class frmgstn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //accnumber.Value = Session["username"].ToString();

            ClientScript.RegisterStartupScript(GetType(), "hwa", "document.getElementById('accnumber').value =" + Session["username"].ToString() + "; ", true);
            ClientScript.RegisterStartupScript(GetType(), "hwa1", "document.getElementById('form1').submit(); ", true);
        }
    }
}