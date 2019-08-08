using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_RptAwailBal
    {
        public Hashtable GetTransations(Hashtable htTopupParams, string username, string catid, string operator_id)
        {
            string whereString = "";
            string lcd = htTopupParams["lcd"].ToString();
           // string lnm = htTopupParams["lnm"].ToString();
            string txtsear = htTopupParams["txtsear"].ToString();
            string searchParamStr = "";

            if (lcd == "0")
            {
                whereString += "  UPPER(lcocode) LIKE UPPER('" + txtsear.ToString().Trim() + "%') ";
                //searchParamStr += " <b>Transaction From : </b> " + from;
            }
            //if (lnm == "1")
            //{
            //    whereString += "   UPPER(lconame) LIKE UPPER('%" + txtsear.ToString().Trim() + "%') ";
            //    //searchParamStr += " <b>Transaction To : </b> " + to;
            //}
            if (catid == "2")
            {
                whereString += "  and PARENTID= '" + operator_id + "' ";
            }
            else if (catid == "5")
            {
                whereString += "  and DISTID= '" + operator_id + "' ";
            }
            else if (catid == "3" || catid=="11")
            {
                whereString += "  and OPERID= '" + operator_id + "' ";
            }
            else if (catid == "10")
            {
                whereString += "  and hoid= '" + operator_id + "' ";
            }
            else
            {
            }

            Cls_Data_RptAwailBal objDataRptAwailBal = new Cls_Data_RptAwailBal();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataRptAwailBal.GetTransations(whereString, username));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }
    }
}
