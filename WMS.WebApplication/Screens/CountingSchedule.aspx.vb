Imports WMS.Logic
Imports System.Data
Imports Made4Net.Shared
Imports Made4Net.DataAccess

Public Class CountingSchedule
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TECountingSched As Made4Net.WebControls.TableEditor

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
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim userid As String = Common.GetCurrentUser()
        Select Case CommandName.ToUpper
            Case "CreateLoadCountJobs".ToUpper
                Dim dr As DataRow = ds.Tables(0).Rows(0)
                If CreateLoadCountJobs(Conversion.Convert.ReplaceDBNull(dr("consignee")), Conversion.Convert.ReplaceDBNull(dr("location")), Conversion.Convert.ReplaceDBNull(dr("warehousearea")), Conversion.Convert.ReplaceDBNull(dr("sku")), Conversion.Convert.ReplaceDBNull(dr("loadid")), Conversion.Convert.ReplaceDBNull(dr("Supplier")), userid) > 0 Then
                    Message = "Load Count Tasks Created"
                Else
                    Throw New M4NException(New Exception(), "No load count tasks created. No loads found.", "No load count tasks created. No loads found.")
                End If

        End Select
    End Sub

#End Region

#Region "Ctor Methods"

    Public Function CreateLoadCountJobs(ByVal pConsignee As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pSku As String, ByVal pLoad As String, ByVal pSupplier As String, ByVal pUser As String) As Integer
        Dim SQL As String = String.Empty
        Dim ds As New DataSet
        Dim dr As DataRow

        'SQL = String.Format("select Invload.* from Invload left outer join receiptdetail rd on rd.consignee = invload.consignee and rd.sku = invload.sku " & _
        '                    "left outer join inboundordheader ih on ih.consignee = rd.consignee and ih.orderid = rd.orderid " & _
        '                    "where Invload.loadid like '{0}%' and Invload.consignee like '{1}%' and Invload.sku like '{2}%' and Invload.location like '{3}%' " & _
        '                    "and Invload.warehousearea like '{4}%' and (activitystatus is null or activitystatus ='') and invload.status<>'LIMBO' and isnull(ih.sourcecompany,'') like '{5}%'", _
        '                    pLoad, pConsignee, pSku, pLocation, pWarehousearea, pSupplier)



        SQL = String.Format("select * from Invload where loadid like '{0}%' and consignee like '{1}%' and sku like '{2}%' and location like '{3}%' and warehousearea like '{4}%' and (activitystatus is null or activitystatus ='') and status<>'LIMBO'", _
                           pLoad, pConsignee, pSku, pLocation, pWarehousearea)



        DataInterface.FillDataset(SQL, ds)

        For Each dr In ds.Tables(0).Rows
            Dim oCounting As New WMS.Logic.Counting
            oCounting.Post(WMS.Lib.TASKTYPE.LOADCOUNTING, dr("loadid"), dr("location"), dr("warehousearea"), dr("consignee"), dr("sku"), "", "", pUser)
            'Commented for RWMS-600
            'oCounting = Nothing
            'Exit For
            'End Commented for RWMS-600
        Next
        'Uncommented for RWMS-600/601 Start
        Return ds.Tables(0).Rows.Count
        'Uncommented for RWMS-600/601 End

        'Commented for RWMS-600/601 Start
        'Return 1
        'Commented for RWMS-600/601 End
    End Function

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub


    Private Sub TECountingSched_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECountingSched.CreatedChildControls
        With TECountingSched
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.CountingSchedule"
                .CommandName = "CreateLoadCountJobs"
            End With
        End With
    End Sub

End Class
