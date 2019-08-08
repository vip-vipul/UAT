using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PrjUpassDAL.Helper;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;
namespace PrjUpassDAL.Transaction
{
    public class Cls_Data_STBInventory_Conformation
    {
        public DataTable STBInvConfList(string username)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                //StrQry = " select receiptno, subtypex, transtype, paymentmode, totalnet amount, lcodiscount discount, xtype, TRANSSUBTYPE1, SCHEME, BOXTYPE, dat_pistrans_insdate insdate,MAKEMODEL ,SKYWORTH ";
                //StrQry += "from view_warehouseauth_list ";
                //StrQry += "inner join aoup_lcopre_pis_spnewstb_det on num_pisnewstb_id = pistrans_id and "; 
                //StrQry += "var_pisnewstb_processed is null where LCOCODE='" + username + "' and STBPENDINGCOUNT< stbcount and vcPENDINGCOUNT< stbcount ";
                //StrQry += "group by receiptno, subtypex, transtype, paymentmode, totalnet , lcodiscount , xtype, TRANSSUBTYPE1, SCHEME, BOXTYPE, dat_pistrans_insdate,MAKEMODEL,SKYWORTH ";
                //StrQry += "order by dat_pistrans_insdate desc ";

                StrQry = "  select receiptno, subtypex, transtype, paymentmode, totalnet amount, lcodiscount discount, xtype, TRANSSUBTYPE1, SCHEME, BOXTYPE, dat_pistrans_insdate insdate,MAKEMODEL ,SKYWORTH ";
                StrQry += " from view_warehouseauth_list ";
                StrQry += " inner join aoup_lcopre_pis_spnewstb_det on num_pisnewstb_id = pistrans_id and  ";
                StrQry += " var_pisnewstb_processed is null where STBPENDINGCOUNT< stbcount and vcPENDINGCOUNT< stbcount and pdcflag='N'";
                StrQry += " group by receiptno, subtypex, transtype, paymentmode, totalnet , lcodiscount , xtype, TRANSSUBTYPE1, SCHEME, BOXTYPE, dat_pistrans_insdate,MAKEMODEL,SKYWORTH ";
                StrQry += " union ";
                StrQry += " select receiptno, subtypex, transtype, paymentmode, totalnet amount, lcodiscount discount, xtype, TRANSSUBTYPE1, SCHEME, BOXTYPE, dat_pistrans_insdate insdate,MAKEMODEL ,SKYWORTH ";
                StrQry += " from view_warehouseauth_list ";
                StrQry += " inner join aoup_lcopre_pis_spnewvc_det on num_pisnewvc_id = pistrans_id and  ";
                StrQry += " var_pisnewvc_processed is null where  vcPENDINGCOUNT< stbcount and pdcflag='N' ";
                StrQry += " group by receiptno, subtypex, transtype, paymentmode, totalnet , lcodiscount , xtype, TRANSSUBTYPE1, SCHEME, BOXTYPE, dat_pistrans_insdate,MAKEMODEL,SKYWORTH order by insdate desc ";

