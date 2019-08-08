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

namespace PrjUpassPl.Reports
{
    public partial class rptBulkDiscountDet : System.Web.UI.Page
    {

        decimal amt = 0;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Discount Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                grdDiscount.PageIndex = 0;
                //setting page heading
                

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                btngrnExel.Visible = false;
                btnGenerateExcel.Visible = false;
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
                    grdDiscount.Visible = false;
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
                    grdDiscount.Visible = true;
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

            Cls_Business_RptDiscount objTran = new Cls_Business_RptDiscount();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid, operator_id);

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
                
                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (dt.Rows.Count == 0)
            {
                grdDiscount.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                btnGenerateExcel.Visible = true;
                grdDiscount.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdDiscount.DataSource = dt;
                grdDiscount.DataBind();



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

        protected void grdDiscount_Sorting(object sender, GridViewSortEventArgs e)
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
                grdDiscount.DataSource = dataTable;
                grdDiscount.DataBind();
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
                operator_id = Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptDiscount objTran = new Cls_Business_RptDiscount();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "Discount_" + datetime + ".csv");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + ","
                        + "Account No." + ","
                        + "VC/MAC Id" + ","
                        + "First Name" + ","
                        + "Last Name" + ","
                        + "Address" + ","
                        + "Zip" + ","
                        + "City" + ","
                        + "State" + ","
                        + "Customer Type" + ","
                        + "Connection Type" + ","
                        + "Mobile" + ","
                        + "LCO Code" + ","
                        + "LCO Name" + ","
                        + "Discount Amount" + ","
                        + "Discount Type" + ","
                        + "Requested By" + ","
                        + "Reason" + ","
                        + "Expiry Date" + ","
                        + "Inserted By" + ","
                        + "Inserted Date";
                    

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + ","
                                + "'" + dt.Rows[i]["accno"].ToString() + ","
                                + dt.Rows[i]["vc"].ToString() + ","
                                + dt.Rows[i]["fname"].ToString() + ","
                                + dt.Rows[i]["lname"].ToString() + ","
                                + dt.Rows[i]["address"].ToString() + ","
                                + dt.Rows[i]["zip"].ToString() + ","
                                + dt.Rows[i]["city"].ToString() + ","
                                + dt.Rows[i]["state"].ToString() + ","
                                + dt.Rows[i]["custtype"].ToString() + ","
                                + dt.Rows[i]["conntype"].ToString() + ","
                                + dt.Rows[i]["mobile"].ToString() + ","
                                + dt.Rows[i]["lcocode"].ToString() + ","
                                + dt.Rows[i]["lconame"].ToString() + ","
                                + dt.Rows[i]["discountamt"].ToString() + ","
                                +  dt.Rows[i]["DISCOUNTTYPE"].ToString() + ","
                                + dt.Rows[i]["requestedby"].ToString() + ","
                                + dt.Rows[i]["reason"].ToString() + ","
                                +  dt.Rows[i]["expirydt"].ToString() + ","
                                + dt.Rows[i]["insby"].ToString() + ","
                                +  dt.Rows[i]["insdt"].ToString();
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
                Response.AddHeader("Content-disposition", "attachment; filename=Discount_" + datetime + ".csv");
                Response.ContentType = "text/csv";
                Response.Redirect("../MyExcelFile/" + "Discount_" + datetime + ".csv");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdDiscount.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdDiscount.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdDiscount.DataSource = dt;
                grdDiscount.DataBind();

                //showing result count
                //lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        protected void grdDiscount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDiscount.PageIndex = e.NewPageIndex;
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

            Cls_Business_RptDiscount objTran = new Cls_Business_RptDiscount();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid, operator_id);

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
                grdDiscount.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                btnGenerateExcel.Visible = true;
                grdDiscount.Visible = true;
                lblSearchMsg.Text = "";
                //ViewState["searched_trans"] = dt;
                grdDiscount.DataSource = dt;
                grdDiscount.DataBind();



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
                operator_id = Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptDiscount objTran = new Cls_Business_RptDiscount();
            Hashtable htResponse = objTran.GetTransations(htTopupParams, username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "Discount_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)
                        + "Account No." + Convert.ToChar(9)
                        + "VC/MAC Id" + Convert.ToChar(9)
                        + "First Name" + Convert.ToChar(9)
                        + "Last Name" + Convert.ToChar(9)
                        + "Address" + Convert.ToChar(9)
                        + "Zip" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9)
                        + "State" + Convert.ToChar(9)
                        + "Customer Type" + Convert.ToChar(9)
                        + "Connection Type" + Convert.ToChar(9)
                        + "Mobile" + Convert.ToChar(9)
                        + "LCO Code" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "Discount Amount" + Convert.ToChar(9)
                        + "Discount Type" + Convert.ToChar(9)
                        + "Requested By" + Convert.ToChar(9)
                        + "Reason" + Convert.ToChar(9)
                        + "Expiry Date" + Convert.ToChar(9)
                        + "Inserted By" + Convert.ToChar(9)
                        + "Inserted Date" + Convert.ToChar(9);
                        

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + "'" + dt.Rows[i]["accno"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["vc"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["fname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["address"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["zip"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["city"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["state"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["custtype"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["conntype"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["mobile"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lconame"].ToString() + Convert.ToChar(9)
                                +  dt.Rows[i]["discountamt"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["DISCOUNTTYPE"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["requestedby"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["reason"].ToString() + Convert.ToChar(9)
                                +  dt.Rows[i]["expirydt"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["insby"].ToString() + Convert.ToChar(9)
                                +  dt.Rows[i]["insdt"].ToString() + Convert.ToChar(9);
              
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
                Response.Redirect("../MyExcelFile/" + "Discount_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdDiscount.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdDiscount.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdDiscount.DataSource = dt;
                grdDiscount.DataBind();

                //showing result count
                //lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }
    }
}