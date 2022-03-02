<%@ Page CodeBehind="HelpScreen.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="WMS.WebApp.HelpScreen" %>
<html>
<head>    
    <title>WarehouseExpert™ Online Help</title>    
    <script lang="javascript">

		var defaultContentPageSet = false;

        function documentElement(id){
            return document.getElementById(id);
        }

        function getDefaultTopic()
        {
			return defaultTopic;
        }

		function setDefaultContentPage()
		{
			if(!defaultContentPageSet)
			{
				window.frames['webcontent'].location = getDefaultTopic();
				defaultContentPageSet = true;
			}
		}

    </script>
    <style>
        frame
        {
            padding: 0;
            margin: 0;
        }
    </style>
    <style type=text/css>frame#webcontent {border-left: 3px solid #426bad;}</style>
</head>

<frameset  frameborder="1" bordercolor="#f0f0f0" border="0" rows="29,*">
    <frame id="webtoolbar"  name="webtoolbar" scrolling="no" noresize="noresize" target="webcontent" src="../Help/webtoolbar.html" style="border-bottom: 1px solid buttonface;">
    <frameset frameborder="6"  bordercolor="#f0f0f0" border="6" cols="250,*" id="nav" style="cursor: col-resize;">
        <frame id="webnavbar"  name="webnavbar" target="webcontent" src="../Help/webnav.html" onresizeend="ResizeContentElements">
        <frame id="webcontent" name="webcontent" src="../Help/webblank.html" style="border-right: none;" onload="setDefaultContentPage()">
    </frameset>
    <noframes>
        <body>
            <p>This page uses frames, but your browser doesn't support them.</p>
        </body>
    </noframes>
</frameset>

</html>
