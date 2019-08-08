using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;
namespace PrjUpassBLL.Transaction
{
   public class Cls_Business_UserAccountMap
    {
       public string UserAccountMap(string username, string IPAdd, string LcoCode, string AccountNo, string ActiveFlag, string Type)
       {
           Cls_Data_UserAccountMap obj = new Cls_Data_UserAccountMap();
           return obj.UserAccountMap(username, IPAdd, LcoCode, AccountNo, ActiveFlag, Type);
       }

       public Hashtable UserAccApp_Details(Hashtable Dt, string username,string userID)
       {
           Hashtable dt = new Hashtable();
           string strfromdate = Dt["from"].ToString();
           string strtodate = Dt["to"].ToString();
           string strFlag = Dt["Flag"].ToString();

           string str = "select * from View_user_acc_map where trunc(instdt)>='" + strfromdate + "' and trunc(instdt)<='" + strtodate + "' ";
           if (strFlag!="0")
           {
               str += " and Flag='"+strFlag+"'";
           }
           str += " and username='" + userID + "'";
           Cls_Data_UserAccountMap obj = new Cls_Data_UserAccountMap();
           dt.Add("htResponse", obj.userAccMap_Det(str, username));
           return dt;
       }

       public Hashtable UserAcc_Details(Hashtable Dt, string username)
       {
           Hashtable dt = new Hashtable();
           string strfromdate = Dt["from"].ToString();
           string strtodate = Dt["to"].ToString();
           string strFlag = Dt["Flag"].ToString();
           string UserID = Dt["UserID"].ToString();
           string str = "select a.username,fname||' '||mname||' '||lname Name,addr,email,mobno,count(accountno) Total from View_user_acc_map a left join view_Lcopre_user_det b on b.username=a.username where trunc(instdt)>='" + strfromdate + "' and trunc(instdt)<='" + strtodate + "' ";
           if (strFlag != "0")
           {
               str += " and a.Flag='" + strFlag + "'";
           }
           if (UserID != "All")
           {
               str += " and a.username='" + UserID + "'";
           }
           str += " group by a.username,fname,mname,lname,addr,mobno,email";
           Cls_Data_UserAccountMap obj = new Cls_Data_UserAccountMap();
           dt.Add("htResponse", obj.userAccMap_Det(str, username));
           return dt;
       }

    }
}
