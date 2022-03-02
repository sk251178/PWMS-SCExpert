Imports System
Imports System.Messaging
Imports System.Threading
Imports System.Data
Imports System.Data.Odbc
Imports Made4Net.DataAccess
Imports Made4Net.DataAccess.Schema
Imports System.Collections.Generic
Imports System.Configuration

Public Interface IMessageProcessor

    Sub ProcessQueue(ByVal qMsg As Message, ByVal e As PeekCompletedEventArgs)

End Interface
