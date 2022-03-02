Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Data

Public Class OrdersAutomationShipmentAssignment

#Region "Members"

    'Filters
    Private mTemplateName As String
    Private mConsignee As String
    Private mOrderType As String
    'Private mPriority As Int32
    Private mCompany As String
    Private mCompanyType As String
    Private mFromRoute As String
    Private mToRoute As String
    Private mFromDoor As String
    Private mToDoor As String
    Private mCarrier As String
    Private mTransportMethod As String
    Private mOrderStatus As String

    ' Rules
    Private mSLAssigned As Boolean
    Private mMAxNumberOfOrders As Int32
    Private mBreakByRoute As Boolean

    ' Other Objects
    Private oLogger As LogHandler
    Private dtShipmentAssignmentFilter As DataTable
    Private dtOrdersToAssign As DataTable

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pTemplateName As String, ByVal pLogger As LogHandler)
        mTemplateName = pTemplateName
        oLogger = pLogger
        dtShipmentAssignmentFilter = New DataTable
        dtOrdersToAssign = New DataTable
    End Sub

#End Region

#Region "Orders Automation"

    Public Sub NotifyOrdersAutomation(ByVal pTemplateName As String)
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.AssignOutboundOrderToShipment
        em.Add("EVENT", EventType)
        em.Add("TEMPLATENAME", pTemplateName)
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

#End Region

