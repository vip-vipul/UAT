using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_rptExpiry_new
    {
        public DataTable GetDetails(Hashtable htAddPlanParams, string username, string operid, string catid)
        {
            Cls_Data_rptExpiry_new obj = new Cls_Data_rptExpiry_new();
            return obj.GetDetails(htAddPlanParams, username, operid, catid);
        }
        public DataTable GetPriceDetails(Hashtable htAddPlanParams, string username, string operid, string catid)
        {
            Cls_Data_rptExpiry_new obj = new Cls_Data_rptExpiry_new();
            return obj.GetPriceDetails(htAddPlanParams, username, operid, catid);
        }

        public DataTable GetPlans(string from, string to, string username, string operid, string catid)
        {
            Cls_Data_rptExpiry_new obj = new Cls_Data_rptExpiry_new();
            return obj.GetPlans(from, to, username, operid, catid);
        }
    }
}
