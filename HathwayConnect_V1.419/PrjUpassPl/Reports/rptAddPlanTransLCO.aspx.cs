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
    public partial class rptAddPlanTransLCO : System.Web.UI.Page
    {
        decimal amt = 0;
        decimal cnt = 0;
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Transaction Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                grdAddPlanSearch.PageIndex = 0;
                //setting page heading
                
                Session["pagenos"] = "1";


                if (Session["category"].ToString() == "2")
                {
                    lblmsonm.Text = "";
                    Label1.Visible = false;
                }
                else
                {
                    Label1.Visible = true;
                    lblmsonm.Text = Session["msoname"].ToString();
                }

                Binddata();
            }
        }


        private Hashtable getAddPlanParamsData()
        {
            string from = Session["fromdt"].ToString();
            string to = Session["todt"].ToString();
            string msoid = "";

            msoid = Session["msoid"].ToString();

            string showall = "";

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("msoid", msoid);
            htSearchParams.Add("showall", showall);
            string tablemonth = Session["tablemonth"].ToString().Trim();
            htSearchParams.Add("tablemonth", tablemonth);//added by Prashanth
            return htSearchParams;
        }

        public void Binddata()
        {
            lblSearchMsg.Text = "";
            grdAddPlanSearch.Visible = true;
            btnAll.Visible = true;

            Hashtable htAddPlanParams = getAddPlanParamsData();

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

            //if (catid == "3")
            //{
            //    Session["lcoid"] = operator_id.ToString();
            //    Session["lconame"] = "All";
            //    Session["showall"] = "1";
            //    Response.Redirect("../Reports/rptAddPlanTransUSER.aspx");//?lcoid=" + operator_id.ToString());// + "'" + "lcoid=" + operator_id.ToString());              
            //}


            Cls_Business_RptAddPlan objTran = new Cls_Business_RptAddPlan();
            Hashtable htResponse = objTran.GetTransations(htAddPlanParams, username, catid, operator_id);

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

            string strParams = htResponse["ParamStr"].ToString();
            if (!String.IsNullOrEmpty(strParams))
            {
                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);
            }


            if (dt.Rows.Count == 0)
            {
                grdAddPlanSearch.Visible = false;
                btnAll.Visible = false;
                lblSearchMsg.Text = "No data found";
                btn_genExl.Visible = false;
            }
            else
            {
                btn_genExl.Visible = true;
                grdAddPlanSearch.Visible = true;
                btnAll.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdAddPlanSearch.DataSource = dt;
                grdAddPlanSearch.DataBind();
                htResponse["htResponse"] = null;
                dt.Dispose();

            }
        }

        private Hashtable getExcelParamsData()
        {
            string username, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                operator_id = Convert.ToString(Session["operator_id"]);
            }

            string from = Session["fromdt"].ToString();
            string to = Session["todt"].ToString();
            string uid = "";
            string loperid = "";
            string ParentId = Session["msoid"].ToString().Trim();

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("uid", uid);
            htSearchParams.Add("loperid", loperid);
            htSearchParams.Add("parentid", ParentId);
            htSearchParams.Add("showall", "1");
            return htSearchParams;
        }

        protected void btnAll_Click(object sender, EventArgs e)
        {
            //Response.Redirect("../Reports/rptAddPlanTransDET.aspx?showall=1");
            string username;
            Hashtable ht = getExcelParamsData();
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
            Cls_Business_RptAddPlan objTran = new Cls_Business_RptAddPlan();
            Hashtable htResponse = objTran.GetTransationsDet(ht, username);

            DataTable dt = null; //check for exception
            dt = (DataTable)htResponse["htResponse"];
            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            else
            {
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
                    + "State" + Convert.ToChar(9);

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
                            + dt.Rows[i]["state"].ToString() + Convert.ToChar(9);
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
                htResponse["htResponse"] = null;
                dt.Dispose();
                Response.Redirect("../MyExcelFile/" + "PlanAdd_" + datetime + ".xls");
            }
        }

        protected void grdAddPlanSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
                {

                    //LinkButton HLink1 = (LinkButton)e.Row.Cells[2].Controls[0];
                    amt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "amtdd"));
                    cnt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "cnt"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[3].Text = "" + amt;
                    e.Row.Cells[4].Text = "" + cnt;
                    //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
                    //e.Item.Cells[8].Text = "" + Total;
                }
            }
        }

        protected void grdAddPlanSearch_Sorting(object sender, GridViewSortEventArgs e)
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
                grdAddPlanSearch.DataSource = dataTable;
                grdAddPlanSearch.DataBind();
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

        protected void grdAddPlanSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("LcoName1"))
            {
                try
                {
                    GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    //Session["showall"] = null;
                    Session["lcoid"] = ((Label)clickedRow.FindControl("lblOperid1")).Text;
                    Session["lconame"] = ((Label)clickedRow.FindControl("lblolconame")).Text;
                    //Session["filename"] = ((Label)e.CommandSource).FindControl("lblfilename").Text;

                }
                catch (Exception ex)
                {
                    Response.Redirect("../errorPage.aspx");
                }
                Response.Redirect("../Reports/rptAddPlanTransUSER.aspx");
            }
        }

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
            Hashtable htAddPlanParams = getAddPlanParamsData();

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
            Cls_Business_RptAddPlan objTran = new Cls_Business_RptAddPlan();
            Hashtable htResponse = objTran.GetTransations(htAddPlanParams, username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PlanAddLCO_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "SrNo" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "LCO Code" + Convert.ToChar(9)
                        //+ "Balance" + Convert.ToChar(9)
                        + "Amount Deducted" + Convert.ToChar(9)
                        + "Count" + Convert.ToChar(9);



                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lconame"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9)
                                //+ dt.Rows[i]["amt"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["amtdd"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cnt"].ToString() + Convert.ToChar(9);

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
                htResponse["htResponse"] = null;
                dt.Dispose();
                Response.Redirect("../MyExcelFile/" + "PlanAddLCO_" + datetime + ".xls");

            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
        }

        protected void grdAddPlanSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAddPlanSearch.PageIndex = e.NewPageIndex;
            Hashtable htAddPlanParams = getAddPlanParamsData();

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
            Cls_Business_RptAddPlan objTran = new Cls_Business_RptAddPlan();
            Hashtable htResponse = objTran.GetTransations(htAddPlanParams, username, catid, operator_id);

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
            if (dt.Rows.Count == 0)
            {
                grdAddPlanSearch.Visible = false;
                btnAll.Visible = false;
                lblSearchMsg.Text = "No data found";
                btn_genExl.Visible = false;
            }
            else
            {
                btn_genExl.Visible = true;
                grdAddPlanSearch.Visible = true;
                btnAll.Visible = true;
                lblSearchMsg.Text = "";
                // ViewState["searched_trans"] = dt;
                grdAddPlanSearch.DataSource = dt;
                grdAddPlanSearch.DataBind();
                htResponse["htResponse"] = null;
                dt.Dispose();
            }
        }
    }
}