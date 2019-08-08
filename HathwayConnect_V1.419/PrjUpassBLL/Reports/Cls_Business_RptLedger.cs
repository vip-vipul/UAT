using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_RptLedger
    {
        public Hashtable GetTransations(Hashtable htAddPlanParams, string username, string catid, string operator_id)
        {
            string whereString = "";
            string dateString = "";
            string from = htAddPlanParams["from"].ToString();
            string to = htAddPlanParams["to"].ToString();
            string searchParamStr = "";

            if (from != null && from != "")
            {
                dateString += "  dt >=  '" + from + "' ";
                searchParamStr += " <b>Transaction From : </b> " + from;
            }
            if (to != null && to != "")
            {
                dateString += " and dt <=  '" + to + "' ";
                searchParamStr += " <b>Transaction To : </b> " + to;
            }
            if (catid == "2")
            {
                whereString += "  PARENTID= '" + operator_id + "' ";
            }
            else if (catid == "5")
            {
                whereString += "  DISTID= '" + operator_id + "' ";
            }
            else if (catid == "3" || catid == "11")
            {
                whereString += "  OPERID= '" + operator_id + "' ";
            }
            else if (catid == "10")
            {
                whereString += "  hoid= '" + operator_id + "' ";
            }
            else
            {
            }

            Cls_Data_RptLedger objDataRptAddPlan = new Cls_Data_RptLedger();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataRptAddPlan.GetTransations(dateString ,whereString, username));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }

        public Hashtable GetTransationsDet(Hashtable htAddPlanParams2, string username2, string catid, string operid)
        {
            string Showall = htAddPlanParams2["showall"].ToString();
            string whereString = "";
            string from = htAddPlanParams2["from"].ToString();
            string to = htAddPlanParams2["to"].ToString();
            string lcoid = htAddPlanParams2["lcoid"].ToString();
            //string showall = htAddPlanParams2["showall"].ToString();
            string searchParamStr = "";

            if (from != null && from != "")
            {
                whereString += "  dt >=  '" + from + "' ";
                searchParamStr += " <b>Transaction From : </b> " + from;
            }
            if (to != null && to != "")
            {
                whereString += " and dt <=  '" + to + "' ";
                searchParamStr += " <b>Transaction To : </b> " + to;
            }
            if (Showall == "1")
            {

                if (catid == "3")
                {
                    whereString += " and OPERID =  '" + operid + "' ";
                }
                else
                {
                    if (lcoid != null && lcoid != "")
                    {

                        whereString += " and lcoid =  '" + lcoid + "' ";
                    }
                }
            }
            


            /*if (showall != null && showall != "")
            {
                whereString += "  ";
            }*/

            Cls_Data_RptLedger objDataRptAddPlan = new Cls_Data_RptLedger();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataRptAddPlan.GetTransationsDet(whereString, username2));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }

        public Hashtable GetTransationsDetLCO(Hashtable htAddPlanParams1, string username1, string catid1, string operator_id1)
        {
            string showall = htAddPlanParams1["showall"].ToString();
            string whereString = "";
            string from = htAddPlanParams1["from"].ToString();
            string to = htAddPlanParams1["to"].ToString();
            string lcoid = htAddPlanParams1["lcoid"].ToString();
            //string showall = htAddPlanParams2["showall"].ToString();
            string searchParamStr = "";

            if (from != null && from != "")
            {
              //  whereString += "  dt >=  '" + from + "' "; // commented by Kiran
                searchParamStr += " <b>Transaction From : </b> " + from;
            }
            if (to != null && to != "")
            {
                //  whereString += " and dt <=  '" + to + "' "; // commented by Kiran
                searchParamStr += " <b>Transaction To : </b> " + to;
            }
            /*  //comented by Kiran
            if (showall == "1")
            {
                if (catid1 == "3")
                {
                    whereString += " and OPERID =  '" + operator_id1 + "' ";
                }
                else
                {
                    if (lcoid != null && lcoid != "")
                    {
                        whereString += " and lcoid =  '" + lcoid + "' ";
                    }
                }
            }
            else
            {
                if (catid1 == "2")
                {
                    whereString += "  and PARENTID= '" + operator_id1 + "' ";
                }
                else if (catid1 == "5")
                {
                    whereString += "  and DISTID= '" + operator_id1 + "' ";
                }
                //else if (catid1 == "3")
                //{
                //    whereString += "  and OPERID= '" + operator_id1 + "' ";
                //}
                else if (catid1 == "10")
                {
                    whereString += "  and hoid= '" + operator_id1 + "' ";
                }

            }
            */

            Cls_Data_RptLedger objDataRptAddPlan = new Cls_Data_RptLedger();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataRptAddPlan.GetTransationsDetLCO(htAddPlanParams1, whereString, username1));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }
    }
}
