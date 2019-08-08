using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Master;
using System.Collections;

namespace PrjUpassBLL.Master
{
    public class Cls_Business_MstHwaymsgBrodcaster
    {
        public string SetBrodcastermsg(string username, Hashtable ht)
        {
            Cls_Data_MstHwaymsgBrodcaster obj = new Cls_Data_MstHwaymsgBrodcaster();
            return obj.SetBrodcastermsg(username, ht);
        }
        public string SetLCOBrodcastMsg(string username, Hashtable ht)
        {
            Cls_Data_MstHwaymsgBrodcaster obj = new Cls_Data_MstHwaymsgBrodcaster();
            return obj.SetLCOBrodcasterMsg(username, ht);
        }
        public string GetLCOBrodcastMsg(string username, string oper_id)
        {
            Cls_Data_MstHwaymsgBrodcaster obj = new Cls_Data_MstHwaymsgBrodcaster();
            return obj.GetLCOBrodcasterMsg(username, oper_id);
        }

        public DataTable getmiadata(string username, string lcocode)
        {
            Cls_Data_MstHwaymsgBrodcaster obj1 = new Cls_Data_MstHwaymsgBrodcaster();
            return obj1.getMIAdata(username, lcocode);
        }
    }
}
