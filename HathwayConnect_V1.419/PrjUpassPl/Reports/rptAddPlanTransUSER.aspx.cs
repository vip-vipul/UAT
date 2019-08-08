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
    public partial class rptAddPlanTransUSER : System.Web.UI.Page
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
                grdTransUserDet.PageIndex = 0;
                //setting page heading
                
                if (Session["category"].ToString() == "3")
                {
                    lbllconm.Text = "";
                    Label1.Visible = false;
                }
                else
                {
                    Label1.Visible = true;
                    lbllconm.Text = Session["lconame"].ToString();
                }
                binddata();
            } 
        }

        public void binddata()
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
            Hashtable htResponse = objTran.GetTransationsU(htAddPlanParams, username, catid, operator_id);

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
               // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>Transaction Parameters : </b>" + strParams);
                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b>" + strParams);

            }

            if (dt.Rows.Count == 0)
            {
                btn_genExl.Visible = false;
                grdTransUserDet.Visible = false;
                lblSearchMsg.Text = "No data found";
               btnAll.Visible = false;
            }
            else
            {
                btn_genExl.Visible = true;
                grdTransUserDet.Visible = true;
                lblSearchMsg.Text = "";
                btnAll.Visible = true;
                ViewState["searched_trans"] = dt;
                grdTransUserDet.DataSource = dt;
                grdTransUserDet.DataBind();
                htResponse["htResponse"] = null;
                dt.Dispose();

                //showing result count
               // lblResultCount.Text = Server.HtmlDecode("<b>Showing Top " + dt.Rows.Count.ToString() + " Matching Results</b>");

                //to get transaction type on frmSerTransDetails.aspx

            }
        }

        private Hashtable getAddPlanParamsData()
        {
            string from = Session["fromdt"].ToString();
            string to = Session["todt"].ToString();
            string search_type = Session["search"].ToString().Trim();
            string txtsearch = Session["txtsearch"].ToString().Trim();
            string Plantype= Session["plantype"].ToString().Trim();
            string Transtype = Session["transtype"].ToString().Trim();
            string Payterm = Session["Payterm"].ToString().Trim();
            string lcoid = "";
            if (Session["category"].ToString() == "3")
            {
                lcoid = Session["lcoid"].ToString();
            }
            else
            {
                lcoid = Session["lcoid"].ToString();
            }
            string showall = "";

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("lcoid", lcoid);
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

        protected void grdTransUserDet_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    e.Row.Cells[4].Text = "" + amt;
                    e.Row.Cells[5].Text = "" + cnt;
                    //(e.Row.FindControl("LinkButton2") as LinkButton).Text = "" + amt;
                    //e.Item.Cells[8].Text = "" + Total;
                }
            }
        }

        protected void grdTransUserDet_Sorting(object sender, GridViewSortEventArgs e)
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
                grdTransUserDet.DataSource = dataTable;
                grdTransUserDet.DataBind();
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

        protected void grdTransUserDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("UserName1"))
            {
                try
                {
                    GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    Session["showall"] = null;
                    Session["uid"] = ((Label)clickedRow.FindControl("lbluid1")).Text;
                    Session["lconame"] = ((Label)clickedRow.FindControl("lbllco")).Text;
                    //Session["filename"] = ((Label)e.CommandSource).FindControl("lblfilename").Text;

                }
                catch (Exception ex)
                {
                    Response.Redirect("../errorPage.aspx");
                }
                Response.Redirect("../Reports/rptAddPlanTransDET.aspx?showall=U");
            }            
        }

        protected void btnAll_Click(object sender, EventArgs e)
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
            /*if (catid == "3")
            {
                Response.Redirect("../Reports/rptAddPlanTransDET.aspx?lcoid=" + Request.QueryString["lcoid"].ToString() + "&lconame=ALL&showall=1");
            }
            else
            {*/
                Response.Redirect("../Reports/rptAddPlanTransDET.aspx?lcoid='" + Session["lcoid"].ToString() + "'&lconame='" + Session["lconame"].ToString() + "'&showall=2");
            //}
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
            Hashtable htResponse = objTran.GetTransationsU(htAddPlanParams, username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PlanAddUser_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "SrNo" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "User Id" + Convert.ToChar(9)
                        + "User Name" + Convert.ToChar(9)
                    //    + "Balance" + Convert.ToChar(9)
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
                                + dt.Rows[i]["uname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["userowner"].ToString() + Convert.ToChar(9)                                
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
                Response.Redirect("../MyExcelFile/" + "PlanAddUser_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
        }

        protected void grdTransUserDet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTransUserDet.PageIndex = e.NewPageIndex;
            binddata();
        }
    }
}