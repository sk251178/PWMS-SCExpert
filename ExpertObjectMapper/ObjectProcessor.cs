using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Data;
using Made4Net.Shared.Logging;
using Made4Net.DataAccess;
using WMS.Logic;
using System.Collections;

namespace ExpertObjectMapper
{
    /// <summary>
    /// Summary description for ObjectProcessor.
    /// </summary>
    public class ObjectProcessor
    {
        #region "Variables"

        protected string _objtype;
        //protected int //_processresult = 0;

        #endregion

        #region "Properties"

        //public int ProcessResult
        //{
        //    get { return _processresult; }
        //}

        #endregion

        #region "Constructors"

        public ObjectProcessor()
        {

        }

        #endregion

        #region "Import Methods"

        #region "Company Methods"

        public DataMapperTransactionResult ProcessCompany(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.COMPANY ds = new DataTemplate.COMPANY();

            if (oLog != null) oLog.WriteLine("Starting to import Company BO", true);
            try
            {
                foreach (XmlNode CompanyNode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
                {
                    if (CompanyNode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                        continue;
                    try
                    {
                        #region "Process DATA Node"

                        WMS.Logic.Company oCompany;
                        if (WMS.Logic.Company.Exists(CompanyNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), CompanyNode.SelectSingleNode("COMPANY").InnerText, CompanyNode.SelectSingleNode("COMPANYTYPE").InnerText.Trim()))
                            oCompany = new WMS.Logic.Company(CompanyNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), CompanyNode.SelectSingleNode("COMPANY").InnerText, CompanyNode.SelectSingleNode("COMPANYTYPE").InnerText.Trim(), true);
                        else
                            oCompany = new WMS.Logic.Company();

                        Utilities.BuildXmlFromObject(ds, "DATA", CompanyNode, oCompany, oLog);

                        oCompany.Save();
                        string firstContact = "";
                        foreach (XmlNode ContactNode in CompanyNode.SelectNodes("CONTACTS/CONTACT"))
                        {
                            try
                            {
                                // Check if there is contact for this company
                                string contactID = "";
                                if (ContactNode.SelectSingleNode("CONTACTID") != null)
                                    contactID = ContactNode.SelectSingleNode("CONTACTID").InnerText.Trim();

                                if (string.IsNullOrEmpty(contactID))
                                    contactID = Made4Net.Shared.Util.getNextCounter("CONTACTID");
                                if (firstContact == "")
                                    firstContact = contactID;
                                WMS.Logic.Contact oContact;
                                if (WMS.Logic.Contact.Exists(contactID)) //if (WMS.Logic.Contact.Exists(oCompany.DEFAULTCONTACTID))
                                    oContact = new WMS.Logic.Contact(contactID, true);
                                else
                                    oContact = new WMS.Logic.Contact();

                                Utilities.BuildXmlFromObject(ds, "CONTACT", ContactNode, oContact, oLog);

                                //oContact.CONTACTID = oCompany.DEFAULTCONTACTID;
                                oContact.CONTACTID = contactID;
                                oContact.Save("SYSTEM");
                                //oCompany.CONTACTS.AddContact(oCompany.DEFAULTCONTACTID, "SYSTEM");
                                oCompany.CONTACTS.AddContact(contactID, "SYSTEM");
                                //oCompany.SetDefaultContact(contactID, "SYSTEM");
                                //if (oLog != null) oLog.WriteLine("Contact saved , for company # : " + oCompany.COMPANY + " conatct ID is :" + oCompany.DEFAULTCONTACTID, true);
                                if (oLog != null) oLog.WriteLine("Contact saved , for company # : " + oCompany.COMPANY + " conatct ID is :" + contactID, true);
                            }
                            catch (Exception ex)
                            {
                                if (oLog != null) oLog.WriteLine("Exception in importing Contact BO", true);
                                if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                                oBuilder.AppendLine(ex.Message);
                                res = DataMapperTransactionResult.BOElementFailed;
                            }
                        }
                        if (CompanyNode.SelectNodes("CONTACTS/CONTACT").Count > 0)
                        {
                            // Check if there is a default contact in the received transaction, and if yes set it as the default contact for the company
                            if (CompanyNode.SelectSingleNode("DEFAULTCONTACT").InnerText != "")
                                oCompany.SetDefaultContact(CompanyNode.SelectSingleNode("DEFAULTCONTACT").InnerText, "SYSTEM");
                            else
                                oCompany.SetDefaultContact(firstContact, "SYSTEM");
                        }

                        #endregion

                    }
                    catch (Made4Net.Shared.M4NException m4nex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in importing Company BO", true);
                        if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                        oBuilder.AppendLine(m4nex.Description);
                        res = DataMapperTransactionResult.BOFailed;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in importing Company BO", true);
                        if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                        oBuilder.AppendLine(ex.Message);
                        res = DataMapperTransactionResult.BOFailed;
                    }
                }
            }
            catch (Made4Net.Shared.M4NException m4nex)
            {
                if (oLog != null) oLog.WriteLine("Exception in importing Company BO", true);
                if (oLog != null) oLog.WriteLine(m4nex.Message.ToString(), true);
                oBuilder.AppendLine(m4nex.Description);
                res = DataMapperTransactionResult.BOFailed;
            }
            catch (Exception ex)
            {
                if (oLog != null) oLog.WriteLine("Exception in importing Company BO", true);
                if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                oBuilder.AppendLine(ex.Message);
                res = DataMapperTransactionResult.BOFailed;
            }
            if (oLog != null) oLog.WriteLine("Finished to import Company BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "Carrier Methods"
        public XmlDocument ExportCarrier(string p, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            #region TransShipment
            DataTemplate.CARRIER ds = new DataTemplate.CARRIER();
            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

            if (oLog != null) oLog.WriteLine("Creating SCExpert BO for Carrier Export Message.", true);
            if (oLog != null) oLog.WriteSeperator();

            WMS.Logic.Carrier oCarrier = new WMS.Logic.Carrier(qSender.Values["CARRIER"], true);

            Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, oCarrier, oLog);
            #endregion
            if (oLog != null)
                oLog.WriteLine("Carrier Export finished.", true);
            return oDoc;

        }

        public DataMapperTransactionResult ProcessCarrier(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.CARRIER ds = new DataTemplate.CARRIER();

            if (oLog != null) oLog.WriteLine("Starting to import Carrier BO", true);
            try
            {
                foreach (XmlNode CarrierNode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
                {
                    if (CarrierNode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                        continue;
                    try
                    {
                        #region "Process DATA Node"

                        WMS.Logic.Carrier oCarrier;
                        if (WMS.Logic.Carrier.Exists(CarrierNode.SelectSingleNode("CARRIER").InnerText.Trim()))
                            oCarrier = new WMS.Logic.Carrier(CarrierNode.SelectSingleNode("CARRIER").InnerText.Trim(), true);
                        else
                            oCarrier = new WMS.Logic.Carrier();

                        Utilities.BuildXmlFromObject(ds, "DATA", CarrierNode, oCarrier, oLog);

                        oCarrier.Save();
                        foreach (XmlNode ContactNode in CarrierNode.SelectNodes("CONTACTS/CONTACT"))
                        {
                            try
                            {
                                // Check if there is contact for this company
                                WMS.Logic.Contact oContact;
                                if (WMS.Logic.Contact.Exists(oCarrier.CONTACTID))
                                    oContact = new WMS.Logic.Contact(oCarrier.CONTACTID, true);
                                else
                                    oContact = new WMS.Logic.Contact();

                                Utilities.BuildXmlFromObject(ds, "CONTACT", ContactNode, oContact, oLog);

                                oContact.CONTACTID = oCarrier.CONTACTID;
                                oContact.Save("SYSTEM");
                                if (oLog != null) oLog.WriteLine("Contact saved , for carrier # : " + oCarrier.CARRIER + " conatct ID is :" + oCarrier.CONTACTID, true);
                            }
                            catch (Exception ex)
                            {
                                if (oLog != null) oLog.WriteLine("Exception in importing Carrier Contact BO", true);
                                if (oLog != null) oLog.WriteLine("Exception : " + ex.ToString(), true);
                                oBuilder.AppendLine(ex.Message);
                                res = DataMapperTransactionResult.BOElementFailed;
                            }
                        }

                        #endregion
                    }
                    catch (Made4Net.Shared.M4NException m4nex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in importing Carrier BO", true);
                        if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                        oBuilder.AppendLine(m4nex.Description);
                        res = DataMapperTransactionResult.BOFailed;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in importing Carrier BO", true);
                        if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                        oBuilder.AppendLine(ex.Message);
                        res = DataMapperTransactionResult.BOFailed;
                    }
                }
            }
            catch (Made4Net.Shared.M4NException m4nex)
            {
                if (oLog != null) oLog.WriteLine("Exception in importing Carrier BO", true);
                if (oLog != null) oLog.WriteLine(m4nex.Message.ToString(), true);
                oBuilder.AppendLine(m4nex.Description);
                res = DataMapperTransactionResult.BOFailed;
            }
            catch (Exception ex)
            {
                if (oLog != null) oLog.WriteLine("Exception in importing Carrier BO", true);
                if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                oBuilder.AppendLine(ex.Message);
                res = DataMapperTransactionResult.BOFailed;
            }
            if (oLog != null) oLog.WriteLine("Finished to import Carrier BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "SKU Methods"

        public DataMapperTransactionResult ProcessSku(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataTemplate.SKU ds = new DataTemplate.SKU();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;

            if (oLog != null) oLog.WriteLine("Starting to import Sku BO", true);
            foreach (XmlNode SkuNode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (SkuNode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"

                    string skuGroup = SkuNode.SelectSingleNode("SKUGROUP").InnerText.Trim();
                    if (!WMS.Logic.SKUGroup.Exists(skuGroup))
                    {
                        if (oLog != null)
                        {
                            oLog.WriteLine($"SKU Group {skuGroup} does not exist and will be added to the database.", true);
                        }
                        WMS.Logic.SKUGroup.Insert(skuGroup, skuGroup, false);
                    }
                    WMS.Logic.SKU oSku;
                    if (WMS.Logic.SKU.SKUExists(SkuNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), SkuNode.SelectSingleNode("SKU").InnerText.Trim()))
                    {
                        if (oLog != null) oLog.WriteLine("SKU exists loading the same", true);
                        try
                        {
                            oSku = new WMS.Logic.SKU(SkuNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), SkuNode.SelectSingleNode("SKU").InnerText.Trim(), true);
                            if (oLog != null) oLog.WriteLine("SKU object created successfully", true);
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Error occured creating SKU object : " + ex.ToString(), true);
                            throw ex;
                        }
                    }
                    else
                    {
                        if (oLog != null) oLog.WriteLine("SKU does not exist", true);
                        oSku = new WMS.Logic.SKU();
                    }

                    if (oLog != null) oLog.WriteLine("Invoking BuildXmlFromObject", true);

                    Utilities.BuildXmlFromObject(ds, "DATA", SkuNode, oSku, oLog);

                    oSku.EDITDATE = DateTime.Now;
                    oSku.EDITUSER = "SYSTEM";
                    try
                    {
                        if (SkuNode.SelectSingleNode("SKUCLASS/CLASSNAME") != null)
                        {
                            // Added for RWMS-1382
                            //if (oLog != null) oLog.WriteLine("Trying to set the class name to sku. class name: " + SkuNode.SelectSingleNode("SKUCLASS/CLASSNAME").InnerText.Trim(), true);
                            //oSku.SKUClass = new WMS.Logic.SkuClass(SkuNode.SelectSingleNode("SKUCLASS/CLASSNAME").InnerText.Trim());
                            //if (oLog != null) oLog.WriteLine("Sku Class set.", true);
                            //RWMS-830,RWMS-587 Added Start
                          if (SkuNode.SelectSingleNode("SKUCLASS/CLASSNAME").InnerText.Trim() == "")
                          {
                          oSku.SKUClassName = string.Empty;
                          }
                          else
                          {
                          //RWMS-830,RWMS-587 Added End
                          if (oLog != null) oLog.WriteLine("Trying to set the class name to sku. class name: " + SkuNode.SelectSingleNode("SKUCLASS/CLASSNAME").InnerText.Trim(), true);
                          oSku.SKUClass = new WMS.Logic.SkuClass(SkuNode.SelectSingleNode("SKUCLASS/CLASSNAME").InnerText.Trim());
                          if (oLog != null) oLog.WriteLine("Sku Class set.", true);
                          }
                            // Ended for RWMS-1382
                        }
                        if(oSku.PARENTSKU==string.Empty) 
                        {
                            oSku.PARENTSKU = null;
                        }
                    }
                    catch (Made4Net.Shared.M4NException m4nex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in importing Sku Class BO", true);
                        if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in importing Sku Class", true);
                        if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    }
                    WMS.Logic.SKU.SKUUOMCollection node1 = new WMS.Logic.SKU.SKUUOMCollection(oSku.CONSIGNEE, oSku.SKU);
                    oSku.UNITSOFMEASURE = node1;
                    oSku.Save();
                    if (oLog != null) oLog.WriteLine("SKU saved : " + oSku.SKU, true);
                    try
                    {
                        if (oLog != null) oLog.WriteLine("Checking UOM Collection : ", true);
                        oSku.UNITSOFMEASURE.Clear();
                        foreach (XmlNode UOMNode in SkuNode.SelectNodes("UOMCOLLECTION/UOMOBJ"))
                        {
                            if (oLog != null) oLog.WriteLine("UOM found : " + UOMNode.SelectSingleNode("UOM").InnerText.Trim(), true);
                            WMS.Logic.SKU.SKUUOM oUom;
                            if (WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, UOMNode.SelectSingleNode("UOM").InnerText.Trim()))
                                oUom = new SKU.SKUUOM(oSku.CONSIGNEE, oSku.SKU, UOMNode.SelectSingleNode("UOM").InnerText.Trim(), true);
                            else
                                oUom = new SKU.SKUUOM();

                            Utilities.BuildXmlFromObject(ds, "UOMOBJ", UOMNode, oUom, oLog);

                            oUom.EDITDATE = DateTime.Now;
                            oUom.EDITUSER = "SYSTEM";
                            //oSku.SetUOM(oUom.UOM,oUom.EANUPC,oUom.GROSSWEIGHT,oUom.NETWEIGHT,oUom.LENGTH,oUom.WIDTH,oUom.HEIGHT,oUom.VOLUME,oUom.LOWERUOM,oUom.UNITSPERMEASURE,oUom.SHIPPABLE,oUom.EDITUSER);
                            if (oUom.LOWERUOM == null) oUom.LOWERUOM = string.Empty;
                            oSku.UNITSOFMEASURE.add(oUom);
                        }
                        if (oSku.UNITSOFMEASURE.Count > 0)
                        {
                            oSku.SaveUomCollection("SYSTEM");
                            if (oLog != null) oLog.WriteLine("UOM Collection saved", true);
                        }
                        else
                        {
                            if (oLog != null) oLog.WriteLine("UOM Collection does not contain any UOMs.", true);
                        }
                    }
                    catch (Made4Net.Shared.M4NException m4nex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in importing Sku UOM", true);
                        if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                        oBuilder.AppendLine(m4nex.Description);
                        res = DataMapperTransactionResult.BOFailed;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in importing Sku UOM", true);
                        if (oLog != null) oLog.WriteLine(ex.ToString(), true);
                        oBuilder.AppendLine(ex.Message);
                        res = DataMapperTransactionResult.BOElementFailed;
                    }
                    // BOM Loading
                    if (oLog != null) oLog.WriteLine("Examining BOM Collection", true);
                    foreach (XmlNode BOMNode in SkuNode.SelectNodes("BOMCOLLECTION/BOMOBJ"))
                    {
                        WMS.Logic.SKU.SKUBOM oSkuBOM;
                        if (BOMNode.SelectSingleNode("PARTSKU").InnerText != string.Empty)
                        {
                            if (WMS.Logic.SKU.SKUBOM.Exists(oSku.CONSIGNEE, oSku.SKU, BOMNode.SelectSingleNode("PARTSKU").InnerText))
                            {
                                oSkuBOM = new WMS.Logic.SKU.SKUBOM(oSku.CONSIGNEE, oSku.SKU, BOMNode.SelectSingleNode("PARTSKU").InnerText, true);
                                oSkuBOM.Update(Convert.ToDecimal(BOMNode.SelectSingleNode("PARTQTY").InnerText), BOMNode.SelectSingleNode("BOMORDER").InnerText, "SYSTEM");
                            }
                            else
                            {
                                oSkuBOM = new WMS.Logic.SKU.SKUBOM();
                                oSkuBOM.Create(oSku.CONSIGNEE, oSku.SKU, BOMNode.SelectSingleNode("PARTSKU").InnerText, Convert.ToDecimal(BOMNode.SelectSingleNode("PARTQTY").InnerText), BOMNode.SelectSingleNode("BOMORDER").InnerText, "SYSTEM");
                            }
                        }
                    }

                    if (oLog != null) oLog.WriteLine("BOM Collection saved", true);
                    // SUBSTITUTE Loading
                    foreach (XmlNode SUBNode in SkuNode.SelectNodes("SUBSTITUTESCOLLECTION/SUBOBJ"))
                    {
                        try
                        {
                            WMS.Logic.SKU.SkuSubstitutes oSkuSub;
                            if (SUBNode.SelectSingleNode("SUBSTITUTESKU").InnerText != string.Empty)
                            {
                                if (WMS.Logic.SKU.SkuSubstitutes.Exists(oSku.CONSIGNEE, oSku.SKU, SUBNode.SelectSingleNode("SUBSTITUTESKU").InnerText))
                                {
                                    // Do nothing
                                }
                                else
                                {
                                    oSkuSub = new WMS.Logic.SKU.SkuSubstitutes();
                                    oSkuSub.Create(oSku.CONSIGNEE, oSku.SKU, 0, SUBNode.SelectSingleNode("SUBSTITUTESKU").InnerText, 0, 0, "", DateTime.Now, DateTime.Now, false, "", "", "SYSTEM");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception in importing Sku Substitute", true);
                            if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                            oBuilder.AppendLine(ex.Message);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                    }
                    // Sku Attributes loading
                    // Clear all attributes
                    try
                    {
                        bool shouldSaveAtt = false;
                        foreach (XmlNode SkuAttNode in SkuNode.SelectNodes("SKUATTRIBUTES/SKUATTRIBUTE"))
                        {
                            if (SkuAttNode.SelectSingleNode("ATTRIBUTENAME") != null)
                            {
                                if (SkuAttNode.SelectSingleNode("ATTRIBUTENAME").InnerText != string.Empty)
                                {
                                    oSku.Attributes.Attributes.Remove(SkuAttNode.SelectSingleNode("ATTRIBUTENAME").InnerText);
                                    oSku.Attributes.Attributes.Add(SkuAttNode.SelectSingleNode("ATTRIBUTENAME").InnerText, SkuAttNode.SelectSingleNode("ATTRIBUTEVALUE").InnerText);
                                    if (oLog != null) oLog.WriteLine("Adding sku attribute " + SkuAttNode.SelectSingleNode("ATTRIBUTENAME").InnerText + " value " + SkuAttNode.SelectSingleNode("ATTRIBUTEVALUE").InnerText, true);
                                    shouldSaveAtt = true;
                                }
                            }
                        }
                        if (shouldSaveAtt)
                        {
                            oSku.Attributes.Save();
                            if (oLog != null) oLog.WriteLine("Sku attributes saved", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in importing Sku Attributes", true);
                        if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                        oBuilder.AppendLine(ex.Message);
                        res = DataMapperTransactionResult.BOElementFailed;
                    }

                    #endregion

                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Sku BO", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Sku BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }
            }
            if (oLog != null) oLog.WriteLine("Finished to import Sku BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "Outbound Methods"

        public DataMapperTransactionResult ProcessOutbound(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.OUTBOUND ds = new DataTemplate.OUTBOUND();
            if (oLog != null) oLog.WriteLine("Starting to import Outbound BO.", true);
            string headerStatus = string.Empty;
            string strExistingStatus = string.Empty;
            foreach (XmlNode OutNode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (OutNode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"

                    WMS.Logic.OutboundOrderHeader oOutDoc;
                    if (WMS.Logic.OutboundOrderHeader.Exists(OutNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), OutNode.SelectSingleNode("ORDERID").InnerText.Trim()))
                    {
                        oOutDoc = new WMS.Logic.OutboundOrderHeader(OutNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), OutNode.SelectSingleNode("ORDERID").InnerText.Trim(), true);
                    }
                    else
                    {
                        oOutDoc = new WMS.Logic.OutboundOrderHeader();
                        oOutDoc.STATUS = "NEW";
                    }

                    //TODO : Commented
                    //headerStatus = oOutDoc.STATUS;
                    //TODO : END Commented

                    //TODO : get the orderstatus
                    string SqlOrdStatus = string.Format("SELECT STATUS FROM OUTBOUNDORHEADER WHERE orderid='{0}'AND CONSIGNEE= '{1}'", OutNode.SelectSingleNode("ORDERID").InnerText.Trim(), OutNode.SelectSingleNode("CONSIGNEE").InnerText.Trim());
                    //if (oLog != null)
                    //    oLog.WriteLine("Sql : " + SqlOrdStatus, true);
                    string StatusOutboundOrder = Convert.ToString(DataInterface.ExecuteScalar(SqlOrdStatus));
                    //if (oLog != null)
                    //    oLog.WriteLine("Order Status : " + StatusOutboundOrder, true);
                    if (StatusOutboundOrder != "")
                    {
                        headerStatus = StatusOutboundOrder;
                        oOutDoc.STATUS = StatusOutboundOrder;
                    }
                    else
                    {
                        StatusOutboundOrder = "NEW";
                        headerStatus = "NEW";
                        oOutDoc.STATUS = "NEW";
                    }

                    //TODO : END

                    try
                    {
                        strExistingStatus = OutNode.SelectSingleNode("STATUS") == null ? string.Empty : OutNode.SelectSingleNode("STATUS").InnerText;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null)
                        {
                            oLog.WriteLine("No Status node exists" + ex.Message);
                        }
                    }

                    //TODO : Commented
                    //if (string.Compare(headerStatus, "NEW") != 0)
                    //{
                    //    sErrorMessage = "Order no longer available for changes.";
                    //    res = DataMapperTransactionResult.BOFailed;
                    //    return res;
                    //}
                    //TODO : END Commented

                    //TODO : check the outbound status
                    if (string.Compare(StatusOutboundOrder, "NEW") != 0)
                    {
                        sErrorMessage = "Order no longer available for changes.";
                        res = DataMapperTransactionResult.BOFailed;
                        return res;
                    }
                    //TODO : END

                    // Get Outbound order data and create Expert object
                    Utilities.BuildXmlFromObject(ds, "DATA", OutNode, oOutDoc, oLog);
                    if (string.Compare(headerStatus, "NEW") == 0 && (string.Compare(strExistingStatus, "CANCELED") == 0 || string.Compare(strExistingStatus, "CANCELLED") == 0))
                    {
                        oOutDoc.STATUS = "CANCELED";
                        oOutDoc.UpdateOrderStatus("SYSTEM");
                        res = DataMapperTransactionResult.Success;
                        return res;
                    }
                    oOutDoc.Save("SYSTEM");

                    if (oLog != null)
                    {
                        oLog.WriteLine("Outbound Order header saved", true);
                    }
                    // Handle Contact informations
                    XmlNode oContactXmlNode;
                    XmlNode oShiptToXmlNode;
                    try
                    {
                        oShiptToXmlNode = OutNode.SelectSingleNode("SHIPTO");
                    }
                    catch
                    {
                        oShiptToXmlNode = null;
                    }
                    try
                    {
                        oContactXmlNode = OutNode.SelectSingleNode("CONTACT");
                    }
                    catch
                    {
                        oContactXmlNode = null;
                    }

                    string contactID = "";
                    if (oShiptToXmlNode != null && oShiptToXmlNode.InnerText != "")
                    {
                        contactID = oShiptToXmlNode.InnerText;
                        if (oLog != null)
                            oLog.WriteLine("Using ShipTo:" + contactID, true);
                    }
                    if (oContactXmlNode != null)
                    {
                        WMS.Logic.Contact oContact = new WMS.Logic.Contact();
                        if (oLog != null)
                            oLog.WriteLine("Starting loading contact BO", true);
                        Utilities.BuildXmlFromObject(ds, "CONTACT", OutNode.SelectSingleNode("CONTACT"), oContact, oLog);
                        if (oShiptToXmlNode != null && oShiptToXmlNode.InnerText != "")
                            oContact.CONTACTID = oShiptToXmlNode.InnerText;
                        else
                            oContact.CONTACTID = Made4Net.Shared.Util.getNextCounter("CONTACTID");
                        oContact.Save("SYSTEM");
                        //oOutDoc.SetShipTo(oContact.CONTACTID);
                        if (oLog != null)
                            oLog.WriteLine("Contact BO Saved", true);
                        contactID = oContact.CONTACTID;
                    }

                    if (contactID != "" && !WMS.Logic.Contact.Exists((contactID)))
                    {
                        contactID = "";
                        if (oLog != null)
                            oLog.WriteLine("ShipTo " + contactID + " does not exist. Using default contact.", true);
                    }
                    if (contactID != "" && !WMS.Logic.CompanyContact.Exists(oOutDoc.CONSIGNEE, oOutDoc.TARGETCOMPANY, oOutDoc.COMPANYTYPE, contactID))
                    {
                        if (oLog != null)
                            oLog.WriteLine("Adding shipto " + contactID + " to companycontacts table.", true);
                        CompanyContact cont = new CompanyContact();
                        cont.CONSIGNEE = oOutDoc.CONSIGNEE;
                        cont.COMPANY = oOutDoc.TARGETCOMPANY;
                        cont.COMPANYTYPE = oOutDoc.COMPANYTYPE;
                        cont.CONTACTID = contactID;
                        cont.CreateCompanyContact(WMS.Lib.USERS.SYSTEMUSER);
                    }

                    if (contactID != "")
                    {
                        if (oLog != null)
                            oLog.WriteLine("Setting shipto to : " + contactID, true);
                        oOutDoc.SetShipTo(contactID);
                    }
                    else
                    {
                        string defaultContact = new Company(oOutDoc.CONSIGNEE, oOutDoc.TARGETCOMPANY, oOutDoc.COMPANYTYPE, true).DEFAULTCONTACTOBJ.CONTACTID;
                        if (oLog != null)
                            oLog.WriteLine("Setting shipto to company default contact " + defaultContact, true);
                        oOutDoc.SetShipTo(defaultContact);
                    }

                    #region "Outbound Lines Region"

                    foreach (XmlNode LineNode in OutNode.SelectNodes("LINES/LINE"))
                    {
                        try
                        {
                            WMS.Logic.OutboundOrderHeader.OutboundOrderDetail oLine;
                            if (WMS.Logic.OutboundOrderHeader.OutboundOrderDetail.Exists(oOutDoc.CONSIGNEE, oOutDoc.ORDERID, Convert.ToInt32(LineNode.SelectSingleNode("ORDERLINE").InnerText.Trim())))
                                oLine = new WMS.Logic.OutboundOrderHeader.OutboundOrderDetail(oOutDoc.CONSIGNEE, oOutDoc.ORDERID, Convert.ToInt32(LineNode.SelectSingleNode("ORDERLINE").InnerText.Trim()), true);
                            else
                                oLine = new WMS.Logic.OutboundOrderHeader.OutboundOrderDetail();

                            oLine.CONSIGNEE = oOutDoc.CONSIGNEE;
                            oLine.ORDERID = oOutDoc.ORDERID;

                            Utilities.BuildXmlFromObject(ds, "LINE", LineNode, oLine, oLog);

                            WMS.Logic.AttributesCollection vAtts = new WMS.Logic.AttributesCollection();
                            WMS.Logic.InventoryAttributeBase vAttsBase = new WMS.Logic.InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.OutboundOrder, oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE.ToString());

                            foreach (XmlNode AttNode in LineNode.SelectNodes("ATTRIBUTES/ATTRIBUTE"))
                            {
                                //RWMS-2937 Start
                                if (AttNode.HasChildNodes)
                                {
                                    vAtts.Add(AttNode.SelectSingleNode("ATTNAME").InnerText, AttNode.SelectSingleNode("ATTVALUE").InnerText);
                                }
                                //RWMS-2937 End
                            }
                            vAttsBase.Add(vAtts);
                            oLine.Attributes = vAttsBase;
                            oOutDoc.AddLine(oLine);
                            //RWMS-1509 and RWMS-1502 Start
                              if (oLine.SKU != oLine.INPUTSKU)
                              {
                              oLine.INPUTSKU = oLine.SKU;
                              }
                              //RWMS-1509 and RWMS-1502 End

                            oLine = null;
                        }
                        catch (Made4Net.Shared.M4NException m4nex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception in importing Outbound Line BO [M4NEx]", true);
                            if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                            oBuilder.AppendLine(m4nex.Description);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception in importing Oubound Line BO", true);
                            if (oLog != null) oLog.WriteLine(ex.ToString(), true);
                            oBuilder.AppendLine(ex.Message);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                    }

                    #endregion

                    #endregion
                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Outbound BO [M4NEx]", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Oubound BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }
            }
            if (oLog != null) oLog.WriteLine("Finished to import Outbound BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "Inbound Methods"

        public DataMapperTransactionResult ProcessInbound(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.INBOUND ds = new DataTemplate.INBOUND();

            if (oLog != null) oLog.WriteLine("Starting to import Inbound BO", true);
            foreach (XmlNode InNode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (InNode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"

                    WMS.Logic.InboundOrderHeader oInDoc;
                    if (WMS.Logic.InboundOrderHeader.Exists(InNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), InNode.SelectSingleNode("ORDERID").InnerText.Trim()))
                        oInDoc = new WMS.Logic.InboundOrderHeader(InNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), InNode.SelectSingleNode("ORDERID").InnerText.Trim(), true);
                    else
                        oInDoc = new WMS.Logic.InboundOrderHeader();

                    Utilities.BuildXmlFromObject(ds, "DATA", InNode, oInDoc, oLog);

                    oInDoc.Save();
                    if (oLog != null)
                    {
                        oLog.WriteLine("Inbound Order header saved", true);
                    }

                    // Handle Contact informations
                    XmlNode oContactXmlNode;
                    XmlNode oRecFromXmlNode;
                    try
                    {
                        oRecFromXmlNode = InNode.SelectSingleNode("RECEIVEDFROM");
                    }
                    catch
                    {
                        oRecFromXmlNode = null;
                    }
                    try
                    {
                        oContactXmlNode = InNode.SelectSingleNode("CONTACT");
                    }
                    catch
                    {
                        oContactXmlNode = null;
                    }

                    string contactID = "";
                    if (oRecFromXmlNode != null && oRecFromXmlNode.InnerText != "")
                    {
                        contactID = oRecFromXmlNode.InnerText;
                        if (oLog != null)
                            oLog.WriteLine("Using RECEIVEDFROM:" + contactID, true);
                    }
                    if (oContactXmlNode != null)
                    {
                        WMS.Logic.Contact oContact = new WMS.Logic.Contact();
                        if (oLog != null)
                            oLog.WriteLine("Starting loading contact BO", true);
                        Utilities.BuildXmlFromObject(ds, "CONTACT", InNode.SelectSingleNode("CONTACT"), oContact, oLog);
                        if (oRecFromXmlNode != null && oRecFromXmlNode.InnerText != "")
                            oContact.CONTACTID = oRecFromXmlNode.InnerText;
                        else
                            oContact.CONTACTID = Made4Net.Shared.Util.getNextCounter("CONTACTID");
                        oContact.Save("SYSTEM");
                        if (oLog != null)
                            oLog.WriteLine("Contact BO Saved", true);
                        contactID = oContact.CONTACTID;
                    }

                    if (contactID != "" && !WMS.Logic.Contact.Exists((contactID)))
                    {
                        contactID = "";
                        if (oLog != null)
                            oLog.WriteLine("Received from " + contactID + " does not exist. Using default contact.", true);
                    }

                    if (contactID != "" && !WMS.Logic.CompanyContact.Exists(oInDoc.CONSIGNEE, oInDoc.SOURCECOMPANY, oInDoc.COMPANYTYPE, contactID))
                    {
                        if (oLog != null)
                            oLog.WriteLine("Adding received from  " + contactID + " to company contacts table.", true);
                        CompanyContact cont = new CompanyContact();
                        cont.CONSIGNEE = oInDoc.CONSIGNEE;
                        cont.COMPANY = oInDoc.SOURCECOMPANY;
                        cont.COMPANYTYPE = oInDoc.COMPANYTYPE;
                        cont.CONTACTID = contactID;
                        cont.CreateCompanyContact(WMS.Lib.USERS.SYSTEMUSER);
                    }

                    if (contactID != "")
                    {
                        if (oLog != null)
                            oLog.WriteLine("Setting received from to : " + contactID, true);
                        oInDoc.SetReceivedFrom(contactID);
                    }
                    else
                    {
                        string defaultContact = new Company(oInDoc.CONSIGNEE, oInDoc.SOURCECOMPANY, oInDoc.COMPANYTYPE, true).DEFAULTCONTACTOBJ.CONTACTID;
                        if (oLog != null)
                            oLog.WriteLine("Setting received from to company default contact " + defaultContact, true);
                        oInDoc.SetReceivedFrom(defaultContact);
                    }

                    foreach (XmlNode LineNode in InNode.SelectNodes("LINES/LINE"))
                    {
                        try
                        {
                            WMS.Logic.InboundOrderDetail oLine;
                            if (WMS.Logic.InboundOrderHeader.LineExists(oInDoc.CONSIGNEE, oInDoc.ORDERID, Convert.ToInt32(LineNode.SelectSingleNode("ORDERLINE").InnerText.Trim())))
                                oLine = new WMS.Logic.InboundOrderDetail(oInDoc.CONSIGNEE, oInDoc.ORDERID, Convert.ToInt32(LineNode.SelectSingleNode("ORDERLINE").InnerText.Trim()), true);
                            else
                                oLine = new WMS.Logic.InboundOrderDetail();

                            // Build line
                            Utilities.BuildXmlFromObject(ds, "LINE", LineNode, oLine, oLog);
                            // Set Attributes
                            WMS.Logic.AttributesCollection vAtts = new WMS.Logic.AttributesCollection();
                            WMS.Logic.InventoryAttributeBase vAttsBase = new WMS.Logic.InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.InboundOrder, oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE.ToString());

                            foreach (XmlNode AttNode in LineNode.SelectNodes("ATTRIBUTES/ATTRIBUTE"))
                            {
                                vAtts.Add(AttNode.SelectSingleNode("ATTNAME").InnerText, AttNode.SelectSingleNode("ATTVALUE").InnerText);
                            }
                            vAttsBase.Add(vAtts);
                            oLine.Attributes = vAttsBase;
                            oInDoc.setLine(oLine);
                            if (oLog != null)
                            {
                                oLog.WriteLine("Line exported | Sku : " + oLine.SKU + " Quantity : " + oLine.QTYORDERED, true);
                            }

                            oLine = null;
                        }
                        catch (Made4Net.Shared.M4NException m4nex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception[Made4Net] while populating Assembly Order Sku Component", true);
                            if (oLog != null) oLog.WriteLine(m4nex.Description, true);
                            oBuilder.AppendLine(m4nex.Description);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception in importing Inbound Line BO", true);
                            if (oLog != null) oLog.WriteLine(ex.ToString(), true);
                            oBuilder.AppendLine(ex.Message);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                    }
                    #endregion
                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception[Made4Net] in importing Inbound BO", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception[System] in importing Inbound BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    if (oLog != null) oLog.WriteLine(ex.StackTrace.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }
            }
            if (oLog != null) oLog.WriteLine("Finished to import Inbound BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "Assembly Order Methods"

        public DataMapperTransactionResult ProcessAssemblyOrder(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.ASSEMBLYWORKORDER ds = new DataTemplate.ASSEMBLYWORKORDER();
            if (oLog != null) oLog.WriteLine("Starting importing Assembly Order BO", true);
            foreach (XmlNode WONode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (WONode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"

                    WMS.Logic.WorkOrderHeader oWODoc;
                    if (WMS.Logic.WorkOrderHeader.Exists(WONode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), WONode.SelectSingleNode("ORDERID").InnerText.Trim()))
                        oWODoc = new WMS.Logic.WorkOrderHeader(WONode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), WONode.SelectSingleNode("ORDERID").InnerText.Trim(), true);
                    else
                        oWODoc = new WMS.Logic.WorkOrderHeader();

                    Utilities.BuildXmlFromObject(ds, "DATA", WONode, oWODoc, oLog);

                    WMS.Logic.AttributesCollection vHeaderAtts = new WMS.Logic.AttributesCollection();
                    WMS.Logic.InventoryAttributeBase vAttsBase = new WMS.Logic.InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.WorkOrder, oWODoc.CONSIGNEE, oWODoc.ORDERID);
                    foreach (XmlNode AttNode in WONode.SelectNodes("ATTRIBUTES/ATTRIBUTE"))
                    {
                        vHeaderAtts.Add(AttNode.SelectSingleNode("NAME").InnerText, AttNode.SelectSingleNode("VALUE").InnerText);
                    }
                    vAttsBase.Add(vHeaderAtts);
                    oWODoc.ATTRIBUTES = vAttsBase;

                    oWODoc.Save("SYSTEM");

                    if (oLog != null) oLog.WriteLine("Assembly Order saved.", true);

                    if (oLog != null) oLog.WriteLine("Populating work Assembly Order Sku Component.", true);
                    foreach (XmlNode LineNode in WONode.SelectNodes("SKUCOMPONENTS/SKUCOMPONENT"))
                    {
                        try
                        {
                            WMS.Logic.AttributesCollection vAtts = new WMS.Logic.AttributesCollection();
                            foreach (XmlNode AttNode in LineNode.SelectNodes("ATTRIBUTES/ATTRIBUTE"))
                            {
                                vAtts.Add(AttNode.SelectSingleNode("NAME").InnerText, AttNode.SelectSingleNode("VALUE").InnerText);
                            }
                            if (WorkOrderBOM.Exists(oWODoc.CONSIGNEE, oWODoc.ORDERID, LineNode.SelectSingleNode("PARTSKU").InnerText))
                                oWODoc.EditPartSku(LineNode.SelectSingleNode("PARTSKU").InnerText, Convert.ToDecimal(LineNode.SelectSingleNode("PARTQTY").InnerText), LineNode.SelectSingleNode("INVENTORYSTATUS").InnerText, vAtts, WMS.Lib.USERS.SYSTEMUSER);
                            else
                                oWODoc.AddPartSku(LineNode.SelectSingleNode("PARTSKU").InnerText, Convert.ToDecimal(LineNode.SelectSingleNode("PARTQTY").InnerText), LineNode.SelectSingleNode("INVENTORYSTATUS").InnerText, vAtts, "SYSTEM");
                        }
                        catch (Made4Net.Shared.M4NException m4nex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception[Made4Net] while populating Assembly Order Sku Component", true);
                            if (oLog != null) oLog.WriteLine(m4nex.Description, true);
                            oBuilder.AppendLine(m4nex.Description);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception[System] while populating Assembly Order Sku Component", true);
                            if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                            oBuilder.AppendLine(ex.Message);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                    }
                    #endregion
                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception[Made4Net] while importing Assembly Order BO", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception[System] while importing Assembly Order BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }
            }
            if (oLog != null) oLog.WriteLine("Finished importing Assembly Order BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "DisAssembly Order Methods"

        public DataMapperTransactionResult ProcessDisAssemblyOrder(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.DISASSEMBLYWORKORDER ds = new DataTemplate.DISASSEMBLYWORKORDER();
            if (oLog != null) oLog.WriteLine("Starting importing DisAssembly Order BO", true);
            foreach (XmlNode WONode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (WONode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"

                    WMS.Logic.WorkOrderHeader oWODoc;
                    if (WMS.Logic.WorkOrderHeader.Exists(WONode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), WONode.SelectSingleNode("ORDERID").InnerText.Trim()))
                        oWODoc = new WMS.Logic.WorkOrderHeader(WONode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), WONode.SelectSingleNode("ORDERID").InnerText.Trim(), true);
                    else
                        oWODoc = new WMS.Logic.WorkOrderHeader();

                    Utilities.BuildXmlFromObject(ds, "DATA", WONode, oWODoc, oLog);

                    WMS.Logic.AttributesCollection vHeaderAtts = new WMS.Logic.AttributesCollection();
                    WMS.Logic.InventoryAttributeBase vAttsBase = new WMS.Logic.InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.WorkOrder, oWODoc.CONSIGNEE, oWODoc.ORDERID);
                    foreach (XmlNode AttNode in WONode.SelectNodes("ATTRIBUTES/ATTRIBUTE"))
                    {
                        vHeaderAtts.Add(AttNode.SelectSingleNode("NAME").InnerText, AttNode.SelectSingleNode("VALUE").InnerText);
                    }
                    vAttsBase.Add(vHeaderAtts);
                    oWODoc.ATTRIBUTES = vAttsBase;

                    oWODoc.Save("SYSTEM");
                    if (oLog != null) oLog.WriteLine("DisAssembly Order saved.", true);

                    if (oLog != null) oLog.WriteLine("Populating work DisAssembly Order Sku Component.", true);
                    foreach (XmlNode LineNode in WONode.SelectNodes("SKUCOMPONENTS/SKUCOMPONENT"))
                    {
                        try
                        {
                            oWODoc.AddPartSku(LineNode.SelectSingleNode("PARTSKU").InnerText, Convert.ToDecimal(LineNode.SelectSingleNode("PARTQTY").InnerText), LineNode.SelectSingleNode("INVENTORYSTATUS").InnerText, null, "SYSTEM");
                        }
                        catch (Made4Net.Shared.M4NException m4nex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception[Made4Net] while populating DisAssembly Order Sku Component", true);
                            if (oLog != null) oLog.WriteLine(m4nex.Description, true);
                            oBuilder.AppendLine(m4nex.Description);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception[System] while populating DisAssembly Order Sku Component", true);
                            if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                            oBuilder.AppendLine(ex.Message);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                    }

                    #endregion

                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception[Made4Net] while importing DisAssembly Order BO", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception[System] while importing DisAssembly Order BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }
            }
            if (oLog != null) oLog.WriteLine("Finished importing DisAssembly Order BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "ValueAdded Order Methods"

        public DataMapperTransactionResult ProcessValueAddedOrder(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.VALUEADDEDSERVICES ds = new DataTemplate.VALUEADDEDSERVICES();

            if (oLog != null) oLog.WriteLine("Starting importing ValueAdded Order BO", true);
            foreach (XmlNode WONode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (WONode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"
                    WMS.Logic.WorkOrderHeader oWODoc;
                    if (WMS.Logic.WorkOrderHeader.Exists(WONode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), WONode.SelectSingleNode("ORDERID").InnerText.Trim()))
                        oWODoc = new WMS.Logic.WorkOrderHeader(WONode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), WONode.SelectSingleNode("ORDERID").InnerText.Trim(), true);
                    else
                        oWODoc = new WMS.Logic.WorkOrderHeader();

                    Utilities.BuildXmlFromObject(ds, "DATA", WONode, oWODoc, oLog);

                    WMS.Logic.AttributesCollection vHeaderAtts = new WMS.Logic.AttributesCollection();
                    WMS.Logic.InventoryAttributeBase vAttsBase = new WMS.Logic.InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.WorkOrder, oWODoc.CONSIGNEE, oWODoc.ORDERID);
                    foreach (XmlNode AttNode in WONode.SelectNodes("ATTRIBUTES/ATTRIBUTE"))
                    {
                        vHeaderAtts.Add(AttNode.SelectSingleNode("NAME").InnerText, AttNode.SelectSingleNode("VALUE").InnerText);
                    }
                    vAttsBase.Add(vHeaderAtts);
                    oWODoc.ATTRIBUTES = vAttsBase;

                    oWODoc.Save("SYSTEM");
                    if (oLog != null) oLog.WriteLine("ValueAdded Order saved.", true);

                    #endregion

                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception[Made4Net] while importing ValueAdded Order BO", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception[System] while importing ValueAdded Order BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }
            }
            if (oLog != null) oLog.WriteLine("Finished importing ValueAdded Order BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "Receipt Methods"

        public DataMapperTransactionResult ProcessReceipt(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.RECEIPT ds = new DataTemplate.RECEIPT();

            if (oLog != null) oLog.WriteLine("Starting to import Receipt BO", true);
            foreach (XmlNode InNode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (InNode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"

                    WMS.Logic.ReceiptHeader oRecDoc;
                    if (WMS.Logic.ReceiptHeader.Exists(InNode.SelectSingleNode("RECEIPT").InnerText.Trim()))
                        oRecDoc = new WMS.Logic.ReceiptHeader(InNode.SelectSingleNode("RECEIPT").InnerText.Trim(), true);
                    else
                        oRecDoc = new WMS.Logic.ReceiptHeader();

                    Utilities.BuildXmlFromObject(ds, "DATA", InNode, oRecDoc, oLog);

                    //oRecDoc.CreateNew(oRecDoc.RECEIPT, oRecDoc.SCHEDULEDDATE, oRecDoc.BOL, oRecDoc.NOTES, oRecDoc.CARRIERCOMPANY, oRecDoc.VEHICLE, oRecDoc.TRAILER, oRecDoc.DRIVER1, oRecDoc.DRIVER2, oRecDoc.SEAL1, oRecDoc.SEAL2, oRecDoc.TRANSPORTREFERENCE, oRecDoc.TRANSPORTTYPE, oRecDoc.DOOR, "SCExpertConnect");
                    oRecDoc.Save("SCExpertConnect");

                    if (oLog != null) oLog.WriteLine("Lines found for receipt " + oRecDoc.RECEIPT + " : " + InNode.SelectNodes("LINES/LINE").Count, true);

                    foreach (XmlNode LineNode in InNode.SelectNodes("LINES/LINE"))
                    {
                        try
                        {
                            WMS.Logic.ReceiptDetail oRecDetail;
                            if (Convert.ToBoolean(WMS.Logic.ReceiptHeader.LineExists(oRecDoc.RECEIPT, Convert.ToInt32(LineNode.SelectSingleNode("RECEIPTLINE").InnerText.Trim()))))
                            {
                                //if (oLog != null) oLog.WriteLine("Trying to load receipt line from DB...", true);
                                oRecDetail = new WMS.Logic.ReceiptDetail(oRecDoc.RECEIPT, Convert.ToInt32(LineNode.SelectSingleNode("RECEIPTLINE").InnerText.Trim()), true);
                            }
                            else
                            {
                                //if (oLog != null) oLog.WriteLine("Creating new receipt detail object", true);
                                oRecDetail = new WMS.Logic.ReceiptDetail();
                            }

                            Utilities.BuildXmlFromObject(ds, "LINE", LineNode, oRecDetail, oLog);

                            WMS.Logic.AttributesCollection vRLAtts = new WMS.Logic.AttributesCollection();
                            WMS.Logic.InventoryAttributeBase vAttsBase = new WMS.Logic.InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Receipt, oRecDetail.CONSIGNEE, oRecDetail.RECEIPT, oRecDetail.RECEIPTLINE.ToString());

                            foreach (XmlNode AttNode in LineNode.SelectNodes("ATTRIBUTES/ATTRIBUTE"))
                            {
                                if (oLog != null) oLog.WriteLine("Creating receipt line detail attributes : ", true);
                                if (oLog != null) oLog.WriteLine("NAME : " + AttNode.SelectSingleNode("ATTNAME").InnerText, true);
                                if (oLog != null) oLog.WriteLine("VALUE : " + AttNode.SelectSingleNode("ATTVALUE").InnerText, true);
                                vRLAtts.Add(AttNode.SelectSingleNode("ATTNAME").InnerText, AttNode.SelectSingleNode("ATTVALUE").InnerText);
                            }
                            vAttsBase.Add(vRLAtts);
                            int NewReceiptLineNum = -1;
                            NewReceiptLineNum = (int)oRecDoc.addLine(oRecDetail.CONSIGNEE, oRecDetail.SKU, oRecDetail.QTYEXPECTED, oRecDetail.REFERENCEORDER, oRecDetail.REFERENCEORDERLINE, oRecDetail.INVENTORYSTATUS, oRecDetail.COMPANY, oRecDetail.COMPANYTYPE, -1, "", 0, "SCExpertConnect", vRLAtts, oRecDetail.INPUTQTY, oRecDetail.INPUTUOM, oRecDetail.INPUTSKU, oRecDetail.RECEIPTLINE, oRecDetail.DOCUMENTTYPE, oRecDetail.ORDERID, oRecDetail.ORDERLINE);
                            //oRecDetail.Attributes = vAttsBase;
                            //oRecDoc.SaveLine(oRecDetail, "SCExpertConnect");

                            if (NewReceiptLineNum == -1)
                                NewReceiptLineNum = oRecDetail.RECEIPTLINE;

                            if (oLog != null)
                            {
                                oLog.WriteLine("Line Sku : " + oRecDetail.SKU + " Quantity : " + oRecDetail.QTYEXPECTED, true);
                            }

                            foreach (XmlNode AsnNode in LineNode.SelectNodes("ASNS/ASN"))
                            {
                                if (oLog != null) oLog.WriteLine("Adding ASN detail ", true);
                                WMS.Logic.AttributesCollection vAtts = new WMS.Logic.AttributesCollection();
                                foreach (XmlNode AttNode in AsnNode.SelectNodes("ASNATTRIBUTES/ASNATTRIBUTE"))
                                {
                                    if (oLog != null) oLog.WriteLine("Creating ASN detail attributes : ", true);
                                    if (oLog != null) oLog.WriteLine("NAME : " + AttNode.SelectSingleNode("NAME").InnerText, true);
                                    if (oLog != null) oLog.WriteLine("VALUE : " + AttNode.SelectSingleNode("VALUE").InnerText, true);
                                    vAtts.Add(AttNode.SelectSingleNode("NAME").InnerText, AttNode.SelectSingleNode("VALUE").InnerText);
                                }
                                WMS.Logic.AsnDetail vASN = new WMS.Logic.AsnDetail();

                                vASN.Create(AsnNode.SelectSingleNode("ASNID").InnerText, oRecDoc.RECEIPT, oRecDetail.RECEIPTLINE, AsnNode.SelectSingleNode("CONTAINER").InnerText, AsnNode.SelectSingleNode("UOM").InnerText, Convert.ToDecimal(AsnNode.SelectSingleNode("UNITS").InnerText), AsnNode.SelectSingleNode("LOADID").InnerText, vAtts, "SCExpertConnect");

                                vAtts = null;
                                vASN = null;
                            }

                            //oLine = null;
                            oRecDetail = null;
                            if (oLog != null)
                            {
                                oLog.WriteLine("Receipt line added ", true);
                            }
                        }
                        catch (Made4Net.Shared.M4NException m4nex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception while importing receipt line...", true);
                            if (oLog != null) oLog.WriteLine(m4nex.Description, true);
                            oBuilder.AppendLine(m4nex.Description);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception while importing receipt line...", true);
                            if (oLog != null) oLog.WriteLine(ex.ToString(), true);
                            oBuilder.AppendLine(ex.Message);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                    }
                    #endregion
                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Receipt BO", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Receipt BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }
            }
            if (oLog != null) oLog.WriteLine("Finished to import Receipt BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "Contact Methods"

        public DataMapperTransactionResult ProcessContact(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.CONTACT ds = new DataTemplate.CONTACT();

            if (oLog != null) oLog.WriteLine("Starting to import Contact BO", true);
            foreach (XmlNode ContactNode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (ContactNode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"

                    WMS.Logic.Contact oContact = new WMS.Logic.Contact();
                    Utilities.BuildXmlFromObject(ds, "DATA", ContactNode, oContact, oLog);
                    oContact.CONTACTID = Made4Net.Shared.Util.getNextCounter("CONTACTID");
                    oContact.Save("SYSTEM");
                    if (oLog != null) oLog.WriteLine("Contact saved , contact ID is :" + oContact.CONTACTID, true);

                    #endregion
                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Contact BO", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Contact BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }
            }
            if (oLog != null) oLog.WriteLine("Finished to import Contact BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "Shipment Methods"

        public DataMapperTransactionResult ProcessShipment(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.SHIPMENT ds = new DataTemplate.SHIPMENT();

            if (oLog != null) oLog.WriteLine("Starting to import Shipment BO", true);
            foreach (XmlNode InNode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (InNode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"

                    WMS.Logic.Shipment oShipDoc;

                    if (WMS.Logic.Shipment.Exists(InNode.SelectSingleNode("SHIPMENT").InnerText))
                        oShipDoc = new WMS.Logic.Shipment(InNode.SelectSingleNode("SHIPMENT").InnerText, true);
                    else
                        oShipDoc = new WMS.Logic.Shipment();

                    Utilities.BuildXmlFromObject(ds, "DATA", InNode, oShipDoc, oLog);

                    oShipDoc.Save("SCExpertConnect");

                    if (oLog != null) oLog.WriteLine("Lines found for Shipment " + oShipDoc.SHIPMENT + " : " + InNode.SelectNodes("DOCUMENTS/DOCUMENT").Count, true);

                    foreach (XmlNode OrderNode in InNode.SelectNodes("DOCUMENTS/DOCUMENT"))
                    {
                        try
                        {
                            //Commented for PWMS-684
                            //// Need to change the schema for the object so it will import lines.
                            //switch (LineNode.SelectSingleNode("ORDERTYPE").InnerText)
                            //{
                            //    case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH: //"FLOWTHROUGH":
                            //        //oShipDoc.AssignOrder(LineNode.SelectSingleNode("CONSIGNEE").InnerText, LineNode.SelectSingleNode("ORDERID").InnerText, "SCExpertConnect", WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH);
                            //        break;
                            //    case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT: //"TRANSSHIPMENT":
                            //        //oShipDoc.AssignOrder(LineNode.SelectSingleNode("CONSIGNEE").InnerText, LineNode.SelectSingleNode("ORDERID").InnerText, "SCExpertConnect", WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT);
                            //        break;
                            //    case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER: //"OUTBOUND":
                            //        //oShipDoc.AssignOrder(LineNode.SelectSingleNode("CONSIGNEE").InnerText, LineNode.SelectSingleNode("ORDERID").InnerText, "SCExpertConnect", WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER);
                            //        break;
                            //}
                            //if (oLog != null)
                            //{
                            //    oLog.WriteLine("Document added to Shipment", true);
                            //}
                            //End Commented for PWMS-684
                            //Added for PWMS-684
                            foreach (XmlNode LineNode in OrderNode.SelectNodes("LINES/LINE"))
                            {
                                try
                                {
                                    int iOrderLine = System.Convert.ToInt32(LineNode.SelectSingleNode("ORDERLINE").InnerText);
                                    decimal dQty = System.Convert.ToDecimal(LineNode.SelectSingleNode("QTYORIGINAL").InnerText);

                                    // Need to change the schema for the object so it will import lines.
                                    switch (OrderNode.SelectSingleNode("ORDERTYPE").InnerText)
                                    {
                                        case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH: //"FLOWTHROUGH":
                                            oShipDoc.AssignOrder(OrderNode.SelectSingleNode("CONSIGNEE").InnerText, OrderNode.SelectSingleNode("ORDERID").InnerText, iOrderLine, dQty, 0, "SCExpertConnect", WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH);
                                            break;
                                        case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT: //"TRANSSHIPMENT":
                                            //oShipDoc.AssignOrder(OrderNode.SelectSingleNode("CONSIGNEE").InnerText, OrderNode.SelectSingleNode("ORDERID").InnerText, "SCExpertConnect", WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT);
                                            break;
                                        case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER: //"OUTBOUND":
                                            oShipDoc.AssignOrder(OrderNode.SelectSingleNode("CONSIGNEE").InnerText, OrderNode.SelectSingleNode("ORDERID").InnerText, iOrderLine, dQty, 0, "SCExpertConnect", WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER);
                                            break;
                                    }
                                    if (oLog != null)
                                    {
                                        oLog.WriteLine("Document added to Shipment", true);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (oLog != null) oLog.WriteLine("System Exception while importing order line details...", true);
                                    if (oLog != null) oLog.WriteLine(ex.ToString(), true);
                                    oBuilder.AppendLine(ex.Message);
                                    res = DataMapperTransactionResult.BOElementFailed;
                                }
                            }
                            //End Added for PWMS-684
                        }
                        catch (Made4Net.Shared.M4NException m4nex)
                        {
                            //Commented for PWMS-684
                            //if (oLog != null) oLog.WriteLine("Exception while importing receipt line...", true);
                            //End Commented for PWMS-684
                            //Added for PWMS-684
                            if (oLog != null) oLog.WriteLine("Exception while importing order details...", true);
                            //End Added for PWMS-684
                            if (oLog != null) oLog.WriteLine(m4nex.Description, true);
                            oBuilder.AppendLine(m4nex.Description);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                        catch (Exception ex)
                        {
                            //Commented for PWMS-684
                            //if (oLog != null) oLog.WriteLine("Exception while importing receipt line...", true);
                            //End Commented for PWMS-684
                            //Added for PWMS-684
                            if (oLog != null) oLog.WriteLine("System Exception while importing order details...", true);
                            //End Added for PWMS-684
                            if (oLog != null) oLog.WriteLine(ex.ToString(), true);
                            oBuilder.AppendLine(ex.Message);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                    }

                    #endregion

                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Shipment BO", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Shipment BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }
            }
            if (oLog != null) oLog.WriteLine("Finished to import Shipment BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "Flowthrough Methods"

        public DataMapperTransactionResult ProcessFlowthrough(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.FLOWTHROUGH ds = new DataTemplate.FLOWTHROUGH();
            if (oLog != null) oLog.WriteLine("Starting to import Flowthrough BO", true);
            foreach (XmlNode OutNode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (OutNode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"

                    WMS.Logic.Flowthrough oDoc;
                    if (WMS.Logic.Flowthrough.Exists(OutNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), OutNode.SelectSingleNode("FLOWTHROUGH").InnerText.Trim()))
                        oDoc = new WMS.Logic.Flowthrough(OutNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), OutNode.SelectSingleNode("FLOWTHROUGH").InnerText.Trim(), true);
                    else
                        oDoc = new WMS.Logic.Flowthrough();

                    // Get Outbound order data and create Expert object
                    Utilities.BuildXmlFromObject(ds, "DATA", OutNode, oDoc, oLog);

                    oDoc.Save("SYSTEM");
                    if (oLog != null)
                    {
                        oLog.WriteLine("Flowthrough Order header saved", true);
                    }
                    // Handle Contact informations
                    // Handle Contact informations
                    XmlNode oContactXmlNode;
                    XmlNode oShiptToXmlNode;
                    try
                    {
                        oShiptToXmlNode = OutNode.SelectSingleNode("SHIPTO");
                    }
                    catch
                    {
                        oShiptToXmlNode = null;
                    }
                    try
                    {
                        oContactXmlNode = OutNode.SelectSingleNode("CONTACT");
                    }
                    catch
                    {
                        oContactXmlNode = null;
                    }
                    if (oShiptToXmlNode != null)
                    {
                        if (oContactXmlNode != null && oShiptToXmlNode.InnerText == "")
                        {
                            // Create new contact information record
                            try
                            {
                                WMS.Logic.Contact oContact = new WMS.Logic.Contact();
                                oLog.WriteLine("Starting loading contact BO", true);
                                Utilities.BuildXmlFromObject(ds, "CONTACT", OutNode.SelectSingleNode("CONTACT"), oContact, oLog);
                                oContact.CONTACTID = Made4Net.Shared.Util.getNextCounter("CONTACTID");
                                oContact.Save("SYSTEM");
                                oDoc.SetShipTo(oContact.CONTACTID);
                                oLog.WriteLine("Contact BO Saved", true);
                            }
                            catch (Exception ex)
                            {
                                if (oLog != null)
                                {
                                    oLog.WriteLine("Exception creating contact data : " + ex.ToString(), true);
                                }
                            }
                        }
                    }

                    #region "Flowthrough Lines Region"

                    foreach (XmlNode LineNode in OutNode.SelectNodes("LINES/LINE"))
                    {
                        try
                        {
                            WMS.Logic.FlowthroughDetail oLine;
                            if (WMS.Logic.FlowthroughDetail.Exists(OutNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), OutNode.SelectSingleNode("FLOWTHROUGH").InnerText.Trim(), Convert.ToInt32(LineNode.SelectSingleNode("FLOWTHROUGHLINE").InnerText.Trim())))
                                oLine = new WMS.Logic.FlowthroughDetail(oDoc.CONSIGNEE, oDoc.FLOWTHROUGH, Convert.ToInt32(LineNode.SelectSingleNode("FLOWTHROUGHLINE").InnerText.Trim()), true);
                            else
                                oLine = new WMS.Logic.FlowthroughDetail();
                            oLine.FLOWTHROUGH = OutNode.SelectSingleNode("FLOWTHROUGH").InnerText.Trim();
                            oLine.CONSIGNEE = OutNode.SelectSingleNode("CONSIGNEE").InnerText.Trim();

                            Utilities.BuildXmlFromObject(ds, "LINE", LineNode, oLine, oLog);

                            oDoc.AddLine(oLine);

                            WMS.Logic.AttributesCollection vAtts = new WMS.Logic.AttributesCollection();
                            foreach (XmlNode AttNode in LineNode.SelectNodes("ATTRIBUTES/ATTRIBUTE"))
                            {
                                vAtts.Add(AttNode.SelectSingleNode("ATTNAME").InnerText, AttNode.SelectSingleNode("ATTVALUE").InnerText);
                            }
                            oLine.EditDetail(oLine.CONSIGNEE, oLine.FLOWTHROUGH, oLine.FLOWTHROUGHLINE, oLine.SKU, oLine.QTYORIGINAL, oLine.INVENTORYSTATUS, oLine.ADDUSER, vAtts, oLine.ROUTE, oLine.STOPNUMBER);
                            oLine = null;
                        }
                        catch (Made4Net.Shared.M4NException m4nex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception in importing Flowthrough BO Line [M4NEx]", true);
                            if (oLog != null) oLog.WriteLine(m4nex.Description, true);
                            oBuilder.AppendLine(m4nex.Description);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception in importing Flowthrough BO Line", true);
                            if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                            oBuilder.AppendLine(ex.Message);
                            res = DataMapperTransactionResult.BOElementFailed;
                        }
                    }

                    #endregion

                    #endregion
                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Flowthrough BO [M4NEx]", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing Flowthrough BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }
            }
            if (oLog != null) oLog.WriteLine("Finished to import Flowthrough BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "Transshipment Methods"
        public XmlDocument ExportTransShipment(string p, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            #region TransShipment
            DataTemplate.TRANSHIPMENT ds = new DataTemplate.TRANSHIPMENT();
            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

            if (oLog != null) oLog.WriteLine("Creating SCExpert BO for TransShipment Export Message.", true);
            if (oLog != null) oLog.WriteSeperator();

            WMS.Logic.TransShipment oTransShipment = new WMS.Logic.TransShipment(qSender.Values["CONSIGNEE"], qSender.Values["DOCUMENT"], true);

            Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, oTransShipment, oLog);
            #endregion
            if (oLog != null)
                oLog.WriteLine("TransShipment Export finished.", true);
            return oDoc;


        }

        public DataMapperTransactionResult ProcessTransshipment(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            DataTemplate.TRANSHIPMENT ds = new DataTemplate.TRANSHIPMENT();
            if (oLog != null) oLog.WriteLine("Starting to import Transshipment BO", true);
            foreach (XmlNode OutNode in FileToProccess.SelectSingleNode("BUSINESSOBJECT").ChildNodes)
            {
                if (OutNode.Name.Equals("BOTYPE", StringComparison.OrdinalIgnoreCase))
                    continue;
                try
                {
                    #region "Process DATA Node"

                    WMS.Logic.TransShipment oDoc;
                    if (WMS.Logic.TransShipment.Exists(OutNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), OutNode.SelectSingleNode("TRANSSHIPMENT").InnerText.Trim()))
                        oDoc = new WMS.Logic.TransShipment(OutNode.SelectSingleNode("CONSIGNEE").InnerText.Trim(), OutNode.SelectSingleNode("TRANSSHIPMENT").InnerText.Trim(), true);
                    else
                        oDoc = new WMS.Logic.TransShipment();

                    // Get Outbound order data and create Expert object
                    Utilities.BuildXmlFromObject(ds, "DATA", OutNode, oDoc, oLog);

                    oDoc.Save("SYSTEM");
                    if (oLog != null)
                    {
                        oLog.WriteLine("TransShipment Order header saved", true);
                    }
                    // Handle Contact informations
                    // Handle Contact informations
                    XmlNode oReceivedFromXmlNode;
                    XmlNode oShipToXmlNode;
                    XmlNode oTargetContactXmlNode;
                    XmlNode oSourceContactXmlNode;
                    try
                    {
                        oShipToXmlNode = OutNode.SelectSingleNode("SHIPTO");
                    }
                    catch
                    {
                        oShipToXmlNode = null;
                    }
                    try
                    {
                        oReceivedFromXmlNode = OutNode.SelectSingleNode("RECEIVEDFROM");
                    }
                    catch
                    {
                        oReceivedFromXmlNode = null;
                    }
                    try
                    {
                        oTargetContactXmlNode = OutNode.SelectSingleNode("TARGETCONTACT");
                    }
                    catch
                    {
                        oTargetContactXmlNode = null;
                    }
                    try
                    {
                        oSourceContactXmlNode = OutNode.SelectSingleNode("SOURCECONTACT");
                    }
                    catch
                    {
                        oSourceContactXmlNode = null;
                    }

                    //ShipTo
                    if (oShipToXmlNode != null)
                    {
                        if (oTargetContactXmlNode != null && oShipToXmlNode.InnerText == "")
                        {
                            // Create new contact information record
                            try
                            {
                                WMS.Logic.Contact oContact = new WMS.Logic.Contact();
                                oLog.WriteLine("Starting loading contact BO", true);
                                Utilities.BuildXmlFromObject(ds, "CONTACT", oTargetContactXmlNode, oContact, oLog);
                                oContact.CONTACTID = Made4Net.Shared.Util.getNextCounter("CONTACTID");
                                oContact.Save("SYSTEM");
                                oDoc.SetShipTo(oContact.CONTACTID);
                                oLog.WriteLine("Contact BO Saved", true);
                            }
                            catch (Exception ex)
                            {
                                if (oLog != null)
                                {
                                    oLog.WriteLine("Exception creating contact data : " + ex.ToString(), true);
                                }
                            }
                        }
                    }

                    //ShipTo
                    if (oReceivedFromXmlNode != null)
                    {
                        if (oSourceContactXmlNode != null && oReceivedFromXmlNode.InnerText == "")
                        {
                            // Create new contact information record
                            try
                            {
                                WMS.Logic.Contact oContact = new WMS.Logic.Contact();
                                oLog.WriteLine("Starting loading contact BO", true);
                                Utilities.BuildXmlFromObject(ds, "CONTACT", oSourceContactXmlNode, oContact, oLog);
                                oContact.CONTACTID = Made4Net.Shared.Util.getNextCounter("CONTACTID");
                                oContact.Save("SYSTEM");
                                oDoc.SetReceivedFrom(oContact.CONTACTID);
                                oLog.WriteLine("Contact BO Saved", true);
                            }
                            catch (Exception ex)
                            {
                                if (oLog != null)
                                {
                                    oLog.WriteLine("Exception creating contact data : " + ex.ToString(), true);
                                }
                            }
                        }
                    }
                    #endregion
                }
                catch (Made4Net.Shared.M4NException m4nex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing TransShipment BO [M4NEx]", true);
                    if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                    oBuilder.AppendLine(m4nex.Description);
                    res = DataMapperTransactionResult.BOFailed;
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in importing TransShipment BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    oBuilder.AppendLine(ex.Message);
                    res = DataMapperTransactionResult.BOFailed;
                }

            }
            if (oLog != null) oLog.WriteLine("Finished to import TransShipment BO", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        #endregion

        #region "Generic Table Methods"

        public DataMapperTransactionResult ProcessGenericTable(XmlDocument FileToProccess, LogFile oLog, ref string sErrorMessage)
        {
            if (oLog != null) oLog.WriteLine("Starting to import data to a generic database table...", true);
            StringBuilder oBuilder = new StringBuilder();
            DataMapperTransactionResult res = DataMapperTransactionResult.Success;
            string sTableName;
            bool streamOpen = false;
            MemoryStream ms = null;
            StreamWriter writer = null;

            try
            {
                if (oLog != null) oLog.WriteLine("Building the destination table schema and loading received data...", true);

                // Remove the BOTYPE node from the received xml so it match to the table dataset schema and could be loaded into the dataset.
                FileToProccess.SelectSingleNode("BUSINESSOBJECT").RemoveChild(FileToProccess.SelectSingleNode("BUSINESSOBJECT/BOTYPE"));

                // Build an appropriate data set to hold the data from the received xml
                sTableName = FileToProccess.SelectSingleNode("BUSINESSOBJECT/DATA/TABLENAME").InnerText;
                // Remove the table name node from the received xml as well...
                FileToProccess.SelectSingleNode("BUSINESSOBJECT/DATA").RemoveChild(FileToProccess.SelectSingleNode("BUSINESSOBJECT/DATA/TABLENAME"));

                DataSet dt = Utilities.BuildDataTableForDatabaseTable(sTableName);

                ms = new MemoryStream();
                writer = new StreamWriter(ms);
                streamOpen = true;
                writer.Write(FileToProccess.InnerXml);
                writer.Flush();
                ms.Position = 0;
                dt.ReadXml(ms);

                if (oLog != null) oLog.WriteLine("Xml content loaded into the dataset...", true);

                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    if (!PostRecordToDatabase(dr, sTableName, oBuilder, oLog))
                        res = DataMapperTransactionResult.BOFailed;
                }
            }
            catch (Made4Net.Shared.M4NException m4nex)
            {
                if (oLog != null) oLog.WriteLine("Exception in importing data into the database...", true);
                if (oLog != null) oLog.WriteLine("Exception : " + m4nex.Description, true);
                oBuilder.AppendLine(m4nex.Description);
                res = DataMapperTransactionResult.BOFailed;
            }
            catch (Exception ex)
            {
                if (oLog != null) oLog.WriteLine("Exception in importing data into the database...", true);
                if (oLog != null) oLog.WriteLine(ex.ToString(), true);
                oBuilder.AppendLine(ex.Message);
                res = DataMapperTransactionResult.BOFailed;
            }
            finally
            {
                if (streamOpen)
                {
                    ms.Dispose();
                    writer.Dispose();
                }
            }
            if (oLog != null) oLog.WriteLine("Finished to import data.", true);
            sErrorMessage = oBuilder.ToString();
            oBuilder = null;
            return res;
        }

        private bool PostRecordToDatabase(DataRow dr, string pTableName, StringBuilder oBuilder, LogFile oLog)
        {
            try
            {
                string sColumns = string.Empty;
                string sValues = string.Empty;
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    sColumns += string.Format("{0},", dc.ColumnName);
                    sValues += string.Format("{0},", Made4Net.Shared.Util.FormatField(dr[dc.ColumnName], "", false));
                }
                sColumns = sColumns.TrimEnd(",".ToCharArray());
                sValues = sValues.TrimEnd(",".ToCharArray());
                string sSql = string.Format("Insert into {0} ({1}) values ({2})", pTableName, sColumns, sValues);
                Made4Net.DataAccess.DataInterface.RunSQL(sSql);
                return true;
            }
            catch (Exception ex)
            {
                if (oLog != null) oLog.WriteLine("Exception in importing data into the database...", true);
                if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                oBuilder.AppendLine(ex.Message);
                return false;
            }
        }

        #endregion

        #endregion

        #region "Export Methods"

        #region "Charges Export Methods"

        public XmlDocument ExportCharges(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.CHARGE ds = new DataTemplate.CHARGE();

            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

            //#region "Charge"

            //if (oLog != null) oLog.WriteLine("Trying to load Charge " + qSender.Values["CHARGEID"], true);
            //string chid = Convert.ToString(qSender.Values["CHARGEID"]);
            //WMS.Logic.ChargeHeader ch = new WMS.Logic.ChargeHeader(chid);

            //Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, ch, oLog);

            //XmlElement oCONSDATA = oDoc.CreateElement("CONSIGNEEDATA");
            //oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oCONSDATA);

            //if (oLog != null) oLog.WriteLine("Consignee data Loaded...", true);
            //XmlElement ConsNode;
            //ConsNode = oDoc.CreateElement("CONSIGNEEDATA");
            //WMS.Logic.Consignee cs = new WMS.Logic.Consignee(ch.Consignee, true);

            //Utilities.BuildObjectFromDocDetail(ds, "CONSIGNEEDATA", oDoc, ConsNode, cs, oLog);

            //oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(ConsNode);

            //XmlElement oLINES = oDoc.CreateElement("LINES");
            //oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oLINES);

            //if (oLog != null) oLog.WriteLine("Found " + ch.ChargeDetails.Count + " Charge line...", true);

            //foreach (WMS.Logic.ChargeDetail oLine in ch.ChargeDetails)
            //{
            //    if (oLog != null) oLog.WriteLine("Loading Charge line...", true);
            //    try
            //    {
            //        XmlElement LineNode;
            //        LineNode = oDoc.CreateElement("LINE");
            //        if (oLog != null) oLog.WriteLine("XML Node created for Line data", true);

            //        Utilities.BuildObjectFromDocDetail(ds, "LINE", oDoc, LineNode, oLine, oLog);

            //        try
            //        {
            //            ch.BillingAgreement = new Agreement(oLine.AgreementName, ch.Consignee, true);
            //            if (oLog != null) oLog.WriteLine("Extracting Agreement line data", true);
            //            WMS.Logic.AgreementDetail ad = ch.BillingAgreement.Lines[oLine.AgreementLine];
            //            Utilities.buildDocLine(oDoc, LineNode, ad.BillBasis.ToString(), "BILLBASIS");
            //            Utilities.buildDocLine(oDoc, LineNode, ad.PricePerUnit.ToString(), "PRICEPERUNIT");
            //        }
            //        catch
            //        {
            //            Utilities.buildDocLine(oDoc, LineNode, "", "BILLBASIS");
            //            Utilities.buildDocLine(oDoc, LineNode, "", "PRICEPERUNIT");
            //        }
            //        if (oLog != null) oLog.WriteLine("XML Node filled with data", true);
            //        oDoc.SelectNodes("DATACOLLECTION/DATA/LINES").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES").Count - 1).AppendChild(LineNode);
            //        LineNode = null;
            //    }
            //    catch (Exception ex)
            //    {
            //        if (oLog != null)
            //        {
            //            oLog.WriteLine("Error processing charge line...", true);
            //            oLog.WriteLine(ex.Message, true);
            //            oLog.WriteLine(ex.StackTrace.ToString(), true);
            //        }
            //    }
            //}

            //#endregion

            //if (oLog != null) oLog.WriteLine("Charge Export finished.", true);
            return oDoc;
        }

        #endregion

        #region "Sku Export Methods"

        public XmlDocument ExportSku(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.SKU ds = new DataTemplate.SKU();
            XmlDocument oDoc = new XmlDocument();

            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

            if (oLog != null) oLog.WriteLine("Creating SCExpert BO for Sku Export Message.", true);
            if (oLog != null) oLog.WriteSeperator();

            #region "Sku"

            WMS.Logic.SKU oSku = new WMS.Logic.SKU(qSender.Values["CONSIGNEE"], qSender.Values["SKU"], true);

            Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, oSku, oLog);

            XmlElement oSKUCLASS = oDoc.CreateElement("SKUCLASS");
            //Utilities.BuildObjectFromDocDetail(ds, "SKUCLASS", oDoc, oSKUCLASS, oSku.SKUClass, oLog);
            oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oSKUCLASS);
            //XmlElement oAttributes = oDoc.CreateElement("ATTRIBUTES");
            //for (int i =0; i< oSku.Attributes.Attributes.Count ; i++)
            //{
            //    XmlElement attributeNode = oDoc.CreateElement("ATTRIBUTE");
            //    Utilities.BuildObjectFromDocDetail(ds, "ATTRIBUTE", oDoc, attributeNode, oSku.Attributes.Attributes[i], oLog);
            //    oDoc.SelectNodes("BUSINESSOBJECT/DATACOLLECTION/DATA/SKUCLASS").Item(oDoc.SelectNodes("BUSINESSOBJECT/DATACOLLECTION/DATA/SKUCLASS").Count - 1).AppendChild(attributeNode);
            //}

            XmlElement oSKUUOM = oDoc.CreateElement("UOMCOLLECTION");
            oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oSKUUOM);
            foreach (WMS.Logic.SKU.SKUUOM uom in oSku.UNITSOFMEASURE)
            {
                XmlElement uomNode = oDoc.CreateElement("UOMOBJ");
                Utilities.BuildObjectFromDocDetail(ds, "UOMOBJ", oDoc, uomNode, uom, oLog);
                oDoc.SelectNodes("DATACOLLECTION/DATA/UOMCOLLECTION").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/UOMCOLLECTION").Count - 1).AppendChild(uomNode);
            }


            XmlElement oSKUBOM = oDoc.CreateElement("BOMCOLLECTION");
            oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oSKUBOM);
            foreach (WMS.Logic.SKU.SKUBOM bom in oSku.BOM)
            {
                XmlElement bomNode = oDoc.CreateElement("BOMOBJ");
                Utilities.BuildObjectFromDocDetail(ds, "BOMOBJ", oDoc, bomNode, bom, oLog);
                oDoc.SelectNodes("DATACOLLECTION/DATA/BOMCOLLECTION").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/BOMCOLLECTION").Count - 1).AppendChild(bomNode);
            }

            #endregion

            if (oLog != null) oLog.WriteLine("Sku Export finished.", true);
            return oDoc;
        }

        #endregion

        #region "Company Export Methods"

        public XmlDocument ExportCompany(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.COMPANY ds = new DataTemplate.COMPANY();
            XmlDocument oDoc = new XmlDocument();

            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

            if (oLog != null) oLog.WriteLine("Creating SCExpert BO for Company Export Message.", true);
            if (oLog != null) oLog.WriteSeperator();

            #region "Company"

            WMS.Logic.Company oCompany = new WMS.Logic.Company(qSender.Values["CONSIGNEE"], qSender.Values["DOCUMENT"], qSender.Values["NOTES"], true);
            Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, oCompany, oLog);

            XmlElement oContactsElem = oDoc.CreateElement("CONTACTS");
            oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oContactsElem);
            foreach (WMS.Logic.CompanyContact oCompContact in oCompany.CONTACTS)
            {
                WMS.Logic.Contact oContact = new WMS.Logic.Contact(oCompContact.CONTACTID, true);
                XmlElement contactNode = oDoc.CreateElement("CONTACT");
                Utilities.BuildObjectFromDocDetail(ds, "CONTACT", oDoc, contactNode, oContact, oLog);
                oDoc.SelectNodes("DATACOLLECTION/DATA/CONTACTS").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/CONTACTS").Count - 1).AppendChild(contactNode);
            }

            #endregion

            if (oLog != null) oLog.WriteLine("Company Export finished.", true);
            return oDoc;
        }

        #endregion

        #region "Create Load Methods"

        public XmlDocument ExportLoad(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

            if (oLog != null) oLog.WriteLine("Creating SCExpert BO for Load Export Message.", true);
            if (oLog != null) oLog.WriteSeperator();

            #region "Load"

            WMS.Logic.Load ld = new WMS.Logic.Load(qSender.Values["FROMLOAD"], true);
            Utilities.buildDocHeader(oDoc, ld.LOADID, "LOADID");
            Utilities.buildDocHeader(oDoc, ld.CONSIGNEE, "CONSIGNEE");
            Utilities.buildDocHeader(oDoc, ld.SKU, "SKU");
            WMS.Logic.SKU sk = new WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU, true);
            Utilities.buildDocHeader(oDoc, sk.UNITPRICE.ToString(), "UNITPRICE");
            Utilities.buildDocHeader(oDoc, ld.ContainerId, "CONTAINERID");
            Utilities.buildDocHeader(oDoc, ld.LOADUOM, "LOADUOM");
            Utilities.buildDocHeader(oDoc, ld.LOCATION, "LOCATION");
            Utilities.buildDocHeader(oDoc, ld.DESTINATIONLOCATION, "DESTINATIONLOCATION");
            Utilities.buildDocHeader(oDoc, ld.HOLDRC, "HOLDRC");
            Utilities.buildDocHeader(oDoc, ld.STATUS, "STATUS");
            Utilities.buildDocHeader(oDoc, ld.ACTIVITYSTATUS, "ACTIVITYSTATUS");
            Utilities.buildDocHeader(oDoc, ld.UNITS.ToString(), "UNITS");
            Utilities.buildDocHeader(oDoc, ld.UNITSALLOCATED.ToString(), "UNITSALLOCATED");
            Utilities.buildDocHeader(oDoc, ld.UNITSPICKED.ToString(), "UNITSPICKED");

            #endregion
            // Receipt Data Add
            #region "Receipt Header"

            try
            {
                XmlElement ReceiptH;
                ReceiptH = oDoc.CreateElement("RECEIPT");
                WMS.Logic.ReceiptHeader vReceipt = new WMS.Logic.ReceiptHeader(ld.RECEIPT, true);
                if (oLog != null) oLog.WriteLine("Receipt BO Loaded.", true);
                if (oLog != null) oLog.WriteLine("Receipt : " + vReceipt.RECEIPT, true);
                Utilities.buildDocLine(oDoc, ReceiptH, vReceipt.RECEIPT, "RECEIPT");
                Utilities.buildDocLine(oDoc, ReceiptH, Utilities.DateTimeToString(vReceipt.SCHEDULEDDATE), "SCHEDULEDDATE");
                Utilities.buildDocLine(oDoc, ReceiptH, vReceipt.STATUS, "STATUS");
                Utilities.buildDocLine(oDoc, ReceiptH, vReceipt.BOL, "BOL");
                Utilities.buildDocLine(oDoc, ReceiptH, vReceipt.NOTES, "NOTES");
                Utilities.buildDocLine(oDoc, ReceiptH, vReceipt.CARRIERCOMPANY, "CARRIERCOMPANY");
                Utilities.buildDocLine(oDoc, ReceiptH, Utilities.DateTimeToString(vReceipt.STARTRECEIPTDATE), "STARTRECEIPTDATE");
                Utilities.buildDocLine(oDoc, ReceiptH, vReceipt.TRANSPORTREFERENCE, "TRANSPORTREFERENCE");
                Utilities.buildDocLine(oDoc, ReceiptH, vReceipt.TRANSPORTTYPE, "TRANSPORTTYPE");
                WMS.Logic.ReceiptDetail oLine = new WMS.Logic.ReceiptDetail(ld.RECEIPT, ld.RECEIPTLINE, true);
                //if (oLog != null) oLog.WriteLine("Receipt line BO Loaded.", true);
                Utilities.buildDocLine(oDoc, ReceiptH, oLine.RECEIPTLINE.ToString(), "RECEIPTLINE");
                Utilities.buildDocLine(oDoc, ReceiptH, oLine.ORDERID.ToString(), "ORDERID");
                WMS.Logic.InboundOrderHeader oOrder = new WMS.Logic.InboundOrderHeader(oLine.CONSIGNEE, oLine.ORDERID, true);
                Utilities.buildDocLine(oDoc, ReceiptH, oOrder.ORDERTYPE.ToString(), "ORDERTYPE");
                Utilities.buildDocLine(oDoc, ReceiptH, oLine.ORDERLINE.ToString(), "ORDERLINE");
                Utilities.buildDocLine(oDoc, ReceiptH, oLine.REFERENCEORDER.ToString(), "REFERENCEORDER");
                Utilities.buildDocLine(oDoc, ReceiptH, oLine.REFERENCEORDERLINE.ToString(), "REFERENCEORDERLINE");
                Utilities.buildDocLine(oDoc, ReceiptH, oLine.QTYEXPECTED.ToString(), "QTYEXPECTED");
                Utilities.buildDocLine(oDoc, ReceiptH, oLine.QTYRECEIVED.ToString(), "QTYRECEIVED");
                Utilities.buildDocLine(oDoc, ReceiptH, oLine.DOCUMENTTYPE.ToString(), "DOCUMENTTYPE");
                oDoc.SelectSingleNode("DATACOLLECTION/DATA").AppendChild(ReceiptH);
            }
            catch (Made4Net.Shared.M4NException m4nex)
            {
                if (oLog != null) oLog.WriteLine("Exception in creating receipt(M4N) for load object", true);
                if (oLog != null) oLog.WriteLine(m4nex.Description, true);
            }
            catch (Exception ex)
            {
                if (oLog != null) oLog.WriteLine("Exception in creating receipt(Ex) for load object", true);
                if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
            }

            #endregion

            #region "Load Attributes"
            try
            {
                if (oLog != null) oLog.WriteLine("Extracting Attributes", true);
                try
                {
                    if (oLog != null) oLog.WriteLine("Extracting Attributes", true);

                    XmlElement oATTRIBUTES = oDoc.CreateElement("ATTRIBUTES");
                    try
                    {
                        oATTRIBUTES = Utilities.BuildAttributeCollectionXmlNode(oDoc, "ATTRIBUTES", "ATTRIBUTE", "NAME", "VALUE", ld.LoadAttributes, oLog);
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception extracting load attributes", true);
                        if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    }
                    oDoc.SelectSingleNode("DATACOLLECTION/DATA").AppendChild(oATTRIBUTES);
                    //oDoc.SelectNodes("BUSINESSOBJECT/DATACOLLECTION/DATA").Item(oDoc.SelectNodes("BUSINESSOBJECT/DATACOLLECTION/DATA").Count - 1).AppendChild(oATTRIBUTES);
                    if (oLog != null) oLog.WriteLine("Finished Exporting Inventory Transaction", true);
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in Inventory Transaction BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                }

                if (oLog != null) oLog.WriteLine("Finished Exporting Inventory Transaction", true);
            }
            catch (Exception ex)
            {
                if (oLog != null) oLog.WriteLine("Exception in Inventory Transaction BO", true);
                if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
            }
            #endregion

            if (oLog != null) oLog.WriteLine("Load Export finished.", true);
            return oDoc;
        }

        #endregion

        #region "Receipt Methods"

        public XmlDocument ExportReceipt(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.RECEIPT ds = new DataTemplate.RECEIPT();

            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

            if (oLog != null) oLog.WriteLine("Creating SCExpert BO for Receipt Close Message.", true);
            if (oLog != null) oLog.WriteSeperator();

            DataTable Receipts = new DataTable();
            DataInterface.FillDataset("SELECT RECEIPT FROM RECEIPTHEADER WHERE RECEIPT = '" + qSender.Values["DOCUMENT"] + "'", Receipts, false, "");
            foreach (DataRow inord in Receipts.Rows)
            {
                WMS.Logic.ReceiptHeader vReceipt = new WMS.Logic.ReceiptHeader(inord["RECEIPT"].ToString(), true);
                if (oLog != null) oLog.WriteLine("Receipt BO Loaded.", true);

                Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, vReceipt, oLog);

                XmlElement oLINES = oDoc.CreateElement("LINES");
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oLINES);
                if (oLog != null) oLog.WriteSeperator();
                if (oLog != null) oLog.WriteLine("Exporting Receipt Lines ...", true);

                foreach (WMS.Logic.ReceiptDetail oLine in vReceipt.LINES)
                {
                    XmlElement LineNode;
                    LineNode = oDoc.CreateElement("LINE");
                    string sOrderType = "";
                    // Add the order type field as well...
                    switch (oLine.DOCUMENTTYPE)
                    {
                        case WMS.Lib.DOCUMENTTYPES.INBOUNDORDER:
                            WMS.Logic.InboundOrderHeader oOrder = null;
                            oOrder = new InboundOrderHeader(oLine.CONSIGNEE, oLine.ORDERID, true);
                            sOrderType = oOrder.ORDERTYPE;
                            break;
                        case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH:
                            WMS.Logic.Flowthrough oFlow = null;
                            oFlow = new Flowthrough(oLine.CONSIGNEE, oLine.ORDERID, true);
                            sOrderType = oFlow.ORDERTYPE;
                            break;
                        default:
                            sOrderType = "";
                            break;
                    }
                    //Utilities.buildDocLine(oDoc, LineNode, oOrder.ORDERTYPE.ToString(), "ORDERTYPE");
                    Utilities.BuildObjectFromDocDetail(ds, "LINE", oDoc, LineNode, oLine, oLog);
                    LineNode.SelectSingleNode("ORDERTYPE").InnerText = sOrderType.ToString();

                    // Add asn loads attributes to the lines
                    XmlElement oASNS = oDoc.CreateElement("ASNS");
                    LineNode.AppendChild(oASNS);
                    oDoc.SelectNodes("DATACOLLECTION/DATA/LINES").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES").Count - 1).AppendChild(LineNode);
                    // Add all loads to the lines
                    try
                    {
                        DataTable RecLineLoads = new DataTable();
                        //DataInterface.FillDataset("SELECT * FROM INVENTORYTRANS WHERE (INVTRNTYPE = 'CREATELOAD') AND (DOCUMENT = '" + vReceipt.RECEIPT + "') AND (LINE = '" + oLine.RECEIPTLINE.ToString() + "') EXCEPT SELECT * FROM INVENTORYTRANS WHERE (INVTRNTYPE = 'UNRCVLOAD') AND (DOCUMENT = '" + vReceipt.RECEIPT + "') AND (LINE = '" + oLine.RECEIPTLINE.ToString() + "') ", RecLineLoads, false, "");
                        string sql = string.Format("select * from INVENTORYTRANS inner join (select loadid from INVENTORYTRANS where DOCUMENT='{0}' and LINE ='{1}' and INVTRNTYPE = 'CREATELOAD' except select loadid from INVENTORYTRANS where DOCUMENT='{0}' and LINE ='{1}' and INVTRNTYPE = 'UNRCVLOAD' ) lds on lds.LOADID = INVENTORYTRANS.LOADID where DOCUMENT='{0}' and LINE ='{1}' and INVTRNTYPE = 'CREATELOAD'",
                            vReceipt.RECEIPT, oLine.RECEIPTLINE.ToString());
                        DataInterface.FillDataset(sql, RecLineLoads, false, "");
                        XmlElement LoadsNode = oDoc.CreateElement("LOADS");
                        oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Count - 1).AppendChild(LoadsNode);

                        foreach (DataRow recLoad in RecLineLoads.Rows)
                        {
                            XmlElement LoadNode = oDoc.CreateElement("LOAD");
                            Utilities.BuildObjectFromDataRow(ds, "LOAD", oDoc, LoadNode, recLoad, oLog);
                            // Add loads attributes to the line
                            WMS.Logic.Load oRecLoad = new WMS.Logic.Load(Convert.ToString(recLoad["LOADID"]), true);
                           //XmlElement oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oRecLoad.LoadAttributes, oLog);

                            //RWMS-748 export only load attributes when SKU classname exists in SKUCLSLOADATT
                               XmlElement oLoadAttributes = oDoc.CreateElement("LOADATTRIBUTES");
                                if (IsSKUClassValidToExportAttributes(Convert.ToString(recLoad["SKU"])) == true)
                                        {
                                            //RWMS-2667 Start
                                            //oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oRecLoad.LoadAttributes, oLog);
                                            string InventoryTranswgt = Convert.ToString(recLoad["WEIGHT"]);
                                            oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oRecLoad.LoadAttributes, InventoryTranswgt, oLog);
                                            //RWMS-2667 End

                                        }
                            LoadNode.AppendChild(oLoadAttributes);
                            //-----------------------------------------------------------------------------------------------------------------------------
                            oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Count - 1).SelectSingleNode("LOADS").AppendChild(LoadNode);
                            LoadNode = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Error occured while generating loads segment for receipt line. Error details:", true);
                        if (oLog != null) oLog.WriteLine(ex.ToString(), true);
                        throw ex;
                    }
                    // Add receiving exceptions to the lines
                    try
                    {
                        DataTable RecLineExceptions = new DataTable();
                        string sql = string.Format("select QTY,REASONCODE from vRECEIVINGEXCEPTION where RECEIPT='{0}' and RECEIPTLINE='{1}'",
                            vReceipt.RECEIPT, oLine.RECEIPTLINE.ToString());
                        DataInterface.FillDataset(sql, RecLineExceptions, false, "");
                        XmlElement ExceptionsNode = oDoc.CreateElement("RECEIVINGEXCEPTIONS");
                        oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Count - 1).AppendChild(ExceptionsNode);

                        foreach (DataRow recEx in RecLineExceptions.Rows)
                        {
                            XmlElement exceptionNode = oDoc.CreateElement("RECEIVINGEXCEPTION");
                            Utilities.BuildObjectFromDataRow(ds, "RECEIVINGEXCEPTION", oDoc, exceptionNode, recEx, oLog);
                            oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Count - 1).SelectSingleNode("RECEIVINGEXCEPTIONS").AppendChild(exceptionNode);
                            exceptionNode = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Error occured while generating exceptions segment for receipt line. Error details:", true);
                        if (oLog != null) oLog.WriteLine(ex.ToString(), true);
                        throw ex;
                    }
                    LineNode = null;
                }
                //Adding the handling units for the current receipt
                XmlElement oHU = oDoc.CreateElement("HANDLINGUNITS");
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oHU);
                DataTable RecHU = new DataTable();
                DataInterface.FillDataset(string.Format("select HUTYPE,sum(HUQTY) as HUQTY from HANDLINGUNITTRANSACTION where transactiontype = 'RECEIPT' and transactiontypeid = '{0}' group by HUTYPE", vReceipt.RECEIPT), RecHU, false, "");
                if (RecHU.Rows.Count > 0)
                {
                    if (oLog != null) oLog.WriteSeperator();
                    if (oLog != null) oLog.WriteLine("Exporting Receipt Handling Units, Total Rows " + RecHU.Rows.Count, true);
                }
                else
                {
                    if (oLog != null) oLog.WriteSeperator();
                    if (oLog != null) oLog.WriteLine("No Handling Units found for Export for this Receipt...", true);
                }
                foreach (DataRow tmpHU in RecHU.Rows)
                {
                    XmlElement HUNode = oDoc.CreateElement("HANDLINGUNIT");
                    Utilities.BuildObjectFromDataRow(ds, "HANDLINGUNIT", oDoc, HUNode, tmpHU, oLog);
                    oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).SelectSingleNode("HANDLINGUNITS").AppendChild(HUNode);
                    HUNode = null;
                }
            }
            if (oLog != null) oLog.WriteLine("Receipt BO export finished.", true);
            return oDoc;
        }


        #endregion

        #region "Outbound Order Methods"

        public XmlDocument ExportOutBound(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.OUTBOUND ds = new DataTemplate.OUTBOUND();
            if (oLog != null) oLog.WriteLine("Proccessing Outbound Order Message...", true);
            XmlDocument oDoc = new XmlDocument();
            try
            {
                WMS.Logic.OutboundOrderHeader OutDoc = new WMS.Logic.OutboundOrderHeader(qSender.Values["CONSIGNEE"], qSender.Values["DOCUMENT"], true);

                //RWMS-2564 RWMS-2563 START
                if (oLog != null) oLog.WriteLine("Get Outbound order header. OrderID : " + qSender.Values["DOCUMENT"].ToString(), true);
                //RWMS-2564 RWMS-2563 END

                oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

                //RWMS-2564 RWMS-2563 START
                if (oLog != null) oLog.WriteLine("Adding XML tags " + @"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>", true);
                //RWMS-2564 RWMS-2563 END

                Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, OutDoc, oLog);

                //RWMS-2564 RWMS-2563 START
                if (oLog != null) oLog.WriteLine("Adding outbound header to XML ", true);
                //RWMS-2564 RWMS-2563 END

                XmlElement oCONTACT = oDoc.CreateElement("CONTACT");
                Utilities.BuildObjectFromDocDetail(ds, "CONTACT", oDoc, oCONTACT, OutDoc.SHIPTO, oLog);

                //RWMS-2564 RWMS-2563 START
                if (oLog != null) oLog.WriteLine("Adding CONTACT  to XML ", true);
                //RWMS-2564 RWMS-2563 END

                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oCONTACT);

                XmlElement oLINES = oDoc.CreateElement("LINES");
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oLINES);

                //RWMS-2563 START
                if (oLog != null) oLog.WriteLine("Adding LINES to XML ", true);
                //RWMS-2563 END

                foreach (WMS.Logic.OutboundOrderHeader.OutboundOrderDetail oLine in OutDoc.Lines)
                {
                    try
                    {
                        XmlElement LineNode;
                        LineNode = oDoc.CreateElement("LINE");

                        Utilities.BuildObjectFromDocDetail(ds, "LINE", oDoc, LineNode, oLine, oLog);

                        // Add line attributes to the line
                       // XmlElement oLineAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "ATTRIBUTES", "ATTRIBUTE", "ATTNAME", "ATTVALUE", oLine.Attributes, oLog);
                       // LineNode.AppendChild(oLineAttributes);
                        //RWMS-758 remvoing the ATTRIBUTES export since LOADATTRIBUES exists
                       //XmlElement oLineAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "ATTRIBUTES", "ATTRIBUTE", "ATTNAME", "ATTVALUE", oLine.Attributes, oLog);
                       //LineNode.AppendChild(oLineAttributes);

                        //-----------------------------------------------------------------------------------------------------------------------------
                        oDoc.SelectNodes("DATACOLLECTION/DATA/LINES").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES").Count - 1).AppendChild(LineNode);
                        try
                        {
                            DataTable OrdLineLoads = new DataTable();
                            //DataInterface.FillDataset("SELECT * FROM INVENTORYTRANS WHERE (INVTRNTYPE = 'SHIPLOAD') AND (CONSIGNEE = '" + OutDoc.CONSIGNEE + "') AND (DOCUMENT = '" + OutDoc.ORDERID + "') AND (LINE = '" + oLine.ORDERLINE.ToString() + "')", OrdLineLoads, false, "");
                           // DataInterface.FillDataset("SELECT OL.LOADID, LOADUOM AS UOM , LD.UNITS AS QTY , LD.STATUS , ISNULL(CN.CONTAINER,OL.LOADID) AS ContainerId, ISNULL(CN.HUTYPE,'') AS HandlingUnitType, ISNULL(CN.SERIAL,'') AS Serial FROM ORDERLOADS OL INNER JOIN LOADS LD ON OL.LOADID=LD.LOADID INNER JOIN PICKDETAIL PD ON OL.PICKLIST=PD.PICKLIST AND OL.PICKLISTLINE=PD.PICKLISTLINE LEFT OUTER JOIN CONTAINER CN ON CN.CONTAINER=LD.HANDLINGUNIT WHERE OL.DOCUMENTTYPE='OUTBOUND' AND (OL.CONSIGNEE = '" + OutDoc.CONSIGNEE + "') AND (OL.ORDERID = '" + OutDoc.ORDERID + "') AND (OL.ORDERLINE = '" + oLine.ORDERLINE.ToString() + "')", OrdLineLoads, false, "");
                           DataInterface.FillDataset("SELECT OL.LOADID, LOADUOM AS UOM , LD.UNITS AS QTY , LD.STATUS ,LD.SKU, ISNULL(CN.CONTAINER,OL.LOADID) AS ContainerId, ISNULL(CN.HUTYPE,'') AS HandlingUnitType, ISNULL(CN.SERIAL,'') AS Serial FROM ORDERLOADS OL INNER JOIN LOADS LD ON OL.LOADID=LD.LOADID INNER JOIN PICKDETAIL PD ON OL.PICKLIST=PD.PICKLIST AND OL.PICKLISTLINE=PD.PICKLISTLINE LEFT OUTER JOIN CONTAINER CN ON CN.CONTAINER=LD.HANDLINGUNIT WHERE OL.DOCUMENTTYPE='OUTBOUND' AND (OL.CONSIGNEE = '" + OutDoc.CONSIGNEE + "') AND (OL.ORDERID = '" + OutDoc.ORDERID + "') AND (OL.ORDERLINE = '" + oLine.ORDERLINE.ToString() + "')", OrdLineLoads, false, "");
                           XmlElement LoadsNode = oDoc.CreateElement("LOADS");
                            oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Count - 1).AppendChild(LoadsNode);

                            foreach (DataRow ordLoad in OrdLineLoads.Rows)
                            {
                                XmlElement LoadNode = oDoc.CreateElement("LOAD");
                                Utilities.BuildObjectFromDataRow(ds, "LOAD", oDoc, LoadNode, ordLoad, oLog);
                                // Add loads attributes to the line
                                WMS.Logic.Load oOrdLoad = new WMS.Logic.Load(Convert.ToString(ordLoad["LOADID"]), true);
                               // XmlElement oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oOrdLoad.LoadAttributes, oLog);
                                //RWMS-758 export validation condition added
                                XmlElement oLoadAttributes = oDoc.CreateElement("LOADATTRIBUTES");
                                    if (IsSKUClassValidToExportAttributes(Convert.ToString(ordLoad["SKU"])) == true)
                                           {
                                               if (oLog != null) oLog.WriteLine("Building load attributes collection...", true);
                                             oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oOrdLoad.LoadAttributes, oLog);
                                           }
                                LoadNode.AppendChild(oLoadAttributes);
                                // Create Container for the loads
                                try
                                {
                                    if (oLog != null) oLog.WriteLine("Proccessing Container data...", true);
                                    XmlElement ContainerNode = oDoc.CreateElement("CONTAINER");

                                    Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["ContainerId"]), "ContainerId");
                                    if (oLog != null) oLog.WriteLine("Field : " + "ContainerId".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["ContainerId"]), true);
                                    Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["HandlingUnitType"]), "HandlingUnitType");
                                    if (oLog != null) oLog.WriteLine("Field : " + "HandlingUnitType".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["HandlingUnitType"]), true);
                                    Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["Serial"]), "Serial");
                                    if (oLog != null) oLog.WriteLine("Field : " + "Serial".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["Serial"]), true);

                                    LoadNode.AppendChild(ContainerNode);
                                }
                                catch (Exception ex)
                                {
                                    if (oLog != null)
                                    {
                                        oLog.WriteLine("Error processing container node...", true);

                                        //RWMS-2564 RWMS-2563 START
                                        oLog.WriteLine(LoadNode.InnerXml.ToString(), true);
                                        //RWMS-2564 RWMS-2563 END

                                        oLog.WriteLine(ex.Message, true);
                                    }
                                }
                                //-----------------------------------------------------------------------------------------------------------------------------
                                oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE/LOADS").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE/LOADS").Count - 1).AppendChild(LoadNode);
                                LoadNode = null;

