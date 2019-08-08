using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassDAL.Helper;
using System.Text;
using System.Net;
using System.IO;
using PrjUpassBLL.Transaction;
using System.Collections;

namespace PrjUpassPl.Transaction
{
    public partial class frmFaultySTB_SWAP : System.Web.UI.Page
    {
        string oper_id = "";
        string username = "";
        string catid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Faulty STB SWAP";
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {
                Session["RightsKey"] = "N";
                oper_id = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
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
            string searhParam = txtSearch.Text;
            string search_type = RadSearchby.SelectedValue.ToString();
            string response_params = user_brmpoid + "$" + searhParam + "$SW";
            if (search_type == "0")
            {
                //if VC ID
                response_params += "$V";
            }
            // string apiResponse = callAPI(response_params, "6");
            string apiResponse = callAPI(response_params, "12");//
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
                    ViewState["custemail"] = lstResponse[1].ToString();
                    lblemail.Text = lstResponse[1].ToString();
                    //DATA ACCESS VALIDATION----------------------------------------------------------------------------------VALIDATION
                    Cls_Validation obj = new Cls_Validation();
                    string validate_cust_accesslco = obj.CustDataAccess(username, oper_id, lco_poid, Session["category"].ToString());
                    if (validate_cust_accesslco.Length == 0)
                    {
                        popMsg.Show();
                        lblPopupResponse.Text = "You have no privileges to access customer information as s/he belongs to other LCO";
                        return;
                    }
                    else
                    {
                        List<string> lcoidAndCity = new List<string>();
                        lcoidAndCity = validate_cust_accesslco.Split('$').ToList();
                        Session["lcoid"] = lcoidAndCity[0];
                        ViewState["cityid"] = lcoidAndCity[1];
                        Session["lco_username"] = lcoidAndCity[2];
                    }
                    string cust_services = lstResponse[15];
                    string[] service_arr = cust_services.Split('^');
                    ViewState["Service_Str"] = null;
                    ViewState["Service_Str"] = cust_services.ToString();
                    string stb_status = "";
                    DataTable dtStbs = new DataTable();
                    dtStbs.Columns.Add(new DataColumn("STB_NO"));
                    dtStbs.Columns.Add(new DataColumn("VC_ID"));
                    dtStbs.Columns.Add(new DataColumn("SERVICE_STRING"));
                    dtStbs.Columns.Add(new DataColumn("Status"));
                    dtStbs.Columns.Add(new DataColumn("PARENT_CHILD_FLAG"));
                    dtStbs.Columns.Add(new DataColumn("TAB_FLAG"));
                    dtStbs.Columns.Add(new DataColumn("Last_Status"));
                    dtStbs.Columns.Add(new DataColumn("Pack_Type"));
                    DataTable sortedDT = new DataTable();
                    string strvcid = "";
                    int k = 1;
                    ViewState["parentsmsg"] = "0";
                    foreach (string service in service_arr)
                    {
                        string stb_no = service.Split('!')[1];
                        string vc_id = service.Split('!')[2];
                        stb_status = service.Split('!')[4];
                        string Pack_Type = service.Split('!')[5];
                        string parent_child_flag = service.Split('!')[6];
                        string Last_Status = service.Split('!')[7];
                        string tab_flag = "";
                        if (parent_child_flag == null || parent_child_flag == "0")
                        {
                            parent_child_flag = "0";
                            lblStbNo.Text = service.Split('!')[1];
                            lblVCID.Text = service.Split('!')[2];
                        }
                        if (parent_child_flag == "1")
                        {
                            k = k + 1;
                            tab_flag = "lnkAddon" + k.ToString();
                            parent_child_flag = "1";
                        }
                        else
                        {
                            tab_flag = "lnkAddon1";
                        }
                        if (stb_status == "10103")
                        {
                            continue; //if status is terminated
                        }
                        if (stb_no == "" || vc_id == "")
                        {
                            continue; //id stan_no or vc_id is blank
                        }
                        DataRow drStbRow = dtStbs.NewRow();
                        drStbRow["STB_NO"] = stb_no;
                        drStbRow["VC_ID"] = vc_id;
                        ViewState["vcid"] = vc_id;
                        strvcid += strvcid + vc_id + ",";
                        drStbRow["SERVICE_STRING"] = service;
                        drStbRow["Status"] = stb_status;
                        ViewState["stb_status"] = stb_status;
                        drStbRow["PARENT_CHILD_FLAG"] = parent_child_flag;
                        drStbRow["TAB_FLAG"] = tab_flag;
                        drStbRow["Last_Status"] = Last_Status;
                        drStbRow["Pack_Type"] = Pack_Type;
                        dtStbs.Rows.Add(drStbRow);
                        DataView dv = dtStbs.DefaultView;
                        dv.Sort = "PARENT_CHILD_FLAG Asc";
                        sortedDT = dv.ToTable();
                    }
                    strvcid = strvcid.TrimEnd(',');
                    if (sortedDT.Rows.Count == 0)
                    {
                        popMsg.Show();
                        lblPopupResponse.Text = "No STB found";
                        return;
                    }
                    ViewState["vcdetail"] = sortedDT;
                    DataTable dtVCinfo = new DataTable();
                    if (sortedDT.Rows.Count > 0)
                    {
                        dtVCinfo.Columns.Add(new DataColumn("TV"));
                        dtVCinfo.Columns.Add(new DataColumn("VC_ID"));
                        dtVCinfo.Columns.Add(new DataColumn("STB_NO"));
                        dtVCinfo.Columns.Add(new DataColumn("STATUS"));
                        dtVCinfo.Columns.Add(new DataColumn("BOX_TYPE"));
                        dtVCinfo.Columns.Add(new DataColumn("SUSPENSION_DATE"));
                        string Parent_Flag = "";
                        int parentflagvalue = 0;
                        string ServiceStatus = "";
                        for (int i = 0; i < sortedDT.Rows.Count; i++)
                        {
                            DataRow drvcinfo = dtVCinfo.NewRow();
                            Parent_Flag = sortedDT.Rows[i]["TAB_FLAG"].ToString();
                            Parent_Flag = Parent_Flag.Substring(Parent_Flag.Length - 1);
                            parentflagvalue = Convert.ToInt32(Parent_Flag);
                            parentflagvalue = parentflagvalue - 1;
                            if (parentflagvalue == 0)
                            {
                                drvcinfo["TV"] = "Main TV";
                                ViewState["VC_IDMAIN"] = sortedDT.Rows[i]["VC_ID"].ToString();
                                ViewState["STB_NOMAIN"] = sortedDT.Rows[i]["STB_NO"].ToString();
                            }
                            if (parentflagvalue != 0)
                            {
                                drvcinfo["TV"] = "Addon " + parentflagvalue.ToString();
                            }
                            drvcinfo["VC_ID"] = sortedDT.Rows[i]["VC_ID"].ToString();
                            drvcinfo["STB_NO"] = sortedDT.Rows[i]["STB_NO"].ToString();
                            ServiceStatus = sortedDT.Rows[i]["Status"].ToString();
                            DateTime dttime = Convert.ToDateTime(sortedDT.Rows[i]["Last_Status"].ToString());
                            drvcinfo["SUSPENSION_DATE"] = dttime.ToString("dd-MMM-yyyy");
                            if (ServiceStatus == "10100")
                            {
                                drvcinfo["SUSPENSION_DATE"] = "";
                                drvcinfo["STATUS"] = "Active";
                            }
                            else
                            {
                                drvcinfo["SUSPENSION_DATE"] = dttime.ToString("dd-MMM-yyyy");
                                drvcinfo["STATUS"] = "In-Active";
                            }
                            drvcinfo["BOX_TYPE"] = sortedDT.Rows[i]["Pack_Type"].ToString();
                            dtVCinfo.Rows.Add(drvcinfo);
                        }
                    }
                    GridVC.DataSource = dtVCinfo;
                    GridVC.DataBind();
                    lblCustNo.Text = cust_id;
                    lblCustName.Text = cust_name;
                    lblCustAddr.Text = cust_addr.Replace('|', ',');
                    lbltxtmobno.Text = cust_mobile_no;
                    ViewState["custaddr"] = lblCustAddr.Text;
                    ViewState["custmob"] = lbltxtmobno.Text;
                    divdet.Visible = true;
                }
            }
            catch(Exception ex)
            {
                divdet.Visible =false;
                popMsg.Show();
                lblPopupResponse.Text = "Customer data not found";
            }
            
        }
        public String callAPI(string Request, string request_code)
        {
            try
            {
                string fromSender = string.Empty;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(Request);
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRM/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.21/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
               //  HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
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
                //FileLogText("Admin", "callAPI", " Error:" + ex.Message.Trim(), "");
                return "1$---$" + ex.Message.Trim();
            }
        }//shri

        protected void btnSwap_click(object sender, EventArgs e)
        {
            int rowindex = Convert.ToInt32((((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex);
            string VC_ID = ((HiddenField)GridVC.Rows[rowindex].FindControl("hdnVC_ID")).Value;
            string STB_NO = ((HiddenField)GridVC.Rows[rowindex].FindControl("hdnSTB_NO")).Value;
            txtSwapSTBID.Text = STB_NO;
            txtswapNewSTBID.Text = "";
            ddlSawpReason.SelectedValue = "0";
            mpeSwapPop.Show();
        }
        protected void btnswapConf_Click(object sender, EventArgs e)
        {
            if (ddlSawpReason.SelectedValue == "0")
            {
                lblPopupResponse1.Text = "Select Reason.";
                mpeSwapPop.Show();
                return;
            }
            ViewState["SWAPSTBID"] = txtSwapSTBID.Text;
            ViewState["SWAPNewSTBID"] = txtswapNewSTBID.Text;
            ViewState["SWAPReason"] = ddlSawpReason.Text;
            Cls_Business_Faulty_Swap objTran = new Cls_Business_Faulty_Swap();
            string OutStatus = "";

            objTran.Get_Faulty_Swap(txtswapNewSTBID.Text, username,out OutStatus);
            if (OutStatus != "" && OutStatus != null)
            {
                lblSTBID.Text = ViewState["SWAPSTBID"].ToString();
                lblNewSTBID.Text = ViewState["SWAPNewSTBID"].ToString();
                lblCofReason.Text = ViewState["SWAPReason"].ToString();
                mpSwapConfirm.Show();
            }
            else
            {
                mpeSwapPop.Hide();
                lblPopupResponse.Text = "Invalid STB.";
                popMsg.Show();
            }
            
        }


        protected void btnModifyConfirm_click(object sender, EventArgs e)
        {
            string user_brmpoid = "";
            if (Session["operator_id"] != null && Session["username"] != null && Session["user_brmpoid"] != null)
            {
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
           /* // string response_params = username + "$" + searhParam + "$SW";
            string response_params = user_brmpoid + "$" + lblCofChildVC.Text + "$SW";

            //if VC ID
            response_params += "$V";

            // string apiResponse = callAPI(response_params, "6");
            string SERVICE_OBJ = "";
            string DEVICE_ID = "";
            string apiResponse = callAPI(response_params, "12");//
            try
            {
                if (apiResponse != "")
                {

                    List<string> lstResponse = new List<string>();

                    lstResponse = apiResponse.Split('$').ToList();
                    ViewState["accountPoid"] = lstResponse[6];
                    string cust_services = lstResponse[15];
                    string[] service_arr = cust_services.Split('^');
                    ViewState["Service_Str"] = null;
                    ViewState["Service_Str"] = cust_services.ToString();

                    ViewState["parentsmsg"] = "0";
                    foreach (string service in service_arr)
                    {
                        string parent_child_flag = service.Split('!')[6];
                        string vc_id = service.Split('!')[2];
                        if (parent_child_flag == "0")
                        {
                            Session["SERVICE_STRING"] = service;
                            SERVICE_OBJ = service.Split('!')[0];

                        }
                        if (vc_id == lblCofChildVC.Text)
                        {
                            DEVICE_ID = service.Split('!')[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            string response_params2 = Session["username"] + "$" + ViewState["accountPoid"] + "$" + DEVICE_ID + "$" + SERVICE_OBJ;//GetHwayOBRMPARENTCHILDSTB_SWAP_Action

            string apiResponse2 = callAPI(response_params2, "36");
            string[] GetAPIresponse = apiResponse2.Split('$');
            if (GetAPIresponse[0] == "0")
            {*/
                Hashtable ht = new Hashtable();
                ht.Add("username", Session["username"].ToString());
                ht.Add("AccountNo", lblCustNo.Text);
                ht.Add("FromVC", "");
                ht.Add("FromSTB", lblNewSTBID.Text);
                ht.Add("TOVC", "");
                ht.Add("TOSTB", lblStbNo.Text);
                ht.Add("Reason", lblCofReason.Text);
                ht.Add("Type", "FSS");
                ht.Add("Status", "");
                ht.Add("CustomerName", lblCustName.Text);
                ht.Add("MobileNo", lbltxtmobno.Text);
                ht.Add("EmailID", lblemail.Text);
                ht.Add("CustomerAdd", lblCustAddr.Text);
                Cls_Business_Faulty_Swap obj = new Cls_Business_Faulty_Swap();
                string response = obj.getSTB_Swap(ht);
                string[] Getresponse = response.Split('#');
                if (Getresponse[0] == "9999")
                {
                    mpSwapConfirm.Hide();
                    lblPopupResponse.Text = Getresponse[1].ToString();
                    popMsg.Show();
                }
                else
                {
                    mpSwapConfirm.Hide();
                    lblPopupResponse.Text = Getresponse[1].ToString();
                    popMsg.Show();
                }
           /* }
            else
            {
                lblPopupResponse.Text = "Failed from OBRM :" + GetAPIresponse[1].ToString();
                popMsg.Show();
            }*/
        }

    }
}