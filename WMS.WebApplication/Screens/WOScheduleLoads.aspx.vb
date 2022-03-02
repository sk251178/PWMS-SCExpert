Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports made4net.Shared.Conversion.Convert
Imports Made4Net.Shared
Imports WMS.Logic
Imports WMS.Lib

Public Class WOScheduleLoads
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEWOScheduleLoads As Made4Net.WebControls.TableEditor

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


#Region "Ctor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "CreatePWTasks"
                CreatePWTasks(ds, UserId)
                Message = "Loads Putaway tasks were created."
        End Select
    End Sub

#End Region

#Region "Ctor Functions"

    Private Sub CreatePWTasks(ByVal ds As DataSet, ByVal UserId As String)
        Dim destLoc As String = ds.Tables(0).Rows(0)("Location")
        Dim destWarehousearea As String = ds.Tables(0).Rows(0)("Warehousearea")
        If destLoc = "" Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Destination Location Caanot be blank", "Destination Location Can not be blank")
        End If
        If destWarehousearea = "" Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Destination Warehousearea Caanot be blank", "Destination Warehousearea Can not be blank")
        End If
        For Each dr As DataRow In ds.Tables(0).Rows
            Dim oLoad As New WMS.Logic.Load(dr("loadid").ToString())
            oLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND, UserId)
            oLoad.SetDestinationLocation(destLoc, destWarehousearea, UserId)
            Dim tm As New TaskManager
            tm.CreatePutAwayTask(oLoad, UserId, False)
        Next
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEWOScheduleLoads_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEWOScheduleLoads.CreatedChildControls
        With TEWOScheduleLoads.ActionBar
            With .Button("Save")
                .CommandName = "CreatePWTasks"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WOScheduleLoads"
            End With
        End With
    End Sub

End Class
