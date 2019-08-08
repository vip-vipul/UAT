using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_rptserviceActDact
    {
        public Hashtable Getsvcstatus(Hashtable htAddPlanParams, string username, string catid, string operator_id)
        {
            Cls_data_rptserviceActDact objData = new Cls_data_rptserviceActDact();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objData.Getactdactstat(htAddPlanParams, username, catid, operator_id));
            return htResponse;
        }

        public Hashtable GetBulkSCHstatus(Hashtable htAddPlanParams, string username,  string operator_id)
        {
            Cls_data_rptserviceActDact objData = new Cls_data_rptserviceActDact();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objData.GetBulkSCHstatus(htAddPlanParams, username, operator_id));
            return htResponse;
        }


        public Hashtable GetBulkSCHDISstatus(Hashtable htAddPlanParams, string username, string operator_id)
        {
            Cls_data_rptserviceActDact objData = new Cls_data_rptserviceActDact();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objData.GetBulkSCHDISstatus(htAddPlanParams, username, operator_id));
            return htResponse;
        }

        public string bulkUploadActTempRemove(string lblbulkid,  string username)
        {
            Cls_data_rptserviceActDact objData = new Cls_data_rptserviceActDact();
            Hashtable htResponse = new Hashtable();
            return objData.bulkUploadActTempRemove(lblbulkid, username);
            //return htResponse;
        }
    }
}
