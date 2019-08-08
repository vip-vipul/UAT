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
using System.Collections;
using PrjUpassBLL.Transaction;
using System.Configuration;
using System.Data.OracleClient;


namespace PrjUpassPl.Transaction
{
    public partial class frmNewSTBSP : System.Web.UI.Page
    {

        Cls_Helper objHelper1 = new Cls_Helper();
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        string operid;
        string username;
        string catid;
        double RateSTB = 0;
        double DicountSTB = 0;
        double NetSTB = 0;
        double RateLCO = 0;
        double DicountLCO = 0;
        double NetLCO = 0;
        double TotalNet = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["RightsKey"] = "N";
            Master.PageHeading = "SP New STB";

            if (!IsPostBack)
            {
                if (Session["operator_id"] != null)
                {

                    operid = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    catid = Convert.ToString(Session["category"]);
                    txtChqDate.Attributes.Add("readonly", "readonly");
                    txtChqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy").Trim();
                    //Cls_Helper.DropDownFill(ddlBank1, " aoup_bank_def where var_bank_compcode ='HWI' order by var_bank_name", "var_bank_name", "num_bank_id", "", "");
                    getcitystate();
                    FillSchemeDetails(ViewState["cityid"].ToString(), ViewState["stateid"].ToString());
                    divdet.Visible = false;
                    btnSearch_Click(null, null);
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
                FillSTBDetails();
            }
        }

        private void FillSTBDetails()
        {


        }

        private void FillSchemeDetails(string cityid, string stateid)
        {
            string query = "select var_lcomst_dasarea dasarea,var_lcomst_company company,var_city_name city from aoup_lcopre_lco_det ";
            query += "   inner join aoup_lcopre_city_def on num_lcomst_cityid=num_city_id";
            query += " where var_lcomst_code='" + txtSearch.Text + "'";

            DataTable dtquery = objHelper1.GetDataTable(query);
            if (dtquery.Rows.Count > 0)
            {

                string Condition = " var_scheme_type = 'SP' and (nvl(num_scheme_cityid," + cityid + ")=" + cityid +
                                   " ) and (nvl(num_scheme_stateid," + stateid + ")=" + stateid + " )";

                if (txtSearch.Text.Trim() != "")
                {
                    Condition += " and (var_lco_code is null or var_lco_code='" + txtSearch.Text.Trim() + "')";
                }
                Condition += " and (var_das_area is null or var_das_area='" + dtquery.Rows[0]["dasarea"].ToString() + "')";
                Condition += " and trim(var_scheme_companyname)=trim('" + dtquery.Rows[0]["company"].ToString() + "') 			AND trunc(dat_scheme_start)<=trunc(sysdate) and trunc(sysdate)<=trunc(dat_scheme_end) ";//AND TRUNC ( dat_scheme_start) <= TRUNC (SYSDATE) AND TRUNC (dat_scheme_end) >= TRUNC (SYSDATE)";

                Cls_Helper.DropDownFill(ddlscheme, "aoup_lcopre_Scheme_master", " var_scheme_name", "num_scheme_id", Condition, "");


            }

        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchBankName(string prefixText, int count)
        {
            string Str = prefixText.Trim();

            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            str += " SELECT a.num_bank_id, a.var_bank_bankcd, a.var_bank_name ";
            str += " FROM view_bank_def a ";
            str += " where upper(var_bank_name) like upper('" + prefixText + "%') and var_bank_compcode='HWI'";

            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                    dr["var_bank_name"].ToString(), dr["num_bank_id"].ToString());
                Operators.Add(item);
            }
            con.Close();
            con.Dispose();
            return Operators;
        }

        public class ChequeDetails
        {
            public string srno { get; set; }
            public string stbno { get; set; }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
            // blusername = SecurityValidation.chkData("N", txtSearch.Text + " " + txtNoofSTB.Text + " " + txtChequeNo.Text + " " + txtBalance.Text + " "
            //    + txtDiscountComb.Text + " " + txtDiscountLCO.Text + " "
            //    + txtMobileNo.Text + " " + txtTotalNet.Text + " " + txtUpfrontComb.Text + " " + txtSTBUpfront.Text + " " + txtRefNo.Text + " "
            //    + txtRateLCO.Text + " " + txtRateComb.Text + " " + txtRRNo.Text + " " + txtRateSTB.Text + " " + txtDiscountSTB.Text + " " + txtNetSTB.Text + " " + txtNetLCO.Text);
            //if (blusername.Length > 0)
            //{
            //    msgboxstr(blusername);
            //    return;
            //}

            string blusername = SecurityValidation.chkData("N", txtSearch.Text);
            if (blusername.Length > 0)
            {
                msgboxstr(blusername);
                return;
            }

            txtRateSTB.Text = "";
            txtDiscountSTB.Text = "";
            txtNetSTB.Text = "";
            txtRateLCO.Text = "";
            txtDiscountLCO.Text = "";
            txtNetLCO.Text = "";
            txtTotalNet.Text = "";
            lblmsg11.Text = "";
            txtRateSTB.Text = "";
            txtAuthCode.Text = "";
            txtNoofSTB.Text = "";
            txtChequeNo.Text = "";
            txtRemark.Text = "";
            txtRRNo.Text = "";
            txtmposuserid.Text = "";
            divdet.Visible = false;
            RBPaymode.SelectedValue = "C";
            //ddlBank1.SelectedIndex = 0;
            txtbankbranch.Text = "";
            //ddlscheme.SelectedIndex = 0;
            gridCheques.DataSource = "";
            gridCheques.DataBind();
            txtbankbranch.Text = "";
            btnSubmit.Visible = true;
            ddlboxtype.Enabled = true;
            txtSearch.Text = Session["username"].ToString();
            if (txtSearch.Text.Trim() != "")
            {
                FillSchemeDetails(ViewState["cityid"].ToString(), ViewState["stateid"].ToString());
                string customerId = Request.Form[hfCustomerId.UniqueID];
                string customerName = Request.Form[txtSearch.UniqueID];
                Cls_BLL_NEWSTB obj = new Cls_BLL_NEWSTB();
                string operator_id = "";
                string category_id = "";
                if (Session["operator_id"] != null && Session["category"] != null)
                {
                    operator_id = Convert.ToString(Session["operator_id"]);
                    category_id = Convert.ToString(Session["category"]);
                }

                string[] responseStr = obj.getLcodetails(username, Session["username"].ToString(), "0", operator_id, "3");
                if (responseStr.Length != 0)
                {
                    lblCustNo.Text = responseStr[0].Trim();
                    Session["lcoCodeR"] = lblCustNo.Text.Trim();

                    lblCustName.Text = responseStr[1].Trim();
                    lblCustAddr.Text = responseStr[2].Trim();
                    lblmobno.Text = responseStr[3].Trim();
                    txtMobileNo.Text = responseStr[3].Trim();
                    lblEmail.Text = responseStr[4].Trim();
                    txtBalance.Text = responseStr[5].Trim();
                    //lblCompanyname.Text = responseStr[6].Trim();
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
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.21/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);

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



        protected void btnAddDetails_Click(object sender, EventArgs e)
        {
            if (txtNoofSTB.Text != "")
            {

                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));

                for (int i = 0; i < Convert.ToInt32(txtNoofSTB.Text); i++)
                {
                    dt.Rows.Add();
                }

                gridCheques.DataSource = dt;
                gridCheques.DataBind();

                popCheques2.Show();
            }
            else
            {
                msgboxstr("Enter no of STB");
                return;
            }
        }

