using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
   public class Cls_Business_rptNew_Activation
    {
       public Hashtable GetNewActivation(Hashtable htAddPlanParams, string username)
       {
           Cls_Data_rptNewActivation objData = new Cls_Data_rptNewActivation();
           Hashtable htResponse = new Hashtable();
           htResponse.Add("htResponse", objData.GetNewActivation(htAddPlanParams, username));
           return htResponse;
       }
       public Hashtable GetSTBSWAPDet(Hashtable htAddPlanParams, string username)
       {
           Cls_Data_rptNewActivation objData = new Cls_Data_rptNewActivation();
           Hashtable htResponse = new Hashtable();
           htResponse.Add("htResponse", objData.GetSTBSWAPDet(htAddPlanParams, username));
           return htResponse;
       }
    }
}
