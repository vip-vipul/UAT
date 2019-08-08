using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassBLL.Transaction;
using System.Collections;
using PrjUpassPl.Helper;
using System.IO;
namespace PrjUpassPl.Transaction
{
    public partial class HwayTransLCOSTBInventoryConformation : System.Web.UI.Page
    {
        public void MessageAlert(String Message, String WindowsLocation)
        {
            String str = "";

            str = "alert('|| " + Message + " ||');";

            if (WindowsLocation != "")
            {
                str += "window.location = '" + WindowsLocation + "';";
            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, str, true);
            return;
        }

        Cls_Business_STBInventory_Conformation obj = new Cls_Business_STBInventory_Conformation();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.PageHeading = "LCO STB Inventory Conformation";

            if (!IsPostBack)
            {
                Session["trnsubtype"] = null;
                Session["transtype"] = null;
                Session["Receiptno"] = null;
                Session["RightsKey"] = "N";

                DataTable Dtreceiptdata = obj.STBInvConfList(Session["username"].ToString());

                if (Dtreceiptdata.Rows.Count > 0)
                {
                    grdCashcollect.DataSource = Dtreceiptdata;
                    grdCashcollect.DataBind();
                }
            }
        }

        protected void lnkReceiptno_click(object sender, EventArgs e)
        {
            int rowindex = Convert.ToInt32((((GridViewRow)(((LinkButton)(sender)).Parent.BindingContainer))).RowIndex);
            string trnsubtype = ((HiddenField)grdCashcollect.Rows[rowindex].FindControl("hdnsubtype")).Value;

            string transtype = ((HiddenField)grdCashcollect.Rows[rowindex].FindControl("hdntype")).Value;
            string receiptno = ((HiddenField)grdCashcollect.Rows[rowindex].FindControl("hdnreceiptno")).Value;
            string hdnmodel = ((HiddenField)grdCashcollect.Rows[rowindex].FindControl("hdnmodel")).Value;

            Session["trnsubtype"] = trnsubtype;
            Session["transtype"] = transtype;
            Session["Receiptno"] = receiptno;

            DataTable dt = obj.STBInvConfDet(Session["username"].ToString(), receiptno, hdnmodel);
            ViewState["transdetail"] = dt;
            Session["pistrans_id"] = dt.Rows[0]["pistrans_id"].ToString();

           
            grdSTBDetails.DataSource = dt;
            grdSTBDetails.DataBind();

            lblReceiptNo.Text = receiptno;

            for (int i = 0; i < grdSTBDetails.Rows.Count; i++)
            {
                RadioButton RdoGood = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoGood");
                RadioButton RdoFaulty = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoFaulty");
                RadioButton RdoUndelivered = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoUndelivered");

                RdoGood.Checked = true;

                RdoGood.GroupName = "Group" + i.ToString();
                RdoFaulty.GroupName = "Group" + i.ToString();
                RdoUndelivered.GroupName = "Group" + i.ToString();
            }

            CheckBox ChkGoodSelectAll = (CheckBox)grdSTBDetails.HeaderRow.FindControl("ChkGoodSelectAll");

            ChkGoodSelectAll.Checked = true;

            div2.Visible = true;
            div1.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            popMsg.Show();
        }

