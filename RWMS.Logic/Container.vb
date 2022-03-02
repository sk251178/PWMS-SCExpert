Public Class Container

    Public fromcontainer As String = String.Empty
    Public targetcompany As String = String.Empty
    Public companytype As String = String.Empty
    Public workregionid As String = String.Empty
    Public ordertype As String = String.Empty
    Public consignee As String = String.Empty
    Public warehousearea As String = String.Empty
    Public picklist As String = String.Empty

    Public cont1fromlocation As String = String.Empty
    Public cont1tolocation As String = String.Empty

    Public cont2fromlocation As String = String.Empty

    Public totaskid As String = String.Empty
    Public fromtaskid As String = String.Empty
    Public tocontainer As String


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New()

    End Sub

    Public Function GetFreeVolumeContainer(ByVal task As String) As Decimal

        Dim sql As String

        sql = "select top 1  CUBECAPACITY - sumvolume as FreeVolume from DelConsContVolume "
        sql += " where task = '{0}' "
        sql = String.Format(sql, task)

        Dim dt As New DataTable

        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        'RWMS-2646 Commented
        'Return Convert.ToDecimal(dt.Rows(0)("FreeVolume").ToString)
        'RWMS-2646 Commented END
        'RWMS-2646
        If dt.Rows.Count > 0 Then
            Return Convert.ToDecimal(dt.Rows(0)("FreeVolume").ToString)
        End If
        'RWMS-2646 END

    End Function

    Public Sub setASSIGNED(ByVal CONTAINER As String)
        Dim sql As String = "select task from TASKS where TASKTYPE  = 'CONTDEL' and STATUS in ('AVAILABLE' ) and TOCONTAINER ='{0}' "
        sql = String.Format(sql, CONTAINER)
        Dim DT As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, DT)

        For Each DR As DataRow In DT.Rows
            Dim t As New WMS.Logic.Task(DR("task"))
            t.AssignUser(WMS.Logic.GetCurrentUser)

        Next
    End Sub
    Public Sub setDeAssignUser(ByVal CONTAINER As String)
        Dim sql As String = "select task from TASKS where TASKTYPE  = 'CONTDEL' and STATUS in ( 'ASSIGNED') and TOCONTAINER ='{0}' and userid = '{1}'"
        sql = String.Format(sql, CONTAINER, WMS.Logic.GetCurrentUser)
        Dim DT As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, DT)

        For Each DR As DataRow In DT.Rows
            Dim t As New WMS.Logic.Task(DR("task"))
            t.DeAssignUser()

        Next
    End Sub

    Public Function getDelTaskUser(ByVal CONTAINER As String) As String
        Dim task As String = ""
        Dim sql As String = "select top 1 task from TASKS where TASKTYPE  = 'CONTDEL' and STATUS in ('AVAILABLE', 'ASSIGNED') and TOCONTAINER ='{0}' and isnull(userid,'') in ('','{1}') order by task desc"
        sql = String.Format(sql, CONTAINER, WMS.Logic.GetCurrentUser)
        Dim DT As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, DT)

        For Each DR As DataRow In DT.Rows
            task = DR("task")
        Next

        Return task
    End Function

    Public Function getContainerConsContainer(ByVal container As String, ByVal task As String) As String

        If Not FillContainerShareParams(task) Then Return Nothing

        Dim sql As String
        Dim sumvolume As Decimal = GetFreeVolumeContainer(task)

        If picklist <> "" Then picklist = " picklist<>'" & picklist & "' and"

        sql = "select top 1 CONTAINER,TASK,fromlocation from DelConsContVolume "
        sql += " inner join (select distinct fromcontainer from DelContSimilarTasks where {9} fromcontainer <> '{0}' "
        sql += " and targetcompany = '{1}' and companytype='{2}' and {3} and ordertype = '{4}' AND tolocation='{6}' and isnull(userid,'') in ('','{7}') AND CONSIGNEE = '{8}' )  as SimilarTasks on SimilarTasks.FROMCONTAINER =  DelConsContVolume.CONTAINER  " 'AND FROMWAREHOUSEAREA='{9}'
        sql += " and sumvolume <= {5} "
        sql += " order by LOCSORTORDER "
        sql = String.Format(sql, fromcontainer, targetcompany, companytype, workregionid, ordertype, sumvolume, cont1tolocation, WMS.Logic.GetCurrentUser, consignee, picklist) ', warehousearea)

        Dim dt As New DataTable

        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then


            totaskid = dt.Rows(0)("task")
            tocontainer = dt.Rows(0)("CONTAINER")
            cont2fromlocation = dt.Rows(0)("fromlocation")

            setASSIGNED(dt.Rows(0)("CONTAINER"))

            Return dt.Rows(0)("task")
        Else
            Return Nothing
        End If

    End Function

    Public Function FillContainerShareParams(ByVal TASK As String) As Boolean
        Dim sql As String
        Dim dt As New DataTable
        sql = "select distinct fromlocation, tolocation, fromcontainer,targetcompany,companytype,workregionid,ordertype,CONSIGNEE,TOWAREHOUSEAREA,picklist from DelContSimilarTasks where TASK = '{0}'"
        sql = String.Format(sql, TASK)
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        If dt.Rows.Count = 0 Then Return False

        cont1fromlocation = dt.Rows(0)("fromlocation")
        cont1tolocation = dt.Rows(0)("tolocation")
        fromcontainer = dt.Rows(0)("fromcontainer")
        targetcompany = dt.Rows(0)("targetcompany")
        companytype = dt.Rows(0)("companytype")
        If Not IsDBNull(dt.Rows(0)("workregionid")) Then
            workregionid = " workregionid = '" & dt.Rows(0)("workregionid") & "' "
        Else
            workregionid = " workregionid is null "
        End If
        fromtaskid = TASK
        ordertype = dt.Rows(0)("ordertype")
        consignee = dt.Rows(0)("CONSIGNEE")
        warehousearea = dt.Rows(0)("TOWAREHOUSEAREA")
        picklist = dt.Rows(0)("picklist")
        Return True

    End Function

   
End Class
