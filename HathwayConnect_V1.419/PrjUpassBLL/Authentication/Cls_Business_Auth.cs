using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PrjUpassDAL.Authentication;
using System.Data;

namespace PrjUpassBLL.Authentication
{
    public class Cls_Business_Auth
    {
        public Hashtable GetAuthResponse(Hashtable credentials) {
            Cls_Data_Auth objAuth = new Cls_Data_Auth();
            return objAuth.Authentication(credentials);
        }

        public string SessionLog(Hashtable credentials)
        {
            Cls_Data_Auth objAuth = new Cls_Data_Auth();
            return objAuth.SessionLog(credentials);
        }

        public string getbroadcastmesg()
        {
            Cls_Data_Auth objAuth = new Cls_Data_Auth();
            return objAuth.getbroadcastmsg();
        }

        public string NumberToText(int number)
        {
            if (number == 0) return "Zero";

            if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };
            num[0] = number % 1000; // units 
            num[1] = number / 1000;
            num[2] = number / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands 
            num[3] = number / 10000000; // crores 
            num[2] = num[2] - 100 * num[3]; // lakhs 
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10; // ones 
                t = num[i] / 10;
                h = num[i] / 100; // hundreds 
                t = t - 10 * h; // tens 
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i > 5)
                        sb.Append("and ");
                    //else

                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }

        public bool hasPageRights(string uid, string uname, string page)
        {
            Cls_Data_Auth obj = new Cls_Data_Auth();
            return obj.checkRights(uid, uname, page);
        }
        //---- Added By RP on 28.06.2017 
        public void GetGSTNo(string LCOCODE, out string GSTNO)
        {
            GSTNO = "";
            Cls_Data_Auth obj = new Cls_Data_Auth();
            obj.GetGSTNo(LCOCODE, out GSTNO);
        }

        public void GetAccepGST(string LCOCODE, out string lconame)
        {
            lconame = "";
            Cls_Data_Auth obj = new Cls_Data_Auth();
            obj.GetAccepGST(LCOCODE, out lconame);
        }

        public string InsertLCAccept(string LcoCode,string IPAdd)
        {
            Cls_Data_Auth obj = new Cls_Data_Auth();
            return obj.InsertLCAccept(LcoCode,IPAdd);
        }
        
        public DataTable GetLCODetails(string username)
        {
            Cls_Data_Auth objAuth = new Cls_Data_Auth();
            Hashtable htResponse = new Hashtable();
            return objAuth.GetLCODetails(username);
        }

        //--------------------------------------------
    }
}
