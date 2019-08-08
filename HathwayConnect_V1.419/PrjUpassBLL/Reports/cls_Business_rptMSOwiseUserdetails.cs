using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.IO;
using PrjUpassDAL.Reports;



namespace PrjUpassBLL.Reports
{
    public class cls_Business_rptMSOwiseUserdetails
    {
      

        public DataTable GetMSOdet(string username, string category,string operid)
        {
            try
            {
                 cls_data_rptUserdetailsMSOwise obj = new cls_data_rptUserdetailsMSOwise();
                 DataTable dt = obj.getMSOuserdetails(username,category, operid);
                return dt;

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_Business_rptMSOwiseUserdetails.cs");
                return null;
            }
        }
    }
}
