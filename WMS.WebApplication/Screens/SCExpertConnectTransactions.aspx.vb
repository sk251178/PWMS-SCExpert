Imports WMS.Logic

Partial Public Class SCExpertConnectTransactions
    Inherits System.Web.UI.Page

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
        Dim oTranslator As New Made4Net.Shared.Translation.Translator
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "ResendTransaction"
                For Each dr In ds.Tables(0).Rows
                    Dim oSender As New Made4Net.Shared.QMsgSender
                    oSender.Add("Action", "TransmitTransaction")
                    oSender.Add("TransactionId", dr("TRANSACTIONID"))
                    oSender.Send("SCExpertConnect", "TransmitTransaction " & dr("TRANSACTIONID"))
                Next
                Message = oTranslator.Translate("Transactions sent to SCExpert Connect Service")
        End Select
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub TEConnectTrans_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEConnectTrans.CreatedChildControls
        With TEConnectTrans.ActionBar
            .AddSpacer()
            .AddExecButton("ResendTransaction", "Transmit Transaction", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("ResendTransaction")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.SCExpertConnectTransactions"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With
        End With
    End Sub
End Class