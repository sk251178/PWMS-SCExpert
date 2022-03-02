Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Data
Imports System.Reflection

Public Class BaseBusinessObject

#Region "Data Base Methods"

    Protected Sub Load(ByVal rs As BaseBusinessObject)
        If BaseBusinessObject.Exist(rs) Then
            Dim type As Type = rs.[GetType]()
            Dim Sql As String = GenerateSelectStatement(rs)
            ' We can load from database
            Dim dt As New DataTable()
            Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt, False, "")

            For Each PropInfo As PropertyInfo In type.GetProperties()
                If PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False).Length > 0 Then
                    Dim fldAtt As DBFieldAttribute = DirectCast(PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False)(0), DBFieldAttribute)
                    If fldAtt IsNot Nothing Then
                        If Not Convert.IsDBNull(dt.Rows(0)(DirectCast(fldAtt, DBFieldAttribute).DBFieldName)) Then
                            Select Case GetPropertyRealType(PropInfo.PropertyType.FullName.ToLower())
                                Case "system.decimal"
                                    PropInfo.SetValue(Me, Convert.ToDecimal(dt.Rows(0)(DirectCast(fldAtt, DBFieldAttribute).DBFieldName)), Nothing)
                                    Exit Select
                                Case "system.double"
                                    PropInfo.SetValue(Me, Convert.ToDouble(dt.Rows(0)(DirectCast(fldAtt, DBFieldAttribute).DBFieldName)), Nothing)
                                    Exit Select
                                Case "system.string"
                                    PropInfo.SetValue(Me, Convert.ToString(dt.Rows(0)(DirectCast(fldAtt, DBFieldAttribute).DBFieldName)), Nothing)
                                    Exit Select
                                Case "system.int16"
                                    PropInfo.SetValue(Me, Convert.ToInt16(dt.Rows(0)(DirectCast(fldAtt, DBFieldAttribute).DBFieldName)), Nothing)
                                    Exit Select
                                Case "system.int32"
                                    PropInfo.SetValue(Me, Convert.ToInt32(dt.Rows(0)(DirectCast(fldAtt, DBFieldAttribute).DBFieldName)), Nothing)
                                    Exit Select
                                Case "system.int64"
                                    PropInfo.SetValue(Me, Convert.ToInt64(dt.Rows(0)(DirectCast(fldAtt, DBFieldAttribute).DBFieldName)), Nothing)
                                    Exit Select
                                Case "system.datetime"
                                    PropInfo.SetValue(Me, Convert.ToDateTime(dt.Rows(0)(DirectCast(fldAtt, DBFieldAttribute).DBFieldName)), Nothing)
                                    Exit Select
                                Case "system.boolean"
                                    PropInfo.SetValue(Me, Convert.ToBoolean(dt.Rows(0)(DirectCast(fldAtt, DBFieldAttribute).DBFieldName)), Nothing)
                                    Exit Select
                            End Select
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Protected Function GenerateSelectStatement(ByVal rs As BaseBusinessObject) As String
        Dim type As Type = rs.[GetType]()
        Dim Sql As String = "SELECT * FROM "
        Dim tblAtt As DBTableAttribute = DirectCast(type.GetCustomAttributes(GetType(DBTableAttribute), False)(0), DBTableAttribute)
        Sql = Sql + tblAtt.DBTableName & " WHERE "
        For Each PropInfo As PropertyInfo In type.GetProperties()
            If PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False).Length > 0 Then
                Dim fldAtt As DBFieldAttribute = DirectCast(PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False)(0), DBFieldAttribute)
                If fldAtt IsNot Nothing Then
                    If DirectCast(fldAtt, DBFieldAttribute).PrimaryKey Then
                        Select Case GetPropertyRealType(PropInfo.PropertyType.FullName.ToLower())
                            Case "system.decimal", "system.double", "system.string"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + PropInfo.GetValue(rs, Nothing) & "' AND "
                                End If
                                Exit Select
                            Case "system.int16", "system.int32", "system.int64"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + PropInfo.GetValue(rs, Nothing) & " AND "
                                End If
                                Exit Select
                            Case "system.datetime"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + Convert.ToDateTime(PropInfo.GetValue(rs, Nothing)).ToString("yyyy-MM-dd hh:mm:ss") & "' AND "
                                End If
                                Exit Select
                            Case "system.boolean"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + Convert.ToInt32(PropInfo.GetValue(rs, Nothing)) & " AND "
                                End If
                                Exit Select
                        End Select
                    End If
                End If
            End If
        Next
        Sql = Sql.TrimEnd(" "c)
        Sql = Sql.TrimEnd("D"c)
        Sql = Sql.TrimEnd("N"c)
        Sql = Sql.TrimEnd("A"c)
        Sql = Sql.TrimEnd(" "c)
        Return Sql
    End Function

    Public Shared Function Exist(ByVal rs As BaseBusinessObject) As Boolean
        Dim type As Type = rs.[GetType]()
        Dim Sql As String = "SELECT COUNT(1) FROM "
        Dim tblAtt As DBTableAttribute = DirectCast(type.GetCustomAttributes(GetType(DBTableAttribute), False)(0), DBTableAttribute)
        Sql = Sql + tblAtt.DBTableName & " WHERE "
        For Each PropInfo As PropertyInfo In type.GetProperties()
            If PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False).Length > 0 Then
                Dim fldAtt As DBFieldAttribute = DirectCast(PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False)(0), DBFieldAttribute)
                If fldAtt IsNot Nothing Then
                    If DirectCast(fldAtt, DBFieldAttribute).PrimaryKey Then
                        Select Case GetPropertyRealType(PropInfo.PropertyType.FullName.ToLower())
                            Case "system.decimal", "system.double", "system.string"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + PropInfo.GetValue(rs, Nothing) & "' AND "
                                End If
                                Exit Select
                            Case "system.int16", "system.int32", "system.int64"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + PropInfo.GetValue(rs, Nothing) & " AND "
                                End If
                                Exit Select
                            Case "system.datetime"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + Convert.ToDateTime(PropInfo.GetValue(rs, Nothing)).ToString("yyyy-MM-dd hh:mm:ss") & "' AND "
                                End If
                                Exit Select
                            Case "system.boolean"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + Convert.ToInt32(PropInfo.GetValue(rs, Nothing)) & " AND "
                                End If
                                Exit Select
                        End Select
                    End If
                End If
            End If
        Next
        Sql = Sql.TrimEnd(" "c)
        Sql = Sql.TrimEnd("D"c)
        Sql = Sql.TrimEnd("N"c)
        Sql = Sql.TrimEnd("A"c)
        Sql = Sql.TrimEnd(" "c)
        If Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Sub Save(ByVal rs As BaseBusinessObject)
        Dim Sql As String = ""
        Dim SqlValues As String = ""
        Dim SqlWhereClause As String = ""

        If Not BaseBusinessObject.Exist(rs) Then
            Dim type As Type = rs.[GetType]()
            Sql = "INSERT INTO "
            Dim tblAtt As DBTableAttribute = DirectCast(type.GetCustomAttributes(GetType(DBTableAttribute), False)(0), DBTableAttribute)
            Sql = Sql + tblAtt.DBTableName & " ("
            SqlValues = SqlValues & " VALUES ("
            For Each PropInfo As PropertyInfo In type.GetProperties()
                If PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False).Length > 0 Then
                    Dim fldAtt As DBFieldAttribute = DirectCast(PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False)(0), DBFieldAttribute)
                    If fldAtt IsNot Nothing Then
                        If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                            Select Case GetPropertyRealType(PropInfo.PropertyType.FullName.ToLower())
                                Case "system.decimal", "system.double", "system.string"
                                    Sql = Sql & DirectCast(fldAtt, DBFieldAttribute).DBFieldName & ","
                                    SqlValues = SqlValues & "'" & PropInfo.GetValue(rs, Nothing) & "',"
                                    Exit Select
                                Case "system.int16", "system.int32", "system.int64"
                                    Sql = Sql & DirectCast(fldAtt, DBFieldAttribute).DBFieldName & ","
                                    SqlValues = SqlValues & PropInfo.GetValue(rs, Nothing) & ","
                                    Exit Select
                                Case "system.datetime"
                                    If Convert.ToDateTime(PropInfo.GetValue(rs, Nothing)) <> DateTime.MinValue Then
                                        Sql = Sql & DirectCast(fldAtt, DBFieldAttribute).DBFieldName & ","
                                        SqlValues = SqlValues & "'" & Convert.ToDateTime(PropInfo.GetValue(rs, Nothing)).ToString("yyyy-MM-dd hh:mm:ss") & "',"
                                    End If
                                    Exit Select
                                Case "system.boolean"
                                    Sql = Sql & DirectCast(fldAtt, DBFieldAttribute).DBFieldName & ","
                                    SqlValues = SqlValues & Convert.ToInt32(PropInfo.GetValue(rs, Nothing)) & ","
                                    Exit Select
                                Case Else
                                    Sql = Sql & DirectCast(fldAtt, DBFieldAttribute).DBFieldName & ","
                                    SqlValues = SqlValues & "'" & PropInfo.GetValue(rs, Nothing) & "',"
                                    Exit Select
                            End Select
                        End If
                    End If
                End If
            Next
            Sql = Sql.TrimEnd(","c)
            SqlValues = SqlValues.TrimEnd(","c)
            Sql = Sql & ") "
            SqlValues = SqlValues & ")"
            Sql = Sql + SqlValues
        Else
            Dim type As Type = rs.[GetType]()
            Sql = "UPDATE "
            Dim tblAtt As DBTableAttribute = DirectCast(type.GetCustomAttributes(GetType(DBTableAttribute), False)(0), DBTableAttribute)
            Sql = Sql + tblAtt.DBTableName
            SqlValues = SqlValues + " SET "
            SqlWhereClause = SqlWhereClause = " WHERE "

            For Each PropInfo As PropertyInfo In Type.GetProperties()
                If PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False).Length > 0 Then
                    Dim fldAtt As DBFieldAttribute = DirectCast(PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False)(0), DBFieldAttribute)
                    If fldAtt IsNot Nothing Then
                        Select Case GetPropertyRealType(PropInfo.PropertyType.FullName.ToLower())
                            Case "system.decimal", "system.double", "system.string"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    If DirectCast(fldAtt, DBFieldAttribute).PrimaryKey Then
                                        SqlWhereClause = (SqlWhereClause + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + PropInfo.GetValue(rs, Nothing) & "' AND "
                                    Else
                                        SqlValues = (SqlValues + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + PropInfo.GetValue(rs, Nothing) & "',"
                                    End If
                                End If
                                Exit Select
                            Case "system.int16", "system.int32", "system.int64"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    If DirectCast(fldAtt, DBFieldAttribute).PrimaryKey Then
                                        SqlWhereClause = (SqlWhereClause + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + PropInfo.GetValue(rs, Nothing) & " AND "
                                    Else
                                        SqlValues = (SqlValues + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + PropInfo.GetValue(rs, Nothing) & ","
                                    End If
                                End If
                                Exit Select
                            Case "system.datetime"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    If Convert.ToDateTime(PropInfo.GetValue(rs, Nothing)) <> DateTime.MinValue Then
                                        If DirectCast(fldAtt, DBFieldAttribute).PrimaryKey Then
                                            SqlWhereClause = (SqlWhereClause + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + Convert.ToDateTime(PropInfo.GetValue(rs, Nothing)).ToString("yyyy-MM-dd hh:mm:ss") & " AND "
                                        Else
                                            SqlValues = (SqlValues + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + Convert.ToDateTime(PropInfo.GetValue(rs, Nothing)).ToString("yyyy-MM-dd hh:mm:ss") & "',"
                                        End If
                                    End If
                                End If
                                Exit Select
                            Case "system.boolean"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    If DirectCast(fldAtt, DBFieldAttribute).PrimaryKey Then
                                        SqlWhereClause = (SqlWhereClause + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + Convert.ToInt32(PropInfo.GetValue(rs, Nothing)) & " AND "
                                    Else
                                        SqlValues = (SqlValues + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + +Convert.ToInt32(PropInfo.GetValue(rs, Nothing)) & ","
                                    End If
                                End If
                                Exit Select
                            Case Else
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    If DirectCast(fldAtt, DBFieldAttribute).PrimaryKey Then
                                        SqlWhereClause = (SqlWhereClause + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + PropInfo.GetValue(rs, Nothing) & "' AND "
                                    Else
                                        SqlValues = (SqlValues + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + PropInfo.GetValue(rs, Nothing) & "',"
                                    End If
                                End If
                                Exit Select
                        End Select
                    End If
                End If
            Next
            SqlWhereClause = SqlWhereClause.TrimEnd(" "c)
            SqlWhereClause = SqlWhereClause.TrimEnd("D"c)
            SqlWhereClause = SqlWhereClause.TrimEnd("N"c)
            SqlWhereClause = SqlWhereClause.TrimEnd("A"c)
            SqlWhereClause = SqlWhereClause.TrimEnd(" "c)

            SqlValues = SqlValues.TrimEnd(","c)

            Sql = Sql + SqlValues + SqlWhereClause
        End If
        Made4Net.DataAccess.DataInterface.RunSQL(Sql)
    End Sub

    Public Shared Sub Delete(ByVal rs As BaseBusinessObject)
        Dim type As Type = rs.[GetType]()
        Dim Sql As String = "DELETE FROM "
        Dim tblAtt As DBTableAttribute = DirectCast(type.GetCustomAttributes(GetType(DBTableAttribute), False)(0), DBTableAttribute)
        Sql = Sql + tblAtt.DBTableName & " WHERE "
        For Each PropInfo As PropertyInfo In type.GetProperties()
            If PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False).Length > 0 Then
                Dim fldAtt As DBFieldAttribute = DirectCast(PropInfo.GetCustomAttributes(GetType(DBFieldAttribute), False)(0), DBFieldAttribute)
                If fldAtt IsNot Nothing Then
                    If DirectCast(fldAtt, DBFieldAttribute).PrimaryKey Then
                        Select Case GetPropertyRealType(PropInfo.PropertyType.FullName.ToLower())
                            Case "system.decimal", "system.double", "system.string"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + PropInfo.GetValue(rs, Nothing) & "' AND "
                                End If
                                Exit Select
                            Case "system.int16", "system.int32", "system.int64"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + PropInfo.GetValue(rs, Nothing) & " AND "
                                End If
                                Exit Select
                            Case "system.datetime"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = '") + Convert.ToDateTime(PropInfo.GetValue(rs, Nothing)).ToString("yyyy-MM-dd hh:mm:ss") & "' AND "
                                End If
                                Exit Select
                            Case "system.boolean"
                                If PropInfo.GetValue(rs, Nothing) IsNot Nothing Then
                                    Sql = (Sql + DirectCast(fldAtt, DBFieldAttribute).DBFieldName & " = ") + Convert.ToInt32(PropInfo.GetValue(rs, Nothing)) & " AND "
                                End If
                                Exit Select
                        End Select
                    End If
                End If
            End If
        Next
        Sql = Sql.TrimEnd(" "c)
        Sql = Sql.TrimEnd("D"c)
        Sql = Sql.TrimEnd("N"c)
        Sql = Sql.TrimEnd("A"c)
        Sql = Sql.TrimEnd(" "c)
        Made4Net.DataAccess.DataInterface.RunSQL(Sql)
    End Sub

    Protected Shared Function GetPropertyRealType(ByVal TypeFullName As String) As String
        If TypeFullName.IndexOf("system.nullable") >= 0 Then
            If TypeFullName.IndexOf("system.decimal") > 0 Then
                Return "system.decimal"
            End If
            If TypeFullName.IndexOf("system.double") > 0 Then
                Return "system.double"
            End If
            If TypeFullName.IndexOf("system.string") > 0 Then
                Return "system.string"
            End If
            If TypeFullName.IndexOf("system.int16") > 0 Then
                Return "system.int16"
            End If
            If TypeFullName.IndexOf("system.int32") > 0 Then
                Return "system.int32"
            End If
            If TypeFullName.IndexOf("system.int64") > 0 Then
                Return "system.int64"
            End If
            If TypeFullName.IndexOf("system.datetime") > 0 Then
                Return "system.datetime"
            End If
            If TypeFullName.IndexOf("system.boolean") > 0 Then
                Return "system.boolean"
            End If
        End If
        Return TypeFullName
    End Function

#End Region

End Class
