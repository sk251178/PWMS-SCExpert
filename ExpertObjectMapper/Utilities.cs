using System;
using System.Xml;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using Made4Net.Shared.Logging;
using System.Text.RegularExpressions;

namespace ExpertObjectMapper
{
	/// <summary>
	/// Summary description for Utilities.
	/// </summary>
	public class Utilities
	{
		public Utilities()
		{

		}

		#region "General Object Methods"

        public static DataSet BuildDataTableForDatabaseTable(string pTableName)
        {
            // the data table to be returned
            DataTable dtRet = new DataTable();
            dtRet.TableName = "BUSINESSOBJECT";

            // get the column schema of the target table
            string sSql = string.Format("select sc.name as FieldName,t.name FieldDataType from syscolumns sc join sysobjects so on sc.id=so.id join systypes AS t ON sc.xusertype=t.xusertype where so.name='{0}'", pTableName);
            DataTable dtFieldsTypes = new DataTable();
            Made4Net.DataAccess.DataInterface.FillDataset(sSql, dtFieldsTypes, false, null);

            foreach (DataRow dr in dtFieldsTypes.Rows)
            {
                switch (dr["FieldDataType"].ToString().ToLower())
                {
                    case "int":
                    case "decimal":
                    case "smallint":
                    case "tinyint":
                    case "float":
                    case "real":
                    case "bigint":
                        dtRet.Columns.Add(dr["FieldName"].ToString(), typeof(decimal));
                        break;
                    case "datetime":
                        dtRet.Columns.Add(dr["FieldName"].ToString(), typeof(string));
                        break;
                    case "text":
                    case "varchar":
                    case "nchar":
                    case "char":
                    case "ntext":
                    case "nvarchar":
                        dtRet.Columns.Add(dr["FieldName"].ToString(), typeof(string));
                        break;
                    case "bit":
                        dtRet.Columns.Add(dr["FieldName"].ToString(), typeof(bool));
                        break;
                    default:
                        dtRet.Columns.Add(dr["FieldName"].ToString(), typeof(string));
                        break;
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dtRet);
            return ds;
        }


		public static void BuildObjectFromDataRow(DataSet ds, string sTableName, XmlDocument TargetDocument, XmlElement TargetElement, DataRow dr, LogFile oLog)
		{
			for (int j = 0; j < ds.Tables[sTableName].Columns.Count; j++)
			{
				string columnname = ds.Tables[sTableName].Columns[j].ColumnName;
				if (ds.Tables[sTableName].Columns[j].ColumnMapping == System.Data.MappingType.Element)
				{
					try
					{
						if (columnname != "UNITPRICE")
						{
							Utilities.buildDocLine(TargetDocument, TargetElement, dr[columnname].ToString() , columnname);
							if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + dr[columnname].ToString(), true);
						}
						else
						{
                            //Added for RWMS-504
                            string strunitprice = "";
                            WMS.Logic.SKU sk;
                            try
                            {
                                sk = new WMS.Logic.SKU(dr["CONSIGNEE"].ToString(), dr["SKU"].ToString(), true);
                                strunitprice = sk.UNITPRICE.ToString();
                            }
                            catch (Exception ex)
                            {
                                strunitprice = "ERROR GETTING UNIT PRICE";
                                if (oLog != null)
                                {
                                    oLog.WriteLine("Exception getting column details : " + columnname, true);
                                    oLog.WriteLine(ex.Message.ToString(), true);
                                }
                            }
                            finally
                            {
                                //((IDisposable)sk).Dispose();
                                sk = null;
                            }
                            //End Added for RWMS-504

                            //Commented for RWMS-504
                            //WMS.Logic.SKU sk = new WMS.Logic.SKU(dr["CONSIGNEE"].ToString() , dr["SKU"].ToString() , true);
                            //Utilities.buildDocLine(TargetDocument, TargetElement, sk.UNITPRICE.ToString()  ,"UNITPRICE");
                            //if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + sk.UNITPRICE.ToString(), true);
                            //End Commented for RWMS-504

                            //Added for RWMS-504
                            Utilities.buildDocLine(TargetDocument, TargetElement, strunitprice, "UNITPRICE");
                            if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + strunitprice, true);
                            //End Added for RWMS-504
						}
					}
					catch(Exception ex)
					{
						Utilities.buildDocLine(TargetDocument, TargetElement, "" , columnname);
						//Utilities.buildDocHeader(TargetDocument, "", columnname);
						if (oLog != null) oLog.WriteLine("Exception building column : " + columnname,true);
						if (oLog != null) oLog.WriteLine(ex.Message.ToString() ,true);
					}
				}
			}
			if (oLog != null) oLog.WriteSeperator();
		}


