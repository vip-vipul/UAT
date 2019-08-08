<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptAddPlanTransUSER.aspx.cs" Inherits="PrjUpassPl.Reports.rptAddPlanTransUSER" %>

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
             window.location.href = "../Reports/rptnoncasreport.aspx";
             return false
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlTransItemDet">
                <div class="maindive">
                <div style="float:right">
                <button onclick="return goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <table width="100%">
                        <tr>
                            <th align="left">
                                <asp:Label ID="Label1" ForeColor="Black" Font-Bold="true" Font-Size="9pt" Font-Names="Tahoma"
                                    runat="server" Text="LCO Name :"></asp:Label>&nbsp;
                                <asp:Label ID="lbllconm" ForeColor="Black" Font-Bold="false" Font-Size="9pt" Font-Names="Tahoma"
                                    runat="server"></asp:Label>
                            </th>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                    <table>
                        <tr align="left">
                            <td style="width: 100px">
                                <asp:Button runat="server" ID="btn_genExl" Text="Generate Excel" CssClass="button"
                                    Visible="false" align="left" OnClick="btn_genExl_Click" Width="100"/>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Button runat="server" ID="btnAll" Text="ALL" CssClass="button" UseSubmitBehavior="false"
                                    Visible="false" OnClick="btnAll_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div class="griddiv">
                                    <asp:GridView runat="server" ID="grdTransUserDet" CssClass="Grid" AutoGenerateColumns="false"
                                        ShowFooter="true" OnRowDataBound="grdTransUserDet_RowDataBound" AllowPaging="true"
                                        PageSize="5" OnSorting="grdTransUserDet_Sorting" OnRowCommand="grdTransUserDet_RowCommand"
                                        OnPageIndexChanging="grdTransUserDet_PageIndexChanging">
                                        <FooterStyle CssClass="GridFooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="lconame" HeaderText="LCO Name" SortExpression="lconame"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="95pt" />
                                            <%--<asp:HyperLinkField DataTextField="uid1" DataNavigateUrlFields="uid1" SortExpression="uid1"
                                  FooterText="Total" HeaderStyle-HorizontalAlign="Center"
                                ControlStyle-Width="75pt" Target="_blank" ItemStyle-HorizontalAlign="left" DataNavigateUrlFormatString="rptAddPlanTransDET.aspx?uid={0}"
                                HeaderText="User ID" />--%>
                                            <asp:TemplateField HeaderText="User ID" ItemStyle-HorizontalAlign="Left" FooterText="Total">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"uname")%>'
                                                        CommandName="UserName1"></asp:LinkButton>
                                                    <asp:Label ID="lbluid1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"uname")%>'
                                                        Visible="false"></asp:Label><asp:Label ID="lbllco" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"lconame")%>'
                                                            Visible="false"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="userowner" HeaderText="User Name" SortExpression="userowner"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="75pt" />
                                            <%-- <asp:BoundField DataField="amt" HeaderText="Balance" SortExpression="amt" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" ControlStyle-Width="45pt"  
                                FooterStyle-HorizontalAlign="Right" />--%>
                                            <asp:BoundField DataField="amtdd" HeaderText="Amount Deducted" SortExpression="amtdd"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ControlStyle-Width="45pt"
                                                FooterStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cnt" HeaderText="Count" SortExpression="cnt" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Right" ControlStyle-Width="45pt" FooterStyle-HorizontalAlign="Right" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
               <%-- <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                     <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()"></asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />--%>
            <asp:PostBackTrigger ControlID="btn_genExl" />
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
