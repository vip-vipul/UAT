using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_RptBulkActTransProces
    {
        public DataTable GetDetails(string username, string operid, string catid, string Unique_id)
        {
            try
            {

                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = " Select a.uploadid, a.total, a.success, a.failed,(a.total-(a.success+a.failed)) remaing" +
                         "  FROM view_lcopre_bulk_Trans_Summary a " +
                         " where a.uploadid ='" + Unique_id + "' ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptExpiry.cs-GetDetails");
                return null;
            }
        }

        public DataTable GetBulkStatusDetails(string username, string upload_id, int IntBulkStatus, string whereString)
        {
            try
            {

                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = "SELECT b.var_lcopre_bulk_useruniqueid  uniqueID,b.var_lcopre_bulk_custid customer_id,b.var_lcopre_bulk_planname planname ,";

                if (IntBulkStatus == 1)
                {
                    StrQry += " (case when b.num_lcopre_bulk_errorcode = '1' then 'Success' else case when b.num_lcopre_bulk_errorcode = '0' then 'Failed' ";
                    StrQry += " else case when b.num_lcopre_bulk_errorcode is null and b.var_lcopre_bulk_process_flag='N' then 'in-progress' ";
                    StrQry += "  end end end  ) Status";
                }


                else if (IntBulkStatus == 2)
                {
                    StrQry += " (case when b.num_lcopre_bulk_errorcode = '1' then 'success' end) Status ";
                }
                else if (IntBulkStatus == 3)
                {
                    StrQry += " (case when b.num_lcopre_bulk_errorcode = '0' then 'failed' end) Status ";
                }
                else if (IntBulkStatus == 4)
                {
                    StrQry += " (case when b.num_lcopre_bulk_errorcode is null  and b.var_lcopre_bulk_process_flag='N'  then 'in-process' end) Status ";
                }


                StrQry += " FROM aoup_lcopre_bulkact_temp b ";
                StrQry += " where " + whereString;


                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptExpiry.cs-GetBulkStatusDetails");
                return null;
            }
        }

        public DataTable GetBulkchange(string username, string upload_id, int IntBulkStatus, string whereString)
        {
            try
            {

                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = "SELECT b.var_lcopre_bulk_useruniqueid  uniqueID,b.var_lcopre_bulk_custid customer_id,b.var_lcopre_bulk_planname planname ,";

                if (IntBulkStatus == 1)
                {
                    StrQry += " (case when b.num_lcopre_bulk_errorcode = '1' then 'Success' else case when b.num_lcopre_bulk_errorcode = '0' then 'Failed' ";
                    StrQry += " else case when b.num_lcopre_bulk_errorcode is null and b.var_lcopre_bulk_process_flag='N' then 'in-progress' ";
                    StrQry += "  end end end  ) Status";
                }


                else if (IntBulkStatus == 2)
                {
                    StrQry += " (case when b.num_lcopre_bulk_errorcode = '1' then 'success' end) Status ";
                }
                else if (IntBulkStatus == 3)
                {
                    StrQry += " (case when b.num_lcopre_bulk_errorcode = '0' then 'failed' end) Status ";
                }
                else if (IntBulkStatus == 4)
                {
                    StrQry += " (case when b.num_lcopre_bulk_errorcode is null  and b.var_lcopre_bulk_process_flag='N'  then 'in-process' end) Status ";
                }


                StrQry += " FROM aoup_lcopre_bulk_chngeact_temp b ";
                StrQry += " where " + whereString;


                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptbulkchange.cs-GetBulkchange");
                return null;
            }
        }
    }
}
