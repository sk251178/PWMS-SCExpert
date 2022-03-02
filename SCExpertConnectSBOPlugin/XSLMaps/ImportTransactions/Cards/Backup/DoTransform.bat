@echo off

AltovaXML /xslt1 "MappingMapToExpertCompany.xslt" /in "C:/Projects/SCExpertTrunk/SCExpertConnectSBOPlugin/XSLMaps/Cards/SBOCardSample.xml" /out "C:/Projects/SCExpertTrunk/SCExpertConnectSBOPlugin/XSLMaps/Cards/ExpertCompany.xml" %*
IF ERRORLEVEL 1 EXIT/B %ERRORLEVEL%
