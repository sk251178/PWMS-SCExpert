Public Class Equipment
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterEquipment As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEEquipmentDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEEquipmentMHC As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents DC3 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEEquipmentTypeEdges As Made4Net.WebControls.TableEditor
    Protected WithEvents TEEquipmentAccessibility As Made4Net.WebControls.TableEditor

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

    Public Sub New(ByVal pSender As Object, ByVal pCommandName As String, ByVal pXMLSchema As String, ByVal pXMLData As String, ByRef pMessage As String)
        Dim ds As New DataSet
        ds = Made4Net.Shared.Util.XmlToDS(pXMLSchema, pXMLData)

        Select Case pCommandName.ToLower()
            Case "createequipment"
                Dim equipObj As New WMS.Logic.HandlingEquipment()
                Dim dr As DataRow = ds.Tables(0).Rows(0)
                equipObj.Create(dr("handlingequipment"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Mobilitycode")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WeightCapacity")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("AllowShareAisle")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("walkthreshold")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("slowthreshold")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("fastthreshold")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("mhehorizontalconst")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("mhehorizontalvariable")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("mhecongestionfactor")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SafetyCheckRequired")), WMS.Logic.GetCurrentUser)
            Case "createequipmentedge"
                Dim edgeObj As New WMS.Logic.MheEdgeType()
                Dim dr As DataRow = ds.Tables(0).Rows(0)
                edgeObj.Create(dr("MHETYPE"), dr("edgetype"), WMS.Logic.GetCurrentUser)
        End Select



    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub


    Private Sub TEMasterEquipment_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterEquipment.CreatedChildControls
        If TEMasterEquipment.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
            With TEMasterEquipment.ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.DLL"
                .ObjectName = "WMS.WebApp.Equipment"
                .CommandName = "CreateEquipment"
            End With
        End If
    End Sub

    Private Sub TEEquipmentTypeEdges_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEEquipmentTypeEdges.CreatedChildControls
        If TEEquipmentTypeEdges.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
            With TEEquipmentTypeEdges.ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.DLL"
                .ObjectName = "WMS.WebApp.Equipment"
                .CommandName = "CreateEquipmentEdge"
            End With
        End If
    End Sub
End Class
