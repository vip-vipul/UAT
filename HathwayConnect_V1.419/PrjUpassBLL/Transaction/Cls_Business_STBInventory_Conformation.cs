using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;
namespace PrjUpassBLL.Transaction
{
    public class Cls_Business_STBInventory_Conformation
    {

        public DataTable STBInvConfList(string userid)
        {
            Cls_Data_STBInventory_Conformation obj = new Cls_Data_STBInventory_Conformation();
            return obj.STBInvConfList(userid);
        }

        public DataTable STBInvConfDet(string username, string receiptno, string makemodel)
        {
            Cls_Data_STBInventory_Conformation obj = new Cls_Data_STBInventory_Conformation();
            return obj.STBInvConfDet(username, receiptno, makemodel);
        }

        public string InsertSTBInvConf(Hashtable ht)
        {
            Cls_Data_STBInventory_Conformation obj = new Cls_Data_STBInventory_Conformation();
            return obj.InsertSTBInvConf(ht);
        }

        public string UpdateInvIntStatus(string username, string flag,  string transid, string upload_id)
        {
            return Cls_Data_STBInventory_Conformation.UpdateInvIntStatus(username, flag, transid, upload_id);

        }
    }
}
