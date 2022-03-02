using Made4Net.General.IO.Folder;
using Made4Net.Shared;
using SCExpertConnectPlugins.BO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using System.Xml;
using System.Xml.Xsl;

namespace SCExpertConnectFilePlugin
{
    public class FilesPlugin : BasePlugin
    {
        private readonly double C_DEFAULT_IMPORT_INTERVAL = 30 * 60 * 1000;
        private readonly string C_EXPORT_COUNTER_PROPERTY_NAME = "ExportFileNameCounter";
        private readonly string C_EXPORT_TIMEFORMAT_PROPERTY_NAME = "ExportFileNameTimeStampFormat";
        private readonly string C_EXPORT_PRIMARYKEYS_PROPERTY_NAME = "ExportFileNamePrimaryKeys";
        private readonly string C_KEYWORD_EXPORT_PRIMARY_KEY = "KEY#";
        private readonly char C_PRIMARY_KEY_DELIMETER = ';';

        private Timer _importTimer = null;
        private readonly IFolderDefinition _importDirectory = null;
        private readonly IFolderDefinition _moveOnFailureFolder;
        private readonly IFolderDefinition _moveOnSuccessFolder;
        private readonly string _importFileNameFilter;
        private readonly Made4Net.Shared.XSLTransformer _xsltTransformer;
        private readonly string _importCustomTranslationFile;
        private readonly string _exportCustomTranslationFile;
        private readonly IFolderDefinition _exportOutputFolder;
        private readonly string _exportOutputFileNamePrefix;

        public FilesPlugin(int pPluginId)
            : base(pPluginId)
        {
            _exportOutputFolder = FolderDefinitionFactory.Create(GetParameterValue("ExportOutputFolder"), null);
            _exportOutputFileNamePrefix = GetParameterValue("ExportOutputFileNamePrefix");
            _importDirectory = FolderDefinitionFactory.Create(GetParameterValue("ImportDirectory"), null);
            _moveOnFailureFolder = FolderDefinitionFactory.Create(GetParameterValue("MoveOnFailureFolder"), null);
            _moveOnSuccessFolder = FolderDefinitionFactory.Create(GetParameterValue("MoveOnSuccessFolder"), null);
            _importFileNameFilter = GetParameterValue("ImportFileNameFilter");
            _xsltTransformer = new Made4Net.Shared.XSLTransformer();
            _importCustomTranslationFile = GetParameterValue("ImportCustomTranslationFile");
            _exportCustomTranslationFile = GetParameterValue("ExportCustomTranslationFile");
        }

        private void WriteToLog(string pText)
        {
            Logger.SafeWriteLine(pText, true);
        }

        public override void Import()
        {
            InitTimer();
            ImportTimer_Elapsed(null, null);
            _importTimer.Elapsed += new ElapsedEventHandler(ImportTimer_Elapsed);
            _importTimer.Start();
        }

        public void TestImport(object source)
        {
            ImportTimer_Elapsed(source, null);
        }

        private void InitTimer()
        {
            if (_importTimer == null)
            {
                try
                {
                    _importTimer = new Timer(double.Parse(GetParameterValue("ImportTimerInterval")) * 1000);
                }
                catch
                {
                    _importTimer = new Timer(C_DEFAULT_IMPORT_INTERVAL);
                }
            }
        }

        private void ImportTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                if (_importTimer != null)
                {
                    _importTimer.Enabled = false;
                }

