Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Made4Net.DataAccess
Imports System.Data
Imports WMS.Logic
Imports Made4Net.Shared

Public Class MultiUOMUnits
    'dictionary class, contain: units, UOMunits
#Region "Class UnitsData"


    Public Class UnitsData

        Private _Units As Decimal
        Private _UnitsUOM As Decimal

        Public Sub New(ByVal pUnitsUOM As Decimal, ByVal pUnits As Decimal)
            _Units = pUnits
            _UnitsUOM = pUnitsUOM
        End Sub

        Public Property Units() As Decimal
            Get
                Return _Units
            End Get
            Set(ByVal value As Decimal)
                _Units = value
            End Set
        End Property

        Public Property UnitsUOM() As Decimal
            Get
                Return _UnitsUOM
            End Get
            Set(ByVal value As Decimal)
                _UnitsUOM = value
            End Set
        End Property


    End Class
#End Region

#Region "variables"

    Public dictUnits As New Dictionary(Of String, MultiUOMUnits.UnitsData)
    Private _consignee As String
    Private _sku As String
    Private _defaultUom As String
    Private _DO1 As Made4Net.Mobile.WebCtrls.DataObject
    Public TotalUomName As String = "TOTAL"
    Public BaseUomName As String = "UOM"
    Public MaxUnits As Decimal = 0

    Property consignee() As String
        Get
            Return _consignee
        End Get
        Set(ByVal value As String)
            _consignee = value
        End Set
    End Property
    Property sku() As String
        Get
            Return _sku
        End Get
        Set(ByVal value As String)
            _sku = value
        End Set
    End Property
    Property defaultUom() As String
        Get
            Return _defaultUom
        End Get
        Set(ByVal value As String)
            _defaultUom = value
        End Set
    End Property

    <CLSCompliant(False)> _
    Property DO1() As Made4Net.Mobile.WebCtrls.DataObject
        Get
            Return _DO1
        End Get
        Set(ByVal value As Made4Net.Mobile.WebCtrls.DataObject)
            _DO1 = value
        End Set
    End Property

    'Public Sub New()

    'End Sub

    <CLSCompliant(False)> _
    Public Sub New(ByVal consignee As String, ByVal sku As String, ByVal defaultUom As String, ByVal maxUnits As Decimal) ', ByVal do1 As Made4Net.Mobile.WebCtrls.DataObject)
        Me._consignee = consignee
        Me._sku = sku
        Me._defaultUom = defaultUom
        Me.MaxUnits = maxUnits
        '  Me._DO1 = do1
    End Sub
#End Region


    Public Sub AddUOMUnits(ByVal UOM As String, ByVal UOMUnits As Decimal)
        Dim Units As Decimal = MobileUtils.ConvertToUnits(consignee, sku, UOM, UOMUnits)

        If Not dictUnits.ContainsKey(UOM) Then
            dictUnits.Add(UOM, New MultiUOMUnits.UnitsData(UOMUnits, Units))
        Else
            EditToDictionary(UOM, UOMUnits, Units)
        End If

        AddTotal(TotalUomName, Units)
    End Sub

    Private Sub AddTotal(ByVal UOM As String, ByVal Units As Decimal)

        Dim UOMUnits As Decimal = MobileUtils.ConvertUnitsToUom(consignee, sku, defaultUom, Units)

        If Not dictUnits.ContainsKey(UOM) Then
            dictUnits.Add(UOM, New MultiUOMUnits.UnitsData(UOMUnits, Units))
        Else
            EditToDictionary(UOM, UOMUnits, Units)
        End If

    End Sub

    Private Sub EditToDictionary(ByVal UOM As String, ByVal UOMUnits As Decimal, ByVal Units As Decimal)

        For Each pair As KeyValuePair(Of String, MultiUOMUnits.UnitsData) In dictUnits
            If pair.Key = UOM Then
                pair.Value.UnitsUOM += UOMUnits
                pair.Value.Units += Units
            End If
        Next

    End Sub

    Public Sub clear()
        Try
            dictUnits.Clear()
        Catch ex As Exception

        End Try
    End Sub


End Class


