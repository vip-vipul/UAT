using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
   public class Cls_Business_RptLastFiveGridBind
    {
       public Hashtable GetLastFiveTransaction(Hashtable htAddPlanParams, string username,string category,string operid)
       {
           string whereString = "";
           string user = htAddPlanParams["user"].ToString();
           whereString += " upper(uname) = upper('" + user + "') ";
           Cls_Data_RptLastFiveGridBind objDataRptAddPlan = new Cls_Data_RptLastFiveGridBind();
           Hashtable htResponse = new Hashtable();
           htResponse.Add("htResponse", objDataRptAddPlan.GetLastFiveTransaction(whereString, username,category,operid));
           
           return htResponse;
       }
    }
}
