using System;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;

namespace PrjUpassPl.Transaction
{
    public partial class TransNotificationPages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session["pagenos"] = "0";
                Master.PageHeading = "Notification";
                Session["RightsKey"] = null;
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
                str += " and num_frm_id in (42,43,45) ";
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
    }
}