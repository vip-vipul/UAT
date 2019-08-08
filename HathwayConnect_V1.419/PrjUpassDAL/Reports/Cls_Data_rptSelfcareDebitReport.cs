using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;

namespace PrjUpassDAL.Reports
{
   public class Cls_Data_rptSelfcareDebitReport
    {

       public DataTable GetTransations(string whereClauseStr, string username)
       {
           try
           {

               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;

               StrQry = "SELECT * FROM view_selfcare_trans_debit WHERE var_custpay_user = '" + username + "'" + whereClauseStr;
              // StrQry = " SELECT * FROM view_selfcare_trans_debiT WHERE var_custpay_user='" + username + "' AND TRUNC(dat_custpay_transdt)>= AND TRUNC(dat_custpay_transdt)<=";
               return ObjHelper.GetDataTable(StrQry);
           }
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptSelfcareDebitReport.cs");
               return null;
           }
       }

    }
}
