using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;

namespace PrjUpassDAL.Master
{
    public class Cls_Data_MstLCORegistration
    {
        public DataSet getCompanyData(string username, string catid, string operid,string companyname)
        {
            try
            {
                DataTable dtCompany = new DataTable("COMPANY");
                //DataTable dtDist = new DataTable("DISTRIBUTOR");
                //DataTable dtSubDist = new DataTable("SUBDISTRIBUTOR");
                //DataTable dtMso = new DataTable("MSO");
                //DataTable dtDistributor = new DataTable("Dist");
                Cls_Helper objHelper = new Cls_Helper();
                string strCompQry = "SELECT a.num_comp_companyid COMP_ID, a.var_comp_companyname COMP_NAME FROM aoup_lcopre_company_det a where a.var_comp_companyname='"+companyname+"'";
                //string strDistQry = "SELECT a.num_comp_distid DIST_ID, a.var_comp_distname DIST_NAME FROM aoup_lcopre_company_det a";
                //string strSubDistQry = "SELECT a.num_comp_subdistid SUBDIST_ID, a.var_comp_subdistname SUBDIST_NAME FROM aoup_lcopre_company_det a";
                //string strMso = "";
                //string strDist = "";
                //if (catid == "2")
                //{
                //    strMso += "select num_oper_id,var_oper_opername from aoup_operator_Def ";
                //    strMso += " where num_oper_id='" + operid + "'";
                //    strMso += " and num_oper_category=2";
                //    strMso += " order by var_oper_opername asc ";


                //    strDist = "select a.num_oper_id,a.var_oper_opername from aoup_operator_Def a,aoup_operator_Def b";
                //    strDist += " where b.num_oper_id='" + operid + "'";
                //    strDist += " and a.num_oper_category=5";
                //    strDist += " and a.num_oper_parentid=b.num_oper_id";
                //    strDist += " order by var_oper_opername asc ";

                //}
                //else if (catid == "5")
                //{
                //    strDist = "select num_oper_id,var_oper_opername from aoup_operator_Def";
                //    strDist += " where num_oper_id='" + operid + "'";
                //    strDist += " and num_oper_category=5";
                //    strDist += " order by var_oper_opername asc ";

                //    strMso += "select b.num_oper_id,b.var_oper_opername from aoup_operator_Def a,aoup_operator_Def b";
                //    strMso += " where a.num_oper_id='" + operid + "'";
                //    strMso += " and a.num_oper_category=5";
                //    strMso += " and a.num_oper_parentid=b.num_oper_id";
                //    strMso += " order by b.var_oper_opername asc ";

                //}

                //else
                //{

                //    strMso += "select num_oper_id,var_oper_opername from aoup_operator_Def ";
                //    strMso += " where num_oper_category=2";
                //    strMso += " order by var_oper_opername asc ";

                //    strDist = "select num_oper_id,var_oper_opername from aoup_operator_Def";
                //    strDist += " where num_oper_category=5";
                //    strDist += " order by var_oper_opername asc ";

                //}

                dtCompany = objHelper.GetDataTable(strCompQry);
                //dtDist = objHelper.GetDataTable(strDistQry);
                //dtSubDist = objHelper.GetDataTable(strSubDistQry);
                //dtMso = objHelper.GetDataTable(strMso);
                //dtDistributor = objHelper.GetDataTable(strDist);
                DataSet dsCompData = new DataSet();
                dsCompData.Tables.Add(dtCompany);
                //dsCompData.Tables.Add(dtDist);
                //dsCompData.Tables.Add(dtSubDist);
                //dsCompData.Tables.Add(dtMso);
                //dsCompData.Tables.Add(dtDistributor);
                return dsCompData;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCORegistration.cs-getCompanyData");
                return null;
            }
        }

