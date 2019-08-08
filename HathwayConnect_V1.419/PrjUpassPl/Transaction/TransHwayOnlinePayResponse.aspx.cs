using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using PrjUpassDAL.Authentication;
using System.Collections;
using System.Configuration;
using PrjUpassBLL.Transaction;
using System.IO;
using System.Data;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayOnlinePayResponse : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //int intorderid;
            Master.PageHeading = "Online Payment Response";
            string BillDeskAuthoMessage = "";
            Session["RightsKey"] = "N";
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["Response"].ToString() != "")
                    {
                        //PrjUpassDAL.Helper.Cls_Security objSecurity = new PrjUpassDAL.Helper.Cls_Security();
                        //objSecurity.InsertIntoDb(Session["username"].ToString(), "ResponseLog_" + Request.QueryString["Response"].ToString(), "TransHwayOnlinePayResponse.cs-pageload");
                        //FileLogText("Admin", "Online Payment Response", " Response:" + Session.SessionID, "");

                        // Response=10001~1~31~MSBI0412001668~00000002.00~0300~Success
                        //FileLogText("Admin", "Online Payment Response", " Response:" + Request.QueryString["Response"].ToString(), "");
                        // FileLogText("Admin", "Online Payment Response", " Error:" + ex.Message.Trim(), "");
                        String ResStr = Request.QueryString["Response"].ToString();
                        String[] strReqPara = ResStr.Split('~');
                        BillDeskAuthoMessage = strReqPara[6];
                        setsession(Convert.ToString(strReqPara[1]));
                        String Indentifier = "";
                        if (Convert.ToInt32(strReqPara[5]) == 0300)
                        {
                            int TransId = Convert.ToInt32(strReqPara[1]);
                            int UbilldeskOrderNo = Convert.ToInt32(strReqPara[2]);
                            string BillDeskRef = strReqPara[3];
                            int BillDeskAuthoStatus = Convert.ToInt32(strReqPara[5]);
                             Indentifier = "BD";
                            try
                            {
                                 Indentifier = strReqPara[8];
                            }
                            catch
                            { }
                            ViewState["TransId"] = TransId;


                            string StrStatus = CallPaymentProc(TransId, UbilldeskOrderNo, BillDeskRef, BillDeskAuthoStatus, BillDeskAuthoMessage, Indentifier);
                             string[] response_arr = StrStatus.Split('$');
                          string   Status = response_arr[0].ToString();
                           if (Status == "successfully")
                            {
                                if (Indentifier == "CT")
                                {
                                    lblResponseMsg.Text = "Transaction Done successfully.";
                                    lbltype.Text = "Citrus Reference ID : ";
                                    Lblrefno.Text = BillDeskRef;
                                    LblCurruntBal.Text = response_arr[1];
                                    ViewState["rcptno"] = response_arr[2];
                                    BtngenexportPDF.Visible = true;
                                }
                                else
                                {
                                    lblResponseMsg.Text = "Transaction Done successfully.";
                                    lbltype.Text = "Bill Desk Reference ID : ";
                                    Lblrefno.Text = BillDeskRef;
                                    LblCurruntBal.Text = response_arr[1];
                                    ViewState["rcptno"] = response_arr[2];
                                    BtngenexportPDF.Visible = true;
                                }
                              //  ,Bill Desk Refrence ID :" + BillDeskRef;

                            }
                            else
                            {
                                if (Indentifier == "CT")
                                {
                                    lblResponseMsg.Text = "Transaction successfully from Citrus ,But In Upass Transaction failed :" + StrStatus;
                                    Divsts.Visible = true;
                                    btnBck.Visible = false;
                                    Lblrefno.Text = BillDeskRef;
                                    BtngenexportPDF.Visible = false;
                                    LblCurruntBal.Text = response_arr[1];
                                }
                                else
                                {
                                    lblResponseMsg.Text = "Transaction successfully from bill desk ,But In Upass Transaction failed :" + StrStatus;
                                    Divsts.Visible = true;
                                    btnBck.Visible = false;
                                    Lblrefno.Text = BillDeskRef;
                                    BtngenexportPDF.Visible = false;
                                    LblCurruntBal.Text = response_arr[1];
                                }
                           }

                        }

                        else if (Convert.ToInt32(strReqPara[5]) == 0399)
                        {
                            Response.Redirect("http://hathwayconnect.com/HwayConnect/Transaction/HwayTransLcoOnlinePayment.aspx", false);
                            //Response.Redirect("http://localhost:34530/Transaction/HwayTransLcoOnlinePayment.aspx");
                        
                        return;
                        }

                        else
                        {
                            if (Indentifier == "CT")
                            {
                                lblResponseMsg.Text = "Failed Transactionn from Bill Desk." + BillDeskAuthoMessage;
                                btnBck.Visible = false;
                                Divsts.Visible = false;
                                BtngenexportPDF.Visible = false;
                            }
                            else
                            {
                                lblResponseMsg.Text = "Failed Transactionn from Citrus." + BillDeskAuthoMessage;
                                btnBck.Visible = false;
                                Divsts.Visible = false;
                                BtngenexportPDF.Visible = false;
                            }
                            return;
                        }

                    }
                }

                catch
                {
                    lblResponseMsg.Text = "Cancel Transaction~Response can not be blank or zero";
                    btnBck.Visible = false;
                    Divsts.Visible = false;
                    return;
                }

            }
        }

        private string setsession(string transId)
        {
            string sissionId = "";
            try
            {
                string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                OracleConnection con = new OracleConnection(strCon);
                string str = "";

                str = "select * from aoup_lcopre_online_session a where num_lcopay_transid=" + transId;


                DataTable DtObj = new DataTable();

                OracleCommand Cmd = new OracleCommand(str, con);
                OracleDataAdapter DaObj = new OracleDataAdapter(Cmd);
                DaObj.Fill(DtObj);



                if (DtObj.Rows.Count != 0)
                {
                    Session["user_id"] = DtObj.Rows[0]["var_userid"];
                    Session["user_brmpoid"] = DtObj.Rows[0]["var_brmpoid"];
                    Session["lco_username"] = DtObj.Rows[0]["var_username"];
                    Session["operator_id"] = DtObj.Rows[0]["var_operid"];
                    Session["category"] = DtObj.Rows[0]["var_category"];
                    Session["name"] = DtObj.Rows[0]["var_name"];
                    Session["last_login"] = DtObj.Rows[0]["var_last_login"];
                    Session["login_flag"] = DtObj.Rows[0]["var_login_flag"];
                    Session["username"] = DtObj.Rows[0]["var_username"];
                    Session["showimage"] = "N";
                    Session["MIAflag"] = "N";
                    sissionId = Convert.ToString(DtObj.Rows[0]["var_session_id"]);
                }

            }
            catch (Exception ex)
            {
            }
            return sissionId;

        }

        protected string CallPaymentProc(int TransId, int UbilldeskOrderNo, string BillDeskRef, int BillDeskAuthoStatus, string BillDeskAuthoMessage, string Indentifier)
        {
            //string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            //OracleConnection con = new OracleConnection(strCon);
            //Cls_Data_Auth auth = new Cls_Data_Auth();
            //string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            Hashtable ht = new Hashtable();
            string strrespo = "";
            //try
            //{
            //   string str = "select * from aoup_lcopre_lco_online_pay_det where trim(num_lcopay_transid)=trim("+intorderid+")";

            //    OracleCommand cmd = new OracleCommand(str, con);
            //    con.Open();
            //    OracleDataReader dr = cmd.ExecuteReader();
               
            //    if (dr.HasRows)
            //    {
            //        while (dr.Read())
            //        {

            //            ht.Add("User", dr["var_lcopay_insby"].ToString());
            //            ht.Add("CustCode", dr["var_lcopay_lcocode"].ToString());
            //            ht.Add("Amount", Convert.ToInt32(dr["num_lcopay_amount"].ToString()));
            //            ht.Add("PayMode", dr["var_lcopay_paymode"].ToString());

            //            ht.Add("chequeddno", dr["var_lcopay_chqddno"].ToString());
            //            ht.Add("CheckDate", dr["dat_lcopay_chequedt"].ToString());

            //            ht.Add("BankName", dr["var_lcopay_bank"].ToString());
            //            ht.Add("Branch", dr["var_lcopay_branch"].ToString());
            //            ht.Add("Remark", dr["var_lcopay_remark"].ToString());
            //            ht.Add("ReceiptNo", dr["var_lcopay_erpreceiptno"].ToString());
            //            ht.Add("IP", Ip);
                     
                 
            //        }
                  
            //    }
            //    else
            //    {
            //        ListItem lst = new ListItem();
            //        lst.Text = "No Record Found ";
            //        lst.Value = "0";
            //        return lst.Text;
                   
            //    }
             try
            {
                ht.Add("TransId", TransId);
                ht.Add("UbilldeskOrderNo", UbilldeskOrderNo);
                ht.Add("BillDeskRef", BillDeskRef);
                ht.Add("BillDeskAuthoStatus", BillDeskAuthoStatus);
                ht.Add("BillDeskAuthoMessage", BillDeskAuthoMessage);
                ht.Add("Indentifier", Indentifier);
                ViewState["BillDeskRef"] = BillDeskRef;
                 
             Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
                string response = obj.LcoOnlinPayment(ht);
                if (response == "ex_occured")
                {
                  strrespo="Transaction successfully from bill desk ,But Upass In failed";
                }
                else
                {

                    strrespo= response;
                }
              
                
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
             
            }
             return strrespo;
           
        }

        private void FileLogText(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\Online_Getway_Payment_Response_" + filename + ".txt", true);
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

        protected void btnBck_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Transaction/TransHwayUserCreditLimit_New.aspx");
            return;


            
        }

        protected void BtngenexportPDF_Click(object sender, EventArgs e)
        {
            string username = Convert.ToString(Session["lco_username"]);
           string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
           OracleConnection con = new OracleConnection(strCon);

           
            try
            {
               double Amount=0;
                string User = "";
                string lcoCode = "";
                string Remark = "";
                try
                {
                    string str = "select * from aoup_lcopre_lco_online_pay_det where trim(num_lcopay_transid)=trim(" + ViewState["TransId"] + ")";

                    OracleCommand cmd = new OracleCommand(str, con);
                    con.Open();
                    OracleDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            User=dr["var_lcopay_insby"].ToString();
                            lcoCode=dr["var_lcopay_lcocode"].ToString();
                            Amount= Convert.ToDouble(dr["num_lcopay_amount"].ToString());                            
                            Remark=dr["var_lcopay_remark"].ToString();
                           
                        }

                    }
                    else
                    {
                        ListItem lst = new ListItem();
                        lst.Text = "No Record Found ";
                        lst.Value = "0";
                        //return lst.Text;
                        return;

                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message.ToString());
                }

                Session["rcpt_pt_rcptno1"] = ViewState["rcptno"].ToString();
                Session["rcpt_pt_rcptno2"] = ViewState["BillDeskRef"].ToString();
                Session["rcpt_pt_date1"] = DateTime.Now.ToString("dd/MM/yyyy");
                Session["rcpt_pt_cashiername"] = "N/A";
                Session["rcpt_pt_address"] = "";
                Session["rcpt_pt_company"] = "";
                Session["rcpt_pt_lcocd1"] = username;
                Session["rcpt_pt_lconm1"] = Session["name"].ToString();
                Session["rcpt_pt_amt1"] = Amount;
                Session["rcpt_pt_paymode1"] = "Online";
                Session["rcpt_pt_cheqno1"] = "N/A";
                Session["rcpt_pt_bnknm1"] = "N/A";
                Session["rcpt_pt_premark1"] = Remark.ToString();
                Response.Write("<script language='javascript'> window.open('../Transaction/rcptPaymentReceiptInvoice.aspx', 'Print_Receipt','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes,location=no,status=no');</script>");
                return;
                    online_payment_pdf myreceiptreport = new online_payment_pdf();
                    string ReportPath = Server.MapPath("online_payment_pdf.rpt");
                    myreceiptreport.Load(ReportPath);

                    myreceiptreport.SetParameterValue("par_rcptno", ViewState["BillDeskRef"].ToString());
                    myreceiptreport.SetParameterValue("par_rcptdt",DateTime.Now.ToString("dd/MM/yyyy"));
                    myreceiptreport.SetParameterValue("par_lcocd", username);
                    myreceiptreport.SetParameterValue("par_lconm", Session["name"].ToString());
                    myreceiptreport.SetParameterValue("par_cashier", "");
                    myreceiptreport.SetParameterValue("par_amt", Amount);                    
                    myreceiptreport.SetParameterValue("par_premark", Remark.ToString());
                    myreceiptreport.SetParameterValue("par_company", "");
                    myreceiptreport.SetParameterValue("par_address","");
                    myreceiptreport.SetParameterValue("par_transid", ViewState["rcptno"].ToString());


                    String ExportPath = Server.MapPath("..\\MyExcelFile\\") + ViewState["BillDeskRef"].ToString() + " myrecpt " + DateTime.Now.ToString("dd-MM-yy hh mm ss") + ".pdf";
                    myreceiptreport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, ExportPath);

                    FileStream fs = null;
                    fs = File.Open(ExportPath, FileMode.Open);
                    byte[] btFile = new byte[fs.Length];
                    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    Response.AddHeader("Content-disposition", "attachment; filename=" + ViewState["BillDeskRef"].ToString() + " myrecpt " + DateTime.Now.ToString("dd-MM-yy hh mm ss") + ".pdf");
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(btFile);
                    Response.End();                   
                
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
        }

        }
    
}