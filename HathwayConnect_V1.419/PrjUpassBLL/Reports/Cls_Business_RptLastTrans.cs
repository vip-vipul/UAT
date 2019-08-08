using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_RptLastTrans
    {
        public Hashtable GetTransations(string username, string catid, string operator_id)
        {
            string whereString = "";
            //string searchParamStr = "";
            if (catid == "2")
            {
                whereString += " where  PARENTID= '" + operator_id + "' ";
            }
            else if (catid == "5")
            {
                whereString += " where  DISTID= '" + operator_id + "' ";
            }
            else if (catid == "3")
            {
                whereString += "  where OPERID= '" + operator_id + "' ";
            }
            else if (catid == "10")
            {
                whereString += " where  hoid= '" + operator_id + "' ";
            }
            else
            {
            }
            Cls_Data_RptLastTrans objDataRptTopup = new Cls_Data_RptLastTrans();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataRptTopup.GetTransations(whereString,username));
            //htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }
    }
}
