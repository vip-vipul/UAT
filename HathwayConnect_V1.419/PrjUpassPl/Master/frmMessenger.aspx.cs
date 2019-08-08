using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassBLL.Master;
using System.Collections;
using System.IO;
using System.Configuration;
using System.Data.OracleClient;
using System.Net;
using PrjUpassBLL.Transaction;


namespace PrjUpassPl.Master
{
    public partial class frmMessenger : System.Web.UI.Page
    {
        string username;
        string catid;
        string oper_id;


        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "Communications";

            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["operator_id"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                Session["RightsKey"] = null;
                cls_BLL_messenger obj = new cls_BLL_messenger();
                String City = "";
                String UserLevel = "";
                String state = "";
                string cityuser = "";
                obj.GetCity(Session["username"].ToString(), out City, out state, out UserLevel, out  cityuser);
                ViewState["cityid"] = City;
                ViewState["UserLevel"] = UserLevel;
                ViewState["state"] = state;
                ViewState["cityuser"] = cityuser;
                ViewState["inboxclick"] = "I";
                bindInboxMessages();
                GetCCrecipiant();



            }
        }

        public void GetCCrecipiant()
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
                if (category_id == "11")
                {
                    str = " select a.var_usermst_username name   from aoup_lcopre_user_det a " +
                   " where  a.VAR_USERMST_MSGLEVEL='CN' " +
                    "union " +
                   " select a.var_usermst_username name   from aoup_lcopre_user_det a " +
                   " where a.num_usermst_stateid=" + ViewState["state"].ToString() + " and a.VAR_USERMST_MSGLEVEL='ST' " +
                   " union " +
                   " select a.var_usermst_username name   from aoup_lcopre_user_det a " +
                   " where a.var_usermst_msgcitystr like '%," + ViewState["cityid"].ToString() + ",%' and a.VAR_USERMST_MSGLEVEL='CT' ";

                }
                else if (category_id == "3")
                {
                    str = " select a.var_usermst_username name   from aoup_lcopre_user_det a " +
                    " where  a.VAR_USERMST_MSGLEVEL='CN' " +
                     "union " +
                    " select a.var_usermst_username name   from aoup_lcopre_user_det a " +
                    " where a.num_usermst_stateid=" + ViewState["state"].ToString() + " and a.VAR_USERMST_MSGLEVEL='ST' " +
                    " union " +
                    " select a.var_usermst_username name   from aoup_lcopre_user_det a " +
                    " where a.var_usermst_msgcitystr like '%," + ViewState["cityid"].ToString() + ",%' and a.VAR_USERMST_MSGLEVEL='CT' " +
                    " union " +
                    " select var_user_username name   from aoup_user_def a, aoup_operator_def b " +
                    " where a.num_user_operid=b.num_oper_clust_id " +
                    " and b.num_oper_id='" + operator_id + "'";
                }
                else
                {
                    if (ViewState["UserLevel"].ToString() == "CT")
                    {
                        str = " select a.var_usermst_username name   from aoup_lcopre_user_det a " +
                  " where  a.VAR_USERMST_MSGLEVEL='CN' " +
                   "union " +
                  " select a.var_usermst_username name   from aoup_lcopre_user_det a " +
                  " where a.num_usermst_stateid=" + ViewState["state"].ToString() + " and a.VAR_USERMST_MSGLEVEL='ST' ";
                    }
                    else if (ViewState["UserLevel"].ToString() == "ST")
                    {
                        str = " select a.var_usermst_username name   from aoup_lcopre_user_det a " +
                            " where  a.VAR_USERMST_MSGLEVEL='CN' ";
                    }
                }

                DataTable tbllco = GetResult(str);

