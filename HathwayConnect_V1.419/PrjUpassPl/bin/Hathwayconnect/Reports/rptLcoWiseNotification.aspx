<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptLcoWiseNotification.aspx.cs" Inherits="PrjUpassPl.Reports.rptLcoWiseNotification" %>

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
        .style67
        {
            width: 190px;
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
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                <div style="float:right">
                <button onclick="goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                     <div class="tblSearchItm" style="width: 50%;">
                    <table width="100%">

                    <tr>
                                        <td align="left" class="cal_image_holder">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; LCO Name&nbsp; :
                                            <asp:DropDownList ID="ddlLco" runat="server"  Height="19px" 
                                                Style="resize: none;" Width="304px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>


                        <tr>
                            <td align="center" class="cal_image_holder">
                                &nbsp;From Date :
                                <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1"></asp:TextBox>
                                <%--</td>
                    <td class="cal_image_holder" align="left">--%>
                                <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                    Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="cal_image_holder">
                                &nbsp;&nbsp;&nbsp; To Date :
                                <asp:TextBox runat="server" ID="txtTo" BorderWidth="1"></asp:TextBox>
                                <%--</td>
                    <td class="cal_image_holder" align="left">--%>
                                <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                    Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                       <tr>
                         <td align="center" class="cal_image_holder">
                               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Type :
                                <asp:DropDownList ID="ddlType" runat="server" Height="19px" Style="resize: none;"
                                                            Width="150px">
                                                             <asp:ListItem Text="Select All" Value="0"></asp:ListItem>
                                                           <asp:ListItem Text="BMAIL" Value="BMAIL"></asp:ListItem>
                                                        </asp:DropDownList>
                            </td>
                        
                        </tr>
                    </table>
                    <table width="100%">
                    <tr>
                        <td align="right">
                                <asp:Label ID="lblUser" runat="server" Text="Search By"></asp:Label>
                                <asp:Label ID="Label37" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                <asp:Label ID="Label44" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" class="style67">
                                <asp:RadioButtonList ID="RadSearchby" AutoPostBack="true" runat="server" 
                                    RepeatDirection="Horizontal" 
                                    onselectedindexchanged="RadSearchby_SelectedIndexChanged" Height="18px" 
                                    Width="185px">
                                    <asp:ListItem Value="0" Selected="True">Account No.</asp:ListItem>
                                    <asp:ListItem Value="1">VC No.</asp:ListItem>                                   
                                </asp:RadioButtonList>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtsearchpara" runat="server" Style="resize: none;" Width="150px" MaxLength="20"
                                    onkeydown="SetContextKey()"></asp:TextBox>                               
                                <asp:Button ID="btnSubmit" runat="server" CssClass="button" 
                                    OnClick="btnSubmit_Click" Text="Submit" UseSubmitBehavior="false" />
                            </td>
                    </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td style= align="center">                               
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <table>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                                Width="110px" OnClick="btnGenerateExcel_Click" Visible="false" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnGenerateExcel" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                     </tr>
                            <tr>
                                <td align="left">
                                    <div class="griddiv">
                                        <asp:GridView ID="grdAddPlantopup" runat="server" AllowPaging="true" 
                                            CssClass="Grid" OnPageIndexChanging="grdAddPlantopup_PageIndexChanging" 
                                            onsorting="grdAddPlantopup_Sorting" PageSize="100" ShowFooter="true" 
                                            Width="100%">
                                            <FooterStyle CssClass="GridFooter" />
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
                     <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()"></asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="" EventName="Click" />--%>
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
