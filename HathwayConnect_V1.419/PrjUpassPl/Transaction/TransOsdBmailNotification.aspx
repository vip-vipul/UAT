<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransOsdBmailNotification.aspx.cs" Inherits="PrjUpassPl.Transaction.TransOsdBmailNotification" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .topHead
        {
            background: #E5E5E5;
            width: 75%;
        }
        .topHead table td
        {
            font-size: 12px;
            
            font-weight: bold;
        }
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
        }
        .delInfoContent
        {
            width: 100%;
        }
        .scroller
        {
            overflow: auto;
            max-height: 250px;
        }
        .plan_scroller
        {
            overflow: auto;
            max-height: 170px;
        }
        .gridHolder
        {
            width: 75%;
        }
        .stbHolder
        {
            height: 150px;
            overflow-y: auto; /*width: 25%;*/
        }
        .custDetailsHolder
        {
            overflow-y: auto; /*width: 85%;*/
        }
        .popBack
        {
            background: white; /* IE 8 */
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=50)"; /* IE 5-7 */
            filter: alpha(opacity=50); /* Netscape */
            -moz-opacity: 0.5; /* Safari 1.x */
            -khtml-opacity: 0.5; /* Good browsers */
            opacity: 0.5;
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

        function back() {
            window.location = "../Transaction/TransNotificationPages.aspx";
        }

        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
        }
        function radAction() {
            var RB1 = document.getElementById("<%=RadIncExc.ClientID%>");

        }

        function getQueryStrings() {
            //Holds key:value pairs
            var queryStringColl = null;

            //Get querystring from url
            var requestUrl = window.location.search.toString();

            if (requestUrl != '') {
                //window.location.search returns the part of the URL 
                //that follows the ? symbol, including the ? symbol
                requestUrl = requestUrl.substring(1);

                queryStringColl = new Array();

                //Get key:value pairs from querystring
                var kvPairs = requestUrl.split('&');
                var kvPairs = kvPairs.split('Flag=');

                return kvPairs;

            }



        }

        var queryStringColl = getQueryStrings();

        if (queryStringColl == null) {
            //OSD oR bMAIL Excel

            document.getElementById("<%=excelSMS.ClientID%>").style.visibility = 'hidden';
            document.getElementById("<%=excelOSD.ClientID%>").style.visibility = 'visible';

        }
        else {
            if (queryStringColl == 'SMS') {
                //OSD oR bMAIL Excel

                document.getElementById("<%=excelSMS.ClientID%>").style.visibility = 'visible';
                document.getElementById("<%=excelOSD.ClientID%>").style.visibility = 'hidden';

            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <div class="maindive">
        <asp:UpdatePanel ID="upl" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                <table cellspacing="0" cellpadding="0" align="center" border="0" width="80%">
                    <tr>
                        <td align="center">
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="right">
                                        <button onclick="back()"  class="button">
                                            Back</button>
                                    </td>
                                </tr>
                                <tr>
                                <td  align="center">
                                <label>&nbsp;</label>
                                 <div class="delInfo custDetailsHolder" runat="server" id="div2">
                                            <center>
                                                <table>
                                                    <tr id="Tr4">
                                                        <td style="padding-left: 35px">
                                                        </td>
                                                        <td align="left" class="style70">
                                                            <asp:Label ID="lblAction" runat="server" Text="Action "></asp:Label>
                                                        </td>
                                                        <td>
                                                            :
                                                        </td>
                                                        <td align="left">
                                                            <asp:RadioButtonList ID="RadIncExc" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                                                OnSelectedIndexChanged="RadIncExc_SelectedIndexChanged">
                                                                <asp:ListItem Value="I" Text="One By One"></asp:ListItem>
                                                                <asp:ListItem Value="A" Text="All"></asp:ListItem>
                                                                <asp:ListItem Value="B"> Bulk Upload </asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td>
                                                            <asp:Button runat="server" ID="btnReset" Width="60" UseSubmitBehavior="false" Visible="false"
                                                                Text="Reset" OnClick="btnReset_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Label runat="server" ID="Label9" ForeColor="Red"></asp:Label>
                                                <table runat="server" id="Table1" visible="false">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label runat="server" ID="Label10" Text="A/C No.:"></asp:Label>
                                                            &nbsp;<asp:Label runat="server" ID="Label11" Text=""></asp:Label>
                                                            &nbsp;&nbsp;
                                                            <asp:Label runat="server" ID="Label13" Text="Customer Name : "></asp:Label><asp:Label
                                                                runat="server" ID="Label14" Text=""></asp:Label>
                                                            &nbsp;&nbsp;
                                                            <div class="griddiv">
                                                                <asp:GridView ID="GridView1" CssClass="Grid" runat="server" AutoGenerateColumns="false">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkStb" AutoPostBack="true"></asp:CheckBox>
                                                                                <asp:HiddenField runat="server" ID="hdnServiceStr" Value='<%# Eval("SERVICE_STRING").ToString()%>' />
                                                                                <asp:HiddenField runat="server" ID="hdnStbVCID" Value='<%# Eval("VC_ID").ToString()%>' />
                                                                                <asp:HiddenField runat="server" ID="hdnStbNo" Value='<%# Eval("STB_NO").ToString()%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="VC/Mac ID" DataField="VC_ID" ItemStyle-HorizontalAlign="Left" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Button runat="server" ID="Button2" Width="60" Visible="false" Text="Add" ValidationGroup="searchValidation"
                                                    OnClick="btnAdd_Click" />
                                            </center>
                                        </div>
                                         <label>&nbsp;</label>
                                </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <div class="delInfo">
                                            <table cellpadding="2" width="96%">
                                                <tr id="TrFromDate" runat="server">
                                                    <td style="padding-left: 35px">
                                                    </td>
                                                    <td align="left" class="style70">
                                                        <asp:Label ID="Label7" runat="server" Text="From Date"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label8" runat="server" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox runat="server" ID="txtFrom" Enabled="false" BorderWidth="1" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                        <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                                            Format="dd-MMM-yyyy">
                                                        </cc1:CalendarExtender>
                                                         <asp:Label ID="lblFromTime" Text="Time: " runat="server"></asp:Label>
                                                         <asp:DropDownList ID="ddlFromHour"  runat="server" Height="19px" Width="40px">
                                                            <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                            <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                         <asp:DropDownList ID="ddlfromMinute"  runat="server" Height="19px" Width="40px">
                                                            <asp:ListItem Text="00" Value="00"></asp:ListItem>
                                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                            <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                            <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                            <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                        </asp:DropDownList>
                                                         <asp:DropDownList ID="ddlFromAmPm"  runat="server" Height="19px" Width="45px">
                                                            <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
                                                            <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TrToDate" runat="server">
                                                    <td style="padding-left: 35px">
                                                    </td>
                                                    <td align="left" class="style70">
                                                        <asp:Label ID="Label15" runat="server" Text="To Date"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label16" runat="server" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox runat="server" Enabled="false" ID="txtTo" BorderWidth="1" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                        <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                                            Format="dd-MMM-yyyy">
                                                        </cc1:CalendarExtender>

                                                         <asp:Label ID="lblToTime" Text="Time: " runat="server"></asp:Label>
                                                         <asp:DropDownList ID="ddlToHour"  runat="server" Height="19px" Width="40px">
                                                            <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                            <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                         <asp:DropDownList ID="ddlToMinue"  runat="server" Height="19px" Width="40px">
                                                            <asp:ListItem Text="00" Value="00"></asp:ListItem>
                                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                            <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                            <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                            <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                        </asp:DropDownList>
                                                         <asp:DropDownList ID="ddlToAmPm"  runat="server" Height="19px" Width="45px">
                                                            <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
                                                            <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TrNotification" runat="server">
                                                    <td style="padding-left: 35px">
                                                    </td>
                                                    <td align="left" class="style70">
                                                        <asp:Label ID="lblNitification" runat="server" Text="Notification"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label17" runat="server" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlNotification" AutoPostBack="true" runat="server" Height="19px"
                                                            Style="resize: none;" Width="304px" OnSelectedIndexChanged="ddlNotification_SelectedIndexChanged">
                                                            <asp:ListItem Text="Select Notification" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="BMail" Value="BMail"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="Trlco" runat="server" visible="false">
                                                    <td style="padding-left: 35px">
                                                    </td>
                                                    <td align="left" class="style70">
                                                        <asp:Label ID="Label1" runat="server" Text="LCO Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text=":"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlLco" AutoPostBack="true" runat="server" Height="19px" Style="resize: none;"
                                                            Width="304px" OnSelectedIndexChanged="ddlLco_SelectedIndexChanged">
                                                            <asp:ListItem Text="Select LCO" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Abc" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="DEF" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TrTemplate" runat="server" visible="true">
                                                    <td style="padding-left: 35px">
                                                    </td>
                                                    <td align="left" class="style70">
                                                        <asp:Label ID="lblMessage" runat="server" Text="Template"></asp:Label>
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="dllTemplate" AutoPostBack="true" OnSelectedIndexChanged="dllTemplate_SelectedIndexChanged"
                                                            runat="server" Height="19px" Style="resize: none;" Width="304px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TrFrequency" runat="server">
                                                    <td style="padding-left: 35px">
                                                    </td>
                                                    <td align="left" class="style70">
                                                        <asp:Label ID="Label2" runat="server" Text="Frequency"></asp:Label>
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlFrequency" runat="server" Height="19px" Style="resize: none;"
                                                            Width="304px">
                                                            <asp:ListItem Text="Select Frequency" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Once" Value="Once"></asp:ListItem>
                                                            <asp:ListItem Text="Hour" Value="Hour"></asp:ListItem>
                                                            <asp:ListItem Text="Day" Value="Day"></asp:ListItem>
                                                            <asp:ListItem Text="Week" Value="Week"></asp:ListItem>
                                                            <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="Tr5" runat="server" visible="false">
                                                    <td style="padding-left: 35px">
                                                    </td>
                                                    <td align="left" class="style70">
                                                        <asp:Label ID="Label3" runat="server" Text="Duration"></asp:Label>
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlDuration" runat="server" Height="19px" Style="resize: none;"
                                                            Width="304px">
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <%--<asp:ListItem Text="Select Duration (In Second)" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                            <asp:ListItem Text="60" Value="60"></asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="Tr6" runat="server" visible="false">
                                                    <td style="padding-left: 35px">
                                                    </td>
                                                    <td align="left" class="style70">
                                                        <asp:Label ID="Label4" runat="server" Text="Position"></asp:Label>
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlPosition" runat="server" Height="19px" Style="resize: none;"
                                                            Width="304px">
                                                            <%-- <asp:ListItem Text="Select Position" Value="0"></asp:ListItem>--%>
                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                            <%-- <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TrFullmsg" runat="server" visible="false">
                                                    <td style="padding-left: 35px">
                                                    </td>
                                                    <td align="left" class="style70">
                                                        <asp:Label ID="Label6" runat="server" Text="Message Format"></asp:Label>
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblFullMsg" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" id="divCustHolder" runat="server">
                                       
                                        <label>
                                            &nbsp;</label>
                                        <div id="divSearchHolder" runat="server" class="delInfo custDetailsHolder" visible="false">
                                            <center>
                                                <table>
                                                    <tr align="center">
                                                        <td>
                                                            <asp:Label runat="server" ID="Label20" Text="Search By"></asp:Label>
                                                            <asp:Label ID="Label59" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoSearchParamType" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Selected="True" Value="0">VC/Mac ID</asp:ListItem>
                                                                <asp:ListItem Value="1">Account No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" Width="90px" ID="txtSearchParam" MaxLength="30"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button runat="server" ID="btnSearch" Text="Search" ValidationGroup="searchValidation"
                                                                UseSubmitBehavior="false" OnClick="btnSearch_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Label runat="server" ID="lblSearcherror" ForeColor="Red"></asp:Label>
                                                <table runat="server" id="tblCustDetails" visible="false">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label runat="server" ID="Label21" Text="A/C No.:"></asp:Label>
                                                            &nbsp;<asp:Label runat="server" ID="lblAcno" Text=""></asp:Label>
                                                            &nbsp;&nbsp;
                                                            <asp:Label runat="server" ID="Label12" Text="Customer Name : "></asp:Label><asp:Label
                                                                runat="server" ID="lblCustName" Text=""></asp:Label>
                                                            <asp:GridView ID="grdStb" CssClass="Grid" runat="server" AutoGenerateColumns="false">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" ID="chkStb" AutoPostBack="true"></asp:CheckBox>
                                                                            <asp:HiddenField runat="server" ID="hdnServiceStr" Value='<%# Eval("SERVICE_STRING").ToString()%>' />
                                                                            <asp:HiddenField runat="server" ID="hdnStbVCID" Value='<%# Eval("VC_ID").ToString()%>' />
                                                                            <asp:HiddenField runat="server" ID="hdnStbNo" Value='<%# Eval("STB_NO").ToString()%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="VC/Mac ID" DataField="VC_ID" ItemStyle-HorizontalAlign="Left" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Button runat="server" ID="btnAdd" Width="60" Visible="false" Text="Add" UseSubmitBehavior="false"
                                                    OnClick="btnAdd_Click" />
                                            </center>
                                        </div>
                                        <label>
                                            &nbsp;</label>
                                        <asp:Label runat="server" ID="lblSearchResponse" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <div class="delInfo" id="divAcountDetails" runat="server" visible="false">
                                            <table cellpadding="2" width="96%">
                                                <tr>
                                                    <td align="center" width="100%" colspan="2">
                                                        <div class="plan_scroller">
                                                            <asp:GridView ID="grdAcountDetails" OnRowDataBound="grdAcountDetails_RowDataBound"
                                                                CssClass="Grid" runat="server" RowStyle-Width="100" RowStyle-HorizontalAlign="Left">
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Button ID="btnCancel" TabIndex="2" runat="server" Font-Bold="True" Text="Cancel"
                                                                        class="button" Width="60" Height="20px" OnClick="btnCancel_Click"></asp:Button>
                                                                    &nbsp;&nbsp;
                                                                    <asp:Button ID="btnSubmit" TabIndex="2" runat="server" Font-Bold="True" Text="Submit"
                                                                        class="button" Width="60" Height="20px" OnClick="btnSubmit_Click"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                </tr>
                            </table>
                            </div>
                        </td>
                    </tr>
                </table>
                </td> </tr> </table>
                <div id="imgrefresh" class="loader transparent">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                        ToolTip="Loading ..." Style="" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnReset" />
                <asp:PostBackTrigger ControlID="RadIncExc" />
            </Triggers>
        </asp:UpdatePanel>
        <table cellspacing="0" cellpadding="0" align="center" border="0" width="80%">
            <tr>
                <td>
                    <div class="delInfo custDetailsHolder" runat="server" id="dvBulk" visible="false">
                        <center>
                            <%--a--%>
                            <table>
                                <tr align="center">
                                    <td align="left">
                                        <asp:FileUpload ID="fupData" runat="server" />
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"
                                            UseSubmitBehavior="false" />
                                        <asp:HyperLink ID="excelOSD" Text="Download Template" NavigateUrl="../GeneratedFiles/DemoExcel/osd_or_bmail.xlsx"
                                            runat="server"></asp:HyperLink>
                                        <asp:HyperLink ID="excelSMS" Text="Download Template" NavigateUrl="../GeneratedFiles/DemoExcel/sms.xlsx"
                                            runat="server"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <asp:Label runat="server" ID="lblStatusHeading" Text="" ForeColor="Red"></asp:Label>
                                        <asp:Label runat="server" ID="lblStatus" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </td>
            </tr>
        </table>
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
    </div>
</asp:Content>
