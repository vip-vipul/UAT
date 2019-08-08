using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;
using PrjUpassPl.Helper;
using PrjUpassBLL.Reports;
using System.IO;
using System.Configuration;
using System.Data.OracleClient;
using Microsoft.VisualBasic;

namespace PrjUpassPl.Reports
{
    public partial class rptChildTVDiscountReport : System.Web.UI.Page
    {
        decimal amt = 0;
        DateTime dtime = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Child TV Discount Report";
            if (!IsPostBack)
            {
              //  Session["RightsKey"] = null;
                Session["RightsKey"] = "N";
                grdchildTVdiscount.PageIndex = 0;
                //setting page heading

                Session["pagenos"] = "1";

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                btngrnExel.Visible = false;
                btnGenerateExcel.Visible = false;
                //FillLcoDetails();
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
                if (dateDiff >= 3)
                {
                    btnGenerateExcel_Click(null, null);
                }
                if (toDt.CompareTo(fromDt) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";
                    grdchildTVdiscount.Visible = false;
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
                    grdchildTVdiscount.Visible = true;
                }
            }

            Hashtable htTopupParams = getTopupParamsData();

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
              //  operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();    //Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_rptChildTVDiscount objTran = new Cls_Business_rptChildTVDiscount();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid);//, operator_id

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            //showing parameters
            string strParams = htResponse["ParamStr"].ToString();
            if (!String.IsNullOrEmpty(strParams))
            {
                // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>Top-up Parameters : </b>" + strParams);
                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (dt.Rows.Count == 0)
            {
                grdchildTVdiscount.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                btnGenerateExcel.Visible = true;
                grdchildTVdiscount.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdchildTVdiscount.DataSource = dt;
                grdchildTVdiscount.DataBind();
                 DivRoot.Style.Add("display", "block");

                //showing result count
                //lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }

            /*DataTable dt = new DataTable("Pager");
            dt.Columns.Add("dtttime");
            dt.Columns.Add("amt");
            dt.Columns.Add("paymode");
            dt.Columns.Add("erprcptno");
            dt.Columns.Add("rcptno");
            dt.Columns.Add("finuid");
            dt.Columns.Add("fiuname");
            dt.Columns.Add("action");
            dt.Rows.Add();
            dt.Rows[0]["dtttime"] = "24-Nov-2014 10:20:04 pm";
            dt.Rows[0]["amt"] = "1000";
            dt.Rows[0]["paymode"] = "Cash";
            dt.Rows[0]["erprcptno"] = "1112";
            dt.Rows[0]["rcptno"] = "5501";
            dt.Rows[0]["finuid"] = "0600001";
            dt.Rows[0]["fiuname"] = "ADMIN";
            dt.Rows[0]["action"] = "Refund";
            dt.Rows.Add();
            dt.Rows[1]["dtttime"] = "23-Nov-2014 11:03:33 pm";
            dt.Rows[1]["amt"] = "2000";
            dt.Rows[1]["paymode"] = "Cheque";
            dt.Rows[1]["erprcptno"] = "4567";
            dt.Rows[1]["rcptno"] = "5572";
            dt.Rows[1]["finuid"] = "0600101";
            dt.Rows[1]["fiuname"] = "UPASS";
            dt.Rows[1]["action"] = "Topup";
            dt.Rows.Add();
            dt.Rows[2]["dtttime"] = "22-Nov-2014 08:11:00 pm";
            dt.Rows[2]["amt"] = "3000";
            dt.Rows[2]["paymode"] = "Cash";
            dt.Rows[2]["erprcptno"] = "98788";
            dt.Rows[2]["rcptno"] = "5598";
            dt.Rows[2]["finuid"] = "060010001";
            dt.Rows[2]["fiuname"] = "UPASS";
            dt.Rows[2]["action"] = "Reversal";
            

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdAddPlantopup.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdAddPlantopup.Visible = true;
                lblSearchMsg.Text = "";
                grdAddPlantopup.DataSource = dt;
                grdAddPlantopup.DataBind();
            }*/
        }

        protected void grdAddPlantopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.Cells.Count > 0)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            //    {
            //        //LinkButton HLink1 = (LinkButton)e.Row.Cells[2].Controls[0];
            //        amt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "amt"));
            //    }
            //    else if (e.Row.RowType == DataControlRowType.Footer)
            //    {
            //        e.Row.Cells[2].Text = "" + amt;
            //        //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
            //        //e.Item.Cells[8].Text = "" + Total;
            //    }
            //}            
        }

