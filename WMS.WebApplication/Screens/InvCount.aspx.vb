Imports WMS.Logic
Imports Made4Net.DataAccess
Imports RWMS.Logic
Imports System.Xml

Public Class InvCount
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEINVCOUNT As Made4Net.WebControls.TableEditor

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
#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim msg As String
        Select Case CommandName.ToLower
            Case "invcount"
                Dim xmldoc As New XmlDocument
                xmldoc.LoadXml(XMLData)

                For Each dr In ds.Tables(0).Rows
                    Dim oSku As New WMS.Logic.SKU(dr("CONSIGNEE"), dr("SKU"))
                    Dim iunits As Integer
                    If Not IsDBNull(dr("ToQtyUOM")) AndAlso Not String.IsNullOrEmpty(dr("ToQtyUOM")) Then
                        iunits = oSku.ConvertToUnits(dr("ToQtyUOM"))
                    Else
                        iunits = oSku.ConvertToUnits(oSku.LOWESTUOM())
                    End If

                    If IsDBNull(dr("TOQTY")) OrElse String.IsNullOrEmpty(dr("TOQTY")) OrElse Not IsNumeric(dr("TOQTY")) Then
                        msg = t.Translate("Illegal value in TOQTYUNITS")
                        Throw New Made4Net.Shared.M4NException(New Exception, msg, msg)
                    End If

                    iunits = iunits * dr("TOQTY")

                    If xmldoc.SelectSingleNode("NewDataSet/Table1[LOADID='" & dr("LOADID") & "']/TOLOCATION").InnerText = "" Then
                        xmldoc.SelectSingleNode("NewDataSet/Table1[LOADID='" & dr("LOADID") & "']/TOLOCATION").InnerText = xmldoc.SelectSingleNode("NewDataSet/Table1[LOADID='" & dr("LOADID") & "']/LOCATION").InnerText
                    End If
                    xmldoc.SelectSingleNode("NewDataSet/Table1[LOADID='" & dr("LOADID") & "']/TOQTY").InnerText = iunits



                Next

                Dim LD1 As New WMS.Logic.Load(Sender, CommandName, XMLSchema, xmldoc.InnerXml, Message)

        End Select
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TEINVCOUNT_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEINVCOUNT.CreatedChildControls
        With TEINVCOUNT.ActionBar
            .AddSpacer()

            .AddExecButton("InvCount", "Count", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("InvCount")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Load"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InvCount"
                .CommandName = "InvCount"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure?"
            End With
        End With
    End Sub

    Private Sub TEINVCOUNT_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEINVCOUNT.AfterItemCommand
        If e.CommandName = "InvCount" Then
            TEINVCOUNT.RefreshData()
        End If
    End Sub


End Class
