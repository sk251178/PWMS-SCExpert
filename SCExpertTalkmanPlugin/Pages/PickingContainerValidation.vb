Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.AppSessionManagement

Public Class PickingContainerValidation
    Inherits AppPageProcessor

    Private Enum ResponseCode
        ValidHUId = 0
        InvalidHUId = 1
        NoUserLoggedIn = 99
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Try
            PrintMessageContent(oLogger)
            Dim sContainerID As String = _msg(0)("Container ID").FieldValue
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Processing unique validation for container id {0}.", sContainerID))
            End If
            If Not ValidateUserLoggedIn() Then
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("User is not logged in. Please sign in again."))
                End If
                Me._responseCode = ResponseCode.NoUserLoggedIn
                Me._responseText = "User is not logged in"
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
            If WMS.Logic.Container.Exists(sContainerID) Then
                'check if the container is from the current assigned picklist 
                Dim sSql As String = ""
                'If Not Session("PCKPicklist") Is Nothing Then
                '    Dim pcklst As Picklist = Session("PCKPicklist")
                '    sSql = String.Format("select COUNT(1) from PICKDETAIL where PICKLIST = '{0}' and TOCONTAINER = '{1}'", pcklst.PicklistID, sContainerID)
                '    'If Not oLogger Is Nothing Then
                '    '    oLogger.Write(String.Format("Sql for validating the container according to picklist: {0}{1}", vbCrLf, sSql))
                '    'End If
                '    If DataInterface.ExecuteScalar(sSql) > 0 Then
                '        If Not oLogger Is Nothing Then
                '            oLogger.Write(String.Format("Conatiner assigned to current picklist."))
                '        End If
                '        _responseCode = ResponseCode.ValidHUId
                '        _responseText = ""
                '        Me.FillSingleRecord(oLogger)
                '    Else
                '        If Not oLogger Is Nothing Then
                '            oLogger.Write(String.Format("Container ID is not unique. Returning error code."))
                '        End If
                '        _responseCode = ResponseCode.InvalidHUId
                '        _responseText = "Container ID is not unique"
                '        Me.FillSingleRecord(oLogger)
                '    End If
                'ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                '    Dim pcks As ParallelPicking = Session("PARPCKPicklist")
                '    sSql = String.Format("select count(1) from PARALLELPICKDETAIL ppd inner join PICKDETAIL pd on pd.PICKLIST = ppd.PICKLIST where PARALLELPICKID = '{0}' and TOCONTAINER = '{1}'", pcks.ParallelPickId, sContainerID)
                '    If DataInterface.ExecuteScalar(sSql) > 0 Then
                '        If Not oLogger Is Nothing Then
                '            oLogger.Write(String.Format("Conatiner assigned to current parallel picklist."))
                '        End If
                '        _responseCode = ResponseCode.ValidHUId
                '        _responseText = ""
                '        Me.FillSingleRecord(oLogger)
                '    Else
                '        If Not oLogger Is Nothing Then
                '            oLogger.Write(String.Format("Container ID is not unique. Returning error code."))
                '        End If
                '        _responseCode = ResponseCode.InvalidHUId
                '        _responseText = "Container ID is not unique"
                '        Me.FillSingleRecord(oLogger)
                '    End If
                If Not Session("UserAssignedTask") Is Nothing Then 'ElseIf Not Session("UserAssignedTask") Is Nothing Then

                    Dim oTask As WMS.Logic.Task = Session("UserAssignedTask")
                    If oTask.TASKTYPE = WMS.Lib.TASKTYPE.PARALLELPICKING Then
                        sSql = String.Format("select count(1) from PICKDETAIL pd left outer join PARALLELPICKDETAIL ppd on pd.PICKLIST = ppd.PICKLIST left outer join container cn on cn.CONTAINER = pd.TOCONTAINER where ppd.PARALLELPICKID='{0}' and TOCONTAINER='{1}' and isnull(cn.ACTIVITYSTATUS,'') = ''", oTask.ParallelPicklist, sContainerID)
                    Else
                        '    sSql = String.Format("select count(1) from PICKDETAIL pd left outer join PARALLELPICKDETAIL ppd on pd.PICKLIST = ppd.PICKLIST left outer join container cn on cn.CONTAINER = pd.TOCONTAINER where pd.PICKLIST='{0}' and TOCONTAINER='{1}' and isnull(cn.ACTIVITYSTATUS,'') = ''", oTask.Picklist, sContainerID)
                        sSql = String.Format("select count(1) from PICKDETAIL pd left outer join PARALLELPICKDETAIL ppd on pd.PICKLIST = ppd.PICKLIST left outer join container cn on cn.CONTAINER = pd.TOCONTAINER where pd.PICKLIST='{0}' and pd.TOCONTAINER='{1}' and isnull(cn.ACTIVITYSTATUS,'') = '' AND cn.STATUS ='{2}'", oTask.Picklist, sContainerID, WMS.Lib.Statuses.Container.STATUSNEW)

                    End If

                    If DataInterface.ExecuteScalar(sSql) > 0 Then
                        'check status of pick detail - if it is completed you cant use the same container

                        If Not oLogger Is Nothing Then
                            oLogger.Write(String.Format("Conatiner assigned to current picklist."))
                        End If

                        _responseCode = ResponseCode.ValidHUId
                        _responseText = ""
                        Me.FillSingleRecord(oLogger)
               
                    Else
                        If Not oLogger Is Nothing Then
                            oLogger.Write(String.Format("Container ID is not unique. Returning error code."))
                        End If
                        _responseCode = ResponseCode.InvalidHUId
                        _responseText = "Container ID is not unique"
                        Me.FillSingleRecord(oLogger)
                    End If
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Container ID is not unique. Returning error code.")
                    End If
                    _responseCode = ResponseCode.InvalidHUId
                    _responseText = "Container ID is not unique"
                    Me.FillSingleRecord(oLogger)
                End If
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("Container ID is valid.")
                End If
                _responseCode = ResponseCode.ValidHUId
                _responseText = ""
                Me.FillSingleRecord(oLogger)
            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error occured: " & ex.ToString)
            End If
            _responseCode = ResponseCode.InvalidHUId
            _responseText = ex.Message
            Me.FillSingleRecord(oLogger)
        End Try
        Return _resp
    End Function

End Class

