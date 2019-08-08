using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;

namespace PrjUpassBLL.Transaction
{
   public class Cls_BLL_TransOsdBmailNotification
    {
       public string SaveOsdBMail(string username, string data, string dtfrom, string dtto)
        {
            Cls_Data_TransOsdBmailNotification obj = new Cls_Data_TransOsdBmailNotification();
            return obj.SaveOsdBMail(username, data, dtfrom, dtto);
        }

       public string SaveOsdBMailActivation(string username, string reference, string data)
       {
           Cls_Data_TransOsdBmailNotification obj = new Cls_Data_TransOsdBmailNotification();
           return obj.SaveOsdBMailActivation(username, reference, data);
       }

       public string SaveOsdBMailbULK(string username)
       {
           Cls_Data_TransOsdBmailNotification obj = new Cls_Data_TransOsdBmailNotification();
           return obj.SaveOsdBMailbULK(username);
       }

    }
}
