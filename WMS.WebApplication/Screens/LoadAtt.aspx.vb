Imports WMS.Logic

Public Class LoadAtt
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TELoadAtt As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen

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
    End Sub

#Region "Ctor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        Dim _loadid As String = String.Empty
        Dim _consignee As String = String.Empty
        Dim _sku As String = String.Empty

        'Put user code to initialize the page here
        If CommandName.ToLower = "setattributes" Then
            Dim oAttribute As AttributesCollection
            For Each dr In ds.Tables(0).Rows
                Try
                    _loadid = dr("LOADID")
                    Dim ld As WMS.Logic.Load = New WMS.Logic.Load(_loadid)

                    Dim oSku As New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)
                    If Not oSku.SKUClass Is Nothing Then
                        oAttribute = WMS.Logic.SkuClass.ExtractLoadAttributes(oSku.SKUClass, dr)
                        ld.setAttributes(oAttribute, Common.GetCurrentUser)
                        Dim invAtt As New WMS.Logic.InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Load, ld.LOADID)
                        invAtt.Add(oAttribute)
                        invAtt.Save(WMS.Logic.GetCurrentUser)
                    End If
                Catch ex As Exception
                    Message = ex.Message
                End Try
            Next
        End If
    End Sub

#End Region





    Private Sub TELoadAtt_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TELoadAtt.CreatedChildControls
        With TELoadAtt
            With .ActionBar
                With .Button("Save")
                    '.ObjectDLL = "WMS.Logic.dll"
                    '.ObjectName = "WMS.Logic.Load"
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.LoadAtt"
                    .CommandName = "SetAttributes"
                End With
            End With
        End With
    End Sub

    Private Sub TELoadAtt_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TELoadAtt.AfterItemCommand
        TELoadAtt.RefreshData()
    End Sub

End Class
