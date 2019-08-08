<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptLcowiseUserdetails.aspx.cs" Inherits="PrjUpassPl.Reports.rptLcowiseUserdetails" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript">

        function InProgress() {

            document.getElementById("imgrefresh2").style.visibility = 'visible';
        }
        function onComplete() {

            document.getElementById("imgrefresh2").style.visibility = 'hidden';
        }
       
        function goBack() {
            window.history.back();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                    <div style="float: right">
                        <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                            Back</button>
                    </div>
                    <div class="tblSearchItm">
                        <table width="100%">
                            <tr>
                                <td align="right" width="53%">
                                    LCO :
                                    <asp:DropDownList ID="ddlLco" AutoPostBack="true" runat="server" Height="19px" Style="resize: none;"
                                        Width="304px">
                                    </asp:DropDownList>
                                </td>
                                <td align="left">
                                    &nbsp;
                                    <asp:Button runat="server" ID="Button1" Text="Search" CssClass="button" UseSubmitBehavior="false"
                                        OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table width="100%">
                        <tr>
                            <td align="left" class="style67">
                                <asp:Button runat="server" ID="btngrnExel" Text="Generate Excel" CssClass="button"
                                    Visible="false" UseSubmitBehavior="false" align="left" OnClick="btn_genExl_Click" />
                            </td>
                            <td class="style68">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td align="left">
                                <asp:Button runat="server" Width="60px" ID="btnAll" Text="All" CssClass="button"
                                    Visible="false" OnClick="btnAll_Click" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="griddiv">
                                    <asp:GridView ID="grdLcodet" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                                        ShowFooter="true" Width="100%" AllowSorting="true" AllowPaging="true" PageSize="100"
                                        OnRowDataBound="grdLcodet_RowDataBound" OnPageIndexChanging="grdLcodet_PageIndexChanging">
                                        <FooterStyle CssClass="GridFooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="left" FooterText="" />
                                            <asp:BoundField HeaderText="LCO Name" DataField="lconame" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Company Name" DataField="companyname" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Distributor" DataField="distributor" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="Sub Distributor" DataField="subdistributor" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="State" DataField="statename" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="City" DataField="cityname" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="User Count" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                FooterStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="Linkuser" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"usercnt")%>'
                                                        CommandName="Usercount" OnClick="Linkuser_Click"></asp:LinkButton>
                                                    <asp:Label ID="lblOperid1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"lcoid")%>'
                                                        Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
                <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                <asp:ImageButton ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()">
                </asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
           
            <asp:PostBackTrigger ControlID="btngrnExel" />
           
            <asp:PostBackTrigger ControlID="btnAll" />
        </Triggers>
    </asp:UpdatePanel>
    <cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server"
        TargetControlID="UpdatePanel1">
        <Animations>
            <OnUpdating>
               <Parallel duration="0">
                    <ScriptAction Script="InProgress();" /> 
               </Parallel>
            </OnUpdating>
            <OnUpdated>
               <Parallel duration="0">
                   <ScriptAction Script="onComplete();" /> 
               </Parallel>
            </OnUpdated>
        </Animations>
    </cc1:UpdatePanelAnimationExtender>
</asp:Content>