                List<XmlDocument> oTranslatedFiles = new List<XmlDocument>();
                using (IFolder inputFolder = _importDirectory.Create(x => WriteToLog(x)))
                using (IFolder failureFolder = _moveOnFailureFolder.Create(x => WriteToLog(x)))
                {
                    WriteToLog(string.Format("Timer elapsed"));

                    foreach (FileInfo oInputFile in inputFolder.DirectoryInfo.GetFiles(_importFileNameFilter))
                    {
                        try
                        {
                            oTranslatedFiles.Clear();
                            InitLogger(oInputFile.Name);
                            WriteToLog(string.Format("Start processing file {0}", oInputFile.Name));


                            XmlDocument OutDoc;
                            if (!string.IsNullOrEmpty(_importCustomTranslationFile))
                            {
                                OutDoc = _xsltTransformer.Transform(oInputFile, _importCustomTranslationFile);
                            }
                            else
                            {
                                try
                                {
                                    OutDoc = new XmlDocument();
                                    OutDoc.Load(oInputFile.FullName);
                                }
                                catch { OutDoc = null; }
                            }

                            if (OutDoc == null)
                            {
                                string newFileName = oInputFile.Name.TrimEnd(oInputFile.Extension.ToCharArray())
                                    + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                                    + new Random().Next(1000).ToString() + "."
                                    + oInputFile.Extension;

                                WriteToLog(string.Format("Failed to parse file, file moved to {0}, (new file name: {1})",
                                    failureFolder.DirectoryInfo.FullName, newFileName));

                                oInputFile.CopyTo(Path.Combine(failureFolder.DirectoryInfo.FullName, newFileName));
                                oInputFile.Delete();
                                continue;
                            }

                            WriteToLog(string.Format("File proccessed successfully, total transactions found: {0}", OutDoc.FirstChild.ChildNodes.Count.ToString()));
                            WriteToLog(string.Format("XML output from plugin: {0}", OutDoc.InnerXml));

                            oTranslatedFiles.Add(OutDoc);
                        }
                        catch (Exception ex)
                        {
                            WriteToLog("Error Occured while importing file:");
                            WriteToLog(ex.ToString());
                        }
                        finally
                        {
                            if (oTranslatedFiles.Count > 0)
                            {
                                WriteToLog(string.Format("Sending results to SCExpertConnect service..."));
                                mTransactionSet = oInputFile.Name;
                                NotifyImportProcessComplete(oTranslatedFiles);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    WriteToLog("Error Occured while Accessing folder...");
                    WriteToLog(ex.ToString());
                }
            }
            finally
            {
                if (_importTimer != null)
                {
                    _importTimer.Enabled = true;
                }
            }
        }


        public override void ProcessResultFromSCExpertConect(string pTransactionSet, Dictionary<string, ExpertObjectMapper.DataMapperProcessResult> pObjectDataMapperResult)
        {
            try
            {
                StringBuilder oStringbuilder = new StringBuilder();
                oStringbuilder.AppendLine(string.Format("Results for processing transaction set {0}", pTransactionSet));
                oStringbuilder.AppendLine("");
                WriteToLog(string.Format("Processing results for transaction set id: {0}", pTransactionSet));
                string sImportFile = null;
                bool ProcessSucceeded = true;
                if (pObjectDataMapperResult.Count == 0)
                    ProcessSucceeded = false;

                foreach (KeyValuePair<string, ExpertObjectMapper.DataMapperProcessResult> kvp in pObjectDataMapperResult)
                {
                    if (kvp.Value.TransactionResult == ExpertObjectMapper.ObjectProcessor.DataMapperTransactionResult.Success)
                    {
                        oStringbuilder.AppendLine(string.Format("Transaction ID {0} succeeded.", kvp.Key));
                        WriteToLog(string.Format("Transaction ID {0} Processed successfully by the SCExpert Connect.", kvp.Key));
                    }
                    else
                    {
                        oStringbuilder.AppendLine(string.Format("Transaction ID {0} failed.", kvp.Key));
                        WriteToLog(string.Format("Transaction ID {0} failed by the SCExpert Connect.", kvp.Key));
                        WriteToLog(string.Format("Error message and description:", kvp.Value.TransactionErrorMessage));
                        ProcessSucceeded = false;
                    }
                }

                using (IFolder importFolder = _importDirectory.Create((x) => WriteToLog(x)))
                using (IFolder successFolder = _moveOnSuccessFolder.Create((x) => WriteToLog(x)))
                using (IFolder failureFolder = _moveOnFailureFolder.Create((x) => WriteToLog(x)))
                {
                    sImportFile = Path.Combine(importFolder.DirectoryInfo.FullName, pTransactionSet);
                    FileInfo oFile = new FileInfo(sImportFile);
                    if (!oFile.Exists)
                    {
                        WriteToLog(string.Format("File {0} not found in directory {1}", sImportFile, importFolder.DirectoryInfo.FullName));
                        return;
                    }
                    string newName;
                    if (ProcessSucceeded)
                    {
                        newName = Path.Combine(successFolder.DirectoryInfo.FullName,
                            oFile.Name.TrimEnd(oFile.Extension.ToCharArray())
                            + DateTime.Now.ToString("_yyyyMMdd_hhmmss")
                            + new Random().Next(1000).ToString()
                            + oFile.Extension);

                        oFile.CopyTo(newName);
                        WriteToLog(string.Format("File was successfully processed and moved to {0}", newName));
                    }
                    else
                    {
                        newName = Path.Combine(failureFolder.DirectoryInfo.FullName,
                            oFile.Name.TrimEnd(oFile.Extension.ToCharArray())
                            + DateTime.Now.ToString("_yyyyMMdd_hhmmss")
                            + new Random().Next(1000).ToString()
                            + oFile.Extension);

                        oFile.CopyTo(newName);
                        WriteToLog(string.Format("Failed to process file, file moved to {0}", newName));
                    }
                    oStringbuilder.AppendLine(string.Format("File moved to {0}", newName));
                    oFile.Delete();
                }

            }
            catch (Exception ex)
            {
                WriteToLog("Error Occured while processing results from SCExpert Connect, Error details:");
                WriteToLog(ex.ToString());
            }
        }

        public override int Export(System.Xml.XmlDocument oXMLDoc)
        {
            try
            {
                InitLogger("Export");
                WriteToLog("Exporting XML...");
                using (IFolder outputFolder = _exportOutputFolder.Create((x) => WriteToLog(x)))
                {
                    string filePath = Path.Combine(outputFolder.DirectoryInfo.FullName, FileNameBuilder(oXMLDoc));
                    WriteToLog(string.Format("trying to write file: {0}", filePath));
                    if (string.IsNullOrEmpty(_exportCustomTranslationFile))
                    {
                        WriteToLog("Output file format is XML");
                        oXMLDoc.Save(filePath);
                    }
                    else
                    {
                        WriteToLog("Running XSLT script on file");
                        FileInfo oFile = new FileInfo(filePath);
                        XslCompiledTransform xslt = new XslCompiledTransform();
                        StreamWriter sw = oFile.CreateText();

                        using (XmlWriter xmlWriter = new XmlTextWriter(sw))
                        {
                            xslt.Load(_exportCustomTranslationFile);
                            xslt.Transform(oXMLDoc, xmlWriter);
                            sw.Write(sw.ToString().Replace("System.IO.StreamWriter", ""));
                        }
                        sw = null;
                        xslt = null;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Error occured in export process:");
                WriteToLog("XML document:" + oXMLDoc.InnerXml.ToString());

                WriteToLog(ex.ToString());
                return -1;
            }
            return 0;
        }

        public override void SaveRawXML(System.Xml.XmlDocument oXMLDoc, string Refname)
        {
            try
            {
                string rawfilepath = string.Empty;
                rawfilepath = Made4Net.DataAccess.Util.GetInstancePath() + "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectLogPath;  //PWMS-817
                string filename = "RawXml_" + Refname;
                string FilePath = string.Format("{0}{1}", rawfilepath, filename);
                oXMLDoc.Save(FilePath + ".xml");
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    WriteToLog("Error Occured while saving XML file.");
                    WriteToLog(ex.ToString());
                }
            }
        }

        private string FileNameBuilder(XmlDocument pXMLDoc)
        {
            string fileName = _exportOutputFileNamePrefix.Split(';')[0];
            string[] values = _exportOutputFileNamePrefix.Substring(_exportOutputFileNamePrefix.IndexOf(';') + 1).Split(';');
            List<string> primaryKeys = null;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == C_EXPORT_TIMEFORMAT_PROPERTY_NAME)
                {
                    try
                    {
                        fileName = fileName.Replace("{" + i + "}", DateTime.Now.ToString(GetParameterValue(C_EXPORT_TIMEFORMAT_PROPERTY_NAME)));
                    }
                    catch (Exception ex)
                    {
                        WriteToLog("Error in Time format:");
                        WriteToLog(ex.ToString());
                    }
                }
                else if (values[i] == C_EXPORT_COUNTER_PROPERTY_NAME)
                {
                    try
                    {
                        fileName = fileName.Replace("{" + i + "}", Made4Net.Shared.Util.getNextCounter(GetParameterValue(C_EXPORT_COUNTER_PROPERTY_NAME)));
                    }
                    catch (Exception ex)
                    {
                        WriteToLog("Error in counter:");
                        WriteToLog(ex.ToString());
                    }
                }
                else if (values[i].StartsWith(C_KEYWORD_EXPORT_PRIMARY_KEY))
                {
                    if (primaryKeys == null)
                        primaryKeys = GetPrimaryKeys();
                    string key;
                    try
                    {
                        key = pXMLDoc.SelectSingleNode(primaryKeys[int.Parse(values[i].Substring(4))]).InnerText;
                    }
                    catch (Exception ex)
                    {
                        key = "";
                        WriteToLog(string.Format("Error in {0}:", values[i]));

                        //RWMS-2563 START
                        WriteToLog(string.Format("Primary key value:" + values[i]));
                        WriteToLog(pXMLDoc.InnerXml.ToString());
                        //RWMS-2563 END

                        WriteToLog(ex.ToString());
                    }
                    fileName = fileName.Replace("{" + i + "}", key);
                }
            }
            return fileName;
        }

        private List<string> GetPrimaryKeys()
        {
            List<string> primaryKeysList = null;
            try
            {
                string primaryKeys = GetParameterValue(C_EXPORT_PRIMARYKEYS_PROPERTY_NAME);
                if (!string.IsNullOrEmpty(primaryKeys))
                {
                    if (primaryKeys.Contains(C_PRIMARY_KEY_DELIMETER.ToString()))
                        primaryKeysList = new List<string>(primaryKeys.Split(C_PRIMARY_KEY_DELIMETER));
                    else
                    {
                        primaryKeysList = new List<string>
                        {
                            primaryKeys
                        };
                    }
                }
            }
            catch { return null; }
            return primaryKeysList;
        }
    }
}