        protected void btnconfirm_click(object sender, EventArgs e)
        {
            if (txtRemark.Text == "")
            {
                MessageAlert("Remark can not blank", "");
                return;
            }

            if (grdSTBDetails.Rows.Count == 0)
            {
                MessageAlert("STB Details not found", "");
                return;
            }


            DataTable transdetail = (DataTable)ViewState["transdetail"];

            string Receipttype = "";
            if (transdetail.Rows[0]["receiptno"].ToString().ToUpper().Contains("SPSR"))
            {
                Receipttype = "SPSR";
            }

            else if (transdetail.Rows[0]["receiptno"].ToString().ToUpper().Contains("SPSN"))
            {
                Receipttype = "SPSN";
            }
            else if (transdetail.Rows[0]["receiptno"].ToString().ToUpper().Contains("PPSN"))
            {
                Receipttype = "PPSN";
            }
            else
            {
                Receipttype = "PPSR";
            }

            DataTable Tbllist = new DataTable();
            Tbllist.Columns.Add("transid");
            Tbllist.Columns.Add("orderno");
            Tbllist.Columns.Add("lcocode");
            Tbllist.Columns.Add("deviceid");
            Tbllist.Columns.Add("boxtype");
            Tbllist.Columns.Add("type");
            Tbllist.Columns.Add("model");
            Tbllist.Columns.Add("manufacturer");
            Tbllist.Columns.Add("company");
            Tbllist.Columns.Add("city");
            Tbllist.Columns.Add("state");
            Tbllist.Columns.Add("scheameid");
            Tbllist.Columns.Add("insby");
            Tbllist.Columns.Add("Flag");
            Tbllist.Columns.Add("STBID");

            String str = "";

            for (int i = 0; i < grdSTBDetails.Rows.Count; i++)
            {
                RadioButton RdoGood = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoGood");
                RadioButton RdoFaulty = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoFaulty");
                RadioButton RdoUndelivered = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoUndelivered");

                String STBStatus = "";

                if (RdoGood.Checked == true)
                {
                    STBStatus = "G";
                }

                else if (RdoFaulty.Checked == true)
                {
                    //  STBStatus = "F";
                    STBStatus = "R";
                }

                else if (RdoUndelivered.Checked == true)
                {
                    STBStatus = "U";
                }

                if (STBStatus == "G")
                {
                    if (grdSTBDetails.Rows[i].Cells[2].Text == "STB")
                    {
                        Tbllist.Rows.Add(transdetail.Rows[0]["transid"], transdetail.Rows[0]["receiptno"], Session["username"].ToString(), grdSTBDetails.Rows[i].Cells[1].Text, transdetail.Rows[0]["var_pisnewstb_boxtype"],
                               "STB", transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["company"], transdetail.Rows[0]["city"],
                                transdetail.Rows[0]["state"], transdetail.Rows[0]["num_pisnewstb_schemeid"], Session["username"].ToString(), "G", grdSTBDetails.Rows[i].Cells[5].Text);
                    }
                    else if (grdSTBDetails.Rows[i].Cells[2].Text == "VC")
                    {
                        Tbllist.Rows.Add(transdetail.Rows[0]["transid"], transdetail.Rows[0]["receiptno"], Session["username"].ToString(), grdSTBDetails.Rows[i].Cells[1].Text, transdetail.Rows[0]["var_pisnewstb_boxtype"],
                             "VC", transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["company"], transdetail.Rows[0]["city"],
                              transdetail.Rows[0]["state"], transdetail.Rows[0]["num_pisnewstb_schemeid"], Session["username"].ToString(), "G", grdSTBDetails.Rows[i].Cells[5].Text);
                    }

                }
                else if (STBStatus == "R")
                {
                    if (grdSTBDetails.Rows[i].Cells[2].Text == "STB")
                    {
                        Tbllist.Rows.Add(transdetail.Rows[0]["transid"], transdetail.Rows[0]["receiptno"], grdSTBDetails.Rows[i].Cells[6].Text, grdSTBDetails.Rows[i].Cells[1].Text, transdetail.Rows[0]["var_pisnewstb_boxtype"],
                                       "STB", transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["company"], transdetail.Rows[0]["city"],
                                        transdetail.Rows[0]["state"], transdetail.Rows[0]["num_pisnewstb_schemeid"], Session["username"].ToString(), "F", grdSTBDetails.Rows[i].Cells[5].Text);
                    }
                    else if (grdSTBDetails.Rows[i].Cells[2].Text == "VC")
                    {
                        Tbllist.Rows.Add(transdetail.Rows[0]["transid"], transdetail.Rows[0]["receiptno"], grdSTBDetails.Rows[i].Cells[6].Text, grdSTBDetails.Rows[i].Cells[1].Text, transdetail.Rows[0]["var_pisnewstb_boxtype"],
                             "VC", transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["company"], transdetail.Rows[0]["city"],
                              transdetail.Rows[0]["state"], transdetail.Rows[0]["num_pisnewstb_schemeid"], Session["username"].ToString(), "F", grdSTBDetails.Rows[i].Cells[5].Text);
                    }
                }
                else if (STBStatus == "U")
                {

                    if (grdSTBDetails.Rows[i].Cells[2].Text == "STB")
                    {
                        Tbllist.Rows.Add(transdetail.Rows[0]["transid"], transdetail.Rows[0]["receiptno"], grdSTBDetails.Rows[i].Cells[6].Text, grdSTBDetails.Rows[i].Cells[1].Text, transdetail.Rows[0]["var_pisnewstb_boxtype"],
                                       "STB", transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["company"], transdetail.Rows[0]["city"],
                                        transdetail.Rows[0]["state"], transdetail.Rows[0]["num_pisnewstb_schemeid"], Session["username"].ToString(), "U", grdSTBDetails.Rows[i].Cells[5].Text);
                    }
                    else if (grdSTBDetails.Rows[i].Cells[2].Text == "VC")
                    {
                        Tbllist.Rows.Add(transdetail.Rows[0]["transid"], transdetail.Rows[0]["receiptno"], grdSTBDetails.Rows[i].Cells[6].Text, grdSTBDetails.Rows[i].Cells[1].Text, transdetail.Rows[0]["var_pisnewstb_boxtype"],
                             "VC", transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["company"], transdetail.Rows[0]["city"],
                              transdetail.Rows[0]["state"], transdetail.Rows[0]["num_pisnewstb_schemeid"], Session["username"].ToString(), "U", grdSTBDetails.Rows[i].Cells[5].Text);
                    }
                }
            }
            if (str != "")
            {
                str = str.Substring(0, str.Length - 1);
            }
            String IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (IPAddress == null)
            {
                IPAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            string uploadResult = "";
            DateTime date = DateTime.Now;
            string cur_timestamp = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");

            string cur_time = DateTime.Now.ToString("dd-MMM-yyyy_hh:mm:ss");

            Random random = new Random();
            string upload_id = Session["username"].ToString() + "_" + cur_time + "_" + random.Next(1000, 9999);
            if (Tbllist.Rows.Count > 0)
            {
                string directoryPath = string.Format(Server.MapPath("MyExcelFile/" + Session["username"].ToString()));

                try
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                }
                catch (Exception ex)
                {
                    // btnBeginTrans.Visible = false;
                    return;
                }
                string file_name = "Warehousedata" + Session["username"].ToString() + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".txt";
                string filepath = Server.MapPath("MyExcelFile/" + Session["username"].ToString()) + "\\" + file_name;
                ExportDataTabletoFile(Tbllist, "", true, filepath);

                

                Cls_Presentation_Helper helper = new Cls_Presentation_Helper();

                String DataSource = (String)helper.GetDatabaseDateSource();
                String UserId = (String)helper.GetDatabaseUserId();
                String Password = (String)helper.GetDatabasePassword();

                string table_columns = "(num_inventval_transid,var_inventval_upload_id  constant \"" + upload_id + "\",var_inventval_orderno,var_inventval_lcocode,var_inventval_deviceid,var_inventval_boxtype,var_inventval_type,var_inventval_model,var_inventval_manufacturer,var_inventval_company,var_inventval_city,var_inventval_state,num_inventval_scheameid,var_inventval_insby,VAR_INVENTVAL_STATUS,NUM_INVENTVAL_STBID,dat_inventval_insdate constant \"" + cur_timestamp + "\",var_inventval_apistatus,var_inventval_apismsg,dat_inventval_valdate,VAR_INVENTVAL_VALTYPE constant \"N\",VAR_INVENTVAL_TRANSTYPE constant \"" + Receipttype + "\",VAR_INVENTVAL_IP constant \"" + IPAddress + "\",VAR_INVENTVAL_REMARK constant \"" + txtRemark.Text.Trim() + "\")";

                 uploadResult = helper.cDataUpload(Server.MapPath("MyExcelFile/" + Session["username"].ToString()) + "\\" + file_name,
                                                       Server.MapPath("MyExcelFile/" + Session["username"].ToString()) + "\\HathwayCTL.txt",
                                                       Server.MapPath("MyExcelFile/" + Session["username"].ToString()) + "\\HathwayLOG.log",
                                                       table_columns,DataSource, UserId, Password, "aoup_lcopre_pis_invent_raw",
                                                      upload_id);

            }
            else
            {
                uploadResult = "0";
            }
            if (uploadResult == "0")
            {
                string response = "";
                if (Tbllist.Rows.Count > 0)
                {
                    response = obj.UpdateInvIntStatus(Session["username"].ToString(), "L", transdetail.Rows[0]["transid"].ToString(), upload_id);
                }
                else
                {
                    response = "9999";
                }
                if (response == "9999")
                {
                    Response.Redirect("FrmWarehouseBulkProcess.aspx?uniqueid=" + upload_id);

                }
                else
                {
                    MessageAlert("Error While Updating","");
                    return;
                }
            }
            else
            {
                if (uploadResult == "")
                {
                    MessageAlert("Error While Uploading...","");
                    return;
                }
                else
                {
                    MessageAlert(uploadResult.ToString(),"");
                    return;
                }
            }

            
        }

