Public Class Pick
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
	Protected WithEvents TEPick As Made4Net.WebControls.TableEditor

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

	Private Sub TEPick_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEPick.CreatedChildControls
		With TEPick
			With .ActionBar
                .AddSpacer()

                With .Button("Save")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Pick"
                    .CommandName = "AdjustPick"
                End With

                .AddExecButton("ApprovePicks", "Approve Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
                With .Button("ApprovePicks")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Pick"
                    .CommandName = "ApprovePick"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to approve the selected picks?"
                End With

                .AddExecButton("CancelPicks", "Cancel Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelPicks")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Pick"
                    .CommandName = "CancelPick"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the selected picks?"
                End With

                .AddExecButton("unallocatePicks", "Unallocate Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarUnAllocatePicks"))
                With .Button("unallocatePicks")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Pick"
                    .CommandName = "unallocatePick"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to unallocated the selected picks?"
                End With

                .AddExecButton("SplitPicks", "Split Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarSplitPicks"))
                With .Button("SplitPicks")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Pick"
                    .CommandName = "SplitPick"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to Split the selected picks?"
                End With
            End With
        End With
	End Sub

    Private Sub TEPick_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEPick.AfterItemCommand
        If e.CommandName = "ApprovePick" Or e.CommandName = "CancelPick" Or e.CommandName = "unallocatePick" Or e.CommandName = "SplitPick" Then
            TEPick.RefreshData()
        End If
    End Sub
End Class