using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_RptLCOPayRev
    {
        public Hashtable GetPaymentRev(Hashtable ht, string username,string operid,string catid)
        {
            string whereString = "";
            string from = ht["from"].ToString();
            string to = ht["to"].ToString();
            string searchParamStr = "";

            if (from != null && from != "")
            {
                whereString += "  a.dat_lcopay_insdt >=  '" + from + "' ";
                searchParamStr += " <b>Transaction From : </b> " + from;
            }
            if (to != null && to != "")
            {
                whereString += " and a.dat_lcopay_insdt <=  '" + to + "' ";
                searchParamStr += " <b>Transaction To : </b> " + to;
            }
            if (catid == "2")
            {
                whereString += "  and a.num_oper_parentid= '" + operid + "' ";
            }
            else if (catid == "5")
            {
                whereString += "  and a.num_oper_distid= '" + operid + "' ";
            }
            else if (catid == "3")
            {
                whereString += "  and a.num_oper_id= '" + operid + "' ";
            }
            else if (catid == "10")
            {
                whereString += "  and a.hoid= '" + operid + "' ";
            }

            Cls_Data_RptLCOPayRev objLcoPayRev = new Cls_Data_RptLCOPayRev();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objLcoPayRev.GetPaymentRev(whereString, username,operid,catid));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }
    }
}
