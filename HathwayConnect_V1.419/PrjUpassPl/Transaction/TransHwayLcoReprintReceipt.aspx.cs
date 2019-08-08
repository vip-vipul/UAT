using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OracleClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using PrjUpassBLL.Transaction;
using PrjUpassDAL.Helper;
using PrjUpassDAL.Authentication;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayLcoReprintReceipt : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        string operid;
        string username;
        string catid;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (rbtnsearch.SelectedValue == "0")
            //{
            //    type = "0";
            //}
            //else
            //{
            //    type = "1";
            //}
            if (!IsPostBack)
            {
                if (Session["operator_id"] != null)
                {
                    Session["RightsKey"] = null;
                    operid = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    catid = Convert.ToString(Session["category"]);
                    reset();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
                //txtChequeBounceDate.Attributes.Add("readonly", "readonly");
            }
            //   btnSubmit.Attributes.Add("onclick", "javascript:return dovalid1()");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchOperators(string prefixText, int count, string contextKey)
        {
            string Str = prefixText.Trim();
            double Num;
            bool isNum = double.TryParse(Str, out Num);

            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            string str = "";
            string catid = "";
            string operid = "";
            if (HttpContext.Current.Session["category"] != null && HttpContext.Current.Session["operator_id"] != null)
            {
                catid = HttpContext.Current.Session["category"].ToString();
                operid = HttpContext.Current.Session["operator_id"].ToString();
            }
            if (contextKey == "0")
            {
                str = " SELECT a.receiptno";
                str += " FROM view_lcopre_payment_reprint a";
                str += " where upper(a.receiptno) Like upper('" + prefixText + "%')";
            }
            else if (contextKey == "1")
            {
                str = " SELECT distinct a.lcocode";
                str += " FROM view_lcopre_payment_reprint a";
                str += " where upper(a.lcocode) Like upper('" + prefixText + "%')";
            }
            if (catid == "2")
            {
                str += " and a.parentid='" + operid.ToString() + "'  ";
            }
            else if (catid == "5")
            {
                str += " and a.distid='" + operid.ToString() + "'  ";
            }
            else if (catid == "10")
            {
                str += " and a.hoid='" + operid.ToString() + "'  ";
            }
            else if (catid == "3")
            {
                str += " and a.operid='" + operid.ToString() + "'  ";
            }
            OracleCommand cmd = new OracleCommand(str, con);

            con.Open();

            List<string> Operators = new List<string>();
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string item = "";
                if (contextKey == "0")
                {
                    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["receiptno"].ToString(), dr["receiptno"].ToString());
                }
                else if (contextKey == "1")
                {
                    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                        dr["lcocode"].ToString(), dr["lcocode"].ToString());
                }
                Operators.Add(item);
            }
            //string[] prefixTextArray = Operators.ToArray<string>();
            con.Close();
            con.Dispose();
            return Operators;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        protected void reset()
        {
            lblmsg.Text = "";
            divdet.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtLCOSearch.Text.Trim() != "")
            {
                string customerId = Request.Form[hfCustomerId.UniqueID];
                string customerName = Request.Form[txtLCOSearch.UniqueID];
                Cls_BLL_TransHwayLcoReprintReceipt obj = new Cls_BLL_TransHwayLcoReprintReceipt();
                // string[] responseStr = obj.getLcodetails(username, txtLCOSearch.Text.Trim(), RadSearchby.SelectedValue, operid);
                string search_type = rbtnsearch.SelectedValue;
                string oper_id = "";
                string username = "";
                string cat_id = "";
                if (Session["operator_id"] != null && Session["username"] != null && Session["category"] != null)
                {
                    oper_id = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    cat_id = Convert.ToString(Session["category"]);
                }

                DataTable dt = obj.GetLcopaymentDetails(username, txtLCOSearch.Text.Trim(), cat_id, oper_id, search_type, "");
                if (dt.Rows.Count != 0)
                {
                    grdPaymentDet.DataSource = dt;
                    grdPaymentDet.DataBind();
                    divdet.Visible = true;
                }
                else
                {
                    if (rbtnsearch.SelectedValue.ToString() == "0")
                    {
                        msgbox("Invalid Receipt", txtLCOSearch);
                        return;
                    }
                    else
                    {
                        msgbox("Invalid Lco Code", txtLCOSearch);
                        return;
                    }
                }
            }
            else
            {
                if (rbtnsearch.SelectedValue.ToString() == "0")
                {
                    msgbox("Please Enter Receipt No.", txtLCOSearch);
                    return;
                }
                else if (rbtnsearch.SelectedValue.ToString() == "1")
                {
                    msgbox("Please Enter Lco Code", txtLCOSearch);
                    return;
                }
            }
        }

        public void msgbox(string message, Control ctrl)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + message + "');", true);
            ctrl.Focus();
        }

        protected void rbtnsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rbtnsearch.SelectedValue == "0")
            //{
            //    //tdBankName.Visible = false;
            //    type = "0";
            //}
            //else if (rbtnsearch.SelectedValue == "1")
            //{
            //    //tdBankName.Visible = true;
            //    type = "1";
            //}
        }

        protected void grdPaymentDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Reprint"))
            {
                try
                {
                    GridViewRow clickedRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    //Session["showall"] = null;
                    int rowindex = clickedRow.RowIndex;
                    ///////
                    Session["rcpt_pt_rcptno1"] = grdPaymentDet.Rows[rowindex].Cells[1].Text;
                    Session["rcpt_pt_date1"] = grdPaymentDet.Rows[rowindex].Cells[9].Text;
                    Session["rcpt_pt_cashiername"] = ((HiddenField)clickedRow.FindControl("hfname")).Value;
                    Session["rcpt_pt_address"] = ((HiddenField)clickedRow.FindControl("hfaddress")).Value;
                    Session["rcpt_pt_company"] = grdPaymentDet.Rows[rowindex].Cells[10].Text;
                    Session["rcpt_pt_lcocd1"] = ((HiddenField)clickedRow.FindControl("hflcocode")).Value;
                    Session["rcpt_pt_lconm1"] = grdPaymentDet.Rows[rowindex].Cells[2].Text;
                    //Session["cashier"] = Session["name"].ToString();

                    Session["rcpt_pt_amt1"] = ((HiddenField)clickedRow.FindControl("hfamt")).Value;
                    Session["rcpt_pt_paymode1"] = grdPaymentDet.Rows[rowindex].Cells[5].Text;
                    Session["rcpt_pt_cheqno1"] = grdPaymentDet.Rows[rowindex].Cells[7].Text;
                    Session["rcpt_pt_bnknm1"] = grdPaymentDet.Rows[rowindex].Cells[6].Text;
                    Session["rcpt_pt_premark1"] = ((HiddenField)clickedRow.FindControl("hfremrk")).Value;
                    //FileLogText(Session["username"].ToString(), "Reprint", Session["rcptno1"].ToString() + "," + Session["date1"].ToString() + "," +
                    //    Session["cashiername"].ToString() + "," + Session["address"].ToString() + "," + Session["company"].ToString() + "," + Session["lcocd1"].ToString() + "," +
                    //    Session["lconm1"].ToString() + "," + Session["amt1"].ToString() + "," + Session["paymode1"].ToString() + "," + Session["cheqno1"].ToString() + "," +
                    //    Session["bnknm1"].ToString() + "," + Session["premark1"].ToString(),"");


                }
                catch (Exception ex)
                {
                    Response.Redirect("../errorPage.aspx");
                }
                Response.Write("<script language='javascript'> window.open('../Transaction/rcptPaymentReceiptInvoice.aspx', 'Print_Receipt','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes,location=no,status=no');</script>");
            }
        }

        protected void grdPaymentDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (rbtnsearch.SelectedValue == "0")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string val = ((HiddenField)(e.Row.FindControl("hfisactive"))).Value;
                    if (val == "Y")
                    {
                        ((LinkButton)(e.Row.FindControl("lbReprint"))).Visible = true;
                        ((Label)(e.Row.FindControl("lblrev"))).Visible = false;
                    }
                    else
                    {
                        ((LinkButton)(e.Row.FindControl("lbReprint"))).Visible = false;
                        ((Label)(e.Row.FindControl("lblrev"))).Visible = true;
                    }
                }
            }
        }

        //public void FileLogText(String Str, String sender, String strRequest, String strResponse)
        //{
        //    string filename = DateTime.Now.ToString("dd-MMM-yyyy");
        //    StreamWriter sw = new StreamWriter(@"C:\temp\Logs\Hway\logdetails_" + filename + ".txt", true);
        //    try
        //    {
        //        sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
        //        sw.WriteLine(strRequest.Trim());
        //        sw.WriteLine(strResponse.Trim());
        //        sw.WriteLine("************************************************************************************************************************");
        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write("Error while writing logs : " + ex.Message.ToString());
        //    }
        //    finally
        //    {
        //        sw.Flush();
        //        sw.Close();
        //        sw.Dispose();
        //    }
        //}
    }
}