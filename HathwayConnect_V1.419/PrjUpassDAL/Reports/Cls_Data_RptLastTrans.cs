using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_RptLastTrans
    {
        public DataTable GetTransations(string whereClauseStr, string username)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT a.transid, a.receiptno, a.custid, a.vcid, a.custname, a.planid, " +
                " a.planname, a.plantype, a.custprice, a.lcoprice, a.expirydt, " +
                  " a.payterm, a.balance, a.companycode, a.flag, a.insby, a.lconame, a.transdt," +
                 " a.transdt1 " +
                " FROM view_cust_last_trans a" +
                "  " + whereClauseStr;
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptLastTrans.cs");
                return null;
            }
        }
    }
}
