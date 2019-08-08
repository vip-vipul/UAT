using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Configuration;
using System.Data;

namespace PrjUpassPl.Transaction
{
    public partial class transhwaylcoinvoicedetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "LCO Invoice Details";
            Session["RightsKey"] = "N";

            string username;
          //  txtlcocode.Text = username = Convert.ToString(Session["username"]);

            if (!IsPostBack)
            {
                FillLcoDetails();
            }
        }

        protected void FillLcoDetails()
        {

            lblmsg.Text = "";
            string str = "";
         //   Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
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
                    return;
                }
                DataTable tbllco = GetResult(str);

                if (tbllco.Rows.Count > 0)
                {
                    ddllco.DataTextField = "name";
                    ddllco.DataValueField = "lcocode";

                    ddllco.DataSource = tbllco;
                    ddllco.DataBind();
                    //if (category_id == "11")
                    //{
                    //    ddllco.Items.Insert(0, new ListItem("Select LCO", "0"));
                    //}
                }
                else
                {
                    lblmsg.Text = "No LCO Details Found";
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

        public String sendcommandforErrorDetails(string lco_code)
        {
            try
            {

                string source = "";

                source = "http://202.88.130.21:2462/HathwayInvoiceDate/getDates?accountNo=" + lco_code;


                HttpWebRequest requestToSender = (HttpWebRequest)WebRequest.Create(source);
                HttpWebResponse responseFromSender = (HttpWebResponse)requestToSender.GetResponse();

                StreamReader loResponseStream = new StreamReader(responseFromSender.GetResponseStream());
                string fullResponse = loResponseStream.ReadToEnd();

                int lastindex = fullResponse.IndexOf("\n");
                string actualRes = fullResponse.Substring(0, lastindex);

                return actualRes;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public String Billno(string billno)
        {
            try
            {

                string source1 = "";

                source1 = "http://202.88.130.21:2462/HathwayInvoice/getInvoice?billID=" + billno;


                HttpWebRequest requestToSender = (HttpWebRequest)WebRequest.Create(source1);
                HttpWebResponse responseFromSender = (HttpWebResponse)requestToSender.GetResponse();

                StreamReader loResponseStream = new StreamReader(responseFromSender.GetResponseStream());
                string fullResponse = loResponseStream.ReadToEnd();

                int lastindex = fullResponse.IndexOf("\n");
                string actualRes = fullResponse.Substring(0, lastindex);

               

                ScriptManager.RegisterStartupScript(this.Page, GetType(), "download", "DownloadFile();", true);
                return actualRes;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblbillno.Visible = true;
            Label2.Visible = true;
            ddlbillno.Visible = true;
            btnpdf.Visible = true;

            string username;
           // txtlcocode.Text = username = Convert.ToString(Session["username"]);
            string lco_code = ddllco.SelectedValue;//txtlcocode.Text.ToString().Trim();
            
            string url = sendcommandforErrorDetails(lco_code);
            // string url = @"{""BillDetails"":[{""billID"":""HW_BMP-100001989"",""billDate"":""01-Sep-2015""},{""billID"":""HW_BMP-100002089"",""billDate"":""01-Nov-2015""},{""billID"":""HW_BMP-100003089"",""billDate"":""16-Dec-2015""}]}";
            var queryString = url.Remove(0, 16);
            queryString = queryString.Substring(0, queryString.Length - 2);
            var temp = queryString.Replace("\"", string.Empty);

            string[] array = temp.Split('}');
            string str = ""; ;
            List<string> values = new List<string>();
            ddlbillno.Items.Clear();
            for (int i = 0; i <= array.Length - 1; i++)
            {
                if (array[i] != "")
                {
                    string strsubchar = array[i].Substring(0, 2);
                    if (strsubchar == "{b")
                    {
                        str = array[i].Replace("{billID:", "");
                        str = str.Replace("billDate:", "");
                        str = str.Replace(",", "(");
                        str = str + ")";

                    }
                    else if (strsubchar == ",{")
                    {

                        str = array[i].Replace(",{billID:", "");
                        str = str.Replace("billDate:", "");
                        str = str.Replace(",", "(");
                        str = str + ")";

                    }
                    
                    values.Add(str);
                }
            }

            ddlbillno.DataSource = values;
            ddlbillno.DataBind();

            if (ddlbillno.SelectedValue == "")
            {
                lblmsg.Text = "No data found";
                Trbillno.Visible = false;
                Tr1.Visible = false;
            }
            if (ddllco.SelectedValue == "0")
            {
                lblmsg.Text = "Please select LCO";
            }
            else { lblmsg.Text = ""; }

            ddlbillno.Items.Insert(0, "Select Billno");

        }

        protected void btnpdf_Click(object sender, EventArgs e)
        {
            string selectbillitem = ddlbillno.SelectedValue;
            string[] billdets = selectbillitem.Split('(');
            string billno = billdets[0].ToString();

            if (ddlbillno.SelectedIndex != 0)
            {
                if (billno != "")
                {
                    Response.Redirect("http://202.88.130.21:2462/HathwayInvoice/getInvoice?billID=" + billno);                    
                }
            }
        }

    }
}