using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using System.Collections;
using System.Configuration;
using System.IO;
namespace PrjUpassPl.Reports
{
    public partial class rptBulkACTDCTFileProcess : System.Web.UI.Page
    {
        decimal amt = 0;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Bulk ACT & DEACT Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
            }
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

        private Hashtable getParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            Hashtable htTopupParams = new Hashtable();
            htTopupParams.Add("from", from);
            htTopupParams.Add("to", to);
            return htTopupParams;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            binddata();
        }
        protected void binddata()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            DateTime fromDt;
            DateTime toDt;
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
            {
                fromDt = new DateTime();
                toDt = new DateTime();
                fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                if (toDt.CompareTo(fromDt) < 0)
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

            Hashtable htTopupParams = getParamsData();

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

            str += "	select LCOCODE, ACC_NO, VC_ID, INSBY, INSDATE, ACTION, PROCESSFLAG, OBRMSTATUS, OBRMDATE, REASON from view_BUlk_ACTDCT_DET	";
            str += "	where trunc(InsDate) >='" + from + "' and trunc(InsDate) <='" + to + "' and OperID ='" + operator_id + "'	";

            OracleCommand cmd = new OracleCommand(str, con);
            OracleDataAdapter DaObj = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            DaObj.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                grdBulkProc.Visible = false;
                lblSearchMsg.Text = "No data found";
                btngrnExel.Visible = false;
            }

            else
            {
                btngrnExel.Visible = true;
                grdBulkProc.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["htResponse"] = dt;
                grdBulkProc.DataSource = dt;
                grdBulkProc.DataBind();
                DivRoot.Style.Add("display", "block");
            }
        }

        protected void grdBulkProc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBulkProc.PageIndex = e.NewPageIndex;
            binddata();
        }

        protected void btngrnExel_Click(object sender, EventArgs e)
        {
            Hashtable htAddPlanParams = getParamsData();

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

            DataTable dt = null; //check for exception
            if (ViewState["htResponse"] != null)
            {
                dt = (DataTable)ViewState["htResponse"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count != 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "Bulk_ActDact_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)
                        + "LCO Code" + Convert.ToChar(9)
                        + "Account No." + Convert.ToChar(9)
                        + "VC ID" + Convert.ToChar(9)
                        + "Inserted By" + Convert.ToChar(9)
                        + "Inserted Date" + Convert.ToChar(9)
                        + "Action" + Convert.ToChar(9)
                        + "File Status" + Convert.ToChar(9)
                        + "OBRM Status" + Convert.ToChar(9)
                        + "OBRM Date" + Convert.ToChar(9)
                        + "Reason" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9) + "'";


                            strrow += dt.Rows[i]["LCOCode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["ACC_NO"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["vc_id"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Insby"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["InsDate"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Action"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Processflag"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["OBRMStatus"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["OBRMDate"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Reason"].ToString() + Convert.ToChar(9);

                            sw.WriteLine(strrow);

                        }
                    }
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception ex)
                {
                    sw.Flush();
                    sw.Close();
                    Response.Write("Error : " + ex.Message.Trim());
                    return;
                }
                Response.Redirect("../MyExcelFile/" + "Bulk_ActDact_" + datetime + ".xls");
            }
        }

    }
}