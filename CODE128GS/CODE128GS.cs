using System;
using System.Collections.Generic;
using System.Text;
using Made4Net.DataAccess;
using System.Data;

namespace Barcode128GS
{
    using System;
    using System.Collections.Generic;

    public class GS128
    {

        #region AICodesFriend

        public class AICodes
        {
            public AICodes(string m_aicode, string m_description, bool m_decimalindicator, bool m_fixlength, int m_datalength, string m_datatype,string m_identifiermapping)
            {
                aicode = m_aicode;
                description = m_description;
                decimalindicator = m_decimalindicator;
                fixlength = m_fixlength;
                datalength = m_datalength;
                datatype = m_datatype;
                identifiermapping = m_identifiermapping;
            }
            public readonly string aicode;
            public readonly string description;
            public readonly bool decimalindicator;
            public readonly bool fixlength;
            public readonly int datalength;
            public readonly string datatype;
            public readonly string identifiermapping;
            //DATATYPE
        }

        #endregion
        #region variables
        //private static string GroupSeparator;

        //private static readonly Dictionary<String, AICodes> dAIs = new Dictionary<String, AICodes>();
        public string GroupSeparator;

        public Dictionary<String, AICodes> dAIs;
         
        //Added for RWMS-784
        public string _aicode = "";
        //End Added for RWMS-784

     #endregion


        #region Constractor
        public GS128()
        { Load(); }

        #endregion

        #region LoadClass

        //was static metod for one instance per whole sation
        private void AIAdd(object aicode, object description, object decimalindicator, object fixlength, object datalength, object datatype, object identifiermapping)
        {
            dAIs.Add(aicode.ToString(), new AICodes(aicode.ToString(), description.ToString(), bool.Parse(decimalindicator.ToString()), bool.Parse(fixlength.ToString()), int.Parse(datalength.ToString()), datatype.ToString(), identifiermapping.ToString()));
        }


        private void Load()
        {
            if (dAIs != null) return;
            dAIs = new Dictionary<String, AICodes>();
            string sql = "SELECT AICODE, DESCRIPTION, DECIMALINDICATOR, FIXLENGTH, DATALENGTH,DATATYPE,IDENTIFIERMAPPING FROM GS1128AI";
            DataTable dt = new DataTable();

            try
            {
                Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, false, "");

                foreach (DataRow dr in dt.Rows)
                    AIAdd(dr["AICODE"], dr["DESCRIPTION"], dr["DECIMALINDICATOR"], dr["FIXLENGTH"], dr["DATALENGTH"], dr["DATATYPE"], dr["IDENTIFIERMAPPING"]);

                GroupSeparator = Made4Net.Shared.Util.GetSystemParameterValue("CODE128GS").ToString();
            }
            catch (Exception ex) { ThrowError("Error in constractor class " + ex.Message); }
        }

        private void ThrowError(string err)
        {
            throw new ApplicationException(err);
        }
        #endregion

        public int getWeight(ref String BarCode, ref string ErrorMessage)
        {
            int ret = 1;
            decimal d;
            Dictionary<String, String> ParceGS1 = new Dictionary<string, string>();
            if (decimal.TryParse(BarCode, out d)) //Upto length of 30, number is being treated as valid decimal with TryParse and returning as a valid weight when expiry date is not included in the barcode   
            {
                //Added for RWMS-441   
                if (d.ToString().Length >= 16) //If length is > 16 as per business requirement, parse the barcode to extract weight   
                {
                    ret = ParceGS1Code128(BarCode, ref ParceGS1, ref ErrorMessage);
                }
                else
                //End Added for RWMS-441  
                return ret;
            }
            else
            {
                ret = ParceGS1Code128(BarCode, ref ParceGS1, ref ErrorMessage);
            }

            //RWMS-441 : Moved the below from the above else part as ParceGS1Code128 is called in both if and else conditions code


            //Commnted for RWMS-jira 271, 328, 324 related to 128 barcode. Start
            //if (ret == 1 && ParceGS1.ContainsKey("310"))
            //    BarCode = ParceGS1["310"].ToString();
            //Commnted for RWMS-jira 271, 328, 324 related to 128 barcode. End

            //Added for RWMS-jira 271, 328, 324 related to 128 barcode. Start
            if (ret == 1 && ParceGS1.ContainsKey("WEIGHT"))
            {
                BarCode = ParceGS1["WEIGHT"].ToString();
            }
            
            //Added for RWMS-jira 271, 328, 324 related to 128 barcode. End

            //}
            return ret;
        }

