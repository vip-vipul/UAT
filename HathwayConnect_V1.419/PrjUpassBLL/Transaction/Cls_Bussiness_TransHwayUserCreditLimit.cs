using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Collections;
using System.Data;


namespace PrjUpassBLL.Transaction
{
    public class Cls_Bussiness_TransHwayUserCreditLimit
    {
        public string UserLimitRev(Hashtable ht)
        {
            Cls_Data_TransHwayUserCreditLimit obj = new Cls_Data_TransHwayUserCreditLimit();
            return obj.UserLimitRevarsal(ht);
        }
        public string[] GetUserDetails(string username,string category, string type, string operid, string searchId)
        {
            Cls_Data_TransHwayUserCreditLimit obj = new Cls_Data_TransHwayUserCreditLimit();
            return obj.GetUserDetails(username,category, type, operid, searchId);
        }

        //---------------------------new page-----------------------------------------
        public DataTable GetAllUserDetails(string username, string category, string operid)
        {
            Cls_Data_TransHwayUserCreditLimit obj = new Cls_Data_TransHwayUserCreditLimit();
            return obj.GetAllUsers(username, category, operid);
        }

        public string[] GetAvailBal(string username, string category, string userid, string operid)
        {
            Cls_Data_TransHwayUserCreditLimit obj = new Cls_Data_TransHwayUserCreditLimit();
            
            return obj.GetAvailBal(username, category, userid, operid);
        }

        public string setCredits(string username, string input_str, string oper_id, string IP)
        {
            Cls_Data_TransHwayUserCreditLimit obj = new Cls_Data_TransHwayUserCreditLimit();
            return obj.SetLimits(username, input_str, oper_id, IP);
        }

        public string userblkUnblock(string username, Hashtable ht)
        {
            Cls_Data_TransHwayUserCreditLimit obj = new Cls_Data_TransHwayUserCreditLimit();

            return obj.userBlockUnblock(username, ht);

        }
    }
}
