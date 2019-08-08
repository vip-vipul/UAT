using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Master;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Master
{
    public class Cls_Business_RptBulkTransProces
    {
        public DataTable GetDetails(string username, string operid, string catid, string Unique_id)
        {
            Cls_Data_RptBulkTransProces obj = new Cls_Data_RptBulkTransProces();
            return obj.GetDetails(username, operid, catid, Unique_id);
        }
        public DataTable GetBulkStatusDetails(string username, string upload_id, int IntBulkStatus)
        {
            string whereString = "";

            //string searchParamStr = "";


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


            Cls_Data_RptBulkTransProces obj = new Cls_Data_RptBulkTransProces();
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


            Cls_Data_RptBulkTransProces obj = new Cls_Data_RptBulkTransProces();
            return obj.GetBulkchange(username, upload_id, IntBulkStatus, whereString);
        }
    
        //----
        public DataTable GetEcafDetails(string username, string operid, string catid, string Unique_id)
        {
            Cls_Data_RptBulkTransProces obj = new Cls_Data_RptBulkTransProces();
            return obj.GetEcafDetails(username, operid, catid, Unique_id);
        }

        public DataTable GetBulkEcafStatusDetails(string username, string upload_id, int IntBulkStatus)
        {
            string whereString = "";

            //string searchParamStr = "";


            if (IntBulkStatus == 1)
            {
                whereString += "   b.var_cust_uniquno= '" + upload_id + "' ";
            }
            else if (IntBulkStatus == 2)
            {
                whereString += " b. var_cust_uniquno= '" + upload_id + "' and b.var_cust_errorcode = '1' ";
            }
            else if (IntBulkStatus == 3)
            {
                whereString += " b.var_cust_uniquno= '" + upload_id + "' and b.var_cust_errorcode = '0'  ";
            }
            else if (IntBulkStatus == 4)
            {
                whereString += " b.var_cust_uniquno= '" + upload_id + "' and b.var_cust_errorcode  is null  and b.var_cust_processflag='N'  ";
            }


            Cls_Data_RptBulkTransProces obj = new Cls_Data_RptBulkTransProces();
            return obj.GetBulkEcafStatusDetails(username, upload_id, IntBulkStatus, whereString);
        }

        public DataTable GetBulkEcafDetails(string username, string from, string to)
        {
            Cls_Data_RptBulkTransProces obj = new Cls_Data_RptBulkTransProces();
            return obj.GetBulkEcafDetails(username, from, to);
        }
        public DataTable GetExcelData(string username, string upload_id, int status)
        {
            Cls_Data_RptBulkTransProces obj = new Cls_Data_RptBulkTransProces();
            return obj.GetExcelData(username, upload_id, status);
        }
    }
}
