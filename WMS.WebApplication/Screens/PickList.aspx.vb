Imports System.Text

Public Class PickList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEPicklistHeader As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEPicklistDetailPartail As Made4Net.WebControls.TableEditor
    'Added for RWMS-323
    Protected WithEvents TEPickingWeightCapture As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    'End Added for RWMS-323

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
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim t As New Made4Net.Shared.Translation.Translator()
        Select Case CommandName.ToLower
            'Added for RWMS-323
            Case "saveweight"
                For Each dr In ds.Tables(0).Rows
                    insert_ORDERLINECASEWEIGHT(dr)
                Next
            Case "editweight"
                'added for RWMS-488 (PWMS-364) Start
                edit_ORDERLINECASEWEIGHT(ds.Tables(0))
                'End for RWMS-488 (PWMS-364)
                'Commented for RWMS-488 (PWMS-364) Start
                'For Each dr In ds.Tables(0).Rows
                '    edit_ORDERLINECASEWEIGHT(dr)
                'Next
                'End Commented for RWMS-488 (PWMS-364)
            Case "deleteweight"
                For Each dr In ds.Tables(0).Rows
                    delete_ORDERLINECASEWEIGHT(dr)
                Next
                'End Added for RWMS-323

            Case "mergepicklists"
                Dim alPicklist As New ArrayList
                For Each dr In ds.Tables(0).Rows
                    alPicklist.Add(dr("picklist"))
                Next
                Dim newId As String = WMS.Logic.Picklist.MergeLists(alPicklist, UserId)
                If newId <> String.Empty Then
                    Dim col As New Made4Net.DataAccess.Collections.GenericCollection
                    col.Add("PICKLIST", newId)
                    Message = t.Translate("Picklist Created #PICKLIST#", col)
                Else
                    Message = t.Translate("No New Picklist Created.")
                End If
            Case "splitpicklistsuom"
                Dim oPicklist As WMS.Logic.Picklist
                For Each dr In ds.Tables(0).Rows
                    oPicklist = New WMS.Logic.Picklist(dr("picklist"))
                    Dim qty As Decimal = 0
                    Try
                        qty = Decimal.Parse(dr("splitinp"))
                    Catch ex As Exception
                        Throw New Made4Net.Shared.M4NException(New Exception(), "Invalid split by units", "Invalid split by units")
                    End Try
                    If qty <= 0 Then
                        Throw New Made4Net.Shared.M4NException(New Exception(), "Split by units must be greater than zero", "Split by units must be greater than zero")
                    End If
                    oPicklist.SplitListByUOM(qty, UserId)
                Next
                If ds.Tables(0).Rows.Count > 0 Then
                    Message = t.Translate("Picklist/s were split.")
                End If
            Case "splitpicklistscube"
                Dim oPicklist As WMS.Logic.Picklist
                For Each dr In ds.Tables(0).Rows
                    oPicklist = New WMS.Logic.Picklist(dr("picklist"))
                    Dim qty As Decimal = 0
                    Try
                        qty = Decimal.Parse(dr("splitinp"))
                    Catch ex As Exception
                        Throw New Made4Net.Shared.M4NException(New Exception(), "Invalid split by units", "Invalid split by units")
                    End Try
                    If qty <= 0 Then
                        Throw New Made4Net.Shared.M4NException(New Exception(), "Split by units must be greater than zero", "Split by units must be greater than zero")
                    End If
                    oPicklist.SplitListByCube(qty, UserId)
                Next
                If ds.Tables(0).Rows.Count > 0 Then
                    Message = t.Translate("Picklist/s were split.")
                End If
            Case "splitpicklistsline"
                Dim oPicklist As WMS.Logic.Picklist
                For Each dr In ds.Tables(0).Rows
                    oPicklist = New WMS.Logic.Picklist(dr("picklist"))
                    Dim qty As Integer = 0
                    Try
                        qty = Integer.Parse(dr("splitinp"))
                    Catch ex As Exception
                        Throw New Made4Net.Shared.M4NException(New Exception(), "Invalid split by units", "Invalid split by units")
                    End Try
                    If qty <= 0 Then
                        Throw New Made4Net.Shared.M4NException(New Exception(), "Split by units must be greater than zero", "Split by units must be greater than zero")
                    End If
                    oPicklist.SplitList(qty, UserId)
                Next
                If ds.Tables(0).Rows.Count > 0 Then
                    Message = t.Translate("Picklist/s were split.")
                End If

                'Added for RWMS-323
            Case "pick"
                For Each dr In ds.Tables(0).Rows
                    Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(dr("PICKLIST"))
                    Try
                        Dim dt As New DataTable
                        Made4Net.DataAccess.DataInterface.FillDataset(String.Format("select * from pickdetail where picklist='{0}'", dr("PICKLIST")), dt)

                        For Each dr1 As DataRow In dt.Rows
                            OutboundOrder.ApprovePicks(dr1)
                        Next

                    Catch ex As Exception
                    End Try
                Next
                'End Added for RWMS-323
            Case "approvepicks"
                'Added for RWMS-323
                For Each dr In ds.Tables(0).Rows
                    OutboundOrder.ApprovePicks(dr)
                Next
                'End Added for RWMS-323
                'Commented for RWMS-323
                'Dim oLoad As WMS.Logic.Load
                'For Each dr In ds.Tables(0).Rows
                '    Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(dr("PICKLIST"))

                '    Dim pd As New WMS.Logic.PicklistDetail(dr("PICKLIST"), dr("PICKLISTLINE"), True)

                '    If pd.Status = "COMPLETE" Then Continue For
                '    Dim oAttributeCollection As WMS.Logic.AttributesCollection
                '    Dim UNITS As Decimal = 0
                '    Try
                '        UNITS = Math.Round(Decimal.Parse(dr("UNITS")), 2)
                '        If UNITS < 0 Then Throw New Exception()
                '    Catch ex As Exception
                '        Throw New ApplicationException(("Invalid UNITS"))
                '        Exit Sub
                '    End Try

                '    Dim oSku As New WMS.Logic.SKU(dr("CONSIGNEE"), dr("SKU"))
                '    If UNITS <> 0 Then
                '        OutboundOrder.MainValidation(dr, oSku)
                '    End If

                '    If pd.Quantity - pd.PickedQuantity > UNITS Then
                '        OutboundOrder.PickShort(pd, pd.Quantity - pd.PickedQuantity, UNITS)
                '    End If
                '    Try
                '        oAttributeCollection = WMS.Logic.SkuClass.ExtractPickingAttributes(oSku.SKUClass, dr)
                '        Dim oPicking As WMS.Logic.Picking = New WMS.Logic.Picking
                '        oPicking.PickLine(oPickList, dr("PICKLISTLINE"), dr("UNITS"), UserId, oAttributeCollection)
                '    Catch ex As Made4Net.Shared.M4NException
                '        Throw ex
                '    Catch ex As Exception
                '    End Try
                'End Commented for RWMS-323
                'Dim oSku As New WMS.Logic.SKU(dr("CONSIGNEE"), dr("SKU"))

                'If (OutboundOrder.weightNeeded(oSku)) Then
                '    If Not IsDBNull(dr("WEIGHT")) And Not IsDBNull(dr("UNITS")) Then

                '        Dim UNITSPERCASE As Decimal = Decimal.Parse(dr("UNITS")) / oSku.ConvertToUnits("CASE")

                '        UNITSPERCASE = Decimal.Parse(dr("UNITS"))

                '        Dim weight As Decimal = Math.Round(Decimal.Parse(dr("WEIGHT")), 2)

                '        Dim weightPerCase As Decimal = Math.Round(weight / UNITSPERCASE, 2)

                '        OutboundOrder.VALIDATEWEIGHT(dr("CONSIGNEE"), dr("SKU"), weightPerCase)

                '        Message = OutboundOrder.updateFromLoadWeight(dr("fromload"), weight)
                '    Else
                '        Throw New ApplicationException(t.Translate("please fill Units and Weight"))
                '        Exit Sub
                '    End If
                'End If

                'Commented for RWMS-323
                'Next
                'End Commented for RWMS-323
                '  Dim PICK As New WMS.Logic.Picking(Sender, CommandName, XMLSchema, XMLData, Message)
        End Select
    End Sub
    'Added for RWMS-323
    Public Sub insert_ORDERLINECASEWEIGHT(ByVal dr As DataRow)
        Dim sql As String
        Dim wgtVal As New RWMS.Logic.WeightValidator

        Dim dtData As New DataTable
        sql = "select distinct CONSIGNEE, ORDERID, ORDERLINE, SKU, LOADUOM from vORDERLINECASEWEIGHTDetail where PICKLIST ='{0}' AND PICKLISTLINE = '{1}'"
        sql = String.Format(sql, dr("picklist"), dr("picklistline"))
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dtData)
        'test code
        Dim drDet As DataRow
        If dtData IsNot Nothing AndAlso dtData.Rows.Count > 0 Then
            drDet = dtData.Rows(0)

            'original code
            Dim oSku As New WMS.Logic.SKU(drDet("CONSIGNEE"), drDet("SKU"))
            'futhjfg
            Dim gotoOverride As Boolean = False
            Dim gotoOverrideMessage As String = ""
            Dim errMsg As String = ""
            Dim WEIGHT As String = dr("UOMWEIGHT").ToString
            Dim UNITS As Decimal = 1
            UNITS = oSku.ConvertToUnits(drDet("LOADUOM")) * UNITS

            If Not wgtVal.ValidateWeightSku(oSku, WEIGHT, UNITS, gotoOverride, gotoOverrideMessage, errMsg, False) Then
                If gotoOverride Then
                    dr("UOMWEIGHT") = WEIGHT
                    Throw New ApplicationException(errMsg & gotoOverrideMessage)
                Else
                    Throw New ApplicationException(errMsg)
                End If

                Exit Sub
            Else
                dr("UOMWEIGHT") = WEIGHT
            End If
            sql = "insert into ORDERLINECASEWEIGHT(Consignee, ORDERID, ORDERLINE, UOMWEIGHT)"
            sql = sql & String.Format("SELECT w.CONSIGNEE, w.ORDERID, w.ORDERLINE, '{0}' FROM dbo.OUTBOUNDORDETAIL AS w INNER JOIN dbo.PICKDETAIL AS d ON w.ORDERID = d.ORDERID AND w.ORDERLINE = d.ORDERLINE AND w.CONSIGNEE = d.CONSIGNEE WHERE (d.PICKLIST ='{1}') AND (d.PICKLISTLINE = '{2}')", dr("UOMWEIGHT"), dr("picklist"), dr("picklistline"))
            Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            'end original code

        Else
            'not inserted
            'message that 'picklist line doesn't require weight'
        End If
        'end test code


    End Sub



    Public Sub edit_ORDERLINECASEWEIGHT(ByVal dr As DataRow)
        Dim sql As String
        Dim wgtVal As New RWMS.Logic.WeightValidator

        Dim oSku As New WMS.Logic.SKU(dr("CONSIGNEE"), dr("SKU"))

        Dim gotoOverride As Boolean = False
        Dim gotoOverrideMessage As String = ""
        Dim errMsg As String = ""
        Dim WEIGHT As String = dr("UOMWEIGHT").ToString
        Dim UNITS As Decimal = 1
        UNITS = oSku.ConvertToUnits(dr("LOADUOM")) * UNITS

        If Not wgtVal.ValidateWeightSku(oSku, WEIGHT, UNITS, gotoOverride, gotoOverrideMessage, errMsg, False) Then
            If gotoOverride Then
                dr("UOMWEIGHT") = WEIGHT
                Throw New ApplicationException(errMsg & gotoOverrideMessage)
            Else
                Throw New ApplicationException(errMsg)
            End If

            Exit Sub
        Else
            dr("UOMWEIGHT") = WEIGHT
        End If

        sql = "update ORDERLINECASEWEIGHT set UOMWEIGHT='{0}' where id='{1}'"
        sql = String.Format(sql, dr("UOMWEIGHT"), dr("id"))
        Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
    End Sub

    'Added for RWMS-488(Retrofit PWMS-364) Start
    ''' <summary>
    ''' JIRA Item: RWMS-673
    ''' Description: Modified the below method to Update the WEIGHT in the ATTRIBUTE Table for the LOADID.
    ''' Added Generic List collection varaible to store the Loadid and get the loadid from the For Loop to update the WEIGHT column in the Attribute Table.
    ''' </summary>
    ''' <param name="dtWeights"></param>
    ''' <remarks></remarks>
    Public Sub edit_ORDERLINECASEWEIGHT(ByVal dtWeights As DataTable)
        Dim wgtVal As New RWMS.Logic.WeightValidator
        Dim dr As DataRow
        Dim blnValidationPass As Boolean = True
        Dim strValidationErrors As StringBuilder = New StringBuilder("Invalid case weight data " & vbCrLf)
        Dim attributeLoadList As New System.Collections.Generic.List(Of String)

        'Do weight validation for each case weight
        For Each dr In dtWeights.Rows

            Dim errMsg As String = ""
            If Not wgtVal.ValidateWeightNoSkuWtNeededCheck(dr("CONSIGNEE"), dr("SKU"), dr("UOMWEIGHT"), errMsg) Then
                blnValidationPass = False
                strValidationErrors.Append(" LOAD - " + dr("LOADID") + " UOMNUM - " + dr("UOMNUM") + " " + errMsg + vbCrLf)
            End If

        Next

        'If all weights have passed validation then update the caseweights
        If blnValidationPass Then
            Dim Sql As String = "UPDATE LOADDETWEIGHT SET UOMWEIGHT = {0}, EDITDATE = GETDATE(), EDITUSER = '{1}' WHERE LOADID = '{2}' AND UOMNUM = '{3}'"
            For Each dr In dtWeights.Rows
                Dim sql1 As String = String.Format(Sql, dr.Item("UOMWEIGHT"), WMS.Logic.Common.GetCurrentUser(), dr.Item("LOADID"), dr.Item("UOMNUM"))
                Made4Net.DataAccess.DataInterface.ExecuteScalar(sql1)

                If Not attributeLoadList.Contains(dr.Item("LOADID")) Then
                    attributeLoadList.Add(dr.Item("LOADID").ToString())
                End If
            Next

            For i As Integer = 0 To attributeLoadList.Count - 1
                Dim dtLoads As New DataTable
                Dim loadId As String = attributeLoadList(i)

                Dim sqlGetSumWeight As String = String.Format("SELECT SUM(UOMWEIGHT) AS 'SUMWEIGHT' FROM LOADDETWEIGHT WHERE LOADID='{0}' GROUP BY LOADID", loadId)
                Made4Net.DataAccess.DataInterface.FillDataset(sqlGetSumWeight, dtLoads)

                If dtLoads.Rows.Count > 0 Then
                    Dim sumWeight As Decimal = Convert.ToDecimal(dtLoads.Rows(0).Item("SUMWEIGHT"))
                    Dim sqlUpdateAttribute As String = String.Empty
                    If AttributeLoadExists(loadId) Then
                        sqlUpdateAttribute = String.Format("UPDATE ATTRIBUTE SET WEIGHT='{0}' WHERE PKEYTYPE ='LOAD' AND PKEY1 = '{1}'", dtLoads.Rows(0).Item("SUMWEIGHT"), loadId)
                    Else
                        sqlUpdateAttribute = String.Format("INSERT INTO ATTRIBUTE (PKEYTYPE,PKEY1,PKEY2,PKEY3,WEIGHT) VALUES ('LOAD','{1}',' ',' ',{0})", dtLoads.Rows(0).Item("SUMWEIGHT"), loadId)
                    End If
                    Made4Net.DataAccess.DataInterface.ExecuteScalar(sqlUpdateAttribute)
                End If
            Next
        Else
            Throw New ApplicationException(strValidationErrors.ToString())
        End If

    End Sub
    'End Added for RWMS-488(Retrofit PWMS-364)
    ''' <summary>
    ''' JIRA Item: RWMS-673
    ''' Description: Added the below method to check for the LOADID exists in Attribute Table.
    ''' Return the boolean value.
    ''' </summary>
    ''' <param name="loadid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AttributeLoadExists(ByVal loadid As String) As Boolean
        Dim SqlLoadExists As String
        Dim retVal As Integer = 0
        SqlLoadExists = String.Format("SELECT COUNT(1) FROM ATTRIBUTE WHERE PKEY1='{0}'", loadid)
        retVal = Made4Net.DataAccess.DataInterface.ExecuteScalar(SqlLoadExists)
        If retVal = 0 Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Sub delete_ORDERLINECASEWEIGHT(ByVal dr As DataRow)
        Dim sql As String
        sql = "delete ORDERLINECASEWEIGHT  where id='{0}'"
        sql = String.Format(sql, dr("id"))
        Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
    End Sub
    'End Added for RWMS-323

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            ViewState.Add("_SelectedPicklistType", Nothing)
        End If
    End Sub

    Private Sub TEPicklistDetailPartail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEPicklistDetailPartail.CreatedChildControls
        If ViewState("_SelectedPicklistType") = WMS.Lib.PICKTYPE.PARTIALPICK Then
            With TEPicklistDetailPartail
                With .ActionBar
                    .AddSpacer()

                    With .Button("Save")
                        .ObjectDLL = "WMS.Logic.dll"
                        .ObjectName = "WMS.Logic.Picklist"
                        .CommandName = "Save"
                    End With

                    .AddExecButton("ApprovePicks", "Approve Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
                    With .Button("ApprovePicks")
                        'Commented for RWMS-2780
                        ''UnCommented PWMS-836 for RWMS 898
                        '.ObjectDLL = "WMS.Logic.dll"
                        '.ObjectName = "WMS.Logic.Picking"
                        '.CommandName = "Pick"
                        ''End UnCommented PWMS-836 For RWMS 898
                        'Commented for RWMS-2780 END

                        '.ObjectDLL = "WMS.WebApp.dll"
                        '.ObjectName = "WMS.WebApp.PickList"
                        '.CommandName = "ApprovePicks"

                        'RWMS-2780
                        .ObjectDLL = "WMS.Logic.dll"
                        .ObjectName = "WMS.Logic.Picking"
                        .CommandName = "approvepicks"
                        'RWMS-2780 END

                        .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                        .ConfirmRequired = True
                        .ConfirmMessage = "Are you sure you want to pick the selected picks?"
                    End With

                    .AddExecButton("CancelPicks", "Cancel Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                    With .Button("CancelPicks")
                        .ObjectDLL = "WMS.Logic.dll"
                        .ObjectName = "WMS.Logic.Picklist"
                        .CommandName = "CancelPicks"
                        .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                        .ConfirmRequired = True
                        .ConfirmMessage = "Are you sure you want to cancel the selected picks?"
                    End With

                    .AddExecButton("unallocatePicks", "Unallocate Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarUnAllocatePicks"))
                    With .Button("unallocatePicks")
                        .ObjectDLL = "WMS.Logic.dll"
                        .ObjectName = "WMS.Logic.Picklist"
                        .CommandName = "unallocatePicks"
                        .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                        .ConfirmRequired = True
                        .ConfirmMessage = "Are you sure you want to unallocated the selected picks?"
                    End With
                    '.AddExecButton("unPick", "Undo Pick", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarUnAllocatePicks"))
                    'With .Button("unPick")
                    '    .ObjectDLL = "WMS.Logic.dll"
                    '    .ObjectName = "WMS.Logic.Picklist"
                    '    .CommandName = "unpick"
                    '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    '    .ConfirmRequired = True
                    '    .ConfirmMessage = "Are you sure you want to undo the selected picks?"
                    'End With
                    .AddSpacer()
                    .AddExecButton("PickShortCancelEx", "Pick Short & Cancel Exceptions", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
                    With .Button("PickShortCancelEx")
                        .ObjectDLL = "WMS.Logic.dll"
                        .ObjectName = "WMS.Logic.Picking"
                        .CommandName = "pickshortcancelexceptions"
                        .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                        .ConfirmRequired = True
                        .ConfirmMessage = "Are you sure you want to Pick Short And Cancel Exceptions for the selected picks?"
                    End With
                    .AddSpacer()
                    .AddExecButton("PickShortLeaveOpen", "Pick Short & Leave Open", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
                    With .Button("PickShortLeaveOpen")
                        .ObjectDLL = "WMS.Logic.dll"
                        .ObjectName = "WMS.Logic.Picking"
                        .CommandName = "pickshortleaveopen"
                        .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                        .ConfirmRequired = True
                        .ConfirmMessage = "Are you sure you want to Pick Short the selected picks?"
                    End With
                    .AddSpacer()
                    .AddExecButton("AllocateLoadForPick", "Allocate Load For Pick", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
                    With .Button("AllocateLoadForPick")
                        .ObjectDLL = "WMS.Logic.dll"
                        .ObjectName = "WMS.Logic.Picking"
                        .CommandName = "AllocateLoadForPickDetail"
                        .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                        .ConfirmRequired = True
                        .ConfirmMessage = "Are you sure you want to allocate loads for the selected picks?"
                    End With
                End With
            End With
        End If
    End Sub

    Private Sub TEPicklistHeader_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEPicklistHeader.CreatedChildControls
        With TEPicklistHeader
            With .ActionBar
                .AddSpacer()

                With .Button("Save")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Picklist"
                    .CommandName = "Save"
                End With

                .AddExecButton("ApprovePicklist", "Approve Picklist", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
                With .Button("ApprovePicklist")
                    'Commented PWMS-836 for RWMS-898
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Picking"
                    'UnCommented PWMS-836 for RWMS-898
                    .CommandName = "Pick"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to approve the selected picklists?"
                End With

                .AddExecButton("CancelPicklist", "Cancel Picklist", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelPicklist")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Picklist"
                    .CommandName = "CancelPicklist"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the selected picklist?"
                End With

                .AddExecButton("unallocatePicklist", "Unallocate Picklist", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarUnAllocatePicks"))
                With .Button("unallocatePicklist")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Picklist"
                    .CommandName = "unallocatePicklist"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to unallocated the selected picklists?"
                End With

                .AddExecButton("PrintPicklist", "Print Picklist", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintPicklist")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Picklist"
                    .CommandName = "PrintPicklist"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With

                '.AddExecButton("PrintShipLabels", "Print Ship Labels", Made4Net.WebControls.SkinManager.GetImageURL("BtnShipLabel"))
                'With .Button("PrintShipLabels")
                '    .ObjectDLL = "WMS.Logic.dll"
                '    .ObjectName = "WMS.Logic.Picklist"
                '    .CommandName = "PrintShipLabels"
                '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                'End With

                .AddExecButton("ReleaseOrder", "Release Picklist", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("ReleaseOrder")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Picklist"
                    .CommandName = "releaseorder"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to release the selected orders?"
                End With

                .AddSpacer()
                .AddExecButton("MergePickLists", "Merge Pick Lists", Made4Net.WebControls.SkinManager.GetImageURL("Receipt"))
                With .Button("MergePickLists")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PickList"
                    .CommandName = "MergePickLists"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to merge the selected lists?"
                End With

                .AddSpacer()
                .AddExecButton("SplitPickListsUOM", "Split Pick Lists By Quantity", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
                With .Button("SplitPickListsUOM")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PickList"
                    .CommandName = "SplitPickListsUOM"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to split the selected lists?"
                End With

                .AddExecButton("SplitPickListsCube", "Split Pick Lists By Cube", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
                With .Button("SplitPickListsCube")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PickList"
                    .CommandName = "SplitPickListsCube"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to split the selected lists?"
                End With

                .AddExecButton("SplitPickListsLine", "Split Pick Lists By Line Number", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
                With .Button("SplitPickListsLine")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PickList"
                    .CommandName = "SplitPickListsLine"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to split the selected lists?"
                End With
            End With
        End With
    End Sub

    Private Sub TEPicklistHeader_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEPicklistHeader.RecordSelected
        Dim tds As DataTable = TEPicklistHeader.CreateDataTableForSelectedRecord(False)
        Try
            ViewState("_SelectedPicklistType") = tds.Rows(0)("PICKTYPE")
        Catch ex As Exception

        End Try
        'Added for RWMS-488(Retrofit PWMS-364) Start
        'Added for Retrofit PWMS-573(RWMS-746) Start
        TEPicklistDetailPartail.RefreshData()
        TEPicklistDetailPartail.RefreshGridInlineValues()

        'Added for Retrofit PWMS-573(RWMS-746) End

        TEPickingWeightCapture.RefreshData()
        TEPickingWeightCapture.RefreshGridInlineValues()
        'End Added for RWMS-488(Retrofit PWMS-364)
    End Sub

    Private Sub TEPicklistHeader_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEPicklistHeader.AfterItemCommand
        'If e.CommandName = "Pick" Or e.CommandName = "CancelPicklist" Or e.CommandName = "unallocatePicklist" Then
        TEPicklistHeader.RefreshData()
        TEPicklistDetailPartail.RefreshData()
        'End If
    End Sub

    Private Sub TEPicklistDetailPartail_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEPicklistDetailPartail.AfterItemCommand
        If e.CommandName = "ApprovePicks" Or e.CommandName = "CancelPicks" Or e.CommandName = "unallocatePicks" Or e.CommandName = "unpick" Then
            TEPicklistHeader.RefreshData()
            TEPicklistDetailPartail.RefreshData()
        End If
    End Sub

    Private Sub TEPicklistHeader_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEPicklistHeader.CreatedGrid
        TEPicklistHeader.Grid.AddExecButton("printlabels", "Print Label", "WMS.Logic.dll", "WMS.Logic.Picklist", 4, Made4Net.WebControls.SkinManager.GetImageURL("LabelPrint"))
        TEPicklistHeader.Grid.AddExecButton("printshiplabels", "Print Ship Labels", "WMS.Logic.dll", "WMS.Logic.Picklist", 4, Made4Net.WebControls.SkinManager.GetImageURL("BtnShipLabel"))

    End Sub

    'Added for RWMS-323
    Private Sub TEPickingWeightCapture_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEPickingWeightCapture.CreatedChildControls
        With TEPickingWeightCapture
            With .ActionBar
                .AddSpacer()
                'Commented for RWMS-488(Retrofit PWMS-364) Start
                'With .Button("Save")
                '    If TEPickingWeightCapture.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                '        .CommandName = "SaveWeight"
                '    ElseIf TEPickingWeightCapture.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                '        .CommandName = "EditWeight"
                '    End If

                '    .ObjectDLL = "WMS.WebApp.dll"
                '    .ObjectName = "WMS.WebApp.PickList"

                'End With
                'End Commented for RWMS-488(Retrofit PWMS-364)

                'Added for RWMS-488(Retrofit PWMS-364) Start

                .AddExecButton("MultiLineEdit", "Multi Line Case Weight", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("MultiLineEdit")

                    .CommandName = "EditWeight"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PickList"

                End With
                'End Added for RWMS-488(Retrofit PWMS-364)

                With .Button("Delete")
                    .CommandName = "DeleteWeight"
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PickList"

                End With
            End With
        End With
    End Sub
    'End Added for RWMS-323
End Class