Public Class ManageMutliUOMUnits
    'Public Shared trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
    '  <CLSCompliant(False)> _
    Public Shared Function SetMutliUOMObj(ByVal consignee As String, ByVal sku As String, ByVal defaultUom As String, Optional ByVal MaxUnits As Decimal = 0) As MultiUOMUnits
        ', ByVal do1 As Made4Net.Mobile.WebCtrls.DataObject) 
        Dim oMultiUOM As MultiUOMUnits
        If Not IsNothing(HttpContext.Current.Session("objMultiUOMUnits")) Then
            oMultiUOM = HttpContext.Current.Session("objMultiUOMUnits")
        Else
            oMultiUOM = New MultiUOMUnits(consignee, sku, defaultUom, MaxUnits) ', do1)
            HttpContext.Current.Session("objMultiUOMUnits") = oMultiUOM
        End If
        Return oMultiUOM
    End Function

    Public Shared Function GetMutliUOMObj() As MultiUOMUnits
        Dim oMultiUOM As MultiUOMUnits
        If Not IsNothing(HttpContext.Current.Session("objMultiUOMUnits")) Then
            oMultiUOM = HttpContext.Current.Session("objMultiUOMUnits")
            'Else
            '    oMultiUOM = New MultiUOMUnits()
        End If
        Return oMultiUOM
    End Function

    Public Shared Function AddUOMUnits(ByVal UOM As String, ByVal UnitsUom As String, ByRef err As String, Optional ByVal fZeroAllow As Boolean = False) As Boolean
        Dim ret As Boolean = True
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim oMultiUOM As MultiUOMUnits = GetMutliUOMObj()
        If Not IsNumeric(UnitsUom) Then
            Return True
            'Exit Function
        End If
        If Convert.ToDecimal(UnitsUom) < 0 Then
            Return True
            'Exit Function
        End If
        If Not fZeroAllow And Convert.ToDecimal(UnitsUom) = 0 Then
            Return True
            'Exit Function
        End If

        If Not fZeroAllow Then
            Dim Units As Decimal = MobileUtils.ConvertToUnits(oMultiUOM.consignee, oMultiUOM.sku, UOM, UnitsUom)

            If IsNumeric(GetTotalEachUnits()) Then
                Units = Units + GetTotalEachUnits()
            End If

            If Units > oMultiUOM.MaxUnits And oMultiUOM.MaxUnits > 0 Then
                err = trans.Translate("Cannot add more quantity")
                Return False
            End If
        End If


        Dim UOMUnits As Decimal = UnitsUom
        If Not IsNothing(oMultiUOM) Then
            oMultiUOM.AddUOMUnits(UOM, UOMUnits)
            HttpContext.Current.Session("objMultiUOMUnits") = oMultiUOM
        Else
            ' MessageQue.Enqueue(trans.Translate("Illegal Container"))
        End If
        Return ret
    End Function

    Public Shared Function GetTotal() As String
        Dim oMultiUOM As MultiUOMUnits = GetMutliUOMObj()
        If oMultiUOM.dictUnits.Count = 0 Then
            Return ""
        Else
            Return oMultiUOM.dictUnits.Item(oMultiUOM.TotalUomName).UnitsUOM.ToString
        End If
    End Function

    Public Shared Function GetTotalEachUnits() As String
        Dim oMultiUOM As MultiUOMUnits = GetMutliUOMObj()
        If oMultiUOM.dictUnits.Count = 0 Then
            Return ""
        Else
            Return oMultiUOM.dictUnits.Item(oMultiUOM.TotalUomName).Units.ToString
        End If
    End Function

    Public Shared Sub Clear(Optional ByVal fDelSesion As Boolean = False)
        Dim oMultiUOM As MultiUOMUnits = GetMutliUOMObj()
        Try
            oMultiUOM.dictUnits.Clear()
        Catch ex As Exception

        End Try

        If fDelSesion Then
            HttpContext.Current.Session.Remove("objMultiUOMUnits")
        End If
        
    End Sub
    <CLSCompliant(False)> _
    Public Shared Sub SetValUOMUnitsAfterDrow(ByRef DO1 As Made4Net.Mobile.WebCtrls.DataObject)
        Dim oMultiUOM As MultiUOMUnits = GetMutliUOMObj()
        Dim i As Integer = 1
        If oMultiUOM.dictUnits.Count > 0 Then
            'set added lable visible true
            For Each pair As KeyValuePair(Of String, MultiUOMUnits.UnitsData) In oMultiUOM.dictUnits
                If pair.Key <> oMultiUOM.TotalUomName And pair.Value.UnitsUOM > 0 Then
                    DO1.Value(oMultiUOM.BaseUomName & i) = pair.Value.UnitsUOM
                    setLabelName(oMultiUOM.BaseUomName & i, pair.Key, DO1)

                    i += 1
                ElseIf pair.Key = oMultiUOM.TotalUomName Then
                    DO1.Value(oMultiUOM.TotalUomName) = pair.Value.UnitsUOM
                    setLabelName(oMultiUOM.TotalUomName, pair.Key, DO1)

                End If
            Next
        Else
            'set all labelse visible false

            Dim sk As New WMS.Logic.SKU(oMultiUOM.consignee, oMultiUOM.sku)
            Dim oUom As WMS.Logic.SKU.SKUUOM

            For Each oUom In sk.UNITSOFMEASURE
                DO1.setVisibility(oMultiUOM.BaseUomName & i, False)
                i += 1
            Next
            DO1.setVisibility(oMultiUOM.TotalUomName, False)
        End If

    End Sub

    <CLSCompliant(False)> _
    Public Shared Sub DROWLABLES(ByRef DO1 As Made4Net.Mobile.WebCtrls.DataObject)
        Dim oMultiUOM As MultiUOMUnits = GetMutliUOMObj()
        Dim sk As New WMS.Logic.SKU(oMultiUOM.consignee, oMultiUOM.sku)

        Dim oUom As WMS.Logic.SKU.SKUUOM
        Dim i As Integer = 1
        For Each oUom In sk.UNITSOFMEASURE

            DO1.AddLabelLine(oMultiUOM.BaseUomName & i)
            DO1.setVisibility(oMultiUOM.BaseUomName & i, False)
            i += 1          

        Next
        DO1.AddLabelLine(oMultiUOM.TotalUomName)
        DO1.setVisibility(oMultiUOM.TotalUomName, False)
    End Sub

    <CLSCompliant(False)> _
    Public Shared Sub setLabelName(ByVal controlID As String, ByVal val As String, ByRef DO1 As Made4Net.Mobile.WebCtrls.DataObject)
        Dim ctrl As System.Web.UI.Control
        ctrl = DO1.FindControl(controlID & "lbl")
        If Not IsNothing(ctrl) Then
           
            CType(ctrl, Made4Net.WebControls.FieldLabel).Value = val
            DO1.setVisibility(controlID, True)
        End If
    End Sub

End Class