#Region "Assign To Shipment"

    Public Sub AssigneOrdersToShipment()
        LoadPolicyTable()
        LoadFilteredOrders()
        BuildShipments()
    End Sub

    Private Sub LoadPolicyTable()
        If Not oLogger Is Nothing Then
            oLogger.Write("Loading Policy Line from ShipmentAssignment table...")
        End If
        Dim SQL As String
        'isnull(PRIORITY,0) as PRIORITY,
        'SQL = String.Format("SELECT isnull(CONSIGNEE,'') as CONSIGNEE, isnull(ORDERTYPE,'') as ORDERTYPE,  isnull(COMPANY,'%') as COMPANY, isnull(COMPANYTYPE,'%')as COMPANYTYPE, isnull(FROMROUTE,'%')as FROMROUTE, isnull(TOROUTE,'%') as TOROUTE, " & _
        '        " isnull(FROMDOOR,'%')as FROMDOOR, isnull(TODOOR,'%')TODOOR, isnull(CARRIER,'%')as CARRIER, isnull(TRANSMETHOD,'%')TRANSMETHOD, isnull(ORDERSTATUS,'%')as ORDERSTATUS, SLASSIGNED, MAXNUMOFORDERS " & _
        '        " FROM SHIPMENTASSIGNMENT WHERE TemplateName = '{0}'", mTemplateName)
        SQL = String.Format("select SLAssigned, MaxNumOfOrders,BreakByRoute from SHIPMENTASSIGNMENT WHERE TemplateName = '{0}'", mTemplateName)
        DataInterface.FillDataset(SQL, dtShipmentAssignmentFilter)
        If dtShipmentAssignmentFilter.Rows.Count = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("No matching policy line found, Exiting process...")
            End If
            'Start added by Mohanraj to move the messsage to dead letter queue since it has exception.
            Throw New Made4Net.Shared.M4NException(New Exception, "Template Name does not exists", "Template Name does not exists")
            'End added by Mohanraj to move the messsage to dead letter queue since it has exception.
        End If
        Dim dr As DataRow = dtShipmentAssignmentFilter.Rows(0)
        'If Not dr.IsNull("CONSIGNEE") Then mConsignee = dr.Item("CONSIGNEE")
        'If Not dr.IsNull("ORDERTYPE") Then mOrderType = dr.Item("ORDERTYPE")
        'If Not dr.IsNull("PRIORITY") Then mPriority = dr.Item("PRIORITY")
        'If Not dr.IsNull("COMPANY") Then mCompany = dr.Item("COMPANY")
        'If Not dr.IsNull("COMPANYTYPE") Then mCompanyType = dr.Item("COMPANYTYPE")
        'If Not dr.IsNull("FROMROUTE") Then mFromRoute = dr.Item("FROMROUTE")
        'If Not dr.IsNull("TOROUTE") Then mToRoute = dr.Item("TOROUTE")
        'If Not dr.IsNull("FROMDOOR") Then mFromDoor = dr.Item("FROMDOOR")
        'If Not dr.IsNull("TODOOR") Then mToDoor = dr.Item("TODOOR")
        'If Not dr.IsNull("CARRIER") Then mCarrier = dr.Item("CARRIER")
        'If Not dr.IsNull("TRANSMETHOD") Then mTransportMethod = dr.Item("TRANSMETHOD")
        'If Not dr.IsNull("ORDERSTATUS") Then mOrderStatus = dr.Item("ORDERSTATUS")
        If Not dr.IsNull("SLASSIGNED") Then mSLAssigned = dr.Item("SLASSIGNED")
        If Not dr.IsNull("MAXNUMOFORDERS") Then mMAxNumberOfOrders = dr.Item("MAXNUMOFORDERS")
        If Not dr.IsNull("BREAKBYROUTE") Then mBreakByRoute = dr.Item("BREAKBYROUTE")

        If Not oLogger Is Nothing Then
            oLogger.Write("Policy line loaded...")
        End If
    End Sub

    'Private Sub LoadFilteredOrders()
    '    If Not oLogger Is Nothing Then
    '        oLogger.Write("Loading Orders According to policy filter...")
    '    End If
    '    Dim SQL As String
    '    'SQL = String.Format("select * from vOrdersAutomation where company like '{0}' " & _
    '    '    " and companytype like '{1}' and status like '{2}' and route >= '{3}' and route <= '{4}' " & _
    '    '    " and door >= '{5}' and door <= '{6}' and carrier like '{7}' and transporttype like '{8}'", _
    '    '    mCompany, mCompanyType, mOrderStatus, mFromRoute, mToRoute, mFromDoor, mToDoor, mCarrier, mTransportMethod)

    '    SQL = String.Format("SELECT * FROM VORDERSAUTOMATION WHERE COMPANY LIKE '{0}' " & _
    '                        " AND COMPANYTYPE LIKE '{1}' AND STATUS LIKE '{2}' AND ROUTE >= '{3}' AND ROUTE <= '{4}' " & _
    '                        " AND TRANSPORTTYPE LIKE '{5}'", _
    '                        mCompany, mCompanyType, mOrderStatus, mFromRoute, mToRoute, mTransportMethod)

    '    If mConsignee <> String.Empty Then
    '        SQL += String.Format(" and consignee in ({0})", FormatStringInClause(mConsignee))
    '    End If
    '    If mOrderType <> String.Empty Then
    '        SQL += String.Format(" and ordertype in ({0})", FormatStringInClause(mOrderType))
    '    End If
    '    DataInterface.FillDataset(SQL, dtOrdersToAssign)
    '    If Not oLogger Is Nothing Then
    '        oLogger.Write(dtOrdersToAssign.Rows.Count & " Orders Loaded...")
    '    End If
    'End Sub

    Private Function FormatStringInClause(ByVal pStr As String) As String
        Dim retString As String = String.Empty
        Dim strArr() As String = pStr.Split(",")
        If strArr.Length = 0 Then
            Return String.Format("'{0}'", pStr)
        Else
            For i As Int32 = 0 To strArr.Length - 1
                retString += String.Format(",'{0}'", strArr(i))
            Next
            retString = retString.TrimStart(",".ToCharArray)
            Return retString
        End If
    End Function

    Private Sub BuildShipments()
        If Not oLogger Is Nothing Then
            oLogger.Write("Starting to build Shipments...")
        End If
        If dtOrdersToAssign.Rows.Count = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("No Orders to assign, Terminating Process...")
            End If
            Return
        End If
        Dim drOrders() As DataRow
        Dim sFilter As String
        'Before first, remove orders with 0 open qty.
        sFilter = "QTYOPEN > 0"
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to filter orders without open quantity...")
        End If
        'First - Filter Orders According to the rules
        ' 1) Check for SL Assigned Rule
        If mSLAssigned Then
            sFilter = String.Format("{0} and staginglane <> ''", sFilter)
            If Not oLogger Is Nothing Then
                oLogger.Write("Trying to filter only orders with Staging Lane...")
            End If
        End If
        drOrders = dtOrdersToAssign.Select(sFilter)
        If Not oLogger Is Nothing Then
            oLogger.Write("Total Orders after SL filter: " & drOrders.Length)
        End If
        If drOrders.Length = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("No Orders to assign, Terminating Process...")
            End If
            Return
        End If

        ' 2) Build Shipment/s
        '  Break by routes
        Dim ShipHT As Hashtable = breakByRoute(drOrders)

        'Dim shipmentAL As New ArrayList
        Dim MyKeys As ICollection
        Dim Key As Object
        MyKeys = ShipHT.Keys()
        For Each Key In MyKeys
            If Not oLogger Is Nothing Then
                oLogger.Write("Trying to build shipment for orders assigned to route: " & Key.ToString)
            End If
            'shipmentAL.AddRange(
            BuildShipment(ShipHT.Item(Key.ToString))
        Next

        'BuildShipments(drOrders)
    End Sub

    Private Sub BuildShipments(ByVal pOrderLines() As DataRow)
        Dim oShip As Shipment
        For i As Int32 = 0 To pOrderLines.Length - 1
            If i Mod mMAxNumberOfOrders = 0 Then
                oShip = New Shipment
                oShip.Create("", DateTime.Now, "", mCarrier, "", "", Nothing, "", "", "", "", "", "", mTransportMethod, "", "", "", WMS.Lib.USERS.SYSTEMUSER)
                If Not oLogger Is Nothing Then
                    oLogger.Write("Shipment " & oShip.SHIPMENT & "Created. Will assign all orders (up to " & mMAxNumberOfOrders & " per wave).")
                End If
            End If
            If Not oLogger Is Nothing Then
                oLogger.Write("Assigning order " & pOrderLines(i)("consignee") & ", " & pOrderLines(i)("orderid") & ", " & pOrderLines(i)("orderline") & " To Shipment")
            End If
            oShip.AssignOrder(pOrderLines(i)("consignee"), pOrderLines(i)("orderid"), pOrderLines(i)("orderline"), pOrderLines(i)("QTYOPEN"), "", WMS.Lib.USERS.SYSTEMUSER)
        Next
    End Sub

    Private Sub BuildShipment(ByVal pOrdersAL As ArrayList)
        Dim oShipment As Shipment
        For i As Int32 = 0 To pOrdersAL.Count - 1
            If i Mod mMAxNumberOfOrders = 0 Then
                oShipment = New Shipment()
                oShipment.Create("", DateTime.Now, "", mCarrier, "", "", Nothing, "", "", "", "", "", "", mTransportMethod, "", "", "", WMS.Lib.USERS.SYSTEMUSER)
                If Not oLogger Is Nothing Then
                    oLogger.Write("Shipment " & oShipment.SHIPMENT & "Created. Will assign all orders (up to " & mMAxNumberOfOrders & " per wave).")
                End If
            End If
            If Not oLogger Is Nothing Then
                oLogger.Write("Assigning order " & pOrdersAL(i)("consignee") & ", " & pOrdersAL(i)("orderid") & " To Shipment")
            End If
            oShipment.AssignOrder(pOrdersAL(i)("consignee"), pOrdersAL(i)("orderid"), pOrdersAL(i)("orderline"), _
            pOrdersAL(i)("QTYOPEN"), 0, WMS.Lib.USERS.SYSTEMUSER, pOrdersAL(i)("DOCUMENTTYPE"))
        Next
    End Sub

    Private Sub LoadFilteredOrders()
        If Not oLogger Is Nothing Then
            oLogger.Write("Loading Orders According to template filter...")
        End If
        Dim templateDataTable As New DataTable
        Dim sql As String = String.Format("Select * from shipmentassignmentdetail where templatename = {0}", Made4Net.Shared.FormatField(mTemplateName))
        DataInterface.FillDataset(sql, templateDataTable)
        Dim oaDataProvider As New OrdersAutomationDataProvider("vOrdersAutomationShipmentAssign", templateDataTable, oLogger)
        dtOrdersToAssign = oaDataProvider.ProvideData()
        If Not oLogger Is Nothing Then
            oLogger.Write(dtOrdersToAssign.Rows.Count & " Orders Loaded...")
        End If
    End Sub

    Private Function breakByRoute(ByVal drOrders() As DataRow) As Hashtable
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to Break by routes...")
        End If
        Dim mHT As New Hashtable
        Dim sRoute As String
        Dim tmpAL As ArrayList
        If Not mBreakByRoute Then
            tmpAL = New ArrayList
            'mHT.Add("Route", tmpAL)
            mHT.Add("ROUTE", tmpAL)
        End If
        For i As Int32 = 0 To drOrders.Length - 1
            If mBreakByRoute Then
                'Commented for RWMS-466
                'sRoute = drOrders(i)("route")
                'Added RWMS-466
                sRoute = drOrders(i)("route").ToString().ToLower()
                If mHT.Contains(sRoute) Then
                    'Add the current row to the shipment collection
                    tmpAL = mHT.Item(sRoute)
                    tmpAL.Add(drOrders(i))
                    mHT.Item(sRoute) = tmpAL
                Else
                    'Add new shipment bucket
                    tmpAL = New ArrayList()
                    tmpAL.Add(drOrders(i))
                    mHT.Add(sRoute, tmpAL)
                End If
            Else
                tmpAL = mHT.Item("ROUTE")
                tmpAL.Add(drOrders(i))
                mHT.Item("ROUTE") = tmpAL
            End If
        Next
        If Not oLogger Is Nothing Then
            oLogger.Write("Break by routes completed. Total shipments after break: " & mHT.Count)
        End If
        Return mHT
    End Function


#End Region

End Class
