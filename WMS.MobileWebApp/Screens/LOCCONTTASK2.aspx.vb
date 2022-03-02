Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Partial Class LOCCONTTASK2
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen

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
            Dim ld As New WMS.Logic.Load(Session("LocationCNTLoadId").ToString())
            DO1.Value("CONSIGNEE") = ld.CONSIGNEE
            DO1.Value("SKU") = ld.SKU
            DO1.Value("LOCATION") = ld.LOCATION
            DO1.Value("WAREHOUSEAREA") = ld.WAREHOUSEAREA

            DO1.Value("LOADID") = ld.LOADID

            'INIT SESSION FOR MULTI COUNT
            Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(ld.CONSIGNEE, ld.SKU, ld.LOADUOM, ld.UNITS)

            Dim ctrl As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")

            ctrl.AllOption = False
            ctrl.TableName = "SKUUOMDESC"
            ctrl.ValueField = "UOM"
            ctrl.TextField = "DESCRIPTION"
            ctrl.Where = String.Format("CONSIGNEE = '{0}' and SKU = '{1}'", ld.CONSIGNEE, ld.SKU)
            ctrl.DataBind()

            Try
                ctrl.SelectedValue = ld.LOADUOM
            Catch ex As Exception
            End Try
            Try
                DO1.Value("SKUDESC") = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select skudesc from sku where sku = '{0}' and consignee = '{1}'", ld.SKU, ld.CONSIGNEE))
            Catch ex As Exception
            End Try
            'Start RWMS-1262/RWMS-1332- get the RDTBlindCounting from warehouseparams
            Try
                Dim whpSQL As String
                whpSQL = "select PARAMVALUE from WAREHOUSEPARAMS where PARAMNAME='RDTBlindCounting'"
                Dim paramval As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(whpSQL)
                If paramval = "1" Then
                    'prepopulate the load units and location
                    DO1.Value("TOUNITS") = ld.UNITS
                End If

            Catch ex As Exception

            End Try
            'End RWMS-1262/RWMS-1332
        End If
    End Sub

    Private Sub doNext()
        Dim CountOk As Boolean = False

        'If Page.IsValid Then
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim ld As New WMS.Logic.Load(Session("LocationCNTLoadId").ToString())
            'ld.Count(DO1.Value("TOUNITS"), DO1.Value("UOM"), ld.LOCATION, WMS.Logic.GetCurrentUser)
            Dim oCntTask As WMS.Logic.CountTask
            If Not Session("LocationBulkCountTask") Is Nothing Then
                oCntTask = Session("LocationBulkCountTask")
            Else
                oCntTask = Session("LocationCountTask")
            End If
            Dim oCounting As New WMS.Logic.Counting(oCntTask.COUNTID)
            'Build and fill count job object
            Dim SK As New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)
            Dim oCountJob As WMS.Logic.CountingJob = oCntTask.getCountJob(oCounting)
            oCountJob.CountedQty = ManageMutliUOMUnits.GetTotalEachUnits() 'ManageMutliUOMUnits.GetTotal() ' DO1.Value("TOUNITS")
            oCountJob.UOM = SK.LOWESTUOM ' DO1.Value("UOM")
            oCountJob.Location = ld.LOCATION
            oCountJob.Warehousearea = ld.WAREHOUSEAREA
            oCountJob.LoadId = Session("LocationCNTLoadId")
            oCountJob.CountedLoads = Session("TaskLocationCNTLoadsDT")
            oCountJob.LocationCountingLoadCounted = True
            oCountJob.LoadCountVerified = False
            CountOk = oCntTask.Count(oCounting, oCountJob, WMS.Logic.GetCurrentUser)

            UpdateLoadCount(ld.LOADID, ld.UNITS)

            If Not CountOk Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Tolerance exceeded - please confirm quantity"))
                Session("CountedUnitsForVerification") = oCountJob.CountedQty
                Session("UOMForVerification") = oCountJob.UOM

                Dim SrcScreen As String
                'If Session("CountingSourceScreen") Is Nothing Then
                SrcScreen = "LOCCONTTASK2"
                'Else
                '   SrcScreen = Session("CountingSourceScreen")
                'End If
                Response.Redirect(MapVirtualPath("Screens/CNTTASKVERFICATION.aspx?SRCSCREEN=" & SrcScreen))

            End If
        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
        If Not CountOk Then
            Response.Redirect(MapVirtualPath("Screens/CNTTASKVERFICATION.aspx?SRCSCREEN=LOCCONTTASK"))
        Else
            ManageMutliUOMUnits.Clear(True)
            Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK1.aspx"))
        End If
        'End If
    End Sub

    Private Sub UpdateLoadCount(ByVal pLoadId As String, ByVal pToQty As Decimal)
        Dim dt As DataTable = Session("TaskLocationCNTLoadsDT")
        For Each dr As DataRow In dt.Rows
            If dr("loadid") = pLoadId Then
                dr("counted") = 1
                dr("toqty") = pToQty
            End If
        Next
        Session("TaskLocationCNTLoadsDT") = dt
    End Sub

    Private Sub doBack()
        Session.Remove("LocationCNTLoadId")
        Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK1.aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("CONSIGNEE", Nothing, "", Session("3PL"))
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")

        'DO1.AddSpacer()
        Dim ld As New WMS.Logic.Load(Session("LocationCNTLoadId").ToString())
        Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(ld.CONSIGNEE, ld.SKU, ld.LOADUOM, ld.UNITS)
        ManageMutliUOMUnits.DROWLABLES(DO1)
        ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)

        DO1.AddTextboxLine("TOUNITS")
        DO1.AddDropDown("UOM")

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"

                Dim errMessage As String = String.Empty
                If Not ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("TOUNITS"), errMessage, True) Then
                    'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMessage)
                    'DO1.Value("TOUNITS") = ""
                    'Return
                End If

                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                doNext()
            Case "back"
                ManageMutliUOMUnits.Clear(True)
                doBack()
          
            Case "addunits"

                Dim errMessage As String = String.Empty
                If Not ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("TOUNITS"), errMessage, True) Then
                    'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMessage)
                    'DO1.Value("TOUNITS") = ""
                    'Return
                End If

                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                DO1.Value("TOUNITS") = ""
            Case "clearunits"
                ManageMutliUOMUnits.Clear()
                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
        End Select
    End Sub

End Class
