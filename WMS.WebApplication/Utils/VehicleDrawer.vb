Imports System.ComponentModel
Imports Made4Net.DataAccess
Imports Made4Net.WebControls
Imports Made4Net.Shared.Web
Imports WMS.Logic


<CLSCompliant(False)> Public Class VehicleDrawer

#Region "Variables"

    Protected WithEvents _Truck As Table
    Protected WithEvents _TruckSchema As Table
    Protected WithEvents _DriverCab As Made4Net.WebControls.Image
    Protected WithEvents _TruckWheels As Made4Net.WebControls.Image

    Protected _TruckType As String
    Protected _TruckSide As String
    Protected _ShowContent As String
    Protected _TruckLoadLength As Double
    Protected _TruckLoadHeight As Double

#End Region

#Region "Properties"

    Public Property TruckSide() As String
        Get
            Return _TruckSide
        End Get
        Set(ByVal Value As String)
            _TruckSide = Value
        End Set
    End Property

    Public Property TruckType() As String
        Get
            Return _TruckType
        End Get
        Set(ByVal Value As String)
            _TruckType = Value
        End Set
    End Property

    Public Property DriverCabImageUrl() As String
        Get
            Return _DriverCab.ImageUrl
        End Get
        Set(ByVal Value As String)
            _DriverCab.ImageUrl = SkinManager.GetImageURL(Value)
        End Set
    End Property

    Public Property TruckWheelsImageUrl() As String
        Get
            Return _TruckWheels.ImageUrl
        End Get
        Set(ByVal Value As String)
            _TruckWheels.ImageUrl = SkinManager.GetImageURL(Value)
        End Set
    End Property

    Public Property TruckLoadLength() As Double
        Get
            Return 368
        End Get
        Set(ByVal Value As Double)
            _TruckLoadLength = 368
        End Set
    End Property

    Public Property TruckLoadHeight() As Double
        Get
            Return 84
        End Get
        Set(ByVal Value As Double)
            _TruckLoadHeight = 84
        End Set
    End Property

    Public Property ShowContent() As Boolean
        Get
            Return _ShowContent
        End Get
        Set(ByVal Value As Boolean)
            _ShowContent = Value
        End Set
    End Property

    Public ReadOnly Property Truck() As Table
        Get
            Return _Truck
        End Get
    End Property

#End Region

    Public Sub New()
        _Truck = New Table
        _TruckSchema = New Table
        _DriverCab = New Made4Net.WebControls.Image
        _TruckWheels = New Made4Net.WebControls.Image
        _ShowContent = True
        _Truck.Visible = True
        _TruckSchema.Visible = True
    End Sub

