using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using PrjUpassDAL.Helper;

namespace PrjUpassPl.Reports
{
    public partial class rptCompDetails : System.Web.UI.Page
    {
        #region "Message Alert"
        public void MessageAlert(String Message, String WindowsLocation)
        {
            String str = "";

            str = "alert('|| " + Message + " ||');";

            if (WindowsLocation != "")
            {
                str += "window.location = '" + WindowsLocation + "';";
            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, str, true);
            return;
        }
        #endregion

        #region "Page Load"
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dtime = DateTime.Now;
            Master.PageHeading = "Complaint Details";
            if (Session["username"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                 btnGenerateExcel.Visible = false;
                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
            }
        }
        #endregion

        #region "Button Submit"
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            lblResultCount.Text = "";
            DateTime fromDt;
            DateTime toDt;
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
            {
                fromDt = new DateTime();
                toDt = new DateTime();
                fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                // DateTime fromDt;
                // DateTime toDt;
                Double dateDiff = 0;
                if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
                {
                    fromDt = new DateTime();
                    toDt = new DateTime();
                    fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                    toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                    dateDiff = (toDt - fromDt).TotalDays;
                }
                
                if (toDt.CompareTo(fromDt) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";
                    GridCompList.Visible = false;
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
            BindGrid();
        }
        #endregion

        #region "BindGrid"
        public void BindGrid()
        {
            Int32 DepId = Convert.ToInt32(Session["UserDepid"]);

            String Query = " select cmpno ,deptnm ,depid ,source,authby,cmptype, cmpsubtype, custnm, custno, cmpdesc,cmpstatus,srvst,TO_CHAR(regdt,'dd-MM-yyyy') regdt, assgnuser, userremark, remarkdate,CHECKEDSTATUS,LCOCODE,companyname,callername,callerno,FLAG,alternateno from crm.mview_rpt_complaint_details ";
            Query += " where checkedstatus='Y' and trunc(regdt) >= TO_DATE('" + txtFrom.Text + "','dd-MM-yyyy') and trunc(regdt) <= TO_DATE('" + txtTo.Text + "','dd-MM-yyyy')";

            if (txtsearchpara.Text != "" && txtsearchpara.Text != null)
            {
                Query += " and ACCOUNTNO = '" + txtsearchpara.Text + "'";
            }

            Query += " and lcocode='"+Session["username"].ToString()+"' order by INSDATE";


            DataTable tblComplaintDetails = new DataTable();
            Cls_Helper obj = new Cls_Helper();
            tblComplaintDetails = obj.GetDataTable(Query);

            if (tblComplaintDetails.Rows.Count > 0)
            {
                GridCompList.DataSource = tblComplaintDetails;
                GridCompList.DataBind();
                btnGenerateExcel.Visible = true;
                ViewState["Tbldetails"] = tblComplaintDetails;
            }
            else
            {
                MessageAlert("No records found.","");//,"../Reports/RptCompDetails.aspx"
            }
        }
        #endregion

        #region "Export Excel"
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable htResponse = (DataTable)ViewState["Tbldetails"];
            DataTable dt = null; //check for exception
            if (htResponse.Rows.Count != 0)
            {
                dt = (DataTable)htResponse;
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;
                DataTable dtExcel = new DataTable();
                var ob = new rptCompDetails();
                string filename = "CompDeatils_" + datetime;
                ClsGenerateExcel generate = new ClsGenerateExcel();
                try
                {
                    int j = 0;
                    dtExcel.Columns.Add("Sr No");
                    dtExcel.Columns.Add("Comp. No.");
                    dtExcel.Columns.Add("Name");
                    dtExcel.Columns.Add("Mobile No");
                    dtExcel.Columns.Add("Description");
                    dtExcel.Columns.Add("Comp. Type");
                    dtExcel.Columns.Add("Status");
                    dtExcel.Columns.Add("Service Status");
                    dtExcel.Columns.Add("Comp. Date");
                    dtExcel.Columns.Add("Assign User");
                    dtExcel.Columns.Add("User Remark");
                    dtExcel.Columns.Add("Remark Date");
                    dtExcel.Columns.Add("Source");
                    dtExcel.Columns.Add("Flag");
                    dtExcel.Columns.Add("LCO Code");
                    dtExcel.Columns.Add("Account No");
                    while (j < dt.Rows.Count)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            dtExcel.Rows.Add(j.ToString(),
                                 dt.Rows[i]["cmpno"].ToString(),
                                 dt.Rows[i]["custnm"].ToString(),
                                 dt.Rows[i]["custno"].ToString(),
                                 dt.Rows[i]["cmpdesc"].ToString(),
                                 dt.Rows[i]["cmptype"].ToString(),
                                 dt.Rows[i]["cmpsubtype"].ToString(),
                                 dt.Rows[i]["cmpstatus"].ToString(),
                                 dt.Rows[i]["srvst"].ToString(),
                                 dt.Rows[i]["regdt"].ToString(),
                                 dt.Rows[i]["userremark"].ToString(),
                                 dt.Rows[i]["remarkdate"].ToString(),
                                     dt.Rows[i]["source"].ToString(),
                                 dt.Rows[i]["Flag"].ToString(),
                                 dt.Rows[i]["lcocode"].ToString(),
                                 dt.Rows[i]["ACCOUNTNO"].ToString());
                        }
                        generate.ExportToExcel(dtExcel, filename, ob);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("Error : " + ex.Message.Trim());
                    return;
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

            /* Verifies that the control is rendered (Export To Excel Is not working)*/
        }
        #endregion
    }
}