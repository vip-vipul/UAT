using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;

namespace PrjUpassBLL.Transaction
{
    public class Cls_Business_TransBulk_AutoRenewal
    {
        public string bulkAutoRenewalTemp(string username)
        {
            Cls_Data_TransBulk_AutoRenewal Data = new Cls_Data_TransBulk_AutoRenewal();
            return  Data.bulkAutoRenewalTemp(username);
        }

        public string bulkAutoRenewalMst(string username)
        {
            Cls_Data_TransBulk_AutoRenewal Data = new Cls_Data_TransBulk_AutoRenewal();
            return Data.bulkAutoRenewalMst(username);
        }
    }
}
