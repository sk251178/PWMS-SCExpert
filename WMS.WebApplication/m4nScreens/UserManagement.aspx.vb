Imports System.Collections.Generic
Imports System.Data.Odbc


Public Class UserManagement
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    <CLSCompliant(False)> Protected WithEvents UserManager1 As WMS.WebCtrls.WebCtrls.UserManager
    <CLSCompliant(False)> Protected WithEvents Screen As WMS.WebCtrls.WebCtrls.Screen

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "assignusers"
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim oUser As New WMS.Logic.User(dr("Userid"))
                    If oUser.AssignedWarehouseArea(dr("wharea")) Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "User is already assigned to " + dr("wharea").ToString(), "User is already assigned to " + dr("wharea").ToString())
                    Else
                        oUser.AssignToWarehouseArea(dr("wharea"))
                    End If
                Next
            Case "unassignusers"
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim oUser As New WMS.Logic.User(dr("Userid"))
                    oUser.UnAssignFromWarehouseArea(dr("wharea"))
                Next
                'RWMS-2723   
            Case "saveuserprofile"
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim UP As New WMS.Logic.User(Sender, CommandName, XMLSchema, XMLData, Message)
                Next
                'RWMS-2723 
        End Select
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

End Class
