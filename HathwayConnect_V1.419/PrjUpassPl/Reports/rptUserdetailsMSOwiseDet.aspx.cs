using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassDAL.Helper;
using System.Data;
using System.Collections;
using PrjUpassPl.Helper;
using PrjUpassBLL.Reports;
using System.IO;

namespace PrjUpassPl.Reports
{
    public partial class rptUserdetailsMSOwiseDet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "User Details Report";
            DateTime dtime = DateTime.Now;
            if (!IsPostBack)
            {
                Session["pagenos"] = "1";
                Session["RightsKey"] = null;
                grdLcodet.PageIndex = 0;
                //setting page heading
                
                BindData();
            }

        }
        protected void BindData()
        {
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
            string lcoid = Convert.ToString(Session["Lcoid"]);
            cls_Business_rptMSOwiseUserdetails objTran = new cls_Business_rptMSOwiseUserdetails();
            DataTable dt = objTran.GetMSOdet(username,catid,operator_id);

            if (dt.Rows.Count == 0)
            {
                grdLcodet.Visible = false;
                lblSearchMsg.Text = "No data found";
                btngrnExel.Visible = false;
            }
            else
            {
                btngrnExel.Visible = true;
                grdLcodet.Visible = true;
                btngrnExel.Visible = true;
                ViewState["dt"] = dt;
                lblSearchMsg.Text = "";
                grdLcodet.DataSource = dt;
                grdLcodet.DataBind();

            }
        }

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
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

            cls_Business_rptLcowiseUserdetails objTran = new cls_Business_rptLcowiseUserdetails();
            DataTable dt = (DataTable)ViewState["dt"];

            if (dt.Rows.Count != 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "UserDetails_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9) + "User Id" + Convert.ToChar(9) + "User Name" + Convert.ToChar(9)
                        + "First Name" + Convert.ToChar(9) + "Middle Name" + Convert.ToChar(9) + "Last Name" + Convert.ToChar(9) + "Address" + Convert.ToChar(9)
                        + "Code" + Convert.ToChar(9) + "Brmpoid" + Convert.ToChar(9) + "state" + Convert.ToChar(9) + "City" + Convert.ToChar(9)
                        + "Email" + Convert.ToChar(9) + "Mobile Number" + Convert.ToChar(9) + "Account number" + Convert.ToChar(9) + "Inserted By" + Convert.ToChar(9)
                        + "Date" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9) + "'" + dt.Rows[i]["username"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["userowner"].ToString() + Convert.ToChar(9) + dt.Rows[i]["fname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["mname"].ToString() + Convert.ToChar(9) + dt.Rows[i]["lname"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["addr"].ToString() + Convert.ToChar(9) + dt.Rows[i]["code"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["brmpoid"].ToString() + Convert.ToChar(9) + dt.Rows[i]["ststeid"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["cityid"].ToString() + Convert.ToChar(9) + dt.Rows[i]["email"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["mobno"].ToString() + Convert.ToChar(9) + dt.Rows[i]["accno"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["insby"].ToString() + Convert.ToChar(9) + dt.Rows[i]["insdt"].ToString() + Convert.ToChar(9);
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
                Response.Redirect("../MyExcelFile/" + "UserDetails_" + datetime + ".xls");
            }

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
        }

        protected void grdLcodet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLcodet.PageIndex = e.NewPageIndex;
            BindData();
        }
    }
}