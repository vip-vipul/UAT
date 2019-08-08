using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using PrjUpassDAL.Reports;

namespace PrjUpassBLL.Reports
{
    public class Cls_Business_RptAddPlan
    {
        public Hashtable GetTransations(Hashtable htAddPlanParams, string username, string catid, string operator_id)
        {
            string whereString = "";
            string from = htAddPlanParams["from"].ToString();
            string to = htAddPlanParams["to"].ToString();
            string msoid = htAddPlanParams["msoid"].ToString();
            string showall = htAddPlanParams["showall"].ToString();
            string tablemonth = htAddPlanParams["tablemonth"].ToString(); 
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
            //if (catid == "2")
            //{
            
            //}
            //else if (catid == "5")
            //{
            //    whereString += "  and DISTID= '" + operator_id + "' ";
            //}
            //if (catid == "3")
            //{
            //    whereString += "  and OPERID= '" + operator_id + "' ";
            //}
            //else
            //{
               if (catid == "11")
            {
                whereString += "  and clustid= '" + operator_id + "' ";
            }
            else
            {
                whereString += "  and PARENTID= '" + msoid + "' ";
            }
               if (tablemonth != "")
               {
                   tablemonth = "AOUP_LCOPRE_CUST_TRANS_DET" + tablemonth;
               }
               else
               {
                   tablemonth = "AOUP_LCOPRE_CUST_TRANS_DET";
               }
           // }
            //else if (catid == "10")
            //{
            //    whereString += "  and hoid= '" + operator_id + "' ";
            //}
            /*else
            {
            }*/

            Cls_Data_RptAddPlan objDataRptAddPlan = new Cls_Data_RptAddPlan();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataRptAddPlan.GetTransations(whereString, username, tablemonth));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }

