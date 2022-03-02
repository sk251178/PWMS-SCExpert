---- ***************  Queue definition ******************

if not exists (select * from MESSAGEQUEUES where QUEUENAME='SCExpertConnect')
insert into MESSAGEQUEUES(QUEUENAME,QUEUEPATH) values ('SCExpertConnect','.\private$\SCExpertConnect')
go

if not exists (select * from MESSAGEQUEUES where QUEUENAME='RESBSKU')
insert into MESSAGEQUEUES(QUEUENAME,QUEUEPATH,RESPONSEQ) values ('RESBSKU','.\private$\RESBSKU','.\private$\RESBSKUResponse')
go

if not exists (select * from MESSAGEQUEUES where QUEUENAME='RESBCOMPANY')
insert into MESSAGEQUEUES(QUEUENAME,QUEUEPATH,RESPONSEQ) values ('RESBCOMPANY','.\private$\RESBCOMPANY','.\private$\RESBCOMPANYResponse')
go

-- **************** Retalix RESB Plugin ***********************
-- Plugin Types
INSERT INTO SCEXPERTCONNECTPLUGINTYPES(PLUGINTYPEID,PLUGINTYPE,PLUGINNAME,ASSEMBLYDLL,CLASSNAME) 
VALUES(1,'SCExpertRESB','SCExpertRESB','SCExpertRESBPlugin.dll','MessageQueueListener')