        //Added for RWMS-784

        public int getWeightDateAICode(ref String BarCode, ref String Weight, ref String UCC128Date, ref String AICode, ref string ErrorMessage)
        {
            int ret = 1;
            decimal d;
            Dictionary<String, String> ParceGS1 = new Dictionary<string, string>();
            if (decimal.TryParse(BarCode, out d)) //Upto length of 30, number is being treated as valid decimal with TryParse and returning as a valid weight when expiry date is not included in the barcode
            {

                if (d.ToString().Length >= 16) //If length is > 16 as per business requirement, parse the barcode to extract weight
                {
                    ret = ParceGS1Code128(BarCode, ref ParceGS1, ref ErrorMessage);
                }
                else
                    BarCode = "";
                    return ret;
            }
            else
            {
                ret = ParceGS1Code128(BarCode, ref ParceGS1, ref ErrorMessage);
            }
            if (ret == 1 && ParceGS1.ContainsKey("WEIGHT"))
            {
                Weight = ParceGS1["WEIGHT"].ToString();
            }
            if (ret == 1 && ParceGS1.ContainsKey("EXPIRYDATE"))
            {
                UCC128Date = ParceGS1["EXPIRYDATE"].ToString();
                //TODO : get the aicode and assignt it to the ref variable AICode                
                AICode = _aicode;                
            }
            else if (ret == 1 && ParceGS1.ContainsKey("MFGDATE"))
            {
                UCC128Date = ParceGS1["MFGDATE"].ToString();
                //TODO : get the aicode and assignt it to the ref variable AICode                
                AICode = _aicode;                
            }
                       
            return ret;
        }

        //End Added for RWMS-784

        public int ParceGS1Code128(String BarCode, ref Dictionary<String, String> ParceGS1, ref string ErrorMessage)
        {
            int ret = 1;
            string AIValue = string.Empty;
            StringBuilder AI = new StringBuilder();
            Made4Net.Shared.Translation.Translator t = new Made4Net.Shared.Translation.Translator(2);//Made4Net.Shared.Translation.Translator.CurrentLanguageID);

            int index = 0;
            int indAI = 0;
            int indLastAI = 5;
            //Commnted for RWMS-jira 271, 328, 324 related to 128 barcode. Start  
            //check if the string starts with the group separator (GS). If yes, remove it
            //if (BarCode.Length > GroupSeparator.Length + 2 && BarCode.StartsWith(GroupSeparator))
            //{
            //    BarCode = BarCode.Substring(GroupSeparator.Length);
            //}
            //else
            //{
            //    ErrorMessage = t.Translate("no valid GS1Code128, Group Separator is missing on the start of Barcode Key Or Barcode Length is too short", null);
            //    return 2;
            //}
            //Commnted for RWMS-jira 271, 328, 324 related to 128 barcode. End 
            //main loop on barcode
            while (index < BarCode.Length)
            {
                AI.Remove(0, AI.Length);
                if (indAI + 2 <= BarCode.Length)
                {
                    AI.Append(BarCode.Substring(indAI, 2));
                    indAI = indAI + 2;
                }
                else return ret;
                //internal AI search loop
                // find the AI (check if the first 2 digits of the string is an AI, if not the first 3 digits and if not 4)
                while (indAI < BarCode.Length && indAI < (index + indLastAI))
                {
                    if (dAIs.ContainsKey(AI.ToString()))
                    {
                        ret = getAIValue(AI.ToString(), BarCode, ref indAI, ref AIValue, ref ErrorMessage);
                        //RWMS -569 - Updated dictionary key as Identifiermapping value instead of AICode, if IdentifierMapping is empty then key is filled as description
                        if (ret == 1)
                        {
                            if (dAIs[AI.ToString()].identifiermapping.ToString().Length ==0 )
                            {
                                ParceGS1.Add(dAIs[AI.ToString()].description , AIValue);
                            }
                            else
                            { 
                                ParceGS1.Add(dAIs[AI.ToString()].identifiermapping, AIValue);
                                //Added for RWMS-784
                                if (_aicode == "")
                                {
                                    if (dAIs[AI.ToString()].identifiermapping.ToString() == "EXPIRYDATE" || dAIs[AI.ToString()].identifiermapping.ToString() == "MFGDATE")
                                    {
                                        _aicode = dAIs[AI.ToString()].aicode;
                                    }
                                }
                                //End Added for RWMS-784
                            }
                            break;
                        }
                        else
                            return ret;
                    }
                    else
                        AI.Append(BarCode[indAI++]);
                }
                // if not found any AI, return 2 (error message “no valid AI”)
                if (ParceGS1.Count == 0)
                {
                    ErrorMessage = t.Translate("no valid AI", null);
                    return 2;
                }

                index = indAI;




                // index++;
            }
            return ret;
        }

