Imports System.ComponentModel
Imports System.Configuration.Install
Imports Made4Net.Shared

<RunInstaller(True)> Public Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

#Region " Component Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Installer overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    Private WithEvents ServiceProcessInstaller1 As System.ServiceProcess.ServiceProcessInstaller
    Private WithEvents ServiceInstaller1 As System.ServiceProcess.ServiceInstaller
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ServiceProcessInstaller1 = New System.ServiceProcess.ServiceProcessInstaller
        Me.ServiceInstaller1 = New System.ServiceProcess.ServiceInstaller
        '
        'ServiceProcessInstaller1
        '
        Me.ServiceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem
        Me.ServiceProcessInstaller1.Password = Nothing
        Me.ServiceProcessInstaller1.Username = Nothing
        '
        'ServiceInstaller1
        '
        'Me.ServiceInstaller1.DisplayName = "Expert TaskManager"
        'Me.ServiceInstaller1.ServiceName = "Expert TaskManager"
        Me.ServiceInstaller1.DisplayName = Util.GetServiceName(ConfigurationSettingsConsts.ServiceInstancename, ConfigurationSettingsConsts.TaskManagerServiceSection)
        Me.ServiceInstaller1.ServiceName = Util.GetServiceName(ConfigurationSettingsConsts.ServiceInstancename, ConfigurationSettingsConsts.TaskManagerServiceSection)

        Me.ServiceInstaller1.ServicesDependedOn = New String() {"MSMQ"}
        Me.ServiceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        Me.ServiceInstaller1.DelayedAutoStart = True
        '
        'ProjectInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.ServiceProcessInstaller1, Me.ServiceInstaller1})

    End Sub

#End Region

    

    Public Overrides Sub Install(ByVal stateSaver As System.Collections.IDictionary)
        Try
            'MsgBox("Uninstall")
            Me.Uninstall(Nothing)
            'MsgBox("Uninstall Complete")
            System.Threading.Thread.Sleep(3000)
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try
        MyBase.Install(stateSaver)
        'System.Diagnostics.Debugger.Break()
        If Not IsNothing(Me.Context.Parameters.Item("SQLSERVER")) Then
            updateConfigFile()
        End If
    End Sub
    Public Function updateConfigFile() As String
        Dim server As String = Me.Context.Parameters.Item("SQLSERVER")
        Dim user As String = Me.Context.Parameters.Item("SQLUSER")
        Dim pwd As String = Me.Context.Parameters.Item("SQLPWD")
        Dim sysdb As String = Me.Context.Parameters.Item("SQLSYSDB")
        Dim datadb As String = Me.Context.Parameters.Item("SQLDATADB")


        Dim Asm As System.Reflection.Assembly = _
        System.Reflection.Assembly.GetExecutingAssembly
        Dim strConfigLoc As String
        strConfigLoc = Asm.Location

        Dim strTemp As String
        Dim strName As String
        Try

        
        strTemp = strConfigLoc
        strName = strTemp.Substring(strTemp.LastIndexOf("\") + 1)
        strTemp = strTemp.Remove(strTemp.LastIndexOf("\"), Len(strTemp) - _
          strTemp.LastIndexOf("\"))
        Dim FileInfo As System.IO.FileInfo = New System.IO.FileInfo(strTemp & "\" & strName & ".config")
        If Not FileInfo.Exists Then
            Throw New Exception(strName & " : Missing config file")
        End If

        Dim XmlDocument As New System.Xml.XmlDocument
        XmlDocument.Load(FileInfo.FullName)

        ' Finds the right node and change it to the new value.
        XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Made4NetSchema_DSN']").Attributes.GetNamedItem("value").InnerText = sysdb
        XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Made4NetSchema_MappedName']").Attributes.GetNamedItem("value").InnerText = user
        XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Made4NetSchema_MappedKey']").Attributes.GetNamedItem("value").InnerText = pwd
        XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Made4NetSchema_DBType']").Attributes.GetNamedItem("value").InnerText = "SQL"
        XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Default_DSN']").Attributes.GetNamedItem("value").InnerText = datadb
        XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Default_MappedName']").Attributes.GetNamedItem("value").InnerText = user
        XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Default_MappedKey']").Attributes.GetNamedItem("value").InnerText = pwd
            XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Default_DBType']").Attributes.GetNamedItem("value").InnerText = "SQL"

            XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='ServiceLogDirectory']").Attributes.GetNamedItem("value").InnerText = strTemp & "\Logs\"

            XmlDocument.Save(FileInfo.FullName)
        Catch ex As Exception
            Throw New Exception(strConfigLoc & " Install Error : " & ex.Message)
        End Try
        Return Nothing
    End Function
End Class
