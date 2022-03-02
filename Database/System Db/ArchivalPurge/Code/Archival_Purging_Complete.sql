-- Please edit the destination server name at line 136 before running the script.


/****** Object:  Table [dbo].[RWMS_LOG_DETAIL]    Script Date: 07/01/2014 17:25:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO


CREATE TABLE [dbo].[RWMS_LOG_DETAIL](
	[LOG_ID] [int] IDENTITY(1,1) NOT NULL,
	[ARCH_ID] [int] NOT NULL,
	[MESSAGE_TYPE] [char](1) NULL,
	[MSG] [nvarchar](max) NULL,
	[EXECUTION_DATE] [datetime] NULL,
	[RUN_STATUS] [smallint] NULL,
	[PARAMETERS_PASSED] [varchar](250) NULL,
 CONSTRAINT [PK_RWMS_LOG_DETAIL] PRIMARY KEY CLUSTERED 
(
	[LOG_ID] ASC,
	[ARCH_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[RWMS_ARCHIVAL_MASTER]    Script Date: 07/01/2014 17:26:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RWMS_ARCHIVAL_MASTER](
	[dest_server_name] [nvarchar](250) NULL,
	[source_db_name] [nvarchar](250) NULL,
	[dest_db_name] [nvarchar](250) NULL,
	[Linkedservername] [nvarchar](250) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RWMS_ARCHIVAL_DETAIL]    Script Date: 07/08/2014 12:06:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RWMS_ARCHIVAL_DETAIL](
	[ARCH_ID] [int] IDENTITY(1,1) NOT NULL,
	[PREFIX_CODE] [NVARCHAR](50) NULL,
	[DESCRIPTION] [NVARCHAR](250) NULL,
	[JOB_TYPE] [char](1) NULL,
	[RETENTION_PERIOD] [int] NULL,
	[VALID_FROM] [datetime] NULL,
	[VALID_TO] [datetime] NULL,
	[CREATED_BY] [NVARCHAR](20) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [NVARCHAR](20) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[PROC_NAME]  AS ('RWMS_SP_'+[PREFIX_CODE]),
	[loglevel] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ARCH_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



/****** Object:  Table [dbo].[RWMS_LOOKUP_JOBTYPE]    Script Date: 07/08/2014 12:06:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RWMS_LOOKUP_JOBTYPE](
	[JOB_TYPE] [char](1) NULL,
	[JOB_DESC] NVARCHAR(500) NULL
) ON [PRIMARY]


GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[RWMS_LOOKUP_LOGLEVEL]    Script Date: 07/08/2014 12:06:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RWMS_LOOKUP_LOGLEVEL](
	[LOGLEVEL] [char](1) NULL,
	[LOG_DESC] NVARCHAR(500) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO	

ALTER TABLE [dbo].[RWMS_LOG_DETAIL]  WITH CHECK ADD  CONSTRAINT [FK_RWMS_LOG_DETAILS_RWMS_ARCHIVAL_DETAIL] FOREIGN KEY([ARCH_ID])
REFERENCES [dbo].[RWMS_ARCHIVAL_DETAIL] ([ARCH_ID])
GO

ALTER TABLE [dbo].[RWMS_LOG_DETAIL] CHECK CONSTRAINT [FK_RWMS_LOG_DETAILS_RWMS_ARCHIVAL_DETAIL]
GO


/*
IF NOT EXISTS ( SELECT 1 FROM RWMS_ARCHIVAL_MASTER ) 
PRINT 'Please update the destination server name in the RWMS_ARCHIVAL_MASTER table to successfully create a linked server. Exiting now...!';
RETURN;
GO*/

-- Please edit the destination server name
INSERT [dbo].[RWMS_ARCHIVAL_MASTER] ([dest_server_name], [source_db_name], [dest_db_name], [Linkedservername]) VALUES (N'INBE00RTLIXP002', N'RWMS', N'RWMS_Archive', NULL);

INSERT INTO [dbo].[RWMS_LOOKUP_JOBTYPE] VALUES ('A', 'Archival Only'), ('P', 'Purging Only'), ('B', 'Archival and Purging only'), ('N', 'No archival/purging');

INSERT INTO [dbo].[RWMS_LOOKUP_LOGLEVEL] VALUES ('E', 'Error messages only'), ('D', 'Error/Debug/Informational messages only'), ('M', 'Informational/Error messages only');

INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('WAVE' , 'Archival and/or Purging for WAVE and dependent tables' , 'B' , 31  , ' ' , ' ' , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('TASKS' , 'Archival and/or Purging for TASKS and dependent tables' , 'B' , 31  , ' ' , ' ' , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('AUDIT' , 'Archival and/or Purging for AUDIT and dependent table' , 'B' , 31  , ' ' , ' ' , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('LOADS' , 'Archival and/or Purging for the LOADS table' , 'N' , 31  , ' ' , ' ' , 'System' , ' ' , ' ' , ' ' , 'E' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('COUNTING' , 'Archival and/or Purging for the COUNTING table' , 'B' , 31  , ' ' , ' ' , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('OUTBOUND' , 'Archival and/or Purging for the OUTBOUND related tables' , 'B' , 31  , ' ' , ' ' , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('RECEIPT' , 'Archival and/or Purging for RECEIPT related tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('TRANSSHIPMENT' , 'Archival and/or Purging for the TRANSSHIPMENT tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('PARALLELPICK' , 'Archival and/or Purging for the PARALLELPICK tables' , 'B' , 31  , GETDATE(), GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('WORKORDER' , 'Archival and/or Purging for the WORKORDER table' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('HANDLINGUNITTRANSACTIONS' , 'Archival and/or Purging for the HANDLINGUNITTRANSACTIONS tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ', ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('INVENTORYTRANS' , 'Archival and/or Purging for the INVENTORYTRANS tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System ' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('SHIPMENT' , 'Archival and/or Purging for SHIPMENT and dependent tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('INBOUND' , 'Archival and/or Purging for INBOUND and dependent tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('FLOWTHROUGH' , 'Archival and/or Purging for FLOWTHROUGH and dependent tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('PICK' , 'Archival and/or Purging for PICK and dependent tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('REPLENISHMENT' , 'Archival and/or Purging for REPLENISHMENT and dependent tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('WORKORDERBOM' , 'Archival and/or Purging for WORKORDERBOM and dependent tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 
INSERT INTO RWMS_ARCHIVAL_DETAIL (PREFIX_CODE, DESCRIPTION, JOB_TYPE, RETENTION_PERIOD, VALID_FROM, VALID_TO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, loglevel ) VALUES ('SHIFT' , 'Archival and/or Purging for SHIFT and dependent tables' , 'B' , 31  , GETDATE() , GETDATE() , 'System' , ' ' , ' ' , ' ' , 'M' ); 

SET IDENTITY_INSERT [dbo].[RWMS_ARCHIVAL_DETAIL] ON;
INSERT [dbo].[RWMS_ARCHIVAL_DETAIL] ([ARCH_ID], [PREFIX_CODE], [DESCRIPTION], [JOB_TYPE], [RETENTION_PERIOD], [VALID_FROM], [VALID_TO], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE], [loglevel]) VALUES (9999, N'NA', N'NA', N'N', NULL, CAST(0x0000000000000000 AS DateTime), CAST(0x0000000000000000 AS DateTime), N'system_default', CAST(0x0000000000000000 AS DateTime), N'', CAST(0x0000000000000000 AS DateTime), N' ');
SET IDENTITY_INSERT [dbo].[RWMS_ARCHIVAL_DETAIL] OFF;


IF OBJECT_ID('RWMS_SP_CREATE_LINKED_SERVER') IS NOT NULL
DROP PROCEDURE RWMS_SP_CREATE_LINKED_SERVER
GO

/*
DECLARE @linkServer VARCHAR(100);
EXEC RWMS_SP_CREATE_LINKED_SERVER 'INBELW014496A\DR_SERVER', @linked_server_name = @linkServer OUTPUT
SELECT @linkServer
*/

-- DENY VIEW DEFINITION ON OBJECT::RWMS_SP_CREATE_LINKED_SERVER TO PUBLIC;

CREATE PROCEDURE RWMS_SP_CREATE_LINKED_SERVER
AS
/*------ Comment section
	Author : Diwakar M
	Created Date : 07/02/2014
	JIRA : RWMS-18
	Purpose : The procedure creates a linked server connection to the archive server
	Execution : EXEC RWMS_SP_CREATE_LINKED_SERVER 
*/
BEGIN TRY

	SET NOCOUNT ON;
	
	DECLARE @server_name NVARCHAR(250);
	DECLARE @login_name VARCHAR(250);
	
	SET @server_name = ( SELECT dest_server_name FROM RWMS_ARCHIVAL_MASTER );
	
	IF EXISTS ( SELECT 1 FROM sys.servers WHERE name = @server_name )
	BEGIN
			EXEC sp_testlinkedserver @servername = @server_name;		
			
			RETURN;
	END
	ELSE
	BEGIN
	
	/*------------[ Create a new login on the source server to map it to on at the destination server------*/
	/*
	CREATE LOGIN maint_admin WITH PASSWORD = 'retalix2adm';
	
	SET @login_name = 'maint_admin'; */
	
	SET @login_name = (SELECT name FROM master.sys.syslogins WHERE name = 'maint_admin');
	
	/*------------[ Adds linked server and also maps logins from source-to-linked server ]------------------*/

		--exec sp_addlinkedserver @server = @servername, @srvproduct=N'', @provider=N'SQLNCLI', @datasrc=@servername;
		
		EXEC master.dbo.sp_addlinkedserver @server = @server_name, @srvproduct=N'SQL Server';
		
		EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname = @server_name, @locallogin = @login_name, @useself = N'True';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'collation compatible', @optvalue=N'false';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'data access', @optvalue=N'true';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'dist', @optvalue=N'false';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'pub', @optvalue=N'false';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'rpc', @optvalue=N'false';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'rpc out', @optvalue=N'false';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'sub', @optvalue=N'false';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'connect timeout', @optvalue=N'0';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'collation name', @optvalue=null;
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'lazy schema validation', @optvalue=N'false';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'query timeout', @optvalue=N'0';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'use remote collation', @optvalue=N'true';
		
		EXEC master.dbo.sp_serveroption @server=@server_name, @optname=N'remote proc transaction promotion', @optvalue=N'true';
		



		/*-------------[ Check the connection ]------------------------*/
		EXEC sp_testlinkedserver @servername= @server_name;
		
		
		--INSERT INTO RWMS_ARCHIVAL_MASTER (dest_server_name, source_db_name, dest_db_name, Linkedservername ) 
		IF EXISTS ( SELECT 1 FROM RWMS_ARCHIVAL_MASTER )
		BEGIN
			UPDATE RWMS_ARCHIVAL_MASTER SET Linkedservername = @server_name;		
		END
	END
		
END TRY
BEGIN CATCH

	SELECT 
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;

END CATCH

GO



/****** Object:  StoredProcedure [dbo].[RWMS_SP_LOG_MESSAGE]    Script Date: 07/08/2014 18:58:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('RWMS_SP_LOG_MESSAGE') IS NOT NULL
DROP PROCEDURE RWMS_SP_LOG_MESSAGE
GO

CREATE PROCEDURE [dbo].[RWMS_SP_LOG_MESSAGE] ( @archid INT, @loglevel CHAR(1), @messagetype CHAR(1), @informational_message NVARCHAR(MAX) = NULL )
AS
		/*
				Author : Diwakar M
				Created Date : 10/05/2014
				JIRA : JIRA-18
				Execution Context : EXEC RWMS_SP_LOG_MESSAGE 1, 'M', 'M', 'text'
		
		*/
BEGIN		

				
				
			    DECLARE @retention_period INT; 
				DECLARE @log_level char(1) ;
				DECLARE @proc_name VARCHAR(250) = ( SELECT PROC_NAME FROM RWMS_ARCHIVAL_DETAIL WHERE arch_id = @archid );
				DECLARE @error_msg NVARCHAR(MAX);
				
				SET @error_msg = COALESCE(ERROR_MESSAGE(), '');
			
				
				SET @retention_period = ( SELECT retention_period FROM RWMS_ARCHIVAL_DETAIL WHERE arch_id = @archid );
				
				if @messagetype = 'E'
                BEGIN
					SET @informational_message = @informational_message + CHAR(13) + @error_msg;
                END
				
				
				
                IF @loglevel = 'E' AND @messagetype = 'E'
                BEGIN
                               
                             ------Log messages-------
                                INSERT INTO RWMS_LOG_DETAIL ( ARCH_ID , MSG , EXECUTION_DATE , RUN_STATUS , PARAMETERS_PASSED, MESSAGE_TYPE  )
                                SELECT 
                                                                @archid,
                                                                @informational_message,
                                                                GETDATE(),
                                                                1,
                                                                @proc_name + ' @arch_id  = ' + CAST(@archid AS CHAR(4)) + ' ,' + ' @retention_period = ' + CAST(@retention_period AS CHAR(3) ),
                                                                @messagetype  ;  
                END

                ELSE IF @loglevel = 'M' AND @messagetype IN ('E', 'M') 
                BEGIN
                                -------Log messages------
                                INSERT INTO RWMS_LOG_DETAIL ( ARCH_ID , MSG , EXECUTION_DATE, MESSAGE_TYPE, RUN_STATUS , PARAMETERS_PASSED  )
                                SELECT
                                                @archid,
                                                @informational_message,
                                                GETDATE(),
                                                @messagetype,
                                                ( CASE WHEN @messagetype = 'E' THEN 1 ELSE 0 END) ,
                                                @proc_name + ' @arch_id  = ' + CAST(@archid AS CHAR(4)) + ' ,' + ' @retention_period = ' + CAST(@retention_period AS CHAR(3) ) ;           
                END      
                
				ELSE IF @loglevel = 'D' AND @messagetype IN ('E', 'M', 'D') 
                BEGIN
                               
                                -------Log messages------
                                INSERT INTO RWMS_LOG_DETAIL ( ARCH_ID , MSG , EXECUTION_DATE, MESSAGE_TYPE, RUN_STATUS , PARAMETERS_PASSED  )
                                SELECT
                                                @archid,
                                                @informational_message,
                                                GETDATE(),
                                                @messagetype,
                                               ( CASE WHEN @messagetype = 'E' THEN 1 ELSE 0 END) ,
                                                @proc_name + ' @arch_id  = ' + CAST(@archid AS CHAR(4)) + ' ,' + ' @retention_period = ' + CAST(@retention_period AS CHAR(3) ) ;           
                END
                
                
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('RWMS_SP_ARCHIVAL_PURGE') IS NOT NULL
DROP PROCEDURE RWMS_SP_ARCHIVAL_PURGE
GO


CREATE PROCEDURE RWMS_SP_ARCHIVAL_PURGE
AS
/*
	Author 		: Diwakar/Chethan
	Created Date 	: 06/03/2014
	Purpose 	: The stored procedure performs archival &/ purging based on user's choice
	JIRA		: 
	Execution	: EXEC RWMS_SP_ARCHIVAL_PURGE

*/
BEGIN TRY

	-- Prevents extra results sent from server to client
	SET NOCOUNT ON;


	/*---------------[ Section : Declaration ]---------------------------------------------*/

	DECLARE @archiveID INT;
	DECLARE @tablename VARCHAR(250);
	DECLARE @jobtype CHAR(1);
	DECLARE @retention_period INT;

	DECLARE @sp_name VARCHAR(128);
	DECLARE @procname VARCHAR(500);



	/*----------------[ Section : Daily frequency ]------------------------------------*/
	
	INSERT INTO RWMS_LOG_DETAIL 
		(ARCH_ID,
		 MESSAGE_TYPE,
		 MSG,
		 EXECUTION_DATE,
		 RUN_STATUS,
		 PARAMETERS_PASSED ) SELECT 9999, NULL, '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Execution started for Archival/Purging for : ' + CONVERT( VARCHAR(25), GETDATE(), 101) + ' ]------------------------------------------------------------------</td></tr></table>', GETDATE(), 0, NULL;  
		
	
	IF NOT EXISTS ( SELECT 1 FROM RWMS_ARCHIVAL_DETAIL WHERE JOB_TYPE <> 'N' )
	BEGIN
		--PRINT 'Nothing to archive/purge. Exiting....'
		INSERT INTO RWMS_LOG_DETAIL 
		(ARCH_ID,
		 MESSAGE_TYPE,
		 MSG,
		 EXECUTION_DATE,
		 RUN_STATUS,
		 PARAMETERS_PASSED ) SELECT 9999, NULL, '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>Start time : ' + CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No tables to archive/purge</td></tr></table>', GETDATE(), 0, NULL;
		RETURN;
	END


	
	DECLARE iLoop CURSOR FOR SELECT ARCH_ID, PREFIX_CODE, JOB_TYPE, RETENTION_PERIOD, PROC_NAME FROM RWMS_ARCHIVAL_DETAIL WHERE JOB_TYPE <> 'N';


	OPEN iLoop;


	FETCH NEXT FROM iLoop INTO @archiveID, @tablename, @jobtype , @retention_period, @sp_name;
	
	

	WHILE @@FETCH_STATUS = 0
	BEGIN

		--PRINT '/*--------- Calling the stored procedure : [ ' + @sp_name + '  ] ----------------*/'

		
		SET @procname = 'SET XACT_ABORT ON; '
		
		SET @procname = @procname + ' EXEC ' + @sp_name + ' ' + @jobtype + ',' + CAST(@retention_period AS CHAR(3));

		--PRINT @procname;
		
		EXECUTE (@procname);
	
	FETCH NEXT FROM iLoop INTO @archiveID, @tablename, @jobtype , @retention_period, @sp_name;


	END

	INSERT INTO RWMS_LOG_DETAIL 
		(ARCH_ID,
		 MESSAGE_TYPE,
		 MSG,
		 EXECUTION_DATE,
		 RUN_STATUS,
		 PARAMETERS_PASSED ) SELECT 9999, NULL, '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Execution completed for Archival/Purging for : ' + CONVERT( VARCHAR(25), GETDATE(), 101) + ' ]------------------------------------------------------------------</td></tr></table>', GETDATE(), 0, NULL;  
		

	CLOSE iLoop;

	DEALLOCATE iLoop;

END TRY
BEGIN CATCH
	
	SELECT 
        	ERROR_NUMBER() AS ErrorNumber
	        ,ERROR_SEVERITY() AS ErrorSeverity
        	,ERROR_STATE() AS ErrorState
        	,ERROR_PROCEDURE() AS ErrorProcedure
        	,ERROR_LINE() AS ErrorLine
        	,ERROR_MESSAGE() AS ErrorMessage;


END CATCH

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('RWMS_SP_CREATE_LOG') IS NOT NULL
DROP PROCEDURE RWMS_SP_CREATE_LOG
GO

-- EXEC RWMS_SP_CREATE_LOG 'E', '\RMWS\Archival_Purging'

CREATE PROCEDURE RWMS_SP_CREATE_LOG ( @drive CHAR(1) = NULL, @directory VARCHAR(500) = 'c:\rwms\LogFiles' )
AS
BEGIN

			/*			
			DECLARE @query VARCHAR(5000);
			DECLARE @drive CHAR(1);
			
			-- Select drive to create a directory
			IF OBJECT_ID('tempdb..#drive') IS NOT NULL
			DROP TABLE #drive;
			
			CREATE TABLE #drive ( id INT NOT NULL IDENTITY(1,1), drive_letter CHAR(1), free_space_in_MB BIGINT );
			
			INSERT INTO #drive
			EXEC master..xp_fixeddrives;
			
			SET @drive = ( SELECT drive_letter FROM #drive WHERE id = ( SELECT MAX(id) FROM #drive ) AND free_space_in_MB > 10000 );
			
			DECLARE @log_dir VARCHAR(1000) = @drive + ':\RMWS\Archival_Purging';
			
    		-- Check if directory to store log files exist
		
			SET @query = 'IF exist ' + @log_dir + '/nul ( echo Directory already exists ) ELSE ( mkdir ' + @log_dir + ' && echo ' + @log_dir + ' created)';
			
			EXEC master..xp_cmdshell @query, NO_OUTPUT;	*/
			
			DECLARE @log_dir VARCHAR(1000) ;
			
			IF @drive IS NULL
			BEGIN
					SET @log_dir = 'c:\rwms\LogFiles';
			END
			ELSE
			BEGIN
					SET @log_dir = @drive + ':' + @directory;
			
			END
			

		-- Creates log file 
		DECLARE @sql varchar(8000);

		DECLARE @loc VARCHAR(5000);

		DECLARE @filelocation VARCHAR(500);
		
		DECLARE @source_db VARCHAR(250) = DB_NAME();
		
		DECLARE @server_name VARCHAR(250) = @@servername;

		SET @loc = 'cd C:\Program Files\Microsoft SQL Server\100\Tools\Binn\ & '

		SET @filelocation =@log_dir + '\Archival_Purging_Log_' + REPLACE( CONVERT(VARCHAR(15), GETDATE(), 102), '.', '_') + '.html'
		
		-- Quit if nothing to write
		IF NOT EXISTS ( SELECT 1 FROM RWMS_LOG_DETAIL (NOLOCK) WHERE CONVERT( VARCHAR(10), EXECUTION_DATE, 101 ) = CONVERT( VARCHAR(10), GETDATE(), 101 ) )
		BEGIN
				RETURN;
		END 

		SET @sql = @loc + ' bcp.exe "SELECT MSG FROM '+@source_db+'..RWMS_LOG_DETAIL (NOLOCK) WHERE CONVERT( VARCHAR(10), EXECUTION_DATE, 101 ) = CONVERT( VARCHAR(10), GETDATE(), 101 )" queryout "'+@filelocation+'"  -T -c -S "'+@server_name+'"';

		EXEC master..xp_cmdshell @sql;
		
		-- Remove log files older than a week
		
		DECLARE @row_num INT, @file_name VARCHAR(500), @filedate VARCHAR(15), @diff_in_days INT;  
  
		 CREATE TABLE #tempDIR ( location VARCHAR(500) );  
		
		 SET @sql = 'dir /b ' + @log_dir + '\Archival_Purging_Log_*.html';
		  
		 INSERT INTO #tempDIR  
		 EXEC master..xp_cmdshell @sql;  
		  
		 -- Quit if no files exist 
		 IF NOT EXISTS (SELECT 1 FROM #tempDIR)  
		 RETURN;  
		  
		  -- Removes the null record
		 DELETE FROM #tempDIR WHERE location IS NULL;  
		
		  
		 DECLARE iLoop CURSOR FOR SELECT ROW_NUMBER() OVER(ORDER BY location) as rownum, location FROM #tempDIR;  
		  
		 OPEN iLoop;  
		  
		 FETCH NEXT FROM iLoop INTO @row_num , @file_name;  
		  
		 WHILE @@FETCH_STATUS = 0  
		 BEGIN  
		  
		  PRINT '-----------------------[' + @file_name + ']------------------------------'  
		   
		  SET @filedate  = ( SELECT REPLACE(LEFT(RIGHT(@file_name, 15),10), '_', '-') );  
		  
		  SET @diff_in_days = ( SELECT DATEDIFF(dd, @filedate, CONVERT(VARCHAR(15), GETDATE(), 102)  ));  
		  
		  IF @diff_in_days > 7
		  BEGIN  
		   ------------------Delete file older than 7 days--------------------  
		   SET @sql = 'del ' + @log_dir + '\' + @file_name + '"';  
		  --PRINT @sql; 
		   EXEC master..xp_cmdshell @sql;  
		  END  
		   
		 FETCH NEXT FROM iLoop INTO @row_num , @file_name;  
		  
		 END  
		  
		 CLOSE iLoop;  
		  
		 DEALLOCATE iLoop;  
		  
		 DROP TABLE #tempDIR;  
		
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('RWMS_SP_PRUNE_LOG') IS NOT NULL
DROP PROCEDURE RWMS_SP_PRUNE_LOG
GO

CREATE PROCEDURE RWMS_SP_PRUNE_LOG
AS
BEGIN
		DECLARE @database_name VARCHAR(250) = DB_NAME();
		DECLARE @sql VARCHAR(2000);
		
		SET @sql = 'DELETE FROM ' + @database_name + '.dbo.RWMS_LOG_DETAIL WHERE DATEDIFF( dd, CONVERT(VARCHAR(10), EXECUTION_DATE, 102), GETDATE() ) > 31'; 
		
		EXEC (@sql);
		
		

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET XACT_ABORT ON
GO  

IF OBJECT_ID('RWMS_SP_AUDIT') IS NOT NULL
DROP PROCEDURE RWMS_SP_AUDIT
GO

CREATE PROCEDURE dbo.RWMS_SP_AUDIT( @jobtype CHAR(1), @retention_period CHAR(3) )    
AS    
/*    
  ---AUDIT    
  Author   : Diwakar
  Created Date  : 07/14/2014    
  Purpose  : The stored procedure performs archival &/ purging for the AUDIT table 
  JIRA  :   RWMS-18  
  Execution : EXEC RWMS_SP_AUDIT 'B', '325'			 SELECT * FROM RWMS_LOG_DETAIL
    
*/    
    
BEGIN TRY    
    
 
  SET NOCOUNT ON;
  
    
BEGIN DISTRIBUTED TRANSACTION;      
   
/*------------------------------[ DECLARATION ]--------------------------------------------*/    
    
declare @retentiondays int;    
set @retentiondays = CAST(@retention_period AS INT);    
    
declare @CountAudit int;    
    
DECLARE @linkedserver VARCHAR(250);    
DECLARE @source_db VARCHAR(250);    
DECLARE @destination_db VARCHAR(250);    
    
DECLARE @query NVARCHAR(MAX);    

DECLARE @count_Audit INT;    
    
DECLARE @information NVARCHAR(MAX);    
    
DECLARE @rowCount_archived INT;    
DECLARE @rowCount_purged INT;    
    
DECLARE @transaction_flag BIT -- 0 for COMMIT and 1 for ROLLBACK    

DECLARE @sql NVARCHAR(4000);
 DECLARE @sql_count INT; 
 DECLARE @sql_count_purge INT;  
    
SET @linkedserver = ( SELECT Linkedservername FROM RWMS_ARCHIVAL_MASTER );    
SET @source_db = ( SELECT source_db_name FROM RWMS_ARCHIVAL_MASTER );    
SET @destination_db = ( SELECT dest_db_name FROM RWMS_ARCHIVAL_MASTER );    
    
DECLARE @arch_id INT  = ( SELECT arch_id FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'AUDIT' );    
    

DECLARE @log_level char(1) = ( SELECT loglevel FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'AUDIT' ); 



/*------------------------------[ Determine the level of logging ]---------------------------*/    
    
    
    
  SET @information = '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Entering RWMS_AUDIT ]------------------------------------------------------------------</td>';    
  SET @information = @information + CHAR(10) + '<tr><td> Archival/ Purging log information for dbo.AUDIT</td></tr> ';    
  SET @information = @information + CHAR(10) + '<tr><td> Start time : ' + CONVERT( VARCHAR(25), GETDATE(), 121) + ' </td></tr>';    

   
/*------------------------------[ AUDIT ]-----------------------------------------*/    
    
		SET @query = 'SELECT 
									@count_Audit = count(1) 
						  FROM
									 '+ @source_db+'.dbo.AUDIT 
						 WHERE
									 datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + '  and 
									 auditid NOT IN ( select auditid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.audit )';
									
		EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @count_Audit  INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @count_Audit = @CountAudit OUTPUT;    
		
		
		
		IF @CountAudit = 0 AND @jobtype <> 'P'
		BEGIN
			     SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
		END
		
	
		
		 IF @CountAudit > 0 AND @jobtype  IN ('A', 'B')    
		 BEGIN
				SET @sql='INSERT into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.AUDIT
									  SELECT
												* 
									  FROM
												'+ @source_db+'.dbo.AUDIT 
									  WHERE 
												datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and 
												AUDITID not in ( select AUDITID from  ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.AUDIT); SELECT @count = @@ROWCOUNT';
												
						EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

				
				  -------------------Logging informational messages----------------------    
        
				  SET @rowCount_archived = @sql_count;    
    
					--PRINT 'Row count for archived : ' + CAST(@rowCount_archived AS CHAR(4));
            
					SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for AUDIT : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
         
      END

	 

	  IF @CountAudit > 0 AND @jobtype = 'B'  
	   BEGIN
      			SET @sql = 'DELETE FROM '+ @source_db+'.dbo.AUDIT
      									WHERE
      											datediff(d,editdate,getdate()) >=  '+CAST(@retentiondays AS CHAR(3)) + ' and 
      											AUDITID in ( select AUDITID from  ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.AUDIT); SELECT @count = @@ROWCOUNT';
      											
      			 EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
         
				 -------------------Logging informational messages----------------------    
        
				 SET @rowCount_purged = @sql_count;    
            
			     SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for AUDIT : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     
											
		END
		
		 -- Since @CountAudit will be 0 
		 IF  @jobtype = 'P'
		 BEGIN
      			SET @sql = 'DELETE FROM '+ @source_db+'.dbo.AUDIT
      									WHERE
      											datediff(d,editdate,getdate()) >=  '+CAST(@retentiondays AS CHAR(3)) + ' and 
      											AUDITID in ( select AUDITID from  ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.AUDIT); SELECT @count = @@ROWCOUNT';
      											
      			 EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
         
				 -------------------Logging informational messages----------------------    
        
				 SET @rowCount_purged = @sql_count;    
            
			     SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for AUDIT : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     
											
		END
      		
      		
      	
		IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
		BEGIN    
				
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
					 -- Set the transaction flag to roll back    
					SET @transaction_flag = 1;    
					
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
     END    
    
 
        SET @information = @information + CHAR(10) + '<td bgcolor="LightBlue">--------------------------------------------[ Exiting RWMS_AUDIT ]------------------------------------------------------------------</td></tr></table>' 


	

	    
   --COMMIT or ROLLBACK TRANSACTION based on transaction_flag;    
   IF @transaction_flag = 1    
   BEGIN    
    ROLLBACK TRANSACTION;    
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;    
   END    
   ELSE    
   BEGIN    
    COMMIT TRANSACTION;       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'M', @informational_message = @information;    
   END    
  
   
    
END TRY    
BEGIN CATCH    
  
IF @@TRANCOUNT > 0    
    ROLLBACK TRANSACTION;    
    
  
   /*-----------------Call the Log SP for any error------------------------*/    
       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;   
     
END CATCH  						

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET XACT_ABORT ON
GO    

IF OBJECT_ID('RWMS_SP_COUNTING') IS NOT NULL
DROP PROCEDURE RWMS_SP_COUNTING
GO

CREATE PROCEDURE [dbo].[RWMS_SP_COUNTING] ( @jobtype CHAR(1), @retention_period CHAR(3) )    
AS    
/*    
	 ---COUNTING ---
	Author   : Diwakar.M
  	Created Date  : 07/22/2014    
  	Purpose  : The stored procedure performs archival &/ purging for COUNTING 
  	JIRA  :  RWMS-18   
  	Execution : EXEC RWMS_SP_COUNTING 'B', '31'    SELECT * FROM RWMS_LOG_DETAIL  
    
*/    
BEGIN TRY    
    
  --Prevents engine from returning result sets to client   
  SET NOCOUNT ON;
  
    
  BEGIN DISTRIBUTED TRANSACTION;    
    
/*------------------------------[ DECLARATION ]--------------------------------------------*/    
    
declare @retentiondays int;    
set @retentiondays = CAST(@retention_period AS INT);    
    
DECLARE @linkedserver VARCHAR(250);    
DECLARE @source_db VARCHAR(250);    
DECLARE @destination_db VARCHAR(250);    
    
DECLARE @query NVARCHAR(MAX);    
    
declare @Count_counting int;
    
DECLARE @information NVARCHAR(MAX);    
    
DECLARE @rowCount_archived INT;    
DECLARE @rowCount_purged INT;    
    
DECLARE @transaction_flag BIT -- 0 for COMMIT and 1 for ROLLBACK    

     DECLARE @sql NVARCHAR(4000);
     DECLARE @sql_count INT; 
    
SET @linkedserver = ( SELECT Linkedservername FROM RWMS_ARCHIVAL_MASTER );    
SET @source_db = ( SELECT source_db_name FROM RWMS_ARCHIVAL_MASTER );    
SET @destination_db = ( SELECT dest_db_name FROM RWMS_ARCHIVAL_MASTER );    
    
DECLARE @arch_id INT  = ( SELECT arch_id FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'COUNTING');        

DECLARE @log_level char(1) = ( SELECT loglevel FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'COUNTING'); 

SET @information = '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Entering RWMS_COUNTING ]------------------------------------------------------------------</td>';    
SET @information = @information + CHAR(10) + '<tr><td> Archival/ Purging log information for COUNTING</td></tr> ';    
SET @information = @information + CHAR(10) + '<tr><td> Start time : ' + CONVERT( VARCHAR(25), GETDATE(), 121) + ' </td></tr>';    



/*------------------------------[ COUNTING ]-----------------------------------------*/    
    
SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for COUNTING </td></tr>';    
     
SET  @query  = 'select 
		@Countcounting = count(1) 
	          from 
		'+ @source_db+'.dbo.counting 
	          where  
		datediff(d,editdate,getdate()) >=  '+CAST(@retentiondays AS CHAR(3)) + ' and   
		status IN (''COMPLETE'', ''CANCELED'') and 
		countid NOT in (select countid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.counting)';


EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countcounting INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countcounting = @Count_counting OUTPUT;    

IF @Count_counting = 0 AND @jobtype <> 'P'
BEGIN
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

IF @Count_counting > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.counting
		   select 
			* 
		   from 
			'+ @source_db+'.dbo.counting 
		   where  
			datediff(d,editdate,getdate()) >=  '+CAST(@retentiondays AS CHAR(3)) + '  and   
			status IN (''COMPLETE'', ''CANCELED'') AND
			countid NOT IN (select countid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.counting); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for COUNTING : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
     
END

IF @Count_counting > 0 AND @jobtype ='B'    
BEGIN

	SET @sql = ' delete FROM '+ @source_db+'.dbo.counting 
		     where  
			datediff(d,editdate,getdate()) >=  '+CAST(@retentiondays AS CHAR(3)) + ' and   
			status IN (''COMPLETE'', ''CANCELED'') AND
			countid in (select countid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.counting); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for COUNTING : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END



IF @jobtype ='P'    
BEGIN

	SET @sql = ' delete FROM '+ @source_db+'.dbo.counting 
		     where  
			datediff(d,editdate,getdate()) >=  '+CAST(@retentiondays AS CHAR(3)) + ' and   
			status IN (''COMPLETE'', ''CANCELED'') AND
			countid in (select countid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.counting); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for COUNTING : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    




 SET @information = @information + CHAR(10) + '<td bgcolor="LightBlue">--------------------------------------------[ Exiting RWMS_COUNTING ]------------------------------------------------------------------</td></tr></table>' 
 

	    
   --COMMIT or ROLLBACK TRANSACTION based on transaction_flag;    
   IF @transaction_flag = 1    
   BEGIN    
    ROLLBACK TRANSACTION;    
    	  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;    

   END    
   ELSE    
   BEGIN    
    COMMIT TRANSACTION;       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'M', @informational_message = @information;    

   END    
    
END TRY    
BEGIN CATCH    
  
IF @@TRANCOUNT > 0    
    ROLLBACK TRANSACTION;    
    
  
   /*-----------------Call the Log SP for any error------------------------*/    
       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;   
     
END CATCH  	


GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET XACT_ABORT ON
GO    

IF OBJECT_ID('RWMS_SP_HANDLINGUNITTRANSACTIONS') IS NOT NULL
DROP PROCEDURE RWMS_SP_HANDLINGUNITTRANSACTIONS
GO

CREATE PROCEDURE [dbo].[RWMS_SP_HANDLINGUNITTRANSACTIONS] ( @jobtype CHAR(1), @retention_period CHAR(3) )    
AS    
/*    
	 ---HANDLINGUNITTRANSACTIONS ( SHIPMENT, INBOUNDORDER, RECEIPT )----
	Author   : Diwakar.M
  	Created Date  : 07/22/2014    
  	Purpose  : The stored procedure performs archival &/ purging for HANDLINGUNITTRANSACTIONS ( SHIPMENT, INBOUNDORDER, RECEIPT )
  	JIRA  :  RWMS-18   
  	Execution : SET XACT_ABORT ON 
  					  GO 
  					  EXEC RWMS_SP_HANDLINGUNITTRANSACTIONS 'B', '31'    
  						SELECT * FROM RWMS_LOG_DETAIL  
    
*/    
BEGIN TRY    
    
  --Prevents engine from returning result sets to client   
  SET NOCOUNT ON;
  
    
  BEGIN DISTRIBUTED TRANSACTION;    
    
/*------------------------------[ DECLARATION ]--------------------------------------------*/    
    
declare @retentiondays int;    
set @retentiondays = CAST(@retention_period AS INT);    
    
DECLARE @linkedserver VARCHAR(250);    
DECLARE @source_db VARCHAR(250);    
DECLARE @destination_db VARCHAR(250);    
    
DECLARE @query NVARCHAR(MAX);    
    
declare @Count_hutransshipment int;
DECLARE @Count_hutransinord INT;
DECLARE @Count_hutransreceipt INT;

    
DECLARE @information NVARCHAR(MAX);    
    
DECLARE @rowCount_archived INT;    
DECLARE @rowCount_purged INT;    
    
DECLARE @transaction_flag BIT -- 0 for COMMIT and 1 for ROLLBACK    

     DECLARE @sql NVARCHAR(4000);
     DECLARE @sql_count INT; 
    
SET @linkedserver = ( SELECT Linkedservername FROM RWMS_ARCHIVAL_MASTER );    
SET @source_db = ( SELECT source_db_name FROM RWMS_ARCHIVAL_MASTER );    
SET @destination_db = ( SELECT dest_db_name FROM RWMS_ARCHIVAL_MASTER );    
    
DECLARE @arch_id INT  = ( SELECT arch_id FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'HANDLINGUNITTRANSACTIONS');        

DECLARE @log_level char(1) = ( SELECT loglevel FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'HANDLINGUNITTRANSACTIONS'); 

SET @information = '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Entering RWMS_HANDLINGUNITTRANSACTIONS ]------------------------------------------------------------------</td>';    
SET @information = @information + CHAR(10) + '<tr><td> Archival/ Purging log information for HANDLINGUNITTRANSACTIONS ( SHIPMENT, INBOUNDORDER, RECEIPT )</td></tr> ';    
SET @information = @information + CHAR(10) + '<tr><td> Start time : ' + CONVERT( VARCHAR(25), GETDATE(), 121) + ' </td></tr>';    



/*------------------------------[ HANDLINGUNITTRANSACTIONS (SHIPMENT) ]-----------------------------------------*/    
    
--Shipmnet handlingunittransactions--

SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for HANDLINGUNITTRANSACTIONS (SHIPMENT)</td></tr>';    
     
SET  @query  = 'select 
		@Counthutransshipment =count(1)
	          from 
		'+ @source_db+'.dbo.handlingunittransaction 
	         where 
		transactiontype=''SHIPMENT'' and 
		transactiontypeid not in (select shipment from '+ @source_db+'.dbo.shipment ) AND
		transactionid NOT IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction)';


EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @Counthutransshipment INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver, @Counthutransshipment = @Count_hutransshipment OUTPUT;    

IF @Count_hutransshipment = 0 AND @jobtype <> 'P'
BEGIN
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

IF @Count_hutransshipment > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction
		   select 
			* 
		   from 
			'+ @source_db+'.dbo.handlingunittransaction 
		  where 
			transactiontype=''SHIPMENT'' and 
			transactiontypeid not in (select shipment from '+ @source_db+'.dbo.shipment ) AND
			transactionid NOT IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for HANDLINGUNITTRANSACTIONS (SHIPMENT) : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
     
END

IF @Count_hutransshipment > 0 AND @jobtype = 'B'    
BEGIN

	SET @sql = 'delete from '+ @source_db+'.dbo.handlingunittransaction 
		    where 
			transactiontype=''SHIPMENT'' and 
			transactiontypeid not in (select shipment from '+ @source_db+'.dbo.shipment ) AND
			transactionid IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for HANDLINGUNITTRANSACTIONS (SHIPMENT) : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END



IF @jobtype = 'P'    
BEGIN

	SET @sql = 'delete from '+ @source_db+'.dbo.handlingunittransaction 
		    where 
			transactiontype=''SHIPMENT'' and 
			transactiontypeid not in (select shipment from '+ @source_db+'.dbo.shipment ) AND
			transactionid IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for HANDLINGUNITTRANSACTIONS (SHIPMENT) : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

      	
          
 END 



/*------------------------------[ HANDLINGUNITTRANSACTIONS (INBOUNDORDER) ]-----------------------------------------*/    


--InboundOrder handlingunittransactions--

SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for HANDLINGUNITTRANSACTIONS (INBOUNDORDER)</td></tr>';    
     
SET  @query  = 'select 
		@Counthutransinord = count(1) 
	          from 
		'+ @source_db+'.dbo.handlingunittransaction 
	          where 
		transactiontype=''INORD'' and 
		ORDERID not in (select ORDERID from '+ @source_db+'.dbo.INBOUNDORDHEADER) and 
		transactionid NOT IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction)';


EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @Counthutransinord INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver, @Counthutransinord = @Count_hutransinord OUTPUT;    

IF @Count_hutransinord = 0 AND @jobtype <> 'P'
BEGIN
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

IF @Count_hutransinord  > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction
		   select 
			* 
		   from 
			'+ @source_db+'.dbo.handlingunittransaction 
		   where 
			transactiontype=''INORD'' and 
			ORDERID not in (select ORDERID from '+ @source_db+'.dbo.INBOUNDORDHEADER) and 
			transactionid NOT IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction); SELECT @count = @@ROWCOUNT';



	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for HANDLINGUNITTRANSACTIONS (INBOUNDORDER) : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
     
END

IF @Count_hutransinord  > 0 AND @jobtype = 'B'    
BEGIN

	SET @sql = 'delete from '+ @source_db+'.dbo.handlingunittransaction 
		    where 
			transactiontype=''INORD'' and 
			ORDERID not in (select ORDERID from '+ @source_db+'.dbo.INBOUNDORDHEADER) AND
			transactionid IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for HANDLINGUNITTRANSACTIONS (INBOUNDORDER) : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END



IF @jobtype = 'P'    
BEGIN

	SET @sql = 'delete from '+ @source_db+'.dbo.handlingunittransaction 
		    where 
			transactiontype=''INORD'' and 
			ORDERID not in (select ORDERID from '+ @source_db+'.dbo.INBOUNDORDHEADER) AND
			transactionid IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for HANDLINGUNITTRANSACTIONS (INBOUNDORDER) : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END 



/*------------------------------[ HANDLINGUNITTRANSACTIONS (RECEIPT) ]-----------------------------------------*/    

--Receipt handlingunittransactions--

SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for HANDLINGUNITTRANSACTIONS (RECEIPT)</td></tr>';    
     
SET  @query  = 'select 
		@Counthutransreceipt = count(1) 
	           from 
		'+ @source_db+'.dbo.handlingunittransaction 
	           where 
		transactiontype=''RECEIPT'' and 
		transactiontypeid not in (select receipt from '+ @source_db+'.dbo.receiptheader) and 
		transactionid NOT IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction)';


EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @Counthutransreceipt INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver, @Counthutransreceipt = @Count_hutransreceipt OUTPUT;    

IF @Count_hutransreceipt = 0 AND @jobtype <> 'P'
BEGIN
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

IF @Count_hutransreceipt > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction
		    select 
			*
		   from 
			'+ @source_db+'.dbo.handlingunittransaction 
		   where 
			transactiontype=''RECEIPT'' and 
			transactiontypeid not in (select receipt from '+ @source_db+'.dbo.receiptheader) AND
			transactionid NOT IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction); SELECT @count = @@ROWCOUNT';

	
	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for HANDLINGUNITTRANSACTIONS (RECEIPT) : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
     
END

IF @Count_hutransreceipt > 0 AND @jobtype = 'B'   
BEGIN

	SET @sql = 'delete from '+ @source_db+'.dbo.handlingunittransaction 
		    where 
			transactiontype=''RECEIPT'' and 
			transactiontypeid not in (select receipt from '+ @source_db+'.dbo.receiptheader) AND
			transactionid IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for HANDLINGUNITTRANSACTIONS (RECEIPT) : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


IF @jobtype = 'P'   
BEGIN

	SET @sql = 'delete from '+ @source_db+'.dbo.handlingunittransaction 
		    where 
			transactiontype=''RECEIPT'' and 
			transactiontypeid not in (select receipt from '+ @source_db+'.dbo.receiptheader) AND
			transactionid IN (select transactionid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.handlingunittransaction); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for HANDLINGUNITTRANSACTIONS (RECEIPT) : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END 



 SET @information = @information + CHAR(10) + '<td bgcolor="LightBlue">--------------------------------------------[ Exiting RWMS_HANDLINGUNITTRANSACTIONS ]------------------------------------------------------------------</td></tr></table>' 
 

	    
   --COMMIT or ROLLBACK TRANSACTION based on transaction_flag;    
   IF @transaction_flag = 1    
   BEGIN    
    ROLLBACK TRANSACTION;    
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;    
   END    
   ELSE    
   BEGIN    
    COMMIT TRANSACTION;       

    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'M', @informational_message = @information;    

   END    
    
END TRY    
BEGIN CATCH    
  
IF @@TRANCOUNT > 0    
    ROLLBACK TRANSACTION;    
    
  
   /*-----------------Call the Log SP for any error------------------------*/    
       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;   
     
END CATCH  	

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET XACT_ABORT ON
GO    

/*
SET XACT_ABORT ON
GO    
EXEC RWMS_SP_OUTBOUND 'B', '200'  

*/

IF OBJECT_ID('RWMS_SP_OUTBOUND') IS NOT NULL
DROP PROCEDURE RWMS_SP_OUTBOUND
GO

CREATE PROCEDURE [dbo].[RWMS_SP_OUTBOUND] ( @jobtype CHAR(1), @retention_period CHAR(3) )    
AS    
/*    
  --outboundorheader, outboundordetail, orderloads, attribute ---
  Author   : Diwakar.M
  Created Date  : 07/21/2014    
  Purpose  : The stored procedure performs archival &/ purging for OUTBOUNDORDETAIL, OUTBOUNDORHEADER , ORDERLOADS and ATTRIBUTE
  JIRA  :  RWMS-18   
  Execution : EXEC RWMS_SP_OUTBOUND 'B', '90'    SELECT * FROM RWMS_LOG_DETAIL  
    
*/    
BEGIN TRY    
    
  -- Rolls back all transaction if the code encounters any runtime error.  
    
  SET NOCOUNT ON;
  
    
  BEGIN DISTRIBUTED TRANSACTION;    
    
/*------------------------------[ DECLARATION ]--------------------------------------------*/    
    
declare @retentiondays int;    
set @retentiondays = CAST(@retention_period AS INT);    
    
declare @Countorderloads int;    
    
DECLARE @linkedserver VARCHAR(250);    
DECLARE @source_db VARCHAR(250);    
DECLARE @destination_db VARCHAR(250);    
    
DECLARE @query NVARCHAR(MAX);    
    
declare @Count_orderloads int;
declare @Count_outboundordetail int;
declare @Count_outboundorheader int;
declare @Count_attributeoutbound int;
    
DECLARE @information NVARCHAR(MAX);    
    
DECLARE @rowCount_archived INT;    
DECLARE @rowCount_purged INT;    
    
DECLARE @transaction_flag BIT -- 0 for COMMIT and 1 for ROLLBACK    

     DECLARE @sql NVARCHAR(4000);
     DECLARE @sql_count INT; 
    
SET @linkedserver = ( SELECT Linkedservername FROM RWMS_ARCHIVAL_MASTER );    
SET @source_db = ( SELECT source_db_name FROM RWMS_ARCHIVAL_MASTER );    
SET @destination_db = ( SELECT dest_db_name FROM RWMS_ARCHIVAL_MASTER );    
    
DECLARE @arch_id INT  = ( SELECT arch_id FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'OUTBOUND');        

DECLARE @log_level char(1) = ( SELECT loglevel FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'OUTBOUND'); 

SET @information = '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Entering RWMS_OUTBOUND ]------------------------------------------------------------------</td>';    
SET @information = @information + CHAR(10) + '<tr><td> Archival/ Purging log information for ORDERLOADS, OUTBOUNDORDETAIL, OUTBOUNDORHEADER AND ATTRIBUTE</td></tr> ';    
SET @information = @information + CHAR(10) + '<tr><td> Start time : ' + CONVERT( VARCHAR(25), GETDATE(), 121) + ' </td></tr>';    

/*------------------------------[ ORDERLOADS ]-----------------------------------------*/    
    
SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for ORDERLOADS </td></tr>';    
     
SET  @query  = 'SELECT
		@Countorderloads = count(1) 
	         FROM
		'+ @source_db+'.dbo.orderloads 
	         WHERE
		documenttype=''OUTBOUND'' and 
		CONSIGNEE+''-''+ORDERID in ( select CONSIGNEE+''-''+ORDERID FROM '+ @source_db+'.dbo.outboundorheader WHERE datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status IN (''SHIPPED'', ''CANCELED'')) and 
		loadid NOT IN (select loadid from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.orderloads)';

EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countorderloads INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countorderloads = @Count_orderloads OUTPUT;    

IF @Count_orderloads = 0 AND @jobtype <> 'P'
BEGIN
	     SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

IF @Count_orderloads > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	 SET @sql = 'INSERT into '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.orderloads
 	 	    SELECT
		 	* 
	 	    FROM
			'+ @source_db+'.dbo.orderloads 
	 	    WHERE
			documenttype=''OUTBOUND'' and CONSIGNEE+''-''+ORDERID in ( select CONSIGNEE+''-''+ORDERID from  '+ @source_db+'.dbo.outboundorheader where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status IN (''SHIPPED'', ''CANCELED'')) and 
			loadid NOT IN (select loadid from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.orderloads); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for ORDERLOADS : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
         
   

END

IF @Count_orderloads > 0 AND @jobtype= 'B'     
BEGIN
	
	SET @sql = 'DELETE FROM '+ @source_db+'.dbo.orderloads 
		    WHERE
			documenttype=''OUTBOUND'' and CONSIGNEE+''-''+ORDERID in ( select CONSIGNEE+''-''+ORDERID from  '+ @source_db+'.dbo.outboundorheader where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status IN (''SHIPPED'', ''CANCELED'')) and 
			loadid in (select loadid from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.orderloads); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	 SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for ORDERLOADS : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END



IF @jobtype= 'P'     
BEGIN
	
	SET @sql = 'DELETE FROM '+ @source_db+'.dbo.orderloads 
		    WHERE
			documenttype=''OUTBOUND'' and CONSIGNEE+''-''+ORDERID in ( select CONSIGNEE+''-''+ORDERID from  '+ @source_db+'.dbo.outboundorheader where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status IN (''SHIPPED'', ''CANCELED'')) and 
			loadid in (select loadid from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.orderloads); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	 SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for ORDERLOADS : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    
       

/*------------------------------[ OUTBOUNDORDETAIL ]-----------------------------------------*/    
    
SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for OUTBOUNDORDETAIL</td></tr>';     
    
SET @query =  'SELECT 
		@Countoutboundordetail = count(1) 
	         FROM
		'+ @source_db+'.dbo.outboundordetail 
	         WHERE	
		CONSIGNEE+''-''+ORDERID in ( select CONSIGNEE+''-''+ORDERID FROM '+ @source_db+'.dbo.outboundorheader where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status IN (''SHIPPED'', ''CANCELED'')) and 
		CONSIGNEE+''-''+ORDERID NOT IN (select CONSIGNEE+''-''+ORDERID from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.outboundordetail)';

EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countoutboundordetail INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countoutboundordetail = @Count_outboundordetail OUTPUT;    

IF @Count_outboundordetail = 0 AND @jobtype <> 'P'
BEGIN
	     SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

IF @Count_outboundordetail > 0 AND @jobtype  IN ('A', 'B') 
BEGIN
	
	 SET @sql = 'INSERT INTO '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.outboundordetail
 		     SELECT 
			*
		     FROM
			'+ @source_db+'.dbo.outboundordetail 
		     WHERE
			CONSIGNEE+''-''+ORDERID IN ( select CONSIGNEE+''-''+ORDERID from  '+ @source_db+'.dbo.outboundorheader where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status IN (''SHIPPED'', ''CANCELED'')) AND
			CONSIGNEE+''-''+ORDERID NOT IN (select CONSIGNEE+''-''+ORDERID from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.outboundordetail); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for OUTBOUNDORDETAIL : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    


END

IF @Count_outboundordetail > 0 AND @jobtype = 'B' 
BEGIN
	
	 SET @sql = 'DELETE FROM '+ @source_db+'.dbo.outboundordetail 
		    WHERE
			CONSIGNEE+''-''+ORDERID IN ( select CONSIGNEE+''-''+ORDERID from  '+ @source_db+'.dbo.outboundorheader where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status IN (''SHIPPED'', ''CANCELED'')) AND
			CONSIGNEE+''-''+ORDERID IN (select CONSIGNEE+''-''+ORDERID from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.outboundordetail); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	 SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for OUTBOUNDORDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


IF @jobtype = 'P' 
BEGIN
	
	 SET @sql = 'DELETE FROM '+ @source_db+'.dbo.outboundordetail 
		    WHERE
			CONSIGNEE+''-''+ORDERID IN ( select CONSIGNEE+''-''+ORDERID from  '+ @source_db+'.dbo.outboundorheader where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status IN (''SHIPPED'', ''CANCELED'')) AND
			CONSIGNEE+''-''+ORDERID IN (select CONSIGNEE+''-''+ORDERID from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.outboundordetail); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	 SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for OUTBOUNDORDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    



/*------------------------------[ OUTBOUNDORHEADER ]-----------------------------------------*/    
    
SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for OUTBOUNDORHEADER</td></tr>';     
    
SET @query =  'SELECT 
		@Countoutboundorheader = count(1)  
	          FROM	
		'+ @source_db+'.dbo.outboundorheader
 	         WHERE
		datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and   
		status IN (''SHIPPED'', ''CANCELED'') and 
		CONSIGNEE+''-''+ORDERID NOT IN (select CONSIGNEE+''-''+ORDERID from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.outboundorheader)';

EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countoutboundorheader INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countoutboundorheader= @Count_outboundorheader OUTPUT;    

IF @Count_outboundorheader= 0 AND @jobtype <> 'P'
BEGIN
	     SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END


IF @Count_outboundorheader > 0 AND @jobtype  IN ('A', 'B') 
BEGIN
	SET @sql = 'INSERT INTO '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.outboundorheader
 		    SELECT 
			*
		    FROM
			'+ @source_db+'.dbo.outboundorheader
 		    WHERE 
			datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and   
			status IN (''SHIPPED'', ''CANCELED'') AND
			CONSIGNEE+''-''+ORDERID NOT IN (select CONSIGNEE+''-''+ORDERID from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.outboundorheader); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for OUTBOUNDORHEADER : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    

END


IF @Count_outboundorheader > 0 AND @jobtype = 'B' 
BEGIN
	SET @sql = 'DELETE FROM '+ @source_db+'.dbo.outboundorheader 
		    WHERE 
			datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and   
			status IN (''SHIPPED'', ''CANCELED'') AND
			CONSIGNEE+''-''+ORDERID IN (select CONSIGNEE+''-''+ORDERID from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.outboundorheader); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for OUTBOUNDORHEADER : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


IF @jobtype = 'P' 
BEGIN
	SET @sql = 'DELETE FROM '+ @source_db+'.dbo.outboundorheader 
		    WHERE 
			datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and   
			status IN (''SHIPPED'', ''CANCELED'') AND
			CONSIGNEE+''-''+ORDERID IN (select CONSIGNEE+''-''+ORDERID from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.outboundorheader); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for OUTBOUNDORHEADER : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


 
 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    





/*------------------------------[ ATTRIBUTE : OUTBOUND ]-----------------------------------------*/    
    
SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for ATTRIBUTE : OUTBOUND</td></tr>';     
    
SET @query =  'SELECT 
		@Countattributeoutbound  = count(1) 
	         FROM
		'+ @source_db+'.dbo.attribute 
	         WHERE
		pkeytype=''OUTBOUND'' and 
		pkey1 + ''-'' + pkey2 not in ( select consignee+''-''+orderid  from  '+ @source_db+'.dbo.outboundorheader) and 
		pkey1 + ''-'' + pkey2 + ''-'' + pkey3 not in ( select pkey1 + ''-'' + pkey2 + ''-'' + pkey3  from  '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''OUTBOUND'' )';


EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countattributeoutbound  INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countattributeoutbound  = @Count_attributeoutbound OUTPUT;    

IF @Count_attributeoutbound = 0 AND @jobtype <> 'P'
BEGIN
	     SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

IF @Count_attributeoutbound > 0 AND @jobtype  IN ('A', 'B') 
BEGIN
	SET @sql = 'INSERT INTO '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute
		    SELECT
			*
		    FROM
			'+ @source_db+'.dbo.attribute 
		    WHERE
			pkeytype=''OUTBOUND'' and 	
			pkey1 + ''-'' + pkey2 not in ( select consignee+''-''+orderid  from '+ @source_db+'.dbo.outboundorheader) and 
			pkey1 + ''-'' + pkey2 + ''-'' + pkey3 not in ( select pkey1 + ''-'' + pkey2 + ''-'' + pkey3  from  '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''OUTBOUND'' ); SELECT @count = @@ROWCOUNT';

	
	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for ATTRIBUTE :  ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    

END


IF @Count_attributeoutbound > 0 AND @jobtype = 'B' 
BEGIN
	SET @sql = 'DELETE FROM '+ @source_db+'.dbo.attribute 
		    WHERE
			pkeytype=''OUTBOUND'' and 	
			pkey1 + ''-'' + pkey2 not in ( select consignee+''-''+orderid  from '+ @source_db+'.dbo.outboundorheader) and 
			pkey1 + ''-'' + pkey2 + ''-'' + pkey3 in ( select pkey1 + ''-'' + pkey2 + ''-'' + pkey3  from  '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''OUTBOUND'' ); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for ATTRIBUTE : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END



IF @jobtype = 'P' 
BEGIN
	SET @sql = 'DELETE FROM '+ @source_db+'.dbo.attribute 
		    WHERE
			pkeytype=''OUTBOUND'' and 	
			pkey1 + ''-'' + pkey2 not in ( select consignee+''-''+orderid  from '+ @source_db+'.dbo.outboundorheader) and 
			pkey1 + ''-'' + pkey2 + ''-'' + pkey3 in ( select pkey1 + ''-'' + pkey2 + ''-'' + pkey3  from  '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''OUTBOUND'' ); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for ATTRIBUTE : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


 
 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    


  SET @information = @information + CHAR(10) + '<td bgcolor="LightBlue">--------------------------------------------[ Exiting RWMS_OUTBOUND ]------------------------------------------------------------------</td></tr></table>' 
 
  
 
       
       
   --COMMIT or ROLLBACK TRANSACTION based on transaction_flag;    
   IF @transaction_flag = 1    
   BEGIN    
    ROLLBACK TRANSACTION;    
     /*-----------------Call the Log SP------------------------*/     

  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;    
   END    
   ELSE    
   BEGIN    
    COMMIT TRANSACTION;       
     /*-----------------Call the Log SP------------------------*/     

  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'M', @informational_message = @information;    
   END    
    
END TRY    
BEGIN CATCH    
  
IF @@TRANCOUNT > 0    
    ROLLBACK TRANSACTION;    
    

   /*-----------------Call the Log SP------------------------*/    
       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;   
     
END CATCH    

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET XACT_ABORT ON
GO  

IF OBJECT_ID('RWMS_SP_PARALLELPICK') IS NOT NULL
DROP PROCEDURE RWMS_SP_PARALLELPICK
GO

CREATE PROCEDURE dbo.RWMS_SP_PARALLELPICK( @jobtype CHAR(1), @retention_period CHAR(3) )    
AS    
/*    
  ---PARALLELPICK and PARALLELPICKDETAIL    
  Author   : Diwakar
  Created Date  : 07/25/2014    
  Purpose  : The stored procedure performs archival &/ purging for the PARALLELPICK & PARALLELPICKDETAIL  tables 
  JIRA  :   RWMS-18  
  Execution : EXEC RWMS_SP_PARALLELPICK 'B', '31'			 SELECT * FROM RWMS_LOG_DETAIL
    
*/    
    
BEGIN TRY    
    
 
  SET NOCOUNT ON;
  
    
BEGIN DISTRIBUTED TRANSACTION;      
   
/*------------------------------[ DECLARATION ]--------------------------------------------*/    
    
declare @retentiondays int;    
set @retentiondays = CAST(@retention_period AS INT);    
    
declare @CountAudit int;    
    
DECLARE @linkedserver VARCHAR(250);    
DECLARE @source_db VARCHAR(250);    
DECLARE @destination_db VARCHAR(250);    
    
DECLARE @query NVARCHAR(MAX);    
    
DECLARE @count_parallelpick INT;    
DECLARE @count_parallelpickdetail INT;    
    
DECLARE @information NVARCHAR(MAX);    
    
DECLARE @rowCount_archived INT;    
DECLARE @rowCount_purged INT;    
    
DECLARE @transaction_flag BIT -- 0 for COMMIT and 1 for ROLLBACK    

DECLARE @sql NVARCHAR(4000);
 DECLARE @sql_count INT; 
    
SET @linkedserver = ( SELECT Linkedservername FROM RWMS_ARCHIVAL_MASTER );    
SET @source_db = ( SELECT source_db_name FROM RWMS_ARCHIVAL_MASTER );    
SET @destination_db = ( SELECT dest_db_name FROM RWMS_ARCHIVAL_MASTER );    
    
DECLARE @arch_id INT  = ( SELECT arch_id FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'PARALLELPICK' );    
    

DECLARE @log_level char(1) = ( SELECT loglevel FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'PARALLELPICK' ); 



/*------------------------------[ Determine the level of logging ]---------------------------*/    
    
    
    
  SET @information = '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Entering RWMS_PARALLELPICK ]------------------------------------------------------------------</td>';    
  SET @information = @information + CHAR(10) + '<tr><td> Archival/ Purging log information for dbo.PARALLELPICKDETAIL  </td></tr> ';    
  SET @information = @information + CHAR(10) + '<tr><td> Start time : ' + CONVERT( VARCHAR(25), GETDATE(), 121) + ' </td></tr>';    

   
/*------------------------------[ PARALLELPICKDETAIL ]-----------------------------------------*/    
    
	SET @query = 'select 
		@Countparallelpickdetail = count(1) 
	        from 
		'+ @source_db+'.dbo.parallelpickdetail 
	        where  
		PICKLIST NOT IN ( SELECT PICKLIST FROM  '+ @source_db+'.dbo.PICKDETAIL ) AND
		PICKLIST NOT IN ( SELECT PICKLIST FROM ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpickdetail )';

	EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @Countparallelpickdetail INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver, @Countparallelpickdetail = @Count_parallelpickdetail OUTPUT;    
					

	IF @Count_parallelpickdetail = 0 AND @jobtype <> 'P'
	BEGIN
		     SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
	END
		
	IF @Count_parallelpickdetail > 0 AND @jobtype  IN ('A', 'B')    
	BEGIN
		SET @sql='insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpickdetail
 			 select 
				* 
			from 
				'+ @source_db+'.dbo.parallelpickdetail 
			where 
				PICKLIST NOT IN ( SELECT PICKLIST FROM '+ @source_db+'.dbo.PICKDETAIL ) AND
				PICKLIST NOT IN ( SELECT PICKLIST FROM ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpickdetail ); SELECT @count = @@ROWCOUNT';


		EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

		 -------------------Logging informational messages----------------------    
        
		  SET @rowCount_archived = @sql_count;    
    
		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for PARALLELPICKDETAIL : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
         
     	END


	 IF @Count_parallelpickdetail > 0 AND @jobtype  = 'B'    
	BEGIN
		SET @sql='delete ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpickdetail 
			  where 
				PICKLIST NOT IN ( SELECT PICKLIST FROM '+ @source_db+'.dbo.PICKDETAIL ) AND
				PICKLIST IN ( SELECT PICKLIST FROM ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpickdetail ); SELECT @count = @@ROWCOUNT';

		EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
         
		 -------------------Logging informational messages----------------------    
        
		 SET @rowCount_purged = @sql_count;    
            
		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for PARALLELPICKDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     
         
	END
	
	IF @jobtype  = 'P'    
	BEGIN
		SET @sql='delete ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpickdetail 
			  where 
				PICKLIST NOT IN ( SELECT PICKLIST FROM '+ @source_db+'.dbo.PICKDETAIL ) AND
				PICKLIST IN ( SELECT PICKLIST FROM ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpickdetail ); SELECT @count = @@ROWCOUNT';

		EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
         
		 -------------------Logging informational messages----------------------    
        
		 SET @rowCount_purged = @sql_count;    
            
		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for PARALLELPICKDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     
         
	END
      		
      		
      		
      	
	IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
	BEGIN    
				
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
		 -- Set the transaction flag to roll back    
		SET @transaction_flag = 1;    
		
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
     	END    

	

	 /*------------------------------[ PARALLELPICK ]-----------------------------------------*/    

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for PARALLELPICK </td></tr>';    
     
		SET @query  = 'select 
			@countparallelpick = count(1) 
		          from  
			'+ @source_db+'.dbo.parallelpick  
		         where 
			parallelpickid NOT IN ( select parallelpickid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpick) AND 
			PARALLELPICKID NOT IN (SELECT PARALLELPICKID FROM '+ @source_db+'.dbo.PARALLELPICKDETAIL)';



	EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @countparallelpick INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@countparallelpick = @count_parallelpick OUTPUT;    

	IF @count_parallelpick  = 0 AND @jobtype <> 'P'
	BEGIN
		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
	END


	IF @count_parallelpick  > 0 
	BEGIN
		 SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpick
 			    select 
				* 
			    from  
				'+ @source_db+'.dbo.parallelpick  
			    where 
				PARALLELPICKID NOT IN (SELECT  PARALLELPICKID FROM  '+ @source_db+'.dbo.PARALLELPICKDETAIL) AND 
				parallelpickid NOT IN ( select parallelpickid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpick); SELECT @count = @@ROWCOUNT';


		EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

		SET @rowCount_archived = @sql_count;    
    
		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for PARALLELPICK : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
     
	END

	IF @count_parallelpick > 0 AND @jobtype = 'B'    
	BEGIN
		SET @sql = ' delete FROM '+ @source_db+'.dbo.parallelpick  
			     where 
				PARALLELPICKID NOT IN (SELECT  PARALLELPICKID FROM  '+ @source_db+'.dbo.PARALLELPICKDETAIL) AND
				parallelpickid IN ( select parallelpickid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpick); SELECT @count = @@ROWCOUNT';

		
		EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

		SET @rowCount_purged = @sql_count; 

		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for PARALLELPICK : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


	END
	
	
	IF @jobtype = 'P'    
	BEGIN
		SET @sql = ' delete FROM '+ @source_db+'.dbo.parallelpick  
			     where 
				PARALLELPICKID NOT IN (SELECT  PARALLELPICKID FROM  '+ @source_db+'.dbo.PARALLELPICKDETAIL) AND
				parallelpickid IN ( select parallelpickid from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.parallelpick); SELECT @count = @@ROWCOUNT';

		
		EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

		SET @rowCount_purged = @sql_count; 

		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for PARALLELPICK : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


	END


 	IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
	 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      		-- Set the transaction flag to roll back    
      		SET @transaction_flag = 1;    
      		
			SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 	END    

	
 SET @information = @information + CHAR(10) + '<td bgcolor="LightBlue">--------------------------------------------[ Exiting RWMS_PARALLELPICK ]------------------------------------------------------------------</td></tr></table>' 
 

	    
   --COMMIT or ROLLBACK TRANSACTION based on transaction_flag;    
   IF @transaction_flag = 1    
   BEGIN    
    ROLLBACK TRANSACTION;    
  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;    

   END    
   ELSE    
   BEGIN    
    COMMIT TRANSACTION;       
    	  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'M', @informational_message = @information;    

   END    
    
END TRY    
BEGIN CATCH    
  
IF @@TRANCOUNT > 0    
    ROLLBACK TRANSACTION;    
    
  
   /*-----------------Call the Log SP for any error------------------------*/    
       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;   
     
END CATCH  	


GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET XACT_ABORT ON
GO    

IF OBJECT_ID('RWMS_SP_RECEIPT') IS NOT NULL
DROP PROCEDURE RWMS_SP_RECEIPT
GO
/*
SET XACT_ABORT ON
GO
EXEC RWMS_SP_RECEIPT 'A', '15'  SELECT * FROM RWMS_LOG_DETAIL



*/


CREATE PROCEDURE [dbo].[RWMS_SP_RECEIPT] ( @jobtype CHAR(1), @retention_period CHAR(3) )    
AS    
/*    
	 --receiptheader, receiptdetail, receivingexception and asndetail, attributereceipt and attributeasn--
	Author   : Diwakar.M
  	Created Date  : 07/22/2014    
  	Purpose  : The stored procedure performs archival &/ purging for RECEIPTDETAIL, RECEIPTHEADER , RECEIVINGEXCEPTION , ASNDETAIL, ATTRIBUTE-RECEIPT and ATTRIBUTE-ASN
  	JIRA  :  RWMS-18   
  	Execution : EXEC RWMS_SP_RECEIPT 'B', '100'    SELECT * FROM RWMS_LOG_DETAIL  
    
*/    
BEGIN TRY    
    
  --Prevents engine from returning result sets to client   
  SET NOCOUNT ON;
  
    
  BEGIN DISTRIBUTED TRANSACTION;    
    
/*------------------------------[ DECLARATION ]--------------------------------------------*/    
    
declare @retentiondays int;    
set @retentiondays = CAST(@retention_period AS INT);    
    
DECLARE @linkedserver VARCHAR(250);    
DECLARE @source_db VARCHAR(250);    
DECLARE @destination_db VARCHAR(250);    
    
DECLARE @query NVARCHAR(MAX);    
    
declare @Count_receiptdetail int;
declare @Count_receiptheader int;
declare @Count_receivingexception int;
declare @Count_asndetail int;
declare @Count_attributereceipt int;
declare @Count_attributeAsn int;
    
DECLARE @information NVARCHAR(MAX);    
    
DECLARE @rowCount_archived INT;    
DECLARE @rowCount_purged INT;    
    
DECLARE @transaction_flag BIT -- 0 for COMMIT and 1 for ROLLBACK    

     DECLARE @sql NVARCHAR(4000);
     DECLARE @sql_count INT; 
    
SET @linkedserver = ( SELECT Linkedservername FROM RWMS_ARCHIVAL_MASTER );    
SET @source_db = ( SELECT source_db_name FROM RWMS_ARCHIVAL_MASTER );    
SET @destination_db = ( SELECT dest_db_name FROM RWMS_ARCHIVAL_MASTER );    
    
DECLARE @arch_id INT  = ( SELECT arch_id FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'RECEIPT');        

DECLARE @log_level char(1) = ( SELECT loglevel FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'RECEIPT'); 

SET @information = '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Entering RWMS_RECEIPT ]------------------------------------------------------------------</td>';    
SET @information = @information + CHAR(10) + '<tr><td> Archival/ Purging log information for RECEIPTDETAIL, RECEIPTHEADER , RECEIVINGEXCEPTION , ASNDETAIL, ATTRIBUTE-RECEIPT and ATTRIBUTE-ASN</td></tr> ';    
SET @information = @information + CHAR(10) + '<tr><td> Start time : ' + CONVERT( VARCHAR(25), GETDATE(), 121) + ' </td></tr>';    




/*------------------------------[ RECEIPTDETAIL ]-----------------------------------------*/    
    
SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for RECEIPTDETAIL </td></tr>';    
     
SET  @query  = 'SELECT 
		@Countreceiptdetail = count(1) 
	          FROM
		 '+ @source_db+'.dbo.receiptdetail 
	          WHERE
		receipt in ( select receipt from '+ @source_db+'.dbo.receiptheader where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) and 
		receipt NOT IN (select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receiptdetail)';



EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countreceiptdetail INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countreceiptdetail = @Count_receiptdetail OUTPUT;    



IF @Count_receiptdetail = 0 AND @jobtype <> 'P'
BEGIN
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

IF @Count_receiptdetail > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'INSERT INTO ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receiptdetail
 		    SELECT 
			* 
		    FROM
			'+ @source_db+'.dbo.receiptdetail 
		    WHERE
			receipt  in ( select receipt from '+ @source_db+'.dbo.receiptheader where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) AND
			receipt NOT IN (select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receiptdetail); SELECT @count = @@ROWCOUNT';



	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;


	
	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for RECEIPTDETAIL : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
     
END

IF @Count_receiptdetail > 0 AND @jobtype = 'B'    
BEGIN

	SET @sql = 'DELETE FROM '+ @source_db+'.dbo.receiptdetail 
		   WHERE
			receipt  IN ( select receipt from '+ @source_db+'.dbo.receiptheader where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) AND
			receipt IN (select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receiptdetail); SELECT @count = @@ROWCOUNT';


   
	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;



	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for RECEIPTDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END



IF @jobtype = 'P'    
BEGIN

	SET @sql = 'DELETE FROM '+ @source_db+'.dbo.receiptdetail 
		   WHERE
			receipt  IN ( select receipt from '+ @source_db+'.dbo.receiptheader where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) AND
			receipt IN (select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receiptdetail); SELECT @count = @@ROWCOUNT';


   
	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;



	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for RECEIPTDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END
 

 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    



/*------------------------------[ RECEIVINGEXCEPTION ]-----------------------------------------*/    

SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for RECEIVINGEXCEPTION </td></tr>';    
     
SET @query  = 'SELECT 
		@Countreceivingexception = count(1) 
	         FROM
		'+ @source_db+'.dbo.receivingexception 
	         WHERE
		receipt  in ( select receipt from '+ @source_db+'.dbo.receiptheader where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) and 
		receipt NOT IN ( select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receivingexception)';



EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countreceivingexception INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countreceivingexception = @Count_receivingexception OUTPUT;    

IF @Count_receivingexception = 0 AND @jobtype <> 'P'
BEGIN
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END


IF @Count_receivingexception > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'INSERT INTO ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receivingexception
 		   SELECT
			*
		   FROM
			'+ @source_db+'.dbo.receivingexception 
		   WHERE
			receipt  in ( select receipt from '+ @source_db+'.dbo.receiptheader where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) AND
			receipt NOT IN ( select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receivingexception); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived  = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for RECEIVINGEXCEPTION : ' + CAST(@rowCount_archived  AS CHAR(10)) + '</td></tr>';     


END
 
IF @Count_receivingexception > 0 AND @jobtype = 'B'    
BEGIN

	SET @sql = 'DELETE FROM '+ @source_db+'.dbo.receivingexception 
		     WHERE
			receipt  in ( select receipt from '+ @source_db+'.dbo.receiptheader  where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) AND
			receipt IN ( select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receivingexception); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for RECEIVINGEXCEPTION : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END
 



IF @jobtype = 'P'    
BEGIN

	SET @sql = 'DELETE FROM '+ @source_db+'.dbo.receivingexception 
		     WHERE
			receipt  in ( select receipt from '+ @source_db+'.dbo.receiptheader  where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) AND
			receipt IN ( select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receivingexception); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for RECEIVINGEXCEPTION : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END 

 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    
 


/*------------------------------[ ASNDETAIL ]-----------------------------------------*/    

SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for ASNDETAIL </td></tr>';    
     
SET @query  = 'select 
		@Countasndetail = count(1) 
                         from 
		'+ @source_db+'.dbo.asndetail 
	         where 
		receipt  in ( select receipt from '+ @source_db+'.dbo.receiptheader where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) and 
		receipt NOT IN (select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.asndetail)';


EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countasndetail INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countasndetail = @Count_asndetail OUTPUT;    


if @Count_asndetail = 0 AND @jobtype <> 'P'
begin
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
end

if @Count_asndetail > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.asndetail
 		    select 
			* 
		    from 
			'+ @source_db+'.dbo.asndetail 
		     where 
			receipt  in ( select receipt from '+ @source_db+'.dbo.receiptheader where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) AND
			receipt NOT IN (select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.asndetail); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for ASNDETAIL : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';     


END
 
IF @Count_asndetail > 0 AND @jobtype = 'B'    
BEGIN

	SET @sql = 'delete FROM '+ @source_db+'.dbo.asndetail 
		   where 
			receipt  in ( select receipt from '+ @source_db+'.dbo.receiptheader where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) AND
			receipt IN (select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.asndetail); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for ASNDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


IF @jobtype = 'P'    
BEGIN

	SET @sql = 'delete FROM '+ @source_db+'.dbo.asndetail 
		   where 
			receipt  in ( select receipt from '+ @source_db+'.dbo.receiptheader where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''CLOSE'',''CANCELLED'')) AND
			receipt IN (select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.asndetail); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for ASNDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END
 

 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    



/*------------------------------[ RECEIPTHEADER ]-----------------------------------------*/    


SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for RECEIPTHEADER </td></tr>';    
     
SET @query  = 'SELECT
		@Countreceiptheader = count(1) 
	          FROM
		'+ @source_db+'.dbo.receiptheader  
	          WHERE
		datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and 
		status in (''CLOSE'',''CANCELLED'') and 
		receipt NOT IN ( select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receiptheader)';



EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countreceiptheader INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countreceiptheader = @Count_receiptheader OUTPUT;    


IF @Count_receiptheader = 0 AND @jobtype <> 'P'
BEGIN
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

IF @Count_receiptheader > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receiptheader
		   select 
			*  
		   from  
			'+ @source_db+'.dbo.receiptheader  
		   where  
			datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and 
			status in (''CLOSE'',''CANCELLED'') and
			 receipt NOT IN ( select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receiptheader); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for RECEIPTHEADER : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';     


END
 
IF @Count_receiptheader > 0 AND @jobtype  = 'B'    
BEGIN

	SET @sql = 'delete FROM '+ @source_db+'.dbo.receiptheader 
		    WHERE	
			datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and 
			status in (''CLOSE'',''CANCELLED'') AND
			receipt IN ( select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receiptheader); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for RECEIPTHEADER : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


IF @jobtype  = 'P'    
BEGIN

	SET @sql = 'delete FROM '+ @source_db+'.dbo.receiptheader 
		    WHERE	
			datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and 
			status in (''CLOSE'',''CANCELLED'') AND
			receipt IN ( select receipt from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.receiptheader); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for RECEIPTHEADER : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END

 IF @rowCount_archived <> @rowCount_purged  and @jobtype = 'B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    


/*------------------------------[ ATTRIBUTE-RECEIPT ]-----------------------------------------*/    


SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for ATTRIBUTE-RECEIPT </td></tr>';    
     
SET @query  = 'SELECT 
								@Countattributereceipt = count(1) 
						  FROM
								'+ @source_db+'.dbo.attribute 
						  WHERE
								pkeytype=''RECEIPT'' and 
								pkey2 not in ( select receipt  from  '+ @source_db+'.dbo.receiptheader) and 
								pkey1 + ''-'' + pkey2 + ''-'' + pkey3 not in ( select pkey1 + ''-'' + pkey2 + ''-'' + pkey3  from  ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''RECEIPT'' )';





EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @Countattributereceipt INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@Countattributereceipt = @Count_attributereceipt OUTPUT;    



IF @Count_attributereceipt = 0 AND @jobtype <> 'P'
BEGIN
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

		
if @Count_attributereceipt > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute
		    select 
			*  
		    from  
			'+ @source_db+'.dbo.attribute 
		    where 
			pkeytype=''RECEIPT'' and 
			pkey2 not in ( select receipt  from  '+ @source_db+'.dbo.receiptheader) and 
			pkey1 + ''-'' + pkey2 + ''-'' + pkey3 not in ( select pkey1 + ''-'' + pkey2 + ''-'' + pkey3  from  ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''RECEIPT'' ); SELECT @count = @@ROWCOUNT';

	
	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count; 
	


	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for ATTRIBUTE-RECEIPT : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';     


END


 
IF @Count_attributereceipt > 0 AND @jobtype = 'B'    
BEGIN
	
	
	
	SET @sql = 'delete A FROM '+ @source_db+'.dbo.attribute A where 
			pkeytype=''RECEIPT'' and 
			pkey2 not in ( select receipt  from '+ @source_db+'.dbo.receiptheader ) and 
			EXISTS ( SELECT 1 FROM ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute B WHERE A.PKEY1 = B.PKEY1 AND A.PKEY2 = B.PKEY2 AND A.PKEY3 = B.PKEY3 AND B.PKEYTYPE = ''RECEIPT'' ); SET @count = @@ROWCOUNT';

			--( pkey1+''-''+pkey2+''-''+pkey3 ) in ( select pkey1+''-''+pkey2+''-''+pkey3  from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''RECEIPT''); SET @count = @@ROWCOUNT;';

	
	
	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;


	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for ATTRIBUTE-RECEIPT: ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END



IF @jobtype = 'P'    
BEGIN
	
	
	
	SET @sql = 'delete A FROM '+ @source_db+'.dbo.attribute A where 
			pkeytype=''RECEIPT'' and 
			pkey2 not in ( select receipt  from '+ @source_db+'.dbo.receiptheader ) and 
			EXISTS ( SELECT 1 FROM ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute B WHERE A.PKEY1 = B.PKEY1 AND A.PKEY2 = B.PKEY2 AND A.PKEY3 = B.PKEY3 AND B.PKEYTYPE = ''RECEIPT'' ); SET @count = @@ROWCOUNT';

			--( pkey1+''-''+pkey2+''-''+pkey3 ) in ( select pkey1+''-''+pkey2+''-''+pkey3  from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''RECEIPT''); SET @count = @@ROWCOUNT;';

	
	
	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;


	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for ATTRIBUTE-RECEIPT: ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END
 

 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
          
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

 END    



/*------------------------------[ ATTRIBUTE-ASN ]-----------------------------------------*/    


SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for ATTRIBUTE-ASN </td></tr>';    
     
SET @query  = 'select 
		@Countattributeasn = count(1) 
	         from 
		'+ @source_db+'.dbo.attribute 
	         where 
		pkeytype=''ASN'' and 
		pkey1 not in ( select asnid  from  '+ @source_db+'.dbo.asndetail) and 
		pkey1  not in ( select pkey1 from  ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''ASN'' )';


EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @Countattributeasn INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@Countattributeasn = @Count_attributeasn OUTPUT;    


IF @Count_attributeasn = 0 AND @jobtype <> 'P'
BEGIN
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

		
if @Count_attributeasn > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute
		    select 
			1 
		   from  
			'+ @source_db+'.dbo.attribute 
		   where 
			pkeytype=''ASN'' and 
			pkey1 not in ( select asnid  from  '+ @source_db+'.dbo.asndetail) and 
			pkey1 not in ( select pkey1  from  ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''ASN'' );SELECT @count = @@ROWCOUNT';

	
	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for ATTRIBUTE-ASN : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';     


END
 

if @Count_attributeasn > 0 AND @jobtype = 'B'    
BEGIN

	SET @sql = 'delete FROM '+ @source_db+'.dbo.attribute 
		    where 
			pkeytype=''ASN'' and 
			pkey1 not in ( select asnid  from  '+ @source_db+'.dbo.asndetail) and 
			pkey1 in ( select pkey1 from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''ASN''  ); SELECT @count = @@ROWCOUNT';

	
	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for ATTRIBUTE-RECEIPT : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


if @jobtype = 'P'    
BEGIN

	SET @sql = 'delete FROM '+ @source_db+'.dbo.attribute 
		    where 
			pkeytype=''ASN'' and 
			pkey1 not in ( select asnid  from  '+ @source_db+'.dbo.asndetail) and 
			pkey1 in ( select pkey1 from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.attribute where pkeytype=''ASN''  ); SELECT @count = @@ROWCOUNT';

	
	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for ATTRIBUTE-RECEIPT : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END
 
 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    

 SET @information = @information + CHAR(10) + '<td bgcolor="LightBlue">--------------------------------------------[ Exiting RWMS_RECEIPT ]------------------------------------------------------------------</td></tr></table>' 
 

	    
   --COMMIT or ROLLBACK TRANSACTION based on transaction_flag;    
   IF @transaction_flag = 1    
   BEGIN    
    ROLLBACK TRANSACTION;    
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;    

   END    
   ELSE    
   BEGIN    
    COMMIT TRANSACTION;       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'M', @informational_message = @information;    

   END    
    
END TRY    
BEGIN CATCH    
  
IF @@TRANCOUNT > 0    
    ROLLBACK TRANSACTION;    
    
  
   /*-----------------Call the Log SP for any error------------------------*/    
       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;   
     
END CATCH  			

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET XACT_ABORT ON
GO    

IF OBJECT_ID('RWMS_SP_TRANSSHIPMENT') IS NOT NULL
DROP PROCEDURE RWMS_SP_TRANSSHIPMENT
GO

CREATE PROCEDURE [dbo].[RWMS_SP_TRANSSHIPMENT] ( @jobtype CHAR(1), @retention_period CHAR(3) )    
AS    
/*    
	 ---transshipment and transshipmentdetails----
	Author   : Diwakar.M
  	Created Date  : 07/22/2014    
  	Purpose  : The stored procedure performs archival &/ purging for TRANSSHIPMENT and TRANSSHIPMENTDETAIL
  	JIRA  :  RWMS-18   
  	Execution : EXEC RWMS_SP_TRANSSHIPMENT 'B', '31'    SELECT * FROM RWMS_LOG_DETAIL  
    
*/    
BEGIN TRY    
    
  --Prevents engine from returning result sets to client   
  SET NOCOUNT ON;
  
    
  BEGIN DISTRIBUTED TRANSACTION;    
    
/*------------------------------[ DECLARATION ]--------------------------------------------*/    
    
declare @retentiondays int;    
set @retentiondays = CAST(@retention_period AS INT);    
    
DECLARE @linkedserver VARCHAR(250);    
DECLARE @source_db VARCHAR(250);    
DECLARE @destination_db VARCHAR(250);    
    
DECLARE @query NVARCHAR(MAX);    
    
declare @Count_transshipment int;
declare @Count_transshipmentdetails int;
    
DECLARE @information NVARCHAR(MAX);    
    
DECLARE @rowCount_archived INT;    
DECLARE @rowCount_purged INT;    
    
DECLARE @transaction_flag BIT -- 0 for COMMIT and 1 for ROLLBACK    

     DECLARE @sql NVARCHAR(4000);
     DECLARE @sql_count INT; 
    
SET @linkedserver = ( SELECT Linkedservername FROM RWMS_ARCHIVAL_MASTER );    
SET @source_db = ( SELECT source_db_name FROM RWMS_ARCHIVAL_MASTER );    
SET @destination_db = ( SELECT dest_db_name FROM RWMS_ARCHIVAL_MASTER );    
    
DECLARE @arch_id INT  = ( SELECT arch_id FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'TRANSSHIPMENT');        

DECLARE @log_level char(1) = ( SELECT loglevel FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'TRANSSHIPMENT'); 

SET @information = '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Entering RWMS_TRANSSHIPMENT ]------------------------------------------------------------------</td>';    
SET @information = @information + CHAR(10) + '<tr><td> Archival/ Purging log information for TRANSSHIPMENT and TRANSSHIPMENTDETAIL</td></tr> ';    
SET @information = @information + CHAR(10) + '<tr><td> Start time : ' + CONVERT( VARCHAR(25), GETDATE(), 121) + ' </td></tr>';    



/*------------------------------[ TRANSSHIPMENTDETAIL ]-----------------------------------------*/    
    
SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for TRANSSHIPMENTDETAIL </td></tr>';    
     
SET  @query  = 'select 
		@Counttransshipmentdetails = count(1) 
	           from 
		'+ @source_db+'.dbo.transshipmentdetails 
	          where 
		consignee+''-''+transshipment  in ( select consignee+''-''+transshipment from '+ @source_db+'.dbo.transshipment where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''RECEIVED'',''SHIPPED'',''CANCELED'')) and 
		consignee+''-''+transshipment NOT IN ( select consignee+''-''+transshipment from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.transshipmentdetails)';


		EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Counttransshipmentdetails INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Counttransshipmentdetails = @Count_transshipmentdetails OUTPUT;    

		IF @Count_transshipmentdetails = 0 AND @jobtype <> 'P'
		BEGIN
			SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
		END

IF @Count_transshipmentdetails > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.transshipmentdetails
 		    select 
			* 
		    from 
			'+ @source_db+'.dbo.transshipmentdetails 
		    where 
			consignee+''-''+transshipment  in ( select consignee+''-''+transshipment  from '+ @source_db+'.dbo.transshipment where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''RECEIVED'',''SHIPPED'',''CANCELED'')) AND
			consignee+''-''+transshipment NOT IN ( select consignee+''-''+transshipment from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.transshipmentdetails); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for TRANSSHIPMENTDETAIL : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
     
END

IF @Count_transshipmentdetails > 0 AND @jobtype = 'B'    
BEGIN

	SET @sql = 'delete FROM '+ @source_db+'.dbo.transshipmentdetails 
		    where 
			consignee+''-''+transshipment  in ( select consignee+''-''+transshipment from '+ @source_db+'.dbo.transshipment where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''RECEIVED'',''SHIPPED'',''CANCELED'')) AND
			consignee+''-''+transshipment IN ( select consignee+''-''+transshipment from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.transshipmentdetails); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for TRANSSHIPMENTDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END



IF @jobtype = 'P'    
BEGIN

	SET @sql = 'delete FROM '+ @source_db+'.dbo.transshipmentdetails 
		    where 
			consignee+''-''+transshipment  in ( select consignee+''-''+transshipment from '+ @source_db+'.dbo.transshipment where datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and status in (''RECEIVED'',''SHIPPED'',''CANCELED'')) AND
			consignee+''-''+transshipment IN ( select consignee+''-''+transshipment from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.transshipmentdetails); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for TRANSSHIPMENTDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    


/*------------------------------[ TRANSSHIPMENT ]-----------------------------------------*/    

SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for TRANSSHIPMENT </td></tr>';    
     
SET @query  = 'select 
		@Counttransshipment = count(1) 
	         from  
		'+ @source_db+'.dbo.transshipment  
	         where  
		datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and 
		status in (''RECEIVED'',''SHIPPED'',''CANCELED'') and 
		consignee+''-''+transshipment NOT IN ( select consignee+''-''+transshipment from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.transshipment)';

EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Counttransshipment INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Counttransshipment = @Count_transshipment OUTPUT;    

IF @Count_transshipment = 0 AND @jobtype <> 'P'
BEGIN
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
END

IF @Count_transshipment > 0 AND @jobtype  IN ('A', 'B')    
BEGIN

	SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.transshipment
 		     select 
			*  
		     from  
			'+ @source_db+'.dbo.transshipment  
		     where  
			datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and 
			status in (''RECEIVED'',''SHIPPED'',''CANCELED'') AND
			consignee+''-''+transshipment NOT IN ( select consignee+''-''+transshipment from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.transshipment); SELECT @count = @@ROWCOUNT';


	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_archived = @sql_count;    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for TRANSSHIPMENT : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
     
END

IF @Count_transshipment > 0 AND @jobtype = 'B'    
BEGIN

	SET @sql = 'delete FROM '+ @source_db+'.dbo.transshipment 
	                    where  
			datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and 
			status in (''RECEIVED'',''SHIPPED'',''CANCELED'') AND
			consignee+''-''+transshipment IN ( select consignee+''-''+transshipment from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.transshipment); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for TRANSSHIPMENT : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END


IF @jobtype = 'P'    
BEGIN

	SET @sql = 'delete FROM '+ @source_db+'.dbo.transshipment 
	                    where  
			datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and 
			status in (''RECEIVED'',''SHIPPED'',''CANCELED'') AND
			consignee+''-''+transshipment IN ( select consignee+''-''+transshipment from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.transshipment); SELECT @count = @@ROWCOUNT';

	EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

	SET @rowCount_purged = @sql_count; 

	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for TRANSSHIPMENT : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


END
 

 IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      	-- Set the transaction flag to roll back    
      	SET @transaction_flag = 1;    
      	
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 END    



 SET @information = @information + CHAR(10) + '<td bgcolor="LightBlue">--------------------------------------------[ Exiting RWMS_TRANSSHIPMENT ]------------------------------------------------------------------</td></tr></table>' 
 

	    
   --COMMIT or ROLLBACK TRANSACTION based on transaction_flag;    
   IF @transaction_flag = 1    
   BEGIN    
    ROLLBACK TRANSACTION;    
    	  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;    

   END    
   ELSE    
   BEGIN    
    COMMIT TRANSACTION;       
    	  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'M', @informational_message = @information;    

   END    
    
END TRY    
BEGIN CATCH    
  
IF @@TRANCOUNT > 0    
    ROLLBACK TRANSACTION;    
    
  
   /*-----------------Call the Log SP for any error------------------------*/    
       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;   
     
END CATCH  	

GO


/****** Object:  StoredProcedure [dbo].[RWMS_SP_WAVE]    Script Date: 07/08/2014 18:59:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET XACT_ABORT ON
GO    
    
-- DROP PROCEDURE [RWMS_SP_WAVE]  SELECT * FROM RWMS_LOG_DETAIL

IF OBJECT_ID('RWMS_SP_WAVE') IS NOT NULL
DROP PROCEDURE RWMS_SP_WAVE
GO
    
CREATE PROCEDURE [dbo].[RWMS_SP_WAVE] ( @jobtype CHAR(1), @retention_period CHAR(3) )    
AS    
/*    
  ---wave ,wavedetail and waveorderdeliverylocation --    
  Author   : Diwakar/Chethan    
  Created Date  : 06/03/2014    
  Purpose  : The stored procedure performs archival &/ purging based on user's choice    
  JIRA  :     
  Execution : EXEC RWMS_SP_WAVE 'B', '15'    SELECT * FROM RWMS_LOG_DETAIL  
    
*/    
    
BEGIN TRY    
    
  -- Rolls back all transaction if the code encounters any runtime error.  
    
  SET NOCOUNT ON;
  
    
  BEGIN DISTRIBUTED TRANSACTION;    
    
/*------------------------------[ DECLARATION ]--------------------------------------------*/    
    
declare @retentiondays int;    
set @retentiondays = CAST(@retention_period AS INT);    
    
declare @Countwavedetail int;    
    
DECLARE @linkedserver VARCHAR(250);    
DECLARE @source_db VARCHAR(250);    
DECLARE @destination_db VARCHAR(250);    
    
DECLARE @query NVARCHAR(MAX);    
    
DECLARE @count_WaveDetail INT;    
    
DECLARE @information NVARCHAR(MAX);    
    
DECLARE @rowCount_archived INT;    
DECLARE @rowCount_purged INT;    
    
DECLARE @transaction_flag BIT -- 0 for COMMIT and 1 for ROLLBACK    

     DECLARE @sql NVARCHAR(4000);
     DECLARE @sql_count INT; 
    
SET @linkedserver = ( SELECT Linkedservername FROM RWMS_ARCHIVAL_MASTER );    
SET @source_db = ( SELECT source_db_name FROM RWMS_ARCHIVAL_MASTER );    
SET @destination_db = ( SELECT dest_db_name FROM RWMS_ARCHIVAL_MASTER );    
    
DECLARE @arch_id INT  = ( SELECT arch_id FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'WAVE' );    
    

DECLARE @log_level char(1) = ( SELECT loglevel FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'WAVE' ); 
    
/*------------------------------[ Determine the level of logging ]---------------------------*/    
    
    
    
  SET @information = '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Entering RWMS_WAVE ]------------------------------------------------------------------</td>';    
  SET @information = @information + CHAR(10) + '<tr><td> Archival/ Purging log information for WAVE tables</td></tr> ';    
  SET @information = @information + CHAR(10) + '<tr><td> Start time : ' + CONVERT( VARCHAR(25), GETDATE(), 121) + ' </td></tr>';    
      
  --
      
    
    
/*------------------------------[ WAVEDETAIL ]-----------------------------------------*/    
    
   SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for WAVEDETAIL </td></tr>';    
     
   set @query = 'SELECT @Countwavedetail = COUNT(1)    
         FROM    
            '+ @source_db+'.dbo.WAVEDETAIL    
         WHERE    
           wave in (select wave  from   '+ @source_db+'.dbo.WAVE where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + '    
           and   status IN (''COMPLETE'', ''CANCELED'')) and     
           wave NOT IN (select wave from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.wavedetail )';    
   
		
    
   EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countwavedetail INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countwavedetail = @count_WaveDetail OUTPUT;    
    
	 
    
   if @count_WaveDetail = 0  AND @jobtype <> 'P' 
   BEGIN    
     SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
   END    
  
   
    
   if @count_WaveDetail > 0 AND @jobtype  IN ('A', 'B')    
   begin    
    
			SET @sql='insert into '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.wavedetail    
			select *  from   '+@source_db+'.dbo.WAVEDETAIL      
			where wave in     
			 (select wave  from    '+@source_db+'.dbo.wave  where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3))+'    
			 and   status IN (''COMPLETE'', ''CANCELED'')) AND wave NOT IN (select wave from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.wavedetail); SELECT @count = @@ROWCOUNT';    
		   
			 EXEC sp_executesql @sql, N'@linkedserver VARCHAR(250), @source_db VARCHAR(250), @destination_db VARCHAR(250), @retentiondays INT, @count INT OUTPUT', @linkedserver = @linkedserver , @source_db = @source_db, @destination_db  = @destination_db, @retentiondays = @retentiondays,  @count = @sql_count OUTPUT;
		    
			-------------------Logging informational messages----------------------    
		        
			SET @rowCount_archived = @sql_count;    
		    
			SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for WAVEDETAIL : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
		      
   END    
 
       
       
   if @count_WaveDetail > 0 AND @jobtype  = 'B'    
   begin    
       
					SET @sql='delete from  '+@source_db+'.dbo.wavedetail      
				  where wave in     
					(select wave  from   '+@source_db+'.dbo.wave  where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3))+'    
					and   status IN (''COMPLETE'', ''CANCELED'')) AND wave IN (select wave from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.wavedetail); SELECT @count = @@ROWCOUNT';    

					EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
			         
			         
				-------------------Logging informational messages----------------------    
			        
				SET @rowCount_purged = @sql_count;    
			            
				SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for WAVEDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     
			         
     END     
    
      if @jobtype  = 'P'    
	  begin    
       
					SET @sql='delete from  '+@source_db+'.dbo.wavedetail      
				  where wave in     
					(select wave  from   '+@source_db+'.dbo.wave  where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3))+'    
					and   status IN (''COMPLETE'', ''CANCELED'')) AND wave IN (select wave from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.wavedetail); SELECT @count = @@ROWCOUNT';    

					EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
			         
			         
				-------------------Logging informational messages----------------------    
			        
				SET @rowCount_purged = @sql_count;    
			            
				SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for WAVEDETAIL : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     
			         
     END     
    
         
     IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
     BEGIN    
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      -- Set the transaction flag to roll back    
      SET @transaction_flag = 1;    
      
	SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
     END    
    
    

  

    
    
/*------------------------------[ WAVEORDERDELIVERYLOCATION ]-----------------------------------------*/    
    
SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for WAVEORDERDELIVERYLOCATION</td></tr>';     
    
declare @Countwaveorderdeliverylocation int;    
declare @count_wave_delivery int;    
    
      
    SET @query='SELECT @Countwaveorderdeliverylocation = COUNT(1)    
             FROM    '+@source_db+'.dbo.waveorderdeliverylocation      
                WHERE  wave IN     
            (SELECT wave  FROM   '+@source_db+'.dbo.wave      
            WHERE  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3))+'    
            AND   status IN (''COMPLETE'', ''CANCELED'')) AND wave IN     
            (SELECT wave FROM '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.waveorderdeliverylocation)';    
                
      
        
    EXECUTE sp_executesql @query,N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,@Countwaveorderdeliverylocation INT OUTPUT',@source_db=@source_db,@destination_db=@destination_db,@retentiondays=@retentiondays,@linkedserver=@linkedserver,@Countwaveorderdeliverylocation=@count_wave_delivery OUTPUT;    
     
       
   if @count_wave_delivery = 0  AND @jobtype <> 'P' 
   BEGIN    
			SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>'; 
   END    
        
   IF @count_wave_delivery > 0 AND @jobtype  IN ('A', 'B')    
    
    BEGIN    
     SET @sql= ' insert into '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.waveorderdeliverylocation    
            select *  from   '+@source_db+'.dbo.waveorderdeliverylocation      
           where wave in     
            (select wave  from   '+@source_db+'.dbo.wave      
            where  datediff(d,editdate,getdate()) >='+CAST(@retentiondays AS char(3))+'    
           and   status IN (''COMPLETE'', ''CANCELED'') and wave not in     
           (select wave from  '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.waveorderdeliverylocation); SELECT @count = @@ROWCOUNT';                                   

     EXEC sp_executesql @sql, N'@linkedserver VARCHAR(250), @source_db VARCHAR(250), @destination_db VARCHAR(250), @retentiondays INT, @count INT OUTPUT', @linkedserver = @linkedserver , @source_db = @source_db, @destination_db  = @destination_db, @retentiondays = @retentiondays,  @count = @sql_count OUTPUT;
        
      
    -------------------Logging informational messages----------------------    
        
    SET @rowCount_archived = @sql_count;    
            
    SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for WAVEORDERDELIVERYLOCATION : ' + CAST(@rowCount_archived AS CHAR(10))  + '</td></tr>'; 
         
   END    
        
        
        
    IF @count_wave_delivery > 0 AND @jobtype  = 'B'    
    BEGIN    
				SET @sql = ' delete from  '+@source_db+'.dbo.waveorderdeliverylocation     
					  where wave in (select wave  from    '+@source_db+'.dbo.wave      
					  where  datediff(d,editdate,getdate()) >='+CAST(@retentiondays AS CHAR(3))+'    
					  and   status IN (''COMPLETE'', ''CANCELED''))    
						   and wave in (select wave from  '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.waveorderdeliverylocation); SELECT @count = @@ROWCOUNT';                               
			      
					
				EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
				
			  -------------------Logging informational messages----------------------    
			        
				SET @rowCount_purged = @sql_count;    
			            
				SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for WAVEORDERDELIVERYLOCATION : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';    
			         
     END     
    
    
     IF @jobtype  = 'P'    
    BEGIN    
				SET @sql = ' delete from  '+@source_db+'.dbo.waveorderdeliverylocation     
					  where wave in (select wave  from    '+@source_db+'.dbo.wave      
					  where  datediff(d,editdate,getdate()) >='+CAST(@retentiondays AS CHAR(3))+'    
					  and   status IN (''COMPLETE'', ''CANCELED''))    
						   and wave in (select wave from  '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.waveorderdeliverylocation); SELECT @count = @@ROWCOUNT';                               
			      
					
				EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
				
			  -------------------Logging informational messages----------------------    
			        
				SET @rowCount_purged = @sql_count;    
			            
				SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for WAVEORDERDELIVERYLOCATION : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';    
			         
     END     
    
     
         
     IF @rowCount_archived <> @rowCount_purged and @jobtype='B'    
     BEGIN    
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
            
      -- Set the transaction flag to roll back    
      SET @transaction_flag = 1;    
      
		SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

     END    
         
         
         
 /*----------------------------------------------[WAVE]-------------------------------------------------*/    
    
SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for WAVE</td></tr>';    
     
    
DECLARE @Countwave int    
DECLARE @count_wave int    
    
       
      SET @query =' select @countwave=COUNT(1) from   '+@source_db+'.dbo.wave     
            where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays as char(3))+'    
            and   status IN (''COMPLETE'', ''CANCELED'') and wave in     
            (select wave from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.wave)';    
            
   EXECUTE sp_executesql @query,N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,@Countwave INT OUTPUT',@source_db=@source_db,@destination_db=@destination_db,@retentiondays=@retentiondays,@linkedserver=@linkedserver,@Countwave=@count_wave OUTPUT;    
   
       
   if @count_wave = 0 AND @jobtype <> 'P'
   BEGIN    
				SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';   
   END    
       
       
 IF @count_wave > 0 AND @jobtype IN ('A','B')    
 BEGIN    
				  SET @sql=' insert into '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.wave    
						select *  from   '+@source_db+'.dbo.wave      
						where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays as char(3))+'    
						and   status IN (''COMPLETE'', ''CANCELED'') and wave not in     
						(select wave from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.wave); SELECT @count = @@ROWCOUNT';    
			            
			           
     				EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
			 
			  -------------------Logging informational messages----------------------    
			        
				SET @rowCount_archived = @sql_count;    
			            
				SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for WAVE : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';     
			         
   END    
       
       
   IF @count_wave > 0 AND @jobtype = 'B'
   BEGIN    
         
				  SET @sql=' delete from '+@source_db+'.dbo.wave     
						  where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays as char(3))+'    
						  and   status IN (''COMPLETE'', ''CANCELED'') and wave in     
						 (select wave from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.wave); SELECT @count = @@ROWCOUNT';
			             
     				EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
			     	
				-------------------Logging informational messages----------------------    
			        
				SET @rowCount_purged = @sql_count;    
			            
				SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for WAVE : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';    
			         
     END     
    
      IF @jobtype = 'P'
	  BEGIN    
         
				  SET @sql=' delete from '+@source_db+'.dbo.wave     
						  where  datediff(d,editdate,getdate()) >= '+CAST(@retentiondays as char(3))+'    
						  and   status IN (''COMPLETE'', ''CANCELED'') and wave in     
						 (select wave from '+QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.wave); SELECT @count = @@ROWCOUNT';
			             
     				EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;
			     	
				-------------------Logging informational messages----------------------    
			        
				SET @rowCount_purged = @sql_count;    
			            
				SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for WAVE : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';    
			         
     END      
         
     IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
     BEGIN    
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
            
      -- Set the transaction flag to roll back    
      SET @transaction_flag = 1;    
      
      	SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
     END    
      
    /**** This should be a RWMS_SP_LOG_MESSAGE with message type 'D' - RPH
	All PRINT statements should be a RWMS_SP_LOG_MESSAGE with message type 'D'
	PRINT @information   
	**********************************************************************/
    --EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'D', @informational_message = @information;    
      
  
   
   /*     
   IF @log_level = 'M'    
   BEGIN    
       
  /*-----------------Call the Log SP------------------------*/     
  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @message_type = 'M', @informational_message = @information;    
      
   END */
  
    SET @information = @information + CHAR(10) + '<td bgcolor="LightBlue">--------------------------------------------[ Exiting RWMS_WAVE ]------------------------------------------------------------------</td></tr></table>' 
 
  
  
       
       
   --COMMIT or ROLLBACK TRANSACTION based on transaction_flag;    
   IF @transaction_flag = 1    
   BEGIN    
    ROLLBACK TRANSACTION;    
    /*-----------------Call the Log SP------------------------*/     

  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;    
   END    
   ELSE    
   BEGIN    
    COMMIT TRANSACTION;       
    /*-----------------Call the Log SP------------------------*/     

  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'M', @informational_message = @information;    
   END    
    
END TRY    
BEGIN CATCH    
  
IF @@TRANCOUNT > 0    
    ROLLBACK TRANSACTION;    
    
   -- PRINT ERROR_MESSAGE();
   /*-----------------Call the Log SP------------------------*/    
       
   --EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @message_type = 'E', @informational_message = NULL;   
/** Changed line below RPH ***/   
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;   
     
END CATCH    
    
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET XACT_ABORT ON
GO    

IF OBJECT_ID('RWMS_SP_WORKORDER') IS NOT NULL
DROP PROCEDURE RWMS_SP_WORKORDER
GO

CREATE PROCEDURE [dbo].[RWMS_SP_WORKORDER] ( @jobtype CHAR(1), @retention_period CHAR(3) )    
AS    
/*    
	--WORKORDER--
	Author   : Diwakar.M
  	Created Date  : 07/25/2014    
  	Purpose  : The stored procedure performs archival &/ purging for the WORKORDER table
  	JIRA  :  RWMS-18   
  	Execution : EXEC RWMS_SP_WORKORDER 'B', '31'    SELECT * FROM RWMS_LOG_DETAIL  
    
*/    
BEGIN TRY    
    
  --Prevents engine from returning result sets to client   
  SET NOCOUNT ON;
  
    
  BEGIN DISTRIBUTED TRANSACTION;    
    
/*------------------------------[ DECLARATION ]--------------------------------------------*/    
    
declare @retentiondays int;    
set @retentiondays = CAST(@retention_period AS INT);    
    
DECLARE @linkedserver VARCHAR(250);    
DECLARE @source_db VARCHAR(250);    
DECLARE @destination_db VARCHAR(250);    
    
DECLARE @query NVARCHAR(MAX);    
    
declare @Count_workorderheader int;
    
DECLARE @information NVARCHAR(MAX);    
    
DECLARE @rowCount_archived INT;    
DECLARE @rowCount_purged INT;    
    
DECLARE @transaction_flag BIT -- 0 for COMMIT and 1 for ROLLBACK    

     DECLARE @sql NVARCHAR(4000);
     DECLARE @sql_count INT; 
    
SET @linkedserver = ( SELECT Linkedservername FROM RWMS_ARCHIVAL_MASTER );    
SET @source_db = ( SELECT source_db_name FROM RWMS_ARCHIVAL_MASTER );    
SET @destination_db = ( SELECT dest_db_name FROM RWMS_ARCHIVAL_MASTER );    
    
DECLARE @arch_id INT  = ( SELECT arch_id FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'WORKORDER');        

DECLARE @log_level char(1) = ( SELECT loglevel FROM RWMS_ARCHIVAL_DETAIL WHERE PREFIX_CODE = 'WORKORDER'); 

SET @information = '<table border="1" style="font:8pt" cellpadding="2" cellspacing="2"><tr bgcolor="LightBlue"><td>--------------------------------------------[ Entering RWMS_WORKORDER ]------------------------------------------------------------------</td>';    
SET @information = @information + CHAR(10) + '<tr><td> Archival/ Purging log information for WORKORDER</td></tr> ';    
SET @information = @information + CHAR(10) + '<tr><td> Start time : ' + CONVERT( VARCHAR(25), GETDATE(), 121) + ' </td></tr>';    



/*------------------------------[ WORKORDERHEADER ]-----------------------------------------*/    
    
	SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Archival/ Purging log information for WORKORDERHEADER </td></tr>';    
     
	SET  @query  = 'select 
		@Countworkorderheader = count(1) 
	          from 
		'+ @source_db+'.dbo.WORKORDERHEADER 
	          where  
		datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and   
		status IN (''COMPLETE'',''CANCELED'') and 
		CONSIGNEE+''-''+ORDERID  NOT IN (select CONSIGNEE+''-''+ORDERID from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.WORKORDERHEADER)';


	EXECUTE sp_executesql @query, N'@source_db VARCHAR(250), @destination_db VARCHAR(250), @linkedserver VARCHAR(250), @retentiondays INT,  @Countworkorderheader INT OUTPUT', @source_db = @source_db, @destination_db = @destination_db, @linkedserver = @linkedserver,@retentiondays = @retentiondays, @Countworkorderheader = @Count_workorderheader OUTPUT;    

	IF @Count_workorderheader = 0 AND @jobtype <> 'P'
	BEGIN
		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : No records to archive/purge</td></tr>';    
	END

	IF @Count_workorderheader > 0 AND @jobtype  IN ('A', 'B')    
	BEGIN

		SET @sql = 'insert into ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.WORKORDERHEADER
		    	   select 
				*  
		   	   from  
				'+ @source_db+'.dbo.WORKORDERHEADER
		   	where  
				datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and   
				status IN (''COMPLETE'',''CANCELED'') AND
				CONSIGNEE+''-''+ORDERID  NOT IN (select CONSIGNEE+''-''+ORDERID from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.WORKORDERHEADER); SELECT @count = @@ROWCOUNT';

		EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

		SET @rowCount_archived = @sql_count;    
    
		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows archived for WORKORDERHEADER : ' + CAST(@rowCount_archived AS CHAR(10)) + '</td></tr>';    
     
	END

	IF @Count_workorderheader > 0 AND @jobtype = 'B'    
	BEGIN
		SET @sql = 'delete from '+ @source_db+'.dbo.WORKORDERHEADER 
			    where  
				datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and   
				status IN (''COMPLETE'',''CANCELED'') AND
				CONSIGNEE+''-''+ORDERID  IN (select CONSIGNEE+''-''+ORDERID from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.WORKORDERHEADER); SELECT @count = @@ROWCOUNT';


		EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

		SET @rowCount_purged = @sql_count; 

		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for WORKORDERHEADER : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


	END
	
	IF @jobtype = 'P'    
	BEGIN
		SET @sql = 'delete from '+ @source_db+'.dbo.WORKORDERHEADER 
			    where  
				datediff(d,editdate,getdate()) >= '+CAST(@retentiondays AS CHAR(3)) + ' and   
				status IN (''COMPLETE'',''CANCELED'') AND
				CONSIGNEE+''-''+ORDERID  IN (select CONSIGNEE+''-''+ORDERID from ' + QUOTENAME(@linkedserver)+'.'+@destination_db+'.dbo.WORKORDERHEADER); SELECT @count = @@ROWCOUNT';


		EXEC sp_executesql @sql, N'@count INT OUTPUT', @sql_count OUTPUT;

		SET @rowCount_purged = @sql_count; 

		SET @information = @information + CHAR(10) + '<tr><td> ' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rows purged for WORKORDERHEADER : ' + CAST(@rowCount_purged AS CHAR(10)) + '</td></tr>';     


	END


 	IF @rowCount_archived <> @rowCount_purged  and @jobtype='B'    
	 BEGIN    
 
					SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Rolling back transaction since the number of rows purged is not equal to rows archived</p></b></td></tr>';    
          
      		-- Set the transaction flag to roll back    
      		SET @transaction_flag = 1;    
      		
			SET @information = @information + CHAR(10) + '<tr><td><b><p style="color:red">' +  CONVERT( VARCHAR(25), GETDATE(), 121) + ' : Transaction rolled back successfully. No records have been archived/purged!</p></b></td></tr>';    

          
 	END    


	
 	SET @information = @information + CHAR(10) + '<td bgcolor="LightBlue">--------------------------------------------[ Exiting RWMS_WORKORDER ]------------------------------------------------------------------</td></tr></table>' 
 

	    
   --COMMIT or ROLLBACK TRANSACTION based on transaction_flag;    
   IF @transaction_flag = 1    
   BEGIN    
    ROLLBACK TRANSACTION;    
    	  EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;    

   END    
   ELSE    
   BEGIN    
    COMMIT TRANSACTION;       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'M', @informational_message = @information;    
    
   END    
    
END TRY    
BEGIN CATCH    
  
IF @@TRANCOUNT > 0    
    ROLLBACK TRANSACTION;    
    
  
   /*-----------------Call the Log SP for any error------------------------*/    
       
    EXEC RWMS_SP_LOG_MESSAGE @archid = @arch_id, @loglevel = @log_level, @messagetype = 'E', @informational_message = @information;   
     
END CATCH  	

GO

    
    
 

/*------------------------------------------------------------------------------------------------------------*/

USE [msdb]
GO

/****** Object:  Job [Archival and Purging]    Script Date: 09/05/2014 11:40:53 ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]]    Script Date: 09/05/2014 11:40:53 ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'Archival and Purging', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Archival and Purging - Stored Procedure]    Script Date: 09/05/2014 11:40:53 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Archival and Purging - Stored Procedure', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=4, 
		@on_success_step_id=2, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC RWMS_SP_ARCHIVAL_PURGE
GO', 
		@database_name=N'RWMS_SYS', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Generate HTML report]    Script Date: 09/05/2014 11:40:53 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Generate HTML report', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE RWMS_SYS
GO
DECLARE @drive CHAR(1) = NULL;
DECLARE @directory VARCHAR(500) = NULL;

EXEC RWMS_SP_CREATE_LOG @drive = @drive, @directory = @directory;', 
		@database_name=N'RWMS_SYS', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO







