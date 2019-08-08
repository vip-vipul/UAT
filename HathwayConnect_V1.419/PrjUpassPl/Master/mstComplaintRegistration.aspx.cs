using System;
using System.Data;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using PrjUpassDAL.Helper;
using PrjUpassBLL.Master;

namespace PrjUpassPl.Master
{
    public partial class mstComplaintRegistration : System.Web.UI.Page
    {
        string oper_id = "";
        string username = "";
        string catid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Complaint Registration";
            DateTime dtime = DateTime.Now;
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {
                oper_id = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                Session["RightsKey"] = "N";
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                lblSearchResponse.Text = "";
                //CRMClassLib.MstMethods.Dropdown.Fill(DdlComplaintType, "aomcm_complainttype_def", "var_complnttyp_type", "num_complnttyp_id", "num_corp_id= " + Session["CORPID"] + " and var_complnttyp_activeflag='Y'", "");
                DataSet ds = Cls_Helper.fnGetdataset("select var_complnttyp_type,num_complnttyp_id from crm.aomcm_complainttype_def where var_complnttyp_activeflag='Y'");
                ddlcomplainttype.DataSource = ds;
                ddlcomplainttype.DataTextField = "var_complnttyp_type";
                ddlcomplainttype.DataValueField = "num_complnttyp_id";
                ddlcomplainttype.DataBind();
                ds.Dispose();
                ddlcomplainttype.Items.Insert(0, "--- Select ---");
                if (RadSearchby.SelectedValue == "1")
                {
                    divdet.Visible = false;
                    divcustomer.Visible = true;
                }
                else
                {
                    divcustomer.Visible = false;
                    divdet.Visible = false;
                }
                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");

                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                RadSearchby_SelectedIndexChanged(null, null);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (txtname.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("T", txtname.Text);
                if (valid != "")
                {
                    lblmsg.Text = valid + " at Name";
                    return;
                }
            }


            if (txtaddress.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("T", txtaddress.Text);
                if (valid != "")
                {
                    lblmsg.Text = valid + " at Address";
                    return;
                }
            }

            if (txtmobno.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("N", txtmobno.Text);
                if (valid != "")
                {
                    lblmsg.Text = valid + " at Mobile Number";
                    return;
                }
            }

            if (txtemailid.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("T", txtemailid.Text);
                if (valid != "")
                {
                    lblmsg.Text = valid + " at EmailID";
                    return;
                }
            }

            if (txtcompanyname.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("T", txtcompanyname.Text);
                if (valid != "")
                {
                    lblmsg.Text = valid + " at Company Name";
                    return;
                }
            }

            if (txtAlternateno.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("N", txtAlternateno.Text);
                if (valid != "")
                {
                    lblmsg.Text = valid + " at Alternate Number";
                    return;
                }
            }

            if (txtcomplaint.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("T", txtcomplaint.Text);
                if (valid != "")
                {
                    lblmsg.Text = valid + " at Complaint";
                    return;
                }
            }

            string expresion;
            if (txtemailid.Text == "" || txtemailid.Text == null)
            {
                //MessageAlert("Please Enter Proper Customer Email", "");
                txtemailid.Focus();
                return;
            }
            else
            {
                expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(txtemailid.Text, expresion))
                {
                    if (Regex.Replace(txtemailid.Text, expresion, string.Empty).Length == 0)
                    {
                        // ch.Email = txtemailid.Text.Trim();
                    }
                    else
                    {
                        //MessageAlert("Enter valid email", "");
                        txtemailid.Focus();
                        return;
                    }
                }
                else
                {
                    //MessageAlert("Enter valid email", "");
                    txtemailid.Focus();
                    return;
                }
            }
            if (txtcomplaint.Text != "" && txtcomplaint.Text.Trim() != "")
            {
                if (txtcomplaint.Text.Length > 300)
                {
                    // MessageAlert("Complaint Should Not Be Grather Than 300 Character", "");
                    txtcomplaint.Focus();
                    return;
                }
                else
                {
                    //ch.Complaint = TxtComplaint.Text;
                }
            }
            else
            {
                //MessageAlert("Please Enter Complaint", "");
                txtcomplaint.Focus();
                return;
            }


            if (ddlcomplainttype.SelectedValue == "0" || ddlcomplainttype.SelectedValue == "")
            {
                //MessageAlert("Please Select Complaint Source", "");
                ddlcomplainttype.Focus();
                return;
            }
            else
            {
                // ch.ComplaintSource = ddlcomplainttype.SelectedValue.ToString();
            }
            Hashtable ht = new Hashtable();
            ht.Add("in_UserId", Session["username"].ToString());
            ht.Add("in_CompltMasID", "1");
            ht.Add("in_ComplNo", "1");
            ht.Add("in_CmpltID", "1");
            ht.Add("in_DeptID", "1");
            ht.Add("in_AuthorizeUserId", "0");
            ht.Add("in_ComplTypeid", ddlcomplainttype.SelectedValue);
            ht.Add("in_ComplSubTyp", "0");
            ht.Add("in_CustomeNM", txtname.Text);
            ht.Add("in_CustCntNo", txtmobno.Text);
            ht.Add("in_Complaint", txtcomplaint.Text);
            ht.Add("in_CurrentStst", "PA");
            ht.Add("in_cmplRegDate", DateTime.Now);
            ht.Add("in_CmplRefId", "0");
            ht.Add("in_ComEntryType", "W");
            ht.Add("in_SmsCmplID", "0");
            ht.Add("in_Mode", "1");
            ht.Add("in_Address", txtaddress.Text);
            ht.Add("in_ComplaintSource", "HC");
            ht.Add("in_Email", txtemailid.Text);
            ht.Add("in_Location", "");
            ht.Add("in_lcocode", Session["username"].ToString());
            //if (RadSearchby.SelectedValue != "0")
            //{
            //    ht.Add("in_cust_account_no", txtAccCustomer.Text);
            //    ht.Add("in_complnt_flag", "C");
            //}
            //else
            //{
            ht.Add("in_cust_account_no", "");
            ht.Add("in_complnt_flag", "S");
            //}
            ht.Add("in_callername", "");
            ht.Add("in_callerno", "");
            ht.Add("in_companyName", txtcompanyname.Text);
            ht.Add("in_Alternateno", txtAlternateno.Text);
            Cls_Business_mstComplaintRegistration obj = new Cls_Business_mstComplaintRegistration();
            string strresponse = obj.InsertComplaint(ht, Session["username"].ToString());
            string[] str = strresponse.Split('$');
            if (str[0] != "9999")
                lblmsg.Text = str[1];
            else
                lblmsg.Text = str[1];
            clear();
        }
        public void clear()
        {
            txtaddress.Text = "";
            txtcomplaint.Text = "";
            txtemailid.Text = "";
            txtmobno.Text = "";
            txtname.Text = "";
            //ddlcomplainttype.SelectedValue = "0";
            divdet.Visible = false;
        }

