using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PrjUpassDAL.Transaction;

namespace PrjUpassBLL.Transaction
{
   public class Cls_Business_Faulty_Swap
    {
       public void Get_Faulty_Swap(string  NewSTB, string username,out string OutStatus)
       {
            OutStatus="";
           string whereString = "";
           whereString += " nds_no ='" + NewSTB + "' and lco_code='"+username+"'";
           Cls_Data_Faulty_Swap objDataRptAddPlan = new Cls_Data_Faulty_Swap();
           Hashtable htResponse = new Hashtable();
            objDataRptAddPlan.Get_Faulty_Swap(whereString, username,out OutStatus);
       }

       public void GetPlanTo_Swap(string username, string strMainTV, string strChildTV, string strType, out string OutStatus)
       {
           OutStatus = "";
           Cls_Data_Faulty_Swap objDataRptAddPlan = new Cls_Data_Faulty_Swap();
           Hashtable htResponse = new Hashtable();
           objDataRptAddPlan.GetPlanTo_Swap(username,strMainTV, strChildTV, strType, out OutStatus);
       }

       public string getSTB_Swap(Hashtable ht)
       {
           Cls_Data_Faulty_Swap obj = new Cls_Data_Faulty_Swap();
           return obj.getSTB_Swap(ht);
       }
    
    }
}
