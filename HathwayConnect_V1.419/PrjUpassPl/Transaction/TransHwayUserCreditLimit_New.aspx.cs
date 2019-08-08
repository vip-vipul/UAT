using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PrjUpassBLL.Transaction;
using System.Data;
using PrjUpassDAL.Authentication;
using System.IO;
using System.Collections;
using System.Data.OracleClient;
using System.Configuration;

namespace PrjUpassPl.Transaction
{
    public partial class TransHwayUserCreditLimit_New : System.Web.UI.Page
    {
        string username = "";
        string operator_id = "";
        string category_id = "";
        string user_id = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Balance Allocation";
           
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null && Session["user_id"] != null)
            {
                Session["RightsKey"] = null;
                username = Convert.ToString(Session["username"]);
                operator_id = Convert.ToString(Session["operator_id"]);
                category_id = Convert.ToString(Session["category"]);
                user_id = Convert.ToString(Session["user_id"]);
            }
            if (!IsPostBack)
            {
                FillLcoDetails();
                updateBalGrid();

                
            }
        }

        protected void FillLcoDetails()
        {
            string str = "";

            // Cls_BLL_TransHwayLcoPayment obj = new Cls_BLL_TransHwayLcoPayment();
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

                    //  lblmsg.Text = "No LCO Details Found";
                    // Paydet.Visible = false;
                    // btnSubmit.Visible = false;
                    //  divdet.Visible = false;
                    // pnllco.Visible = false;
                    return;
                }
                DataTable tbllco = GetResult(str);

                if (tbllco.Rows.Count > 0)
                {
                    // pnllco.Visible = true;
                    ddlLco.DataTextField = "name";
                    ddlLco.DataValueField = "opid";

                    ddlLco.DataSource = tbllco;
                    ddlLco.DataBind();
                    //if (category_id == "11")
                    //{
                    //    ddlLco.Items.Insert(0, new ListItem("Select LCO", "0"));
                    //}
                    //else if (category_id == "3")
                    //{
                    //    //ddllco_SelectedIndexChanged(null, null);
                    //}
                    //  opID = tbllco.Rows[0]["opid"].ToString();

                }
                else
                {
                    //  lblmsg.Text = "No LCO Details Found";
                    // divdet.Visible = false;
                    // Paydet.Visible = false;
                    // btnSubmit.Visible = false;
                    // pnllco.Visible = false;
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


        protected void ddlLco_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateBalGrid();
        }

        protected void updateBalGrid()
        {
           
                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();

            

            Cls_Bussiness_TransHwayUserCreditLimit balObj = new Cls_Bussiness_TransHwayUserCreditLimit();

            if (category_id == "3" || category_id == "11")
            {
                string[] avail_bal = balObj.GetAvailBal(username, category_id, user_id, operator_id);
                if (avail_bal.Length != 0)
                {
                    lblAvailBal.Text = avail_bal[0].Trim();
                    hdnAvailBal.Value = avail_bal[0].Trim();
                    lbltotalbalance.Text = avail_bal[1].Trim();
                    lblallocatedbalance.Text = avail_bal[2].Trim();
                    hdnAvailCreditBal.Value = avail_bal[3].Trim();
                    lblCreditBal.Text = avail_bal[3].Trim();
                }
                else
                {
                    //lblAvailBal.Text = "Failed to get available credits";
                    lblAvailBal.Text = "0";
                    lblallocatedbalance.Text = "0";
                    lbltotalbalance.Text = "0";

                }
                balBox.Visible = true;
            }
            else
            {
                balBox.Visible = false;
            }
            grdUsers.DataSource = null;
            grdUsers.DataBind();
            DataTable dtUsers = balObj.GetAllUserDetails(username, category_id, operator_id);
            if (dtUsers == null || dtUsers.Rows.Count == 0)
            {
                lblResponse.Text = "No Users Found";
                btnManageCredits.Visible = false;
                grdUsers.DataSource = null;
                grdUsers.DataBind();
                return;
            }
            else
            {
                grdUsers.DataSource = dtUsers;
                grdUsers.DataBind();
                btnManageCredits.Visible = true;
            }
        }

        protected void btnManageCredits_Click(object sender, EventArgs e)
        {


            if (category_id == "11")
            {

                operator_id = ddlLco.SelectedValue.Split('#')[0].ToString();
            }


            double inc_cr = 0;
            double dec_cr = 0;
            double total_cr = 0;
            double avail_bal = 0;
            string strCreditInfo = "";
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            foreach (GridViewRow row in grdUsers.Rows)
            {
                TextBox txtInc = (TextBox)row.FindControl("txtIncLimit");
                TextBox txtDec = (TextBox)row.FindControl("txtDecLimit");
                double avail_usr_bal = Convert.ToDouble(row.Cells[2].Text.Trim());
                HiddenField hdnUserId = (HiddenField)row.FindControl("hdnUserId");
                if (txtInc.Text.Trim() != "0" || txtDec.Text.Trim() != "0")
                {
                    if (txtDec.Text.Trim() != "0")
                    { //check decrease value < avail bal
                        if (avail_usr_bal < Convert.ToDouble(txtDec.Text.Trim()))
                        {
                            lblResponse.Text = "Decrease value must be less than allocated balance";
                            txtDec.Focus();
                            return;
                        }
                    }
                    strCreditInfo += hdnUserId.Value + "$" + txtInc.Text + "$" + txtDec.Text + "~";
                    if (txtInc.Text.Trim() != "0")
                    {
                        inc_cr = inc_cr + Convert.ToDouble(txtInc.Text.Trim());
                    }
                    if (txtDec.Text.Trim() != "0")
                    {
                        dec_cr = dec_cr + Convert.ToDouble(txtDec.Text.Trim());
                    }
                }
            }
            strCreditInfo = strCreditInfo.TrimEnd('~');
            total_cr = inc_cr - dec_cr;
            avail_bal = Convert.ToDouble(hdnAvailBal.Value);// -Convert.ToDouble(hdnAvailCreditBal.Value);// hdnAvailCreditBal is New Credit Balance 
            
            if (avail_bal < total_cr)
            {
                lblResponse.Text = "You dont have enough balance for distribution";
                return;
            }
            else
            {
                if (strCreditInfo == "")
                {
                    lblResponse.Text = "Set balance for atleast one user";
                    return;
                }
                Cls_Bussiness_TransHwayUserCreditLimit crd = new Cls_Bussiness_TransHwayUserCreditLimit();
                string creditRes = crd.setCredits(username, strCreditInfo, operator_id, Ip);

                if (creditRes.Split('$')[0] != "9999")
                {
                    lblResponse.Text = "Setting balance failed : " + creditRes.Split('$')[1];
                    return;
                }
                else
                {
                    lblResponse.Text = creditRes.Split('$')[1];
                    updateBalGrid();
                    return;
                }
            }

        }

        protected void grdUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string bal = e.Row.Cells[2].Text;
                if (bal == "0" || bal == "" || bal == "&nbsp;")
                {
                    ((TextBox)e.Row.FindControl("txtDecLimit")).Enabled = false;
                    if (bal == "" || bal == "&nbsp;")
                    {
                        e.Row.Cells[2].Text = "0";
                    }
                }

                e.Row.Cells[5].Visible = false;

                if (e.Row.Cells[5].Text == "Y")
                {

                    ((Button)e.Row.FindControl("btnBlockunblock")).Text = "Unblock";
                    ((TextBox)e.Row.FindControl("txtDecLimit")).Enabled = false;
                    ((TextBox)e.Row.FindControl("txtIncLimit")).Enabled = false;


                }
                else
                {
                    ((Button)e.Row.FindControl("btnBlockunblock")).Text = "Block";
                    ((TextBox)e.Row.FindControl("txtDecLimit")).Enabled = true;
                    ((TextBox)e.Row.FindControl("txtIncLimit")).Enabled = true;

                }

                if (e.Row.Cells[0].Text == Session["username"].ToString())
                {
                    ((Button)e.Row.FindControl("btnBlockunblock")).Text = "Block";
                    ((Button)e.Row.FindControl("btnBlockunblock")).Visible = false;

                }
                else
                {
                    ((Button)e.Row.FindControl("btnBlockunblock")).Visible = true;
                }
            }
        }


        protected void btnBlockunblock_Click(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

            int index = gvRow.RowIndex;

            string userId = grdUsers.Rows[index].Cells[0].Text;
            string username = grdUsers.Rows[index].Cells[1].Text;

            HiddenField hdnblkUnblck = (HiddenField)grdUsers.Rows[index].FindControl("hdnblkUnblck");
            string status = hdnblkUnblck.Value;



            lblpopuserid.Text = userId;
            lblpopusername.Text = username;

            if (status == "N")
            {
                lblmsgbox.Text = "Are you sure , you want to block this user ? ";
                hdnpopstatus.Value = "Y";
            }
            else
            {
                lblmsgbox.Text = "Are you sure , you want to unblock this user ? ";
                hdnpopstatus.Value = "N";
            }

            //  lblmsgbox.Text = "";
            popMsgBox.Show();

        }
        protected void btncnfmBlck_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null && Session["user_id"] != null)
            {

                username = Convert.ToString(Session["username"]);
                operator_id = Convert.ToString(Session["operator_id"]);
                category_id = Convert.ToString(Session["category"]);
                user_id = Convert.ToString(Session["user_id"]);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }



            Cls_Data_Auth ob = new Cls_Data_Auth();
            string ip = ob.GetIPAddress(HttpContext.Current.Request);

            Hashtable htdata = new Hashtable();

            htdata.Add("userid", lblpopuserid.Text.Trim());
            htdata.Add("username", lblpopusername.Text.Trim());
            htdata.Add("ip", ip);
            htdata.Add("status", hdnpopstatus.Value);

            Cls_Bussiness_TransHwayUserCreditLimit objct = new Cls_Bussiness_TransHwayUserCreditLimit();

            string result = objct.userblkUnblock(username, htdata);
            if (result == "9999")
            {

                lblResponse.Text = "Updated Successfully";
                //blcked successfully ;

            }
            else
            {
                lblResponse.Text = "failed while updating";
                return;
            }

            if (category_id == "3")
            {
                updateBalGrid();
            }

        }

        //private void FileLogText(String Str, String sender, String strRequest, String strResponse)
        //{
        //    string filename = DateTime.Now.ToString("dd-MMM-yyyy");
        //    StreamWriter sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\Online_Getway_Payment_Response_" + filename + ".txt", true);
        //    try
        //    {
        //        sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
        //        sw.WriteLine(strRequest.Trim());
        //        sw.WriteLine(strResponse.Trim());
        //        sw.WriteLine("************************************************************************************************************************");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("Error while writing logs : " + ex.Message.ToString());
        //    }
        //    finally
        //    {
        //        sw.Flush();
        //        sw.Close();
        //        sw.Dispose();
        //    }
        //}

 //       public void ResponseBildesk()
 //       {
 //           string BillDeskAuthoMessage = "";
 //           if (Request.QueryString["Response"] != null)
 //           {
 //               try
 //               {
 //                   if (Request.QueryString["Response"].ToString() != "")
 //                   {




 //                       // Response=10001~1~31~MSBI0412001668~00000002.00~0300~Success
 //                       FileLogText("Admin", "Online Payment Response", " Response:" + Request.QueryString["Response"].ToString(), "");
 //                       // FileLogText("Admin", "Online Payment Response", " Error:" + ex.Message.Trim(), "");
 //                       String ResStr = Request.QueryString["Response"].ToString();
 //                       String[] strReqPara = ResStr.Split('~');
 //                       if (Convert.ToInt32(strReqPara[5]) == 0300)
 //                       {
 //                           int TransId = Convert.ToInt32(strReqPara[1]);
 //                           int UbilldeskOrderNo = Convert.ToInt32(strReqPara[2]);
 //                           string BillDeskRef = strReqPara[3];
 //                           int BillDeskAuthoStatus = Convert.ToInt32(strReqPara[5]);
 //                           BillDeskAuthoMessage = strReqPara[6];

 //                           string StrStatus = CallPaymentProc(TransId, UbilldeskOrderNo, BillDeskRef, BillDeskAuthoStatus, BillDeskAuthoMessage);
 //                           string[] response_arr = StrStatus.Split('$');
 //                           string Status = response_arr[0].ToString();
 //                           if (Status == "successfully")
 //                           {

 //                               lblResponseMsg.Text = "Transaction Done successfully,Bill Desk Refrence ID :" + BillDeskRef;
 //                               //Lblrefno.Text = BillDeskRef;
 //                               //LblCurruntBal.Text = response_arr[1];
 //                               //  ,Bill Desk Refrence ID :" + BillDeskRef;

 //                           }
 //                           else
 //                           {
 //                               lblResponseMsg.Text = "Transaction successfully from bill desk,Bill Desk Refrence ID :"+ BillDeskRef+",Transaction failed from UPASS :" + StrStatus;
 //                               // Divsts.Visible = false;
 //                           }





 //                       }

 //                       else if (Convert.ToInt32(strReqPara[5]) == 0399)
 //                       {

 //                           Response.Redirect("http://124.153.73.21/HwayLCOSMSUAT/Transaction/HwayTransLcoOnlinePayment.aspx");
 //                           return;
 //                       }

 //                       else
 //                       {
 //                           lblResponseMsg.Text = "Failed Transactionn from Bill Desk." + BillDeskAuthoMessage;
 //                           return;
 //                       }

 //                   }
 //               }

 //               catch
 //               {
 //                   lblResponseMsg.Text = "Cancel Transaction~Response can not be blank or zero";
 //                   return;
 //               }


 //           }
        
 //}

        
    }
}