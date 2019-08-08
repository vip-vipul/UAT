using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassDAL.Authentication;
using System.Collections;
using PrjUpassBLL.Authentication;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Data;

namespace PrjUpassPl
{
    public partial class Login1 : System.Web.UI.Page
    {
        private double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            //Response.Redirect("/HwayConnect/");
            //return;
            //Double SchemeUpto = ConvertToUnixTimestamp(Convert.ToDateTime("15/03/2019"));
            //Int32 SchemeuptoInt = Convert.ToInt32(SchemeUpto);
            DateTime str = Convert.ToDateTime(System.DateTime.Now.ToString("dd-MMM-yyyy"));

            if (!IsPostBack)
            {
                FillCapctha();
            }

            hdnWebUrl.Value = string.Format("http{0}://{1}:{2}{3}",
                                (Request.IsSecureConnection) ? "s" : "",
                                 Request.Url.Host,
                                 Request.Url.Port,
                                 Page.ResolveUrl("~/RandStrGenerator.asmx/GenerateRandStr")
                               );

            Cls_Business_Auth objAuth = new Cls_Business_Auth();
            String broadcastmesg = objAuth.getbroadcastmesg();
            lblbroadcast.InnerText = broadcastmesg.Trim();

            String Ip = GetIPAddress(HttpContext.Current.Request);
            //  Session["NewCap"] = Session["captcha"].ToString();
        }
        public string GetIPAddress(HttpRequest request)
        {
            string ip;
            try
            {
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    if (ip.IndexOf(",") > 0)
                    {
                        string[] ipRange = ip.Split(',');
                        int le = ipRange.Length - 1;
                        ip = ipRange[le];
                    }
                }
                else
                {
                    ip = request.UserHostAddress;
                }
            }
            catch { ip = null; }

