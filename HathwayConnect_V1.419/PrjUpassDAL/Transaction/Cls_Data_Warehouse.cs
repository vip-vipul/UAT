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
   public class Cls_Data_Warehouse
    {
        public DataTable Getdata(string Uploadid, string Type, string username)
        {
            try
            {
                Cls_Helper obj = new Cls_Helper();
                    string StrQry = "  select var_inventval_upload_id UploadID,var_inventval_orderno orderno,var_inventval_lcocode lcocode,var_inventval_deviceid deviceid,var_inventval_boxtype boxtype, ";
                    StrQry += "var_inventval_type Type,case when var_inventval_apistatus='0' then 'Success' when var_inventval_apistatus='1' then 'Fail' ";
                    StrQry += "when var_inventval_apistatus is null then 'Pending' end APIStatus,var_inventval_apismsg apimsg  from aoup_lcopre_pis_invent_raw  ";
                    StrQry += "  where  var_inventval_valtype='LM' and var_inventval_upload_id='" + Uploadid + "' and var_inventval_insby='" + username + "' ";
                    if (Type != "All" && Type != "")
                    {
                        StrQry += " and var_inventval_apistatus='" + Type + "'";
                    }
                    else if (Type == "All")
                    {

                    }
                    else if (Type == "")
                    {
                        StrQry += " and var_inventval_apistatus is null ";
                    }

                return obj.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_Warehouse.cs");
                return null;
            }
        }

        public DataTable GetInvBulkValiddata(Hashtable ht, string username)
        {
            try
            {
                Cls_Helper obj = new Cls_Helper();
                string from = ht["From"].ToString();
                string to = ht["TO"].ToString();
                string StrQry = " select var_inventval_upload_id upload_id,count(*) totalstb,sum(case when var_inventval_apistatus='0' then 1 else 0 end ) success,";
                StrQry += "  sum(case when var_inventval_apistatus='1' then 1 else 0 end ) fail, ";
                StrQry += "  sum(case when var_inventval_apistatus is null then 1 else 0 end ) pending from aoup_lcopre_pis_invent_raw  ";
                StrQry += "  where  var_inventval_valtype='IM' and var_inventval_insby= '" + username + "' and ";
                StrQry += " trunc(TO_DATE(dat_inventval_insdate,'DD-Mon-YYYY HH12:MI:SS'))>='" + from + "' and trunc(TO_DATE(dat_inventval_insdate,'DD-Mon-YYYY HH12:MI:SS'))<='" + to + "' ";
                StrQry += "  group by var_inventval_upload_id ";
                return obj.GetDataTable(StrQry);
            }
            catch (Exception ex)
            {
                Cls_Security objSecurity = new Cls_Security();
                objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_Warehouse.cs");
                return null;
            }
        }

       public DataTable Getbulkprocessdata(string username, string uploadid)
       {
           try
           {
               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;

               StrQry = "  select var_inventval_upload_id upload_id,count(*) totalstb,sum(case when var_inventval_apistatus='0' then 1 else 0 end ) success,";
               StrQry += "  sum(case when var_inventval_apistatus='1' then 1 else 0 end ) fail, ";
               StrQry += "  sum(case when var_inventval_apistatus is null then 1 else 0 end ) pending from aoup_lcopre_pis_invent_raw  ";
               StrQry += "  where var_inventval_upload_id='" + uploadid + "' and var_inventval_valtype='LM'";
               StrQry += "  group by var_inventval_upload_id";


               return ObjHelper.GetDataTable(StrQry);
           }
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_Warehouse.cs");
               return null;
           }
       }

       public DataTable GetWarehouseAllocationList(string username)
       {
           try
           {
               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;

               StrQry = " select * from view_warehouseauth_list where finusrauth = 'A' and housrauth = 'A'  and (stbpendingcount > 0 or confirmstatus='R') and  PDC='U' ";
               if (username != "")
               {
                   StrQry += " and var_pisnewstb_lcocode='" + username + "'";
               }
               StrQry += " order by dat_pistrans_insdate desc";
               return ObjHelper.GetDataTable(StrQry);
           }

           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_Warehouse.cs");
               return null;
           }
       }

       public DataTable GetReceiptData(string username, string receiptno)
       {
           try
           {
               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;

               if (receiptno.ToUpper().Contains("SPSN"))
               {
                   StrQry = "  select '' stbnopp, num_pistrans_id transid,var_pisnewstb_lcocode code,var_lcomst_name name,b.num_pisnewstb_rate STBRate,b.num_pisnewstb_discount STBDiscount,  b.num_pisnewstb_net STBNet,b.num_pisnewstb_lcorate LCORate,b.num_pisnewstb_lcodiscount LCODiscount,b.num_pisnewstb_lconet LCONet,b.num_pisnewstb_netamount TotalNet,var_pistrans_paymode paymode, ";
                   StrQry += " var_pistrans_cashier cashier,var_pisnewstb_stbcount stbcount,var_pisnewstb_stbpendingcount pendingcount,var_scheme_name scheme,var_pisnewstb_boxtype boxtype,var_pisnewstb_type type,ct.var_city_name City,sd.var_state_name State,VAR_SKYWORTH SKYWORTH,num_pisnewstb_faultycount Faulty,num_pisnewstb_foreclousrecount Foreclosre from aoup_lcopre_pis_trans_det a ";
                   StrQry += " left outer join aoup_lcopre_pis_spnewstb_mst b on a.num_pistrans_transid=b.num_pistrans_transid ";
                   StrQry += " left outer join aoup_lcopre_Scheme_master c on c.num_scheme_id=b.num_pisnewstb_scheme_id  ";
                   StrQry += " left outer join aoup_lcopre_lco_det l on l.var_lcomst_code=b.var_pisnewstb_lcocode ";
                   StrQry += " inner join Aoup_lcopre_city_def ct on ct.num_city_id=a.num_pistrans_cityid inner join aoup_lcopre_state_def sd on sd.num_state_id=a.num_pistrans_stateid";
                   StrQry += " where var_pistrans_transtype='SP'  and var_pistrans_receiptno='" + receiptno + "'";
                   //  StrQry += " and var_pisnewstb_stbpendingcount<>0 ";
               }
               else if (receiptno.ToUpper().Contains("SPSR"))
               {

                   StrQry = " select '' stbnopp, num_pisstbrpr_id transid,var_pisstbrpt_lco_code code,var_lcomst_name name,b.num_pisstbrpr_rate STBRate,num_pisstbrpr_discount STBDiscount,b.num_pisstbrpr_net STBNet,b.num_pisstbrpr_lcorate LCORate,b.num_pisstbrpr_lcodiscount LCODiscount,b.num_pisstbrpr_lconet LCONet,b.num_pisstbrpr_netamount TotalNet,var_pistrans_paymode paymode,  var_pistrans_cashier cashier,var_pisstbrpr_stbcount stbcount,var_pisstbrpr_stbpendingcount pendingcount ,var_scheme_name scheme,var_pisnewstb_boxtype boxtype ";
                   StrQry += " ,num_scheme_value schemevalue,var_pisnewstb_type type,ct.var_city_name City,sd.var_state_name State,VAR_SKYWORTH SKYWORTH,'0' Fualty,'0' Foreclosre from aoup_lcopre_pis_trans_det a   ";
                   StrQry += " left outer join aoup_lcopre_pis_spstbrepir_mst b on a.num_pistrans_transid=b.num_pisstbrpr_transid ";
                   StrQry += " left outer join aoup_lcopre_scheme_master c on c.num_scheme_id=b.num_pisstbrpr_scheme_id  ";
                   StrQry += " left outer join aoup_lcopre_lco_det l on l.var_lcomst_code=b.var_pisstbrpt_lco_code ";
                   StrQry += " inner join Aoup_lcopre_city_def ct on ct.num_city_id=a.num_pistrans_cityid inner join aoup_lcopre_state_def sd on sd.num_state_id=a.num_pistrans_stateid";
                   StrQry += " where var_pistrans_transtype='SP'  and var_pistrans_receiptno='" + receiptno + "'";
                   StrQry += " and var_pisstbrpr_stbpendingcount<>0 ";

               }

               else if (receiptno.ToUpper().Contains("PPSN"))
               {
                   StrQry = "  SELECT	var_pisnewstb_stbno stbnopp,num_pisnewstb_transid transid, var_pisnewstb_accno code, var_pisnewstb_vcno name,b.num_pisnewstb_rate STBRate,b.num_pisnewstb_discount STBDiscount,b.num_pisnewstb_net STBNet,b.num_pisnewstb_lcorate LCORate,b.num_pisnewstb_lcodiscount LCODiscount,b.num_pisnewstb_lconet LCONet,num_pisnewstb_amount TotalNet, var_pisnewstb_paymode paymode, var_pisnewstb_insby cashier, ";
                   StrQry += "  '1' stbcount,'1' pendingcount, var_scheme_name scheme,var_pisnewstb_boxtype boxtype ,num_scheme_value schemevalue ,'STB' type,ct.var_city_name City,sd.var_state_name State,VAR_SKYWORTH SKYWORTH,'0' Fualty,'0' Foreclosre FROM aoup_lcopre_pis_trans_det a LEFT OUTER JOIN ";
                   StrQry += "  aoup_lcopre_pis_ppnewstb b ON a.num_pistrans_transid = b.num_pisnewstb_transid LEFT OUTER JOIN 	aoup_lcopre_scheme_master c ";
                   StrQry += "  ON c.num_scheme_id = b.num_pisnewstb_schemeid inner join Aoup_lcopre_city_def ct on ct.num_city_id=a.num_pistrans_cityid inner join aoup_lcopre_state_def sd on sd.num_state_id=a.num_pistrans_stateid  WHERE	var_pistrans_transtype = 'PP' and var_pistrans_receiptno='" + receiptno + "'";
                   StrQry += "  and var_pisnewstb_warehouseuser is null ";

               }
               else
               {
                   StrQry = "  SELECT  var_pisstbrpr_stbno stbnopp, num_pisstbrpr_transid transid, var_pisstbrpr_accno code,var_pisstbrpr_vcno name, num_pisstbrpr_rate STBRate,num_pisstbrpr_discount STBDiscount,num_pisstbrpr_net STBNet,num_pisstbrpr_lcorate LCORate,num_pisstbrpr_lcodiscount LCODiscount,num_pisstbrpr_lconet LCONet,num_pisstbrpr_amount TotalNet, var_pisstbrpr_paymode paymode, var_pisstrpr_insby cashier, ";
                   StrQry += "  '1' stbcount,'1' pendingcount, var_scheme_name scheme,var_pisstbrpr_boxtype boxtype,num_scheme_value schemevalue,'STB' type,ct.var_city_name City,sd.var_state_name State,VAR_SKYWORTH SKYWORTH,'0' Fualty,'0' Foreclosre  FROM aoup_lcopre_pis_trans_det a LEFT OUTER JOIN ";
                   StrQry += "  aoup_lcopre_pis_ppstbrepair b ON a.num_pistrans_transid = b.num_pisstbrpr_transid LEFT OUTER JOIN  aoup_lcopre_scheme_master c ";
                   StrQry += "  ON c.num_scheme_id = b.num_pisstbrpr_schemeid inner join Aoup_lcopre_city_def ct on ct.num_city_id=a.num_pistrans_cityid inner join aoup_lcopre_state_def sd on sd.num_state_id=a.num_pistrans_stateid WHERE var_pistrans_transtype = 'PP' and var_pistrans_receiptno='" + receiptno + "' ";
                   StrQry += "  and var_pistrpr_warehouseuser is null ";
               }

               return ObjHelper.GetDataTable(StrQry);
           }
           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_Warehouse.cs");
               return null;
           }
       }

       public string InsertForeClosure(string sp, Hashtable ht)
       {
           string ConStr = ConfigurationSettings.AppSettings["ConString"].ToString().Trim();
           OracleConnection ConObj = new OracleConnection(ConStr);
           int returnValue = 0;
           try
           {
               OracleCommand cmd = new OracleCommand(sp, ConObj);
               cmd.CommandType = CommandType.StoredProcedure;


               cmd.Parameters.Add("In_UserId", OracleType.VarChar);
               cmd.Parameters["In_UserId"].Value = ht["Userid"];

               if (ht["TransId"] == "" || ht["TransId"] == null)
               {
                   cmd.Parameters.Add("IN_SNEWSTB_Transid", OracleType.Number);
                   cmd.Parameters["IN_SNEWSTB_Transid"].Value = 0;
               }
               else
               {
                   cmd.Parameters.Add("IN_SNEWSTB_Transid", OracleType.Number);
                   cmd.Parameters["IN_SNEWSTB_Transid"].Value = ht["TransId"];
               }



               if (ht["STBCount"] == "" || ht["STBCount"] == null)
               {
                   cmd.Parameters.Add("IN_SNEWSTB_noofstb", OracleType.Number);
                   cmd.Parameters["IN_SNEWSTB_noofstb"].Value = 0;
               }
               else
               {
                   cmd.Parameters.Add("IN_SNEWSTB_noofstb", OracleType.Number);
                   cmd.Parameters["IN_SNEWSTB_noofstb"].Value = ht["STBCount"];
               }


               if (ht["Reason"] == "" || ht["Reason"] == null)
               {
                   cmd.Parameters.Add("IN_reason", OracleType.VarChar);
                   cmd.Parameters["IN_reason"].Value = DBNull.Value;
               }
               else
               {
                   cmd.Parameters.Add("IN_reason", OracleType.VarChar);
                   cmd.Parameters["IN_reason"].Value = ht["Reason"];
               }
               if (ht["Receipttype"] == "" || ht["Receipttype"] == null)
               {
                   cmd.Parameters.Add("IN_receipttype", OracleType.VarChar);
                   cmd.Parameters["IN_receipttype"].Value = DBNull.Value;
               }
               else
               {
                   cmd.Parameters.Add("IN_receipttype", OracleType.VarChar);
                   cmd.Parameters["IN_receipttype"].Value = ht["Receipttype"];
               }

               cmd.Parameters.Add("Out_ErrorCode", OracleType.Number);
               cmd.Parameters["Out_ErrorCode"].Direction = ParameterDirection.Output;

               cmd.Parameters.Add("Out_ErrorMsg", OracleType.VarChar, 1000);
               cmd.Parameters["Out_ErrorMsg"].Direction = ParameterDirection.Output;

               ConObj.Open();
               cmd.ExecuteNonQuery();

               int exeResult = Convert.ToInt32(cmd.Parameters["Out_ErrorCode"].Value);
               string ExeResultMsg = Convert.ToString(cmd.Parameters["out_ErrorMsg"].Value);
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

       public DataTable GetSTBList(string username, string TransId)
       {
           try
           {
               Cls_Helper ObjHelper = new Cls_Helper();
               string StrQry;

               StrQry = "select var_pisnewstb_stbno STB, var_scheme_name Scheme from aoup_lcopre_pis_spnewstb_det ";
               StrQry += "left outer join aoup_lcopre_scheme_master on num_scheme_id = num_pisnewstb_schemeid ";
               StrQry += "where num_pisnewstb_id = " + TransId;

               return ObjHelper.GetDataTable(StrQry);
           }

           catch (Exception ex)
           {
               Cls_Security objSecurity = new Cls_Security();
               objSecurity.InsertIntoDb(username, ex.Message.ToString(), "Cls_Data_Warehouse.cs");
               return null;
           }
       }

   }
}
