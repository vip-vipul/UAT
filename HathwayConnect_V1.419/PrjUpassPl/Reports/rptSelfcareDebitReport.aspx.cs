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
    public partial class rptSelfcareDebitReport : System.Web.UI.Page
    {

        decimal amt = 0;
        DateTime dtime = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Selfcare TCS Report";
            if (!IsPostBack)
            {
                //  Session["RightsKey"] = null;
                Session["RightsKey"] = "N";
                // grdselfcaredebit.PageIndex = 0;
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
                    grdselfcaredebit.Visible = false;
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
                    grdselfcaredebit.Visible = true;
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

            Cls_Business_rptSelfcareDebitReport objTran = new Cls_Business_rptSelfcareDebitReport();
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
                grdselfcaredebit.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                btnGenerateExcel.Visible = true;
                grdselfcaredebit.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdselfcaredebit.DataSource = dt;
                grdselfcaredebit.DataBind();
                DivRoot.Style.Add("display", "block");


            }


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
                grdselfcaredebit.DataSource = dataTable;
                grdselfcaredebit.DataBind();
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

            Cls_Business_rptSelfcareDebitReport objTran = new Cls_Business_rptSelfcareDebitReport();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
                if (dt.Rows.Count > 0)
                {
                    DateTime dd = DateTime.Now;
                    string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                    StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "SelfcareDebit_" + datetime + ".csv");
                    try
                    {
                        int j = 0;
                        String strheader = "Sr.No." + ","
                            + "Transaction Id" + ","
                            + "Receipt No" + ","
                            + "Customer Id" + ","
                            + "VC Id" + ","
                            + "Amount" + ","
                            + "Inserted By" + ","
                            + "Transaction Date" + ","
                            + "User" + ","
                        + "Flag" + ","
                            + "Credit" + ","
                            + "Child Id" + ","
                            + "Deduct Type" + ","
                            + "LCO Code" + ","
                    + "LCO Name" + ","
                    + "Collection Amount" + ","
                    + "LCO GSTN" + ","
                    + "JV GSTN" + ",";
                        ;

                        while (j < dt.Rows.Count)
                        {
                            sw.WriteLine(strheader);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                j = j + 1;
                                string strrow = j.ToString() + ","
                                    + "'" + dt.Rows[i]["num_custpay_transid"].ToString() + ","
                                    + dt.Rows[i]["var_custpay_receiptno"].ToString() + ","
                                + dt.Rows[i]["var_custpay_custid"].ToString() + ","
                                    + dt.Rows[i]["var_custpay_vcid"].ToString() + ","
                                    + dt.Rows[i]["amount"].ToString() + ","
                                    + dt.Rows[i]["var_custpay_insby"].ToString() + ","
                                    + dt.Rows[i]["dat_custpay_transdt"].ToString() + ","
                                    + dt.Rows[i]["var_custpay_user"].ToString() + ","
                                + dt.Rows[i]["var_custpay_page_flag"].ToString() + ","
                                + dt.Rows[i]["var_custpay_das_area"].ToString() + ","
                                + dt.Rows[i]["num_custpay_lco_shares"].ToString() + ","
                                + dt.Rows[i]["var_custpay_deducttype"].ToString() + ","
                                + dt.Rows[i]["var_LCOMST_code"].ToString() + ","
                                + dt.Rows[i]["var_lcomst_name"].ToString() + ","
                                + dt.Rows[i]["Collection_amount"].ToString() + ","
                                + dt.Rows[i]["lco_gstn"].ToString() + ","
                                + dt.Rows[i]["jv_gstn"].ToString() + ","
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
                    Response.Redirect("../MyExcelFile/" + "SelfcareDebit_" + datetime + ".csv");
                }
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdselfcaredebit.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdselfcaredebit.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdselfcaredebit.DataSource = dt;
                grdselfcaredebit.DataBind();



            }
        }

        protected void grdAddPlantopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdselfcaredebit.PageIndex = e.NewPageIndex;
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

            Cls_Business_rptSelfcareDebitReport objTran = new Cls_Business_rptSelfcareDebitReport();
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
                grdselfcaredebit.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                btnGenerateExcel.Visible = true;
                grdselfcaredebit.Visible = true;
                lblSearchMsg.Text = "";
                //ViewState["searched_trans"] = dt;
                grdselfcaredebit.DataSource = dt;
                grdselfcaredebit.DataBind();



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

            Cls_Business_rptSelfcareDebitReport objTran = new Cls_Business_rptSelfcareDebitReport();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];

                if (dt.Rows.Count > 0)
                {
                    DateTime dd = DateTime.Now;
                    string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                    StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "SelfcareDebit_" + datetime + ".xls");
                    try
                    {
                        int j = 0;
                        String strheader = "Sr.No." + Convert.ToChar(9)
                          + "Transaction Id" + Convert.ToChar(9)
                          + "Receipt No" + Convert.ToChar(9)
                          + "Customer Id" + Convert.ToChar(9)
                          + "Amount" + Convert.ToChar(9)
                          + "Inserted By" + Convert.ToChar(9)
                          + "Transaction Date" + Convert.ToChar(9)
                          + "User" + Convert.ToChar(9)
                          + "Flag" + Convert.ToChar(9)
                          + "Credit" + Convert.ToChar(9)
                          + "Deduct Type" + Convert.ToChar(9)
                          + "LCO Code" + Convert.ToChar(9)
                    + "LCO Name" + Convert.ToChar(9)
                    + "Collection Amount" + Convert.ToChar(9)
                    + "LCO GSTN" + Convert.ToChar(9)
                    + "JV GSTN" + Convert.ToChar(9)
                      ;


                        while (j < dt.Rows.Count)
                        {
                            sw.WriteLine(strheader);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                j = j + 1;
                                string strrow = j.ToString() + Convert.ToChar(9)

                                      + dt.Rows[i]["num_custpay_transid"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["var_custpay_receiptno"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_custpay_custid"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["amount"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["var_custpay_insby"].ToString() + Convert.ToChar(9)
                                    + "'" + dt.Rows[i]["dat_custpay_transdt"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["var_custpay_user"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_custpay_page_flag"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_custpay_das_area"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_custpay_deducttype"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_LCOMST_code"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_lcomst_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["Collection_amount"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lco_gstn"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["jv_gstn"].ToString() + Convert.ToChar(9)
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
                    Response.Redirect("../MyExcelFile/" + "SelfcareDebit_" + datetime + ".xls");
                }
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdselfcaredebit.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdselfcaredebit.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdselfcaredebit.DataSource = dt;
                grdselfcaredebit.DataBind();


            }
        }

    }
}