        public void ExportDataTabletoFile(DataTable datatable, string delimited, bool exportcolumnsheader, string file)
        {

            StreamWriter str = new StreamWriter(file, false, System.Text.Encoding.Default);


            foreach (DataRow datarow in datatable.Rows)
            {

                string row = string.Empty;



                foreach (object items in datarow.ItemArray)
                {



                    row += items.ToString() + "\t";

                }

                str.WriteLine(row.Remove(row.Length - 1, 1));



            }

            str.Flush();

            str.Close();



        }

        protected void OnCheckedChanged_ChkGoodSelectAll(object sender, EventArgs e)
        {
            CheckBox ChkGoodSelectAll = (CheckBox)grdSTBDetails.HeaderRow.FindControl("ChkGoodSelectAll");
            CheckBox ChkFaultySelectAll = (CheckBox)grdSTBDetails.HeaderRow.FindControl("ChkFaultySelectAll");
            CheckBox ChkUndeliveredSelectAll = (CheckBox)grdSTBDetails.HeaderRow.FindControl("ChkUndeliveredSelectAll");

            ChkFaultySelectAll.Checked = false;
            ChkUndeliveredSelectAll.Checked = false;

            if (ChkGoodSelectAll.Checked == true)
            {
                for (int i = 0; i < grdSTBDetails.Rows.Count; i++)
                {
                    RadioButton RdoGood = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoGood");
                    RadioButton RdoFaulty = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoFaulty");
                    RadioButton RdoUndelivered = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoUndelivered");

                    RdoGood.Checked = true;
                    RdoFaulty.Checked = false;
                    RdoUndelivered.Checked = false;
                }
            }
        }

