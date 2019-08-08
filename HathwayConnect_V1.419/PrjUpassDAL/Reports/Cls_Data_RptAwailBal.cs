using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;

namespace PrjUpassDAL.Reports
{
    public class Cls_Data_RptAwailBal
    {
        public DataTable GetTransations(string whereClauseStr, string username)//,string catid, string operator_id)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                StrQry = "SELECT lcocode, lconame, actuallim,allocatedlimit,availablelimit, cityname, statename, companyname, distributor, subdistributor,last_transdt, DASAREA " +
                                " FROM view_lco_Awailbal_det " +
                " where " + whereClauseStr;

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptAwailBal.cs");
                return null;
            }
        }
    }
}
