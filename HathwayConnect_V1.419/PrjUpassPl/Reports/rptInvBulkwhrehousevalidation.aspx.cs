using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Collections;
using System.Configuration;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Reports
{
    public partial class rptInvBulkwhrehousevalidation : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Invenroty Bulk Warehouse Validation Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bridgrid();
        }

        public DataTable GetResult(String Query)
        {
            DataTable MstTbl = new DataTable();


            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            con.Open();

            OracleCommand Cmd = new OracleCommand(Query, con);
            OracleDataAdapter AdpData = new OracleDataAdapter();
            AdpData.SelectCommand = Cmd;
            AdpData.Fill(MstTbl);

            con.Close();

            return MstTbl;
        }

        private Hashtable getTopupParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            Hashtable htTopupParams = new Hashtable();
            htTopupParams.Add("From", from);
            htTopupParams.Add("TO", to);
            return htTopupParams;
        }

        public void bridgrid()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            DateTime fromDT;
            DateTime ToDT;
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                fromDT = new DateTime();
                ToDT = new DateTime();
                fromDT = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                ToDT = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                if (ToDT.CompareTo(fromDT) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";
                    lblSearchMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else if (Convert.ToDateTime(txtFrom.Text.ToString()) > DateTime.Now.Date)
                {
                    lblSearchMsg.Text = "You can not select date greater than current date!";
                    return;
                }
                else if (Convert.ToDateTime(txtTo.Text.ToString()) > DateTime.Now.Date)
                {
                    lblSearchMsg.Text = "You can not select date greater than current date!";
                    return;
                }
                else
                {
                    lblSearchMsg.Text = "";
                }
            }
            
            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            DataTable dt = new DataTable();
            Hashtable htTopupParams = getTopupParamsData();
            Cls_Business_Warehouse obj = new Cls_Business_Warehouse();
            dt = obj.GetInvBulkValiddata(htTopupParams, Session["username"].ToString());
            if (dt.Rows.Count == 0)
            {
                grdBulkProc.Visible = false;
                lblSearchMsg.Text = "No data found";

            }

            else
            {
                grdBulkProc.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdBulkProc.DataSource = dt;
                grdBulkProc.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdBulkProc.ClientID + "', 400, 1200 , 37 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");
            }

        }

        protected void grdBulkProc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("UniqId"))
            {
                try
                {
                    GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    string uniqueid = ((Label)clickedRow.FindControl("lbluid1")).Text;
                    Response.Redirect("../Transaction/FrmWarehouseBulkProcess.aspx?uniqueid=" + uniqueid);
                }
                catch (Exception ex)
                {

                }

            }
        }

    }
}