        protected void OnCheckedChanged_ChkFaultySelectAll(object sender, EventArgs e)
        {
            CheckBox ChkGoodSelectAll = (CheckBox)grdSTBDetails.HeaderRow.FindControl("ChkGoodSelectAll");
            CheckBox ChkFaultySelectAll = (CheckBox)grdSTBDetails.HeaderRow.FindControl("ChkFaultySelectAll");
            CheckBox ChkUndeliveredSelectAll = (CheckBox)grdSTBDetails.HeaderRow.FindControl("ChkUndeliveredSelectAll");

            ChkGoodSelectAll.Checked = false;
            ChkUndeliveredSelectAll.Checked = false;

            if (ChkFaultySelectAll.Checked == true)
            {
                for (int i = 0; i < grdSTBDetails.Rows.Count; i++)
                {
                    RadioButton RdoGood = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoGood");
                    RadioButton RdoFaulty = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoFaulty");
                    RadioButton RdoUndelivered = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoUndelivered");

                    RdoGood.Checked = false;
                    RdoFaulty.Checked = true;
                    RdoUndelivered.Checked = false;
                }
            }
        }

        protected void OnCheckedChanged_UndeliveredSelectAll(object sender, EventArgs e)
        {
            CheckBox ChkGoodSelectAll = (CheckBox)grdSTBDetails.HeaderRow.FindControl("ChkGoodSelectAll");
            CheckBox ChkFaultySelectAll = (CheckBox)grdSTBDetails.HeaderRow.FindControl("ChkFaultySelectAll");
            CheckBox ChkUndeliveredSelectAll = (CheckBox)grdSTBDetails.HeaderRow.FindControl("ChkUndeliveredSelectAll");

            ChkGoodSelectAll.Checked = false;
            ChkFaultySelectAll.Checked = false;

            if (ChkUndeliveredSelectAll.Checked == true)
            {
                for (int i = 0; i < grdSTBDetails.Rows.Count; i++)
                {
                    RadioButton RdoGood = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoGood");
                    RadioButton RdoFaulty = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoFaulty");
                    RadioButton RdoUndelivered = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoUndelivered");

                    RdoGood.Checked = false;
                    RdoFaulty.Checked = false;
                    RdoUndelivered.Checked = true;
                }
            }
        }

