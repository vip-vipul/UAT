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

namespace PrjUpassPl.Master
{
    public partial class TransAssignBtnRights : System.Web.UI.Page
    {
        string username = "";
        string operator_id = "";
        string category_id = "";
        string user_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "User Rights Update";

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
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "TransAssignBtnRights.aspx.cs");
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
            ClearCheckbox();
            div1.Visible = false;
            int rindex = (((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer))).RowIndex;
            HiddenField hdnUserID = (HiddenField)grdUsers.Rows[rindex].FindControl("hdnUserID");
            HiddenField hdnUserName = (HiddenField)grdUsers.Rows[rindex].FindControl("hdnUserName");
            Session["UserUpdate"] = hdnUserID.Value;
            lblLcoID.Text = hdnUserID.Value;
            lblLcoName.Text = hdnUserName.Value;
            div2.Visible = true;
            div3.Visible = true;
            div5.Visible = true;
            string str = "select a.*,nvl(b.var_user_useraccmap_flag,'N') AccMap FROM AOUP_LCOPRE_ACCESS_CONTROL a,aoup_user_def b where var_access_username=var_user_username and var_user_username='" + lblLcoID.Text + "' ";
            DataTable tbllco = GetResult(str);
            if (tbllco.Rows.Count > 0)
            {

                if (tbllco.Rows[0]["AccMap"].ToString() == "Y")
                {
                    chkUserAccMap.Checked = true;
                }
                else
                {
                    chkUserAccMap.Checked = false;
                }

                if (tbllco.Rows[0]["var_access_add"].ToString() == "Y")
                {
                    chkAdd.Checked = true;
                }
                else
                {
                    chkAdd.Checked = false;
                }
                if (tbllco.Rows[0]["var_access_renew"].ToString() == "Y")
                {
                    chkRenew.Checked = true;
                }
                else
                {
                    chkRenew.Checked = false;
                }
                if (tbllco.Rows[0]["var_access_change"].ToString() == "Y")
                {
                    chkChange.Checked = true;
                }
                else
                {
                    chkChange.Checked = false;
                }
                if (tbllco.Rows[0]["var_access_cancel"].ToString() == "Y")
                {
                    chkCancel.Checked = true;
                }
                else
                {
                    chkCancel.Checked = false;
                }
                if (tbllco.Rows[0]["var_access_discount"].ToString() == "Y")
                {
                    chkDiscount.Checked = true;
                }
                else
                {
                    chkDiscount.Checked = false;
                }

                if (tbllco.Rows[0]["var_access_retrack"].ToString() == "Y")
                {
                    chkRetrack.Checked = true;
                }
                else
                {
                    chkRetrack.Checked = false;
                }
                if (tbllco.Rows[0]["var_access_custmodify"].ToString() == "Y")
                {
                    chkCustModify.Checked = true;
                }
                else
                {
                    chkCustModify.Checked = false;
                }
                if (tbllco.Rows[0]["var_access_stbswap"].ToString() == "Y")
                {
                    chkSTBSwap.Checked = true;
                }
                else
                {
                    chkSTBSwap.Checked = false;
                }
                if (tbllco.Rows[0]["var_access_autorenew"].ToString() == "Y")
                {
                    chkAutoRenew.Checked = true;
                }
                else
                {
                    chkAutoRenew.Checked = false;
                }
                if (tbllco.Rows[0]["var_access_deactivate"].ToString() == "Y")
                {
                    chkDeactivate.Checked = true;
                }
                else
                {
                    chkDeactivate.Checked = false;
                }

                if (tbllco.Rows[0]["var_access_terminate"].ToString() == "Y")
                {
                    chkTerminate.Checked = true;
                }
                else
                {
                    chkTerminate.Checked = false;
                }
                if (tbllco.Rows[0]["var_access_focpack"].ToString() == "Y")
                {
                    chkFocPack.Checked = true;
                }
                else
                {
                    chkFocPack.Checked = false;
                }
            }

