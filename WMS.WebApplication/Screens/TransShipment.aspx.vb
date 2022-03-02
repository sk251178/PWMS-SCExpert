Imports WMS.Logic

Public Class Forwarding
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterTransShipment As Made4Net.WebControls.TableEditor
    Protected WithEvents TETransShipmentDetails As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector

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
            Case "assignsl"
                Dim SLSetter As New StagingLaneAssignmentSetter
                For Each dr In ds.Tables(0).Rows
                    SLSetter.SetDocumentStagingLane(WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT, dr("CONSIGNEE"), dr("TRANSSHIPMENT"))
                Next
                Message = ts.Translate("Orders Assigned To Staging Lanes!")
            Case "updatedetail"
                dr = ds.Tables(0).Rows(0)
                Dim td As New WMS.Logic.TransShipmentDetail(dr("Consignee"), dr("Transshipment"), dr("HandlingUnitID"))
                td.Update(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HandlingUnitType")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Weight")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Volume")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Notes")), Common.GetCurrentUser())
            Case "createdetail"
                dr = ds.Tables(0).Rows(0)
                Dim td As New WMS.Logic.TransShipmentDetail
                td.Create(dr("Consignee"), dr("Transshipment"), dr("HandlingUnitID"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HandlingUnitType")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Weight")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Volume")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Notes")), Common.GetCurrentUser())
        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEMasterTransShipment_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterTransShipment.CreatedChildControls
        With TEMasterTransShipment.ActionBar

            With .Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.TransShipment"
                If TEMasterTransShipment.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "UPDATE"
                Else
                    .CommandName = "CREATE"
                End If
            End With
            .AddSpacer()
            .AddExecButton("AssignSL", "Assign Orders to SL", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("AssignSL")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Forwarding"
                .CommandName = "AssignSL"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to assign the orders to Staging Lane?"
            End With
            .AddSpacer()
            .AddExecButton("cancelTransShipment", "Cancel TransShipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
            With .Button("cancelTransShipment")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.TransShipment"
                .CommandName = "CANCEL"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to cancel the TransShipment?"
            End With
            .AddSpacer()
            .AddExecButton("Receive", "Receive TransShipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarReceiveOrders"))
            With .Button("Receive")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.TransShipment"
                .CommandName = "RECEIVE"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = False
            End With
            .AddExecButton("Ship", "Ship TransShipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarShipOrders"))
            With .Button("Ship")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.TransShipment"
                .CommandName = "SHIP"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to ship the selected TransShipment?"
            End With
            .AddExecButton("PrintSH", "Print Shipping Manifest", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
            With .Button("PrintSH")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.TransShipment"
                .CommandName = "PrintSH"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

        End With
    End Sub

    Private Sub TEMasterTransShipment_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterTransShipment.CreatedGrid
        TEMasterTransShipment.Grid.AddExecButton("printlabels", "Print Label", "WMS.Logic.dll", "WMS.Logic.TransShipment", 4, Made4Net.WebControls.SkinManager.GetImageURL("LabelPrint"))
    End Sub

    Private Sub TETransShipmentDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TETransshipmentDetails.CreatedChildControls
        With TETransShipmentDetails.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Forwarding"
            If TETransShipmentDetails.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "UpdateDetail"
            Else
                .CommandName = "CreateDetail"
            End If
        End With
    End Sub

End Class