        protected void grdSTBDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
            }
        }

        protected void lnkdownload_click(object sender, EventArgs e)
        {

            if (grdSTBDetails.Rows.Count > 0)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;
                StreamWriter sw = new StreamWriter(Server.MapPath("../MyExcelFile/") + Session["username"].ToString() + "_Warehousedatalco_" + datetime + ".xls");
                try
                {
                    int j = 0;
                    String strheader = "STB/VC/MAC" + Convert.ToChar(9)
                        + "Type" + Convert.ToChar(9)
                        + "Status(G,F,U)" + Convert.ToChar(9);
                    while (j < grdSTBDetails.Rows.Count)
                    {
                        sw.WriteLine(strheader);

                        for (int i = 0; i < grdSTBDetails.Rows.Count; i++)
                        {
                            j = j + 1;

                            RadioButton RdoGood = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoGood");
                            RadioButton RdoFaulty = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoFaulty");
                            RadioButton RdoUndelivered = (RadioButton)grdSTBDetails.Rows[i].FindControl("RdoUndelivered");
                            String Stbno = "'"+grdSTBDetails.Rows[i].Cells[1].Text;
                            String Type = grdSTBDetails.Rows[i].Cells[2].Text;
                            String Status = "";


                            if (RdoGood.Checked == true)
                            {
                                Status = "G";
                            }

                            else if (RdoFaulty.Checked == true)
                            {
                                Status = "F";
                            }

                            else if (RdoUndelivered.Checked == true)
                            {
                                Status = "U";
                            }


                            string strrow = Stbno + Convert.ToChar(9)
                                          + Type + Convert.ToChar(9)
                                          + Status + Convert.ToChar(9);
                            ;
                            sw.WriteLine(strrow);
                        }
                    }
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception ex)
                {
                    sw.Flush();
                    sw.Close();
                    lblmsg.Text = "Error : " + ex.Message.Trim();
                    return;
                }
                Response.Redirect("../MyExcelFile/" + Session["username"].ToString() + "_Warehousedatalco_" + datetime + ".xls");


            }
            else
            {
                lblmsg.Text = "No Data Found.";
            }
        }
        protected void lnkbulk_click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile == true)
            {
                DateTime dd = DateTime.Now;
                string datetime = dd.Day + "" + dd.Month + "" + dd.Year + "" + dd.Hour + "" + dd.Minute + "" + dd.Second;
                String Path = Server.MapPath("~//ImageGarbage") + "\\" + datetime + Session["username"].ToString() + "_Warehouse";

                if (FileUpload1.HasFile)
                {
                    FileUpload1.SaveAs(Path + ".txt");
                }

                string[] lines = System.IO.File.ReadAllLines(Path + ".txt");

                if (lines.Length > grdSTBDetails.Rows.Count)
                {
                    MessageAlert("Please upload valid file","");
                    return;
                }

                DataTable transdetail = (DataTable)ViewState["transdetail"];

                string Receipttype = "";
                if (transdetail.Rows[0]["receiptno"].ToString().ToUpper().Contains("SPSR"))
                {
                    Receipttype = "SPSR";
                }

                else if (transdetail.Rows[0]["receiptno"].ToString().ToUpper().Contains("SPSN"))
                {
                    Receipttype = "SPSN";
                }
                else if (transdetail.Rows[0]["receiptno"].ToString().ToUpper().Contains("PPSN"))
                {
                    Receipttype = "PPSN";
                }
                else
                {
                    Receipttype = "PPSR";
                }

                DataTable Tbllist = new DataTable();
                Tbllist.Columns.Add("transid");
                Tbllist.Columns.Add("orderno");
                Tbllist.Columns.Add("lcocode");
                Tbllist.Columns.Add("deviceid");
                Tbllist.Columns.Add("boxtype");
                Tbllist.Columns.Add("type");
                Tbllist.Columns.Add("model");
                Tbllist.Columns.Add("manufacturer");
                Tbllist.Columns.Add("company");
                Tbllist.Columns.Add("city");
                Tbllist.Columns.Add("state");
                Tbllist.Columns.Add("scheameid");
                Tbllist.Columns.Add("insby");
                Tbllist.Columns.Add("Flag");
                Tbllist.Columns.Add("STBID");

                String str = "";

                String Message = "";
                try
                {
                    foreach (var line in lines)
                    {
                        var STB = line.Split('\t')[0].Replace("'","");
                        var type = line.Split('\t')[1];
                        var STBStatus = line.Split('\t')[2];

                        if (type == "STB" || type == "VC")
                        {
                        }
                        else
                        {
                            Message = "Please provide Proper Type(STB or VC)";
                            str = "";
                            break;
                        }

                        if (STBStatus != "G" || STBStatus != "F" || STBStatus != "U")
                        {

                        }
                        else
                        {
                            Message = "Please provide Proper Type(Good, Faulty or Undiliverd)";
                            str = "";
                            break;
                        }
                        DataTable tbl = (from DataRow dr in transdetail.Rows
                                         where dr["STB"].ToString() == STB
                                         select dr).CopyToDataTable();
                        string Stbid = tbl.Rows[0]["stbid"].ToString();
                        string Usertrans = tbl.Rows[0]["warehouse"].ToString();

                        str += Stbid;

                        if (STBStatus == "G")
                        {
                            if (type == "STB")
                            {
                                Tbllist.Rows.Add(transdetail.Rows[0]["transid"], transdetail.Rows[0]["receiptno"], Session["username"].ToString(), STB, transdetail.Rows[0]["var_pisnewstb_boxtype"],
                                       "STB", transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["company"], transdetail.Rows[0]["city"],
                                        transdetail.Rows[0]["state"], transdetail.Rows[0]["num_pisnewstb_schemeid"], Session["username"].ToString(), STBStatus, Stbid);
                            }
                            else if (type == "VC")
                            {
                                Tbllist.Rows.Add(transdetail.Rows[0]["transid"], transdetail.Rows[0]["receiptno"], Session["username"].ToString(), STB, transdetail.Rows[0]["var_pisnewstb_boxtype"],
                                     "VC", transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["company"], transdetail.Rows[0]["city"],
                                      transdetail.Rows[0]["state"], transdetail.Rows[0]["num_pisnewstb_schemeid"], Session["username"].ToString(), STBStatus, Stbid);
                            }
                        }
                        else
                        {
                            if (type == "STB")
                            {
                                Tbllist.Rows.Add(transdetail.Rows[0]["transid"], transdetail.Rows[0]["receiptno"], Usertrans, STB, transdetail.Rows[0]["var_pisnewstb_boxtype"],
                                       "STB", transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["company"], transdetail.Rows[0]["city"],
                                        transdetail.Rows[0]["state"], transdetail.Rows[0]["num_pisnewstb_schemeid"], Session["username"].ToString(), STBStatus, Stbid);
                            }
                            else if (type == "VC")
                            {
                                Tbllist.Rows.Add(transdetail.Rows[0]["transid"], transdetail.Rows[0]["receiptno"], Usertrans, STB, transdetail.Rows[0]["var_pisnewstb_boxtype"],
                                     "VC", transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["makemodel"], transdetail.Rows[0]["company"], transdetail.Rows[0]["city"],
                                      transdetail.Rows[0]["state"], transdetail.Rows[0]["num_pisnewstb_schemeid"], Session["username"].ToString(), STBStatus, Stbid);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    MessageAlert("Error in Uploaded Data", "");
                    return;
                }

                if (str == "")
                {
                    MessageAlert(Message, "");
                    return;
                }

                String IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (IPAddress == null)
                {
                    IPAddress = Request.ServerVariables["REMOTE_ADDR"];
                }
                string uploadResult = "";
                DateTime date = DateTime.Now;
                string cur_timestamp = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");

                string cur_time = DateTime.Now.ToString("dd-MMM-yyyy_hh:mm:ss");

                Random random = new Random();
                string upload_id = Session["username"].ToString() + "_" + cur_time + "_" + random.Next(1000, 9999);
                if (Tbllist.Rows.Count > 0)
                {
                    string directoryPath = string.Format(Server.MapPath("MyExcelFile/" + Session["username"].ToString()));

                    try
                    {
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        // btnBeginTrans.Visible = false;
                        return;
                    }
                    string file_name = "Warehousedata" + Session["username"].ToString() + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".txt";
                    string filepath = Server.MapPath("MyExcelFile/" + Session["username"].ToString()) + "\\" + file_name;
                    ExportDataTabletoFile(Tbllist, "", true, filepath);



                    Cls_Presentation_Helper helper = new Cls_Presentation_Helper();

                    String DataSource = (String)helper.GetDatabaseDateSource();
                    String UserId = (String)helper.GetDatabaseUserId();
                    String Password = (String)helper.GetDatabasePassword();

                    string table_columns = "(num_inventval_transid,var_inventval_upload_id  constant \"" + upload_id + "\",var_inventval_orderno,var_inventval_lcocode,var_inventval_deviceid,var_inventval_boxtype,var_inventval_type,var_inventval_model,var_inventval_manufacturer,var_inventval_company,var_inventval_city,var_inventval_state,num_inventval_scheameid,var_inventval_insby,VAR_INVENTVAL_STATUS,NUM_INVENTVAL_STBID,dat_inventval_insdate constant \"" + cur_timestamp + "\",var_inventval_apistatus,var_inventval_apismsg,dat_inventval_valdate,VAR_INVENTVAL_VALTYPE constant \"N\",VAR_INVENTVAL_TRANSTYPE constant \"" + Receipttype + "\",VAR_INVENTVAL_IP constant \"" + IPAddress + "\",VAR_INVENTVAL_REMARK constant \"" + txtRemark.Text.Trim() + "\")";

                    uploadResult = helper.cDataUpload(Server.MapPath("MyExcelFile/" + Session["username"].ToString()) + "\\" + file_name,
                                                          Server.MapPath("MyExcelFile/" + Session["username"].ToString()) + "\\HathwayCTL.txt",
                                                          Server.MapPath("MyExcelFile/" + Session["username"].ToString()) + "\\HathwayLOG.log",
                                                          table_columns, DataSource, UserId, Password, "aoup_lcopre_pis_invent_raw",
                                                         upload_id);

                }
                else
                {
                    uploadResult = "0";
                }
                if (uploadResult == "0")
                {
                    string response = "";
                    if (Tbllist.Rows.Count > 0)
                    {
                        response = obj.UpdateInvIntStatus(Session["username"].ToString(), "L", transdetail.Rows[0]["transid"].ToString(), upload_id);
                    }
                    else
                    {
                        response = "9999";
                    }
                    if (response == "9999")
                    {
                        Response.Redirect("FrmWarehouseBulkProcess.aspx?uniqueid=" + upload_id);

                    }
                    else
                    {
                        MessageAlert("Error While Updating", "");
                        return;
                    }
                }
                else
                {
                    if (uploadResult == "")
                    {
                        MessageAlert("Error While Uploading...", "");
                        return;
                    }
                    else
                    {
                        MessageAlert(uploadResult.ToString(), "");
                        return;
                    }
                }

            }
            else
            {
                MessageAlert("Please upload file","");
                return;
            }
        }
    }
}