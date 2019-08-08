using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Reports;
using System.Collections;

namespace PrjUpassBLL.Reports
{
   public class Cls_business_lcoshareDef
    {
       public DataTable getPlanDetails(Hashtable htAddPlanParams, string username, string ctgr)
       {
           Cls_Data_lcoSharedef obj = new Cls_Data_lcoSharedef();
           return obj.getPlanData(htAddPlanParams, username, ctgr);
       }
    }
}
