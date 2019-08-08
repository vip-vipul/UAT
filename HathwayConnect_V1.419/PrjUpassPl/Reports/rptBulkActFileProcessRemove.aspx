<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptBulkActFileProcessRemove.aspx.cs" Inherits="PrjUpassPl.Reports.rptBulkActFileProcessRemove" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ShowPopup() {

            // var myVar = setInterval(function () { ShowModalPopup() }, 30000);

            // show next slide now
            // set timer for the slide after this one
            setTimeout(function () {
                ShowModalPopup();       // repeat
            }, 30000);

        }

        function goBack() {
            window.location.href = "../Reports/rptBulkActFileProcess.aspx";
            return false;
        }
        function closemsgbox() {

            $find("mpeMsgBox").hide();

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:UpdatePanel runat="server" ID="GridBulkStstus">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlRegisterLCO">
                <div class="maindive">
                    <div style="float: right">
                        <button onclick="goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                            Back</button>
                    </div>
                    <asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
                    <div class="griddiv">
                        <div id="DivGried" runat="server">
                            <asp:GridView runat="server" ID="grdBulkstatus" CssClass="Grid" AutoGenerateColumns="false"
                                ShowFooter="true" AllowPaging="true" PageSize="10" OnRowCommand="grdBulkProc_RowCommand"   OnPageIndexChanging="grdBulkProc_PageIndexChanging" >
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Count" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblbulkid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"num_lcopre_bulk_id")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Unique ID" DataField="useruniqueid" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="customer account" DataField="custid" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Plan Name" DataField="planname" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Process Date" DataField="date1" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderText="Count" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text='Remove' CommandName="Remove"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="Numeric" />
                            </asp:GridView>
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
                                Are you sure to Remove ?
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
                                <input id="Button3" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closemsgbox();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
