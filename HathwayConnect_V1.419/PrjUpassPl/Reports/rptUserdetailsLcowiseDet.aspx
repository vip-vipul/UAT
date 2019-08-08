<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptUserdetailsLcowiseDet.aspx.cs" Inherits="PrjUpassPl.Reports.rptUserdetailsLcowiseDet" %>

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
             return false;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                <div style="float:right">
                <button onclick="return goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <div>
                        <table width="100%">
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
                    <div class="griddiv">
                    <asp:GridView ID="grdLcodet" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                        ShowFooter="true" Width="100%" AllowSorting="true" AllowPaging="true" PageSize="100"
                        OnPageIndexChanging="grdLcodet_PageIndexChanging">
                        <FooterStyle CssClass="GridFooter" />
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="LCO Code" DataField="lcocode" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="User Id" DataField="username" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <%--<asp:BoundField HeaderText="User Name" DataField="userowner" HeaderStyle-HorizontalAlign="Left"
                    ItemStyle-HorizontalAlign="left"   />--%>
                            <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%# Eval("fname").ToString() %>&nbsp;<%# Eval("mname").ToString()%>&nbsp;<%# Eval("lname").ToString()%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Address" DataField="addr" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Pincode" DataField="code" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="BrmPoid" DataField="brmpoid" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="State" DataField="ststeid" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="City" DataField="cityid" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="User Type" DataField="flag" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Email" DataField="email" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Mobile No" DataField="mobno" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Account No" DataField="accno" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                                 <asp:BoundField HeaderText="DAS Area" DataField="DASAREA" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Balance" DataField="balance" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Inserted By" DataField="insby" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField HeaderText="Date" DataField="insdt" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="left" />
                        </Columns>
                    </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
                <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />

            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />--%>
            <asp:PostBackTrigger ControlID="btngrnExel" />
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
