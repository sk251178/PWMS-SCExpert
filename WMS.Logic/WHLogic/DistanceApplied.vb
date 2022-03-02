Imports Made4Net.Algorithms.Interfaces
Imports Made4Net.Algorithms
Imports DataManager.ServiceModel
Imports System.Text
Imports Made4Net.DataAccess
Imports System.Collections.Generic
Imports Made4Net.Shared

Public Class DistanceApplied
    ''' <summary>
    ''' Updates the Rows returened by vPutAway view with Applied Distance for the Distance Column. Note the vPutAway only considers straight cartesian line
    ''' for calculating the distance which is not correct.
    ''' </summary>
    ''' <param name="startLocation"></param>The start location where the Load can be picked. This is Piclocation of the SKU. Mandatory non null field.
    ''' <param name="locationsToUpdate"></param>A collection of most elligble locations returned by vPutAway view.
    ''' <param name="oLogger"></param>Instantiated and initialized logger object.
    ''' <returns>Boolean</returns>Returns True if succeds or else False.
    ''' <remarks>
    ''' Note when calling this method, the location scoring has not been done, after calling this method location scoring should be done so
    ''' that the scored locations are in according to the actual distance.
    ''' </remarks>
    Public Shared Function UpdateWithActualDistance(ByVal startLocation As String,
                                                    ByRef locationsToUpdate As System.Data.DataRow(), ByVal operatorsEqpHeight As Double,
                                                    Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim _sp As IShortestPathProvider = ShortestPath.GetInstance()
        Dim path As Path
        Dim dist As Double = 0D
        Dim returnVal As Boolean = False
        Dim foundLocation As New Dictionary(Of String, Double)

        ' Fetch source Location
        Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"
        Dim locationsFrom As New DataTable()
        DataInterface.FillDataset([String].Format(sql, startLocation), locationsFrom)
        Dim from As DataRow = locationsFrom.Rows(0)

        If Not oLogger Is Nothing Then
            oLogger.Write("Updating the elligible Locations returned from VPutAway with Actual Distance(Applied)")
        End If
        Dim watch As System.Diagnostics.Stopwatch = Stopwatch.StartNew()
        If locationsToUpdate.Length > 0 Then
            If startLocation <> String.Empty Then

                ' check if height  is zero, if yes don't pass ny rule.

                Dim rules As List(Of Rules) = New List(Of Rules)()

                Dim rule As Rules = New Rules()

                rule.Parameter = Made4Net.Algorithms.Constants.Height
                rule.Data = operatorsEqpHeight
                rule.Operator = ">"

                rules.Add(rule)

                For Each dr As DataRow In locationsToUpdate
                    Try
                        If Not foundLocation.ContainsKey(dr("location").ToString()) Then
                            If IsDBNull(dr("FILLEDGE")) And IsDBNull(dr("PICKEDGE")) Then
                                oLogger.Write(String.Format("Could not calculate shortest path, PickEdge and FillEdge is not configured for location={0}.  Using the distance returened by vPutAway( Distance={1}).", dr("location").ToString(), dr("DISTANCE")))
                                oLogger.Write(String.Format("VPutAway retured  Heights values: Location={0},VPutWay Distance={1},Location Height={2}, PWMSLOCATIONHEIGHT={3},PENDINGLOADSHEIGHT={4},PWPENDINGLOADSHEIGHT={5}", dr("location").ToString(), dr("DISTANCE").ToString(), dr("HEIGHT").ToString(), dr("PWMSLOCATIONHEIGHT").ToString(), dr("PENDINGLOADSHEIGHT").ToString(), dr("PWPENDINGLOADSHEIGHT").ToString()))
                            Else
                                oLogger.Write(String.Format("VPutAway retured values: Location={0},VPutWay Distance={1},Location Height={2}, PWMSLOCATIONHEIGHT={3},PENDINGLOADSHEIGHT={4},PWPENDINGLOADSHEIGHT={5}", dr("location").ToString(), dr("DISTANCE").ToString(), dr("HEIGHT").ToString(), dr("PWMSLOCATIONHEIGHT").ToString(), dr("PENDINGLOADSHEIGHT").ToString(), dr("PWPENDINGLOADSHEIGHT").ToString()))
                                path = _sp.GetShortestPathWithContsraints(from, dr, "LOADPW", "LOADPW", False, rules, GetShortestPathLogger(oLogger))
                                If path.Distance.SourceToTargetLocation > 0 Then
                                    dist = path.Distance.SourceToTargetLocation
                                    oLogger.Write(String.Format("Found ShortestPath for FromLocation={0} and ToLocation={1},vPutAway Distance={2}, Acutal Distance={3}", startLocation, dr("location").ToString(), dr("DISTANCE").ToString(), dist))
                                    dr("DISTANCE") = dist
                                Else
                                    oLogger.Write(String.Format("Could not find a valid path under given conditions between FromLocation={0} and ToLocation={1}. Using the distance returned by vPutAway( Distance={2}).", startLocation, dr("location").ToString(), dr("DISTANCE")))
                                End If
                                foundLocation.Add(dr("location").ToString(), dist)
                            End If
                        Else
                            dr("DISTANCE") = foundLocation.Item(dr("location").ToString())
                        End If


                        returnVal = True
                    Catch ex As Exception
                        oLogger.Write(String.Format("Could not calculate shortest path for FromLocation={0} and ToLocation={1}. The locations are not configured with correct data for PickEdge and FillEdge. Using the distance returened by vPutAway( Distance={2}).", startLocation, dr("location").ToString(), dr("DISTANCE")))
                    End Try
                Next
            Else
                oLogger.Write("Could not update the elligible Locations returned from VPutAway with Actual Distance(Applied) since the start location is passed as null or empty.")
            End If
            watch.Stop()
            oLogger.Write(String.Format("Updating the elligible Locations Done(Applied). Time taken : {0} Milliseconds for {1} incoming locations", watch.Elapsed.TotalMilliseconds, locationsToUpdate.Length))
        Else
            oLogger.Write("VPutAway did not return any elligle locations. Cannot update applied distance.")
        End If
        Return returnVal
    End Function

    ''' <summary>
    ''' Gets the Pickloaction of the given SKU.
    ''' </summary>
    ''' <param name="ld"></param>Load
    ''' <param name="oLogger"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)>
    Public Shared Function GetPickLocationForSKU(ByVal ld As Load,
                                                 Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As String
        Dim pickLocation As String
        If ld IsNot Nothing Then
            If ld.SKU IsNot Nothing And ld.SKU <> String.Empty Then
                Try
                    pickLocation = DataInterface.ExecuteScalar(String.Format("select PICKLOC.LOCATION from SKU inner join PICKLOC on SKU.SKU = '{0}' AND SKU.SKU = PICKLOC.SKU ", ld.SKU))
                Catch ex As Exception
                    oLogger.Write(String.Format("Exception: Picklocation for SKU {0} cannot be fetched from Database.", ld.SKU))
                End Try
                If String.IsNullOrEmpty(pickLocation) Or String.IsNullOrWhiteSpace(pickLocation) Then
                    oLogger.Write(String.Format("Picklocation for SKU {0} don't exists in Database.", ld.SKU))
                Else
                    oLogger.Write(String.Format("Picklocation for Load {0} having SKU {1} found as {2}.", ld.LOADID, ld.SKU, pickLocation))
                End If
            Else
                oLogger.Write(String.Format("Load {0} donot have a SKU assigned, cannot fetch Picklocation.", ld.LOADID))
            End If
        Else
            oLogger.Write("Load passed to method GetPickLocationForSKU is null, cannot fetch picklocation")
        End If
        Return pickLocation
    End Function

    <CLSCompliant(False)>
    Public Shared Function GetPickLocAndFrontRackLocForSKU(ByVal ld As Load, ByRef picLocFrontRackLoc As String,
                                                 Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As String
        Dim pickLocation As String
        picLocFrontRackLoc = ""
        If ld IsNot Nothing Then
            If ld.SKU IsNot Nothing And ld.SKU <> String.Empty Then
                Try
                    pickLocation = DataInterface.ExecuteScalar(String.Format("select PICKLOC.LOCATION from SKU inner join PICKLOC on SKU.SKU = '{0}' AND SKU.SKU = PICKLOC.SKU ", ld.SKU))
                Catch ex As Exception
                    oLogger.Write(String.Format("Exception: Picklocation for SKU {0} cannot be fetched from Database.", ld.SKU))
                End Try
                If String.IsNullOrEmpty(pickLocation) Or String.IsNullOrWhiteSpace(pickLocation) Then
                    oLogger.Write(String.Format("Picklocation for SKU {0} don't exists in Database.", ld.SKU))
                Else

                    picLocFrontRackLoc = DataInterface.ExecuteScalar(String.Format("SELECT LOCATION FROM LOCATION WHERE frontracklocation ='{0}'", pickLocation))
                    If Len(picLocFrontRackLoc) <= 0 Then
                        oLogger.Write(String.Format("Picklocation for Load {0} having SKU {1} found as {2}. No Fill Location Defined. ", ld.LOADID, ld.SKU, pickLocation))
                    Else
                        oLogger.Write(String.Format("Picklocation for Load {0} having SKU {1} found as {2}. Fill Location found:{3}", ld.LOADID, ld.SKU, pickLocation, picLocFrontRackLoc))
                    End If
                End If
            Else
                oLogger.Write(String.Format("Load {0} donot have a SKU assigned, cannot fetch Picklocation.", ld.LOADID))
            End If
        Else
            oLogger.Write("Load passed to method GetPickLocAndFrontRackLocForSKU is null, cannot fetch picklocation")
        End If
        Return pickLocation
    End Function



End Class