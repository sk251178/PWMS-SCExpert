Imports System.Collections
Imports Made4Net.DataAccess.DataInterface

Public Class GTINBarcodeParser

    Private mApplicationIdentifiers As Hashtable
    Private mSCExpertApplicationIdentifiers As Hashtable
    Private mSCExpertIdentifierMapping As DataTable

    Public ReadOnly Property ApplicationIdentifiersCollection() As Hashtable
        Get
            Return mApplicationIdentifiers
        End Get
    End Property

    Public ReadOnly Property ApplicationIdentifierValue(ByVal pIdentifierKey As String) As String
        Get
            If mApplicationIdentifiers.ContainsKey(pIdentifierKey) Then
                Return mApplicationIdentifiers.Item(pIdentifierKey)
            End If
            Return String.Empty
        End Get
    End Property

    Public ReadOnly Property SCExpertApplicationIdentifiersCollection() As Hashtable
        Get
            Return mSCExpertApplicationIdentifiers
        End Get
    End Property

    Public ReadOnly Property SCExpertApplicationIdentifierValue(ByVal pPropertyName As String) As String
        Get
            If mSCExpertApplicationIdentifiers.ContainsKey(pPropertyName) Then
                Return mSCExpertApplicationIdentifiers(pPropertyName)
            End If
            Return String.Empty
        End Get
    End Property

    Public Sub New()
        mApplicationIdentifiers = New Hashtable
        mSCExpertApplicationIdentifiers = New Hashtable
        mSCExpertIdentifierMapping = New DataTable
        Dim sSql As String = String.Format("select * from applicationidentifiers where isnull(identifiermapping,'') <> ''")
        FillDataset(sSql, mSCExpertIdentifierMapping)
    End Sub

    Public Sub ParseBarcode(ByVal pBarcode As String)
        Dim sItemId, sSerial As String
        Dim dWeight As Decimal
        Dim dMfgDate As String
        sItemId = pBarcode.Substring(2, 14)
        sSerial = pBarcode.Substring(36, 8)
        dWeight = Convert.ToDecimal(pBarcode.Substring(20, 6)) / 10
        dMfgDate = pBarcode.Substring(28, 6)
        mSCExpertApplicationIdentifiers.Add("SKU", sItemId)
        mSCExpertApplicationIdentifiers.Add("WEIGHT", dWeight)
        mSCExpertApplicationIdentifiers.Add("SERIAL", sSerial)
        mSCExpertApplicationIdentifiers.Add("MFGDATE", dMfgDate)
    End Sub

    Public Sub ParseBarcodeWithParentheses(ByVal pBarcode As String)
        'parse the barcode string and build the mapping info.
        Dim IdentifiersArr() As String = pBarcode.Split("(")
        Dim TempIdentifier() As String
        For i As Int32 = 0 To IdentifiersArr.Length - 1
            TempIdentifier = IdentifiersArr(i).Replace("(", "").Split(")")
            If TempIdentifier.Length <= 1 Then
                Continue For
            End If
            If TempIdentifier(0).Length = 4 Then
                Dim rDigit As String = TempIdentifier(0).Substring(3, 1)
                mApplicationIdentifiers.Add(TempIdentifier(0), TempIdentifier(1) / Convert.ToDecimal(Math.Pow(10, rDigit)))
            Else
                mApplicationIdentifiers.Add(TempIdentifier(0), TempIdentifier(1))
            End If
        Next

        Dim sFilter As String
        Dim SCExpertIdMap() As DataRow
        'and fill up the scexpert hash table so it will ease up access from the ui...
        For Each entry As DictionaryEntry In mApplicationIdentifiers
            sFilter = String.Format("identifiercode='{0}'", entry.Key)
            SCExpertIdMap = mSCExpertIdentifierMapping.Select(sFilter)
            If SCExpertIdMap.Length >= 1 Then
                mSCExpertApplicationIdentifiers.Add(SCExpertIdMap(0)("identifiermapping"), mApplicationIdentifiers(entry.Key)) 'SCExpertIdMap(0)("identifiercode"))
            End If
        Next
    End Sub

End Class
