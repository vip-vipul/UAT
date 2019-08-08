using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Transaction
{
    public partial class frmSelfcare : System.Web.UI.Page
    {
        string username = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            String sid = Session.SessionID;
            Master.PageHeading = "Selfcare";
            
            if (Session["username"] != null)
            {
                username = Convert.ToString(Session["username"]);
                Session["RightsKey"] = "N";
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                Session["pagenos"] = "1";

            }
        }

        protected void lnkEnableSelfcare_Click(object sender, EventArgs e)
        {
            String str = "select * from mso_lco_master_rpt where lco_code='"+username+"'";

            DataTable tbldate = GetResult(str);

            if (tbldate.Rows.Count == 0)
            {
                lbltext.Text = "Dear Business Partner, This service is enabled only for GST registered LCos.";
                btnNo.Text = "Ok";
                btnYes.Visible = false;
            }
            else
            {
                str = "select  * from aoup_lcopre_selfcare_Access where var_selfcare_lcocode='" + username + "'";
                DataTable tbldate1 = GetResult(str);
                if (tbldate1.Rows.Count == 0)
                {
                    lbltext.Text = "This will enable the online payment options for your customer. For details, please contact your Hathway Representative. Do you want to continue?";

                    btnNo.Text = "No";
                    btnYes.Visible = true;

                }
                else
                {
                    lbltext.Text = "Dear Business Partner, Online selfcare services for your customers is already enabled.";
                    btnNo.Text = "Ok";
                    btnYes.Visible = false;
                }
            }
            popBasicEnableSelfcare.Show();
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
                Cls_Business_Selfcare objSelf = new Cls_Business_Selfcare();
                string result = objSelf.addSelfcare(username);
                if (result.Split('$')[0] == "9999")
                {
                    lbltext.Text = "Your request has been accepted. Online selfcare services for your customers will be enabled in 24-48 hours.";
                    btnNo.Text = "Ok";
                    btnYes.Visible = false;
                    popBasicEnableSelfcare.Show();
                    return;
                }
                else
                {
                    lbltext.Text = result.Split('$')[1];
                    popBasicEnableSelfcare.Show();
                    btnNo.Text = "Ok";
                    btnYes.Visible = false;
                    return;
                }
            
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            popBasicEnableSelfcare.Hide();
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

    }
}