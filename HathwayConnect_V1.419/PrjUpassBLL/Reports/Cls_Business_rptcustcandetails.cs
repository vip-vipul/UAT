using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_rptcustcandetails
    {
        public Hashtable GetTransationsDet(Hashtable htAddPlanParams2, string username2)
        {
            string whereString = "";
            string from = htAddPlanParams2["from"].ToString();
            string to = htAddPlanParams2["to"].ToString();            
            string searchParamStr = "";

            if (from != null && from != "")
            {
                whereString += "  dt >=  '" + from + "' ";                
            }
            if (to != null && to != "")
            {
                whereString += " and dt <=  '" + to + "' ";               
            }

            whereString += " and FLAG1 in ('CH','C') ";
            Cls_Data_rptcustcandetails obj = new Cls_Data_rptcustcandetails();           
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", obj.GetTransationsDet(whereString, username2));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }
    }
}
