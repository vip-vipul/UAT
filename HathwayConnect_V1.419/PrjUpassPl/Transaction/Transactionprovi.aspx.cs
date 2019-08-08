using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjUpassPl.Transaction
{
    public partial class Transactionprovi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["RightsKey"] = "N";
            
        }
    }
}