using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Reports;
using System.Collections;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_rptpartyledopenbal
    {
        public DataTable getpartyledDet(Hashtable htAddPlanParams)
        {
            Cls_Data_rptpartyledopenbal obj = new Cls_Data_rptpartyledopenbal();
            return obj.getpartyledDet(htAddPlanParams);
        }
    }
}
