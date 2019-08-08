<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="HwayTransLCOSTBInventoryConformation.aspx.cs" Inherits="PrjUpassPl.Transaction.HwayTransLCOSTBInventoryConformation" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function closePopup() {
            $find("mpeConfirmation").hide();
            return false;
        }
        function closeMsgPopup() {
            $find("mpeMsg").hide();
            return false;
        }
        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
        }
    </script>
    <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            margin: 10px;
        }
        .delInfoContent
        {
            width: 95%;
        }
        .style67
        {
            width: 123px;
        }
    </style>
    <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 780px;
            margin: 5px;
            background-color: White;
            margin-top: 8px;
            height: auto;
        }
        .delInfo1
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 780px;
            margin: 5px;
            background-color: White;
            margin-top: 8px;
            height: 165px;
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
        .style68
        {
            width: 184px;
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
     function closeGridPopup() {
         $find("mpeGridBox").hide();
         return false;
     }

     }
     
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="maindive">
                <asp:Panel ID="pnlView" runat="server" ScrollBars="Auto" Style="height: 320px">
                    <asp:Label ID="lblmsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                    <div id="div1" runat="server" visible="true">
                        <div class="delInfo">
                            <asp:GridView runat="server" ID="grdCashcollect" CssClass="Grid" AutoGenerateColumns="false"
                                AllowPaging="true" PageSize="100" Width="100%">
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Receipt No">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReceiptno" runat="server" Text='<%# Eval("receiptno") %>'
                                                OnClick="lnkReceiptno_click"></asp:LinkButton>
                                            <asp:HiddenField ID="hdnreceiptno" runat="server" Value='<%# Eval("receiptno").ToString()%>' />
                                            <asp:HiddenField ID="hdnsubtype" runat="server" Value='<%# Eval("subtypex").ToString()%>' />
                                            <asp:HiddenField ID="hdntype" runat="server" Value='<%# Eval("transtype").ToString()%>' />
                                            <asp:HiddenField ID="hdnmodel" runat="server" Value='<%# Eval("SKYWORTH").ToString()%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Pay Mode" DataField="paymentmode" HeaderStyle-HorizontalAlign="Center"
                                        Visible="false" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Amount" DataField="amount" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Discount" DataField="discount" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Transaction Type" DataField="xtype" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Transaction Sub Type" DataField="TRANSSUBTYPE1" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Scheme" DataField="SCHEME" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Box Type" DataField="BOXTYPE" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true" ItemStyle-HorizontalAlign="Left" />
                                </Columns>
                                <PagerSettings Mode="Numeric" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div id="div2" runat="server" visible="false">
                        <div class="delInfo">
                            <table width="90%">
                                <tr>
                                    <td colspan="4">
                                        Receipt No. :
                                        <asp:Label ID="lblReceiptNo" runat="server" Text=""></asp:Label>
                                       
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                        <asp:LinkButton ID="lnkdownload" runat="server" OnClick="lnkdownload_click" >Download</asp:LinkButton>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:FileUpload ID="FileUpload1" class="fileInput" runat="server" Width="150px" />
                                                    <asp:LinkButton ID="lnkbulk" runat="server" Font-Bold="true" OnClick="lnkbulk_click"> Bulk Upload </asp:LinkButton>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="lnkbulk" />
                                                    <asp:PostBackTrigger ControlID="lnkdownload" />
                                                </Triggers>
                                            </asp:UpdatePanel>

                                        <asp:GridView ID="grdSTBDetails" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                                            Width="100%" OnRowDataBound="grdSTBDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="STB/VC" DataField="STB" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Type" DataField="transtype" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Scheme" DataField="Scheme" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="Left" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px" HeaderText="STB Status"
                                                    HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                     
                                                        <asp:CheckBox ID="ChkGoodSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged_ChkGoodSelectAll"
                                                            Text="Select All" />
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:CheckBox ID="ChkFaultySelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged_ChkFaultySelectAll"
                                                            Text="Select All" />
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:CheckBox ID="ChkUndeliveredSelectAll" Visible="false" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged_UndeliveredSelectAll"
                                                            Text="Select All" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="RdoGood" runat="server" Text="Good" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:RadioButton ID="RdoFaulty" runat="server" Text="Faulty" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:RadioButton ID="RdoUndelivered"  Visible="false" runat="server" Text="Undelivered" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="stbid" DataField="stbid" />
                                                <asp:BoundField HeaderText="warehouse" DataField="warehouse" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left" valign="middle">
                                        <asp:Label runat="server" ID="Label7" Text="Remark : "></asp:Label>
                                        <asp:Label ID="Label43" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                        <asp:TextBox ID="txtRemark" Style="resize: none;" TextMode="MultiLine" Width="250px"
                                            Height="30px" runat="server"></asp:TextBox>
                                        <br />
                                        <br />
                                    </td>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnSubmit" TabIndex="2" runat="server" Font-Bold="True" Text="Submit"
                                                class="button" Width="60" OnClick="btnSubmit_Click"></asp:Button>
                                        </td>
                                    </tr>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="popMsg" runat="server" BehaviorID="mpeMsg" TargetControlID="hdnPop2"
        PopupControlID="pnlMessage" CancelControlID="imgClose2">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnPop2" runat="server" />
    <asp:Panel ID="pnlMessage" runat="server" CssClass="Popup" Style="width: 430px; height: 160px;
        display: none;">
        <asp:Image ID="imgClose2" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closeMsgPopup();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Message
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
                        <asp:Label ID="lblPopupResponse" runat="server" Text="Do You want to continue?"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnconfirm" runat="server" OnClick="btnconfirm_click" Width="70px"
                            Text="Confirm" />
                        <asp:Button ID="btnCloseMsg" runat="server" OnClientClick="closeMsgPopup();" Width="70px"
                            Text="Close" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
</asp:Content>
