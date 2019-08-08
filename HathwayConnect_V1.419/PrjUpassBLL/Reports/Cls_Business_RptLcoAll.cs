using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_RptLcoAll
    {
        public string[] getLcoDatadetails(string username, string LcoName, string type, string operid, string catid)
        {
            Cls_Data_RptLcoAll obj = new Cls_Data_RptLcoAll();
            return obj.GetLcoDetails(username, LcoName, type, operid, catid);
        }

        public string[] getLcoDataById(string username, string operid)
        {
            Cls_Data_RptLcoAll obj = new Cls_Data_RptLcoAll();
            return obj.GetLcoDataById(username, operid);
        }

        public Hashtable GetTransationsLcoDet(Hashtable htLcoAllParams, string username, string catid, string operator_id)
        {
            string whereString = "";
            string lcd = htLcoAllParams["lcd"].ToString();
            string lnm = htLcoAllParams["lnm"].ToString();
            string txtsear = htLcoAllParams["txtsear"].ToString();
            string searchParamStr = "";

            if (lcd == "0")
            {
                whereString += "  UPPER(lcocode) LIKE UPPER('" + txtsear.ToString().Trim() + "') ";
            }
            if (lnm == "1")
            {
                whereString += "   UPPER(lconame) LIKE UPPER('%" + txtsear.ToString().Trim() + "') ";
            }
            if (catid == "2")
            {
                whereString += "  and PARENTID= '" + operator_id + "' ";
            }
            else if (catid == "5")
            {
                whereString += "  and DISTID= '" + operator_id + "' ";
            }
            else if (catid == "3"  || catid=="11")
            {
                whereString += "  and OPERID= '" + operator_id + "' ";
            }
            else if (catid == "10")
            {
                whereString += "  and hoid= '" + operator_id + "' ";
            }
            else
            {
            }

            Cls_Data_RptLcoAll objDataRptLcoAll = new Cls_Data_RptLcoAll();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataRptLcoAll.GetTransationsLcoDets(whereString, username));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }

        public Hashtable GetTransationsLastF(Hashtable htLcoAllParams1, string username1, string catid1, string operator_id1)
        {
            string whereString = "";
            string lcd = htLcoAllParams1["lcd"].ToString();
            string lnm = htLcoAllParams1["lnm"].ToString();
            string txtsear = htLcoAllParams1["txtsear"].ToString();
            string searchParamStr = "";

            if (lcd == "0")
            {
                whereString += "  UPPER(lcocode) LIKE UPPER('" + txtsear.ToString().Trim() + "') ";
            }
            if (lnm == "1")
            {
                whereString += "   UPPER(lconame) LIKE UPPER('" + txtsear.ToString().Trim() + "') ";
            }
            if (catid1 == "2")
            {
                whereString += "  and PARENTID= '" + operator_id1 + "' ";
            }
            else if (catid1 == "5")
            {
                whereString += "  and DISTID= '" + operator_id1 + "' ";
            }
            else if (catid1 == "3")
            {
                whereString += "  and OPERID= '" + operator_id1 + "' ";
            }
            else if (catid1 == "10")
            {
                whereString += "  and hoid= '" + operator_id1 + "' ";
            }
            else
            {
            }

            Cls_Data_RptLcoAll objDataRptLcoAll = new Cls_Data_RptLcoAll();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse1", objDataRptLcoAll.GetTransationsLastFDets(whereString, username1));
            htResponse.Add("ParamStr1", searchParamStr);
            return htResponse;
        }

        public Hashtable GetTransationsTop(Hashtable htLcoAllParams2, string username2, string catid2, string operator_id2)
        {
            string whereString = "";
            string lcd = htLcoAllParams2["lcd"].ToString();
            string lnm = htLcoAllParams2["lnm"].ToString();
            string txtsear = htLcoAllParams2["txtsear"].ToString();
            string searchParamStr = "";

            if (lcd == "0")
            {
                whereString += "  UPPER(lcocode) LIKE UPPER('" + txtsear.ToString().Trim() + "') ";
            }
            if (lnm == "1")
            {
                whereString += "   UPPER(lconame) LIKE UPPER('" + txtsear.ToString().Trim() + "') ";
            }
            //if (catid2 == "2")
            //{
            //    whereString += "  and PARENTID= '" + operator_id2 + "' ";
            //}
            //else if (catid2 == "5")
            //{
            //    whereString += "  and DISTID= '" + operator_id2 + "' ";
            //}
            //else if (catid2 == "3")
            //{
              whereString += "  and OPERID= '" + operator_id2 + "' ";
            //}
            //else if (catid2 == "10")
            //{
            //    whereString += "  and hoid= '" + operator_id2 + "' ";
            //}
            //else
            //{
            //}

            Cls_Data_RptLcoAll objDataRptLcoAll = new Cls_Data_RptLcoAll();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse2", objDataRptLcoAll.GetTransationsTopDets(whereString, username2));
            htResponse.Add("ParamStr2", searchParamStr);
            return htResponse;
        }

        public Hashtable GetTransationsRevrs(Hashtable htLcoAllParams3, string username3, string catid3, string operator_id3)
        {
            string whereString = "";
            string lcd = htLcoAllParams3["lcd"].ToString();
            string lnm = htLcoAllParams3["lnm"].ToString();
            string txtsear = htLcoAllParams3["txtsear"].ToString();
            string searchParamStr = "";

            if (lcd == "0")
            {
                whereString += "  UPPER(lcocode) LIKE UPPER('" + txtsear.ToString().Trim() + "') ";
            }
            if (lnm == "1")
            {
                whereString += "   UPPER(lconame) LIKE UPPER('%" + txtsear.ToString().Trim() + "') ";
            }
            //if (catid3 == "2")
            //{
            //    whereString += "  and PARENTID= '" + operator_id3 + "' ";
            //}
            //else if (catid3 == "5")
            //{
            //    whereString += "  and DISTID= '" + operator_id3 + "' ";
            //}
            //else if (catid3 == "3")
            //{
               whereString += "  and OPERID= '" + operator_id3 + "' ";
            //}
            //else if (catid3 == "10")
            //{
            //    whereString += "  and hoid= '" + operator_id3 + "' ";
            //}
           

            Cls_Data_RptLcoAll objDataRptLcoAll = new Cls_Data_RptLcoAll();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse3", objDataRptLcoAll.GetTransationsReversDets(whereString, username3));
            htResponse.Add("ParamStr3", searchParamStr);
            return htResponse;
        }

        public DataTable GetLedgerData(string _lcoid, string username, string catid, string operator_id)
        {
            string whereString = "";
            string from = DateTime.Today.ToString("dd-MMM-yyyy");
            string to = DateTime.Today.ToString("dd-MMM-yyyy");
            string lcoid = _lcoid;

            if (from != null && from != "")
            {
                whereString += "  dt >=  '" + from + "' ";
            }
            if (to != null && to != "")
            {
                whereString += " and dt <=  '" + to + "' ";
            }
            if (lcoid != null && lcoid != "")
            {
                whereString += " and lcoid =  '" + lcoid + "' ";
            }
            //if (catid == "2")
            //{
            //    whereString += "  and PARENTID= '" + operator_id + "' ";
            //}
            //else if (catid == "5")
            //{
            //    whereString += "  and DISTID= '" + operator_id + "' ";
            //}
            //else if (catid == "3")
            //{
              whereString += "  and OPERID= '" + operator_id + "' ";
            //}
            //else if (catid == "10")
            //{
            //    whereString += "  and hoid= '" + operator_id + "' ";
            //}

            Cls_Data_RptLcoAll objDataRptAddPlan = new Cls_Data_RptLcoAll();
            return objDataRptAddPlan.GetPartyLedger(whereString, username);
        }

        public DataTable GetLedgerDet(string _lcoid, string username2)
        {
            string whereString = "";
            string from = DateTime.Today.ToString("dd-MMM-yyyy");
            string to = DateTime.Today.ToString("dd-MMM-yyyy");
            string lcoid = _lcoid;

            if (from != null && from != "")
            {
                whereString += "  dt >=  '" + from + "' ";
            }
            if (to != null && to != "")
            {
                whereString += " and dt <=  '" + to + "' ";
            }
            if (lcoid != null && lcoid != "")
            {
                whereString += " and lcoid =  '" + lcoid + "' ";
            }
            Cls_Data_RptLcoAll objDataRptAddPlan = new Cls_Data_RptLcoAll();
            return objDataRptAddPlan.GetPartyLedgerDet(whereString, username2);
        }

        public DataTable GetServiceData(string username, string catid, string operid)
        {
            Cls_Data_RptLcoAll objDataRptAddPlan = new Cls_Data_RptLcoAll();
            return objDataRptAddPlan.GetServiceData(username, catid, operid);
        }

        public DataTable GetUserDet(string username, string catid, string operid)
        {
            Cls_Data_RptLcoAll objDataRptAddPlan = new Cls_Data_RptLcoAll();
            return objDataRptAddPlan.GetUserDet(username, operid);
        }
    }
}