                if (tbllco.Rows.Count > 0)
                {
                    String CCuser = "";

                    foreach (DataRow dr in tbllco.Rows)
                    {
                        CCuser += dr["name"] + ",";
                    }

                    ViewState["CCuser"] = CCuser;
                }
                else
                {
                    ViewState["CCuser"] = "";
                    return;
                }


            }
            catch (Exception ex)
            {
                Response.Write("Error  : " + ex.Message.ToString());
            }
            finally
            {
                conObj.Close();
                conObj.Dispose();
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
                DataTable tbllco = new DataTable();
                if (category_id == "11")
                {
                    str = "   SELECT var_lcomst_code name, var_lcomst_code opid ";  //num_lcomst_operid
                    str += "  FROM aoup_lcopre_lco_det a ,aoup_operator_def c,aoup_user_def u ";
                    str += "  WHERE a.num_lcomst_operid = c.num_oper_id and  a.num_lcomst_operid=u.num_user_operid and u.var_user_username=a.var_lcomst_code  ";

                    str += "  and c.num_oper_clust_id =" + operator_id;
                    str += "   union ";
                    str += "  select a.var_usermst_username name,a.var_usermst_username opid   from aoup_lcopre_user_det a ";
                    str += "  where a.var_usermst_msgcitystr like '%," + ViewState["cityid"].ToString() + ",%'  and a.VAR_USERMST_MSGLEVEL='CT' ";
                    // DataTable tbllco = GetResult(str);
                    ViewState["UserLevel"] = "D";
                }
                else if (category_id == "3")
                {
                    str = " select var_user_username name , var_user_username opid  from aoup_user_def a, aoup_operator_def b " +
                          " where a.num_user_operid=b.num_oper_clust_id " +
                          " and b.num_oper_id='" + operator_id + "' ";

                    tbllco = GetResult(str);

                    if (tbllco.Rows.Count > 0)
                    {

                    }
                    else
                    {
                            str = "SELECT   b.var_usermst_username name, b.var_usermst_username opid " +
                              "FROM    aoup_lcopre_user_det b, aoup_lcopre_city_def c,aoup_lcopre_state_def e" +
                              " WHERE    b.num_usermst_stateid = e.num_state_id  and b.num_usermst_cityid=c.num_city_id ";
                            //if (ViewState["UserLevel"].ToString() == "CT")
                            //{
                                str += "   and b.var_usermst_msgcitystr like '%," + ViewState["cityid"].ToString() + ",%'  and b.var_usermst_msglevel='CT' and var_usermst_msgcitystr is not null ";
                            //}

                            tbllco = GetResult(str);
                            if (tbllco.Rows.Count > 0)
                            {

                            }
                            else
                            {
                                str = "SELECT   b.var_usermst_username name, b.var_usermst_username opid " +
                                 "FROM    aoup_lcopre_user_det b, aoup_lcopre_city_def c,aoup_lcopre_state_def e" +
                                 " WHERE    b.num_usermst_stateid = e.num_state_id and b.num_usermst_cityid=c.num_city_id ";
                                //if (ViewState["UserLevel"].ToString() == "ST")
                                //{
                                    str += " and b.num_usermst_stateid=" + ViewState["state"].ToString() + " and b.var_usermst_msglevel='ST'";
                               //}

                                tbllco = GetResult(str);
                                if (tbllco.Rows.Count > 0)
                                {

                                }
                                else
                                {

                                }
                            }
                        
                    }
                }
                else
                {
                    str = "SELECT   b.var_usermst_username name, b.var_usermst_username opid " +
                            "FROM    aoup_lcopre_user_det b, aoup_lcopre_city_def c,aoup_lcopre_state_def e,aoup_operator_def d" +
                            " WHERE    b.num_usermst_stateid = e.num_state_id and b.num_usermst_cityid=c.num_city_id and b.num_usermst_operid=d.num_oper_id";
                    if (ViewState["UserLevel"].ToString() == "CT")
                    {
                        str += "   and b.num_usermst_cityid in (" + ViewState["cityid"].ToString().TrimEnd(',').TrimStart(',') + ") and b.var_usermst_msglevel='CT' and b.var_usermst_flag  in ('M')";
                    }
                    else if (ViewState["UserLevel"].ToString() == "ST")
                    {
                        str += " and b.num_usermst_stateid=" + ViewState["state"].ToString() + "  and d.num_oper_parentid=" + oper_id;
                    }
                }

                tbllco = GetResult(str);

                if (tbllco.Rows.Count > 0)
                {

                    multi.DataTextField = "name";
                    multi.DataValueField = "opid";

                    multi.DataSource = tbllco;
                    multi.DataBind();
                    if (category_id != "3")
                    {
                        multi.Items.Insert(0, new ListItem("-- Select All --", "selectall"));
                    }
                }
                else
                {
                    lblmsgbox.Text = "No data found !!";
                    popMsgBox.Show();
                    return;
                }


            }
            catch (Exception ex)
            {
                Response.Write("Error  : " + ex.Message.ToString());
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

        public void bindInboxMessages()
        {

            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            cls_BLL_messenger ob = new cls_BLL_messenger();

            DataTable dt = new DataTable();

            dt = ob.fillinbox(username, ViewState["cityid"].ToString().TrimStart(',').TrimEnd(','), ViewState["UserLevel"].ToString(), ViewState["state"].ToString(), "", "");

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;

            }
            if (dt.Rows.Count > 0)
            {
                grdInbox.DataSource = dt;
                grdInbox.DataBind();


            }
            else
            {
                grdInbox.EmptyDataText = "No Messages In Inbox";
                grdInbox.DataBind();
            }
            dt.Dispose();

        }

        public void bindOutboxMessages()
        {

            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            cls_BLL_messenger ob = new cls_BLL_messenger();

            DataTable dt = new DataTable();

            dt = ob.fillsentMsgs(username, "", "");

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;

            }
            if (dt.Rows.Count > 0)
            {
                grdSentitem.DataSource = dt;
                grdSentitem.DataBind();



            }
            else
            {

                grdSentitem.EmptyDataText = "No Messages In Outbox";
                grdSentitem.DataBind();
            }

            dt.Dispose();

        }

        protected void lnkinbox_Click(object sender, EventArgs e)
        {
            lbltextfrm.Text = "From :";
            indoxli.Attributes["class"] = "one  selected";
            sentli.Attributes["class"] = "two";
            //draftli.Attributes["class"] = "three";
            //trashli.Attributes["class"] = "four";
            inboxtab.Visible = true;
            sentitemtab.Visible = false;
            drafttab.Visible = false;
            trashtab.Visible = false;
            ViewState["inboxclick"] = "I";
            bindInboxMessages();


        }

        protected void lnkOpen_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            btnReply.Visible = true;
            ViewState["OpenRowindex"] = null;
            int rowindex = Convert.ToInt32((((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex);
            ViewState["OpenRowindex"] = rowindex;
            string date = grdInbox.Rows[rowindex].Cells[3].Text.Trim();
            string subject = grdInbox.Rows[rowindex].Cells[2].Text.Trim();
            string msg = grdInbox.Rows[rowindex].Cells[5].Text.Trim();
            string from = grdInbox.Rows[rowindex].Cells[0].Text.Trim();
            string file = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgfile")).Value;

            string msgType = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgType")).Value; //hdnINBOXMsgTO
            string msgTOAall = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgTO")).Value;
            string msgmainid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgId")).Value; //hdnINBOXMsgTO
            string msgsubid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXmsgsubId")).Value;
            string msgreadby = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnmsgread")).Value;

            String readby = msgreadby + Session["username"].ToString() + ",";
            HiddenField hdnRead = (HiddenField)grdInbox.Rows[rowindex].FindControl("hdnRead");

            if (hdnRead.Value.Trim() == "N")
            {
                cls_BLL_messenger obj = new cls_BLL_messenger();
                string result = obj.OpenMail(username, msgmainid, msgsubid, readby);
            }

            hdnRead.Value = "Y";
            grdInbox.Rows[rowindex].Cells[0].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[1].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[2].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[3].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[4].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[5].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[6].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[7].Font.Bold = false;
            //grdInbox.Rows[rowindex].Cells[8].Font.Bold = false;


            ViewState["file"] = file;
            lblfrmTO.Text = "From ";
            lblMsgdate.Text = date;
            lblMsgsubject.Text = subject;
            txtMsgOContent.Text = msg;
            lblMsgfrom.Text = from + "  To:" + msgTOAall.TrimEnd(',');
            if (file == "No File Found")
            {

                trAttach.Visible = false;
                // lblMsgfileName.Text = file;
            }
            else
            {
                trAttach.Visible = true;
                try
                {
                    lblMsgfileName.Text = file.Split('Œ')[1].ToString();
                }
                catch
                {
                    lblMsgfileName.Text = file;
                }
            }
            lblMsgType.Text = msgType;
            PopOpenMail.Show();


        }

        protected void lnkOpenOutBox_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            btnReply.Visible = false;
            ViewState["OpenRowindexOut"] = null;
            int rowindex = Convert.ToInt32((((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex);
            ViewState["OpenRowindexOut"] = rowindex;
            string date = grdSentitem.Rows[rowindex].Cells[0].Text.Trim();
            string subject = grdSentitem.Rows[rowindex].Cells[1].Text.Trim();
            string msg = grdSentitem.Rows[rowindex].Cells[3].Text.Trim();
            string from = username;
            string file = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXMsgfile")).Value;

            ViewState["file"] = file;
            string msgType = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOTBOXMsgType")).Value; //hdnINBOXMsgTO
            string msgTOAall = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXMsgTO")).Value;
            //hdnOUTBOXMsgTO  hdnOTBOXMsgType     hdnOUTBOXMsgfile
            lblfrmTO.Text = "To ";
            lblMsgdate.Text = date;
            lblMsgsubject.Text = subject;
            txtMsgOContent.Text = msg;
            lblMsgfrom.Text = msgTOAall.TrimEnd(',');
            if (file == "No File Found")
            {

                trAttach.Visible = false;
                // lblMsgfileName.Text = file;
            }
            else
            {
                trAttach.Visible = true;
                try
                {
                    lblMsgfileName.Text = file.Split('Œ')[1].ToString();
                }
                catch
                {
                    lblMsgfileName.Text = file;
                }
            }
            lblMsgType.Text = msgType;

            PopOpenMail.Show();


        }

        //lnkOpenOutBox_Click
        protected void lnkReply_Click(object sender, EventArgs e)
        {
            ViewState["UpexcFileName"] = null;
            hdnsendto.Value = "";
            lblheading.Text = "Reply";
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            int rowindex;

            rowindex = Convert.ToInt32((((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex);


            string date = grdInbox.Rows[rowindex].Cells[3].Text.Trim();
            string subject = grdInbox.Rows[rowindex].Cells[2].Text.Trim();
            string msg = grdInbox.Rows[rowindex].Cells[5].Text.Trim();
            string from = grdInbox.Rows[rowindex].Cells[0].Text.Trim() + ",";
            string mainid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgId")).Value;

            string Filetxt = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgfile")).Value;
            string messagetype = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgType")).Value;
            string messagetoall = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnmsgtoall")).Value;
            string messagetoallid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnmsgtoallid")).Value;


            string msgTOAall = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgTO")).Value; //hdnINBOXMsgTO
            string msgmainid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgId")).Value; //hdnINBOXMsgTO
            string msgsubid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXmsgsubId")).Value;
            string msgreadby = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnmsgread")).Value;
            string msgdeleteby = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdndelete")).Value;
            string msgrepliedby = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnmsgreply")).Value;

            String readby = msgreadby + Session["username"].ToString() + ",";
            ViewState["msgrepliedby"] = Session["username"].ToString() + ",";
            HiddenField hdnRead = (HiddenField)grdInbox.Rows[rowindex].FindControl("hdnRead");

            if (hdnRead.Value.Trim() == "N")
            {
                cls_BLL_messenger obj = new cls_BLL_messenger();
                string result = obj.OpenMail(username, msgmainid, msgsubid, readby);
            }

            hdnRead.Value = "Y";

            grdInbox.Rows[rowindex].Cells[0].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[1].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[2].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[3].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[4].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[5].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[6].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[7].Font.Bold = false;
            //grdInbox.Rows[rowindex].Cells[8].Font.Bold = false;
            // ViewState["UpexcFileName"] = Filetxt;
            string[] bindRply = msgTOAall.TrimEnd(',').Split(',');
            string FInalReply = "";

            if (messagetoall == "CN" || messagetoall == "ST" || messagetoall == "CT" || messagetoall == "D")
            {
            }
            else
            {
                foreach (string str in bindRply)
                {
                    if (username == str)
                    {

                    }
                    else
                    {
                        FInalReply += str + ",";
                    }
                }

                from += FInalReply;
            }

            if (from != "")
            {
                DataTable dtMulti = new DataTable();
                dtMulti.Columns.Add(new DataColumn("name"));
                dtMulti.Columns.Add(new DataColumn("opid"));

                string[] dtFinalString = from.Split(',');



                foreach (string str1 in dtFinalString)
                {
                    DataRow tempDr = dtMulti.NewRow();
                    tempDr["name"] = str1;
                    tempDr["opid"] = str1;
                    dtMulti.Rows.Add(tempDr);
                }


                if (dtMulti.Rows.Count > 0)
                {
                    multi.DataSource = dtMulti;
                    multi.DataTextField = "name";
                    multi.DataValueField = "opid";
                    multi.DataBind();

                }

            }
            else
            {
                Response.Redirect("~/ErrorPage.aspx");

                return;
            }

            hdnsendto.Value = from;

            //multi.SelectedValue = from;

            txtSubject.Text = subject;
            txtSubject.Enabled = false;
            ddlMessageType.SelectedValue = messagetype;
            ddlMessageType.Enabled = false;
            txtMessageContent.Text = "\n \n \n \n...........................\n" + msg;
            txtMessageContent.Focus();
            // txtFile.Text = Filetxt;

            ViewState["mainid"] = mainid;
            ViewState["Submainid"] = msgsubid;
            ViewState["NewMail"] = "R";
            ViewState["OpenRowindex"] = null;
            popMail.Show();

        }

        protected void btnReply_Click(object sender, EventArgs e)
        {
            ViewState["UpexcFileName"] = null;
            hdnsendto.Value = "";
            lblheading.Text = "Reply";
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            int rowindex;

            rowindex = Convert.ToInt32(ViewState["OpenRowindex"]);


            string date = grdInbox.Rows[rowindex].Cells[3].Text.Trim();
            string subject = grdInbox.Rows[rowindex].Cells[2].Text.Trim();
            string msg = grdInbox.Rows[rowindex].Cells[5].Text.Trim();
            string from = grdInbox.Rows[rowindex].Cells[0].Text.Trim() + ",";
            string mainid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgId")).Value;

            string Filetxt = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgfile")).Value;
            string messagetype = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgType")).Value;
            string messagetoall = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnmsgtoall")).Value;
            string messagetoallid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnmsgtoallid")).Value;


            string msgTOAall = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgTO")).Value; //hdnINBOXMsgTO
            string msgmainid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgId")).Value; //hdnINBOXMsgTO
            string msgsubid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXmsgsubId")).Value;
            string msgreadby = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnmsgread")).Value;
            string msgdeleteby = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdndelete")).Value;
            string msgrepliedby = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnmsgreply")).Value;

            String readby = msgreadby + Session["username"].ToString() + ",";
            ViewState["msgrepliedby"] = Session["username"].ToString() + ",";
            HiddenField hdnRead = (HiddenField)grdInbox.Rows[rowindex].FindControl("hdnRead");

            if (hdnRead.Value.Trim() == "N")
            {
                cls_BLL_messenger obj = new cls_BLL_messenger();
                string result = obj.OpenMail(username, msgmainid, msgsubid, readby);
            }

            hdnRead.Value = "Y";

            grdInbox.Rows[rowindex].Cells[0].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[1].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[2].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[3].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[4].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[5].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[6].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[7].Font.Bold = false;
            //grdInbox.Rows[rowindex].Cells[8].Font.Bold = false;
            // ViewState["UpexcFileName"] = Filetxt;
            string[] bindRply = msgTOAall.TrimEnd(',').Split(',');
            string FInalReply = "";

            if (messagetoall == "CN" || messagetoall == "ST" || messagetoall == "CT" || messagetoall == "D")
            {
            }
            else
            {
                foreach (string str in bindRply)
                {
                    if (username == str)
                    {

                    }
                    else
                    {
                        FInalReply += str + ",";
                    }
                }

                from += FInalReply;
            }

            if (from != "")
            {
                DataTable dtMulti = new DataTable();
                dtMulti.Columns.Add(new DataColumn("name"));
                dtMulti.Columns.Add(new DataColumn("opid"));

                string[] dtFinalString = from.Split(',');



                foreach (string str1 in dtFinalString)
                {
                    DataRow tempDr = dtMulti.NewRow();
                    tempDr["name"] = str1;
                    tempDr["opid"] = str1;
                    dtMulti.Rows.Add(tempDr);
                }


                if (dtMulti.Rows.Count > 0)
                {
                    multi.DataSource = dtMulti;
                    multi.DataTextField = "name";
                    multi.DataValueField = "opid";
                    multi.DataBind();

                }

            }
            else
            {
                Response.Redirect("~/ErrorPage.aspx");

                return;
            }

            hdnsendto.Value = from;

            //multi.SelectedValue = from;

            txtSubject.Text = subject;
            txtSubject.Enabled = false;
            ddlMessageType.SelectedValue = messagetype;
            ddlMessageType.Enabled = false;
            txtMessageContent.Text = "\n \n \n \n...........................\n" + msg;
            txtMessageContent.Focus();
            // txtFile.Text = Filetxt;

            ViewState["mainid"] = mainid;
            ViewState["Submainid"] = msgsubid;
            ViewState["NewMail"] = "R";
            ViewState["OpenRowindex"] = null;
            popMail.Show();
        }//btnReply_Click

        protected void imgAttach_Click(object sender, EventArgs e)
        {

            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }



            int rowindex;

            rowindex = Convert.ToInt32((((GridViewRow)(((ImageButton)(sender)).Parent.BindingContainer))).RowIndex);


            string file = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgfile")).Value;

            if (file == "No File Found")
            {

                trAttach.Visible = false;
                lblmsgbox.Text = "No attachment available..";
                popMsgBox.Show();
            }
            else
            {
                string getExtension = System.IO.Path.GetExtension(file).ToLower();

                downloadFiles(file, getExtension);

            }


        }

        protected void imgAttachSent_Click(object sender, EventArgs e)
        {

            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }



            int rowindex;

            rowindex = Convert.ToInt32((((GridViewRow)(((ImageButton)(sender)).Parent.BindingContainer))).RowIndex);


            string file = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXMsgfile")).Value;


            if (file == "No File Found")
            {

                trAttach.Visible = false;
                lblmsgbox.Text = "No attachment available..";
                popMsgBox.Show();
            }
            else
            {
                string getExtension = System.IO.Path.GetExtension(file).ToLower();

                downloadFiles(file, getExtension);

            }


        }

        protected void btnDownloadAttachmentPop_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }




            string file = Convert.ToString(ViewState["file"]);


            if (file == "No File Found")
            {

                trAttach.Visible = false;
                lblmsgbox.Text = "No attachment available..";
                popMsgBox.Show();
            }
            else
            {
                string getExtension = System.IO.Path.GetExtension(file).ToLower();

                downloadFiles(file, getExtension);
            }

        }

        public void downloadFiles(string file, string getExtension)
        {
            if (getExtension == ".jpg" || getExtension == ".jpeg" || getExtension == ".png" || getExtension == ".gif")
            {
                string localFilename = @"C:\inetpub\wwwroot\yesbank\messengerFiles\" + file;
                string FileName = file; // It's a file name displayed on downloaded file on client side.

                using (FileStream fileStream = File.OpenRead(localFilename))
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    Response.Clear();
                    Response.ContentType = "image/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + "");
                    Response.BinaryWrite(memStream.ToArray());
                    Response.Flush();
                    Response.Close();
                    Response.End();
                }
            }
            else if (getExtension == ".xls" || getExtension == ".xlsx")
            {
                string localFilename = @"C:\inetpub\wwwroot\yesbank\messengerFiles\" + file;
                string FileName = file; // It's a file name displayed on downloaded file on client side.

                using (FileStream fileStream = File.OpenRead(localFilename))
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + "");
                    Response.BinaryWrite(memStream.ToArray());
                    Response.Flush();
                    Response.Close();
                    Response.End();
                }

                //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                //response.ClearContent();
                //response.Clear();
                //response.ContentType = "application/x-msexcel";
                //response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
                //response.TransmitFile(localFilename);
                //response.Flush();
                //response.End();
            }
            else if (getExtension == ".pdf")
            {
                string localFilename = @"C:\inetpub\wwwroot\yesbank\messengerFiles\" + file;
                string FileName = file; // It's a file name displayed on downloaded file on client side.

                using (FileStream fileStream = File.OpenRead(localFilename))
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + "");
                    Response.BinaryWrite(memStream.ToArray());
                    Response.Flush();
                    Response.Close();
                    Response.End();
                }

            }
            else if (getExtension == ".doc" || getExtension == ".docx")
            {
                string localFilename = @"C:\inetpub\wwwroot\yesbank\messengerFiles\" + file;
                string FileName = file; // It's a file name displayed on downloaded file on client side.

                using (FileStream fileStream = File.OpenRead(localFilename))
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + "");
                    Response.BinaryWrite(memStream.ToArray());
                    Response.Flush();
                    Response.Close();
                    Response.End();
                }
            }
            else if (getExtension == ".zip" || getExtension == ".rar")
            {
                string localFilename = @"C:\inetpub\wwwroot\yesbank\messengerFiles\" + file;
                string FileName = file; // It's a file name displayed on downloaded file on client side.

                using (FileStream fileStream = File.OpenRead(localFilename))
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + "");
                    Response.BinaryWrite(memStream.ToArray());
                    Response.Flush();
                    Response.Close();
                    Response.End();
                }
            }
            else if (getExtension == ".txt")
            {
                string localFilename = @"C:\inetpub\wwwroot\yesbank\messengerFiles\" + file;
                string FileName = file; // It's a file name displayed on downloaded file on client side.

                using (FileStream fileStream = File.OpenRead(localFilename))
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    Response.Clear();
                    Response.ContentType = "text/plain";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + "");
                    Response.BinaryWrite(memStream.ToArray());
                    Response.Flush();
                    Response.Close();
                    Response.End();
                }


            }

            else
            {
                lblmsgbox.Text = "Error while downloading the file !!";
                popMsgBox.Show();

            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            string CommandName = btn.CommandName;
            int rowindex = Convert.ToInt32((((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex);
            ViewState["deletRow"] = rowindex;
            if (CommandName == "Inbox")
            {
                ViewState["deletetype"] = "I";
            }
            else
            {
                ViewState["deletetype"] = "S";
            }
            popConfirm.Show();
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            int rowindex = Convert.ToInt32(ViewState["deletRow"]);

            string main = "";
            string sub = "";
            string to = "";
            String delete = "";
            ViewState["NewMail"] = "D";
            if (Convert.ToString(ViewState["deletetype"]) == "I")
            {
                main = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgId")).Value;
                sub = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXmsgsubId")).Value;
                to = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgTO")).Value;
                delete = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdndelete")).Value;
                if (delete != "")
                {
                    delete = delete + Convert.ToString(Session["username"]) + ",";
                }
                else
                {
                    delete = Convert.ToString(Session["username"]) + ",";
                }
            }
            else
            {
                main = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXMsgId")).Value;
                sub = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXmsgsubId")).Value;
                to = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXMsgTO")).Value;
                delete = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXdelete")).Value;
                if (delete != "")
                {
                    delete = delete + Convert.ToString(Session["username"]) + ",";
                }
                else
                {
                    delete = Convert.ToString(Session["username"]) + ",";
                }
            }


            Hashtable htDATAsent = getMailparams();
            htDATAsent.Add("mainid", main);
            htDATAsent.Add("subid", sub);
            htDATAsent.Add("deletetype", ViewState["deletetype"]);
            htDATAsent.Add("delete", delete);

            cls_BLL_messenger obj = new cls_BLL_messenger();
            string result = obj.sentMailDatains(username, htDATAsent);

            if (result == "9999")
            {
                if (Convert.ToString(ViewState["deletetype"]) == "I")
                {
                    bindInboxMessages();
                }
                else
                {
                    bindOutboxMessages();
                }

                popMail.Hide();
                lblmsgbox.Text = "Message deleted successfully..";
                popMsgBox.Show();

                ViewState["deletRow"] = null;
            }
            else
            {

                //error while deleting;

            }
            popConfirm.Hide();
        }

        protected void lnksentitem_Click(object sender, EventArgs e)
        {
            lbltextfrm.Text = "To :";
            indoxli.Attributes["class"] = "one";
            sentli.Attributes["class"] = "two selected";
            //draftli.Attributes["class"] = "three";
            //trashli.Attributes["class"] = "four";
            inboxtab.Visible = false;
            sentitemtab.Visible = true;
            drafttab.Visible = false;
            trashtab.Visible = false;
            ViewState["inboxclick"] = "S";
            bindOutboxMessages();

        }

        protected void lnkdraft_Click(object sender, EventArgs e)
        {
            indoxli.Attributes["class"] = "one";
            sentli.Attributes["class"] = "two";
            //draftli.Attributes["class"] = "three selected";
            //trashli.Attributes["class"] = "four";
            inboxtab.Visible = false;
            sentitemtab.Visible = false;
            drafttab.Visible = true;
            trashtab.Visible = false;

        }

        protected void lnktrash_Click(object sender, EventArgs e)
        {
            inboxtab.Visible = false;
            sentitemtab.Visible = false;
            drafttab.Visible = false;
            trashtab.Visible = true;
            indoxli.Attributes["class"] = "one";
            sentli.Attributes["class"] = "two";
            //draftli.Attributes["class"] = "three";
            //trashli.Attributes["class"] = "four selected";
        }

        protected void btnNewMail_Click(object sender, EventArgs e)
        {
            ViewState["UpexcFileName"] = null;
            ViewState["mainid"] = null;
            ViewState["Submainid"] = null;

            multi.Items.Clear();
            FillLcoDetails();
            txtMessageContent.Text = "";
            txtSubject.Text = "";
            txtSubject.Enabled = true;
            ddlMessageType.Enabled = true;
            ddlMessageType.SelectedIndex = 0;
            txtFile.Text = "";
            hdnsendto.Value = "";
            ViewState["NewMail"] = "Y";
            popMail.Show();
        }

        public void uploadExcel()
        {


            try
            {
                string filenameID = "excl_" + Session["username"].ToString() + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + attachfile.PostedFile.FileName;
                attachfile.SaveAs("C:/inetpub/wwwroot/yesbank/messengerFiles/" + filenameID);
                ViewState["UpexcFileName"] = filenameID;
            }
            catch (Exception ex)
            {

                return;
            }
        }

        public void uploadWord()
        {


            try
            {
                string filenameID = "Word_" + Session["username"].ToString() + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + attachfile.PostedFile.FileName;
                attachfile.SaveAs("C:/inetpub/wwwroot/yesbank/messengerFiles/" + filenameID);
                ViewState["UpexcFileName"] = filenameID;
            }
            catch (Exception ex)
            {

                return;
            }
        }

        public void uploadPdf()
        {
            if (attachfile.HasFile)
            {

                string strpath = System.IO.Path.GetExtension(attachfile.FileName).ToLower();
                if (strpath != ".pdf")
                {
                    // msgboxstr("Only PDF file are accepted ");
                    return;
                }
                else
                {
                    string filenameID = "pdf_" + Session["username"].ToString() + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + strpath;//Path.GetFileName(fupidproof.PostedFile.FileName);

                    attachfile.SaveAs("C:/inetpub/wwwroot/yesbank/messengerFiles/" + filenameID);
                    ViewState["UpexcFileName"] = filenameID;

                }
            }
        }

        public void uploadImage()
        {
            if (attachfile.HasFile)
            {

                string strpath = System.IO.Path.GetExtension(attachfile.FileName).ToLower();
                if (strpath != ".jpg" && strpath != ".jpeg" && strpath != ".gif" && strpath != ".png" && strpath != ".pdf")
                {
                    //msgboxstr("Only image formats (jpg, png, gif) are accepted ");
                    return;
                }
                else
                {
                    string filenameID = "img_" + Session["username"].ToString() + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + strpath;//Path.GetFileName(fupidproof.PostedFile.FileName);



                    Stream strm = attachfile.PostedFile.InputStream;

                    attachfile.SaveAs("C:/inetpub/wwwroot/yesbank/messengerFiles/" + filenameID);

                    ViewState["UpexcFileName"] = filenameID;
                }
            }
        }

        public void uploadText()
        {
            if (attachfile.HasFile)
            {

                string strpath = System.IO.Path.GetExtension(attachfile.FileName).ToLower();
                if (strpath != ".txt")
                {
                    //msgboxstr("Only image formats (jpg, png, gif) are accepted ");
                    return;
                }
                else
                {
                    string filenameID = "Note_" + Session["username"].ToString() + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + strpath;//Path.GetFileName(fupidproof.PostedFile.FileName);



                    Stream strm = attachfile.PostedFile.InputStream;

                    attachfile.SaveAs("C:/inetpub/wwwroot/yesbank/messengerFiles/" + filenameID);

                    ViewState["UpexcFileName"] = filenameID;
                }
            }
        }

        public void uploadZip()
        {
            if (attachfile.HasFile)
            {

                string strpath = System.IO.Path.GetExtension(attachfile.FileName).ToLower();
                if (strpath != ".zip" || strpath != ".rar")
                {
                    //msgboxstr("Only image formats (jpg, png, gif) are accepted ");
                    return;
                }
                else
                {
                    string filenameID = "Compress_" + Session["username"].ToString() + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + strpath;//Path.GetFileName(fupidproof.PostedFile.FileName);



                    Stream strm = attachfile.PostedFile.InputStream;

                    attachfile.SaveAs("C:/inetpub/wwwroot/yesbank/messengerFiles/" + filenameID);

                    ViewState["UpexcFileName"] = filenameID;
                }
            }
        }

        public void msgfileattachment()
        {
            string getExtension = System.IO.Path.GetExtension(attachfile.FileName).ToLower();
            if (getExtension == ".xls" || getExtension == ".xlsx")
            {
                uploadExcel();
            }
            else if (getExtension == ".pdf")
            {
                uploadPdf();
            }
            else if (getExtension == ".jpg" || getExtension == ".jpeg" || getExtension == ".png" || getExtension == ".gif")
            {
                uploadImage();
            }
            else if (getExtension == ".doc" || getExtension == ".docx")
            {

                uploadWord();
            }
            else if (getExtension == ".zip" || getExtension == ".rar")
            {

                uploadZip();
            }
            else if (getExtension == ".txt")
            {

                uploadText();
            }
            else
            {

            }
        }

        public Hashtable getMailparams()
        {

            Hashtable htmailparams = new Hashtable();

            string msgto = hdnsendto.Value;
            htmailparams.Add("msgtocc", Convert.ToString(ViewState["CCuser"]));
            if (msgto == "selectall,")
            {
                htmailparams.Add("msgto", "");
                if (ViewState["UserLevel"].ToString() == "CN")
                {
                    htmailparams.Add("msgtoall", ViewState["UserLevel"].ToString());
                    htmailparams.Add("msgtoallID", "");
                }
                if (ViewState["UserLevel"].ToString() == "ST")
                {
                    htmailparams.Add("msgtoall", ViewState["UserLevel"].ToString());
                    htmailparams.Add("msgtoallID", ViewState["state"].ToString());
                }
                if (ViewState["UserLevel"].ToString() == "CT")
                {
                    htmailparams.Add("msgtoall", ViewState["UserLevel"].ToString());
                    htmailparams.Add("msgtoallID", ViewState["cityuser"].ToString());
                }
                if (ViewState["UserLevel"].ToString() == "D")
                {
                    htmailparams.Add("msgtoall", ViewState["UserLevel"].ToString());
                    htmailparams.Add("msgtoallID", Session["operator_id"].ToString());
                }
            }
            else
            {
                htmailparams.Add("msgtoallID", "");
                htmailparams.Add("msgtoall", "");
                htmailparams.Add("msgto", msgto);
            }

            htmailparams.Add("city", ViewState["cityuser"].ToString());
            htmailparams.Add("state", ViewState["state"].ToString());
            htmailparams.Add("msgsubject", txtSubject.Text);
            if (ViewState["UpexcFileName"] != null)
            {
                htmailparams.Add("msgfile", ViewState["UpexcFileName"].ToString());
            }
            else
            {
                htmailparams.Add("msgfile", "No File Found");
            }
            htmailparams.Add("msgType", ddlMessageType.SelectedItem.Text);
            htmailparams.Add("msgContecnt", txtMessageContent.Text);
            htmailparams.Add("NewMail", Convert.ToString(ViewState["NewMail"]));
            return htmailparams;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {

          string  blusername = SecurityValidation.chkData("T", txtfrom.Text+""+txtSubject.Text+""+txtMsgOContent.Text);
            if (blusername.Length > 0)
            {
                trErr.Visible = true;
                lblerror.Text = blusername;
                popMail.Show();   
                return;
            }

            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {
                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }
            string filenameID = "";
            try
            {
                if (attachfile.HasFile)
                {
                    filenameID = "file_" + Session["username"].ToString() + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + "Œ" + attachfile.PostedFile.FileName;

                    ViewState["UpexcFileName"] = filenameID;

                }
                else
                {
                    if (ViewState["UpexcFileName"] == null)
                    {
                        ViewState["UpexcFileName"] = "No File Found";
                    }
                    else
                    {
                        filenameID = ViewState["UpexcFileName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                ViewState["UpexcFileName"] = "No File Found";
            }

            if (hdnsendto.Value == "" || hdnsendto.Value == "0")
            {
                trErr.Visible = true;
                lblerror.Text = "Reciepient Cannot Be Null!";
                popMail.Show();
                return;
            }

            if (multi.SelectedIndex == -1)
            {
                trErr.Visible = true;
                lblerror.Text = "Reciepient Cannot Be Null!";
                popMail.Show();
                return;
            }

            if (txtSubject.Text == "")
            {
                trErr.Visible = true;
                lblerror.Text = "Message Subject Cannot Be Null!";
                popMail.Show();
                return;
            }

            if (ddlMessageType.SelectedIndex == 0)
            {
                trErr.Visible = true;
                lblerror.Text = "Message Type Cannot Be Null!";
                popMail.Show();
                return;
            }
            if (txtMessageContent.Text == "")
            {
                trErr.Visible = true;
                lblerror.Text = "Message Body Cannot Be Null!";
                popMail.Show();
                return;
            }



            hdnsendto.Value = hdnsendto.Value.TrimEnd(',') + ",";


            Hashtable htDATAsent = getMailparams();
            if (ViewState["mainid"] != null)
            {
                htDATAsent.Add("mainid", ViewState["mainid"].ToString());
                htDATAsent.Add("subid", ViewState["Submainid"].ToString());
                htDATAsent.Add("repliedby", ViewState["msgrepliedby"].ToString());
            }
            cls_BLL_messenger obj = new cls_BLL_messenger();
            string result = obj.sentMailDatains(username, htDATAsent);

            if (result == "9999")
            {

                popMail.Hide();
                lblmsgbox.Text = "Message sent successfully..";
                popMsgBox.Show();
                if (ViewState["inboxclick"].ToString() == "I")
                {
                    // bindInboxMessages();
                    lnksentitem_Click(sender, e);
                }
                else
                {
                    bindOutboxMessages();
                }

                if (filenameID != "" && filenameID != "No File Found")
                {
                    attachfile.SaveAs("C:/inetpub/wwwroot/yesbank/messengerFiles/" + filenameID);
                }
            }
            else
            {
                popMail.Hide();
                lblmsgbox.Text = "Error while sending Message";
                popMsgBox.Show();
                //error while sending mail;
            }

        }

        protected void grdInbox_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //  e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[4].Visible = true;
                // Label lblmsgs = (Label)e.Row.Cells[4].FindControl("lblmsgs");

                HiddenField hdnINBOXMsgfile = (HiddenField)e.Row.FindControl("hdnINBOXMsgfile");
                HiddenField hdnmsgread = (HiddenField)e.Row.FindControl("hdnmsgread");
                HiddenField hdnRead = (HiddenField)e.Row.FindControl("hdnRead");
                HiddenField hdnReply = (HiddenField)e.Row.FindControl("hdnmsgreply");
                HiddenField hdndelete = (HiddenField)e.Row.FindControl("hdndelete");
                //  String Readby = Session["username"].ToString() + ",";
                String Readby = Session["username"].ToString() + ",";
                String Replyby = Session["username"].ToString() + ",";
                if (hdnReply.Value.Trim().Contains(Readby))
                {
                    e.Row.Cells[7].Text = "Replied";
                  
                }
                else
                {
                    e.Row.Cells[7].Text = "";
                    
                }
                if (hdnmsgread.Value.Trim().Contains(Readby))
                {
                    hdnRead.Value = "Y";
                    e.Row.Cells[0].Font.Bold = false;
                    e.Row.Cells[1].Font.Bold = false;
                    e.Row.Cells[2].Font.Bold = false;
                    e.Row.Cells[3].Font.Bold = false;
                    e.Row.Cells[4].Font.Bold = false;
                    e.Row.Cells[5].Font.Bold = false;
                    e.Row.Cells[6].Font.Bold = false;
                    Button lnk2 = (Button)e.Row.FindControl("lnkDelete");
                    lnk2.Visible = true;
                    //  e.Row.Cells[7].Font.Bold = false;
                    //  e.Row.Cells[8].Font.Bold = false;
                }
                else
                {
                    hdnRead.Value = "N";
                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[1].Font.Bold = true;
                    e.Row.Cells[2].Font.Bold = true;
                    e.Row.Cells[3].Font.Bold = true;
                    e.Row.Cells[4].Font.Bold = true;
                    e.Row.Cells[5].Font.Bold = true;
                    e.Row.Cells[6].Font.Bold = true;
                    // e.Row.Cells[7].Font.Bold = true;
                    //   e.Row.Cells[8].Font.Bold = true;
                    Button lnk2 = (Button)e.Row.FindControl("lnkDelete");
                    lnk2.Visible = false;
                }



                ImageButton imgAttach = (ImageButton)e.Row.FindControl("imgAttach");
                if (hdnINBOXMsgfile.Value == "No File Found")
                {
                    imgAttach.Visible = false;
                }

                //if (e.Row.Cells[3].Text.Length > 30)
                //{
                //    lblmsgs.Text = e.Row.Cells[3].Text.Substring(0, 30) + "....";
                //}
                //else
                //{
                //    lblmsgs.Text = e.Row.Cells[3].Text;

                //}
            }
        }

        protected void grdSentitem_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                Label lblmsgs = (Label)e.Row.Cells[4].FindControl("lblmsgs");
                HiddenField hdnOUTBOXMsgfile = (HiddenField)e.Row.FindControl("hdnOUTBOXMsgfile");
                ImageButton imgAttach = (ImageButton)e.Row.FindControl("imgAttach");
                if (hdnOUTBOXMsgfile.Value == "No File Found")
                {
                    imgAttach.Visible = false;
                }
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.TrimEnd(',');
                if (e.Row.Cells[3].Text.Length > 30)
                {
                    lblmsgs.Text = e.Row.Cells[3].Text.Substring(0, 30) + "....";
                }
                else
                {
                    lblmsgs.Text = e.Row.Cells[3].Text;

                }
            }
        }

        protected void grdInbox_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdInbox.PageIndex = e.NewPageIndex;
            bindInboxMessages();
            GetCCrecipiant();
        }

        protected void grdSentitem_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSentitem.PageIndex = e.NewPageIndex;
            bindOutboxMessages();
        }


        protected void grdInbox_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdInbox, "Select$" + e.Row.RowIndex);
            }
        }

        public void grdSentitem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (ViewState["PreviousRowIndex1"] != null)
            {
                var previousRowIndex = (int)ViewState["PreviousRowIndex1"];
                GridViewRow PreviousRow = grdSentitem.Rows[previousRowIndex];
            }
            int rowindex = 0;
            if (ViewState["OpenRowindexOut"].ToString() != "")
            {
                rowindex = Convert.ToInt32(ViewState["OpenRowindexOut"]);
            }
            else
            {
                rowindex = Convert.ToInt32(e.CommandArgument);
            }
            GridViewRow gvRow = grdSentitem.Rows[rowindex];
            ViewState["PreviousRowIndex1"] = rowindex;

            btnReply.Visible = true;
            ViewState["OpenRowindex"] = null;

            ViewState["OpenRowindex"] = rowindex;
            string date = grdSentitem.Rows[rowindex].Cells[0].Text.Trim();
            string subject = grdSentitem.Rows[rowindex].Cells[1].Text.Trim();
            string msg = grdSentitem.Rows[rowindex].Cells[3].Text.Trim();
            string from = username;
            string file = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXMsgfile")).Value;

            ViewState["file"] = file;
            string msgType = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOTBOXMsgType")).Value; //hdnINBOXMsgTO
            string msgTOAall = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXMsgTO")).Value;
            //hdnOUTBOXMsgTO  hdnOTBOXMsgType     hdnOUTBOXMsgfile
            lblfrmTO.Text = "To ";
            lblMsgdate.Text = date;
            lblMsgsubject.Text = subject;
            txtMsgOContent.Text = msg;
            lblMsgfrom.Text = msgTOAall.TrimEnd(',');
            if (file == "No File Found")
            {

                trAttach.Visible = false;
                // lblMsgfileName.Text = file;
            }
            else
            {
                trAttach.Visible = true;
                try
                {
                    lblMsgfileName.Text = file.Split('Œ')[1].ToString();
                }
                catch
                {
                    lblMsgfileName.Text = file;
                }
            }
            lblMsgType.Text = msgType;


            PopOpenMail.Show();
        }

        protected void grdSentitem_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdSentitem, "Select$" + e.Row.RowIndex);
            }
        }

        protected void grdSentitem_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["operator_id"] != null && Session["category"] != null)
            {

                username = Convert.ToString(Session["username"]);
                catid = Convert.ToString(Session["category"]);
                oper_id = Convert.ToString(Session["lcoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            btnReply.Visible = false;
            ViewState["OpenRowindexOut"] = null;
            int rowindex = grdSentitem.SelectedRow.RowIndex;
            GridViewRow row = grdSentitem.Rows[rowindex];
            //  int rowindex = Convert.ToInt32((((GridViewRow)(((Button)(sender)).Parent.BindingContainer))).RowIndex);
            ViewState["OpenRowindexOut"] = rowindex;
            string to = grdSentitem.Rows[rowindex].Cells[0].Text.Trim();
            string msg = grdSentitem.Rows[rowindex].Cells[5].Text.Trim();
            string subject = grdSentitem.Rows[rowindex].Cells[2].Text.Trim();
            string date = grdSentitem.Rows[rowindex].Cells[3].Text.Trim();
            string from = username;
            string file = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXMsgfile")).Value;

            ViewState["file"] = file;
            string msgType = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOTBOXMsgType")).Value; //hdnINBOXMsgTO
            string msgTOAall = ((HiddenField)grdSentitem.Rows[rowindex].FindControl("hdnOUTBOXMsgTO")).Value;
            //hdnOUTBOXMsgTO  hdnOTBOXMsgType     hdnOUTBOXMsgfile
            lblfrmTO.Text = "To ";
            lblMsgdate.Text = date;
            lblMsgsubject.Text = subject;
            txtMsgOContent.Text = msg;
            lblMsgfrom.Text = msgTOAall.TrimEnd(',');
            if (file == "No File Found")
            {

                trAttach.Visible = false;
                // lblMsgfileName.Text = file;
            }
            else
            {
                trAttach.Visible = true;
                try
                {
                    lblMsgfileName.Text = file.Split('Œ')[1].ToString();
                }
                catch
                {
                    lblMsgfileName.Text = file;
                }
            }
            lblMsgType.Text = msgType;

            PopOpenMail.Show();

        }

        protected void grdInbox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = grdInbox.SelectedRow.RowIndex;
            GridViewRow row = grdInbox.Rows[rowindex];
            ViewState["PreviousRowIndex"] = rowindex;

            btnReply.Visible = true;
            ViewState["OpenRowindex"] = null;

            ViewState["OpenRowindex"] = rowindex;
            string date = grdInbox.Rows[rowindex].Cells[3].Text.Trim();
            string subject = grdInbox.Rows[rowindex].Cells[2].Text.Trim();
            string msg = grdInbox.Rows[rowindex].Cells[5].Text.Trim();
            string from = grdInbox.Rows[rowindex].Cells[0].Text.Trim();
            string file = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgfile")).Value;

            string msgType = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgType")).Value; //hdnINBOXMsgTO
            string msgTOAall = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgTO")).Value;
            string msgmainid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXMsgId")).Value; //hdnINBOXMsgTO
            string msgsubid = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnINBOXmsgsubId")).Value;
            string msgreadby = ((HiddenField)grdInbox.Rows[rowindex].FindControl("hdnmsgread")).Value;

            String readby = msgreadby + Session["username"].ToString() + ",";
            HiddenField hdnRead = (HiddenField)grdInbox.Rows[rowindex].FindControl("hdnRead");

            if (hdnRead.Value.Trim() == "N")
            {
                cls_BLL_messenger obj = new cls_BLL_messenger();
                string result = obj.OpenMail(username, msgmainid, msgsubid, readby);
            }

            hdnRead.Value = "Y";
            grdInbox.Rows[rowindex].Cells[0].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[1].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[2].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[3].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[4].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[5].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[6].Font.Bold = false;
            grdInbox.Rows[rowindex].Cells[7].Font.Bold = false;
            //grdInbox.Rows[rowindex].Cells[8].Font.Bold = false;

            ViewState["file"] = file;
            lblfrmTO.Text = "From ";
            lblMsgdate.Text = date;
            lblMsgsubject.Text = subject;
            txtMsgOContent.Text = msg;
            lblMsgfrom.Text = from + "  To:" + msgTOAall.TrimEnd(',');
            if (file == "No File Found")
            {

                trAttach.Visible = false;
                // lblMsgfileName.Text = file;
            }
            else
            {
                trAttach.Visible = true;
                try
                {
                    lblMsgfileName.Text = file.Split('Œ')[1].ToString();
                }
                catch
                {
                    lblMsgfileName.Text = file;
                }
            }
            lblMsgType.Text = msgType;


            PopOpenMail.Show();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            string blusername = SecurityValidation.chkData("T", txtfrom.Text+" "+txtSubject.Text+" "+txtsubjectsearch.Text+" "+txtMessageContent);
            if (blusername.Length > 0)
            {
                grdInbox.EmptyDataText = blusername;
                return;
            }

            cls_BLL_messenger ob = new cls_BLL_messenger();
            DataTable dt = new DataTable();

            if (grdInbox.Visible == true)
            {

                if (txtfrom.Text != "" && txtsubjectsearch.Text != "")
                {
                    dt = ob.fillinbox(username, ViewState["cityid"].ToString(), ViewState["UserLevel"].ToString(), ViewState["state"].ToString(), txtfrom.Text, txtsubjectsearch.Text);
                }
                else if (txtsubjectsearch.Text != "")
                {
                    dt = ob.fillinbox(username, ViewState["cityid"].ToString(), ViewState["UserLevel"].ToString(), ViewState["state"].ToString(), "", txtsubjectsearch.Text);
                }
                else if (txtfrom.Text != "")
                {
                    dt = ob.fillinbox(username, ViewState["cityid"].ToString(), ViewState["UserLevel"].ToString(), ViewState["state"].ToString(), txtfrom.Text, "");
                }
                else
                {
                    dt = ob.fillinbox(username, ViewState["cityid"].ToString(), ViewState["UserLevel"].ToString(), ViewState["state"].ToString(), "", "");
                }

                if (dt == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;

                }
                if (dt.Rows.Count > 0)
                {
                    grdInbox.DataSource = dt;
                    grdInbox.DataBind();


                }
                else
                {
                    grdInbox.EmptyDataText = "No Messages In Inbox";
                    grdInbox.DataBind();
                }
                dt.Dispose();
            }
            else
            {

                if (txtfrom.Text != "" && txtsubjectsearch.Text != "")
                {
                    dt = ob.fillsentMsgs(username, txtfrom.Text, txtsubjectsearch.Text);
                }
                else if (txtsubjectsearch.Text != "")
                {
                    dt = ob.fillsentMsgs(username, "", txtsubjectsearch.Text);
                }
                else if (txtfrom.Text != "")
                {
                    dt = ob.fillsentMsgs(username, txtfrom.Text, "");
                }
                else
                {
                    dt = ob.fillsentMsgs(username, "", "");
                }


                if (dt == null)
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;

                }
                if (dt.Rows.Count > 0)
                {
                    grdSentitem.DataSource = dt;
                    grdSentitem.DataBind();

                }
                else
                {

                    grdSentitem.EmptyDataText = "No Messages In Outbox";
                    grdSentitem.DataBind();
                }

                dt.Dispose();
            }

        }
    }
}