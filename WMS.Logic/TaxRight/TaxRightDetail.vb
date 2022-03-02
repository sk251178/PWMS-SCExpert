Imports Made4Net.Shared
Friend Class TaxRightDetail
    Inherits FixedWidthLine

    Public Sub New(dr As DataRow)
        If dr IsNot Nothing Then
            Barcode = dr.Field(Of String)("Barcode")
            CartonQuantity = dr.Field(Of Decimal?)("CartonQuantity")
            Description = dr.Field(Of String)("Description")
            LineNumber = dr.Field(Of Integer?)("LineNumber")
            PickBin = dr.Field(Of String)("PickBin")
            RecordDesignator = "D"
            UPC = dr.Field(Of String)("UPC")
        End If
    End Sub

    <FixedPositionField(StartsAt:=0, FieldWidth:=1)>
    Public Property RecordDesignator As String
    <RelativePositionField(StartsAfter:="RecordDesignator", FieldWidth:=3)>
    Public Property LineNumber As Integer?
    <RelativePositionField(StartsAfter:="LineNumber", FieldWidth:=12)>
    Public Property UPC As String
    <RelativePositionField(StartsAfter:="UPC", FieldWidth:=30)>
    Public Property Description As String
    <RelativePositionField(StartsAfter:="Description", FieldWidth:=4)>
    Public Property CartonQuantity As Decimal?
    <RelativePositionField(StartsAfter:="CartonQuantity", FieldWidth:=8)>
    Public Property PickBin As String
    <RelativePositionField(StartsAfter:="PickBin", FieldWidth:=20)>
    Public Property Barcode As String
End Class

