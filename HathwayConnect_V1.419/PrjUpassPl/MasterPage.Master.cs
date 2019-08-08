using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PrjUpassDAL.Helper;
using System.IO;
using PrjUpassBLL.Transaction;
using PrjUpassBLL.Master;
using PrjUpassDAL.Authentication;
using PrjUpassBLL.Authentication;
using System.Data.OracleClient;
using System.Collections.Specialized;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PrjUpassPl
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {

        string operator_id = "";
        string username = "";
        string user_id = "";
        string category = "";
        //sets page heading
        public string PageHeading
        {
            get
            {
                return tdurl.InnerText;
            }
            set
            {
                tdurl.InnerText = value;
            }

        }





        protected void Page_Load(object sender, EventArgs e)
        {
            // setChange(); //hides ok button and shows change password button on FIRST TIME password change popup
            // setGenPassChange(); //hides ok button and shows change password button on ANYTIME password change popup 
            if (Session["operator_id"] != null && Session["username"] != null && Session["user_id"] != null)
            {
                operator_id = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                user_id = Convert.ToString(Session["user_id"]);
                category = Convert.ToString(Session["category"]);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            //
            if (System.IO.File.Exists(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpg")))
            {
                Image2.ImageUrl = "~/ProfileImages/" + Session["username"].ToString() + ".jpg?" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                lnkremoveimg.Visible = true;
                var length = new System.IO.FileInfo(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpg")).Length;
                if (length > 10000)
                {
                    //Based on scalefactor image size will vary
                    string oldName = Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpg"), NewFileName = Server.MapPath("~/ProfileImages/" + "New1.jpg");
                    File.Copy(oldName, NewFileName);
                    File.Delete(oldName);
                    string path = Server.MapPath("~/ProfileImages/" + "New1.jpg");
                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    string targetPath = Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpg");
                    GenerateThumbnails(0.5, stream, targetPath);
                    stream.Close();
                    File.Delete(NewFileName);
                }

            }
            else
            {
                lnkremoveimg.Visible = false;
                Image2.ImageUrl = "~/ProfileImages/adduser.png?" + DateTime.Now.ToString("ddMMyyyyhhmmss");
            }


            string pageName = this.MasterBody.Page.GetType().FullName;
            if (Request.Form.Count > 0)
            {
                if (Request.Form["Name"] != null && Request.Form["username"] != null)
                {
                    Label2.Text = "Welcome " + Request.Form["Name"] + " (" + Request.Form["username"] + ")";
                }
                else
                {
                    Label2.Text = "";
                }

            }
            else
            {
                Label2.Text = "";
            }

            if (Session["username"] != null && Session["RPAuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["RPAuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                    return;
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                Cls_Business_TxnAssignPlan pl1 = new Cls_Business_TxnAssignPlan();

                string city = "";
                String dasarea = "";
                String operid = "";
                string Flag = "";
                string JVNO = "";
                string statename = "";
                pl1.GetUserCity(Session["username"].ToString(), out city, out dasarea, out operid, out JVNO, out Flag, out statename);
                ViewState["cityid"] = city;
                ViewState["dasarea"] = dasarea;
                ViewState["lco_operid"] = operid;
                ViewState["JVFlag"] = Flag;
                ViewState["JVNO"] = JVNO;
                if (ViewState["JVFlag"].ToString() == "Y")
                {

                    if (System.IO.File.Exists(Server.MapPath("~/Img/" + JVNO + ".jpg")))
                    {
                        bodyimg.Attributes.Add("style", "background:url(../Img/" + JVNO + ".jpg) no-repeat");
                    }
                    else
                    {
                        bodyimg.Attributes.Add("style", "background:url(../Img/Background.jpg) no-repeat"); 
                    }
                }
                else
                {
                    bodyimg.Attributes.Add("style", "background:url(../Img/Background.jpg) no-repeat"); 
                }


                bodyimg.Attributes.Add("onunload", "javascript:return bodyUnload()");
                bodyimg.Attributes.Add("onclick", "javascript:clicked=true");
                //imglogo.Attributes.Add("Style", "dispaly:none");
                //UploadLogo((Session["username"]).ToString());

                /*if (Request.Form.Count > 0)
                {

                }
                else
                {
                    NameValueCollection collections = new NameValueCollection();
                    collections.Add("username", Session["username"].ToString());
                    collections.Add("Name", Session["name"].ToString());
                    string remoteUrl = Request.RawUrl.ToString();//System.Web.HttpContext.Current.Request.Url.AbsolutePath;

                    string html = "<html><head>";
                    html += "</head><body onload='document.forms[0].submit()'>";
                    html += string.Format("<form name='PostForm' method='POST' action='{0}'>", remoteUrl);
                    foreach (string key in collections.Keys)
                    {
                        html += string.Format("<input name='{0}' type='hidden' value='{1}'>", key, collections[key]);
                    }
                    html += "</form></body></html>";
                    Response.Clear();
                    Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");
                    Response.HeaderEncoding = Encoding.GetEncoding("ISO-8859-1");
                    Response.Charset = "ISO-8859-1";
                    Response.Write(html);
                    Response.End();
                }*/

            }

            //Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);




            foreach (DictionaryEntry entry in HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Remove((string)entry.Key);
            }





            if (Session["MIAflag"].ToString() == "Y")
            {
                DataTable miaData = new DataTable();

                DateTime DT = DateTime.Today;

                string Curday = DT.ToString("dd");
                string curMonth = DT.ToString("MMM-yyyy");
                string curDate = DT.ToString("dd-MMM-yyyy");
                string miamsoarea = "";
                string miamsoregisdate = "";
                string miamsoregisno = "";

                try
                {
                    miamsoarea = ConfigurationSettings.AppSettings["miamsoarea"].ToString().Trim();
                }
                catch { miamsoarea = ""; }

                try
                {
                    miamsoregisdate = ConfigurationSettings.AppSettings["miamsoregisdate"].ToString().Trim();
                }
                catch { miamsoregisdate = ""; }

                try
                {
                    miamsoregisno = ConfigurationSettings.AppSettings["miamsoregisno"].ToString().Trim().Replace('*', '&');
                }
                catch { miamsoregisno = ""; }

                Cls_Business_MstHwaymsgBrodcaster ob = new Cls_Business_MstHwaymsgBrodcaster();
                miaData = ob.getmiadata(username, username);
                if (miaData.Rows.Count > 0)
                {
                    if (Curday != "")
                    {
                        MIADOC1._lblMIAday = Curday;
                    }
                    if (curMonth != "")
                    {
                        MIADOC1._lblMIAmonth = curMonth;
                    }
                    if (miaData.Rows[0]["LCONAME"].ToString() != "")
                    {
                        MIADOC1._lblMIAlcoName = miaData.Rows[0]["LCONAME"].ToString();
                        MIADOC1._lblMIAlcoName2 = miaData.Rows[0]["LCONAME"].ToString();
                        MIADOC1._lbllconame = miaData.Rows[0]["LCONAME"].ToString();
                    }
                    if (miaData.Rows[0]["ADDRESS"].ToString() != "")
                    {
                        MIADOC1._lblMIAlcoaddress = miaData.Rows[0]["ADDRESS"].ToString();
                    }

                    if (miaData.Rows[0]["REGISNO"].ToString() != "")
                    {
                        MIADOC1._lbllcoregisno = miaData.Rows[0]["REGISNO"].ToString();
                    }

                    if (miaData.Rows[0]["REGISDATE"].ToString() != "")
                    {
                        MIADOC1._lblMIAlcodate = miaData.Rows[0]["REGISDATE"].ToString();
                    }

                    if (miaData.Rows[0]["HEADOFF"].ToString() != "")
                    {
                        MIADOC1._lblMIAlcoheadOffice = miaData.Rows[0]["HEADOFF"].ToString();
                    }

                    if (miaData.Rows[0]["TERRITORY"].ToString() != "")
                    {
                        MIADOC1._lbllcoterritory = miaData.Rows[0]["TERRITORY"].ToString();
                    }

                    if (miaData.Rows[0]["AREA"].ToString() != "")
                    {
                        MIADOC1._lblMIAlcoarea = miaData.Rows[0]["AREA"].ToString();
                    }
                    if (miaData.Rows[0]["stateaddress"].ToString() != "")
                    {
                        MIADOC1._lblMIAStateAddress = miaData.Rows[0]["stateaddress"].ToString();
                    }

                    //
                    if (miamsoarea != "")
                    {
                        MIADOC1._lblmiamsoarea = miamsoarea;
                    }

                    if (miamsoregisdate != "")
                    {
                        MIADOC1._lblmiamsoregisdate = miamsoregisdate;
                    }

                    if (miamsoregisno != "")
                    {
                        MIADOC1._lblmiamsoregisno = miamsoregisno;
                    }

                }

                PopMiaTerms.Show();

            }
            else
            {

                PopMiaTerms.Hide();
            }

            if (tdurl.InnerText == "Home")
            {
                pagehead.Visible = false;

                Uhome.Visible = true;
                Uhome.InnerText = "               ";

            }
            else
            {
                Uhome.InnerText = "Home";
                if (pageName != "ASP.transaction_home_aspx")
                {
                    tdurl.Visible = false;
                    Ul2.InnerText = tdurl.InnerText;
                    tdurl.InnerText = "";
                    Uhome.Visible = true;
                }
                else
                {
                    tdurl.Visible = true;
                    Uhome.Visible = true;
                    Ul2.InnerText = "";
                    Ul2.Visible = false;


                }
                //if (pageName.ToLower().Contains("asp.reports"))
                //{
                //    Ul2.InnerText = tdurl.InnerText;
                //    tdurl.InnerText = "";
                //    Uhome.Visible = true;
                //}
            }


            if (Session["operator_id"] != null && Session["username"] != null && Session["user_id"] != null)
            {
                operator_id = Convert.ToString(Session["operator_id"]);
                username = Convert.ToString(Session["username"]);
                user_id = Convert.ToString(Session["user_id"]);
                category = Convert.ToString(Session["category"]);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (Session["RightsKey"] != null && Session["RightsKey"].ToString() == "N")
            {

            }
            else
            {

                //checking user rights
                Cls_Business_Auth objChk = new Cls_Business_Auth();
                string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
                bool hasPageRights = objChk.hasPageRights(user_id, username, page);
                if (!hasPageRights)
                {
                    Response.Redirect("~/NoAccessRights.aspx");// need to remove
                    return;
                }
            }

            if (!IsPostBack)
            {
                // session log
                //try
                //{
                //    Cls_Data_Auth auth = new Cls_Data_Auth();
                //    string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                //    string page = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
                //    Cls_Business_Auth objAuth = new Cls_Business_Auth();
                //    Hashtable sessionlog = new Hashtable();

                //    sessionlog["username"] = Session["username"];
                //    sessionlog["sessionid"] = Session.SessionID;
                //    sessionlog["pagename"] = page;
                //    sessionlog["name"] = Session["name"];
                //    sessionlog["IP"] = Ip;

                //    string resssion = objAuth.SessionLog(sessionlog);
                //}
                //catch { }

                if (Session["showimage"] != null)
                {
                    if (Session["showimage"].ToString() == "Y")
                    {
                        HomeimageDisplay();
                    }
                }

                lblmsg.Text = GetMsg();
                if (Request.UserAgent.IndexOf("AppleWebKit") > 0)
                {

                    Request.Browser.Adapters.Clear();

                }

                //checks first time login
                if (Session["login_flag"].ToString() == "Y")
                {
                    //old user no action needed
                }
                else
                {
                    //popPass.Show(); // first time login
                }


                //checks session
                if (Session["user_id"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }
                else
                {
                    if (Session["name"] != null)
                    {
                        LblLoggedInUser.Text = Session["name"].ToString();
                        //LblLoggedInUser.Text = "MAHIM CABLE";
                    }
                    if (Session["username"] != null)
                    {
                        LblLoggedInUserName.Text = Session["username"].ToString();
                    }
                    if (Session["last_login"] != null)
                    {
                        lblLastLogin.Text = Session["last_login"].ToString();
                    }
                    SetBalance();
                }
                //MainMenu.Nodes.Clear();
                //GenerateMenu();
            }

            string lco_bc_msg = showLCOBroadcastMsg(username, operator_id);
            // lblLCOBroadcastMsg.Text = lco_bc_msg;

            //  MainMenu.Nodes.Clear();
            // GenerateMenu();
            //sets breadrum text
            if (Session["breadclum"] != null)
            {
                // lblBreadcrum.Text = Session["breadclum"].ToString();
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

        //public void UploadLogo(string UserId)
        //{

        //    String NoImageURL = "~/Img/Logo.png";
        //    String GetImgidQuery = " select num_lcomst_directno jvno,num_lcomst_cityid cityid from aoup_lcopre_lco_det where  var_lcomst_code = '" + UserId + "'";// Session["operator_id"];

        //    DataTable tblGetImageId = GetResult(GetImgidQuery);

        //    if (tblGetImageId.Rows.Count > 0)
        //    {
        //        String GetImageQuery = "select blob_lcouploadimg_image img from aoup_lcopre_lco_uploadimg where  num_lcouploadimg_jvno = " + tblGetImageId.Rows[0]["jvno"];
        //        //GetImageQuery += " and num_lcouploadimg_city=" + tblGetImageId.Rows[0]["cityid"];
        //        DataTable tblGetImage = GetResult(GetImageQuery);

        //        if (tblGetImage.Rows.Count > 0)
        //        {
        //            if (tblGetImage.Rows[0]["img"].ToString() != "")
        //            {
        //                byte[] Image = (byte[])tblGetImage.Rows[0]["img"];

        //                if (Image.Length > 5)
        //                {
        //                    String FileNameSign = "LogoImage_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + "_S.jpg";

        //                    WriteToFile(Server.MapPath(FileNameSign), ref Image);
        //                    imglogo.ImageUrl = "~/ImageGarbage/" + FileNameSign;
        //                    //    bodyimg.Attributes.Add("style", "background:url(" + "../ImageGarbage/" + FileNameSign + ") no-repeat;");

        //                }

        //                else
        //                {
        //                   imglogo.ImageUrl = NoImageURL;
        //                    //  bodyimg.Attributes.Add("style", "background:url(" + NoImageURL + ") no-repeat;");
        //                }
        //            }

        //            else
        //            {
        //                imglogo.ImageUrl = NoImageURL;
        //                //bodyimg.Attributes.Add("style", "background:url(" + NoImageURL + ") no-repeat;");
        //            }
        //        }

        //        else
        //        {
        //     imglogo.ImageUrl = NoImageURL;
        //            //bodyimg.Attributes.Add("style", "background:url(" + NoImageURL + ") no-repeat;");
        //        }
        //    }
        //    else
        //    {
        //   imglogo.ImageUrl = NoImageURL;
        //        //    bodyimg.Attributes.Add("style", "background:url(" + NoImageURL + ") no-repeat;");
        //    }

        //}
        protected void WriteToFile(string strPath, ref byte[] Buffer)
        {
            String a = strPath;
            int counter = 0;
            String val = "", val1 = "";

            for (int i = 1; i < a.Length; i++)
            {
                if (counter == 0 || counter == 1)
                {
                    val1 = a.Substring(a.Length - i, 1) + val1;
                    if (counter == 0)
                    {
                        val = a.Substring(a.Length - i, 1) + val;
                    }
                }
                if (a.Substring(a.Length - i, 1) == "\\")
                {
                    counter = counter + 1;
                }
            }

            String b = a.Replace(val1, "\\ImageGarbage");

            FileStream newFile = new FileStream(b + val, FileMode.Create);
            ViewState["strPath"] = val.TrimStart('\\');
            ViewState["ImagePath"] = b + val;
            newFile.Write(Buffer, 0, Buffer.Length);
            newFile.Close();
        }
        public void HomeimageDisplay()
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            con.Open();
            String Strdiscount = "select * from aoup_lcopre_homeimageconfig";
            OracleCommand cmd1 = new OracleCommand(Strdiscount, con);
            OracleDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                if (dr1["flag"].ToString() == "Y")
                {
                    pophomeimage.Show();
                    Session["showimage"] = "N";
                }
            }

            if (!dr1.HasRows)
            {

            }
            con.Close();
            dr1.Dispose();


        }

     
        
        //shows lco broadcast message
        public string showLCOBroadcastMsg(string username, string oper_id)
        {
            Cls_Business_MstHwaymsgBrodcaster obj1 = new Cls_Business_MstHwaymsgBrodcaster();
            return obj1.GetLCOBrodcastMsg(username, oper_id);
        }

        public string GetMsg()
        {
            string stritemQuery = "select msg from view_lcopre_lcomsg where lcocode= '" + Convert.ToString(Session["username"]) + "'";
            string dtMenuItemCat = Cls_Helper.fnGetScalar(stritemQuery);
            return dtMenuItemCat;
        }

        //generates menu dynamically
        //public void GenerateMenu()
        //{
        //    DataTable dtMenuHeading = GetMenuHeadings();
        //    DataTable dtMenuItem = GetMenuItems();

        //    //defalut home location
        //    TreeNode homeNode = new TreeNode();
        //    if (category == "3")
        //    {
        //        homeNode.Target = "../Reports/rptLCOAllDetails.aspx";
        //    }
        //    else
        //    {
        //        homeNode.Target = "../Transaction/Home.aspx";
        //    }

        //    homeNode.Text = "Home";
        //    homeNode.ToolTip = "Home";
        //   // MainMenu.Nodes.Add(homeNode);

        //    //dynamic tree generation
        //    foreach (DataRow menu_head_row in dtMenuHeading.Rows)
        //    {
        //        string row_str;
        //        string[] row_data = new string[3];
        //        row_str = string.Join("|", menu_head_row.ItemArray.Select(p => p.ToString()).ToArray());
        //        row_data = row_str.Split('|');

        //        TreeNode mainMenuItem = new TreeNode();
        //        mainMenuItem.Text = row_data[2];
        //       // MainMenu.Nodes.Add(mainMenuItem);

        //        foreach (DataRow menu_item_row in dtMenuItem.Rows)
        //        {
        //            string item_row_str;
        //            string[] item_row_data = new string[5];
        //            item_row_str = string.Join("|", menu_item_row.ItemArray.Select(p => p.ToString()).ToArray());
        //            item_row_data = item_row_str.Split('|');

        //            string frm_id = item_row_data[0];
        //            string frm_name = item_row_data[1];
        //            string frm_cat = item_row_data[2];
        //            string frm_url = item_row_data[3];

        //            TreeNode subMenuItem = new TreeNode();
        //            subMenuItem.Text = frm_name;
        //            subMenuItem.ToolTip = frm_name;
        //            subMenuItem.Target = frm_url;

        //            if (frm_cat == row_data[0])
        //            {
        //                mainMenuItem.ChildNodes.Add(subMenuItem);
        //            }
        //        }
        //    }
        //}

        //get Master nodes - categories
        public DataTable GetMenuHeadings()
        {
            Cls_Helper objHelper1 = new Cls_Helper();
            string strQuery = "select a.CAT_ID, a.CAT_CODE, a.CAT_DESC" +
                " from view_lcopre_category a " +
                " where CAT_USERID = " + user_id +
                " order by a.CAT_ORDER";
            DataTable dtMenuCat = objHelper1.GetDataTable(strQuery);
            return dtMenuCat;
        }

        //get Child nodes - froms
        public DataTable GetMenuItems()
        {
            Cls_Helper objHelper2 = new Cls_Helper();//Session["operator_id"].ToString()
            string stritemQuery = "select DISTINCT(a.num_frm_id), a.var_frm_name, a.num_frm_cat, a.var_frm_file, a.num_frm_sortorder" +
                                " from aoup_lcopre_frm_def a, aoup_lcopre_menu_rights b" +
                                " where a.num_frm_status = 1 and b.num_rights_operid = " + operator_id + " and b.num_rights_userid = " + user_id +
                                " and a.num_frm_id=b.num_rights_frmid order by a.num_frm_sortorder ";
            DataTable dtMenuItemCat = objHelper2.GetDataTable(stritemQuery);
            return dtMenuItemCat;
        }

        //sign out
        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }

        //fetched selected child node path to show breadcrum
        protected void MainMenu_SelectedNodeChanged(object sender, EventArgs e)
        {
            //if (MainMenu.SelectedNode.Target != null && MainMenu.SelectedNode.Target != "")
            //{
            //    Session["breadclum"] = MainMenu.SelectedNode.ValuePath.Replace("/", " >> ");
            //    Response.Redirect(MainMenu.SelectedNode.Target);
            //}
        }

        //shows anytime change password popup
        protected void lbChangePass_Click(object sender, EventArgs e)
        {
            lblGenPassResponse.Text = "";
            popGenPass.Show();
        }

        //any time change password click
        //protected void btnGenChangePass_Click(object sender, EventArgs e)
        //{
        //    Cls_Data_Auth auth = new Cls_Data_Auth();
        //    string Ip = auth.GetIPAddress(HttpContext.Current.Request);
        //    string username = "";
        //    if (Session["username"] != null)
        //    {
        //        username = Convert.ToString(Session["username"]);
        //    }
        //    else
        //    {
        //        Session.Abandon();
        //        Response.Redirect("~/Login.aspx");
        //    }
        //    string cur_pass = txtGenCurPass.Text;
        //    string new_pass = txtGenNewPass.Text;
        //    string con_pass = txtGenConPass.Text;
        //    if (new_pass.Length < 8)
        //    {
        //        lblGenPassResponse.Text = "Password must have minimum 8 characters";
        //        popGenPass.Show();
        //        return;
        //    }
        //    Cls_Business_TransHome objHome = new Cls_Business_TransHome();
        //    string result = objHome.updatePass(username, cur_pass, new_pass, Ip);
        //    if (result.Split('$')[0] == "9999")
        //    {
        //        lblGenPassResponse.Text = "Password changed successfully";
        //        setGenPassOk();
        //        popGenPass.Show();
        //        return;
        //    }
        //    else
        //    {
        //        lblGenPassResponse.Text = result.Split('$')[1];
        //        popGenPass.Show();
        //        return;
        //    }
        //}

        //sets ok button and hides change button
        //public void setGenPassOk()
        //{
        //    btnGenPassOk.Visible = true;
        //    btnGenChangePass.Visible = false;
        //    btnOKGenPass.Visible = false;
        //}
        //sets change button and hides ok button
        //public void setGenPassChange()
        //{
        //    btnGenPassOk.Visible = false;
        //    btnGenChangePass.Visible = true;
        //    btnOKGenPass.Visible = true;
        //}


        //------------------------------------fist time password change popup functions----------------------------
        //sets ok button and hides change button
        //public void setOk()
        //{
        //    btn_OK.Visible = true;
        //    btnChangePass.Visible = false;
        //}
        //sets change button and hides ok button
        //public void setChange()
        //{
        //    btn_OK.Visible = false;
        //    btnChangePass.Visible = true;
        //}

        //first time password change click
        //protected void btnChangePass_Click(object sender, EventArgs e)
        //{
        //    string cur_pass = txtCurPass.Text;
        //    string new_pass = txtNewPass.Text;
        //    string con_pass = txtConPass.Text;
        //    if (new_pass.Length < 8)
        //    {
        //        lblPassResponse.Text = "Password must have minimum 8 characters";
        //        popPass.Show();
        //        return;
        //    }
        //    Cls_Data_Auth auth = new Cls_Data_Auth();
        //    string Ip = auth.GetIPAddress(HttpContext.Current.Request);
        //    Cls_Business_TransHome objHome = new Cls_Business_TransHome();
        //    string result = objHome.updatePass(username, cur_pass, new_pass, Ip);
        //    if (result.Split('$')[0] == "9999")
        //    {
        //        lblPassResponse.Text = "Password changed successfully";
        //        Session["login_flag"] = "Y";
        //        lblPassResponse.Text = result.Split('$')[1];
        //        popPass.Show();
        //        setOk();
        //        return;
        //    }
        //    else {
        //        lblPassResponse.Text = result.Split('$')[1];
        //        popPass.Show();
        //        return;
        //    }
        //}

        protected void RedirectAfterPassChange()
        {
            string category = "";
            if (Session["category"] != null)
            {
                category = Session["category"].ToString();
            }
            else
            {
                Session.Abandon();
                Session.Clear();
                Response.Redirect("~/ErrorPage.aspx");
            }
            if (category == "3")
            {
                //if Logged in user is LCO
                Response.Redirect("~/Transaction/Home.aspx");
            }
            else
            {
                Response.Redirect("~/Transaction/Home.aspx");
            }
        }

        protected void btn_OK_Click(object sender, EventArgs e)
        {
            RedirectAfterPassChange();
        }

        protected void btnGenPassOk_Click(object sender, EventArgs e)
        {
            RedirectAfterPassChange();
        }

        protected void btnGenChangePass_Click(object sender, EventArgs e)
        {
            string blusername = SecurityValidation.chkData("T", txtGenCurPass.Text + "" + txtGenNewPass.Text + "" + txtGenConPass.Text);
            if (blusername.Length > 0)
            {
                lblGenPassResponse.Text = blusername;
                popGenPass.Show();
                return;
            }
            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            string username = "";
            if (Session["username"] != null)
            {
                username = Convert.ToString(Session["username"]);
            }
            else
            {
                Session.Abandon();
                Session.Clear();
                Response.Redirect("~/Login.aspx");
            }
            string cur_pass = txtGenCurPass.Text;
            string new_pass = txtGenNewPass.Text;
            string con_pass = txtGenConPass.Text;
            if (new_pass.Length < 8)
            {
                lblGenPassResponse.Text = "Password must have minimum 8 characters";
                popGenPass.Show();
                return;
            }
            Cls_Business_TransHome objHome = new Cls_Business_TransHome();
            string result = objHome.updatePass(username, cur_pass, new_pass, Ip);
            if (result.Split('$')[0] == "9999")
            {
                lblGenPassResponse.Text = "Password changed successfully";
                setGenPassOk();
                popGenPass.Show();
                return;
            }
            else
            {
                lblGenPassResponse.Text = result.Split('$')[1];
                popGenPass.Show();
                return;
            }
        }

        public void SetBalance()
        {
            string Balance = "";
            string InBalance = "";
            Cls_Business_TransHome obj = new Cls_Business_TransHome();
            obj.GetBalance(Session["username"].ToString(), out Balance, out InBalance);
            lblBalance.Text = Balance;
            lblInvBalance.Text = InBalance;
        }
        public void setGenPassOk()
        {
            btnGenPassOk.Visible = true;
            btnGenChangePass.Visible = false;
            btnOKGenPass.Visible = false;
        }


        protected void BtnAcceptMiaterms_Click(object sender, EventArgs e)
        {
            lblmiatermserror.Text = "";

            if (chkterm.Checked == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Terms & Conditions')", true);
                PopMiaTerms.Show();
                return;
            }
            else
            {
            }

            Cls_Data_Auth auth = new Cls_Data_Auth();
            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
            String Username = Session["username"].ToString();

            Cls_Business_TransHome objHome = new Cls_Business_TransHome();
            string result = objHome.MIASTATUSINS(Username, "A", Ip);

            if (result.Split('$')[0] == "9999")
            {
                Session["MIAflag"] = "N";
                //Response.Redirect("~/Transaction/Home.aspx");
                popreviewconfirm.Show();
                PopMiaTerms.Hide();
            }
            else
            {
                lblmiatermserror.Text = result.Split('$')[1];
                PopMiaTerms.Show();
            }

        }

        protected void BtnRejectMiaterms_Click(object sender, EventArgs e)
        {

            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/Login.aspx");

        }

        protected void lnkremoveimg_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpg")))
            {
                File.Delete(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpg"));


                Image2.ImageUrl = "http://124.153.73.21/HwayLCOSMSUAT/ProfileImages/adduser.png";
                lnkremoveimg.Visible = false;
            }

        }



        protected void lnkimgupload_Click(object sender, EventArgs e)
        {

            if (flpimg.PostedFile != null && flpimg.PostedFile.ContentLength > 0)
            {
                string strExtension = System.IO.Path.GetExtension(flpimg.FileName).ToLower();
                //if (flpimg.PostedFile.ContentLength < 500000)
                //{
                if (strExtension != ".jpg")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only image formats (jpg) are accepted')", true);
                    return;
                }

                string fileName = Path.GetFileName(flpimg.PostedFile.FileName);
                //string[] tokens = str.Split(',');
                string[] fileName2 = fileName.Split('.');
                string filenameNew = fileName2[0];
                string folder = Server.MapPath("~/ProfileImages/");
                string strFinalName = Session["username"].ToString() + strExtension;
                if (System.IO.File.Exists(Server.MapPath("~/ProfileImages/")))
                {
                    try
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpg")))
                        {
                            System.IO.File.Delete(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpg"));
                        }
                        if (System.IO.File.Exists(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".png")))
                        {
                            System.IO.File.Delete(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".png"));
                        }
                        if (System.IO.File.Exists(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpeg")))
                        {
                            System.IO.File.Delete(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpeg"));
                        }
                    }
                    catch
                    { }
                    //flpimg.PostedFile.SaveAs(Path.Combine(folder, strFinalName));

                    string filename = strFinalName;

                    string targetPath = Server.MapPath("~/ProfileImages/" + filename);
                    Stream strm = flpimg.PostedFile.InputStream;
                    var targetFile = targetPath;
                    if (flpimg.PostedFile.ContentLength > 5000)
                    {
                        //Based on scalefactor image size will vary
                        GenerateThumbnails(0.5, strm, targetFile);
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Image Uploaded Succesfully')", true);
                    lnkremoveimg.Visible = true;
                    Image2.ImageUrl = "~/ProfileImages/" + strFinalName;
                    // Image2.ImageUrl = "~/ProfileImages/" + strFinalName;
                }
                else
                {
                    Directory.CreateDirectory(folder);
                    //flpimg.PostedFile.SaveAs(Path.Combine(folder, strFinalName));
                    try
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpg")))
                        {
                            System.IO.File.Delete(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpg"));
                        }
                        if (System.IO.File.Exists(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".png")))
                        {
                            System.IO.File.Delete(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".png"));
                        }
                        if (System.IO.File.Exists(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpeg")))
                        {
                            System.IO.File.Delete(Server.MapPath("~/ProfileImages/" + Session["username"].ToString() + ".jpeg"));
                        }
                    }
                    catch
                    { }
                    string filename = strFinalName;
                    string targetPath = Server.MapPath("~/ProfileImages/" + filename);
                    Stream strm = flpimg.PostedFile.InputStream;
                    var targetFile = targetPath;
                    if (flpimg.PostedFile.ContentLength > 5000)
                    {
                        //Based on scalefactor image size will vary
                        GenerateThumbnails(0.5, strm, targetFile);
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Image Uploaded Succesfully')", true);
                    lnkremoveimg.Visible = true;
                    Image2.ImageUrl = "~/ProfileImages/" + strFinalName + "?" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                    //Image2.ImageUrl = "~/ProfileImages/" + strFinalName;
                }
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please upload less then 500KB file')", true);
                //    return;

                //}
            }

        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        private void GenerateThumbnails1(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }
    }
}
