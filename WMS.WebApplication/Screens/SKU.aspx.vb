Public Class SKU
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected AttTable As Made4Net.WebControls.Table
    Protected attributeDataTable As DataTable
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMaster As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected DC1 As Made4Net.WebControls.DataConnector
    Protected DC2 As Made4Net.WebControls.DataConnector
    Protected DC3 As Made4Net.WebControls.DataConnector
    Protected WithEvents TESKUUOM As Made4Net.WebControls.TableEditor
    Protected WithEvents TEBOM As Made4Net.WebControls.TableEditor
    Protected DC4 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEGeneral As Made4Net.WebControls.TableEditor
    Protected WithEvents TEInventory As Made4Net.WebControls.TableEditor
    Protected WithEvents DC5 As Made4Net.WebControls.DataConnector
    Protected WithEvents DC6 As Made4Net.WebControls.DataConnector
    Protected WithEvents DC7 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEPickLoc As Made4Net.WebControls.TableEditor
    Protected WithEvents DC8 As Made4Net.WebControls.DataConnector
    Protected WithEvents attab As Made4Net.WebControls.ActionBar
    Protected WithEvents TESkuAtt As Made4Net.WebControls.TableEditor
    Protected WithEvents TELaborPerformance As Made4Net.WebControls.TableEditor
    Protected WithEvents DC9 As Made4Net.WebControls.DataConnector
    Protected WithEvents TESkuSubt As Made4Net.WebControls.TableEditor

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

