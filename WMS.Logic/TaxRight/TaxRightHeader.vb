Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports System.Web.UI.WebControls
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.[Shared].Net.FTP

Friend Class TaxRightHeader
    Inherits FixedWidthLine

    Private ReadOnly details As List(Of TaxRightDetail) = New List(Of TaxRightDetail)()
    Private ReadOnly significantFieldsCollection As Dictionary(Of String, String) = New Dictionary(Of String, String)(StringComparer.InvariantCultureIgnoreCase)

    Public Function Where(pckList As String) As String
        Return $"where picklist = '{pckList}'"
    End Function

    Public ReadOnly Iterator Property FixedWidthLines As IEnumerable(Of FixedWidthLine)
        Get
            If details.Any() Then
                Yield Me
                For Each line As FixedWidthLine In details
                    Yield line
                Next
            End If
        End Get
    End Property

    Public Sub New(pckList As String)
        Dim dtHeader As DataTable = New DataTable()
        DataInterface.FillDataset($"select * from TaxRightHeader {Where(pckList)}", dtHeader)

        If dtHeader.Rows.Count > 0 Then
            Dim dr As DataRow = dtHeader.Rows(0)
            StampingUnit = dr.Field(Of String)("StampingUnit")
            Address = dr.Field(Of String)("Address1")
            Address2 = dr.Field(Of String)("Address2")
            City = dr.Field(Of String)("City")
            CustomerName = dr.Field(Of String)("CustomerName")
            CustomerNumber = dr.Field(Of String)("CustomerNumber")
            DeliveryDate = dr.Field(Of Date?)("DeliveryDate")
            PhoneNumber = dr.Field(Of String)("PhoneNumber")
            RecordDesignator = "H"
            RouteName = dr.Field(Of String)("Route")
            RouteNumber = dr.Field(Of String)("Route")
            State = dr.Field(Of String)("State")
            StopNumber = dr.Field(Of Integer?)("StopNumber")
            ZipCode = dr.Field(Of String)("ZipCode")

            significantFieldsCollection.Add("StampingUnit", StampingUnit)
            significantFieldsCollection.Add("CustomerName", CustomerName)
            significantFieldsCollection.Add("CustomerNumber", CustomerNumber)
            significantFieldsCollection.Add("Route", RouteNumber)
            significantFieldsCollection.Add("State", State)
            significantFieldsCollection.Add("StopNumber", If(StopNumber.HasValue, Convert.ToString(StopNumber.Value, CultureInfo.InvariantCulture), ""))

            Dim dtDetail As DataTable = New DataTable()
            DataInterface.FillDataset($"select * from TaxRightDetail {Where(pckList)}", dtDetail)

            For Each detailDataRow As DataRow In dtDetail.Rows
                details.Add(New TaxRightDetail(detailDataRow))
            Next
            LineItems = details.Count
        End If
    End Sub

    Public ReadOnly Property SignificantFields As ReadOnlyDictionary(Of String, String)
        Get
            Return New ReadOnlyDictionary(Of String, String)(significantFieldsCollection)
        End Get
    End Property

    <FixedPositionField(StartsAt:=0, FieldWidth:=1)>
    Public Property RecordDesignator As String
    <FixedPositionField(StartsAt:=1, FieldWidth:=16)>
    Public Property StampingUnit As String
    <FixedPositionField(StartsAt:=17, FieldWidth:=30)>
    Public Property CustomerName As String
    <FixedPositionField(StartsAt:=47, FieldWidth:=10)>
    Public Property CustomerNumber As String
    <FixedPositionField(StartsAt:=57, FieldWidth:=14)>
    Public Property Jurisdiction As String
    <FixedPositionField(StartsAt:=71, FieldWidth:=16)>
    Public Property InvoiceNumber As String
    <FixedPositionField(StartsAt:=87, FieldWidth:=3)>
    Public Property LineItems As Integer?
    <FixedPositionField(StartsAt:=90, FieldWidth:=16)>
    Public Property SelectorID As String
    <FixedPositionField(StartsAt:=106, FieldWidth:=16)>
    Public Property Miscellaneous1 As String
    <FixedPositionField(StartsAt:=122, FieldWidth:=16)>
    Public Property Miscellaneous2 As String
    <FixedPositionField(StartsAt:=138, FieldWidth:=16)>
    Public Property Miscellaneous3 As String
    <FixedPositionField(StartsAt:=154, FieldWidth:=16)>
    Public Property Miscellaneous4 As String
    <FixedPositionField(StartsAt:=170, FieldWidth:=20, Justification:=JustificationType.Right)>
    Public Property RouteName As String
    <FixedPositionField(StartsAt:=190, FieldWidth:=5, Justification:=JustificationType.Right)>
    Public Property RouteNumber As String
    <FixedPositionField(StartsAt:=195, FieldWidth:=5)>
    Public Property StopNumber As Integer?
    <FixedPositionField(StartsAt:=200, FieldWidth:=25)>
    Public Property Address As String
    <FixedPositionField(StartsAt:=225, FieldWidth:=25)>
    Public Property Address2 As String
    <FixedPositionField(StartsAt:=250, FieldWidth:=25)>
    Public Property City As String
    <FixedPositionField(StartsAt:=275, FieldWidth:=2)>
    Public Property State As String
    <FixedPositionField(StartsAt:=277, FieldWidth:=10)>
    Public Property ZipCode As String
    <FixedPositionField(StartsAt:=287, FieldWidth:=20)>
    Public Property PhoneNumber As String
    <FixedPositionField(StartsAt:=307, FieldWidth:=10, CustomFormat:="MM/dd/yyyy")>
    Public Property DeliveryDate As Date?
    <FixedPositionField(StartsAt:=317, FieldWidth:=3)>
    Public Property NumberOfLabels As String
    <FixedPositionField(StartsAt:=320, FieldWidth:=25)>
    Public Property Comment1 As String
    <FixedPositionField(StartsAt:=345, FieldWidth:=25)>
    Public Property Comment2 As String
    <FixedPositionField(StartsAt:=370, FieldWidth:=30)>
    Public Property Miscellaneous5 As String
    <FixedPositionField(StartsAt:=400, FieldWidth:=30)>
    Public Property Miscellaneous6 As String
    <FixedPositionField(StartsAt:=430, FieldWidth:=30)>
    Public Property Miscellaneous7 As String
    <FixedPositionField(StartsAt:=460, FieldWidth:=30)>
    Public Property Miscellaneous8 As String
    <FixedPositionField(StartsAt:=490, FieldWidth:=30)>
    Public Property Miscellaneous9 As String
End Class
