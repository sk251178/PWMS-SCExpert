Public Partial Class HandlingUnitStorageTemplate
    Inherits System.Web.UI.Page

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    'Added for PWMS-418 Start

#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower

            Case "deletehut"
                dr = ds.Tables(0).Rows(0)
                If Not WMS.Logic.Utils.deleteHandlingUnitStorage(dr("HUSTORAGETEMPLATEID"), Message) Then
                    Throw New ApplicationException(Message)
                End If

                WMS.Logic.HandelingUnit.Delete(dr("HUSTORAGETEMPLATEID"))

        End Select
    End Sub

#End Region
    ' Added for PWMS-418 End

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    'Added for PWMS-418 Start
    Private Sub TEMasterHandlingUnitStorageTemplate_CreatedChildControls(sender As Object, e As EventArgs) Handles TEMasterHandlingUnitStorageTemplate.CreatedChildControls
        With TEMasterHandlingUnitStorageTemplate.ActionBar
            If TEMasterHandlingUnitStorageTemplate.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                With .Button("Delete")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.HandlingUnitStorageTemplate"
                    .CommandName = "deletehut"
                End With

            End If


        End With
    End Sub
    ' Added for PWMS-418 End

End Class