        public Hashtable GetTransationsMSO(Hashtable htAddPlanParams, string username, string catid, string operator_id)
        {
            string whereString = "";
            string from = htAddPlanParams["from"].ToString();
            string to = htAddPlanParams["to"].ToString();
            string Transactiontype = htAddPlanParams["Transactiontype"].ToString();
            string Payterm = htAddPlanParams["Payterm"].ToString();
            string plantype = htAddPlanParams["plantype"].ToString();
            string tablemonth = htAddPlanParams["tablemonth"].ToString(); 
            string searchParamStr = "";

            if (from != null && from != "")
            {
                whereString += " dt >=  '" + from + "' ";
                searchParamStr += " <b>Transaction From : </b> " + from;
            }
            if (to != null && to != "")
            {
                whereString += " and dt <=  '" + to + "' ";
                searchParamStr += " <b>Transaction To : </b> " + to;
            }
            if (Transactiontype != "All")
            {
                whereString += " and planflag in('" + Transactiontype + "') ";
            }

            if (plantype != "All")
            {
                whereString += " and PlanType =  '" + plantype + "' ";
            }

            if (Payterm != "All")
            {
                whereString += " and PAYTERM =  '" + Payterm + "' ";
            }
            if (tablemonth != "")
            {
                tablemonth = "AOUP_LCOPRE_CUST_TRANS_DET" + tablemonth;
            }
            else
            {
                tablemonth = "AOUP_LCOPRE_CUST_TRANS_DET";
            }
            //if (catid == "2")
            //{
            //    whereString += "  and PARENTID= '" + operator_id + "' ";
            //}
            //else if (catid == "5")
            //{
            //    whereString += "  and DISTID= '" + operator_id + "' ";
            //}
            /*else if (catid == "3")
            {
                whereString += "  and OPERID= '" + operator_id + "' ";
            }*/
            if (catid == "10")
            {
                whereString += " and hoid= '" + operator_id + "' ";
            }
            /*else
            {
            }*/

            Cls_Data_RptAddPlan objDataRptAddPlan = new Cls_Data_RptAddPlan();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataRptAddPlan.GetTransationsMSO(whereString, username, tablemonth));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }

        public Hashtable GetTransationsU(Hashtable htAddPlanParams1, string username1, string catid1, string operator_id1)
        {
            string whereString = "";
            string from = htAddPlanParams1["from"].ToString();
            string to = htAddPlanParams1["to"].ToString();
            string lcoid = htAddPlanParams1["lcoid"].ToString();
             string search_type = htAddPlanParams1["search"].ToString().Trim();
            string txtsearch = htAddPlanParams1["txtsearch"].ToString().Trim();
            string showall = htAddPlanParams1["showall"].ToString();
            string Plantype = htAddPlanParams1["Plantype"].ToString().Trim();
            string Transtype = htAddPlanParams1["Transtype"].ToString();
            string Payterm = htAddPlanParams1["Payterm"].ToString();
            string tablemonth = htAddPlanParams1["tablemonth"].ToString(); 
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
            if (Transtype != "All" && Transtype!="")
            {
                whereString += " and planflag in('" + Transtype + "') ";
            }

            if (Plantype != "All" && Plantype!="")
            {
                whereString += " and PlanType =  '" + Plantype + "' ";
            }

            if (Payterm != "All" && Payterm!="")
            {
                whereString += " and PAYTERM =  '" + Payterm + "' ";
            }
            if (catid1 == "3")
            {
                whereString += "  and OPERID= '" + operator_id1 + "' ";
                if (txtsearch.Length > 0)
                {
                    if (search_type == "0")
                    {
                        whereString += @" and upper(""Account No"")= upper('" + txtsearch + "')";
                    }
                    else if (search_type == "1")
                    {
                        whereString += @" and upper(""VC/MAC Id"")= upper('" + txtsearch + "')";
                    }
                    else if (search_type == "2")
                    {
                        whereString += @" and upper(""Lco Code"")= upper('" + txtsearch + "')";
                    }
                }
            }
            else
            {
                if (lcoid != null && lcoid != "" && showall != "0" && showall !="1")
                {
                    whereString += " and lcoid =  '" + lcoid + "' ";
                }               
            }
            if (showall != null && showall != "")
            {
                whereString += "  ";
            }
            if (tablemonth != "")
            {
                tablemonth = "AOUP_LCOPRE_CUST_TRANS_DET" + tablemonth;
            }
            else
            {
                tablemonth = "AOUP_LCOPRE_CUST_TRANS_DET";
            }
            Cls_Data_RptAddPlan objDataRptAddPlan = new Cls_Data_RptAddPlan();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataRptAddPlan.GetTransationsU(whereString, username1, tablemonth));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }

        public Hashtable GetTransationsDet(Hashtable htAddPlanParams2, string username2)
        {
            string whereString = "";
            string from = htAddPlanParams2["from"].ToString();
            string to = htAddPlanParams2["to"].ToString();
            string uid = htAddPlanParams2["uid"].ToString();
            string loperid = htAddPlanParams2["loperid"].ToString();
            string parentid = htAddPlanParams2["parentid"].ToString();
          //  string Msoid = htAddPlanParams2["msoperid"].ToString();
            string search_type = htAddPlanParams2["search"].ToString().Trim();
            string txtsearch = htAddPlanParams2["txtsearch"].ToString().Trim();
            string showall = htAddPlanParams2["showall"].ToString();
            string Plantype = htAddPlanParams2["Plantype"].ToString().Trim();
            string Transtype = htAddPlanParams2["Transtype"].ToString();
            string Payterm = htAddPlanParams2["Payterm"].ToString();
            string tablemonth = htAddPlanParams2["tablemonth"].ToString(); 
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
            if (uid != null && uid != "" && showall!="0" && showall !="1")
            {
                whereString += " and uname =  '" + uid + "' ";
            }
            if (showall == "1")
            {
                whereString += " and parentid='" + parentid.ToString() + "'";
            }
            if (showall == "2")
            {
                whereString += " and loperid =  '" + loperid + "' ";
               
            }

            if (Transtype != "All" && Transtype != "")
            {
                whereString += " and flag in('" + Transtype + "') ";
            }

            if (Plantype != "All" && Plantype != "")
            {
                whereString += " and plntyp =  '" + Plantype + "' ";
            }

            if (Payterm != "All" && Payterm != "")
            {
                whereString += " and payterm =  '" + Payterm + "' ";
            }

            if (showall != null && showall != "")               
            {
                whereString += "  ";
            }


            if (txtsearch.Length > 0)
            {
                if (search_type == "0")
                {
                    whereString += @" and upper(""Account No"")= upper('" + txtsearch + "')";
                }
                else if (search_type == "1")
                {
                    whereString += @" and upper(vc)= upper('" + txtsearch + "')";
                }
                else if (search_type == "2")
                {
                    whereString += @" and upper(""Lco Code"")= upper('" + txtsearch + "')";
                }
            }
            if (tablemonth != "")
            {
                tablemonth = "AOUP_LCOPRE_CUST_TRANS_DET" + tablemonth;
            }
            else
            {
                tablemonth = "AOUP_LCOPRE_CUST_TRANS_DET";
            }

            Cls_Data_RptAddPlan objDataRptAddPlan = new Cls_Data_RptAddPlan();
            Hashtable htResponse = new Hashtable();
            htResponse.Add("htResponse", objDataRptAddPlan.GetTransationsDet(whereString, username2, tablemonth));
            htResponse.Add("ParamStr", searchParamStr);
            return htResponse;
        }
    }
}
