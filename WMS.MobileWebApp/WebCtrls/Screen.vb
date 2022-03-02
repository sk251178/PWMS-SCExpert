Imports Made4Net.WebControls
Imports Made4Net.Mobile
Imports Made4Net.Mobile.WebCtrls
Imports System.ComponentModel

Namespace WebCtrls

    <CLSCompliant(False)> Public Class Screen
        Inherits WMS.WebCtrls.WebCtrls.Screen

        Protected _ShowMainMenu As Boolean
        Protected _ShowScreenMenu As Boolean

        Public Property ShowMainMenu() As Boolean
            Get
                Return _ShowMainMenu
            End Get
            Set(ByVal Value As Boolean)
                _ShowMainMenu = Value
            End Set
        End Property

        Public Property ShowScreenMenu() As Boolean
            Get
                Return _ShowScreenMenu
            End Get
            Set(ByVal Value As Boolean)
                _ShowScreenMenu = Value
            End Set
        End Property

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            EnforeProperties()
            MyBase.OnInit(e)
        End Sub

        Protected Sub EnforeProperties()
            Me.HideBanner = True
            Me.HideMenu = True
            'Me.NoLoginRequired = True
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            ' MyBase.OnLoad(e)
            EnforeProperties()
        End Sub

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            'If Made4Net.Mobile.MessageQue.Count > 0 Then
            '    Dim s As String
            '    s = String.Format("<script language=JavaScript>alert('{0}');</script>", Made4Net.Shared.Strings.PSQ(Made4Net.Mobile.MessageQue.Dequeue(), "\"))
            '    writer.Write(s)
            'End If
        End Sub

    End Class

End Namespace
