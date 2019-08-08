using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassBLL.Transaction;
using System.Collections;
using PrjUpassPl.BulkUploadService;
using System.IO;
using PrjUpassDAL.Authentication;
using PrjUpassBLL.Reports;
using PrjUpassDAL.Helper;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayBulkRenewConf : System.Web.UI.Page
    {
        string username;
        string uploadid;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["RightsKey"] = "N";

            if (Session["username"] != null)
            {
                username = Session["username"].ToString().Trim();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            if (Session["upload_id"] != null)
            {
                uploadid = Session["upload_id"].ToString().Trim();
            }
            else
            {
                Response.Redirect("../Transaction/TransHwayBulkRenewal.aspx");
            }

            if (!IsPostBack)
            {
                binddata("Y");
            }
        }

        protected void binddata(string flag)
        {
            if (flag == "Y")
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
                DataTable dt = new DataTable();

                Cls_BLL_TransHwayBulkRenewal objTran = new Cls_BLL_TransHwayBulkRenewal();
                dt = objTran.getUploadedData(username, uploadid);

                if (dt == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    grdExpiry.Visible = false;
                    btnBeginTrans.Visible = false;
                    btnCancelTransactions.Visible = false;
                    lblStatus.Text = "No data found";
                    lblConfirmTxt.Text = "No plans found to renew. kindly go back to previous page and begin";
                }
                else
                {
                    lblTransCount.Text = "Total number of plan renewals are : " + dt.Rows.Count.ToString();
                    //btnGenerateExcel.Visible = true;
                    btnBeginTrans.Visible = true;
                    btnCancelTransactions.Visible = true;
                    grdExpiry.Visible = true;
                    lblStatus.Text = "";
                    grdExpiry.DataSource = dt;
                    grdExpiry.DataBind();
                }
            }

        }

        public void msgbox(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + message + "');", true);
        }

        private void FileLogText1(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\HwayObrm_BulkRenewal_" + filename + ".txt", true);
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
                if(sw!=null)
                {
                sw.Flush();
                sw.Close();
                sw.Dispose();
                }
            }
        }

        public String callAPI(string Request, string request_code)
        {
            try
            {
                Service1 serviceObj = new Service1();
                // FileLogText(username, "Bulk_Renewal_Call_API", Request + "_" + request_code, serviceObj.Url.ToString());

                String api_res = serviceObj.processHwayOBRM(Request, request_code);

                return api_res;
            }
            catch (Exception ex)
            {
                //FileLogText(username, "Bulk_Renewal_Call_API", " Error:" + ex.Message.Trim(), "");
                return "1$---$" + ex.Message.Trim();
            }
        }

        private void updateStatus(string cust_no, string vcid, string plan_name, string action, string err_code, string err_msg, string lco_code)
        {
            string username = "";
            if (Session["username"] != null)
            {
                username = Session["username"].ToString();
            }
            string upload_id = "";
            if (Session["upload_id"] != null && Session["upload_id"].ToString() != "")
            {
                upload_id = Session["upload_id"].ToString();
            }
            else
            {
                msgbox("Upload id not found,  please reselect data and try again...");
                return;
            }
            Hashtable htData = new Hashtable();
            htData["username"] = username;
            htData["upload_id"] = upload_id;
            htData["cust_no"] = cust_no;
            htData["plan_name"] = plan_name;
            htData["action"] = action;
            htData["err_code"] = err_code;
            htData["err_msg"] = err_msg;
            htData["lco_code"] = lco_code;
            htData["vcid"] = vcid;
            Cls_Business_TransHwayBulkOperation objStatus = new Cls_Business_TransHwayBulkOperation();
            objStatus.bulkStatusUpdt(htData);
        }

        public string validProcess(Hashtable htData)
        {
            Cls_Business_TransHwayBulkOperation businessObj = new Cls_Business_TransHwayBulkOperation();
            string valid_response = businessObj.validateBulkTrans(htData);
            string[] res = valid_response.Split('$');
            if (res[0] != "9999")
            {
                return "FAIL$" + res[1];
            }
            else
            {
                return res[1]; //request id
            }
        }

        public string OBRMprocess(string[] data, string action)
        {
            //data index -
            //0-brmpoid,1-accpoid,2-servicepoid,3-planpoid,4-lcooperid,5-custname,6-address,7-pkgid,
            //8-dealpoid,9-purchasepoid,10-activationdt,11-expirydt
            string brm_poid = data[0];
            string account_poid = data[1];
            string service_poid = data[2];
            string plan_poid = data[3];
            string package_poid = data[7];
            string deal_poid = data[8];
            string purchase_poid = data[9];
            string trans_req = brm_poid + "$" + account_poid + "$" + service_poid + "$" + plan_poid;
            string trans_req_code = "";
            string trans_response = "";
            trans_req_code = "7";
            trans_req += "$" + purchase_poid;
            trans_response = callAPI(trans_req, trans_req_code);
            trans_req = "";
            trans_req_code = "";
            return trans_response;
        }

        protected void btnBeginTrans_Click(object sender, EventArgs e)
        {
            btnBeginTrans.Enabled = false;
            btnBeginTrans.Visible = false;
            string process_flag = "";
            DataTable dtUploadedData = null;
            string upload_id = "";
            if (Session["upload_id"] != null && Session["upload_id"].ToString() != "")
            {
                upload_id = Session["upload_id"].ToString();
            }
            else
            {
                msgbox("Upload id not found,  please reselect data and try again...");
                return;
            }

            //fetch uploaded data
            Cls_Business_TransHwayBulkOperation obj = new Cls_Business_TransHwayBulkOperation();
            dtUploadedData = obj.getUploadedData(username, upload_id);
            if (dtUploadedData == null)
            {
                msgbox("Failed to retrieve uploaded data, please reselect data and try again...");
                return;
            }
            if (dtUploadedData.Rows.Count == 0)
            {
                msgbox("No data found for upload id : " + upload_id + ",  please reselect data and try again...");
                return;
            }
            process_flag = obj.bulkprocessupdt(username, upload_id);
            //loop over all fetched data
            int cnt = 0;
            foreach (DataRow row in dtUploadedData.Rows)
            {
                string customer_no = row.ItemArray[0].ToString().Trim();
                string vc = row.ItemArray[1].ToString().Trim();
                string lco_code = row.ItemArray[2].ToString().Trim();
                string plan_name = row.ItemArray[3].ToString().Trim();
                string autorenewFlag = row.ItemArray[9].ToString().Trim(); // added by vivek 20150627
                string action = "R";//row.ItemArray[4].ToString().Trim();
                string user_brmpoid = row.ItemArray[8].ToString().Trim();
                string req_data = user_brmpoid + "$" + customer_no + "$SW$" + vc;
                string api_response;
                string[] req_data_arr;
                string cust_info_res;
                string service_info_res;
                string validated_data;
                string[] validated_data_arr;
                string request_id = "";
                string obrm_status = "";
                string obrm_msg = "";
                Hashtable ht = new Hashtable();

                try
                {
                    //customer search api call
                    api_response = callAPI(req_data, "13");
                    req_data_arr = api_response.Split(new string[] { "##" }, StringSplitOptions.None);
                    cust_info_res = req_data_arr[0].TrimEnd('$');
                    service_info_res = req_data_arr[1];

                }
                catch (Exception ex)
                {
                    //TODO - failed to get customer details from web service
                    updateStatus(customer_no, vc, plan_name, action, "0", "Customer data not found", lco_code);
                    continue;
                }

                //validate customer details using procedure and get important parameters for transaction
                //0-brmpoid,1-accpoid,2-servicepoid,3-planpoid,4-lcooperid,5-custname,6-address,7-pkgid,
                //8-dealpoid,9-purchasepoid,10-activationdt,11-expirydt
                Cls_Business_TransHwayBulkOperation objValidate = new Cls_Business_TransHwayBulkOperation();
                validated_data = objValidate.bulkValidate(username, upload_id, cust_info_res, service_info_res, action, plan_name, lco_code);
                validated_data_arr = validated_data.Split('#');
                if (validated_data_arr[0] == "9999")
                {
                    string[] data = validated_data_arr[1].Split('$');
                    string _plan_poid = data[3];
                    string _lco_oper_id = data[4];
                    string _cust_name = data[5];
                    string _cust_addr = data[6];
                    string activation_date = data[10];
                    string expiry_date = data[11];
                    Cls_Data_Auth auth = new Cls_Data_Auth();
                    string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                    ht.Add("username", username);
                    ht.Add("lcoid", _lco_oper_id);
                    ht.Add("lco_code", lco_code);
                    ht.Add("custid", customer_no);
                    ht.Add("vcid", vc);
                    ht.Add("custname", _cust_name);
                    ht.Add("cust_addr", _cust_addr);
                    ht.Add("planid", _plan_poid);
                    ht.Add("flag", action);
                    ht.Add("expdate", expiry_date);
                    ht.Add("actidate", activation_date);
                    ht.Add("request", req_data);
                    ht.Add("reason_id", "");
                    ht.Add("IP", Ip);

                    //validation procedure call
                    string validation_res = validProcess(ht);
                    if (validation_res.StartsWith("FAIL"))
                    {
                        //TODO - failure while processing transaction
                        string validation_fail_reason = validation_res.Split('$')[1];
                        updateStatus(customer_no, vc, plan_name, action, "0", "Transaction failed - " + validation_fail_reason, lco_code);
                        continue;
                    }
                    else
                    {
                        request_id = validation_res;
                    }

                    //obrm api call
                    string transaction_response = OBRMprocess(data, "R");
                    obrm_status = transaction_response.Split('$')[0];
                    try
                    {
                        if (obrm_status == "0" || obrm_status == "1")
                        {
                            obrm_msg = transaction_response.Split('$')[2];
                        }
                        else
                        {
                            obrm_status = "1";
                            obrm_msg = transaction_response;
                        }
                    }
                    catch (Exception ex)
                    {
                        obrm_status = "1";
                        obrm_msg = transaction_response;
                    }

                    //final bulk provi ins procedure call
                    ht.Add("obrmsts", obrm_status);
                    ht.Add("request_id", request_id);
                    ht.Add("response", transaction_response);
                    ht.Add("autorenew", autorenewFlag);// added by vivek 20150627
                    Cls_Business_TransHwayBulkOperation objBusiness = new Cls_Business_TransHwayBulkOperation();
                    string resp = objBusiness.bulkTransIns(ht);
                    string[] finalres = resp.Split('$');
                    if (finalres[0] == "9999")
                    {

                        //

                        //TODO - msgboxstr_refresh("Transaction successful : " + obrm_msg);
                        updateStatus(customer_no, vc, plan_name, action, "1", "Transaction Successful - " + obrm_msg, lco_code);
                        Cls_Business_TransHwayBulkOperation objoperations = new Cls_Business_TransHwayBulkOperation(); // added by vivek 20150627
                        string Strres = objoperations.ProvECS(ht); // added by vivek 20150627
                    }
                    else
                    {
                        if (obrm_status == "0")
                        {
                            //TODO - msgboxstr_refresh("Transaction successful by OBRM but failure at UPASS : " + finalres[1]);
                            updateStatus(customer_no, vc, plan_name, action, "0", "Transaction successful by OBRM but failure at UPASS - " + finalres[1], lco_code);
                        }
                        else
                        {
                            //TODO - msgboxstr("Transaction failed : " + finalres[1] + " : " + obrm_msg);
                            updateStatus(customer_no, vc, plan_name, action, "0", "Failure-OBRM - " + finalres[1] + " : " + obrm_msg, lco_code);
                        }
                    }
                }
                else
                {
                    //TODO - validation failed
                    updateStatus(customer_no, vc, plan_name, action, "0", "Transaction failed - " + validated_data_arr[1], lco_code);
                    continue;
                }
            }
            //final temp to master movement 
            Cls_Business_TransHwayBulkOperation busObj = new Cls_Business_TransHwayBulkOperation();
            string summary_data = busObj.masterMovement(username, upload_id);
            if (summary_data.StartsWith("9999"))
            {
                string summary_fig = summary_data.Split('$')[1];
                lblSumFile.Text = summary_fig.Split('~')[0];
                lblSumTotal.Text = summary_fig.Split('~')[1];
                lblSumSuccess.Text = summary_fig.Split('~')[2];
                lblSumFailure.Text = summary_fig.Split('~')[3];
            }
            else
            {
                lblStatus.Text = summary_data.Split('$')[1];
            }
            grdExpiry.Visible = false;
            pnlSummary.Visible = true;
            //msgbox("Process complete...");
            //clear upload key from session, so that on page refresh, system will redirect to previous page (bulk renewal selection page) 
          
            //if (Session["upload_id"] != null)
            //{
            //    Session.Remove("upload_id");
            //}

            lblStatusHeading.Text = "Process complete...";
        }




        protected void btnCancelTransactions_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Transaction/TransHwayBulkRenewal.aspx");
        }

        protected void lblSumTotal_Click(object sender, EventArgs e)
        {
            string file_name = lblSumFile.Text;
            generateExcel(uploadid, 0, file_name);
        }

        protected void lblSumSuccess_Click(object sender, EventArgs e)
        {
            string file_name = lblSumFile.Text;
            generateExcel(uploadid, 1, file_name);
        }

        protected void lblSumFailure_Click(object sender, EventArgs e)
        {
            try
            {
                string file_name = lblSumFile.Text;
                generateExcel(uploadid, 2, file_name);
            }
            catch (Exception ex)
            {
                ////sw.Flush();
                // sw.Close();
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "GetExcelData");
            }
        }


      /*  protected void generateExcel(string upload_id, int status_flag, string file_name)
        {
            String xls_file_name = "";
            DateTime dd = DateTime.Now;
            string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

            //try
            //{
              
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
                Cls_Business_rptBulkUpload obj = new Cls_Business_rptBulkUpload();
                DataTable dt = null;
                dt = obj.GetExcelData(username, upload_id, status_flag);
                if (dt == null)
                {
                    lblStatus.Text = "Something went wrong while fetching details...";
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    lblStatus.Text = "No data found for clicked count figure...";
                    return;
                }

                
                xls_file_name = file_name;
                if (status_flag == 0)
                {
                    xls_file_name += "_all_";
                }
                else if (status_flag == 1)
                {
                    xls_file_name += "_success_";
                }
                else if (status_flag == 2)
                {
                    xls_file_name += "_fail_";
                }

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + xls_file_name + datetime + ".xls");

                int j = 0;
                String strheader = "Sr. No." + Convert.ToChar(9) +
                                   "Customer No" + Convert.ToChar(9) +
                                   "VC ID" + Convert.ToChar(9) +
                                   "LCO Code" + Convert.ToChar(9) +
                                   "Plan Name" + Convert.ToChar(9) +
                                   "Transaction Type" + Convert.ToChar(9) +
                                   "Upload Id" + Convert.ToChar(9) +
                                   "Date Time" + Convert.ToChar(9) +
                                   "Status" + Convert.ToChar(9) +
                                   "Status Message" + Convert.ToChar(9);


                while (j < dt.Rows.Count)
                {
                    sw.WriteLine(strheader);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        j = j + 1;
                        string strrow = j.ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["cust_no"].ToString() + Convert.ToChar(9) +
                            "'" + dt.Rows[i]["vc_id"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["lco_code"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["plan_name"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["action"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["upload_id"].ToString() + Convert.ToChar(9) +
                            "'" + dt.Rows[i]["upload_date"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["status"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["message"].ToString() + Convert.ToChar(9);
                        sw.WriteLine(strrow);
                    }
                }
                sw.Flush();
                sw.Close();
            //}
            //catch (Exception ex)
            //{
            //    ////sw.Flush();
            //    // sw.Close();
            //    Cls_Security objSecurity = new Cls_Security();
            //    objSecurity.InsertIntoDb(username, ex.Message.ToString(), "GetExcelData");

            //    Response.Write("Error : " + ex.Message.Trim());
            //    return;
            //}
            Response.Redirect("../MyExcelFile/" + xls_file_name + datetime + ".xls");
        }*/

        protected void generateExcel(string upload_id, int status_flag, string file_name)
        {
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
            Cls_Business_rptBulkUpload obj = new Cls_Business_rptBulkUpload();
            DataTable dt = null;
            dt = obj.GetExcelData(username, upload_id, status_flag);
            if (dt == null)
            {
                lblStatus.Text = "Something went wrong while fetching details...";
                return;
            }
            if (dt.Rows.Count == 0)
            {
                lblStatus.Text = "No data found for clicked count figure...";
                return;
            }

            DateTime dd = DateTime.Now;
            string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

            string xls_file_name = file_name;
            if (status_flag == 0)
            {
                xls_file_name += "_all_";
            }
            else if (status_flag == 1)
            {
                xls_file_name += "_success_";
            }
            else if (status_flag == 2)
            {
                xls_file_name += "_fail_";
            }

            StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + xls_file_name + datetime + ".xls");
            try
            {
                int j = 0;
                String strheader = "Sr. No." + Convert.ToChar(9) +
                                   "Customer No" + Convert.ToChar(9) +
                                   "VC ID" + Convert.ToChar(9) +
                                   "LCO Code" + Convert.ToChar(9) +
                                   "Plan Name" + Convert.ToChar(9) +
                                   "Transaction Type" + Convert.ToChar(9) +
                                   "Upload Id" + Convert.ToChar(9) +
                                   "Date Time" + Convert.ToChar(9) +
                                   "Status" + Convert.ToChar(9) +
                                   "Status Message" + Convert.ToChar(9);


                while (j < dt.Rows.Count)
                {
                    sw.WriteLine(strheader);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        j = j + 1;
                        string strrow = j.ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["cust_no"].ToString() + Convert.ToChar(9) +
                            "'" + dt.Rows[i]["vc_id"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["lco_code"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["plan_name"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["action"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["upload_id"].ToString() + Convert.ToChar(9) +
                            "'" + dt.Rows[i]["upload_date"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["status"].ToString() + Convert.ToChar(9) +
                            dt.Rows[i]["message"].ToString() + Convert.ToChar(9);
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
            Response.Redirect("../MyExcelFile/" + xls_file_name + datetime + ".xls");
        }
    }
}