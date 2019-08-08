using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
   public class Cls_Business_RptDiscount
    {
       public Hashtable GetTransations(Hashtable htTopupParams, string username, string catid, string operator_id)
       {
           string whereString = "";
           string from = htTopupParams["from"].ToString();
           string to = htTopupParams["to"].ToString();
           string searchParamStr = "";

           if (from != null && from != "")
           {
               whereString += "  dt >=  '" + from + "' ";
               searchParamStr += " <b>Transaction From : </b> " + from;
           }
           if (to != null && to != "")
           {
               whereString += " and dt <=  '" + to + "' ";
               searchParamStr += " <b>Transaction To : </b> " + to;
           }
           whereString += " and flg ='SP' ";
           whereString += " and LCOCODE ='" + username + "' ";
           
           //if (catid == "2")
           //{
           //    whereString += "  and PARENTID= '" + operator_id + "' ";
           //}
           //else if (catid == "5")
           //{
           //    whereString += "  and DISTID= '" + operator_id + "' ";
           //}
           //else if (catid == "3")
           //{
           //    whereString += "  and OPERID= '" + operator_id + "' ";
           //}
           //else if (catid == "10")
           //{
           //    whereString += "  and hoid= '" + operator_id + "' ";
           //}
           //else
           //{
           //}

           Cls_Data_RptDiscount objDataRptTopup = new Cls_Data_RptDiscount();
           Hashtable htResponse = new Hashtable();
           htResponse.Add("htResponse", objDataRptTopup.GetTransations(whereString, username));
           htResponse.Add("ParamStr", searchParamStr);
           return htResponse;
       }
    }
}
