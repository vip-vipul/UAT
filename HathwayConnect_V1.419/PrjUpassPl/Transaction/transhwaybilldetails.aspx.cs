using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjUpassPl.Transaction
{
    public partial class transhwaybilldetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Master.PageHeading = "Account Invoice Details";
            Session["RightsKey"] = "N";




        }

        public String sendcommandforErrorDetails(string acc_no)
        {
            try
            {

                string source = "";

                source = "http://202.88.130.21:2462/HathwayInvoiceDate/getDates?accountNo=" + acc_no;


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

        protected void ddlbillno_SelectedIndexChanged(object sender, EventArgs e)
        {

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

                /*Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + source);
                Response.AddHeader("Connection", "Keep-Alive");
                Response.OutputStream.Write(buffer, 0, bytesRead);
                Response.Flush();*/


                /*FileStream fs = null;
                fs = File.Open(source1, FileMode.Open);
                byte[] btFile = new byte[fs.Length];
                fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                Response.AddHeader("Content-disposition", "attachment;filename=source1");
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(btFile);
                Response.End();*/

                ScriptManager.RegisterStartupScript(this.Page, GetType(), "download", "DownloadFile();", true);

                // We can do what ever we want, You can redirect to another page or anything you want
                //Here I am changing the ddlSelectedAction selected index to 0

                return actualRes;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string blusername = SecurityValidation.chkData("N", txtaccno.Text);
            if (blusername.Length > 0)
            {
                lblmessage.Text = blusername;

                return;
            }

            lblbillno.Visible = true;
            Label2.Visible = true;
            ddlbillno.Visible = true;
            btnpdf.Visible = true;

            string acc_no = txtaccno.Text.ToString().Trim();

            string url = sendcommandforErrorDetails(acc_no);
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

            ddlbillno.Items.Insert(0, "Select Billno");

            //List<string> values = new List<string>();
            //List<string> values1 = new List<string>();
            //foreach (string a in array)
            //{
            //    foreach (string b in array)
            //    {

            //        string s = "HW_BMP";
            //        string ori = a.Remove(0, 8);
            //        string q = "billDate";
            //        string date1 = b.Substring(9, 11);

            //        if (ori.Contains(s))
            //        {
            //            string value = ori+ "(" + date1 + ")";
            //            values.Add(value);
            //        }

            //    }
            //}

            // ddlbillno.DataSource = values;
            //ddlbillno.DataBind();




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
                    // Server.Transfer("http://202.88.130.21:2462/HathwayInvoice/getInvoice?billID=" + billno);
                    //Billno(billno);
                }
            }
        }
    }
}