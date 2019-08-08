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
using PrjUpassBLL.Master;
namespace PrjUpassPl.Reports
{
    public partial class rptBtnUserRights : System.Web.UI.Page
    {
        string username = "";
        string operator_id = "";
        string category_id = "";
        string user_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "User Provisioning Rights Report";

            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null && Session["user_id"] != null)
            {
                Session["RightsKey"] = "N";
                username = Convert.ToString(Session["username"]);
                operator_id = Convert.ToString(Session["operator_id"]);
                category_id = Convert.ToString(Session["category"]);
                user_id = Convert.ToString(Session["user_id"]);
            }
            if (!IsPostBack)
            {
                FillLcoDetails();
                FillGrid();
            }
        }
        protected void FillLcoDetails()
        {
            string str = "";
            string operator_id = "";
            string category_id = "";
            if (Session["operator_id"] != null && Session["category"] != null)
            {
                operator_id = Convert.ToString(Session["operator_id"]);
                category_id = Convert.ToString(Session["category"]);
            }
            try
            {

                str = "   SELECT '('||var_lcomst_code||')'||a.var_lcomst_name name,var_lcomst_code lcocode, num_lcomst_operid||'#'||var_lcomst_code opid ";  //num_lcomst_operid
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
                    return;
                }
                DataTable tbllco = GetResult(str);

                if (tbllco.Rows.Count > 0)
                {
                    ddlLco.DataTextField = "name";
                    ddlLco.DataValueField = "opid";
                    ddlLco.DataSource = tbllco;
                    ddlLco.DataBind();
                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                Response.Write("Error while online payment : " + ex.Message.ToString());
            }
            finally
            {
            }

        }
        public DataTable GetResult(String Query)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                return ObjHelper.GetDataTable(Query);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "rptBtnUserRights.aspx.cs");
                return null;
            }
        }

        protected void FillGrid()
        {
            cls_Business_rptLcowiseUserdetails objDetails = new cls_Business_rptLcowiseUserdetails();
            DataTable dt = objDetails.getLcoSubDetails(username, category_id, operator_id);
            grdUsers.DataSource = dt;
            grdUsers.DataBind();
            if (grdUsers.Rows.Count > 0)
            {
                div1.Visible = true;
            }
            else
            {
                div1.Visible = false;
                lblResponse.Text = "Data Not Found";
            }
        }

        protected void lnkUserID_Click(object sender, EventArgs e)
        {
            div1.Visible = false;
            int rindex = ((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer)).RowIndex;
            HiddenField hdnUserID = (HiddenField)grdUsers.Rows[rindex].FindControl("hdnUserID");
            HiddenField hdnUserName = (HiddenField)grdUsers.Rows[rindex].FindControl("hdnUserName");
            Session["UserUpdate"] = hdnUserID.Value;
            div3.Visible = true;
            cls_Business_rptLcowiseUserdetails objDetails = new cls_Business_rptLcowiseUserdetails();
            DataTable dt = objDetails.getLcoSubRight_Details(Session["UserUpdate"].ToString(), category_id, operator_id);
            if (dt.Rows.Count > 0)
            {
                grdUsers_2.DataSource = dt;
                grdUsers_2.DataBind();
            }
            else
            {
               /* dt.Columns.Add(new DataColumn("VAR_ACCESS_USERNAME"));
                dt.Columns.Add(new DataColumn("PlanAdd"));
                dt.Columns.Add(new DataColumn("PlanRenew"));
                dt.Columns.Add(new DataColumn("PlanChange"));
                dt.Columns.Add(new DataColumn("PlanCancel"));
                dt.Columns.Add(new DataColumn("Discount"));
                dt.Columns.Add(new DataColumn("Retrack"));
                dt.Columns.Add(new DataColumn("CustModify"));
                dt.Columns.Add(new DataColumn("STBSwap"));
                dt.Columns.Add(new DataColumn("VCSwap"));
                dt.Columns.Add(new DataColumn("AutoRenew"));
                dt.Columns.Add(new DataColumn("Deactivate"));
                dt.Columns.Add(new DataColumn("Div_TERMINATE"));
                dt.Columns.Add(new DataColumn("FocPack"));
                dt.Columns.Add(new DataColumn("VAR_ACCESS_INSBY"));
                dt.Columns.Add(new DataColumn("VAR_ACCESS_INSDT"));*/
                dt.Rows.Add(3,Session["UserUpdate"].ToString(), "No", "No", "No", "No", "No", "No", "No", "No", "No", "No", "No", "No", "No", "", System.DateTime.Now);
                grdUsers_2.DataSource = dt;
                grdUsers_2.DataBind();
            }
        }


    }
}