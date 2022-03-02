Imports Made4Net.DataAccess
Imports Made4Net.Algorithms.ShortestPath
Imports Made4Net.Algorithms
Imports Made4Net.Shared
Imports Made4Net.Shared.Conversion.Convert 'RWMS-1212
'Imports WMS.Logic.CalcDistances   


Public Class WHMapEdges
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents TEWHNodes As Made4Net.WebControls.TableEditor

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

    Public Sub New()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)

        Select Case CommandName.ToLower
            'Start RWMS-1212 
            Case "newedge"
                dr = ds.Tables(0).Rows(0)
                Dim objCalcDistance As New WMS.Logic.CalcDistances
                objCalcDistance.CreateEdge(ReplaceDBNull(dr("FROMNODEID")), ReplaceDBNull(dr("TONODEID")), ReplaceDBNull(dr("DISTANCE")), ReplaceDBNull(dr("EDGETYPE")), WMS.Logic.Common.GetCurrentUser, ReplaceDBNull(dr("CLEARANCE")))
            Case "editedge"
                dr = ds.Tables(0).Rows(0)
                Dim objCalcDistance As New WMS.Logic.CalcDistances(dr("EDGE_ID"))
                objCalcDistance.UpdateEdge(ReplaceDBNull(dr("DISTANCE")), ReplaceDBNull(dr("EDGETYPE")), WMS.Logic.Common.GetCurrentUser, ReplaceDBNull(dr("CLEARANCE")))
                'End RWMS-1212 
            Case "shortestpathcalc"
                Dim inb As New WMS.Logic.CalcDistances(Sender, CommandName, Message)
            Case "distcalc"
                Dim inb As New WMS.Logic.CalcDistances(Sender, CommandName, Message)

        End Select

    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEWHNodes_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEWHNodes.CreatedChildControls

        'Commented by Surya for button only need to be fired when clicked - Start

        'With TEWHNodes.ActionBar
        '    .AddExecButton("DistCalc", "Distance Calucation", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        '    With .Button("DistCalc")
        '        Dim x As Integer
        '        x = calcEdgeDistances()
        '    End With
        '    '.AddExecButton("Clear", "Clear", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        '    'Dim SQL0 As String
        '    'SQL0 = String.Format("UPDATE WAREHOUSEMAPEDGES SET DISTANCE=0")
        '    'DataInterface.RunSQL(SQL0)
        '    'Made4Net.DataAccess.DataInterface.RunSQL(SQL0)
        'End With

        'Commented by Surya for button only need to be fired when clicked - End
        ''Start RWMS-1212
        With TEWHNodes.ActionBar
            .AddExecButton("DistCalc", "Edge Distance Calculation", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("DistCalc")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WHMapEdges"
                .CommandName = "DistCalc"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Normal
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to recalculate Edge Distance?"

            End With

            'End With

            'With TEWHNodes.ActionBar
            .AddExecButton("ShortestPathCalc", "Shortest Path Calculation", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("ShortestPathCalc")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.CalcDistances"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WHMapEdges"
                .CommandName = "ShortestPathCalc"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Normal
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to recalculate shortest distances?"
            End With

            With .Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WHMapEdges"
                If TEWHNodes.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "newedge"
                ElseIf TEWHNodes.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "editedge"
                End If
            End With
            ''End RWMS-1212
        End With

    End Sub

    'Commented By Surya As We have moved it to Wms.logic Start

    'Public Function calcEdgeDistances() As Integer

    '    Dim dt1 As New DataTable
    '    Dim dt As New DataTable
    '    Dim dr As DataRow
    '    dt = New DataTable()
    '    dt1 = New DataTable()
    '    Dim SQL1 As String
    '    Dim SQL As String

    '    'SQL = String.Format(" WITH CTE AS ( SELECT FROMNODEID, TONODEID, B.XCOORDINATE, B.YCOORDINATE FROM WAREHOUSEMAPEDGES A" & _
    '    '                   " INNER JOIN WAREHOUSEMAPNODES B ON ( A.FROMNODEID = B.NODEID ) OR ( A.TONODEID = B.NODEID ))" & _
    '    '                     "SELECT DISTINCT B.FROMNODEID,B.TONODEID," & _
    '    '                                      "SUBSTRING( STUFF(( SELECT ';' + CAST(XCOORDINATE AS CHAR(5)) + ';' + CAST(YCOORDINATE AS CHAR(5) ) FROM CTE A1 WHERE A1.FROMNODEID = B.FROMNODEID AND A1.TONODEID = B.TONODEID FOR XML PATH('')), 1,1, '') , 1, 5) AS X1," & _
    '    '                                      "SUBSTRING( STUFF(( SELECT ';' + CAST(XCOORDINATE AS CHAR(5)) + ';' + CAST(YCOORDINATE AS CHAR(5) ) FROM CTE A1 WHERE A1.FROMNODEID = B.FROMNODEID AND A1.TONODEID = B.TONODEID FOR XML PATH('')), 1,1, '') , 5, 5) AS Y1," & _
    '    '                                      "SUBSTRING( STUFF(( SELECT ';' + CAST(XCOORDINATE AS CHAR(5)) + ';' + CAST(YCOORDINATE AS CHAR(5) ) FROM CTE A1 WHERE A1.FROMNODEID = B.FROMNODEID AND A1.TONODEID = B.TONODEID FOR XML PATH('')), 1,1, '') , 9, 5) AS X2," & _
    '    '                                      "SUBSTRING( STUFF(( SELECT ';' + CAST(XCOORDINATE AS CHAR(5)) + ';' + CAST(YCOORDINATE AS CHAR(5) ) FROM CTE A1 WHERE A1.FROMNODEID = B.FROMNODEID AND A1.TONODEID = B.TONODEID FOR XML PATH('')), 1,1, '') , 13, 5) AS Y2" + " " + " FROM CTE B")

    '    SQL = String.Format("SELECT * FROM ( SELECT  B.FROMNODEID, B.XCOORDINATE AS X1, B.YCOORDINATE AS Y1, C.TONODEID, C.XCOORDINATE AS X2, C.YCOORDINATE AS Y2 FROM WAREHOUSEMAPEDGES A" & _
    '                         " LEFT JOIN ( SELECT DISTINCT FROMNODEID, XCOORDINATE, YCOORDINATE FROM  WAREHOUSEMAPEDGES A" & _
    '                         " JOIN WAREHOUSEMAPNODES B ON A.FROMNODEID = B.NODEID ) AS B ON B.FROMNODEID = A.FROMNODEID" & _
    '                         " LEFT JOIN ( SELECT DISTINCT TONODEID, XCOORDINATE, YCOORDINATE FROM  WAREHOUSEMAPEDGES A JOIN  WAREHOUSEMAPNODES B ON    A.TONODEID = B.NODEID ) AS C ON C.TONODEID = A.TONODEID ) MapEdgePoints where MapEdgePoints.FROMNODEID IS NOT NULL AND MapEdgePoints.TONODEID IS NOT NULL")

    '    DataInterface.FillDataset(SQL, dt1)

    '    If (dt1.Rows.Count = 0) Then
    '        Return -1
    '    Else

    '        For Each dr In dt1.Rows

    '            Dim dist As Double
    '            dist = WHQueryPath.CalcDistance(Convert.ToInt32(dr("X1")),
    '                                       Convert.ToInt32(dr("Y1")),
    '                                       Convert.ToInt32(dr("X2")),
    '                                       Convert.ToInt32(dr("Y2")))

    '            SQL1 = String.Format("UPDATE WAREHOUSEMAPEDGES SET DISTANCE={0} WHERE FROMNODEID='{1}' and TONODEID='{2}'", dist, dr("FROMNODEID"), dr("TONODEID"))
    '            DataInterface.RunSQL(SQL1)
    '            Made4Net.DataAccess.DataInterface.RunSQL(SQL1)

    '        Next

    '    End If


    '    Return 1



    'End Function

    'Commented By Surya As We have moved it to Wms.logic End
End Class
