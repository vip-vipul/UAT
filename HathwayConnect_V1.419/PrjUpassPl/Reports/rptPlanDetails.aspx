<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptPlanDetails.aspx.cs" Inherits="PrjUpassPl.Reports.rptPlanDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
    function goBack() {
         window.location.href = "../Reports/rptnoncasreport.aspx";
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
        <div class="maindive">
        <div style="float:right">
                <button onclick="goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
            <asp:Button ID="btnPlanDl" runat="server" Text="Download Plan Details" OnClick="btnPlanDl_Click" />
            <br />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
            </div>
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
