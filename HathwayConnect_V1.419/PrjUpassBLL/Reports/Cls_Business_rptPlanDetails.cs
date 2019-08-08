using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_rptPlanDetails
    {
        public DataTable getPlanDetails() {
            Cls_Data_rptPlanDetails obj = new Cls_Data_rptPlanDetails();
            return obj.getPlanData();
        }
    }
}
