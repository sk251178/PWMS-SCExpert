Public Class PinGrid
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    'Protected WithEvents ddInvAct As Made4Net.WebControls.DropDownList
    'Protected WithEvents lblSelectObject As Made4Net.WebControls.Label
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TECompanies As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDrivers As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDepots As Made4Net.WebControls.TableEditor

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
        Return Screen1.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Screen1.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'If Not IsPostBack Then
        'ddInvAct.DataBind()
        'ddInvAct_SelectedIndexChanged(Nothing, Nothing)
        'End If
    End Sub

    'Private Sub ddInvAct_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddInvAct.SelectedIndexChanged
        'If ddInvAct.SelectedValue = "COMP" Then
        'setCompanies()
        'ElseIf ddInvAct.SelectedValue = "DEPOT" Then
        '    setDepos()
        'ElseIf ddInvAct.SelectedValue = "DRIVERS" Then
        '    setDrivers()
        'End If
    '   Response.Write("<Script langauge=""vbscript"">" & vbCrLf)
    '    Response.Write("try {" & vbCrLf)
    '    Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdValCommand"").value='hide';" & vbCrLf)
    '    Response.Write("window.parent.frames(""frmViewData"").document.Form1.submit();" & vbCrLf)
    '    Response.Write("} catch (ex){}" & vbCrLf)
    '    Response.Write("</Script>" & vbCrLf)
    'End Sub

    Protected Sub setCompanies()
        'TEPinGrid.DefaultDT = "DTCompMan"
        'TEPinGrid.Restart()
        'Dim vals As New Specialized.NameValueCollection
        'vals.Add("OBJECTTYPE", "COMP")
        'TEPinGrid.PreDefinedValues = vals
    End Sub

    Protected Sub setDrivers()
        'TEPinGrid.DefaultDT = "DTDriver"
        'TEPinGrid.Restart()
        'Dim vals As New Specialized.NameValueCollection
        'vals.Add("OBJECTTYPE", "DRIVERS")
        'TEPinGrid.PreDefinedValues = vals
    End Sub

    Protected Sub setDepos()
        'TEPinGrid.DefaultDT = "DTDepot"
        'TEPinGrid.Restart()
        'Dim vals As New Specialized.NameValueCollection
        'vals.Add("OBJECTTYPE", "DEPOT")
        'TEPinGrid.PreDefinedValues = vals
    End Sub

    Private Sub TECompanies_CreatedChildControls1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TECompanies_CreatedGrid1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TECompanies.Grid.AddExecButton("Center", "Center", "WMS.Logic.dll", "WMS.Logic.TMSWebSupport", 2, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
    End Sub

    Private Sub TEDrivers_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs)
        'Dim tempds As DataTable = TEPinGrid.CreateDataTableForSelectedRecord(False)
        'Response.Write("<Script langauge=""vbscript"">" & vbCrLf)
        'Response.Write("try {" & vbCrLf)
        'Response.Write("window.parent.document.getElementById(""command"").value='pointitem';" & vbCrLf)
        'Select Case ddInvAct.SelectedValue
        '    Case "COMP"
        'Response.Write("try {" & vbCrLf)
        'Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdVal1"").value='" & tempds.Rows(0)("CONSIGNEE") & "';" & vbCrLf)
        'Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdVal2"").value='" & tempds.Rows(0)("COMPANY") & "';" & vbCrLf)
        'Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdVal3"").value='" & tempds.Rows(0)("COMPANYTYPE") & "';" & vbCrLf)
        'Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdObjectType"").value='COMP';" & vbCrLf)
        'Response.Write("} catch (ex){}" & vbCrLf)
        'Response.Write("window.parent.document.getElementById(""args"").value='" & tempds.Rows(0)("POINTID") & ",CMP," & tempds.Rows(0)("CONSIGNEE") & "/" & tempds.Rows(0)("COMPANY") & "/" & tempds.Rows(0)("COMPANYTYPE") & "';" & vbCrLf)

        '    Case "DEPOT"
        'Response.Write("try {" & vbCrLf)
        'Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdVal1"").value='" & tempds.Rows(0)("DEPOTNAME") & "';" & vbCrLf)
        'Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdObjectType"").value='DEPOT';" & vbCrLf)
        'Response.Write("} catch (ex){}" & vbCrLf)
        'Response.Write("window.parent.document.getElementById(""args"").value='" & tempds.Rows(0)("POINTID") & ",DPT," & tempds.Rows(0)("DEPOTNAME") & "';" & vbCrLf)
        '    Case "DRIVERS"
        'Response.Write("try {" & vbCrLf)
        'Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdVal1"").value='" & tempds.Rows(0)("DRIVERID") & "';" & vbCrLf)
        'Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdObjectType"").value='DRIVER';" & vbCrLf)
        'Response.Write("} catch (ex){}" & vbCrLf)
        'Response.Write("window.parent.document.getElementById(""args"").value='" & tempds.Rows(0)("POINTID") & ",DRV," & tempds.Rows(0)("DRIVERID") & "';" & vbCrLf)
        'End Select
        'Response.Write("window.parent.document.getElementById(""btnAct"").click();" & vbCrLf)
        'Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdValCommand"").value='show';" & vbCrLf)
        'Response.Write("window.parent.frames(""frmViewData"").document.Form1.submit();" & vbCrLf)


        'Response.Write("} catch (ex){}" & vbCrLf)
        'Response.Write("</Script>" & vbCrLf)
    End Sub


    Private Sub TECompanies_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECompanies.CreatedChildControls
        'Dim li As New Made4Net.WebControls.LinkImage
        'li.ImageUrl = Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnGeoCode")
        'li.OnClick = "ShowPoint()"
        'li.ToolTip = "GeoCode"
        'li.Style.Add("cursor", "hand")

        'Dim li2 As New Made4Net.WebControls.LinkImage
        'li2.ImageUrl = Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit")
        'li2.OnClick = "Clear()"
        'li2.ToolTip = "Clear"
        'li2.Style.Add("cursor", "hand")
        With TECompanies.ActionBar
            '.AddSpacer()
            .AddExecButton("geocode", "GeoCode", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnGeoCode"))
            With .Button("geocode")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Route"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
        End With
        'With TECompanies.ActionBar
        '.AddSpacer()
        '.AddControl(li)

        '.AddControl(li2)
        '.Button("save").Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        '.FindControl("btnCSVExport").Visible = False
        '.FindControl("btnChart").Visible = False
        'End With

    End Sub

    Private Sub TECompanies_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECompanies.CreatedGrid
        TECompanies.Grid.AddExecButton("Center", "Center", "WMS.Logic.dll", "WMS.Logic.TMSWebSupport", 2, Made4Net.WebControls.SkinManager.GetImageURL("GridColumnLocate"))
    End Sub

    Private Sub TEDepots_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDepots.CreatedChildControls
        With TEDepots
            '.FindControl("btnCSVExport").Visible = False
            '.FindControl("btnChart").Visible = False
        End With
    End Sub

    Private Sub TEDrivers_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDrivers.CreatedChildControls
        With TEDrivers
            ' .FindControl("btnCSVExport").Visible = False
            '.FindControl("btnChart").Visible = False
        End With
    End Sub

    
    Private Sub TEDepots_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDepots.CreatedGrid
        TEDepots.Grid.AddExecButton("Center", "Center", "WMS.Logic.dll", "WMS.Logic.TMSWebSupport", 2, Made4Net.WebControls.SkinManager.GetImageURL("GridColumnLocate"))
    End Sub

    Private Sub TEDrivers_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDrivers.CreatedGrid
        TEDrivers.Grid.AddExecButton("Center", "Center", "WMS.Logic.dll", "WMS.Logic.TMSWebSupport", 2, Made4Net.WebControls.SkinManager.GetImageURL("GridColumnLocate"))
    End Sub
End Class
