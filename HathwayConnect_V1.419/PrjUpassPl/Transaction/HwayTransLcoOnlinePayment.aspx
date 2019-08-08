<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="HwayTransLcoOnlinePayment.aspx.cs" Inherits="PrjUpassPl.Transaction.HwayTransLcoOnlinePayment" %>
  <%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Distributor Balance</title>
    <link href="../CSS/main.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        .ImageBilldesk
        {
            background-image: url('../Images/billdesk1.jpg');
            background-repeat: no-repeat;
            height: 80px;
            width: 120px;
        }
        
        .ImageCitrus
        {
            background-image: url('../Images/Citrus_Payment_Gateway.png');
            background-repeat: no-repeat;
            height: 80px;
            width: 120px;
        }
    </style>
    <style type="text/css">
        .topHead
        {
            background: #E5E5E5;
        }
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            margin: 10px;
            width: 700px;
        }
        .delInfoContent
        {
            width: 95%;
        }
        .scroller
        {
            overflow: auto;
            max-height: 250px;
        }
        .style67
        {
            width: 145px;
        }
        .style68
        {
            width: 104px;
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
    <script type="text/javascript" language="javascript">
     function back()
    {
    
        window.location.href="../Transaction/TransBalanceManagementPages.aspx";
    }
        function hideButton() {
            $("#<%= btnSubmit.ClientID %>").hide();
        }
      
        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
            $('#<%= txtCashAmt.ClientID %>').keyup(function () {
                inWords($('#<%= txtCashAmt.ClientID %>').val());
            });
        }    
    </script>
  
    <asp:Panel ID="pnlView" runat="server" ScrollBars="Auto" >
      <div class="maindive">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblmsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
            </ContentTemplate>           
        </asp:UpdatePanel>
        
  <div>
             <div class="delInfo">
                <asp:Panel ID="pnllco" runat="server">
                    <table width="100%">
                        <tr>
                            <td colspan="3" align="left">
                                <b>
                                    <asp:Label runat="server" ID="Label2" Text="LCO : "></asp:Label>
                                </b>
                            </td>
                            <td>
                            <asp:DropDownList
                                            ID="ddllco" runat="server" OnSelectedIndexChanged="ddllco_SelectedIndexChanged"
                                            AutoPostBack="true" Width="300">
                                        </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                </div>
            <div id="divdet" runat="server">
                <div class="delInfo" id="Lcodet">
                    <cc1:Accordion ID="LCOAccordion" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                        FadeTransitions="true" SuppressHeaderPostbacks="true" TransitionDuration="250"
                        FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None">
                        <Panes>
                            <cc1:AccordionPane ID="LCOAccordionPane" runat="server">
                                <Header>
                                    <a href="#" class="href">LCO Details</a></Header>
                                <Content>
                                    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>--%>
                                    <table width="100%">
                                        <tr>
                                            <td colspan="3" align="left">
                                                <b>
                                                    <asp:Label runat="server" ID="Label4" Text="LCO Details:"></asp:Label>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="80px">
                                                <asp:Label runat="server" ID="Label21" Text="LCO Code"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label11" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustNo"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="80px">
                                                <asp:Label runat="server" ID="Label12" Text="Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustName"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="80px">
                                                <asp:Label runat="server" ID="Label5" Text="Address"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label6" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustAddr"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="80px">
                                                <asp:Label runat="server" ID="Label15" Text="Mobile No."></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label16" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblmobno"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="80px">
                                                <asp:Label runat="server" ID="Label18" Text="Email"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label19" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblEmail"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="80px">
                                                <asp:Label runat="server" ID="Label10" Text="Current Balance"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label17" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCurrBalance"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--</ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>--%>
                                </Content>
                            </cc1:AccordionPane>
                        </Panes>
                    </cc1:Accordion>
                </div>
                <div class="delInfo" id="Paydet" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="2">
                                <tr>
                                    <td colspan="6" align="left">
                                        <b>
                                            <asp:Label runat="server" ID="Label14" Text="Payment Details:"></asp:Label>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td width="120px" align="left">
                                        <asp:Label ID="lblCashAmt" runat="server" Text="Amount"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label20" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label1" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCashAmt" runat="server" Style="resize: none;" Width="200" MaxLength="9"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtCashAmt"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    
                                 
                                </tr>
                              <%--  <tr>
                                    <td width="120px" align="left">
                                        <asp:Label ID="Label23" runat="server" Text="Amount In Words"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label25" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="4">
                                        <asp:Label ID="lblamtinword" runat="server"></asp:Label>
                                    </td>
                                </tr>--%>
                            </table>
                          
                            
                            <table width="100%">
                                <tr id="Tr1" runat="server">
                                    <td align="left" width="120px">
                                        <asp:Label ID="lblReferenceNo" runat="server" Text="Remark"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="4">
                                        <asp:TextBox ID="txtRemark" runat="server" Style="resize: none;" Width="200" TextMode="MultiLine"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom"
                                            TargetControlID="txtRemark" ValidChars=" -_">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 <tr id="Tr2" runat="server">
                                        <td align="left">
                                            <asp:Label ID="Label3" runat="server" Text="Gateway"></asp:Label>
                                        </td>
                                        <td width="5px">
                                            <asp:Label ID="Label8" runat="server" Text=":"></asp:Label>
                                        </td>
                                        <td align="left" width="70px">
                                            <asp:RadioButton ID="rdoBill" Text="BillDesk" runat="server" GroupName="A" Checked="false"/>
                                        </td>
                                        <td align="left" width="88px">
                                            <asp:Image ID="Image1" runat="server" Width="57px" ImageUrl="~/Images/billdesk1.jpg"
                                                Height="35px" />
                                        </td>
                                        <td align="left"  width="65px">
                                            <asp:RadioButton ID="rdcitrus" Text="PAYU" runat="server" GroupName="A" Checked="false"/>
                                        </td>
                                        <td align="left">
                                            <asp:Image ID="Image2" runat="server" Width="60px" ImageUrl="~/Images/PayU.png"
                                                Height="35px" />
                                        </td>
                                    </tr>
                            </table>
                        </ContentTemplate>
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </div>
                <div class="delInfo">
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSubmit" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <%--<asp:Button ID="btnCancel" TabIndex="2" runat="server" Font-Bold="True" Text="Cancel"
                                            class="button" Width="60" OnClick="btnCancel_Click1"></asp:Button>
                                        &nbsp;&nbsp;--%>
                                        <asp:Button ID="btnSubmit" TabIndex="2" runat="server" Font-Bold="True" Text="Submit"
                                            class="button" Width="60" OnClientClick="hideButton();" OnClick="btnSubmit_Click">
                                            
                                        </asp:Button>
                                         &nbsp;
                                        
                                          <asp:Button runat="server" ID="btnBck" Text="Back" CssClass="button" UseSubmitBehavior="false"
                                        OnClientClick="back();" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        </div>
        <asp:HiddenField ID="HdnMaxamount" runat="server" Value="1000000"/>
        <%-- ----------------------------------------------------Loader------------------------------------------------------------------ --%>
        <div id="imgrefresh" class="loader transparent">
            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                ToolTip="Loading ..." Style="" />
        </div>
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
        <cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender2" runat="server"
            TargetControlID="UpdatePanel2">
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
    </asp:Panel>
</asp:Content>