#Region "Ctor"

    Public Sub New()

    End Sub

    '' PWMS-475 - Retofit of RWMS-695 - Modified the changes in saveinventory case statement
    Public Sub New(ByVal pSender As Object, ByVal pCommandName As String, ByVal pXMLSchema As String, ByVal pXMLData As String, ByRef pMessage As String)
        Dim ds As New DataSet
        ds = Made4Net.Shared.Util.XmlToDS(pXMLSchema, pXMLData)
        Dim consignee As String = ds.Tables(0).Rows(0)("consignee")

        Select Case pCommandName.ToLower
            Case "delete"
                Dim sku As String = ds.Tables(0).Rows(0)("sku")
                Dim osku As New WMS.Logic.SKU(consignee, sku)

                'Added for PWMS-351 Start
                If Not WMS.Logic.Utils.deleteSKU(consignee, sku, pMessage) Then
                    Throw New ApplicationException(pMessage)
                End If

                'Added for PWMS-351 End
                osku.Delete()
            Case "insertbom"
                Dim oBom As New WMS.Logic.SKU.SKUBOM()
                oBom.Create(consignee, ds.Tables(0).Rows(0)("bomsku"), ds.Tables(0).Rows(0)("partsku"), ds.Tables(0).Rows(0)("partqty"), Nothing, WMS.Logic.Common.GetCurrentUser)
            Case "editbom"
                Dim oBom As New WMS.Logic.SKU.SKUBOM(consignee, ds.Tables(0).Rows(0)("bomsku"), ds.Tables(0).Rows(0)("partsku"))
                oBom.Update(ds.Tables(0).Rows(0)("partqty"), Nothing, WMS.Logic.Common.GetCurrentUser)
            Case "insertsubs"
                Dim oSubs As New WMS.Logic.SKU.SkuSubstitutes()
                oSubs.Create(consignee, ds.Tables(0).Rows(0)("sku"), 1, ds.Tables(0).Rows(0)("substitutesku"), ds.Tables(0).Rows(0)("priority"), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SUBSTITUTESKUQTY")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SUBSTITUTIONTYPE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("fromdate")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("todate")), ds.Tables(0).Rows(0)("MULTYLEVEL"), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("company")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("companytype")), WMS.Logic.Common.GetCurrentUser)
            Case "editsubs"
                Dim oSubs As New WMS.Logic.SKU.SkuSubstitutes(consignee, ds.Tables(0).Rows(0)("sku"), ds.Tables(0).Rows(0)("substitutesku"))
                oSubs.Update(ds.Tables(0).Rows(0)("priority"), 1, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SUBSTITUTESKUQTY")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SUBSTITUTIONTYPE")), _
                ds.Tables(0).Rows(0)("MULTYLEVEL"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("company")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("companytype")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("fromdate")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("todate")), WMS.Logic.Common.GetCurrentUser)
            Case "updateskuheader"
                Dim oSKU As WMS.Logic.SKU = New WMS.Logic.SKU(consignee, ds.Tables(0).Rows(0)("SKU"))
                oSKU.Update(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SKUDESC")),
                            oSKU.SKUSHORTDESC, oSKU.MANUFACTURERSKU, oSKU.VENDORSKU, oSKU.OTHERSKU, oSKU.SKUGROUP, oSKU.NEWSKU,
                            oSKU.OPORTUNITYRELPFLAG, oSKU.STATUS, oSKU.SKUClassName, oSKU.INVENTORY, oSKU.INITIALSTATUS, oSKU.VELOCITY,
                            oSKU.FIFOINDIFFERENCE, oSKU.ONSITEMIN, oSKU.ONSITEMAX, oSKU.LASTCYCLECOUNT, oSKU.CYCLECOUNTINT, oSKU.LOWLIMITCOUNT,
                            oSKU.PREFLOCATION, oSKU.PREFWAREHOUSEAREA, oSKU.PREFPUTREGION, oSKU.PICKSORTORDER,
                            Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("defaultuom")), oSKU.OVERPICKPCT, oSKU.UNITPRICE, oSKU.OVERRECEIVEPCT,
                            oSKU.DailyDemand, oSKU.DailyPicks, oSKU.TRANSPORTATIONCLASS, oSKU.HAZCLASS, oSKU.HUTYPE, oSKU.DEFAULTRECUOM, oSKU.STORAGECLASS,
                            oSKU.NOTES, oSKU.BASEITEM, oSKU.BASEITEMQTY, WMS.Logic.Common.GetCurrentUser, oSKU.COUNTTOLERANCE, oSKU.AUTOADJUSTCOUNTQTY, Nothing, oSKU.DEFAULTRECLOADUOM, oSKU.OVERALLOCMODE,
                            oSKU.RECEIVINGWEIGHTCAPTUREMETHOD, oSKU.SHIPPINGWEIGHTCAPTUREMETHOD, oSKU.RECEIVINGWEIGHTTOLERANCE, oSKU.SHIPPINGWEIGHTTOLERANCE, oSKU.FULLPICKALLOCATION, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("PARENTSKU")))

            Case "saveinventory"
                Dim oSKU As New WMS.Logic.SKU(consignee, ds.Tables(0).Rows(0)("sku"))
                oSKU.Update(oSKU.SKUDESC, oSKU.SKUSHORTDESC, oSKU.MANUFACTURERSKU, oSKU.VENDORSKU, oSKU.OTHERSKU, oSKU.SKUGROUP, oSKU.NEWSKU,
                    oSKU.OPORTUNITYRELPFLAG, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("status")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("classname")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("inventory")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("initialstatus")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("velocity")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("fifoindifference")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("onsitemin")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("onsitemax")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("lastcyclecount")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("cyclecountint")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("lowlimitcount")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("preflocation")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("prefwarehousearea")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("prefputregion")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("picksortorder")),
                    oSKU.DEFAULTUOM, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("overpickpct")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("unitprice")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("overreceivepct")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("dailydemand")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("dailypicks")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("transportationclass")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("hazclass")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("hutype")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("defaultrecuom")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("storageclass")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("notes")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("baseitem")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("baseitemqty")), WMS.Logic.Common.GetCurrentUser(), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("counttolerance")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("AUTOADJUSTCOUNTQTY")), Nothing,
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("DEFAULTRECLOADUOM")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("OVERALLOCMODE")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("RECEIVINGWEIGHTCAPTUREMETHOD")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SHIPPINGWEIGHTCAPTUREMETHOD")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("RECEIVINGWEIGHTTOLERANCE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SHIPPINGWEIGHTTOLERANCE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("FULLPICKALLOCATION")), oSKU.PARENTSKU)
                'Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("AUTOADJUSTCOUNTQTY"))
            Case "multieditsku"
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim oSKU As New WMS.Logic.SKU(dr("consignee"), dr("sku"))
                    oSKU.Update(oSKU.SKUDESC, oSKU.SKUSHORTDESC, oSKU.MANUFACTURERSKU, oSKU.VENDORSKU, oSKU.OTHERSKU, oSKU.SKUGROUP, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("newsku")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("OPORTUNITYRELPFLAG")), oSKU.STATUS,
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("classname")), oSKU.INVENTORY,
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("initialstatus")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("velocity")),
                    oSKU.FIFOINDIFFERENCE, oSKU.ONSITEMIN,
                    oSKU.ONSITEMAX, oSKU.LASTCYCLECOUNT,
                    oSKU.CYCLECOUNTINT, oSKU.LOWLIMITCOUNT,
                    oSKU.PREFLOCATION, oSKU.PREFWAREHOUSEAREA,
                    oSKU.PREFPUTREGION, oSKU.PICKSORTORDER,
                    oSKU.DEFAULTUOM, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("overpickpct")),
                    oSKU.UNITPRICE, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("overreceivepct")),
                    oSKU.DailyDemand, oSKU.DailyPicks,
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("transportationclass")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("hazclass")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("hutype")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("defaultrecuom")),
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("storageclass")), oSKU.NOTES,
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("baseitem")), oSKU.BASEITEMQTY,
                    WMS.Logic.Common.GetCurrentUser(), oSKU.COUNTTOLERANCE, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("AUTOADJUSTCOUNTQTY")), Nothing, oSKU.DEFAULTRECLOADUOM, oSKU.OVERALLOCMODE,
                    oSKU.RECEIVINGWEIGHTCAPTUREMETHOD, oSKU.SHIPPINGWEIGHTCAPTUREMETHOD,
                    oSKU.RECEIVINGWEIGHTTOLERANCE, oSKU.SHIPPINGWEIGHTTOLERANCE, oSKU.FULLPICKALLOCATION, oSKU.PARENTSKU)
                Next
            Case "newuom"
                Dim oSKU As New WMS.Logic.SKU(ds.Tables(0).Rows(0)("consignee"), ds.Tables(0).Rows(0)("sku"))
                oSKU.CreateUOM(ds.Tables(0).Rows(0)("UOM"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("EANUPC")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("GROSSWEIGHT")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NetWeight")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("Length")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("WIDTH")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("HEIGHT")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("VOLUME")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LOWERUOM")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("UNITSPERMEASURE")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SHIPPABLE")), WMS.Logic.Common.GetCurrentUser(), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NESTEDQUANTITY")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NESTEDWIDTHDIFF")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NESTEDHEIGHTDIFF")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NESTEDDEPTHDIFF")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NESTEDVOLUMEDIFF")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LaborGrabType")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LaborHandlingType")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborpackagetype")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborpreparationtype")))
            Case "updateuom"
                Dim oSKU As New WMS.Logic.SKU(ds.Tables(0).Rows(0)("consignee"), ds.Tables(0).Rows(0)("sku"))
                oSKU.UpdateUOM(ds.Tables(0).Rows(0)("UOM"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("EANUPC")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("GROSSWEIGHT")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NetWeight")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("Length")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("WIDTH")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("HEIGHT")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("VOLUME")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LOWERUOM")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("UNITSPERMEASURE")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SHIPPABLE")), WMS.Logic.Common.GetCurrentUser(), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NESTEDQUANTITY")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NESTEDWIDTHDIFF")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NESTEDHEIGHTDIFF")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NESTEDDEPTHDIFF")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NESTEDVOLUMEDIFF")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LaborGrabType")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LaborHandlingType")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborpackagetype")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborpreparationtype")))

            Case "insertpickloc"
                Dim pLoc As New WMS.Logic.PickLoc()

                pLoc.Consignee = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("Consignee"))
                'Started for PWMS-756
                pLoc.MaximumReplQty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("MAXREPLQTY"))
                pLoc.ReplPolicy = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("REPLPolicy"))
                pLoc.ReplQty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("replqty"))
                'Ended for PWMS-756
                pLoc.Location = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("Location"))
                pLoc.MaximumQty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("maximumqty"))
                pLoc.NormalReplPolicy = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("normalreplpolicy"))
                pLoc.NormalReplQty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("normalreplqty"))
                pLoc.OverAllocatedQty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("overallocatedqty"))
                pLoc.SKU = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("sku"))
                pLoc.Warehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("warehousearea"))

                pLoc.Create(WMS.Logic.GetCurrentUser)
            Case "updatepickloc"
                Dim pLoc As New WMS.Logic.PickLoc(ds.Tables(0).Rows(0)("Location"), ds.Tables(0).Rows(0)("warehousearea"), ds.Tables(0).Rows(0)("consignee"), ds.Tables(0).Rows(0)("sku"))
                'Started for PWMS-756
                pLoc.MaximumReplQty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("MAXREPLQTY"))
                pLoc.ReplPolicy = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("REPLPolicy"))
                pLoc.ReplQty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("replqty"))
                'Ended for PWMS-756
                pLoc.MaximumQty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("maximumqty"))
                pLoc.NormalReplPolicy = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("normalreplpolicy"))
                pLoc.NormalReplQty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("normalreplqty"))
                pLoc.OverAllocatedQty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("overallocatedqty"))

                pLoc.Update(WMS.Logic.GetCurrentUser)
            Case "deletepickloc"
                Dim pLoc As New WMS.Logic.PickLoc(ds.Tables(0).Rows(0)("Location"), ds.Tables(0).Rows(0)("warehousearea"), ds.Tables(0).Rows(0)("consignee"), ds.Tables(0).Rows(0)("sku"))
                pLoc.Delete()
            Case "updateattributes"
                Dim skuAtt As New WMS.Logic.SKU.SkuAttributes()
                skuAtt.PopulateFromDataTable(ds.Tables(0))
                If skuAtt.Attributes.Item("WGT") = "" Then skuAtt.Attributes.Remove("WGT")
                If skuAtt.Attributes.Item("TOLPCT") = "" Then skuAtt.Attributes.Remove("TOLPCT")
                If skuAtt.Attributes.Item("DAYREC") = "" Then skuAtt.Attributes.Remove("DAYREC")
                If skuAtt.Attributes.Item("DANG") = "" Then skuAtt.Attributes.Remove("DANG")
                If skuAtt.Attributes.Item("MINDAYSTOSHIP") = "" Then skuAtt.Attributes.Remove("MINDAYSTOSHIP")
                If skuAtt.Attributes.Item("SHORTDATEDDAYS") = "" Then skuAtt.Attributes.Remove("SHORTDATEDDAYS")
                If skuAtt.Attributes.Item("DAYSTORECEIVE") = "" Then skuAtt.Attributes.Remove("DAYSTORECEIVE")
                If skuAtt.Attributes.Item("SHELFLIFE") = "" Then skuAtt.Attributes.Remove("SHELFLIFE")
                If skuAtt.Attributes.Count > 0 Then skuAtt.Update()
            Case "insertattributes"
                Dim skuAtt As New WMS.Logic.SKU.SkuAttributes()
                skuAtt.PopulateFromDataTable(ds.Tables(0))
                If skuAtt.Attributes.Item("WGT") = "" Then skuAtt.Attributes.Remove("WGT")
                If skuAtt.Attributes.Item("TOLPCT") = "" Then skuAtt.Attributes.Remove("TOLPCT")
                If skuAtt.Attributes.Item("DAYREC") = "" Then skuAtt.Attributes.Remove("DAYREC")
                If skuAtt.Attributes.Item("DANG") = "" Then skuAtt.Attributes.Remove("DANG")
                If skuAtt.Attributes.Item("MINDAYSTOSHIP") = "" Then skuAtt.Attributes.Remove("MINDAYSTOSHIP")
                If skuAtt.Attributes.Item("SHORTDATEDDAYS") = "" Then skuAtt.Attributes.Remove("SHORTDATEDDAYS")
                If skuAtt.Attributes.Item("DAYSTORECEIVE") = "" Then skuAtt.Attributes.Remove("DAYSTORECEIVE")
                If skuAtt.Attributes.Item("SHELFLIFE") = "" Then skuAtt.Attributes.Remove("SHELFLIFE")
                If skuAtt.Attributes.Count > 0 Then skuAtt.Insert()

        End Select
    End Sub


