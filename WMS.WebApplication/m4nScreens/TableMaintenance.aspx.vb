
Public Class TableMaintenance
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TE As Made4Net.WebControls.TableEditor
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents ddSysTable As Made4Net.WebControls.DropDownListValidated
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListValidated1 As Made4Net.WebControls.DropDownListValidated
    Protected WithEvents ddAppTable As Made4Net.WebControls.DropDownListValidated

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            SetSysTableList()
            SetAppTableList()
        End If
    End Sub

    Private Sub SetSysTableList()
        Dim adp As New Made4Net.DataAccess.Schema.SchemaAdapter(Made4Net.Schema.CONNECTION_NAME)
        Dim DT As DataTable = adp.GetTableList()
        Dim DR As DataRow
        With ddSysTable
            .AllOption = True
            .Items.Add("")
            For Each DR In DT.Rows
                If Not DR(1) = "dtproperties" Then
                    .Items.Add(DR(1))
                End If
            Next
        End With

    End Sub

    Private Sub SetAppTableList()
        Dim adp As New Made4Net.DataAccess.Schema.SchemaAdapter("Default")
        Dim DT As DataTable = adp.GetTableList()
        Dim DR As DataRow
        With ddAppTable
            .AllOption = True
            .Items.Add("")
            For Each DR In DT.Rows
                If Not DR(1) = "dtproperties" Then
                    .Items.Add(DR(1))
                End If
            Next
        End With

    End Sub

    Private Sub ddSysTable_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddSysTable.SelectedIndexChanged
        ddAppTable.Value = ""
        Dim oconn As Made4Net.DataAccess.CachedConnection = New Made4Net.DataAccess.CachedConnection(Made4Net.Schema.CONNECTION_NAME)
        TE.ConnectionID = oconn.ConnId
        TE.ConnectionName = Made4Net.Schema.CONNECTION_NAME
        TE.TableName = ddSysTable.Value
        oconn = Nothing
        TE.Restart()
    End Sub

    Private Sub ddAppTable_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddAppTable.SelectedIndexChanged
        ddSysTable.Value = ""
        Dim oconn As Made4Net.DataAccess.CachedConnection
        If IsNothing(Made4Net.DataAccess.DataInterface.ConnectionName) OrElse Made4Net.DataAccess.DataInterface.ConnectionName.Equals("") Then
            oconn = New Made4Net.DataAccess.CachedConnection("Default")
        Else
            oconn = New Made4Net.DataAccess.CachedConnection(Made4Net.DataAccess.DataInterface.ConnectionName)
        End If
        TE.ConnectionID = oconn.ConnId
        TE.ConnectionName = oconn.ConnName
        TE.TableName = ddAppTable.Value
        oconn = Nothing
        TE.Restart()
    End Sub

    Private Sub TE_ErrorEvent(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorErrorEventArgs) Handles TE.ErrorEvent
        Screen1.Warn(e.Exception.Message)
        Throw e.Exception
    End Sub

End Class
