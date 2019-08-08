using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
    public class Cls_Business_TransBulk_Discount
    {
        public string bulkUploadTemp(string username)
        {
            Cls_Data_TransBulk_Discount obj = new Cls_Data_TransBulk_Discount();
            return obj.bulkUploadTemp(username);
        }

        public string bulkUploadMst(string username)
        {
            Cls_Data_TransBulk_Discount obj = new Cls_Data_TransBulk_Discount();
            return obj.bulkUploadMst(username);
        }

        public string bulkPlanUpload(string username)
        {
            Cls_Data_TransBulk_Discount obj = new Cls_Data_TransBulk_Discount();
            return obj.bulkPlanUpload(username);
        }
    }
}
