using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
   public class Cls_BLL_AutoRenewCancel
    {
        public string AutoRenewCancel(string username, Hashtable ht)
        {
            Cls_Data_AutoRenewCancel obj = new Cls_Data_AutoRenewCancel();
            return obj.AutoRenewCancel(username, ht);

            
        }
    }
}
