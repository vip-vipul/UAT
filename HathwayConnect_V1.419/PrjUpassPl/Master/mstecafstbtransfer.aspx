<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="mstecafstbtransfer.aspx.cs" Inherits="PrjUpassPl.Master.mstecafstbtransfer" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function goBack() {
            window.location.href = "../Reports/EcafPages.aspx";
            return false;
        }
        function closemsgbox() {
            $find("mpeMsgBox").hide();
        }

        function closemsgboxErr() {
            $find("mpeMsgBoxErr").hide();

        }
    </script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <div class="maindive">
        <asp:Panel runat="server" ID="pnlRegisterLCO">
            <div style="float: right">
                <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                    Back</button>
            </div>
            <asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
            <div id="div1" class="delInfo1" runat="server">
                <table runat="server" align="center" width="700px" id="Table2" border="0">
                    <tr>
                        <td align="left">
                            LCO :
                            <asp:Label ID="lbllcocode" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="left">
                            LCO Balance:
                            <asp:Label ID="lbllcobal" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            STB Rate:
                            <asp:Label ID="lblstbrate" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divtextbox" class="delInfo1" runat="server">
                <table runat="server" align="center" width="700px" id="tbl1" border="0">
                    <tr>
                        <td align="center">
                            <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                <asp:ListItem Value="S" Selected="True">Single Transfer</asp:ListItem>
                                <asp:ListItem Value="B">Bulk Transfer</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <br />
                <table runat="server" align="center" width="700px" id="Table1" border="0">
                    <tr runat="server" id="trsingle">
                        <td align="right">
                            <asp:Label ID="Label1" runat="server" Text="Enter STB No"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtstbno" runat="server" Width="50%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="right" runat="server" id="trbulk" visible="false">
                        <td align="right">
                            <asp:Label ID="Label3" runat="server" Text="Upload STB Details"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:FileUpload ID="FileUpload1" Height="21px" Width="250px" runat="server" />
                                    <a href="../GeneratedFiles/DemoExcel/BulkSTBTransfer.xlsx" id="bulktransaction" runat="server">
                                        Download Template</a>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btncnfmBlck" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                        </td>
                    </tr>
                </table>
                <br />
                <table runat="server" align="center" width="700px" id="Table3" border="0">
                    <tr>
                        <td colspan="4" align="center">
                            <input id="BtnUpdate" type="button" value="Search" class="button" onclick="ShowPopup();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Label ID="lblerrormsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="popMsgBox" runat="server" BehaviorID="mpeMsgBox" TargetControlID="hdnMsgBox"
            PopupControlID="pnlMsgBox">
        </cc1:ModalPopupExtender>
        <asp:HiddenField ID="hdnMsgBox" runat="server" />
        <asp:Panel ID="pnlMsgBox" runat="server" CssClass="Popup" Style="width: 450px; height: 160px;
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
                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            <br />
                            <%--   <asp:Label ID="lblconfirm" runat="server" Width="250px"></asp:Label>--%>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="btncnfmBlck" runat="server" Text="Confirm" OnClick="btncnfmBlck_Click"
                                class="button" />
                            &nbsp;&nbsp;
                            <input id="Button3" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                onclick="closemsgbox();" />
                        </td>
                    </tr>
                </table>
            </center>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="PopMsgBoxErr" runat="server" BehaviorID="mpeMsgBoxErr"
            TargetControlID="hdnMsgBoxErr" PopupControlID="PnlPopErr">
        </cc1:ModalPopupExtender>
        <asp:HiddenField ID="hdnMsgBoxErr" runat="server" />
        <asp:Panel ID="PnlPopErr" runat="server" CssClass="Popup" Style="width: 330px; height: 160px;
            display: none">
            <%-- display: none; --%>
            <center>
                <br />
                <table width="100%">
                    <tr>
                        <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                            &nbsp;&nbsp;&nbsp;Information
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
                            <asp:Label ID="lblInformation" runat="server" Text=""></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="BtnOkInfo" runat="server" Text="OK" OnClick="btnOK_Click" class="button" />
                            <%--  <input id="BtnCloseInfo" class="button" runat="server" type="button" value="OK" style="width: 100px;"
                                onclick="closemsgboxErr();" />--%>
                        </td>
                    </tr>
                </table>
            </center>
        </asp:Panel>
        <script type="text/javascript">
            function ShowPopup() {

                var value = $('#<%=rblType.ClientID %> input:checked ').val();
                var stbrate = document.getElementById('<%=lblstbrate.ClientID%>').innerText;

                $("#<%=lblMsg.ClientID %>").text("you want to Add STB Details With STB Rate : " + stbrate);


                $find("mpeMsgBox").show();
                return false;
            }
        </script>
    </div>
</asp:Content>
