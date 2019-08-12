using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Transaction;
using PrjUpassDAL.Authentication;
using PrjUpassBLL.Master;
using System.Data;
using System.Collections;

namespace PrjUpassPl.Transaction
{
    public partial class Home : System.Web.UI.Page
    {
        string username = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            incrementDate();
            String sid = Session.SessionID;
            Master.PageHeading = "Home";
            if (Session["username"] != null && Session["RPAuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["RPAuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                    return;
                }
                else
                {
                    username = Convert.ToString(Session["username"]);
                    Session["RightsKey"] = "Y";
                    hdnlco.Value = username;
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                Session["pagenos"] = "1";

                cls_BLL_messenger obj = new cls_BLL_messenger();
                String City = "";
                String UserLevel = "";
                String state = "";
                string cityuser = "";
                obj.GetCity(Session["username"].ToString(), out City, out state, out UserLevel, out  cityuser);
                ViewState["cityid"] = City;
                ViewState["UserLevel"] = UserLevel;
                ViewState["state"] = state;
                ViewState["cityuser"] = cityuser;

                bindInboxMessages();
            }
        }

        protected void imgAndroid_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Cls_Data_Auth obj = new Cls_Data_Auth();
                string strContact = obj.LCOContactNo(Convert.ToString(Session["username"]));
                txtLCOExist.Text = strContact;
                popMsgBox.Show();
                ckOld.Checked = true;
                txtLCOExist.Visible = true;
                ckNew.Checked=false;
                txtNewContactNo.Visible = false;
            }
            catch (Exception ex)
            {
                
            }
        }
        protected void ChckedChanged(object sender, EventArgs e)
        {
            if (ckNew.Checked == true)
            {
                txtLCOExist.ForeColor=System.Drawing.Color.Gray;
                ckOld.Checked = false;
                txtNewContactNo.Visible = true;
                popMsgBox.Show();
            }
            else if (ckOld.Checked == true)
            {
                txtLCOExist.Visible = true;
                ckNew.Checked = true;
                txtNewContactNo.Visible = false;
                popMsgBox.Show();
            }
            if (ckOld.Checked != true && ckNew.Checked != true)
            {
                txtLCOExist.Visible = true;
                ckOld.Checked = true;
                txtNewContactNo.Visible = false;
                popMsgBox.Show();
            }
            
        }
        protected void ChckedChanged1(object sender, EventArgs e)
        {
            if (ckOld.Checked == true)
            {
                txtLCOExist.Visible = true;
                ckNew.Checked = false;
                txtNewContactNo.Visible = false;
                popMsgBox.Show();
            }
            else if (ckNew.Checked == true)
            {
                //txtLCOExist.Visible = false;
                ckOld.Checked = false;
                txtNewContactNo.Visible = true;
                popMsgBox.Show();
            }
            if (ckOld.Checked != true && ckNew.Checked!=true)
            {
                txtLCOExist.Visible = true;
                ckOld.Checked = true;
                txtNewContactNo.Visible = false;
                popMsgBox.Show();
            }
            //else
            //{
            //    popMsgBox.Show();
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", " alert('Please Select Any One Mobile No');", true);
            //    // return;
            //}
        }
        protected void btncnfmBlck_Click(object sender, EventArgs e)
        {
            try
            {
                if (ckOld.Checked == false && ckNew.Checked == false)
                {
                    
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", " alert('Please Select Any One Mobile No');", true);
                    //popMsgBox.Show();
                    return;
                }
                else
                {
                    Cls_Data_Auth obj = new Cls_Data_Auth();
                    string strNewNO = "";
                    if (ckNew.Checked == true)
                    {
                        if (txtNewContactNo.Text != "")
                        {
                            if (txtNewContactNo.Text.Length != 10)
                            {
                                
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", " alert('Please Enter Mobile No should be 10 digits');", true);
                                popMsgBox.Show();
                                return;
                            }
                            string strMobileNo = txtNewContactNo.Text.Substring(0, 1);
                            if (!(strMobileNo == "9" || strMobileNo == "8" || strMobileNo == "7" || strMobileNo == "6"))
                            {
                                
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Please Enter valid Mobile No');", true);
                                popMsgBox.Show();
                                return;
                            }
                        }
                        else
                        {
                            
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", " alert('Please Enter Other Mobile No');", true);
                            popMsgBox.Show();
                            return;
                        }
                        strNewNO = txtNewContactNo.Text;
                    }
                    else
                    {
                        strNewNO = "";
                    }

                    string strresponse = obj.SendAndroidUrl(Convert.ToString(Session["username"]), strNewNO);
                    string[] spliresponse = strresponse.Split('$');
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert(\"" + spliresponse[1] + "\");", true); 
                }
                
            }
            catch (Exception)
            {
                
            }
        }

        public void bindInboxMessages()
        {

            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            cls_BLL_messenger ob = new cls_BLL_messenger();

            DataTable dt = new DataTable();

            dt = ob.fillinbox(username, ViewState["cityid"].ToString().TrimStart(',').TrimEnd(','), ViewState["UserLevel"].ToString(), ViewState["state"].ToString(), "", "");

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;

            }
            int Countmsg = 0;
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    String Readby = Session["username"].ToString() + ",";

                    if (dr["readby"].ToString().Contains(Readby))
                    {

                    }
                    else
                    {
                        Countmsg++;
                    }
                }


            }
            else
            {
            }
            dt.Dispose();
            lblinboxcount.Text = Countmsg.ToString();
        }

        protected void BtnImgGlobalRenewal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataTable responseStr = new DataTable();
                if (username != "")
                {
                    string customerId = Request.Form[username];
                    string customerName = Request.Form[username];
                    Cls_BLL_TransHwayGlobalAutoRenew obj = new Cls_BLL_TransHwayGlobalAutoRenew();
                    string operator_id = "";
                    string category_id = "";
                    string status = "";
                    if (Session["operator_id"] != null && Session["category"] != null)
                    {
                        operator_id = Convert.ToString(Session["operator_id"]);
                        category_id = Convert.ToString(Session["category"]);
                    }
                    responseStr = obj.LcoPayment(username);
                    if (responseStr.Rows.Count != 0)
                    {
                        lblGlobalmsg.Text = responseStr.Rows[0]["var_lcomst_code"].ToString().Trim();
                        Session["lcoCodeR"] = lblGlobalmsg.Text.Trim();

                        lblGlobalmsg.Text = responseStr.Rows[0]["var_lcomst_name"].ToString().Trim();
                        if (responseStr.Rows[0]["var_lcoautorenew_flag"].ToString().Trim() == "N")
                        {
                            status = "0";
                            Session["status"] = status.ToString();
                            lblmsg.Text = "The Global Renewal is Disable..! Do You Want To Enable it? ";
                        }
                        else if (responseStr.Rows[0]["var_lcoautorenew_flag"].ToString().Trim() == "Y")
                        {
                            status = "1";
                            Session["status"] = status.ToString();
                            lblmsg.Text = "The Global Renewal is Enable..! Do You Want To Disable it?";
                        }
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('No Such LCO Found');", true);
                        popMsgBox.Show();
                        return;
                    }
                    btnclose.Visible = false;
                    btncancel.Visible = true;
                    btnsubmit.Visible = false;
                    btnconfirm.Visible = true;
                    btncancel.Value = "No";
                    popglobalmsgbox.Show();
                }
                else
                {
                    if (responseStr.Rows[0]["var_lcoautorenew_flag"].ToString().Trim() == "N")
                    {

                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnconfirm_Click(object sender, EventArgs e)
        {
            btnsubmit.Visible = true;
            btnconfirm.Visible = false;
            if (Session["status"].ToString() == "0")
            {
                lblmsg.Text = "This will Enable Auto Renewal for all your Customers. Are You Sure..! Do You Want To Continue.. ?";
            }
            else
            {
                lblmsg.Text = "This will Disable Auto Renewal for all your Customers. Are You Sure..! Do You Want To Continue.. ?";
            }
                popglobalmsgbox.Show();
        }

        protected void btnsubmimCon_Click(object sender, EventArgs e)
        {
            //if (btnsubmit.Text == "Yes")
            //{
            Session["lblmsg"] = null;
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);

            Hashtable ht = new Hashtable();
            string loggedInUser;
            if (Session["username"] != null)
            {
                loggedInUser = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            ht.Add("User", loggedInUser);
            ht.Add("lcocode", username);
            if (Session["status"] == "1")
            {
                ht.Add("flag", "N");
            }
            else if (Session["status"] == "0")
            {
                ht.Add("flag", "Y");
            }


            Cls_BLL_TransHwayGlobalAutoRenew obj = new Cls_BLL_TransHwayGlobalAutoRenew();
            string response = obj.statuschange(ht);

            if (response == "ex_occured")
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            string[] strrnw = new string[4];

            strrnw = response.Split(',');

            lblmsg.Text = strrnw[1];


            if (response.StartsWith("9999"))
            {
                lblmsg.Text = response.Split(',')[1];

            }
            else
            {
                lblmsg.Text = response;
            }
            btncancel.Visible = false;
            btnsubmit.Visible = false;
            btnclose.Visible = true;
            popglobalmsgbox.Show();
        }

        public void incrementDate()
        {
            var date = DateTime.UtcNow;
            var incDate = date.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss.SSSZ");
            var x = incDate;
            //DateTime.ToString("yyyy-MM-dd")       2019-08-29T14:51:13.672Z
        }
    }
}