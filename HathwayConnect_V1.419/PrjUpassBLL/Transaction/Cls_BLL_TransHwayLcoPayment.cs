using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
    public class Cls_BLL_TransHwayLcoPayment
    {
        public string[] getLcolist(string username, string prefixText, string type)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.SearchOperators(username, prefixText, type);
        }
        public string[] GetLcopaymentDetails(string username, string LcoName, string catid , string operid,string type,string BankName)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.GetLcopaymentDetails(username, LcoName, catid, operid,type,BankName);
        }       

        public string[] getLcodetails(string username, string LcoName, string type, string operid, string catid)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.GetLcoDetails(username, LcoName, type, operid, catid);
        }

        public string[] getplandetails(string username, string Planname, string plantype, string city, string poids, string operid)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.GetplanDetails(username, Planname, plantype, city, poids, operid);
        }

        public string LcoPayment(Hashtable ht)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.LcoPaymentDetails(ht);
        }
        
        public string LcoPaymentRev(Hashtable ht)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.LcoPaymentRevarsal(ht);
        }
        public string[] GetLcoCashpaymentDetails(string username, string LcoName, string catid, string operid)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.GetLcoCashpaymentDetails(username, LcoName, catid, operid);
        }
        public string LcoCashPaymentRevarsal(Hashtable ht)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.LcoCashPaymentRevarsal(ht);
        }

        public string[] getTaxDetails(string username, string rcpt_no)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.getTaxDetails(username, rcpt_no);
        }
        public Hashtable LcoOnlinePaymentTransID(Hashtable ht)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.LcoOnlinePaymentTransID(ht);
        }


        public string LcoOnlinPayment(Hashtable ht)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.LcoOnlinPayment(ht);
        }
        public string[] getPanDetails(string username, string company)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.getPanDetails(username, company);
        }
        //----- Added By RP on 28.07.2017
        public Hashtable InverntryOnlinePaymentTransID(Hashtable ht)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.InventryOnlinePaymentTransID(ht);
        }
        public string InventryOnlinPayment(Hashtable ht)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.InventryOnlinPayment(ht);
        }
        //-------
        //------------ added By RP on 11/12/2017
        public string[] getPanDetailsPIS(string username, string company)
        {
            Cls_Data_TransHwayLcoPayment obj = new Cls_Data_TransHwayLcoPayment();
            return obj.getPanDetailsPIS(username, company);
        }

        //-------
    }
}
