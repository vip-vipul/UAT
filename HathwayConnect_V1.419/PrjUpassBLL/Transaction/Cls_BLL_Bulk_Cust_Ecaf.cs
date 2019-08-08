using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;
namespace PrjUpassBLL.Transaction
{
    public class Cls_BLL_Bulk_Cust_Ecaf
    {


        public string Bulk_Cust_Ecaf_temp(string username)
        {
            Cls_DLL_Bulk_Cust_Ecaf obj = new Cls_DLL_Bulk_Cust_Ecaf();
            return obj.Bulk_Cust_Ecaf_temp(username);
        }

        public string Bulk_Cust_Ecaf_mst(string username)
        {
            Cls_DLL_Bulk_Cust_Ecaf obj = new Cls_DLL_Bulk_Cust_Ecaf();
            return obj.Bulk_Cust_Ecaf_mst(username);
        }
        public string Bulk_Cust_Ecaf_inst(string username, string Uniquno)
        {
            Cls_DLL_Bulk_Cust_Ecaf obj = new Cls_DLL_Bulk_Cust_Ecaf();
            return obj.Bulk_Cust_Ecaf_inst(username, Uniquno);
        }
        



    }
}
