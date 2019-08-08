using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Master;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;
using System.Configuration;
using System.Data.OracleClient;

namespace PrjUpassPl.Master
{
    public partial class mstHwayMsgBrodcast : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            Cls_Business_MstHwaymsgBrodcaster obj = new Cls_Business_MstHwaymsgBrodcaster();
            string username = "";

            if (rbtnFlag.SelectedValue == "0")
            {
                ht["msgFlag"] = "D";
            }
            else
            {
                ht["msgFlag"] = "C";
            }

            ht["Brodcastermsg"] = txtBrodcastermsg.Text.Trim();

            if (Session["username"] != null)
            {
                username = Convert.ToString(Session["username"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            //Modified by shrikant on 25 dec 2014
            string response = "";
            if (rbtnFlag.SelectedValue == "1" && txtBrodcastermsg.Text.Trim() == "")
            {
                response = "Message concated successfully";
            }
            else
            {
                response = obj.SetBrodcastermsg(username, ht);
            }
            //
            if (response == "ex_occured")
            {
                //exception occured
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            else
            {
                lblResponseMsg.Text = response;
            }
        }
    }
}