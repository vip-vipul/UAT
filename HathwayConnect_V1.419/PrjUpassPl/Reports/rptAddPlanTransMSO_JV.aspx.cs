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
    public partial class rptAddPlanTransMSO_JV : System.Web.UI.Page
    {
        decimal amt = 0;
        decimal cnt = 0;
        DateTime dtime = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Transaction Report";
            Session["RightsKey"] = "N";



            if (!IsPostBack)
            {

                grdAddPlanSearch.PageIndex = 0;
                //setting page heading
                Session["pagenos"] = "1";


                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();


                //RadSearchby.Items[0].Attributes.CssStyle.Add("visibility", "hidden");
                //RadSearchby.Items[2].Attributes.CssStyle.Add("visibility", "hidden");
                RadSearchby.Items[1].Selected = false;

            }
            // 
        }


        private Hashtable getAddPlanParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            string search_type = RadSearchby.SelectedValue.ToString();
            Session["fromdt"] = txtFrom.Text;
            Session["todt"] = txtTo.Text;
            Session["search"] = search_type.ToString().Trim();

            string txtsearch = "";
            if (txtsearchpara.Text.Length > 0)
            {
                txtsearch = txtsearchpara.Text.ToString().Trim();
            }
            Session["txtsearch"] = txtsearch.ToString().Trim();
            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("search", search_type);//added by Rushali
            htSearchParams.Add("txtsearch", txtsearch);//added by Rushali
            // Added By RP on 09/10/2017 
            if (ddlPlantype.SelectedItem.Text != "All")
            {
                Session["plantype"] = ddlPlantype.SelectedItem.Text;
                htSearchParams.Add("plantype", ddlPlantype.SelectedItem.Text);
            }
            else
            {
                Session["plantype"] = "All";
                htSearchParams.Add("plantype", "All");
            }
            if (ddlTranType.SelectedItem.Text != "All")
            {
                // htSearchParams.Add("type", "a7");
                Session["transtype"] = ddlTranType.SelectedItem.Text;
                htSearchParams.Add("transtype", ddlTranType.SelectedItem.Text);
            }
            else
            {
                Session["transtype"] = "All";
                htSearchParams.Add("transtype", "All");
            }
            if (ddlPayTerm.SelectedItem.Text != "All")
            {
                Session["Payterm"] = ddlPayTerm.SelectedItem.Text;
                htSearchParams.Add("Payterm", ddlPayTerm.SelectedItem.Text);
            }
            else
            {
                Session["Payterm"] = "All";
                htSearchParams.Add("Payterm", "All");
            }
            //htSearchParams.Add("Payterm", ddlPayTerm.SelectedValue.Trim());


            return htSearchParams;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string from = txtFrom.Text;
            string to = txtTo.Text;
            DateTime fromDt;
            DateTime toDt;

            //if (RadSearchby.Items[1].Selected == true ||(txtsearchpara.Text == ""))               
            //{
            //    Response.Write("<script type=\"text/javascript\">alert('Please Enter VC No.');</script>");
            //    return;

            //}
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
            {
                fromDt = new DateTime();
                toDt = new DateTime();
                fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                if (toDt.CompareTo(fromDt) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";
                    grdAddPlanSearch.Visible = false;
                    btnAll.Visible = false;
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
                    grdAddPlanSearch.Visible = true;
                    btnAll.Visible = true;
                }
            }

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
            string search_type = RadSearchby.SelectedValue.ToString();
            Session["search"] = search_type.ToString().Trim();

            string txtsearch = "";
            if (txtsearchpara.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("T", txtsearchpara.Text);

                if (valid == "")
                    txtsearch = txtsearchpara.Text.ToString().Trim();
                else
                {
                    lblSearchMsg.Text = valid.ToString();
                    btnAll.Visible = false;
                    return;

                }

            }
            Session["txtsearch"] = txtsearch.ToString().Trim();
            if (catid == "3")
            {
                Session["lcoid"] = operator_id.ToString();
                Session["lconame"] = "All";
                Session["showall"] = "1";
                Response.Redirect("../Reports/rptAddPlanTransUSER_JV.aspx");//?lcoid=" + operator_id.ToString());// + "'" + "lcoid=" + operator_id.ToString());              
            }
            if (catid == "11")
            {
                Session["msoid"] = operator_id.ToString();
                Session["msoname"] = "All";
                Session["showall"] = "1";
                Response.Redirect("../Reports/rptAddPlanTransLCO_JV.aspx");
            }
            if (catid == "2")
            {
                Session["msoid"] = operator_id.ToString();
                Session["msoname"] = "All";
                Session["showall"] = "1";
                Response.Redirect("../Reports/rptAddPlanTransLCO_JV.aspx");
            }
            if (catid != "0" && catid != "10")
            {
                Cls_Business_RptAddPlan_JV objTran = new Cls_Business_RptAddPlan_JV();
                Hashtable htResponse = objTran.GetTransationsMSO(htAddPlanParams, username, catid, operator_id);

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

                //showing parameters
                string strParams = htResponse["ParamStr"].ToString();
                if (!String.IsNullOrEmpty(strParams))
                {
                    // lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'>Transaction Parameters : </b>" + strParams);
                    lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'> </b>" + strParams);

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
            else if (catid == "0" || catid == "10")
            {
                ExportExcel(from, to);



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

            string from = txtFrom.Text;
            string to = txtTo.Text;
            string uid = "";
            string loperid = "";
            string ParentId = "";
            string search_type = RadSearchby.SelectedValue.ToString();
            Session["search_type"] = search_type.ToString().Trim();

            string txtsearch = "";
            if (txtsearchpara.Text.Length > 0)
            {
                txtsearch = txtsearchpara.Text.ToString().Trim();
            }
            Session["txtsearch"] = txtsearch.ToString().Trim();
            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("uid", uid);
            htSearchParams.Add("loperid", loperid);
            htSearchParams.Add("parentid", ParentId);
            htSearchParams.Add("showall", "0");
            htSearchParams.Add("search", search_type);//added by Rushali
            htSearchParams.Add("txtsearch", txtsearch);//added by Rushali
            return htSearchParams;
        }
        protected void btnAll_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            ExportExcel(from, to);
        }

        protected void ExportExcel(string from, string to)
        {
            //Response.Redirect("../Reports/rptAddPlanTransDET.aspx?showall=0");
            Hashtable ht = getExcelParamsData();
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

            Cls_Business_RptAddPlan_JV objTran = new Cls_Business_RptAddPlan_JV();
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
                ScriptManager.RegisterStartupScript(this, GetType(), "onComplete", "onComplete();", true);
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
                    e.Row.Cells[2].Text = "" + amt;
                    e.Row.Cells[3].Text = "" + cnt;
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
            if (e.CommandName.Equals("MSOName"))
            {
                try
                {
                    GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    Session["showall"] = null;
                    Session["msoid"] = ((Label)clickedRow.FindControl("lblOperid1")).Text;
                    Session["msoname"] = ((Label)clickedRow.FindControl("lblolconame")).Text;
                    //Session["filename"] = ((Label)e.CommandSource).FindControl("lblfilename").Text;

                }
                catch (Exception ex)
                {
                    Response.Redirect("../errorPage.aspx");
                }
                Response.Redirect("../Reports/rptAddPlanTransLCO_JV.aspx");
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
            Cls_Business_RptAddPlan_JV objTran = new Cls_Business_RptAddPlan_JV();
            Hashtable htResponse = objTran.GetTransationsMSO(htAddPlanParams, username, catid, operator_id);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PlanAddMSO_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "SrNo" + Convert.ToChar(9)
                        + "MSO Name" + Convert.ToChar(9)

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
                                + dt.Rows[i]["msoname"].ToString() + Convert.ToChar(9)

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
                Response.Redirect("../MyExcelFile/" + "PlanAddMSO_" + datetime + ".xls");

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
            Cls_Business_RptAddPlan_JV objTran = new Cls_Business_RptAddPlan_JV();
            Hashtable htResponse = objTran.GetTransationsMSO(htAddPlanParams, username, catid, operator_id);

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

        protected void RadSearchby_SelectedIndexChanged1(object sender, EventArgs e)
        {
            txtsearchpara.Enabled = true;


        }

        protected void btnreset_Click(object sender, EventArgs e)
        {
            RadSearchby.Items[1].Selected = false;
            txtsearchpara.Enabled = false;
            txtsearchpara.Text = "";
            lblSearchMsg.Text = "";

        }

    }
}