@echo off

AltovaXML /xslt1 "MappingMapToInboundOrderScheme.xslt" /in "C:/Projects/SCExpertTrunk/SCExpertConnectSBOPlugin/XSLMaps/Documents/PurchaseOrders/SapPurchaseOrder.xml" /out "C:/Projects/SCExpertTrunk/SCExpertConnectSBOPlugin/XSLMaps/Documents/PurchaseOrders/InboundOrderScheme.xml" %*
IF ERRORLEVEL 1 EXIT/B %ERRORLEVEL%
