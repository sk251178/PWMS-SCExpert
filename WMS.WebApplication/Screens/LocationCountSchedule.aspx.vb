Public Class LocationCountSchedule
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents DC3 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMaster As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDetail1 As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDetail2 As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDetail3 As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDetail4 As Made4Net.WebControls.TableEditor
    Protected WithEvents DC4 As Made4Net.WebControls.DataConnector

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
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim t As New Made4Net.Shared.Translation.Translator()
        Select Case CommandName.ToLower
            Case "createcountjob"
                For Each dr In ds.Tables(0).Rows

                    Dim C As New WMS.Logic.Counting(WMS.Lib.TASKTYPE.LOCATIONCOUNTING, "", False)
                    If Not WMS.Logic.Counting.Exists(WMS.Lib.TASKTYPE.LOCATIONCOUNTING, dr("LOCATION")) Then

                        C.CreateLocationCountJobs(dr("WAREHOUSEAREA"), "", dr("LOCATION"), WMS.Lib.TASKTYPE.LOCATIONCOUNTING, "", "", WMS.Logic.GetCurrentUser)
                        Message = t.Translate("location count job created successfully")
                    End If

                Next

        End Select

    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub


    Protected Sub TEMaster_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEMaster.CreatedChildControls


        With TEMaster.ActionBar
            .AddExecButton("CreateCountJob", "Create Count Job", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("CreateCountJob")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.LocationCountSchedule"
                .CommandName = "CreateCountJob"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to Create Count Job?"
            End With
        End With



    End Sub

End Class