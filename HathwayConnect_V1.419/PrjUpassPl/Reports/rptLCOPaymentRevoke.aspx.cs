using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PrjUpassBLL.Reports;
using System.Data;
using System.IO;
using System.Drawing;

namespace PrjUpassPl.Reports
{
    public partial class rptLCOPaymentRevoke : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "LCO Payment Revoke Report";
            if (!IsPostBack)
            {
                Session["pagenos"] = "1";
                Session["RightsKey"] = null;
                grdLCOPayRev.PageIndex = 0;
                
                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                btngrnExel.Visible = false;
            }
        }

        private Hashtable getTopupParamsData()
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
            string from = txtFrom.Text;
            string to = txtTo.Text;
            //lblResultCount.Text = "";
            DateTime fromDt;
            DateTime toDt;
            lbldaterange.Text = "";
            btngrnExel.Visible = false;
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
            {
                fromDt = new DateTime();
                toDt = new DateTime();
                fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                if (toDt.CompareTo(fromDt) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";
                    grdLCOPayRev.Visible = false;
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
                    grdLCOPayRev.Visible = true;
                }
            }

            Hashtable htTopupParams = getTopupParamsData();
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

            Cls_Business_RptLCOPayRev objRptLCOPayRev = new Cls_Business_RptLCOPayRev();
            Hashtable ht = objRptLCOPayRev.GetPaymentRev(htTopupParams, username, operator_id, catid);

            DataTable dt = null; //check for exception
            if (ht["htResponse"] != null)
            {
                dt = (DataTable)ht["htResponse"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdLCOPayRev.Visible = false;
                lblSearchMsg.Text = "No data found";
                lbldaterange.Text = "";
            }
            else
            {
                btngrnExel.Visible = true;
                grdLCOPayRev.Visible = true;
                lbldaterange.Text = "<b>Report Generated between </b>" + from + "<b> and </b>" + to;
                lblSearchMsg.Text = "";
                ViewState["pay_rev"] = dt;
                grdLCOPayRev.DataSource = dt;
                grdLCOPayRev.DataBind();

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdLCOPayRev.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");

                //showing result count
                //lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }


        }

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            // lblResultCount.Text = "";           

            Hashtable htTopupParams = getTopupParamsData();

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

            Cls_Business_RptLCOPayRev objRptLCOPayRev = new Cls_Business_RptLCOPayRev();
            Hashtable htResponse = objRptLCOPayRev.GetPaymentRev(htTopupParams, username, operator_id, catid);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "LCOPaymentRevoke_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)
                        + "LCO Code" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        //+ "Company Code" + Convert.ToChar(9)
                        + "Payment Mode" + Convert.ToChar(9)
                        + "Voucher NO." + Convert.ToChar(9)
                        + "Amount" + Convert.ToChar(9);
                    strheader += "Reason" + Convert.ToChar(9)
                        + "Remark" + Convert.ToChar(9);

                    strheader += "Company Name" + Convert.ToChar(9)
                        + "Distributor" + Convert.ToChar(9)
                        + "Sub Distributor" + Convert.ToChar(9)
                        + "State" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9);

                    strheader += "Inserted By" + Convert.ToChar(9)
                    + "Inserted Date" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_lcopay_lcocode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_lcomst_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["payment_mode"].ToString() + Convert.ToChar(9);
                                //+ dt.Rows[i]["var_lcopay_companycode"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["var_lcopay_voucherno"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["num_lcopay_amount"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["var_reason_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_lcopay_remark"].ToString() + Convert.ToChar(9);

                            strrow += dt.Rows[i]["companyname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["distributor"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["subdistributor"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["statename"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cityname"].ToString() + Convert.ToChar(9);

                            strrow += dt.Rows[i]["var_lcopay_insby"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["date1"].ToString() + Convert.ToChar(9);
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
                Response.Redirect("../MyExcelFile/" + "LCOPaymentRevoke_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdLCOPayRev.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdLCOPayRev.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["pay_rev"] = dt;
                grdLCOPayRev.DataSource = dt;
                grdLCOPayRev.DataBind();
            }
        }

        protected void grdLCOPayRev_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLCOPayRev.PageIndex = e.NewPageIndex;
            Hashtable htTopupParams = getTopupParamsData();
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

            Cls_Business_RptLCOPayRev objRptLCOPayRev = new Cls_Business_RptLCOPayRev();
            Hashtable ht = objRptLCOPayRev.GetPaymentRev(htTopupParams, username, operator_id, catid);

            DataTable dt = null; //check for exception
            if (ht["htResponse"] != null)
            {
                dt = (DataTable)ht["htResponse"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdLCOPayRev.Visible = false;
                lblSearchMsg.Text = "No data found";
                lbldaterange.Text = "";
            }
            else
            {
                btngrnExel.Visible = true;
                grdLCOPayRev.Visible = true;
                //lbldaterange.Text = "<b>Report Generated between </b>" + from + "<b> and </b>" + to;
                lblSearchMsg.Text = "";
               // ViewState["pay_rev"] = dt;
                grdLCOPayRev.DataSource = dt;
                grdLCOPayRev.DataBind();

                

            }
        }

        protected void grdLCOPayRev_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnChqBouncedDt = (HiddenField)e.Row.FindControl("hdnChqBouncedDt");
                HiddenField hdnPayMode = (HiddenField)e.Row.FindControl("hdnPayMode");

                if (hdnChqBouncedDt.Value != "" && (hdnPayMode.Value.Trim() == "Cheque" || hdnPayMode.Value.Trim() == "DD"))
                {
                    e.Row.BackColor = ColorTranslator.FromHtml("#ffccff");
                }
            }
        }
    }
}