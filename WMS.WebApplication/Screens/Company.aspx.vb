Public Class Company
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMaster As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDetail3 As Made4Net.WebControls.TableEditor
    Protected WithEvents DC3 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEDetail1 As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEAssignContacts As Made4Net.WebControls.TableEditor
    'Added for RWMS-1373 and RWMS-1263
    Protected WithEvents TableCustExpDays As Made4Net.WebControls.TableEditor
    'Ended for RWMS-1373 and RWMS-1263
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

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim ts As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim selectedCompanyCollection As Specialized.NameValueCollection = CType(Session("SelectedCompany"), Specialized.NameValueCollection)

        Dim comp As New WMS.Logic.Company(selectedCompanyCollection("Consignee"), selectedCompanyCollection("Company"), selectedCompanyCollection("CompanyType"))
        Select Case CommandName.ToLower
            Case "assigncontacts"
                For Each contactDr As DataRow In ds.Tables(0).Rows
                    comp.AddContact(contactDr("ContactID"), WMS.Logic.GetCurrentUser())
                Next
            Case "unassigncontacts"
                For Each contactDr As DataRow In ds.Tables(0).Rows
                    comp.RemoveContact(contactDr("ContactID"), WMS.Logic.GetCurrentUser())
                Next
            Case "createcontact", "updatecontact"
                Dim cont As New WMS.Logic.Company(Sender, CommandName, XMLSchema, XMLData, Message)
                updateContact(ds.Tables(0))
                'Added for RWMS-1373 and RWMS-1263
            Case "newcustexpdays"
                InsertCustExpDays(ds.Tables(0), selectedCompanyCollection)
            Case "editcustexpdays"
                updateCustExpDays(ds.Tables(0), selectedCompanyCollection)
            Case "deletecustexpdays"
                deleteCustExpDays(ds.Tables(0), selectedCompanyCollection)
                'Ended for RWMS-1373 and RWMS-1263
        End Select

    End Sub
    'Added for RWMS-1373 and RWMS-1263
    Public Shared Sub InsertCustExpDays(ByVal dt As DataTable, ByVal selectedCompanyCollection As Specialized.NameValueCollection)
        Try
            For Each dr As DataRow In dt.Rows

                Dim sql As String = String.Format("insert into CUSTOMEREXPIRATIONDAYS (CONSIGNEE, COMPANY, COMPANYTYPE, SKU, EXPIRATIONDAYS, ADDDATE, ADDUSER, EDITDATE, EDITUSER) values('{0}','{1}','{2}','{3}','{4}',getdate(),'{5}',getdate(),'{5}')", _
                selectedCompanyCollection("CONSIGNEE"), selectedCompanyCollection("COMPANY"), selectedCompanyCollection("COMPANYTYPE"), dr("SKU"), dr("EXPIRATIONDAYS"), WMS.Logic.GetCurrentUser)

                Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

            Next
        Catch ex As Exception
        End Try
    End Sub

    Public Shared Sub updateCustExpDays(ByVal dt As DataTable, ByVal selectedCompanyCollection As Specialized.NameValueCollection)
        Try
            For Each dr As DataRow In dt.Rows
                Dim sql As String = String.Format("update CUSTOMEREXPIRATIONDAYS set EXPIRATIONDAYS='{0}', EDITDATE=GETDATE(), EDITUSER='{1}' FROM CUSTOMEREXPIRATIONDAYS where CONSIGNEE='{2}' and COMPANY='{3}' and COMPANYTYPE='{4}' and SKU='{5}'", dr("EXPIRATIONDAYS"), WMS.Logic.GetCurrentUser, selectedCompanyCollection("CONSIGNEE"), selectedCompanyCollection("COMPANY"), selectedCompanyCollection("COMPANYTYPE"), dr("SKU"))
                Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            Next
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Sub deleteCustExpDays(ByVal dt As DataTable, ByVal selectedCompanyCollection As Specialized.NameValueCollection)
        Try
            For Each dr As DataRow In dt.Rows
                Dim sql As String = String.Format("delete CUSTOMEREXPIRATIONDAYS where CONSIGNEE='{0}' and COMPANY='{1}' and COMPANYTYPE='{2}' and SKU='{3}'", selectedCompanyCollection("CONSIGNEE"), selectedCompanyCollection("COMPANY"), selectedCompanyCollection("COMPANYTYPE"), dr("SKU"))
                Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            Next
        Catch ex As Exception
        End Try
    End Sub
    'Ended for RWMS-1373 and RWMS-1263
    Public Shared Sub updateContact(ByVal dt As DataTable)
        Try
            For Each contactDr As DataRow In dt.Rows
                If Not IsDBNull(contactDr("WEEKENDROUTE")) Then
                    Dim sql As String = String.Format("update contact set WEEKENDROUTE = '{0}' where contactid='{1}'", contactDr("WEEKENDROUTE"), contactDr("ContactID"))
                    Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TEMaster_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMaster.CreatedChildControls
        With TEMaster.ActionBar.Button("Save")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.Company"
            If TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "NewCompany"
            ElseIf TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "EditCompany"
            End If
        End With
    End Sub

    Private Sub TEDetail3_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDetail3.CreatedChildControls
        With TEDetail3.ActionBar.Button("Save")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.Company"
            .CommandName = "SetDelivery"
        End With
    End Sub

    Private Sub TEDetail1_CreatedChildControls1(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDetail1.CreatedChildControls
        With TEDetail1.ActionBar.Button("Save")
            '.ObjectDLL = "WMS.Logic.dll"
            '.ObjectName = "WMS.Logic.Company"
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Company"
            If TEDetail1.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "CreateContact"
            ElseIf TEDetail1.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "UpdateContact"
            End If
            '.CommandName = "SetContact"
        End With
        With TEDetail1.ActionBar.Button("Delete")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.Company"
            .CommandName = "RemoveContact"
        End With
        TEDetail1.ActionBar.AddSpacer()
        TEDetail1.ActionBar.AddExecButton("SetDefaultContact", "Set Default Contact", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        With TEDetail1.ActionBar.Button("SetDefaultContact")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .CommandName = "SetDefaultContact"
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.Company"
        End With

        TEDetail1.ActionBar.AddSpacer()
        TEDetail1.ActionBar.AddExecButton("UnassignContacts", "Unassign Contacts", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCloseReceipt"))
        With TEDetail1.ActionBar.Button("UnassignContacts")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .CommandName = "UnassignContacts"
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Company"
        End With
    End Sub

    Protected Sub TEAssignContacts_CreatedChildControls1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEAssignContacts.CreatedChildControls
        TEAssignContacts.ActionBar.AddExecButton("AssignContacts", "Assign Contacts", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        With TEAssignContacts.ActionBar.Button("AssignContacts")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .CommandName = "AssignContacts"
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Company"
        End With

    End Sub

    Protected Sub TEMaster_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMaster.RecordSelected
        Dim tds As DataTable = TEMaster.CreateDataTableForSelectedRecord()
        Dim vals As New Specialized.NameValueCollection
        vals.Clear()
        vals.Add("Consignee", tds.Rows(0)("CONSIGNEE"))
        vals.Add("Company", tds.Rows(0)("Company"))
        vals.Add("CompanyType", tds.Rows(0)("CompanyType"))
        Session.Add("SelectedCompany", vals)


    End Sub

    Private Sub TEMaster_RecordUnSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMaster.RecordUnSelected
        Session.Remove("SelectedCompany")
    End Sub
    'Added for RWMS-1373 and RWMS-1263
    Private Sub TableCustExpDays_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TableCustExpDays.CreatedChildControls
        With TableCustExpDays.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Company"

            If TableCustExpDays.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "NewCustExpDays"
            ElseIf TableCustExpDays.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "EditCustExpDays"
            End If
        End With
        With TableCustExpDays.ActionBar.Button("Delete")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Company"
            .CommandName = "DeleteCustExpDays"
        End With
    End Sub
    'Ended for RWMS-1373 and RWMS-1263
End Class
