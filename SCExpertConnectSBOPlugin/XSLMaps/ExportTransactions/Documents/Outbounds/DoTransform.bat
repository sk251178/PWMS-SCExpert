@echo off

AltovaXML /xslt1 "MappingMapToSBODocumentSample1.xslt" /in "C:/Projects/SCExpertTrunk/SCExpertConnectSBOPlugin/XSLMaps/ExportTransactions/Documents/Outbounds/OutboundOrderSample.xml" /out "C:/Projects/SCExpertTrunk/SCExpertConnectSBOPlugin/XSLMaps/ExportTransactions/Documents/SBODocumentSample.xml" %*
IF ERRORLEVEL 1 EXIT/B %ERRORLEVEL%
