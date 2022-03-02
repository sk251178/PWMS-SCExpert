Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

<CLSCompliant(False)>
Public Class AgreementDetailCollection
    Inherits ArrayList

#Region "Variables"

    Protected _agreement As Agreement

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property item(ByVal index As Int32) As AgreementDetail
        Get
            Return CType(MyBase.Item(index), AgreementDetail)
        End Get
    End Property

    Public ReadOnly Property Line(ByVal pLine As Int32) As AgreementDetail
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If item(i).LineNumber = pLine Then
                    Return (CType(MyBase.Item(i), AgreementDetail))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByRef oAgreement As Agreement)
        _agreement = oAgreement
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = String.Format("Select LINE,TRANTYPE from BILLINGAGREEMENTDETAIL where AGREEMENTNAME = {0} and Consignee = {1}", Made4Net.Shared.Util.FormatField(_agreement.Name), Made4Net.Shared.Util.FormatField(_agreement.Consignee))
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Dim agd As AgreementDetail = AgreementDetail.getAgreementLine(_agreement, dr("line"), dr("trantype"))
            If Not agd Is Nothing Then Me.add(agd)
        Next
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function add(ByVal pObject As AgreementDetail) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As AgreementDetail)
        MyBase.Insert(index, Value)
    End Sub

    Public Shadows Sub Remove(ByVal pObject As AgreementDetail)
        MyBase.Remove(pObject)
    End Sub

    Public Sub SaveLine(ByVal pLine As Int32, ByVal pTrantype As AgreementDetail.TransactionTypes, ByVal pDocumenttype As String _
            , ByVal pUom As String, ByVal pBillBasis As AgreementDetail.BillingBasis, ByVal pUnitPrice As Double, ByVal pIsPerc As Boolean, ByVal pPerc As Double, ByVal pUsePrice As Boolean _
            , ByVal pPriceList As String, ByVal pCurrency As String, ByVal pMinPerTran As Double, ByVal pMaxPerTran As Double, ByVal pMinPerRun As Double, ByVal pMaxPerRun As Double _
            , ByVal pPeriodType As PeriodTypes, ByVal pPeriod As String, ByVal pStartdate As DateTime, ByVal pActive As Boolean, ByVal pUser As String, ByVal pPriceFactorIndex As String, ByVal pSkuGroup As String, ByVal pEndDate As DateTime _
            , ByVal pStoragePeriodTime As Int32, ByVal pStoragePeriodType As PeriodTypes, ByVal pRunCondition As String, ByVal pChargeDescription As String _
            , ByVal pHandlingUnitType As String, ByVal pStoragePartialPeriod As Boolean, ByVal pIsStorageRange As Boolean, Optional ByVal AdditionalTranType As String = Nothing)

        Dim ad As AgreementDetail
        Dim LineExist As Boolean = System.Convert.ToBoolean(DataInterface.ExecuteScalar("SELECT COUNT(1) FROM BILLINGAGREEMENTDETAIL WHERE AGREEMENTNAME='" & _agreement.Name & "' AND LINE='" & pLine & "'"))
        If Not LineExist Then
            Select Case (pTrantype)
                Case AgreementDetail.TransactionTypes.Inbound
                    ad = New InboundAgreementDetail(_agreement)
                Case AgreementDetail.TransactionTypes.Outbound
                    ad = New OutboundAgreementDetail(_agreement)
                    'Case AgreementDetail.TransactionTypes.Constant
                    '    ad = New ConstantAgreementDetail(_agreement)
                    'Case AgreementDetail.TransactionTypes.Storage
                    '    ad = New StorageAgreementDetail(_agreement)
                    'Case Else
                    '    ad = New AdditionalAgreementDetail(_agreement)
                    '    CType(ad, AdditionalAgreementDetail).AdditionalTransactionType = AdditionalTranType
            End Select
            ad.LastRunDate = pStartdate

        Else
            Select Case (pTrantype)
                Case AgreementDetail.TransactionTypes.Inbound
                    ad = New InboundAgreementDetail(_agreement, pLine)
                Case AgreementDetail.TransactionTypes.Outbound
                    ad = New OutboundAgreementDetail(_agreement, pLine)
                    'Case AgreementDetail.TransactionTypes.Constant
                    '    ad = New ConstantAgreementDetail(_agreement, pLine)
                    'Case AgreementDetail.TransactionTypes.Storage
                    '    ad = New StorageAgreementDetail(_agreement, pLine)
                    'Case Else
                    '    ad = New AdditionalAgreementDetail(_agreement, pLine)
                    '    CType(ad, AdditionalAgreementDetail).AdditionalTransactionType = AdditionalTranType
            End Select
        End If



        ad.LineNumber = pLine
        ad.TransactionType = pTrantype
        ad.DocumentType = pDocumenttype
        ad.UOM = pUom
        ad.BILLBASIS = pBillBasis
        ad.PRICEPERUNIT = pUnitPrice
        ad.IsPercentage = pIsPerc
        ad.Percentage = pPerc
        ad.UsePriceList = pUsePrice
        If pUsePrice And Not pPriceList Is Nothing Then
            ad.PriceList = New PriceList(pPriceList)
        End If
        ad.Currency = pCurrency
        ad.MinimumPerTran = pMinPerTran
        ad.MaximumPerTran = pMaxPerTran

        ad.MaximumPerRun = pMaxPerRun
        ad.MinimumPerRun = pMinPerRun

        ad.PriceFactorIndex = pPriceFactorIndex
        ad.SkuGroup = pSkuGroup
        ad.EditDate = pEndDate
        ad.StoragePeriodTime = pStoragePeriodTime
        ad.StoragePeriodType = pStoragePeriodType
        ad.RunCondition = pRunCondition
        ad.ChargeDescription = pChargeDescription

        ad.PeriodType = pPeriodType
        ad.Period = pPeriod
        ad.StartDate = pStartdate

        ad.Active = pActive
        ad.HandlingUnitType = pHandlingUnitType
        ad.StoragePartialPeriod = pStoragePartialPeriod
        ad.IsStorageRange = pIsStorageRange
        ' ad.Save(pUser)
        Me.add(ad)
    End Sub

    Public Sub Save(ByVal sUserId As String)
        'For Each aggDet As AgreementDetail In Me
        '    aggDet.Save(sUserId)
        'Next
    End Sub


#End Region

End Class