using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_rptBulkUpload
    {
        public DataTable GetBulkDetails(string from, string to, string username, string category, string lco)
        {
            Cls_Data_rptBulkActUpload obj = new Cls_Data_rptBulkActUpload();
            return obj.GetBulkDetails(from, to, username, category, lco);
        }

        public DataTable GetExcelData(string username, string upload_id, int status)
        {
            Cls_Data_rptBulkActUpload obj = new Cls_Data_rptBulkActUpload();
            return obj.GetExcelData(username, upload_id, status);
        }
    }
}
