Imports System.IO
Imports WMS.Logic
Imports HtmlAgilityPack

Public Class PWMSBaseLogger
    Inherits System.Web.UI.Page


    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        'Be sure to call the base class's OnLoad method!
        MyBase.OnLoad(e)
    End Sub

    Public Sub WriteToRDTLog(ByVal Data As String)
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If Not rdtLogger Is Nothing Then
            rdtLogger.Write(Data)
        End If
    End Sub

    Public Sub WriteToRDTLog(fmt As String, ParamArray args() As Object)
        WriteToRDTLog(String.Format(fmt, args))
    End Sub

    Private Sub Pre_RenderComplete(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        If Not Server.GetLastError() Is Nothing Then
            WriteToRDTLog("Pre_Init, server error, returning")
            Return
        End If
        Try
            Dim strRDTLogging As String = Made4Net.Shared.AppConfig.GetSystemParameter("RDTExtendedLogging", "0")
            If (Convert.ToInt32(strRDTLogging) = 1) Then
                Dim excludeListForScreens() As String = New String() {"Form", "__VIEWSTATE", "__EVENTVALIDATION", "__LASTFOCUS", "__EVENTTARGET", "__EVENTARGUMENT", "__EVENTVALIDATION", "RadScriptManager", "Screen1", "DO1"}
                Dim excludeListForM4NScreens() As String = New String() {"Form", "__VIEWSTATE", "__EVENTVALIDATION", "__LASTFOCUS", "__EVENTTARGET", "__EVENTARGUMENT", "__EVENTVALIDATION", "RadScriptManager", "Screen1"}
                Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                WriteToRDTLog("**********************************************************    Rendered to RDT    *********************************************************************")
                If Not Me.IsPostBack Then
                    Dim pageName() As String = HttpContext.Current.Request.Url.ToString().Split("/")
                    WriteToRDTLog(pageName(pageName.Length - 1))
                    Dim ctrl As Control = Page.FindControl("DO1")

                    If pageName(5).ToLower() = "LoginSaCH.aspx".ToLower() Then
                        Dim ctr As Control = Page.FindControl("Form2")
                        FindSafteyCheck(ctr)
                    End If
                    If Not ctrl Is Nothing Then
                        FindTheControls(ctrl)
                        If HttpContext.Current.Request.Url.ToString().ToLower().Contains("showmode.aspx") Then
                            FindTheLinks(ctrl)
                        End If
                        WriteToRDTLog("--Buttons-- ")
                        FindTheButtons(ctrl)
                    ElseIf HttpContext.Current.Request.Url.ToString().ToLower().Contains("main.aspx") OrElse HttpContext.Current.Request.Url.ToString().Contains("ShowMode.aspx") Then
                        FindTheLinks(Me)
                        WriteToRDTLog("--Buttons-- ")
                        FindTheButtons(Me)
                    End If
                ElseIf HttpContext.Current.Request.Url.ToString().ToLower().Contains("WeightCaptureNeededCLD1.aspx".ToLower()) Then
                    Dim pageName() As String = HttpContext.Current.Request.Url.ToString().ToLower().Split("/")
                    WriteToRDTLog(pageName(pageName.Length - 1))
                    Dim ctrl As Control = Page.FindControl("DO1")
                    If Not ctrl Is Nothing Then
                        FindTheControls(ctrl)
                        WriteToRDTLog("--Buttons-- ")
                        FindTheButtons(ctrl)
                    End If
                End If
                WriteToRDTLog("*************************************************************************************************************************************************")
            End If
        Catch ex As Exception
            WriteToRDTLog("RDT Extended logging error : " + ex.Message)
        End Try
    End Sub

    Private Sub Pre_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Server.GetLastError() Is Nothing Then
            WriteToRDTLog("Pre_Init, server error, returning")
            Return
        End If
        Try
            Dim strRDTLogging As String = Made4Net.Shared.AppConfig.GetSystemParameter("RDTExtendedLogging", "0")
            If (Convert.ToInt32(strRDTLogging) = 1) Then
                Dim excludeListForScreens() As String = New String() {"Form", "__VIEWSTATE", "__EVENTVALIDATION", "__LASTFOCUS", "__EVENTTARGET", "__EVENTARGUMENT", "__EVENTVALIDATION", "RadScriptManager", "Screen1", "DO1"}
                Dim excludeListForM4NScreens() As String = New String() {"Form", "__VIEWSTATE", "__EVENTVALIDATION", "__LASTFOCUS", "__EVENTTARGET", "__EVENTARGUMENT", "__EVENTVALIDATION", "RadScriptManager", "Screen1"}
                Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                If Me.IsPostBack Then
                    WriteToRDTLog("************************************************************   Post back  *********************************************************************")
                    Dim pageName() As String = HttpContext.Current.Request.Url.ToString().Split("/")
                    WriteToRDTLog(pageName(pageName.Length - 1))

                    Dim form As System.Collections.Specialized.NameValueCollection = HttpContext.Current.Request.Form()
                    'Dim valueOfViewState As String = form.Get("__VIEWSTATE")
                    'If Not String.IsNullOrEmpty(valueOfViewState) Then
                    '    Dim writer As New StringWriter()
                    '    Dim p As New ViewStateParser(writer)

                    '    WriteToRDTLog("Original ViewState: " & valueOfViewState)

                    '    Dim decode As Byte() = Convert.FromBase64String(valueOfViewState)
                    '    WriteToRDTLog("Plain Base64 Decoded ViewState: " & System.Text.Encoding.ASCII.GetString(decode))

                    '    p.ParseViewStateGraph(valueOfViewState)
                    '    WriteToRDTLog("ViewStateParser Decoded ViewState: " & writer.ToString())
                    'End If

                    If HttpContext.Current.Request.Url.ToString().ToLower().Contains("m4nscreens") Then
                        If form.Count > 0 Then
                            For Each key As String In form
                                If Not IfKeyMatchedExcludeList(key, excludeListForScreens) Then
                                    Dim valueWithGet As String = form.Get(key)
                                    WriteToRDTLog(key + " = " + valueWithGet)
                                End If
                            Next
                        End If
                    ElseIf HttpContext.Current.Request.Url.ToString().ToLower().Contains("screens") Then
                        If form.Count > 0 Then
                            For Each key As String In form
                                If Not IfKeyMatchedExcludeList(key, excludeListForM4NScreens) Then
                                    If key.StartsWith("DO1:") Then
                                        If Not (key.Contains("ActionBar:")) Then
                                            Dim currCtrl As Control = Page.FindControl(key)
                                            If Not (TypeOf currCtrl Is System.Web.UI.WebControls.Button OrElse TypeOf currCtrl Is System.Web.UI.WebControls.ImageButton) Then
                                                Dim splitedComponents() As String = key.Split(":")
                                                Dim actualControlNameOfInterest As String = splitedComponents(1)
                                                Dim controlNameForLabelSearch As String = actualControlNameOfInterest.Replace("val", "lbl")
                                                If controlNameForLabelSearch IsNot Nothing AndAlso controlNameForLabelSearch <> [String].Empty Then
                                                    Dim ctrl As Control = Page.FindControl(splitedComponents(0) & ":" & controlNameForLabelSearch)
                                                    If TypeOf ctrl Is System.Web.UI.WebControls.Label Then
                                                        Dim lbl As System.Web.UI.WebControls.Label = ctrl
                                                        Dim valueWithGet As String = form.Get(key)
                                                        WriteToRDTLog(key + "->" + t.Translate(lbl.Text) + " = " + valueWithGet)
                                                    Else
                                                        Dim valueWithGet As String = form.Get(key)
                                                        WriteToRDTLog(key + " = " + valueWithGet)
                                                    End If
                                                Else
                                                    Dim valueWithGet As String = form.Get(key)
                                                    WriteToRDTLog(key + " = " + valueWithGet)
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If

                    If HttpContext.Current.Request.Url.ToString().ToLower().Contains("m4nscreens") Then
                        WriteToRDTLog("Post back Control: " & getPostBackControlName())
                    ElseIf HttpContext.Current.Request.Url.ToString().ToLower().Contains("screens") Then
                        Dim postBackControlName As String = getPostBackControlName()
                        Dim pageform As System.Collections.Specialized.NameValueCollection = HttpContext.Current.Request.Form()
                        If pageform.Count > 0 Then
                            For Each key As String In pageform
                                If key.EndsWith(postBackControlName) Then
                                    WriteToRDTLog("Button Clicked: " & pageform.Get(key))
                                End If
                            Next
                        End If
                    End If

                    'WriteToRDTLog("Printing all the controls of the page")
                    'FindTheControls(Me)
                    WriteToRDTLog("*************************************************************************************************************************************************")

                End If
                If HttpContext.Current.Request.UrlReferrer.ToString().ToLower().EndsWith("main.aspx") AndAlso Not HttpContext.Current.Request.UrlReferrer.ToString().ToLower().Equals(HttpContext.Current.Request.Url.ToString().ToLower()) Then
                    ' Log the menu scrren chosen.
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("modeid")) Then
                        Dim sql As String = String.Format("SELECT ModeName  FROM MOBILEMODE where MODEID='{0}'", HttpContext.Current.Request.QueryString("modeid"))
                        Dim modeName As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql, "Made4NetSchema")
                        If Not String.IsNullOrEmpty(modeName) Then
                            Dim path As String = HttpContext.Current.Request.UrlReferrer.GetLeftPart(UriPartial.Path)
                            Dim pageName() As String = path.Split("/")
                            WriteToRDTLog("Menu Navigation : " + pageName(pageName.Length - 1) + " -> " + modeName)
                        End If
                    End If
                ElseIf HttpContext.Current.Request.UrlReferrer.ToString().ToLower().Contains("showmode.aspx") AndAlso Not HttpContext.Current.Request.UrlReferrer.ToString().ToLower().Equals(HttpContext.Current.Request.Url.ToString().ToLower()) Then
                    ' Log the menu scrren chosen.
                    Dim modeId As String = HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query).Get("modeid")
                    If Not String.IsNullOrEmpty(modeId) Then
                        Dim path As String = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)
                        Dim pageName() As String = path.Split("/")
                        Dim sql As String = String.Format("SELECT SCREENNAME FROM vMOBILESCREEN WHERE MODEID='{0}'  and Url like '%{1}%'", modeId, pageName(pageName.Length - 1))
                        Dim screenName As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql, "Made4NetSchema")
                        If Not String.IsNullOrEmpty(screenName) Then
                            sql = String.Format("SELECT ModeName  FROM MOBILEMODE where MODEID='{0}'", modeId)
                            Dim modeName As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql, "Made4NetSchema")
                            If Not String.IsNullOrEmpty(modeName) Then
                                WriteToRDTLog("Mode Navigation : " + modeName + " -> " + screenName)
                            Else
                                Dim cpageName() As String = HttpContext.Current.Request.UrlReferrer.ToString().Split("/")
                                WriteToRDTLog("Mode Navigation : " + cpageName(cpageName.Length - 1) + " -> " + screenName)
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            WriteToRDTLog("RDT Extended logging error, Exception " & ex.Message)
        End Try
    End Sub

    Private Sub Page_Error(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Error
        WriteToRDTLog("***************************Page_Error***************************")
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If Not rdtLogger Is Nothing Then
            rdtLogger.Write("Application Unhandled Exception: " & Server().GetLastError().Message)
            rdtLogger.Write("Stack Trace: " & Server.GetLastError().StackTrace())
        End If
    End Sub

    Public Sub FindTheControls(parent As Control)
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        For Each c As Control In parent.Controls
            If TypeOf c Is Control AndAlso Not String.IsNullOrEmpty(c.ID) AndAlso Not c.ID.EndsWith("dots") AndAlso c.Visible = True Then
                If TypeOf c Is Made4Net.WebControls.TextBoxValidated OrElse TypeOf c Is Made4Net.WebControls.MobileDropDown OrElse TypeOf c Is Made4Net.WebControls.FieldLabel Then
                    If TypeName(c) = "TextBoxValidated" Or TypeName(c) = "MobileDropDown" Or TypeName(c) = "FieldLabel" Then
                        Dim controlNameForLabelSearch As String = c.ID.Replace("val", "lbl")
                        If Not c.ID.Equals(controlNameForLabelSearch) AndAlso c.ID.EndsWith("val") Then
                            If controlNameForLabelSearch IsNot Nothing AndAlso controlNameForLabelSearch <> [String].Empty Then
                                Dim ctrl As Control = parent.FindControl(controlNameForLabelSearch)
                                Try
                                    If ctrl IsNot Nothing AndAlso TypeOf ctrl Is Made4Net.WebControls.FieldLabel Then
                                        Dim lbl As Made4Net.WebControls.FieldLabel = ctrl
                                        If Not lbl Is Nothing Then
                                            Dim valueWithGet As String = GetValue(c)
                                            WriteToRDTLog(c.ID + "->" + t.Translate(lbl.Text) + " = " + valueWithGet)
                                        Else
                                            Dim valueWithGet As String = GetValue(c)
                                            WriteToRDTLog(c.ID + " = " + valueWithGet)
                                        End If
                                    Else
                                        Dim valueWithGet As String = GetValue(c)
                                        WriteToRDTLog(c.ID + " = " + valueWithGet)
                                    End If
                                Catch ex As Exception
                                    WriteToRDTLog(ex.Message + "\n" + ex.StackTrace)
                                End Try
                            Else
                                Dim valueWithGet As String = GetValue(c)
                                WriteToRDTLog(c.ID + " = " + valueWithGet)
                            End If


                        End If
                    End If
                End If
            End If
            ' Dim valueWithGet As String = GetValue(c)
            'WriteToRDTLog(c.ID + " = " + TypeName(c))

            If c.Controls.Count > 0 Then
                Me.FindTheControls(c)
            End If
        Next

    End Sub

    Public Sub FindTheButtons(parent As Control)
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        For Each c As Control In parent.Controls

            If TypeOf c Is Control AndAlso Not String.IsNullOrEmpty(c.ID) AndAlso Not c.ID.EndsWith("dots") Then
                If TypeOf c Is Made4Net.WebControls.Button OrElse TypeOf c Is Made4Net.WebControls.ImageButton OrElse TypeOf c Is Made4Net.WebControls.LinkButton Then
                    If TypeName(c) = "Button" Or TypeName(c) = "ImageButton" Or TypeName(c) = "LinkButton" Then
                        If TypeOf c Is Made4Net.WebControls.Button Then
                            Dim button As Made4Net.WebControls.Button = c
                            If Not String.IsNullOrEmpty(button.Text) Then
                                WriteToRDTLog(button.Text)
                            End If
                        End If
                    End If
                End If
            End If
            If c.Controls.Count > 0 Then
                Me.FindTheButtons(c)
            End If
        Next
    End Sub

    Public Sub FindSafteyCheck(parent As Control)
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        For Each c As Control In parent.Controls
            If TypeOf c Is System.Web.UI.WebControls.DropDownList Then
                Dim cntrl As System.Web.UI.WebControls.DropDownList = c
                WriteToRDTLog(cntrl.ID + " : " + cntrl.SelectedValue)
                Return
            ElseIf c.Controls.Count > 0 Then
                Me.FindSafteyCheck(c)
            End If
        Next
    End Sub

    Public Sub FindTheLinks(parent As Control)
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        For Each c As Control In parent.Controls
            Dim m4nLitrlCntrl As Made4Net.WebControls.LiteralControl
            Dim sysLitrlCntrl As System.Web.UI.LiteralControl
            If TypeName(c) = "LiteralControl" Then
                If TypeOf c Is System.Web.UI.LiteralControl Then
                    sysLitrlCntrl = c
                    If Not String.IsNullOrEmpty(sysLitrlCntrl.Text) AndAlso sysLitrlCntrl.Text.StartsWith("<a") Then
                        Dim htmlDoc As HtmlDocument = New HtmlDocument()
                        htmlDoc.LoadHtml(sysLitrlCntrl.Text)
                        Dim href As String = htmlDoc.DocumentNode.SelectNodes("//a").FindFirst("a").Attributes("href").Value
                        Dim innerHtml As String = htmlDoc.DocumentNode.SelectNodes("//a").FindFirst("a").InnerHtml

                        Dim components() As String = innerHtml.Split(New String() {"<br>"}, StringSplitOptions.None)
                        If components.Length > 1 Then
                            WriteToRDTLog(components(1) + " : " + href)
                        Else
                            WriteToRDTLog(innerHtml + " : " + href)
                        End If

                    End If
                ElseIf TypeOf c Is Made4Net.WebControls.LiteralControl Then
                    m4nLitrlCntrl = c
                    If Not String.IsNullOrEmpty(m4nLitrlCntrl.Text) AndAlso m4nLitrlCntrl.Text.StartsWith("<a") Then
                        Dim htmlDoc As HtmlDocument = New HtmlDocument()
                        htmlDoc.LoadHtml(m4nLitrlCntrl.Text)
                        Dim href As String = htmlDoc.DocumentNode.SelectNodes("//a").FindFirst("a").Attributes("href").Value
                        Dim innerHtml As String = htmlDoc.DocumentNode.SelectNodes("//a").FindFirst("a").InnerHtml


                        Dim components() As String = innerHtml.Split(New String() {"<br>"}, StringSplitOptions.None)
                        If components.Length > 1 Then
                            WriteToRDTLog(components(1) + " : " + href)
                        Else
                            WriteToRDTLog(innerHtml + " : " + href)
                        End If
                    End If
                End If
            End If
            If c.Controls.Count > 0 Then
                Me.FindTheLinks(c)
            End If
        Next
    End Sub

    Private Function getPostBackControlName() As String

        Dim control As Control = Nothing

        'first we will check the "__EVENTTARGET" because if post back made by       the controls

        'which used "_doPostBack" function also available in Request.Form collection.

        Dim ctrlname As String = Page.Request.Params("__EVENTTARGET")

        If ctrlname IsNot Nothing AndAlso ctrlname <> [String].Empty Then
            control = Page.FindControl(ctrlname)
        Else
            ' if __EVENTTARGET is null, the control is a button type and we need to
            ' iterate over the form collection to find it

            Dim ctrlStr As String = [String].Empty

            Dim c As Control = Nothing

            For Each ctl As String In Page.Request.Form
                'handle ImageButton they having an additional "quasi-property" in their Id which identifies
                'mouse x and y coordinates

                If ctl.EndsWith(".x") OrElse ctl.EndsWith(".y") Then

                    ctrlStr = ctl.Substring(0, ctl.Length - 2)
                    c = Page.FindControl(ctrlStr)
                Else
                    c = Page.FindControl(ctl)
                End If

                If TypeOf c Is System.Web.UI.WebControls.Button OrElse TypeOf c Is System.Web.UI.WebControls.ImageButton Then
                    control = c
                    Exit For
                End If

            Next
        End If

        Return control.ID
    End Function

    Private Function IfKeyMatchedExcludeList(key As String, excludeList As String()) As Boolean
        Dim matchFound As Boolean = False

        For Each term As String In excludeList
            If key.Contains(term) Then
                matchFound = True
                Exit For
            End If
        Next
        Return matchFound

    End Function

    Private Function GetValue(c As Control) As String
        Dim value As String = String.Empty
        If Not c Is Nothing Then
            If TypeName(c) = "TextBoxValidated" Then
                Dim cntrl As Made4Net.WebControls.TextBoxValidated = c
                value = cntrl.Value
            ElseIf TypeName(c) = "MobileDropDown" Then
                Dim cntrl As Made4Net.WebControls.MobileDropDown = c
                If Not cntrl.TableData Is Nothing Then
                    value = cntrl.SelectedValue
                Else
                    value = String.Empty
                End If
            ElseIf TypeName(c) = "FieldLabel" Then
                Dim cntrl As Made4Net.WebControls.FieldLabel = c
                value = cntrl.Value
            ElseIf TypeName(c) = "Button" Then
                Dim cntrl As Made4Net.WebControls.Button = c
                value = cntrl.Text
            ElseIf TypeName(c) = "ImageButton" Then
                Dim cntrl As Made4Net.WebControls.ImageButton = c
                value = cntrl.AlternateText
            ElseIf TypeName(c) = "ImageButton" Then
                Dim cntrl As Made4Net.WebControls.LinkButton = c
                value = cntrl.Text
            ElseIf TypeName(c) = "LiteralControl" Then
                Dim cntrl As System.Web.UI.LiteralControl = c
                value = cntrl.Text
            ElseIf TypeName(c) = "DropDownList" Then
                Dim cntrl As System.Web.UI.WebControls.DropDownList = c
                value = cntrl.SelectedValue
            End If
        End If
        Return value
    End Function


End Class