            return ip;
        }

        void FillCapctha()
        {

            try
            {

                Random random = new Random();

                string combination = "0123456789";

                StringBuilder captcha = new StringBuilder();

                for (int i = 0; i < 6; i++)

                    captcha.Append(combination[random.Next(combination.Length)]);

                Session["captcha"] = captcha.ToString();

                imgCaptcha.ImageUrl = "GenerateCaptcha.aspx?" + DateTime.Now.Ticks.ToString();
            }
            catch
            {
                throw;
            }

        }

        protected void imgcatcharefresh_Click(object sender, ImageClickEventArgs e)
        {
            FillCapctha();
            txtcaptcha.Text = "";
        }
        protected void ibtLogIn_Click(object sender, ImageClickEventArgs e)
        {
            //if (Session["captcha"].ToString() == txtcaptcha.Text.Trim())
            //{

            //}
            //else
            //{
            //    Response.Write("<script>alert('Please provide Proper Captcha');</script>");
            //    return;
            //}
            string blusername = SecurityValidation.chkData("T", txtUsername.Text + "" + txtPassword.Text);
            if (blusername.Length > 0)
            {
                Response.Write("<script>alert('Incorrect values entered');</script>");
                return;
            }
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            Hashtable credentials = new Hashtable();
            credentials["username"] = txtUsername.Text;
            credentials["password"] = txtPassword.Text;
            credentials["IP"] = Ip;

            Cls_Business_Auth objAuth = new Cls_Business_Auth();
            Hashtable authResponse = objAuth.GetAuthResponse(credentials);
            if (authResponse["ex_ocuured"] == null)
            {
                int responseFlag = Convert.ToInt32(authResponse["response_code"]);
                string responseMsg = authResponse["response_msg"].ToString();
                if (responseFlag == 9999)
                {


                    Session["user_id"] = authResponse["user_id"];
                    Session["user_brmpoid"] = authResponse["user_brmpoid"];
                    Session["username"] = txtUsername.Text;
                    Session["operator_id"] = authResponse["operator_id"];
                    Session["category"] = authResponse["user_operator_category"];
                    Session["name"] = authResponse["user_name"];
                    Session["last_login"] = authResponse["last_login"];
                    Session["login_flag"] = authResponse["login_flag"];
                    Session["showimage"] = "Y";
                    Session["MIAflag"] = authResponse["MIAflag"];
                    string oddeven = "";
                    try
                    {
                        oddeven = CreateAlphaNeumericTransferCode();
                        Session["RPAuthToken"] = Convert.ToString(oddeven);
                        // now create a new cookie with this guid value  
                        Response.Cookies.Add(new HttpCookie("AuthToken", Convert.ToString(oddeven)));
                    }
                    catch { }

                    //var url = "http://localhost:16218/Transaction/Home.aspx";


                    try
                    {
                        Hashtable sessionlog = new Hashtable();
                        sessionlog["username"] = Session["username"];
                        sessionlog["sessionid"] = Session.SessionID;
                        sessionlog["pagename"] = "Login.aspx";
                        sessionlog["name"] = Session["name"];
                        sessionlog["IP"] = Ip;

                        string resssion = objAuth.SessionLog(sessionlog);
                    }
                    catch { }


                    String GSTNO = "";
                    objAuth.GetGSTNo(Session["operator_id"].ToString(), out GSTNO);

                    if (GSTNO != "")
                    {
                        lblLoginResult.Text = "User login successful";
                        //if (authResponse["user_operator_category"].ToString() == "3")
                        //{
                        //    //if Logged in user is LCO
                        //    Response.Redirect("~/Reports/rptLCOAllDetails.aspx");
                        //}
                        //else
                        //{
                        if (authResponse["user_operator_category"].ToString() == "3" || authResponse["user_operator_category"].ToString() == "11")
                        {
                            NameValueCollection collections = new NameValueCollection();
                            collections.Add("username", txtUsername.Text.Trim());
                            collections.Add("Name", authResponse["user_name"].ToString());
                            //string remoteUrl = "https://hathwayconnectuat.com/Transaction/Home.aspx"; //-- UAT
                            //string remoteUrl = "http://localhost:2388/Transaction/Home.aspx";//--- LOCAL
                            string remoteUrl = "http://local.hathway.com/Transaction/Home.aspx";//--- LOCAL
                            string html = "<html><head>";
                            html += "</head><body onload='document.forms[0].submit()'>";
                            html += string.Format("<form name='PostForm' method='POST' action='{0}'>", remoteUrl);
                            foreach (string key in collections.Keys)
                            {
                                html += string.Format("<input name='{0}' type='hidden' value='{1}'>", key, collections[key]);
                            }
                            html += "</form></body></html>";
                            Response.Clear();
                            Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");
                            Response.HeaderEncoding = Encoding.GetEncoding("ISO-8859-1");
                            Response.Charset = "ISO-8859-1";
                            Response.Write(html);
                            Response.End();
                            Response.Redirect("~/Transaction/Home.aspx", true);//for lco


                        }
                        else
                        {
                            string script = "<script language=\"javascript\" type=\"text/javascript\">alert('You Are Not Authorised to Login in this Portal.');</script>";
                            Response.Write(script);
                        }
                    }
                    else
                    {
                        String AccepLCOCOde = "";
                        objAuth.GetAccepGST(txtUsername.Text, out AccepLCOCOde);
                        if (AccepLCOCOde == "")
                        {
                            DateTime DT = DateTime.Today;
                            string CURDATE = DT.ToString("dd-MMM-yyyy");
                            Cls_Data_Auth objTran = new Cls_Data_Auth();
                            DataTable htResponse = objTran.GetLCODetails(txtUsername.Text);

                            if (htResponse.Rows.Count > 0)
                            {
                                if (htResponse.Rows[0]["var_compconfig_localaddress"].ToString() != "")
                                {
                                    userGST._lblCompanyAddress = htResponse.Rows[0]["var_compconfig_localaddress"].ToString();
                                }
                                if (htResponse.Rows[0]["var_lcomst_company"].ToString() != "")
                                {
                                    userGST._lblCompanyName = htResponse.Rows[0]["var_lcomst_company"].ToString();
                                }
                                if (htResponse.Rows[0]["var_lcomst_code"].ToString() != "")
                                {
                                    userGST._lblLCOCode = htResponse.Rows[0]["var_lcomst_code"].ToString();
                                }
                                if (htResponse.Rows[0]["var_lcomst_name"].ToString() != "")
                                {
                                    userGST._lblLCOName = htResponse.Rows[0]["var_lcomst_name"].ToString();
                                }
                                if (htResponse.Rows[0]["var_lcomst_name"].ToString() != "")
                                {
                                    userGST._lblLCONameHead = htResponse.Rows[0]["var_lcomst_name"].ToString();
                                }

                                userGST._lblSYSDATE = CURDATE;
                                userGST._lblSYSDATETIME = DT.ToString();

                            }
                            popMsg.Show();
                            //else
                            //{ 
                            //}

                        }
                        else
                        {
                            if (authResponse["user_operator_category"].ToString() == "3" || authResponse["user_operator_category"].ToString() == "11")
                            {
                                NameValueCollection collections = new NameValueCollection();
                                collections.Add("username", txtUsername.Text.Trim());
                                collections.Add("Name", authResponse["user_name"].ToString());
                                string remoteUrl = "http://local.hathway.com/Transaction/Home.aspx";//--- LOCAL
                                //string remoteUrl = "http://localhost:2388/Transaction/Home.aspx"; //-- LOCAL
                               //string remoteUrl = "https://hathwayconnectuat.com/Transaction/Home.aspx";// UAT
                                string html = "<html><head>";
                                html += "</head><body onload='document.forms[0].submit()'>";
                                html += string.Format("<form name='PostForm' method='POST' action='{0}'>", remoteUrl);
                                foreach (string key in collections.Keys)
                                {
                                    html += string.Format("<input name='{0}' type='hidden' value='{1}'>", key, collections[key]);
                                }
                                html += "</form></body></html>";
                                Response.Clear();
                                Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");
                                Response.HeaderEncoding = Encoding.GetEncoding("ISO-8859-1");
                                Response.Charset = "ISO-8859-1";
                                Response.Write(html);
                                Response.End();
                                Response.Redirect("Transaction/frmAssignPlan.aspx");//"~/Transaction/Home.aspx", true);//for lco
                            }
                            else
                            {
                                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('You Are Not Authorised to Login in this Portal.');</script>";
                                Response.Write(script);
                            }
                        }

                    }
                }
                else
                {
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtUsername.Focus();
                    string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + responseMsg + "');</script>";
                    Response.Write(script);
                }
            }
            else
            {
                Response.Redirect("~/ErrorPage.aspx");
            }
        }
        //----Added by RP on 22.07.2019
        private string CreateAlphaNeumericTransferCode()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";
            string SpecialCharater = "_-+=~^@#$+";
            string characters = numbers;

            characters += alphabets + small_alphabets + numbers + SpecialCharater;

            int length = 15;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }

            return otp;

        }

        //----Added by RP on 28.06.2017 
        protected void btnReject_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://gst.hathway.net:2462/prodgst/review");
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            try
            {
                Cls_Business_Auth objAuth = new Cls_Business_Auth();
                string result = objAuth.InsertLCAccept(txtUsername.Text, Ip);
                string[] Getresponse = result.Split('$');
                if (Getresponse[0] == "9999")
                {
                    Response.Redirect("~/Transaction/Home.aspx", true);
                    lblmsg.Text = "";
                }
                else
                {
                    // msgboxstr(Getresponse[1].ToString());
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = "Error Occured.Try Again !";
                //throw;
            }
        }
        //-----
        public void msgbox(string message, Control ctrl)
        {
            string msg = "<script type=\"text/javascript\">alert(\"" + message + "\");</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", msg);
            ctrl.Focus();
        }

        protected void lnkforgot_Click1(object sender, EventArgs e)
        {
            lblmsg.Text = "";
            txtname.Text = "";
            ModalPopupExtender1.Show();
        }
        protected void btnreset_Click(object sender, EventArgs e)
        {
            btnreset.Enabled = false;
            if (txtname.Text.Trim() == "")
            {
                msgbox("Please Enter UserName", txtname);
                return;
            }
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);

            Hashtable ht = new Hashtable();
            ht.Add("UserName", txtname.Text.Trim());
            ht.Add("IP", Ip);
            Cls_Bussiness_forgotpass objbuss = new Cls_Bussiness_forgotpass();
            string response = objbuss.ForgotDetails(ht);
            if (response == "ex_occured")
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            lblmsg.Visible = true;
            // lblmsg.Text = response;
            Session["txt"] = response;
            lblmsg.Text = Session["txt"].ToString();
            btnreset.Enabled = true;
            // txtname.Text = "";
            ModalPopupExtender1.Show();
        }


        protected void btnRefreshForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");

        }


    }
}