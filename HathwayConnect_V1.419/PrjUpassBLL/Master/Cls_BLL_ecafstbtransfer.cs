using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PrjUpassDAL.Master;
using System.Data;

namespace PrjUpassBLL.Master
{
    public class Cls_BLL_ecafstbtransfer
    {
        public string InsertDetails(string username, Hashtable ht)
        {
            Cls_Data_ecafstbtransfer obj = new Cls_Data_ecafstbtransfer();
            return obj.InsertDetails(username, ht);
        }
        public string InsertLCOTransfer(string username, Hashtable ht)
        {
            Cls_Data_ecafstbtransfer obj = new Cls_Data_ecafstbtransfer();
            return obj.InsertLCOTransfer(username, ht);
        }
        public string InsertAdminTransfer(string username, Hashtable ht)
        {
            Cls_Data_ecafstbtransfer obj = new Cls_Data_ecafstbtransfer();
            return obj.InsertAdminTransfer(username, ht);
        }
        public string InsertLCOConfig(string username, Hashtable ht)
        {
            Cls_Data_ecafstbtransfer obj = new Cls_Data_ecafstbtransfer();
            return obj.InsertLCOConfig(username, ht);
        }

        public DataTable GetAdminRejectedList(string username, Hashtable ht)
        {
            Cls_Data_ecafstbtransfer obj = new Cls_Data_ecafstbtransfer();
            return obj.GetAdminRejectedList(username, ht);
        }

        public DataTable STBTransferList(string username, Hashtable ht)
        {
            Cls_Data_ecafstbtransfer obj = new Cls_Data_ecafstbtransfer();
            return obj.STBTransferList(username, ht);
        }
    }
}
