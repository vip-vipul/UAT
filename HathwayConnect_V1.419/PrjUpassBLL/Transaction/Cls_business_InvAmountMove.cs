using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
   public class Cls_business_InvAmountMove
    {
       public string UpdateAmount(string sp, Hashtable ht)
       {
           Cls_Data_TransInvAmountMove obj = new Cls_Data_TransInvAmountMove();
           return obj.UpdateAmount(sp, ht);
       }
    }
}
