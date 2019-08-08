using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PrjUpassBLL.Reports;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;
using System.IO;

namespace PrjUpassPl.Reports
{
    public partial class rptserviceActDact : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                grdactdact.PageIndex = 0;
                //setting page heading
                Master.PageHeading = "Service Activation Deactivation Report";

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
            }
        }

        private Hashtable getParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            Session["fromdt"] = txtFrom.Text;
            Session["todt"] = txtTo.Text;


            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("status", Ddlstatus.SelectedValue);
            return htSearchParams;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
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
                    grdactdact.Visible = false;
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
                    grdactdact.Visible = true;
                }
            }

            Hashtable htAddPlanParams = getParamsData();

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

            Cls_Business_rptserviceActDact objTran = new Cls_Business_rptserviceActDact();
            Hashtable htResponse = objTran.Getsvcstatus(htAddPlanParams, username, catid, operator_id);

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

            lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b><b>Transaction From : </b>" + from + "<b>Transaction To : </b>" + to);


            if (dt.Rows.Count == 0)
            {
                btngrnExel.Visible = false;
                grdactdact.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                grdactdact.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdactdact.DataSource = dt;
                grdactdact.DataBind();

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdactdact.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                DivRoot.Style.Add("display", "block");

            }
        }

        protected void btngrnExel_Click(object sender, EventArgs e)
        {
            Hashtable htAddPlanParams = getParamsData();

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

            Cls_Business_rptserviceActDact objTran = new Cls_Business_rptserviceActDact();
            Hashtable htResponse = objTran.Getsvcstatus(htAddPlanParams, username, catid, operator_id);

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

            if (dt.Rows.Count != 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "ServiceActDact_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)

                        + "STB Number" + Convert.ToChar(9)
                        + "Customer Code" + Convert.ToChar(9)
                        //+ "Account Poid" + Convert.ToChar(9)
                        //+ "Service Poid" + Convert.ToChar(9)
                        + "VC Id" + Convert.ToChar(9)
                        + "Status" + Convert.ToChar(9)
                        + "Transaction By" + Convert.ToChar(9)
                        + "Transaction Date" + Convert.ToChar(9)
                        + "Order Id" + Convert.ToChar(9)

                        + "LCO Code" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "Company Name" + Convert.ToChar(9)
                        + "Distributor" + Convert.ToChar(9)
                        + "Sub Distributor" + Convert.ToChar(9)
                        + "State" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9);
                        

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9) + "'";

                            strrow += dt.Rows[i]["STBNO"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["CUSTNO"].ToString() + Convert.ToChar(9)
                                
                                //+ dt.Rows[i]["ACCPOID"].ToString() + Convert.ToChar(9)
                                //+ dt.Rows[i]["SVCPOID"].ToString() + Convert.ToChar(9)
                                +"'"
                                + dt.Rows[i]["VCID"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["STATUS"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["TRANSBY"].ToString() + Convert.ToChar(9)
                                +"'"
                                + dt.Rows[i]["TRANSDT"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["ORDERID"].ToString() + Convert.ToChar(9);

                            strrow += dt.Rows[i]["lcode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lnaame"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["companyname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["distributor"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["subdistributor"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["statename"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cityname"].ToString() + Convert.ToChar(9);
                            
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
                Response.Redirect("../MyExcelFile/" + "ServiceActDact_" + datetime + ".xls");
            }
        }

        protected void grdactdact_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdactdact.PageIndex = e.NewPageIndex;
            Hashtable htAddPlanParams = getParamsData();

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

            Cls_Business_rptserviceActDact objTran = new Cls_Business_rptserviceActDact();
            Hashtable htResponse = objTran.Getsvcstatus(htAddPlanParams, username, catid, operator_id);

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
                btngrnExel.Visible = false;
                grdactdact.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                grdactdact.Visible = true;
                lblSearchMsg.Text = "";
                //ViewState["searched_trans"] = dt;
                grdactdact.DataSource = dt;
                grdactdact.DataBind();
            }
        }

        //protected void grdactdact_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.Cells.Count > 0)
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
        //        {
        //            //LinkButton HLink1 = (LinkButton)e.Row.Cells[2].Controls[0];
        //            crlimit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "crlimit"));
        //            opnbal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "openinbal"));
        //            debit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "drlimit"));
        //            credit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "crlimit"));
        //            closebal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "closingbal"));
        //        }
        //        else if (e.Row.RowType == DataControlRowType.Footer)
        //        {
        //            e.Row.Cells[3].Text = "" + crlimit;
        //            e.Row.Cells[4].Text = "" + opnbal;
        //            e.Row.Cells[5].Text = "" + debit;
        //            e.Row.Cells[6].Text = "" + credit;
        //            e.Row.Cells[7].Text = "" + closebal;
        //        }
        //    }
        //}

    }
}