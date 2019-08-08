using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;

namespace PrjUpassBLL.Transaction
{
    public class Cls_Business_TransHome
    {
        /*
        public string isFirstLogin(string username) { 
            Cls_Data_TransHome obj = new Cls_Data_TransHome();
            return obj.isFirstLogin(username);
        }
         */

        public string MIASTATUSINS(string username, string status, string IP)
        {
            Cls_Data_TransHome obj = new Cls_Data_TransHome();
            return obj.MIASTATUSINS(username, status, IP);
        }  

        public string updatePass(string username, string cur_pass, string new_pass, string IP)
        {
            Cls_Data_TransHome obj = new Cls_Data_TransHome();
            return obj.updatePassword(username, cur_pass, new_pass, IP);
        }
        public void GetBalance(string username, out string Balance, out string InBalance)
        {
            Balance = "";
            InBalance = "";
            Cls_Data_TransHome obj = new Cls_Data_TransHome();
            obj.GetInventryBalance(username, out Balance, out InBalance);
        }
    }
}
