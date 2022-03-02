Imports System.ComponentModel
Imports System.Configuration.Install

Public Class ServiceInstaller

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent


    End Sub
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mServiceProcessInstaller = New System.ServiceProcess.ServiceProcessInstaller
        Me.serviceInstaller1 = New System.ServiceProcess.ServiceInstaller
        '
        'mServiceProcessInstaller
        '
        Me.mServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem
        Me.mServiceProcessInstaller.Password = Nothing
        Me.mServiceProcessInstaller.Username = Nothing
        '
        'serviceInstaller1
        '
        'Me.serviceInstaller1.ServiceName = "Expert Talkman Interface"
        'Me.serviceInstaller1.DisplayName = Made4Net.Shared.Util.GetServiceName(System.Reflection.Assembly.GetExecutingAssembly().Location)
        'Me.serviceInstaller1.ServiceName = Made4Net.Shared.Util.GetServiceName(System.Reflection.Assembly.GetExecutingAssembly().Location)
        Me.serviceInstaller1.DisplayName = Made4Net.Shared.Util.GetServiceName(Made4Net.Shared.ConfigurationSettingsConsts.ServiceInstancename, "PWMSVoiceConfig")
        Me.serviceInstaller1.ServiceName = Made4Net.Shared.Util.GetServiceName(Made4Net.Shared.ConfigurationSettingsConsts.ServiceInstancename, "PWMSVoiceConfig")
        Me.serviceInstaller1.ServicesDependedOn = New String() {"MSMQ"}
        Me.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Manual
        '
        'ServiceInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.mServiceProcessInstaller, Me.serviceInstaller1})

    End Sub

End Class
