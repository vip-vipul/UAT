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
    public partial class rptcustcandetails : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Pack Cancellation Report";
            if (!IsPostBack)
            {               
                grdTransDet.PageIndex = 0;                             
                btn_genExcel.Visible = false;

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
            }
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
                }
            }
            binddata();
        }
        private Hashtable getAddPlanParamsData()
        {
            string username, operator_id, catid1;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid1 = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);
            }

            string from = txtFrom.Text; ;
            string to = txtTo.Text;                   

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);           
        
            return htSearchParams;
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
           
            Cls_Business_rptcustcandetails objTran = new Cls_Business_rptcustcandetails();
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

            if (dt.Rows.Count == 0)
            {
                grdTransDet.Visible = false;
                lblSearchMsg.Text = "No data found";               
                btn_genExcel.Visible = false;
            }
            else
            {
                grdTransDet.Visible = true;               
                btn_genExcel.Visible = true;
                lblSearchMsg.Text = "";               
                grdTransDet.DataSource = dt;
                grdTransDet.DataBind();
                htResponse["htResponse"] = null;
                dt.Dispose();
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
           
            Cls_Business_rptcustcandetails  objTran = new Cls_Business_rptcustcandetails ();
            Hashtable htResponse = objTran.GetTransationsDet(htAddPlanParams, username);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "PackCancellation_" + datetime + ".xls");
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
                    + "State" + Convert.ToChar(9)
                    + "OBRM Status" + Convert.ToChar(9);


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
                            + dt.Rows[i]["state"].ToString() + Convert.ToChar(9)
                            + dt.Rows[i]["OBRMSTATUS"].ToString() + Convert.ToChar(9);
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
                Response.Redirect("../MyExcelFile/" + "PackCancellation_" + datetime + ".xls");
            }
            

            if (dt.Rows.Count == 0)
            {
                grdTransDet.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {          
                btn_genExcel.Visible = true;
                grdTransDet.Visible = true;
            }
        }

        protected void grdTransDet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTransDet.PageIndex = e.NewPageIndex;
            binddata();
        }
    }
}