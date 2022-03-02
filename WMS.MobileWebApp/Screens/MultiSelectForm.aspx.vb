Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Data

Partial Public Class MultiSelectForm
    Inherits PWMSRDTBase

#Region "Variables"

    Private _fromscreen As String
    Private _selectedtext As String

#End Region

#Region "Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Bind Control to data
        If Not IsPostBack Then
            Dim RequestSkuCode As String = Session("SKUCODE")
            _fromscreen = Session("FROMSCREEN")
            Dim dt As DataTable = New DataTable
            DataInterface.FillDataset("SELECT DISTINCT vSKUCODE.SKU, vSKUCODE.SKU + ' ' + SKUDESC AS DESCR FROM vSKUCODE INNER JOIN SKU ON vSKUCODE.CONSIGNEE=SKU.CONSIGNEE AND vSKUCODE.SKU=SKU.SKU WHERE SKUCODE = '" & RequestSkuCode & "' OR SKU.SKU ='" & RequestSkuCode & "'", dt)
            ValueList.DataTextField = "DESCR"
            ValueList.DataValueField = "SKU"            
            ValueList.DataSource = dt
            ValueList.DataBind()

            ValueList.SelectedValue = ValueList.Items(0).Value
        End If
    End Sub

    Sub donext()
        Session("SELECTEDSKU") = ValueList.SelectedValue
        Try
            Response.Redirect(MapVirtualPath("Screens/" & Session("FROMSCREEN") & ".aspx"))
            
        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Made4Net.Shared.M4NException

        Catch ex As ApplicationException

        Catch e As Exception

        Finally
            Session.Remove("FROMSCREEN")
            Session.Remove("SKUCODE")
        End Try
    End Sub

    Protected Sub Redirect(ByVal url As String)
        Try
            Response.Redirect(url)
        Catch ex As Exception
            If TypeOf ex Is System.Threading.ThreadAbortException Then
                System.Threading.Thread.ResetAbort()
                Return
            End If

        End Try
    End Sub

    Protected Sub SelectValue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectValue.Click

        donext()
    End Sub

    'Private Sub ValueList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ValueList.SelectedIndexChanged
    '    _selectedtext = ValueList.SelectedValue
    'End Sub

    Private Sub Up_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Up.Click
        If ValueList.SelectedIndex > 0 Then
            ValueList.SelectedIndex = ValueList.SelectedIndex - 1
        End If
    End Sub

    Private Sub Down_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Down.Click
        If ValueList.SelectedIndex < ValueList.Items.Count - 1 Then
            ValueList.SelectedIndex = ValueList.SelectedIndex + 1
        End If
    End Sub

#End Region




End Class