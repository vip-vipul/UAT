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
using System.Data.OracleClient;
using System.Configuration;

namespace PrjUpassPl.Reports
{
    public partial class rptsplcoprecolldetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dtime = DateTime.Now;
            Master.PageHeading = "LCO Collection Details";
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                //setting page heading
               

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.AddDays(-1).ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.AddDays(-1).ToString("dd-MMM-yyyy").Trim();
            }
        }
        private Hashtable getLedgerParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            string search_type = RadSearchby.SelectedValue.ToString();                

            Session["fromdt"] = txtFrom.Text;
            Session["todt"] = txtTo.Text;
            string txtsearch = "";
            if (txtsearchpara.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("T", txtsearchpara.Text);

                if (valid == "")
                {
                    txtsearch = txtsearchpara.Text.ToString().Trim();
                }
               
               // txtsearch = txtsearchpara.Text.ToString().Trim();
            }
            Session["txtsearch"] = txtsearch.ToString().Trim();

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("search", search_type);//added by Rushali
            htSearchParams.Add("txtsearch", txtsearch);//added by Rushali
            return htSearchParams;
        }
        protected void binddata()
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
                    grdExpiry.Visible = false;
                    lblSearchMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else if (Convert.ToDateTime(txtFrom.Text.ToString()) >= DateTime.Now.Date)
                {
                    lblSearchMsg.Text = "You can not select From date later or equal to  current date!"; //"You can not select From date later than 15 days from current date!";
                    return;
                }
                else if (Convert.ToDateTime(txtTo.Text.ToString()) >= DateTime.Now.Date)
                {
                    lblSearchMsg.Text = "You can not select To date later or equal to current date!"; // "You can not select To date later than 15 days from current date!";
                    return;
                }
                else
                {
                    lblSearchMsg.Text = "";
                    grdExpiry.Visible = true;
                }
            }

            Hashtable htAddPlanParams = getLedgerParamsData();
           

            string username, catid, operator_id,search_type;
            string txtsearch = "";

            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);
                search_type = Convert.ToString(Session["search"]);
                txtsearch = Convert.ToString(Session["txtsearch"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            DataTable dt = new DataTable();
           
                Cls_Business_rptsplcoprecolldetails objTran = new Cls_Business_rptsplcoprecolldetails();
                dt = objTran.lcoprecolldetails(htAddPlanParams, username, operator_id, catid);
                if (dt == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    btnGenerateExcel.Visible = false;
                    grdExpiry.Visible = false;
                    lblSearchMsg.Text = "No data found";
                }
                else
                {
                    btnGenerateExcel.Visible = true;
                    grdExpiry.Visible = true;
                    lblSearchMsg.Text = "";
                    ViewState["searched_trans"] = dt;
                    grdExpiry.DataSource = dt;
                    grdExpiry.DataBind();
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdExpiry.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                    DivRoot.Style.Add("display", "block");

                }
                       
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            binddata();
        }

        protected void grdExpiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdExpiry.PageIndex = e.NewPageIndex;
            binddata();
        }

        protected void grdExpiry_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {

            ExportExcel();
        }
        protected void ExportExcel()
        {
            Hashtable htAddPlanParams = getLedgerParamsData();

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
            DataTable dt = new DataTable();

            Cls_Business_rptsplcoprecolldetails objTran = new Cls_Business_rptsplcoprecolldetails();
            dt = objTran.lcoprecolldetails(htAddPlanParams, username, operator_id, catid);

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "lcoprecolldetails_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)
                        + "Account Number" + Convert.ToChar(9)
                        + "Customer Name" + Convert.ToChar(9)
                        + "Entity Code" + Convert.ToChar(9)
                        + "LCO_Name" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9)
                        + "State" + Convert.ToChar(9)
                        + "Area" + Convert.ToChar(9)
                        + "Receipt No." + Convert.ToChar(9)
                        + "Amount" + Convert.ToChar(9)
                        + "Receipt Date" + Convert.ToChar(9)
                        + "Reversal date" + Convert.ToChar(9)
                        + "Created By Username" + Convert.ToChar(9)
                        + "Description" + Convert.ToChar(9)
                        + "Customer Type" + Convert.ToChar(9)
                        + "Payment Mode" + Convert.ToChar(9)
                        + "Cheque No." + Convert.ToChar(9)
                        + "Cheque Date" + Convert.ToChar(9)
                        + "Bank Name" + Convert.ToChar(9)
                        + "Branch Name" + Convert.ToChar(9)
                        + "Bank Code" + Convert.ToChar(9)
                        + "Payment Channel" + Convert.ToChar(9)
                        + "Upass Receipt No." + Convert.ToChar(9)
                        + "Reversal Status" + Convert.ToChar(9)
                        + "JV" + Convert.ToChar(9)
                        + "Distributer" + Convert.ToChar(9)
                        + "Sub Distributer" + Convert.ToChar(9)
                        + "Company" + Convert.ToChar(9)
                        + "Report Date" + Convert.ToChar(9)
                        + "From Date" + Convert.ToChar(9)
                        + "to Date" + Convert.ToChar(9);
                    
                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["account_no"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["customer_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["entity_code"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lco_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["city"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["state"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["area"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["receipt_no"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["amount"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["receipt_date"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["reversal_date"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["created_by_username"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["description"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["customer_type"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["payment_mode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cheque_no"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cheque_date"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["bank_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["branch_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["bank_code"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["payment_channel"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["upass_reciept_no"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["reversal_status"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["jv"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["distributer"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["sub_distributer"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["company"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["report_date"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["from_date"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["to_date"].ToString() + Convert.ToChar(9)
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

                dt.Dispose();
                Response.Redirect("../MyExcelFile/" + "lcoprecolldetails_" + datetime + ".xls");
                
            }
            if (dt.Rows.Count == 0)
            {
                grdExpiry.Visible = false;
                lblSearchMsg.Text = "No data found";
            }

        }

        protected void btnreset_Click(object sender, EventArgs e)
        {
           
            RadSearchby.Items[1].Selected = false;
            txtsearchpara.Enabled = false;
            txtsearchpara.Text = "";
        }

        protected void RadSearchby_SelectedIndexChanged1(object sender, EventArgs e)
        {
            txtsearchpara.Enabled = true;
        }        
    }
}