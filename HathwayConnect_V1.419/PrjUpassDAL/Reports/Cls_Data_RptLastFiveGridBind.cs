
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_RptLastFiveGridBind
    {
        public DataTable GetLastFiveTransaction(string whereClauseStr, string username,string category,string operid)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT a.transid, a.receiptno, a.custid, a.vc, a.custname, a.planid, ";
                StrQry += "  a.plnname, a.plntyp, a.amtdd, a.custprice, a.expdt, a.payterm, ";
                StrQry += "   a.bal, a.companycode, a.flag, a.insby, a.dt, a.tdt, a.uname, ";
                StrQry += "   a.userowner, a.usrid, a.lcocode, a.lconame, a.jvname, ";
                StrQry += "   a.erplco_ac, a.distname, a.subdist, a.city, a.state, a.reason  ";
                StrQry += "  FROM view_lcopre_user_trans_det a  ";
                if (category == "3" || category == "11")
                {
                    StrQry += " where a.operid='" + operid + "' ";
                }
                else
                {
                    StrQry += "   where " + whereClauseStr;
                }
                StrQry += " and rownum <=5 ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptLastFiveGridBind.cs");
                return null;
            }
        }
    }
}
