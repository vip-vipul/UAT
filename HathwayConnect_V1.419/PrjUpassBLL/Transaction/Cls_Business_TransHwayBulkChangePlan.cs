using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
    public class Cls_Business_TransHwayBulkChangePlan
    {
        public string bulkUploadTemp(string username, string upload_id) {
            Cls_Data_TransHwayBulkChangePlan obj = new Cls_Data_TransHwayBulkChangePlan();
            return obj.bulkUploadTemp(username, upload_id);
        }

        public string bulkValidate(string username, string upload_id, string cust_data, string service_data, string action, string plan_name, string lco_code) {
            Cls_Data_TransHwayBulkChangePlan obj = new Cls_Data_TransHwayBulkChangePlan();
            return obj.bulkValidate(username, upload_id, cust_data, service_data, action, plan_name, lco_code);
        }

        public DataTable getUploadedData(string username, string upload_id) {
            Cls_Data_TransHwayBulkChangePlan obj = new Cls_Data_TransHwayBulkChangePlan();
            return obj.getUploadedData(username, upload_id);
        }
        public string bulkprocessupdt(string username, string upload_id)
        {
            Cls_Data_TransHwayBulkChangePlan obj = new Cls_Data_TransHwayBulkChangePlan();
            return obj.bulkprocessupdt(username, upload_id);
        }

        public string validateBulkTrans(Hashtable ht)
        {
            Cls_Data_TransHwayBulkChangePlan objAsPl = new Cls_Data_TransHwayBulkChangePlan();
            return objAsPl.validateBulkTrans(ht);
        }

        public string bulkTransIns(Hashtable ht)
        {
            Cls_Data_TransHwayBulkChangePlan objAsPl = new Cls_Data_TransHwayBulkChangePlan();
            return objAsPl.bulkTransIns(ht);
        }

        public void bulkStatusUpdt(Hashtable ht)
        {
            Cls_Data_TransHwayBulkChangePlan objAsPl = new Cls_Data_TransHwayBulkChangePlan();
            objAsPl.bulkStatusUpdt(ht);
        }

        public string masterMovement(string username, string upload_id)
        {
            Cls_Data_TransHwayBulkChangePlan objAsPl = new Cls_Data_TransHwayBulkChangePlan();
            return objAsPl.masterMovement(username, upload_id);
        }

        public string ProvECS(Hashtable ht)
        {
            Cls_Data_TransHwayBulkChangePlan objAsPl = new Cls_Data_TransHwayBulkChangePlan();
            return objAsPl.ProvECS(ht);
        }

        
    }
}
