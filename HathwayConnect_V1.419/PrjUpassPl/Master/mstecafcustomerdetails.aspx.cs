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
using System.Text;
using System.Net;
using System.IO;
using PrjUpassBLL.Transaction;
using PrjUpassDAL.Transaction;
using PrjUpassDAL.Authentication;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PrjUpassPl.Master
{
    public partial class mstecafcustomerdetails : System.Web.UI.Page
    {
        string username;
        String user_brmpoid;
        protected void Page_Load(object sender, EventArgs e)
        {

            Master.PageHeading = "ECAF Entry";
            if (Session["username"] != null || Session["operator_id"] != null)
            {
                username = Session["username"].ToString();
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
                return;
            }


            if (ViewState["Childtvcount"] != null)
            {
                if (Convert.ToInt32(ViewState["Childtvcount"]) > 2)
                {
                    btnAddChildTV.Visible = false;
                }
            }

            if (!IsPostBack)
            {
                lblserachmand.Visible = true;
                GetUserCity();
                FillProof();
                GetUserDteail();
            }
        }

        public void GetUserDteail()
        {
            try
            {
                Cls_Business_TxnAssignPlan objAsPl = new Cls_Business_TxnAssignPlan();
                string responseStr = objAsPl.getDistributorDetails(Session["username"].ToString(), Session["operator_id"].ToString());

                if (responseStr == "ex_occured")
                {
                    Response.Redirect("~/ErrorPage.aspx");
                    return;
                }

                string[] arrDistDetails = new string[2];
                arrDistDetails = responseStr.Split('~');
                string cust_name = arrDistDetails[0];
                string avail_bal = arrDistDetails[1];
                string user = arrDistDetails[2];

                double balance = 0;
                lblDistName.Text = cust_name;
                lblAvailBal.Text = avail_bal;
                balance = Convert.ToDouble(lblAvailBal.Text);

                if (balance < 1000)
                {
                    lblAvailBal.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblAvailBal.ForeColor = System.Drawing.Color.Black;

                }
                lbluser.Text = user;
            }
            catch (Exception ex)
            {

            }

        }

        public void FillProof()
        {
            Cls_BLL_mstCrf objdet = new Cls_BLL_mstCrf();
            String Response = objdet.getIDdata(Session["username"].ToString().Trim());

            Response = Response.Substring(0, Response.Length - 1);

            String[] Responsearr = Response.Split('!');

            DataTable TblIdPrrof = new DataTable();
            TblIdPrrof.Columns.Add("ID");
            TblIdPrrof.Columns.Add("Name");
            string[] idarr = Responsearr[0].ToString().Substring(0, Responsearr[0].ToString().Length - 1).Split('$');

            for (int i = 0; i < idarr.Length; i++)
            {
                TblIdPrrof.Rows.Add(idarr[i].ToString(), idarr[i + 1].ToString());
                i++;
            }

            ddlID.Items.Clear();
            if (TblIdPrrof.Rows.Count > 0)
            {
                ddlID.DataTextField = "Name";
                ddlID.DataValueField = "ID";

                ddlID.DataSource = TblIdPrrof;
                ddlID.DataBind();
                ddlID.Items.Insert(0, new ListItem("-- Select Id Proof --", ""));
            }

            DataTable TblresPrrof = new DataTable();
            TblresPrrof.Columns.Add("ID");
            TblresPrrof.Columns.Add("Name");
            string[] resarr = Responsearr[1].ToString().Substring(0, Responsearr[1].ToString().Length - 1).Split('$');

            for (int i = 0; i < resarr.Length; i++)
            {
                TblresPrrof.Rows.Add(resarr[i].ToString(), resarr[i + 1].ToString());
                i++;
            }

            ddlResi.Items.Clear();
            if (TblresPrrof.Rows.Count > 0)
            {
                ddlResi.DataTextField = "Name";
                ddlResi.DataValueField = "ID";

                ddlResi.DataSource = TblresPrrof;
                ddlResi.DataBind();
                ddlResi.Items.Insert(0, new ListItem("-- Select Resi. Proof --", ""));
            }

        }

        public void GetUserCity()
        {
            string strConn = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection conn = new OracleConnection(strConn);
            String str = "";

            /*str += " select n.var_city_name,a.var_lcomst_dasarea,a.num_lcomst_cityid from aoup_lcopre_lco_det a,aoup_lcopre_city_def n  where a.var_lcomst_code='" + Session["username"].ToString().Trim() + "'";
            str += " and a.num_lcomst_cityid=n.num_city_id";*/
            str += " select n.var_city_name,a.var_lcomst_dasarea,a.num_lcomst_cityid from aoup_lcopre_lco_det a,aoup_lcopre_city_def n ";
            str += "  where a.num_lcomst_operid=" + Session["operator_id"].ToString().Trim() + "";
            str += " and a.num_lcomst_cityid=n.num_city_id";
            conn.Open();
            OracleCommand cmd2 = new OracleCommand(str, conn);
            OracleDataReader dr4 = cmd2.ExecuteReader();

            while (dr4.Read())
            {
                Session["city"] = dr4["var_city_name"].ToString();
                Session["cityID"] = dr4["num_lcomst_cityid"].ToString();
                Session["DASAREA"] = dr4["var_lcomst_dasarea"].ToString();
            }

            dr4.Close();
            conn.Close();
        }

        public void GetBaseplan(String Planpoid)
        {
            string strConn = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection conn = new OracleConnection(strConn);
            String str = "";

            str += " select var_plan_name from aoup_lcopre_plan_def where var_plan_plantype='B' and ";
            str += " var_plan_planpoid='" + Planpoid + "' and rownum=1 ";
            str += " Union ";
            str += " select var_plan_name from aoup_lcopre_jvplan_def where var_plan_plantype='B' and ";
            str += " var_plan_planpoid='" + Planpoid + "' and rownum=1";
            conn.Open();
            OracleCommand cmd2 = new OracleCommand(str, conn);
            OracleDataReader dr4 = cmd2.ExecuteReader();

            while (dr4.Read())
            {
                ViewState["Old_PlanName"] = dr4["var_plan_name"].ToString();

            }

            dr4.Close();
            conn.Close();
        }

        public String FillAddDeatil(String selectionfalg, String Entity)
        {

            Cls_BLL_mstCrf objdet = new Cls_BLL_mstCrf();
            string response = objdet.GetAdddeatils(Session["username"].ToString(), selectionfalg, Entity);


            return response;
        }

        public void data()
        {
            ViewState["cafno"] = null;
            txtresoproof.Text = "";
            txtidproof.Text = "";
            ddlID.Visible = true;
            txtidproof.Visible = false;
            imgiprooftxtclose.Visible = false;
            ddlResi.Visible = true;
            txtresoproof.Visible = false;
            imgresiprooftxtclose.Visible = false;
            ddlID.SelectedValue = "";
            ddlResi.SelectedValue = "";
            divTabPanels.Visible = false;
            divTabPanels1.Visible = false;
            btnsubmit.Text = "Next >>";
            btnback.Visible = false;
            txtfname.Text = "";
            txtmname.Text = "";
            txtlname.Text = "";
            txtmobno.Text = "";
            txtlandline.Text = "";
            txtemailid.Text = "";
            txtcity.Text = "";
            ddlpin.Items.Clear();
            ddlstreet.Items.Clear();
            ddllocation.Items.Clear();
            ddlarea.Items.Clear();
            ddlbuilding.Items.Clear();
            txtflatno.Text = "";
            txtadd.Text = "";
            fupidproof.Attributes.Clear();
            fupphoto.Attributes.Clear();
            fupresiproof.Attributes.Clear();
            txtOtp.Text = "";
            Image2.ImageUrl = "~/Images/no_doc_uploaded.jpg";
            Image3.ImageUrl = "~/Images/no_doc_uploaded.jpg";
            Image4.ImageUrl = "~/Images/no_doc_uploaded.jpg";
            divTabPanels.Visible = false;
            divsubmit.Visible = false;
            ViewState["photoimage"] = null;
            ViewState["PDFDOCNAME"] = null;
            ViewState["resiproof"] = null;
            ViewState["idproofimage"] = null;
            ViewState["basic_poids"] = null;
            ViewState["resiproofpath"] = null;
            ViewState["resiproofpdf"] = null;
            ViewState["PDFDOCNAME"] = null;
            ViewState["Pdfdocimage"] = null;
            ViewState["idproofimagePDF"] = null;
            ViewState["idprooftrpath"] = null;
            lblphotoname.Visible = false;
            lblphotoname.Text = "";
            imgbtnphoto.Visible = false;
            fupphoto.Visible = true;
            ViewState["photoimage"] = null;
            Image4.ImageUrl = "~/Images/no_doc_uploaded.jpg";
            lblreiprrofname.Visible = false;
            lblreiprrofname.Text = "";
            imgbtnresiproof.Visible = false;
            fupresiproof.Visible = true;
            ViewState["resiproof"] = null;
            ViewState["resiproofpath"] = null;
            ViewState["resiproofpdf"] = null;
            Image3.ImageUrl = "~/Images/no_doc_uploaded.jpg";
            lblPDFDOCNAME.Visible = false;
            lblPDFDOCNAME.Text = "";
            imgbtnclose.Visible = false;
            fuploadpdf.Visible = true;
            ViewState["PDFDOCNAME"] = null;
            ViewState["PDFDOCNAME"] = null;
            ViewState["Pdfdocimage"] = null;
            lblidproofname.Visible = false;
            lblidproofname.Text = "";
            imgbtnidproof.Visible = false;
            fupidproof.Visible = true;
            ViewState["idproofimage"] = null;
            ViewState["idproofimagePDF"] = null;
            ViewState["idprooftrpath"] = null;
            Image2.ImageUrl = "~/Images/no_doc_uploaded.jpg";
        }

        //private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        //{
        //    using (var image = System.Drawing.Image.FromStream(sourcePath))
        //    {
        //        var newWidth = (int)(image.Width * scaleFactor);
        //        var newHeight = (int)(image.Height * scaleFactor);
        //        Bitmap thumbnailImg = new Bitmap(newWidth, newHeight);
        //        Bitmap bmpNew = new Bitmap(thumbnailImg);
        //        thumbnailImg.Dispose();
        //        thumbnailImg = null;
        //        var thumbGraph = Graphics.FromImage(bmpNew);
        //        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
        //        thumbGraph.DrawImage(image, imageRectangle);
        //        bmpNew.Save(targetPath, image.RawFormat);
        //        thumbGraph.Dispose();
        //        bmpNew.Dispose();
        //    }
        //}

        protected void imgbtnphoto_click(object sender, EventArgs e)
        {
            lblphotoname.Visible = false;
            lblphotoname.Text = "";
            imgbtnphoto.Visible = false;
            fupphoto.Visible = true;
            ViewState["photoimage"] = null;
            Image4.ImageUrl = "~/Images/no_doc_uploaded.jpg";
        }

        protected void lnkfupphoto_click(object sender, EventArgs e)
        {
            //if (txtSearchParam.Text.Trim() != "")
            //{
            if (fupphoto.HasFile)
            {
                string strpath = System.IO.Path.GetExtension(fupphoto.FileName).ToLower();
                if (strpath != ".jpg" && strpath != ".jpeg" && strpath != ".gif")
                {
                    msgboxstr("Only image formats (jpg, png, gif) are accepted ");
                    return;
                }
                else
                {

                    bool SizeValid = SecurityValidation.SizeUploadValidation(fupphoto);

                    if (SizeValid)
                    {
                        string filenamePhoto = "Photo_" + ViewState["Custvc"].ToString().Replace(":", "") + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + strpath;//Path.GetFileName(fupphoto.PostedFile.FileName);
                        //Save images into Images folder

                        String targetFile = "C:/Inetpub/wwwroot/yesbank/MobImages/" + filenamePhoto;
                        //Stream strm = fupphoto.PostedFile.InputStream;
                        //GenerateThumbnails(0.5, strm, targetFile);
                        fupphoto.SaveAs(targetFile);

                        ViewState["photoimage"] = filenamePhoto;

                        GenerateThumbnail(filenamePhoto, "Image4");
                        //Image4.ImageUrl = "http://hathwayconnect.com/yesbank/MobImages/" + filenamePhoto;

                        lblphotoname.Visible = true;
                        lblphotoname.Text = fupphoto.FileName;
                        imgbtnphoto.Visible = true;
                        fupphoto.Visible = false;
                    }
                    else
                    {
                        msgboxstr("Please choose image with less than 5 MB");
                    }
                }
            }
            //}
        }

        protected void imgbtnresiproof_click(object sender, EventArgs e)
        {
            lblreiprrofname.Visible = false;
            lblreiprrofname.Text = "";
            imgbtnresiproof.Visible = false;
            fupresiproof.Visible = true;
            ViewState["resiproof"] = null;
            ViewState["resiproofpath"] = null;
            ViewState["resiproofpdf"] = null;
            Image3.ImageUrl = "~/Images/no_doc_uploaded.jpg";
        }

        protected void lnkfupresiproof_click(object sender, EventArgs e)
        {
            //  if (txtSearchParam.Text.Trim() != "")
            //  {
            if (ddlResi.SelectedValue != "")
            {
                if (fupresiproof.HasFile)
                {
                    string strpath = System.IO.Path.GetExtension(fupresiproof.FileName).ToLower();
                    if (strpath != ".jpg" && strpath != ".jpeg" && strpath != ".gif" && strpath != ".png" && strpath != ".pdf")
                    {
                        msgboxstr("Only image formats (jpg, png, gif) are accepted ");
                        return;
                    }
                    else
                    {
                        bool SizeValid = SecurityValidation.SizeUploadValidation(fupresiproof);

                        if (SizeValid)
                        {
                            string filenameResi = "Resi_" + ViewState["Custvc"].ToString().Replace(":", "") + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + strpath;//Path.GetFileName(fupresiproof.PostedFile.FileName);

                            if (strpath != ".pdf")
                            {

                                String targetFile = "C:/Inetpub/wwwroot/yesbank/MobImages/" + filenameResi;
                                //Stream strm = fupresiproof.PostedFile.InputStream;
                                //GenerateThumbnails(0.5, strm, targetFile);
                                fupresiproof.SaveAs("C:/Inetpub/wwwroot/yesbank/MobImages/" + filenameResi);

                            }
                            else
                            {
                                fupresiproof.SaveAs("C:/Inetpub/wwwroot/yesbank/MobImages/" + filenameResi);

                            }

                            ViewState["resiproof"] = filenameResi;
                            ViewState["resiproofpath"] = strpath;
                            ViewState["resiproofpdf"] = fupresiproof.FileName;
                            if (strpath != ".pdf")
                            {
                                Image3.Visible = true;
                                GenerateThumbnail(filenameResi, "Image3");
                                //Image3.ImageUrl = "http://hathwayconnect.com/yesbank/MobImages/" + filenameResi;
                            }
                            else
                            {
                                Image3.Visible = true;
                                Image3.ImageUrl = "~/Images/no-preview.jpg";
                            }

                            lblreiprrofname.Visible = true;
                            lblreiprrofname.Text = fupresiproof.FileName;
                            imgbtnresiproof.Visible = true;
                            fupresiproof.Visible = false;
                        }
                        else
                        {
                            msgboxstr("Please choose image with less than 5 MB");
                        }
                    }
                }
            }
            else
            {
                msgboxstr("Please select Residential proof type");
            }
            //  }
        }

        protected void imgbtnclose_click(object sender, EventArgs e)
        {
            lblPDFDOCNAME.Visible = false;
            lblPDFDOCNAME.Text = "";
            imgbtnclose.Visible = false;
            fuploadpdf.Visible = true;
            ViewState["PDFDOCNAME"] = null;
            ViewState["PDFDOCNAME"] = null;
            ViewState["Pdfdocimage"] = null;
            Image8.ImageUrl = "~/Images/no_doc_uploaded.jpg";
        }

        protected void lnkpdfupload_click(object sender, EventArgs e)
        {

            if (fuploadpdf.HasFile)
            {

                string strpath = System.IO.Path.GetExtension(fuploadpdf.FileName).ToLower();
                if (strpath != ".pdf")
                {
                    msgboxstr("Only PDF file are accepted ");
                    return;
                }
                else
                {

                    bool SizeValid = SecurityValidation.SizeUploadValidation(fuploadpdf);

                    if (SizeValid)
                    {
                        string filenameID = "PDFDOC_" + ViewState["Custvc"].ToString().Replace(":", "") + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + strpath;//Path.GetFileName(fupidproof.PostedFile.FileName);
                        //Save images into Images folder
                        fuploadpdf.SaveAs("C:/Inetpub/wwwroot/yesbank/MobImages/" + filenameID);

                        ViewState["PDFDOCNAME"] = fuploadpdf.FileName;
                        ViewState["Pdfdocimage"] = filenameID;
                        lblPDFDOCNAME.Visible = true;
                        lblPDFDOCNAME.Text = fuploadpdf.FileName;
                        imgbtnclose.Visible = true;
                        fuploadpdf.Visible = false;
                        Image8.ImageUrl = "~/Images/no-preview.jpg";
                        Image8.Visible = true;
                    }
                    else
                    {
                        msgboxstr("Please choose file with less than 5 MB");
                    }
                }
            }
        }

        protected void imgbtnidproof_click(object sender, EventArgs e)
        {
            lblidproofname.Visible = false;
            lblidproofname.Text = "";
            imgbtnidproof.Visible = false;
            fupidproof.Visible = true;
            ViewState["idproofimage"] = null;
            ViewState["idproofimagePDF"] = null;
            ViewState["idprooftrpath"] = null;
            Image2.ImageUrl = "~/Images/no_doc_uploaded.jpg";
        }

        protected void lnkfupidproof_click(object sender, EventArgs e)
        {


            //if (txtSearchParam.Text.Trim() != "")
            //{
            if (ddlID.SelectedValue != "")
            {
                if (fupidproof.HasFile)
                {
                    string strpath = System.IO.Path.GetExtension(fupidproof.FileName).ToLower();
                    if (strpath != ".jpg" && strpath != ".jpeg" && strpath != ".gif" && strpath != ".png" && strpath != ".pdf")
                    {
                        msgboxstr("Only image formats (jpg, png, gif) are accepted ");
                        return;
                    }
                    else
                    {

                        bool SizeValid = SecurityValidation.SizeUploadValidation(fupidproof);

                        if (SizeValid)
                        {
                            string filenameID = "Proofid_" + ViewState["Custvc"].ToString().Replace(":", "") + DateTime.Now.ToString("dd_MMM_yyyyhhmmss") + strpath;//Path.GetFileName(fupidproof.PostedFile.FileName);

                            if (strpath != ".pdf")
                            {
                                String targetFile = "C:/Inetpub/wwwroot/yesbank/MobImages/" + filenameID;
                                //Stream strm = fupidproof.PostedFile.InputStream;
                                //GenerateThumbnails(0.5, strm, targetFile);
                                fupidproof.SaveAs("C:/Inetpub/wwwroot/yesbank/MobImages/" + filenameID);
                            }
                            else
                            {
                                fupidproof.SaveAs("C:/Inetpub/wwwroot/yesbank/MobImages/" + filenameID);
                            }
                            ViewState["idproofimage"] = filenameID;
                            ViewState["idproofimagePDF"] = fupidproof.FileName;
                            ViewState["idprooftrpath"] = strpath;
                            if (strpath != ".pdf")
                            {
                                Image2.Visible = true;
                                GenerateThumbnail(filenameID, "Image2");
                                //Image2.ImageUrl = "http://hathwayconnect.com/yesbank/MobImages/" + filenameID;
                            }
                            else
                            {
                                Image2.Visible = true;
                                Image2.ImageUrl = "~/Images/no-preview.jpg";
                            }

                            lblidproofname.Visible = true;
                            lblidproofname.Text = fupidproof.FileName;
                            imgbtnidproof.Visible = true;
                            fupidproof.Visible = false;
                        }
                        else
                        {
                            msgboxstr("Please choose image with less than 5 MB");
                        }
                    }
                }
            }

            else
            {
                msgboxstr("Please select ID proof type");
            }
            //}
        }




        protected void ddlID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlID.SelectedValue == "")
            {
                lblidproofname.Visible = false;
                lblidproofname.Text = "";
                imgbtnidproof.Visible = false;
                fupidproof.Visible = true;
                ViewState["idproofimage"] = null;
                ViewState["idproofimagePDF"] = null;
                ViewState["idprooftrpath"] = null;
                Image2.ImageUrl = "~/Images/no_doc_uploaded.jpg";
                return;
            }
            if (ddlID.SelectedItem.Text.Trim().ToUpper() == "OTHER")
            {
                txtidproof.Visible = true;
                imgiprooftxtclose.Visible = true;
                ddlID.Visible = false;
            }
            else
            {
                txtidproof.Visible = false;
                imgiprooftxtclose.Visible = false;
                ddlID.Visible = true;
            }
        }

        protected void ddlResi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlResi.SelectedValue == "")
            {
                lblreiprrofname.Visible = false;
                lblreiprrofname.Text = "";
                imgbtnresiproof.Visible = false;
                fupresiproof.Visible = true;
                ViewState["resiproof"] = null;
                ViewState["resiproofpath"] = null;
                ViewState["resiproofpdf"] = null;
                Image3.ImageUrl = "~/Images/no_doc_uploaded.jpg";
                return;
            }

            if (ddlResi.SelectedItem.Text.Trim().ToUpper() == "OTHER")
            {
                txtresoproof.Visible = true;
                imgresiprooftxtclose.Visible = true;
                ddlResi.Visible = false;
            }
            else
            {
                txtresoproof.Visible = false;
                imgresiprooftxtclose.Visible = false;
                ddlResi.Visible = true;
            }
        }


        protected void GenerateThumbnail(String Filename, String Img)
        {
            try
            {
                string path = "C:/Inetpub/wwwroot/yesbank/MobImages/" + Filename;
                System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                using (System.Drawing.Image thumbnail = image.GetThumbnailImage(100, 100, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        thumbnail.Save(memoryStream, ImageFormat.Png);
                        Byte[] bytes = new Byte[memoryStream.Length];
                        memoryStream.Position = 0;
                        memoryStream.Read(bytes, 0, (int)bytes.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        if (Img == "Image2")
                        {
                            Image2.ImageUrl = "data:image/png;base64," + base64String;
                        }

                        if (Img == "Image4")
                        {
                            Image4.ImageUrl = "data:image/png;base64," + base64String;
                        }

                        if (Img == "Image3")
                        {
                            Image3.ImageUrl = "data:image/png;base64," + base64String;
                        }

                        if (Img == "imgprevphoto")
                        {
                            imgprevphoto.ImageUrl = "data:image/png;base64," + base64String;
                        }
                        if (Img == "imgprevidproof")
                        {
                            imgprevidproof.ImageUrl = "data:image/png;base64," + base64String;
                        }

                        if (Img == "imgprevresiproof")
                        {
                            imgprevresiproof.ImageUrl = "data:image/png;base64," + base64String;
                        }
                    }
                }
            }
            catch
            { }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }


        protected void imgiprooftxtclose_click(object sender, EventArgs e)
        {
            txtidproof.Visible = false;
            imgiprooftxtclose.Visible = false;
            ddlID.Visible = true;
        }

        protected void imgresiprooftxtclose_click(object sender, EventArgs e)
        {
            txtresoproof.Visible = false;
            imgresiprooftxtclose.Visible = false;
            ddlResi.Visible = true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                btnAddChildTV.Visible = false;
                ViewState["basic_poids"] = null;

                data();
                ViewState["Custstb"] = null;

                ViewState["Custvc"] = null;
                ViewState["apiResponse"] = null;
                ViewState["SDHD"] = null;
                ViewState["TV"] = null;
                if (RadSearchby.SelectedValue == "1")
                {

                    ViewState["RadSearchby"] = RadSearchby.SelectedValue;
                    if (txtStbnosearch.Text.Trim() == "" && txtSearchParam.Text.Trim() == "" && txtsearchMACID.Text.Trim() == "")
                    {
                        msgboxstr("Please Enter STB ID/VC ID OR MAC ID/VM . ");
                        return;
                    }
                    if (txtStbnosearch.Text.Trim() != "")
                    {
                        if (txtSearchParam.Text.Trim() == "")
                        {
                            msgboxstr("VC ID can not be blank");
                            return;
                        }
                        if (txtsearchMACID.Text.Trim() != "")
                        {
                            msgboxstr("Please enter either STB ID and VC ID OR MAC ID/VM .");
                            return;
                        }
                        lblSTBID.Visible = true;
                        lblVCID.Visible = true;
                    }
                    if (txtSearchParam.Text.Trim() != "")
                    {
                        if (txtStbnosearch.Text.Trim() == "")
                        {
                            msgboxstr("STB ID can not be blank");
                            return;
                        }
                        if (txtsearchMACID.Text.Trim() != "")
                        {
                            msgboxstr("Please enter either STB ID and VC ID OR MAC ID/VM .");
                            return;
                        }
                        lblSTBID.Visible = true;
                        lblVCID.Visible = true;
                    }
                    //if (txtSearchParam.Text.Trim() == "")  -- comment by RP on/11/07/2017
                    //{
                    //    txtSearchParam.Focus();
                    //    msgboxstr("Please Provide VC ID");
                    //    return;
                    //}

                    //if (txtStbnosearch.Text.Trim() == "")
                    //{
                    //    txtStbnosearch.Focus();
                    //    msgboxstr("STB ID cannot be blank");
                    //    return;
                    //}
                }
                else
                {
                    ViewState["RadSearchby"] = RadSearchby.SelectedValue;
                    if (txtSearchParam.Text.Trim() == "")
                    {
                        txtSearchParam.Focus();
                        msgboxstr("Please Provide VC ID or A/c. No. or STB ID");
                        return;
                    }
                }

                String SearchFlag = "";

                if (RadSearchby.SelectedValue == "0")
                {
                    string response_params = "";
                    if (rdoSearchParamType.SelectedValue == "0")
                    {
                        SearchFlag = "VC";
                        response_params = user_brmpoid + "$" + txtSearchParam.Text.Trim() + "$SW$V";
                    }
                    else if (rdoSearchParamType.SelectedValue == "1")
                    {
                        SearchFlag = "ACC";
                        response_params = user_brmpoid + "$" + txtSearchParam.Text.Trim() + "$SW";
                    }
                    else if (rdoSearchParamType.SelectedValue == "2")
                    {
                        //SearchFlag = "STB";

                        //Cls_BLL_mstCrf obj = new Cls_BLL_mstCrf();
                        //string response = obj.FetchCustomerDetail(Session["username"].ToString(), txtSearchParam.Text.Trim(), SearchFlag);

                        //if (response.Split('$')[0].ToString() == "9999")
                        //{

                        //    String[] Deatilsarr = response.Split('$')[1].ToString().Split('~');

                        //    response_params = user_brmpoid + "$" + Deatilsarr[14].ToString() + "$SW";
                        //}
                        //else
                        //{
                        //    msgboxstr(response.Split('$')[1].ToString());
                        //    return;
                        //}
                        SearchFlag = "STB";
                        response_params = user_brmpoid + "$" + txtSearchParam.Text.Trim() + "$SW$V";
                    }

                    // string apiResponse = callAPI(response_params, "25"); COMMEND BY RP ON 11.07.2017
                    string apiResponse = callAPI(response_params, "12");
                    FillCustomerDetail(apiResponse);
                }
                else
                {
                    string strMacID = "";
                    if (txtsearchMACID.Text.Trim() != "")
                    {
                        strMacID = txtsearchMACID.Text.Trim();
                    }
                    else if (txtSearchParam.Text.Trim() != "")
                    {
                        strMacID = txtSearchParam.Text.Trim();
                    }
                    string response_params = user_brmpoid + "$" + strMacID + "$SW$V";
                    if (rdoSearchParamType.SelectedValue == "0")
                    {
                        SearchFlag = "VC";
                    }
                    else if (rdoSearchParamType.SelectedValue == "1")
                    {
                        SearchFlag = "ACC";
                    }
                    else if (rdoSearchParamType.SelectedValue == "2")
                    {
                        SearchFlag = "STB";
                    }

                    string apiResponse = callAPI(response_params, "12");


                    List<string> lstResponse = new List<string>();
                    lstResponse = apiResponse.Split('$').ToList();
                    string cust_id = lstResponse[0];

                    if (lstResponse[1].ToString() == "-1")
                    {
                        if (cust_id == "*")
                        {
                        }
                    }
                    if (cust_id != "*")
                    {
                        if (txtSearchParam.Text != "")
                        {
                            msgboxstr("VC is already Pair with another STB No. Please use another VC");
                            return;
                        }
                        else
                        {
                            //  msgboxstr("MAC ID is already Pair with another STB No. Please use another MAC ID");
                            msgboxstr("MAC-id/VM is not available in inventory. Please use another MAC-id/VM .");
                            return;
                        }
                    }
                    Cls_BLL_mstCrf objVal = new Cls_BLL_mstCrf();
                    string LCOCODE = objVal.GetLcoCode(Session["operator_id"].ToString());
                    Session["MainLCOCODE"] = LCOCODE;
                    if (LCOCODE == "")
                    {
                        msgboxstr("LCO Details not found");
                        return;
                    }

                    string FLD_FLAGS = "";
                    String DEVICE_ID = "";
                    string reqcode = "";
                    string req = "";
                    String StrPriErroResponse = "";
                    String[] final_obrm_status;

                    string APISERIAL_NO = "";
                    string APIVENDOR_WARRANTY_END = "";
                    string PAIWARRANTY_END = "";
                    string APIACCOUNT_NO = "";
                    string APICATEGORY = "";
                    string APICOMPANY = "";
                    string APIDEVICE_ID = "";
                    string APIDEVICE_TYPE = "";
                    string APIMANUFACTURER = "";
                    string APIMODEL = "";
                    string APISOURCE = "";
                    string APISTATE_ID = "";
                    string poid = "";
                    string DELIVERY_STATUS = "";
                    if (txtsearchMACID.Text.Trim() != "")
                    {
                        DEVICE_ID = txtsearchMACID.Text.Trim();
                        FLD_FLAGS = "121";
                        reqcode = "40";
                        req = Session["username"].ToString() + "$" + DEVICE_ID + "$" + FLD_FLAGS;
                        StrPriErroResponse = callAPI(req, reqcode);
                        final_obrm_status = StrPriErroResponse.Split('$');

                        if (final_obrm_status[0].ToString() == "0")
                        {
                            APISERIAL_NO = final_obrm_status[1].ToString();
                            APIVENDOR_WARRANTY_END = final_obrm_status[2].ToString();
                            PAIWARRANTY_END = final_obrm_status[3].ToString();
                            APIACCOUNT_NO = final_obrm_status[4].ToString();
                            APICATEGORY = final_obrm_status[5].ToString();
                            APICOMPANY = final_obrm_status[6].ToString();
                            APIDEVICE_ID = final_obrm_status[7].ToString();
                            APIDEVICE_TYPE = final_obrm_status[8].ToString();
                            APIMANUFACTURER = final_obrm_status[9].ToString();
                            APIMODEL = final_obrm_status[10].ToString();
                            APISOURCE = final_obrm_status[11].ToString();
                            APISTATE_ID = final_obrm_status[12].ToString().Trim();
                            poid = final_obrm_status[13].ToString().Trim();
                            DELIVERY_STATUS = final_obrm_status[14].ToString().Trim();
                            if (APISOURCE != LCOCODE)
                            {
                                msgboxstr("Device does not belong to this LCO");
                                return;
                            }

                            if (DELIVERY_STATUS == "2")
                            {
                                msgboxstr("This Device is undelivered. It can not be activated");
                                return;
                            }

                            if (DELIVERY_STATUS == "3")
                            {
                                msgboxstr("This Device is Faulty. It can not be activated");
                                return;
                            }

                            if (DELIVERY_STATUS == "4")
                            {
                                msgboxstr("This Device is Lost in Transit. It can not be activated");
                                return;
                            }
                        }
                        else
                        {
                            msgboxstr("Error while calling Obrm OP Search");
                            return;
                        }
                    }

                    if (txtStbnosearch.Text.Trim() != "")
                    {
                        DEVICE_ID = txtStbnosearch.Text.Trim();
                        FLD_FLAGS = "121";
                        reqcode = "40";
                        req = Session["username"].ToString() + "$" + DEVICE_ID + "$" + FLD_FLAGS;
                        StrPriErroResponse = callAPI(req, reqcode);
                        final_obrm_status = StrPriErroResponse.Split('$');

                        if (final_obrm_status[0].ToString() == "0")
                        {
                            APISERIAL_NO = final_obrm_status[1].ToString();
                            APIVENDOR_WARRANTY_END = final_obrm_status[2].ToString();
                            PAIWARRANTY_END = final_obrm_status[3].ToString();
                            APIACCOUNT_NO = final_obrm_status[4].ToString();
                            APICATEGORY = final_obrm_status[5].ToString();
                            APICOMPANY = final_obrm_status[6].ToString();
                            APIDEVICE_ID = final_obrm_status[7].ToString();
                            APIDEVICE_TYPE = final_obrm_status[8].ToString();
                            APIMANUFACTURER = final_obrm_status[9].ToString();
                            APIMODEL = final_obrm_status[10].ToString();
                            APISOURCE = final_obrm_status[11].ToString();
                            APISTATE_ID = final_obrm_status[12].ToString().Trim();
                            poid = final_obrm_status[13].ToString().Trim();
                            DELIVERY_STATUS = final_obrm_status[14].ToString().Trim();
                            if (APISOURCE != LCOCODE)
                            {
                                msgboxstr("Device does not belong to this LCO");
                                return;
                            }

                            if (DELIVERY_STATUS == "2")
                            {
                                msgboxstr("This Device is undelivered. It can not be activated");
                                return;
                            }

                            if (DELIVERY_STATUS == "3")
                            {
                                msgboxstr("This Device is Faulty. It can not be activated");
                                return;
                            }

                            if (DELIVERY_STATUS == "4")
                            {
                                msgboxstr("This Device is Lost in Transit. It can not be activated");
                                return;
                            }
                        }
                        else
                        {
                            msgboxstr("Error while calling Obrm OP Search");
                            return;
                        }
                    }




                    if (txtSearchParam.Text.Trim() != "")
                    {
                        DEVICE_ID = txtSearchParam.Text.Trim();
                        FLD_FLAGS = "120";
                        reqcode = "40";
                        req = Session["username"].ToString() + "$" + DEVICE_ID + "$" + FLD_FLAGS;
                        StrPriErroResponse = callAPI(req, reqcode);
                        final_obrm_status = StrPriErroResponse.Split('$');

                        if (final_obrm_status[0].ToString() == "0")
                        {
                            APISERIAL_NO = final_obrm_status[1].ToString();
                            APIVENDOR_WARRANTY_END = final_obrm_status[2].ToString();
                            PAIWARRANTY_END = final_obrm_status[3].ToString();
                            APIACCOUNT_NO = final_obrm_status[4].ToString();
                            APICATEGORY = final_obrm_status[5].ToString();
                            APICOMPANY = final_obrm_status[6].ToString();
                            APIDEVICE_ID = final_obrm_status[7].ToString();
                            APIDEVICE_TYPE = final_obrm_status[8].ToString();
                            APIMANUFACTURER = final_obrm_status[9].ToString();
                            APIMODEL = final_obrm_status[10].ToString();
                            APISOURCE = final_obrm_status[11].ToString();
                            APISTATE_ID = final_obrm_status[12].ToString().Trim();
                            poid = final_obrm_status[13].ToString().Trim();
                            DELIVERY_STATUS = final_obrm_status[14].ToString().Trim();
                            if (APISOURCE != LCOCODE)
                            {
                                msgboxstr("Device does not belong to this LCO");
                                return;
                            }

                            if (DELIVERY_STATUS == "2")
                            {
                                msgboxstr("This Device is undelivered. It can not be activated");
                                return;
                            }

                            if (DELIVERY_STATUS == "3")
                            {
                                msgboxstr("This Device is Faulty. It can not be activated");
                                return;
                            }

                            if (DELIVERY_STATUS == "4")
                            {
                                msgboxstr("This Device is Lost in Transit. It can not be activated");
                                return;
                            }
                        }
                        else
                        {
                            msgboxstr("Error while calling Obrm OP Search");
                            return;
                        }
                    }






                    string strVCNO = "", strSTBNO = "", strMACID = "";
                    if (txtsearchMACID.Text != "")
                    {
                        strVCNO = txtsearchMACID.Text.Trim();
                        strSTBNO = txtsearchMACID.Text.Trim();
                        ViewState["strVCNO"] = "";
                        ViewState["strSTBNO"] = strSTBNO;
                    }
                    else
                    {
                        strVCNO = txtSearchParam.Text.Trim();
                        strSTBNO = txtStbnosearch.Text.Trim();
                        ViewState["strVCNO"] = strVCNO;
                        ViewState["strSTBNO"] = strSTBNO;
                    }



                    //string result = objVal.Validatenewcust(Session["username"].ToString(), strSTBNO, strVCNO);

                    //if (result.Split('$')[0].ToString() == "9999")
                    //{
                    if (txtSearchParam.Text != "")
                    {
                        lblRenewNowmsg.Text = "VC [" + txtSearchParam.Text.Trim() + "] is available do you want to pair with STB [" + txtStbnosearch.Text.Trim() + "]";
                    }
                    else
                    {
                        lblRenewNowmsg.Text = "MAC ID [" + txtsearchMACID.Text.Trim() + "] is available do you want to activate ";
                    }
                    PopUpRenewNowConfirm.Show();
                    //}
                    //else
                    //{
                    // msgboxstr(result.Split('$')[1].ToString());
                    //}
                }
            }
            catch (Exception ex)
            {
                divTabPanels.Visible = false;
                divsubmit.Visible = false;
                msgboxstr("Error while getting Customer details");
            }

        }

        protected void btnrenewnowconfimreconfirm_Click(object sender, EventArgs e)
        {
            ViewState["Custstb"] = txtStbnosearch.Text.Trim();
            ViewState["Custvc"] = txtSearchParam.Text.Trim();
            if (txtsearchMACID.Text != "")
            {
                ViewState["Custstb"] = txtsearchMACID.Text.Trim();
                ViewState["Custvc"] = txtsearchMACID.Text.Trim();
            }
            divTabPanels.Visible = true;
            divsubmit.Visible = true;
            txtcity.Text = Session["city"].ToString();
            String Response = FillAddDeatil("P", Session["city"].ToString());
            ddlpin.Items.Clear();
            DataTable Tblpincode = new DataTable();
            Tblpincode.Columns.Add("ID");
            Tblpincode.Columns.Add("Name");
            ViewState["State"] = Response.Split('*')[1].ToString();
            string[] Responsearr = Response.Split('*')[0].Split('$');

            foreach (string str in Responsearr)
            {
                if (str != "")
                {
                    String[] strarr = str.Split('~');
                    Tblpincode.Rows.Add(strarr[1], strarr[0]);
                }
            }
            ddlpin.Items.Clear();
            if (Tblpincode.Rows.Count > 0)
            {
                ListItem lst = new ListItem();
                ddlpin.DataTextField = "ID";
                ddlpin.DataValueField = "Name";

                ddlpin.DataSource = Tblpincode;
                ddlpin.DataBind();
                ddlpin.Items.Insert(0, new ListItem("-- Select Zip Code --", ""));
            }

        }

        protected void btnRenewNowConfirm_Click(object sender, EventArgs e)
        {
            if (txtSearchParam.Text != "")
            {
                Label26.Text = "VC [" + txtSearchParam.Text.Trim() + "] is available do you want to pair with STB [" + txtStbnosearch.Text.Trim() + "]";
            }
            else
            {
                Label26.Text = "MAC ID [" + txtsearchMACID.Text.Trim() + "] is available do you want to activate ";
            }
            popreconfrimpair.Show();
        }

        public void bindAlltable(string service_str)
        {
            ViewState["dtAddonPlans"] = null;
            ViewState["dtAddonPlansReg"] = null;
            ViewState["dtBasicPlans"] = null;
            ViewState["basic_poids"] = "";
            ViewState["old_plan_name"] = "";

            ViewState["ServicePoid"] = service_str.Split('!')[0];

            DataTable dtAddonPlans = new DataTable();
            DataTable dtBasicPlans = new DataTable();
            DataTable dtAddonPlansReg = new DataTable();
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("DEAL_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtBasicPlans.Columns.Add(new DataColumn("LCO_PRICE"));
            dtBasicPlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtBasicPlans.Columns.Add(new DataColumn("EXPIRY"));
            dtBasicPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtBasicPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_STATUS"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtBasicPlans.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtBasicPlans.Columns.Add(new DataColumn("GRACE"));

            dtAddonPlansReg.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAddonPlansReg.Columns.Add(new DataColumn("PLAN_POID"));
            dtAddonPlansReg.Columns.Add(new DataColumn("DEAL_POID"));
            dtAddonPlansReg.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAddonPlansReg.Columns.Add(new DataColumn("LCO_PRICE"));
            dtAddonPlansReg.Columns.Add(new DataColumn("ACTIVATION"));
            dtAddonPlansReg.Columns.Add(new DataColumn("EXPIRY"));
            dtAddonPlansReg.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAddonPlansReg.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAddonPlansReg.Columns.Add(new DataColumn("PLAN_STATUS"));

            // created by vivek 16-nov-2015 
            dtAddonPlansReg.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtAddonPlansReg.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtAddonPlansReg.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtAddonPlansReg.Columns.Add(new DataColumn("GRACE"));

            dtAddonPlans.Columns.Add(new DataColumn("PLAN_NAME"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("DEAL_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("CUST_PRICE"));
            dtAddonPlans.Columns.Add(new DataColumn("LCO_PRICE"));
            dtAddonPlans.Columns.Add(new DataColumn("ACTIVATION"));
            dtAddonPlans.Columns.Add(new DataColumn("EXPIRY"));
            dtAddonPlans.Columns.Add(new DataColumn("PACKAGE_ID"));
            dtAddonPlans.Columns.Add(new DataColumn("PURCHASE_POID"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_STATUS"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_RENEWFLAG"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_CHANGEFLAG"));
            dtAddonPlans.Columns.Add(new DataColumn("PLAN_ACTIONFLAG"));
            dtAddonPlans.Columns.Add(new DataColumn("GRACE"));




            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            string all_plan_string = service_str.Split('!')[3]; //--this is plan string under service

            string city = "";
            if (Session["cityID"] != null && Session["cityID"].ToString() != "")
            {
                city = Session["cityID"].ToString();
            }
            string service_data = obj.getServiceDataDL(Session["username"].ToString(), city, all_plan_string, ViewState["customer_no"].ToString());
            string[] service_data_arr = service_data.Split('#');
            if (service_data_arr[0] != "9999")
            {

            }
            else
            {
                string basic_data_str = service_data_arr[1];
                string basic_poids = "";
                if (basic_data_str != null && basic_data_str != "")
                {
                    string[] basic_plan_arr = basic_data_str.Split('~');
                    basic_poids = dataTableBuilder(dtBasicPlans, basic_plan_arr, "B");

                    ViewState["dtBasicPlans"] = dtBasicPlans;
                    ViewState["old_plan_name"] = dtBasicPlans.Rows[0]["plan_name"].ToString();
                    ViewState["expdt"] = dtBasicPlans.Rows[0]["EXPIRY"].ToString();
                    ViewState["ActDate"] = dtBasicPlans.Rows[0]["ACTIVATION"].ToString();
                    ViewState["Packageid"] = dtBasicPlans.Rows[0]["PACKAGE_ID"].ToString(); ;
                    ViewState["Dealid"] = dtBasicPlans.Rows[0]["DEAL_POID"].ToString(); ;
                    ViewState["basic_poids"] = basic_poids;
                }

                string addon_data_str = service_data_arr[2];
                string addon_poids = "";
                if (addon_data_str != null && addon_data_str != "")
                {
                    string[] addon_plan_arr = addon_data_str.Split('~');
                    addon_poids = dataTableBuilder(dtAddonPlans, addon_plan_arr, "AD");
                    ViewState["dtAddonPlans"] = dtAddonPlans;
                }
                else
                {
                }

                string alacarte_data_str = service_data_arr[3];
                string ala_poids = "";
                if (alacarte_data_str != null && alacarte_data_str != "")
                {
                    string[] alacarte_plan_arr = alacarte_data_str.Split('~');
                    ala_poids = dataTableBuilder(dtAddonPlans, alacarte_plan_arr, "AL");
                    ViewState["dtAddonPlans"] = dtAddonPlans;
                }
                else
                {
                }

            }

        }

        protected string dataTableBuilder(DataTable dt, string[] arr_data, string plan_type_flag)
        {
            string poid_list = "";
            int contvalue = 0;
            foreach (string plan_data in arr_data)
            {
                string[] plan_details_arr = plan_data.Split('$');
                string plan_poid = plan_details_arr[0];
                string plan_name = plan_details_arr[1];
                string plan_custprice = plan_details_arr[2];
                string plan_lcoprice = plan_details_arr[3];
                string plan_activation = plan_details_arr[4];
                string plan_expiry = plan_details_arr[5];
                string plan_dealpoid = plan_details_arr[6];
                string plan_package = plan_details_arr[7];
                string plan_purchase = plan_details_arr[8];
                string plan_status = plan_details_arr[9];
                string plan_renewflag = "";
                string plan_changeflag = "";
                string plan_actionflag = "";
                string Plan_Grace_Date = "";
                // created by vivek 16-nov-2015 
                plan_renewflag = plan_details_arr[10];
                plan_changeflag = plan_details_arr[11];
                plan_actionflag = plan_details_arr[12];
                Plan_Grace_Date = plan_details_arr[13];

                // if (plan_type_flag.Trim() == "B") //if basic plans, then accept only active poids
                // {
                if (plan_status.Trim() == "Active")
                {
                    poid_list = "" + plan_poid + "";
                    if (plan_type_flag.Trim() == "B")
                    {
                        contvalue = contvalue + 1;
                    }
                }
                // }
                // else
                // { //if addon and alacarte plans, then accept all poids
                //     poid_list += "'" + plan_poid + "',";
                // }

                DataRow tempDr = dt.NewRow();
                tempDr["PLAN_POID"] = plan_poid;
                tempDr["PLAN_NAME"] = plan_name;
                //tempDr["PLAN_TYPE"] = "B";
                tempDr["DEAL_POID"] = plan_dealpoid;
                //tempDr["PRODUCT_POID"] = dr_plan[0]["PRODUCT_POID"];
                tempDr["CUST_PRICE"] = plan_custprice;
                tempDr["LCO_PRICE"] = plan_lcoprice;
                //tempDr["PAYTERM"] = dr_plan[0]["PAYTERM"];
                //tempDr["CITYID"] = city;
                //tempDr["CITY_NAME"] = dr_plan[0]["CITY_NAME"];
                //tempDr["COMPANY_CODE"] = dr_plan[0]["COMPANY_CODE"];
                //tempDr["INSDT"] = dr_plan[0]["INSDT"];
                //tempDr["INSBY"] = dr_plan[0]["INSBY"];
                tempDr["ACTIVATION"] = plan_activation;
                tempDr["EXPIRY"] = plan_expiry;
                tempDr["PACKAGE_ID"] = plan_package;
                tempDr["PURCHASE_POID"] = plan_purchase;
                tempDr["PLAN_STATUS"] = plan_status;
                // created by vivek 16-nov-2015 

                tempDr["PLAN_RENEWFLAG"] = plan_renewflag;
                tempDr["PLAN_CHANGEFLAG"] = plan_changeflag;
                tempDr["PLAN_ACTIONFLAG"] = plan_actionflag;
                tempDr["GRACE"] = Plan_Grace_Date;

                dt.Rows.Add(tempDr);
            }
            ViewState["Countvalue"] = contvalue;
            return poid_list;
        }//shri

        public void FillCustomerDetail(String apiResponse)
        {
            ViewState["Custstb"] = null;
            ViewState["Custvc"] = null;
            string oper_id = "";
            string user_brmpoid = "";
            if (Session["operator_id"] != null && Session["username"] != null && Session["user_brmpoid"] != null)
            {
                username = Convert.ToString(Session["username"]);
                oper_id = Convert.ToString(Session["operator_id"]);
                user_brmpoid = Convert.ToString(Session["user_brmpoid"]);
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/Login.aspx");
            }

            List<string> lstResponse = new List<string>();
            lstResponse = apiResponse.Split('$').ToList();
            string cust_id = lstResponse[0];
            ViewState["customer_no"] = lstResponse[0];
            string lco_poid = lstResponse[13];
            Cls_Validation obj = new Cls_Validation();
            string validate_cust_accesslco = obj.CustDataAccess(username, oper_id, lco_poid, Session["category"].ToString());

            if (validate_cust_accesslco.Length == 0)
            {
                msgboxstr("You have no privileges to access customer information as s/he belongs to other LCO");
                return;
            }
            else
            {
                if (cust_id != "*")
                {


                    ViewState["apiResponse"] = apiResponse;

                    btnAddChildTV.Visible = true;

                    ViewState["SDHD"] = lstResponse[15].ToString().Split('!')[5].ToString();
                    ViewState["TV"] = lstResponse[15].ToString().Split('!')[6].ToString();
                    ViewState["Status"] = lstResponse[15].ToString().Split('!')[4].ToString();
                    ViewState["accountPoid"] = lstResponse[6];

                    ViewState["CustNo"] = cust_id;
                    ViewState["CustName"] = lstResponse[3].ToString();
                    ViewState["Custadd"] = lstResponse[4].ToString();

                    if (lstResponse[5].ToString() != "")
                    {
                        txtmobno.Text = lstResponse[5].ToString();
                    }
                    txtfname.Text = lstResponse[3].ToString().Split(' ')[0].ToString();
                    txtmname.Text = lstResponse[3].ToString().Split(' ')[1].ToString();
                    txtlname.Text = lstResponse[3].ToString().Split(' ')[2].ToString();
                    txtadd.Text = lstResponse[4].ToString();
                    txtemailid.Text = lstResponse[1].ToString();

                    txtcity.Text = Session["city"].ToString();
                    ViewState["CustCity"] = Session["city"].ToString();
                    ViewState["Custpin"] = lstResponse[2].ToString();

                    String Response = FillAddDeatil("P", ViewState["CustCity"].ToString());
                    ddlpin.Items.Clear();
                    DataTable Tblpincode = new DataTable();
                    Tblpincode.Columns.Add("ID");
                    Tblpincode.Columns.Add("Name");
                    ViewState["State"] = Response.Split('*')[1].ToString();
                    string[] Responsearr = Response.Split('*')[0].Split('$');

                    foreach (string str in Responsearr)
                    {
                        if (str != "")
                        {
                            String[] strarr = str.Split('~');
                            Tblpincode.Rows.Add(strarr[1], strarr[0]);
                        }
                    }

                    if (Tblpincode.Rows.Count > 0)
                    {
                        ListItem lst = new ListItem();
                        ddlpin.DataTextField = "ID";
                        ddlpin.DataValueField = "Name";

                        ddlpin.DataSource = Tblpincode;
                        ddlpin.DataBind();
                        ddlpin.Items.Insert(0, new ListItem("-- Select Zip Code --", ""));

                        try
                        {
                            if (lstResponse[2].ToString() != "")
                            {
                                ddlpin.SelectedValue = lstResponse[2].ToString();


                                ddlpin_SelectedIndexChanged(null, null);
                                if (lstResponse[9].ToString() != "")
                                {
                                    ddlarea.SelectedIndex = ddlarea.Items.IndexOf(ddlarea.Items.FindByText(lstResponse[9].ToString()));
                                    string srea = ddlarea.SelectedValue;
                                    ddlarea_SelectedIndexChanged(null, null);
                                    if (lstResponse[12].ToString() != "")
                                    {
                                        ddlstreet.SelectedValue = lstResponse[12].ToString();
                                        ddlstreet_SelectedIndexChanged(null, null);
                                        if (lstResponse[11].ToString() != "")
                                        {
                                            ddllocation.SelectedValue = lstResponse[11].ToString();
                                            ddllocation_SelectedIndexChanged(null, null);
                                            if (lstResponse[10].ToString() != "")
                                            {
                                                ddlbuilding.SelectedValue = lstResponse[10].ToString();
                                            }
                                        }
                                    }


                                }
                            }
                        }
                        catch (Exception exx)
                        {

                        }
                    }
                    divTabPanels.Visible = true;
                    divsubmit.Visible = true;


                    string cust_services = lstResponse[15];
                    string[] service_arr = cust_services.Split('^');
                    String vc_id = "";
                    String parent_child_flag = "";
                    String serviceinfores = "";
                    String stb_no = "";
                    String Vc = "";
                    if (rdoSearchParamType.SelectedValue == "0")
                    {
                        Vc = txtSearchParam.Text.Trim();
                    }
                    foreach (string service in service_arr)
                    {
                        vc_id = service.Split('!')[2];
                        stb_no = service.Split('!')[1];
                        ViewState["strVCNO"] = vc_id;
                        ViewState["strSTBNO"] = stb_no;
                        parent_child_flag = service.Split('!')[6];

                        if (rdoSearchParamType.SelectedValue == "0")
                        {
                            if (Vc.Trim() == vc_id.Trim())
                            {
                                serviceinfores = service;
                                break;
                            }
                        }
                        else
                        {
                            if (parent_child_flag == "0")
                            {
                                serviceinfores = service;
                                break;
                            }
                        }
                    }

                    ViewState["cust_services"] = lstResponse[15];

                    ViewState["Custstb"] = stb_no;
                    ViewState["Custvc"] = vc_id;


                    bindAlltable(serviceinfores);
                    divTabPanels.Visible = true;
                    divTabPanels1.Visible = false;
                    btnsubmit.Text = "Next >>";

                    int k = 1;
                    foreach (string service in service_arr)
                    {
                        parent_child_flag = service.Split('!')[6];
                        if (parent_child_flag == "1")
                        {
                            k = k + 1;
                        }
                        else
                        {

                        }

                    }

                    ViewState["Childtvcount"] = k;

                    if (k > 2)
                    {
                        btnAddChildTV.Visible = false;
                    }

                    DataTable Tblcustdetail = new DataTable();
                    GetCustDetail(cust_id, vc_id, stb_no, Tblcustdetail);

                    if (Tblcustdetail.Rows.Count > 0)
                    {
                        if (Tblcustdetail.Rows[0]["imagepath"].ToString() != "http://localhost/yesbank/MobImages/")
                        {

                            string fileName = Path.GetFileName(Tblcustdetail.Rows[0]["imagepath"].ToString());

                            ViewState["photoimage"] = fileName;
                            GenerateThumbnail(fileName, "Image4");
                            //Image4.ImageUrl = "http://hathwayconnect.com/yesbank/MobImages/" + fileName;

                            lblphotoname.Visible = true;
                            lblphotoname.Text = fileName.Replace('_', ' ');
                            imgbtnphoto.Visible = true;
                            fupphoto.Visible = false;
                        }
                        else
                        {
                            Image4.ImageUrl = "~/Images/no_doc_uploaded.jpg";
                        }

                        if (Tblcustdetail.Rows[0]["signpath"].ToString() != "http://localhost/yesbank/MobImages/")
                        {
                            string fileName = Path.GetFileName(Tblcustdetail.Rows[0]["signpath"].ToString());
                            ViewState["PDFDOCNAME"] = fileName;
                            ViewState["Pdfdocimage"] = fileName;
                            lblPDFDOCNAME.Visible = true;
                            lblPDFDOCNAME.Text = fileName.Replace('_', ' ');
                            imgbtnclose.Visible = true;
                            fuploadpdf.Visible = false;
                            Image8.Visible = true;
                            Image8.ImageUrl = "~/Images/no-preview.jpg";
                        }
                        else
                        {
                            Image8.ImageUrl = "~/Images/no_doc_uploaded.jpg";
                            Image8.Visible = true;
                        }

                        if (Tblcustdetail.Rows[0]["idproofpath"].ToString() != "http://localhost/yesbank/MobImages/")
                        {
                            string fileName = Path.GetFileName(Tblcustdetail.Rows[0]["idproofpath"].ToString());
                            string strpath = Path.GetExtension(Tblcustdetail.Rows[0]["idproofpath"].ToString());
                            ViewState["idproofimage"] = fileName;
                            ViewState["idproofimagePDF"] = fileName;
                            ViewState["idprooftrpath"] = strpath;
                            if (strpath != ".pdf")
                            {
                                Image2.Visible = true;
                                GenerateThumbnail(fileName, "Image2");
                                //Image2.ImageUrl = "http://hathwayconnect.com/yesbank/MobImages/" + fileName;
                            }
                            else
                            {
                                Image2.Visible = true;
                                Image2.ImageUrl = "~/Images/no-preview.jpg";
                            }

                            lblidproofname.Visible = true;
                            lblidproofname.Text = fileName.Replace('_', ' '); ;
                            imgbtnidproof.Visible = true;
                            fupidproof.Visible = false;
                        }
                        else
                        {
                            Image2.ImageUrl = "~/Images/no_doc_uploaded.jpg";
                        }

                        if (Tblcustdetail.Rows[0]["addressproofpath"].ToString() != "http://localhost/yesbank/MobImages/")
                        {
                            string fileName = Path.GetFileName(Tblcustdetail.Rows[0]["addressproofpath"].ToString());
                            string strpath = Path.GetExtension(Tblcustdetail.Rows[0]["addressproofpath"].ToString());
                            ViewState["resiproof"] = fileName;
                            ViewState["resiproofpath"] = strpath;
                            ViewState["resiproofpdf"] = fileName;
                            if (strpath != ".pdf")
                            {
                                Image3.Visible = true;
                                GenerateThumbnail(fileName, "Image3");
                                //Image3.ImageUrl = "http://hathwayconnect.com/yesbank/MobImages/" + fileName;
                            }
                            else
                            {
                                Image3.Visible = true;
                                Image3.ImageUrl = "~/Images/no-preview.jpg";
                            }

                            lblreiprrofname.Visible = true;
                            lblreiprrofname.Text = fileName.Replace('_', ' ');
                            imgbtnresiproof.Visible = true;
                            fupresiproof.Visible = false;
                        }
                        else
                        {
                            Image3.ImageUrl = "~/Images/no_doc_uploaded.jpg";
                        }

                        if (Tblcustdetail.Rows[0]["IDPROOFID"].ToString() != "")
                        {
                            ddlID.SelectedValue = Tblcustdetail.Rows[0]["IDPROOFID"].ToString();
                        }
                        if (ddlID.SelectedItem.Text.Trim().ToUpper() == "OTHER")
                        {
                            ddlID.Visible = false;
                            txtidproof.Visible = true;
                            imgiprooftxtclose.Visible = true;
                            txtidproof.Text = Tblcustdetail.Rows[0]["IDPROOFVALUE"].ToString();
                        }

                        if (Tblcustdetail.Rows[0]["RESIPROOFID"].ToString() != "")
                        {
                            ddlResi.SelectedValue = Tblcustdetail.Rows[0]["RESIPROOFID"].ToString();
                        }

                        if (ddlResi.SelectedItem.Text.Trim().ToUpper() == "OTHER")
                        {
                            ddlResi.Visible = false;
                            txtresoproof.Visible = true;
                            imgresiprooftxtclose.Visible = true;
                            txtresoproof.Text = Tblcustdetail.Rows[0]["RESIPROOFVALUE"].ToString();
                        }

                        if (Tblcustdetail.Rows[0]["cafno"].ToString() != "")
                        {
                            ViewState["cafno"] = Tblcustdetail.Rows[0]["cafno"].ToString();
                        }
                        else
                        {
                            ViewState["cafno"] = "";
                        }

                    }
                }
                else
                {
                    msgboxstr(lstResponse[2].ToString());
                    return;
                }
            }
        }

        public void GetCustDetail(String Accno, String VC, String Stbno, DataTable TblDetail)
        {
            string strCon = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection con = new OracleConnection(strCon);
            con.Open();

            String str = "select * from view_ecaf_customerdetail_repor " +
              "where   accno= '" + Accno + "' and stbno='" + Stbno + "' and vcid='" + VC + "' and updatedonform='Y'";



            OracleCommand Cmd = new OracleCommand(str, con);
            OracleDataAdapter AdpData = new OracleDataAdapter();
            AdpData.SelectCommand = Cmd;
            AdpData.Fill(TblDetail);

            con.Close();

        }

        public void msgboxstr(string message)
        {
            // lnkatag_Click(null, null);
            lblPopupResponse.Text = message;
            popMsg.Show();
        }

        protected void ddlpin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlpin.SelectedValue.Trim() != "")
            {
                String Response = FillAddDeatil("A", ddlpin.SelectedValue.Trim());

                DataTable TblArea = new DataTable();
                TblArea.Columns.Add("ID");
                TblArea.Columns.Add("Name");
                string[] Responsearr = Response.Split('*')[0].Split('$');

                foreach (string str in Responsearr)
                {
                    if (str != "")
                    {
                        String[] strarr = str.Split('~');
                        TblArea.Rows.Add(strarr[1], strarr[0]);
                    }
                }
                ddlarea.Items.Clear();
                if (TblArea.Rows.Count > 0)
                {
                    ListItem lst = new ListItem();
                    ddlarea.DataTextField = "Name";
                    ddlarea.DataValueField = "ID";

                    ddlarea.DataSource = TblArea;
                    ddlarea.DataBind();
                    ddlarea.Items.Insert(0, new ListItem("-- Select Area --", ""));
                }
                ddlstreet.Items.Clear();
                ddllocation.Items.Clear();
                ddlbuilding.Items.Clear();
            }
            else
            {
                ddlarea.Items.Clear();
                ddlstreet.Items.Clear();
                ddllocation.Items.Clear();
                ddlbuilding.Items.Clear();
            }
        }

        protected void ddlarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlarea.SelectedValue.Trim() != "")
            {
                String Response = FillAddDeatil("S", ddlarea.SelectedValue.Trim());

                DataTable TblStreet = new DataTable();
                TblStreet.Columns.Add("ID");
                TblStreet.Columns.Add("Name");
                string[] Responsearr = Response.Split('*')[0].Split('$');

                foreach (string str in Responsearr)
                {
                    if (str != "")
                    {
                        String[] strarr = str.Split('~');
                        TblStreet.Rows.Add(strarr[1], strarr[0]);
                    }
                }
                ddlstreet.Items.Clear();
                if (TblStreet.Rows.Count > 0)
                {
                    ddlstreet.DataTextField = "ID";
                    ddlstreet.DataValueField = "Name";

                    ddlstreet.DataSource = TblStreet;
                    ddlstreet.DataBind();
                    ddlstreet.Items.Insert(0, new ListItem("-- Select Street --", ""));
                    ddlstreet_SelectedIndexChanged(null, null);
                }
                ddllocation.Items.Clear();
                ddlbuilding.Items.Clear();
            }
            else
            {
                ddlstreet.Items.Clear();
                ddllocation.Items.Clear();
                ddlbuilding.Items.Clear();
            }
        }

        protected void ddlstreet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlstreet.SelectedValue.Trim() != "")
            {
                String Response = FillAddDeatil("L", ddlstreet.SelectedValue.Trim());

                DataTable TblLocation = new DataTable();
                TblLocation.Columns.Add("ID");
                TblLocation.Columns.Add("Name");
                string[] Responsearr = Response.Split('*')[0].Split('$');

                foreach (string str in Responsearr)
                {
                    if (str != "")
                    {
                        String[] strarr = str.Split('~');
                        TblLocation.Rows.Add(strarr[1], strarr[0]);
                    }
                }
                ddllocation.Items.Clear();
                if (TblLocation.Rows.Count > 0)
                {
                    ddllocation.DataTextField = "ID";
                    ddllocation.DataValueField = "Name";

                    ddllocation.DataSource = TblLocation;
                    ddllocation.DataBind();
                    ddllocation.Items.Insert(0, new ListItem("-- Select Location --", ""));
                    ddllocation_SelectedIndexChanged(null, null);
                }
                ddlbuilding.Items.Clear();
            }
            else
            {
                ddllocation.Items.Clear();
                ddlbuilding.Items.Clear();
            }
        }

        protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddllocation.SelectedValue.Trim() != "")
            {
                String Response = FillAddDeatil("B", ddllocation.SelectedValue.Trim());

                DataTable TblBulding = new DataTable();
                TblBulding.Columns.Add("ID");
                TblBulding.Columns.Add("Name");
                string[] Responsearr = Response.Split('*')[0].Split('$');

                foreach (string str in Responsearr)
                {
                    if (str != "")
                    {
                        String[] strarr = str.Split('~');
                        TblBulding.Rows.Add(strarr[1], strarr[0]);
                    }
                }
                ddlbuilding.Items.Clear();
                if (TblBulding.Rows.Count > 0)
                {
                    ddlbuilding.DataTextField = "ID";
                    ddlbuilding.DataValueField = "Name";

                    ddlbuilding.DataSource = TblBulding;
                    ddlbuilding.DataBind();
                    ddlbuilding.Items.Insert(0, new ListItem("-- Select Bulding --", ""));

                }

            }
            else
            {
                ddlbuilding.Items.Clear();
            }
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            divTabPanels.Visible = true;
            divTabPanels1.Visible = false;
            btnsubmit.Text = "Next >>";
            btnback.Visible = false;
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            if (btnsubmit.Text.Trim() == "Next >>")
            {
                txtOtp.Text = "";
                txtOtp.Focus();
                lblerrmsgotp.Text = "";
                if (RadSearchby.SelectedValue == "0")
                {
                    if (txtSearchParam.Text.Trim() == "")
                    {
                        txtSearchParam.Focus();
                        msgboxstr("Please Enter VC ID");
                        return;
                    }
                }
                if (RadSearchby.SelectedValue == "1")
                {
                    //if (txtStbnosearch.Text.Trim() == "")
                    //{
                    //    txtStbnosearch.Focus();
                    //    msgboxstr("STB ID cannot be blank");
                    //    return;
                    //}
                    if (txtStbnosearch.Text.Trim() == "" && txtSearchParam.Text.Trim() == "" && txtsearchMACID.Text.Trim() == "")
                    {
                        msgboxstr("Please Enter STB ID/VC ID OR MAC ID ");
                        return;
                    }
                    if (txtStbnosearch.Text.Trim() != "")
                    {
                        if (txtSearchParam.Text.Trim() == "")
                        {
                            msgboxstr("VC ID can not be blank");
                            return;
                        }
                        if (txtsearchMACID.Text.Trim() != "")
                        {
                            msgboxstr("Please enter either STB ID and VC ID OR MAC ID");
                            return;
                        }
                    }
                    if (txtSearchParam.Text.Trim() != "")
                    {
                        if (txtStbnosearch.Text.Trim() == "")
                        {
                            msgboxstr("STB ID can not be blank");
                            return;
                        }
                        if (txtsearchMACID.Text.Trim() != "")
                        {
                            msgboxstr("Please enter either STB ID and VC ID OR MAC ID");
                            return;
                        }
                    }
                }

                //first name
                if (txtfname.Text.Trim() == "")
                {
                    txtfname.Focus();
                    msgboxstr("First Name cannot be blank");
                    return;
                }
                else
                {
                    string valid = SecurityValidation.chkData("T", txtfname.Text);
                    if (valid != "")
                    {
                        txtfname.Focus();
                        msgboxstr(valid.ToString() + " at First Name");
                        return;
                    }
                }




                //middle name
                if (txtmname.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("T", txtmname.Text);
                    if (valid != "")
                    {
                        txtmname.Focus();
                        msgboxstr(valid.ToString() + " at Middle Name");
                        return;
                    }
                }



                //last name
                if (txtlname.Text.Trim() == "")
                {
                    txtlname.Focus();
                    msgboxstr("Last Name cannot be blank");
                    return;
                }
                else
                {
                    string valid = SecurityValidation.chkData("T", txtlname.Text);
                    if (valid != "")
                    {
                        txtlname.Focus();
                        msgboxstr(valid.ToString() + " at Last Name");
                        return;
                    }
                }


                //mobile number
                if (txtmobno.Text.Trim() == "")
                {
                    txtmobno.Focus();
                    msgboxstr("Mobile No. cannot be blank");
                    return;
                }
                else
                {
                    string valid = SecurityValidation.chkData("N", txtmobno.Text);
                    if (valid != "")
                    {
                        txtmobno.Focus();
                        msgboxstr(valid.ToString() + " at Mobile Number");
                        return;
                    }
                }

                //LandLine
                if (txtlandline.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("N", txtlandline.Text);
                    if (valid != "")
                    {
                        txtlandline.Focus();
                        msgboxstr(valid.ToString() + " at LandLine Number");
                        return;
                    }
                }



                Regex rx = new Regex(@"^(0|\+91)?[789]\d{9}$");

                if (rx.IsMatch(txtmobno.Text.Trim()) == false || txtmobno.Text.Length > 10)
                {
                    txtmobno.Focus();
                    msgboxstr("Please Provide proper mobile No.");
                    return;
                }


                // Email
                if (txtemailid.Text.Trim() != "")
                {
                    string valid = SecurityValidation.chkData("T", txtemailid.Text);
                    if (valid == "")
                    {
                        bool email = emailIsValid(txtemailid.Text.Trim());
                        if (email == false)
                        {
                            txtemailid.Focus();
                            msgboxstr("Please provide proper email id");
                            return;
                        }
                    }
                    else
                    {
                        txtemailid.Focus();
                        msgboxstr(valid + " at Email ID");
                        return;
                    }
                }


                if (ddlpin.SelectedValue == "")
                {
                    msgboxstr("Please select Pin Code");
                    return;
                }



                if (ddlarea.SelectedValue == "")
                {
                    msgboxstr("Please select Area");
                    return;
                }
                if (txtadd.Text.Trim() == "")
                {
                    txtadd.Focus();
                    msgboxstr("Pease provide Address");
                    return;
                }



                //Flat No
                if (txtflatno.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("T", txtflatno.Text);
                    if (valid != "")
                    {
                        txtflatno.Focus();
                        msgboxstr(valid.ToString() + " at Flat Number");
                        return;
                    }
                }

                //Address
                if (txtadd.Text.Length > 0)
                {
                    string valid = SecurityValidation.chkData("T", txtadd.Text);
                    if (valid != "")
                    {
                        txtadd.Focus();
                        msgboxstr(valid.ToString() + " at Address");
                        return;
                    }
                }


                divTabPanels.Visible = false;
                divTabPanels1.Visible = true;
                btnsubmit.Text = "Submit";
                btnback.Visible = true;
            }

            else
            {

                txtOtp.Text = "";
                txtOtp.Focus();
                lblerrmsgotp.Text = "";

                /* if (ddlstreet.SelectedValue == "")
                 {
                     msgboxstr("Please select Street");
                     return;
                 }
                 if (ddllocation.SelectedValue == "")
                 {
                     msgboxstr("Please select Location");
                     return;
                 }
                 if (ddlbuilding.SelectedValue == "")
                 {
                     msgboxstr("Please select Building");
                     return;
                 }*/

                lblfirstname.Text = txtfname.Text.Trim();
                lbllastname.Text = txtlname.Text.Trim();

                lblmidname.Text = txtmname.Text.Trim();
                lblmobno.Text = txtmobno.Text.Trim();
                lbllandline.Text = txtlandline.Text.Trim();
                lblemail.Text = txtemailid.Text.Trim();
                lblpin.Text = ddlpin.SelectedValue;
                lblcity.Text = txtcity.Text;
                lblarea.Text = ddlarea.SelectedItem.Text;
                lblstreet.Text = ddlstreet.SelectedValue;
                lbllocation.Text = ddllocation.SelectedValue;
                lblbuilding.Text = ddlbuilding.SelectedValue;
                lblflatno.Text = txtflatno.Text;
                lbladress.Text = txtadd.Text;
                if (ddlID.SelectedValue != "")
                {
                    lblidproof.Text = ddlID.SelectedItem.Text;
                }

                if (ddlID.SelectedItem.Text.Trim().ToUpper() == "OTHER")
                {
                    lblidproof.Text = txtidproof.Text;
                }


                if (ddlResi.SelectedValue != "")
                {
                    lblresiproof.Text = ddlResi.SelectedItem.Text;
                }

                if (ddlResi.SelectedItem.Text.Trim().ToUpper() == "OTHER")
                {
                    lblresiproof.Text = txtresoproof.Text;

                }
                if (ViewState["photoimage"] != null)
                {
                    GenerateThumbnail(ViewState["photoimage"].ToString(), "imgprevphoto");
                    //imgprevphoto.ImageUrl = "http://hathwayconnect.com/yesbank/MobImages/" + ViewState["photoimage"].ToString();
                }
                else
                {
                    imgprevphoto.ImageUrl = "~/Images/no_doc_uploaded.jpg"; //sanket
                }

                if (ViewState["idproofimage"] != null)
                {

                    if (ViewState["idprooftrpath"].ToString() != ".pdf")
                    {
                        imgprevidproof.Visible = true;
                        lblprevidprood.Visible = false;
                        GenerateThumbnail(ViewState["idproofimage"].ToString(), "imgprevidproof");
                        // imgprevidproof.ImageUrl = "http://hathwayconnect.com/yesbank/MobImages/" + ViewState["idproofimage"].ToString();
                    }
                    else
                    {


                        imgprevidproof.Visible = true;
                        lblprevidprood.Visible = true;
                        imgprevidproof.ImageUrl = "~/Images/no-preview.jpg";
                        lblprevidprood.Text = ViewState["idproofimagePDF"].ToString();
                    }
                }

                else
                {
                    imgprevidproof.Visible = true;
                    lblprevidprood.Visible = false;
                    imgprevidproof.ImageUrl = "~/Images/no_doc_uploaded.jpg";
                }

                if (ViewState["resiproof"] != null)
                {
                    if (ViewState["resiproofpath"].ToString() != ".pdf")
                    {
                        imgprevresiproof.Visible = true;
                        lblprevresiproof.Visible = false;
                        GenerateThumbnail(ViewState["resiproof"].ToString(), "imgprevresiproof");
                        //imgprevresiproof.ImageUrl = "http://hathwayconnect.com/yesbank/MobImages/" + ViewState["resiproof"].ToString();
                    }
                    else
                    {

                        imgprevresiproof.Visible = true;
                        lblprevresiproof.Visible = true;
                        imgprevresiproof.ImageUrl = "~/Images/no-preview.jpg";
                        lblprevresiproof.Text = ViewState["resiproofpdf"].ToString();
                    }


                }

                else
                {
                    imgprevresiproof.Visible = true;
                    lblprevresiproof.Visible = false;
                    imgprevresiproof.ImageUrl = "~/Images/no_doc_uploaded.jpg";
                }

                if (ViewState["PDFDOCNAME"] != null)
                {
                    lblprevpdf.Text = ViewState["PDFDOCNAME"].ToString();
                    Image9.Visible = true;
                    Image9.ImageUrl = "~/Images/no-preview.jpg";
                }
                else
                {
                    lblprevpdf.Text = ""; //ViewState["PDFDOCNAME"].ToString();
                    Image9.Visible = true;
                    Image9.ImageUrl = "~/Images/no_doc_uploaded.jpg";
                }



                poppreview.Show();

            }

        }

        protected void BtnPreviewconfrim_Click(object sender, EventArgs e)
        {

            ViewState["OTP"] = "";


            Cls_BLL_mstCrf ob = new Cls_BLL_mstCrf();

            string otpID = ob.getOtpNewCust(Session["username"].ToString(), txtmobno.Text.Trim()); //ViewState["mobile"].ToString()

            ViewState["OTP"] = otpID.ToString();

            popupRenewAll.Show();
        }

        protected void BtnAutoRenewAll_Click(object sender, EventArgs e)
        {

            lblerrmsgotp.Text = "";
            Cls_BLL_mstCrf objVal = new Cls_BLL_mstCrf();

            string result = objVal.ValidOtpNewCust(Session["username"].ToString(), ViewState["OTP"].ToString(), txtOtp.Text.Trim());


            if (result.Split('$')[0].ToString() == "9999")
            {
                if (ViewState["basic_poids"] != null)
                {
                    GetBaseplan(ViewState["basic_poids"].ToString().TrimEnd(',').Replace("'", "").ToString());
                }
                string Basicpoid = "";
                if (ViewState["basic_poids"] != null)
                {

                    Basicpoid = ViewState["basic_poids"].ToString();

                }
                string Status = objVal.CheckBasicPlanType(Session["username"].ToString(), Basicpoid);


                DataTable TblPlan = new DataTable();
                TblPlan.Columns.Add("plan_name");
                TblPlan.Columns.Add("plantype");
                TblPlan.Columns.Add("planpoid");
                TblPlan.Columns.Add("dealpoid");
                TblPlan.Columns.Add("productpoid");
                TblPlan.Columns.Add("custprice");
                TblPlan.Columns.Add("lcoprice");
                TblPlan.Columns.Add("payterm");
                TblPlan.Columns.Add("devicetype");

                String TV = "";
                String SDHD = "";
                if (ViewState["TV"] != null)
                {
                    TV = ViewState["TV"].ToString();
                }
                else
                {
                    TV = "";
                }

                if (ViewState["SDHD"] != null)
                {
                    SDHD = ViewState["SDHD"].ToString();
                }
                else
                {
                    SDHD = "";
                }


                string strVCNO = "", strSTBNO = "", strMACID = "";
                if (txtsearchMACID.Text != "")
                {
                    strVCNO = "";
                    strSTBNO = ViewState["strSTBNO"].ToString();
                }
                else
                {
                    strVCNO = ViewState["strVCNO"].ToString();
                    strSTBNO = ViewState["strSTBNO"].ToString();
                }



                string Response = objVal.getBasePlanDetails(Session["username"].ToString(), TV, SDHD, 1, strVCNO, strSTBNO);

                if (Response != "")
                {
                    String[] Responsesrr = Response.Split('~');
                    foreach (string strres in Responsesrr)
                    {
                        if (strres != "")
                        {
                            String[] resfinal = strres.Split('$');
                            TblPlan.Rows.Add(resfinal[0].ToString(), resfinal[1].ToString(), resfinal[2].ToString(), resfinal[3].ToString(), resfinal[4].ToString(), resfinal[5].ToString(),
                                resfinal[6].ToString(), resfinal[7].ToString(), resfinal[8].ToString());

                        }
                    }




                    if (TblPlan.Rows.Count > 0)
                    {
                        GrdPlan.DataSource = TblPlan;
                        GrdPlan.DataBind();



                        //if (ViewState["basic_poids"] != null)
                        //{
                        //    foreach (GridViewRow gr in GrdPlan.Rows)
                        //    {
                        //        RadioButton rb = (RadioButton)gr.Cells[0].FindControl("RbntCheckplan");
                        //        if (rb != null)
                        //        {
                        //            Label hf = (Label)gr.Cells[0].FindControl("lblplanname");
                        //            HiddenField hfpkgid = (HiddenField)gr.Cells[0].FindControl("hdnplanpoid");
                        //            if (ViewState["basic_poids"].ToString().TrimEnd(',').Replace("'", "").ToString() == hfpkgid.Value)
                        //            {
                        //                rb.Checked = true;
                        //                ViewState["Old_PlanName"] = hf.Text;
                        //            }
                        //        }
                        //    }
                        //}
                    }

                    if (RadSearchby.SelectedValue == "0")
                    {
                        Oldplan.Visible = true;
                    }
                    else
                    {
                        Oldplan.Visible = false;
                    }

                    if (Status == "Y")
                    {
                        PopUpPlan.Show();
                    }
                    else
                    {
                        PopCnfm.Show();
                        lblStb.Text = ViewState["Custstb"].ToString();

                        lblVC.Text = ViewState["Custvc"].ToString();

                        if (ViewState["Old_PlanName"] != null)
                        {
                            lblBasePack.Text = ViewState["Old_PlanName"].ToString();
                        }
                        ViewState["planpoid"] = ViewState["basic_poids"];
                        Oldplan.Visible = false;
                    }




                }

            }
            else
            {
                popupRenewAll.Show();
                lblerrmsgotp.Text = result.Split('$')[1].ToString();
            }
        }

        protected void rdbplanpayterm_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_BLL_mstCrf objVal = new Cls_BLL_mstCrf();
            DataTable TblPlan = new DataTable();
            TblPlan.Columns.Add("plan_name");
            TblPlan.Columns.Add("plantype");
            TblPlan.Columns.Add("planpoid");
            TblPlan.Columns.Add("dealpoid");
            TblPlan.Columns.Add("productpoid");
            TblPlan.Columns.Add("custprice");
            TblPlan.Columns.Add("lcoprice");
            TblPlan.Columns.Add("payterm");
            TblPlan.Columns.Add("devicetype");

            String TV = "";
            String SDHD = "";
            if (ViewState["TV"] != null)
            {
                TV = ViewState["TV"].ToString();
            }
            else
            {
                TV = "";
            }

            if (ViewState["SDHD"] != null)
            {
                SDHD = ViewState["SDHD"].ToString();
            }
            else
            {
                SDHD = "";
            }

            string strVCNO = "", strSTBNO = "", strMACID = "";
            if (txtsearchMACID.Text != "")
            {
                strVCNO = "";
                strSTBNO = ViewState["strSTBNO"].ToString();
            }
            else
            {
                strVCNO = ViewState["strVCNO"].ToString();
                strSTBNO = ViewState["strSTBNO"].ToString();
            }

            string Response = objVal.getBasePlanDetails(Session["username"].ToString(), TV, SDHD, Convert.ToInt32(rdbplanpayterm.SelectedValue), strVCNO, strSTBNO);

            if (Response != "")
            {
                String[] Responsesrr = Response.Split('~');
                foreach (string strres in Responsesrr)
                {
                    if (strres != "")
                    {
                        String[] resfinal = strres.Split('$');
                        TblPlan.Rows.Add(resfinal[0].ToString(), resfinal[1].ToString(), resfinal[2].ToString(), resfinal[3].ToString(), resfinal[4].ToString(), resfinal[5].ToString(),
                            resfinal[6].ToString(), resfinal[7].ToString(), resfinal[8].ToString());

                    }
                }

                if (TblPlan.Rows.Count > 0)
                {
                    GrdPlan.DataSource = TblPlan;
                    GrdPlan.DataBind();

                    if (Convert.ToString(ViewState["basic_poids"]) != "")
                    {
                        foreach (GridViewRow gr in GrdPlan.Rows)
                        {
                            RadioButton rb = (RadioButton)gr.Cells[0].FindControl("RbntCheckplan");
                            if (rb != null)
                            {
                                Label hf = (Label)gr.Cells[0].FindControl("lblplanname");
                                HiddenField hfpkgid = (HiddenField)gr.Cells[0].FindControl("hdnplanpoid");

                                if (ViewState["basic_poids"].ToString().TrimEnd(',').Replace("'", "").ToString() == hfpkgid.Value)
                                {
                                    rb.Checked = true;
                                    //ViewState["Old_PlanName"] = hf.Text;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void btnPopCnfm_Click(object sender, EventArgs e)
        {
            if (chkTerm.Checked == false)
            {
                lblchkterm.Visible = true;
                lblchkterm.Text = "Please select Terms & Conditions";
                PopCnfm.Show();
                return;
            }
            else
            {
                lblchkterm.Visible = false;
                lblchkterm.Text = "";
            }

            if (ViewState["RadSearchby"].ToString() == "0")
            {
                ExistingCutomerUpdate();
            }
            else if (ViewState["RadSearchby"].ToString() == "1")
            {

                NewCustomerCreation();
            }
        }

        public void ExistingCutomerUpdate()
        {

            DataTable dtFOCPlanStatus = new DataTable();
            dtFOCPlanStatus.Columns.Add("PlanName");
            dtFOCPlanStatus.Columns.Add("RenewStatus");

            string user_brmpoid = Session["user_brmpoid"].ToString();
            ViewState["user_brmpoid"] = user_brmpoid;

            string obrm_status = "";

            string apiResponse = "";

            if (ViewState["apiResponse"] != null)
            {
                apiResponse = ViewState["apiResponse"].ToString();
            }
            else
            {
                apiResponse = "";
            }


            // string apiResponse = "1090702162$$$PRASANT KUMAR BAHINIPATI$TEST4$0$0.0.0.1 /account 3701719292 88$0.0.0.1 /account 3701719292 88$*$*$*$*$*$0.0.0.1 /account 2083044737 28$1090702162$0.0.0.1 /service/catv 3701721090 12!5945038149851732!000090309360!0.0.0.1 /plan 604303747 0|2016-01-06T00:00:00Z|1970-01-01T05:30:00Z|0.0.0.1 /deal 604307075 0|76917802|0.0.0.1 /purchased_product 3701721218 5|1!10100!SD!0";
            try
            {
                if (apiResponse != "")
                {
                    List<string> lstResponse = new List<string>();
                    lstResponse = apiResponse.Split('$').ToList();
                    string account_no = lstResponse[0];
                    ViewState["accountno"] = account_no;
                    ViewState["customer_name"] = lstResponse[3];
                    ViewState["cust_addr"] = lstResponse[4];


                    string accountPoid = lstResponse[6];
                    ViewState["accountPoid"] = accountPoid;
                    string cust_services = lstResponse[15];
                    string[] service_arr = cust_services.Split('^');
                    string Service_Poid = service_arr[0].Split('!')[0];
                    string Plan_Det = service_arr[0].Split('!')[3];
                    string[] Plan_arr = Plan_Det.Split('|');
                    string Old_Plan_poid = ViewState["basic_poids"].ToString(); //Plan_arr[0].Substring(Plan_arr[0].IndexOf("plan") + 5);
                    //Old_Plan_poid = Old_Plan_poid.Substring(0, Old_Plan_poid.LastIndexOf(" "));
                    string Plan_Dealpoid = Plan_arr[3].ToString();
                    string PackageId = Plan_arr[4].ToString();
                    string DELIVERYPREFER = lstResponse[7];
                    string PayinfoObj = lstResponse[8];

                    string Address = txtadd.Text.ToString().Trim();
                    string FristName = txtfname.Text.ToString().Trim();
                    string lastname = txtlname.Text.ToString().Trim();
                    string txtmob = txtmobno.Text.ToString().Trim();
                    string txtmiddlename = txtmname.Text.ToString().Trim();

                    String statename = ViewState["State"].ToString();
                    String areacode = "";
                    if (ddlarea.SelectedValue != "")
                    {
                        String[] arrarea = ddlarea.SelectedValue.Split('-');

                        areacode = arrarea[0].ToString() + "|" + arrarea[0].ToString() + "-" + arrarea[1].ToString() + "-" + arrarea[2].ToString() + "|" + ddlarea.SelectedValue;
                    }
                    String num_obrm_res_idproof = ddlResi.SelectedValue;
                    String num_obrm_photo_idproof = ddlID.SelectedValue;
                    String Custcity = txtcity.Text.Trim();
                    String streetname = ddlstreet.SelectedValue;
                    string area = ddlarea.SelectedItem.Text;
                    string location = ddllocation.SelectedValue;
                    String buildname = ddlbuilding.SelectedValue;
                    string cust_email = txtemailid.Text.Trim();


                    string responsemodify_params = user_brmpoid + "$" + account_no + "$" + accountPoid + "$" + Service_Poid + "$" + Address + "$" + FristName + "$" + lastname + "$" + txtmob + "$" + ddlpin.SelectedValue + "$" + Custcity + "$" + streetname + "$" + area + "$" + buildname + "$" + location + "$" + cust_email + "$" + num_obrm_photo_idproof + "$" + num_obrm_res_idproof + "$" + DELIVERYPREFER + "$" + PayinfoObj + "$" + areacode + "$" + statename + "$" + txtmiddlename;

                    try
                    {
                        string modifyapi_resp = callAPI(responsemodify_params, "16");
                        string[] final_obrm_status = modifyapi_resp.Split('$');
                        obrm_status = final_obrm_status[1];
                        string obrm_msg = "";

                        if (obrm_status == "0")
                        {
                            dtFOCPlanStatus.Rows.Add("Customer Deatils", final_obrm_status[2].ToString());

                            bool status = false;
                            if (ViewState["basic_poids"].ToString() != "")
                            {
                                if (ViewState["basic_poids"].ToString() != ViewState["planpoid"].ToString())
                                {
                                    status = processChangePlanTransaction("CH", ViewState["Planid"].ToString(), ViewState["expdt"].ToString(), "", "");

                                    DataRow row1 = dtFOCPlanStatus.NewRow();
                                    row1["PlanName"] = ViewState["OldPlanName"].ToString();
                                    row1["RenewStatus"] = ViewState["ErrorMessage"];
                                    dtFOCPlanStatus.Rows.Add(row1);

                                    if (status == true)
                                    {

                                        DataTable dtAddonPlans = (DataTable)ViewState["dtAddonPlans"];
                                        if (dtAddonPlans != null)
                                        {
                                            for (int rowindexi = 1; rowindexi <= dtAddonPlans.Rows.Count; rowindexi++)
                                            {
                                                int rowindex = rowindexi - 1;
                                                String hdnPlanName = dtAddonPlans.Rows[rowindex]["plan_name"].ToString();

                                                String hdnPlanPoid = dtAddonPlans.Rows[rowindex]["PLAN_POID"].ToString();
                                                ViewState["planpoid"] = hdnPlanPoid;

                                                ViewState["activation"] = dtAddonPlans.Rows[rowindex]["ACTIVATION"].ToString(); ;
                                                ViewState["expiry"] = dtAddonPlans.Rows[rowindex]["EXPIRY"].ToString();

                                                ViewState["packageid"] = dtAddonPlans.Rows[rowindex]["PACKAGE_ID"].ToString();
                                                ViewState["dealpoid"] = dtAddonPlans.Rows[rowindex]["DEAL_POID"].ToString();

                                                if (hdnPlanName.Trim().Contains("FREE") == true)
                                                {
                                                    processTransaction("C");
                                                    dtFOCPlanStatus.Rows.Add(hdnPlanName, ViewState["ErrorMessage"].ToString());
                                                }

                                            }
                                            for (int rowindexi = 1; rowindexi <= dtAddonPlans.Rows.Count; rowindexi++)
                                            {
                                                int rowindex = rowindexi - 1;
                                                String hdnPlanName = dtAddonPlans.Rows[rowindex]["plan_name"].ToString();

                                                String hdnPlanPoid = dtAddonPlans.Rows[rowindex]["PLAN_POID"].ToString();
                                                ViewState["planpoid"] = hdnPlanPoid;

                                                ViewState["activation"] = dtAddonPlans.Rows[rowindex]["ACTIVATION"].ToString(); ;
                                                ViewState["expiry"] = dtAddonPlans.Rows[rowindex]["EXPIRY"].ToString();

                                                ViewState["packageid"] = dtAddonPlans.Rows[rowindex]["PACKAGE_ID"].ToString();
                                                ViewState["dealpoid"] = dtAddonPlans.Rows[rowindex]["DEAL_POID"].ToString();

                                                if (ViewState["old_plan_name"].ToString().Contains("BASIC") == true || ViewState["old_plan_name"].ToString().Contains("PRIME") == true)
                                                {
                                                    if (hdnPlanName.Trim().Contains("SPECIAL") == true)
                                                    {
                                                        Cancellation_ProcessSpecial(hdnPlanPoid, "SPECIAL", hdnPlanName, ViewState["Custvc"].ToString());
                                                        dtFOCPlanStatus.Rows.Add(hdnPlanName, ViewState["ErrorMessage"].ToString());
                                                        break;
                                                    }
                                                }

                                            }
                                        }


                                        bool statusadd = processChangePlanTransaction("A", ViewState["Newplan_Poid"].ToString(), ViewState["expdt"].ToString(), "", "");

                                        DataRow row2 = dtFOCPlanStatus.NewRow();
                                        row2["PlanName"] = ViewState["NewPlanName"].ToString();
                                        row2["RenewStatus"] = ViewState["ErrorMessage"];
                                        dtFOCPlanStatus.Rows.Add(row2);
                                    }
                                }
                            }
                            else
                            {
                                processTransaction("A");

                                dtFOCPlanStatus.Rows.Add(ViewState["PlanName"].ToString(), ViewState["ErrorMessage"].ToString());
                            }

                            Hashtable ht = new Hashtable();

                            ht["custtype"] = "";
                            ht["appicanttit"] = "Mr";
                            ht["fname"] = txtfname.Text.Trim();
                            ht["lname"] = txtlname.Text.Trim();
                            ht["mob"] = txtmobno.Text.Trim();
                            ht["landline"] = txtlandline.Text.Trim();
                            ht["email"] = txtemailid.Text.Trim();
                            ht["pin"] = ddlpin.SelectedValue;
                            ht["street"] = ddlstreet.SelectedValue;
                            ht["city"] = txtcity.Text.Trim();
                            ht["location"] = ddllocation.SelectedValue;
                            ht["area"] = ddlarea.SelectedItem.Text;
                            ht["building"] = ddlbuilding.SelectedValue;
                            ht["flatno"] = txtflatno.Text.Trim();
                            ht["stbno"] = ViewState["Custstb"].ToString();
                            ht["vcno"] = ViewState["Custvc"].ToString();
                            ht["accno"] = Convert.ToString(ViewState["CustNo"]);
                            ht["add"] = txtadd.Text.Trim();
                            if (ViewState["idproofimage"] != null)
                            {
                                ht["idproofimage"] = ViewState["idproofimage"];
                            }
                            else
                            {
                                ht["idproofimage"] = "";
                            }

                            ht["idvalue"] = ddlID.SelectedValue;
                            ht["macid"] = ViewState["Custvc"].ToString();
                            if (ViewState["photoimage"] != null)
                            {
                                ht["photoimage"] = ViewState["photoimage"].ToString();
                            }
                            else
                            {
                                ht["photoimage"] = "";
                            }
                            ht["resivalue"] = ddlResi.SelectedValue;

                            if (ViewState["resiproof"] != null)
                            {
                                ht["resiproof"] = ViewState["resiproof"].ToString();
                            }
                            else
                            {
                                ht["resiproof"] = "";
                            }

                            if (ViewState["Pdfdocimage"] != null)
                            {
                                ht["signatureimage"] = ViewState["Pdfdocimage"].ToString();
                            }
                            else
                            {
                                ht["signatureimage"] = "";
                            }
                            ht["state"] = ViewState["State"].ToString();
                            ht["Child"] = "N";
                            ht["idproofvalue"] = txtidproof.Text.Trim();
                            ht["resiproofvalue"] = txtresoproof.Text.Trim();
                            ht["cafno"] = Convert.ToString(ViewState["cafno"]);
                            Cls_Data_Auth auth = new Cls_Data_Auth();
                            string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                            ht["ip"] = Ip;
                            Cls_BLL_mstCrf obj = new Cls_BLL_mstCrf();
                            String res = obj.insrtCrfdata(Session["username"].ToString(), ht);

                            GetUserDteail();
                            if (status == true)
                            {
                                data();
                                Gridrenew.DataSource = dtFOCPlanStatus;
                                Gridrenew.DataBind();
                                lblAllStatus.Text = "Pack Status";
                                Gridrenew.HeaderRow.Cells[1].Text = "Change Status";
                                popMsg.Hide();
                                popallrenewal.Show();
                                return;

                            }
                            else
                            {
                                data();
                                Gridrenew.DataSource = dtFOCPlanStatus;
                                Gridrenew.DataBind();
                                lblAllStatus.Text = "Pack Status";
                                Gridrenew.HeaderRow.Cells[1].Text = "Change Status";
                                popMsg.Hide();
                                popallrenewal.Show();
                                return;
                            }
                        }
                        else
                        {
                            lblPopupResponse.Text = "Modify in OBRM failed," + final_obrm_status[2];
                            data();
                            popMsg.Show();
                            return;


                        }
                    }
                    catch (Exception ex)
                    {
                        obrm_status = "1";
                        // obrm_msg = api_response;
                        // msgbox("Transaction  done in upass Successful but Error while call Modify API from OBRM");
                        lblPopupResponse.Text = "Transaction  done in upass Successful but Error while call Modify API from OBRM";
                        data();
                        popMsg.Show();
                        return;

                    }

                }



            }
            catch (Exception ex)
            {
                lblPopupResponse.Text = "Error while Get customer information  from OBRM";
                data();
                popMsg.Show();
                return;
                // msgbox("Transaction  done in upass Successful but Error while Get customer information  from OBRM");

            }

        }

        protected bool processChangePlanTransaction(string transaction_action, string planpoid, string expdate, string purchage_id, string activationdate)
        {
            try
            {
                //gathering data
                string ChangeStatus = "";
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData = new Hashtable();
                string plan_poid = planpoid;
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                string _tvType = "";
                string oldaction = transaction_action;
                string maintStatus = "INACTIVE";

                if (Session["username"] != null && Session["operator_id"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["lcoid"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }

                try
                {
                    _vc_id = ViewState["Custvc"].ToString();
                    if (ViewState["TV"].ToString() == "0")
                    {
                        _tvType = "MAIN";
                    }
                    else
                    {
                        _tvType = "CHILD";
                    }
                    if (ViewState["Status"].ToString() == "10100")
                    {
                        maintStatus = "ACTIVE";
                    }
                    else
                    {
                        maintStatus = "INACTIVE";
                    }

                    if (ViewState["AddedFOC"] == null)
                    {
                        ViewState["AddedFOC"] = "0";
                    }
                }
                catch (Exception ex)
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";
                    msgboxstr("Something went wrong while transaction. Plan data not found.");
                    Cls_Security objSecurity = new Cls_Security();
                    objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-processChangePlanTransaction");
                    return false;
                }


                //processing 
                string Request = "";
                //validating...
                if (transaction_action == "A")
                {
                    activation_date = "";
                    expiry_date = "";
                }
                else if (transaction_action == "CH")
                {

                    activation_date = ViewState["ActDate"].ToString();
                    expiry_date = ViewState["expdt"].ToString();

                }
                else if (transaction_action == "R")
                {

                    activation_date = activationdate;
                    expiry_date = expdate;

                }

                //------------
                Cls_Business_TxnAssignPlan obj3 = new Cls_Business_TxnAssignPlan();
                string responseCount = obj3.ChannelCount(Session["username"].ToString(), plan_poid);
                string ChannelCount = responseCount.Split('~')[2];
                //--------------

                if (ViewState["ServicePoid"] != null && ViewState["accountPoid"] != null)
                {
                    if (transaction_action == "A")
                    {
                        Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + ChannelCount;
                    }
                    else if (transaction_action == "CH")
                    {
                        Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + ViewState["Packageid"] + "$" + ViewState["Dealid"];
                        // Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + htPlanData["packageid"] + "$" + htPlanData["dealpoid"];
                    }
                    else if (transaction_action == "R")
                    {
                        Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + purchage_id;
                    }






                    ht.Add("username", _username);
                    ht.Add("lcoid", _oper_id);
                    ht.Add("custid", ViewState["CustNo"].ToString());
                    ht.Add("vcid", _vc_id);
                    ht.Add("custname", ViewState["CustName"].ToString());
                    ht.Add("cust_addr", ViewState["Custadd"].ToString());
                    ht.Add("planid", plan_poid);
                    ht.Add("flag", transaction_action);
                    ht.Add("expdate", expiry_date);
                    ht.Add("actidate", activation_date);
                    ht.Add("request", Request);
                    ht.Add("reason_id", reason_id);
                    ht.Add("IP", Ip);

                    ht.Add("MainTVStatus", maintStatus);
                    ht.Add("TVType", _tvType);
                    ht.Add("DeviceType", ViewState["Device_Type"].ToString());

                    ht.Add("FOCCount", ViewState["AddedFOC"].ToString());

                    ht.Add("BasicPoid", ViewState["basic_poids"].ToString());


                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    string response = obj.ValidateProvTrans(ht);
                    string[] res = response.Split('$');

                    if (res[0] != "9999")
                    {
                        if (transaction_action == "C")
                        {
                            msgboxstr(res[1]);

                        }
                        ViewState["ErrorMessage"] = "something went wrong, Error from UPASS:" + res[1];
                        return false;
                    }
                    else
                    {
                        request_id = res[1];
                    }
                }
                else
                {
                    if (transaction_action == "CH")
                    {
                        msgboxstr("something went wrong, Service or account details not found...Please relogin");
                    }
                    ViewState["ErrorMessage"] = "something went wrong,Error from UPASS:Service or account details not found...Please relogin";
                    return false;
                }

                //OBRM call to get response...
                // Request = _username + "$" + Request;

                string req_code = "";
                if (transaction_action == "A")
                {
                    req_code = "5";
                }

                else if (transaction_action == "CH")
                {
                    req_code = "8";
                }
                else if (transaction_action == "R")
                {
                    req_code = "7";
                }
                Request = _user_brmpoid + "$" + Request;
                string api_response = callAPI(Request, req_code);
                //string api_response = "0$ACCOUNT - Service add plan completed successfully$0.0.0.1 /account 81788441 9$0.0.0.1 /service/catv 81788185 39";
                string[] final_obrm_status = api_response.Split('$');
                string obrm_status = final_obrm_status[0];
                string obrm_msg = "";

                try
                {
                    if (obrm_status == "0" || obrm_status == "1")
                    {
                        obrm_msg = final_obrm_status[2];
                    }
                    else
                    {
                        obrm_status = "1";
                        obrm_msg = api_response;

                    }
                }
                catch (Exception ex)
                {
                    obrm_status = "1";
                    obrm_msg = api_response;
                    ViewState["ErrorMessage"] = api_response.ToString();
                }

                ht.Add("obrmsts", obrm_status);
                ht.Add("request_id", request_id);
                ht.Add("response", api_response);

                Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
                string resp = obj1.ProvTransResEcaf(ht); // "9999";
                string[] finalres = resp.Split('$');
                if (finalres[0] == "9999")
                {

                    ViewState["ErrorMessage"] = "Transaction successful.";
                }
                else
                {
                    if (obrm_status == "0")
                    {
                        ViewState["ErrorMessage"] = "Transaction successful by OBRM but failure at UPASS : " + finalres[1];
                    }
                    else
                    {
                        if (transaction_action == "CH")
                        {
                            msgboxstr("Transaction failed : " + finalres[1] + " : " + obrm_msg);
                            ViewState["ErrorMessage"] = "Transaction failed from OBRM";
                        }
                        ViewState["ErrorMessage"] = "Transaction failed from OBRM.";
                    }
                    return false;

                }

                return true;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                ViewState["ErrorMessage"] = ex.Message;
                objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-ProcessTransaction");
                return false;
            }
        }

        public void NewCustomerCreation()
        {
            try
            {
                DataTable dtFOCPlanStatus = new DataTable();
                dtFOCPlanStatus.Columns.Add("PlanName");
                dtFOCPlanStatus.Columns.Add("RenewStatus");

                string obrm_status = "";
                string user_brmpoid = Session["user_brmpoid"].ToString();
                string Address = txtadd.Text.ToString().Trim();
                string FristName = txtfname.Text.ToString().Trim();
                string lastname = txtlname.Text.ToString().Trim();
                string txtmob = txtmobno.Text.ToString().Trim();
                string txtstb = lblStb.Text.ToString().Trim();
                string vcid = "";
                if (lblStb.Text.ToString().Trim() == lblVC.Text.ToString().Trim())
                {
                    vcid = "";
                }
                else
                {
                    vcid = lblVC.Text.ToString().Trim();
                }


                string package = lblBasePack.Text.ToString().Trim();

                String statename = ViewState["State"].ToString();
                String areacode = "";
                if (ddlarea.SelectedValue != "")
                {
                    String[] arrarea = ddlarea.SelectedValue.Split('-');

                    areacode = arrarea[0].ToString() + "|" + arrarea[0].ToString() + "-" + arrarea[1].ToString() + "-" + arrarea[2].ToString() + "|" + ddlarea.SelectedValue;
                }
                String num_obrm_res_idproof = ddlResi.SelectedValue;
                String num_obrm_photo_idproof = ddlID.SelectedValue;
                String Custcity = txtcity.Text.Trim();
                String streetname = ddlstreet.SelectedValue;
                string area = ddlarea.SelectedItem.Text;
                string location = ddllocation.SelectedValue;
                String buildname = ddlbuilding.SelectedValue;
                string cust_email = txtemailid.Text.Trim();


                ViewState["ErrorMessage"] = null;
                //gathering data
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData = new Hashtable();
                string transaction_action = "A"; //A-Add , C-Cancel, R-Renew
                string transaction_type = "B"; //grid from where function is called 

                string plan_poid = "";
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                string _tvType = "";
                string _foccount = "";
                string maintStatus = "INACTIVE";
                if (Session["username"] != null && Session["operator_id"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["operator_id"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }


                _vc_id = vcid;

                _tvType = "MAIN";
                maintStatus = "ACTIVE";
                ViewState["AddedFOC"] = "0";


                //processing 
                string Request = "";
                //validating...
                if (transaction_action == "A")
                {
                    plan_poid = ViewState["planpoid"].ToString();
                    activation_date = "";
                    expiry_date = "";
                }


                ht.Add("username", Session["username"].ToString());
                ht.Add("lcoid", Session["operator_id"].ToString());
                ht.Add("custid", "");
                ht.Add("vcid", lblVC.Text.ToString().Trim());
                ht.Add("custname", txtfname.Text.Trim() + " " + txtmname.Text.Trim() + " " + txtlname.Text.Trim());
                ht.Add("cust_addr", txtadd.Text.Trim());
                ht.Add("planid", plan_poid);
                ht.Add("flag", transaction_action);
                ht.Add("expdate", expiry_date);
                ht.Add("actidate", activation_date);
                ht.Add("request", Request);
                ht.Add("reason_id", reason_id);
                ht.Add("IP", Ip);

                ht.Add("MainTVStatus", maintStatus);
                ht.Add("TVType", _tvType);
                ht.Add("DeviceType", Convert.ToString(ViewState["devicetype"]));

                ht.Add("FOCCount", ViewState["AddedFOC"].ToString());

                ht.Add("BasicPoid", "");

                string strVCNO = "", strSTBNO = "", strMACID = "";
                if (txtsearchMACID.Text != "")
                {
                    strVCNO = "";
                    strSTBNO = ViewState["strSTBNO"].ToString();
                }
                else
                {
                    strVCNO = ViewState["strVCNO"].ToString();
                    strSTBNO = ViewState["strSTBNO"].ToString();
                }


                ht.Add("STBNO", strSTBNO);

                Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                string response = obj.ValidateProvTrans(ht);

                string[] res = response.Split('$');
                if (res[0] != "9999")
                {
                    ViewState["ErrorMessage"] = res[1].ToString();
                    msgboxstr(res[1]);
                    return;
                }
                else
                {
                    if (res[1] == "A")
                    {
                        transaction_action = "A";
                    }
                    else
                    {
                        request_id = res[1];
                    }
                }


                //package = package.Replace("&","andRP");
                string responsemodify_params = user_brmpoid + "$" + FristName + "$" + lastname + "$" + Address + "$" + area + "$" + areacode + "$" + buildname + "$" + streetname + "$" + location + "$" + ddlpin.SelectedValue + "$" + Custcity + "$" + statename + "$" + txtstb + "$" + vcid + "$" + txtmob + "$" + cust_email + "$" + package + "$" + Session["MainLCOCODE"].ToString() + "";// Session["username"].ToString() + "";
                FileLogText1(responsemodify_params, "Call API create user", "", "");

                string modifyapi_resp = callAPI(responsemodify_params, "18");
                FileLogText1(modifyapi_resp, "response API create user", "", "");
                string[] final_obrm_status = modifyapi_resp.Split('$');
                obrm_status = final_obrm_status[1];
                string obrm_msg = "";



                if (obrm_status == "0")
                {
                    ViewState["SDHD"] = ViewState["devicetype"].ToString();
                    String response_params = user_brmpoid + "$" + ViewState["Custvc"].ToString() + "$SW$V";
                    string apiResponse = callAPI(response_params, "25");

                    List<string> lstResponse = new List<string>();
                    lstResponse = apiResponse.Split('$').ToList();
                    string cust_id = lstResponse[0];

                    if (cust_id == "*")
                    {
                        msgboxstr("Unable to fetch account number for VC -" + ViewState["Custvc"].ToString());
                        return;
                    }

                    ht.Add("obrmsts", obrm_status);
                    ht.Add("request_id", request_id);
                    ht.Add("response", modifyapi_resp);
                    ht.Remove("custid");
                    ht.Add("custid", cust_id);

                    Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
                    string resp = obj1.ProvTransResEcaf(ht); // "9999";
                    string[] finalres = resp.Split('$');
                    if (finalres[0] == "9999")
                    {

                    }
                    else
                    {
                        msgboxstr(resp);
                        return;
                    }

                    Hashtable htcust = new Hashtable();

                    htcust["custtype"] = "";
                    htcust["appicanttit"] = "Mr";
                    htcust["fname"] = txtfname.Text.Trim();
                    htcust["lname"] = txtlname.Text.Trim();
                    htcust["mob"] = txtmobno.Text.Trim();
                    htcust["landline"] = txtlandline.Text.Trim();
                    htcust["email"] = txtemailid.Text.Trim();
                    htcust["pin"] = ddlpin.SelectedValue;
                    htcust["street"] = ddlstreet.SelectedValue;
                    htcust["city"] = txtcity.Text.Trim();
                    htcust["location"] = ddllocation.SelectedValue;
                    htcust["area"] = ddlarea.SelectedItem.Text;
                    htcust["building"] = ddlbuilding.SelectedValue;
                    htcust["flatno"] = txtflatno.Text.Trim();
                    htcust["stbno"] = ViewState["Custstb"].ToString();
                    htcust["vcno"] = ViewState["Custvc"].ToString();
                    htcust["accno"] = cust_id;
                    htcust["add"] = txtadd.Text.Trim();
                    if (ViewState["idproofimage"] != null)
                    {
                        htcust["idproofimage"] = ViewState["idproofimage"];
                    }
                    else
                    {
                        htcust["idproofimage"] = "";
                    }

                    htcust["idvalue"] = ddlID.SelectedValue;
                    htcust["macid"] = ViewState["Custvc"].ToString();
                    if (ViewState["photoimage"] != null)
                    {
                        htcust["photoimage"] = ViewState["photoimage"].ToString();
                    }
                    else
                    {
                        htcust["photoimage"] = "";
                    }
                    htcust["resivalue"] = ddlResi.SelectedValue;

                    if (ViewState["resiproof"] != null)
                    {
                        htcust["resiproof"] = ViewState["resiproof"].ToString();
                    }
                    else
                    {
                        htcust["resiproof"] = "";
                    }

                    if (ViewState["Pdfdocimage"] != null)
                    {
                        htcust["signatureimage"] = ViewState["Pdfdocimage"].ToString();
                    }
                    else
                    {
                        htcust["signatureimage"] = "";
                    }
                    htcust["state"] = ViewState["State"].ToString();
                    htcust["Child"] = "N";
                    htcust["idproofvalue"] = txtidproof.Text.Trim();
                    htcust["resiproofvalue"] = txtresoproof.Text.Trim();
                    htcust["cafno"] = Convert.ToString(ViewState["cafno"]);
                    htcust["ip"] = Ip;
                    Cls_BLL_mstCrf objcust = new Cls_BLL_mstCrf();
                    String rescust = objcust.insrtCrfdata(Session["username"].ToString(), htcust);

                    dtFOCPlanStatus.Rows.Add("Customer created successfully! Account No :", cust_id);
                    dtFOCPlanStatus.Rows.Add(ViewState["PlanName"].ToString(), "Plan Added successfully");

                    data();
                    GetUserDteail();
                    Gridrenew.DataSource = dtFOCPlanStatus;
                    Gridrenew.DataBind();
                    lblAllStatus.Text = "Pack Status";
                    Gridrenew.HeaderRow.Cells[1].Text = "New Customer";
                    popMsg.Hide();
                    popallrenewal.Show();
                    return;

                }
                else
                {
                    msgboxstr("Transaction failed : OBRM " + final_obrm_status[1] + " : " + final_obrm_status[2]);
                    data();
                    return;
                }


            }

            catch (Exception ex)
            {
                //System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                //msgboxstr(ex.Message + " Line: " + trace.GetFrame(0).GetFileLineNumber());
                msgboxstr("error while create customer details!");
                data();
                return;
            }



        }

        private void FileLogText1(String Str, String sender, String strRequest, String strResponse)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy");
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(@"C:\temp\Logs\HwayOBRM\HwayObrm_WebEcaf_" + filename + ".txt", true);
                sw.WriteLine(sender + ":-" + Str + "                      " + DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(strRequest.Trim());
                sw.WriteLine(strResponse.Trim());
                sw.WriteLine("************************************************************************************************************************");
            }
            catch (Exception ex)
            {
                //Response.Write("Error while writing logs : " + ex.Message.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }


        protected void btnPopPlan_Click(object sender, EventArgs e)
        {
            ViewState["PlanName"] = null;
            ViewState["planpoid"] = null;
            ViewState["devicetype"] = null;
            lblPlanselect.Text = "";

            for (int i = 0; GrdPlan.Rows.Count > i; i++)
            {

                RadioButton rb = (RadioButton)GrdPlan.Rows[i]
                        .Cells[0].FindControl("RbntCheckplan");
                if (rb != null)
                {
                    if (rb.Checked)
                    {
                        Label hf = (Label)GrdPlan.Rows[i]
                                        .Cells[0].FindControl("lblplanname");
                        HiddenField hfpkgid = (HiddenField)GrdPlan.Rows[i]
                                        .Cells[0].FindControl("hdnplanpoid");
                        HiddenField hfdevicetype = (HiddenField)GrdPlan.Rows[i]
                                        .Cells[0].FindControl("hdndevicetype");
                        if (hf != null)
                        {
                            ViewState["PlanName"] = hf.Text;
                            ViewState["planpoid"] = hfpkgid.Value;
                            ViewState["devicetype"] = hfdevicetype.Value;
                            break;
                        }
                    }
                }
            }

            if (ViewState["PlanName"] == null || ViewState["PlanName"] == "")
            {
                lblPlanselect.Text = "Please select package!";
                PopUpPlan.Show();

            }
            else
            {
                lbladdplanconfirm.Text = "Are you sure you want to add <b>" + ViewState["PlanName"].ToString() + "</b> ?";
                popaddplanconfirm.Show();
            }


        }

        protected void btnepnladdplanconfirm_Click(object sender, EventArgs e)
        {


            PopUpPlan.Hide();

            lblStb.Text = ViewState["Custstb"].ToString();

            lblVC.Text = ViewState["Custvc"].ToString();

            lblBasePack.Text = ViewState["PlanName"].ToString();
            string Oldplan = "";
            if (ViewState["Old_PlanName"] != null)
            {
                Oldplan = ViewState["Old_PlanName"].ToString();
            }
            lblOldBasePack.Text = Oldplan;
            chkTerm.Checked = true;

            PopCnfm.Show();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            popTerms.Show();
        }

        protected void btnTerms_Click(object sender, EventArgs e)
        {
            popTerms.Hide();
            PopCnfm.Show();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            data();
        }

        protected void RadSearchby_SelectedIndexChanged(object sender, EventArgs e)
        {
            data();
            if (RadSearchby.SelectedIndex == 1)
            {
                rdoSearchParamType.Visible = false;
                rdoSearchParamType.SelectedValue = "0";
                txtSearchParam.Text = "";
                txtStbnosearch.Text = "";
                txtsearchMACID.Text = "";
                txtStbnosearch.Visible = true;
                lblvctext.Text = "VC ID <span class='red'>*</span> :";
                lblstbtext.Text = "STB No. <span class='red'>*</span> :";
                lblOr.Text = "<span style='font-weight: bold'> OR </span> ";// " OR ";

                txtsearchMACID.Visible = true;
                lblMacIDtext.Text = "MAC ID/VM. <span class='red'>*</span> :";
                lblsearchby.Text = "Please Enter :-";
                lblserachmand.Visible = false;
                btnSearch.Text = "Pair";
                String Response = FillAddDeatil("P", Session["city"].ToString());

                DataTable Tblpincode = new DataTable();
                Tblpincode.Columns.Add("ID");
                Tblpincode.Columns.Add("Name");
                ViewState["State"] = Response.Split('*')[1].ToString();
                string[] Responsearr = Response.Split('*')[0].Split('$');

                foreach (string str in Responsearr)
                {
                    if (str != "")
                    {
                        Tblpincode.Rows.Add(str, str);
                    }
                }
                ddlpin.Items.Clear();
                if (Tblpincode.Rows.Count > 0)
                {
                    ListItem lst = new ListItem();
                    ddlpin.DataTextField = "ID";
                    ddlpin.DataValueField = "Name";

                    ddlpin.DataSource = Tblpincode;
                    ddlpin.DataBind();
                    ddlpin.Items.Insert(0, new ListItem("-- Select Zip Code --", ""));
                }
            }
            else
            {
                rdoSearchParamType.Visible = true;
                rdoSearchParamType.SelectedValue = "0";
                lblvctext.Text = "";
                lblstbtext.Text = "";
                lblOr.Text = "";
                lblMacIDtext.Text = "";
                txtsearchMACID.Text = "";
                txtsearchMACID.Visible = false;
                txtSearchParam.Text = "";
                txtStbnosearch.Text = "";
                lblsearchby.Text = "Search By ";
                btnSearch.Text = "Search";
                txtStbnosearch.Visible = false;
                lblserachmand.Visible = true;
            }
        }

        protected void processTransaction(String Action)
        {
            try
            {
                ViewState["ErrorMessage"] = null;
                //gathering data
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData = new Hashtable();
                string transaction_action = Action; //A-Add , C-Cancel, R-Renew
                string transaction_type = "RAD"; //grid from where function is called 

                string plan_poid = "";
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                string _tvType = "";
                string _foccount = "";
                string maintStatus = "INACTIVE";
                if (Session["username"] != null && Session["operator_id"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["operator_id"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }


                _vc_id = ViewState["Custvc"].ToString();
                if (ViewState["TV"].ToString() == "0")
                {
                    _tvType = "MAIN";
                }
                else
                {
                    _tvType = "CHILD";
                }
                if (ViewState["Status"].ToString() == "10100")
                {
                    maintStatus = "ACTIVE";
                }
                else
                {
                    maintStatus = "INACTIVE";
                }


                //processing 
                string Request = "";

                //if cancel and renew
                plan_poid = ViewState["planpoid"].ToString();


                if (transaction_action == "C")
                {
                    activation_date = ViewState["activation"].ToString();
                    expiry_date = ViewState["expiry"].ToString();
                    reason_id = "";
                    reason_text = "";
                }



                if (ViewState["ServicePoid"] != null && ViewState["accountPoid"] != null)
                {
                    ht.Add("username", Session["username"].ToString());
                    ht.Add("lcoid", Session["operator_id"].ToString());
                    ht.Add("custid", ViewState["CustNo"].ToString());
                    ht.Add("vcid", _vc_id);
                    ht.Add("custname", ViewState["CustName"].ToString());
                    ht.Add("cust_addr", ViewState["Custadd"].ToString());
                    ht.Add("planid", plan_poid);
                    ht.Add("flag", transaction_action);
                    ht.Add("expdate", expiry_date);
                    ht.Add("actidate", activation_date);
                    ht.Add("request", Request);
                    ht.Add("reason_id", reason_id);
                    ht.Add("IP", Ip);

                    ht.Add("MainTVStatus", maintStatus);
                    ht.Add("TVType", _tvType);
                    ht.Add("DeviceType", ViewState["SDHD"].ToString());


                    ht.Add("FOCCount", "0");
                    ht.Add("bucket1foc", "0");  ///
                    ht.Add("bucket2foc", "0");  ///
                    ht.Add("BasicPoid", ViewState["basic_poids"].ToString());


                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    string response = obj.ValidateProvTrans(ht);

                    string[] res = response.Split('$');
                    if (res[0] != "9999")
                    {
                        msgboxstr(res[1]);
                        return;
                    }
                    else
                    {
                        if (res[1] == "A")
                        {
                            transaction_action = "A";
                        }
                        else
                        {
                            request_id = res[1];
                        }
                    }
                }
                else
                {
                    ViewState["ErrorMessage"] = "something went wrong, Service or account details not found...Please relogin";
                    msgboxstr("something went wrong, Service or account details not found...Please relogin");
                    return;
                }
                //------------
                Cls_Business_TxnAssignPlan obj3 = new Cls_Business_TxnAssignPlan();
                string responseCount = obj3.ChannelCount(Session["username"].ToString(), plan_poid);
                string ChannelCount = responseCount.Split('~')[2];
                //--------------

                if (transaction_action == "A")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + ChannelCount;
                }
                else
                    if (transaction_action == "C")
                    {
                        Request = ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + ViewState["packageid"] + "$" + ViewState["dealpoid"];
                    }
                    else
                    {
                        ViewState["ErrorMessage"] = "Something went wrong while transaction.";
                        msgboxstr("Something went wrong while transaction.");
                        return;
                    }

                Request = _user_brmpoid + "$" + Request;
                string req_code = "";
                if (transaction_action == "A")
                {
                    req_code = "5";
                }
                else if (transaction_action == "R")
                {
                    req_code = "7";
                }
                else if (transaction_action == "C")
                {
                    req_code = "8";
                }
                string api_response = callAPI(Request, req_code);
                string[] final_obrm_status = api_response.Split('$');
                string obrm_status = final_obrm_status[0];
                string obrm_msg = "";

                try
                {
                    if (obrm_status == "0" || obrm_status == "1")
                    {
                        obrm_msg = final_obrm_status[2];
                    }
                    else
                    {
                        obrm_status = "1";
                        obrm_msg = api_response;
                    }
                }
                catch (Exception ex)
                {
                    obrm_status = "1";
                    ViewState["ErrorMessage"] = api_response;
                    obrm_msg = api_response;
                }

                ht.Add("obrmsts", obrm_status);
                ht.Add("request_id", request_id);
                ht.Add("response", api_response);

                Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
                string resp = obj1.ProvTransResEcaf(ht); // "9999";
                string[] finalres = resp.Split('$');
                if (finalres[0] == "9999")
                {
                    ViewState["ErrorMessage"] = "Transaction successful : " + obrm_msg;
                }
                else
                {
                    if (obrm_status == "0")
                    {
                        ViewState["ErrorMessage"] = "Transaction successful by OBRM but failure at UPASS : " + finalres[1];
                        msgboxstr("Transaction successful by OBRM but failure at UPASS : " + finalres[1]);
                    }
                    else
                    {
                        ViewState["ErrorMessage"] = "Transaction failed : " + finalres[1] + " : " + obrm_msg;
                        msgboxstr("Transaction failed : " + finalres[1] + " : " + obrm_msg);
                    }
                }

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                ViewState["ErrorMessage"] = ex.Message.ToString();
                objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-ProcessTransaction");
                return;
            }
        }

        public void Cancellation_ProcessSpecial(string Planpoid, string CancelTvType, string planname, string childvc_check)
        {

            int Error_break = 0;

            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();

            if (ViewState["cust_services"].ToString() != "")
            {
                string cust_services = ViewState["cust_services"].ToString();
                string[] service_arr = cust_services.Split('^');
                int k = 0;

                int a = service_arr.Length; //3

                string[] updated_service_arr = new string[a]; //3
                foreach (string SerivceIndexs in service_arr)
                {

                    string Tv_Type_check = SerivceIndexs.Split('!')[6];
                    if (Tv_Type_check == "0")
                    {
                        updated_service_arr[a - 1] = SerivceIndexs;
                    }
                    else
                    {
                        updated_service_arr[k] = SerivceIndexs;
                        k = k + 1;
                    }
                }

                /* foreach (string service in service_arr)
                 {*/

                foreach (string service in updated_service_arr)
                {
                    if (Error_break > 0)
                    {
                        break;
                    }
                    string service_poid = service.Split('!')[0];
                    string vc = service.Split('!')[2];
                    string stb_status = service.Split('!')[4];
                    string Device_Type = service.Split('!')[5];
                    string Tv_Type = service.Split('!')[6];

                    //if (planname.Contains("SPECIAL") == false)
                    //{


                    if (CancelTvType == "SPECIAL")
                    {

                        if (childvc_check != vc)
                        {
                            continue;
                        }
                    }

                    // }

                    string all_plan_string = service.Split('!')[3];
                    string service_data = "";
                    if (planname.Contains("SPECIAL") == true)
                    {

                        service_data = obj.getvcwise_spl_planstr(Session["username"].ToString(), all_plan_string);
                    }
                    string[] service_data_arr = service_data.Split('#');
                    if (service_data_arr[0] != "9999")
                    {
                        continue;
                    }
                    else if (service_data_arr[0] == "9999" && service_data_arr[1].Replace("~", "") != "")
                    {

                        string data_str = service_data_arr[1];

                        string[] data_arr = data_str.Split('~');
                        foreach (string plan_data in data_arr)
                        {
                            if (plan_data == "")
                            {
                                continue;
                            }
                            Tv_Type = service.Split('!')[6];
                            string[] plan_details_arr = plan_data.Split('$');

                            Hashtable htData = new Hashtable();
                            if (plan_details_arr[9].ToString() == "Active")
                            {
                                if (plan_details_arr[1].ToString().Contains("SPECIAL") == false)
                                {
                                    continue;
                                }

                                htData["planname"] = plan_details_arr[1].ToString();
                                htData["planpoid"] = plan_details_arr[0].ToString();
                                htData["dealpoid"] = plan_details_arr[6].ToString();
                                htData["custprice"] = plan_details_arr[2].ToString();
                                htData["lcoprice"] = plan_details_arr[3].ToString();
                                htData["activation"] = plan_details_arr[4].ToString();
                                htData["expiry"] = plan_details_arr[5].ToString();
                                htData["packageid"] = plan_details_arr[7].ToString();
                                htData["purchasepoid"] = plan_details_arr[8].ToString();
                                htData["IP"] = "";
                                htData["autorenew"] = "N";
                                string conResult = callGetProviConfirm_CancellationProcess(htData, "C");
                                string[] result_arr = conResult.Split('#');
                                if (result_arr[0].ToString() != "9999")
                                {


                                    if (CancelTvType == "MAIN")
                                    {
                                        if (Tv_Type == "1")
                                        {
                                            Error_break = Error_break + 1;
                                            break;
                                        }
                                    }
                                    continue;
                                }
                                else
                                {
                                    string[] conResult_arr = result_arr[1].Split('$');
                                    htData["refund_amt"] = conResult_arr[1];
                                    htData["days_left"] = conResult_arr[0];
                                    ViewState["transaction_data"] = htData;
                                    ViewState["hdnPopupAction"] = "C";
                                    if (Tv_Type.ToString() == "0")
                                    {
                                        Tv_Type = "MAIN";

                                    }
                                    else
                                    {
                                        Tv_Type = "CHILD";
                                    }
                                    processTransaction_cancellation(vc, Tv_Type, stb_status, service_poid);

                                }

                                if (CancelTvType == "MAIN")
                                {
                                    if (Tv_Type == "CHILD")
                                    {
                                        if (ViewState["ErrorMessage"].ToString().Contains("Transaction successful :") == false)
                                        {
                                            Error_break = Error_break + 1;
                                            break;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }

        }

        protected void processTransaction_cancellation(string Vcid, string Tv_Type, string Status, string Servicepoid)  //Added by Vivek Singh on 12-Jul-2016
        {
            try
            {
                ViewState["ErrorMessage"] = null;
                //gathering data
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData = new Hashtable();
                string transaction_action = "C";


                string plan_poid = "";
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                string _tvType = "";

                string maintStatus = "INACTIVE";
                if (Session["username"] != null && Session["operator_id"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["operator_id"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }

                _vc_id = Vcid;

                _tvType = Tv_Type;

                if (Status == "10100")
                {
                    maintStatus = "ACTIVE";
                }
                else
                {
                    maintStatus = "INACTIVE";
                }

                ViewState["AddedFOC"] = "0";

                if (ViewState["transaction_data"] != null)
                {
                    htPlanData = ViewState["transaction_data"] as Hashtable;
                }
                else
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction. Plan data not found.";

                    return;
                }

                //processing 
                string Request = "";


                plan_poid = htPlanData["planpoid"].ToString();
                activation_date = htPlanData["activation"].ToString();
                expiry_date = htPlanData["expiry"].ToString();
                if (transaction_action == "C")
                {
                    reason_id = "";
                    reason_text = "";
                }


                if (ViewState["ServicePoid"] != null && ViewState["accountPoid"] != null)
                {

                    ht.Add("username", _username);
                    ht.Add("lcoid", _oper_id);
                    ht.Add("custid", ViewState["CustNo"].ToString());
                    ht.Add("vcid", _vc_id);
                    ht.Add("custname", ViewState["CustName"].ToString());
                    ht.Add("cust_addr", ViewState["Custadd"].ToString());
                    ht.Add("planid", plan_poid);
                    ht.Add("flag", transaction_action);
                    ht.Add("expdate", expiry_date);
                    ht.Add("actidate", activation_date);
                    ht.Add("request", Request);
                    ht.Add("reason_id", reason_id);
                    ht.Add("IP", Ip);

                    ht.Add("MainTVStatus", maintStatus);
                    ht.Add("TVType", _tvType);
                    ht.Add("DeviceType", ViewState["SDHD"].ToString());
                    ht.Add("FOCCount", 0);
                    ht.Add("BasicPoid", ViewState["basic_poids"].ToString());


                    Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                    string response = obj.ValidateProvTrans(ht);

                    string[] res = response.Split('$');
                    if (res[0] != "9999")
                    {
                        ViewState["ErrorMessage"] = res[1].ToString();
                        return;
                    }
                    else
                    {
                        if (res[1] == "A")
                        {
                            transaction_action = "A";
                        }
                        else
                        {
                            request_id = res[1];
                        }
                    }
                }
                else
                {
                    ViewState["ErrorMessage"] = "something went wrong, Service or account details not found...Please relogin";

                    return;
                }

                if (transaction_action == "C")
                {
                    Request = ViewState["accountPoid"].ToString() + "$" + Servicepoid.ToString() + "$" + plan_poid + "$" + htPlanData["packageid"] + "$" + htPlanData["dealpoid"];
                }
                else
                {
                    ViewState["ErrorMessage"] = "Something went wrong while transaction.";
                    return;
                }

                Request = _user_brmpoid + "$" + Request;

                string api_response = callAPI(Request, "8");
                //string api_response = "0$ACCOUNT - Service add plan completed successfully$0.0.0.1 /account 81788441 9$0.0.0.1 /service/catv 81788185 39";
                string[] final_obrm_status = api_response.Split('$');
                string obrm_status = final_obrm_status[0];
                string obrm_msg = "";

                try
                {
                    if (obrm_status == "0" || obrm_status == "1")
                    {
                        obrm_msg = final_obrm_status[2];
                    }
                    else
                    {
                        obrm_status = "1";
                        obrm_msg = api_response;
                    }
                }
                catch (Exception ex)
                {
                    obrm_status = "1";
                    ViewState["ErrorMessage"] = api_response;
                    obrm_msg = api_response;
                }

                ht.Add("obrmsts", obrm_status);
                ht.Add("request_id", request_id);
                ht.Add("response", api_response);

                Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
                string resp = obj1.ProvTransResEcaf(ht); // "9999";
                string[] finalres = resp.Split('$');
                if (finalres[0] == "9999")
                {
                    ViewState["ErrorMessage"] = "Transaction successful : " + obrm_msg;
                    ViewState["renewfoc"] = "Y";
                }
                else
                {
                    if (obrm_status == "0")
                    {
                        ViewState["ErrorMessage"] = "Transaction successful by OBRM but failure at UPASS : " + finalres[1];
                        ViewState["renewfoc"] = "Y";
                    }
                    else
                    {
                        ViewState["ErrorMessage"] = "Transaction failed : " + finalres[1] + " : " + obrm_msg;
                        ViewState["renewfoc"] = "N";
                    }
                }

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                ViewState["ErrorMessage"] = ex.Message.ToString();
                objSecurity.InsertIntoDb(Session["username"].ToString(), ex.Message.ToString(), "frmAssignPlan.cs-processTransaction_cancellation");
                return;
            }
        }

        protected string callGetProviConfirm_CancellationProcess(Hashtable htData, string flag) //Added by Vivek Singh on 12-Jul-2016
        {
            Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
            Hashtable ht = new Hashtable();
            ht["username"] = username;
            ht["lco_id"] = Session["operator_id"].ToString();
            ht["cust_no"] = ViewState["CustNo"].ToString();
            ht["vc_id"] = ViewState["Custvc"].ToString();
            ht["cust_name"] = ViewState["CustName"].ToString();
            ht["plan_id"] = htData["planpoid"];
            ht["flag"] = flag;
            ht["IP"] = htData["IP"];
            if (htData["expiry"] != null)
            {
                ht["expiry"] = htData["expiry"];
            }
            else
            {
                ht["expiry"] = "";
            }
            if (htData["activation"] != null)
            {
                ht["activation"] = htData["activation"];
            }
            else
            {
                ht["activation"] = ""; // add plan
            }

            ht["existing_poids"] = htData["existing_poids"];

            string result = obj.getProviConfirm(ht);


            return result.ToString();

        }

        public void FunUpdateStatus(string action, string errorcode, string StrPriErroResponse)
        {
            string strConn = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection conn = new OracleConnection(strConn);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                OracleCommand Cmd = new OracleCommand("aoup_lcopre_mob_pln_updt_sts", conn);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar);
                Cmd.Parameters["in_username"].Value = Session["username"].ToString();

                Cmd.Parameters.Add("in_account", OracleType.VarChar);
                Cmd.Parameters["in_account"].Value = ViewState["accountno"];

                Cmd.Parameters.Add("in_custname", OracleType.VarChar);
                Cmd.Parameters["in_custname"].Value = ViewState["customer_name"];

                Cmd.Parameters.Add("in_custAddress", OracleType.VarChar);
                Cmd.Parameters["in_custAddress"].Value = ViewState["cust_addr"];

                Cmd.Parameters.Add("in_accountpoid", OracleType.VarChar);
                Cmd.Parameters["in_accountpoid"].Value = ViewState["accountPoid"];

                Cmd.Parameters.Add("in_obrmid", OracleType.VarChar);
                Cmd.Parameters["in_obrmid"].Value = ViewState["user_brmpoid"];


                Cmd.Parameters.Add("in_transmod", OracleType.VarChar);
                Cmd.Parameters["in_transmod"].Value = action;

                Cmd.Parameters.Add("in_errorcode", OracleType.VarChar);
                Cmd.Parameters["in_errorcode"].Value = errorcode;

                Cmd.Parameters.Add("in_errormsg", OracleType.VarChar);
                Cmd.Parameters["in_errormsg"].Value = StrPriErroResponse;



                Cmd.Parameters.Add("OUT_DATA", OracleType.VarChar, 500);
                Cmd.Parameters["OUT_DATA"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.ExecuteNonQuery();
                if (conn.State != ConnectionState.Closed)
                    conn.Close();

            }
            catch (Exception ex)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                // FileLog(ex.Message.ToString());
            }

        }

        public static bool emailIsValid(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected void btnpairchildconfirm_Click(object sender, EventArgs e)
        {
            if (txtMacId.Text.Trim() != "")//txtMacId
            {
                lblchildfinalconfirm.Text = "You want to activate MAC ID[" + txtMacId.Text.Trim() + "]";

            }
            else
            {
                lblchildfinalconfirm.Text = "You want to activate VC[" + txtpairVC.Text.Trim() + "] with STB[" + txtpairstb.Text.Trim() + "]";
            }
            popchildfinalconfirm.Show();

        }

        protected void btnPairChiltTV_Click(object sender, EventArgs e)
        {
            if (txtpairstb.Text.Trim() == "" && txtpairVC.Text.Trim() == "" && txtMacId.Text.Trim() == "")
            {
                msgboxstr("Please Enter STB ID/VC ID OR MAC ID ");
                return;
            }
            if (txtpairstb.Text.Trim() != "")
            {
                if (txtpairVC.Text.Trim() == "")
                {
                    msgboxstr("VC ID can not be blank");
                    return;
                }
                if (txtMacId.Text.Trim() != "")
                {
                    msgboxstr("Please enter either STB ID and VC ID OR MAC ID");
                    return;
                }
                lblSTBID.Visible = true;
                lblVCID.Visible = true;
            }
            if (txtpairVC.Text.Trim() != "")
            {
                if (txtpairstb.Text.Trim() == "")
                {
                    msgboxstr("STB ID can not be blank");
                    return;
                }
                if (txtMacId.Text.Trim() != "")
                {
                    msgboxstr("Please enter either STB ID and VC ID OR MAC ID");
                    return;
                }
                lblSTBID.Visible = true;
                lblVCID.Visible = true;
            }
            //if (txtpairstb.Text.Trim() == "")   ---- COMMEND BY RP ON 11/07/2017
            //{
            //    msgboxstr("STB ID can not be blank");
            //    return;
            //}
            //if (txtpairVC.Text.Trim() == "")
            //{
            //    msgboxstr("VC ID can not be blank");
            //    return;
            //}
            if (txtMacId.Text.Trim() != "")
            {
                lblSTBID.Visible = false;
                lblVCID.Visible = false;
                lblMACID.Visible = true;
            }
            else
            {
                lblSTBID.Visible = true;
                lblVCID.Visible = true;
                lblMACID.Visible = false;
            }
            lblstbchild.Text = txtpairstb.Text.Trim();
            lblvcidchild.Text = txtpairVC.Text.Trim();
            lblmacidchild.Text = txtMacId.Text.Trim();
            lblaccnochildconfim.Text = lblSAccNo.Text.Trim();
            lblplannamechildconfim.Text = lblSPlanName.Text.Trim();
            lblmrpchildconfim.Text = lblsMrp.Text.Trim();
            popchildtvconfirm.Show();
        }


        protected void btnbtnchildfinalconfirm_Click(object sender, EventArgs e)
        {
            try
            {
                PopuPSearchDetails.Show();
                lblchildtverr.Text = "";
                string strMacID = "";
                if (txtpairVC.Text.Trim() != "")
                {
                    strMacID = txtpairVC.Text.Trim();

                }
                else if (txtMacId.Text.Trim() != "")
                {
                    strMacID = txtMacId.Text.Trim();
                }

                string strVCNO = "", strSTBNO = "", strMACID = "";
                if (txtpairVC.Text.Trim() != "")
                {
                    strVCNO = txtpairVC.Text.Trim();
                    strSTBNO = txtpairstb.Text.Trim();

                }
                else
                {
                    strVCNO = txtMacId.Text.Trim();
                    strSTBNO = txtMacId.Text.Trim();
                }
                // String response_params = user_brmpoid + "$" + txtpairVC.Text.Trim() + "$SW$V";
                String response_params = user_brmpoid + "$" + strMacID + "$SW$V";
                //string apiResponse = callAPI(response_params, "25"); comment by RP no 11/07/2017
                string apiResponse = callAPI(response_params, "12");
                List<string> lstResponse = new List<string>();
                lstResponse = apiResponse.Split('$').ToList();
                string cust_id = lstResponse[0];

                if (cust_id != "*")
                {
                    msgboxstr("VC No./MAC No [" + strMacID + "] already pair with another Account No.");
                    PopuPSearchDetails.Hide();
                    return;
                }

                ViewState["ErrorMessage"] = null;
                //gathering data
                Cls_Data_Auth auth = new Cls_Data_Auth();
                string Ip = auth.GetIPAddress(HttpContext.Current.Request);
                Hashtable ht = new Hashtable();
                Hashtable htPlanData = new Hashtable();
                string transaction_action = "A"; //A-Add , C-Cancel, R-Renew
                string transaction_type = "B"; //grid from where function is called 

                string plan_poid = "";
                string activation_date = "";
                string expiry_date = "";
                string _username = "";
                string _user_brmpoid = "";
                string _oper_id = "";
                string _vc_id = "";
                string request_id = "";
                string reason_id = "";
                string reason_text = "";
                string _tvType = "";
                string _foccount = "";
                string maintStatus = "INACTIVE";
                if (Session["username"] != null && Session["operator_id"] != null && Session["user_brmpoid"] != null)
                {
                    _username = Session["username"].ToString();
                    _oper_id = Session["operator_id"].ToString();
                    _user_brmpoid = Session["user_brmpoid"].ToString();
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");
                }


                _vc_id = txtpairVC.Text.Trim();

                _tvType = "CHILD";
                maintStatus = "ACTIVE";
                ViewState["AddedFOC"] = "0";


                //processing 
                string Request = "";
                //validating...
                if (transaction_action == "A")
                {
                    plan_poid = ViewState["Chiltv_Planpoid"].ToString();
                    activation_date = "";
                    expiry_date = "";
                }


                ht.Add("username", Session["username"].ToString());
                ht.Add("lcoid", Session["operator_id"].ToString());
                ht.Add("custid", ViewState["CustNo"].ToString());
                ht.Add("vcid", strMacID);
                ht.Add("custname", txtfname.Text.Trim() + " " + txtmname.Text.Trim() + " " + txtlname.Text.Trim());
                ht.Add("cust_addr", txtadd.Text.Trim());
                ht.Add("planid", plan_poid);
                ht.Add("flag", transaction_action);
                ht.Add("expdate", expiry_date);
                ht.Add("actidate", activation_date);
                ht.Add("request", Request);
                ht.Add("reason_id", reason_id);
                ht.Add("IP", Ip);

                ht.Add("MainTVStatus", maintStatus);
                ht.Add("TVType", _tvType);
                ht.Add("DeviceType", ViewState["SDHD"].ToString());

                ht.Add("FOCCount", ViewState["AddedFOC"].ToString());

                ht.Add("BasicPoid", "");
                ht.Add("STBNO", strSTBNO);

                Cls_Business_TxnAssignPlan obj = new Cls_Business_TxnAssignPlan();
                string response = obj.ValidateProvTrans(ht);

                string[] res = response.Split('$');
                if (res[0] != "9999")
                {
                    lblchildtverr.Text = res[1].ToString();

                    return;
                }
                else
                {
                    if (res[1] == "A")
                    {
                        transaction_action = "A";
                    }
                    else
                    {
                        request_id = res[1];
                    }
                }

                plan_poid = ViewState["Chiltv_Planpoid"].ToString();
                long Datelong = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
                if (txtMacId.Text != "")
                {
                    response_params = user_brmpoid + "$" + ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + ViewState["Childtv_PlanName"].ToString() + "$" + txtMacId.Text.Trim() + "$" + txtMacId.Text.Trim() + "$" + Datelong;
                }
                else
                {
                    response_params = user_brmpoid + "$" + ViewState["accountPoid"].ToString() + "$" + ViewState["ServicePoid"].ToString() + "$" + plan_poid + "$" + ViewState["Childtv_PlanName"].ToString() + "$" + txtpairstb.Text.Trim() + "$" + txtpairVC.Text.Trim() + "$" + Datelong;
                }
                apiResponse = callAPI(response_params, "23");
                string[] final_obrm_status = apiResponse.Split('$');
                String obrm_status = final_obrm_status[0];

                if (obrm_status == "0")
                {

                    int childtvcount = Convert.ToInt32(ViewState["Childtvcount"]);
                    childtvcount = childtvcount + 1;
                    ViewState["Childtvcount"] = childtvcount;
                    ht.Add("obrmsts", obrm_status);
                    ht.Add("request_id", request_id);
                    ht.Add("response", apiResponse);

                    Cls_Business_TxnAssignPlan obj1 = new Cls_Business_TxnAssignPlan();
                    string resp = obj1.ProvTransResEcaf(ht); // "9999";
                    string[] finalres = resp.Split('$');
                    if (finalres[0] == "9999")
                    {
                        Hashtable htcust = new Hashtable();

                        htcust["custtype"] = "";
                        htcust["appicanttit"] = "Mr";
                        htcust["fname"] = txtfname.Text.Trim();
                        htcust["lname"] = txtlname.Text.Trim();
                        htcust["mob"] = txtmobno.Text.Trim();
                        htcust["landline"] = txtlandline.Text.Trim();
                        htcust["email"] = txtemailid.Text.Trim();
                        htcust["pin"] = ddlpin.SelectedValue;
                        htcust["street"] = ddlstreet.SelectedValue;
                        htcust["city"] = txtcity.Text.Trim();
                        htcust["location"] = ddllocation.SelectedValue;
                        htcust["area"] = ddlarea.SelectedItem.Text;
                        htcust["building"] = ddlbuilding.SelectedValue;
                        htcust["flatno"] = txtflatno.Text.Trim();
                        if (txtpairstb.Text.Trim() != "")
                        {
                            htcust["stbno"] = txtpairstb.Text.Trim();
                        }
                        else
                        {
                            htcust["stbno"] = strMacID;

                        }
                        htcust["vcno"] = strMacID;// txtpairVC.Text.Trim();
                        htcust["accno"] = Convert.ToString(ViewState["CustNo"]);
                        htcust["add"] = txtadd.Text.Trim();
                        if (ViewState["idproofimage"] != null)
                        {
                            htcust["idproofimage"] = ViewState["idproofimage"];
                        }
                        else
                        {
                            htcust["idproofimage"] = "";
                        }

                        htcust["idvalue"] = ddlID.SelectedValue;
                        htcust["macid"] = ViewState["Custvc"].ToString();
                        if (ViewState["photoimage"] != null)
                        {
                            htcust["photoimage"] = ViewState["photoimage"].ToString();
                        }
                        else
                        {
                            htcust["photoimage"] = "";
                        }
                        htcust["resivalue"] = ddlResi.SelectedValue;

                        if (ViewState["resiproof"] != null)
                        {
                            htcust["resiproof"] = ViewState["resiproof"].ToString();
                        }
                        else
                        {
                            htcust["resiproof"] = "";
                        }

                        if (ViewState["Pdfdocimage"] != null)
                        {
                            htcust["signatureimage"] = ViewState["Pdfdocimage"].ToString();
                        }
                        else
                        {
                            htcust["signatureimage"] = "";
                        }
                        htcust["state"] = ViewState["State"].ToString();
                        htcust["Child"] = "Y";
                        htcust["idproofvalue"] = txtidproof.Text.Trim();
                        htcust["resiproofvalue"] = txtresoproof.Text.Trim();
                        htcust["cafno"] = Convert.ToString(ViewState["cafno"]);
                        htcust["ip"] = Ip;
                        Cls_BLL_mstCrf objcust = new Cls_BLL_mstCrf();
                        String rescust = objcust.insrtCrfdata(Session["username"].ToString(), htcust);
                        msgboxstr("Child TV created Successfully");
                        txtpairVC.Text = "";
                        txtpairstb.Text = "";
                        PopuPSearchDetails.Hide();
                        GetUserDteail();
                        return;
                    }
                    else
                    {
                        msgboxstr("Transaction Success from OBRM but Failure at UPASS");
                        txtpairVC.Text = "";
                        txtpairstb.Text = "";
                        PopuPSearchDetails.Hide();
                        return;
                    }
                }
                else
                {
                    lblchildtverr.Text = "Transaction failed : OBRM " + final_obrm_status[1] + " : " + final_obrm_status[2];
                    return;
                }
            }
            catch (Exception ex)
            {
                lblchildtverr.Text = "Error while Creating Child TV [" + ex.Message + "]";
            }
            GetUserDteail();

        }

        protected void btnAddChildTV_Click(object sender, EventArgs e)
        {

            int childtvcount = Convert.ToInt32(ViewState["Childtvcount"]);

            if (childtvcount > 2)
            {
                msgboxstr("You reached max Connection Limit");
                PopuPSearchDetails.Hide();
                return;
            }

            ViewState["PlanName_Price"] = null;
            ViewState["Chiltv_Planpoid"] = null;
            txtpairVC.Text = "";
            txtpairstb.Text = "";
            if (ViewState["basic_poids"] != null)
            {
                GetBaseplan(ViewState["basic_poids"].ToString().TrimEnd(',').Replace("'", "").ToString());
            }

            if (ViewState["basic_poids"] == "")
            {
                msgboxstr("Base Package in Main Connection is Inactive");
                return;
            }

            //Session["city"],  Session["DASAREA"] ,  ViewState["Old_PlanName"]
            if (ViewState["Old_PlanName"] == null)
            {
                msgboxstr("Base Package in Main Connection is Inactive");
                return;
            }
            lblSAccNo.Text = ViewState["CustNo"].ToString();
            string[] neWplan = ViewState["Old_PlanName"].ToString().Split(' ');
            string first = "";
            string finalString = "";
            for (int i = 0; i < neWplan.Length; i++)
            {
                if (i == 1)
                {
                    first = "ADDITIONAL" + " " + neWplan[i].ToString();

                }
                else
                {
                    first = neWplan[i].ToString();
                }

                finalString += first + " ";
            }

            lblSPlanName.Text = finalString.ToString().Trim();

            GetNewplanPrice(lblSPlanName.Text, Session["cityID"].ToString(), Session["DASAREA"].ToString());

            if (ViewState["PlanName_Price"] != null)
            {
                lblsMrp.Text = ViewState["PlanName_Price"].ToString();
            }
            else
            {
                msgboxstr("Additional Plan Not Found.");
                return;
            }
            ViewState["Childtv_PlanName"] = finalString.ToString().Trim();
            txtMacId.Text = "";
            txtpairstb.Text = "";
            txtpairVC.Text = "";
            lblchildtverr.Text = "";
            PopuPSearchDetails.Show();
        }

        public void GetNewplanPrice(string Plan, string city, string dasarea)
        {
            string strConn = ConfigurationSettings.AppSettings["conString"].ToString().Trim();
            OracleConnection conn = new OracleConnection(strConn);


            string _getMrp = "select  num_plan_custprice , var_plan_planpoid from aoup_lcopre_plan_def where var_plan_plantype='B' and var_plan_name='" + Plan + "' ";
            _getMrp += " and num_plan_cityid='" + city + "'   and var_plan_dasarea='" + dasarea + "' and rownum=1 ";
            _getMrp += "Union select  num_plan_custprice , var_plan_planpoid from aoup_lcopre_plan_def where var_plan_plantype='B' and var_plan_name='" + Plan + "' ";
            _getMrp += " and num_plan_cityid='" + city + "'   and var_plan_dasarea='" + dasarea + "' and rownum=1";

            conn.Open();
            OracleCommand cmd2 = new OracleCommand(_getMrp, conn);
            OracleDataReader drPrice = cmd2.ExecuteReader();

            while (drPrice.Read())
            {
                ViewState["PlanName_Price"] = drPrice["num_plan_custprice"].ToString();
                ViewState["Chiltv_Planpoid"] = drPrice["var_plan_planpoid"].ToString();
            }

            drPrice.Close();
            conn.Close();
        }

        public String callAPI(string Request, string request_code)
        {
            try
            {
                string fromSender = string.Empty;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(Request);
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRM/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request); myRequest.Method = "POST";
                // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://localhost/TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://124.153.73.21//TestHwayOBRMUAT/Default.aspx?CompCode=OBRM&ReqCode=" + request_code + "&Request=" + Request);
                //myRequest.Method = "POST";
                //myRequest.ContentType = "application/x-www-form-urlencoded";
                //myRequest.ContentLength = data.Length;
                //myRequest.Timeout = 90000;
                //Stream newStream = myRequest.GetRequestStream();
                //newStream.Write(data, 0, data.Length);
                //using (HttpWebResponse responseFromSender = (HttpWebResponse)myRequest.GetResponse())
                //{
                //    using (StreamReader responseReader = new StreamReader(responseFromSender.GetResponseStream()))
                //    {
                //        fromSender = responseReader.ReadToEnd();
                //    }
                //}
                //String Res = fromSender.Split('%')[0];
                //return Res;
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;
                myRequest.Timeout = 90000;
                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                using (HttpWebResponse responseFromSender = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (StreamReader responseReader = new StreamReader(responseFromSender.GetResponseStream()))
                    {
                        fromSender = responseReader.ReadToEnd();
                    }
                }
                String Res = fromSender.Split('%')[0];
                return Res;
            }
            catch (Exception ex)
            {
                return "1$---$" + ex.Message.Trim();
            }
        }


    }
}