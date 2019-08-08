<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="mstecafstbtranslcoapp.aspx.cs" Inherits="PrjUpassPl.Master.mstecafstbtranslcoapp" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>
    <style>
        #popout.opened
        {
            right: 0px;
        }
    </style>
    <style type="text/css">
        #popout
        {
            position: absolute;
            top: 0;
            right: 0;
            width: 200px;
            background-color: #ccc;
        }
    </style>
    <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 650px;
            margin: 5px;
        }
        .delInfo1
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 650px;
            margin: 5px;
        }
        .delInfoContent
        {
            width: 95%;
        }
        .nontrColor
        {
            border: 0px solid #094791;
        }
        
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
    <script type="text/javascript">
        function goBack() {
            window.location.href = "../Reports/EcafPages.aspx";
            return false;
        }

        function closemsgbox() {
            window.location.href = "../Reports/EcafPages.aspx";
            return false;
        }

        function closemsgboxErr() {

            $find("mpeMsgBoxErr").hide();

        }
        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:UpdatePanel runat="server" ID="upl">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlRegisterLCO">
                <div class="maindive">
                    <div style="float: right">
                        <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                            Back</button>
                    </div>
                    <div class="griddiv">
                        <div id="DivGried" runat="server">
                            <table align="center" width="100%">
                                <tr>
                                    <td align="center">
                                        <asp:GridView runat="server" ID="grdSTBSwap" CssClass="Grid" AutoGenerateColumns="false"
                                            Width="700px" ShowFooter="true">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkSTB" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="STB" DataField="stbno" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Amount" DataField="amount" HeaderStyle-HorizontalAlign="right"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                <asp:BoundField HeaderText="LCO Code" DataField="lco" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Date" DataField="insdate" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="400px" HeaderText="STB Status"
                                                    HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdntransid" runat="server" Value='<%# Eval("transid").ToString()%>' />
                                                        <asp:RadioButton ID="RdoAccept" runat="server" Text="Accept" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:RadioButton ID="RdoCancel" runat="server" Text="Cancel" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="Numeric" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        Remark:
                                        <asp:TextBox ID="txtremark" runat="server" TextMode="MultiLine" Height="40px" Width="250px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="90%" align="center">
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Button ID="BtnSubmit" runat="server" Text="Submit" OnClick="BtnSubmit_Click" />
                                        &nbsp;&nbsp;
                                        <input id="Button3" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                            onclick="goBack();" />
                                </tr>
                            </table>
                        </div>
                    </div>
            </asp:Panel>
            <!---------------------------cnfmPopup------------------------>
            <cc1:ModalPopupExtender ID="popMsgBox" runat="server" BehaviorID="mpeMsgBox" TargetControlID="hdnMsgBox"
                PopupControlID="pnlMsgBox">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnMsgBox" runat="server" />
            <asp:Panel ID="pnlMsgBox" runat="server" CssClass="Popup" Style="width: 330px; height: 160px;
                display: none">
                <%-- display: none; --%>
                <center>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                &nbsp;&nbsp;&nbsp;Confirmation
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table width="90%">
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="lblResponseMsg" ForeColor="Blue" runat="server"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:HiddenField ID="hdnpopstatus" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="btncnfmBlck" runat="server" Text="Confirm" OnClick="btncnfmBlck_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="Btnclose" runat="server" OnClick="btnClose_Click" Text="Close" class="button" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <!---------------------------Close------------------------>
            <cc1:ModalPopupExtender ID="PopMsgBoxErr" runat="server" BehaviorID="mpeMsgBoxErr"
                TargetControlID="hdnMsgBoxErr" PopupControlID="pnlMsgBoxErr">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnMsgBoxErr" runat="server" />
            <asp:Panel ID="pnlMsgBoxErr" runat="server" CssClass="Popup" Style="width: 330px;
                height: 160px; display: none">
                <%-- display: none; --%>
                <center>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                &nbsp;&nbsp;&nbsp;Error
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table width="90%">
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="lblerror" ForeColor="Blue" runat="server"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:HiddenField ID="HiddenField2" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <input id="Button1" class="button" runat="server" type="button" value="Close" style="width: 100px;"
                                    onclick="closemsgboxErr();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." Style="" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server"
        TargetControlID="upl">
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
