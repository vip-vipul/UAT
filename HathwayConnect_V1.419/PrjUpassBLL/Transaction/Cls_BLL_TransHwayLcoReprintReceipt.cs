using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
    public class Cls_BLL_TransHwayLcoReprintReceipt
    {
        public DataTable GetLcopaymentDetails(string username, string LcoName, string catid, string operid, string type, string BankName)
        {
            Cls_Data_TransHwayLcoReprintReceipt obj = new Cls_Data_TransHwayLcoReprintReceipt();
            return obj.GetLcopaymentDetails(username, LcoName, catid, operid, type, BankName);
        }    
    }
}
