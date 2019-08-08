using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjUpassPl.Transaction
{
    public partial class frmInventoryManu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Inventory Management";
            if (!IsPostBack)
            {
                Session["pagenos"] = "0";

                Session["RightsKey"] = null;
                // BindData();

            }
        }
    }
}