        public void ClearPaymode()
        {
            txtBankName.Text = "";
            txtChequeNo.Text = "";
            hfBankId.Value = "0";
            txtbankbranch.Text = "";
            txtChqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy").Trim();
            txtRRNo.Text = "";
            txtRefNo.Text = "";
            txtmposuserid.Text = "";
            txtAuthCode.Text = "";
        }

        protected void RBPaymode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearPaymode();
            if (RBPaymode.SelectedValue == "Q")
            {
                txtRefNo.Visible = false;
                txtChequeNo.Visible = true;
                trCheque.Visible = true;
                trMPOS.Visible = false;
                Label24.Text = "Cheque No.";
                Label15.Text = "Cheque Date";
                txtChequeNo.MaxLength = 6;
                lblchqcaption.Text = "**Credit in account will be given subject to clearance of cheque.";
            }
            else if (RBPaymode.SelectedValue == "M")
            {
                trCheque.Visible = false;
                trMPOS.Visible = true;
                lblchqcaption.Text = "";
            }
            else if (RBPaymode.SelectedValue == "D")
            {
                txtRefNo.Visible = false;
                txtChequeNo.Visible = true;
                trCheque.Visible = true;
                trMPOS.Visible = false;
                txtChequeNo.MaxLength = 6;
                Label15.Text = "DD Date";
                Label24.Text = "DD No.";
                lblchqcaption.Text = "**Credit in account will be given subject to clearance of Demand Draft.";
            }
            else if (RBPaymode.SelectedValue == "N")
            {
                txtRefNo.Visible = true;
                txtChequeNo.Visible = false;
                trCheque.Visible = true;
                trMPOS.Visible = false;
                Label15.Text = "Ref Date";
                Label24.Text = "Ref No.";
                lblchqcaption.Text = "";
                // lblchqcaption.Text = "**Credit in account will be given subject to clearance of Demand Draft.";
            }
            else
            {
                lblchqcaption.Text = "";
                trCheque.Visible = false;
                trMPOS.Visible = false;
            }



        }

        protected void btngrdsubmit_click(object sender, EventArgs e)
        {


            string AccessString = "";
            for (int i = 0; i < gridCheques.Rows.Count; i++)
            {

                TextBox txtstbno = (TextBox)gridCheques.Rows[i].Cells[2].FindControl("txtSTBNo");
                DropDownList ddlboxtype = (DropDownList)gridCheques.Rows[i].Cells[2].FindControl("ddlboxtype");

                if (txtstbno.Text == "")
                {
                    lblmsg1.Text = "Enter STB No.";
                    return;

                }
                else
                {
                    AccessString += txtstbno.Text + "#" + ddlboxtype.SelectedValue + "$";
                }

                if (ddlscheme.SelectedValue != "0")
                {/*
                    ArrayList Amt = new System.Collections.ArrayList(ddlscheme.SelectedValue.ToString().Split('$'));

                    if (Convert.ToInt32(txtNoofSTB.Text.Trim()) == 0)
                    {
                        txtRateSTB.Text = Amt[1].ToString();
                    }
                    else
                    {
                        txtRateSTB.Text = (Convert.ToDouble(Amt[1].ToString()) * Convert.ToInt32(txtNoofSTB.Text.Trim())).ToString();
                    }   ----------RP ----------------- */

                    getSchemeDetails();
                    double count = Convert.ToDouble(txtNoofSTB.Text);
                    txtRateSTB.Text = Convert.ToString(RateSTB);
                    txtDiscountSTB.Text = Convert.ToString(DicountSTB);
                    txtNetSTB.Text = Convert.ToString(NetSTB);
                    txtRateLCO.Text = Convert.ToString(RateLCO);
                    txtDiscountLCO.Text = Convert.ToString(DicountLCO);
                    txtNetLCO.Text = Convert.ToString(NetLCO);
                    txtTotalNet.Text = ((Convert.ToDouble(txtNetLCO.Text) + Convert.ToDouble(txtNetSTB.Text)) * count).ToString();


                }

            }
        }

        public void getSchemeDetails()
        {
            Cls_BLL_NEWSTB obj = new Cls_BLL_NEWSTB();
            string[] responceStr = obj.getschemeDetails(ddlscheme.SelectedValue.ToString(), Session["username"].ToString());
            if (responceStr.Length != 0)
            {
                int count = 0;
                if (txtNoofSTB.Text.Trim() != "")
                {
                    count = Convert.ToInt32(txtNoofSTB.Text.Trim());
                }
                RateSTB = Convert.ToDouble(responceStr[0].Trim());
                DicountSTB = Convert.ToDouble(responceStr[1].Trim());
                NetSTB = Convert.ToDouble(responceStr[2].Trim());
                RateLCO = Convert.ToDouble(responceStr[3].Trim());
                DicountLCO = Convert.ToDouble(responceStr[4].Trim());
                NetLCO = Convert.ToDouble(responceStr[5].Trim());
                ddlboxtype.SelectedValue = Convert.ToString(responceStr[6].Trim());
                ViewState["software_hardware"] = Convert.ToString(responceStr[7].Trim());
                ViewState["stbcount"] = Convert.ToString(responceStr[8].Trim());
                string AllowPDC = Convert.ToString(responceStr[9].Trim());
                ViewState["PDCTenure"] = Convert.ToString(responceStr[10].Trim());
                ViewState["LCOUpfront"] = Convert.ToString(responceStr[11].Trim());
                ViewState["STBUpfront"] = Convert.ToString(responceStr[12].Trim());
                txtSTBUpfront.Text = Convert.ToString(responceStr[12].Trim());
                txtlcoUpfront.Text = Convert.ToString(responceStr[11].Trim());
                txtUpfrontComb.Text = Convert.ToString(Convert.ToDouble(txtSTBUpfront.Text) + Convert.ToDouble(txtlcoUpfront.Text));
                ViewState["AllowPDC"] = AllowPDC;
                txtpdcpaidamount.Text = "0";
                if (AllowPDC == "Y" || AllowPDC == "W")
                {
                    if (AllowPDC == "Y")
                    {
                        lnkpdcdetails.Text = "Select PDC Detail";
                    }
                    else
                    {
                        lnkpdcdetails.Text = "Select WPDC Detail";
                    }
                    lnkpdcdetails.Visible = true;
                    trpdcamount.Visible = true;
                    Label75.Text = " Total Net ";
                    txtlcoUpfront.Visible = true;
                    txtSTBUpfront.Visible = true;
                    txtUpfrontComb.Visible = true;
                    Label74.Visible = true;

                    txtTotalNet.Text = ((Convert.ToDouble(txtlcoUpfront.Text) + Convert.ToDouble(txtSTBUpfront.Text)) * count).ToString();
                    ViewState["RemainingPDCAmount"] = (((Convert.ToDouble(NetSTB) + Convert.ToDouble(NetLCO)) * count) - Convert.ToDouble(txtTotalNet.Text)).ToString();
                    txtpdcpaidamount.Text = ViewState["RemainingPDCAmount"].ToString();
                }
                else
                {
                    lnkpdcdetails.Visible = false;
                    trpdcamount.Visible = false;
                    Label75.Text = "Total Net ";
                    txtlcoUpfront.Visible = false;
                    txtSTBUpfront.Visible = false;
                    txtUpfrontComb.Visible = false;
                    Label74.Visible = false;
                    ViewState["RemainingPDCAmount"] = "0";
                    txtpdcpaidamount.Text = "0";
                    txtTotalNet.Text = ((Convert.ToDouble(NetSTB) + Convert.ToDouble(NetLCO)) * count).ToString();
                }

            }

        }


        protected void ddlScheme_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtNoofSTB.Text.Trim() == "")
            {
                msgboxstr("Plese provide No. of STB");
                ddlscheme.SelectedValue = "0";
                return;
            }

            int count = 0;
            if (txtNoofSTB.Text.Trim() != "")
            {
                count = Convert.ToInt32(txtNoofSTB.Text.Trim());
            }
            if (ddlscheme.SelectedValue == "" || ddlscheme.SelectedValue == "0")
            {
                txtTotalNet.Text = "";
                txtRateLCO.Text = "";
                txtDiscountLCO.Text = "";
                txtNetLCO.Text = "";
                txtRateSTB.Text = "";
                txtDiscountSTB.Text = "";
                txtNetSTB.Text = "";
            }

            else
            {
                //ArrayList Amt = new System.Collections.ArrayList(ddlscheme.SelectedValue.ToString().Split('$')); --RP

                if (count == 0)
                {
                    //getSchemeDetails();
                }
                else
                {
                    getSchemeDetails();
                    if (ViewState["stbcount"] != null)
                    {
                        if (count > Convert.ToInt32(ViewState["stbcount"]))
                        {
                            msgboxstr("Please enter proper STB count");
                            return;
                        }
                    }
                    /* ddlboxtype.Enabled = false;
                     txtRateSTB.Text = (Convert.ToDouble(RateSTB.ToString()) * count).ToString();
                     txtDiscountSTB.Text = (Convert.ToDouble(DicountSTB.ToString()) * count).ToString();
                     txtNetSTB.Text = (Convert.ToDouble(NetSTB.ToString()) * count).ToString();
                     txtRateLCO.Text = (Convert.ToDouble(RateLCO.ToString()) * count).ToString();
                     txtDiscountLCO.Text = (Convert.ToDouble(DicountLCO.ToString()) * count).ToString();
                     txtNetLCO.Text = (Convert.ToDouble(NetLCO.ToString()) * count).ToString();
                     txtTotalNet.Text = (Convert.ToDouble(txtNetLCO.Text) + Convert.ToDouble(txtNetSTB.Text)).ToString();*/
                    ddlboxtype.Enabled = false;
                    // txtRateComb.Text=Convert.ToDouble;
                    txtRateSTB.Text = Convert.ToString(RateSTB);
                    txtDiscountSTB.Text = Convert.ToString(DicountSTB);
                    txtNetSTB.Text = Convert.ToString(NetSTB);
                    txtRateLCO.Text = Convert.ToString(RateLCO);
                    txtDiscountLCO.Text = Convert.ToString(DicountLCO);
                    txtNetLCO.Text = Convert.ToString(NetLCO);

                    txtRateComb.Text = Convert.ToString(RateSTB + RateLCO);
                    txtNetComb.Text = Convert.ToString(NetSTB + NetLCO);
                    txtDiscountComb.Text = Convert.ToString(DicountSTB + DicountLCO);



                }

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string blusername = SecurityValidation.chkData("N",txtNoofSTB.Text+""+txtMobileNo.Text);
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


            lblcust.Text = lblCustNo.Text;
            lblnoofstb.Text = txtNoofSTB.Text;
            lblSchemeName.Text = ddlscheme.SelectedItem.ToString();
            lblamount1.Text = txtTotalNet.Text.Trim();
            if (ViewState["AllowPDC"].ToString() == "Y")
            {
                if (ViewState["AccessString"] != "")
                {
                    lblCofAmount.Text = "Upfront Amount";
                    lblLCOSubscriptionTotalText.Text = "Upfront Subscription Total";
                    lblSTBTotalText.Text = "Upfront STB Activation Total";
                    lblSTBTotal.Text = Convert.ToString(Convert.ToDouble(txtSTBUpfront.Text) * Convert.ToDouble(txtNoofSTB.Text));
                    lblLCOTotal.Text = Convert.ToString(Convert.ToDouble(txtlcoUpfront.Text) * Convert.ToDouble(txtNoofSTB.Text));
                }
                else
                {
                    msgboxstr("Please provide proper PDC details. ");
                    return;
                }

            }
            else if (ViewState["AllowPDC"].ToString() == "W")
            {
                if (ViewState["AccessString"] != "")
                {
                    lblCofAmount.Text = "Upfront Amount";
                    lblLCOSubscriptionTotalText.Text = "Upfront Subscription Total";
                    lblSTBTotalText.Text = "Upfront STB Activation Total";
                    lblSTBTotal.Text = Convert.ToString(Convert.ToDouble(txtSTBUpfront.Text) * Convert.ToDouble(txtNoofSTB.Text));
                    lblLCOTotal.Text = Convert.ToString(Convert.ToDouble(txtlcoUpfront.Text) * Convert.ToDouble(txtNoofSTB.Text));
                }
                else
                {
                    msgboxstr("Please provide proper Wallet PDC details. ");
                    return;
                }

            }
            else
            {
                lblCofAmount.Text = "Amount";
                //lblLCOSubscriptionTotalText.Text = "Subscription Total";
                //lblSTBTotalText.Text = "STB Total";
                //lblSTBTotal.Text = Convert.ToString(Convert.ToDouble(txtNetSTB.Text) * Convert.ToDouble(txtNoofSTB.Text));
                //lblLCOTotal.Text = Convert.ToString(Convert.ToDouble(txtNetLCO.Text) * Convert.ToDouble(txtNoofSTB.Text));
            }
            if (ValidatePage())
            {
                popupModifyConfirm.Show();
            }
        }

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{

        //    lblcust.Text = lblCustNo.Text;
        //    lblnoofstb.Text = txtNoofSTB.Text;
        //    lblSchemeName.Text = ddlscheme.SelectedItem.ToString();
        //    lblamount1.Text = txtTotalNet.Text.Trim();
        //    if (ViewState["AllowPDC"].ToString() == "Y")
        //    {
        //        if (Convert.ToDouble(ViewState["RemainingPDCAmount"].ToString()) != Convert.ToDouble(txtpdcpaidamount.Text))
        //        {
        //            msgboxstr("Please provide proper PDC details. ");
        //            return;
        //        }
        //        lblCofAmount.Text = "Upfront Amount";
        //        lblLCOSubscriptionTotalText.Text = "Upfront Subscription Total";
        //        lblSTBTotalText.Text = "Upfront STB Activation Total";
        //        lblSTBTotal.Text = Convert.ToString(Convert.ToDouble(txtSTBUpfront.Text) * Convert.ToDouble(txtNoofSTB.Text));
        //        lblLCOTotal.Text = Convert.ToString(Convert.ToDouble(txtlcoUpfront.Text) * Convert.ToDouble(txtNoofSTB.Text));
        //    }
        //    else
        //    {
        //        lblCofAmount.Text = "Amount";
        //        lblLCOSubscriptionTotalText.Text = "LCO Subscription Total";
        //        lblSTBTotalText.Text = "STB Total";
        //        lblSTBTotal.Text = Convert.ToString(Convert.ToDouble(txtNetSTB.Text) * Convert.ToDouble(txtNoofSTB.Text));
        //        lblLCOTotal.Text = Convert.ToString(Convert.ToDouble(txtNetLCO.Text) * Convert.ToDouble(txtNoofSTB.Text));
        //    }
        //    if (ValidatePage())
        //    {
        //        popupModifyConfirm.Show();
        //    }
        //}

        protected void btnModifyConfirm_click(object sender, EventArgs e)
        {

            Cls_BLL_NEWSTB obj = new Cls_BLL_NEWSTB();
            /*String AccessString = "";
            for (int i = 0; i < gridCheques.Rows.Count; i++)
            {

                TextBox txtstbno = (TextBox)gridCheques.Rows[i].Cells[2].FindControl("txtSTBNo");
                DropDownList ddlboxtype = (DropDownList)gridCheques.Rows[i].Cells[2].FindControl("ddlboxtype");

                if (txtstbno.Text == "")
                {
                    lblmsg1.Text = "Enter STB No.";
                    return;

                }
                else
                {
                    AccessString += txtstbno.Text + "#" + ddlboxtype.SelectedValue + "$";
                }
            }

            if (AccessString != "")
            {
                AccessString = AccessString.Remove(AccessString.Length - 1);

            }
            else
            {
                msgboxstr("Enter STB details");

                return;
            }
            */
            if (ValidatePage())
            {
                if (!CheckIFSTBNoAlreadyExist(txtNoofSTB.Text.Trim()))
                {
                    getcitystate();
                    Hashtable ht = new Hashtable();
                    ht.Add("STBCount", txtNoofSTB.Text);
                    ht.Add("STBLCO", lblCustNo.Text);
                    ht.Add("Remark", txtRemark.Text.Trim());
                    ht.Add("MobileNo", txtMobileNo.Text.Trim());
                    ht.Add("SchemeID", ddlscheme.SelectedValue.ToString());
                    ht.Add("STBSRNo", "");
                    if (RBPaymode.SelectedValue == "Q" || RBPaymode.SelectedValue == "D")
                    {
                        ht.Add("ChequeNo", txtChequeNo.Text);
                        ht.Add("ChequeDate", Convert.ToDateTime(txtChqDate.Text.Trim()));
                    }
                    else if (RBPaymode.SelectedValue == "N")
                    {
                        ht.Add("ChequeNo", txtRefNo.Text);
                        ht.Add("ChequeDate", Convert.ToDateTime(txtChqDate.Text.Trim()));
                    }
                    else
                    {
                        ht.Add("ChequeNo", null);
                        ht.Add("ChequeDate", Convert.ToDateTime(DateTime.MinValue));
                    }
                    ht.Add("BankId", hfBankId.Value.Trim());
                    ht.Add("Branch", txtbankbranch.Text.Trim());
                    ht.Add("RRNo", txtRRNo.Text.Trim());
                    ht.Add("mposid", txtmposuserid.Text.Trim());
                    ht.Add("AuthCode", txtAuthCode.Text.Trim());
                    ht.Add("transtype", "SP");
                    ht.Add("transsubtype", "SPSN");
                    ht.Add("PayMode", "L");//RBPaymode.SelectedValue);
                    ht.Add("City", ViewState["cityid"].ToString());
                    ht.Add("State", ViewState["stateid"].ToString());
                    ht.Add("RateSTB", txtRateSTB.Text);
                    ht.Add("DiscountSTB", txtDiscountSTB.Text);
                    if (ViewState["AllowPDC"].ToString() == "Y" || ViewState["AllowPDC"].ToString() == "W")
                    {
                        ht.Add("NetSTB", txtSTBUpfront.Text);
                    }
                    else
                    {
                        ht.Add("NetSTB", txtNetSTB.Text);
                    }
                    ht.Add("RateLCO", txtRateLCO.Text);
                    ht.Add("DiscountLCO", txtDiscountLCO.Text);
                    if (ViewState["AllowPDC"].ToString() == "Y" || ViewState["AllowPDC"].ToString() == "W")
                    {
                        ht.Add("NetLCO", txtlcoUpfront.Text);
                    }
                    else
                    {
                        ht.Add("NetLCO", txtNetLCO.Text);
                    }
                    ht.Add("TotalNet", txtTotalNet.Text);
                    ht.Add("insertBy", Session["username"].ToString());
                    ht.Add("boxtype", ddlboxtype.SelectedValue);
                    if (ddltype.SelectedValue == "O")
                    {
                        ht.Add("type", txtOther.Text);
                    }
                    else
                    {
                        ht.Add("type", ddltype.SelectedValue);
                    }
                    if (ViewState["AccessString"] == null)
                    {
                        ht.Add("AccessString", "");
                    }
                    else
                    {

                        string AccessString = ViewState["AccessString"].ToString();
                        if (AccessString != "")
                        {

                            AccessString = AccessString.TrimEnd('$');//.Remove(AccessString.Length - 1);
                            ht.Add("AccessString", AccessString);
                        }
                        else
                        {
                            ht.Add("AccessString", "");
                        }

                    }
                    try
                    {

                        string result = obj.InsertNewSTBSP("aoup_lcopre_pis_newstbsp_ins", ht);

                        string[] Getresponse = result.Split('$');
                        if (Getresponse[0].Contains("9999"))
                        {
                            divdet.Visible = false;
                            ResetForm();
                            string[] Workorder = Getresponse[1].Split('~');
                            ViewState["Result"] = "9999";
                            ViewState["transID"] = Workorder[0];
                            // 

                            Response.Write("<script language='javascript'> window.open('../Transaction/rcptPaymentReceiptInvoicePIS.aspx?transID=" + ViewState["transID"] + "', 'Print_Receipt','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes,location=no,status=no');</script>");
                            //msgboxstr("Work Order generated Successfully,Work Order No:" + Workorder[1].ToString());
                            msgboxstr("The hardware ownership belongs to hathway,Work Order No:" + Workorder[1].ToString());
                        }
                        else
                        {
                            msgboxstr(Getresponse[1].ToString());

                        }
                    }
                    catch (Exception ex)
                    {
                        msgboxstr("Error Occured.Try Again !");

                    }
                }
                else
                {
                    msgboxstr("STB No. Already Exist");
                }
            }

        }

        public void getcitystate()
        {

            string str = "";
            str = "select num_usermst_stateid,num_usermst_cityid from aoup_lcopre_user_det where var_usermst_username='" + Session["username"].ToString() + "'";

            DataTable dtscity = objHelper1.GetDataTable(str);
            if (dtscity.Rows.Count > 0)
            {
                ViewState["cityid"] = dtscity.Rows[0]["num_usermst_cityid"].ToString();
                ViewState["stateid"] = dtscity.Rows[0]["num_usermst_stateid"].ToString();
            }

        }

        private bool CheckIFSTBNoAlreadyExist(string stbNo)
        {
            bool result = false;

            if (stbNo != "")
            {
                string query = "Select Count(var_pisnewstb_stbno) from aoup_lcopre_pis_newstb where var_pisnewstb_stbno =" + stbNo + "";

                int count = Cls_Helper.ExecuteScalarQuery(query);

                if (count != null && count > 0)
                    result = true;
                else
                    result = false;
            }

            return result;
        }

        private bool ValidatePage()
        {
            bool result = true;

            if (ddltype.SelectedValue == "0")
            {
                msgboxstr("Please select type.");
                return false;
            }
            if (ddltype.SelectedValue == "O")
            {
                if (txtOther.Text == "")
                {
                    msgboxstr("Plese enter Other Details.");
                    return false;
                }
            }
            if (txtNoofSTB.Text.Trim() == "" || Convert.ToInt32(txtNoofSTB.Text) == 0)
            {
                msgboxstr("Plese provide No. of STB");
                return false;
            }

            if (ddlscheme.SelectedValue == "0")
            {
                msgboxstr("Please Select Scheme ");
                return false;
            }
            if (ddltype.SelectedValue.ToString() == "STB")
            {
                if (ddlboxtype.SelectedValue == "0")
                {
                    msgboxstr("Please Select Box Type ");
                    return false;
                }
            }

            //if (RBPaymode.SelectedValue == "Q")
            //{
            //    if (txtBankName.Text == "")
            //    {
            //        msgboxstr("Please Enter Bank Name");
            //        return false;
            //    }

            //    if (txtbankbranch.Text == "")
            //    {
            //        msgboxstr("Please Enter Bank Branch");
            //        return false;
            //    }
            //    if (txtChequeNo.Text == "" || txtChequeNo.Text == "0")
            //    {
            //        msgboxstr("Cheque No. can not be blank or zero");
            //        return false;
            //    }
            //    if (txtChequeNo.Text.Length != 6)
            //    {
            //        msgboxstr("Enter Enter 6 digits Cheque No.");
            //        return false;
            //    }

            //}
            //else if (RBPaymode.SelectedValue == "M")
            //{
            //    if (txtRRNo.Text.Trim() == string.Empty)
            //    {
            //        msgboxstr("RR No. can not be blank");
            //        return false;
            //    }

            //    if (txtmposuserid.Text.Trim() == string.Empty)
            //    {
            //        msgboxstr("MPOS User Id can not be blank");
            //        return false;
            //    }

            //    if (txtAuthCode.Text.Trim() == string.Empty)
            //    {
            //        msgboxstr("Auth Code can not be blank");
            //        return false;
            //    }
            //    hfBankId.Value = "0";

            //}
            //if (RBPaymode.SelectedValue == "N")
            //{
            //    if (txtBankName.Text == "")
            //    {
            //        msgboxstr("Please Enter Bank Name ");
            //        return false;
            //    }
            //    if (txtbankbranch.Text == "")
            //    {
            //        msgboxstr("Please Enter Bank Branch ");
            //        return false;
            //    }
            //    if (txtRefNo.Text.Trim() == "")
            //    {
            //        msgboxstr("Please Enter Reference No.");
            //        return false;
            //    }

            //    if (txtRefNo.Text.Length != 12)
            //    {
            //        msgboxstr("Please Enter 12 digits Reference No.");
            //        return false;
            //    }

            //}
            //if (RBPaymode.SelectedValue == "D")
            //{
            //    if (txtBankName.Text == "")
            //    {
            //        msgboxstr("Please Enter Bank Name");
            //        return false;
            //    }

            //    if (txtbankbranch.Text == "")
            //    {
            //        msgboxstr("Please Enter Bank Branch");
            //        return false;
            //    }
            //    if (txtChequeNo.Text == "" || txtChequeNo.Text == "0")
            //    {
            //        msgboxstr("DD No. can not be blank or zero");
            //        return false;
            //    }
            //    if (txtChequeNo.Text.Length != 6)
            //    {
            //        msgboxstr("Enter Enter 6 digits DD No.");
            //        return false;
            //    }
            //}
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

        public void msgboxstr(string message)
        {
            lblPopupResponse.Text = message;
            popMsg.Show();
        }

        protected void ResetForm()
        {
            ClearPaymode();
            txtRateSTB.Text = "";
            txtAuthCode.Text = "";
            txtNoofSTB.Text = "";
            // txtSearch.Text = "";
            txtChequeNo.Text = "";
            txtRemark.Text = "";
            txtRRNo.Text = "";
            txtmposuserid.Text = "";
            divdet.Visible = true;
            RBPaymode.SelectedValue = "C";
            // ddlBank1.SelectedValue = "0";
            txtBankName.Text = "";
            ddlscheme.SelectedValue = "0";
            gridCheques.DataSource = "";
            gridCheques.DataBind();
            txtbankbranch.Text = "";
            trMPOS.Visible = false;
            trCheque.Visible = false;
            ddltype.SelectedValue = "0";
            ddlboxtype.Enabled = true;
            txtRateSTB.Text = "";
            txtDiscountSTB.Text = "";
            txtNetSTB.Text = "";
            txtRateLCO.Text = "";
            txtDiscountLCO.Text = "";
            txtNetLCO.Text = "";
            txtTotalNet.Text = "";
            ddlboxtype.SelectedValue = "0";
        }

        protected void txtNoofSTB_TextChanged(object sender, EventArgs e)
        {
            if (txtNoofSTB.Text.Trim() == "")
            {
                return;
            }

            if (ddlscheme.SelectedValue == "0")
            {
                return;
            }

            getSchemeDetails();

            double count = Convert.ToDouble(txtNoofSTB.Text);
            txtRateSTB.Text = Convert.ToString(RateSTB);
            txtDiscountSTB.Text = Convert.ToString(DicountSTB);
            txtNetSTB.Text = Convert.ToString(NetSTB);
            txtRateLCO.Text = Convert.ToString(RateLCO);
            txtDiscountLCO.Text = Convert.ToString(DicountLCO);
            txtNetLCO.Text = Convert.ToString(NetLCO);
            txtTotalNet.Text = ((Convert.ToDouble(txtNetLCO.Text) + Convert.ToDouble(txtNetSTB.Text)) * count).ToString();

        }

        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddltype.SelectedValue == "O")
            {
                txtOther.Visible = true;
            }
            else
            {
                txtOther.Visible = false;
            }
        }


        protected void lbtnSchemeDetail_Click(object sender, EventArgs e)
        {
            BindSchemeDetails();
            popCheques3.Show();
        }
        public void BindSchemeDetails()
        {

            if (ddlscheme.SelectedIndex != 0)
            {
                Cls_BLL_NEWSTB obj = new Cls_BLL_NEWSTB();
                string[] responceStr = obj.BindschemeDetails(ddlscheme.SelectedValue.ToString(), Session["username"].ToString());
                if (responceStr.Length != 0)
                {
                    // RateSTB = Convert.ToDouble(responceStr[0].Trim());
                    lblDetSchemeName.Text = responceStr[0].Trim();
                    lblDetschemedescr.Text = responceStr[1].Trim();
                    lblDettermallow.Text = responceStr[2].Trim();
                    lblDetpenalty.Text = responceStr[3].Trim();
                    lblDetPlanchangeallow.Text = responceStr[4].Trim();
                    lblDetplanactallow.Text = responceStr[5].Trim();
                    lblDetsubpayterm.Text = responceStr[6].Trim();
                    lblDetsubscrplanallow.Text = responceStr[7].Trim();
                    lblDetLCORate.Text = responceStr[8].Trim();
                    lblDetLCODiscount.Text = responceStr[9].Trim();
                    lblDetLCONet.Text = responceStr[10].Trim();
                    lblDetStbCount.Text = responceStr[11].Trim();
                    lblDetStbStatus.Text = responceStr[12].Trim();
                    lblDetBoxType.Text = responceStr[13].Trim();
                    lblDetstbmakemodule.Text = responceStr[14].Trim();
                    lblDetSTBRate.Text = responceStr[15].Trim();
                    lblSTBDiscount.Text = responceStr[16].Trim();
                    lblSTBNet.Text = responceStr[17].Trim();
                }
            }
        }

        protected void lnkpdcdetails_Click(object sender, EventArgs e)
        {
            if (ViewState["CurrentTable"] != null)
            {
                SetPreviousData();
            }
            else
            {
                SetInitialRow();
            }
            string columnHeader = "";
            if (!lnkpdcdetails.Text.Contains("WPDC"))
            {
                columnHeader = gridPDCChques.Columns[0].HeaderText;
            }
            else
            {
                columnHeader = GridView1.Columns[0].HeaderText;
            }
            if (columnHeader.Contains("Cheque No"))
            {
                popCheques.Show();
            }
            else
            {
                ModalPopupExtender1.Show();
            }

        }

        protected void btnpdcClose_click(object sender, EventArgs e)
        {
            ViewState["CurrentTable"] = null;
            ViewState["AccessString"] = "";
            if (lnkpdcdetails.Text.Contains("WPDC"))
            { ModalPopupExtender1.Show(); }
            else { popCheques.Show(); }

            txtpdcpaidamount.Text = "0";
            if (ViewState["txtTotalNet"] != null)
            {
                txtTotalNet.Text = ViewState["txtTotalNet"].ToString();
            }
            SetInitialRow();

        }

        protected void btnpdcsubmit_click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (dt.Columns.Contains("RowNumber"))
            {
            }
            else
            {
                dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            }
            if (dt.Columns.Contains("Column1"))
            {
            }
            else
            {
                dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            }
            if (dt.Columns.Contains("Column2"))
            {
            }
            else
            {
                dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            }
            if (dt.Columns.Contains("Column3"))
            {
            }
            else
            {
                dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            }

            string columnHeader = "";
            if (!lnkpdcdetails.Text.Contains("WPDC"))
            {
                columnHeader = gridPDCChques.Columns[0].HeaderText;
            }
            else
            {
                columnHeader = GridView1.Columns[0].HeaderText;
            }

            if (!columnHeader.Contains("Date"))
            {
                if (dt.Columns.Contains("Column4"))
                {
                }
                else
                {
                    dt.Columns.Add(new DataColumn("Column4", typeof(string)));
                }
            }
            int rowIndex = 1;
            if (ViewState["txtTotalNet"] != null)
            {
                txtTotalNet.Text = ViewState["txtTotalNet"].ToString();
            }
            string AccessString = "";

            if (!columnHeader.Contains("Date"))
            {
                if (gridPDCChques.Rows.Count > 0)
                {
                    double PDCamount = 0;
                    for (int i = 0; i < gridPDCChques.Rows.Count; i++)
                    {

                        TextBox txtchqno = (TextBox)gridPDCChques.Rows[i].Cells[1].FindControl("txtpdcchequeno");
                        TextBox txtPDCChqDate = (TextBox)gridPDCChques.Rows[i].Cells[2].FindControl("txtPDCChqDate");
                        DropDownList ddlpdcbank = (DropDownList)gridPDCChques.Rows[i].Cells[3].FindControl("ddlpdcbank");
                        TextBox txtpdcamount = (TextBox)gridPDCChques.Rows[i].Cells[2].FindControl("txtpdcamount");
                        if (txtchqno.Text == "")
                        {
                            lblPDCMsg.Text = "Enter cheque No";
                            popCheques.Show();
                            return;
                        }

                        if (txtchqno.Text.Length != 6)
                        {
                            lblPDCMsg.Text = "Enter 6 digit cheque No";
                            popCheques.Show();
                            return;
                        }
                        if (txtPDCChqDate.Text == "")
                        {
                            lblPDCMsg.Text = "Enter cheque Date";
                            popCheques.Show();
                            return;

                        }

                        if (ddlpdcbank.SelectedValue == "" || ddlpdcbank.SelectedValue == "0")
                        {
                            lblPDCMsg.Text = "Please select bank";
                            popCheques.Show();
                            return;

                        }

                        if (Convert.ToDateTime(txtPDCChqDate.Text.Trim()) <= DateTime.Now)
                        {
                            lblPDCMsg.Text = "Cheque date can not be current date or less then current date";
                            popCheques.Show();
                            return;
                        }

                        if (txtpdcamount.Text == "")
                        {
                            lblPDCMsg.Text = "Please enter amount";
                            popCheques.Show();
                            return;

                        }
                        else
                        {
                            PDCamount += Convert.ToDouble(txtpdcamount.Text);
                            AccessString += txtchqno.Text + "#" + txtPDCChqDate.Text + "#" + ddlpdcbank.SelectedValue + "#" + txtpdcamount.Text + "$";
                            ViewState["AccessString"] = AccessString;

                            dt.Rows.Add(rowIndex, txtchqno.Text, txtPDCChqDate.Text, ddlpdcbank.SelectedValue, txtpdcamount.Text);
                            ViewState["CurrentTable"] = dt;
                            rowIndex++;
                        }
                    }
                    if (PDCamount < (Convert.ToDouble(ViewState["RemainingPDCAmount"].ToString())) || PDCamount > (Convert.ToDouble(ViewState["RemainingPDCAmount"].ToString())))
                    {
                        lblPDCMsg.Text = "PDC Amount can not be greater or less than total net";
                        popCheques.Show();
                        return;
                    }
                    ViewState["pdcamount"] = PDCamount;
                    txtpdcpaidamount.Text = PDCamount.ToString();
                    ViewState["txtTotalNet"] = Convert.ToDouble(txtTotalNet.Text);
                    //txtTotalNet.Text = (Convert.ToDouble(ViewState["txtTotalNet"]) - PDCamount).ToString();

                }
            }
            else
            {
                if (GridView1.Rows.Count > 0)
                {
                    double PDCamount = 0;
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {

                        // TextBox txtchqno = (TextBox)gridPDCChques.Rows[i].Cells[1].FindControl("txtpdcchequeno");
                        TextBox txtPDCChqDate = (TextBox)GridView1.Rows[i].Cells[0].FindControl("txtPDCChqDate");
                        DropDownList ddlpdcbank = (DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlCategory");
                        TextBox txtpdcamount = (TextBox)GridView1.Rows[i].Cells[2].FindControl("txtpdcamount");

                        if (txtPDCChqDate.Text == "")
                        {
                            lblWPDCmsg.Text = "Enter Date";
                            ModalPopupExtender1.Show();
                            return;

                        }

                        if (ddlpdcbank.SelectedValue == "" || ddlpdcbank.SelectedValue == "0")
                        {
                            lblWPDCmsg.Text = "Please select Category";
                            ModalPopupExtender1.Show();
                            return;

                        }

                        if (Convert.ToDateTime(txtPDCChqDate.Text.Trim()) <= DateTime.Now)
                        {
                            lblWPDCmsg.Text = "Date can not be current date or less then current date";
                            ModalPopupExtender1.Show();
                            return;
                        }

                        if (txtpdcamount.Text == "")
                        {
                            lblWPDCmsg.Text = "Please enter amount";
                            ModalPopupExtender1.Show();
                            return;

                        }
                        else
                        {
                            PDCamount += Convert.ToDouble(txtpdcamount.Text);
                            //if (ddlpdcbank.SelectedIndex == 0)
                            //{
                            //    AccessString += " " + "#" + txtPDCChqDate.Text + "#" + " " + "#" + txtpdcamount.Text + "#HINV$";
                            //}
                            //else
                            //{
                            //    AccessString += " " + "#" + txtPDCChqDate.Text + "#" + " " + "#" + txtpdcamount.Text + "#SINV$";
                            //}
                            if (ddlpdcbank.SelectedIndex == 0)
                            {
                                AccessString += " " + "#" + txtPDCChqDate.Text + "# #0#WINV#" + txtpdcamount.Text + "#0$";
                            }
                            else
                            {
                                AccessString += " " + "#" + txtPDCChqDate.Text + "# #0#WINV#0#" + txtpdcamount.Text + "$";
                            }
                            ViewState["AccessString"] = AccessString;

                            dt.Rows.Add(rowIndex, txtPDCChqDate.Text, ddlpdcbank.SelectedValue, txtpdcamount.Text);
                            ViewState["CurrentTable"] = dt;
                            rowIndex++;
                        }
                    }
                    if (PDCamount < (Convert.ToDouble(ViewState["RemainingPDCAmount"].ToString())) || PDCamount > (Convert.ToDouble(ViewState["RemainingPDCAmount"].ToString())))
                    {
                        lblWPDCmsg.Text = "Wallet PDC Amount can not be greater or less than total net";
                        ModalPopupExtender1.Show();
                        return;
                    }
                    ViewState["pdcamount"] = PDCamount;
                    txtpdcpaidamount.Text = PDCamount.ToString();
                    ViewState["txtTotalNet"] = Convert.ToDouble(txtTotalNet.Text);

                    //txtTotalNet.Text = (Convert.ToDouble(ViewState["txtTotalNet"]) - PDCamount).ToString();

                }
            }
        }

        private void SetInitialRow()
        {
            string Getinstallment = "";
            if (!lnkpdcdetails.Text.Contains("WPDC"))
            {
                Getinstallment = "select num_noofinstallment noofinstallment from aoup_lcopre_scheme_master  where var_allowpdc='Y' and num_scheme_id=" + ddlscheme.SelectedValue;
            }
            else
            {
                Getinstallment = "select num_noofinstallment noofinstallment from aoup_lcopre_scheme_master  where var_allowpdc='W' and num_scheme_id=" + ddlscheme.SelectedValue;
            }


            DataTable dtGetinstallment = objHelper1.GetDataTable(Getinstallment);

            DataTable dt = new DataTable();
            DataRow dr = null;
            if (dt.Columns.Contains("RowNumber"))
            {
            }
            else
            {
                dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            }
            if (dt.Columns.Contains("Column1"))
            {
            }
            else
            {
                dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            }
            if (dt.Columns.Contains("Column2"))
            {
            }
            else
            {
                dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            }
            if (dt.Columns.Contains("Column3"))
            {
            }
            else
            {
                dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            }

            string columnHeader = "";
            if (!lnkpdcdetails.Text.Contains("WPDC"))
            {
                columnHeader = gridPDCChques.Columns[0].HeaderText;
            }
            else
            {
                columnHeader = GridView1.Columns[0].HeaderText;
            }
            if (columnHeader.Contains("Cheque No"))
            {
                if (dt.Columns.Contains("Column4"))
                {
                }
                else
                {
                    dt.Columns.Add(new DataColumn("Column4", typeof(string)));
                }
            }

            if (dtGetinstallment.Rows.Count > 0)
            {
                if (dtGetinstallment.Rows[0]["noofinstallment"] != null || dtGetinstallment.Rows[0]["noofinstallment"] != "")
                {
                    Int32 NoOfinstallment = Convert.ToInt32(dtGetinstallment.Rows[0]["noofinstallment"]);
                    for (int i = 0; i < NoOfinstallment; i++)
                    {
                        dr = dt.NewRow();
                        dr["RowNumber"] = i + 1;
                        dr["Column1"] = string.Empty;
                        dr["Column2"] = string.Empty;
                        dr["Column3"] = string.Empty;
                        if (columnHeader.Contains("Cheque No"))
                        {
                            dr["Column4"] = string.Empty;
                        }
                        dt.Rows.Add(dr);
                    }

                }

                ViewState["CurrentTable"] = dt;
                if (columnHeader.Contains("Cheque No"))
                {
                    gridPDCChques.DataSource = dt;
                    gridPDCChques.DataBind();
                    for (int j = 0; j < gridPDCChques.Rows.Count; j++)
                    {
                        string str = "";
                        str += " SELECT a.var_bank_name, a.num_bank_id FROM view_bank_def a  where var_bank_compcode='HWI'";
                        DropDownList Ddlbankname = (DropDownList)gridPDCChques.Rows[j].FindControl("ddlpdcbank");

                        Cls_Helper.DropDownFill(Ddlbankname, "", "", " ", "", str);
                    }
                }
                else
                {

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    //for (int j = 0; j < GridView1.Rows.Count; j++)
                    //{
                    //    string str = "";
                    //    str += " SELECT a.var_bank_name, a.num_bank_id FROM view_bank_def a  where var_bank_compcode='HWI'";
                    //    DropDownList Ddlbankname = (DropDownList)gridPDCChques.Rows[j].FindControl("ddlpdcbank");

                    //    Cls_Helper.DropDownFill(Ddlbankname, "", "", " ", "", str);
                    //}
                }
            }
        }

        private void AddNewRowToGrid()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox chequeno = (TextBox)gridPDCChques.Rows[rowIndex].Cells[1].FindControl("txtpdcchequeno");
                        TextBox chqdate = (TextBox)gridPDCChques.Rows[rowIndex].Cells[2].FindControl("txtpdcChqDate");
                        DropDownList bankname = (DropDownList)gridPDCChques.Rows[rowIndex].Cells[3].FindControl("ddlpdcbank");
                        TextBox amount = (TextBox)gridPDCChques.Rows[rowIndex].Cells[4].FindControl("txtpdcamount");

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Column1"] = chequeno.Text;
                        dtCurrentTable.Rows[i - 1]["Column2"] = chqdate.Text;
                        dtCurrentTable.Rows[i - 1]["Column3"] = bankname.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["Column4"] = amount.Text;

                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;

                    gridPDCChques.DataSource = dtCurrentTable;
                    gridPDCChques.DataBind();
                    for (int j = 0; j < gridPDCChques.Rows.Count; j++)
                    {
                        string str = "";
                        str += " SELECT a.var_bank_name, a.num_bank_id FROM view_bank_def a  where var_bank_compcode='HWI'";
                        DropDownList Ddlbankname = (DropDownList)gridCheques.Rows[j].FindControl("ddlpdcbank");

                        Cls_Helper.DropDownFill(Ddlbankname, "", "", " ", "", str);
                    }
                }
                else
                {
                    SetInitialRow();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        private void SetPreviousData()
        {

            string columnHeader = "";
            if (!lnkpdcdetails.Text.Contains("WPDC"))
            {
                columnHeader = gridPDCChques.Columns[0].HeaderText;
            }
            else
            {
                columnHeader = GridView1.Columns[0].HeaderText;
            }
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    String columnIndex = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (columnHeader.Contains("Date"))
                        {
                            columnIndex = "column3";
                        }
                        else
                        {
                            columnIndex = "column4";
                        }

                        if (dt.Rows[i]["Column1"].ToString() == "" && dt.Rows[i]["Column2"].ToString() == "" && dt.Rows[i][columnIndex].ToString() == "")
                        {
                        }
                        else
                        {

                            if (!columnHeader.Contains("Date"))
                            {

                                TextBox chequeno = (TextBox)gridPDCChques.Rows[rowIndex].Cells[0].FindControl("txtpdcchequeno");
                                DropDownList bankname = (DropDownList)gridPDCChques.Rows[rowIndex].Cells[2].FindControl("ddlpdcbank");
                                chequeno.Text = dt.Rows[i]["Column1"].ToString();
                                bankname.SelectedValue = dt.Rows[i]["Column3"].ToString();
                            }
                            else
                            {
                                DropDownList ddlCategory = (DropDownList)GridView1.Rows[rowIndex].Cells[2].FindControl("ddlCategory");
                                ddlCategory.SelectedValue = dt.Rows[i]["Column2"].ToString();
                            }


                            // DropDownList bankname = (DropDownList)gridPDCChques.Rows[rowIndex].Cells[2].FindControl("ddlCategory");
                        }


                        if (!columnHeader.Contains("Date"))
                        {
                            TextBox amount = (TextBox)gridPDCChques.Rows[rowIndex].Cells[3].FindControl("txtpdcamount");
                            TextBox chqdate = (TextBox)gridPDCChques.Rows[rowIndex].Cells[1].FindControl("txtpdcChqDate");
                            chqdate.Text = dt.Rows[i]["Column2"].ToString();

                            amount.Text = dt.Rows[i]["Column4"].ToString();
                            rowIndex++;
                        }
                        else
                        {
                            TextBox amount = (TextBox)GridView1.Rows[rowIndex].Cells[2].FindControl("txtpdcamount");
                            TextBox chqdate = (TextBox)GridView1.Rows[rowIndex].Cells[0].FindControl("txtpdcChqDate");
                            chqdate.Text = dt.Rows[i]["Column1"].ToString();

                            amount.Text = dt.Rows[i]["Column3"].ToString();
                            rowIndex++;
                        }
                    }



                }
            }
        }


    }
}