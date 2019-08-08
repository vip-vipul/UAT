using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Transaction
{
    public partial class FrmSTBForeClosureMst_LCO : System.Web.UI.Page
    {
        Cls_Business_Warehouse obj = new Cls_Business_Warehouse();
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "STB Foreclosure";

            if (!IsPostBack)
            {

                filldetail();
            }
        }

        public void filldetail()
        {
            if (Session["Receiptno"] != null)
            {
                txtSearch.Text = Session["Receiptno"].ToString();
                DataTable Dtreceiptno = obj.GetReceiptData(Session["username"].ToString(), Session["Receiptno"].ToString());

                if (Dtreceiptno.Rows.Count > 0)
                {

                    if (Session["Receiptno"].ToString().ToUpper().Contains("SPSN") || Session["Receiptno"].ToString().ToUpper().Contains("SPSR"))
                    {
                        lbldislcocode.Text = "LCO Code";
                        lbldislconame.Text = "LCO Name";
                    }
                    else
                    {
                        lbldislcocode.Text = "Account Code";
                        lbldislconame.Text = "VC No";
                    }
                    lbllcocode.Text = Dtreceiptno.Rows[0]["code"].ToString();
                    lbllconame.Text = Dtreceiptno.Rows[0]["name"].ToString();
                    lblscheme.Text = Dtreceiptno.Rows[0]["scheme"].ToString();
                    lblstbcount.Text = Dtreceiptno.Rows[0]["stbcount"].ToString();
                    lblpendingcount.Text = Dtreceiptno.Rows[0]["pendingcount"].ToString();
                    lblboxtype.Text = Dtreceiptno.Rows[0]["boxtype"].ToString();
                    lblRateSTB.Text = Dtreceiptno.Rows[0]["STBRate"].ToString();
                    lblDiscSTB.Text = Dtreceiptno.Rows[0]["STBDiscount"].ToString();
                    lblNetSTB.Text = Dtreceiptno.Rows[0]["STBNet"].ToString();
                    lblRateLCO.Text = Dtreceiptno.Rows[0]["LCORate"].ToString();
                    lblDiscLCO.Text = Dtreceiptno.Rows[0]["LCODiscount"].ToString();
                    lblNetLCO.Text = Dtreceiptno.Rows[0]["LCONet"].ToString();
                    lblTotalNet.Text = Dtreceiptno.Rows[0]["TotalNet"].ToString();

                    if (Dtreceiptno.Rows[0]["paymode"].ToString() == "C")
                    {
                        lbltype.Text = "Cash";
                    }
                    else if (Dtreceiptno.Rows[0]["paymode"].ToString() == "Q")
                    {
                        lbltype.Text = "Cheque";
                    }
                    else if (Dtreceiptno.Rows[0]["paymode"].ToString() == "D")
                    {
                        lbltype.Text = "DD";
                    }
                    else if (Dtreceiptno.Rows[0]["paymode"].ToString() == "N")
                    {
                        lbltype.Text = "NEFT";
                    }
                    else
                    {
                        lbltype.Text = "MPOS";
                    }

                    lblcashier.Text = Dtreceiptno.Rows[0]["cashier"].ToString();
                    Session["GridTitle"] = Dtreceiptno.Rows[0]["type"].ToString();

                    if (Dtreceiptno.Rows[0]["type"].ToString() == "O")
                    {
                        Session["GridTitle"] = "Other";
                    }

                    ViewState["TransId"] = Dtreceiptno.Rows[0]["transid"].ToString();


                    divdet.Visible = true;


                    lblTotalCount.Text = Dtreceiptno.Rows[0]["stbcount"].ToString();//lblstbcount.Text;
                    lblBalanceCount.Text = Dtreceiptno.Rows[0]["pendingcount"].ToString(); // lblpendingcount.Text;
                    lblforeclosure.Text = Dtreceiptno.Rows[0]["Foreclosre"].ToString();
                    lblfaulty.Text = Dtreceiptno.Rows[0]["faulty"].ToString();
                    string foreclosure = "0";
                    string faulty = "0";
                    string BalanceCount = "0";
                    if (lblforeclosure.Text == "")
                    {
                        lblforeclosure.Text = "0";
                    }
                    else
                    {
                        foreclosure = lblforeclosure.Text;
                    }

                    if (lblfaulty.Text == "")
                    {
                        lblfaulty.Text = "0";
                    }
                    else
                    {
                        faulty = lblfaulty.Text;
                    }

                    if (lblBalanceCount.Text == "")
                    {
                        lblBalanceCount.Text = "0";
                    }
                    else
                    {
                        BalanceCount = lblBalanceCount.Text;
                    }
                    lnkAllocated.Text = Convert.ToString(Convert.ToDouble(lblTotalCount.Text) - Convert.ToDouble(foreclosure) - Convert.ToDouble(faulty) - Convert.ToDouble(BalanceCount));
                }

                else
                {
                    msgboxstr("Transaction already done");
                    Response.Redirect("../Transaction/frmSTBForeClosureList_LCO.aspx");
                    return;
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() != "")
            {

                String result = "";

            }
        }

        public void msgboxstr(string message)
        {
            lblPopupResponse.Text = message;
            popMsg.Show();
        }

        protected void gridSTB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Text = Session["GridTitle"].ToString();

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtnoofbox.Text == "")
            {
                msgboxstr("Please enter no. of boxes");
                return;
            }

            if (ddlreason.SelectedValue == "")
            {
                msgboxstr("Please enter reason");
                return;
            }
            popMsgBox.Show();
        }

        protected void btncnfmBlck_Click(object sender, EventArgs e)
        {

            Int32 STBCount = Convert.ToInt32(txtnoofbox.Text);
            Int32 cnt = 0;
            Hashtable ht = new Hashtable();

            ht.Add("TransId", ViewState["TransId"].ToString());
            ht.Add("Userid", Session["username"].ToString());
            ht.Add("STBCount", STBCount);
            ht.Add("Reason", ddlreason.SelectedItem.Text);
            if (txtSearch.Text.ToUpper().Contains("SPSR"))
            {
                ht.Add("Receipttype", "SPSR");
            }

            else if (txtSearch.Text.ToUpper().Contains("SPSN"))
            {
                ht.Add("Receipttype", "SPSN");
            }
            else if (txtSearch.Text.ToUpper().Contains("PPSN"))
            {
                ht.Add("Receipttype", "PPSN");
            }
            else
            {
                ht.Add("Receipttype", "PPSR");
            }
            try
            {

                string result = obj.InsertForeClosure("aoup_lcopre_pis_foreclosu_ins", ht);

                string[] Getresponse = result.Split('$');
                if (Getresponse[0] == "9999")
                {
                    ViewState["Result"] = "9999";
                    msgboxstr("Entery Added successfully");

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

        protected void btnClodeMsg1_click(object sender, EventArgs e)
        {
            if (ViewState["Result"] == null)
            {
                lblPopupResponse.Text = "";
                popMsg.Hide();
                return;
            }
            if (ViewState["Result"].ToString() == "9999")
            {
                Response.Redirect("~/Transaction/FrmSTBForeClosureList_LCO.aspx", true);
            }
            else
            {
                lblPopupResponse.Text = "";
                popMsg.Hide();
            }
        }
        public void GenerateReport(DataTable dt, string pdfname)
        {
        }
        protected void lnkReceiptno_click(object sender, EventArgs e)
        {
            string receiptno = txtSearch.Text;
            Session["Receiptno"] = receiptno;

            String TransId = ViewState["TransId"].ToString();

            DataTable dt = obj.GetSTBList(Session["username"].ToString(), TransId);

            grdSTBDetails.DataSource = dt;
            grdSTBDetails.DataBind();
            popGridBox.Show();
        }
    }
}