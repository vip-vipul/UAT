using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
    public class Cls_BLL_TransHwayGlobalAutoRenew
    {
        public DataTable LcoPayment(string ht)
        {
            Cls_Data_TransHwayGlobalAutoRenew obj = new Cls_Data_TransHwayGlobalAutoRenew();
            return obj.LcoPaymentDetails(ht);
        }

        public string statuschange(Hashtable ht)
        {
            Cls_Data_TransHwayGlobalAutoRenew obj = new Cls_Data_TransHwayGlobalAutoRenew();
            return obj.GARstatuschange(ht);
        }

    }
}
