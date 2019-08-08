using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
    public class Cls_BLL_NEWSTB
    {
        public string[] getLcodetails(string username, string LcoName, string type, string operid, string catid)
        {
            Cls_Data_NewSTB obj = new Cls_Data_NewSTB();
            return obj.GetLcoDetails(username, LcoName, type, operid, catid);
        }
        /*-------------------------RP---------------*/
        public string[] getschemeDetails(string SchemeID, string username)
        {
            Cls_Data_NewSTB obj = new Cls_Data_NewSTB();
            return obj.GetSchemDetails(SchemeID, username);
        }
        /*--------------------------PP---------------*/
        public string InsertNewSTBPP(string sp, Hashtable ht)
        {
            Cls_Data_NewSTB obj = new Cls_Data_NewSTB();
            return obj.InsertNewSTBPP(sp, ht);
        }
        /*--------------------------SP---------------*/
        public string InsertNewSTBSP(string sp, Hashtable ht)
        {
            Cls_Data_NewSTB obj = new Cls_Data_NewSTB();
            return obj.InsertNewSTBSP(sp, ht);
        }

        /*------------- RP on 07.09.2017 -------*/
        public string[] BindschemeDetails(string SchemeID, string userName)
        {
            Cls_Data_NewSTB obj = new Cls_Data_NewSTB();
            return obj.BindSchemDetails(SchemeID, userName);
        }
    }
}
