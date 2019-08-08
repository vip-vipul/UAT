using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Collections;
using System.Data;

namespace PrjUpassBLL.Transaction
{
    public class Cls_BLL_TransHwayBulkRenewal
    {
        public string GetBulkRenewData(string username, string uploadid, string filename, string fromdt, string todt, string plan, string plan_type, string vcid)
        {
            Cls_data_TransHwayBulkRenewal obj = new Cls_data_TransHwayBulkRenewal();
            return obj.BulkRenewMoveData(username, uploadid, filename, fromdt, todt, plan, plan_type, vcid);
        }

        public DataTable getUploadedData(string username, string upload_id)
        {
            Cls_data_TransHwayBulkRenewal obj = new Cls_data_TransHwayBulkRenewal();
            return obj.getUploadedRenewalData(username, upload_id);
        }

        public DataTable GetExpDetails(Hashtable htAddPlanParams, string username, string operid, string catid, string type, string planname)
        {
            Cls_data_TransHwayBulkRenewal obj = new Cls_data_TransHwayBulkRenewal();
            return obj.GetExpDetails(htAddPlanParams, username, operid, catid, type, planname);
        }

        public DataTable FillPlanCombo(Hashtable htAddPlanParams, string username, string operid, string catid, string type, string lcoCode)
        {
            Cls_data_TransHwayBulkRenewal obj = new Cls_data_TransHwayBulkRenewal();
            return obj.FillplanCombo(htAddPlanParams, username, operid, catid, type, lcoCode);
        }
    }
}
