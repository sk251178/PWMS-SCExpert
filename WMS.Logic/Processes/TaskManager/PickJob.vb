<CLSCompliant(False)> Public Class PickJob
    Public picklist As String
    Public fromload As String
    Public units As Decimal
    Public uomunits As Decimal
    Public adjustedunits As Decimal
    Public originaluom As String
    Public uom As String
    Public consingee As String
    Public sku As String
    Public skudesc As String
    Public fromlocation As String
    Public fromwarehousearea As String
    Public pickedqty As Double
    Public pickeduom As String
    Public parallelpicklistid As String
    Public parallelpicklistseq As Int32
    Public oAttributeCollection As AttributesCollection
    Public SystemPickShort As Boolean
    Public container As String
    Public oncontainer As String    'for parallel picking
    Public LabelPrinterName As String
    Public BasedOnPartPickedLine As Boolean
    Public PickDetLines As New System.Collections.Generic.List(Of Integer)
    Public TaskConfirmation As ITaskConfirmation

    Public oSku As WMS.Logic.SKU
End Class
