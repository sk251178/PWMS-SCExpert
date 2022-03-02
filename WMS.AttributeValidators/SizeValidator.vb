<CLSCompliant(False)> Public Class SizeValidator
    Implements WMS.Logic.IReceivingAttributeValidator

    Public Sub New()

    End Sub

    Public Function Validate(ByVal oReceipt As WMS.Logic.ReceiptHeader, ByVal iReceiptLine As Integer, ByVal sUom As String, ByVal sLocation As String, ByVal iUnits As Double, ByVal sLoadStatus As String, ByVal sStatReasonCode As String, ByVal sUserId As String, ByVal sAttributeName As String, ByVal oAttributeValue As Object, ByVal oAttributeCollection As Logic.AttributesCollection) As Boolean Implements Logic.IReceivingAttributeValidator.Validate
        If oAttributeValue Is Nothing Then
            Throw New ApplicationException("Value of attribute size cannot be empty")
        End If
        Dim oSizeValue As String = Convert.ToString(oAttributeValue).ToUpper
        If oSizeValue = "BL" Then
            Throw New ApplicationException("Value of attribute size cannot be ""BL""")
        End If
        Return True
    End Function
End Class
