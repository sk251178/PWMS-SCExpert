Imports Made4Net.GeoData
Imports Made4Net.Algorithms
Imports Made4Net.Algorithms.GeneticAlgorithm
Imports Made4Net.Algorithms.Interfaces
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert
Imports System.Threading


Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Xml.XPath
Imports System.Xml


#Region "ContactNetworkBuilder"

Public Class ContactNetworkBuilder
    Public Sub New()
    End Sub

#Region "Google"
    Public Shared Function GetGoogleDitance(ByVal pOrigin As String, _
                                        ByVal pDestination As String, _
                                          ByVal mode As String, _
                            ByVal language As String, _
                            ByRef distance As Double, _
                            ByRef duration As Double) As String


        '' address format <strret>+<city>+<state>|
        '' address format <lat>+<lon>|
        ''http://maps.googleapis.com/maps/api/distancematrix/xml?origins=Latvia,Jurmala&destinations=5%20D%C4%81rza%20iela,R%C4%ABga,Latvia&mode=driving&language=EN-US&sensor=false

        distance = -1
        duration = -1
        If pOrigin = String.Empty Then Return "Undefined Origin"
        If pDestination = String.Empty Then Return "Undefined Destination"

        If mode = String.Empty Then mode = "driving" ''driving,walking,bicycling
        If language = String.Empty Then language = "en"

        Dim url As String


        url = String.Format("http://maps.googleapis.com/maps/api/distancematrix/xml?origins={0}&destinations={1}&mode={2}&language={3}&sensor=false", _
                    pOrigin, pDestination, mode, language)

        'System.Windows.Forms.MessageBox.Show(pOrigin)
        'System.Windows.Forms.MessageBox.Show(pDestination)
        'System.Windows.Forms.MessageBox.Show(url)
        ''        url = System.Web.HttpUtility.UrlEncode(url)

        Dim sr As New StreamReader(System.Net.WebRequest.Create(url).GetResponse().GetResponseStream())
        Dim xpathDoc As XPathDocument
        xpathDoc = New XPathDocument(sr)

        Dim xmlNav As XPathNavigator
        xmlNav = xpathDoc.CreateNavigator()

        Dim rows As XPathNodeIterator
        rows = xmlNav.Select("/DistanceMatrixResponse/row")
        Dim originscnt As Integer = 0
        Dim destionationcnt As Integer = 0
        Dim status As String

        rows.MoveNext()
        Dim rowsnavigator As XPathNavigator = rows.Current
        Dim elementText As XPathNodeIterator = rowsnavigator.SelectDescendants(XPathNodeType.Text, False)

        elementText.MoveNext()
        status = elementText.Current.OuterXml
        If status = "OK" Then
            elementText.MoveNext()
            duration = CDbl(elementText.Current.OuterXml)
            elementText.MoveNext()
            elementText.MoveNext()
            distance = CDbl(elementText.Current.OuterXml)
        End If
        Return status

    End Function




    Public Function buildNetworkbyGoogle() As String
        Dim continueCalc As Integer = Integer.MaxValue


        ''While continueCalc > 0
        ''Try
        Dim sql As String = String.Format("select distinct P1,P2 from vContactParam2")
        Dim dtNeighbours As New DataTable
        DataInterface.FillDataset(sql, dtNeighbours)

        Dim cnt As Integer = 0
        Dim status As String
        Dim distance, duration As Double
        Dim Origin, Destination As String
        Dim oGeoNetworkDistance As New GeoNetworkDistance()

        For Each drNeighbours As DataRow In dtNeighbours.Rows()
            Dim SourceStop As New GeoPointNode(drNeighbours("P1").ToString)
            Dim TargetStop As New GeoPointNode(drNeighbours("P2").ToString)


            distance = 0D
            duration = 0D
            Origin = Int2Deg(SourceStop.LATITUDE).ToString.Replace(",", ".") & "," & Int2Deg(SourceStop.LONGITUDE).ToString.Replace(",", ".")
            Destination = Int2Deg(TargetStop.LATITUDE).ToString.Replace(",", ".") & "," & Int2Deg(TargetStop.LONGITUDE).ToString.Replace(",", ".")


            status = GetGoogleDitance(Origin, Destination, "", "", distance, duration)
            If status.ToUpper = "OK" Then
                oGeoNetworkDistance.Save(SourceStop.POINTID, TargetStop.POINTID, distance, duration, Common.GetCurrentUser)
            End If


            cnt += 1
        Next

        continueCalc = DataInterface.ExecuteScalar("select count(*) from vContactParam2")

        'Catch ex As Exception
        '    continueCalc = 0
        '    Return ex.ToString()
        'End Try


        ''End While
        Return "Done."


    End Function


    Public Shared Function Int2Deg(ByVal x As Integer) As Double
        x = Math.Floor(x / 100)
        Dim secs, minutes As Integer
        Dim deg = Math.DivRem(x, 3600, minutes)
        minutes = Math.DivRem(minutes, 60, secs)
        Return deg + (minutes + (secs / 60.0)) / 60.0
    End Function

