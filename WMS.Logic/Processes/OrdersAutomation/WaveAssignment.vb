Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Data

Public Class OrdersAutomationWaveAssignment

#Region "Members"

    'Filters
    Private mTemplateName As String
    'Private mConsignee As String
    'Private mOrderType As String
    'Private mPriority As Int32
    'Private mCompany As String
    'Private mCompanyType As String
    'Private mFromRoute As String
    'Private mToRoute As String
    'Private mFromDoor As String
    'Private mToDoor As String
    'Private mCarrier As String
    'Private mTransportMethod As String
    'Private mOrderStatus As String
    'Private mFromVolume As Decimal
    'Private mToVolume As Decimal
    'Private mFromRequestedDate As Int32
    'Private mToRequestedDate As Int32

    ' Rules
    Private mSLAssigned As Boolean
    Private mMAxNumberOfOrders As Int32
    Private mWaveType As String
    Private mPlanMethod As String
    'Private mBreakByRoute As Boolean
    Private mBreakByShipment As Boolean

    ' Other Objects
    Private oLogger As LogHandler
    Private dtWaveAssignmentFilter As DataTable
    Private dtOrdersToAssign As DataTable

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pTemplateName As String, ByVal pLogger As LogHandler)
        mTemplateName = pTemplateName
        oLogger = pLogger
        dtWaveAssignmentFilter = New DataTable
        dtOrdersToAssign = New DataTable
    End Sub

#End Region

#Region "Orders Automation Notifications"

    Public Sub NotifyOrdersAutomation(ByVal pTemplateName As String)
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.AssignOutboundOrderToWave
        em.Add("EVENT", EventType)
        em.Add("TEMPLATENAME", pTemplateName)
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

#End Region

