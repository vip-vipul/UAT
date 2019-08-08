using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL;
using PrjUpassDAL.Transaction;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
    public class Cls_BLL_FrmDiscount
    {
        public string InsrtDiscount(Hashtable ht)
        {
            Cls_Data_FrmDiscount ob = new Cls_Data_FrmDiscount();
            return ob.DiscountInsert(ht);

        }

    }
}
