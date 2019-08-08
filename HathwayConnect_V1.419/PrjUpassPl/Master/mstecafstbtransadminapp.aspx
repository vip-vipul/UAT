<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="mstecafstbtransadminapp.aspx.cs" Inherits="PrjUpassPl.Master.mstecafstbtransadminapp" %>
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
    <script type="text/javascript">
        function closemsgbox() {

            $find("popout").hide();

        }

        $(document).ready(function () {
            //     $("#popout").animate({ 'center': 0 }, "slow");

            $("#popout").animate({ 'right': 300 }, 'slow');

        });
        function goBack() {
            window.location.href = "../Reports/EcafPages.aspx";
            return false;
        }
        function closemsgboxErr() {

            $find("mpeMsgBoxErr").hide();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <div id="popout" style="width: 630px; height: 250px; overflow: auto" align="right">
        <asp:Panel ID="pnllcoconformation" runat="server">
            <center>
                <br />
                <table width="100%" align="right">
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
                            <asp:GridView runat="server" ID="grdSTBSwap" CssClass="Grid" AutoGenerateColumns="false"
                                ShowFooter="true">
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
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" HeaderText="STB Status"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdntransid" runat="server" Value='<%# Eval("transid").ToString()%>' />
                                            <asp:RadioButton ID="RdoAccept" runat="server" Text="Accept" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="RdoCancel" runat="server" Text="Reject" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="Numeric" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            Remark:
                            <asp:TextBox ID="txtremark" runat="server" TextMode="MultiLine" Height="40px" Width="250px">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="90%" align="center">
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="btncnfmBlck" runat="server" Text="Confirm" class="button" Style="width: 100px;"
                                OnClick="btncnfmBlck_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="Btnclose" runat="server" OnClick="btnClose_Click" Text="Close" class="button" />
                    </tr>
                </table>
            </center>
        </asp:Panel>
    </div>
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
                        <asp:Label ID="lblinfo" runat="server"></asp:Label>
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
                     <asp:Button ID="ButBtnCloseInfoton1" runat="server" OnClick="btnCloseInfo_Click" Text="Close" class="button" />
                      <%--  <input id="BtnCloseInfo" class="button" runat="server" type="button" value="OK" style="width: 100px;"
                            onclick="closemsgboxErr();" />--%>
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
</asp:Content>
