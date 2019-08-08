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
    public partial class rptSmsDelivery : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "SMS Delivery Report";
            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                //setting page heading
                

                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();

                FillLcoDetails();

            }
        }

        protected void FillLcoDetails()
        {
            string str = "";
            string operator_id = "";
            string category_id = "";
            if (Session["operator_id"] != null && Session["category"] != null)
            {
                operator_id = Convert.ToString(Session["operator_id"]);
                category_id = Convert.ToString(Session["category"]);
            }
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {

                str = "   SELECT '('||var_lcomst_code||')'||a.var_lcomst_name name,var_lcomst_code lcocode ";
                str += "     FROM aoup_lcopre_lco_det a ,aoup_operator_def c,aoup_user_def u ";
                str += "  WHERE a.num_lcomst_operid = c.num_oper_id and  a.num_lcomst_operid=u.num_user_operid and u.var_user_username=a.var_lcomst_code  ";
                if (category_id == "11")
                {
                    str += "  and c.num_oper_clust_id =" + operator_id;
                }
                else if (category_id == "3")
                {
                    str += "and a.num_lcomst_operid =  " + operator_id + " ";
                }
                else
                {

                    //  lblmsg.Text = "No LCO Details Found";
                    // Paydet.Visible = false;
                    // btnSubmit.Visible = false;
                    //  divdet.Visible = false;
                    // pnllco.Visible = false;
                    return;
                }
                DataTable tbllco = GetResult(str);

                if (tbllco.Rows.Count > 0)
                {
                    // pnllco.Visible = true;
                    ddlLco.DataTextField = "name";
                    ddlLco.DataValueField = "lcocode";

                    ddlLco.DataSource = tbllco;
                    ddlLco.DataBind();
                    //if (category_id == "11")
                    //{
                    //    ddlLco.Items.Insert(0, new ListItem("Select LCO", "0"));
                    //}
                    //else if (category_id == "3")
                    //{
                    //    //ddllco_SelectedIndexChanged(null, null);
                    //}
                }
                else
                {
                    //  lblmsg.Text = "No LCO Details Found";
                    // divdet.Visible = false;
                    // Paydet.Visible = false;
                    // btnSubmit.Visible = false;
                    // pnllco.Visible = false;
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }

        }
        public DataTable GetResult(String Query)
        {
            DataTable MstTbl = new DataTable();


            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            con.Open();

            OracleCommand Cmd = new OracleCommand(Query, con);
            OracleDataAdapter AdpData = new OracleDataAdapter();
            AdpData.SelectCommand = Cmd;
            AdpData.Fill(MstTbl);

            con.Close();

            return MstTbl;
        }



        private Hashtable getLedgerParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            Session["fromdt"] = txtFrom.Text;
            Session["todt"] = txtTo.Text;

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("lco", ddlLco.SelectedValue.ToString().Trim());
            return htSearchParams;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            binddata();
        }
        protected void binddata()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            DateTime boundry_start = DateTime.Now.Date.AddDays(-15);
            DateTime boundry_end = DateTime.Now.Date.AddDays(15);
            string date_err_msg = "Date should not be greater than current date";
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

                else if (Convert.ToDateTime(txtFrom.Text.ToString()).Date > DateTime.Now.Date)
                {
                    lblSearchMsg.Text = date_err_msg;//"You can not select From date later than 15 days from current date!";
                    return;
                }
                else if (Convert.ToDateTime(txtTo.Text.ToString()).Date > DateTime.Now.Date)
                {
                    lblSearchMsg.Text = date_err_msg;// "You can not select To date earlier than 15 days from current date!";
                    return;
                }
               
               
                else
                {
                    lblSearchMsg.Text = "";
                    grdExpiry.Visible = true;
                }
            }

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
            if (catid != "0" && catid != "10")
            {
                Cls_Business_rptSMSDelivery objTran = new Cls_Business_rptSMSDelivery();
                dt = objTran.GetDetails(htAddPlanParams, username, operator_id, catid);                
                if (dt == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }

                lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b><b>Details From : </b>" + from + "<b> To : </b>" + to);

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
                }
            }
            else if (catid == "0" || catid == "10")
            {
                ExportExcel(from, to);
            }
        }
        
        protected void grdExpiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdExpiry.PageIndex = e.NewPageIndex;
            binddata();
        }

        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            ExportExcel(from, to);
        }
        protected void ExportExcel(string from, string to)
        {

            DateTime boundry_start = DateTime.Now.Date.AddDays(-15);
            DateTime boundry_end = DateTime.Now.Date.AddDays(15);
            string date_err_msg = "Date should not be greater than current date";
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
               
                else if (Convert.ToDateTime(txtFrom.Text.ToString()).Date > DateTime.Now.Date)
                {
                    lblSearchMsg.Text = date_err_msg;//"You can not select From date later than 15 days from current date!";
                    return;
                }
                else if (Convert.ToDateTime(txtTo.Text.ToString()).Date > DateTime.Now.Date)
                {
                    lblSearchMsg.Text = date_err_msg;// "You can not select To date earlier than 15 days from current date!";
                    return;
                }
               
                else
                {
                    lblSearchMsg.Text = "";

                }
            }

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

            Cls_Business_rptSMSDelivery objTran = new Cls_Business_rptSMSDelivery();
            dt = objTran.GetDetails(htAddPlanParams, username, operator_id, catid);

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "SMSDeliver_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)                        
                        + @"VC/MAC ID" + Convert.ToChar(9)
                        + "Mobile No" + Convert.ToChar(9)
                        + "Message" + Convert.ToChar(9)
                        + "Status" + Convert.ToChar(9)
                        + "Inserted By" + Convert.ToChar(9)
                        + "Inserted Date" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_sms_vcid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["num_sms_clcontact"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_sms_message"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_sms_status"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["var_sms_username"].ToString() + Convert.ToChar(9) + "'" 
                                + dt.Rows[i]["date_sms_dt"].ToString() + Convert.ToChar(9);                            
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
                Response.Redirect("../MyExcelFile/" + "SMSDeliver_" + datetime + ".xls");
                if (catid == "0" || catid == "10")
                {
                    Response.Redirect("../Reports/rptSmsDelivery.aspx");

                }
            }

            if (dt.Rows.Count == 0)
            {
                grdExpiry.Visible = false;
                lblSearchMsg.Text = "No data found";
            }

        }
    }
}