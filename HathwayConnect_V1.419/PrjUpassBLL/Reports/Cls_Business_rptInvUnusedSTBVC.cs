using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
   public class Cls_Business_rptInvUnusedSTBVC
    {
       public DataTable GetDetails(Hashtable HT,string username)
       {
           Cls_Data_rptInvUnusedSTBVC obj = new Cls_Data_rptInvUnusedSTBVC();
           return obj.GetDetails(HT, username);
       
       }
    }
}
