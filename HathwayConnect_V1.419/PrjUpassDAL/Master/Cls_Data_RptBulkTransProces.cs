using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Master
{
   public class Cls_Data_RptBulkTransProces
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


               StrQry += " FROM aoup_lcopre_bulk_temp b ";
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


               StrQry += " FROM aoup_lcopre_bulk_change_temp b ";
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


       //-------

       public DataTable GetEcafDetails(string username, string operid, string catid, string Unique_id)
       {
           try
           {

               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;
               StrQry = " Select a.uploadid, a.total, a.success, a.failed,(a.total-(a.success+a.failed)) remaing" +
                        "  FROM view_lcopre_bulk_ecaf_summary a " +

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

       public DataTable GetBulkEcafStatusDetails(string username, string upload_id, int IntBulkStatus, string whereString)
       {
           try
           {

               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;
               StrQry = "SELECT b.var_cust_uniquno uniqueID,b.var_cust_stb STBNO,b.var_cust_vc VCID ,";

               if (IntBulkStatus == 1)
               {
                   StrQry += " (case when b.var_cust_errorcode = '1' then 'Success' else case when b.var_cust_errorcode = '0' then 'Failed' ";
                   StrQry += " else case when b.var_cust_errorcode is null and b.var_cust_processflag='N' then 'in-progress' ";
                   StrQry += "  end end end  ) Status";
               }


               else if (IntBulkStatus == 2)
               {
                   StrQry += " (case when b.var_cust_errorcode = '1' then 'success' end) Status ";
               }
               else if (IntBulkStatus == 3)
               {
                   StrQry += " (case when b.var_cust_errorcode = '0' then 'failed' end) Status ";
               }
               else if (IntBulkStatus == 4)
               {
                   StrQry += " (case when b.var_cust_errorcode is null  and b.var_cust_processflag='N'  then 'in-process' end) Status ";
               }


               StrQry += " FROM aoup_crf_cust_bulk_temp b ";
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

       public DataTable GetBulkEcafDetails(string username, string from, string to)
       {
           try
           {

               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;
               StrQry = "SELECT * from view_lcopre_bulk_ecaf_summary where trunc(dat_lcopre_bulk_insdt)>='" + from + "' and trunc(dat_lcopre_bulk_insdt)<='" + to + "' and var_lcopre_bulk_insby='" + username + "'";

               return ObjHelper.GetDataTable(StrQry);
           }
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptExpiry.cs-GetBulkStatusDetails");
               return null;
           }
       }
       public DataTable GetExcelData(string username, string upload_id, int status)
       {
           try
           {
               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;
               StrQry = " select * from view_lcopre_BulkEcaf a " +
                   " where a.UNIQUNO = '" + upload_id + "' ";
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
