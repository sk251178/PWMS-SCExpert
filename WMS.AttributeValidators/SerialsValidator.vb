Imports Made4Net.DataAccess
Imports WMS.Logic

<CLSCompliant(False)> Public Class SerialsValidator
    Implements WMS.Logic.IPickingAttributeValidator

    Public Sub New()

    End Sub

    Public Function Validate(ByVal oPickListHeader As Logic.Picklist, ByVal iPickListLine As Integer, ByVal iUnits As Double, ByVal sUserId As String, ByVal sAttributeName As String, ByVal oAttributeValue As Object, ByVal oAttributeCollection As Logic.AttributesCollection) As Boolean Implements Logic.IPickingAttributeValidator.Validate
        Dim SQL As String
        Dim dtLoads As New DataTable, dtAtt As New DataTable
        Dim dr As DataRow
        Dim sAtt As String
        Dim oPickDetail As WMS.Logic.PicklistDetail

        'get all serials of the 
        oPickDetail = oPickListHeader.Lines.PicklistLine(iPickListLine)
        SQL = String.Format("Select distinct toload from pickdetail where orderid = '{0}' and consignee = '{1}' and sku = '{2}' and  status<> 'canceled'", _
            oPickDetail.OrderId, oPickDetail.Consignee, oPickDetail.SKU)
        DataInterface.FillDataset(SQL, dtLoads)
        For Each dr In dtLoads.Rows
            Try
                Dim oLoad As New Load(Convert.ToString(dr("toload")))
                sAtt = sAtt & "," & oLoad.LoadAttributes.Attribute("SERIAL")
            Catch ex As Exception
            End Try
        Next
        If sAtt <> "" Then
            'now check if one of the current pick's serials is in the previous picks serials
            Dim serArr() As String = CType(oAttributeValue, String).Split(",")
            Dim idx As Int32
            For idx = 0 To serArr.Length - 1
                If sAtt.IndexOf(serArr(idx)) > 0 Then
                    Throw New ApplicationException("Serial of same item cannot be scanned twice!")
                End If
            Next
        End If
        Return True
    End Function

End Class
