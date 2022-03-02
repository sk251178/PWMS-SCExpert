Imports Made4Net.Net.TCPIP
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports System.Data
Imports Made4Net.Shared.Web
Imports Made4Net.WebControls

Public Class LinManScr
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents pnlAppInfo As Made4Net.WebControls.Panel
    Protected WithEvents TEAdmPnl As Made4Net.WebControls.TableEditor
    Protected uDt As DataTable

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "disconnect"
                For Each dr In ds.Tables(0).Rows
                    Try
                        Dim userid, appId, sessionid, ipAddress, conn As String
                        appId = dr("APPID")
                        userid = dr("USERID")
                        sessionid = dr("SESSIONID")
                        ipAddress = dr("IPADDR")
                        conn = Made4Net.DataAccess.DataInterface.ConnectionName
                        Dim Session_Id As String
                        If ipAddress = "" Or ipAddress Is Nothing Then
                            Session_Id = sessionid
                        Else
                            Session_Id = ipAddress & "_" & sessionid
                        End If

                        Dim key As String = "DisConnect" & "@" & userid & "@" & Session_Id & "@" & appId & "@" & conn
                        Dim SQL As String = "select param_value from sys_param where param_code = 'LicenseServer'"
                        Dim server As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
                        SQL = "SELECT PARAM_VALUE FROM SYS_PARAM WHERE PARAM_CODE = 'LicenseServerPort'"
                        Dim port As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
                        Dim tcpClient As New Made4Net.Net.TCPIP.Client(server, port)
                        Dim ret As Boolean = Convert.ToBoolean(tcpClient.SendRequest(key))

                    Catch ex As Exception

                    End Try
                Next
                Message = "Users disconnected."
        End Select
    End Sub

#End Region

#Region "Methods"

    Public Sub GetConnectedUsersTbl()
        Dim tcpClient As New Made4Net.Net.TCPIP.Client(Made4Net.Shared.Util.GetSystemParameterValue("LicenseServer"), Made4Net.Shared.Util.GetSystemParameterValue("LicenseServerPort"))
        Dim ret As String = tcpClient.SendRequest("GETUSERTBL")
        uDt = New DataTable        
        Dim fdt As DataTable = New DataTable

        Try
            uDt = BuildDataTable()
            fdt = BuildDataTable()

            ret = ret.Trim().TrimEnd("#")
            Dim LogInStr As String() = ret.Split("~")

            For Each str As String In LogInStr
                Dim sStr As String() = str.Split("$")
                Dim dr As DataRow
                Try
                    dr = uDt.NewRow()
                    dr.Item("USERID") = sStr(2)

                    If sStr(1).IndexOf("_") > -1 Then
                        dr.Item("IPADDR") = sStr(1).Split("_")(0)
                        dr.Item("SESSIONID") = sStr(1).Split("_")(1)
                    Else
                        dr.Item("IPADDR") = ""
                        dr.Item("SESSIONID") = sStr(1)
                    End If

                    dr.Item("APPID") = sStr(0)
                    uDt.Rows.Add(dr)
                Catch ex As Exception

                End Try
            Next

            Try
                If Not TEAdmPnl.Grid.GetSelectedRecords Is Nothing And TEAdmPnl.Grid.GetSelectedRecords.Count > 0 Then
                    Dim wherecond As String = "USERID IN ("
                    For i As Int32 = 0 To TEAdmPnl.Grid.GetSelectedRecords.Count - 1
                        wherecond = wherecond & "'" & TEAdmPnl.Grid.GetSelectedRecords(i)(0) & "',"
                    Next
                    wherecond = wherecond.TrimEnd(",") & ")"
                    'Dim wherecond As String
                    'For i As Int32 = 0 To TEAdmPnl.Grid.GetSelectedRecords.Count - 1
                    '    wherecond = wherecond & " (USERID = '" & TEAdmPnl.Grid.GetSelectedRecords(i)(0) & "' AND SESSIONID='" & TEAdmPnl.Grid.GetSelectedRecords(i)(2) & "') OR"
                    'Next
                    'wherecond = wherecond.TrimEnd("OR".ToCharArray)

                    Dim drarr As DataRow() = uDt.Select(wherecond)

                    For j As Int32 = 0 To drarr.Length - 1
                        Dim r As DataRow
                        r = fdt.NewRow()
                        r.Item("USERID") = drarr(j)(0)
                        r.Item("IPADDR") = drarr(j)(1)
                        r.Item("SESSIONID") = drarr(j)(2)
                        r.Item("APPID") = drarr(j)(3)
                        fdt.Rows.Add(r)
                    Next

                    uDt = fdt
                End If
            Catch ex As Exception

            End Try

        Catch ex As Exception

        End Try
    End Sub

    Public Function BuildDataTable() As DataTable
        Dim dt As New DataTable
        Dim keys(3) As DataColumn

        Dim USERID As DataColumn = New DataColumn("USERID")
        USERID.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(USERID)
        keys(0) = USERID
        Dim IPADDR As DataColumn = New DataColumn("IPADDR")
        IPADDR.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(IPADDR)
        keys(1) = IPADDR
        Dim SESSIONID As DataColumn = New DataColumn("SESSIONID")
        SESSIONID.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(SESSIONID)
        keys(2) = SESSIONID
        Dim APPID As DataColumn = New DataColumn("APPID")
        APPID.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(APPID)
        keys(3) = APPID
        dt.PrimaryKey = keys
        Return dt
    End Function

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetConnectedUsersTbl()
    End Sub

    Private Sub TEAdmPnl_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAdmPnl.CreatedGrid
        AddHandler TEAdmPnl.Grid.DataCreated, AddressOf TEAdmPnlDataCreated
    End Sub

    Sub TEAdmPnlDataCreated(ByVal source As Object, ByVal e As Made4Net.WebControls.DataGridDataCreatedEventArgs)
        Try
            GetConnectedUsersTbl()
            e.Data = uDt
        Catch ex As Exception
        End Try
    End Sub

    Private Sub TEAdmPnl_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAdmPnl.CreatedChildControls
        With TEAdmPnl
            With .ActionBar
                .AddExecButton("Disconnect", "Disconnect", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
                With .Button("Disconnect")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.LinManScr"
                    .CommandName = "Disconnect"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
            End With
        End With
    End Sub

    Private Sub TEAdmPnl_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEAdmPnl.AfterItemCommand
        TEAdmPnl.Restart()
    End Sub

End Class
