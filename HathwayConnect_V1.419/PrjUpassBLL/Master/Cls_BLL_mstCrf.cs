using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Master;
using System.Collections;

namespace PrjUpassBLL.Master
{
   public class Cls_BLL_mstCrf
    {

       public string FetchCustomerDetail(String username,String Accno,String SearchFlag)
       {
           Cls_Data_mstCrf objcrf = new Cls_Data_mstCrf();
           return objcrf.FetchCustomerDetail(username, Accno, SearchFlag);
       }

       public string getIDdata(string username)
       {
           Cls_Data_mstCrf ob = new Cls_Data_mstCrf();
           string IDdetails = ob.getDocDetails(username);
           return IDdetails;

       }

       public string GetAdddeatils(string username, String selectionfalg, string Entity)
       {
           Cls_Data_mstCrf obj = new Cls_Data_mstCrf();
           return obj.GetAdddeatils(username, selectionfalg, Entity);
       }

       public string CheckBasicPlanType(string username, String planpoid)
       {
           Cls_Data_mstCrf obj = new Cls_Data_mstCrf();
           return obj.CheckBasicPlanType(username, planpoid);
       }



       public string getOtpNewCust(string username, string mobile)
       {
          
           Cls_Data_mstCrf ob1 = new Cls_Data_mstCrf();
           return ob1.genOTPNEwCust(username, mobile);

       }

       public string ValidOtpNewCust(string username, string otpid, string otp)
       {
           Cls_Data_mstCrf ob2 = new Cls_Data_mstCrf();
           return ob2.ValidateOTPNEwCust(username, otpid, otp);

       }

       public string Validatenewcust(string username, string STB, string VC)
       {
           Cls_Data_mstCrf ob2 = new Cls_Data_mstCrf();
           return ob2.Validatenewcust(username, STB, VC);

       }

       public string getBasePlanDetails(string username, String TVType, String SDHD,int Payterm,string VCNO,string STBNO)
       {
           Cls_Data_mstCrf obj = new Cls_Data_mstCrf();
           return obj.GetPlanDetails(username, TVType, SDHD, Payterm, VCNO, STBNO);
       }

       public string insrtCrfdata(string username, Hashtable ht)
       {
           Cls_Data_mstCrf objdata = new Cls_Data_mstCrf();
           return objdata.InsertCRFData(username, ht);


       }


       public string GetLcoCode(string Operid)
       {
           Cls_Data_mstCrf ob2 = new Cls_Data_mstCrf();
           return ob2.GetLcoCode(Operid);

       }

    }
}
