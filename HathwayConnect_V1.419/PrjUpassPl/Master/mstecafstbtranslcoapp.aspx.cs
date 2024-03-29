﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;
using System.Collections;
using PrjUpassBLL.Master;
using System.Net;
using System.Text;
using System.IO;

namespace PrjUpassPl.Master
{
    public partial class mstecafstbtranslcoapp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.PageHeading = "STB Transfer Approval";
                if (Session["username"] != null)
                {
                    string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                    OracleConnection con = new OracleConnection(strCon);

                    string query = "";
                    query += " select num_stbtransfer_id transid, var_stbtransfer_stbno stbno,var_stbtransfer_lcocode lco,num_stbtransfer_stbamount amount,var_stbtransfer_adminlevel,";
                    query += " date_stbtransfer_insdate insdate from AOUP_LCOPRE_STBTRANSFER_MST where var_stbtransfer_translcocode='" + Session["username"].ToString() + "' and var_stbtransfer_status is null ";
                    query += " and var_stbtransfer_adminlevel='N' and ( var_stbtransfer_adminstatus is null or var_stbtransfer_adminstatus='A') and var_stbtransfer_refid='" + Session["RefId"].ToString() + "'";
                    OracleCommand cmd = new OracleCommand(query, con);
                    OracleDataAdapter DaObj = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    DaObj.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        grdSTBSwap.DataSource = dt;
                        grdSTBSwap.DataBind();
                        for (int i = 0; i < grdSTBSwap.Rows.Count; i++)
                        {
                            RadioButton RdoAccept = (RadioButton)grdSTBSwap.Rows[i].FindControl("RdoAccept");
                            RadioButton RdoCancel = (RadioButton)grdSTBSwap.Rows[i].FindControl("RdoCancel");

                            RdoAccept.Checked = true;
                            RdoAccept.GroupName = "Group" + i.ToString();
                            RdoCancel.GroupName = "Group" + i.ToString();
                        }
                    }
                    else
                    {

                        ViewState["ErrorInfo"] = "9999";
                        PopMsgBoxErr.Show();
                        lblerror.Text = "No Record Found";
                        return;
                    }
                }
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Reports/EcafPages.aspx");
        }

        protected void btnCloseInfo_Click(object sender, EventArgs e)
        {
            if (ViewState["ErrorInfo"] != null)
            {
                if (ViewState["ErrorInfo"].ToString() == "9999")
                {
                    Response.Redirect("../Reports/EcafPages.aspx");
                }
            }
        }
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (txtremark.Text.Trim() == "")
            {
                PopMsgBoxErr.Show();
                lblerror.Text = "Please enter remark";
                return;
            }
            popMsgBox.Show();
            btncnfmBlck.Visible = true;
            lblResponseMsg.Text = "Are You Sure?";
        }

        protected void btncnfmBlck_Click(object sender, EventArgs e)
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);

            String AccessString = "";

            for (int i = 0; i < grdSTBSwap.Rows.Count; i++)
            {
                RadioButton RdoAccept = (RadioButton)grdSTBSwap.Rows[i].FindControl("RdoAccept");
                RadioButton RdoCancel = (RadioButton)grdSTBSwap.Rows[i].FindControl("RdoCancel");
                String Status = "";
                CheckBox ChkMenu = (CheckBox)grdSTBSwap.Rows[i].Cells[0].FindControl("ChkSTB");

                if (ChkMenu.Checked == true)
                {
                    if (grdSTBSwap.Rows.Count == 1)
                    {
                        if (RdoAccept.Checked == true)
                        {
                            string STB = grdSTBSwap.Rows[i].Cells[1].Text;
                            string getaccno = "select account_no from reports.hwcas_brm_cust_master@caslive_new where stb='" + STB + "' and rownum=1";
                            OracleCommand cmdGetAccNo = new OracleCommand(getaccno, con);
                            OracleDataAdapter DaObjAccNo = new OracleDataAdapter(cmdGetAccNo);
                            DataTable dtAccNo = new DataTable();
                            string AccNo = "";
                            DaObjAccNo.Fill(dtAccNo);
                            if (dtAccNo.Rows.Count > 0)
                            {
                                AccNo = dtAccNo.Rows[0]["account_no"].ToString();
                            }
                            else
                            {
                                PopMsgBoxErr.Show();
                                lblResponseMsg.Text = "Account No not found for STB :" + STB;
                                return;
                            }

                            string ServiceObj = "";
                            string response_params = Convert.ToString(Session["user_brmpoid"]) + "$" + AccNo + "$SW";
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

                                    foreach (string service in service_arr)
                                    {
                                        string stb_no = service.Split('!')[1];

                                        if (stb_no == STB)
                                        {
                                            ServiceObj = service.Split('!')[0];
                                        }
                                    }
                                }
                                else
                                {
                                    PopMsgBoxErr.Show();
                                    lblerror.Text = "Failed From OBRM";
                                    return;
                                }
                                string response_paramsSTBTransfer = Session["username"].ToString() + "$" + ViewState["accountPoid"].ToString() + "$" + STB + "$" + AccNo;
                                string apiResponseSTBTransfer = callAPI(response_paramsSTBTransfer, "37");//
                                string[] apiResponseSTBTransferArr = apiResponseSTBTransfer.Split('$');

                                if (apiResponseSTBTransferArr[0].ToString() == "0")
                                {

                                }
                                else
                                {
                                    PopMsgBoxErr.Show();
                                    lblerror.Text = "Failed From OBRM : " + apiResponseSTBTransferArr[1].ToString();
                                    return;
                                }


                            }
                            catch (Exception ex)
                            {
                                PopMsgBoxErr.Show();
                                lblerror.Text = "Failed From OBRM" + ex.Message;
                                return;
                            }
                        }
                    }

                    string msgsubid = ((HiddenField)grdSTBSwap.Rows[i].FindControl("hdntransid")).Value;
                    if (RdoAccept.Checked == true)
                    {
                        Status = "A";
                    }
                    else
                    {
                        Status = "C";
                    }
                    AccessString += msgsubid + "#" + Status + "$";
                }
            }

            if (AccessString != "")
            {
                AccessString = AccessString.Remove(AccessString.Length - 1);
            }
            else
            {

                PopMsgBoxErr.Show();
                lblerror.Text = "Please select atleast one record";
                return;
            }


            Hashtable ht = new Hashtable();

            ht.Add("in_username", Session["username"].ToString());
            ht.Add("in_AccessString", AccessString);
            ht.Add("in_remark", txtremark.Text.Trim());


            Cls_BLL_ecafstbtransfer obj = new Cls_BLL_ecafstbtransfer();
            string response = obj.InsertLCOTransfer(Session["username"].ToString(), ht);
            string[] responseArr = response.Split('$');

            popMsgBox.Show();
            lblResponseMsg.Text = responseArr[1].ToString();
            btncnfmBlck.Visible = false;
            ViewState["ErrorInfo"] = "9999";

        }
        private void FileLogText(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\HwayObrm_Web_" + filename + ".txt", true);
            try
            {
                sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(strRequest.Trim());
                sw.WriteLine(strResponse.Trim());
                sw.WriteLine("************************************************************************************************************************");
            }
            catch (Exception ex)
            {
                Response.Write("Error while writing logs : " + ex.Message.ToString());
            }
            finally
            {
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        public String callAPI(string Request, string request_code)
        {
            try
            {
                string fromSender = string.Empty;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(Request);
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.20/testhwayobrmcallservice/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRM/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
               // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.21/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                  HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
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


    }
}