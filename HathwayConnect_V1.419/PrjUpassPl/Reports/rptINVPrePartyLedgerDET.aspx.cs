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
    public partial class rptINVPrePartyLedgerDET : System.Web.UI.Page
    {
        decimal opnbal = 0;
        decimal debit = 0;
        decimal credit = 0;
        decimal closebal = 0;

        decimal opnbal1 = 0;
        decimal debit1 = 0;
        decimal credit1 = 0;
        decimal closebal1 = 0;
        DateTime dtime = DateTime.Now;

        string username, catid, operator_id;

        protected void Page_Load(object sender, EventArgs e)
        {

            Master.PageHeading = "Inventory Party Ledger Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                grdPartyLedger.PageIndex = 0;
                grdPLcoDet.PageIndex = 0;
                Session["pagenos"] = "1";


                //if (Session["username"] != null || Session["operator_id"] != null)
                //{
                //    username = Session["username"].ToString();

                //    catid = Convert.ToString(Session["category"]);
                //    operator_id = Convert.ToString(Session["operator_id"]);
                //}



                if (Session["username"] != null || Session["operator_id"] != null)
                {
                    username = Session["username"].ToString();

                    catid = Convert.ToString(Session["category"]);
                    if (catid == "11")
                    {
                        operator_id = Convert.ToString(Session["Soperatorid"]);
                    }
                    else
                    {
                        operator_id = Convert.ToString(Session["operator_id"]);
                    }
                }

                //setting page heading


                if (Request.QueryString["showall"].ToString() == "1")
                {
                    lbllconm.Text = Session["lconame"].ToString();
                }
                else
                {
                    lbllconm.Text = "All";
                }
                binddata();
                binddatalco();
            }
        }

        public void binddata()
        {
            Hashtable htLedgerParams = getLedgerParamsData();

            string username;
            if (Session["username"] != null)
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

            Cls_Business_RptINVLedger objTran = new Cls_Business_RptINVLedger();
            Hashtable htResponse = objTran.GetTransationsDet(htLedgerParams, username, catid, operator_id);

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
                // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>LCO Party Ledger Parameters : </b>" + strParams);
                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'> </b>" + strParams);

            }

            if (dt.Rows.Count == 0)
            {
                btn_genExl.Visible = false;
                grdPartyLedger.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btn_genExl.Visible = true;
                grdPartyLedger.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdPartyLedger.DataSource = dt;
                grdPartyLedger.DataBind();

                //showing result count
                ////lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        public void binddatalco()
        {
            Hashtable htLedgerParams = getLedgerParamsData();
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

            Cls_Business_RptINVLedger objTran = new Cls_Business_RptINVLedger();
            Hashtable htResponse = objTran.GetTransationsDetLCO(htLedgerParams, username, catid, operator_id);

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
                // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>LCO Party Ledger Parameters : </b>" + strParams);
                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (dt.Rows.Count == 0)
            {
                btn_genExl.Visible = false;
                grdPLcoDet.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btn_genExl.Visible = true;
                grdPLcoDet.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdPLcoDet.DataSource = dt;
                grdPLcoDet.DataBind();

                //showing result count
                ////lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        private Hashtable getLedgerParamsData()
        {
            string username, catid, operator_id;

            username = Session["username"].ToString();
            catid = Convert.ToString(Session["category"]);
            operator_id = Convert.ToString(Session["operator_id"]);

            string from = Session["fromdt"].ToString();
            string to = Session["todt"].ToString();
            string lcoid = "";
            string showall = "";

            if (Request.QueryString["showall"] == "0")
            {

                showall = Request.QueryString["showall"].ToString();
            }
            else if (Request.QueryString["showall"] == "1")
            {
                showall = Request.QueryString["showall"].ToString();
                if (catid != "3")
                {
                    lcoid = Session["lcoid"].ToString();
                }
            }
            /*string showall = "";
            if (Request.QueryString["showall"] == "0")
            {
                uid = "";
                showall = Request.QueryString["showall"].ToString();
            }
            else
            {*/

            //showall = "";
            //}

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("lcoid", lcoid);
            htSearchParams.Add("showall", showall);
            //htSearchParams.Add("showall", showall);
            return htSearchParams;
        }

        protected void grdPartyLedger_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.Cells.Count > 0)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            //    {
            //        //LinkButton HLink1 = (LinkButton)e.Row.Cells[2].Controls[0];
            //        //opnbal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "openinbal"));
            //        debit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "drlimit"));
            //        credit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "crlimit"));
            //        //closebal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "closingbal"));
            //    }
            //    else if (e.Row.RowType == DataControlRowType.Footer)
            //    {
            //        ////e.Row.Cells[3].Text = "" + opnbal;
            //        e.Row.Cells[4].Text = "" + debit;
            //        e.Row.Cells[5].Text = "" + credit;
            //        ////e.Row.Cells[6].Text = "" + closebal;
            //        //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
            //        //e.Item.Cells[8].Text = "" + Total;
            //    }
            //}
        }

        protected void grdPartyLedger_Sorting(object sender, GridViewSortEventArgs e)
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
                grdPartyLedger.DataSource = dataTable;
                grdPartyLedger.DataBind();
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

        protected void grdPLcoDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.Cells.Count > 0)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            //    {

            //        opnbal1 += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "openinbal"));
            //        debit1 += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "drlimit"));
            //        credit1 += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "crlimit"));
            //        closebal1 += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "closingbal"));
            //    }
            //    else if (e.Row.RowType == DataControlRowType.Footer)
            //    {
            //        e.Row.Cells[3].Text = "" + opnbal1;
            //        e.Row.Cells[4].Text = "" + debit1;
            //        e.Row.Cells[5].Text = "" + credit1;
            //        e.Row.Cells[6].Text = "" + closebal1;
            //        //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
            //        //e.Item.Cells[8].Text = "" + Total;
            //    }
            //}
        }

        protected void btn_genExl_Click(object sender, EventArgs e)
        {

            Hashtable htLedgerParams = getLedgerParamsData();

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
            string username1, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username1 = Session["username"].ToString();

                catid = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }


            Cls_Business_RptINVLedger objTran = new Cls_Business_RptINVLedger();
            Hashtable htResponse = objTran.GetTransationsDet(htLedgerParams, username, catid, operator_id);




            Cls_Business_RptINVLedger objTran1 = new Cls_Business_RptINVLedger();
            Hashtable htResponse1 = objTran.GetTransationsDetLCO(htLedgerParams, username1, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse1["htResponse"] != null)
            {
                dt = (DataTable)htResponse1["htResponse"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PartyLedger_" + datetime + ".xls");
                try
                {
                    /*int j = 0;

                    String strheader = "LCO Code" + Convert.ToChar(9) + "LCO Name" + Convert.ToChar(9) + "Opening Balance" + Convert.ToChar(9);
                    strheader += "Debit" + Convert.ToChar(9) + "Credit" + Convert.ToChar(9) + "Closing Balance" + Convert.ToChar(9);



                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j= j + 1;
                            string strrow = j.ToString() + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9) + dt.Rows[i]["lconame"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["openinbal"].ToString() + Convert.ToChar(9) + dt.Rows[i]["drlimit"].ToString() + Convert.ToChar(9);
                            strrow += dt.Rows[i]["crlimit"].ToString() + Convert.ToChar(9) + dt.Rows[i]["closingbal"].ToString() + Convert.ToChar(9);
                            sw.WriteLine(strrow);
                        }
                    }*/

                    DataTable dt1 = null; //check for exception
                    if (htResponse["htResponse"] != null)
                    {
                        dt1 = (DataTable)htResponse["htResponse"];
                    }

                    if (dt1 == null)
                    {
                        Response.Redirect("~/ErrorPage.aspx");
                        return;
                    }
                    String strheader1 = "Sr.No." + Convert.ToChar(9)
                        + "Ledger Date" + Convert.ToChar(9)
                        + "Ledger Type" + Convert.ToChar(9)
                        + "Remark" + Convert.ToChar(9);
                    strheader1 += "Debit" + Convert.ToChar(9)
                        + "Credit" + Convert.ToChar(9)
                        + "Balance" + Convert.ToChar(9)
                         ;
                    sw.WriteLine(strheader1);
                    int k = 0;


                    while (k < dt1.Rows.Count)
                    {

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            k = k + 1;
                            string strrow = k.ToString() + Convert.ToChar(9) + "'"
                                + dt1.Rows[i]["dt1"].ToString() + Convert.ToChar(9)
                                + dt1.Rows[i]["ltype"].ToString() + Convert.ToChar(9)
                                + dt1.Rows[i]["premark"].ToString() + Convert.ToChar(9);
                            strrow += dt1.Rows[i]["drlimit"].ToString() + Convert.ToChar(9)
                                + dt1.Rows[i]["crlimit"].ToString() + Convert.ToChar(9);
                            strrow += dt1.Rows[i]["balance"].ToString() + Convert.ToChar(9)
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
                Response.Redirect("../MyExcelFile/" + "PartyLedger_" + datetime + ".xls");
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
                // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>LCO Party Ledger Parameters : </b>" + strParams);
                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'> </b>" + strParams);

            }

            /*if (dt.Rows.Count == 0)
            {
                btn_genExl.Visible = false;
                grdPartyLedger.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btn_genExl.Visible = true;
                grdPartyLedger.Visible = true;
                lblSearchMsg.Text = "";               
            }*/
        }

        protected void grdPLcoDet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPLcoDet.PageIndex = e.NewPageIndex;
            binddatalco();
        }

        protected void grdPartyLedger_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPartyLedger.PageIndex = e.NewPageIndex;
            binddata();
        }
    }
}