        private int getAIValue(string AI, string Barcode, ref int indAI, ref string AIValue, ref string ErrorMessage)
        {
            int ret = 1;
            int iDevisionDecimalIndicator = 1;
            Made4Net.Shared.Translation.Translator t = new Made4Net.Shared.Translation.Translator(2);//Made4Net.Shared.Translation.Translator.CurrentLanguageID);

            try
            {
                // check in the decimal indicator is true. If yes, keep the next character as decimal indicator
                if (dAIs[AI].decimalindicator)
                {
                    iDevisionDecimalIndicator = DevisionDecimalIndicator(int.Parse(Barcode[indAI++].ToString()));
                }

                //check if the fix length field is true. if yes, take the next X characters (not including the decimal indicator if exists) as the value. 
                //If there is a decimal indicator, use it (if the decimal indicator is 1 -> divide by 10, if 2 -> by 100 and so on)
                if (dAIs[AI].fixlength)
                {
                    AIValue = Barcode.Substring(indAI, dAIs[AI].datalength);
                    indAI = indAI + AIValue.Length;
                }
                //if the fix length field is false, search for the next GS (according to the sys_param value) or EOF. 
                //check in the number of characters from the AI to the next GS/EOF is <= data length for this AI. 
                //If it is bigger, return 3 (error message “could not parse barcode”). If the length is OK, use this as the value. 
                //Again, in there is a decimal indicator, use it
                else
                {
                    //get next GS
                    if (Barcode.Substring(indAI).Contains(GroupSeparator))
                    {
                        AIValue = Barcode.Substring(indAI, Barcode.Substring(indAI).IndexOf(GroupSeparator));
                    }
                    else
                    {
                        AIValue = Barcode.Substring(indAI);
                    }
                    if (AIValue.Length > dAIs[AI].datalength)
                    {
                        ErrorMessage = t.Translate("could not parse barcode (AIValue.Length > dAIs[AI].datalength);", null) + " AIValue = " + AIValue;
                        return 3;
                    }
                    indAI = indAI + AIValue.Length;

                }
                //check if Value that we get in fit datatype format
                //If the DATATYPE is N, check that the value is numeric. If it is D - date
                //(format either YYMMDD or YYMMDDHHMM), If it is A - string.
                //If the data does not fit the type, return 4 (error "wrong data type")
                switch (dAIs[AI].datatype.ToUpper())
                {
                    case "N":
                        decimal result;
                        if (!decimal.TryParse(AIValue, out result))                        
                        {
                            ErrorMessage = "wrong data type, value must be numeric. " + " aicode = " + dAIs[AI].aicode + " AIValue = " + AIValue + " datatype = " + dAIs[AI].datatype.ToUpper();
                            return 4;
                        }
                        break;
                    case "D":
                        DateTime dDATE;
                        if (!(DateTime.TryParseExact(AIValue, "yyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dDATE) ||
                            DateTime.TryParseExact(AIValue, "yyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dDATE)))
                        {
                            ErrorMessage = "wrong data type, value must be DateTime, format(YYMMDD or YYMMDDHHMM). " + " aicode = " + dAIs[AI].aicode + " AIValue = " + AIValue + " datatype = " + dAIs[AI].datatype.ToUpper();
                            return 4;
                        }
 
                        break;
                    //case "A":
                    //    Console.WriteLine("Case 2");
                    //   
                    default:
                    //    ErrorMessage = "wrong data type" ;
                    //    return 4;
                        break;
                }

                if (dAIs[AI].decimalindicator)
                {
                    decimal d;
                    d = Math.Round(decimal.Parse(AIValue) / iDevisionDecimalIndicator, 2);
                    AIValue = d.ToString();
                }
            }
            catch
            {
                ErrorMessage = t.Translate("could not parse barcode", null);
                return 3;
            }

            return ret;
        }

        private int DevisionDecimalIndicator(int DecimalIndicator)
        {
            int DevisionDecimalIndicator = 1;
            for (int i = 0; i < DecimalIndicator; i++)
            {
                DevisionDecimalIndicator = DevisionDecimalIndicator * 10;
            }
            return DevisionDecimalIndicator;
        }
    }
}
