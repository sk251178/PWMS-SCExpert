Imports Made4Net.Shared.Collections
Imports Made4Net.DataAccess
Imports System.Collections
Imports Made4Net.Mobile
Imports Made4Net.Shared.Web
Imports WMS.Logic
<CLSCompliant(False)> Public Class CNT
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Session.Remove("LoadCNTLoadId")
            Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("WAREHOUSEAREA")
            dd.AllOption = False
            dd.TableName = "WAREHOUSEAREA"
            dd.ValueField = "WAREHOUSEAREACODE"
            dd.TextField = "WAREHOUSEAREADESCRIPTION"
            dd.DataBind()
            Try
                dd.SelectedValue = Session("LoginWHArea")
            Catch ex As Exception
            End Try

        End If

        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("LOADID") = Session("SKUSEL_LOADID")
            DO1.Value("LOCATION") = Session("SKUSEL_LOCATION")
            DO1.Value("WAREHOUSEAREA") = Session("SKUSEL_WAREHOUSEAREA")
            DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")

            Session.Remove("SKUSEL_LOADID")
            Session.Remove("SKUSEL_LOCATION")
            Session.Remove("SKUSEL_WAREHOUSEAREA")
            Session.Remove("SKUSEL_CONSIGNEE")
            Session.Remove("SELECTEDSKU")
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("LoadCNTLoadId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub createNewLoadFromCounting()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If DO1.Value("SKU") = String.Empty OrElse DO1.Value("LOCATION") = String.Empty Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Please scan a SKU and Location"))
            Return
        End If

        'TOOK FROM LOCATION
        If Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select COUNT(1) from WAREHOUSEAREA where WAREHOUSEAREACODE = '{0}'", DO1.Value("WAREHOUSEAREA"))) = "0" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Warehouse area does not exist"))
            Return
        End If


        If Not WMS.Logic.Location.Exists(DO1.Value("LOCATION"), DO1.Value("WAREHOUSEAREA")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location does not exist"))
            DO1.Value("LOCATION") = ""
            DO1.FocusField = "LOCATION"
            Return
        End If

        ' Check for sku
        Dim sqlSelectSKU As String = "SELECT DISTINCT (vSC.CONSIGNEE+' '+ vSC.SKU) as SKU, case isnull(SKU.STATUS,0) when 0 then '-- ' else '' end + vSC.CONSIGNEE + ' ' + vSC.SKU + ' ' + SKUDESC AS DESCR FROM vSKUCODE vSC INNER JOIN SKU ON vSC.CONSIGNEE=SKU.CONSIGNEE AND vSC.SKU=SKU.SKU WHERE vSC.CONSIGNEE like '%" + DO1.Value("CONSIGNEE", Session("consigneeSession")) + "' AND (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')"
        Dim skusDT As New DataTable()
        Made4Net.DataAccess.DataInterface.FillDataset(sqlSelectSKU, skusDT)
        If skusDT.Rows.Count = 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("SKU does not exist"))
            DO1.Value("SKU") = ""
            DO1.FocusField = "SKU"
            Return
        Else
            If skusDT.Rows.Count = 1 Then
                DO1.Value("CONSIGNEE") = skusDT.Rows(0)("SKU").ToString().Split(" ")(0)
                DO1.Value("SKU") = skusDT.Rows(0)("SKU").ToString().Split(" ")(1)
            Else
                ' Go to Sku select screen
                Session("FROMSCREEN") = "CNT"
                'Session("SKUCODE") = DO1.Value("SKU").Trim
                ' Add all controls to session for restoring them when we back from that sreen
                'Session("SKUSEL_LOADID") = DO1.Value("LOADID").Trim
                Session("SKUSEL_SKUSDT") = skusDT
                Session("SKUSEL_LOCATION") = DO1.Value("LOCATION").Trim
                Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
            End If
        End If
        sqlSelectSKU = "SELECT count(1) FROM INVLOAD WHERE INVLOAD.CONSIGNEE = '" + DO1.Value("CONSIGNEE", Session("consigneeSession")) + "' AND SKU = '" & DO1.Value("SKU") & "' AND LOCATION ='" & DO1.Value("LOCATION") & "'"
        'RWMS-1247/RWMS-1333 Start
        Dim sqlLooseIDSql As String = "SELECT ISNULL(LOOSEID,0) LOOSEID FROM LOCATION WHERE LOCATION='" & DO1.Value("LOCATION") & "'"
        If CType(Made4Net.DataAccess.DataInterface.ExecuteScalar(sqlSelectSKU).ToString(), Integer) > 0 Then
            If Made4Net.DataAccess.DataInterface.ExecuteScalar(sqlLooseIDSql).ToString() = "True" Then
                'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("A load with entered sku exists in entered location"))
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("A load with entered sku exists in entered location.Please add quantity to exisiting load"))
                DO1.Value("LOCATION") = ""
                DO1.FocusField = "LOCATION"
                Return
            End If
        End If
        'RWMS-1247/RWMS-1333 End

        Session("CNTCreateLoadConsignee") = DO1.Value("CONSIGNEE", Session("consigneeSession"))
        Session("CNTCreateLoadSku") = DO1.Value("SKU")
        Session("CNTCreateLoadLocation") = DO1.Value("LOCATION")
        Session("CountMisSKUWarehouseArea") = DO1.Value("WAREHOUSEAREA")
        Response.Redirect(MapVirtualPath("Screens/CNTCreateLoad.aspx"))
    End Sub


    'Public Shared Function ConvertToDictionary(ByVal nvColl As Specialized.NameValueCollection) As IDictionary

    '    Dim results As Hashtable = New Hashtable(nvColl.Count)

    '    For Each key As String In nvColl

    '        results(key) = nvColl(key)

    '    Next

    '    Return results

    'End Function

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("LOCATION")
        DO1.AddDropDown("WAREHOUSEAREA")
        DO1.AddTextboxLine("CONSIGNEE", Nothing, "", False, Session("3PL"))
        DO1.AddTextboxLine("SKU")
        DO1.AddSpacer()
    End Sub


    Private Sub doNext()
        'If Page.IsValid Then
        ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
        If DO1.Value("LOADID").Trim = "" Then
            If DO1.Value("SKU").Trim <> "" Then
                ' Check for sku
                If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & DO1.Value("WAREHOUSEAREA").Trim & "' WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                    ' Go to Sku select screen
                    Session("FROMSCREEN") = "CNT"
                    Session("SKUCODE") = DO1.Value("SKU").Trim
                    ' Add all controls to session for restoring them when we back from that sreen
                    Session("SKUSEL_LOADID") = DO1.Value("LOADID").Trim
                    Session("SKUSEL_LOCATION") = DO1.Value("LOCATION").Trim
                    Session("SKUSEL_WAREHOUSEAREA") = DO1.Value("WAREHOUSEAREA").Trim
                    Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE", Session("consigneeSession")).Trim
                    Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
                ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & DO1.Value("WAREHOUSEAREA").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                    DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & DO1.Value("WAREHOUSEAREA").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'")
                End If
            End If
        End If

        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim dt As New DataTable
        Dim sql As String
        Dim LoadId, Loc, Warehousearea, Cons, Sku As String
        LoadId = DO1.Value("LOADID")
        Loc = DO1.Value("LOCATION")
        Warehousearea = DO1.Value("WAREHOUSEAREA")
        Cons = DO1.Value("CONSIGNEE", Session("consigneeSession"))
        Sku = DO1.Value("SKU")

        'sql = String.Format("SELECT LOADID FROM LOADS WHERE LOADID LIKE '%{0}' and location like '%{1}' and consignee like '{2}%' and sku like '{3}%'", LoadId, Loc, Cons, Sku)
        Select Case LoadFindType(LoadId, Loc, Warehousearea, Cons, Sku)
            Case LoadSearchType.ByLoad
                sql = String.Format("SELECT LOADID FROM invload WHERE LOADID LIKE '{0}'", LoadId.Trim())
            Case LoadSearchType.ByLocationAndSku
                sql = String.Format("SELECT LOADID FROM invload INNER JOIN SKU ON invload.CONSIGNEE = SKU.CONSIGNEE AND invload.SKU = SKU.SKU WHERE invload.LOCATION LIKE '{0}%' and invload.WAREHOUSEAREA LIKE '{2}%' AND (SKU.SKU like '{1}%' or SKU.MANUFACTURERSKU like '{1}%' or SKU.VENDORSKU ='{1}' or SKU.OTHERSKU ='{1}')", Loc.Trim(), Sku.Trim(), Warehousearea.Trim())
            Case LoadSearchType.ByLocationAndConsigneeAndSku
                sql = String.Format("SELECT LOADID FROM invload INNER JOIN SKU ON invload.CONSIGNEE = SKU.CONSIGNEE AND invload.SKU = SKU.SKU WHERE invload.LOCATION LIKE '{0}%' and invload.WAREHOUSEAREA LIKE '{2}%'  AND SKU.CONSIGNEE LIKE '{1}%' AND (SKU.SKU like '{2}%' or SKU.MANUFACTURERSKU like '{2}%' or SKU.VENDORSKU ='{2}' or SKU.OTHERSKU ='{2}')", Loc.Trim(), Cons.Trim(), Sku.Trim(), Warehousearea.Trim())
            Case LoadSearchType.ByLocation
                sql = String.Format("SELECT LOADID FROM invload WHERE LOCATION LIKE '{0}%' and WAREHOUSEAREA LIKE '{1}%'", Loc.Trim(), Warehousearea.Trim())
            Case LoadSearchType.BySku
                sql = String.Format("SELECT LOADID FROM invload INNER JOIN SKU ON invload.CONSIGNEE = SKU.CONSIGNEE AND invload.SKU = SKU.SKU WHERE SKU.CONSIGNEE LIKE '{0}%' AND (SKU.SKU like '{1}%' or SKU.MANUFACTURERSKU like '{1}%' or SKU.VENDORSKU ='{1}' or SKU.OTHERSKU ='{1}')", Cons.Trim(), Sku.Trim())
        End Select

        If sql = String.Empty Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Can not search by entered values"))
            Return
        End If

        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 1 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("More than 1 load in location, enter loadid"))
            Return
        ElseIf dt.Rows.Count = 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("No Load Found"))
            Return
        End If
        Dim ld As WMS.Logic.Load
        Try
            ld = New WMS.Logic.Load(dt.Rows(0)(0).ToString())
            If Not ld.isInventory Then
                Throw New ApplicationException("Load does not exist")
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage())
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        Session("LoadCNTLoadId") = ld.LOADID
        'Create the task for load counting and redirect to the tasks screen
        Dim oCnt As New Counting()
        If WMS.Logic.Counting.Exists(WMS.Lib.TASKTYPE.LOADCOUNTING, ld.LOADID) Then
            oCnt = New Counting(WMS.Lib.TASKTYPE.LOADCOUNTING, ld.LOADID)
        Else
            oCnt.CreateLoadCountJobs(ld.CONSIGNEE, ld.LOCATION, ld.WAREHOUSEAREA, ld.SKU, ld.LOADID, WMS.Logic.Common.GetCurrentUser)
        End If
        If Not oCnt.COUNTID = "" Then
            Dim tm As New WMS.Logic.TaskManager()
            tm.GetCountingTask(oCnt.COUNTID, WMS.Logic.Common.GetCurrentUser)
            Session("CountingSourceScreen") = "CNT"
            Response.Redirect(MapVirtualPath("Screens/CNTTASK.aspx"))
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Could not create count job for selected load"))
        End If
        'End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "newload"
                createNewLoadFromCounting()
        End Select
    End Sub

    Private Function LoadFindType(ByVal LoadId As String, ByVal Loc As String, ByVal Warehousearea As String, ByVal cons As String, ByVal sk As String) As Int32
        If LoadId <> "" Then Return LoadSearchType.ByLoad
        If Loc <> "" And Warehousearea <> "" And sk <> "" Then Return LoadSearchType.ByLocationAndSku
        If Loc <> "" And Warehousearea <> "" And cons <> "" And sk <> "" Then Return LoadSearchType.ByLocationAndConsigneeAndSku
        If cons <> "" And sk <> "" Then Return LoadSearchType.BySku
        If Loc <> "" And Warehousearea <> "" Then Return LoadSearchType.ByLocation
    End Function

    Public Enum LoadSearchType
        ByLoad = 1
        ByLocationAndSku = 2
        ByLocationAndConsigneeAndSku = 3
        BySku = 4
        ByLocation = 6
    End Enum

End Class