#Region "Assign To Wave"

    Public Sub AssigneOrdersToWave()
        LoadPolicyTable()
        LoadFilteredOrders()
        BuildWave()
    End Sub

    Private Sub LoadPolicyTable()
        If Not oLogger Is Nothing Then
            oLogger.Write("Loading Policy Line from WaveAssignment table...")
        End If
        'Dim SQL As String
        'SQL = String.Format("SELECT isnull(CONSIGNEE,'') as CONSIGNEE, isnull(ORDERTYPE,'') as ORDERTYPE, isnull(COMPANY,'%') as COMPANY, isnull(COMPANYTYPE,'%')as COMPANYTYPE, isnull(FROMROUTE,'')as FROMROUTE, isnull(TOROUTE,'ZZZZZZZZZZZZ') as TOROUTE, " & _
        '        " isnull(FROMDOOR,'')as FROMDOOR, isnull(TODOOR,'ZZZZZZZZZZZZZZ') as TODOOR, isnull(CARRIER,'%')as CARRIER, isnull(TRANSMETHOD,'%')TRANSMETHOD, isnull(ORDERSTATUS,'%')as ORDERSTATUS, SLASSIGNED, MAXNUMOFORDERS, WAVETYPE, PLANMETHOD, BREAKBYSHIPMENT, " & _
        '        " isnull(FROMVOLUME,'99999999')as FROMVOLUME, isnull(TOVOLUME,'99999999') as TOVOLUME, isnull(FROMREQUESTEDDELIVERYDATE,'')as FROMREQUESTEDDELIVERYDATE, isnull(TOREQUESTEDDELIVERYDATE,'99999999') as TOREQUESTEDDELIVERYDATE " & _
        '        " FROM WAVEASSIGNMENT WHERE TemplateName = '{0}'", mTemplateName)
        'DataInterface.FillDataset(SQL, dtWaveAssignmentFilter)
        'If dtWaveAssignmentFilter.Rows.Count = 0 Then
        '    If Not oLogger Is Nothing Then
        '        oLogger.Write("No matching policy line found, Exiting process...")
        '    End If
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Template Name does not exists", "Template Name does not exists")
        'End If

        Dim sql As String = String.Format("select MAXNUMOFORDERS,WAVETYPE,PLANMETHOD,BREAKBYSHIPMENT from WAVEASSIGNMENT where templatename ='{0}'", mTemplateName)
        DataInterface.FillDataset(sql, dtWaveAssignmentFilter)
        If dtWaveAssignmentFilter.Rows.Count = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("No matching policy line found, Exiting process...")
            End If
            'Start added by Mohanraj to move the messsage to dead letter queue since it has exception.
            Throw New Made4Net.Shared.M4NException(New Exception, "Template Name does not exists", "Template Name does not exists")
            'End added by Mohanraj to move the messsage to dead letter queue since it has exception.
        End If


        Dim dr As DataRow = dtWaveAssignmentFilter.Rows(0)
        'If Not dr.IsNull("CONSIGNEE") Then mConsignee = dr.Item("CONSIGNEE")
        'If Not dr.IsNull("ORDERTYPE") Then mOrderType = dr.Item("ORDERTYPE")
        'If Not dr.IsNull("COMPANY") Then mCompany = dr.Item("COMPANY")
        'If Not dr.IsNull("COMPANYTYPE") Then mCompanyType = dr.Item("COMPANYTYPE")
        'If Not dr.IsNull("FROMROUTE") Then mFromRoute = dr.Item("FROMROUTE")
        'If Not dr.IsNull("TOROUTE") Then mToRoute = dr.Item("TOROUTE")
        'If Not dr.IsNull("FROMDOOR") Then mFromDoor = dr.Item("FROMDOOR")
        ''If Not dr.IsNull("TODOOR") Then mToDoor = dr.Item("TODOOR")
        'If Not dr.IsNull("CARRIER") Then mCarrier = dr.Item("CARRIER")
        'If Not dr.IsNull("TRANSMETHOD") Then mTransportMethod = dr.Item("TRANSMETHOD")
        ' If Not dr.IsNull("ORDERSTATUS") Then mOrderStatus = dr.Item("ORDERSTATUS")
        ' If Not dr.IsNull("SLASSIGNED") Then mSLAssigned = dr.Item("SLASSIGNED")
        If Not dr.IsNull("MAXNUMOFORDERS") Then mMAxNumberOfOrders = dr.Item("MAXNUMOFORDERS")
        If Not dr.IsNull("WAVETYPE") Then mWaveType = dr.Item("WAVETYPE")
        If Not dr.IsNull("PLANMETHOD") Then mPlanMethod = dr.Item("PLANMETHOD")
        If Not dr.IsNull("BREAKBYSHIPMENT") Then mBreakByShipment = dr.Item("BREAKBYSHIPMENT")
        'If Not dr.IsNull("FROMVOLUME") Then mFromVolume = dr.Item("FROMVOLUME")
        'If Not dr.IsNull("TOVOLUME") Then mToVolume = dr.Item("TOVOLUME")
        'If Not dr.IsNull("FROMREQUESTEDDELIVERYDATE") Then mFromRequestedDate = dr.Item("FROMREQUESTEDDELIVERYDATE")
        'If Not dr.IsNull("TOREQUESTEDDELIVERYDATE") Then mToRequestedDate = dr.Item("TOREQUESTEDDELIVERYDATE")
        If Not oLogger Is Nothing Then
            oLogger.Write("Policy line loaded...")
        End If
    End Sub

    'Private Sub LoadFilteredOrders()
    '    If Not oLogger Is Nothing Then
    '        oLogger.Write("Loading Orders According to policy filter...")
    '    End If
    '    Dim SQL As String
    '    SQL = String.Format("select * from vOrdersAutomation where company like '{0}' " & _
    '        " and companytype like '{1}' and status like '{2}' and route >= '{3}' and route <= '{4}' " & _
    '        " and door >= '{5}' and door <= '{6}'  and carrier like '{7}' and transporttype like '{8}' and ordervolume >= {9} and ordervolume <= {10} and datediff(dd,getdate(),requesteddate) >= {11} and datediff(dd,getdate(),requesteddate) <= {12}", _
    '        mCompany, mCompanyType, mOrderStatus, mFromRoute, mToRoute, mFromDoor, mToDoor, mCarrier, mTransportMethod, mFromVolume, mToVolume, mFromRequestedDate, mToRequestedDate)
    '    If Not oLogger Is Nothing Then
    '        oLogger.Write("Loading consignee filter")
    '    End If
    '    If mConsignee <> String.Empty Then
    '        SQL += String.Format(" and consignee in ({0})", FormatStringInClause(mConsignee))
    '    End If
    '    If Not oLogger Is Nothing Then
    '        oLogger.Write("Loading order type filter")
    '    End If
    '    If mOrderType <> String.Empty Then
    '        SQL += String.Format(" and ordertype in ({0})", FormatStringInClause(mOrderType))
    '    End If
    '    If Not oLogger Is Nothing Then
    '        oLogger.Write("Filtered Orders SQL " & SQL)
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

    Private Sub BuildWave()
        If Not oLogger Is Nothing Then
            oLogger.Write("Starting to build waves...")
        End If
        If dtOrdersToAssign.Rows.Count = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("No Orders to assign, Terminating Process...")
            End If
            Return
        End If
        Dim drOrders() As DataRow
        Dim sFilter As String
        'First - Filter Orders According to the rules

        ' 1) Check for SL Assigned Rule
        If mSLAssigned Then
            sFilter = " staginglane <> ''"
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

        ' 2) Break by shipments
        Dim ShipHT As Hashtable = BreakByShipment(drOrders)

        ' 3) Build Wave/s
        Dim WaveAL As New ArrayList
        Dim MyKeys As ICollection
        Dim Key As Object
        MyKeys = ShipHT.Keys()
        For Each Key In MyKeys
            If Not oLogger Is Nothing Then
                oLogger.Write("Trying to Build Waves for orders assigned to shipment: " & Key.ToString)
            End If
            WaveAL.AddRange(BuildWave(ShipHT.Item(Key.ToString)))
        Next

        ' 4) Check if Wave shoud be planned
        Select Case mPlanMethod
            Case WMS.Lib.Plan.PLANMODES.PLAN
                PlanWaves(WaveAL, False)
            Case WMS.Lib.Plan.PLANMODES.PLANANDRELEASE
                PlanWaves(WaveAL, True)
        End Select
    End Sub

    Private Sub PlanWaves(ByVal WaveAL As ArrayList, ByVal pDoRelease As Boolean)
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to Plan Waves:")
        End If
        For Each oWave As Wave In WaveAL
            If Not oLogger Is Nothing Then
                oLogger.Write("Planning wave " & oWave.WAVE & "...")
            End If
            oWave.Plan(pDoRelease, WMS.Lib.USERS.SYSTEMUSER)
        Next
    End Sub

    Private Function BuildWave(ByVal pOrdersAL As ArrayList) As ArrayList
        Dim WaveAL As New ArrayList
        Dim oWave As Wave
        For i As Int32 = 0 To pOrdersAL.Count - 1
            If i Mod mMAxNumberOfOrders = 0 Then
                oWave = New Wave
                oWave.Create("", mWaveType, WMS.Lib.USERS.SYSTEMUSER)
                WaveAL.Add(oWave)
                If Not oLogger Is Nothing Then
                    oLogger.Write("Wave " & oWave.WAVE & "Created. Will assign all orders (up to " & mMAxNumberOfOrders & " per wave).")
                End If
            End If
            If Not oLogger Is Nothing Then
                oLogger.Write("Assigning order " & pOrdersAL(i)("consignee") & ", " & pOrdersAL(i)("orderid") & " To Shipment")
            End If
            oWave.AssignOrderLine(pOrdersAL(i)("consignee"), pOrdersAL(i)("orderid"), pOrdersAL(i)("orderline"), False, WMS.Lib.USERS.SYSTEMUSER)
        Next
        Return WaveAL
    End Function

    Private Function BreakByShipment(ByVal drOrders() As DataRow) As Hashtable
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to Break by shipments...")
        End If
        Dim mHT As New Hashtable
        Dim sShipId As String
        Dim tmpAL As ArrayList
        If Not mBreakByShipment Then
            tmpAL = New ArrayList
            mHT.Add("SHIPID", tmpAL)
        End If
        For i As Int32 = 0 To drOrders.Length - 1
            If mBreakByShipment Then
                sShipId = drOrders(i)("shipment")
                If mHT.Contains(sShipId) Then
                    'Add the current row to the shipment collection
                    tmpAL = mHT.Item(sShipId)
                    tmpAL.Add(drOrders(i))
                    mHT.Item(sShipId) = tmpAL
                Else
                    'Add new shipment bucket
                    tmpAL = New ArrayList()
                    tmpAL.Add(drOrders(i))
                    mHT.Add(sShipId, tmpAL)
                End If
            Else
                tmpAL = mHT.Item("SHIPID")
                tmpAL.Add(drOrders(i))
                mHT.Item("SHIPID") = tmpAL
            End If
        Next
        If Not oLogger Is Nothing Then
            oLogger.Write("Break by shipments completed. Total shipments after break: " & mHT.Count)
        End If
        Return mHT
    End Function

    Private Sub LoadFilteredOrders()
        If Not oLogger Is Nothing Then
            oLogger.Write("Loading Orders According to template filter...")
        End If
        Dim templateDataTable As New DataTable
        Dim sql As String = String.Format("Select * from waveassignmentdetail where templatename = {0}", Made4Net.Shared.FormatField(mTemplateName))
        DataInterface.FillDataset(sql, templateDataTable)
        Dim oaDataProvider As New OrdersAutomationDataProvider("vOrdersAutomationWaveAssign", templateDataTable, oLogger)
        dtOrdersToAssign = oaDataProvider.ProvideData()
        If Not oLogger Is Nothing Then
            oLogger.Write(dtOrdersToAssign.Rows.Count & " Orders Loaded...")
        End If
    End Sub

#End Region

End Class
