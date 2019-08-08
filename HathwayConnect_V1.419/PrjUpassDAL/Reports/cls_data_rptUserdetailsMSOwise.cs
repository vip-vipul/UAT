
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;
using PrjUpassDAL.Helper;


namespace PrjUpassDAL.Reports
{
    public class cls_data_rptUserdetailsMSOwise
    {
        public DataTable getMSOuserdetails(string username, string category, string operid)
        {
            try
            {

                string StrQry = "SELECT a.userid, a.username, a.userowner, a.fname, a.mname, ";
                StrQry += "  a.lname, a.addr, a.code, a.brmpoid, a.ststeid, a.cityid, a.email, ";
                StrQry += "  a.mobno, a.compcode, a.accno, a.insby, a.insdt, a.flag,a.OPER_ID,a.PARENTID,a.HO_OPER_ID ";
                StrQry += "  FROM view_msopre_user_det a ";
                if (category == "2")
                {
                    StrQry += " where a.PARENTID='" + operid + "'";
                }
                else if (category == "3")
                {
                    StrQry += " where a.OPER_ID='" + operid + "'";
                }
                else if (category == "10")
                {
                    StrQry += " where a.HO_OPER_ID='" + operid + "'";
                }

                Cls_Helper ObjHelper = new Cls_Helper();
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_data_rptUserdetailsMSOwise.cs");
                return null;
            }




        }
    }
}
