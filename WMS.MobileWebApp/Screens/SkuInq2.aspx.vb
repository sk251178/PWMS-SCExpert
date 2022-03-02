Imports Made4Net.Shared.Web

<CLSCompliant(False)> Public Class SkuInq2
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
            Try
                Dim sql As String = String.Format("select consignee,sku,isnull(skudesc,'') as skudesc,isnull(skugroup,'') as skugroup,isnull(cls.classdescription,'') as classdescription ,isnull(velocity,'') as velocity " & _
                                    "from sku left outer join skucls cls on sku.classname = cls.classname where consignee='{0}' and sku='{1}'", Session("SKUInqConsingee"), Session("SKUInqSKU"))
                Dim dt As New DataTable
                Dim dr As DataRow
                Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
                dr = dt.Rows(0)
                Dim c As New WMS.Logic.Consignee(dr("CONSIGNEE"))
                DO1.Value("CONSIGNEE") = c.CONSIGNEENAME
                DO1.Value("SKU") = dr("SKU")
                DO1.Value("SKUDESC") = dr("SKUDESC")
                DO1.Value("ClassName") = dr("CLASSDESCRIPTION")
                DO1.Value("SkuGroup") = dr("SKUGROUP")
                DO1.Value("Velocity") = dr("VELOCITY")

                Dim oSku As New Logic.SKU(dr("CONSIGNEE"), dr("SKU"))

                Try
                    If WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, "CASE") Then
                        DO1.Value("CPQ") = oSku.ConvertToUnits("CASE")
                    Else
                        DO1.setVisibility("CPQ", False)
                    End If
                Catch ex As Exception
                End Try

                Dim TiHi As String = String.Empty

                If WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, "LAYER") Then
                    Dim oUom As New WMS.Logic.SKU.SKUUOM(oSku.CONSIGNEE, oSku.SKU, "LAYER")
                    TiHi = Math.Truncate(oUom.UNITSPERMEASURE) & "/"
                Else
                    TiHi = "/"
                End If



                If WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, "PALLET") Then
                    Dim oUom As New WMS.Logic.SKU.SKUUOM(oSku.CONSIGNEE, oSku.SKU, "PALLET")
                    TiHi &= Math.Truncate(oUom.UNITSPERMEASURE)
                End If


                DO1.Value("Ti/Hi") = TiHi

                sql = String.Format("select top 1 LOCATION,WAREHOUSEAREA from PICKLOC where CONSIGNEE='{0}' and SKU='{1}'", oSku.CONSIGNEE, oSku.SKU)
                Dim dt1 As New DataTable
                Dim dr1 As DataRow

                Try
                    Made4Net.DataAccess.DataInterface.FillDataset(sql, dt1)
                    dr1 = dt1.Rows(0)
                    DO1.Value("PickLoc") = dr1("LOCATION")
                    DO1.Value("WAREHOUSEAREA") = dr1("WAREHOUSEAREA")
                Catch ex As Exception

                End Try

            Catch ex As Exception
                doNext()
            End Try
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("SKUInqConsingee")
        Session.Remove("SKUInqSKU")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Session.Remove("SKUInqConsingee")
        Session.Remove("SKUInqSKU")
        Response.Redirect(MapVirtualPath("Screens/SKUInq.aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Consignee", Nothing, "", Session("3PL"))
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("SKUGroup")
        DO1.AddLabelLine("ClassName")
        DO1.AddLabelLine("Velocity")
        DO1.AddLabelLine("Ti/Hi")

        DO1.AddLabelLine("CPQ")
        DO1.AddLabelLine("PickLoc")
        DO1.AddLabelLine("WAREHOUSEAREA")
        'WAREHOUSEAREA 
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub
End Class
