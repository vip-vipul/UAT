<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="mstecafstbtranslcoconfig.aspx.cs" Inherits="PrjUpassPl.Master.mstecafstbtranslcoconfig" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*.GridFooter
        {
            border: 1px solid #094791;
            border-radius: 0px;
            color: White;
            background: #094791;
            width: 100px;
        }*/
        
        .cal_image_holder
        {
            width: 7%;
        }
        .Grid th a
        {
            color: #ffffff;
            cursor: pointer;
        }
        /*.style67
        {
            width: 142px;
        }
        .style68
        {
            width: 975px;
        }*/
        .transparent
        {
            /* IE 8 */
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=50)"; /* IE 5-7 */
            filter: alpha(opacity=50); /* Netscape */
            -moz-opacity: 0.5; /* Safari 1.x */
            -khtml-opacity: 0.5; /* Good browsers */
            opacity: 0.5;
        }
        .loader
        {
            position: fixed;
            text-align: center;
            height: 100%;
            width: 100%;
            top: 0;
            right: 0;
            left: 0;
            z-index: 9999999;
            background-color: #FFFFFF;
            opacity: 0.3;
            visibility: hidden;
        }
        .loader img
        {
            padding: 10px;
            position: fixed;
            top: 45%;
            left: 50%;
        }
    </style>
    <script>
        function goBack() {
            window.location.href = "../Reports/EcafPages.aspx";
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                    <div style="float: right">
                        <button onclick="return goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                            Back</button>
                    </div>
                    <div class="tblSearchItm" style="width: 100%;">
                        <table width="100%" align="center">
                            <tr>
                                <td align="center">
                                    State&nbsp;&nbsp; :
                                    <asp:DropDownList ID="ddlState" runat="server" Height="19px" AutoPostBack="true"
                                        Style="resize: none;" Width="304px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    City&nbsp;&nbsp;&nbsp;&nbsp; :
                                    <asp:DropDownList ID="ddlCity" runat="server" Height="19px" Style="resize: none;"
                                        Width="304px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    DAS&nbsp;&nbsp;&nbsp;&nbsp; :
                                    <asp:DropDownList ID="ddlDAS" runat="server" Height="19px" Style="resize: none;"
                                        Width="304px">
                                        <asp:ListItem Text="-- Select DAS --" Value=""></asp:ListItem>
                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="DAS I" Value="DAS II"></asp:ListItem>
                                        <asp:ListItem Text="DAS II" Value="DAS II"></asp:ListItem>
                                        <asp:ListItem Text="DAS III" Value="DAS III"></asp:ListItem>
                                        <asp:ListItem Text="DAS IV" Value="DAS IV"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    LCO&nbsp;&nbsp;&nbsp;&nbsp; :
                                    <asp:TextBox ID="txtlco" runat="server" Width="304px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    Exclude :
                                    <asp:DropDownList ID="ddlAdminLevel" runat="server" Height="19px" Style="resize: none;"
                                        Width="304px">
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td style="padding-left: 50px" align="center">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button" OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
