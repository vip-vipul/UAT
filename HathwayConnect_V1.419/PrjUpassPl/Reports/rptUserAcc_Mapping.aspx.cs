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
using PrjUpassBLL.Transaction;
namespace PrjUpassPl.Reports
{
    public partial class rptUserAcc_Mapping : System.Web.UI.Page
    {
        DateTime dtime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["RightsKey"] = "N";
                grdUserAccMap.PageIndex = 0;
                bindUserID();
                Master.PageHeading = "User Account Mapping Report";
                txtFrom.Attributes.Add("readonly", "readonly");
                txtTo.Attributes.Add("readonly", "readonly");
                txtFrom.Text = dtime.ToString("dd-MMM-yyyy").Trim();
                txtTo.Text = dtime.ToString("dd-MMM-yyyy").Trim();
            }
        }
        public void bindUserID()
        {
            string _getState = "select userid,username from view_Lcopre_user_det where operid='" + Session["operator_id"].ToString() + "'and flag='Executive'";
            Cls_Helper ob = new Cls_Helper();


            DataTable dt = new DataTable();
            dt = ob.GetDataTable(_getState);
            if (dt.Rows.Count > 0)
            {
                ddlUserID.DataSource = dt;
                ddlUserID.DataTextField = "username";
                ddlUserID.DataValueField = "userid";
                ddlUserID.DataBind();
                ddlUserID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));

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
            htSearchParams.Add("UserID", ddlUserID.SelectedItem.ToString());
            htSearchParams.Add("Flag", ddlFlag.SelectedValue.ToString());
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
                    grdUserAccMap.Visible = false;
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
                    grdUserAccMap.Visible = true;
                }
            }
            BindUserdata();
        }


        public void BindUserdata()
        {
            btngrnExel.Visible = false;
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
            Cls_Business_UserAccountMap objTran = new Cls_Business_UserAccountMap();
            htResponse = objTran.UserAcc_Details(htAddPlanParams, username);
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
                grdUsers.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                grdUsers.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_User"] = dt;
                grdUsers.DataSource = dt;
                grdUsers.DataBind();

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdUserAccMap.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                div1.Visible = true;
                DivMainContent.Visible = false;

            }
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
            Cls_Business_UserAccountMap objTran = new Cls_Business_UserAccountMap();
            htResponse = objTran.UserAccApp_Details(htAddPlanParams, username, Session["UserUpdate"].ToString());
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
                grdUserAccMap.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                grdUserAccMap.Visible = true;
                lblSearchMsg.Text = "";
                ViewState["searched_trans"] = dt;
                grdUserAccMap.DataSource = dt;
                grdUserAccMap.DataBind();

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdUserAccMap.ClientID + "', 400, 1200 , 46 ,false); </script>", false);
                div1.Visible = false;
                DivMainContent.Visible = true;

            }
        }

        protected void lnkUserID_Click(object sender, EventArgs e)
        {
            div1.Visible = false;
            int rindex = ((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer)).RowIndex;
            HiddenField hdnUserID = (HiddenField)grdUsers.Rows[rindex].FindControl("hdnUserID");
            HiddenField hdnUserName = (HiddenField)grdUsers.Rows[rindex].FindControl("hdnUserName");
            Session["UserUpdate"] = hdnUserID.Value;
            Binddata();
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

                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + "UserAccMap_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "Sr.No." + Convert.ToChar(9)
                        + "User ID" + Convert.ToChar(9)
                        + "Account No" + Convert.ToChar(9)
                        + "Action" + Convert.ToChar(9);

                    while (j < dt.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            j = j + 1;
                            string strrow = j.ToString() + Convert.ToChar(9) + "'";


                            strrow +=
                                 dt.Rows[i]["username"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["accountno"].ToString() + Convert.ToChar(9)
                                + dt.Rows[i]["flag"].ToString() + Convert.ToChar(9);


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
                Response.Redirect("../MyExcelFile/" + "UserAccMap_" + datetime + ".xls");
            }
        }

        protected void grdactdact_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdUserAccMap.PageIndex = e.NewPageIndex;
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
            Hashtable htResponse = objTran.GetNewActivation(htAddPlanParams, username);

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
                grdUserAccMap.Visible = false;
                lblSearchMsg.Text = "No data found";
            }
            else
            {
                btngrnExel.Visible = true;
                grdUserAccMap.Visible = true;
                lblSearchMsg.Text = "";
                grdUserAccMap.DataSource = dt;
                grdUserAccMap.DataBind();
            }
        }


    }
}