#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub TEMaster_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMaster.CreatedGrid
        TEMaster.Grid.AddExecButton("PrintLabels", "Print Label", "WMS.Logic.dll", "WMS.Logic.SKU", 4, Made4Net.WebControls.SkinManager.GetImageURL("LabelPrint"))
    End Sub

    Private Sub TESKUUOM_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TESKUUOM.CreatedChildControls
        With TESKUUOM.ActionBar.Button("SAVE")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.SKU"
            If TESKUUOM.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "NewUom"
            ElseIf TESKUUOM.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "UpdateUom"
            End If
        End With
        With TESKUUOM.ActionBar.Button("DELETE")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.SKU"
            .CommandName = "deluom"
        End With
    End Sub

    Private Sub TEMaster_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMaster.CreatedChildControls
        With TEMaster
            With .ActionBar
                With .Button("Save")
                    If TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                        .ObjectDLL = "WMS.Logic.dll"
                        .ObjectName = "WMS.Logic.SKU"
                        .CommandName = "newsku"
                    ElseIf TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                        .ObjectDLL = "WMS.WebApp.dll"
                        .ObjectName = "WMS.WebApp.SKU"
                        .CommandName = "updateskuheader"
                    ElseIf TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.MultilineEdit Then
                        .ObjectDLL = "WMS.WebApp.dll"
                        .ObjectName = "WMS.WebApp.SKU"
                        .CommandName = "multieditsku"
                    End If
                End With
                With .Button("Delete")
                    .ObjectDLL = "WMS.WebAPP.dll"
                    .ObjectName = "WMS.WebApp.SKU"
                    .CommandName = "Delete"
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to delete the sku?"
                End With
            End With
        End With
    End Sub

    Protected Sub TEBOM_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEBOM.CreatedChildControls
        With TEBOM
            With .ActionBar
                With .Button("Save")
                    .ObjectDLL = "WMS.WebAPP.dll"
                    .ObjectName = "WMS.WebApp.SKU"
                    If TEBOM.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                        .CommandName = "InsertBOM"
                    ElseIf TEBOM.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                        .CommandName = "EditBOM"
                    End If

                End With
            End With
        End With
    End Sub

    Private Sub TESkuSubt_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TESkuSubt.CreatedChildControls

        With TESkuSubt
            With .ActionBar
                With .Button("Save")
                    .ObjectDLL = "WMS.WebAPP.dll"
                    .ObjectName = "WMS.WebApp.SKU"
                    If TESkuSubt.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                        .CommandName = "InsertSubs"
                    ElseIf TESkuSubt.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                        .CommandName = "EditSubs"
                    End If
                End With
            End With
        End With

    End Sub

    Protected Sub TEInventory_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEInventory.CreatedChildControls
        If TEInventory.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
            With TEInventory
                With .ActionBar
                    With .Button("Save")
                        .ObjectDLL = "WMS.WebAPP.dll"
                        .ObjectName = "WMS.WebApp.SKU"
                        .CommandName = "SaveInventory"
                    End With
                End With
            End With
        End If
    End Sub

    Protected Sub TEPickLoc_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEPickLoc.CreatedChildControls

        With TEPickLoc
            With .ActionBar
                With .Button("Save")
                    .ObjectDLL = "WMS.WebAPP.dll"
                    .ObjectName = "WMS.WebApp.SKU"
                    If TEPickLoc.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                        .CommandName = "InsertPickLoc"
                    ElseIf TEPickLoc.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                        .CommandName = "UpdatePickLoc"
                    End If
                End With
                With .Button("Delete")
                    .ObjectDLL = "WMS.WebAPP.dll"
                    .ObjectName = "WMS.WebApp.SKU"
                    .CommandName = "DeletePickLoc"
                End With
            End With
        End With
    End Sub

    Protected Sub TESkuAtt_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TESkuAtt.CreatedChildControls
        With TESkuAtt.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebAPP.dll"
            .ObjectName = "WMS.WebApp.SKU"
            If TESkuAtt.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "UpdateAttributes"
            ElseIf TESkuAtt.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "InsertAttributes"
            End If
        End With
    End Sub
End Class