        protected void grdAddPlantopup_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable;
            string sort_direction;
            if (ViewState[e.SortExpression.ToString()] == null)
            {
                ViewState[e.SortExpression.ToString()] = Convert.ToString(e.SortDirection);
                sort_direction = Convert.ToString(e.SortDirection);
            }
            else
            {
                if (ViewState[e.SortExpression.ToString()].ToString() == "Ascending")
                {
                    ViewState[e.SortExpression.ToString()] = "Descending";
                    sort_direction = "Descending";
                }
                else
                {
                    ViewState[e.SortExpression.ToString()] = "Ascending";
                    sort_direction = "Ascending";
                }
            }

            if (ViewState["searched_trans"] != null)
            {
                dataTable = (DataTable)ViewState["searched_trans"];
            }
            else
            {
                return;
            }
            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + SetSortDirection(sort_direction);
                grdchildTVdiscount.DataSource = dataTable;
                grdchildTVdiscount.DataBind();
            }
        }

        protected string SetSortDirection(string sortDirection)
        {
            if (sortDirection == "Ascending")
            {
                return "DESC";
            }
            else
            {
                return "ASC";
            }
        }

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            lblResultCount.Text = "";

            Hashtable htTopupParams = getTopupParamsData();

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
               // operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();  //Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_rptChildTVDiscount objTran = new Cls_Business_rptChildTVDiscount();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
                if (dt.Rows.Count > 0)
                {
                    DateTime dd = DateTime.Now;
                    string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                    StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "ChildTVDiscount_" + datetime + ".csv");
                    try
                    {
                        int j = 0;
                        String strheader = "Sr.No." + ","
                            + "Receipt No" + ","
                            + "Customer ID" + ","
                            + "VC Id" + ","
                            + "Plan Name" + ","
                            + "Plan Type" + ","
                            + "Flag" + ","
                            + "LCO Price" + ","
                            + "Rate" + ","
                        + "Amount" + ","
                            + "Credit" + ","
                            + "Child Id" + ","
                            + "CR Type" + ","
                        + "Inserted By" + ","
                            + "LCO Code" + ","
                            + "Date" + ","
                            + "City" + ","
                            + "State" + ","
                        + "DAS Area" + ","
                            + "Source Flag" + ","
                            + "Share Type" + ","
                             + "JV Name" + ","
                               + "JV No." + ",";

                        while (j < dt.Rows.Count)
                        {
                            sw.WriteLine(strheader);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                j = j + 1;
                                string strrow = j.ToString() + ","
                                    + "'" + dt.Rows[i]["var_distrans_receiptno"].ToString() + ","
                                    + dt.Rows[i]["var_distrans_custid"].ToString() + ","
                                + dt.Rows[i]["var_distrans_vcid"].ToString() + ","
                                    + dt.Rows[i]["var_distrans_planname"].ToString() + ","
                                    + dt.Rows[i]["var_distrans_plantype"].ToString() + ","
                                    + dt.Rows[i]["var_distrans_flag"].ToString() + ","
                                    + dt.Rows[i]["num_distrans_lcoprice"].ToString() + ","
                                    + dt.Rows[i]["num_distrans_rate"].ToString() + ","
                                + dt.Rows[i]["num_distrans_amt"].ToString() + ","
                                + dt.Rows[i]["num_distrans_credited"].ToString() + ","
                                + dt.Rows[i]["num_distrans_childid"].ToString() + ","
                                + dt.Rows[i]["var_distrans_crtype"].ToString() + ","
                                + dt.Rows[i]["var_distrans_insby"].ToString() + ","
                                + dt.Rows[i]["var_distrans_lcocode"].ToString() + ","
                                + dt.Rows[i]["dat_distrans_transdt"].ToString() + ","
                                + dt.Rows[i]["var_distrans_city"].ToString() + ","
                                + dt.Rows[i]["var_distrans_state"].ToString() + ","
                                + dt.Rows[i]["var_distrans_dasarea"].ToString() + ","
                                + dt.Rows[i]["var_distrans_sourceflag"].ToString() + ","
                                + dt.Rows[i]["var_distrans_sharetype"].ToString() + ","
                                 + dt.Rows[i]["var_distrans_jvname"].ToString() + ","
                                  + dt.Rows[i]["var_distrans_jvno"].ToString() + ","
                                  ;
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
                    Response.AddHeader("Content-disposition", "attachment; filename=Topup_" + datetime + ".csv");
                    Response.ContentType = "text/csv";
                    Response.Redirect("../MyExcelFile/" + "ChildTVDiscount_" + datetime + ".csv");
                }
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdchildTVdiscount.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdchildTVdiscount.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdchildTVdiscount.DataSource = dt;
                grdchildTVdiscount.DataBind();

                //showing result count
                //lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        protected void grdAddPlantopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdchildTVdiscount.PageIndex = e.NewPageIndex;
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

            Cls_Business_rptChildTVDiscount objTran = new Cls_Business_rptChildTVDiscount();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid);

            DataTable dt = null;
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }




            if (dt.Rows.Count == 0)
            {
                grdchildTVdiscount.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                btnGenerateExcel.Visible = true;
                grdchildTVdiscount.Visible = true;
                lblSearchMsg.Text = "";
                //ViewState["searched_trans"] = dt;
                grdchildTVdiscount.DataSource = dt;
                grdchildTVdiscount.DataBind();



            }
        }

        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            lblResultCount.Text = "";

            Hashtable htTopupParams = getTopupParamsData();

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                //operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();  //Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_rptChildTVDiscount objTran = new Cls_Business_rptChildTVDiscount();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];

                if (dt.Rows.Count>0)
                {
                    DateTime dd = DateTime.Now;
                    string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                    StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "ChildTVDiscount_" + datetime + ".xls");
                    try
                    {
                        int j = 0;
                        String strheader = "Sr.No." + Convert.ToChar(9)
                            + "Receipt No" + Convert.ToChar(9)
                            + "Customer ID" + Convert.ToChar(9)
                            + "VC Id" + Convert.ToChar(9)
                            + "Plan Name" + Convert.ToChar(9)
                            + "Plan Type" + Convert.ToChar(9)
                            + "Flag" + Convert.ToChar(9)
                            + "LCO Price" + Convert.ToChar(9)
                            + "Rate" + Convert.ToChar(9)
                        + "Amount" + Convert.ToChar(9)
                            + "Credit" + Convert.ToChar(9)
                            + "Child Id" + Convert.ToChar(9)
                            + "CR Type" + Convert.ToChar(9)
                        + "Inserted By" + Convert.ToChar(9)
                            + "LCO Code" + Convert.ToChar(9)
                            + "Date" + Convert.ToChar(9)
                            + "City" + Convert.ToChar(9)
                            + "State" + Convert.ToChar(9)
                        + "DAS Area" + Convert.ToChar(9)
                            + "Source Flag" + Convert.ToChar(9)
                            + "Share Type" + Convert.ToChar(9)
                             + "JV Name" + Convert.ToChar(9)
                               + "JV No."+ Convert.ToChar(9);

                        while (j < dt.Rows.Count)
                        {
                            sw.WriteLine(strheader);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                j = j + 1;
                                string strrow = j.ToString() + Convert.ToChar(9)
                                   + "'" + dt.Rows[i]["var_distrans_receiptno"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["var_distrans_custid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_distrans_vcid"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["var_distrans_planname"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["var_distrans_plantype"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["var_distrans_flag"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["num_distrans_lcoprice"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["num_distrans_rate"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["num_distrans_amt"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["num_distrans_credited"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["num_distrans_childid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_distrans_crtype"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_distrans_insby"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_distrans_lcocode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["dat_distrans_transdt"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_distrans_city"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_distrans_state"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_distrans_dasarea"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_distrans_sourceflag"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_distrans_sharetype"].ToString() + Convert.ToChar(9)
                                 + dt.Rows[i]["var_distrans_jvname"].ToString() + Convert.ToChar(9)
                                  + dt.Rows[i]["var_distrans_jvno"].ToString() + Convert.ToChar(9)
                                  ;

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
                    Response.Redirect("../MyExcelFile/" + "ChildTVDiscount_" + datetime + ".xls"); 
                }
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdchildTVdiscount.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdchildTVdiscount.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdchildTVdiscount.DataSource = dt;
                grdchildTVdiscount.DataBind();

                //showing result count
                //lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        //protected void lnkGenerateReceipt_Click(object sender, EventArgs e)
        //{
        //    int indexno = (((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer))).RowIndex;
        //    int rindex = Convert.ToInt32(indexno.ToString());


        //    HiddenField hdndtttime = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdndtttime");
        //    string tempDateTimeValue = hdndtttime.Value;
        //    string[] DateTimeValue = tempDateTimeValue.Split(',');
        //    string DateTime = DateTimeValue[0].ToString();


        //    HiddenField hdnamt = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnamt");
        //    string tempAmount = hdnamt.Value;
        //    string[] tempAmountValue = tempAmount.Split(',');
        //    string Amount = tempAmountValue[0].ToString();

        //    HiddenField hdnrcptno = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnerprcptno");
        //    string tempReceiptonlineno = hdnrcptno.Value;
        //    string[] temptransactionIDValue = tempReceiptonlineno.Split(',');
        //    string Receiptonlineno = temptransactionIDValue[0].ToString();

        //    HiddenField hdnerprcptno = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnrcptno");
        //    string tempreceiptno = hdnerprcptno.Value;
        //    string[] tempreceiptnovalue = tempreceiptno.Split(',');
        //    string ReceiptNo = tempreceiptnovalue[0].ToString();

        //    HiddenField hdnlcocode = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnlcocode");
        //    string templcocode = hdnlcocode.Value;
        //    string[] templcocodevalue = templcocode.Split(',');
        //    string Lcocode = templcocodevalue[0].ToString();

        //    HiddenField hdnlconame = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnlconame");
        //    string templconame = hdnlconame.Value;
        //    string[] templconamevalue = templconame.Split(',');
        //    string Lconame = templconamevalue[0].ToString();

        //    HiddenField hdnpaymode = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnpaymode");
        //    string temphdnpaymode = hdnpaymode.Value;
        //    string[] temphdnpaymodevalue = temphdnpaymode.Split(',');
        //    string Paymentmode = temphdnpaymodevalue[0].ToString();

        //    HiddenField hdncity = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdncity");
        //    string temphdncity = hdncity.Value;
        //    string[] temphdncityvalue = temphdncity.Split(',');
        //    string City = temphdncityvalue[0].ToString();

        //    HiddenField hdnBANKNAME = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnBANKNAME");
        //    string temphdnBANKNAME = hdnBANKNAME.Value;
        //    string[] temptemphdnBANKNAMEvalue = temphdnBANKNAME.Split(',');
        //    string Bankname = temptemphdnBANKNAMEvalue[0].ToString();

        //    HiddenField hdnCHEQUEDDNO = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnCHEQUEDDNO");
        //    string temphdnCHEQUEDDNO = hdnCHEQUEDDNO.Value;
        //    string[] temphdnCHEQUEDDNOval = temphdnCHEQUEDDNO.Split(',');
        //    string ChequeNo = temphdnCHEQUEDDNOval[0].ToString();


        //    HiddenField hdnrr = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnrr");
        //    string temphdnCHEQUEDDNOhdnrr = hdnrr.Value;
        //    string[] temphdnCHEQUEDDNOvalhdnrr = temphdnCHEQUEDDNOhdnrr.Split(',');
        //    string Chdnrr = temphdnCHEQUEDDNOvalhdnrr[0].ToString();

        //    HiddenField hdnauthno = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnauthno");
        //    string temphdnhdnauthno = hdnauthno.Value;
        //    string[] temptemphdnhdnauthno = temphdnhdnauthno.Split(',');
        //    string Ctemptemphdnhdnauthno = temptemphdnhdnauthno[0].ToString();

        //    HiddenField hdnmpos = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnmpos");
        //    string temphhdnmpos = hdnmpos.Value;
        //    string[] temphdnmpos = temphhdnmpos.Split(',');
        //    string Ctemphdnmpos = temphdnmpos[0].ToString();

        //    HiddenField hdnremark = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnremark");
        //    string temphdnremark = hdnremark.Value;
        //    string[] temphdntemphdnremark = temphdnremark.Split(',');
        //    string Ctemphdntemphdnremark = temphdntemphdnremark[0].ToString();
        //    //hdnremark

        //    HiddenField hdnfinuid = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnfinuid");
        //    string temphdnfiuname = hdnfinuid.Value;
        //    string[] temphdnfiunameF = temphdnfiuname.Split(',');
        //    string CtemphdnfiunameF = temphdnfiunameF[0].ToString();

        //    HiddenField hdnlco_paymode = (HiddenField)grdAddPlantopup.Rows[rindex].FindControl("hdnlco_paymode");
        //    string temphdnlco_paymode = hdnlco_paymode.Value;
        //    string[] temphdnlco_paymodeF = temphdnlco_paymode.Split(',');
        //    string Clco_paymodF = temphdnlco_paymodeF[0].ToString();

        //    Session["rcpt_pt_datetime"] = DateTime;
        //    Session["rcpt_pt_receiptno"] = ReceiptNo;
        //    Session["rcpt_pt_Lcocode"] = Lcocode;
        //    Session["rcpt_pt_Lconame"] = Lconame;
        //    Session["rcpt_pt_Amount"] = Amount;
        //    Session["rcpt_pt_Paymentmode"] = Clco_paymodF;

        //    if (Paymentmode == "Q")
        //    {
        //        Session["data"] = "**Credit in account will be given subject to clearance of Cheque.";
        //    }
        //    if (Paymentmode == "DD")
        //    {
        //        Session["data"] = "**Credit in account will be given subject to clearance of Demant Draft.";
        //    }


        //    Session["rcpt_pt_City"] = City;
        //    Session["rcpt_pt_Bankname"] = Bankname;
        //    Session["rcpt_pt_ChequeNo"] = ChequeNo;
        //    Session["rcpt_pt_receiptonlineno"] = Receiptonlineno;

        //    Session["rcpt_rr"] = Chdnrr;
        //    Session["rcpt_auth"] = Ctemptemphdnhdnauthno;
        //    Session["rcpt_mpos"] = Ctemphdnmpos;
        //    Session["rcpt_pt_premark1"] = Ctemphdntemphdnremark;
        //    Session["rcpt_pt_cashiername"] = CtemphdnfiunameF;


        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "newpage", "customOpen('../Reports/rcptPaymentReceiptInvoice.aspx');", true);
        //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdAddPlantopup.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
        //    DivRoot.Style.Add("display", "block");
        //    //Response.Write("<script language='javascript'> window.open('../Reports/rcptPaymentReceiptInvoice.aspx', 'Print_Receipt','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes,location=no,status=no');</script>");

        //    //   ClientScript.RegisterStartupScript(typeof(Page), "alertMessage", "<script type='text/javascript'>alert('Payment Done Successfully');window.location.replace('../Transaction/rcptPaymentReceiptInvoice.aspx?lcoid=" + Request.QueryString["lcoid"].ToString().Trim() + "&msoname=" + Request.QueryString["msoname"].ToString().Trim() + "&lcoadd=" + Request.QueryString["lcoadd"].ToString().Trim() + "&lcocont=" + Request.QueryString["lcocont"].ToString().Trim() + "&lconame=" + Request.QueryString["lconame"].ToString().Trim() + "');</script>");
        //}

    }
}