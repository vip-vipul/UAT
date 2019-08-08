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
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Reports
{
    public partial class rptbulkACD_DEACTSch : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                grdactdact.PageIndex = 0;
                //setting page heading
                Master.PageHeading = "Bulk ACT & DEACT Scheduler Report";

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
            Binddata();   
        }

        public void Binddata()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
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

            Hashtable htAddPlanParams = getParamsData();
            Hashtable htResponse =new Hashtable();
            if (rblFlag.SelectedValue == "1")
            {
                Cls_Business_rptserviceActDact objTran = new Cls_Business_rptserviceActDact();
                htResponse = objTran.GetBulkSCHstatus(htAddPlanParams, username, operator_id);
            }
            else if (rblFlag.SelectedValue == "2")
            {
                Cls_Business_rptserviceActDact objTran = new Cls_Business_rptserviceActDact();
                 htResponse = objTran.GetBulkSCHDISstatus(htAddPlanParams, username, operator_id);
            
            }
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
                if (rblFlag.SelectedValue == "1")
                {
                    grdactdact.Columns[10].Visible = false;
                    grdactdact.Columns[11].Visible = false;
                    grdactdact.Columns[12].Visible = true;
                }
                else
                {
                    grdactdact.Columns[12].Visible = false;
                }
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
            Hashtable htResponse = objTran.GetBulkSCHstatus(htAddPlanParams, username, operator_id);

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

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "BulkScheduler_ActDact_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)
                        + "Account No" + Convert.ToChar(9)
                        + "VC Id" + Convert.ToChar(9)
                        + "LCO Code" + Convert.ToChar(9)
                        + "Status" + Convert.ToChar(9)
                        + "Reason" + Convert.ToChar(9)
                        + "Process Flag" + Convert.ToChar(9)
                        + "Scheduler Date" + Convert.ToChar(9)
                        + "Inserted By" + Convert.ToChar(9)
                        + "Inserted Date" + Convert.ToChar(9); 

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9) + "'";


                            strrow += dt.Rows[i]["accno"].ToString() + Convert.ToChar(9)
                                + "'"
                                + dt.Rows[i]["VCID"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lcocode"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["ACTION"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["reason"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["PROCESSFLAG"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["SCHDATE"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["insby"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["insdt"].ToString() + Convert.ToChar(9);
                               
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
                Response.Redirect("../MyExcelFile/" + "BulkScheduler_ActDact_" + datetime + ".xls");
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
            Hashtable htResponse = objTran.GetBulkSCHstatus(htAddPlanParams, username,  operator_id);

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

        

        protected void grdactdact_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Remove"))
            {
                try
                {
                    GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    string lblbulkid = ((Label)clickedRow.FindControl("lblbulkid")).Text;
                    Session["Bulk_ID"] = lblbulkid;
                    popMsgBox.Show();
                    
                }
                catch (Exception ex)
                {
                    Response.Write("Error : " + ex.Message.Trim());
                    return;
                }

            }

        }
        protected void btncnfmBlck_Click(object sender, EventArgs e)
        {
            try
            {
                Cls_Business_rptserviceActDact obj = new Cls_Business_rptserviceActDact();
                string pro_output = obj.bulkUploadActTempRemove(Session["Bulk_ID"].ToString(), Session["username"].ToString().Trim());
                if (pro_output.Split('#')[0] == "9999")
                {
                    lblSearchMsg.Text = "Selected Data Removed Successfully";
                    Binddata();
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error : " + ex.Message.Trim());
                return;
            }
        }
    }
}