-- Plugin Instances
-----------
--- SKU ---
-----------
INSERT INTO SCEXPERTCONNECTPLUGINS(PLUGINID,PLUGINDESCRIPTION,PLUGINTYPEID,PLUGINTRANSACTIONTYPE,LOGPATH,LOGLEVEL,BOTYPE,IMPORTTRANSLATIONFILE,WAREHOUSEID,COMMITBOONBOELEMENTFAILURE) 
VALUES(1,'SKU Interface',1,'IMPORT','C:\SCExpertConnectFiles\Logs\',1,'SKU',null,'SCEXPERT',1)

-- Import Params
INSERT INTO SCEXPERTCONNECTPLUGINPARAMS(PLUGINID,PARAMNAME,PARAMVALUE) VALUES(1,'MessageQueueName','RESBSKU')	

-- Transaction Keys
insert into SCEXPERTCONNECTPLUGINTRANSACTIONKEYS values(1,'CONSIGNEE')
insert into SCEXPERTCONNECTPLUGINTRANSACTIONKEYS values(1,'SKU')

---------------
--- Company ---
---------------
INSERT INTO SCEXPERTCONNECTPLUGINS(PLUGINID,PLUGINDESCRIPTION,PLUGINTYPEID,PLUGINTRANSACTIONTYPE,LOGPATH,LOGLEVEL,BOTYPE,IMPORTTRANSLATIONFILE,WAREHOUSEID,COMMITBOONBOELEMENTFAILURE) 
VALUES(2,'SKU Interface',1,'IMPORT','C:\SCExpertConnectFiles\Logs\',1,'COMPANY',null,'SCEXPERT',1)

-- Import Params
INSERT INTO SCEXPERTCONNECTPLUGINPARAMS(PLUGINID,PARAMNAME,PARAMVALUE) VALUES(2,'MessageQueueName','RESBCOMPANY')	

-- Transaction Keys
insert into SCEXPERTCONNECTPLUGINTRANSACTIONKEYS values(2,'CONSIGNEE')
insert into SCEXPERTCONNECTPLUGINTRANSACTIONKEYS values(2,'COMPANY')


-------------------------------------------------------
-- ************* SCExpertConnect **********************
-------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].SCEXPERTCONNECTPLUGINTYPES') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].SCEXPERTCONNECTPLUGINTYPES
go

/****** Object:  Table [dbo].[SCEXPERTCONNECTPLUGINTYPES]    Script Date: 05/24/2010 20:01:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCEXPERTCONNECTPLUGINTYPES](
	[PLUGINTYPEID] [int] NOT NULL,
	[PLUGINTYPE] [nvarchar](20) NOT NULL,
	[PLUGINNAME] [nvarchar](50) NULL,
	[ASSEMBLYDLL] [nvarchar](50) NOT NULL,
	[CLASSNAME] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_SCEXPERTCONNECTPLUGINTYPES] PRIMARY KEY CLUSTERED 
(
	[PLUGINTYPEID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].SCEXPERTCONNECTPLUGINPARAMS') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].SCEXPERTCONNECTPLUGINPARAMS
go

/****** Object:  Table [dbo].[SCEXPERTCONNECTPLUGINPARAMS]    Script Date: 05/24/2010 20:01:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCEXPERTCONNECTPLUGINPARAMS](
	[PLUGINID] [int] NOT NULL,
	[PARAMNAME] [nvarchar](50) NOT NULL,
	[PARAMVALUE] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_SCEXPERTCONNECTPLUGINPARAMS] PRIMARY KEY CLUSTERED 
(
	[PLUGINID] ASC,
	[PARAMNAME] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].SCEXPERTCONNECTPLUGINS') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].SCEXPERTCONNECTPLUGINS
go

/****** Object:  Table [dbo].[SCEXPERTCONNECTPLUGINS]    Script Date: 05/24/2010 20:01:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SCEXPERTCONNECTPLUGINS](
	[PLUGINID] [int] NOT NULL,
	[PLUGINDESCRIPTION] [nvarchar](50) NOT NULL,
	[PLUGINTYPEID] [int] NULL,
	[PLUGINTRANSACTIONTYPE] [nvarchar](50) NULL,
	[LOGPATH] [nvarchar](250) NULL,
	[LOGLEVEL] [int] NULL,
	[TRANSLATIONFILE] [binary](5000) NULL,
	[WAREHOUSEID] [nvarchar](20) NULL,
 CONSTRAINT [PK_SCEXPERTCONNECTPLUGINS] PRIMARY KEY CLUSTERED 
(
	[PLUGINID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].SCEXPERTPLUGINEVENTREGISTRATION') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].SCEXPERTPLUGINEVENTREGISTRATION
go

/****** Object:  Table [dbo].[SCEXPERTPLUGINEVENTREGISTRATION]    Script Date: 05/24/2010 20:01:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCEXPERTPLUGINEVENTREGISTRATION](
	[PLUGINID] [int] NOT NULL,
	[EVENTID] [int] NOT NULL,
	[WAREHOUSEID] [nvarchar](20) NOT NULL,
	[CONSIGNEE] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SCEXPERTPLUGINEVENTREGISTRATION] PRIMARY KEY CLUSTERED 
(
	[PLUGINID] ASC,
	[EVENTID] ASC,
	[WAREHOUSEID] ASC,
	[CONSIGNEE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SCEXPERTCONNECTPLUGINTYPES] ADD  CONSTRAINT [DF_SCEXPERTCONNECTPLUGINTYPES_ASSEMBLYDLL]  DEFAULT ('') FOR [ASSEMBLYDLL]
GO

ALTER TABLE [dbo].[SCEXPERTCONNECTPLUGINTYPES] ADD  CONSTRAINT [DF_SCEXPERTCONNECTPLUGINTYPES_CLASSNAME]  DEFAULT ('') FOR [CLASSNAME]
GO

ALTER TABLE [dbo].[SCEXPERTCONNECTPLUGINS] ADD  CONSTRAINT [DF_Table_1_PLUGINDESC]  DEFAULT ('') FOR [PLUGINDESCRIPTION]
GO

ALTER TABLE [dbo].[SCEXPERTPLUGINEVENTREGISTRATION] ADD  CONSTRAINT [DF_SCEXPERTPLUGINEVENTREGISTRATION_CONSIGNEE]  DEFAULT ('') FOR [CONSIGNEE]
GO

-----------------------------------------------------------------------
---- ******** Connect version 2.0 (4.8.27) update *********************
-----------------------------------------------------------------------
if not exists (select * from syscolumns sc join sysobjects so on sc.id=so.id 
				where so.name='SCEXPERTCONNECTPLUGINS' and sc.name='BOTYPE')
ALTER Table SCEXPERTCONNECTPLUGINS add BOTYPE nvarchar(20)
go
	
if not exists (select * from syscolumns sc join sysobjects so on sc.id=so.id 
				where so.name='SCEXPERTCONNECTPLUGINS' and sc.name='COMMITBOONBOELEMENTFAILURE')
ALTER Table SCEXPERTCONNECTPLUGINS add COMMITBOONBOELEMENTFAILURE BIT not null default 1
go

/****** Object:  Table [dbo].[SCEXPERTCONNECTPLUGINTRANSACTIONKEYS]    Script Date: 06/28/2010 10:58:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCEXPERTCONNECTPLUGINTRANSACTIONKEYS](
	[PLUGINID] [int] NOT NULL,
	[TRANSACTIONKEY] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_SCEXPERTCONNECTPLUGINTRANSACTIONKEYS] PRIMARY KEY CLUSTERED 
(
	[PLUGINID] ASC,
	[TRANSACTIONKEY] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SCEXPERTCONNECTTRANSACTION]    Script Date: 06/15/2010 14:53:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SCEXPERTCONNECTTRANSACTION](
	[TRANSACTIONID] [int] IDENTITY(1,1) NOT NULL,
	[TRANSACTIONDATE] [datetime] NOT NULL,
	[TRANSACTIONTYPE] [nvarchar](10) NOT NULL,
	[PLUGINID] [int] NOT NULL,
	[TRANSACTIONSTATUS] [nvarchar](20) NOT NULL,
	[TRANSACTIONSET] [nvarchar](20) NOT NULL,
	[TRANSACTIONBOTYPE] [nvarchar](20) NOT NULL,
	[TRANSACTIONBOKEY] [nvarchar](50) NOT NULL,
	[TRANSACTIONDATA] [nvarchar](max) NOT NULL,
	[TRANSACTIONERROR] [nvarchar](500) NULL,
 CONSTRAINT [PK_SCEXPERTCONNECTTRANSACTION] PRIMARY KEY CLUSTERED 
(
	[TRANSACTIONID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SCEXPERTCONNECTTRANSACTION] ADD  CONSTRAINT [DF_SCEXPERTCONNECTTRANSACTION_TRANSACTIONDATE]  DEFAULT (getdate()) FOR [TRANSACTIONDATE]
GO

ALTER TABLE [dbo].[SCEXPERTCONNECTTRANSACTION] ADD  CONSTRAINT [DF_SCEXPERTCONNECTTRANSACTION_TRANSACTIONSET]  DEFAULT ('') FOR [TRANSACTIONSET]
GO


-----------------------------------------------------------------------
---- ******** Connect version 4.8.28 update *********************
-----------------------------------------------------------------------
if not exists (select * from syscolumns sc join sysobjects so on sc.id=so.id 
				where so.name='SCEXPERTCONNECTPLUGINS' and sc.name='ALERTPROCESSRESULT')
ALTER Table SCEXPERTCONNECTPLUGINS add ALERTPROCESSRESULT nvarchar(20)
go
-- Values: None, OnError, Always


if not exists (select * from syscolumns sc join sysobjects so on sc.id=so.id 
				where so.name='SCEXPERTCONNECTPLUGINS' and sc.name='ALERTRECIPIENT')
ALTER Table SCEXPERTCONNECTPLUGINS add ALERTRECIPIENT nvarchar(100)
go


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].vSCExpertConectTransactions') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].vSCExpertConectTransactions
go

create view vSCExpertConectTransactions as 
select SCEXPERTCONNECTTRANSACTION.*,SCEXPERTCONNECTPLUGINS.PLUGINDESCRIPTION, SCEXPERTCONNECTPLUGINS.WAREHOUSEID,
SCEXPERTCONNECTPLUGINTYPES.PLUGINTYPEID,SCEXPERTCONNECTPLUGINTYPES.ASSEMBLYDLL,SCEXPERTCONNECTPLUGINTYPES.CLASSNAME
from SCEXPERTCONNECTTRANSACTION inner join SCEXPERTCONNECTPLUGINS
on SCEXPERTCONNECTPLUGINS.PLUGINID = SCEXPERTCONNECTTRANSACTION.PLUGINID 
inner join SCEXPERTCONNECTPLUGINTYPES on SCEXPERTCONNECTPLUGINS.PLUGINTYPEID = SCEXPERTCONNECTPLUGINTYPES.PLUGINTYPEID
go

insert into dbo.sys_menu (id,parent_id,screen_id,label,href,ordinal)
values ('SCExpertConnect','Setup','','SCExpert Connect','','100')
go

insert into dbo.sys_screen (screen_id,title,url,helptopic)
values ('scp','SCExpert Connect Plugins Types','Screens/SCExpertConnectSetupPluginTypes.aspx','')
go

insert into dbo.sys_menu (id,parent_id,screen_id,label,href,ordinal)
values ('SCExpertConnectPluginsTypes','SCExpertConnect','scp','SCExpert Connect Plugins Types','','10')
go

insert into dbo.sys_screen (screen_id,title,url,helptopic)
values ('scs','SCExpert Connect Plugins Setup','Screens/SCExpertConnectPluginSetup.aspx','')
go

insert into dbo.sys_menu (id,parent_id,screen_id,label,href,ordinal)
values ('SCExpertConnectSetup','SCExpertConnect','scs','SCExpert Connect Plugins Setup','','15')
go

insert into dbo.sys_screen (screen_id,title,url,helptopic)
values ('sct','SCExpert Connect Transactions','Screens/SCExpertConnectTransactions.aspx','')
go

insert into dbo.sys_menu (id,parent_id,screen_id,label,href,ordinal)
values ('SCExpertConnectTransactions','SCExpertConnect','sct','SCExpert Connect Transactions','','20')
go

insert into sys_param (param_code,param_name,param_value)
values('SCExpertConnectMailSenderFrom','SCExpertConnectMailSenderFrom','Support@made4net.com')

insert into sys_param (param_code,param_name,param_value)
values('SystemMailSenderSMTPServer','SystemMailSenderSMTPServer','neo10.made4net.com')


-----------------------------------------------------------------------
---- ********    Connect version 4.8.35 update    *********************
-----------------------------------------------------------------------

-----------------------------------------------------------------------
-- Filters should formed as pairs of 
-- <XPath for the current transaction node, Data (content of the node to match)>
-- Can be several pairs, seperated by ; (semicolon)
-----------------------------------------------------------------------

if not exists (select * from syscolumns sc join sysobjects so on sc.id=so.id 
				where so.name='SCEXPERTCONNECTPLUGINS' and sc.name='IMPORTTRANSACTIONCONTENTFILTER')
ALTER Table SCEXPERTCONNECTPLUGINS add IMPORTTRANSACTIONCONTENTFILTER nvarchar(200)
go

if not exists (select * from syscolumns sc join sysobjects so on sc.id=so.id 
				where so.name='SCEXPERTCONNECTPLUGINS' and sc.name='IMPORTTRANSLATIONFILE')
ALTER Table SCEXPERTCONNECTPLUGINS add IMPORTTRANSLATIONFILE nvarchar(200)
go

if not exists (select * from syscolumns sc join sysobjects so on sc.id=so.id 
				where so.name='SCEXPERTCONNECTPLUGINS' and sc.name='EXPORTTRANSACTIONCONTENTFILTER')
ALTER Table SCEXPERTCONNECTPLUGINS add EXPORTTRANSACTIONCONTENTFILTER nvarchar(200)
go

if not exists (select * from syscolumns sc join sysobjects so on sc.id=so.id 
				where so.name='SCEXPERTCONNECTPLUGINS' and sc.name='EXPORTTRANSLATIONFILE')
ALTER Table SCEXPERTCONNECTPLUGINS add EXPORTTRANSLATIONFILE nvarchar(200)
go

--if exists (select * from syscolumns sc join sysobjects so on sc.id=so.id 
--				where so.name='SCEXPERTCONNECTPLUGINS' and sc.name='TRANSLATIONFILE')
--update SCEXPERTCONNECTPLUGINS set IMPORTTRANSACTIONCONTENTFILTER = TRANSLATIONFILE
--go
--ALTER Table SCEXPERTCONNECTPLUGINS drop column TRANSLATIONFILE
--go

--************************************************
-- Events Registration (For export interfaces)
--************************************************
--INSERT INTO SCEXPERTPLUGINEVENTREGISTRATION(PLUGINID,EVENTID,WAREHOUSEID,CONSIGNEE) VALUES(11,36,'SCEXPERT','') -- snapshot
--INSERT INTO SCEXPERTPLUGINEVENTREGISTRATION(PLUGINID,EVENTID,WAREHOUSEID,CONSIGNEE) VALUES(12,40,'SCEXPERT','') -- receipt close
--INSERT INTO SCEXPERTPLUGINEVENTREGISTRATION(PLUGINID,EVENTID,WAREHOUSEID,CONSIGNEE) VALUES(13,21,'SCEXPERT','') -- outbound order shipped
--INSERT INTO SCEXPERTPLUGINEVENTREGISTRATION(PLUGINID,EVENTID,WAREHOUSEID,CONSIGNEE) VALUES(14,281,'SCEXPERT','') -- outbound order loaded
--INSERT INTO SCEXPERTPLUGINEVENTREGISTRATION(PLUGINID,EVENTID,WAREHOUSEID,CONSIGNEE) VALUES(15,20,'SCEXPERT','') -- outbound order planned
--INSERT INTO SCEXPERTPLUGINEVENTREGISTRATION(PLUGINID,EVENTID,WAREHOUSEID,CONSIGNEE) VALUES(16,31,'SCEXPERT','') -- Inventory Adjustments


