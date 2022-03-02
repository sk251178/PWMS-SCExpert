Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert

Partial Public Class ShiftMaster
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
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


#Region "Ctors"
    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Try
            Dim ds As New DataSet
            Dim UserId As String = Common.GetCurrentUser()
            ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
            For Each dr As DataRow In ds.Tables(0).Rows
                Select Case CommandName.ToLower
                    Case "createshiftinstance"
                        Dim shiftcode As String = ReplaceDBNull(dr("SHIFTCODE"))
                        Dim shiftday As String = ReplaceDBNull(dr("SHIFTDAY"))
                        Dim sql As String = String.Format("select top 1 isnull(WAREHOUSEAREA,'') WAREHOUSEAREA from SHIFTMASTER where SHIFTCODE='{0}' ", _
                                                    shiftcode)
                        Dim warehousearea As String = DataInterface.ExecuteScalar(sql)

                        If shiftcode <> String.Empty Then
                            Dim oShift As New WMS.Logic.Shift
                            Try
                                oShift.CreateInstance(shiftcode, warehousearea, shiftday)
                                Message = "Shift Instance created successfully."
                            Catch ex As Exception
                                Throw ex
                            End Try
                        End If
                    Case "assignusertoshift"
                        Dim MasterShiftCode As String = dr("MASTERSHIFTCODE")

                        DataInterface.RunSQL(String.Format("IF EXISTS(select * from USERWAREHOUSE where USERID='{2}') update USERWAREHOUSE set DEFAULTSHIFT='{0}' where WAREHOUSE='{1}' and USERID='{2}' ELSE insert into USERWAREHOUSE([USERID], [WAREHOUSE], [DEFAULTWHAREA], [DEFAULTCONSIGNEE], [DEFAULTSHIFT]) values('{2}', '{1}', '{1}', NULL, '{0}'); ", _
                            MasterShiftCode, Warehouse.CurrentWarehouse, dr("USERID")), Made4Net.Schema.CONNECTION_NAME)
                        Message = "Users successfully assigned to Shift Master."
                    Case "deassignuserfromshift"
                        DataInterface.RunSQL(String.Format("update USERWAREHOUSE set DEFAULTSHIFT='' where WAREHOUSE='{0}' and USERID='{1}'", _
                            Warehouse.CurrentWarehouse, dr("USERID")), Made4Net.Schema.CONNECTION_NAME)
                        Message = "Users successfully deassigned from Shift Master."
                    Case "removeshift"
                        ' RWMS-2126
                        Dim shiftcode As String = ReplaceDBNull(dr("SHIFTCODE"))
                        ' RWMS-2128: Check No user is logged in 
                        Dim sql As String = String.Format("select count(*) from WHACTIVITY where Shift = '{0}'", shiftcode)
                        Dim numberOfUserLoggedIn As Integer = DataInterface.ExecuteScalar(sql)
                        If numberOfUserLoggedIn > 0 Then
                            Throw New Exception("Cannot delete shift as there are users logged-in this shift.")
                        End If
                        ' Check no user assigned
                        sql = String.Format("Select count(*) from USERWAREHOUSE where DEFAULTSHIFT = '{0}'", shiftcode)
                        Dim numberOfUsersAssigned As Integer = DataInterface.ExecuteScalar(sql, Made4Net.Schema.CONNECTION_NAME)
                        If numberOfUsersAssigned = 0 Then
                            DataInterface.BeginTransaction()
                            Try
                                sql = String.Format("delete from SHIFTDETAIL where SHIFTCODE = '{0}'", shiftcode)
                                DataInterface.RunSQL(sql)
                                sql = String.Format("delete from SHIFTMASTER where SHIFTCODE = '{0}'", shiftcode)
                                DataInterface.RunSQL(sql)
                                DataInterface.CommitTransaction()
                            Catch ex As Exception
                                DataInterface.RollBackTransaction()
                                Throw ex
                            End Try
                        Else
                            Throw New Exception("Cannot delete shift as there are active users.")
                        End If
                        ' RWMS-2126
                End Select
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'RWMS-1713
    'Private Sub TEShiftDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEShiftDetail.CreatedChildControls
    '    With TEShiftDetail.ActionBar
    '        .AddSpacer()
    '        .AddExecButton("CreateShiftInstance", "Create Shift Instance", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
    '        With .Button("CreateShiftInstance")
    '            .ObjectDLL = "WMS.WebApp.dll"
    '            .ObjectName = "WMS.WebApp.ShiftMaster"
    '            .CommandName = "CreateShiftInstance"
    '            .ConfirmMessage = "Do you want create Instance of Shift?"
    '            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
    '        End With
    '    End With
    'End Sub
    'RWMS-1713
    Private Sub TEUserDefShift_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUserDefShift.CreatedChildControls
        With TEUserDefShift.ActionBar
            .AddSpacer()
            .AddExecButton("DeAssignUserfromShift", "DeAssign User from Shift", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarReplLoc"))
            With .Button("DeAssignUserfromShift")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ShiftMaster"
                .CommandName = "DeAssignUserfromShift"
                .ConfirmMessage = "Do you want deassign User from Shift?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

        End With
    End Sub

    Private Sub TEUserDefNotInShift_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUserDefNotInShift.CreatedChildControls
        With TEUserDefNotInShift.ActionBar
            .AddSpacer()
            .AddExecButton("AssignUsertoSelectedShift", "Assign User to Selected Shift", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("AssignUsertoSelectedShift")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ShiftMaster"
                .CommandName = "AssignUsertoShift"
                .ConfirmMessage = "Do you want assign User to Shift?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
        End With
    End Sub

    Private Sub TEShiftMaster_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEShiftMaster.RecordSelected
        Dim tds As DataTable = TEShiftMaster.CreateDataTableForSelectedRecord()
        Dim vals As New Specialized.NameValueCollection
        vals.Clear()
        vals.Add("MASTERSHIFTCODE", tds.Rows(0)("SHIFTCODE"))
        TEUserDefShift.PreDefinedValues = vals

        TEUserDefNotInShift.FilterExpression = String.Format("(Defaultshift <> '{1}' or Defaultshift is null)", Warehouse.CurrentWarehouse, tds.Rows(0)("SHIFTCODE"))
        TEUserDefNotInShift.PreDefinedValues = vals
    End Sub

    Private Sub TEUserDefShift_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUserDefShift.CreatedGrid
        TEUserDefShift.FilterExpression = "WAREHOUSE='" & Warehouse.CurrentWarehouse & "'"

    End Sub
    ' RWMS-2126
    Private Sub TEShiftMaster_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEShiftMaster.CreatedChildControls
        If TEShiftMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
            With TEShiftMaster.ActionBar.Button("Delete")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ShiftMaster"
                .CommandName = "RemoveShift"
                .ConfirmMessage = "Are you sure you want to delete this?"
            End With
        End If
    End Sub
    ' RWMS-2126
#End Region


End Class