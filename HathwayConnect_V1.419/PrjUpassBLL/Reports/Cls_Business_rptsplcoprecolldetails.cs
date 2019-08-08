using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_rptsplcoprecolldetails
    {
        public DataTable lcoprecolldetails(Hashtable htAddPlanParams, string username, string operid, string catid)
        {
            
            cls_Data_lcoprecolldetails obj = new cls_Data_lcoprecolldetails();            
            return obj.lcoprecolldetails(htAddPlanParams, username, operid, catid);
        }
    }
}
