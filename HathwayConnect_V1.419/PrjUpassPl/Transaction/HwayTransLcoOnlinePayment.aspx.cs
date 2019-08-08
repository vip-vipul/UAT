using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Transaction;
using System.Data.OracleClient;
using System.Configuration;
using PrjUpassDAL.Authentication;
using System.Collections;


namespace PrjUpassPl.Transaction
{
    public partial class HwayTransLcoOnlinePayment : System.Web.UI.Page
    {
        string username = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.PageHeading = "Online Payment";
                Session["pagenos"] = "1";
                if (Session["operator_id"] != null)
                {
                    Session["RightsKey"] = null;

                    username = Convert.ToString(Session["username"]);
                    FillLcoDetails();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }

                /*ViewState["Allowcitrus"] = getcitrusdetail();


                if (ViewState["Allowcitrus"].ToString() == "Y")
                {
                    Tr2.Visible = true;
                }
                else
                {
                    Tr2.Visible = false;
                }*/
            }
        }

        protected String getcitrusdetail()
        {
            string Citrusflag = "N";
            lblmsg.Text = "";
            string str = "";
            Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();

            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {

                str = " select b.var_comp_citrux_flag from aoup_lcopre_lco_det a,aoup_lcopre_company_det b,aoup_lcopre_city_def c  ";
                str += "  where a.var_lcomst_company=b.var_comp_company  and a.num_lcomst_cityid=c.num_city_id and b.var_comp_city=c.var_city_name    ";
                if (Convert.ToString(Session["category"]) == "11")
                {
                    str += "  and a.var_lcomst_code ='" + ddllco.SelectedValue + "'";
                }
                else if (Convert.ToString(Session["category"]) == "3")
                {
                    str += "  and a.var_lcomst_code ='" + Session["username"].ToString() + "'";
                }

                else
                {

                    Citrusflag = "N";
                }
                DataTable tbllco = GetResult(str);

                if (tbllco.Rows.Count > 0)
                {
                    Citrusflag = tbllco.Rows[0]["var_comp_citrux_flag"].ToString();
                }
                else
                {
                    Citrusflag = "N";
                }

            }
            catch (Exception ex)
            {
                Citrusflag = "N";
                Response.Write("Error while online payment : " + ex.Message.ToString());
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }
            return Citrusflag;
        }

        protected void FillLcoDetails()
        {

            lblmsg.Text = "";
            string str = "";
            Paydet.Visible = false;
            btnSubmit.Visible = false;
            divdet.Visible = false;
            pnllco.Visible = false;
            Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
            string operator_id = "";
            string category_id = "";
            if (Session["operator_id"] != null && Session["category"] != null)
            {
                operator_id = Convert.ToString(Session["operator_id"]);
                category_id = Convert.ToString(Session["category"]);
            }
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            try
            {

                str = "   SELECT '('||var_lcomst_code||')'||a.var_lcomst_name name,var_lcomst_code lcocode ";
                str += "     FROM aoup_lcopre_lco_det a ,aoup_operator_def c,aoup_user_def u ";
                str += "  WHERE a.num_lcomst_operid = c.num_oper_id and  a.num_lcomst_operid=u.num_user_operid and u.var_user_username=a.var_lcomst_code  ";
                if (category_id == "11")
                {
                    str += "  and c.num_oper_clust_id =" + operator_id;
                }
                else if (category_id == "3")
                {
                    str += "and a.num_lcomst_operid =  " + operator_id + " ";
                }
                else
                {

                    lblmsg.Text = "No LCO Details Found";
                    Paydet.Visible = false;
                    btnSubmit.Visible = false;
                    divdet.Visible = false;
                    pnllco.Visible = false;
                    return;
                }
                DataTable tbllco = GetResult(str);

                if (tbllco.Rows.Count > 0)
                {
                    pnllco.Visible = true;
                    ddllco.DataTextField = "name";
                    ddllco.DataValueField = "lcocode";

                    ddllco.DataSource = tbllco;
                    ddllco.DataBind();
                    if (category_id == "11")
                    {
                        ddllco.Items.Insert(0, new ListItem("Select LCO", "0"));
                    }
                    else if (category_id == "3")
                    {
                        ddllco_SelectedIndexChanged(null, null);
                    }
                }
                else
                {
                    lblmsg.Text = "No LCO Details Found";
                    divdet.Visible = false;
                    Paydet.Visible = false;
                    btnSubmit.Visible = false;
                    pnllco.Visible = false;
                }

            }
            catch (Exception ex)
            {
                Response.Write("Error while online payment : " + ex.Message.ToString());
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
            }

        }

        protected void ddllco_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblmsg.Text = "";
            string str = "";
            Session["lco_username"] = ddllco.SelectedValue;
            Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
            if (ddllco.SelectedValue != "0")
            {
                str = "  SELECT a.lcoid, a.distid, a.msoid, a.hoid, a.lcomstcode, a.lcomstname, ";
                str += "  a.lcomstaddress, a.lcomstmobileno, a.lcomstemail, ";
                str += "  a.currentcreditlimit ";
                str += "  FROM veiw_lcopre_paylco_search a ";
                str += " where a.lcomstcode='" + ddllco.SelectedValue + "'";


                OracleCommand cmd = new OracleCommand(str, conObj);

                conObj.Open();

                List<string> Operators = new List<string>();
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        lblCustNo.Text = dr["LCOMSTCODE"].ToString();
                        ViewState["lcocode"] = dr["LCOMSTCODE"].ToString();
                        lblCustName.Text = dr["LCOMSTNAME"].ToString();
                        lblCustAddr.Text = dr["LCOMSTEMAIL"].ToString();
                        lblmobno.Text = dr["LCOMSTMOBILENO"].ToString();
                        lblEmail.Text = dr["LCOMSTEMAIL"].ToString();
                        lblCurrBalance.Text = dr["CURRENTCREDITLIMIT"].ToString();



                    }
                    Paydet.Visible = true;
                    btnSubmit.Visible = true;
                    divdet.Visible = true;
                }
                else
                {
                    lblmsg.Text = "No LCO Details Found";
                    divdet.Visible = false;
                    Paydet.Visible = false;
                    btnSubmit.Visible = false;
                }
            }
            else
            {
                lblmsg.Text = "Please select LCO";
                divdet.Visible = false;
                Paydet.Visible = false;
                btnSubmit.Visible = false;
                lblCustNo.Text = "";
                ViewState["lcocode"] = null;
                lblCustName.Text = "";
                lblCustAddr.Text = "";
                lblmobno.Text = "";
                lblEmail.Text = "";
                lblCurrBalance.Text = "";
            }
        }

        public DataTable GetResult(String Query)
        {
            DataTable MstTbl = new DataTable();


            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            con.Open();

            OracleCommand Cmd = new OracleCommand(Query, con);
            OracleDataAdapter AdpData = new OracleDataAdapter();
            AdpData.SelectCommand = Cmd;
            AdpData.Fill(MstTbl);

            con.Close();

            return MstTbl;
        }


        public void msgbox(string message, Control ctrl)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + message + "');", true);
            ctrl.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //if (ViewState["Allowcitrus"].ToString() == "Y")
            //{
            //    if (rdoBill.Checked == false && rdcitrus.Checked == false)
            //    {
            //        msgbox("Please Select Gateway", lblCustNo);
            //        return;
            //    }
            //}
            string blusername = SecurityValidation.chkData("T", txtRemark.Text);
            if (blusername.Length > 0)
            {
                lblmsg.Text = blusername;
                return;
            }

            blusername = SecurityValidation.chkData("N", txtCashAmt.Text);
            if (blusername.Length > 0)
            {
                lblmsg.Text = blusername;
                return;
            }

            lblmsg.Text = "";
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            if (lblCustNo.Text.Trim() == "")
            {
                msgbox("Please Select LCO", lblCustNo);
                return;
            }
            else if (txtCashAmt.Text.Trim() == "")
            {
                msgbox("Please Enter Amount", txtCashAmt);
                return;
            }

            try
            {
                if (Convert.ToInt64(txtCashAmt.Text.Trim()) > Convert.ToInt64(HdnMaxamount.Value))
                {
                    msgbox("Please Enter Amount less than or Equal to " + HdnMaxamount.Value, txtCashAmt);
                    return;
                }
            }
            catch
            {
            }


            btnSubmit.Visible = false;

            Hashtable ht = new Hashtable();
            string loggedInUser;
            if (Session["username"] != null)
            {
                if (Convert.ToString(Session["category"]) == "3")
                {
                    loggedInUser = Session["username"].ToString();
                }
                else
                {
                    loggedInUser = ddllco.SelectedValue; //Session["username"].ToString();
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            String PaymentFlag = "BD";
            /*if (ViewState["Allowcitrus"].ToString() == "Y")
            {
                Tr2.Visible = true;
                if (rdoBill.Checked == true)
                {
                    PaymentFlag="BD";
                }
                else
                {
                    PaymentFlag="CT";
                }

            }
            else
            {
                Tr2.Visible = false;
                PaymentFlag = "BD";
            }*/
            if (rdoBill.Checked == true)
            {
                PaymentFlag = "BD";
            }
            else
            {
                PaymentFlag = "PU";
            }


            ht.Add("User", loggedInUser);
            ht.Add("CustCode", loggedInUser);
            ht.Add("Amount", Convert.ToInt32(txtCashAmt.Text.Trim()));
            ht.Add("PayMode", "O");
            ht.Add("chequeddno", "N/A");
            ht.Add("CheckDate", DateTime.MinValue);

            ht.Add("BankName", "*");
            ht.Add("Branch", "*");
            ht.Add("Remark", txtRemark.Text.Trim());
            ht.Add("ReceiptNo", "*");
            ht.Add("IP", Ip);
            ht.Add("user_id", Session["user_id"]);
            ht.Add("user_brmpoid", Session["user_brmpoid"]);
            ht.Add("operator_id", Session["operator_id"]);
            ht.Add("category", Session["category"]);

            ht.Add("name", Session["name"]);
            ht.Add("last_login", Session["last_login"]);
            ht.Add("login_flag", Session["login_flag"]);
            string sessionId = "(S(" + Session.SessionID.ToString() + "))";
            ht.Add("Session", sessionId);
            ht.Add("Identifier", PaymentFlag);
            ht.Add("CrLimittype", "HWV");
            Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
            // string response = obj.LcoPayment(ht);
            //reset();

            string response = "";
            Hashtable transreceipt = obj.LcoOnlinePaymentTransID(ht);
            int TransID = Convert.ToInt32(transreceipt["Transid"]);

            //int TransID = obj.LcoOnlinePaymentTransID(ht);
            if (TransID != 0)
            {
                PrjUpassDAL.Helper.Cls_Validation bojValidation = new PrjUpassDAL.Helper.Cls_Validation();
                string strTransCode = bojValidation.lcoTransCode(loggedInUser);
                if (strTransCode.Length > 0)
                {
                    int TranAmt = Convert.ToInt32(txtCashAmt.Text.Trim());
                    string RetrnUrl = "http://124.153.73.21/HwayLCOSMSUAT/Transaction/TransHwayOnlinePayResponse.aspx";
                    //string RetrnUrl = "http://localhost:2388/Transaction/TransHwayOnlinePayResponse.aspx";
                    if (PaymentFlag == "BD")
                    {
                        Response.Redirect("http://124.153.73.21/UbillDesk/Request.aspx?CompanyCode=10001&TransNo=" + TransID + "&Amount=" + TranAmt + "&ReturnURL=" + RetrnUrl + "&AdditionalInfo2=" + strTransCode + "&AdditionalInfo3=" + loggedInUser + "&AdditionalInfo4=" + Convert.ToString(transreceipt["receipt"]));
                    }
                    else
                    {
                        Response.Redirect("http://124.153.73.21/UbillDesk/Request.aspx?CompanyCode=30002&TransNo=" + TransID + "&Amount=" + TranAmt + "&ReturnURL=" + RetrnUrl + "&AdditionalInfo2=" + strTransCode + "&AdditionalInfo3=" + loggedInUser + "&AdditionalInfo4=" + Convert.ToString(transreceipt["receipt"]));
                    }
                }
            }
        }
    }
}