#Region "Methods"

    Public Sub CreateTruck()
        With _Truck

            .Width = Unit.Percentage(30)
            .BorderColor = System.Drawing.Color.FromArgb(116, 133, 163)

            CreateTruckSchema()

            Dim combinedTable As New Table

            Dim cabCell As New TableCell
            cabCell.Controls.Add(_DriverCab)

            Dim wheelCell As New TableCell
            wheelCell.Controls.Add(_TruckWheels)

            Dim schemaCell As New TableCell
            schemaCell.BorderWidth = Unit.Pixel(1)
            schemaCell.BorderColor = System.Drawing.Color.SteelBlue
            schemaCell.BorderStyle = BorderStyle.Solid
            If Me.TruckSide.ToLower = "left" Then
                schemaCell.HorizontalAlign = HorizontalAlign.Left
            Else
                schemaCell.HorizontalAlign = HorizontalAlign.Right
            End If
            _TruckSchema.Height = System.Web.UI.WebControls.Unit.Percentage(100)
            '_TruckSchema.Width = System.Web.UI.WebControls.Unit.Pixel(TruckLoadLength)
            schemaCell.Controls.Add(_TruckSchema)

            combinedTable.AddRow(0)
            combinedTable.AddedRow.ID = "TruckSchema_" & _TruckSide
            combinedTable.Rows(0).Cells.Add(schemaCell)
            If Me.TruckSide.ToLower = "left" Then
                combinedTable.Rows(0).HorizontalAlign = HorizontalAlign.Left
            Else
                combinedTable.Rows(0).HorizontalAlign = HorizontalAlign.Right
            End If
            combinedTable.Rows(0).Height = System.Web.UI.WebControls.Unit.Percentage(100)
            combinedTable.AddRow(1)
            combinedTable.AddedRow.ID = "WheelsImage_" & _TruckSide
            combinedTable.Rows(1).Cells.Add(wheelCell)
            combinedTable.Rows(1).VerticalAlign = VerticalAlign.Bottom
            If Me.TruckSide.ToLower = "left" Then
                combinedTable.Rows(1).HorizontalAlign = HorizontalAlign.Left
            Else
                combinedTable.Rows(1).HorizontalAlign = HorizontalAlign.Right
            End If
            combinedTable.Height = System.Web.UI.WebControls.Unit.Percentage(100)
            Dim combinedCell As New TableCell
            combinedCell.Height = System.Web.UI.WebControls.Unit.Percentage(100)
            If Me.TruckSide.ToLower = "left" Then
                combinedCell.HorizontalAlign = HorizontalAlign.Left
            Else
                combinedCell.HorizontalAlign = HorizontalAlign.Right
            End If
            combinedCell.Controls.Add(combinedTable)
            _Truck.AddRow(0)
            _Truck.Rows(0).VerticalAlign = VerticalAlign.Bottom
            If Me.TruckSide.ToLower = "left" Then
                _Truck.Rows(0).Cells.Add(cabCell)
                _Truck.Rows(0).Cells.Add(combinedCell)
            Else
                _Truck.Rows(0).Cells.Add(combinedCell)
                _Truck.Rows(0).Cells.Add(cabCell)
            End If
        End With
    End Sub

    Public Sub CreateTruckSchema()
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow

        SQL = String.Format("select distinct bay from vehiclelocations where vehicletype = {0} and side = {1} order by bay", _
            Made4Net.Shared.Util.FormatField(_TruckType), Made4Net.Shared.Util.FormatField(_TruckSide))
        DataInterface.FillDataset(SQL, dt)

        Dim bayTable As Table
        _TruckSchema.AddRow(0)
        If Me.TruckSide.ToLower = "left" Then
            _TruckSchema.Rows(0).HorizontalAlign = HorizontalAlign.Left
        Else
            _TruckSchema.Rows(0).HorizontalAlign = HorizontalAlign.Right
        End If
        For Each dr In dt.Rows
            bayTable = CreateBayTable(dr("bay"))
            bayTable.ID = "Bay_" & _TruckSide & "_" & dr("bay")
            bayTable.BorderColor = System.Drawing.Color.SteelBlue
            bayTable.BorderStyle = BorderStyle.Solid
            bayTable.BorderWidth = Unit.Pixel(1)
            bayTable.HorizontalAlign = HorizontalAlign.Left
            bayTable.Visible = True

            Dim currBay As New TableCell
            currBay.VerticalAlign = VerticalAlign.Bottom
            currBay.HorizontalAlign = HorizontalAlign.Left
            currBay.Controls.Add(bayTable)
            _TruckSchema.Rows(0).Cells.Add(currBay)
        Next
    End Sub

    Public Function CreateBayTable(ByVal pBayId As String) As Table
        Dim tmpTable As New Table
        tmpTable.Visible = True
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow

        SQL = String.Format("select * from vehiclelocations where vehicletype = {0} and side = {1} and bay = {2} order by level desc ", _
            Made4Net.Shared.Util.FormatField(_TruckType), Made4Net.Shared.Util.FormatField(_TruckSide), Made4Net.Shared.Util.FormatField(pBayId))
        DataInterface.FillDataset(SQL, dt)

        'calc the bays height and width
        Dim bayHeight As Double = GetMaxBayHeight(_TruckType)
        Dim locHeightUnit As Double = TruckLoadHeight / bayHeight

        Dim bayWidth As Double = GetMaxLocLength(_TruckType)
        Dim locWidthUnit As Double = TruckLoadLength / bayWidth

        For Each dr In dt.Rows
            Dim cont As Boolean = False
            Dim tmpcell As New TableCell
            tmpcell.ID = "VehicleLocationID" & dr("Location") & dr("side")
            tmpcell.Attributes.Add("DropID", dr("Location") & dr("side"))
            Try
                cont = LocContent(dr("Location"))
            Catch ex As Exception
            End Try
            If cont And _ShowContent Then
                tmpcell.BackColor = System.Drawing.Color.FromArgb(154, 202, 238)
            End If
            tmpcell.Visible = True
            tmpcell.BorderStyle = BorderStyle.NotSet
            tmpcell.BorderWidth = Unit.Pixel(2)
            tmpcell.Width = System.Web.UI.WebControls.Unit.Pixel(locWidthUnit * dr("width"))
            tmpcell.Height = System.Web.UI.WebControls.Unit.Pixel(locHeightUnit * dr("height"))

            Dim status As String = dr("status")
            Select Case status.ToLower
                Case "active"
                    tmpcell.BorderColor = System.Drawing.Color.Green
                Case "inactive"
                    tmpcell.BorderColor = System.Drawing.Color.Red
                Case "reserved"
                    tmpcell.BorderColor = System.Drawing.Color.Blue
            End Select
            tmpcell.HorizontalAlign = HorizontalAlign.Left
            tmpcell.Text = dr("Location")
            tmpTable.AddRow()
            tmpTable.AddedRow.Cells.Add(tmpcell)
        Next

        Return tmpTable
    End Function

    Private Function GetMaxBayHeight(ByVal pVehicleType As String) As Double
        Dim SQL As String
        SQL = String.Format("select max (bayheight) from vehicletypebayheight where vehicletype = {0} group by vehicletype", _
            Made4Net.Shared.Util.FormatField(pVehicleType))
        Return (Convert.ToDouble(DataInterface.ExecuteScalar(SQL)))
    End Function

    Private Function GetMaxLocLength(ByVal pVehicleType As String) As Double
        Dim SQL As String
        SQL = String.Format("select max (sidewidth) from vehicletypesidewidth where vehicletype = {0} group by vehicletype", _
            Made4Net.Shared.Util.FormatField(pVehicleType))
        Return (Convert.ToDouble(DataInterface.ExecuteScalar(SQL)))
    End Function

    Private Function LocContent(ByVal pLocation As String) As Boolean
        Dim SQL As String
        SQL = String.Format("select count(1) from loadingplan where vehiclelocation = {0} ", _
            Made4Net.Shared.Util.FormatField(pLocation))
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(SQL))
    End Function

#End Region

End Class
