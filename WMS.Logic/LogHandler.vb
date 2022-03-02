Imports System
Imports System.IO
Imports System.Web
Imports Made4Net.Shared

Public Class LogHandler
    Implements ILogHandler

#Region "Variables"

    <CLSCompliant(False)> Protected _logfilepath As String
    <CLSCompliant(False)> Protected _filename As String
    <CLSCompliant(False)> Protected _sw As StreamWriter = Nothing
    <CLSCompliant(False)> Protected _writetimestamp As Boolean = True

#End Region

#Region "Properties"

    Public ReadOnly Property LogFilePath() As String Implements ILogHandler.LogFilePath
        Get
            Return _logfilepath
        End Get
    End Property

    Public ReadOnly Property FileName() As String Implements ILogHandler.FileName
        Get
            Return _filename
        End Get
    End Property

    Public Property WriteTimeStamp() As Boolean Implements ILogHandler.WriteTimeStamp
        Get
            Return _writetimestamp
        End Get
        Set(ByVal Value As Boolean)
            _writetimestamp = Value
        End Set
    End Property

#End Region

    Public Sub New(ByVal paramLogFileDir As String, ByVal paramLogFileName As String)
        Try
            _logfilepath = paramLogFileDir
            _filename = paramLogFileName
        Catch ex As Exception
        End Try
    End Sub

    Public Sub StartWrite() Implements ILogHandler.StartWrite
        Try
            If _logfilepath = "" Or _logfilepath Is Nothing Then
                Throw New ApplicationException("File Path not specified")
            End If
            If _filename = "" Or FileName Is Nothing Then
                Throw New ApplicationException("File name not specified")
            End If
            Dim dInfo As New DirectoryInfo(LogFilePath)
            If (Not dInfo.Exists) Then
                dInfo.Create()
            End If
            _sw = New StreamWriter(LogFilePath + FileName)
            _sw.AutoFlush = True
            dInfo = Nothing
        Catch ex As Exception
        End Try
    End Sub

    Public Sub Write(ByVal fmt As String, ByVal ParamArray args() As Object) Implements ILogHandler.Write
        Write(String.Format(fmt, args))
    End Sub

    Public Sub Write(ByVal Data As String) Implements ILogHandler.Write
        Try
            If _sw Is Nothing Then
                StartWrite()
            End If
            If _writetimestamp Then
                _sw.Write((String.Format("{0:dd-MM-yyyy HH:mm:ss.fff} ", DateTime.Now)))
            End If
            _sw.WriteLine(Data)
        Catch ex As Exception
        End Try
    End Sub

    Public Sub WriteException(ByVal Ex As System.Exception) Implements ILogHandler.WriteException
        If _sw Is Nothing Then
            StartWrite()
        End If
        writeSeperator()
        _sw.Write((String.Format("{0:dd-MM-yyyy HH:mm:ss.fff} ", DateTime.Now)))
        _sw.WriteLine("Error Occured: " & Ex.Message)
        _sw.WriteLine("Error Stack: " & Ex.StackTrace)
        writeSeperator()
    End Sub

    Public Sub WriteException(ByVal Ex As M4NException)
        If _sw Is Nothing Then
            StartWrite()
        End If
        writeSeperator()
        _sw.Write((String.Format("{0:dd-MM-yyyy HH:mm:ss.fff} ", DateTime.Now)))
        _sw.WriteLine("Error Occured: " & Ex.GetErrMessage(0))
        writeSeperator()
    End Sub

    Public Sub writeSeperator() Implements ILogHandler.writeSeperator
        Try
            _sw.WriteLine(New String("-", 100))
        Catch ex As Exception
        End Try
    End Sub

    Public Sub writeSeperator(ByVal pChr As Char, ByVal pLen As Integer) Implements ILogHandler.writeSeperator
        Try
            _sw.WriteLine(New String(pChr, pLen))
        Catch ex As Exception
        End Try
    End Sub

    Public Sub EndWrite() Implements ILogHandler.EndWrite
        Try
            If _sw Is Nothing Then Return
            _sw.Flush()
            _sw.Close()
            _sw = Nothing
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Function GetRDTLogger() As ILogHandler
        Return GetWMSRDTLogger()
    End Function
    Public Shared Function GetWMSRDTLogger() As ILogHandler
        Dim appLogger As WMS.Logic.LogHandler = Nothing
        If (HttpContext.Current Is Nothing) Then
            Return Nothing
        ElseIf Not (HttpContext.Current.Session("RDTLogger") Is Nothing) Then
            appLogger = HttpContext.Current.Session("RDTLogger")
        ElseIf Not (HttpContext.Current.Session("WMSLogger") Is Nothing) Then
            appLogger = HttpContext.Current.Session("WMSLogger")
        End If
        Return appLogger
    End Function
End Class