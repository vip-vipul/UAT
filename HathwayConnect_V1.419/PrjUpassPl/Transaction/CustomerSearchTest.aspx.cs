using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using PrjUpassBLL.Transaction;
using System.Configuration;
using System.Data.OracleClient;

namespace PrjUpassPl.Transaction
{
    public partial class CustomerSearchTest : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        static string addon_poids = "";
        static string ala_poids = "";
        static string city = "";
        DataTable dtAddonPlans;
        DataTable dtBasicPlans;
        DataTable dtAlacartePlans;
        string cust_no;
        string browser_no; 

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["RightsKey"] = null;
            dtBasicPlans = new DataTable();
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("DEAL_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtBasicPlans.Columns.Add(new DataColumn("LCO_PRICE"));
            dtBasicPlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtBasicPlans.Columns.Add(new DataColumn("EXPIRY"));
            dtBasicPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtBasicPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_STATUS"));

            dtAddonPlans = new DataTable();
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("DEAL_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAddonPlans.Columns.Add(new DataColumn("LCO_PRICE"));
            dtAddonPlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtAddonPlans.Columns.Add(new DataColumn("EXPIRY"));
            dtAddonPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAddonPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_STATUS"));

            dtAlacartePlans = new DataTable();
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("DEAL_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAlacartePlans.Columns.Add(new DataColumn("LCO_PRICE"));
            dtAlacartePlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtAlacartePlans.Columns.Add(new DataColumn("EXPIRY"));
            dtAlacartePlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAlacartePlans.Columns.Add(new DataColumn("PLAN_STATUS"));

            if (!IsPostBack)
            {
                resetSearchBox();
            }

        }

        protected override void OnSaveStateComplete(EventArgs e)
        {
            cust_no = Request.QueryString["cust_no"].ToString();
            browser_no = Request.QueryString["browser_no"].ToString();
            simulate_cust_search(cust_no, browser_no);
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

        protected void resetAllGrids()
        {
            dtAddonPlans.Clear();
            dtAlacartePlans.Clear();
            dtBasicPlans.Clear();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            resetAllGrids();
            lblSearchResponse.Text = "";
            string searhParam = txtSearchParam.Text;
            string search_type = rdoSearchParamType.SelectedValue.ToString();
            string user_brmpoid = "2975299";
            string response_params = user_brmpoid + "$" + searhParam + "$SW";
            if (search_type == "0")
            {
                //if VC ID
                response_params += "$V";
            }

            string apiResponse = callAPI(response_params, "12");

            LoadTestLogText("Test in Progress : " + searhParam, "Testing", response_params, apiResponse, browser_no);

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
                        //btnReset_Click(sender, e);
                        lblSearchResponse.Text = "No STB found";
                        return;
                    }
                    grdStb.DataSource = sortedDT;
                    grdStb.DataBind();

                    //ticking first stb value and bind all fields with details
                    ((CheckBox)(grdStb.Rows[0].FindControl("chkStb"))).Checked = true;
                    HiddenField hdnDefaultService = ((HiddenField)(grdStb.Rows[0].FindControl("hdnServiceStr")));
                    bindAllGrids(hdnDefaultService.Value);

                    lblCustNo.Text = cust_id;
                    lblCustName.Text = cust_name;
                    lblCustAddr.Text = cust_addr.Replace('|', ',');
                    //string cust_poid = lstResponse[7];
                    //string service_poid = "";//responseArr[13];
                    //string plan_poid = "";// responseArr[14];
                    //ViewState["service_poid"] = service_poid;
                    //ViewState["plan_poid"] = plan_poid;
                    // ViewState["cust_poid"] = cust_poid;
                    // ViewState["cust_no"] = cust_id;
                    setSearchBox();
                    pnlCustDetails.Visible = true;
                    pnlGridHolder.Visible = true;
                    //btnReset.Visible = true;
                }
                else
                {
                    resetSearchBox();
                    lblSearchResponse.Text = "Failed to receive customer data";
                    pnlCustDetails.Visible = false;
                    pnlGridHolder.Visible = false;
                    //btnReset.Visible = false;
                }

            }
            catch (Exception)
            {
                resetSearchBox();
                lblSearchResponse.Text = "Customer data not found";
                pnlCustDetails.Visible = false;
                pnlGridHolder.Visible = false;
                //btnReset.Visible = false;
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
            string service_data = obj.getServiceDataBL(Session["username"].ToString(), city, all_plan_string, ViewState["customer_no"].ToString());
            string[] service_data_arr = service_data.Split('#');
            if (service_data_arr[0] != "9999")
            {
                msgboxstr("Something went wrong from procedure");
                return;
            }

            //generating basic table
            string basic_data_str = service_data_arr[1];
            if (basic_data_str != null && basic_data_str != "")
            {
                string[] basic_plan_arr = basic_data_str.Split('~');
                string basic_poids = dataTableBuilder(dtBasicPlans, basic_plan_arr);
            }


            //generating addon table
            string addon_data_str = service_data_arr[2];
            if (addon_data_str != null && addon_data_str != "")
            {
                string[] addon_plan_arr = addon_data_str.Split('~');
                addon_poids = dataTableBuilder(dtAddonPlans, addon_plan_arr);
                addon_poids = addon_poids.TrimEnd(',');
            }
            else
            {
                addon_poids = "'0'";
            }


            //generating alacarte table
            string alacarte_data_str = service_data_arr[3];
            if (alacarte_data_str != null && alacarte_data_str != "")
            {
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
                //AlacarteAccordion.Visible = false;
            }
            else
            {
                lblAlacartePlan.Visible = true;
                //AlacarteAccordion.Visible = true;
                //AlacarteAccordion.SelectedIndex = -1;
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
                //AddonAccordion.Visible = false;
            }
            else
            {
                lblAddonPlan.Visible = true;
                //AddonAccordion.Visible = true;
                //AddonAccordion.SelectedIndex = -1;
            }
        }

        public String callAPI(string Request, string request_code)
        {
            try
            {
                string fromSender = string.Empty;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(Request);
                //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRM/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.20/TestHwayOBRM/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/testhwayobrmcallservice/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.16//TestHwayOBRM/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
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
                FileLogText("Admin", "callAPI", " Error:" + ex.Message.Trim(), "", browser_no);
                return "1$---$" + ex.Message.Trim();
            }
        }

        private void FileLogText(String Str, String sender, String strRequest, String strResponse, string browser_no)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\LoadTestExep_br_" + browser_no + "_" + filename + ".txt", true);
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

        private void LoadTestLogText(String Str, String sender, String strRequest, String strResponse, string browser_no)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw =null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\LoadTestLog_br_" + browser_no + "_" + filename + ".txt", true);
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

        public void simulate_cust_search(string cust_no, string browser_no)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {
                string query = "SELECT a.account_no FROM aoup_lcopre_cust_master a WHERE rownum <= " + cust_no;
                //List<string> acc_no_list = new List<string>();
                OracleCommand cmd = new OracleCommand(query, conObj);
                conObj.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr["account_no"] != null && dr["account_no"].ToString() != "")
                    {
                        string acc_no = dr["account_no"].ToString();
                        rdoSearchParamType.SelectedValue = "1";
                        txtSearchParam.Text = acc_no;
                        btnSearch_Click(btnSearch, new EventArgs());
                    }
                }
                LoadTestLogText("End of Test", "Test", "End of Test", "End of Test", browser_no);
                conObj.Close();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert(\"Something went wrong : " + ex.Message + "\");", true);
                PrjUpassDAL.Helper.Cls_Security objSecurity = new PrjUpassDAL.Helper.Cls_Security();
                objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "CustomerSearchTest.cs-SimulateCustSearch");
                return;
            }
        }
    }
}