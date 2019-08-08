using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassDAL.Helper;
using System.Data;
using PrjUpassBLL.Reports;
using System.Collections;
using System.IO;
namespace PrjUpassPl.Reports
{
    public partial class rptSWAP_FaultyDetails : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        string user_brmpoid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["operator_id"] != null && Session["username"] != null && Session["user_brmpoid"] != null)
                {
                    user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
                Session["RightsKey"] = "N";
                grdNewActivation.PageIndex = 0;
                Master.PageHeading = "STB SWAP Report";
                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");
                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                
            }
        }
        private Hashtable getParamsData()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;

            Session["fromdt"] = txtFrom.Text;
            Session["todt"] = txtTo.Text;
            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("from", from);
            htSearchParams.Add("to", to);
            htSearchParams.Add("OperID", Session["operator_id"].ToString());
            return htSearchParams;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
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
                if (toDt.CompareTo(fromDt) < 0)
                {
                    lblSearchMsg.Text = "To date must be later than From date";
                    grdNewActivation.Visible = false;
                    lblSearchMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else if (Convert.ToDateTime(txtFrom.Text.ToString()) > DateTime.Now.Date)
                {
                    lblSearchMsg.Text = "You can not select date greater than current date!";
                    return;
                }
                else if (Convert.ToDateTime(txtTo.Text.ToString()) > DateTime.Now.Date)
                {
                    lblSearchMsg.Text = "You can not select date greater than current date!";
                    return;
                }
                else
                {
                    lblSearchMsg.Text = "";
                    grdNewActivation.Visible = true;
                }
            }
            Binddata();
        }

        public void Binddata()
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Hashtable htAddPlanParams = getParamsData();
            Hashtable htResponse = new Hashtable();
            Cls_Business_rptNew_Activation objTran = new Cls_Business_rptNew_Activation();
            htResponse = objTran.GetSTBSWAPDet(htAddPlanParams, username);
            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            lblSearchParams.Text = Server.HtmlDecode("<b style='color:#094791;'></b><b>Transaction From : </b>" + from + "<b>Transaction To : </b>" + to);


            if (dt.Rows.Count == 0)
            {
                btngrnExel.Visible = false;
                grdNewActivation.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                grdNewActivation.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdNewActivation.DataSource = dt;
                grdNewActivation.DataBind();
                DivRoot.Style.Add("display", "block");

            }
        }
        protected void btngrnExel_Click(object sender, EventArgs e)
        {
            Hashtable htAddPlanParams = getParamsData();

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            DataTable dt = null; //check for exception
            if (ViewState["searched_trans"] != null)
            {
                dt = (DataTable)ViewState["searched_trans"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }

            if (dt.Rows.Count != 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "New_Activation_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)
                        + "Account No" + Convert.ToChar(9)
                        + "Main STB" + Convert.ToChar(9)
                        + "Child STB" + Convert.ToChar(9)
                        + "Old STB" + Convert.ToChar(9)
                        + "New STB" + Convert.ToChar(9)
                        + "Reason" + Convert.ToChar(9)
                        + "SWAP Type" + Convert.ToChar(9)
                        + "Customer Name" + Convert.ToChar(9)
                        + "Customer Address" + Convert.ToChar(9)
                        + "Mobile No" + Convert.ToChar(9)
                        + "Email ID" + Convert.ToChar(9)
                        + "Inserted By" + Convert.ToChar(9)
                        + "Inserted Date" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9) + "'";
                            strrow +=
                                 dt.Rows[i]["AccountNo"].ToString() + Convert.ToChar(9)
                                 + dt.Rows[i]["MainStb"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["ChildSTB"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["OldStb"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["newStb"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["REASON"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["SWAP_TYPE"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["CUSTOMERNAME"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["CUSTOMERADD"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["MOBILENO"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["EMAILID"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["INSBY"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["INSDATE"].ToString() + Convert.ToChar(9);


                            sw.WriteLine(strrow);

                        }
                    }
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception ex)
                {
                    sw.Flush();
                    sw.Close();
                    Response.Write("Error : " + ex.Message.Trim());
                    return;
                }
                Response.Redirect("../MyExcelFile/" + "New_Activation_" + datetime + ".xls");
            }
        }

        protected void grdactdact_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdNewActivation.PageIndex = e.NewPageIndex;
            Hashtable htAddPlanParams = getParamsData();

            string username, catid, operator_id;
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                catid = Convert.ToString(Session["category"]);
                operator_id = Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            Cls_Business_rptNew_Activation objTran = new Cls_Business_rptNew_Activation();
            Hashtable htResponse = objTran.GetSTBSWAPDet(htAddPlanParams, username);

            DataTable dt = null; //check for exception
            if (htResponse["htResponse"] != null)
            {
                dt = (DataTable)htResponse["htResponse"];
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }




            if (dt.Rows.Count == 0)
            {
                btngrnExel.Visible = false;
                grdNewActivation.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                grdNewActivation.Visible = true;
                lblSearchMsg.Text = "";
                grdNewActivation.DataSource = dt;
                grdNewActivation.DataBind();
            }
        }

    }
}