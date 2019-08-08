using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Reports;
using System.Collections;


namespace PrjUpassBLL.Reports
{
   public class Cls_Business_RptLastFive
    {
        public DataSet GetLcoUserDetails(string username, string catid, string operid)
        {
            Cls_Data_RptLastFive obj = new Cls_Data_RptLastFive();
            return obj.GetLcoUserDetails(username, catid, operid);
        }
    }
}
