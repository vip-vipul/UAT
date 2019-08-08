using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Reports;
using System.Collections;
using System.Data;
using System.IO;

namespace PrjUpassPl.Reports
{
    public partial class rptSelfCareTransaction : System.Web.UI.Page
    {
        string username, catid, operator_id;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "SelfCare Transaction Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = "A";
                grdSelfCareReport.PageIndex = 0;

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
                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                //BindData();
            }
        }

        protected void BindData()
        {

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
            
            Cls_Business_RptSelfCareDetails objTran = new Cls_Business_RptSelfCareDetails();
            string lcocode = username;
            DataTable dt = objTran.getSelfCareDetails(txtFrom.Text, txtTo.Text, lcocode);

            if (dt.Rows.Count == 0)
            {               
                lblSearchMsg.Text = "No data found";
                lbldaterange.Visible = false;
                lbldaterange.Text = string.Empty ;
            }
            else
            {
                lblSearchMsg.Text = string.Empty;
                ViewState["SelfCareDetails"] = dt;
                grdSelfCareReport.DataSource = dt;
                grdSelfCareReport.DataBind();
                btnGenerateExcel.Visible = true;
                lbldaterange.Text = "<b>Report Generated between </b>" + txtFrom.Text + "<b> and </b>" + txtTo.Text;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
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
                if (toDt.CompareTo(fromDt) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";
                    grdSelfCareReport.Visible = false;
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
                    grdSelfCareReport.Visible = true;
                }
            }

           // Hashtable htTopupParams = getTopupParamsData();

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

            BindData();
        }

        protected void grdSelfCareReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSelfCareReport.PageIndex = e.NewPageIndex;
            
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
            BindData();
        }

        protected void grdSelfCareReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdSelfCareReport_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            // lblResultCount.Text = "";           

            //Hashtable htTopupParams = getTopupParamsData();

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

            if (ViewState["SelfCareDetails"] != null)
            {
                dt = (DataTable)ViewState["SelfCareDetails"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PISTransDet_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    //String strheader = "Sr.No." + Convert.ToChar(9)
                    String strheader = "No" + Convert.ToChar(9)
                        + "Transaction Id" + Convert.ToChar(9)
                        + "PG Transaction Status" + Convert.ToChar(9)
                        //+ "Company Code" + Convert.ToChar(9)
                        + "Amount" + Convert.ToChar(9)
                        + "PG Transaction Id" + Convert.ToChar(9)
                        + "Account No" + Convert.ToChar(9)
                        + "Payment Mode" + Convert.ToChar(9)
                         + "Email" + Convert.ToChar(9)
                         + "PG Request DateTime" + Convert.ToChar(9)
                    + "PG Response DateTime" + Convert.ToChar(9)
                    + "Receipt No" + Convert.ToChar(9)
                    + "Subscription Date" + Convert.ToChar(9)
                    + "Plan Expiry Date" + Convert.ToChar(9)
                    + "Subscribed Transaction Status" + Convert.ToChar(9)
                    + "Transaction Description" + Convert.ToChar(9)
                    + "PackName" + Convert.ToChar(9)
                    + "Plan Name" + Convert.ToChar(9)
                    + "Product Name" + Convert.ToChar(9)
                    + "Plan CallType" + Convert.ToChar(9)
                    + "IsLive" + Convert.ToChar(9)
                    + "Platform Type" + Convert.ToChar(9)
                    + "Device Id" + Convert.ToChar(9)
                    + "City" + Convert.ToChar(9)
                    + "LCO Name" + Convert.ToChar(9)
                    + "Pack Amount" + Convert.ToChar(9)
                    + "Refund Amount" + Convert.ToChar(9)
                    + "Payable Amount" + Convert.ToChar(9)
                    + "Discount Amount" + Convert.ToChar(9)
                    + "Hathway Share Percentage" + Convert.ToChar(9)
                    + "LCO Share Percentage" + Convert.ToChar(9)
                    + "Actual Hathway Share Amount" + Convert.ToChar(9)
                    + "Actual LCO Share Amount" + Convert.ToChar(9)
                    + "LCO PG Name" + Convert.ToChar(9)
                    + "BRM_POID" + Convert.ToChar(9)
                    + "LCO_Code" + Convert.ToChar(9)
                    +"JV" + Convert.ToChar(9)
                    +"Company" + Convert.ToChar(9)
                    +"Transaction Date" + Convert.ToChar(9);

                    //strheader += "Reason" + Convert.ToChar(9)
                    //    + "Remark" + Convert.ToChar(9);

                    //strheader += "Company Name" + Convert.ToChar(9)
                    //    + "Distributor" + Convert.ToChar(9)
                    //    + "Sub Distributor" + Convert.ToChar(9)
                    //    + "State" + Convert.ToChar(9)
                    //    + "City" + Convert.ToChar(9);

                    //strheader += "Inserted By" + Convert.ToChar(9)
                    //+ "Inserted Date" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["transactionid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["pgtransactionstatus"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["amount"].ToString() + Convert.ToChar(9);
                            //+ dt.Rows[i]["var_lcopay_companycode"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["pgtransactionid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["accountno"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["paymentmode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["email"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["pgrequestdatetime"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["pgresponsedatetime"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["receiptno"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["subscriptiondate"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["planexpirydate"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["subscribedtransactionstatus"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["transactiondescription"].ToString() + Convert.ToChar(9)

                            + dt.Rows[i]["packname"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["planname"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["productname"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["plancalltype"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["islive"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["platformtype"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["deviceid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["city"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lco_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["packamount"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["refundamount"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["payableamount"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["discountamount"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["hathwaysharepercentage"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lcosharepercentage"].ToString() + Convert.ToChar(9)
                            +dt.Rows[i]["actualhathwayshareamount"].ToString() + Convert.ToChar(9)
                                 + dt.Rows[i]["actuallcoshareamount"].ToString() + Convert.ToChar(9)
                                  + dt.Rows[i]["lcopgname"].ToString() + Convert.ToChar(9)
                                   + dt.Rows[i]["brm_poid"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["lco_code"].ToString() + Convert.ToChar(9)
                                     + dt.Rows[i]["jv"].ToString() + Convert.ToChar(9)
                                      + dt.Rows[i]["company"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["transaction_date"].ToString() + Convert.ToChar(9);
                            //+ dt.Rows[i]["date1"].ToString() + Convert.ToChar(9);
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
                Response.Redirect("../MyExcelFile/" + "PISTransDet_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdSelfCareReport.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdSelfCareReport.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["pay_rev"] = dt;

                grdSelfCareReport.DataSource = dt;
                grdSelfCareReport.DataBind();
            }
        }
    }
}