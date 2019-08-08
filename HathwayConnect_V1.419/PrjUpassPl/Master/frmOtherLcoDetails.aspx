<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmOtherLcoDetails.aspx.cs" Inherits="PrjUpassPl.Master.frmOtherLcoDetails" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

 function back()
    {
    
        window.location.href="../Master/mstLCOAdminPages.aspx";
        return false;
    }
</script>
<style type="text/css">
        .delInfo
        {
            /*padding: 10px;
            border: 1px solid #094791;*/
            width: 95%;
            margin: 5px;
            padding: 10px;
            border: 1px solid #094791;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
<asp:Panel runat="server" ID="pnlRegisterLCO">
 <div class="maindive">
 <div style="float:right">
                <button onclick="return back();"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
        <asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
        <table width="800px">
            <tr>
                <td align="center">
                    <div class="delInfo" id="divsearchLco" runat="server">
                        <table runat="server" align="center" width="500px" id="tbl1" border="0">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblUser" runat="server" Text="Search LCO By"></asp:Label>
                                    <asp:Label ID="Label59" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="RadSearchby" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"><%--OnSelectedIndexChanged="RadSearchby_SelectedIndexChanged1"--%>
                                        <asp:ListItem Value="0" Selected="True">CODE</asp:ListItem>
                                        <asp:ListItem Value="1">Name</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtLCOSearch" runat="server" onkeydown = "SetContextKey()" Style="resize: none;" Width="150px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ServiceMethod="SearchOperators" MinimumPrefixLength="1"
                                        UseContextKey="true" CompletionInterval="100" EnableCaching="true" CompletionSetCount="3"
                                        TargetControlID="txtLCOSearch" FirstRowSelected="false" ID="AutoCompleteExtender1"
                                        runat="server" CompletionListCssClass="autocomplete" CompletionListItemCssClass="autocompleteItem"
                                        CompletionListHighlightedItemCssClass="autocompleteItemHover">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="hfLCOCode" runat="server" />
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%--<div id="divdet" runat="server">--%>
                    <asp:Panel runat="server" ID="pnlDetails">
                        <div class="delInfo">
                            <table width="100%">
                                <tr>
                                    <td colspan="6" align="left">
                                        <b>
                                            <asp:Label runat="server" ID="Label7" Text="LCO Details:"></asp:Label>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label8" Text="LCO Code"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label9" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblLCOCode" Text=""></asp:Label>
                                    </td>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label10" Text="LCO Name"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label14" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblLCOName" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label15" Text="Address"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label16" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" colspan="5">
                                        <asp:Label runat="server" ID="lblAddress" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label17" Text="JV"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label18" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblJV" Text=""></asp:Label>
                                    </td>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label19" Text="Direct"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label20" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblDirect" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label22" Text="Distributor"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label23" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblDistributor" Text=""></asp:Label>
                                    </td>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label24" Text="Sub Distributor"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label25" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblSubDistributor" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label26" Text="State"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label27" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblState" Text=""></asp:Label>
                                    </td>
                                    <td align="left" width="80px">
                                        <asp:Label runat="server" ID="Label28" Text="City"></asp:Label>
                                    </td>
                                    <td width="10px">
                                        <asp:Label runat="server" ID="Label29" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="lblCity" Text=""></asp:Label>
                                    </td>
                                </tr>
                               <%-- <tr >
                        <td align="left" class="style67">
                            <asp:Label ID="Label38" Text="Auto Renew" runat="server"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                           
                            <asp:CheckBox ID="chkecsstatus" runat="server" />
                           
                        </td>
                        <td align="left">
                            
                        </td>
                        <td>
                            
                        </td>
                        <td align="left">
                           
                          
                        </td>
                    </tr>--%>
                            </table>
                        </div>
                        <div class="delInfo">
                            <table width="100%">
                               
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label runat="server" ID="Label21" Text="Area Manager" Width="100px"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label runat="server" ID="Label11" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtAreaMang" runat="server" Width="125px" MaxLength="25"></asp:TextBox></td>
                                    <td align="right">
                                        <asp:Label runat="server" ID="Label12" Text="P&T License Expiry Date"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtptexdt" runat="server" Width="100px"></asp:TextBox>
                                        <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtptexdt" PopupButtonID="imgFrom"
                                            Format="dd-MMM-yyyy">
                                        </cc1:CalendarExtender>
                                     </td>
                                </tr>
                                <tr>
                                    <td align="right" >
                                        <asp:Label runat="server" ID="Label1" Text="Interconnect Agreement Expiry Date"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label runat="server" ID="Label2" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtintagreexdt" runat="server" Width="100px"></asp:TextBox>
                                        <asp:Image runat="server" ID="imgto" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <cc1:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="txtintagreexdt" PopupButtonID="imgto"
                                            Format="dd-MMM-yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                         <td align="right">
                                             <asp:Label ID="Label4" runat="server" Text="Executive" MaxLength="25"></asp:Label> </td>
                                             <td>
                                                 <asp:Label ID="Label6" runat="server" Text=":"></asp:Label>
                                             </td>
                                   
                                    <td align="left">
                                        <asp:TextBox ID="txtexecutive" runat="server" Width="125px"></asp:TextBox>
                                    </td>
                                   
                                </tr>
                            </table>
                        </div>                        
                        <div class="delInfo">
                            <asp:Button ID="btnCancel" CssClass="button" runat="server" Text="Reset" 
                                UseSubmitBehavior="false" onclick="btnCancel_Click"
                                 />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" UseSubmitBehavior="true"
                                 ValidationGroup="lco" onclick="btnSubmit_Click" />
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="lco"
                                ShowMessageBox="true" ShowSummary="false" DisplayMode="BulletList" />
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
 </div>
    </asp:Panel>
</asp:Content>
