using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_rptBulkActUpload
    {
        public DataTable GetBulkDetails(string from, string to, string username, string category, string lco)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = "select * from view_lcopre_bulk_stat a where 1=1 ";
                if (category == "3")
                {
                    StrQry += " and a.lcocode = '" + username + "'";
                }
                StrQry += " and trunc(insdt) >= '" + from + "' and trunc(insdt) <= '" + to + "'";
                StrQry += " and a.lcocode='" + lco + "'";
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptBulkActUpload-GetBulkDetails");
                return null;
            }
        }

        public DataTable GetExcelData(string username, string upload_id, int status)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = " select * from view_lcopre_bulk_master a " +
                    " where a.upload_id = '" + upload_id + "' ";
                if (status == 1)
                {
                    StrQry += " and a.status = 'Success' ";
                }
                else if (status == 2)
                {
                    StrQry += " and a.status = 'Failed' ";
                }
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptBulkActUpload-GetExcelData");
                return null;
            }
        }
    }
}