#End Region


    Public Sub buildNetworkbyParam()
        Dim continueCalc As Integer = Integer.MaxValue

        Dim isdistcost As String = Made4Net.Shared.AppConfig.Get("isDistCost", 1)
        Dim distcosttype As String = Made4Net.Shared.AppConfig.Get("DistCostType", 1)
        Dim isdistanceadd As String = Made4Net.Shared.AppConfig.Get("isDistanceAdd", 1)

        While continueCalc > 0
            Try
                Dim sql As String = String.Format("select distinct P1,P2 from vContactParam2")
                Dim dtNeighbours As New DataTable
                DataInterface.FillDataset(sql, dtNeighbours)

                Dim cnt As Integer = 0
                For Each drNeighbours As DataRow In dtNeighbours.Rows()
                    Dim ContactStop As New GeoPointNode(drNeighbours("P1").ToString)
                    Dim NeighbourStop As New GeoPointNode(drNeighbours("P2").ToString)

                    Dim distance As Double = 0D
                    Dim drvtime As Double = 0D
                    Try

                        GeoNetworkItem.GetDistance(ContactStop, NeighbourStop, distance, drvtime, Common.GetCurrentUser, isdistanceadd, isdistcost, distcosttype)
                    Catch ex As Exception
                    End Try
                    cnt += 1
                Next

                continueCalc = DataInterface.ExecuteScalar("select count(*) from vContactParam2")

            Catch ex As Exception
                continueCalc = 0
            End Try


        End While




    End Sub


   

    '' -----
    Public Sub buildNetworkbyContact(ByVal p As Object)
        Try
            Dim drContact As DataRow = DirectCast(p, DataRow)
            Dim contactid As String = ReplaceDBNull(drContact("CONTACTID").ToString())
            Dim fromparam As String = ReplaceDBNull(drContact("fromparam").ToString())
            Dim toparam As String = ReplaceDBNull(drContact("toparam").ToString())
            Dim pointid As String = ReplaceDBNull(drContact("POINTID").ToString())
            If pointid = String.Empty Then Exit Sub

            Dim sql As String = String.Format("select distinct CONTACTID,  POINTID from vContactParam where param>='{0}' and param<='{1}' and contactid<>'{2}' ", _
                fromparam, toparam, contactid)

            Dim dtNeigbours As New DataTable
            DataInterface.FillDataset(sql, dtNeigbours)
            Dim ContactStop As New GeoPointNode(pointid)
            Dim cnt As Integer = 0
            For Each drNeigbours As DataRow In dtNeigbours.Rows()
                Dim neigbourpointid As String = ReplaceDBNull(drNeigbours("POINTID"))
                Dim NeigbourStop As New GeoPointNode(neigbourpointid)
                Dim distance As Double = 0D
                Dim drvtime As Double = 0D
                GeoNetworkItem.GetDistance(ContactStop, NeigbourStop, distance, drvtime, Common.GetCurrentUser, 1, 1, 1)
                cnt += 1
                distance = 0
                drvtime = 0
                GeoNetworkItem.GetDistance(NeigbourStop, ContactStop, distance, drvtime, Common.GetCurrentUser, 1, 1, 1)
                cnt += 1
            Next

        Catch ex As Exception

        End Try
    End Sub

End Class

#End Region

