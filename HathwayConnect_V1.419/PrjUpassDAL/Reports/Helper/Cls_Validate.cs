using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrjUpassDAL.Helper
{
    class Cls_Validate
    {
        public bool CheckBalance(string username, double planamt, double availBal)
        {
            if (planamt > availBal)
                return false;
            else
                return true;
        }

        public bool MaxPackvalid(string username, double NumberofPack)
        {
            if (NumberofPack > 100)
                return false;
            else
                return true;
        }
    }
}
