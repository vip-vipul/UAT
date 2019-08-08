using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Transaction;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
   public class Cls_Business_Warehouse
    {
        public DataTable Getdata(string Uploadid, string Type, string username)
        {
            Cls_Data_Warehouse obj = new Cls_Data_Warehouse();
            return obj.Getdata(Uploadid, Type, username);
        }

        public DataTable GetInvBulkValiddata(Hashtable Ht, string username)
        {
            Cls_Data_Warehouse obj = new Cls_Data_Warehouse();
            return obj.GetInvBulkValiddata(Ht, username);
        }

       public DataTable Getbulkprocessdata(string userid, string uploadid)
       {
           Cls_Data_Warehouse obj = new Cls_Data_Warehouse();
           return obj.Getbulkprocessdata(userid, uploadid);
       }

       public DataTable GetWarehouseAllocationList(string userid)
       {
           Cls_Data_Warehouse obj = new Cls_Data_Warehouse();
           return obj.GetWarehouseAllocationList(userid);
       }

       public DataTable GetReceiptData(string userid, string receiptno)
       {
           Cls_Data_Warehouse obj = new Cls_Data_Warehouse();
           return obj.GetReceiptData(userid, receiptno);
       }

       public string InsertForeClosure(string sp, Hashtable ht)
       {
           Cls_Data_Warehouse obj = new Cls_Data_Warehouse();
           return obj.InsertForeClosure(sp, ht);
       }

       public DataTable GetSTBList(string userid, string TransId)
       {
           Cls_Data_Warehouse obj = new Cls_Data_Warehouse();
           return obj.GetSTBList(userid, TransId);
       }
    }
}
