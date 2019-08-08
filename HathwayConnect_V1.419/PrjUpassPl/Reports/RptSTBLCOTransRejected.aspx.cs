using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using PrjUpassBLL.Master;
using System.IO;

namespace PrjUpassPl.Reports
{
    public partial class RptSTBLCOTransRejected : System.Web.UI.Page
    {
        string operator_id = "";
        string username = "";
        string user_id = "";
        string category = "";
        DateTime dtime = DateTime.Now;

        public void msgbox(string message, Control ctrl)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + message + "');", true);
            ctrl.Focus();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "STB Transfer Rejected List";
            Session["RightsKey"] = "N";
            if (!IsPostBack)
            {
                if (Session["operator_id"] != null)
                {
                    operator_id = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    user_id = Convert.ToString(Session["user_id"]);
                    category = Convert.ToString(Session["category"]);

                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }
                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
            }
        }

        public Hashtable dataParams()
        {
            Hashtable htParams = new Hashtable();
            htParams.Add("fromDt", txtFrom.Text);
            htParams.Add("ToDt", txtTo.Text);
            htParams.Add("operid", Convert.ToString(Session["operator_id"]));
            return htParams;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
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
                    msgbox("To date must be later than From date", txtTo);
                    return;
                }
            }
            else
            {
                msgbox("From and To date cannot be blank", txtFrom);
                return;
            }
            Hashtable htdta = dataParams();

            DataTable dt = new DataTable(); ; //check for exception
            Cls_BLL_ecafstbtransfer objTran = new Cls_BLL_ecafstbtransfer();
            dt = objTran.GetAdminRejectedList(username, htdta);

            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    grdDetail.Visible = false;
                    btnExcel.Visible = false;
                    msgbox("No Data Found", txtFrom);
                }
                else
                {

                    btnExcel.Visible = true;
                    grdDetail.DataSource = dt;
                    grdDetail.DataBind();
                    grdDetail.Visible = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdDetail.ClientID + "', 400, 1200 , 37 ,false); </script>", false);
                    DivRoot.Style.Add("display", "block");
                }
            }
        }

        protected void btn_genExl_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text;
            string to = txtTo.Text;
            Hashtable htdta = dataParams();
            Cls_BLL_ecafstbtransfer objTran = new Cls_BLL_ecafstbtransfer();
            DataTable htResponse = objTran.GetAdminRejectedList(Session["username"].ToString(), htdta);
            DataTable dt = null; //check for exception
            if (htResponse.Rows.Count != 0)
            {
                dt = (DataTable)htResponse;

                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "STBTransRejected" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr No" + Convert.ToChar(9)
                                    + "Reference No" + Convert.ToChar(9)
                                    + "STB No" + Convert.ToChar(9)
                                    + "Admin Remark" + Convert.ToChar(9)
                                    + "Inserted By" + Convert.ToChar(9)
                                    + "Inserted Date" + Convert.ToChar(9)
                                    + "Rejected By" + Convert.ToChar(9)
                                    + "Rejected Date" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["refid"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["stbno"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["adminremark"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["insby"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["insdate"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["adminapprovlby"].ToString() + Convert.ToChar(9)
                                    + dt.Rows[i]["adminappdate"].ToString() + Convert.ToChar(9);


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
                Response.Redirect("../MyExcelFile/" + "STBTransRejected" + datetime + ".xls");

            }
        }

        protected void grdDetail_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDetail.PageIndex = e.NewPageIndex;
            Hashtable htTopupParams = dataParams();
            DataTable dt = new DataTable();

            Cls_BLL_ecafstbtransfer objTran = new Cls_BLL_ecafstbtransfer();
            dt = objTran.GetAdminRejectedList(username, htTopupParams);


            if (dt.Rows.Count == 0)
            {
                grdDetail.Visible = false;
                btnExcel.Visible = false;
                msgbox("No Data Found", txtFrom);
            }
            else
            {

                btnExcel.Visible = true;
                grdDetail.DataSource = dt;
                grdDetail.DataBind();
                grdDetail.Visible = true;
            }

        }
    }
}