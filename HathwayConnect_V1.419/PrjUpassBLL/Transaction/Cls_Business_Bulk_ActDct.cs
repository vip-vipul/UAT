using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;

namespace PrjUpassBLL.Transaction
{
    public class Cls_Business_Bulk_ActDct
    {
        public string bulkUploadTemp(string username, string IPAdd,string  upload_id)
        {
            Cls_Data_Bulk_ActDct obj = new Cls_Data_Bulk_ActDct();
            return obj.bulkUploadTemp(username, IPAdd,upload_id);
        }
        public string bulkSchedulUploadTemp(string username, string IPAdd, string upload_id)
        {
            Cls_Data_Bulk_ActDct obj = new Cls_Data_Bulk_ActDct();
            return obj.bulkSchedulUploadTemp(username, IPAdd, upload_id);
        }

        public string bulkUploadMst(string username)
        {
            Cls_Data_Bulk_ActDct obj = new Cls_Data_Bulk_ActDct();
            return obj.bulkUploadMst(username);
        }
    }
}
