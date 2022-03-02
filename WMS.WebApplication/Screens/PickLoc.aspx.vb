Imports WMS.Logic

Public Class PickLoc
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMaster As Made4Net.WebControls.TableEditor

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


#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim ts As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "newpickloc"
                dr = ds.Tables(0).Rows(0)
                Dim pk As WMS.Logic.PickLoc = New WMS.Logic.PickLoc
                If Not dr.IsNull("LOCATION") Then pk.Location = dr.Item("LOCATION")
                If Not dr.IsNull("WAREHOUSEAREA") Then pk.Warehousearea = dr.Item("WAREHOUSEAREA")

                If Not dr.IsNull("CONSIGNEE") Then pk.Consignee = dr.Item("CONSIGNEE")
                If Not dr.IsNull("SKU") Then pk.SKU = dr.Item("SKU")
                'Started commented for PWMS-756
                'If Not dr.IsNull("NORMALREPLQTY") Then
                '    pk.NormalReplQty = dr.Item("NORMALREPLQTY")
                'Else
                '    pk.NormalReplQty = 0
                'End If
                'Ended Commented for PWMS-756

                'Started for PWMS-756
                If Not dr.IsNull("REPLQTY") Then
                    pk.ReplQty = dr.Item("REPLQTY")
                Else
                    pk.ReplQty = 0
                End If
                'Ended for PWMS-756
                'Started commented for PWMS-756
                'If Not dr.IsNull("MAXIMUMQTY") Then
                '    pk.MaximumQty = dr.Item("MAXIMUMQTY")
                'Else
                '    pk.MaximumQty = pk.SetPickLocMaxBySku()
                'End If
                'Ended Commented for PWMS-756

                'Started for PWMS-756
                If Not dr.IsNull("MAXREPLQTY") Then
                    pk.MaximumReplQty = dr.Item("MAXREPLQTY")
                Else
                    pk.MaximumReplQty = pk.SetPickLocMaxBySku()
                End If

                'Ended for PWMS-756
                'Started commented for PWMS-756
                'If Not dr.IsNull("NORMALREPLPOLICY") Then pk.NormalReplPolicy = dr.Item("NORMALREPLPOLICY")
                'Ended Commented for PWMS-756
                'Started for PWMS-756
                If Not dr.IsNull("REPLPOLICY") Then pk.ReplPolicy = dr.Item("REPLPOLICY")
                'Ended for PWMS-756

                If Not dr.IsNull("BATCHPICKLOCATION") Then
                    pk.BatchPickLocation = dr.Item("BATCHPICKLOCATION")
                Else
                    pk.BatchPickLocation = Boolean.FalseString
                End If

                pk.Create(UserId)
            Case "editpickloc"
                Dim skuchange As Boolean = False
                dr = ds.Tables(0).Rows(0)
                Dim pk As WMS.Logic.PickLoc = New WMS.Logic.PickLoc(dr.Item("LOCATION"), dr.Item("WAREHOUSEAREA"), dr.Item("CONSIGNEE"), dr.Item("SKU"))
                'If Not dr.IsNull("LOCATION") Then pk.Location = dr.Item("LOCATION")
                'If Not dr.IsNull("WAREHOUSEAREA") Then pk.Location = dr.Item("WAREHOUSEAREA")

                If Not dr.IsNull("CONSIGNEE") Then pk.Consignee = dr.Item("CONSIGNEE")
                If Not dr.IsNull("SKU") Then
                    If pk.SKU.ToLower <> Convert.ToString(dr.Item("SKU")).ToLower Then
                        skuchange = True
                    End If
                    pk.SKU = dr.Item("SKU")
                End If
                'Started commented for PWMS-756
                'If Not dr.IsNull("NORMALREPLQTY") Then
                '    pk.NormalReplQty = dr.Item("NORMALREPLQTY")
                'Else
                '    pk.NormalReplQty = 0
                'End If
                'Ended Commented for PWMS-756

                'Started for PWMS-756
                If Not dr.IsNull("REPLQTY") Then
                    pk.ReplQty = dr.Item("REPLQTY")
                Else
                    pk.ReplQty = 0
                End If
                'Ended for PWMS-756
                'Started commented for PWMS-756
                'If Not dr.IsNull("MAXIMUMQTY") Then
                '    pk.MaximumQty = dr.Item("MAXIMUMQTY")
                'End If
                'Ended Commented for PWMS-756

                'Started for PWMS-756
                If Not dr.IsNull("MAXREPLQTY") Then
                    pk.MaximumReplQty = dr.Item("MAXREPLQTY")
                Else
                    pk.MaximumReplQty = pk.SetPickLocMaxBySku()
                End If
                'Ended for PWMS-756

                'Started commented for PWMS-756
                'If Not dr.IsNull("NORMALREPLPOLICY") Then pk.NormalReplPolicy = dr.Item("NORMALREPLPOLICY")
                'Ended Commented for PWMS-756

                'Started for PWMS-756
                If Not dr.IsNull("REPLPOLICY") Then pk.ReplPolicy = dr.Item("REPLPOLICY")
                'Ended for PWMS-756
                If skuchange Then
                    pk.MaximumQty = pk.SetPickLocMaxBySku()
                End If

                If Not dr.IsNull("BATCHPICKLOCATION") Then
                    pk.BatchPickLocation = dr.Item("BATCHPICKLOCATION")
                Else
                    pk.BatchPickLocation = Boolean.FalseString
                End If

                pk.Update(UserId)
                'Added for RWMS-2510 Start
            Case "deletepickloc"
                DeletePickLoc(ds.Tables(0))
                'Added for RWMS-2510 End
        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEMaster_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMaster.CreatedChildControls
        With TEMaster
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.PickLoc"
                If TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "NewPickLoc"
                ElseIf TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "EditPickLoc"
                End If
            End With
            'Added for RWMS-2510 Start
            With .ActionBar.Button("Delete")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.PickLoc"
                If TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "DeletePickLoc"
                    .CustomConfirmValidationMessage = "Are you sure?, Are you sure. There is inventory for this SKU?"
                    .CustomConfirmValidationRequired = True
                    .Script = "<script language=""JavaScript"">function m4nButton_CustomConfirm(t){var msgs = t.split(','); if(document.getElementById('TEMaster_re_Form_field_Inventory').value > 0 ) { return confirm(msgs[1]); } else { return confirm(msgs[0]); } }</script>"
                End If
            End With
            'Added for RWMS-2510 End
        End With
    End Sub

    'Added for RWMS-2510 Start
    Public Shared Sub DeletePickLoc(ByVal dt As DataTable)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        Dim t As New Made4Net.Shared.Translation.Translator()
        'Try
        Dim strErr As String
        'Dim oPckLoc As New WMS.Logic.PickLoc
        For Each dr As DataRow In dt.Rows
            strErr = ""
            Dim strPickLoc As String = dr("LOCATION")
            Dim oPickLoc As New WMS.Logic.PickLoc(dr("LOCATION"), dr("WAREHOUSEAREA"), dr("CONSIGNEE"), dr("SKU"))
            If Not oPickLoc.CheckCanDelete(strPickLoc, strErr) Then
                'TODO Display the error message
                Throw New ApplicationException(t.Translate(strErr))
            Else
                oPickLoc.Delete()

                Dim em As New EventManagerQ
                Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.PickLocationDeleted
                Dim activitystatus As String = WMS.Lib.Actions.Audit.PICKLOCDEL
                em.Add("EVENT", EventType)
                em.Add("ACTIVITYTYPE", activitystatus)
                em.Add("CONSIGNEE", dr("CONSIGNEE"))
                em.Add("WAREHOUSEAREA", dr("WAREHOUSEAREA"))
                em.Add("SKU", dr("SKU"))
                em.Add("FROMLOC", strPickLoc)
                em.Add("TOLOC", strPickLoc)
                em.Add("USERID", UserId)
                em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                em.Send(WMSEvents.EventDescription(EventType))
            End If
        Next
        'Catch ex As Exception
        '    'Return False
        'End Try

    End Sub
    'Added for RWMS-2510 End
End Class