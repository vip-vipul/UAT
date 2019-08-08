using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Transaction;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;
namespace PrjUpassPl.Transaction
{
    public partial class frmSTBForeClosureList_LCO : System.Web.UI.Page
    {
        Cls_Business_Warehouse obj = new Cls_Business_Warehouse();
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "LCO STB Foreclosure List";

            if (!IsPostBack)
            {
                Session["trnsubtype"] = null;
                Session["transtype"] = null;
                Session["Receiptno"] = null;
                Session["RightsKey"] = "N";
                FillLcoDetails();
                string username = Convert.ToString(Session["username"]);
                DataTable Dtreceiptdata = obj.GetWarehouseAllocationList(username);
                if (Dtreceiptdata.Rows.Count > 0)
                {
                    grdCashcollect.DataSource = Dtreceiptdata;
                    grdCashcollect.DataBind();
                    grdCashcollect.Visible = true;
                }
                else
                {
                    grdCashcollect.Visible = false;
                    lblResponse.Text = "No Data Found..";
                }
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
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection conObj = new OracleConnection(ConStr);
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

        protected void lnkReceiptno_click(object sender, EventArgs e)
        {
            int rowindex = Convert.ToInt32((((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer))).RowIndex);
            string trnsubtype = ((HiddenField)grdCashcollect.Rows[rowindex].FindControl("hdnsubtype")).Value;

            string transtype = ((HiddenField)grdCashcollect.Rows[rowindex].FindControl("hdntype")).Value;
            string receiptno = ((HiddenField)grdCashcollect.Rows[rowindex].FindControl("hdnreceiptno")).Value;

            Session["trnsubtype"] = trnsubtype;
            Session["transtype"] = transtype;
            Session["Receiptno"] = receiptno;

            Response.Redirect("~/Transaction/FrmSTBForeClosureMst_LCO.aspx");

        }
        
        public void Getdata(string WorkOrder)
        {
            DataTable Dtreceiptdata = obj.GetWarehouseAllocationList(WorkOrder);

            if (Dtreceiptdata.Rows.Count > 0)
            {
                grdCashcollect.DataSource = Dtreceiptdata;
                grdCashcollect.DataBind();
                grdCashcollect.Visible = true;
            }
            else
            {
                lblResponse.Text = "No Data Found..";
                grdCashcollect.Visible = false;
            }
        }

        protected void ddlLco_SelectedIndexChanged(object sender, EventArgs e)
        {
            string LCOCOde = ddlLco.SelectedValue.Split('#')[1].ToString();
            DataTable Dtreceiptdata = obj.GetWarehouseAllocationList(LCOCOde);
            if (Dtreceiptdata.Rows.Count > 0)
            {
                grdCashcollect.DataSource = Dtreceiptdata;
                grdCashcollect.DataBind();
                grdCashcollect.Visible = true;
            }
            else
            {
                lblResponse.Text = "No Data Found..";
                grdCashcollect.Visible = false;

            }
        }
    }
}