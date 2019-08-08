using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.IO;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_rptSMSDelivery
    {
        public DataTable GetDetails(Hashtable htAddPlanParams, string username, string operid, string catid)
        {
            try
            {
                string from = htAddPlanParams["from"].ToString();
                string to = htAddPlanParams["to"].ToString();
                string lco = htAddPlanParams["lco"].ToString();

                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = " select a.var_sms_vcid,a.num_sms_clcontact,a.var_sms_message,a.var_sms_status,a.var_sms_username,to_char(a.date_sms_dt, 'dd-Mon-yyyy') date_sms_dt " +
                         " FROM view_lcopre_notif_sms_log a " +
                         " where a.dt >='" + from + "' " +
                         " and a.dt <='" + to + "' " +
                       // " and upper(a.var_sms_username) =upper('" + username + "') ";

                       " and a.var_sms_username='" + lco + "'";


                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptSMSDelivery.cs-GetDetails");
                return null;
            }
        }
    }
}
