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
using System.Threading;
using System.Net;
using System.Globalization;

namespace PrjUpassPl.Reports
{
    public partial class rptObrmCustMast : System.Web.UI.Page
    {
        string operid;
        string username;
        string catid;
        DataSet dsTime;
        protected void Page_Load(object sender, EventArgs e)
        {

            Master.PageHeading = "Customer Master Report";

            if (!IsPostBack)
            {
                // Session["RightsKey"] = null;
                Session["RightsKey"] = "N";

                string _getTime = "select to_char(max(JOB_END_TIME),'DD-MON-YYYY HH:MI:SS AM') JOB_END_TIME from reports.BULK_JOB_MASTER@caslive where JOB_TYPE = 'CUSTOMER_MASTER_LCO'";
                dsTime = Getdata(_getTime);
                if (dsTime.Tables.Count > 0)
                {
                    if (dsTime.Tables[0].Rows.Count > 0)
                    {
                        lblLastRefreshTime.Text = "Data Refresh At : " + dsTime.Tables[0].Rows[0]["JOB_END_TIME"].ToString();
                        ViewState["TiMe"] = dsTime.Tables[0].Rows[0]["JOB_END_TIME"].ToString();
                    }
                }

                getdateData();
            }// 
        }


        public DataSet Getdata(string st)
        {

            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conn = new OracleConnection(ConStr);
            DataSet ds = new DataSet();
            try
            {
                OracleDataAdapter da = new OracleDataAdapter(st, conn);

                da.Fill(ds);
                da.Dispose();
                conn.Close();
                conn.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // string username = "";
            string oper_id = "";
            string user_brmpoid = "";
            if (Session["operator_id"] != null && Session["username"] != null && Session["user_brmpoid"] != null)
            {
                username = Convert.ToString(Session["username"]);
                oper_id = Convert.ToString(Session["operator_id"]);
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            binddata();

        }


        protected void binddata()
        {
            string search_type = RadSearchby.SelectedValue.ToString();
            try
            {
                string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                OracleConnection con = new OracleConnection(strCon);
                string str = "";

                str = "select * from view_lcopre_custmast_rpt a ";

                string valid = "";
                if (search_type == "0" && txtsearchpara.Text.Length < 11)
                {
                    valid = SecurityValidation.chkData("N", txtsearchpara.Text);
                    if (valid == "")
                        str += " where a.account_no= '" + txtsearchpara.Text.ToString() + "'";
                    else
                    {
                        lblSearchMsg.Text = valid.ToString();
                        
                    }

                }
                else if (search_type == "1")
                {
                    valid = SecurityValidation.chkData("T", txtsearchpara.Text);
                    if (valid == "")
                        str += " where a.vc= '" + txtsearchpara.Text.ToString() + "'";
                    else
                    {
                        lblSearchMsg.Text = valid.ToString();
                        
                    }

                }
                else if (search_type == "2")
                {
                    valid = SecurityValidation.chkData("T", txtsearchpara.Text);
                    if (valid == "")
                        str += " where a.lco_code= '" + txtsearchpara.Text.ToString() + "'";
                    else
                    {
                        lblSearchMsg.Text = valid.ToString();
                       
                    }
                }

                DataTable DtObj = new DataTable();

                OracleCommand Cmd = new OracleCommand(str, con);
                OracleDataAdapter DaObj = new OracleDataAdapter(Cmd);
                DaObj.Fill(DtObj);

                if (search_type != "2")
                {
                    if (DtObj.Rows.Count == 0)
                    {
                        btnGenerateExcel.Visible = false;
                        grdtransdet.Visible = false;
                        lblSearchMsg.Text = "No data found";
                    }
                    else
                    {
                        btnGenerateExcel.Visible = true;
                        grdtransdet.Visible = true;
                        lblSearchMsg.Text = "";
                        ViewState["searched_Details"] = DtObj;
                        grdtransdet.DataSource = DtObj;
                        grdtransdet.DataBind();
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdtransdet.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                        DivRoot.Style.Add("display", "block");

                    }
                }
                else
                {


                }

            }
            catch (Exception ex)
            {

                Response.Write("Error : " + ex.Message.Trim());
                return;
            }



        }

        protected void ExportExcel()
        {

            //string search_type = RadSearchby.SelectedValue.ToString();

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
            dt = (DataTable)ViewState["searched_Details"];

            if (dt == null)
            {

                return;
            }

            if (dt.Rows.Count > 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "CustMaster_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr. No." + Convert.ToChar(9)
                        + "Account Number" + Convert.ToChar(9)
                        + "VC Id" + Convert.ToChar(9)
                        + "MAC Id" + Convert.ToChar(9)
                        + "STB" + Convert.ToChar(9)
                        + "First Name" + Convert.ToChar(9)
                         + "Middle Name" + Convert.ToChar(9)
                          + "Last Name" + Convert.ToChar(9)
                        + "Address" + Convert.ToChar(9)
                        + "LCO Name" + Convert.ToChar(9)
                        + "LCO code" + Convert.ToChar(9)
                        + "Mobile" + Convert.ToChar(9)
                        + "Package" + Convert.ToChar(9)
                        + "City" + Convert.ToChar(9)
                        + "State" + Convert.ToChar(9)
                        + "Zip" + Convert.ToChar(9)

                        + "Customer Type" + Convert.ToChar(9)
                        + "Customer Category" + Convert.ToChar(9)
                        + "Agreement No" + Convert.ToChar(9)
                        + "Product Name" + Convert.ToChar(9)
                        + "Connection Type" + Convert.ToChar(9)
                        + "Home Phone" + Convert.ToChar(9)
                        + "Work Phone" + Convert.ToChar(9)
                        //+ "Plan Type" + Convert.ToChar(9)
                        + "Start Date" + Convert.ToChar(9)
                        + "End Date" + Convert.ToChar(9)
                         + "MRP" + Convert.ToChar(9)
                        + "Renewal Status" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["account_no"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["vc"].ToString() + Convert.ToChar(9)
                                 + dt.Rows[i]["mac"].ToString() + Convert.ToChar(9)
                                  + dt.Rows[i]["stb"].ToString() + Convert.ToChar(9)

                                + dt.Rows[i]["first_name"].ToString() + Convert.ToChar(9)
                                 + dt.Rows[i]["middle_name"].ToString() + Convert.ToChar(9)
                                  + dt.Rows[i]["last_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["address"].ToString() + Convert.ToChar(9)

                                + dt.Rows[i]["lco_name"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["lco_code"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["mobile"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["planname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["city"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["state"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["zip"].ToString() + Convert.ToChar(9)

                                + dt.Rows[i]["customer_type"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cust_category"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["agreement_no"].ToString() + Convert.ToChar(9)

                                + dt.Rows[i]["productname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["connection_type"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["homephone"].ToString() + Convert.ToChar(9)

                                + dt.Rows[i]["workphone"].ToString() + Convert.ToChar(9)

                              //  + dt.Rows[i]["plantype"].ToString() + Convert.ToChar(9)
                                + "'"
                                + dt.Rows[i]["startdate"].ToString() + Convert.ToChar(9)
                                + "'"
                                + dt.Rows[i]["enddate"].ToString() + Convert.ToChar(9)
                                + "'"
                                 + dt.Rows[i]["custprice"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["renewflagstatus"].ToString() + Convert.ToChar(9);
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
                Response.Redirect("../MyExcelFile/" + "CustMaster_" + datetime + ".xls");
                if (catid == "0" || catid == "10")
                {
                    Response.Redirect("../Reports/rptExpiry.aspx");

                }

            }


            if (dt.Rows.Count == 0)
            {
                grdtransdet.Visible = false;
                //   lblSearchMsg.Text = "No data found";
            }

        }

        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }

        protected void grdtransdet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdtransdet.PageIndex = e.NewPageIndex;
            // binddata();
        }

        protected void RadSearchby_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["category"].ToString() == "3")
            {
                if (RadSearchby.SelectedValue == "2")
                {
                    lblSearchMsg.Text = "";
                    txtsearchpara.Text = Session["username"].ToString();
                    ViewState["islco"] = "1";
                    btnGenerateExcel.Visible = false;
                    btnCustDetail.Visible = true;
                    txtsearchpara.Enabled = false;
                    btnSearch.Visible = false;
                    grdtransdet.Visible = false;
                    radCMRdate.Visible = true;
                    btnCustDetail.Visible = true;
                }
                else
                {
                    lblSearchMsg.Text = "";
                    ViewState["islco"] = "0";
                    btnSearch.Visible = true;
                    grdtransdet.Visible = false;
                    txtsearchpara.Text = "";
                    txtsearchpara.Enabled = true;
                    btnGenerateExcel.Visible = false;
                    btnCustDetail.Visible = false;
                    radCMRdate.Visible = false;
                    btnCustDetail.Visible = false;
                }
            }
        }

        protected void btnCustDetail_Click(object sender, EventArgs e)
        {
            ViewState["islco"] = "1";
            grdtransdet.Visible = false;
            string acc = Session["username"].ToString();
            if (radCMRdate.SelectedIndex == 0)
            {
                Response.Redirect("http://202.88.130.21:2462/CustomerMasterLCO/getReport?ACCOUNT_NO=" + acc);
            }
            else if (radCMRdate.SelectedIndex == 1)
            {
                Response.Redirect("http://202.88.130.21:2462/CustomerMasterLCOLM/getReport?ACCOUNT_NO=" + acc);
            }
            else if (radCMRdate.SelectedIndex == 2)
            {
                Response.Redirect("http://202.88.130.21:2462/CustomerMasterLCOL2LM/getReport?ACCOUNT_NO=" + acc);
            }
            else
            {
                return;
            }

        }

        public void getdateData()
        {

            radCMRdate.Items.Add(new ListItem("Current CMR", "0"));
            radCMRdate.Items.Add(new ListItem(DateTime.Now.AddMonths(-1).ToString("MMM") + " " + "End CMR", "1"));
            radCMRdate.Items.Add(new ListItem(DateTime.Now.AddMonths(-2).ToString("MMM") + " " + "End CMR", "2"));
            radCMRdate.SelectedIndex = 0;

        }
    }
}