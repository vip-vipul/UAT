using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;

namespace PrjUpassDAL.Reports
{
   public class Cls_Data_rptChildTVDiscount
    {
       public DataTable GetTransations(string whereClauseStr, string username)
       {
           try{
       
               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;

               StrQry = "SELECT * FROM VIEW_CHILD_DISC_TRANS WHERE var_distrans_user = '" + username + "' and " + whereClauseStr;

               return ObjHelper.GetDataTable(StrQry);
           }
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_rptChildTVDiscount.cs");
               return null;
           }
       }
    }
}
