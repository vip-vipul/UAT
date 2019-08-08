using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassDAL.Helper;
using System.IO;
using System.Net;
using System.Text;
using PrjUpassBLL.Transaction;
using System.Data;
using System.Collections;

namespace PrjUpassPl.Transaction
{
    public partial class frmInvAmountMove : System.Web.UI.Page
    {
        Cls_Helper objHelper1 = new Cls_Helper();
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        string operid;
        string username;
        string catid;
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["RightsKey"] = "N";
            Master.PageHeading = "Moving Amount Details";
            if (!IsPostBack)
            {
                if (Session["operator_id"] != null)
                {

                    operid = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    catid = Convert.ToString(Session["category"]);
                    divdet.Visible = false;
                    btnSearch_Click(null, null);
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
            }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string blusername = SecurityValidation.chkData("N", txtSearch.Text+""+txtMobileNo.Text+""+txtLCOBanalce.Text);
            if (blusername.Length > 0)
            {
                msgboxstr(blusername);
                return;
            }

            blusername = SecurityValidation.chkData("T", txtRemark.Text);
            if (blusername.Length > 0)
            {
                msgboxstr(blusername);
                return;
            }

            lblmsg11.Text = "";
            divdet.Visible = false;
            btnSubmit.Visible = true;
            txtSearch.Text = Session["username"].ToString();
            if (txtSearch.Text.Trim() != "")
            {
                Cls_BLL_NEWSTB obj = new Cls_BLL_NEWSTB();
                string operator_id = "";
                operator_id = Convert.ToString(Session["operator_id"]);

                string[] responseStr = obj.getLcodetails(Session["username"].ToString(), Session["username"].ToString(), "0", operator_id, "3");
                if (responseStr.Length != 0)
                {
                    lblCustNo.Text = responseStr[0].Trim();
                    Session["lcoCodeR"] = lblCustNo.Text.Trim();

                    lblCustName.Text = responseStr[1].Trim();
                    lblCustAddr.Text = responseStr[2].Trim();
                    lblmobno.Text = responseStr[3].Trim();
                    txtMobileNo.Text = responseStr[3].Trim();
                    lblEmail.Text = responseStr[4].Trim();
                    lblInvBalance.Text = responseStr[5].Trim();
                    txtLCOBanalce.Text = responseStr[6].Trim();
                    divdet.Visible = true;
                    LCOAccordion.Visible = true;
                    LCOAccordion.SelectedIndex = 0;
                    Label32.Text = "LCO Details";
                    Label4.Text = "LCO Details";
                    Label1.Text = "LCO Code";

                }
                else
                {
                    msgboxstr("No Such LCO Found");
                    LCOAccordion.Visible = false;
                    return;
                }

            }
            else
            {

                msgboxstr("Please Enter Code ");
                LCOAccordion.Visible = false;
                return;
            }
        }

        public String callAPI(string Request, string request_code)
        {
            try
            {
                string fromSender = string.Empty;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(Request);
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/testhwayobrmcallservice/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRM/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.20//TestHwayOBRM/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.21/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);

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

                return "1$---$" + ex.Message.Trim();
            }
        }

        public void msgboxstr(string message)
        {
            lblPopupResponse.Text = message;
            popMsg.Show();
        }
        public void msgboxstr1(string message)
        {
            lblmsgsuc1.Text = message;
            popMsg1.Show();
        }
        protected void ResetForm()
        {
            txtRemark.Text = "";
            divdet.Visible = true;
            txtAmount.Text = "";
            txtLCOBanalce.Text = "";
            txtMobileNo.Text = "";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string blusername = SecurityValidation.chkData("N", txtSearch.Text + "" + txtMobileNo.Text);
            if (blusername.Length > 0)
            {
                msgboxstr(blusername);
                return;
            }

            blusername = SecurityValidation.chkData("T", txtRemark.Text);
            if (blusername.Length > 0)
            {
                msgboxstr(blusername);
                return;
            }

            lblPostInvBalance.Text = Convert.ToString(Convert.ToDouble(lblInvBalance.Text) - Convert.ToDouble(txtAmount.Text));
            lblLCOBalance.Text = txtLCOBanalce.Text;
            lblInventoryBalance.Text = lblInvBalance.Text;
            lblMovingAmount.Text = txtAmount.Text;
            lblRemark.Text = txtRemark.Text;
            if (ValidatePage())
            {
                popupModifyConfirm.Show();
            }
        }

        public void closeMsgPopupnew1()
        {
            popMsg1.Hide();
            btnSearch_Click(null,null);
        }
        private bool ValidatePage()
        {
            bool result = true;

            if (Convert.ToDouble(lblInvBalance.Text) < Convert.ToDouble(txtAmount.Text))
            {
                msgboxstr("Inventory Amount Less Moving Amount Please Check it.");
                return false;
            }
            if (txtAmount.Text == "")
            {
                msgboxstr("Please Enter Moving Amount.");
                return false;
            }
            if (txtMobileNo.Text.Trim() == "")
            {
                msgboxstr("Mobile no can not be blank");
                return false;
            }
            if (txtMobileNo.Text.Length != 10)
            {
                msgboxstr(" Please Enter Mobile No should be 10 digits");
                return false;
            }
            if (txtMobileNo.Text != "")
            {
                string strMobileNo = txtMobileNo.Text.Substring(0, 1);
                if (!(strMobileNo == "9" || strMobileNo == "8" || strMobileNo == "7"))
                {
                    msgboxstr(" Please Enter valid Mobile No");
                    return false;
                }
            }
            if (txtRemark.Text.Trim() == string.Empty)
            {
                msgboxstr("Enter Remark ");
                return false;
            }
            return result;
        }
        protected void btnModifyConfirm_click(object sender, EventArgs e)
        {

            Cls_business_InvAmountMove obj = new Cls_business_InvAmountMove();
            if (ValidatePage())
            {
                Hashtable ht = new Hashtable();
                ht.Add("LCOcode", lblCustNo.Text);
                ht.Add("LCOBalance", txtLCOBanalce.Text);
                ht.Add("InvBalance", lblInvBalance.Text);
                ht.Add("Amount", txtAmount.Text);
                ht.Add("Remark", txtRemark.Text.Trim());
                ht.Add("MobileNo", txtMobileNo.Text.Trim());

                try
                {

                    string result = obj.UpdateAmount("aoup_lcopre_inv_transbal", ht);

                    string[] Getresponse = result.Split('$');
                    if (Getresponse[0].Contains("9999"))
                    {
                        divdet.Visible = false;
                        ResetForm();
                        msgboxstr1("Amount Moved Successfully");
                    }
                    else
                    {
                        msgboxstr1(Getresponse[1].ToString());

                    }
                }
                catch (Exception ex)
                {
                    msgboxstr("Error Occured.Try Again !");

                }

            }

        }

    }
}