<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="ReportPrint.aspx.cs" Inherits="PrjUpassPl.Transaction.ReportPrint" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%--<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Report Print</title>
    <link href="../CSS/main.css" rel="stylesheet" type="text/css" />
    <script src="../JS/printElement.js" type="text/javascript"></script>
    <style type="text/css">
        .topHead
        {
            background: #E5E5E5;
        }
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            margin: 10px;
            width: 60%;
        }
        .delInfoContent
        {
            width: 95%;
        }
        .scroller
        {
            overflow: auto;
            max-height: 250px;
        }
        .style67
        {
            width: 145px;
        }
        .style68
        {
            width: 104px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script>
        function printElem() {
            $("#CrystalReportViewer1").printElement(
				{
				    leaveOpen: false,
				    printMode: 'popup'
				});
        }
    </script>
    <%--<asp:Panel ID="pnlView" runat="server" ScrollBars="Auto">--%>
    <p align="left">
        &nbsp;&nbsp;&nbsp;&nbsp;
        <input onclick="printElem();" value="Print" type="button"
            style="border-bottom-style: inset; border-right-style: inset; border-top-style: inset;
            border-left-style: inset" />
        <%--<asp:Button ID="btnPrint" runat="server" Text="Print" Style="border-bottom-style: inset;
            border-right-style: inset; border-top-style: inset; border-left-style: inset"
            OnClick="btnPrint_Click" />--%>
        &nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Close" Style="border-bottom-style: inset;
            border-right-style: inset; border-top-style: inset; border-left-style: inset" />
    </p>
    <p align="left">
        <cr:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
            BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" />
        <p>
        </p>
        <p>
        </p>
    </p>
    <%--</asp:Panel>--%>
</asp:Content>