        #region "Button SEARCH"
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtsearchpara.Text.Length > 0)
            {
                string valid = SecurityValidation.chkData("N", txtsearchpara.Text);

                if (valid == "")
                {

                    string from = txtFrom.Text;
                    string to = txtTo.Text;
                    DateTime fromDt;
                    DateTime toDt;
                    if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
                    {
                        fromDt = new DateTime();
                        toDt = new DateTime();
                        fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                        toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                        // DateTime fromDt;
                        // DateTime toDt;
                        Double dateDiff = 0;
                        if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to))
                        {
                            fromDt = new DateTime();
                            toDt = new DateTime();
                            fromDt = DateTime.ParseExact(from, "dd-MMM-yyyy", null);
                            toDt = DateTime.ParseExact(to, "dd-MMM-yyyy", null);
                            dateDiff = (toDt - fromDt).TotalDays;
                        }

                        if (toDt.CompareTo(fromDt) < 0)
                        {
                            lblmsg.Text = "To date must be later than From date";
                            GridCompList.Visible = false;
                            lblmsg.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else if (Convert.ToDateTime(txtFrom.Text.ToString()) > DateTime.Now.Date)
                        {
                            lblmsg.Text = "You can not select date greater than current date!";
                            return;
                        }
                        else if (Convert.ToDateTime(txtTo.Text.ToString()) > DateTime.Now.Date)
                        {
                            lblmsg.Text = "You can not select date greater than current date!";
                            return;
                        }
                        else
                        {
                            lblmsg.Text = "";
                        }
                    }
                    BindGrid();

                }
                else
                {
                    lblmsg.Text = valid;
                    return;
                }

            }

        }
        #endregion

        #region "BindGrid"
        public void BindGrid()
        {
            Int32 DepId = Convert.ToInt32(Session["UserDepid"]);

            String Query = " select cmpno ,deptnm ,depid ,source,authby,cmptype, cmpsubtype, custnm, custno, cmpdesc,cmpstatus,srvst,TO_CHAR(regdt,'dd-MM-yyyy') regdt, assgnuser, userremark, remarkdate,CHECKEDSTATUS,LCOCODE,companyname,callername,callerno,flag,Alternateno from crm.mview_rpt_complaint_details ";
            Query += " where checkedstatus='Y' and trunc(regdt) >= TO_DATE('" + txtFrom.Text + "','dd-MM-yyyy') and trunc(regdt) <= TO_DATE('" + txtTo.Text + "','dd-MM-yyyy')";

            if (txtsearchpara.Text != "" && txtsearchpara.Text != null)
            {
                Query += " and cmpno = '" + txtsearchpara.Text + "'";
            }

            Query += " and lcocode='" + Session["username"].ToString() + "' order by INSDATE";


            DataTable tblComplaintDetails = new DataTable();
            Cls_Helper obj = new Cls_Helper();
            tblComplaintDetails = obj.GetDataTable(Query);

            if (tblComplaintDetails.Rows.Count > 0)
            {
                GridCompList.DataSource = tblComplaintDetails;
                GridCompList.DataBind();
                ViewState["Tbldetails"] = tblComplaintDetails;
            }
            else
            {
                lblmsg.Text = "No records found.";
                // MessageAlert("No records found.", "");//,"../Reports/RptCompDetails.aspx"
            }
        }
        #endregion



        public String callAPI(string Request, string request_code)
        {
            try
            {
                string fromSender = string.Empty;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(Request);
                HttpWebRequest myRequest = null;
                myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                //myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.21/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
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
        }

        protected void RadSearchby_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadSearchby.SelectedValue == "0")
            {
                divcustomer.Visible = false;
                divdet.Visible = true;
                griddiv.Visible = false;
                bindlcodet();
            }
            else if (RadSearchby.SelectedValue == "1")
            {
                divcustomer.Visible = true;
                divdet.Visible = false;
                griddiv.Visible = true;
            }
            GridCompList.DataSource = null;
            GridCompList.DataBind();
            lblmsg.Text = "";
        }

        public void bindlcodet()
        {
            lblSearchResponse.Text = "";
            Cls_Business_mstComplaintRegistration obj = new Cls_Business_mstComplaintRegistration();
            string operator_id = "";
            string category_id = "";
            if (Session["operator_id"] != null && Session["category"] != null)
            {
                operator_id = Convert.ToString(Session["operator_id"]);
                category_id = Convert.ToString(Session["category"]);
            }
            string[] responseStr = obj.getLcodetails(username, username.Trim(), "0", operator_id, category_id);
            if (responseStr.Length != 0)
            {
                txtname.Text = responseStr[1].Trim();
                txtaddress.Text = responseStr[2].Trim();
                txtmobno.Text = responseStr[3].Trim();
                txtemailid.Text = responseStr[4].Trim();
                txtcompanyname.Text = responseStr[6].Trim();
                divdet.Visible = true;
            }
            else
            {
                return;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

    }
}