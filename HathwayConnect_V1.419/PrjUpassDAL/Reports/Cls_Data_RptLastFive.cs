using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;
using PrjUpassDAL.Reports;


namespace PrjUpassDAL.Reports
{
    public class Cls_Data_RptLastFive
    {
        public DataSet GetLcoUserDetails(string username, string catid, string operid)
        {
            try
            {

                DataTable dtLco = new DataTable("Lco");

                Cls_Helper objHelper = new Cls_Helper();

                string StrLco = "";
                string StrUser = "";
                if (catid == "2")
                {
                    StrLco += "select a.num_oper_id,a.var_oper_opername from aoup_operator_Def a , aoup_lcopre_lco_det b ";
                    StrLco += " where a.num_oper_parentid='" + operid + "'";
                    StrLco += "  and b.num_lcomst_operid=a.num_oper_id ";
                    StrLco += " and a.num_oper_category=3";
                    StrLco += " and a.var_oper_compcode='HWP' ";
                    StrLco += " order by a.var_oper_opername asc ";
                }
                else if (catid == "5")
                {
                    StrLco = "select a.num_oper_id,a.var_oper_opername from aoup_operator_Def a,aoup_lcopre_lco_det b ";
                    StrLco += " where a.num_oper_distid='" + operid + "'";
                    StrLco += "  and b.num_lcomst_operid=a.num_oper_id ";
                    StrLco += " and a.num_oper_category=3";
                    StrLco += " and a.var_oper_compcode='HWP' ";
                    StrLco += " order by a.var_oper_opername asc ";
                }
                else if (catid == "3")
                {
                    StrLco = "select a.num_oper_id,a.var_oper_opername from aoup_operator_Def a,aoup_lcopre_lco_det b ";
                    StrLco += " where a.num_oper_id='" + operid + "'";
                    StrLco += "  and b.num_lcomst_operid=a.num_oper_id ";
                    StrLco += " and a.num_oper_category=3";
                    StrLco += " and a.var_oper_compcode='HWP' ";
                    StrLco += " order by a.var_oper_opername asc ";
                }
                else if (catid == "10")
                {
                    StrLco = "select a.num_oper_id,a.var_oper_opername from aoup_operator_Def a,aoup_lcopre_lco_det b,aoup_operator_Def ho ";
                    StrLco += " where ho.num_oper_parentid='" + operid + "'";
                    StrLco += "  and b.num_lcomst_operid=a.num_oper_id ";
                    StrLco += " and a.num_oper_parentid=ho.num_oper_id ";
                    StrLco += " and a.num_oper_category=3";
                    StrLco += " and a.var_oper_compcode='HWP' ";
                    StrLco += " order by a.var_oper_opername asc ";
                }

                else
                {
                    StrLco += "select a.num_oper_id,a.var_oper_opername from aoup_operator_Def,aoup_lcopre_lco_det b ";
                    StrLco += " where a.num_oper_category=3";
                    StrLco += "  and b.num_lcomst_operid=a.num_oper_id ";
                    StrLco += " and a.var_oper_compcode='HWP' ";
                    StrLco += " order by a.var_oper_opername asc ";
                }


                dtLco = objHelper.GetDataTable(StrLco);

                DataSet dsCompData = new DataSet();

                dsCompData.Tables.Add(dtLco);

                return dsCompData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_RptLastFive.cs-GetLcoUserDetails");
                return null;
            }
        }
    }
}
