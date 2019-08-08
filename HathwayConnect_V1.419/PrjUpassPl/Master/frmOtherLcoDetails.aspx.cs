using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassDAL.Helper;
using System.Collections;

using System.Data.OracleClient;
using System.Configuration;
using PrjUpassBLL.Master;

namespace PrjUpassPl.Master
{
    public partial class frmOtherLcoDetails : System.Web.UI.Page
    {
        string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        string username;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Lco Other Details";
            if (!IsPostBack)
            {
                
                if (Session["operator_id"] != null)
                {
                    Session["RightsKey"] = null;
                    string operid = Convert.ToString(Session["operator_id"]);
                    username = Convert.ToString(Session["username"]);
                    string catid = Convert.ToString(Session["category"]);

                    RadSearchby.SelectedValue = "0";
                    pnlDetails.Visible = false;

                    if (catid == "3")
                    {
                        divsearchLco.Visible = false;
                        loadLCOSection();
                        //loadglobalon();


                    }
                    else
                    {
                        divsearchLco.Visible = true;
                    }
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }

            }
        }

        private Hashtable getLCOParamsData()
        {
            string lcocd = "";
            string lconm = "";
            if (RadSearchby.SelectedValue.ToString() == "0")
            {
                lcocd = Session["username"].ToString(); //txtLCOSearch.Text;
            }
            else if (RadSearchby.SelectedValue.ToString() == "1")
            {
                lconm = txtLCOSearch.Text;
            }

            Hashtable htSearchParams = new Hashtable();
            htSearchParams.Add("lcocd", lcocd);
            htSearchParams.Add("lconm", lconm);
            return htSearchParams;
        }

        protected void loadLCOSection()
        {
            Hashtable htLCOParams = getLCOParamsData();
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                string username = Convert.ToString(Session["username"]);
                string catid = Convert.ToString(Session["category"]);
                string operator_id = Convert.ToString(Session["operator_id"]);
                Cls_Business_MstLCOUpdateDetails obj = new Cls_Business_MstLCOUpdateDetails();
                Hashtable htResponse = obj.GetTransations(htLCOParams, username, catid, operator_id);

                DataTable dt = null;
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
                    lblLCOCode.Text = "";
                    lblLCOName.Text = "";
                    //txtEmail.Text = "";
                    //txtMobile.Text = "";
                    lblSubDistributor.Text = "";
                    lblState.Text = "";
                    lblDistributor.Text = "";
                    lblDirect.Text = "";
                    lblCity.Text = "";
                    lblAddress.Text = "";
                    lblJV.Text = "";
                    lblResponseMsg.Text = "No data found...";
                    pnlDetails.Visible = false;
                }
                else
                {
                    lblLCOCode.Text = dt.Rows[0]["lcocode"].ToString();
                    lblLCOName.Text = dt.Rows[0]["lconame"].ToString();
                   // txtEmail.Text = dt.Rows[0]["email"].ToString();

                    //txtMobile.Text = dt.Rows[0]["mobileno"].ToString();

                    lblAddress.Text = dt.Rows[0]["addr"].ToString();
                    lblCity.Text = dt.Rows[0]["city"].ToString();
                    lblState.Text = dt.Rows[0]["state"].ToString();
                    lblDistributor.Text = dt.Rows[0]["distname"].ToString();
                    lblSubDistributor.Text = dt.Rows[0]["subdist"].ToString();
                    lblDirect.Text = dt.Rows[0]["directno"].ToString();
                    lblJV.Text = dt.Rows[0]["jvno"].ToString();

                    

                    Session["LcoCode"] = lblLCOCode.Text.Trim().ToString();
                    ViewState["lcoid2"] = dt.Rows[0]["lcoid"].ToString();
                    ViewState["searched_trans"] = dt;
                    lblResponseMsg.Text = "";

                    pnlDetails.Visible = true;

                }


            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadLCOSection();
            //loadglobalon();