		public static void BuildObjectFromQSender(DataSet ds, string sTableName, XmlDocument TargetDocument, XmlElement TargetElement, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
		{
            //RWMS-344: Begin
            string sQSenderNotes = string.Empty;
            //if (oLog != null) oLog.WriteLine("BuildObjectFromQSender:");
            //if (oLog != null) oLog.WriteLine("ds.Tables[sTableName].Columns.Count:" + ds.Tables[sTableName].Columns.Count.ToString());

            //if (oLog != null) oLog.WriteLine("ds.Tables[sTableName].Columns Begin :");
            //for (int jj = 0; jj < ds.Tables[sTableName].Columns.Count; jj++)
            //{
            //    string columnname = ds.Tables[sTableName].Columns[jj].ColumnName;
            //    if (oLog != null) oLog.WriteLine("Field columnname:" + columnname, true);
            //    //if (oLog != null) oLog.WriteLine("Field columnname : " + columnname + "value:" + qSender.Values[columnname].ToString(),true);
            //}
            //if (oLog != null) oLog.WriteLine("ds.Tables[sTableName].Columns END :");

            //for (int i = 0; i < qSender.Values.Count - 1; i++)
            //    oLog.WriteLine(string.Format("qSender Field :{0} : value :{1}", qSender.Values.Keys[i], qSender.Values[i]), true);
            //RWMS-344: End

			for (int j = 0; j < ds.Tables[sTableName].Columns.Count; j++)
			{
				string columnname = ds.Tables[sTableName].Columns[j].ColumnName;
				if (ds.Tables[sTableName].Columns[j].ColumnMapping == System.Data.MappingType.Element)
				{
                    if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' '), true);
					try
					{
						switch (columnname)
						{
							case "UNITPRICE" :
								WMS.Logic.SKU sk = new WMS.Logic.SKU(qSender.Values["CONSIGNEE"].ToString() , qSender.Values["SKU"].ToString() , true);
								Utilities.buildDocHeader(TargetDocument, sk.UNITPRICE.ToString() , columnname);
								if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + sk.UNITPRICE.ToString(), true);
								break;
                            //case "QTY" :
                            //    Utilities.buildDocHeader(TargetDocument, qSender.Values["TOQTY"].ToString() , "QTY");
                            //    if (oLog != null) oLog.WriteLine("Field : " + "QTY".PadRight(25, ' ') + " , Value : " + qSender.Values["TOQTY"].ToString(), true);
                            //    break;
                            //case "TRANTYPE" :
                            //    Utilities.buildDocHeader(TargetDocument, qSender.Values["ACTIVITYTYPE"].ToString(), columnname);
                            //    if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + qSender.Values["ACTIVITYTYPE"].ToString(), true);
                            //    break;
							default :
                                //RWMS-344: Begin
                                //if (oLog != null) oLog.WriteLine("Inside default : " + columnname,true);
                                if (qSender.Values[columnname].ToString().StartsWith(",") == true)
                                {
                                    if (oLog != null) oLog.WriteLine("starts with  ,: " + columnname, true);
                                    //if (oLog != null) oLog.WriteLine("buildDocHeader : Value:" + Value, true);
                                    //qSender.Values[columnname] = qSender.Values[columnname].ToString().Split(',')[1];
                                    sQSenderNotes = qSender.Values[columnname].ToString();
                                    qSender.Values[columnname] = sQSenderNotes.Substring(1, sQSenderNotes.Length - 1);
                                    //if (oLog != null) oLog.WriteLine("qSender.Values[columnname]  : " + qSender.Values[columnname].ToString(), true);
                                }
                                //RWMS-344: End
                                Utilities.buildDocHeader(TargetDocument,  qSender.Values[columnname].ToString(), columnname);
								if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + qSender.Values[columnname].ToString(), true);
								break;
						}
					}
					catch
					{
                        if (oLog != null) oLog.WriteLine("BuildObjectFromQSender: Exception building column : " + columnname, true);
						if (oLog != null) oLog.WriteLine($"'{columnname}' Value is missing in Queue message " , true);
					}
				}
			}
			if (oLog != null) oLog.WriteSeperator();
		}


		public static void BuildObjectFromDocHeader(DataSet ds, string sTableName, XmlDocument TargetDocument, Object SourceObject, LogFile oLog)
		{
			for (int j = 0; j < ds.Tables[sTableName].Columns.Count; j++)
			{
				string columnname = ds.Tables[sTableName].Columns[j].ColumnName;
				if (ds.Tables[sTableName].Columns[j].ColumnMapping == System.Data.MappingType.Element)
				{
					// Get Document Property
					Type myType = SourceObject.GetType();
					try
					{
						switch (columnname)
						{
							case "TARGETCOMPANYNAME" :
								WMS.Logic.Company cm = new WMS.Logic.Company(((WMS.Logic.OutboundOrderHeader)(SourceObject)).CONSIGNEE ,((WMS.Logic.OutboundOrderHeader)(SourceObject)).TARGETCOMPANY, ((WMS.Logic.OutboundOrderHeader)(SourceObject)).COMPANYTYPE, true);
								Utilities.buildDocHeader(TargetDocument, cm.COMPANYNAME , columnname);
								if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + ((WMS.Logic.OutboundOrderHeader)(SourceObject)).SHIPTO.CONTACTID, true);
								break;
							case "SHIPTO" :
								// Set XML Node
								Utilities.buildDocHeader(TargetDocument, ((WMS.Logic.OutboundOrderHeader)(SourceObject)).SHIPTO.CONTACTID , columnname);
								if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + ((WMS.Logic.OutboundOrderHeader)(SourceObject)).SHIPTO.CONTACTID, true);
								break;
							case "RECEIVEDFROM" :
								// Set XML Node
								Utilities.buildDocHeader(TargetDocument, ((WMS.Logic.InboundOrderHeader)(SourceObject)).RECEIVEDFROM.CONTACTID , columnname);
								if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + ((WMS.Logic.InboundOrderHeader)(SourceObject)).RECEIVEDFROM.CONTACTID, true);
								break;
							default :
								PropertyInfo myInfo = myType.GetProperty(columnname);
								// Set XML Node
                                string sValue = "";
                                if (myInfo.PropertyType.FullName.Equals("System.DateTime", StringComparison.OrdinalIgnoreCase))
                                    sValue = ((DateTime)myInfo.GetValue(SourceObject, null)).ToString(Made4Net.Shared.Util.GetSystemParameterValue("DateFormat"));
                                else
                                    sValue = myInfo.GetValue(SourceObject, null).ToString();
								Utilities.buildDocHeader(TargetDocument, sValue, columnname);
								if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + myInfo.GetValue(SourceObject, null).ToString(), true);
								break;
						}
					}
					catch(Exception ex)
					{
						Utilities.buildDocHeader(TargetDocument, "", columnname);
						if (oLog != null) oLog.WriteLine("Exception building column : " + columnname,true);
						if (oLog != null) oLog.WriteLine(ex.Message.ToString() ,true);
					}
				}
			}
			if (oLog != null) oLog.WriteSeperator();
		}


		public static void BuildObjectFromDocDetail(DataSet ds, string sTableName, XmlDocument TargetDocument, XmlElement TargetElement, Object SourceObject, LogFile oLog)
		{
			for (int j = 0; j < ds.Tables[sTableName].Columns.Count; j++)
			{
				string columnname = ds.Tables[sTableName].Columns[j].ColumnName;
				if (ds.Tables[sTableName].Columns[j].ColumnMapping == System.Data.MappingType.Element)
				{
					try
					{
						Type myType = SourceObject.GetType();
						PropertyInfo ConsigneeInfo = null;
						PropertyInfo SkuInfo = null;
						WMS.Logic.SKU sk = null;
						WMS.Logic.SKU.SKUUOM su = null;
                        bool Exception = false;

						switch (columnname)
						{
							case "SKUVOLUME" :
								ConsigneeInfo = myType.GetProperty("CONSIGNEE");
								SkuInfo = myType.GetProperty("SKU");
								sk = new WMS.Logic.SKU(ConsigneeInfo.GetValue(SourceObject, null).ToString() , SkuInfo.GetValue(SourceObject, null).ToString() , true);
								su = new  WMS.Logic.SKU.SKUUOM(ConsigneeInfo.GetValue(SourceObject, null).ToString() , SkuInfo.GetValue(SourceObject, null).ToString(), sk.getLowestUom(), true);
								Utilities.buildDocLine(TargetDocument, TargetElement, su.VOLUME.ToString() , columnname);
								if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + su.VOLUME.ToString(), true);
								break;
							case "SKUWEIGHT" :
								ConsigneeInfo = myType.GetProperty("CONSIGNEE");
								SkuInfo = myType.GetProperty("SKU");
								sk = new WMS.Logic.SKU(ConsigneeInfo.GetValue(SourceObject, null).ToString() , SkuInfo.GetValue(SourceObject, null).ToString() , true);
								su = new  WMS.Logic.SKU.SKUUOM(ConsigneeInfo.GetValue(SourceObject, null).ToString() , SkuInfo.GetValue(SourceObject, null).ToString(), sk.getLowestUom(), true);
								Utilities.buildDocLine(TargetDocument, TargetElement, su.GROSSWEIGHT.ToString() , columnname);
								if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + su.GROSSWEIGHT.ToString(), true);
								break;
							case "UNITPRICE" :
								ConsigneeInfo = myType.GetProperty("CONSIGNEE");
								SkuInfo = myType.GetProperty("SKU");
								//sk = new WMS.Logic.SKU(ConsigneeInfo.GetValue(SourceObject, null).ToString() , SkuInfo.GetValue(SourceObject, null).ToString() , true);
                                //Utilities.buildDocLine(TargetDocument, TargetElement, sk.UNITPRICE.ToString() , columnname);
                                string unprice = Made4Net.DataAccess.DataInterface.ExecuteScalar(string.Format("select isnull(unitprice,0) as unitprice from sku where consignee = '{0}' and sku = '{1}'", ConsigneeInfo.GetValue(SourceObject, null).ToString(), SkuInfo.GetValue(SourceObject, null).ToString())).ToString();
								Utilities.buildDocLine(TargetDocument, TargetElement, unprice, columnname);
                                if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + unprice, true);
								break;
							case "SHIPTO" :
								// Set XML Node
                                try
                                {
                                    Utilities.buildDocHeader(TargetDocument, ((WMS.Logic.OutboundOrderHeader)(SourceObject)).SHIPTO.CONTACTID, columnname);
                                    if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + ((WMS.Logic.OutboundOrderHeader)(SourceObject)).SHIPTO.CONTACTID, true);
                                }
                                catch (Exception)
                                {
                                    Exception = true;
                                }
                                if (Exception)
                                {
                                    Utilities.buildDocHeader(TargetDocument, ((WMS.Logic.Flowthrough)(SourceObject)).ShipTo, columnname);
                                    if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + ((WMS.Logic.Flowthrough)(SourceObject)).ShipTo, true);
                                }
								break;
							case "RECEIVEDFROM" :
								// Set XML Node
								Utilities.buildDocHeader(TargetDocument, ((WMS.Logic.InboundOrderHeader)(SourceObject)).RECEIVEDFROM.CONTACTID , columnname);
								if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + ((WMS.Logic.InboundOrderHeader)(SourceObject)).RECEIVEDFROM.CONTACTID, true);
								break;
                            case "ORDERCONTACTID" :
                                // Set XML Node
                                try
                                {
                                    Utilities.buildDocLine(TargetDocument, TargetElement,  ((WMS.Logic.OutboundOrderHeader)(SourceObject)).SHIPTO.CONTACTID, "SHIPTO");
                                    if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + ((WMS.Logic.OutboundOrderHeader)(SourceObject)).SHIPTO.CONTACTID, true);
                                }
                                catch (Exception)
                                {
                                    Exception = true;
                                }
                                if (Exception)
                                {
                                    Utilities.buildDocLine(TargetDocument, TargetElement, ((WMS.Logic.Flowthrough)(SourceObject)).ShipTo, "SHIPTO");
                                    if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + ((WMS.Logic.Flowthrough)(SourceObject)).ShipTo, true);
                                }
                                break;
							default :
								// Get Document Property
								PropertyInfo myInfo = myType.GetProperty(columnname);
                                string sValue = "";
                                if (myInfo.PropertyType.FullName.Equals("System.DateTime",StringComparison.OrdinalIgnoreCase))
                                    sValue = ((DateTime)myInfo.GetValue(SourceObject, null)).ToString(Made4Net.Shared.Util.GetSystemParameterValue("DateFormat"));
                                else
                                    sValue = myInfo.GetValue(SourceObject, null).ToString();
								Utilities.buildDocLine(TargetDocument, TargetElement, sValue , columnname);
								if (oLog != null) oLog.WriteLine("Field : " + columnname.PadRight(25, ' ') + " , Value : " + myInfo.GetValue(SourceObject, null).ToString(), true);
								break;
						}
					}
					catch(Exception ex)
					{
						Utilities.buildDocLine(TargetDocument, TargetElement, "" , columnname);
						if (oLog != null) oLog.WriteLine("Exception building column : " + columnname,true);
						if (oLog != null) oLog.WriteLine(ex.Message.ToString() ,true);
					}
				}
			}
			if (oLog != null) oLog.WriteSeperator();
		}


		public static void BuildXmlFromObject(DataSet ds, string sTableName, XmlNode SourceNode, Object TargetObject, LogFile oLog)
		{
			for (int j = 0; j < ds.Tables[sTableName].Columns.Count; j++)
			{
				string columnname = ds.Tables[sTableName].Columns[j].ColumnName;
				if (ds.Tables[sTableName].Columns[j].ColumnMapping == System.Data.MappingType.Element)
				{
					try
					{
						SetObjectPropertyFromXml(SourceNode, TargetObject, columnname, columnname, "", oLog);
					}
					catch(Exception ex)
					{
						if (oLog != null) oLog.WriteLine("Exception building column : " + columnname,true);
						if (oLog != null) oLog.WriteLine(ex.Message.ToString() ,true);
					}
				}
			}
			if (oLog != null) oLog.WriteSeperator();
		}

		#endregion

		#region "General XML Methods"

		public static void AddCollectionNode(XmlDocument oXmlDoc, XmlElement oXmlElemnt, string NodeName)
		{
			XmlElement oXmlTempNode;
			oXmlTempNode = oXmlDoc.CreateElement(NodeName);
			oXmlElemnt.AppendChild(oXmlTempNode);
		}

        public static void buildDocLine(XmlDocument oXmlDoc, XmlElement oXmlElemnt, string Value, string NodeName)
		{
			XmlElement oXmlTempNode;
			oXmlTempNode = oXmlDoc.CreateElement(NodeName);
			oXmlTempNode.AppendChild(oXmlDoc.CreateTextNode(Value));
			oXmlElemnt.AppendChild(oXmlTempNode);
		}

        public static void buildDocHeader(XmlDocument oXmlDoc, string Value, string NodeName)
		{
			XmlElement oXmlTempNode;
			try
			{
                oXmlTempNode = oXmlDoc.CreateElement(NodeName);
				oXmlTempNode.AppendChild(oXmlDoc.CreateTextNode(Value));
				oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA").AppendChild(oXmlTempNode);
			}
			catch(Exception ex)
			{
				ex.ToString();
			}
		}

		public static string DateTimeToString(DateTime vDate)
		{
			string strDateFormat;

			if ( vDate == DateTime.MinValue || vDate == DateTime.MaxValue )
				return "";

			try
			{
				//strDateFormat = vDate.ToString("yyyy-MM-dd HH:mm:ss");
                strDateFormat = vDate.ToString(Made4Net.Shared.Util.GetSystemParameterValue("DateFormat"));
			}
			catch
			{
				strDateFormat = "";
			}

			return strDateFormat;
		}

		public static DateTime StringToDateTime(string vDateString)
		{
			DateTime strDateFormat;

			if (vDateString=="")
				return DateTime.Now;

			try
			{
				//strDateFormat = Made4Net.Shared.Util.WMSStringToDate(vDateString);
				strDateFormat = Convert.ToDateTime(vDateString);
			}
			catch
			{
				return DateTime.Now;
			}

			return strDateFormat;
		}

		#endregion

		#region "General Methods"

        public static bool ConvertToBoolean(string vStrToConvert, bool vDefaultValue)
		{
			try
			{
			    return Convert.ToBoolean(vStrToConvert);
			}
			catch
			{
                return vDefaultValue;
			}
		}

		public static double ConvertToDouble(string vStrToConvert, double vDefaultValue)
		{
			try
			{
				return Convert.ToDouble(vStrToConvert);
			}
			catch
			{
				return vDefaultValue;
			}
		}

		public static decimal ConvertToDecimal(string vStrToConvert, decimal vDefaultValue)
		{
			try
			{
				return Convert.ToDecimal(vStrToConvert);
			}
			catch
			{
				return vDefaultValue;
			}
		}

		public static string ConvertToString(string vStrToConvert, string vDefaultValue)
		{
			try
			{
				return Convert.ToString(vStrToConvert);
			}
			catch
			{
				return vDefaultValue;
			}
		}

		public static int ConvertToInteger(string vStrToConvert, int vDefaultValue)
		{
            try
            {
                if (Regex.IsMatch(vStrToConvert, @"^\d+?$"))
                {
                    return Convert.ToInt32(vStrToConvert);
                }
                else if (Regex.IsMatch(vStrToConvert, @"^\d+(\.0+)?$"))
                {
                    return Convert.ToInt32(Regex.Replace(vStrToConvert, @"(\.0+)?$", ""));
                }
                else
                {
                    return vDefaultValue;
                }
            }
            catch
            {
                return vDefaultValue;
            }
		}

		#endregion

		#region "Expert XML Methods"

		public static void SetObjectPropertyFromXml(XmlNode SourceObject, object DesctinationObject, string SourceXPath, string DestinationProperty, string DestinationDefaultValue, LogFile oLog)
		{
			string SourceObjectValue = "";
			bool SourceObjectEmpty = true;
			try
			{
				SourceObjectValue = SourceObject.SelectSingleNode(SourceXPath).InnerText;
				SourceObjectEmpty = false;
			}
			catch(Exception ex)
			{
				SourceObjectEmpty = true;
				ex.ToString();
			}
			if ( !SourceObjectEmpty )
			{
				if ( SourceObjectValue != "")
				{
					Type myType = DesctinationObject.GetType();
					PropertyInfo myInfo = myType.GetProperty(DestinationProperty);
					if(myInfo != null)
                    {
						switch (myInfo.PropertyType.ToString())
						{
							case "System.Boolean":
								bool res;
								try
								{
									if (SourceObjectValue == "1")
										res = true;
									else if (SourceObjectValue == "0")
										res = false;
									else
										res = Convert.ToBoolean(SourceObjectValue);

									myInfo.SetValue(DesctinationObject, res, null);
								}
								catch
								{

								}
								break;
							case "System.DateTime":
								myInfo.SetValue(DesctinationObject, Utilities.StringToDateTime(SourceObjectValue), null);
								break;
							case "System.Decimal":
								myInfo.SetValue(DesctinationObject, Utilities.ConvertToDecimal(SourceObjectValue, 1), null);
								break;
							case "System.Double":
								myInfo.SetValue(DesctinationObject, Utilities.ConvertToDouble(SourceObjectValue, 1), null);
								break;
							case "System.Int32":
								myInfo.SetValue(DesctinationObject, Utilities.ConvertToInteger(SourceObjectValue, 1), null);
								break;
							default:
								myInfo.SetValue(DesctinationObject, SourceObjectValue.Trim(), null);
								break;
						}
						if (oLog != null)
						{
							oLog.WriteLine($"Property Name : {DestinationProperty,-20} | Property Value(From XML) : {SourceObjectValue}", true);
						}
					}
					else
                    {
						if (oLog != null)
						{
							oLog.WriteLine($"Property Name : {DestinationProperty,-20} | [NOT FOUND in class {myType.Name}]", true);
						}
					}
				}
				else
				{
					if (DestinationDefaultValue != "")
					{
						Type myType = DesctinationObject.GetType();
						PropertyInfo myInfo = myType.GetProperty(DestinationProperty);
						switch(myInfo.PropertyType.ToString())
						{
							case "System.Boolean"  :
								//myInfo.SetValue(DesctinationObject, Utilities.ConvertToBoolean(DestinationDefaultValue, false), null);
                                bool res;
                                try
                                {
                                    res = Convert.ToBoolean(SourceObjectValue);
                                    myInfo.SetValue(DesctinationObject, res, null);
                                }
                                catch
                                {

                                }
								break;
							case "System.DateTime" :
								myInfo.SetValue(DesctinationObject, Utilities.StringToDateTime(DestinationDefaultValue), null);
								break;
							case "System.Decimal" :
								myInfo.SetValue(DesctinationObject, Utilities.ConvertToDecimal(DestinationDefaultValue, 1), null);
								break;
							case "System.Double" :
                                myInfo.SetValue(DesctinationObject, Utilities.ConvertToDouble(DestinationDefaultValue, 1), null);
								break;
							case "System.Int32" :
								myInfo.SetValue(DesctinationObject, Utilities.ConvertToInteger(DestinationDefaultValue, 1), null);
								break;
							default :
								myInfo.SetValue(DesctinationObject, DestinationDefaultValue, null);
								break;
						}
						if ( oLog != null) oLog.WriteLine("Property Name :" + DestinationProperty.PadRight(20, ' ')  + " | Property Value(From XML) : " + DestinationDefaultValue.PadRight(100, ' '), true);
					}
				}
			}
		}


		public static XmlElement BuildAttributeCollectionXmlNode(XmlDocument oDoc, string vCollectionNodeName, string vChildNodeName, string vNameNode, string vValueNode, DataRow odr, LogFile oLog)
		{
			//Creating CollectionNode
			XmlElement oAttributes = oDoc.CreateElement(vCollectionNodeName);
			try
			{
				for (int i = 4; i < odr.Table.Columns.Count; i++)
				{
					XmlElement oAttribute = oDoc.CreateElement(vChildNodeName);
					try
					{
						Utilities.buildDocLine(oDoc, oAttribute, odr.Table.Columns[i].ColumnName.ToString() , vNameNode);
						string val;

						switch(odr[i].GetType().ToString())
						{
							case "System.Boolean"  :
								val = ((bool)odr[i]).ToString();
								break;
							case "System.DateTime" :
								val = Utilities.DateTimeToString((DateTime)odr[i]);
								break;
							case "System.Decimal" :
								val = Convert.ToString(ConvertToDecimal(Convert.ToString(odr[i]), 0));
								break;
							case "System.Int32" :
								val = ((int)odr[i]).ToString() ;
								break;
							case "System.DBNull" :
								val = "";
								break;
							default :
								val = (string)odr[i];
								break;
						}
						Utilities.buildDocLine(oDoc, oAttribute, val, vValueNode);
						oAttributes.AppendChild(oAttribute);
					}
					catch(Exception ex)
					{
                        if (oLog != null) oLog.WriteLine("Error occured building node for attribute collection...." + ex.ToString(), true);
						//ex.ToString();
					}
					oAttribute = null;
				}
				return oAttributes;
			}
			catch(Exception ex)
			{
				if (oLog != null)
				{
					oLog.WriteLine("Error exporting BO Attributes : " +  ex.Message ,true);
				}
			}
			oAttributes = null;
			return null;
		}


		public static XmlElement BuildAttributeCollectionXmlNode(XmlDocument oDoc, string vCollectionNodeName, string vChildNodeName, string vNameNode, string vValueNode, WMS.Logic.InventoryAttributeBase vAttributeCollection, LogFile oLog)
		{
			//Creating CollectionNode
			XmlElement oAttributes = oDoc.CreateElement(vCollectionNodeName);
			try
			{
				for (int i = 0; i< vAttributeCollection.Attributes.Count ;i++)
				{
					XmlElement oAttribute = oDoc.CreateElement(vChildNodeName);
					try
					{
						Utilities.buildDocLine(oDoc, oAttribute, vAttributeCollection.Attributes.Keys[i].ToString() , vNameNode);
						string val;

						switch(vAttributeCollection.Attributes[i].GetType().ToString())
						{
							case "System.Boolean"  :
								val = ((bool)vAttributeCollection.Attributes[i]).ToString();
								break;
							case "System.DateTime" :
								val = Utilities.DateTimeToString((DateTime)vAttributeCollection.Attributes[i]);
								break;
							case "System.Decimal" :
								val = ((decimal)vAttributeCollection.Attributes[i]).ToString() ;
								break;
							case "System.Int32" :
								val = ((int)vAttributeCollection.Attributes[i]).ToString() ;
								break;
							case "System.DBNull" :
								val = "";
								break;
							default :
								val = (string)vAttributeCollection.Attributes[i];
								break;
						}
						Utilities.buildDocLine(oDoc, oAttribute, val, vValueNode);
						oAttributes.AppendChild(oAttribute);
					}
					catch(Exception ex)
					{
                        if (oLog != null) oLog.WriteLine("Error occured building node for attribute collection...." + ex.ToString(), true);
						//ex.ToString();
					}
					oAttribute = null;
				}
				return oAttributes;
			}
			catch(Exception ex)
			{
				if (oLog != null)
				{
					oLog.WriteLine("Error exporting BO Attributes : " +  ex.Message ,true);
				}
			}
			oAttributes = null;
			return null;
		}

        //RWMS-2667 Start
        public static XmlElement BuildAttributeCollectionXmlNode(XmlDocument oDoc, string vCollectionNodeName, string vChildNodeName, string vNameNode, string vValueNode, WMS.Logic.InventoryAttributeBase vAttributeCollection, string Overrideweight, LogFile oLog)
        {
            //Creating CollectionNode
            XmlElement oAttributes = oDoc.CreateElement(vCollectionNodeName);
            try
            {
                for (int i = 0; i < vAttributeCollection.Attributes.Count; i++)
                {
                    XmlElement oAttribute = oDoc.CreateElement(vChildNodeName);
                    try
                    {
                        string keyName = vAttributeCollection.Attributes.Keys[i].ToString();

                        Utilities.buildDocLine(oDoc, oAttribute, vAttributeCollection.Attributes.Keys[i].ToString(), vNameNode);
                        string val;

                        switch (vAttributeCollection.Attributes[i].GetType().ToString())
                        {
                            case "System.Boolean":
                                val = ((bool)vAttributeCollection.Attributes[i]).ToString();
                                break;
                            case "System.DateTime":
                                val = Utilities.DateTimeToString((DateTime)vAttributeCollection.Attributes[i]);
                                break;
                            case "System.Decimal":
                                val = ((decimal)vAttributeCollection.Attributes[i]).ToString();
                                break;
                            case "System.Int32":
                                val = ((int)vAttributeCollection.Attributes[i]).ToString();
                                break;
                            case "System.DBNull":
                                val = "";
                                break;
                            default:
                                val = (string)vAttributeCollection.Attributes[i];
                                break;
                        }

                        if (keyName == "WEIGHT")
                        {
                            val = Overrideweight;
                        }
                        Utilities.buildDocLine(oDoc, oAttribute, val, vValueNode);
                        oAttributes.AppendChild(oAttribute);
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Error occured building node for attribute collection...." + ex.ToString(), true);
                        //ex.ToString();
                    }
                    oAttribute = null;
                }
                return oAttributes;
            }
            catch (Exception ex)
            {
                if (oLog != null)
                {
                    oLog.WriteLine("Error exporting BO Attributes : " + ex.Message, true);
                }
            }
            oAttributes = null;
            return null;
        }
        //RWMS-2667 End
		#endregion

		#region "Methods"

		public static string GetProccessId(string pAppId)
		{
			string sessionId;
			Process[] localProcesses = Process.GetProcessesByName(pAppId.ToLower().Replace(".exe", ""));
			if (localProcesses.Length == 1 )
				sessionId = localProcesses[0].Id.ToString();
			else
				sessionId = "PNF";

			return sessionId;
		}

		#endregion
	}
}