        public string setLCOData(string username, Hashtable ht)
        {
            OracleConnection ConObj = new OracleConnection(ConfigurationSettings.AppSettings["ConString"].ToString().Trim());
            try
            {

                ConObj.Open();
                OracleCommand Cmd = new OracleCommand("aoup_lcopre_lco_ins_new", ConObj);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("in_username", OracleType.VarChar, 10);
                Cmd.Parameters["in_username"].Value = username;

                Cmd.Parameters.Add("in_lcouser_id", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcouser_id"].Value = ht["userid"];

                Cmd.Parameters.Add("in_lconame", OracleType.VarChar, 100);
                Cmd.Parameters["in_lconame"].Value = ht["name"];

                Cmd.Parameters.Add("in_lcofirstname", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcofirstname"].Value = ht["fname"];

                Cmd.Parameters.Add("in_lcomiddlename", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcomiddlename"].Value = ht["mname"];

                Cmd.Parameters.Add("in_lcolastname", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcolastname"].Value = ht["lname"];

                Cmd.Parameters.Add("in_lcoaddress", OracleType.VarChar, 500);
                Cmd.Parameters["in_lcoaddress"].Value = ht["addr"];

                Cmd.Parameters.Add("in_lcopin", OracleType.Number);
                Cmd.Parameters["in_lcopin"].Value = Convert.ToInt64(ht["pin"]);

                Cmd.Parameters.Add("in_lcocode", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcocode"].Value = ht["code"];

                Cmd.Parameters.Add("in_lcostateid", OracleType.Number);
                Cmd.Parameters["in_lcostateid"].Value = Convert.ToInt64(ht["state"]);

                Cmd.Parameters.Add("in_lcocityid", OracleType.Number);
                Cmd.Parameters["in_lcocityid"].Value = Convert.ToInt64(ht["city"]);

                Cmd.Parameters.Add("in_lcoemail", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcoemail"].Value = ht["email"];

                Cmd.Parameters.Add("in_lcophone", OracleType.Number);
                Cmd.Parameters["in_lcophone"].Value = Convert.ToInt64(ht["mobile"]);

                Cmd.Parameters.Add("in_lcojvno", OracleType.Number);
                Cmd.Parameters["in_lcojvno"].Value = Convert.ToInt64(ht["jv"]);

                Cmd.Parameters.Add("in_lcodirectno", OracleType.Number);
                Cmd.Parameters["in_lcodirectno"].Value = Convert.ToInt64(ht["direct"]);

                Cmd.Parameters.Add("in_lcobrmpoid", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcobrmpoid"].Value = ht["brmpoid"];


                Cmd.Parameters.Add("in_lcocompany", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcocompany"].Value = ht["company"];

                Cmd.Parameters.Add("in_lcodist", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcodist"].Value = ht["distributor"];

                Cmd.Parameters.Add("in_lcosubdist", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcosubdist"].Value = ht["subdistributor"];

                //  Cmd.Parameters.Add("in_lcosubdist", OracleType.VarChar, 100);
                //  Cmd.Parameters["in_lcosubdist"].Value = ht["subdistributor"];

                Cmd.Parameters.Add("in_lcocompcode", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcocompcode"].Value = ht["compcode"];

                Cmd.Parameters.Add("in_insflag", OracleType.VarChar, 10);
                Cmd.Parameters["in_insflag"].Value = ht["flag"];

                Cmd.Parameters.Add("in_lcooperid", OracleType.VarChar, 100);
                Cmd.Parameters["in_lcooperid"].Value = ht["lcoid"];

                Cmd.Parameters.Add("out_data", OracleType.VarChar, 500);
                Cmd.Parameters["out_data"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("out_errcode", OracleType.Number);
                Cmd.Parameters["out_errcode"].Direction = ParameterDirection.Output;

                Cmd.Parameters.Add("in_parentid", OracleType.Number);
                Cmd.Parameters["in_parentid"].Value = 0;

                Cmd.Parameters.Add("in_distid", OracleType.VarChar, 100);
                Cmd.Parameters["in_distid"].Value = 0;

                Cmd.Parameters.Add("in_user", OracleType.VarChar, 100);
                Cmd.Parameters["in_user"].Value = ht["LcoUserName"];
                Cmd.Parameters.Add("in_ecsstatus", OracleType.VarChar, 2);
                Cmd.Parameters["in_ecsstatus"].Value = ht["ecssattus"];


                Cmd.ExecuteNonQuery();
                ConObj.Close();

                int exeResult = Convert.ToInt32(Cmd.Parameters["out_errcode"].Value);
                string Str;

                if (exeResult == 9999)
                {
                    if (Convert.ToInt32(ht["flag"]) == 0)
                    {
                        Str = " LCO registered successfully...";
                    }
                    else
                    {
                        Str = " LCO updated successfully...";
                    }
                }
                else
                {
                    Str = Convert.ToString(Cmd.Parameters["out_data"].Value);
                }
                ConObj.Dispose();
                return Str;
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCORegistration.cs-setLCOData");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }

        public DataTable getLCOData(string whereClauseStr, string username)
        {
            try
            {
                //DataTable dtLCO = new DataTable("LCO");
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;
                StrQry = " SELECT a.var_hway_brm_poid, a.var_hway_lco_code, a.var_hway_lco_name, ";
                StrQry += " a.var_hway_first_name, a.var_hway_middle_name, ";
                StrQry += " a.var_hway_last_name, a.var_hway_address, a.var_hway_zipcode, ";
                StrQry += " a.var_hway_city, a.var_hway_state, a.var_hway_email, ";
                StrQry += " a.var_hway_phone, a.var_hway_company, a.var_hway_jv, ";
                StrQry += " a.var_hway_dt, a.var_hway_sdt, a.var_hway_area, ";
                StrQry += " a.var_hway_pref_dom, a.var_hway_ent_tax_no, ";
                StrQry += " a.var_hway_erp_control_acct_id, a.var_hway_pan_no, ";
                StrQry += " a.var_hway_st_reg_no, a.var_hway_vat_tax_no, ";
                StrQry += " a.var_hway_report_date, a.var_hway_pp_type ";
                StrQry += "  FROM view_hway_lco_master a ";



                StrQry += " where " + whereClauseStr;

                /*dtLCO = objHelper.GetDataTable(strLCOQry);
                DataSet dsLCOData = new DataSet();
                dsLCOData.Tables.Add(dtLCO);
                return dsLCOData;*/
                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Data_MstLCOPreUDefine.cs-getUserData");
                return null;
            }
        }
    }
}
