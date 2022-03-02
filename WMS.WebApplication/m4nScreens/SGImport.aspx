<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SGImport.aspx.vb" Inherits="WMS.WebApp.SGImport" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SG Import</title>
    <!-- #include file="~/include/head.html" -->
</head>
<body onLoad="InitializeTimer()">
	<!-- #include file="~/include/Header.html" -->
	<form id="form1" runat="server">
			<cc3:screen id="Screen1" NoLoginRequired=true HideMenu=true HideBanner=true runat="server"></cc3:screen>
	</form>
	<script LANGUAGE = "JavaScript">
		var secs
		var timerID = null
		var timerRunning = false
		var delay = 700

		function InitializeTimer() {
			secs = 7
			StopTheClock()
			StartTheTimer()
		}
		
		function StopTheClock() {
			if (timerRunning)
        		clearTimeout(timerID)
			timerRunning = false
		}

		function StartTheTimer() {
			if (secs==0) {
				StopTheClock()
        	window.focus()
    	} else {
			self.status = secs
        	secs = secs - 1
        	timerRunning = true
        	timerID = self.setTimeout("StartTheTimer()", delay)
    	}
	}
	</script>
</body>
</html>

