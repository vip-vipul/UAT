using System;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;


namespace PrjUpassPl.Transaction
{
    public partial class TransBulkPages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Bulk Management";
            if (!IsPostBack)
            {
                Session["pagenos"] = "0";
                Master.PageHeading = "Bulk Management";
                Session["RightsKey"] = null;
                lnkDetail.BackColor = System.Drawing.Color.Red;
                lnkDetail.ForeColor = System.Drawing.Color.White;
                lnkDetail1.BackColor = System.Drawing.Color.White;
                lnkDetail1.ForeColor = System.Drawing.Color.Red;
                lnkDetail1.BorderColor = System.Drawing.Color.Red;
                //BindData();
            }
        }
        private void BindData()
        {
            try
            {
                string username, catid, operator_id, user_id;
                if (Session["username"] != null || Session["operator_id"] != null)
                {
                    user_id = Session["user_id"].ToString();
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

                string conn = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
                OracleConnection oraConn = new OracleConnection(conn);


                string str = "select DISTINCT(a.num_frm_id), a.var_frm_name, a.num_frm_cat, a.var_frm_file, a.num_frm_sortorder ";
                str += " from aoup_lcopre_frm_def a, aoup_lcopre_menu_rights b ";
                str += " where a.num_frm_status = 1 and b.num_rights_operid = " + operator_id + " and b.num_rights_userid = " + user_id + " ";
                str += " and num_frm_cat='2' and a.num_frm_id=b.num_rights_frmid  ";
                str += " and num_frm_id in (32,30,73,82) ";
                str += " order by a.num_frm_sortorder ";

                oraConn.Open();
                OracleDataAdapter da = new OracleDataAdapter(str, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                //xDlstState.DataSource = ds;
                //xDlstState.DataBind();
                oraConn.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
        }

        protected void lnkatag_Click(object sender, EventArgs e)
        {
            tr1.Visible = true;
            tr2.Visible = false;
            lnkDetail.BackColor = System.Drawing.Color.Red;
            lnkDetail.ForeColor = System.Drawing.Color.White;
            lnkDetail1.BackColor = System.Drawing.Color.White;
            lnkDetail1.ForeColor = System.Drawing.Color.Red;
            lnkDetail1.BorderColor = System.Drawing.Color.Red;
        }
        protected void lnkatag1_Click(object sender, EventArgs e)
        {
            tr2.Visible = true;
            tr1.Visible = false;
            lnkDetail1.BackColor = System.Drawing.Color.Red;
            lnkDetail1.ForeColor = System.Drawing.Color.White;
            lnkDetail.BackColor = System.Drawing.Color.White;
            lnkDetail.ForeColor = System.Drawing.Color.Red;
            lnkDetail.BorderColor = System.Drawing.Color.Red;
        }
    }
}