                return ObjHelper.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_STBInventory_Conformation.cs");
                return null;
            }
        }

        public DataTable STBInvConfDet(string username, string receiptno,string makemodel)
        {
            try
            {
                Cls_Helper ObjHelper = new Cls_Helper();
                string StrQry;

                /* StrQry = "select pistrans_id, var_pisnewstb_stbno STB, scheme, num_pisnewstb_stbid stbid from view_warehouseauth_list ";
                 StrQry += "inner join aoup_lcopre_pis_spnewstb_det on num_pisnewstb_id = pistrans_id and ";
                 StrQry += "var_pisnewstb_confirmstatus is null and confirmstatus is null and var_pisnewstb_processed is null ";
                 StrQry += "where receiptno = '" + receiptno + "'";*/

                //StrQry = "select DISTINCT transid,receiptno,var_pisnewstb_boxtype,city,state,num_pisnewstb_schemeid,pistrans_id, var_pisnewstb_stbno STB,var_pisnewvc_vcno VC, ";
                //StrQry += " scheme, num_pisnewstb_stbid stbid ,makemodel,company ";
                //StrQry += " from view_warehouseauth_list a ";
                //StrQry += " inner join aoup_lcopre_pis_spnewstb_det on num_pisnewstb_id = pistrans_id and var_pisnewstb_processed is null ";
                //if (makemodel == "Y")
                //{
                //    StrQry += " inner join aoup_lcopre_pis_spnewvc_det on num_pisnewvc_stbid = num_pisnewstb_stbid and num_pisnewstb_id=num_pisnewvc_id  ";
                //}
                //else
                //{
                //    StrQry += " left join aoup_lcopre_pis_spnewvc_det on num_pisnewvc_stbid = num_pisnewstb_stbid and num_pisnewstb_id=num_pisnewvc_id  ";
                //}
                //StrQry += "  ";
                //StrQry += "where receiptno = '" + receiptno + "'";

                //StrQry = "select DISTINCT transid,receiptno,var_pisnewstb_boxtype,city,state,num_pisnewstb_schemeid,pistrans_id, var_pisnewstb_stbno STB,";
                //StrQry += " scheme, num_pisnewstb_stbid stbid ,makemodel,company ,'STB' transtype,warehouse ";
                //StrQry += " from view_warehouseauth_list a  ";
                //StrQry += " inner join aoup_lcopre_pis_spnewstb_det on num_pisnewstb_id = pistrans_id and var_pisnewstb_processed is null ";
                //StrQry += " where receiptno = '" + receiptno + "' ";
                //StrQry += " union ";
                //StrQry += " select DISTINCT transid,receiptno,var_pisnewvc_boxtype,city,state,num_pisnewvc_schemeid,pistrans_id, var_pisnewvc_vcno STB, ";
                //StrQry += " scheme, num_pisnewvc_stbid stbid ,makemodel,company ,'VC' transtype,warehouse ";
                //StrQry += " from view_warehouseauth_list a  ";
                //StrQry += " inner join aoup_lcopre_pis_spnewvc_det on num_pisnewvc_id = pistrans_id  ";
                //StrQry += " where receiptno = '" + receiptno + "'  and var_pisnewvc_processed is null ";
                //StrQry += " order by transtype";

                StrQry = " select  b.num_pistrans_id transid,var_pistrans_receiptno receiptno,var_pisnewstb_boxtype,h.var_city_name city,f.var_state_name state,num_pisnewstb_schemeid,a.num_pistrans_transid pistrans_id, var_pisnewstb_stbno STB,";
                StrQry += " var_scheme_name scheme, num_pisnewstb_stbid stbid ,var_stb_makemodel makemodel,var_lcomst_company company ,'STB' transtype,var_pisnewstb_insby warehouse ";
                StrQry += " from aoup_lcopre_pis_trans_det a ";
                StrQry += " left outer join aoup_lcopre_pis_spnewstb_mst b on a.num_pistrans_transid=b.num_pistrans_transid ";
                StrQry += " left outer join aoup_lcopre_scheme_master c on c.num_scheme_id=b.num_pisnewstb_scheme_id  ";
                StrQry += " inner join aoup_lcopre_pis_spnewstb_det on num_pisnewstb_id = b.num_pistrans_id  ";
                StrQry += " left outer join aoup_lcopre_lco_det  g on g.var_lcomst_code=b.var_pisnewstb_lcocode ";
                StrQry += " left outer join aoup_lcopre_state_def f on f.num_state_id=g.num_lcomst_stateid ";
                StrQry += " left outer join aoup_lcopre_city_def  h on h.num_city_id=g.num_lcomst_cityid  ";
                StrQry += " where var_pistrans_receiptno = '" + receiptno + "' and var_pisnewstb_processed is null ";
                StrQry += " union ";
                StrQry += " select  b.num_pistrans_id transid,var_pistrans_receiptno receiptno,var_pisnewstb_boxtype,h.var_city_name city,f.var_state_name state,num_pisnewvc_schemeid,a.num_pistrans_transid pistrans_id, var_pisnewvc_vcno STB, ";
                StrQry += " var_scheme_name scheme, num_pisnewvc_stbid stbid ,var_stb_makemodel makemodel,var_lcomst_company company ,'VC' transtype,var_pisnewvc_insby warehouse  ";
                StrQry += " from aoup_lcopre_pis_trans_det a ";
                StrQry += " left outer join aoup_lcopre_pis_spnewstb_mst b on a.num_pistrans_transid=b.num_pistrans_transid ";
                StrQry += " left outer join aoup_lcopre_scheme_master c on c.num_scheme_id=b.num_pisnewstb_scheme_id  ";
                StrQry += " inner join aoup_lcopre_pis_spnewvc_det on num_pisnewvc_id = b.num_pistrans_id  ";
                StrQry += " left outer join aoup_lcopre_lco_det  g on g.var_lcomst_code=b.var_pisnewstb_lcocode ";
                StrQry += " left outer join aoup_lcopre_state_def f on f.num_state_id=g.num_lcomst_stateid ";
                StrQry += " left outer join aoup_lcopre_city_def  h on h.num_city_id=g.num_lcomst_cityid  ";
                StrQry += " where var_pistrans_receiptno = '" + receiptno + "' and var_pisnewvc_processed is null ";
                StrQry += " order by transtype ";

                return ObjHelper.GetDataTable(StrQry);
            }

            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_STBInventory_Conformation.cs");
                return null;
            }
        }

        public string InsertSTBInvConf(Hashtable ht)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();

            OracleConnection ConObj = new OracleConnection(ConStr);
            int returnValue = 0;

            try
            {
                OracleCommand cmd = new OracleCommand("aoup_lcopre_pis_Conform_ins", ConObj);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("in_username", OracleType.VarChar).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("in_receiptno", OracleType.VarChar).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("in_pistrans_id", OracleType.VarChar).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("in_str", OracleType.VarChar).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("in_ipaddress", OracleType.VarChar).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("in_remark", OracleType.VarChar).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("Out_ErrCode", OracleType.Number).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Out_ErrMsg", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;

                cmd.Parameters["in_username"].Value = ht["in_username"];
                cmd.Parameters["in_receiptno"].Value = ht["in_receiptno"];
                cmd.Parameters["in_pistrans_id"].Value = ht["in_pistrans_id"];
                cmd.Parameters["in_str"].Value = ht["in_str"];
                cmd.Parameters["in_ipaddress"].Value = ht["in_ipaddress"];
                cmd.Parameters["in_remark"].Value = ht["in_remark"];

                ConObj.Open();
                cmd.ExecuteNonQuery();

                int exeResult = Convert.ToInt32(cmd.Parameters["Out_ErrCode"].Value);
                string ExeResultMsg = Convert.ToString(cmd.Parameters["Out_ErrMsg"].Value);
                string Str;

                return exeResult + "$" + ExeResultMsg;

                ConObj.Dispose();
                return Str;

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(ht["UserName"].ToString(), ex.Message.ToString(), "frmNewSTB.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }


        public static string UpdateInvIntStatus(string username, string flag, string transid, string upload_id)
        {
            string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
            OracleConnection ConObj = new OracleConnection(ConStr);
            int returnValue = 0;
            try
            {
                OracleCommand cmd = new OracleCommand("aoup_lcopre_invninterstatusupd", ConObj);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add("IN_username", OracleType.VarChar);
                cmd.Parameters["IN_username"].Value = username;

                cmd.Parameters.Add("In_flag", OracleType.VarChar);
                cmd.Parameters["In_flag"].Value = flag;

                cmd.Parameters.Add("In_receipt", OracleType.VarChar);
                cmd.Parameters["In_receipt"].Value = transid;

                cmd.Parameters.Add("In_upload_id", OracleType.VarChar);
                cmd.Parameters["In_upload_id"].Value = upload_id;

                cmd.Parameters.Add("Out_ErrorCode", OracleType.Number);
                cmd.Parameters["Out_ErrorCode"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("Out_ErrorMsg", OracleType.VarChar, 1000);
                cmd.Parameters["Out_ErrorMsg"].Direction = ParameterDirection.Output;

                ConObj.Open();
                cmd.ExecuteNonQuery();

                string exeResult = Convert.ToString(cmd.Parameters["Out_ErrorCode"].Value);
                string ExeResultMsg = Convert.ToString(cmd.Parameters["out_ErrorMsg"].Value);
                string Str;

                return exeResult;// +"$" + ExeResultMsg;

                ConObj.Dispose();
                return exeResult;

            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_frmInventoryInternal.cs");
                return "ex_occured";
            }
            finally
            {
                ConObj.Close();
                ConObj.Dispose();
            }
        }
    }
}
