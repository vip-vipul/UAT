using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
    public class Cls_Business_TransHwayBulkOperation
    {
        public string bulkUploadTemp(string username, string upload_id) {
            Cls_Data_TransHwayBulkOperation obj = new Cls_Data_TransHwayBulkOperation();
            return obj.bulkUploadTemp(username, upload_id);
        }

        public string bulkUploadDuplicate(string username, string upload_id)
        {
            Cls_Data_TransHwayBulkOperation obj = new Cls_Data_TransHwayBulkOperation();
            return obj.bulkUploadDuplicate(username, upload_id);
        }

        public string bulkValidate(string username, string upload_id, string cust_data, string service_data, string action, string plan_name, string lco_code) {
            Cls_Data_TransHwayBulkOperation obj = new Cls_Data_TransHwayBulkOperation();
            return obj.bulkValidate(username, upload_id, cust_data, service_data, action, plan_name, lco_code);
        }

        public DataTable getUploadedData(string username, string upload_id) {
            Cls_Data_TransHwayBulkOperation obj = new Cls_Data_TransHwayBulkOperation();
            return obj.getUploadedData(username, upload_id);
        }
        public string bulkprocessupdt(string username, string upload_id)
        {
            Cls_Data_TransHwayBulkOperation obj = new Cls_Data_TransHwayBulkOperation();
            return obj.bulkprocessupdt(username, upload_id);
        }

        public string validateBulkTrans(Hashtable ht)
        {
            Cls_Data_TransHwayBulkOperation objAsPl = new Cls_Data_TransHwayBulkOperation();
            return objAsPl.validateBulkTrans(ht);
        }

        public string bulkTransIns(Hashtable ht)
        {
            Cls_Data_TransHwayBulkOperation objAsPl = new Cls_Data_TransHwayBulkOperation();
            return objAsPl.bulkTransIns(ht);
        }

        public void bulkStatusUpdt(Hashtable ht)
        {
            Cls_Data_TransHwayBulkOperation objAsPl = new Cls_Data_TransHwayBulkOperation();
            objAsPl.bulkStatusUpdt(ht);
        }

        public string masterMovement(string username, string upload_id)
        {
            Cls_Data_TransHwayBulkOperation objAsPl = new Cls_Data_TransHwayBulkOperation();
            return objAsPl.masterMovement(username, upload_id);
        }

        public string ProvECS(Hashtable ht)
        {
            Cls_Data_TransHwayBulkOperation objAsPl = new Cls_Data_TransHwayBulkOperation();
            return objAsPl.ProvECS(ht);
        }

        public void GetAvalBalance(string UserName, out string AvalBalance)
        {
            AvalBalance = "";
            Cls_Data_TransHwayBulkOperation obj = new Cls_Data_TransHwayBulkOperation();
            obj.GetAvalBalance(UserName, out AvalBalance);
        }
        public void GetRequToday(string UserName, out string RequToday)
        {
            RequToday = "";
            Cls_Data_TransHwayBulkOperation obj = new Cls_Data_TransHwayBulkOperation();
            obj.GetRequToday(UserName, out RequToday);

        }
        public void GetRequTomor(string UserName, out string RequTomor)
        {
            RequTomor = "";
            Cls_Data_TransHwayBulkOperation obj = new Cls_Data_TransHwayBulkOperation();
            obj.GetRequTomor(UserName, out RequTomor);
        }

        public string bulkUploadActTemp(string username, string upload_id)
        {
            Cls_Data_TransHwayBulkOperation obj = new Cls_Data_TransHwayBulkOperation();
            return obj.bulkUploadActTemp(username, upload_id);
        }

        public string bulkUploadActTempRemove(string lblbulkid,string upload_id, string date,string username)
        {
            Cls_Data_TransHwayBulkOperation obj = new Cls_Data_TransHwayBulkOperation();
            return obj.bulkUploadActTempRemove(lblbulkid,upload_id, date, username);
        }
    }
}
