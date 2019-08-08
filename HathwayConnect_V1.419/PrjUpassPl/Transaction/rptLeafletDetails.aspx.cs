using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PrjUpassBLL.Transaction;

namespace PrjUpassPl.Transaction
{
    public partial class rptLeafletDetails : System.Web.UI.Page
    {
        string username, catid, operator_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.PageHeading = "Leaflet";
                Session["RightsKey"] = "N";
                if (Session["username"] != null || Session["operator_id"] != null)
                {
                    username = Session["username"].ToString();
                    catid = Convert.ToString(Session["category"]);
                    operator_id = Convert.ToString(Session["operator_id"]);
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx");

                }
                getleaflet();
            }
        } 

        public void getleaflet()
        {

            if (Session["username"] != null || Session["operator_id"] != null)
            {
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


            cls_business_rptleafletdetails obj = new cls_business_rptleafletdetails();
            String City = "";
            String area = "";
            String state = "";
            obj.GetCity(username, out City, out state, out area);
            ViewState["cityid"] = City;
            ViewState["area"] = area;
            ViewState["state"] = state;


            cls_business_rptleafletdetails ob = new cls_business_rptleafletdetails();

            DataTable dt = ob.getleaflet(username, ViewState["state"].ToString(), ViewState["area"].ToString());

            if (dt == null)
            {
                Response.Redirect("~/ErrorPage.aspx");
                return;
            }
            nodatafound.Visible = false;

            if (dt.Rows.Count == 0)
            {
                nodatafound.Visible = true;
                //no data found 
            }
            else
            {
                xDlstState.DataSource = dt;
                xDlstState.DataBind();

            
            }


        }

        protected void xDlstState_EditCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                HiddenField hdnname = (HiddenField)e.Item.FindControl("hdnname");
                string filename = hdnname.Value;

                HiddenField lbname = (HiddenField)e.Item.FindControl("hdnPath");

                string filepath = lbname.Value;  //hdnname
                string path = filepath;
                
                
                string getExtension = System.IO.Path.GetExtension(path).ToLower();

                downloadFiles(filename, getExtension, filepath);

            }
        }

        public void downloadFiles(string file, string getExtension, string filepath)
        {
           

            if (getExtension == ".jpg" || getExtension == ".jpeg" || getExtension == ".png" || getExtension == ".gif")
            {
                string localFilename = filepath;
                string FileName = file; // It's a file name displayed on downloaded file on client side.

                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "image/jpeg";
                response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
                response.TransmitFile(localFilename);
                response.Flush();
                response.End();
            }
            else if (getExtension == ".xls" || getExtension == ".xlsx")
            {
                string localFilename = filepath;
                string FileName = file; // It's a file name displayed on downloaded file on client side.

                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "application/x-msexcel";
                response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
                response.TransmitFile(localFilename);
                response.Flush();
                response.End();
            }
            else if (getExtension == ".pdf")
            {
                string localFilename = filepath;
                string FileName = file; // It's a file name displayed on downloaded file on client side.

                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "application/pdf";
                response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
                response.TransmitFile(localFilename);
                response.Flush();
                response.End();
            }
            else if (getExtension == ".doc" || getExtension == ".docx")
            {
                string localFilename = filepath;
                string FileName = file; // It's a file name displayed on downloaded file on client side.

                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "application/msword";
                response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
                response.TransmitFile(localFilename);
                response.Flush();
                response.End();
            }

            else
            {
                // lblmsgbox.Text = "Error while downloading the file !!";
                // popMsgBox.Show();

            }
        }
    }
}