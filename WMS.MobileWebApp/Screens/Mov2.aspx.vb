Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports RWMS.Logic

<CLSCompliant(False)> Public Class Mov2
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
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            If (Session("LoadMoveLoadId") Is Nothing) Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
            End If
            Dim ld As New WMS.Logic.Load(Session("LoadMoveLoadId").ToString())
            DO1.Value("CONSIGNEE") = ld.CONSIGNEE
            DO1.Value("SKU") = ld.SKU
            DO1.Value("UOM") = ld.LOADUOM
            DO1.Value("UNITS") = ld.UOMUnits
            DO1.Value("LOCATION") = ld.LOCATION
            DO1.Value("WAREHOUSEAREA") = ld.WAREHOUSEAREA
            DO1.Value("LOADID") = ld.LOADID
            Try
                DO1.Value("SKUDESC") = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select skudesc from sku where sku = '{0}' and consignee = '{1}'", ld.SKU, ld.CONSIGNEE))
                DO1.Value("TOQTY") = ld.UOMUnits ' ld.UNITS
            Catch ex As Exception
            End Try
            Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")
            dd.AllOption = False
            dd.TableName = "SKUUOMDESC"
            dd.ValueField = "UOM"
            dd.TextField = "DESCRIPTION"
            dd.Where = String.Format("CONSIGNEE = '{0}' and SKU = '{1}'", ld.CONSIGNEE, ld.SKU)
            dd.DataBind()
            Try
                dd.SelectedValue = ld.LOADUOM
            Catch ex As Exception
            End Try


            'INIT SESSION FOR MULTI COUNT
            Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(ld.CONSIGNEE, ld.SKU, ld.LOADUOM, ld.UNITS)

            Try
                dd = New Made4Net.WebControls.MobileDropDown
                dd = DO1.Ctrl("TOWAREHOUSEAREA")
                dd.AllOption = False
                dd.TableName = "WAREHOUSEAREA"
                dd.ValueField = "WAREHOUSEAREACODE"
                dd.TextField = "WAREHOUSEAREADESCRIPTION"
                dd.DataBind()
                Try
                    dd.SelectedValue = Session("LoginWHArea")
                Catch ex As Exception
                End Try

            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("LoadMoveLoadId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("LoadMoveLoadId")
        Response.Redirect(MapVirtualPath("Screens/Mov.aspx"))
    End Sub

    Private Sub doNext()
        'If Page.IsValid Then
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'Check if location is good for use
        Dim oloc As WMS.Logic.Location
        Try
            oloc = New WMS.Logic.Location(DO1.Value("TOLOCATION"), DO1.Value("TOWAREHOUSEAREA"))
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try



        Dim ld As New WMS.Logic.Load(Session("LoadMoveLoadId").ToString())

        Dim err As String
        If Not AppUtil.ChangeLocationValidation(DO1.Value("TOLOCATION"), DO1.Value("TOWAREHOUSEAREA"), ld.CONSIGNEE, ld.SKU, err) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
            DO1.Value("TOLOCATION") = ""
            Return
        End If

        Dim errMessage As String = String.Empty
        If Not ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("TOQTY"), errMessage) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMessage)
            DO1.Value("TOQTY") = ""
            Return
        End If

        ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
        If Not IsNumeric(ManageMutliUOMUnits.GetTotalEachUnits()) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "TOQTY field is mandatory")
            Return
        End If

        Dim units As Decimal
        units = ManageMutliUOMUnits.GetTotalEachUnits() 'GetTotal()
        Try

            'units = DO1.Value("TOQTY")
            If Convert.ToDecimal(units) > 0 And Convert.ToDecimal(units) < ld.UNITS Then
                Dim newLoadid As String = Made4Net.Shared.Util.getNextCounter("LOAD")
                ld.Split(oloc.Location, oloc.WAREHOUSEAREA, units, newLoadid, WMS.Logic.GetCurrentUser)
                AppUtil.isBackLocMoveFront(newLoadid, oloc.Location, oloc.WAREHOUSEAREA, "", err)

            Else
                ld.Move(oloc.Location, oloc.WAREHOUSEAREA, "", WMS.Logic.GetCurrentUser)
                AppUtil.isBackLocMoveFront(ld.LOADID, oloc.Location, oloc.WAREHOUSEAREA, "", err)
            End If
            If err <> "" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
            End If
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load moving process complete."))
        Session.Remove("LoadMoveLoadId")
        ManageMutliUOMUnits.Clear(True)
        Response.Redirect(MapVirtualPath("Screens/Mov.aspx"))
        ' End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        If (Session("LoadMoveLoadId") Is Nothing) Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
        End If
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("CONSIGNEE", Nothing, "", Session("3PL"))
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        ' DO1.AddLabelLine("UOM")
        DO1.AddLabelLine("UNITS")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        '  DO1.AddSpacer()
        Dim ld As New WMS.Logic.Load(Session("LoadMoveLoadId").ToString())
        Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(ld.CONSIGNEE, ld.SKU, ld.LOADUOM, ld.UNITS)
        ManageMutliUOMUnits.DROWLABLES(DO1)
        ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
        DO1.AddTextboxLine("TOQTY", False, "next")
        DO1.AddDropDown("UOM")
        DO1.AddTextboxLine("TOLOCATION", False, "next")
        DO1.AddDropDown("TOWAREHOUSEAREA") ', True, "next")

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                
                doNext()
                'DO1.Value("TOQTY") = ""
            Case "back"
                ManageMutliUOMUnits.Clear(True)
                doBack()
            Case "menu"
                ManageMutliUOMUnits.Clear(True)
                doMenu()
            Case "addunits"
               
                Dim errMessage As String = String.Empty
                If Not ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("TOQTY"), errMessage) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMessage)
                    DO1.Value("TOQTY") = ""
                    Return
                End If

                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                DO1.Value("TOQTY") = ""
            Case "clearunits"
                ManageMutliUOMUnits.Clear()
                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
        End Select
    End Sub

End Class
