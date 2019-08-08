<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmDashboard.aspx.cs" Inherits="PrjUpassPl.Master.frmDashboard" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" >

        $(document).ready(function () {
            $(".EmptyData").parents("table").css("border-width", "0px").prop("border", "0");
        });
        function back() {

            window.location.href = "../Master/mstLCOAdminPages.aspx";
            return false;
        }
    </script>
    <style type="text/css">
        .delInfo
        {
            /*padding: 10px;
            border: 1px solid #094791;*/
            width: 95%;
            margin: 5px;
            padding: 10px;
            border: 1px solid #094791;
        }
        .delInfoContent
        {
            width: 95%;
        }
        .style67
        {
            width: 365px;
        }
        .style71
        {
            width: 258px;
        }
        .style74
        {
            width: 295px;
        }
        .style75
        {
            width: 70px;
        }
        .style76
        {
            width: 487px;
        }
        .style77
        {
            width: 539px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:Panel runat="server" ID="pnlRegisterLCO">
        <div class="maindive">
         
            <table width="30%" >
                <tr>
                    <td align="left" class="cal_image_holder">
                        <b>
                        <asp:Label ID="Label144" runat="server" Text="LCO "></asp:Label>
                        </b>
                    </td>
                    <td>
                        <b>
                        <asp:Label ID="Label145" runat="server" Text=":"></asp:Label>
                        </b>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddllco" runat="server" OnSelectedIndexChanged="ddllco_SelectedIndexChanged"
                                            AutoPostBack="true" Width="250">
                        </asp:DropDownList>
                    </td>
                </tr>

                </table>
                <cc1:TabContainer ID="Dashboard" Width="95%" runat="server" ScrollBars="Auto" 
                    ActiveTabIndex="0">
                    <cc1:TabPanel ID="TabPanel1" runat="server" TabIndex="1" HeaderText="Overview" >
                        <ContentTemplate>
                            <br />
                            <div style="float: left;">
                                <center>
                                    <h4>
                                        Pack Dispersion
                                    </h4>
                                </center>
                                <asp:Chart ID="Chart1" runat="server" Height="500px" Width="500px" RightToLeft="No">
                                    <Series>
                                        <asp:Series Name="Series1" XValueMember="title" YValueMembers="value" Color="255, 128, 112"
                                            ShadowColor="Black" ChartArea="ChartArea1" ChartType="Pie" CustomProperties="PieLabelStyle=Outside"
                                            Legend="Dotnet Chart Example" />
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
                                            <Area3DStyle Enable3D="True" LightStyle="Realistic" />
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                            <div style="float: right;width: 45%;;">
                                <center>
                                    <h4>
                                        Last Refeshed at : &nbsp;&nbsp; <asp:Label ID="lblSummaryason" runat="server"></asp:Label></h4>
                                </center>

                                <asp:GridView ID="grdsubscriberBase" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                                        ShowFooter="false" Width="100%" EnableModelValidation="True">
                                        <Columns>
                                            <asp:BoundField HeaderText="Particulars" DataField="title" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Total" DataField="col1" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Main TV" DataField="col2" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Child TV" DataField="col3" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <FooterStyle CssClass="GridFooter" />
                                    </asp:GridView>
                                    <asp:GridView ID="grdsubscriberBase2" runat="server" AutoGenerateColumns="False"
                                        CssClass="Grid" ShowFooter="false" ShowHeader="false" Width="100%" EnableModelValidation="True">
                                        <Columns>
                                            <asp:BoundField HeaderText="Particulars" DataField="title" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Total" DataField="col1" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Main TV" DataField="col2" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Child TV" DataField="col3" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <FooterStyle CssClass="GridFooter" />
                                    </asp:GridView>
                                    <asp:GridView ID="grdbase1" runat="server" AutoGenerateColumns="False"
                                        CssClass="Grid" ShowFooter="false" ShowHeader="false" Width="100%" EnableModelValidation="True">
                                        <Columns>
                                            <asp:BoundField HeaderText="Particulars" DataField="title" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Total" DataField="col1" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Main TV" DataField="col2" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Child TV" DataField="col3" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <FooterStyle CssClass="GridFooter" />
                                    </asp:GridView>
                              
                                <div style="float: right">
                                    All Figures in nos.
                                </div>
                                <br />
                                <center>
                                    <h4 style="display:none">
                                        Revenue Enhancement Opportunity</h4>
                                </center>
                                <asp:GridView ID="grdOverview2" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                                    Width="100%" AllowSorting="True" EnableModelValidation="True" OnRowDataBound="grdOverview2_RowDataBound" Visible="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="Particulars" DataField="title">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Total Revenue" DataField="col1" DataFormatString="{0:0}"
                                            HtmlEncode="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="LCO Share" DataField="col2" DataFormatString="{0:0}"
                                            HtmlEncode="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Hathway Share" DataField="col3" DataFormatString="{0:0}"
                                            HtmlEncode="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle CssClass="GridFooter" />
                                </asp:GridView>
                                <div style="float: right;display:none">
                                    All Figures in Rs.
                                </div>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tbpDash" runat="server" HeaderText="Base Movement">
                        <ContentTemplate>
                            <br />
                            <div style="text-align: center">
                                <table style="width: 100%; text-align: left;">
                                    <tr>
                                        <td align="center" class="style77" colspan="2">
                                            <h4>&nbsp; Base Movement Summary for the Month</h4>
                                        </td>
                                        <td align="left">
                                            
                                        </td>
                                    </tr>
                                </table>
                                <div style="padding-left: 20%; padding-right: 20%">
                                      <asp:GridView ID="grdoverView" runat="server" AutoGenerateColumns="False" CssClass="Grid" Style="border: none !important"
                                    Width="100%" AllowSorting="True" EnableModelValidation="True" OnRowDataBound="grdoverView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField HeaderText="Particulars" DataField="title">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="" DataField="priority">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Total" DataField="col1">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Main TV" DataField="col2">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Child TV" DataField="col3">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle CssClass="GridFooter" />
                                </asp:GridView>
                                <b style="color:Blue">Please note the Opening to Closing balance may not tally due to re-activations</b>
                                    <%--  <asp:GridView ID="grdsubscriberBase1" runat="server" AutoGenerateColumns="False"
                                        CssClass="Grid" ShowFooter="false" ShowHeader="false" Width="90%" EnableModelValidation="True"
                                        >
                                        <Columns>
                                            <asp:BoundField HeaderText="Particulars" DataField="title" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Total" DataField="col1" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Main TV" DataField="col2" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Child TV" DataField="col3" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <FooterStyle CssClass="GridFooter" />
                                    </asp:GridView>--%>
                                    
                                    <%--    <asp:GridView ID="grdsubscriberBase3" runat="server" AutoGenerateColumns="False"
                                        CssClass="Grid" ShowFooter="false" ShowHeader="false" Width="90%" EnableModelValidation="True"
                                       >
                                        <Columns>
                                            <asp:BoundField HeaderText="Particulars" DataField="title" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Total" DataField="col1" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center"  DataFormatString="{0:0}"/>
                                            <asp:BoundField HeaderText="Main TV" DataField="col2" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:0}" />
                                            <asp:BoundField HeaderText="Child TV" DataField="col3" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center"  DataFormatString="{0:0}" />
                                        </Columns>
                                        <FooterStyle CssClass="GridFooter" />
                                    </asp:GridView>--%>
                                    <center>
                                        <h4 style="display:none"  >
                                            Aging of Disconnected STB</h4>
                                    </center>
                                    <asp:GridView ID="grdsubscriberBase4" runat="server" AutoGenerateColumns="False"
                                        CssClass="Grid" ShowFooter="false" ShowHeader="false" Width="90%" EnableModelValidation="True"
                                        OnRowDataBound="grdsubscriberBase4_RowDataBound" Visible="false">
                                        <Columns>
                                            <asp:BoundField HeaderText="Particulars" DataField="title" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Total" DataField="col1" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Main TV" DataField="col2" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Child TV" DataField="col3" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <FooterStyle CssClass="GridFooter" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tbpDash1" runat="server" HeaderText="Expiry">
                        <ContentTemplate>
                            <br />
                            <table style="width: 100%; text-align: center;">
                                <tr>
                                    <td align="right" class="style76">
                                        &nbsp;Last Refeshed at :
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblexpsumm" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="grdExpiry" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                                ShowFooter="false" Width="100%" AllowSorting="True" EnableModelValidation="True"
                                OnRowCreated="grdExpiry_OnRowCreated" OnRowDataBound="grdExpiry_RowDataBound"
                                Style="border: none !important">
                                <Columns>
                                    <asp:BoundField HeaderText="Particulars" DataField="title">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total" DataField="col16" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col17" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col18" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total" DataField="col4" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col5" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col6" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total" DataField="col7" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col8" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col9" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total" DataField="col10" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col11" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col12" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total" DataField="col13" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col14" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col15" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="" DataField="priority" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle CssClass="GridFooter" />
                                <EmptyDataRowStyle CssClass="EmptyData" />
                            </asp:GridView>
                            <asp:GridView ID="GrdExpiry1" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                                ShowFooter="false" ShowHeader="false" Width="100%" AllowSorting="True" EnableModelValidation="True"
                                OnRowDataBound="grdExpiry1_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="Particulars" DataField="title">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total" DataField="col16" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col17" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col18" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total" DataField="col4" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col5" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col6" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total" DataField="col7" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col8" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col9" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total" DataField="col10" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col11" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col12" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total" DataField="col13" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col14" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col15" DataFormatString="{0:0}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle CssClass="GridFooter" />
                            </asp:GridView>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Retention"  Visible="false">
                        <ContentTemplate>
                            <br />
                            <table style="width: 100%; text-align: center;">
                                <tr>
                                    <td align="right" class="style74">
                                        &nbsp;Current Month Opening Active STB Base :
                                    </td>
                                    <td align="left" class="style75">
                                        <asp:Label ID="lblCurrentStb" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" class="style71">
                                        Last Month Opening Active STB Base :
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblLastSTB" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" class="style71">
                                        Last Month Renewal :
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblLastRenewal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style74">
                                        Total Expiry FTM :
                                    </td>
                                    <td align="left" class="style75">
                                        <asp:Label ID="lblTotExpiry" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" class="style71">
                                        Last month Reduction % :
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblLastReduction" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" class="style71">
                                    </td>
                                    <td align="left">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style74">
                                        Renewal MTD :
                                    </td>
                                    <td align="left" class="style75">
                                        <asp:Label ID="lblRenewlMTD" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" class="style71">
                                        Last Month Expiry :
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblLastExpiry" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" class="style71">
                                    </td>
                                    <td align="left">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style74">
                                        MTD Achievement % :
                                    </td>
                                    <td align="left" class="style75">
                                        <asp:Label ID="lblMTDAchieved" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" class="style71">
                                        Last Month Achievement % :
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblLastAchievement" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" class="style71">
                                    </td>
                                    <td align="left">
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="grdretention1" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                                ShowFooter="false" Width="100%" AllowSorting="True" EnableModelValidation="True" >
                                <Columns>
                                    <asp:BoundField HeaderText="Date" DataField="title">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="For the Month #" DataField="col1">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="FTD" DataField="col2">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Today + 1" DataField="col3">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Today + 2" DataField="col4">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Today + 3" DataField="col5">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Today + 4" DataField="col6">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle CssClass="GridFooter" />
                            </asp:GridView>
                            <asp:GridView ID="grdretention2" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                                ShowFooter="true" Width="100%" AllowSorting="True" EnableModelValidation="True">
                                <Columns>
                                    <asp:BoundField HeaderText="Expired Bucket" DataField="title" FooterText="Total :">
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Count" DataField="col1">
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col2">
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col3">
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Value" DataField="col4">
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Main TV" DataField="col5">
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Child TV" DataField="col6">
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="150px" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle CssClass="GridFooter" />
                            </asp:GridView>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
    
        </div>
    </asp:Panel>
</asp:Content>
