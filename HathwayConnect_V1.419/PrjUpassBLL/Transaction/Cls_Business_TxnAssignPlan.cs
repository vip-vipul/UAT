using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjUpassDAL.Transaction;
using System.Data;
using System.Collections;

namespace PrjUpassBLL.Transaction
{
    public class Cls_Business_TxnAssignPlan
    {


        public string InsertDiscount(Hashtable htAddPlanParams, string username, string operid, string catid)
        {
            Cls_Data_TxnAssignPlan objAsPl1 = new Cls_Data_TxnAssignPlan();
            return objAsPl1.InsertDiscount(htAddPlanParams, username, operid, catid);
        }


        public DataTable GetPackagedef(string username)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.GetPackagedef(username);
        }

        public string getDistributorDetails(string username, string oper_id) { 
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.getDistData(username, oper_id); 
        }

        public string getDistBalance(string username) { 
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.getDistBalance(username); 
        }

        public string serviceStatusUpdateBLL(Hashtable ht)
        {
            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            return obj.serviceStatusUpdateDAL(ht);
        }    

        public string ValidateProvTrans(Hashtable ht)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.ValidateProvTrans(ht);
        }

        public string ValidateProvTransFoc2(Hashtable ht)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.ValidateProvTransFoc2(ht);
        }

        public string ProvTransResEcaf(Hashtable ht)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.ProvTransResEcaf(ht);
        }

        public string ProvTransRes(Hashtable ht)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.ProvTransRes(ht);
        }

        public string ProvECS(Hashtable ht)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.ProvECS(ht);
        }

        public string ProvECSSingle(Hashtable ht)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.ProvECSSingle(ht);
        }

        //public void GetUserCity(string username, out String cityid, out String dasarea)
        //{
        //    cityid = "";
        //    dasarea = "";
        //    Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
        //    objAsPl.GetuserCity(username, out cityid, out dasarea);
        //}

        public void GetUserCity(string username, out String cityid, out String dasarea, out String operid, out string JVNO, out string Flag, out string StateName)
        {
            cityid = "";
            dasarea = "";
            operid = "";
            Flag = "";
            JVNO = "";
            StateName = "";
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            objAsPl.GetuserCity(username, out cityid, out dasarea, out operid, out JVNO, out Flag, out StateName);
        }


        public string GetCityFromBrmPoid(string username, string lcobrmpoid)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.GetCityFromBrmPoid(username, lcobrmpoid);
        }
        
            public string getServiceStatus(string status)
        {
            if (status == "10100")
            {
                return "A";
            }
            else if (status == "10102")
            {
                return "IA";
            }
            else if (status == "10103")
            {
                return "CL";
            }
            else
            {
                return "Ex";
            }
        }


public string getServiceDataBL(string username, string city_id, string service_str, string AccountNO)
        {
            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            return obj.getServiceDataDL(username, city_id, service_str, AccountNO);
        }


        public DataTable getProviReasons(string username, string reason_type_flag) {
            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            return obj.getProviReasons(username, reason_type_flag);
        }

        public string getProviConfirm(Hashtable htData)
        {
            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            return obj.getProviConfirm(htData);
        }

        //--------------------OLD METHODS--------------------

        public DataTable getPlanData(string username, List<string> plan_ids) {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.getPlanData(username, plan_ids);
        }

        public DataTable getExistingPlanDataBll(string username, List<string> plan_ids)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.getExistingPlanData(username, plan_ids);
        }

        public string SetPlansBLL(Hashtable ht) {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.SetPlans(ht);
        }

        public DataTable getCustLastTrans(string username, string cust_id, string rownum)
        {
            Cls_Data_TxnAssignPlan obj1 = new Cls_Data_TxnAssignPlan();
            return obj1.getCustLastTrans(username, cust_id, rownum);
        }

        public string autorenewstatus(string username,string vcid,string customerid,string planname)
        {
            Cls_Data_TxnAssignPlan objstatus= new Cls_Data_TxnAssignPlan();
            return objstatus.autorenewstatus(username, vcid, customerid, planname);
        }
        public string autorenewstatuslco(string username)
        {
            Cls_Data_TxnAssignPlan objstatus = new Cls_Data_TxnAssignPlan();
            return objstatus.autorenewstatuslco(username);
        }
        

        public string getRefund(Hashtable htData)
        {
            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            return obj.getRefund(htData);
        }

        public string getFreePlanDetails(string username, string BasicPlanPoidId, string FocPackPoId)
        {
            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            return obj.getFreePlanDetails(username, BasicPlanPoidId, FocPackPoId);
        }

        public string getFreePlanDetails2(string username, string BasicPlanPoidId, string FocPackPoId, string lang)
        {
            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            return obj.getFreePlanDetails2(username, BasicPlanPoidId, FocPackPoId, lang);
        }

        public string Funaddplanbalancecheck(string username, string oper_id, string Newplan_Poid)
        {
           
            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            return obj.Funaddplanbalancecheck(username, oper_id, Newplan_Poid);
        }
        public string saveModifiesCust(string username, string account, string firstname, string middlename, string lastname, string email, string mobile, string address, string term, string lcocode, string firstname_old, string middlename_old, string lastname_old, string email_old, string mobile_old, string address_old)
        {

            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            return obj.saveModifiesCust(username, account, firstname, middlename, lastname, email, mobile, address, term, lcocode, firstname_old, middlename_old, lastname_old, email_old, mobile_old, address_old);
        }

        public string getvcwise_planstr(string username, string planstr, string planpoid) //Added by Vivek Singh on 12-Jul-2016
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.getvcwise_planstr(username, planstr, planpoid);
        }

        public string getvcwise_spl_planstr(string username, string planstr) //Added by Vivek Singh on 14-Jul-2016
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.getvcwise_planstr_specialplan(username, planstr);
        }
        public string Check_NFCPlan(string username, string PlanList, string Cityid, string AccountNo, string operid, string NewPlanPOIDs, string Flag, string VCID)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.Check_NFCPlan(username, PlanList, Cityid, AccountNo, operid, NewPlanPOIDs, Flag, VCID);
        }

        public DataTable getNCFPlanDetails(string username, string PlanPoids, string JVFlag, string Cityid, string DASArea, string OperID, string JVNO)
        {
            Cls_Data_TxnAssignPlan obj1 = new Cls_Data_TxnAssignPlan();
            return obj1.getNCFPlanDetails(username, PlanPoids, JVFlag, Cityid, DASArea, OperID, JVNO);
        }

        public string quickpayurl(string username,string  AccountNo,string  MobileNo,string  finalURL)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.quickpayurl(username, AccountNo, MobileNo, finalURL);
        }

        //----------------
        public string ChannelCount(string username, string PlanPoid)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.ChannelCount( username,  PlanPoid);
        }

        public string LCODet(string username)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.LCODet(username);
        }
        //------
        public string getCancelServiceDataBL(string username, string city_id, string service_str, string AccountNO)
        {
            Cls_Data_TxnAssignPlan obj = new Cls_Data_TxnAssignPlan();
            return obj.getCancelServiceDataDL(username, city_id, service_str, AccountNO);
        }
        //--
        public string ProvChangeTransRes(Hashtable ht)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.ProvChangeTransRes(ht);
        }

        public string AlignPlan(string username, string PlanList, string Cityid, string AccountNo, string operid, string NewPlanPOIDs, string Flag, string VCID)
        {
            Cls_Data_TxnAssignPlan objAsPl = new Cls_Data_TxnAssignPlan();
            return objAsPl.AlignPlan(username, PlanList, Cityid, AccountNo, operid, NewPlanPOIDs, Flag, VCID);
        }
    }
}
