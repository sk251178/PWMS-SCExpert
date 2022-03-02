Imports WMS.Logic
Imports Made4Net.Shared
Imports Made4Net.Shared.Conversion.Convert

Partial Public Class ContactManagement
    Inherits System.Web.UI.Page


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


#Region "CTor"

    Public Sub New()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "CreateContact"
                dr = ds.Tables(0).Rows(0)
                Dim oContact As New WMS.Logic.Contact()
                oContact.CreateContact(ReplaceDBNull(dr("contactid")), ReplaceDBNull(dr("street1")), ReplaceDBNull(dr("street2")), ReplaceDBNull(dr("city")), ReplaceDBNull(dr("state")), ReplaceDBNull(dr("zip")), ReplaceDBNull(dr("contact1name")), ReplaceDBNull(dr("contact2name")), _
                    ReplaceDBNull(dr("contact1phone")), ReplaceDBNull(dr("contact2phone")), ReplaceDBNull(dr("contact1fax")), ReplaceDBNull(dr("contact2fax")), ReplaceDBNull(dr("contact1email")), ReplaceDBNull(dr("contact2email")), ReplaceDBNull(dr("route")), _
                    ReplaceDBNull(dr("STAGINGLANE")), ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), ReplaceDBNull(dr("PICKUPCONFIRMATIONTYPE")), ReplaceDBNull(dr("DELIVERYCONFIRMATIONTYPE")), UserId)
                'oContact.SetContact("", ReplaceDBNull(dr("street1")), ReplaceDBNull(dr("street2")), ReplaceDBNull(dr("city")), ReplaceDBNull(dr("state")), ReplaceDBNull(dr("zip")), ReplaceDBNull(dr("contact1name")), ReplaceDBNull(dr("contact2name")), _
                '    ReplaceDBNull(dr("contact1phone")), ReplaceDBNull(dr("contact2phone")), ReplaceDBNull(dr("contact1fax")), ReplaceDBNull(dr("contact2fax")), ReplaceDBNull(dr("contact1email")), ReplaceDBNull(dr("contact2email")), ReplaceDBNull(dr("route")), _
                '    ReplaceDBNull(dr("STAGINGLANE")), ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), ReplaceDBNull(dr("PICKUPCONFIRMATIONTYPE")), ReplaceDBNull(dr("DELIVERYCONFIRMATIONTYPE")), UserId)
                Company.updateContact(ds.Tables(0))
            Case "EditContact"
                dr = ds.Tables(0).Rows(0)
                Dim oContact As New WMS.Logic.Contact()
                oContact.UpdateContact(ReplaceDBNull(dr("CONTACTID")), ReplaceDBNull(dr("street1")), ReplaceDBNull(dr("street2")), ReplaceDBNull(dr("city")), ReplaceDBNull(dr("state")), ReplaceDBNull(dr("zip")), ReplaceDBNull(dr("contact1name")), ReplaceDBNull(dr("contact2name")), _
                                    ReplaceDBNull(dr("contact1phone")), ReplaceDBNull(dr("contact2phone")), ReplaceDBNull(dr("contact1fax")), ReplaceDBNull(dr("contact2fax")), ReplaceDBNull(dr("contact1email")), ReplaceDBNull(dr("contact2email")), ReplaceDBNull(dr("route")), _
                                    ReplaceDBNull(dr("STAGINGLANE")), ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), ReplaceDBNull(dr("PICKUPCONFIRMATIONTYPE")), ReplaceDBNull(dr("DELIVERYCONFIRMATIONTYPE")), UserId)
                'oContact.SetContact(ReplaceDBNull(dr("CONTACTID")), ReplaceDBNull(dr("street1")), ReplaceDBNull(dr("street2")), ReplaceDBNull(dr("city")), ReplaceDBNull(dr("state")), ReplaceDBNull(dr("zip")), ReplaceDBNull(dr("contact1name")), ReplaceDBNull(dr("contact2name")), _
                '    ReplaceDBNull(dr("contact1phone")), ReplaceDBNull(dr("contact2phone")), ReplaceDBNull(dr("contact1fax")), ReplaceDBNull(dr("contact2fax")), ReplaceDBNull(dr("contact1email")), ReplaceDBNull(dr("contact2email")), ReplaceDBNull(dr("route")), _
                '   ReplaceDBNull(dr("STAGINGLANE")), ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), ReplaceDBNull(dr("PICKUPCONFIRMATIONTYPE")), ReplaceDBNull(dr("DELIVERYCONFIRMATIONTYPE")), UserId)
                Company.updateContact(ds.Tables(0))
            Case "AssignCompany"
                Dim oComp As WMS.Logic.Company
                For Each dr In ds.Tables(0).Rows
                    oComp = New WMS.Logic.Company(dr("CONSIGNEE"), dr("company"), dr("companytype"))
                    oComp.AddContact(Session("CONTACTID"), UserId)
                Next
            Case "UnAssignCompany"
                Dim oComp As WMS.Logic.Company
                For Each dr In ds.Tables(0).Rows
                    oComp = New WMS.Logic.Company(dr("CONSIGNEE"), dr("company"), dr("companytype"))
                    oComp.RemoveContact(Session("CONTACTID"), UserId)
                Next
        End Select
    End Sub


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub TEMasterContact_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterContact.CreatedChildControls
        With TEMasterContact.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.ContactManagement"
            If TEMasterContact.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "CreateContact"
            Else
                .CommandName = "EditContact"
            End If
        End With
    End Sub

    Private Sub TEAssignCompanies_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignCompanies.CreatedChildControls
        TEAssignCompanies.ActionBar.AddSpacer()
        TEAssignCompanies.ActionBar.AddExecButton("AssignCompany", "Assign Company", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEAssignCompanies.ActionBar.Button("AssignCompany")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.ContactManagement"
        End With
    End Sub

    Private Sub TECompanies_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECompanies.CreatedChildControls
        TECompanies.ActionBar.AddSpacer()
        TECompanies.ActionBar.AddExecButton("UnAssignCompany", "UnAssign Company", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TECompanies.ActionBar.Button("UnAssignCompany")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.ContactManagement"
        End With
    End Sub

    Private Sub TEMasterContact_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMasterContact.RecordSelected
        Dim tds As DataTable = TEMasterContact.CreateDataTableForSelectedRecord(False)
        Dim vals As New Specialized.NameValueCollection
        vals.Add("CONTACTID", tds.Rows(0)("CONTACTID"))
        TEMasterContact.PreDefinedValues = vals
        Session("CONTACTID") = tds.Rows(0)("CONTACTID")
    End Sub

    'Private Sub TEContactRoutes_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEContactRoutes.CreatedChildControls

    'End Sub
End Class