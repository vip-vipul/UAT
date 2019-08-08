using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;

namespace PrjUpassBLL.Transaction
{
   public class Cls_Business_Selfcare
    {
        public string addSelfcare(string username)
        {
            Cls_Selfcare obj = new Cls_Selfcare();
            return obj.addEnableSelfcare(username);
        } 
    }
}
