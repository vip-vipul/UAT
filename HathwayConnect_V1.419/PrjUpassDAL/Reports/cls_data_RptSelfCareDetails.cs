using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;

namespace PrjUpassDAL.Reports
{
   public  class cls_data_RptSelfCareDetails
    {
       public DataTable GetSelfCareDetails(string strQry)
       {
           try
           {
               Cls_Helper ObjHelper = new Cls_Helper();
               return ObjHelper.GetDataTable(strQry);
           }
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb("admin_ho", ex.Message.ToString(), "cls_data_RptSelfCareDetails.cs");
               return null;
           }
       }
    }
}