            string username;
            if (Session["username"] != null)
            {
                username = Session["username"].ToString();
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }
        }
         
        //protected void loadglobalon()
        //{
        //    try
        //    {
              
        //    string strConn = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
        //    OracleConnection conn = new OracleConnection(strConn);
        //    String str = "";

        //    str += " select var_lcoautorenew_flag from aoup_lcopre_autorenew_config a where a.var_lco_code_autorenew='" + Session["username"].ToString().Trim() + "'";

                              

        //        conn.Open();
        //        OracleCommand cmd2 = new OracleCommand(str, conn);
        //        OracleDataReader dr4 = cmd2.ExecuteReader();
        //        if (dr4.HasRows)
        //        {

        //        }
        //        else
        //        {
        //            chkecsstatus.Checked = false;
        //            return;
        //        }
        //        while (dr4.Read())
        //        {

        //            if (dr4["var_lcoautorenew_flag"].ToString() == "Y")
        //            {
        //                chkecsstatus.Checked = true;
        //            }
        //            else
        //            {
        //                chkecsstatus.Checked = false;
        //            }

        //        }                
        //        dr4.Close();
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.Message.ToString());
        //    }
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                if (txtAreaMang.Text == "" || txtAreaMang.Text == null)
                {
                    lblResponseMsg.Text = "Please enter Area Manager..!!";
                    return;
                }
                if (txtintagreexdt.Text == "" || txtintagreexdt.Text == null)
                {
                    lblResponseMsg.Text = "Please enter Interconnect Agreement Expiry Date..!!";
                    return;
                }
                if (txtptexdt.Text == "" || txtptexdt.Text == null)
                {
                    lblResponseMsg.Text = "Please enter P&T License Expiry Date..!!";
                    return;
                }
                if (txtexecutive.Text == "" || txtexecutive.Text == null)
                {
                    lblResponseMsg.Text = "Please enter executive..!!";
                    return;
                }
                if (Convert.ToDateTime(txtintagreexdt.Text.ToString()) < DateTime.Now.Date)
                {
                    lblResponseMsg.Text = "You can not select Interconnect Agreement Expiry Date  earlier than current date!";//"You can not select From date later than 15 days from current date!";
                    return;
                }
                if (Convert.ToDateTime(txtptexdt.Text.ToString()) < DateTime.Now.Date)
                {
                    lblResponseMsg.Text = "You can not select P&T License Expiry Date earlier than current date!";//"You can not select From date later than 15 days from current date!";
                    return;
                }
                string username = Convert.ToString(Session["username"]);
                string catid = Convert.ToString(Session["category"]);
                string operator_id = Convert.ToString(Session["operator_id"]);
                Hashtable ht = new Hashtable();

                string lcocode = Session["LcoCode"].ToString();
                ht["lcocode"] = lcocode;
                string areamanager = txtAreaMang.Text.ToString().Trim();
                ht["areamanager"] = areamanager;
                string ptexdt = txtptexdt.Text.ToString().Trim();
                ht["ptexdt"] = ptexdt;
                string intagreeexpdt = txtintagreexdt.Text.ToString().Trim();
                ht["intagreeexpdt"] = intagreeexpdt;
                string executive = txtexecutive.Text.ToString().Trim();
                ht["executive"] = executive;
                //if (chkecsstatus.Checked == true)
                //{
                //    ht["ecssattus"] = "Y";
                //}
                //else
                //{
                //    ht["ecssattus"] = "N";
                //}

              
                Cls_Business_MstLcoOtherDetails obj = new Cls_Business_MstLcoOtherDetails();
                string response = obj.setLcoData(username, ht);

                if (response == "ex_occured")
                {
                    //exception occured
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }
                else
                {
                    lblResponseMsg.Text = "Other Lco Details Updated Successfully...!!!";
                    reset();
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }                           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        protected void reset()
        {
            txtintagreexdt.Text = "";
            txtAreaMang.Text = "";
            txtexecutive.Text = "";
            txtptexdt.Text = "";
            txtLCOSearch.Text = "";
            //chkecsstatus.Checked = false;
            //lblResponseMsg.Text = "";
        }
    }
}