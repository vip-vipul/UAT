using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PrjUpassBLL.Transaction;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;
using PrjUpassDAL.Helper;

namespace PrjUpassPl.Transaction
{
    public partial class frmCustomerSearch1 : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        //static string plantype = "AD";
        //static string poidlist;
        static string addon_poids = "";
        static string ala_poids = "";
        //static string city = "";
        string oper_id = "";
        string username = "";
        string catid = "";
        DataTable dtAddonPlans;
        DataTable dtBasicPlans;
        DataTable dtAlacartePlans;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["operator_id"] != null)
            {
                Session["RightsKey"] = null;
                Cls_Business_TxnAssignPlan pl1 = new Cls_Business_TxnAssignPlan();
                string city = "";
                String dasarea = "";
                String operid = "";
                string Flag = "";
                string JVNO = "";
                string statename = "";
                pl1.GetUserCity(Session["username"].ToString(), out city, out dasarea, out operid, out JVNO, out Flag, out statename);
                ViewState["cityid"] = city;
                Session["JVFlagS"] = Flag;
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            Master.PageHeading = "Customer Details";
            dtBasicPlans = new DataTable();
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_NAME"));
            //dtBasicPlans.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("DEAL_POID"));
            //dtBasicPlans.Columns.Add(new DataColumn("PRODUCT_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtBasicPlans.Columns.Add(new DataColumn("LCO_PRICE"));
            //dtBasicPlans.Columns.Add(new DataColumn("PAYTERM"));
            //dtBasicPlans.Columns.Add(new DataColumn("CITYID"));
            //dtBasicPlans.Columns.Add(new DataColumn("CITY_NAME"));
            //dtBasicPlans.Columns.Add(new DataColumn("COMPANY_CODE"));
            //dtBasicPlans.Columns.Add(new DataColumn("INSBY"));
            //dtBasicPlans.Columns.Add(new DataColumn("INSDT"));
            dtBasicPlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtBasicPlans.Columns.Add(new DataColumn("EXPIRY"));
            dtBasicPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtBasicPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_STATUS"));
            //ViewState["customer_basic_plans"] = dtBasicPlans;

            //table which holds addon plans
            dtAddonPlans = new DataTable();
            //dtAddonPlans.Columns.Add(new DataColumn("PLAN_ID"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_NAME"));
            //dtAddonPlans.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("DEAL_POID"));
            //dtAddonPlans.Columns.Add(new DataColumn("PRODUCT_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAddonPlans.Columns.Add(new DataColumn("LCO_PRICE"));
            //dtAddonPlans.Columns.Add(new DataColumn("PAYTERM"));
            //dtAddonPlans.Columns.Add(new DataColumn("CITYID"));
            //dtAddonPlans.Columns.Add(new DataColumn("CITY_NAME"));
            //dtAddonPlans.Columns.Add(new DataColumn("COMPANY_CODE"));
            //dtAddonPlans.Columns.Add(new DataColumn("INSBY"));
            //dtAddonPlans.Columns.Add(new DataColumn("INSDT"));
            dtAddonPlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtAddonPlans.Columns.Add(new DataColumn("EXPIRY"));
            dtAddonPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAddonPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_STATUS"));
            //ViewState["customer_addon_plans"] = dtAddonPlans;

            //table which holds a-la-carte plans

            dtAlacartePlans = new DataTable();
            //dtAlacartePlans.Columns.Add(new DataColumn("PLAN_ID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_NAME"));
            //dtAlacartePlans.Columns.Add(new DataColumn("PLAN_TYPE"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("DEAL_POID"));
            //dtAlacartePlans.Columns.Add(new DataColumn("PRODUCT_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAlacartePlans.Columns.Add(new DataColumn("LCO_PRICE"));
            //dtAlacartePlans.Columns.Add(new DataColumn("PAYTERM"));
            //dtAlacartePlans.Columns.Add(new DataColumn("CITYID"));
            //dtAlacartePlans.Columns.Add(new DataColumn("CITY_NAME"));
            //dtAlacartePlans.Columns.Add(new DataColumn("COMPANY_CODE"));
            //dtAlacartePlans.Columns.Add(new DataColumn("INSBY"));
            //dtAlacartePlans.Columns.Add(new DataColumn("INSDT"));
            dtAlacartePlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtAlacartePlans.Columns.Add(new DataColumn("EXPIRY"));
            dtAlacartePlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_STATUS"));
            //ViewState["customer_alacarte_plans"] = dtAlacartePlans;

            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {
                oper_id = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {

                resetSearchBox();
            }
        }

        protected void chkStb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gv = (GridViewRow)chk.NamingContainer;
            if (grdStb.Rows.Count == 1)
            {
                chk.Checked = true;
                return;
            }
            int rownumber = gv.RowIndex;
            if (chk.Checked)
            {
                int i;
                for (i = 0; i <= grdStb.Rows.Count - 1; i++)
                {
                    if (i != rownumber)
                    {
                        CheckBox chkcheckbox = ((CheckBox)(grdStb.Rows[i].FindControl("chkStb")));
                        chkcheckbox.Checked = false;
                    }
                }
                HiddenField hdnServiceString = (HiddenField)grdStb.Rows[rownumber].FindControl("hdnServiceStr");
                string service_str = hdnServiceString.Value;
                resetAllGrids();
                bindAllGrids(service_str);
            }
            else
            {
                chk.Checked = true;
            }
        }

        protected void resetAllGrids()
        {
            dtAddonPlans.Clear();
            dtAlacartePlans.Clear();
            dtBasicPlans.Clear();
        }

        protected string dataTableBuilder(DataTable dt, string[] arr_data)
        {
            string poid_list = "";
            foreach (string plan_data in arr_data)
            {
                string[] plan_details_arr = plan_data.Split('$');
                string plan_poid = plan_details_arr[0];
                poid_list += "'" + plan_poid + "',";
                string plan_name = plan_details_arr[1];
                string plan_custprice = plan_details_arr[2];
                string plan_lcoprice = plan_details_arr[3];
                string plan_activation = plan_details_arr[4];
                string plan_expiry = plan_details_arr[5];
                string plan_dealpoid = plan_details_arr[6];
                string plan_package = plan_details_arr[7];
                string plan_purchase = plan_details_arr[8];
                string plan_status = plan_details_arr[9];
                DataRow tempDr = dt.NewRow();
                tempDr["PLAN_POID"] = plan_poid;
                tempDr["PLAN_NAME"] = plan_name;
                //tempDr["PLAN_TYPE"] = "B";
                tempDr["DEAL_POID"] = plan_dealpoid;
                //tempDr["PRODUCT_POID"] = dr_plan[0]["PRODUCT_POID"];
                tempDr["CUST_PRICE"] = plan_custprice;
                tempDr["LCO_PRICE"] = plan_lcoprice;
                //tempDr["PAYTERM"] = dr_plan[0]["PAYTERM"];
                //tempDr["CITYID"] = city;
                //tempDr["CITY_NAME"] = dr_plan[0]["CITY_NAME"];
                //tempDr["COMPANY_CODE"] = dr_plan[0]["COMPANY_CODE"];
                //tempDr["INSDT"] = dr_plan[0]["INSDT"];
                //tempDr["INSBY"] = dr_plan[0]["INSBY"];
                tempDr["ACTIVATION"] = plan_activation;
                tempDr["EXPIRY"] = plan_expiry;
                tempDr["PACKAGE_ID"] = plan_package;
                tempDr["PURCHASE_POID"] = plan_purchase;
                tempDr["PLAN_STATUS"] = plan_status;
                dt.Rows.Add(tempDr);
            }
            return poid_list;
        }

        protected void bindAllGrids(string service_str)
        {
            try
            {
                //separating and manupulating plan poids
                ViewState["ServicePoid"] = service_str.Split('!')[0];
                lblStbNo.Text = service_str.Split('!')[1];

                //showing service status
                Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                string ServiceStatus = obj.getServiceStatus(service_str.Split('!')[4]);
                if (ServiceStatus == "A") { lbactive.Visible = false; lbdeactive.Visible = true; }
                else if (ServiceStatus == "IA") { lbactive.Visible = true; lbdeactive.Visible = false; }
                else if (ServiceStatus == "CL") { lbactive.Visible = false; lbdeactive.Visible = false; }
                else if (ServiceStatus == "EX") { lbactive.Visible = false; lbdeactive.Visible = false; }


                string all_plan_string = service_str.Split('!')[3]; //--this is plan string under service

                string city = "";
                if (ViewState["cityid"] != null && ViewState["cityid"].ToString() != "")
                {
                    city = ViewState["cityid"].ToString();
                }
                string service_data = obj.getServiceDataBL(Session["username"].ToString(), city, all_plan_string, ViewState["customer_no"].ToString());
                string[] service_data_arr = service_data.Split('#');
                if (service_data_arr[0] != "9999")
                {
                    //show only customer information but hide plan details
                    setSearchBox();
                    pnlCustDetails.Visible = true;
                    pnlGridHolder.Visible = false;
                    btnReset.Visible = true;
                    lblSearchResponse.Text = "";
                    msgboxstr("Something went wrong while fetching plan details...");
                    return;
                }
                else
                {
                    //show only customer information as well as plan details
                    setSearchBox();
                    pnlCustDetails.Visible = true;
                    pnlGridHolder.Visible = true;
                    btnReset.Visible = true;
                    lblSearchResponse.Text = "";
                }

                //generating basic table
                string basic_data_str = service_data_arr[1];
                if (basic_data_str != null && basic_data_str != "")
                {
                    //DataTable dtBasicPlans = (DataTable)ViewState["customer_basic_plans"];
                    string[] basic_plan_arr = basic_data_str.Split('~');
                    string basic_poids = dataTableBuilder(dtBasicPlans, basic_plan_arr);
                }


                //generating addon table
                string addon_data_str = service_data_arr[2];
                if (addon_data_str != null && addon_data_str != "")
                {
                    //DataTable dtAddonPlans = (DataTable)ViewState["customer_addon_plans"];
                    string[] addon_plan_arr = addon_data_str.Split('~');
                    addon_poids = dataTableBuilder(dtAddonPlans, addon_plan_arr);
                    addon_poids = addon_poids.TrimEnd(',');
                    //poidlist = addon_poids;
                }
                else
                {
                    addon_poids = "'0'";
                }


                //generating alacarte table
                string alacarte_data_str = service_data_arr[3];
                if (alacarte_data_str != null && alacarte_data_str != "")
                {
                    // DataTable dtAlacartePlans = (DataTable)ViewState["customer_alacarte_plans"];
                    string[] alacarte_plan_arr = alacarte_data_str.Split('~');
                    ala_poids = dataTableBuilder(dtAlacartePlans, alacarte_plan_arr);
                    ala_poids = ala_poids.TrimEnd(',');
                }
                else
                {
                    ala_poids = "'0'";
                }

                grdCarte.DataSource = dtAlacartePlans;
                grdCarte.DataBind();
                if (dtAlacartePlans.Rows.Count == 0)
                {
                    lblAlacartePlan.Visible = false;
                    AlacarteAccordion.Visible = false;
                }
                else
                {
                    lblAlacartePlan.Visible = true;
                    AlacarteAccordion.Visible = true;
                    AlacarteAccordion.SelectedIndex = -1;
                }


                grdBasicPlanDetails.DataSource = dtBasicPlans;
                grdBasicPlanDetails.DataBind();
                if (dtBasicPlans.Rows.Count == 0)
                {
                    lblBasicPlan.Visible = false;
                }
                else
                {
                    lblBasicPlan.Visible = true;
                }


                grdAddOnPlan.DataSource = dtAddonPlans;
                grdAddOnPlan.DataBind();
                if (dtAddonPlans.Rows.Count == 0)
                {
                    lblAddonPlan.Visible = false;
                    AddonAccordion.Visible = false;
                }
                else
                {
                    lblAddonPlan.Visible = true;
                    AddonAccordion.Visible = true;
                    AddonAccordion.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                FileLogText("BindAllGrid", "", Session["username"].ToString(), ex.Message.ToString());
            }
        }

        protected void resetSearchBox()
        {
            divCustHolder.Attributes.Remove("width");
            divCustHolder.Attributes["width"] = "126%";
            divStbHolder.Attributes.Remove("style");
            divStbHolder.Attributes["style"] = "display:none";
            divSearchHolder.Attributes.Remove("class");
            divSearchHolder.Attributes["class"] = "delInfo";
        }

        protected void setSearchBox()
        {
            divCustHolder.Attributes.Remove("width");
            divCustHolder.Attributes["width"] = "77.5%";
            divStbHolder.Attributes.Remove("style");
            divSearchHolder.Attributes.Remove("class");
            divSearchHolder.Attributes["class"] = "delInfo custDetailsHolder";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            resetAllGrids();
            lblSearchResponse.Text = "";
            string searhParam = txtSearchParam.Text;
            string search_type = rdoSearchParamType.SelectedValue.ToString();
            // string username = "";
            string oper_id = "";
            string user_brmpoid = "";
            string category = "";
            if (Session["operator_id"] != null && Session["username"] != null && Session["category"] != null && Session["user_brmpoid"] != null)
            {
                username = Convert.ToString(Session["username"]);
                oper_id = Convert.ToString(Session["operator_id"]);
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
                category = Convert.ToString(Session["category"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            // string response_params = username + "$" + searhParam + "$SW";
            string response_params = user_brmpoid + "$" + searhParam + "$SW";
            if (search_type == "0")
            {
                //if VC ID
                response_params += "$V";
            }

            string apiResponse = callAPI(response_params, "12");
            //string apiResponse = callAPI(response_params, "6");

            try
            {
                if (apiResponse != "")
                {
                    List<string> lstResponse = new List<string>();
                    lstResponse = apiResponse.Split('$').ToList();
                    string cust_id = lstResponse[0]; //account_no
                    ViewState["customer_no"] = cust_id;
                    string cust_name = lstResponse[3];
                    ViewState["customer_name"] = cust_name;
                    string cust_addr = lstResponse[4];
                    ViewState["accountPoid"] = lstResponse[6];
                    string lco_poid = lstResponse[13];
                    string cust_mobile_no = lstResponse[5];

                    //DATA ACCESS VALIDATION----------------------------------------------------------------------------------VALIDATION
                    if (category != "10") //No validation for national level user
                    {
                        Cls_Validation obj = new Cls_Validation();
                        string validate_cust_access = obj.CustDataAccess(username, oper_id, lco_poid, category);
                        if (validate_cust_access.Length == 0)
                        {
                            resetAllGrids();
                            resetSearchBox();
                            pnlCustDetails.Visible = false;
                            pnlGridHolder.Visible = false;
                            lblSearchResponse.Text = "You have no privileges to access customer information as s/he belongs to other LCO";
                            return;
                        }
                    }
                    else
                    {   //if user is not a LCO, then get city using brmpoid 
                        Cls_Business_TxnAssignPlan objCity = new Cls_Business_TxnAssignPlan();
                        string city = objCity.GetCityFromBrmPoid(Session["username"].ToString(), lco_poid);
                        ViewState["cityid"] = city;
                    }


                    string cust_services = lstResponse[15];
                    string[] service_arr = cust_services.Split('^');
                    DataTable dtStbs = new DataTable();
                    dtStbs.Columns.Add(new DataColumn("STB_NO"));
                    dtStbs.Columns.Add(new DataColumn("VC_ID"));
                    dtStbs.Columns.Add(new DataColumn("SERVICE_STRING"));
                    dtStbs.Columns.Add(new DataColumn("Status"));

                    DataTable sortedDT = new DataTable();

                    foreach (string service in service_arr)
                    {
                        string stb_no = service.Split('!')[1];
                        string vc_id = service.Split('!')[2];
                        string stb_status = service.Split('!')[4];
                        if (stb_status == "10103")
                        {
                            continue; //if status is terminated
                        }
                        if (stb_no == "" || vc_id == "")
                        {
                            continue;
                        }
                        DataRow drStbRow = dtStbs.NewRow();
                        drStbRow["STB_NO"] = stb_no;
                        drStbRow["VC_ID"] = vc_id;
                        drStbRow["SERVICE_STRING"] = service;
                        drStbRow["Status"] = stb_status;
                        dtStbs.Rows.Add(drStbRow);

                        DataView dv = dtStbs.DefaultView;
                        dv.Sort = "Status Asc";
                        sortedDT = dv.ToTable();
                    }
                    if (sortedDT.Rows.Count == 0)
                    {
                        btnReset_Click(sender, e);
                        lblSearchResponse.Text = "No STB found";
                        return;
                    }
                    grdStb.DataSource = sortedDT;
                    grdStb.DataBind();

                    //ticking first stb value and bind all fields with details
                    ((CheckBox)(grdStb.Rows[0].FindControl("chkStb"))).Checked = true;
                    HiddenField hdnDefaultService = ((HiddenField)(grdStb.Rows[0].FindControl("hdnServiceStr")));
                    bindAllGrids(hdnDefaultService.Value);
                    
                    //show last five transactions of customer 
                    showLastTransaction(username, cust_id);
                    
                    lblCustNo.Text = cust_id;
                    lblCustName.Text = cust_name;
                    lblCustAddr.Text = cust_addr.Replace('|', ',');
                    lbltxtmobno.Text = cust_mobile_no;
                    //--------------------------------------------------------comented on 10-Jan-2015
                    //setSearchBox();
                    //pnlCustDetails.Visible = true; 
                    //pnlGridHolder.Visible = true;
                    //btnReset.Visible = true;
                }
                else
                {
                    resetSearchBox();
                    lblSearchResponse.Text = "Failed to receive customer data";
                    pnlCustDetails.Visible = false;
                    pnlGridHolder.Visible = false;
                    btnReset.Visible = false;
                }

            }
            catch (Exception)
            {
                resetSearchBox();
                lblSearchResponse.Text = "Customer data not found";
                pnlCustDetails.Visible = false;
                pnlGridHolder.Visible = false;
                btnReset.Visible = false;
            }
        }

        public String callAPI(string Request, string request_code)
        {
            try
            {

                string fromSender = string.Empty;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(Request);
             HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRM/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.21//TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
       //  HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost//TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
               // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/testhwayobrmcallservice/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;
                myRequest.Timeout = 90000;
                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                using (HttpWebResponse responseFromSender = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (StreamReader responseReader = new StreamReader(responseFromSender.GetResponseStream()))
                    {
                        fromSender = responseReader.ReadToEnd();
                    }
                }
                String Res = fromSender.Split('%')[0];
                return Res;
            }
            catch (Exception ex)
            {
                FileLogText("Admin", "callAPI", " Error:" + ex.Message.Trim(), "");
                return "1$---$" + ex.Message.Trim();
            }
        }

        private void FileLogText(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\HwayObrm_Web_" + filename + ".txt", true);
                sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(strRequest.Trim());
                sw.WriteLine(strResponse.Trim());
                sw.WriteLine("************************************************************************************************************************");
            }
            catch (Exception ex)
            {
                //Response.Write("Error while writing logs : " + ex.Message.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            resetSearchBox();
            pnlCustDetails.Visible = false;
            pnlGridHolder.Visible = false;
            btnReset.Visible = false;
            lblSearchResponse.Text = "";
        }

        public void msgbox(string message, Control ctrl)
        {
            lblPopupResponse.Text = message;
            popMsg.Show();
            ctrl.Focus();
        }

        public void msgboxstr(string message)
        {
            lblPopupResponse.Text = message;
            popMsg.Show();
        }

        public void showLastTransaction(string username, string cust_id) 
        {
            lblNoTransData.Visible = false;
            LastFiveTransAccordion.SelectedIndex = -1;
            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
            string rownum = hdnTransRowNo.Value.ToString();
            DataTable dtLastFiveTrans = obj.getCustLastTrans(username, cust_id, rownum);
            if (dtLastFiveTrans != null)
            {
                if (dtLastFiveTrans.Rows.Count > 0)
                {
                    grdLastFiveTrans.DataSource = dtLastFiveTrans;
                    grdLastFiveTrans.DataBind();
                    grdLastFiveTrans = null;
                }
                else
                {
                    lblNoTransData.Text = "Transaction Data Not Found";
                    lblNoTransData.Visible = true;
                }
            }
            else {
                lblNoTransData.Text = "Failed To Fetch Transaction Data";
                lblNoTransData.Visible = true;
            }
        }
    }
}
