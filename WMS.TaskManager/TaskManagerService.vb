Imports System.ServiceProcess

Public Class Service1
    Inherits System.ServiceProcess.ServiceBase

    Dim oTaskManager As TaskManager

#Region " Component Designer generated code "

    Public Sub New()
        MyBase.New()

        ' This call is required by the Component Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call

    End Sub

    'UserService overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' The main entry point for the process
    <MTAThread()> _
    Shared Sub Main()
        Dim ServicesToRun() As System.ServiceProcess.ServiceBase

        ' More than one NT Service may run within the same process. To add
        ' another service to this process, change the following line to
        ' create a second service object. For example,
        '
        '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1, New MySecondUserService}
        '
        ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1}

        System.ServiceProcess.ServiceBase.Run(ServicesToRun)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.
    ' Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        Me.ServiceName = "Expert Task Manager"
    End Sub

#End Region

    Protected Overrides Sub OnStart(ByVal args() As String)
        Dim Connected As Boolean = Connect()
        If Not Connected Then
            Throw New ApplicationException("License Not Found!")
        End If
        oTaskManager = New TaskManager
        oTaskManager.StartQueue()
    End Sub

    Protected Overrides Sub OnStop()
        DisConnect()
        oTaskManager.StopQueue()
    End Sub


#Region "License Management"

    Private Function Connect() As Boolean
        Dim ret As Boolean
        'Dim licUser As String = Made4Net.Shared.AppConfig.Get("LicenseUserId")
        ''Dim licUser As String = Made4Net.Shared.GetAppConfigNameValue("TaskManagerServiceConfig", "LicenseUserId", True)
        Dim licUser As String = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceSection,
                    Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceLicenseUserId) '' PWMS-817
        ret = True 'Made4Net.DataAccess.DataInterface.Connect(licUser)
        Return ret
    End Function

    Private Function DisConnect() As Boolean
        Dim ret As Boolean = False
        'Dim licUser As String = Made4Net.Shared.AppConfig.Get("LicenseUserId")
        ' Dim licUser As String = Made4Net.Shared.GetAppConfigNameValue("TaskManagerServiceConfig", "LicenseUserId", True)
        Dim licUser As String = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceSection,
                    Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceLicenseUserId) '' PWMS-817
        ret = True ' Made4Net.DataAccess.DataInterface.Disconnect(licUser)
        Return ret
    End Function

#End Region

End Class