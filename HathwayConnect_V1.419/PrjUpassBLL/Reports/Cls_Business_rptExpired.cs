﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_rptExpired
    {
        public DataTable GetDetails(Hashtable htAddPlanParams, string username, string operid, string catid)
        {
            Cls_Data_rptExpired obj = new Cls_Data_rptExpired();            
            return obj.GetDetails(htAddPlanParams, username, operid, catid);
        }
    }
}