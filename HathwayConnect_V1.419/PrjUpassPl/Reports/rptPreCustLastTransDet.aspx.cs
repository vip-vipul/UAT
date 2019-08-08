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
    public partial class rptPreCustLastTransDet : System.Web.UI.Page
    {
        decimal custprice = 0;
        decimal lcoprice = 0;
        decimal balance = 0;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Customer Last Five Transactions";
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                grdlasttrans.PageIndex = 0;
                //setting page heading
                
            }
            binddata();
        }

        public void binddata()
        {
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

            Cls_Business_RptLastTrans objTran = new Cls_Business_RptLastTrans();
            Hashtable htResponse = objTran.GetTransations(username, catid, operator_id);

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
            //string strParams = htResponse["ParamStr"].ToString();
            //if (!String.IsNullOrEmpty(strParams))
            //{
            //    lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>Customer Last Transaction Parameters : </b>" + strParams);
            //}

            if (dt.Rows.Count == 0)
            {
                btn_genExl.Visible = false;
                grdlasttrans.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btn_genExl.Visible = true;
                grdlasttrans.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdlasttrans.DataSource = dt;
                grdlasttrans.DataBind();

                //showing result count
                // lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        protected void grdlasttrans_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.Cells.Count > 0)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            //    {
            //        //LinkButton HLink1 = (LinkButton)e.Row.Cells[2].Controls[0];
            //        custprice += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "custprice"));
            //        lcoprice += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "lcoprice"));
            //        balance += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "balance"));
            //    }
            //    else if (e.Row.RowType == DataControlRowType.Footer)
            //    {
            //        e.Row.Cells[9].Text = "" + custprice;
            //        e.Row.Cells[10].Text = "" + lcoprice;
            //        e.Row.Cells[13].Text = "" + balance;
            //        //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
            //        //e.Item.Cells[8].Text = "" + Total;
            //    }
            //}
        }

        protected void grdlasttrans_Sorting(object sender, GridViewSortEventArgs e)
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
                grdlasttrans.DataSource = dataTable;
                grdlasttrans.DataBind();
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

            Cls_Business_RptLastTrans objTran = new Cls_Business_RptLastTrans();
            Hashtable htResponse = objTran.GetTransations(username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
               
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "CustLastTrans_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Transaction ID" + Convert.ToChar(9) + "Receipt No." + Convert.ToChar(9) + "Customer ID" + Convert.ToChar(9) + "Customer Name" + Convert.ToChar(9);
                    strheader += "VC ID" + Convert.ToChar(9) + "Plan ID" + Convert.ToChar(9) + "Plan Name" + Convert.ToChar(9) + "Plan Type" + Convert.ToChar(9) + "Customer Price" + Convert.ToChar(9);
                    strheader += "LCO Price" + Convert.ToChar(9) + "Expiry" + Convert.ToChar(9) + "Payment Term" + Convert.ToChar(9) + "Balance" + Convert.ToChar(9) + "Company Code" + Convert.ToChar(9);
                    strheader += "Status" + Convert.ToChar(9) + "Transaction By" + Convert.ToChar(9) + "'" +"Transaction Date" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {

                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + dt.Rows[i]["transid"].ToString() + Convert.ToChar(9) + dt.Rows[i]["receiptno"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["custid"].ToString() + Convert.ToChar(9) + dt.Rows[i]["custname"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["vcid"].ToString() + Convert.ToChar(9) + dt.Rows[i]["planid"].ToString() + Convert.ToChar(9) + dt.Rows[i]["planname"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["plantype"].ToString() + Convert.ToChar(9) + dt.Rows[i]["custprice"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["lcoprice"].ToString() + Convert.ToChar(9) + "'" + dt.Rows[i]["expirydt"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["payterm"].ToString() + Convert.ToChar(9) + dt.Rows[i]["balance"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["companycode"].ToString() + Convert.ToChar(9) + dt.Rows[i]["flag"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["lconame"].ToString() + Convert.ToChar(9) + "'" + dt.Rows[i]["transdt1"].ToString() + Convert.ToChar(9);

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
                Response.Redirect("../MyExcelFile/" + "CustLastTrans_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            
            if (dt.Rows.Count == 0)
            {
                btn_genExl.Visible = false;
                grdlasttrans.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdlasttrans.Visible = true;
                lblSearchMsg.Text = "";                             
            }
        }

        protected void grdlasttrans_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdlasttrans.PageIndex = e.NewPageIndex;
            binddata();
        }
    }
}