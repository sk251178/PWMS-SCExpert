using Made4Net.General.IO.Folder;
using Made4Net.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Messaging;
using System.Text.RegularExpressions;
using WMS.Logic;
using IO = System.IO;

namespace WMS.TaxRight
{
    public class TaxRightHandler : Made4Net.Shared.QHandler
    {
        private readonly bool useLogs;
        private readonly IFolder logFolder;
        private readonly IFolder exportFolder;
        private readonly IFolder failedFolder;
        private readonly string exportFileFormat;

        private readonly Regex regex = new Regex(@"{\s*(?<type>FIELD|DATE)\s*:(?<value>.*?)}",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Func<DateTime> CurrentTimeDelegate { get; set; }

        public DateTime CurrentTime
        {
            get
            {
                if(CurrentTimeDelegate == null)
                {
                    return DateTime.Now;
                }
                return CurrentTimeDelegate.Invoke();
            }
        }

        public TaxRightHandler() : base("TaxRight", false)
        {
            useLogs = Made4Net.Shared.Util.GetAppConfigNameBooleanValue(ConfigurationSettingsConsts.TaxRightServiceSection,
                ConfigurationSettingsConsts.TaskManagerServiceUseLogs);

            if (useLogs)
            {
                string logDirectoryConfigValue = Made4Net.Shared.Util.GetAppConfigNameValue(
                    ConfigurationSettingsConsts.TaxRightServiceSection,
                    ConfigurationSettingsConsts.TaxRightServiceLogFolder);

                logFolder = FolderDefinitionFactory.Create(logDirectoryConfigValue, null).Create(null);
            }

            string exportFolderConfigValue = Made4Net.Shared.Util.GetAppConfigNameValue(
                ConfigurationSettingsConsts.TaxRightServiceSection,
                    ConfigurationSettingsConsts.TaxRightExportFolder);


            exportFolder = FolderDefinitionFactory.Create(exportFolderConfigValue, null).Create(null);

            exportFileFormat = Made4Net.Shared.Util.GetAppConfigNameValue(
                ConfigurationSettingsConsts.TaxRightServiceSection,
                    ConfigurationSettingsConsts.TaxRightExportFileFormat);

            failedFolder = FolderDefinitionFactory.Create(
                IO.Path.Combine(Made4Net.DataAccess.Util.GetInstancePath(), "Interfaces", "Export", "TaxRight", "Failed"),
                null).Create(null);
        }

        protected string ExpandFileFormat(ReadOnlyDictionary<string,string> significantFields)
        {
            string fileName = regex.Replace(exportFileFormat, (Match m) =>
            {
                if (string.Equals(m.Groups["type"].Value, "FIELD", StringComparison.InvariantCultureIgnoreCase))
                {
                    string fieldName = m.Groups["value"].Value.Trim();
                    if (significantFields.ContainsKey(fieldName))
                    {
                        return significantFields[fieldName];
                    }
                }
                else if (string.Equals(m.Groups["type"].Value, "DATE", StringComparison.InvariantCultureIgnoreCase))
                {
                    return CurrentTime.ToString(m.Groups["value"].Value);
                }
                return string.Empty;
            });
            foreach (char c in IO.Path.GetInvalidFileNameChars())
            {
                fileName.Replace(c, '-');
            }
            return fileName;
        }

        protected override void ProcessQueue(Message message, QMsgSender sender, PeekCompletedEventArgs eventArgs)
        {
            if(sender == null)
            {
                throw new ArgumentException($"{nameof(sender)} cannot be null");
            }

            string logFileName = $"TAXRIGHT-{sender.Values["DOCUMENT"]}-{CurrentTime:yyyyMMddHHmmss-fff}.txt";
            ILogHandler logger = useLogs ? new LogHandler(logFolder.DirectoryInfo.FullName, logFileName) : null;

            try
            {
                TaxRightFile taxRightFile = new TaxRightFile(sender.Values);
                if(taxRightFile.FixedWidthLines.Any())
                {
                    logger.SafeWrite(PrintMessageContent(sender));
                    string exportFileName = ExpandFileFormat(taxRightFile.SignificantFields);
                    string exportFilePath = IO.Path.Combine(exportFolder.DirectoryInfo.FullName, exportFileName);
                    string failedFilePath = IO.Path.Combine(failedFolder.DirectoryInfo.FullName, exportFileName);

                    IO.StreamWriter sw = null;
                    try
                    {
                        try
                        {
                            sw = new IO.StreamWriter(exportFilePath, false);
                        }
                        catch(Exception e1)
                        {
                            logger.SafeWrite($"Error: Unable to export the file to the chosen location: {exportFilePath}");
                            logger.SafeWriteSeperator();
                            logger.SafeWriteException(e1);
                            logger.SafeWriteSeperator();

                            logger.SafeWrite($"Export file will be written to: {failedFilePath}");
                            try
                            {
                                sw = new IO.StreamWriter(failedFilePath, false);
                            }
                            catch(Exception e2)
                            {
                                logger.SafeWrite($"Error: Unable to export the file to the failed location: {failedFilePath}");
                                logger.SafeWriteSeperator();
                                logger.SafeWriteException(e2);
                                logger.SafeWriteSeperator();
                                logger.SafeWrite("File contents:");
                                logger.SafeWriteSeperator();
                                foreach (FixedWidthLine line in taxRightFile.FixedWidthLines)
                                {
                                    logger.SafeWrite(line.AsString(null));
                                }
                                logger.SafeWriteSeperator();
                                throw;
                            }
                        }

                        logger.SafeWrite($"Writing TaxRight file to: {exportFilePath}");
                        logger.SafeWriteSeperator();
                        foreach (FixedWidthLine line in taxRightFile.FixedWidthLines)
                        {
                            string text = line.AsString(logger);
                            logger.SafeWrite(text);
                            sw.WriteLine(text);
                        }
                        sw.Close();
                        logger.SafeWriteSeperator();
                        logger.SafeWrite("Export complete.");

                    }
                    finally
                    {
                        sw?.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                logger.SafeWriteException(e);
                throw;
            }
        }

        protected new string PrintMessageContent(QMsgSender sender)
        {
            string msgContent = "******************** MESSAGE CONTENT *****************";
            msgContent += "\r\n";
            for (int i = 0; i < sender.Values.Count - 1; i++)
            {
                msgContent = msgContent + sender.Values.GetKey(i) + " : " + sender.Values[i] + "\r\n";
            }
            msgContent = msgContent + "******************** END OF MESSAGE CONTENT *****************" + "\r\n";
            return msgContent;
        }
    }
}