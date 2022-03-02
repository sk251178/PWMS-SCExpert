<%@ Page language="vb" contenttype="image/jpeg" CodeBehind="getmapimg.aspx.vb" AutoEventWireup="false" Inherits="WMS.WebApp.getmapimg" %>  
<%@ import namespace="system.drawing" %>  
<%@ import namespace="system.drawing.imaging" %>  
<%@ import namespace="system.drawing.drawing2d" %>  
<%  
 
response.clear ' make sure Nothing has gone to the client  

dim imgOut as bitmap
dim gimg as string = Request.Params.Get("guid")
imgOut=Session.Item(gimg)
imgOut.save(response.outputstream, imageformat.jpeg) ' output to the user  
imgOut.dispose() 
Session.Remove(gimg)
response.end  
 
%>  
