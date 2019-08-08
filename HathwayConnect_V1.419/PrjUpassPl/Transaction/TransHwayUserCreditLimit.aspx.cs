using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PrjUpassBLL.Transaction;
using PrjUpassDAL.Transaction;
using System.Data.OracleClient;
using System.Configuration;
using PrjUpassDAL.Authentication;


namespace PrjUpassPl.Transaction
{
    public partial class TransHwayUserCreditLimit : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        static string operid;
        static string username;
        static string catid;
        static string type = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["operator_id"] != null)
                {
                    Session["RightsKey"] = null;
                    reset();
                    operid = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    catid = Convert.ToString(Session["category"]);
                    if (RadSearchby.SelectedValue.ToString() == "0")
                    {
                        type = "0";
                    }
                    else
                    {
                        type = "1";
                    }
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
            }
       //     btnSubmit.Attributes.Add("onclick", "javascript:return dovalid1()");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchOperators(string prefixText, int count)
        {
            string Str = prefixText.Trim();
            double Num;
            bool isNum = double.TryParse(Str, out Num);

            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";

            str += " SELECT a.userid, a.username, a.userowner, a.curentcreditlimit, ";
            str += "  a.mobileno, a.useroperid, a.operid, a.opercategory, a.parentid, ";
            str += " a.distid ";
            str += " FROM view_hway_userdetails a ";
            if (type == "0")
            {
                str += " where upper(a.username) like upper('" + prefixText.ToString() + "%')";
            }
            else if (type == "1")
            {
                str += " where upper(a.userowner) like  upper('" + prefixText.ToString() + "%')";
            }
            if (catid == "2")
            {
                str += " and a.parentid='" + operid.ToString() + "'  ";
            }
            else if (catid == "5")
            {
                str += " and a.distid='" + operid.ToString() + "'  ";
            }
            else if (catid == "3")
            {
                str += " and a.operid ='" + operid.ToString() + "'";
            }
            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (type == "0")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["username"].ToString(), dr["username"].ToString());
                    Operators.Add(item);
                }
                else if (type == "1")
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                            dr["userowner"].ToString(), dr["userowner"].ToString());
                    Operators.Add(item);
                }
            }
            //string[] prefixTextArray = Operators.ToArray<string>();
            con.Close();
            return Operators;

        }


      

        protected void btnSearch_Click(object sender, EventArgs e)
        {                        
            lblmobno.Text = "";
            lblUserId.Text = "";
            lblUserName.Text = "";
            lblCurLimit.Text = "";
            txtNewLimit.Text = "";
            lblmsg.Text = "";
                        
            Cls_Bussiness_TransHwayUserCreditLimit obj = new Cls_Bussiness_TransHwayUserCreditLimit();
            string[] str = obj.GetUserDetails(username, catid, RadSearchby.SelectedValue.ToString(), operid, txtLCOSearch.Text.ToString());
            try
            {
                if (str.Length != 0)
                {
                    lblUserId.Text = str[0].Trim();
                    lblUserName.Text = str[1].Trim();
                    lblmobno.Text = str[2].Trim();
                    lblCurLimit.Text = str[3].Trim();
                    divdet.Visible = true;
                }
                else
                {
                    divdet.Visible = false;
                    msgbox("No Such User Found", txtLCOSearch);
                    return;

                }


            }
            catch (Exception ex)
            {
                msgbox("Please Select User by code or name", txtLCOSearch);
                return;
            }
            finally
            {

            }
        }

        public void msgbox(string message, Control ctrl)
        {
            string msg = "<script type=\"text/javascript\">alert(\"" + message + "\");</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", msg);
            ctrl.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            if (txtNewLimit.Text.Trim() == "")
            {
                msgbox("Please Enter Amount", txtNewLimit);
                return;
            }
           
            Hashtable ht = new Hashtable();
            //string loggedInUser;
            //if (Session["username"] != null)
            //{
            //    loggedInUser = Session["username"].ToString();
            //}
            //else
            //{
            //    Session.Abandon();
            //    Response.Redirect("~/Login.aspx");
            //    return;
            //}
            ht.Add("UserName", username);
            ht.Add("LcoId", operid);
            ht.Add("UserId", lblUserId.Text);
            ht.Add("Amount", Convert.ToInt32(txtNewLimit.Text.Trim()));
            ht.Add("Flag", "A");
            ht.Add("Remark", "");
            ht.Add("IP", Ip);
            Cls_Bussiness_TransHwayUserCreditLimit obj = new Cls_Bussiness_TransHwayUserCreditLimit();
            string response = obj.UserLimitRev(ht);
            reset();
            if (response == "ex_occured")
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            lblmsg.Text = response;
        }

        protected void reset()
        {
            txtNewLimit.Text = "";
            lblmobno.Text = "";
            lblCurLimit.Text = "";
            lblmsg.Text = "";
            lblUserId.Text = "";
            lblUserName.Text = "";
            divdet.Visible = false;
            txtLCOSearch.Text = "";
        }

        protected void RadSearchby_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadSearchby.SelectedValue.ToString() == "0")
            {
                type = "0";
            }
            else
            {
                type = "1";
            }
        }


         

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        } 
    }
}