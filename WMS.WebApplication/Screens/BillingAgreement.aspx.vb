Public Class BillingAgreement
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEBillingAgreementMaster As Made4Net.WebControls.TableEditor
    Protected WithEvents TEBillingAgreementDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents TEBillingInboundAgreementDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents TEBillingOutboundAgreementDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents TEBillingStorageAgreementDetail As Made4Net.WebControls.TableEditor

    Protected WithEvents TEBillingAssemblyAgreementDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents TEBillingDisassemblyAgreementDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents TEBillingVAAgreementDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents TEBillingConstAgreementDetail As Made4Net.WebControls.TableEditor

    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable    
    Protected WithEvents Dataconnector1 As Made4Net.WebControls.DataConnector
    Protected WithEvents Dataconnector2 As Made4Net.WebControls.DataConnector
    Protected WithEvents Dataconnector3 As Made4Net.WebControls.DataConnector
    Protected WithEvents Dataconnector4 As Made4Net.WebControls.DataConnector
    Protected WithEvents Dataconnector5 As Made4Net.WebControls.DataConnector
    Protected WithEvents Dataconnector6 As Made4Net.WebControls.DataConnector
    Protected WithEvents Dataconnector7 As Made4Net.WebControls.DataConnector

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here        
    End Sub

    Private Sub TEBillingAgreementMaster_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillingAgreementMaster.CreatedChildControls
        With TEBillingAgreementMaster
            .ActionBar.AddSpacer()
            .ActionBar.AddExecButton("Bill", "Run Agreement", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))

            'With .ActionBar.Button("Save")
            '    .ObjectDLL = "WMS.Logic.dll"
            '    .ObjectName = "WMS.Logic.Agreement"
            '    If TEBillingAgreementMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
            '        .CommandName = "InsertAgreementHeader"
            '    ElseIf TEBillingAgreementMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
            '        .CommandName = "UpdateAgreementHeader"
            '    End If
            'End With

            With .ActionBar.Button("Bill")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.Logic.Billing.dll"
                .ObjectName = "WMS.Logic.Billing.Agreement"
            End With

        End With
    End Sub

    'Private Sub TEBillingInboundAgreementDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillingInboundAgreementDetail.CreatedChildControls
    '    With TEBillingInboundAgreementDetail
    '        With .ActionBar.Button("Save")
    '            .ObjectDLL = "WMS.Logic.dll"
    '            .ObjectName = "WMS.Logic.Agreement"
    '            .CommandName = "addagreementline"
    '        End With
    '    End With
    'End Sub


    'Private Sub TEBillingOutboundAgreementDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillingOutboundAgreementDetail.CreatedChildControls
    '    With TEBillingOutboundAgreementDetail
    '        With .ActionBar.Button("Save")
    '            .ObjectDLL = "WMS.Logic.dll"
    '            .ObjectName = "WMS.Logic.Agreement"
    '            .CommandName = "addagreementline"
    '        End With
    '    End With
    'End Sub

    'Private Sub TEBillingStorageAgreementDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillingStorageAgreementDetail.CreatedChildControls
    '    With TEBillingStorageAgreementDetail
    '        With .ActionBar.Button("Save")
    '            .ObjectDLL = "WMS.Logic.dll"
    '            .ObjectName = "WMS.Logic.Agreement"
    '            .CommandName = "addagreementline"
    '        End With
    '    End With
    'End Sub

    'Private Sub TEBillingVAAgreementDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillingVAAgreementDetail.CreatedChildControls
    '    With TEBillingVAAgreementDetail
    '        With .ActionBar.Button("Save")
    '            .ObjectDLL = "WMS.Logic.dll"
    '            .ObjectName = "WMS.Logic.Agreement"
    '            .CommandName = "addagreementline"
    '        End With
    '    End With
    'End Sub

    'Private Sub TEBillingDisassemblyAgreementDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillingDisassemblyAgreementDetail.CreatedChildControls
    '    With TEBillingDisassemblyAgreementDetail
    '        With .ActionBar.Button("Save")
    '            .ObjectDLL = "WMS.Logic.dll"
    '            .ObjectName = "WMS.Logic.Agreement"
    '            .CommandName = "addagreementline"
    '        End With
    '    End With
    'End Sub

    'Private Sub TEBillingAssemblyAgreementDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillingAssemblyAgreementDetail.CreatedChildControls
    '    With TEBillingAssemblyAgreementDetail
    '        With .ActionBar.Button("Save")
    '            .ObjectDLL = "WMS.Logic.dll"
    '            .ObjectName = "WMS.Logic.Agreement"
    '            .CommandName = "addagreementline"
    '        End With
    '    End With
    'End Sub

    'Private Sub TEBillingConstAgreementDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillingConstAgreementDetail.CreatedChildControls
    '    With TEBillingConstAgreementDetail
    '        With .ActionBar.Button("Save")
    '            .ObjectDLL = "WMS.Logic.dll"
    '            .ObjectName = "WMS.Logic.Agreement"
    '            .CommandName = "addagreementline"
    '        End With
    '    End With
    'End Sub
End Class