            string str1 = "select num_rights_frmid FROM aoup_lcopre_menu_rights  where num_rights_username='" + lblLcoID.Text + "' and num_rights_menuid='3'";
            DataTable tbllco1 = GetResult(str1);
            if (tbllco1.Rows.Count > 0)
            {
                for (int i = 0; i < tbllco1.Rows.Count; i++)
                {
                    if (tbllco1.Rows[i][0].ToString() == "247")
                    {
                        chkLegal.Checked = true;
                    }
                    if (tbllco1.Rows[i][0].ToString() == "246")
                    {
                        chkNotification.Checked = true;
                    }
                    if (tbllco1.Rows[i][0].ToString() == "245")
                    {
                        chkLCOAdmin.Checked = true;
                    }
                    if (tbllco1.Rows[i][0].ToString() == "244")
                    {
                        chkEcaf.Checked = true;
                    }
                    if (tbllco1.Rows[i][0].ToString() == "243")
                    {
                        chkInventoryManagement.Checked = true;
                    }
                    if (tbllco1.Rows[i][0].ToString() == "242")
                    {
                        chkBulkManagement.Checked = true;
                    }
                    if (tbllco1.Rows[i][0].ToString() == "241")
                    {
                        chkBalanceManagement.Checked = true;
                    }
                    if (tbllco1.Rows[i][0].ToString() == "3")
                    {
                        ChkPackManagement.Checked = true;
                    }

                    if (tbllco1.Rows[i][0].ToString() == "102")
                    {
                        chkDashboard.Checked = true;
                    }
                    if (tbllco1.Rows[i][0].ToString() == "111")
                    {
                        chkMassenger.Checked = true;
                    }
                    if (tbllco1.Rows[i][0].ToString() == "46")
                    {
                        chkReports.Checked = true;
                    }
                }
            }
        }

        protected void btnSubmitConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable DT = new Hashtable();
                DT.Add("LCOCode", lblLcoID.Text);
                if (chkUserAccMap.Checked == true)
                {
                    DT.Add("UserAccMap", "Y");
                }
                else
                {
                    DT.Add("UserAccMap", "N");
                }
                if (chkAdd.Checked == true)
                {
                    DT.Add("Add", "Y");
                }
                else
                {
                    DT.Add("Add", "N");
                }
                if (chkAutoRenew.Checked == true)
                {
                    DT.Add("AutoRenew", "Y");
                }
                else
                {
                    DT.Add("AutoRenew", "N");
                }
                if (chkCancel.Checked == true)
                {
                    DT.Add("Cancel", "Y");
                }
                else
                {
                    DT.Add("Cancel", "N");
                }
                if (chkChange.Checked == true)
                {
                    DT.Add("Change", "Y");
                }
                else
                {
                    DT.Add("Change", "N");
                } if (chkCustModify.Checked == true)
                {
                    DT.Add("CustModify", "Y");
                }
                else
                {
                    DT.Add("CustModify", "N");
                }
                if (chkDeactivate.Checked == true)
                {
                    DT.Add("Deactivate", "Y");
                }
                else
                {
                    DT.Add("Deactivate", "N");
                }
                if (chkDiscount.Checked == true)
                {
                    DT.Add("Discount", "Y");
                }
                else
                {
                    DT.Add("Discount", "N");
                }
                if (chkFocPack.Checked == true)
                {
                    DT.Add("FocPack", "Y");
                }
                else
                {
                    DT.Add("FocPack", "N");
                }
                if (chkRenew.Checked == true)
                {
                    DT.Add("Renew", "Y");
                }
                else
                {
                    DT.Add("Renew", "N");
                }
                if (chkRetrack.Checked == true)
                {
                    DT.Add("Retrack", "Y");
                }
                else
                {
                    DT.Add("Retrack", "N");
                }
                if (chkSTBSwap.Checked == true)
                {
                    DT.Add("STBSwap", "Y");
                }
                else
                {
                    DT.Add("STBSwap", "N");
                }
                if (chkTerminate.Checked == true)
                {
                    DT.Add("Terminate", "Y");
                }
                else
                {
                    DT.Add("Terminate", "N");
                }

                //---Tiles
                string Pages = "";
                if (chkDashboard.Checked == true)
                {
                    Pages += "102#Y$";
                }
                else
                {
                    Pages += "102#N$";
                }

                if (ChkPackManagement.Checked == true)
                {
                    Pages += "3#Y$";
                }
                else
                {
                    Pages += "3#N$";
                }

                if (chkBalanceManagement.Checked == true)
                {
                    Pages += "241#Y$";
                }
                else
                {
                    Pages += "241#N$";
                }

                if (chkBulkManagement.Checked == true)
                {
                    Pages += "242#Y$";
                }
                else
                {
                    Pages += "242#N$";
                }

                if (chkMassenger.Checked == true)
                {
                    Pages += "111#Y$";
                }
                else
                {
                    Pages += "111#N$";
                }

                if (chkReports.Checked == true)
                {
                    Pages += "46#Y$";
                }
                else
                {
                    Pages += "46#N$";
                }

                if (chkInventoryManagement.Checked == true)
                {
                    Pages += "243#Y$";
                }
                else
                {
                    Pages += "243#N$";
                }

                if (chkEcaf.Checked == true)
                {
                    Pages += "244#Y$";
                }
                else
                {
                    Pages += "244#N$";
                }

                if (chkNotification.Checked == true)
                {
                    Pages += "246#Y$";
                }
                else
                {
                    Pages += "246#N$";
                }

                if (chkLCOAdmin.Checked == true)
                {
                    Pages += "245#Y$";
                }
                else
                {
                    Pages += "245#N$";
                }

                if (chkLegal.Checked == true)
                {
                    Pages += "247#Y$";
                }
                else
                {
                    Pages += "247#N$";
                }

                Pages = Pages.TrimEnd('#');
                DT.Add("Pages", Pages);
                Cls_Business_MstLCOUpdateDetails objinst = new Cls_Business_MstLCOUpdateDetails();
                string response = "";

                response = objinst.LCOAssignRights(username, DT);
                if (response == "ex_occured")
                {
                    msgbox(response);
                }
                else
                {
                    msgbox(response);
                    div2.Visible = false;
                    div3.Visible = false;
                    div5.Visible = false;
                }
            }
            catch (Exception)
            {
                div2.Visible = false;
                div3.Visible = false;
                div5.Visible = false;
                div1.Visible = true;
                //throw;
            }
        }
        public void msgbox(string message)
        {
            lblPopupResponse.Text = message;
            popMsg.Show();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            popCheques.Show();

        }
        protected void btnClodeMsg1_click(object sender, EventArgs e)
        {
            FillGrid();
            div2.Visible = false;
            div1.Visible = true;
            div3.Visible = false;
            div5.Visible = false;
            lblPopupResponse.Text = "";
            popMsg.Hide();
            ClearCheckbox();
            return;
        }
        public void ClearCheckbox()
        {
            chkAdd.Checked = false;
            chkAutoRenew.Checked = false;
            chkCancel.Checked = false;
            chkChange.Checked = false;
            chkCustModify.Checked = false;
            chkDeactivate.Checked = false;
            chkDiscount.Checked = false;
            chkFocPack.Checked = false;
            chkRenew.Checked = false;
            chkRetrack.Checked = false;
            chkSTBSwap.Checked = false;
            chkTerminate.Checked = false;
            chkBalanceManagement.Checked = false;
            chkBulkManagement.Checked = false;
            chkDashboard.Checked = false;
            chkEcaf.Checked = false;
            chkInventoryManagement.Checked = false;
            chkLCOAdmin.Checked = false;
            chkLegal.Checked = false;
            chkMassenger.Checked = false;
            chkNotification.Checked = false;
            ChkPackManagement.Checked = false;
            chkReports.Checked = false;
           
        }
    }
}