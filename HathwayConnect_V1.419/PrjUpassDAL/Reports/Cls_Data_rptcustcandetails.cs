using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.IO;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_rptcustcandetails
    {
        public DataTable GetTransationsDet(string whereClauseStr2, string username2)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT custid, custname, custaddr, vc, plnname, plntyp, userowner,uname,tdt,amtdd,expdt,payterm,bal " +
                    " , lcocode, lconame, jvname, erplco_ac, distname, subdist, city, state, custprice, flag, reason,OBRMSTATUS " +
                                " FROM view_AddPlan_Trans_det " +
                " where " + whereClauseStr2;               

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username2, ex.Message.ToString(), "Cls_Data_rptcustcandetails.cs");
                return null;
            }
        }
    }
}
