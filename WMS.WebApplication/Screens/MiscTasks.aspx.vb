Imports Made4Net.Shared
Imports Made4Net.General.Helpers

Public Class MiscTasks
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMiscTasks As Made4Net.WebControls.TableEditor

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

    Sub New()

    End Sub

    Public Property SessionLogger As ILogHandler
        Get
            Dim val As ILogHandler = HttpContext.Current.Session("WMSLogger")
            Return val
        End Get
        Set(value As ILogHandler)
            If value Is Nothing Then
                HttpContext.Current.Session.Remove("WMSLogger")
            Else
                HttpContext.Current.Session("WMSLogger") = value
            End If
        End Set
    End Property

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet

        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)

        If SessionLogger IsNot Nothing Then
            SessionLogger.SafeWrite("Inbound Data:")
            ds.Tables(0).ToLog(AddressOf SessionLogger.Write, AddressOf SessionLogger.writeSeperator)
        End If

        Select Case CommandName.ToLower
            Case "insert"
                Try
                    Dim oTsk As New WMS.Logic.MiscTask()
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Dim stdtime As TimeSpan = CType(Conversion.Convert.ReplaceDBNull(dr("STDTIME_HHMMSS")), String).FromHHMMSS()
                    oTsk.ADDUSER = UserId
                    oTsk.EDITUSER = UserId
                    oTsk.PRIORITY = Conversion.Convert.ReplaceDBNull(dr("PRIORITY"))
                    oTsk.STDTIME = stdtime.TotalSeconds
                    oTsk.TASKSUBTYPE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TASKSUBTYPE"))
                    oTsk.TASKTYPE = WMS.Lib.TASKTYPE.MISC
                    oTsk.Create()
                    If Not Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("userid")) Is Nothing And _
                    Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("userid")) <> "" Then
                        oTsk.AssignUser(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("userid")))
                    End If
                Catch ex As Exception
                    Throw New Made4Net.Shared.M4NException(New Exception, "Could not add task.", "Could not add task.")
                End Try
            Case "update"
                Try
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Dim oTsk As New WMS.Logic.MiscTask(dr("task"))
                    Dim stdtime As TimeSpan = CType(Conversion.Convert.ReplaceDBNull(dr("STDTIME_HHMMSS")), String).FromHHMMSS()
                    oTsk.EDITUSER = UserId
                    oTsk.PRIORITY = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRIORITY"))
                    oTsk.STDTIME = stdtime.TotalSeconds
                    oTsk.TASKSUBTYPE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TASKSUBTYPE"))
                    oTsk.Update()
                Catch ex As Exception
                    Throw New Made4Net.Shared.M4NException(New Exception, "Could not edit task.", "Could not edit task.")
                End Try

        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Protected Sub TEMiscTasks_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEMiscTasks.CreatedChildControls
        With TEMiscTasks.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebApp.DLL"
            .ObjectName = "WMS.WebApp.MiscTasks"
            If TEMiscTasks.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "insert"
            ElseIf TEMiscTasks.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "update"
            End If
        End With
    End Sub
End Class