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
using System.Drawing;
using System.Text;

namespace PrjUpassPl.Reports
{
    public partial class rptAddPlanTransSelfDET : System.Web.UI.Page
    {
        decimal amtdd = 0;
        decimal bal = 0;
        DateTime dtime = DateTime.Now;
        string catid1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Transaction Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                Session["pagenos"] = "1";

                grdTransDet.PageIndex = 0;
                //setting page heading
                
                if (Request.QueryString["showall"] == "0" || Request.QueryString["showall"] == "1" || Request.QueryString["showall"] == "2")
                {
                    lbllconm.Text = "All";
                }
                //else if (Request.QueryString["showall"] == "1")
                //{
                //    lbllconm.Text = Session["lconame"].ToString();
                //}
                else
                {
                    lbllconm.Text = Session["lconame"].ToString();
                }
                btn_genExl.Visible = false;
                btn_genExcel.Visible = false;
                binddata();
            }
        }

        public void binddata()
        {
            Hashtable htAddPlanParams = getAddPlanParamsData();

            string username;
            if (Session["username"] != null)
            {
                username = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_RptAddPlan_Self objTran = new Cls_Business_RptAddPlan_Self();
            Hashtable htResponse = objTran.GetTransationsDet(htAddPlanParams, username);

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
                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>Transaction Parameters : </b>" + strParams);
            }

            if (dt.Rows.Count == 0)
            {
                grdTransDet.Visible = false;
                lblSearchMsg.Text = "No data found";
                btn_genExl.Visible = false;
                btn_genExcel.Visible = false;
            }
            else
            {
                grdTransDet.Visible = true;
                btn_genExl.Visible = true;
                btn_genExcel.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdTransDet.DataSource = dt;
                grdTransDet.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdTransDet.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");

                htResponse["htResponse"] = null;
                dt.Dispose();

                //showing result count
                // lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        private Hashtable getAddPlanParamsData()
        {
            string username, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid1 = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);
            }

            string from = Session["fromdt"].ToString();
            string to = Session["todt"].ToString();
            string uid = "";
            string loperid = "";
            string showall = "";
            string ParentId = "";
            if (Request.QueryString["showall"] == "0")
            {

                showall = Request.QueryString["showall"].ToString();
            }
            else if (Request.QueryString["showall"] == "1")
            {
                showall = Request.QueryString["showall"].ToString();
                ParentId = Session["msoid"].ToString().Trim();
            }
            else if (Request.QueryString["showall"] == "2")
            {
                if (catid1 == "3")
                {
                    loperid = Session["lcoid"].ToString();
                    showall = Request.QueryString["showall"].ToString();
                }
                else
                {
                    loperid = Session["lcoid"].ToString();
                    showall = Request.QueryString["showall"].ToString();
                }
            }
            else
            {
                uid = Session["uid"].ToString();
                showall = "";
            }
            string search_type = Session["search"].ToString().Trim();
            string txtsearch = Session["txtsearch"].ToString().Trim();
          
            string Plantype = Session["plantype"].ToString().Trim();
            string Transtype = Session["transtype"].ToString().Trim();
            string Payterm = Session["Payterm"].ToString().Trim();

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("uid", uid);
            htSearchParams.Add("loperid", loperid);
            htSearchParams.Add("parentid", ParentId);
            htSearchParams.Add("showall", showall);
            htSearchParams.Add("search", search_type);//added by Rushali
            htSearchParams.Add("txtsearch", txtsearch);//added by Rushali
            htSearchParams.Add("Plantype", Plantype);//added by Prashanth
            htSearchParams.Add("Transtype", Transtype);//added by Prashanth
            htSearchParams.Add("Payterm", Payterm);//added by Prashanth
            string tablemonth = Session["tablemonth"].ToString().Trim();
            htSearchParams.Add("tablemonth", tablemonth);//added by Prashanth
            return htSearchParams;
        }

        //protected void grdTransDet_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.Cells.Count > 0)
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
        //        {
        //            amtdd += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "amtdd"));
        //            bal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "bal"));
        //        }
        //        else if (e.Row.RowType == DataControlRowType.Footer)
        //        {
        //            e.Row.Cells[12].Text = "" + amtdd;
        //            e.Row.Cells[15].Text = "" + bal;
        //        }
        //    }
        //}

        protected void grdTransDet_Sorting(object sender, GridViewSortEventArgs e)
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
                grdTransDet.DataSource = dataTable;
                grdTransDet.DataBind();
                dataTable.Dispose();
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


        //below event generates CSV

        private void ExportGridToCSV()
        {
            binddata();

            DateTime dd = DateTime.Now;
            string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PlanAdd_" + datetime + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            grdTransDet.DataBind();
            //To Export all pages
            grdTransDet.AllowPaging = false;

            StringBuilder columnbind = new StringBuilder();
            for (int k = 0; k < grdTransDet.Columns.Count; k++)
            {

                columnbind.Append(grdTransDet.Columns[k].HeaderText + ',');
            }

            columnbind.Append("\r\n");
            for (int i = 0; i < grdTransDet.Rows.Count; i++)
            {
                for (int k = 0; k < grdTransDet.Columns.Count; k++)
                {

                    columnbind.Append(grdTransDet.Rows[i].Cells[k].Text + ',');
                }

                columnbind.Append("\r\n");
            }
            Response.Output.Write(columnbind.ToString());
            Response.Flush();
            Response.End();

        } 

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
            Hashtable htAddPlanParams = getAddPlanParamsData();

            string username;
            if (Session["username"] != null)
            {
                username = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            //Cls_Business_RptAddPlan objTran = new Cls_Business_RptAddPlan();
            //Hashtable htResponse = objTran.GetTransationsDet(htAddPlanParams, username);

            DataTable dt = null; //check for exception
            if (ViewState["searched_trans"] != null)
            {
                dt = (DataTable)ViewState["searched_trans"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PlanAdd_" + datetime + ".csv");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + ","
                        + "Trans ID" + ","
                        + "Customer ID" + ","
                        + "Customer Name" + ","
                        + "Customer Address" + ","
                        + "VC ID" + ","
                        + "Plan Name" + ","
                        + "Plan Type" + ","
                        + "Transaction Type" + ","
                        + "Reason" + ","
                        + "User ID" + ","
                        + "User Name" + ","
                        + "'" + "Transaction Date & Time" + ","
                        + "MRP" + ","
                        + "Amount deducted" + ","
                        + "LCO MRP" + ","
                        + "LCO Discount" + ","
                        + "'" + "Expiry date" + ","
                        + "Pay Term" + ","
                        + "Balance" + ","
                    + "LCO Code" + ","
                    + "LCO Name" + ","
                    + "JV Name" + ","
                    + "ERP LCO A/C" + ","
                    + "Distributor" + ","
                    + "Sub distributor" + ","
                    + "City" + ","
                    + "State" + ","
                    + "DAS Area" + ","
                    + "OBRM Status"+ ","
                    + "Source" + ","
                    + "LCO Share" + ","
                    + "LCO Share Type" + ","
                    + "Discount";
                    

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + ","
                                + dt.Rows[i]["transid"].ToString() +","
                                + dt.Rows[i]["custid"].ToString() + ","
                                + dt.Rows[i]["custname"].ToString() + ","
                                + dt.Rows[i]["custaddr"].ToString() + ","
                                + dt.Rows[i]["vc"].ToString() + ","
                                + dt.Rows[i]["plnname"].ToString() + ","
                                + dt.Rows[i]["plntyp"].ToString() + ","
                                + dt.Rows[i]["flag"].ToString() + ","
                                + dt.Rows[i]["reason"].ToString() + ","
                                + dt.Rows[i]["uname"].ToString() + ","
                                + dt.Rows[i]["userowner"].ToString() + ","
                                + "'" + dt.Rows[i]["tdt"].ToString() + ","
                            + dt.Rows[i]["custprice"].ToString() + ","
                            + dt.Rows[i]["amtdd"].ToString() + ","
                            + dt.Rows[i]["netlcoprice"].ToString() + ","
                            + dt.Rows[i]["LCODISCOUNT"].ToString() + ","
                            + "'" + dt.Rows[i]["expdt"].ToString() + ","
                            + dt.Rows[i]["payterm"].ToString() + "," 
                            + dt.Rows[i]["bal"].ToString() + ","
                            + dt.Rows[i]["lcocode"].ToString() + ","
                            + dt.Rows[i]["lconame"].ToString() + ","
                            + dt.Rows[i]["jvname"].ToString() + ","
                            + dt.Rows[i]["erplco_ac"].ToString() + ","
                            + dt.Rows[i]["distname"].ToString() + "," 
                            + dt.Rows[i]["subdist"].ToString() + ","
                            + dt.Rows[i]["city"].ToString() + ","
                            + dt.Rows[i]["state"].ToString() + ","
                             + dt.Rows[i]["AREA"].ToString() + ","
                             + dt.Rows[i]["OBRMSTATUS"].ToString() + ","
                            + dt.Rows[i]["SFLAG"].ToString() + ","
                            + dt.Rows[i]["LSHARE"].ToString() + ","
                            + dt.Rows[i]["LSHARETYPE"].ToString() + ","
                            + dt.Rows[i]["discount"].ToString() ;

                            
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
                dt.Dispose();
                Response.AddHeader("Content-disposition", "attachment; filename=Topup_" + datetime + ".csv");
                Response.ContentType = "text/csv";
                Response.Redirect("../MyExcelFile/" + "PlanAdd_" + datetime + ".csv");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdTransDet.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btn_genExl.Visible = true;
                btn_genExcel.Visible = true;
                grdTransDet.Visible = true;
               

            }
        }

        protected void grdTransDet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTransDet.PageIndex = e.NewPageIndex;
            binddata();
        }

        //below event generates excel

        protected void ExportExcel()
        {
            binddata();
            DateTime dd = DateTime.Now;
            string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PlanAdd_" + datetime + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                grdTransDet.DataBind();
                //To Export all pages
                grdTransDet.AllowPaging = false;

                //string hdnslctcols = hdnslctcolumns.Value;

                //Cls_Business_RptCustEcsDetails objTran = new Cls_Business_RptCustEcsDetails();
                //DataTable dt = objTran.getCustEcsDetails(username, catid, operator_id, "", hdnslctcols);

                grdTransDet.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grdTransDet.HeaderRow.Cells)
                {
                    cell.BackColor = grdTransDet.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grdTransDet.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grdTransDet.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grdTransDet.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                grdTransDet.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

        }


        protected void btn_genExcel_Click(object sender, EventArgs e)
        {

            Hashtable htAddPlanParams = getAddPlanParamsData();

            string username;
            if (Session["username"] != null)
            {
                username = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            //Cls_Business_RptAddPlan objTran = new Cls_Business_RptAddPlan();
            //Hashtable htResponse = objTran.GetTransationsDet(htAddPlanParams, username);

            DataTable dt = null; //check for exception
            if (ViewState["searched_trans"] != null)
            {
                dt = (DataTable)ViewState["searched_trans"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PlanAdd_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)
                        + "Trans ID" + Convert.ToChar(9)
                        + "Customer ID" + Convert.ToChar(9)
                        + "Customer Name" + Convert.ToChar(9)
                        + "Customer Address" + Convert.ToChar(9)
                        + "VC ID" + Convert.ToChar(9)
                        + "Plan Name" + Convert.ToChar(9)
                        + "Plan Type" + Convert.ToChar(9)
                        + "Transaction Type" + Convert.ToChar(9)
                        + "Reason" + Convert.ToChar(9)
                        + "User ID" + Convert.ToChar(9)
                        + "User Name" + Convert.ToChar(9)
                        + "'" + "Transaction Date & Time" + Convert.ToChar(9)
                        + "MRP" + Convert.ToChar(9)
                        + "Amount deducted" + Convert.ToChar(9)
                        + "LCO MRP" + Convert.ToChar(9)
                        + "LCO Discount" + Convert.ToChar(9)
                        + "'" + "Expiry date" + Convert.ToChar(9)
                        + "Pay Term" + Convert.ToChar(9)
                        + "Balance" + Convert.ToChar(9)
                    + "LCO Code" + Convert.ToChar(9)
                    + "LCO Name" + Convert.ToChar(9)
                    + "JV Name" + Convert.ToChar(9)
                    + "ERP LCO A/C" + Convert.ToChar(9)
                    + "Distributor" + Convert.ToChar(9)
                    + "Sub distributor" + Convert.ToChar(9)
                    + "City" + Convert.ToChar(9)
                    + "State" + Convert.ToChar(9)
                    + "DAS Area" + Convert.ToChar(9)
                    + "OBRM Status" + Convert.ToChar(9)
                    + "Source" + Convert.ToChar(9)
                    + "LCO Share" + Convert.ToChar(9)
                    + "LCO Share Type" + Convert.ToChar(9)
                    + "Discount" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["transid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["custid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["custname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["custaddr"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["vc"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["plnname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["plntyp"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["flag"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["reason"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["uname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["userowner"].ToString() + Convert.ToChar(9)
                                + "'" + dt.Rows[i]["tdt"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["custprice"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["amtdd"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["netlcoprice"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["LCODISCOUNT"].ToString() + Convert.ToChar(9)
                            + "'" + dt.Rows[i]["expdt"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["payterm"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["bal"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["lconame"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["jvname"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["erplco_ac"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["distname"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["subdist"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["city"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["state"].ToString() + Convert.ToChar(9)
                             + dt.Rows[i]["AREA"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["OBRMSTATUS"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["SFLAG"].ToString() + Convert.ToChar(9)
                             + dt.Rows[i]["LSHARE"].ToString() + Convert.ToChar(9)
                              + dt.Rows[i]["LSHARETYPE"].ToString() + Convert.ToChar(9)
                              + dt.Rows[i]["discount"].ToString() + Convert.ToChar(9);
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
                dt.Dispose();
                Response.Redirect("../MyExcelFile/" + "PlanAdd_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                grdTransDet.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btn_genExl.Visible = true;
                btn_genExcel.Visible = true;
                grdTransDet.Visible = true;
            }
        }
    }
}