                                //Start PWMS-808 , RWMS-872
                                if (oOrdLoad.LoadDetWeights.Count > 0)
                                {
                                    try
                                    {
                                        XmlElement oLoadDetWeights = oDoc.CreateElement("LOADDETWEIGHT");
                                        if (IsSKUClassValidToExportAttributes(Convert.ToString(ordLoad["SKU"])) == true)
                                        {
                                            foreach (WMS.Logic.LoadDetWeight detweight in oOrdLoad.LoadDetWeights)
                                            {
                                                XmlElement detweightNode = oDoc.CreateElement("DETWEIGHT");
                                                Utilities.BuildObjectFromDocDetail(ds, "DETWEIGHT", oDoc, detweightNode, detweight, oLog);
                                                oLoadDetWeights.AppendChild(detweightNode);
                                            }
                                        }

                                        oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE/LOADS").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE/LOADS").Count - 1).AppendChild(oLoadDetWeights);
                                        oLoadDetWeights = null;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (oLog != null)
                                        {
                                            oLog.WriteLine("Error processing loaddetweight node...", true);
                                            oLog.WriteLine(ex.Message, true);
                                        }
                                    }
                                }
                                //End PWMS-808,RWMS-872

                            }
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null)
                            {
                                oLog.WriteLine("Error processing container node...", true);

                                //RWMS-2564 RWMS-2563 START
                                oLog.WriteLine(oLINES.InnerXml.ToString(), true);
                                //RWMS-2564 RWMS-2563 END

                                oLog.WriteLine(ex.Message, true);
                            }
                        }
                        LineNode = null;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null)
                        {
                            oLog.WriteLine("Error processing outbound order line...", true);

                            //RWMS-2564 RWMS-2563 START
                            oLog.WriteLine(oLINES.InnerXml.ToString(), true);
                            //RWMS-2564 RWMS-2563 END

                            oLog.WriteLine(ex.Message, true);
                        }
                    }
                }

                if (oLog != null) oLog.WriteLine("PROCCESSING ORDER SHIP MESSAGE FINISHED...", true);
            }
            catch (Exception ex)
            {
                if (oLog != null)
                {
                    //RWMS-2563 START
                    oLog.WriteLine("XML document....", true);
                    oLog.WriteLine(oDoc.InnerXml.ToString(), true);
                    //RWMS-2563 END

                    oLog.WriteLine("Exception Processing Outbound Order Message...", true);
                    oLog.WriteLine(ex.Message, true);
                    oLog.WriteLine(ex.StackTrace, true);
                }
            }
            return oDoc;
        }


        #endregion

        #region "Inbound Order Methods"

        public XmlDocument ExportInBound(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.INBOUND ds = new DataTemplate.INBOUND();
            if (oLog != null) oLog.WriteLine("Proccessing Inbound Order Message...", true);
            XmlDocument oDoc = new XmlDocument();
            try
            {
                WMS.Logic.InboundOrderHeader InDoc = new WMS.Logic.InboundOrderHeader(qSender.Values["CONSIGNEE"], qSender.Values["DOCUMENT"], true);
                oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

                Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, InDoc, oLog);

                XmlElement oCONTACT = oDoc.CreateElement("CONTACT");
                Utilities.BuildObjectFromDocDetail(ds, "CONTACT", oDoc, oCONTACT, InDoc.RECEIVEDFROM, oLog);
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oCONTACT);

                XmlElement oLINES = oDoc.CreateElement("LINES");
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oLINES);

                foreach (WMS.Logic.InboundOrderDetail oLine in InDoc.Lines)
                {
                    try
                    {
                        XmlElement LineNode;
                        LineNode = oDoc.CreateElement("LINE");

                        Utilities.BuildObjectFromDocDetail(ds, "LINE", oDoc, LineNode, oLine, oLog);

                        // Add line attributes to the line
                        XmlElement oLineAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "ATTRIBUTES", "ATTRIBUTE", "ATTNAME", "ATTVALUE", oLine.Attributes, oLog);
                        LineNode.AppendChild(oLineAttributes);
                        //-----------------------------------------------------------------------------------------------------------------------------
                        oDoc.SelectNodes("DATACOLLECTION/DATA/LINES").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES").Count - 1).AppendChild(LineNode);
                        LineNode = null;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null)
                        {
                            oLog.WriteLine("Error processing Inbound order line...", true);
                            oLog.WriteLine(ex.Message, true);
                        }
                    }
                }

                if (oLog != null) oLog.WriteLine("PROCCESSING INBOUND ORDER MESSAGE FINISHED...", true);
            }
            catch (Exception ex)
            {
                if (oLog != null)
                {
                    oLog.WriteLine("Exception Processing Inbound Order Message...", true);
                    oLog.WriteLine(ex.Message, true);
                    oLog.WriteLine(ex.StackTrace, true);
                }
            }
            return oDoc;
        }


        #endregion

        #region "Inventory Adjustments Methods"

        public XmlDocument ExportInventoryAdjustment(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.INVTRANS ds = new DataTemplate.INVTRANS();

            //WMS.Logic.InventoryTransaction InvTran = new WMS.Logic.InventoryTransaction();
            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

            if (oLog != null) oLog.WriteLine("Exporting Inventory Transaction", true);

            try
            {
                string sReceipt = qSender.Values["DOCUMENT"].ToString();
                int sReceiptLine = Convert.ToInt32(qSender.Values["DOCUMENTLINE"]);
                Utilities.BuildObjectFromQSender(ds, "DATA", oDoc, null, qSender, oLog);
                Utilities.buildDocLine(oDoc, (XmlElement)oDoc.SelectSingleNode("DATACOLLECTION/DATA"), qSender.Values["FROMWEIGHT"], "FROMWEIGHT");
                Utilities.buildDocLine(oDoc, (XmlElement)oDoc.SelectSingleNode("DATACOLLECTION/DATA"), qSender.Values["TOWEIGHT"], "TOWEIGHT");
                Utilities.buildDocLine(oDoc, (XmlElement)oDoc.SelectSingleNode("DATACOLLECTION/DATA"), sReceipt, "RECEIPT");
                Utilities.buildDocLine(oDoc, (XmlElement)oDoc.SelectSingleNode("DATACOLLECTION/DATA"), sReceiptLine.ToString(), "RECEIPTLINE");
                WMS.Logic.ReceiptDetail oLine = new WMS.Logic.ReceiptDetail(sReceipt, sReceiptLine, true);
                Utilities.buildDocLine(oDoc, (XmlElement)oDoc.SelectSingleNode("DATACOLLECTION/DATA"), oLine.ORDERID.ToString(), "ORDERID");
                WMS.Logic.InboundOrderHeader oOrder = new WMS.Logic.InboundOrderHeader(oLine.CONSIGNEE, oLine.ORDERID, true);
                Utilities.buildDocLine(oDoc, (XmlElement)oDoc.SelectSingleNode("DATACOLLECTION/DATA"), oOrder.ORDERTYPE.ToString(), "ORDERTYPE");
                Utilities.buildDocLine(oDoc, (XmlElement)oDoc.SelectSingleNode("DATACOLLECTION/DATA"), oLine.ORDERLINE.ToString(), "ORDERLINE");
            }
            catch (Exception ex)
            {
                if (oLog != null) oLog.WriteLine("Error occured while trying to get the receipt / inbound order data. \r\n Exception details: " + ex.ToString(), true);
            }

            // Adding field From Status / To Status
            try
            {
                string str = qSender.Values["FROMSTATUS"].ToString();
            }
            catch
            {
                if (oLog != null) oLog.WriteLine("Not Inventory Status changed", true);
                Utilities.buildDocHeader(oDoc, qSender.Values["FROMSTATUS"].ToString(), "FROMSTATUS");
                Utilities.buildDocHeader(oDoc, qSender.Values["TOSTATUS"].ToString(), "TOSTATUS");
            }
            try
            {
                if (oLog != null) oLog.WriteLine("Extracting Attributes", true);

                try
                {
                    if (oLog != null) oLog.WriteLine("Extracting Attributes", true);
                    WMS.Logic.Load oLoad = new WMS.Logic.Load(qSender.Values["FROMLOAD"].ToString(), true);
                    XmlElement oATTRIBUTES = oDoc.CreateElement("ATTRIBUTES");
                    //try
                    //{
                    //    WMS.Logic.Load oLoad = new WMS.Logic.Load(qSender.Values["FROMLOAD"].ToString(), true);
                    //    oATTRIBUTES = Utilities.BuildAttributeCollectionXmlNode(oDoc, "ATTRIBUTES", "ATTRIBUTE", "NAME", "VALUE", oLoad.LoadAttributes, oLog);
                    //    //LoadNode.AppendChild(oLoadAttributes);
                    //}
                    //catch (Exception ex)
                    //{
                    //    if (oLog != null) oLog.WriteLine("Exception in Inventory Transaction BO", true);
                    //    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    //}
                    if (IsSKUClassValidToExportAttributes(oLoad.SKU) == true)
                    {

                        try
                        {

                            oATTRIBUTES = Utilities.BuildAttributeCollectionXmlNode(oDoc, "ATTRIBUTES", "ATTRIBUTE", "NAME", "VALUE", oLoad.LoadAttributes, oLog);

                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception in Inventory Transaction BO", true);
                            if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                        }
                    }
                    oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oATTRIBUTES);
                    if (oLog != null) oLog.WriteLine("Finished Exporting Inventory Transaction", true);
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in Inventory Transaction BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                }

                if (oLog != null) oLog.WriteLine("Finished Exporting Inventory Transaction", true);
            }
            catch (Exception ex)
            {
                if (oLog != null) oLog.WriteLine("Exception in Inventory Transaction BO", true);
                if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
            }
            return oDoc;
        }

        #endregion

        #region "Produce Methods"

        public XmlDocument ExportProduce(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.PRODUCE ds = new DataTemplate.PRODUCE();

            //if (oLog != null) oLog.WriteLine("Trying To Load Xml Document to fill from message.", true);

            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

            if (oLog != null) oLog.WriteLine("Exporting PRODUCE Transaction", true);

            Utilities.BuildObjectFromQSender(ds, "DATA", oDoc, null, qSender, oLog);
            try
            {
                if (oLog != null) oLog.WriteLine("Extracting Attributes", true);
                try
                {
                    if (oLog != null) oLog.WriteLine("Extracting Attributes", true);

                    XmlElement oATTRIBUTES = oDoc.CreateElement("ATTRIBUTES");
                    try
                    {
                        WMS.Logic.Load oLoad = new WMS.Logic.Load(qSender.Values["FROMLOAD"].ToString(), true);
                        oATTRIBUTES = Utilities.BuildAttributeCollectionXmlNode(oDoc, "ATTRIBUTES", "ATTRIBUTE", "NAME", "VALUE", oLoad.LoadAttributes, oLog);
                        //LoadNode.AppendChild(oLoadAttributes);
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in PRODUCE Transaction BO", true);
                        if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    }
                    oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oATTRIBUTES);
                    if (oLog != null) oLog.WriteLine("Finished Exporting PRODUCE Transaction", true);
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in PRODUCE Transaction BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                }

                if (oLog != null) oLog.WriteLine("Finished Exporting PRODUCE Transaction", true);
            }
            catch (Exception ex)
            {
                if (oLog != null) oLog.WriteLine("Exception in PRODUCE Transaction BO", true);
                if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
            }
            return oDoc;
        }

        #endregion

        #region "Consume Methods"

        public XmlDocument ExportConsume(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.CONSUME ds = new DataTemplate.CONSUME();

            //WMS.Logic.InventoryTransaction InvTran = new WMS.Logic.InventoryTransaction();
            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

            if (oLog != null) oLog.WriteLine("Exporting CONSUME Transaction", true);

            Utilities.BuildObjectFromQSender(ds, "DATA", oDoc, null, qSender, oLog);
            try
            {
                if (oLog != null) oLog.WriteLine("Extracting Attributes", true);
                try
                {
                    if (oLog != null) oLog.WriteLine("Extracting Attributes", true);

                    XmlElement oATTRIBUTES = oDoc.CreateElement("ATTRIBUTES");
                    try
                    {
                        WMS.Logic.Load oLoad = new WMS.Logic.Load(qSender.Values["FROMLOAD"].ToString(), true);
                        oATTRIBUTES = Utilities.BuildAttributeCollectionXmlNode(oDoc, "ATTRIBUTES", "ATTRIBUTE", "NAME", "VALUE", oLoad.LoadAttributes, oLog);
                        //LoadNode.AppendChild(oLoadAttributes);
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null) oLog.WriteLine("Exception in CONSUME Transaction BO", true);
                        if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                    }
                    oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oATTRIBUTES);
                    if (oLog != null) oLog.WriteLine("Finished Exporting CONSUME Transaction", true);
                }
                catch (Exception ex)
                {
                    if (oLog != null) oLog.WriteLine("Exception in CONSUME Transaction BO", true);
                    if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                }

                if (oLog != null) oLog.WriteLine("Finished Exporting CONSUME Transaction", true);
            }
            catch (Exception ex)
            {
                if (oLog != null) oLog.WriteLine("Exception in CONSUME Transaction BO", true);
                if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
            }
            return oDoc;
        }

        #endregion

        #region "Assembly Work Order Methods"

        public XmlDocument ExportAssemblyWorkOrder(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.ASSEMBLYWORKORDER ds = new DataTemplate.ASSEMBLYWORKORDER();
            if (oLog != null) oLog.WriteLine("Proccessing Assembly Work Order Message...", true);
            XmlDocument oDoc = new XmlDocument();
            try
            {
                WMS.Logic.WorkOrderHeader OutDoc = new WMS.Logic.WorkOrderHeader(Convert.ToString(qSender.Values["CONSIGNEE"]), Convert.ToString(qSender.Values["DOCUMENT"]), true);
                oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

                Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, OutDoc, oLog);

                XmlElement oSkuComponents = oDoc.CreateElement("SKUCOMPONENTS");
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oSkuComponents);

                foreach (WorkOrderBOM wOBom in OutDoc.PARTSKUCOLLECTION)
                {
                    try
                    {
                        XmlElement woBomNode = oDoc.CreateElement("SKUCOMPONENT");
                        Utilities.BuildObjectFromDocDetail(ds, "SKUCOMPONENT", oDoc, woBomNode, wOBom, oLog);

                        //-----------------------------------------------------------------------------------------------------------------------------

                        if (oLog != null) oLog.WriteLine("Extracting Attributes", true);

                        XmlElement oATTRIBUTES = oDoc.CreateElement("ATTRIBUTES");
                        try
                        {
                            oATTRIBUTES = Utilities.BuildAttributeCollectionXmlNode(oDoc, "ATTRIBUTES", "ATTRIBUTE", "NAME", "VALUE", wOBom.ATTRIBUTES, oLog);
                            woBomNode.AppendChild(oATTRIBUTES);
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception extracting work order attributes", true);
                            if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                        }
                        oDoc.SelectNodes("DATACOLLECTION/DATA/SKUCOMPONENTS").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/SKUCOMPONENTS").Count - 1).AppendChild(woBomNode);
                        woBomNode = null;
                        oATTRIBUTES = null;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null)
                        {
                            oLog.WriteLine("Error processing Assembly Work Order...", true);
                            oLog.WriteLine(ex.Message, true);
                        }
                    }
                }
                if (oLog != null) oLog.WriteLine("PROCCESSING ASSEMBLY WORK ORDER MESSAGE FINISHED...", true);
            }
            catch (Exception ex)
            {
                if (oLog != null)
                {
                    oLog.WriteLine("Exception Processing assembly work order Message...", true);
                    oLog.WriteLine(ex.Message, true);
                    oLog.WriteLine(ex.StackTrace, true);
                }
            }
            return oDoc;
        }

        #endregion

        #region "Disassembly Work Order Methods"

        public XmlDocument ExportDisassemblyWorkOrder(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.DISASSEMBLYWORKORDER ds = new DataTemplate.DISASSEMBLYWORKORDER();
            if (oLog != null) oLog.WriteLine("Proccessing Disassembly Work Order Message...", true);
            XmlDocument oDoc = new XmlDocument();
            try
            {
                WMS.Logic.WorkOrderHeader OutDoc = new WMS.Logic.WorkOrderHeader(Convert.ToString(qSender.Values["CONSIGNEE"]), Convert.ToString(qSender.Values["DOCUMENT"]), true);
                oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

                Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, OutDoc, oLog);

                XmlElement oSkuComponents = oDoc.CreateElement("SKUCOMPONENTS");
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oSkuComponents);

                foreach (WorkOrderBOM wOBom in OutDoc.PARTSKUCOLLECTION)
                {
                    try
                    {
                        XmlElement woBomNode = oDoc.CreateElement("SKUCOMPONENT");
                        Utilities.BuildObjectFromDocDetail(ds, "SKUCOMPONENT", oDoc, woBomNode, wOBom, oLog);
                        //-----------------------------------------------------------------------------------------------------------------------------
                        oDoc.SelectNodes("DATACOLLECTION/DATA/SKUCOMPONENTS").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/SKUCOMPONENTS").Count - 1).AppendChild(woBomNode);
                        woBomNode = null;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null)
                        {
                            oLog.WriteLine("Error processing Disassembly Work Order...", true);
                            oLog.WriteLine(ex.Message, true);
                        }
                    }
                }

                if (oLog != null) oLog.WriteLine("PROCCESSING DISASSEMBLY WORK ORDER MESSAGE FINISHED...", true);
            }
            catch (Exception ex)
            {
                if (oLog != null)
                {
                    oLog.WriteLine("Exception Processing disassembly work order Message...", true);
                    oLog.WriteLine(ex.Message, true);
                    oLog.WriteLine(ex.StackTrace, true);
                }
            }
            return oDoc;
        }

        #endregion

        #region "Value Added Work Order Methods"

        public XmlDocument ExportValueAddedWorkOrder(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.VALUEADDEDSERVICES ds = new DataTemplate.VALUEADDEDSERVICES();
            if (oLog != null) oLog.WriteLine("Proccessing Value Added Work Order Message...", true);
            XmlDocument oDoc = new XmlDocument();
            try
            {
                WMS.Logic.WorkOrderHeader OutDoc = new WMS.Logic.WorkOrderHeader(Convert.ToString(qSender.Values["CONSIGNEE"]), Convert.ToString(qSender.Values["DOCUMENT"]), true);
                oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

                Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, OutDoc, oLog);

                if (oLog != null) oLog.WriteLine("PROCCESSING VALUE ADDED WORK ORDER MESSAGE FINISHED...", true);
            }
            catch (Exception ex)
            {
                if (oLog != null)
                {
                    oLog.WriteLine("Exception Processing value added work order Message...", true);
                    oLog.WriteLine(ex.Message, true);
                    oLog.WriteLine(ex.StackTrace, true);
                }
            }
            return oDoc;
        }

        #endregion

        #region "Inventory SnapShot Methods"

        public XmlDocument ExportInventrySnapShot(string vAction, LogFile oLog, string pConsignee)
        {
            DataTemplate.INVSNAPSHOT ds = new DataTemplate.INVSNAPSHOT();

            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION></DATACOLLECTION>");

            if (oLog != null) oLog.WriteLine("Exporting Snap Shot Transaction", true);

            string sql = "select distinct consignee,sku from vSnapShot";
            if (pConsignee != string.Empty)
                sql = sql + string.Format(" where consignee='{0}'", pConsignee);
            DataTable dtSkus = new DataTable();
            DataInterface.FillDataset(sql, dtSkus, true, "");

            sql = "select * from vSnapShot";
            if (pConsignee != string.Empty)
                sql = sql + string.Format(" where consignee='{0}'", pConsignee);
            DataTable dt = new DataTable();
            DataInterface.FillDataset(sql, dt, true, "");

            string sSku, sConsignee, skuFilter;
            DataRow[] SKUDRArray;
            foreach (DataRow drSku in dtSkus.Rows)
            {
                sConsignee = drSku["consignee"].ToString();
                sSku = drSku["sku"].ToString();
                skuFilter = string.Format("consignee='{0}' and sku='{1}'", sConsignee, sSku);
                SKUDRArray = dt.Select(skuFilter);
                if (SKUDRArray.Length > 0)
                {
                    XmlElement LineNode;
                    LineNode = oDoc.CreateElement("DATA");
                    Utilities.BuildObjectFromDataRow(ds, "DATA", oDoc, LineNode, drSku, oLog);

                    XmlElement Statuses = oDoc.CreateElement("STATUSES");

                    foreach (DataRow dr in SKUDRArray)
                    {
                        try
                        {

                            XmlElement StatusNode = oDoc.CreateElement("STATUS");
                            //Utilities.BuildObjectFromDataRow(ds, "STATUSES", oDoc, StatusNode, dr, oLog);
                            Utilities.BuildObjectFromDataRow(ds, "STATUS", oDoc, StatusNode, dr, oLog);

                            if (oLog != null) oLog.WriteLine("Extracting Attributes", true);

                            XmlElement oATTRIBUTES = oDoc.CreateElement("ATTRIBUTES");
                            oATTRIBUTES = Utilities.BuildAttributeCollectionXmlNode(oDoc, "ATTRIBUTES", "ATTRIBUTE", "NAME", "VALUE", dr, oLog);
                            StatusNode.AppendChild(oATTRIBUTES);
                            Statuses.AppendChild(StatusNode);
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null) oLog.WriteLine("Exception in Inventory Snapshot BO", true);
                            if (oLog != null) oLog.WriteLine(ex.Message.ToString(), true);
                        }
                    }

                    LineNode.AppendChild(Statuses);
                    oDoc.SelectNodes("DATACOLLECTION").Item(oDoc.SelectNodes("DATACOLLECTION").Count - 1).AppendChild(LineNode);
                }
            }
            if (oLog != null) oLog.WriteLine("Finished exporting inventory snap shot", true);
            return oDoc;
        }

        #endregion

        #region "Shipment Ship Methods"

        public XmlDocument ExportShipment(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            StringBuilder oBuilder = new StringBuilder();
            DataTemplate.SHIPMENT ds = new DataTemplate.SHIPMENT();
            if (oLog != null) oLog.WriteLine("Proccessing Shipment Message...", true);
            XmlDocument oDoc = new XmlDocument();

            try
            {
                WMS.Logic.Shipment ShipDoc = new WMS.Logic.Shipment(qSender.Values["DOCUMENT"], true);

                Hashtable LoadsCache = BuildLoadsCache(qSender.Values["DOCUMENT"]);                     //Shipment Loads Cache
                DataTable OrderLinesLoads = GetOrderLineLoadsTable(qSender.Values["DOCUMENT"]);         //Order Lines Loads
                DataTable dtShipmentDetails = CreateShipmentDetailsCache(qSender.Values["DOCUMENT"]);   //Shipment Details Cache

                oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

                Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, ShipDoc, oLog);

                XmlElement oDOCUMENTS = oDoc.CreateElement("DOCUMENTS");
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oDOCUMENTS);

                #region Outbound orders

                // First add all the Outbound orders to the shipment xml
                if (oLog != null) oLog.WriteLine("Proccessing Outbound orders for this shipment [" + ShipDoc.Orders.Count + "]", true);
                foreach (WMS.Logic.OutboundOrderHeader oDocument in ShipDoc.Orders)
                {
                    try
                    {
                        XmlElement DocumentNode;
                        DocumentNode = oDoc.CreateElement("DOCUMENT");

                        Utilities.BuildObjectFromDocDetail(ds, "DOCUMENT", oDoc, DocumentNode, oDocument, oLog);

                        //WMS.Logic.OutboundOrderHeader OutDoc = new WMS.Logic.OutboundOrderHeader(oDocument.CONSIGNEE, oDocument.ORDERID, true);
                        WMS.Logic.OutboundOrderHeader OutDoc = ShipDoc.Orders.getOrder(oDocument.CONSIGNEE, oDocument.ORDERID);

                        if (oLog != null) oLog.WriteLine("Creating Contact node", true);
                        XmlElement oCONTACT = oDoc.CreateElement("CONTACT");
                        Utilities.BuildObjectFromDocDetail(ds, "CONTACT", oDoc, oCONTACT, oDocument.SHIPTO, oLog);
                        DocumentNode.AppendChild(oCONTACT);
                        if (oLog != null) oLog.WriteLine("Contact node created", true);

                        XmlElement oLINES = oDoc.CreateElement("LINES");
                        DocumentNode.AppendChild(oLINES);

                        foreach (WMS.Logic.OutboundOrderHeader.OutboundOrderDetail oLine in OutDoc.Lines)
                        {
                            try
                            {
                                // if line does not exist in the shipment detail, do not export its data...
                                if (!LineEsixtsInShipmentDetailsCollection(oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE, dtShipmentDetails))
                                    continue;

                                XmlElement LineNode;
                                LineNode = oDoc.CreateElement("LINE");

                                Utilities.BuildObjectFromDocDetail(ds, "LINE", oDoc, LineNode, oLine, oLog);

                                //-----------------------------------------------------------------------------------------------------------------------------
                                DocumentNode.SelectNodes("LINES").Item(DocumentNode.SelectNodes("LINES").Count - 1).AppendChild(LineNode);

                                #region Loads

                                try
                                {
                                    // Add all loads to shipment
                                    DataRow[] OrdLineLoads = OrderLinesLoads.Select(string.Format("DOCUMENTTYPE='OUTBOUND' AND CONSIGNEE = '{0}' AND ORDERID = '{1}' AND ORDERLINE = '{2}'", oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE.ToString()));
                                    XmlElement LoadsNode = oDoc.CreateElement("LOADS");
                                    DocumentNode.SelectNodes("LINES/LINE").Item(DocumentNode.SelectNodes("LINES/LINE").Count - 1).AppendChild(LoadsNode);

                                    foreach (DataRow ordLoad in OrdLineLoads)
                                    {
                                        XmlElement LoadNode = oDoc.CreateElement("LOAD");
                                        Utilities.BuildObjectFromDataRow(ds, "LOAD", oDoc, LoadNode, ordLoad, oLog);
                                        // Add loads attributes to the line
                                        //WMS.Logic.Load oOrdLoad = new WMS.Logic.Load(Convert.ToString(ordLoad["LOADID"]), true);
                                        WMS.Logic.Load oOrdLoad = (WMS.Logic.Load)LoadsCache[Convert.ToString(ordLoad["LOADID"])];
                                        //XmlElement oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oOrdLoad.LoadAttributes, oLog);
                                       //RWMS-758 added exprot loadattributes validation condition
                                          XmlElement oLoadAttributes = oDoc.CreateElement("LOADATTRIBUTES");
                                          if (IsSKUClassValidToExportAttributes(oLine.SKU))
                                          {
                                          oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oOrdLoad.LoadAttributes, oLog);
                                          }

                                        LoadNode.AppendChild(oLoadAttributes);
                                        // Create Container for the loads
                                        if (oLog != null) oLog.WriteLine("Proccessing Container data...", true);
                                        XmlElement ContainerNode = oDoc.CreateElement("CONTAINER");
                                        try
                                        {
                                            Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["ContainerId"]), "ContainerId");
                                            if (oLog != null) oLog.WriteLine("Field : " + "ContainerId".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["ContainerId"]), true);
                                            Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["HandlingUnitType"]), "HandlingUnitType");
                                            if (oLog != null) oLog.WriteLine("Field : " + "HandlingUnitType".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["HandlingUnitType"]), true);
                                            Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["Serial"]), "Serial");
                                            if (oLog != null) oLog.WriteLine("Field : " + "Serial".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["Serial"]), true);

                                        }
                                        catch (Exception ex)
                                        {
                                            if (oLog != null)
                                            {
                                                oLog.WriteLine("Error processing container node...", true);
                                                oLog.WriteLine(ex.Message, true);
                                            }
                                        }
                                        LoadNode.AppendChild(ContainerNode);
                                        //-----------------------------------------------------------------------------------------------------------------------------
                                        DocumentNode.SelectNodes("LINES/LINE").Item(DocumentNode.SelectNodes("LINES/LINE").Count - 1).SelectSingleNode("LOADS").AppendChild(LoadNode);
                                        LoadNode = null;
                                    }
                                }
                                catch { }

                                #endregion

                                LineNode = null;
                            }
                            catch (Exception ex)
                            {
                                if (oLog != null)
                                {
                                    oLog.WriteLine("Error processing outbound order line...", true);
                                    oLog.WriteLine(ex.Message, true);
                                }
                            }
                        }
                        oDoc.SelectNodes("DATACOLLECTION/DATA/DOCUMENTS").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/DOCUMENTS").Count - 1).AppendChild(DocumentNode);
                        DocumentNode = null;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null)
                        {
                            oLog.WriteLine("Error processing shipment document...", true);
                            oLog.WriteLine(ex.Message, true);
                        }
                    }
                }

                #endregion

                #region Flowthrough orders

                if (oLog != null) oLog.WriteSeperator();
                if (oLog != null) oLog.WriteLine("Proccessing Flowthroughs for this shipment [" + ShipDoc.Flowthroughs.Count + "]", true);
                // First add all the Flowthrough to the shipment xml
                foreach (WMS.Logic.Flowthrough oFlowDoc in ShipDoc.Flowthroughs)
                {
                    try
                    {
                        XmlElement DocumentNode;
                        DocumentNode = oDoc.CreateElement("DOCUMENT");

                        Utilities.BuildObjectFromDocDetail(ds, "DOCUMENT", oDoc, DocumentNode, oFlowDoc, oLog);

                        WMS.Logic.Flowthrough OutDoc = new WMS.Logic.Flowthrough(oFlowDoc.CONSIGNEE, oFlowDoc.FLOWTHROUGH, true);

                        XmlElement oLINES = oDoc.CreateElement("LINES");
                        DocumentNode.AppendChild(oLINES);

                        foreach (WMS.Logic.FlowthroughDetail oLine in oFlowDoc.LINES)
                        {
                            try
                            {
                                XmlElement LineNode;
                                LineNode = oDoc.CreateElement("LINE");

                                Utilities.BuildObjectFromDocDetail(ds, "LINE", oDoc, LineNode, oLine, oLog);

                                //-----------------------------------------------------------------------------------------------------------------------------
                                DocumentNode.SelectNodes("LINES").Item(DocumentNode.SelectNodes("LINES").Count - 1).AppendChild(LineNode);
                                try
                                {
                                    DataTable OrdLineLoads = new DataTable();
                                    //DataInterface.FillDataset("SELECT OL.LOADID, LOADUOM AS UOM , LD.UNITS AS QTY , LD.STATUS , ISNULL(CN.CONTAINER,OL.LOADID) AS ContainerId,ISNULL(CN.HUTYPE,'') AS HandlingUnitType, ISNULL(CN.SERIAL,'') AS Serial FROM ORDERLOADS OL INNER JOIN LOADS LD ON OL.LOADID=LD.LOADID INNER JOIN SHIPMENTLOADS SHPLD ON OL.LOADID=SHPLD.LOADID LEFT OUTER JOIN CONTAINER CN ON CN.CONTAINER=LD.HANDLINGUNIT WHERE OL.DOCUMENTTYPE='FLWTH' AND (OL.CONSIGNEE = '" + OutDoc.CONSIGNEE + "') AND (OL.ORDERID = '" + OutDoc.FLOWTHROUGH + "') AND (OL.ORDERLINE = '" + oLine.FLOWTHROUGHLINE.ToString() + "') AND SHPLD.SHIPMENT = '" + ShipDoc.SHIPMENT + "'", OrdLineLoads, false, "");
                                   DataInterface.FillDataset("SELECT OL.LOADID, LOADUOM AS UOM , LD.UNITS AS QTY ,LD.SKU, LD.STATUS , ISNULL(CN.CONTAINER,OL.LOADID) AS ContainerId,ISNULL(CN.HUTYPE,'') AS HandlingUnitType, ISNULL(CN.SERIAL,'') AS Serial FROM ORDERLOADS OL INNER JOIN LOADS LD ON OL.LOADID=LD.LOADID INNER JOIN SHIPMENTLOADS SHPLD ON OL.LOADID=SHPLD.LOADID LEFT OUTER JOIN CONTAINER CN ON CN.CONTAINER=LD.HANDLINGUNIT WHERE OL.DOCUMENTTYPE='FLWTH' AND (OL.CONSIGNEE = '" + OutDoc.CONSIGNEE + "') AND (OL.ORDERID = '" + OutDoc.FLOWTHROUGH + "') AND (OL.ORDERLINE = '" + oLine.FLOWTHROUGHLINE.ToString() + "') AND SHPLD.SHIPMENT = '" + ShipDoc.SHIPMENT + "'", OrdLineLoads, false, "");
                                    XmlElement LoadsNode = oDoc.CreateElement("LOADS");
                                    DocumentNode.SelectNodes("LINES/LINE").Item(DocumentNode.SelectNodes("LINES/LINE").Count - 1).AppendChild(LoadsNode);

                                    foreach (DataRow ordLoad in OrdLineLoads.Rows)
                                    {
                                        XmlElement LoadNode = oDoc.CreateElement("LOAD");
                                        Utilities.BuildObjectFromDataRow(ds, "LOAD", oDoc, LoadNode, ordLoad, oLog);
                                        // Add loads attributes to the line
                                        WMS.Logic.Load oOrdLoad = new WMS.Logic.Load(Convert.ToString(ordLoad["LOADID"]), true);
                                       // XmlElement oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oOrdLoad.LoadAttributes, oLog);
                                       //RWMS-758 adding export loadattributes validation condition
                                          XmlElement oLoadAttributes = oDoc.CreateElement("LOADATTRIBUTES");
                                          if (IsSKUClassValidToExportAttributes(Convert.ToString(ordLoad["SKU"])) == true)
                                          {
                                          oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oOrdLoad.LoadAttributes, oLog);
                                          }

                                        LoadNode.AppendChild(oLoadAttributes);
                                        // Create Container for the loads
                                        if (oLog != null) oLog.WriteLine("Proccessing Container data...", true);
                                        XmlElement ContainerNode = oDoc.CreateElement("CONTAINER");
                                        try
                                        {
                                            Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["ContainerId"]), "ContainerId");
                                            if (oLog != null) oLog.WriteLine("Field : " + "ContainerId".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["ContainerId"]), true);
                                            Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["HandlingUnitType"]), "HandlingUnitType");
                                            if (oLog != null) oLog.WriteLine("Field : " + "HandlingUnitType".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["HandlingUnitType"]), true);
                                            Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["Serial"]), "Serial");
                                            if (oLog != null) oLog.WriteLine("Field : " + "Serial".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["Serial"]), true);

                                        }
                                        catch (Exception ex)
                                        {
                                            if (oLog != null)
                                            {
                                                oLog.WriteLine("Error processing container node...", true);
                                                oLog.WriteLine(ex.Message, true);
                                            }
                                        }
                                        LoadNode.AppendChild(ContainerNode);
                                        //-----------------------------------------------------------------------------------------------------------------------------
                                        DocumentNode.SelectNodes("LINES/LINE").Item(DocumentNode.SelectNodes("LINES/LINE").Count - 1).SelectSingleNode("LOADS").AppendChild(LoadNode);
                                        LoadNode = null;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (oLog != null)
                                    {
                                        oLog.WriteLine("Error processing line node...", true);
                                        oLog.WriteLine(ex.Message, true);
                                    }
                                }
                                LineNode = null;
                            }
                            catch (Exception ex)
                            {
                                if (oLog != null)
                                {
                                    oLog.WriteLine("Error processing outbound order line...", true);
                                    oLog.WriteLine(ex.Message, true);
                                }
                            }
                        }


                        oDoc.SelectNodes("DATACOLLECTION/DATA/DOCUMENTS").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/DOCUMENTS").Count - 1).AppendChild(DocumentNode);
                        DocumentNode = null;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null)
                        {
                            oLog.WriteLine("Error processing shipment document...", true);
                            oLog.WriteLine(ex.Message, true);
                        }
                    }
                }

                #endregion

                #region Handling Units

                //Adding the handling units for the current receipt
                XmlElement oHU = oDoc.CreateElement("HANDLINGUNITS");
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oHU);
                DataTable ShipHU = new DataTable();
                DataInterface.FillDataset(string.Format("SELECT HUTYPE,SUM(HUQTY) AS HUQTY FROM HANDLINGUNITTRANSACTION WHERE TRANSACTIONTYPE = 'SHIPMENT' AND TRANSACTIONTYPEID = '{0}' GROUP BY HUTYPE", qSender.Values["DOCUMENT"]), ShipHU, false, "");
                if (ShipHU.Rows.Count > 0)
                {
                    if (oLog != null) oLog.WriteSeperator();
                    if (oLog != null) oLog.WriteLine("Exporting Shipment Handling Units, Total Rows " + ShipHU.Rows.Count, true);
                }
                else
                {
                    if (oLog != null) oLog.WriteSeperator();
                    if (oLog != null) oLog.WriteLine("No Handling Units found for Export for this Shipment...", true);
                }
                foreach (DataRow tmpHU in ShipHU.Rows)
                {
                    XmlElement HUNode = oDoc.CreateElement("HANDLINGUNIT");
                    Utilities.BuildObjectFromDataRow(ds, "HANDLINGUNIT", oDoc, HUNode, tmpHU, oLog);
                    oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).SelectSingleNode("HANDLINGUNITS").AppendChild(HUNode);
                    HUNode = null;
                }

                #endregion

                if (oLog != null) oLog.WriteLine("PROCCESSING SHIPMENT SHIP MESSAGE FINISHED...", true);
            }
            catch (Exception ex)
            {
                if (oLog != null)
                {
                    oLog.WriteLine("Exception Processing Shipment Message...", true);
                    oLog.WriteLine(ex.Message, true);
                    oLog.WriteLine(ex.StackTrace, true);
                }
            }
            return oDoc;
        }



        private Hashtable BuildLoadsCache(string pShipment)
        {
            Hashtable htLoads = new Hashtable();
            string Sql = string.Format("SELECT LOADS.*, LOADS.HANDLINGUNIT as CONTAINERID, HUTYPE, ORDERID, ORDERLINE, DOCUMENTTYPE, attribute.*, skuuom.netweight,skuuom.volume, HUTYPE as HandlingUnitType FROM SHIPMENTLOADS INNER JOIN LOADS ON SHIPMENTLOADS.LOADID=LOADS.LOADID INNER JOIN ORDERLOADS ON SHIPMENTLOADS.LOADID=ORDERLOADS.LOADID  left outer JOIN CONTAINER ON dbo.CONTAINER.CONTAINER = dbo.LOADS.HANDLINGUNIT left outer join attribute on loads.loadid = attribute.pkey1 and attribute.pkeytype = 'LOAD' left outer join skuuom on loads.consignee = skuuom.consignee and loads.sku = skuuom.sku and (skuuom.loweruom = '' or skuuom.loweruom is null) WHERE SHIPMENT = '{0}'", pShipment);
            DataTable dt = new DataTable();
            DataInterface.FillDataset(Sql, dt, false, null);
            Sql = string.Format("SELECT attribute.* FROM SHIPMENTLOADS left outer join attribute on SHIPMENTLOADS.loadid = attribute.pkey1 and attribute.pkeytype = 'LOAD' WHERE SHIPMENT = '{0}'", pShipment);
            DataTable dtAttributes = new DataTable();
            DataInterface.FillDataset(Sql, dtAttributes, false, null);
            foreach (DataRow dr in dt.Rows)
            {
                if (!htLoads.ContainsKey(dr["loadid"].ToString()))
                {
                    DataRow[] loadatt = null;
                    loadatt = dtAttributes.Select(string.Format("pkey1 = '{0}'", dr["loadid"].ToString()));
                    DataRow dratt = null;
                    if (loadatt.Length > 0)
                        dratt = loadatt[0];
                    htLoads.Add(dr["loadid"].ToString(), new WMS.Logic.Load(dr, dratt));
                }
            }
            return htLoads;
        }

        private DataTable GetOrderLineLoadsTable(string pShipment)
        {
            string Sql = string.Format("SELECT OL.DOCUMENTTYPE,OL.CONSIGNEE, OL.ORDERID, OL.ORDERLINE, OL.LOADID, LOADUOM AS UOM , LD.UNITS AS QTY , LD.STATUS , ISNULL(CN.CONTAINER,OL.LOADID) AS ContainerId, " +
                "ISNULL(CN.HUTYPE,'') AS HandlingUnitType, ISNULL(CN.SERIAL,'') AS Serial " +
                "FROM ORDERLOADS OL INNER JOIN LOADS LD ON OL.LOADID=LD.LOADID INNER JOIN SHIPMENTLOADS SHPLD ON OL.LOADID=SHPLD.LOADID " +
                "LEFT OUTER JOIN CONTAINER CN ON CN.CONTAINER=LD.HANDLINGUNIT WHERE SHPLD.SHIPMENT = '{0}'", pShipment);
            DataTable dt = new DataTable();
            DataInterface.FillDataset(Sql, dt, false, null);
            return dt;
        }

        private DataTable CreateShipmentDetailsCache(string pShipment)
        {
            string Sql = string.Format("select * from SHIPMENTDETAIL where SHIPMENT = '{0}'", pShipment);
            DataTable dt = new DataTable();
            DataInterface.FillDataset(Sql, dt, false, null);
            return dt;
        }
          private bool IsSKUClassValidToExportAttributes(string strSKU)
                        {
                  Boolean bReturn = false;
                  string Sql = string.Format("SELECT COUNT(1) as cnt FROM dbo.SKU INNER JOIN SKUCLSLOADATT on SKU.CLASSNAME = SKUCLSLOADATT.CLASSNAME WHERE SKU.SKU = '{0}'", strSKU);
                  DataTable dt = new DataTable();
                  DataInterface.FillDataset(Sql, dt, false, null);
                  if (Convert.ToInt16(dt.Rows[0]["cnt"]) > 0)
                  {
                  bReturn = true;
                  }

                  return bReturn;
                  }

        private bool LineEsixtsInShipmentDetailsCollection(string pConsignee, string pOrderid, int pOrderLine, DataTable dtShipmentDetails)
        {
            try
            {
                string sFilter = string.Format("CONSIGNEE='{0}' and ORDERID='{1}' and ORDERLINE={2}", pConsignee, pOrderid, pOrderLine);
                DataRow[] drArr = dtShipmentDetails.Select(sFilter);
                if (drArr.Length > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }

        }

        #endregion

        #region "Flowthrough Methods"

        public XmlDocument ExportFlowthrough(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.FLOWTHROUGH ds = new DataTemplate.FLOWTHROUGH();
            if (oLog != null) oLog.WriteLine("Proccessing FLOWTHROUGH Message...", true);
            XmlDocument oDoc = new XmlDocument();
            try
            {
                WMS.Logic.Flowthrough OutDoc = new WMS.Logic.Flowthrough(qSender.Values["CONSIGNEE"], qSender.Values["DOCUMENT"], true);
                oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

                Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, OutDoc, oLog);

                XmlElement oLINES = oDoc.CreateElement("LINES");
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oLINES);

                foreach (WMS.Logic.FlowthroughDetail oLine in OutDoc.LINES)
                {
                    try
                    {
                        XmlElement LineNode;
                        LineNode = oDoc.CreateElement("LINE");

                        Utilities.BuildObjectFromDocDetail(ds, "LINE", oDoc, LineNode, oLine, oLog);

                        //-----------------------------------------------------------------------------------------------------------------------------
                        oDoc.SelectNodes("DATACOLLECTION/DATA/LINES").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES").Count - 1).AppendChild(LineNode);
                        try
                        {
                            XmlElement LoadsNode = oDoc.CreateElement("LOADS");
                            oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Count - 1).AppendChild(LoadsNode);
                            LoadsNode = null;
                        }
                        catch { }
                        try
                        {
                            DataTable OrdLineLoads = new DataTable();
                            DataInterface.FillDataset("SELECT OL.LOADID, LOADUOM AS UOM , LD.UNITS AS QTY , LD.STATUS , ISNULL(CN.CONTAINER,OL.LOADID) AS ContainerId, ISNULL(CN.HUTYPE,'') AS HandlingUnitType, ISNULL(CN.SERIAL,'') AS Serial FROM ORDERLOADS OL INNER JOIN LOADS LD ON OL.LOADID=LD.LOADID LEFT OUTER JOIN CONTAINER CN ON CN.CONTAINER=LD.HANDLINGUNIT WHERE OL.DOCUMENTTYPE='flwth' AND (OL.CONSIGNEE = '" + oLine.CONSIGNEE + "') AND (OL.ORDERID = '" + oLine.FLOWTHROUGH + "') AND (OL.ORDERLINE = '" + oLine.FLOWTHROUGHLINE.ToString() + "')", OrdLineLoads, false, "");
                            XmlElement LoadsNode = oDoc.CreateElement("LOADS");
                            oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE").Count - 1).AppendChild(LoadsNode);

                            foreach (DataRow ordLoad in OrdLineLoads.Rows)
                            {
                                XmlElement LoadNode = oDoc.CreateElement("LOAD");
                                Utilities.BuildObjectFromDataRow(ds, "LOAD", oDoc, LoadNode, ordLoad, oLog);
                                // Add loads attributes to the line
                                WMS.Logic.Load oOrdLoad = new WMS.Logic.Load(Convert.ToString(ordLoad["LOADID"]), true);
                                XmlElement oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oOrdLoad.LoadAttributes, oLog);
                                LoadNode.AppendChild(oLoadAttributes);
                                // Create Container for the loads
                                try
                                {
                                    if (oLog != null) oLog.WriteLine("Proccessing Container data...", true);
                                    XmlElement ContainerNode = oDoc.CreateElement("CONTAINER");

                                    Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["ContainerId"]), "ContainerId");
                                    if (oLog != null) oLog.WriteLine("Field : " + "ContainerId".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["ContainerId"]), true);
                                    Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["HandlingUnitType"]), "HandlingUnitType");
                                    if (oLog != null) oLog.WriteLine("Field : " + "HandlingUnitType".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["HandlingUnitType"]), true);
                                    Utilities.buildDocLine(oDoc, ContainerNode, Convert.ToString(ordLoad["Serial"]), "Serial");
                                    if (oLog != null) oLog.WriteLine("Field : " + "Serial".PadRight(25, ' ') + " , Value : " + Convert.ToString(ordLoad["Serial"]), true);

                                    LoadNode.AppendChild(ContainerNode);
                                }
                                catch (Exception ex)
                                {
                                    if (oLog != null)
                                    {
                                        oLog.WriteLine("Error processing container node...", true);
                                        oLog.WriteLine(ex.Message, true);
                                    }
                                }
                                //-----------------------------------------------------------------------------------------------------------------------------
                                oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE/LOADS").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE/LOADS").Count - 1).AppendChild(LoadNode);
                                LoadNode = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (oLog != null)
                            {
                                oLog.WriteLine("Error processing container node...", true);
                                oLog.WriteLine(ex.Message, true);
                            }
                        }
                        LineNode = null;
                    }
                    catch (Exception ex)
                    {
                        if (oLog != null)
                        {
                            oLog.WriteLine("Error processing FLOWTHROUGH line...", true);
                            oLog.WriteLine(ex.Message, true);
                        }
                    }
                }

                if (oLog != null) oLog.WriteLine("PROCCESSING FLOWTHROUGH MESSAGE FINISHED...", true);
            }
            catch (Exception ex)
            {
                if (oLog != null)
                {
                    oLog.WriteLine("Exception Processing FLOWTHROUGH Message...", true);
                    oLog.WriteLine(ex.Message, true);
                    oLog.WriteLine(ex.StackTrace, true);
                }
            }
            return oDoc;
        }

        #endregion

        #region Packing List Methods

        public XmlDocument ExportPackingList(string vAction, Made4Net.Shared.QMsgSender qSender, LogFile oLog)
        {
            DataTemplate.PACKINGLIST ds = new DataTemplate.PACKINGLIST();
            if (oLog != null) oLog.WriteLine("Proccessing Packing List Message...", true);
            XmlDocument oDoc = new XmlDocument();
            try
            {
                WMS.Logic.PackingListHeader OutDoc = new WMS.Logic.PackingListHeader(Convert.ToString(qSender.Values["DOCUMENT"]), true);
                oDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA></DATA></DATACOLLECTION>");

                Utilities.BuildObjectFromDocHeader(ds, "DATA", oDoc, OutDoc, oLog);

                XmlElement oCONTACT = oDoc.CreateElement("CONTACT");
                WMS.Logic.Contact oCont = new WMS.Logic.Contact(OutDoc.CONTACTID, true);
                Utilities.BuildObjectFromDocDetail(ds, "CONTACT", oDoc, oCONTACT, oCont, oLog);

                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oCONTACT);

                XmlElement oLOADS = oDoc.CreateElement("LOADS");
                oDoc.SelectNodes("DATACOLLECTION/DATA").Item(oDoc.SelectNodes("DATACOLLECTION/DATA").Count - 1).AppendChild(oLOADS);
                DataTable OrdLoads = new DataTable();
                DataInterface.FillDataset("SELECT PD.LOADID,CONSIGNEE, SKU, UNITS AS QTY, LOADUOM AS UOM, STATUS FROM PACKINGLISTDETAIL PD INNER JOIN LOADS ON PD.LOADID=LOADS.LOADID WHERE PD.PACKINGLISTID= '" + Convert.ToString(qSender.Values["DOCUMENT"]) + "'", OrdLoads, false, "");

                foreach (DataRow ordLoad in OrdLoads.Rows)
                {
                    try
                    {
                        XmlElement LoadNode = oDoc.CreateElement("LOAD");
                        Utilities.BuildObjectFromDataRow(ds, "LOAD", oDoc, LoadNode, ordLoad, oLog);
                        // Add loads attributes to the line
                        WMS.Logic.Load oOrdLoad = new WMS.Logic.Load(Convert.ToString(ordLoad["LOADID"]), true);
                        XmlElement oLoadAttributes = Utilities.BuildAttributeCollectionXmlNode(oDoc, "LOADATTRIBUTES", "LOADATTRIBUTE", "NAME", "VALUE", oOrdLoad.LoadAttributes, oLog);
                        LoadNode.AppendChild(oLoadAttributes);
                        //-----------------------------------------------------------------------------------------------------------------------------
                        oDoc.SelectNodes("DATACOLLECTION/DATA/LOADS").Item(oDoc.SelectNodes("DATACOLLECTION/DATA/LOADS").Count - 1).AppendChild(LoadNode);
                        LoadNode = null;

                    }
                    catch (Exception ex)
                    {
                        if (oLog != null)
                        {
                            oLog.WriteLine("Error processing Packing List load...", true);
                            oLog.WriteLine(ex.Message, true);
                        }
                    }
                }

                if (oLog != null) oLog.WriteLine("PROCCESSING PACKING LIST MESSAGE FINISHED...", true);
            }
            catch (Exception ex)
            {
                if (oLog != null)
                {
                    oLog.WriteLine("Exception Processing Packing List Message...", true);
                    oLog.WriteLine(ex.Message, true);
                    oLog.WriteLine(ex.StackTrace, true);
                }
            }
            return oDoc;
        }

        #endregion

        #endregion

        #region DataMapperTransactionResult

        public enum DataMapperTransactionResult
        {
            Success = 0,
            BOFailed = 1,
            BOElementFailed = 2
        }

        public static string TransactionResultToString(DataMapperTransactionResult pProcRes)
        {
            string ret;
            switch (pProcRes)
            {
                case DataMapperTransactionResult.Success:
                    ret = "Success";
                    break;
                case DataMapperTransactionResult.BOFailed:
                    ret = "BOFailed";
                    break;
                case DataMapperTransactionResult.BOElementFailed:
                    ret = "BOElementFailed";
                    break;
                default:
                    ret = string.Empty;
                    break;
            }
            return ret;
        }

        #endregion



    }

    public class DataMapperProcessResult
    {
        #region Members

        private ObjectProcessor.DataMapperTransactionResult mTransactionResult;
        private string mTransactionErrorMessage;

        #endregion

        #region Properties

        public ObjectProcessor.DataMapperTransactionResult TransactionResult
        {
            get { return mTransactionResult; }
        }

        public string TransactionErrorMessage
        {
            get { return mTransactionErrorMessage; }
        }

        #endregion

        #region Ctor

        public DataMapperProcessResult(ExpertObjectMapper.ObjectProcessor.DataMapperTransactionResult pTransactionResult, string pTransactionErrorMesage)
        {
            mTransactionResult = pTransactionResult;
            mTransactionErrorMessage = pTransactionErrorMesage;
        }

        #endregion
    }

    public class ExpertObjectTypes
    {
        public const string SKU = "SKU";
        public const string COMPANY = "COMPANY";
        public const string CARRIER = "CARRIER";
        public const string CONTACT = "CONTACT";
        public const string OUTBOUND = "OUTBOUND";
        public const string INBOUND = "INBOUND";
        public const string RECEIPT = "RECEIPT";
        public const string LOAD = "LOAD";
        public const string ASSEMBLY = "ASSEMBLY";
        public const string DISASSEMBLY = "DISASSEMBLY";
        public const string VALUEADDED = "VALUEADDED";
        public const string SHIPMENT = "SHIPMENT";
        public const string FLOWTHROUGH = "FLOWTHROUGH";
        public const string TRANSSHIPMENT = "TRANSSHIPMENT";
        public const string DBTABLE = "DBTABLE";
    }

    public class ExpertMessgesTypes
    {
        public const string CONSUME = "CONSUME";
        public const string PRODUCE = "PRODUCE";
        public const string CARRIER = "CARRIER";
        public const string SKU = "SKU";
        public const string CHARGE = "CHARGE";
        public const string RECEIPT = "RECEIPT";
        public const string ORDERSHIPPED = "OUTBOUND";
        public const string INBOUND = "INBOUND";
        public const string CHANGEQTY = "CHANGEQTY";
        public const string LOADSTATUSCHANGE = "SETSTATUS";
        public const string INVSNAPSHOT = "SNAPSHOT";
        public const string LOAD = "LOAD";
        public const string ASSEMBLY = "ASSEMBLY";
        public const string DISASSEMBLY = "DISASSEMBLY";
        public const string VALUEADDED = "VALUEADDED";
        public const string SHIPMENT = "SHIPMENT";
        public const string FLOWTHROUGH = "FLOWTHROUGH";
        public const string PACKINGLIST = "PACKINGLIST";
        public const string DBTABLEDATAEXPORT = "DBTABLEDATAEXPORT";
    }

}