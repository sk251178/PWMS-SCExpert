@echo off

AltovaXML /xslt1 "MappingMapToOutboundScheme.xslt" /in "C:/Projects/SCExpertTrunk/SCExpertConnectSBOPlugin/XSLMaps/Documents/SalesOrders/SapSaleOrderSample.xml" /out "C:/Projects/SCExpertTrunk/SCExpertConnectSBOPlugin/XSLMaps/Documents/SalesOrders/OutboundScheme.xml" %*
IF ERRORLEVEL 1 EXIT/B %ERRORLEVEL%
