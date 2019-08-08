using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;

namespace PrjUpassBLL.Transaction
{
    public class cls_business_rcptpis
    {
        public string getrcptData(string username, string transid)
        {
            cls_data_rcptpis ob = new cls_data_rcptpis();
            return ob.getrcptData(username, transid);
        }
    }
}
