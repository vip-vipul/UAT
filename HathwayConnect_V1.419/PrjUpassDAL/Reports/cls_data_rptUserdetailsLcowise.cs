using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Helper;
using System.Data;
using System.IO;

namespace PrjUpassDAL.Reports
{
    public class cls_data_rptUserdetailsLcowise
    {
        public DataTable GetLcodet(string username, string strQry)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                return ObjHelper.GetDataTable(strQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "cls_data_rptUserdetailsLcowise.cs");
                return null;
            }
        }
    }
}
