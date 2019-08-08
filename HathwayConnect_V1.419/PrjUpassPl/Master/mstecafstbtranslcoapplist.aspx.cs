using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Configuration;
using System.Data;

namespace PrjUpassPl.Master
{
    public partial class mstecafstbtranslcoapplist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.PageHeading = "STB Transfer LCO Approval";
                string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                OracleConnection con = new OracleConnection(strCon);
                string GetData = "";
                GetData += " select  var_stbtransfer_refid Refid,var_stbtransfer_lcocode lco ,max(date_stbtransfer_insdate) insdate from  AOUP_LCOPRE_STBTRANSFER_MST where  var_stbtransfer_status is null";
                GetData += " and var_stbtransfer_adminlevel='N' and ( var_stbtransfer_adminstatus is null or var_stbtransfer_adminstatus='A') and var_stbtransfer_translcocode='" + Session["username"].ToString() + "'";
                GetData += " group by var_stbtransfer_refid,var_stbtransfer_lcocode";
                GetData += " order by max(date_stbtransfer_insdate) ";
                DataTable dt = new DataTable();
                OracleCommand cmd = new OracleCommand(GetData, con);
                OracleDataAdapter DaObj = new OracleDataAdapter(cmd);

                DaObj.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    grdBulkstatus.DataSource = dt;
                    grdBulkstatus.DataBind();
                }
                else
                {
                    popMsgBox.Show();
                }
            }
        }

        protected void lnkinbox_Click(object sender, EventArgs e)
        {
            int rowindex = Convert.ToInt32((((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer))).RowIndex);
            LinkButton lnlvalue = (LinkButton)grdBulkstatus.Rows[rowindex].FindControl("lnkRefid");
            if (lnlvalue.Text != "")
            {
                Session["RefId"] = lnlvalue.Text;
                Response.Redirect("../Master/mstecafstbtranslcoapp.aspx");
            }

        }

    }
}