<CLSCompliant(False)> Public Interface IReceivingAttributeValidator
    Function Validate(ByVal oReceipt As ReceiptHeader, ByVal iReceiptLine As Int32, ByVal sUom As String, ByVal sLocation As String, ByVal iUnits As Double, ByVal sLoadStatus As String, ByVal sStatReasonCode As String, ByVal sUserId As String, ByVal sAttributeName As String, ByVal oAttributeValue As Object, ByVal oAttributeCollection As AttributesCollection) As Boolean

End Interface

<CLSCompliant(False)> Public Interface IPickingAttributeValidator
    Function Validate(ByVal oPickListHeader As Picklist, ByVal iPickListLine As Int32, ByVal iUnits As Double, ByVal sUserId As String, ByVal sAttributeName As String, ByVal oAttributeValue As Object, ByVal oAttributeCollection As AttributesCollection) As Boolean

End Interface

<CLSCompliant(False)> Public Interface IVerificationAttributeValidator
    Function Validate(ByVal oLoad As Load, ByVal iUnits As Double, ByVal sUserId As String, ByVal sAttributeName As String, ByVal oAttributeValue As Object, ByVal oAttributeCollection As AttributesCollection) As Boolean

End Interface

<CLSCompliant(False)> Public Interface ICountingAttributeValidator
    Function Validate(ByVal oLoad As Load, ByVal iUnits As Double, ByVal sUserId As String, ByVal sAttributeName As String, ByVal oAttributeValue As Object, ByVal oAttributeCollection As AttributesCollection) As Boolean

End Interface