using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_RptBulkActTransProces
    {
        public DataTable GetDetails(string username, string operid, string catid, string Unique_id)
        {
            Cls_Data_RptBulkActTransProces obj = new Cls_Data_RptBulkActTransProces();
            return obj.GetDetails(username, operid, catid, Unique_id);
        }
      
        public DataTable GetBulkStatusDetails(string username, string upload_id, int IntBulkStatus)
        {
            string whereString = "";

            if (IntBulkStatus == 1)
            {
                whereString += "   b.var_lcopre_bulk_useruniqueid= '" + upload_id + "' ";
            }
            else if (IntBulkStatus == 2)
            {
                whereString += " b. var_lcopre_bulk_useruniqueid= '" + upload_id + "' and b.num_lcopre_bulk_errorcode = '1' ";
            }
            else if (IntBulkStatus == 3)
            {
                whereString += " b.var_lcopre_bulk_useruniqueid= '" + upload_id + "' and b.num_lcopre_bulk_errorcode = '0'  ";
            }
            else if (IntBulkStatus == 4)
            {
                whereString += " b.var_lcopre_bulk_useruniqueid= '" + upload_id + "' and b.num_lcopre_bulk_errorcode  is null  and b.var_lcopre_bulk_process_flag='N'  ";
            }


            Cls_Data_RptBulkActTransProces obj = new Cls_Data_RptBulkActTransProces();
            return obj.GetBulkStatusDetails(username, upload_id, IntBulkStatus, whereString);
        }
        public DataTable GetBulkchange(string username, string upload_id, int IntBulkStatus)
        {
            string whereString = "";

            if (IntBulkStatus == 1)
            {
                whereString += "   b.var_lcopre_bulk_useruniqueid= '" + upload_id + "' ";
            }
            else if (IntBulkStatus == 2)
            {
                whereString += " b. var_lcopre_bulk_useruniqueid= '" + upload_id + "' and b.num_lcopre_bulk_errorcode = '1' ";
            }
            else if (IntBulkStatus == 3)
            {
                whereString += " b.var_lcopre_bulk_useruniqueid= '" + upload_id + "' and b.num_lcopre_bulk_errorcode = '0'  ";
            }
            else if (IntBulkStatus == 4)
            {
                whereString += " b.var_lcopre_bulk_useruniqueid= '" + upload_id + "' and b.num_lcopre_bulk_errorcode  is null  and b.var_lcopre_bulk_process_flag='N'  ";
            }


            Cls_Data_RptBulkActTransProces obj = new Cls_Data_RptBulkActTransProces();
            return obj.GetBulkchange(username, upload_id, IntBulkStatus, whereString);
        }
    }
}
