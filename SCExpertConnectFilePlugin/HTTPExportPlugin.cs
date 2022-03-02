using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Timers;
using System.Data;
using Made4Net.Shared;
using Made4Net.DataAccess;
using ExpertObjectMapper;
using SCExpertConnectPlugins.BO;
using System.Net;

namespace SCExpertConnectFilePlugin
{
    public class HTTPExportPlugin : BasePlugin
    {
        #region Members

        private string mHTTPURL;
        private string mSearchResponse;

        #endregion

        #region Ctor

        /// <summary>
        /// Http Plugin
        /// </summary>
        /// <param name="pPluginId"></param>
        public HTTPExportPlugin(int pPluginId)
            : base(pPluginId)
        {
            if (Logger != null)
            {
                Logger.WriteLine("HttpExport Plugin Construction");
            }
        }

        #endregion

        #region Overrides

        #region Expoert

        public override int Export(XmlDocument oXMLDoc)
        {
            int retval = 0;
            InitLogger("HTTPExport");
            if (Logger != null)
            {
                Logger.WriteLine("HttpExport Plugin called");
                Logger.WriteLine("Attemtping to retrieve the order ID of input XML");
            }
            string OrderId = String.Empty;
            try
            {
                OrderId = RetrieveOrderId(oXMLDoc);
                if(String.IsNullOrEmpty(OrderId))
                {
                    if (Logger != null)
                    {
                        Logger.WriteLine("Order Id from input XML is empty , XML:", true);
                        Logger.WriteLine(oXMLDoc.InnerXml, true);
                        retval = -1;
                    }
                }
                else
                {
                    if (Logger != null)
                    {
                        Logger.WriteLine("Order Id from input XML : " + OrderId);
                    }
                }
            }
            catch (Exception )
            {
                if (Logger != null)
                {
                    Logger.WriteLine("Error Occured while trying to retrive Order Id from input XML , XML:", true);
                    Logger.WriteLine(oXMLDoc.InnerXml, true);
                }
                retval = -1;
            }
            if (Logger != null)
            {
                Logger.WriteLine("Attempting to Retrieve LockPlannedOrdURL parameter");
            }
            try
            {
                mHTTPURL = GetParameterValue("LockPlannedOrdURL");
                if (String.IsNullOrEmpty(mHTTPURL))
                {
                    if (Logger != null)
                    {
                        Logger.WriteLine("LockPlannedOrdURL parameter is empty:", true);
                        retval = -1;
                    }
                }
                else
                {
                    if (Logger != null)
                    {
                        Logger.WriteLine("LockPlannedOrdURL : " + mHTTPURL);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.WriteLine("Error Occured while trying to retrive  LockPlannedOrdURL parameter, Stack Trace flows:", true);
                    Logger.WriteLine(ex.StackTrace, true);
                }
                retval = -1;
            }

            if (Logger != null)
            {
                Logger.WriteLine("Attempting to Retrieve OutboundHTTPResponseString parameter");
            }
            try
            {
                mSearchResponse = GetParameterValue("OutboundHTTPResponseString");
                if (String.IsNullOrEmpty(mSearchResponse))
                {
                    if (Logger != null)
                    {
                        Logger.WriteLine("OutboundHTTPResponseString parameter is empty.:", true);
                        retval = -1;
                    }
                }
                else
                {
                    if (Logger != null)
                    {
                        Logger.WriteLine("OutboundHTTPResponseString : " + mSearchResponse);
                    }
                }
                if (Logger != null)
                {
                    Logger.WriteLine("Attempting to convert OutboundHTTPResponseString to XML String");
                }
                // Format xmltag=Result;value=OK
                String[] components = mSearchResponse.Split(';');
                if(components.Length != 2)
                {
                    if (Logger != null)
                    {
                        Logger.WriteLine("Invalid OutboundHTTPResponseString parameter supplied : " + mSearchResponse);
                        retval = -1;
                    }
                }
                else
                {
                    String[] xmlTag = components[0].Split('=');
                    if (xmlTag.Length != 2)
                    {
                        if (Logger != null)
                        {
                            Logger.WriteLine("Invalid OutboundHTTPResponseString parameter sppplied 'xmltag=' don't exist in the supplied string: " + mSearchResponse);
                            retval = -1;
                        }
                    }
                    else
                    {
                        String tagName = xmlTag[1];
                        String[] values = components[1].Split('=');
                        if (values.Length != 2)
                        {
                            if (Logger != null)
                            {
                                Logger.WriteLine("Invalid OutboundHTTPResponseString parameter sppplied 'values=' don't exist in the supplied string: " + mSearchResponse);
                                retval = -1;
                            }
                        }
                        else
                        {
                            string value = values[1];
                            mSearchResponse = "<" + tagName + ">" + value + "<" + tagName + ">";
                            Logger.WriteLine("Final response string to search in the returened XML from host: " + mSearchResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.WriteLine("Error Occured while trying to retrive  OutboundHTTPResponseString parameter, Stack Trace flows:", true);
                    Logger.WriteLine(ex.Message, true);
                }
                retval = -1;
            }

            if(retval == 0)// All OK till now?
            {
                try
                {
                    if (OrderId.Length > 0)
                    {
                        string OrderResponse = DoHTTPCall(OrderId);
                        if(OrderResponse.Length > 0)
                        {
                            XmlDocument returnXml = new XmlDocument();
                            returnXml.LoadXml(OrderResponse);
                            XmlElement resultNode = (XmlElement)returnXml.SelectSingleNode("/*/*[local-name()='status']");
                            if (resultNode != null)
                            {
                                String status = resultNode.InnerText.Trim();
                                if (status.Trim().ToLower().Equals("OK".ToLower()))
                                {
                                    if (OrderResponse.IndexOf(mSearchResponse) != -1)
                                    {
                                        if (Logger != null)
                                        {
                                            Logger.WriteLine("Result found with status OK");
                                        }
                                        retval = 0;
                                    }
                                }
                                else
                                {
                                    if (Logger != null)
                                    {
                                        Logger.WriteLine("Not able to find Result Node with OK Response");
                                    }
                                    retval = -1;
                                }
                            }
                            else
                            {
                                if (Logger != null)
                                {
                                    Logger.WriteLine("Not able to find Result Node with OK Response");
                                }
                                retval = -1;
                            }
                        }
                        retval = -1;
                    }
                    retval = -1;
                }
                catch (Exception ex)
                {
                    if (Logger != null)
                    {
                        Logger.WriteLine("Error Occured while Export , Error details:", true);
                        Logger.WriteLine(ex.Message, true);
                    }
                    retval = -1;
                }
            }
            else
            {
                if (Logger != null)
                {
                    Logger.WriteLine("Cannot proceed to do HTTP call, some of the preconditions did not match. Abroting.", true);
                }
            }
            return retval;
        }

        private string DoHTTPCall(string orderid)
        {
            string responseFromServer = "";
            try
            {
                string OrderURL = null;
                if (mHTTPURL.EndsWith("/"))
                {
                    OrderURL = mHTTPURL + orderid;
                }
                else
                {
                    OrderURL = mHTTPURL + "/" + orderid;
                }
                if (Logger != null)
                {
                    Logger.WriteLine("Proceeding to make HTTP GET to URL " + OrderURL, true);
                }

                WebRequest webRequest = WebRequest.Create(OrderURL);
                WebResponse webResp = webRequest.GetResponse();
                if (Logger != null)
                {
                    Logger.WriteLine("HTTP Response received with status " + ((HttpWebResponse)webResp).StatusDescription, true);
                }


                // Get the stream containing content returned by the server.
                Stream dataStream = webResp.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.
                responseFromServer = reader.ReadToEnd();

                if (Logger != null)
                {
                    Logger.WriteLine("HTTP Response Details :  " + responseFromServer, true);
                }

                // Clean up the streams and the response.
                reader.Close();
                webResp.Close();

            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.WriteLine("Error Occured while making HTTP call , Error details: " + ex.Message, true);
                }
            }
            return responseFromServer;
        }

        private string RetrieveOrderId(XmlDocument oXMLDoc)
        {
            string OrderId = "";
            XmlElement OutNode = (XmlElement)oXMLDoc.SelectSingleNode("DATACOLLECTION/DATA/ORDERID");
            if (OutNode != null)
            {
                OrderId = OutNode.InnerText.Trim();
            }
            return OrderId;
        }

        #endregion

        #endregion


    }
}