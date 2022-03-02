<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="BaseAppComponentEditor.ascx.vb" Inherits="WMS.WebApp.BaseAppComponentEditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<cc2:tableeditor id="TE" runat="server" DefaultDT="SYS_AC_BASE" AfterUpdateMode="Grid" AfterInsertMode="Grid"
	AutoSelectGridItem="Never" SearchButtonPos="UnderSearchForm" GridMode="Normal" DefaultMode="Grid"
	ManualMode="False" GridPageSize="20" ObjectID="TE" SearchButtonGroup="Search1" GridType="Normal"
	EnableCSVExport="Global" EnableQuickChart="Global" EnableQuickReport="Global" EnableSavedSearch="Global"
	PersistSearchValues="Global" DESIGNTIMEDRAGDROP="3" ConnectionName="Made4NetSchema" DisableSwitches="n"
	ForbidRequiredFieldsInSearchMode="True"></cc2:tableeditor>
