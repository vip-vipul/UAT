<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mstecafcustomerdetails.aspx.cs"
    Inherits="PrjUpassPl.Master.mstecafcustomerdetails" MasterPageFile="~/MasterPage.Master" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.9.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function hideVCOnKeyPressMACSearch() {
            var txtMacIdLength = document.getElementById('<%=txtsearchMACID.ClientID %>');
            if (txtMacIdLength.value.length > 0) {
                document.getElementById('<%=txtSearchParam.ClientID%>').disabled = 'true';
                document.getElementById('<%=txtStbnosearch.ClientID%>').disabled = 'true';
            }
            else {
                document.getElementById('<%=txtSearchParam.ClientID%>').disabled = false;
                document.getElementById('<%=txtStbnosearch.ClientID%>').disabled = false;

            }
        }

        function hideMACOnKeyPressSTBSearch() {
            var txtVCIdLength = document.getElementById('<%=txtSearchParam.ClientID %>');
            var txtMacIdSTBLength = document.getElementById('<%=txtStbnosearch.ClientID %>');

            if (txtMacIdSTBLength.value.length > 0) {
                document.getElementById('<%=txtsearchMACID.ClientID%>').disabled = 'true';
            }
            else if (txtMacIdSTBLength.value.length == 0 && txtVCIdLength.value.length == 0) {

                document.getElementById('<%=txtsearchMACID.ClientID%>').disabled = false;
            }
            else {
                document.getElementById('<%=txtSearchParam.ClientID%>').disabled = false;
                document.getElementById('<%=txtStbnosearch.ClientID%>').disabled = false;
            }
        }

        function hideMACOnKeyPressVCSearch() {
            var txtVCIdLength = document.getElementById('<%=txtSearchParam.ClientID %>');
            var txtMacIdSTBLength = document.getElementById('<%=txtStbnosearch.ClientID %>');

            var radioButtons = document.getElementById("<%=RadSearchby.ClientID%>");

            var GetRadioval=($('#<%=RadSearchby.ClientID %> input[type=radio]:checked').val());
            if (GetRadioval == 1) {
                if (txtVCIdLength.value.length > 0) {
                    document.getElementById('<%=txtsearchMACID.ClientID%>').disabled = 'true';
                }
                else if (txtMacIdSTBLength.value.length == 0 && txtVCIdLength.value.length == 0) {
                   
                    document.getElementById('<%=txtsearchMACID.ClientID%>').disabled = false;
                }
                else {
                    document.getElementById('<%=txtSearchParam.ClientID%>').disabled = false;
                    document.getElementById('<%=txtStbnosearch.ClientID%>').disabled = false;

                }
            }
            else {
                document.getElementById('<%=txtSearchParam.ClientID%>').disabled = false;
                document.getElementById('<%=txtStbnosearch.ClientID%>').disabled = false;
            }
        }



        function hideVCOnKeyPressMAC() {
            var txtMacIdLength = document.getElementById('<%=txtMacId.ClientID %>');
            if (txtMacIdLength.value.length > 0) {
                document.getElementById('<%=txtpairVC.ClientID%>').disabled = 'true';
                document.getElementById('<%=txtpairstb.ClientID%>').disabled = 'true';
            }
            else {
                document.getElementById('<%=txtpairVC.ClientID%>').disabled = false;
                document.getElementById('<%=txtpairstb.ClientID%>').disabled = false;

            }
        }

        function hideMACOnKeyPressSTB() {
            var txtVCIdLength = document.getElementById('<%=txtpairVC.ClientID %>');
            var txtMacIdSTBLength = document.getElementById('<%=txtpairstb.ClientID %>');
        
            if (txtMacIdSTBLength.value.length > 0) {
                document.getElementById('<%=txtMacId.ClientID%>').disabled = 'true';
            }
            else if (txtMacIdSTBLength.value.length == 0 && txtVCIdLength.value.length == 0) {
           
                document.getElementById('<%=txtMacId.ClientID%>').disabled = false;
            }
            else {
                document.getElementById('<%=txtpairVC.ClientID%>').disabled = false;
                document.getElementById('<%=txtpairstb.ClientID%>').disabled = false;
            }
        }

        function hideMACOnKeyPressVC() {
            var txtVCIdLength = document.getElementById('<%=txtpairVC.ClientID %>');
            var txtMacIdSTBLength = document.getElementById('<%=txtpairstb.ClientID %>');
           
            if (txtVCIdLength.value.length > 0) {
                document.getElementById('<%=txtMacId.ClientID%>').disabled = 'true';
            }
            else if (txtMacIdSTBLength.value.length == 0 && txtVCIdLength.value.length == 0) {
                alert('a');
                document.getElementById('<%=txtMacId.ClientID%>').disabled = false;
            }
            else {
                document.getElementById('<%=txtpairVC.ClientID%>').disabled = false;
                document.getElementById('<%=txtpairstb.ClientID%>').disabled = false;

            }
        }
        function InIEvent() {
            $("html, body").scrollTop(100);

            $("#<%= fupidproof.ClientID %>").change(function () {
                __doPostBack('<%= lnkfupidproof.UniqueID %>', '');
            });

            $("#<%= fupresiproof.ClientID %>").change(function () {

                __doPostBack('<%= lnkfupresiproof.UniqueID %>', '');
            });

            $("#<%= fupphoto.ClientID %>").change(function () {

                __doPostBack('<%= lnkfupphoto.UniqueID %>', '');
            });

            $("#<%= fuploadpdf.ClientID %>").change(function () {

                __doPostBack('<%= lnkpdfupload.UniqueID %>', '');
            });





        }

        $(document).ready(InIEvent);

        function goBack() {
            window.history.back();
        }

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 46 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function Alfhabet(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 65 || charCode > 122)) {
                return false;
            }
            return true;
        }

        function checkNum() {

            if ((event.keyCode > 64 && event.keyCode < 91) || (event.keyCode > 96 && event.keyCode < 123) || event.keyCode == 8)

                return true;
            else {
                return false;
            }
        }
        function ValidateEmail(mail) {
            if (/^\w+([\.-]?\ w+)*@\w+([\.-]?\ w+)*(\.\w{2,3})+$/.test(myForm.emailAddr.value)) {
                return (true)
            }
            alert("You have entered an invalid email address!")
            return (false)
        }
        function validate() {





        }

        function onlyAlphabets(e, t) {

            try {

                if (window.event) {

                    var charCode = window.event.keyCode;

                }

                else if (e) {

                    var charCode = e.which;

                }

                else { return true; }

                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))

                    return true;

                else

                    return false;

            }

            catch (err) {

                alert(err.Description);

            }

        }
        function closepreview() {
            $find("mpepreview").hide();
            return false;

        }


        function closebtnchildfinalconfirmPopup() {
            $find("mpechildfinalconfirm").hide();
            return false;
        }

        function closepnladdplanconfirmPopup() {
            $find("mpeaddplanconfirm").hide();
            $find("mpeServicePlan").show();
            return false;
        }

        function closepreview1() {
            $find("mpepSearch").hide();

            return false;

        }



        function closechildtvconfrim() {
            $find("mpepchildtvconfirm").hide();
            return false;

        }

        function closeRenewNowConfPopup() {
            $find("mpeRenewNowConf").hide();
            $find("mpeRenewNowConfreconfrimpair").hide();
            return false;

        }

        function closerenewalPlanPopup() {
            $find("mpeRenewPlan").hide();
            return false;
        }
        function closeServicePopupS() {

            $find("mpeServiceS").hide();
            return false;
        }

        function closeServicePopupPlan() {

            $find("mpeServicePlan").hide();
            return false;
        }

        function closeMsgPopup() {
            $find("mpeMsg").hide();
            return false;
        }

        function closeServicePopupcnfm() {

            $find("mpeServicecnfm").hide();
            return false;
        }
        function closeServicePopupterms() {

            $find("mpeServiceterms").hide();
            return false;
        }

        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';

        }
    </script>
    <script type="text/javascript">
        function RadioCheck(rb) {
            var gv = document.getElementById("<%=GrdPlan.ClientID%>");
            var rbs = gv.getElementsByTagName("input");

            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }
                }
            }
        }

        function goBack() {
            window.location.href = "../Reports/EcafPages.aspx";
            return false;
        }
    </script>
    <style type="text/css">
        .red
        {
            color: Red;
        }
        .topHead
        {
            background: #E5E5E5;
            width: 96.5%;
            margin-top: 8px !important;
            padding: 3px !important;
        }
        .topHead table td
        {
            font-size: 12px;
            font-weight: bold;
        }
        .delInfo
        {
            /*padding: 10px;
            border: 1px solid #094791;*/
            width: 95%;
            margin: 5px;
            padding: 10px;
            border: 1px solid #094791;
        }
        .delInfo1
        {
            /*padding: 10px;
            border: 1px solid #094791;*/
            width: 97%;
            margin: 5px;
            border: 1px solid #094791;
        }
        .delInfoContent
        {
            width: 95%;
        }
        
        #Radio1
        {
            width: 131px;
        }
        #Radio2
        {
            width: 131px;
        }
        
        .style72
        {
        }
        .stylePh
        {
            width: 120px;
        }
        .style73
        {
            width: 7px;
        }
        .style74
        {
            width: 99px;
        }
        .style75
        {
            width: 180px;
        }
        .style76
        {
            width: 60px;
        }
        .style77
        {
            width: 0px;
        }
        .style78
        {
            width: 150px;
        }
        .style79
        {
            width: 90px;
        }
        .style81
        {
            width: 156px;
        }
        .style82
        {
            width: 93px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InIEvent);
    </script>
    <asp:Panel runat="server" ID="pnlRegisterLCO">
        <div class="maindive">
            <div style="float: right">
                <button onclick=" return goBack()" style="margin-right: 5px; margin-top: -15px;"
                    class="button">
                    Back</button>
            </div>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class="delInfo topHead">
                        <table class='delInfoContent'>
                            <tr>
                                <td align="right" width="130px">
                                    Distributor Name:
                                </td>
                                <td align="left" width="350px">
                                    <asp:Label ID="lblDistName" runat="server"></asp:Label>
                                </td>
                                <td align="right" width="50px">
                                    User:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbluser" runat="server"></asp:Label>
                                </td>
                                <td align="right" width="130px" id="avbal" runat="server">
                                    Available Balance:
                                </td>
                                <td align="left" width="67px" id="lblavbal" runat="server">
                                    <asp:Label ID="lblAvailBal" runat="server" Text="0000"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="delInfo" id="divsearchLco" runat="server">
                <table runat="server" align="center" id="tbl1" border="0">
                    <tr>
                        <td align="center">
                            <asp:RadioButtonList ID="RadSearchby" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="True" OnSelectedIndexChanged="RadSearchby_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Existing Customer</asp:ListItem>
                                <asp:ListItem Value="1">New Customer</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="style78" style="text-align: right;">
                            <asp:Label ID="lblsearchby" runat="server" Text="Search By"></asp:Label>
                            <asp:Label ID="lblserachmand" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdoSearchParamType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="0">VC/Mac ID</asp:ListItem>
                                <asp:ListItem Value="1">Account No</asp:ListItem>
                                <asp:ListItem Value="2">STB No.</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="text-align: left;">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                <asp:Literal ID="lblvctext" runat="server"></asp:Literal>
                                <asp:TextBox ID="txtSearchParam" runat="server" Width="120px" MaxLength="17" onkeyup="hideMACOnKeyPressVCSearch()" ></asp:TextBox>
                                <asp:Literal ID="lblstbtext" runat="server"></asp:Literal>
                                <asp:TextBox ID="txtStbnosearch" runat="server" Width="120px" Visible="false" MaxLength="17" onkeyup="hideMACOnKeyPressSTBSearch()" ></asp:TextBox>
                                <asp:Literal ID="lblOr" runat="server" ></asp:Literal>
                                <asp:Literal ID="lblMacIDtext" runat="server"></asp:Literal>
                                <asp:TextBox ID="txtsearchMACID" runat="server" Width="120px" Visible="false" MaxLength="17" onkeyup="hideVCOnKeyPressMACSearch()"></asp:TextBox>
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo" id="divTabPanels" runat="server" visible="false">
                <div class="delInfo" id="div2" runat="server">
                    <table width="100%">
                        <tr id="tr3">
                            <td colspan="6" align="left">
                                <b>
                                    <asp:Label ID="Label61" runat="server" Text="Customer Details"></asp:Label>
                                </b>
                            </td>
                            <td colspan="5" align="right">
                                <asp:Button ID="btnAddChildTV" runat="server" Text="ADD CHILD TV" OnClick="btnAddChildTV_Click"
                                    Visible="False" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="9">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style79">
                                <asp:Label ID="Label17" runat="server" Text="First Name" Width="73px"></asp:Label>
                                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label18" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtfname" runat="server" onkeydown="SetContextKey()" Style="resize: none;"
                                    Width="150px" onkeypress="return Alfhabet(event);"></asp:TextBox>
                            </td>
                            <td align="left" width="100px">
                                <asp:Label ID="Label19" runat="server" Text="Middle Name" Width="83px"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label20" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" class="style81">
                                <asp:TextBox ID="txtmname" runat="server" onkeydown="SetContextKey()" Style="resize: none;"
                                    Width="150px" onkeypress="return Alfhabet(event);"></asp:TextBox>
                            </td>
                            <td align="left" class="style82">
                                <asp:Label ID="Label3" runat="server" Text="Last Name" Width="78px"></asp:Label>
                                <asp:Label ID="Label69" runat="server" ForeColor="Red" Text="*"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label10" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtlname" runat="server" onkeydown="SetContextKey()" Style="resize: none;"
                                    Width="150px" onkeypress="return Alfhabet(event);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tr4">
                            <td align="left" class="style79">
                                <asp:Label ID="Label22" runat="server" Text="Mobile No."></asp:Label>
                                <asp:Label ID="Label113" runat="server" ForeColor="Red" Text="*"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtmobno" runat="server" onkeydown="SetContextKey()" Style="resize: none;"
                                    Width="150px" MaxLength="10" onkeypress="return isNumber(event)"></asp:TextBox>
                            </td>
                            <td align="left" width="80px">
                                <asp:Label ID="Label24" runat="server" Text="LandLine"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label25" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" class="style81">
                                <asp:TextBox ID="txtlandline" runat="server" onkeydown="SetContextKey()" Style="resize: none;"
                                    Width="150px" MaxLength="10" onkeypress="return isNumber(event)"></asp:TextBox>
                            </td>
                            <td align="left" class="style82">
                                <asp:Label ID="Label14" runat="server" Text="Email Id"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label30" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtemailid" runat="server" onkeydown="SetContextKey()" Style="resize: none;"
                                    Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="delInfo" id="divaddress" runat="server">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <b>
                                    <asp:Label ID="Label31" runat="server" Text="Address"></asp:Label>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="9">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="80px">
                                <asp:Label ID="Label32" runat="server" Text="Pin Code"></asp:Label>
                                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label33" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlpin" runat="server" AutoPostBack="True" Style="resize: none;
                                    margin-bottom: 0px;" Width="180px" OnSelectedIndexChanged="ddlpin_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td align="left" width="90px">
                                <asp:Label ID="Label34" runat="server" Text="City"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label35" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtcity" runat="server" onkeydown="SetContextKey()" Style="resize: none;"
                                    Width="180px" Enabled="false"></asp:TextBox>
                            </td>
                            <td align="left" width="80px">
                                <asp:Label ID="Label36" runat="server" Text="Area"></asp:Label>
                                <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label37" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlarea" runat="server" AutoPostBack="True" Style="resize: none;
                                    margin-bottom: 0px;" Width="180px" OnSelectedIndexChanged="ddlarea_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="80px">
                                <asp:Label ID="Label38" runat="server" Text="Street"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label39" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlstreet" runat="server" AutoPostBack="True" Style="resize: none;
                                    margin-bottom: 0px;" Width="180px" OnSelectedIndexChanged="ddlstreet_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td align="left" width="80px">
                                <asp:Label ID="Label40" runat="server" Text="Location"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label41" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddllocation" runat="server" AutoPostBack="True" Style="resize: none;
                                    margin-bottom: 0px;" Width="182px" OnSelectedIndexChanged="ddllocation_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td align="left" width="80px">
                                <asp:Label ID="Label42" runat="server" Text="Building"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label43" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlbuilding" runat="server" Style="resize: none; margin-bottom: 0px;"
                                    Width="180px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="80px">
                                <asp:Label ID="Label44" runat="server" Text="Flat no."></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label45" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtflatno" runat="server" onkeydown="SetContextKey()" Style="resize: none;"
                                    Width="180px"></asp:TextBox>
                            </td>
                            <td align="left" width="80px">
                                <asp:Label ID="lbladd" runat="server" Text="Address"></asp:Label>
                                <asp:Label ID="Label15" runat="server" ForeColor="Red" Text="*"></asp:Label>
                            </td>
                            <td width="10px">
                                <asp:Label ID="Label47" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtadd" runat="server" onkeydown="SetContextKey()" Style="resize: none;"
                                    TextMode="MultiLine" Width="180px"></asp:TextBox>
                            </td>
                            <td align="left" width="80px">
                                &nbsp;
                            </td>
                            <td width="10px">
                                &nbsp;
                            </td>
                            <td align="left">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="delInfo" id="divTabPanels1" runat="server" visible="false">
                <div class="delInfo" id="divdoc" runat="server">
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="6">
                                <b>
                                    <asp:Label ID="Label1" runat="server" Text="Document Details"></asp:Label>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="8">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="width: 50%; float: left; height: 180px; border: thin solid;">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 60px">
                                                Select Id Proof
                                            </td>
                                            <td width="10px">
                                                :
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlID" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlID_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtidproof" runat="server" Visible="false"></asp:TextBox>
                                                <asp:ImageButton ID="imgiprooftxtclose" runat="server" Visible="false" ImageUrl="~/Images/close.png"
                                                    OnClick="imgiprooftxtclose_click" Style="width: 20px; position: absolute; float: right;
                                                    margin-left: 10px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="style76">
                                                ID Proof
                                            </td>
                                            <td width="10px">
                                                :
                                            </td>
                                            <td align="left" class="style75">
                                                <asp:FileUpload ID="fupidproof" runat="server" />
                                                <asp:LinkButton ID="lnkfupidproof" runat="server" OnClick="lnkfupidproof_click"></asp:LinkButton>
                                                <asp:Label ID="lblidproofname" runat="server" Text="" Visible="false"></asp:Label>
                                                <asp:ImageButton ID="imgbtnidproof" runat="server" Visible="false" ImageUrl="~/Images/close.png"
                                                    OnClick="imgbtnidproof_click" Style="width: 20px; position: absolute; float: right;
                                                    margin-left: 10px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="style76">
                                                &nbsp;
                                            </td>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td align="left" class="style75">
                                                <asp:Image ID="Image2" runat="server" Height="116px" Width="139px" ImageUrl="~/Images/no_doc_uploaded.jpg" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 49%; float: right; height: 180px; border: thin solid;">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 100px">
                                                Select Resi. Proof
                                            </td>
                                            <td width="10px">
                                                :
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlResi" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlResi_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtresoproof" runat="server" Visible="false"></asp:TextBox>
                                                <asp:ImageButton ID="imgresiprooftxtclose" runat="server" Visible="false" ImageUrl="~/Images/close.png"
                                                    OnClick="imgresiprooftxtclose_click" Style="width: 20px; position: absolute;
                                                    float: right; margin-left: 10px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Resi Proof
                                            </td>
                                            <td width="10px">
                                                :
                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="fupresiproof" runat="server" />
                                                <asp:LinkButton ID="lnkfupresiproof" runat="server" OnClick="lnkfupresiproof_click"></asp:LinkButton>
                                                <asp:Label ID="lblreiprrofname" runat="server" Text="" Visible="false"></asp:Label>
                                                <asp:ImageButton ID="imgbtnresiproof" runat="server" Visible="false" ImageUrl="~/Images/close.png"
                                                    OnClick="imgbtnresiproof_click" Style="width: 20px; position: absolute; float: right;
                                                    margin-left: 10px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="style76">
                                                &nbsp;
                                            </td>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:Image ID="Image3" runat="server" Height="116px" Width="139px" ImageUrl="~/Images/no_doc_uploaded.jpg" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 50%; float: left; height: 180px; border: thin solid;">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 100px">
                                                Photo
                                            </td>
                                            <td width="10px">
                                                :
                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="fupphoto" runat="server" />
                                                <asp:LinkButton ID="lnkfupphoto" runat="server" OnClick="lnkfupphoto_click"></asp:LinkButton>
                                                <asp:Label ID="lblphotoname" runat="server" Text="" Visible="false"></asp:Label>
                                                <asp:ImageButton ID="imgbtnphoto" runat="server" Visible="false" ImageUrl="~/Images/close.png"
                                                    OnClick="imgbtnphoto_click" Style="width: 20px; margin-left: 10px; top: 1107px;
                                                    left: 295px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="style77">
                                                &nbsp;
                                            </td>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:Image ID="Image4" runat="server" Height="116px" Width="139px" ImageUrl="~/Images/no_doc_uploaded.jpg" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 49%; float: right; height: 180px; border: thin solid;">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100px">
                                                PDF Document
                                            </td>
                                            <td width="10px">
                                                :
                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="fuploadpdf" runat="server" />
                                                <asp:LinkButton ID="lnkpdfupload" runat="server" OnClick="lnkpdfupload_click"></asp:LinkButton>
                                                <asp:Label ID="lblPDFDOCNAME" runat="server" Text="" Visible="false"></asp:Label>
                                                <asp:ImageButton ID="imgbtnclose" runat="server" Visible="false" ImageUrl="~/Images/close.png"
                                                    OnClick="imgbtnclose_click" Style="width: 20px; position: absolute; float: right;
                                                    margin-left: 10px;" />
                                            </td>
                                            <tr>
                                                <td align="left" class="style77">
                                                    &nbsp;
                                                </td>
                                                <td width="10px">
                                                    &nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:Image ID="Image8" runat="server" Height="116px" Width="139px" ImageUrl="~/Images/no_doc_uploaded.jpg" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="divinfo" id="divsubmit" runat="server" visible="false">
                <table>
                    <tr>
                        <td align="center" colspan="6">
                            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                            &nbsp;
                            <asp:Button ID="btnback" runat="server" Text="Back" OnClick="btnback_Click" />
                            &nbsp;
                            <asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClick="btnsubmit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:Panel>
    <asp:Button ID="btnpreivedummy" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="poppreview" runat="server" BehaviorID="mpepreview" TargetControlID="btnpreivedummy"
        PopupControlID="pnlpreview" CancelControlID="imgClose4">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlpreview" runat="server" CssClass="Popup" Style="width: 70%; height: 630px;
        margin-top: 10px; display: none;">
        <%-- display: none; --%>
        <asp:Image ID="Image1" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closepreview();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Preview
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>
            <div class="delInfo1" id="div3" runat="server">
                <table width="100%">
                    <tr id="tr2">
                        <td colspan="6" align="left">
                            <b>Customer Details </b>
                            <td class="style82">
                    </tr>
                    <tr>
                        <td align="left" colspan="9">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="style79">
                            First Name
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:Label ID="lblfirstname" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" width="100px">
                            Middle Name
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left" class="style81">
                            <asp:Label ID="lblmidname" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="style82">
                            Last Name
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:Label ID="lbllastname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr5">
                        <td align="left" class="style79">
                            Mobile No.
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:Label ID="lblmobno" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" width="80px">
                            LandLine
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left" class="style81">
                            <asp:Label ID="lbllandline" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="style82">
                            Email Id
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:Label ID="lblemail" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo1" id="div4" runat="server">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <b>Address </b>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="9">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="style79">
                            Pin Code
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:Label ID="lblpin" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" width="100px">
                            City
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left" class="style81">
                            <asp:Label ID="lblcity" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" width="80px">
                            Area
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:Label ID="lblarea" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="80px">
                            Street
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:Label ID="lblstreet" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" width="80px">
                            Location
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:Label ID="lbllocation" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" width="80px">
                            Building
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:Label ID="lblbuilding" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="80px">
                            Flat no.
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:Label ID="lblflatno" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" width="80px">
                            Address
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left" colspan="6">
                            <asp:Label ID="lbladress" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo1" id="div5" runat="server">
                <table width="100%">
                    <tr>
                        <td align="left" colspan="6">
                            <b>Document Details </b>
                            <tr>
                                <td align="left" colspan="6">
                                    <hr />
                                </td>
                            </tr>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 50%; float: left; height: 126px; border: thin solid;">
                                <table width="100%">
                                    <tr>
                                        <td align="left" class="style76">
                                            Photo
                                        </td>
                                        <td width="10px">
                                            :
                                        </td>
                                        <td align="left" class="style75">
                                            <asp:Image ID="imgprevphoto" runat="server" Height="100px" Width="139px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 49%; float: right; height: 126px; border: thin solid;">
                                <table width="100%">
                                    <tr>
                                        <td align="left" class="style76">
                                            PDF
                                        </td>
                                        <td width="10px">
                                            :
                                        </td>
                                        <td align="left" class="style75">
                                            <asp:Label ID="lblprevpdf" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style76">
                                            &nbsp;
                                        </td>
                                        <td width="10px">
                                            &nbsp;
                                        </td>
                                        <td align="left" class="style75">
                                            <asp:Image ID="Image9" runat="server" Height="100px" Width="139px" ImageUrl="~/Images/no_doc_uploaded.jpg" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 50%; float: left; height: 140px; border: thin solid;">
                                <table width="100%">
                                    <tr>
                                        <td align="left" class="style76">
                                            Id Proof
                                        </td>
                                        <td width="10px">
                                            :
                                        </td>
                                        <td align="left" class="style75">
                                            <asp:Label ID="lblidproof" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style76">
                                            &nbsp;
                                        </td>
                                        <td width="10px">
                                            &nbsp;
                                        </td>
                                        <td align="left" class="style75">
                                            <asp:Label ID="lblprevidprood" runat="server" Text=""></asp:Label>
                                            <asp:Image ID="imgprevidproof" runat="server" Height="100px" Width="139px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 49%; float: right; height: 140px; border: thin solid;">
                                <table width="100%">
                                    <tr>
                                        <td align="left" class="style76">
                                            Resi.Proof
                                        </td>
                                        <td width="10px">
                                            :
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblresiproof" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style76">
                                            &nbsp;
                                        </td>
                                        <td width="10px">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblprevresiproof" runat="server" Text=""></asp:Label>
                                            <asp:Image ID="imgprevresiproof" runat="server" Height="100px" Width="139px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="delInfo1" id="div6" runat="server">
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Button ID="BtnPreviewconfrim" runat="server" CssClass="button" Text="Confirm"
                                Width="100px" OnClick="BtnPreviewconfrim_Click" />
                            &nbsp;&nbsp;
                            <input id="Button7" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                onclick="closepreview();" />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="popreconfrimpair" runat="server" BehaviorID="mpeRenewNowConfreconfrimpair"
        TargetControlID="hdnRenewNowConfirmreconfrimpair" PopupControlID="pnlRenewNowConfirmreconfrimpair">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnRenewNowConfirmreconfrimpair" runat="server" />
    <asp:Panel ID="pnlRenewNowConfirmreconfrimpair" runat="server" CssClass="Popup" Style="width: 430px;
        display: none; height: 160px;">
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
                        <asp:Label ID="Label27" runat="server" Text="Are you sure?" Font-Bold="true"></asp:Label><br />
                        <br />
                        <asp:Label ID="Label26" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnrenewnowconfimreconfirm" runat="server" CssClass="button" Text="Yes"
                            Width="100px" OnClick="btnrenewnowconfimreconfirm_Click" />
                        &nbsp;&nbsp;
                        <input id="Button11" class="button" runat="server" type="button" value="No" style="width: 100px;"
                            onclick="closeRenewNowConfPopup();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="PopUpRenewNowConfirm" runat="server" BehaviorID="mpeRenewNowConf"
        TargetControlID="hdnRenewNowConfirm" PopupControlID="pnlRenewNowConfirm">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnRenewNowConfirm" runat="server" />
    <asp:Panel ID="pnlRenewNowConfirm" runat="server" CssClass="Popup" Style="width: 430px;
        display: none; height: 160px;">
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
                        <asp:Label ID="lblRenewNowmsg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnRenewNowConfirm" runat="server" CssClass="button" Text="Yes" Width="100px"
                            OnClick="btnRenewNowConfirm_Click" />
                        &nbsp;&nbsp;
                        <input id="Button5" class="button" runat="server" type="button" value="No" style="width: 100px;"
                            onclick="closeRenewNowConfPopup();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="popaddplanconfirm" runat="server" BehaviorID="mpeaddplanconfirm"
        TargetControlID="hdnaddplanconfirm" PopupControlID="pnladdplanconfirm">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnaddplanconfirm" runat="server" />
    <asp:Panel ID="pnladdplanconfirm" runat="server" CssClass="Popup" Style="width: 430px;
        display: none; height: 160px; display: none;">
        <%--  --%>
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
                        <asp:Literal ID="lbladdplanconfirm" runat="server"></asp:Literal>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnepnladdplanconfirm" runat="server" CssClass="button" Text="Yes"
                            Width="100px" OnClick="btnepnladdplanconfirm_Click" />
                        &nbsp;&nbsp;
                        <input id="Button10" class="button" runat="server" type="button" value="No" style="width: 100px;"
                            onclick="closepnladdplanconfirmPopup();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="popMsg" runat="server" BehaviorID="mpeMsg" TargetControlID="hdnPop2"
        PopupControlID="pnlMessage">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnPop2" runat="server" />
    <asp:Panel ID="pnlMessage" runat="server" CssClass="Popup" Style="width: 430px; height: 160px;
        display: none;">
        <%-- display: none; --%>
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
                        <asp:Label ID="lblPopupResponse" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <input id="btnClodeMsg" class="button" runat="server" type="button" value="Close"
                            style="width: 100px;" onclick="closeMsgPopup();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <!----OtpPoupup---------------->
    <asp:Button ID="btndummy" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="popupRenewAll" runat="server" BehaviorID="mpeServiceS"
        TargetControlID="btndummy" PopupControlID="pnlServiceS" CancelControlID="imgClose4">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlServiceS" runat="server" CssClass="Popup" Style="width: 400px;
        height: auto; display: none;">
        <%-- display: none; --%>
        <asp:Image ID="imgClose4" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closeServicePopupS();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;OTP
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>
            <table width="90%">
                <tr>
                    <td style="color: #094791; font-weight: bold;" align="center" colspan="3">
                        Please Enter OTP
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:TextBox ID="txtOtp" runat="server" placeholder="Enter OTP Number" Width="110px"
                            MaxLength="6"></asp:TextBox>
                        <br />
                        <asp:Label ID="lblerrmsgotp" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <%--   <asp:HiddenField ID="HiddenField9" runat="server" />--%>
                        <br />
                        <asp:Button ID="BtnAutoRenewAll" runat="server" CssClass="button" Width="100px" Text="Confirm"
                            OnClick="BtnAutoRenewAll_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <input type="button" class="button" value="Cancel" style="width: 100px;" onclick="closeServicePopupS();" />
                        <br />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <!----OtpPoupup---------------->
    <asp:Button ID="Button2" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="PopUpPlan" runat="server" BehaviorID="mpeServicePlan"
        TargetControlID="Button2" PopupControlID="PanelPlan" CancelControlID="imgClose5">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:Panel ID="PanelPlan" runat="server" CssClass="Popup" Style="width: 550px; height: auto;
        display: none;">
        <%-- display: none; --%>
        <asp:Image ID="imgClose5" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closeServicePopupPlan();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Select Package &nbsp;&nbsp;<asp:Label ID="lblPlanselect" ForeColor="Red"
                            runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>
            </table>
            <table width="90%">
                <tr id="tr1" runat="server">
                    <td align="left" width="100px">
                        <asp:Label ID="Label75" Font-Bold="true" runat="server" Text="Plan Payterm"></asp:Label>
                    </td>
                    <td width="5px">
                        <asp:Label ID="Label76" runat="server" Text=":"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:RadioButtonList ID="rdbplanpayterm" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                    OnSelectedIndexChanged="rdbplanpayterm_SelectedIndexChanged">
                                    <asp:ListItem Text="30 Days" Value="1" Selected="True"></asp:ListItem>
                                    <%--<asp:ListItem Text="3 Month" Value="3"  ></asp:ListItem>
                                    <asp:ListItem Text="6 Month" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="12 Month" Value="12"></asp:ListItem>--%>
                                </asp:RadioButtonList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <div id="Div1" runat="server" style="overflow: auto; height: auto; width: 100%; max-height: 300px;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="GrdPlan" CssClass="Grid" runat="server" Width="100%" AutoGenerateColumns="False"
                                        EnableModelValidation="True">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Plan">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblplanname" runat="server" Text='<%# Eval("plan_name") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MRP.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmrp" runat="server" Text='<%# Eval("custprice") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="RbntCheckplan" runat="server" onclick="RadioCheck(this);" />
                                                    <asp:HiddenField ID="hdnplantype" runat="server" Value='<%# Eval("plantype") %>' />
                                                    <asp:HiddenField ID="hdnplanpoid" runat="server" Value='<%# Eval("planpoid") %>' />
                                                    <asp:HiddenField ID="hdndealpoid" runat="server" Value='<%# Eval("dealpoid") %>' />
                                                    <asp:HiddenField ID="hdnproductpoid" runat="server" Value='<%# Eval("productpoid") %>' />
                                                    <asp:HiddenField ID="hdncustprice" runat="server" Value='<%# Eval("custprice") %>' />
                                                    <asp:HiddenField ID="hdnlcoprice" runat="server" Value='<%# Eval("lcoprice") %>' />
                                                    <asp:HiddenField ID="hdnpayterm" runat="server" Value='<%# Eval("payterm") %>' />
                                                    <asp:HiddenField ID="hdndevicetype" runat="server" Value='<%# Eval("devicetype") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <%--  <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="Label106" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>--%>
                <tr>
                    <td align="center" colspan="3">
                        <%--   <asp:HiddenField ID="HiddenField9" runat="server" />--%>
                        <br />
                        <asp:Button ID="btnPopPlan" runat="server" CssClass="button" Width="100px" Text="Confirm"
                            OnClick="btnPopPlan_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <input type="button" class="button" value="Cancel" style="width: 100px;" onclick="closeServicePopupPlan();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <!----PopUpCnfm---------------->
    <asp:Button ID="Button3" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="PopCnfm" runat="server" BehaviorID="mpeServicecnfm" TargetControlID="Button3"
        PopupControlID="PanelCnfm" CancelControlID="imgClose6">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:Panel ID="PanelCnfm" runat="server" CssClass="Popup" Style="width: 400px; height: auto;
        display: none;">
        <%-- display: none; --%>
        <asp:Image ID="imgClose6" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closeServicePopupcnfm();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Confirmation
                    </td>
                </tr>
                <tr>
                    <td class="style74">
                        <asp:Label ID="Label110" runat="server" Text="STB No. "></asp:Label>
                    </td>
                    <td class="style73">
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblStb" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style74">
                        <asp:Label ID="Label111" runat="server" Text="V.C. No./MAC Id"></asp:Label>
                        &nbsp;
                    </td>
                    <td class="style73">
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblVC" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="Oldplan">
                    <td class="style74">
                        <asp:Label ID="Label8" runat="server" Text="Old Basic Package"></asp:Label>
                    </td>
                    <td class="style73">
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblOldBasePack" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style74">
                        <asp:Label ID="Label112" runat="server" Text="Basic Package"></asp:Label>
                    </td>
                    <td class="style73">
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblBasePack" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style72" colspan="3">
                        <asp:CheckBox ID="chkTerm" runat="server" Checked="true" />
                        I Accept The
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Terms & Conditions</asp:LinkButton><br />
                        &nbsp;&nbsp;&nbsp;<asp:Label ID="lblchkterm" runat="server" ForeColor="Red" Text=""></asp:Label>
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
                    </td>
                </tr>
                <%--  <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="Label106" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>--%>
                <tr>
                    <td align="center" colspan="3">
                        <%--   <asp:HiddenField ID="HiddenField9" runat="server" />--%>
                        <br />
                        <asp:Button ID="btnPopCnfm" runat="server" CssClass="button" Width="100px" Text="Confirm"
                            OnClick="btnPopCnfm_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <input type="button" class="button" value="Cancel" style="width: 100px;" onclick="closeServicePopupcnfm();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <!----TermsNCondition---------------->
    <asp:Button ID="Button1" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="popTerms" runat="server" BehaviorID="mpeServiceterms"
        TargetControlID="Button1" PopupControlID="Panelterm" CancelControlID="imgClose7">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="HiddenField3" runat="server" />
    <asp:Panel ID="Panelterm" runat="server" CssClass="Popup" Style="width: 400px; height: auto;
        display: none;">
        <%-- display: none; --%>
        <asp:Image ID="imgClose7" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closeServicePopupterms();"
            ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Terms And Conditions
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" ForeColor="Blue" Font-Size="Large" Text="Subscriber Declaration :"></asp:Label>
                        <div style="height: 220px; margin-left: 2px; text-align: left;">
                            <p>
                                I Have Read And Understand The Terms And Conditions Provided Herewith And Acknowledge
                                That The Tariff Plan Selected By Me And The Applicable Rates Together Constitute
                                The Entire Terms And Conditions And I Shall Be Bound By The Same. I Hereby Declare
                                And Confirm That I Have Received The Above Hardware And The Information Contained
                                Herein Is True And Accurate In Every Respect. I Also Acknowledge The Channel Package
                                Subscription Plan Selected By Me And Rates Applicable For The Same. I Agree To Make
                                Payment To The Cable Network From My Channel Package Subscription Within 15 Days
                                Of Each Bill Date, Failing Which I Will Pay Interest @ 15% Per Annum As Per Tariff
                                Orders Of TRAI
                            </p>
                        </div>
                    </td>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" ForeColor="Blue" Font-Size="Large" Text="Terms and conditions :"></asp:Label>
                            <div style="overflow: scroll; height: 250px; margin-left: 2px; text-align: left;">
                                1. Definitions:<br />
                                (a) “addressable system” means an electronic device or more than one electronic
                                devices put in an integrated system through which signals of television channels
                                can be sent in encrypted form, which can be decoded by the device or devices at
                                the premises of the subscriber within limits of the authorization made, through
                                the Conditional Access System and Subscriber Management System on the explicit choice
                                and request of such subscriber, by the cable operator to the subscriber.<br />
                                (b) “alternative tariff package” (ATP) means a tariff package which a service provider
                                may offer, in addition to the standard tariff package, for supply of a set box to
                                the subscriber for receiving programmes;<br />
                                (c) “Authority” means Telecom Regulatory Authority of India established under sub-section
                                (1) of section 3 of the Telecom Regulatory, Authority of India Act, 1997 (24 of
                                1997);<br />
                                (d) “authorized officer” shall have the same meaning as given in clause (a) of section
                                2 of the Cable Television Networks (Regulation) Act, 1995 (7 of 1995);<br />
                                (e) “broadcaster” means any person including an individual, group of persons, public
                                or body corporate, firm or any organization or body who or which is providing programming
                                services and includes his or her authorized distribution agencies;<br />
                                (f) “basic service tier” means a package of free-to-air channels offered by the
                                cable operator to a subscriber with an option to subscribe, for a single price to
                                the subscribers of the area in which his cable television networks is providing
                                service;<br />
                                (g) “DAS Area” means the area notified under sub-section (1) of the section 4A of
                                the Cable Television Networks (Regulation) Act, 1995 (7 of 1995);<br />
                                (h) “LCO” means a Local Cable Operator i.e. person who provides cable service through
                                a cable television network or otherwise controls or is responsible for the management
                                and operation of a cable television network;<br />
                                (i) “Cable Service” means the transmission by cables of programmes including retransmission
                                by cables of any broadcast television signals;<br />
                                (j) “cable television network” means any system consisting of closed transmission
                                paths and associated signal generation, control and distribution equipment, designed
                                to provide cable service for reception by multiple subscribers;<br />
                                (k) “free to air channel” or “FTA channel” means a channel for which no fees is
                                to be paid to the broadcaster for its retransmission through electromagnetic waves
                                through cable or through space intended to be received by the general public either
                                directly or indirectly;<br />
                                (l) “multi system operator” (MSO) means a cable operator who receives a programming
                                service from a broadcaster or his authorized agencies and retransmits the same or
                                transmits his own programming service for simultaneous reception either by multiple
                                subscribers directly or through one or more cable operators, and includes authorized
                                distribution agencies by whatever name called;<br />
                                (m) “hathway” means MSO<br />
                                (n) “pay channel” means a channel for which fees is to be paid to the broadcaster
                                for its retransmission through electromagnetic waves through cable or through space
                                intended to be received by the general public either directly or indirectly and
                                which would require the use of an addressable system attached with the receiver
                                set of a subscriber;<br />
                                (o) “programme” means any television broadcast and includes -<br />
                                (i) Exhibition of films, features, dramas, advertisements and serials<br />
                                (ii) Any audio or visual or audio-visual live programme or presentation and the
                                expression “programming service” shall be construed accordingly;<br />
                                (p) “service provider” means the Government as service provider and includes a licensee
                                as well as any broadcaster, multi system operator (MSO), cable operator or distributor
                                of TV channels;<br />
                                (q) “set top box” or “STB” means a device, which is connected to, or is part of
                                a television and which allows a subscriber to receive in unencrypted/descrambled
                                form subscribed pay and FTA channels through an addressable system;<br />
                                (r) “standard tariff package” (STP) means a package of tariff as may be determined
                                by the Authority for supply of a set top box to the subscriber by a service provider
                                for receiving programme;<br />
                                (s) “subscriber” means a person who receives the signal of a service provider at
                                a place indicated by him to the service provider without further transmitting it
                                to any other person;<br />
                                (t) “You” means the subscriber.<br />
                                2. Provision of Service:<br />
                                2.1 Cable service shall be made available to the subscriber with effect from the
                                date of activation of the STB and on terms and conditions contained herein and also
                                contained in the consumer charter which is available on the website www.hathway.com<br />
                                2.2 The subscriber shall fill in the Customer Application Form (CAF) in duplicate
                                and submit the CAF to the LCO. The subscriber shall ensure that the information
                                stated in the Customer Application Form (CAF) is and shall continue to be complete
                                and accurate in all respects and the subscriber hereby undertakes to immediately
                                notify its LCO of any change thereto. Photo identification and Address proof has
                                also to be submitted along with the CAF, else the same will be treated as an incomplete
                                CAF. The LCO shall return the duplicate copy of the CAF to the subscriber duly acknowledged.<br />
                                2.3 All incomplete Customer Application Forms shall be rejected and the deficiencies
                                shall be informed to the subscriber.<br />
                                2.4 The LCO will respond within 2 working days of receipt of application, and inform
                                the subscriber of the deficiencies and shortcomings in the CAF submitted by him.<br />
                                2.5 In case of technical or operational non feasibility at the location requested
                                by the subscriber, LCO will inform the subscriber the reasons for the same within
                                2 working days from the date of receipt of the CAF by LCO. In the event, the STB
                                is not installed within two working days, a rebate of Rs.15/- per day for the first
                                five days and Rs.10/- per day thereafter will be offered to the subscriber.<br />
                                2.6 Under the Hire Purchase scheme, the ownership of the STB will be transferred
                                upon payment of the last monthly installment as stated overleaf. However till such
                                time that all the installments are fully paid the MSO (“Hathway Cable &Datacom Limited”)
                                shall remain and continue to remain the sole and absolute owner of the STB.<br />
                                2.7 Under One time activation scheme the MSO (“Hathway Cable &Datacom Limited”)
                                shall remain the sole and absolute owner of the STB.<br />
                                2.8 Under the 3 year rental scheme, the ownership of the STB will be transferred
                                upon payment of the last monthly rental payment.<br />
                                2.9 Under the outright sale, the STB ownership will be transferred to the subscriber<br />
                                2.10 Under Hire Purchase / Rental STB plans, should a subscriber seek termination
                                of cable services, LCO will arrange for a refund of the amount paid as Security
                                Deposit after deducting a fifteen per cent depreciation for each year of usage,
                                provided the STB has been returned in a working condition along with all accessories
                                like remote control, AC adapter (if any) and connecting cables and has not been
                                tampered with.<br />
                                2.11 Monthly rentals for the STB will be payable to LCO and will be a part of the
                                regular invoice raised to the subscriber for the cable services rendered by Hathway.<br />
                                2.12 Each STB comes with a one year warranty. During the warranty period no repair
                                and maintenance charges are payable, provided the STB has been used in normal working
                                conditions and is not tampered with. There is no warranty applicable on the remote
                                control.<br />
                                2.13 During the warranty period, the STB will be repaired or replaced within 24
                                hours of receipt of complaint. After the expiry of the warranty period, repairs
                                to the STB would have to be paid for by the subscriber and a replacement STB may
                                be offered, if available. Alternatively if the subscriber opts for the optional
                                Annual Maintenance Contract (AMC) of Rs.200/- per annum, they will definitely be
                                provided a standby STB and no repair charges would have to be paid for the STB only
                                (remote excluded) provided the STB has been used under normal working conditions
                                and is not tampered with.<br />
                                2.14 Changes in the rates of taxes & Government duties will be informed to subscribers
                                and passed on.<br />
                                2.15 In case of STB malfunction, the LCO will replace or repair the STB within 24
                                hours of receipt of complaint. Repair charges will be payable if the STB is out
                                of warranty period.<br />
                                2.16 Refund of security deposit will be made available to the subscriber within
                                seven days upon receipt of STB, provided the same has not been tampered with.<br />
                                2.17 STB will not be made available to a subscriber on rental scheme again if he/she
                                has already availed of this at the same location in the past.<br />
                                2.18 The subscriber shall have the option to select packages or channels on an a
                                la carte basis by ticking the same on the CRF. The subscriber shall select the payment
                                methodology and the payment term on the same along with the STB details where the
                                subscriber wants these channels to be activated. Upon receipt of the fully filled
                                CRF and complete and correct in all respects, the channels selected by the subscriber
                                shall be activated within 48 hours of its receipt.<br />
                                2.19 Composition of channels in any package that the subscriber has availed of,
                                will not be altered for a period of six months from the date of enrolment. Should
                                there be a change in the same due to any channel becoming unavailable on our network,
                                an alternative channel from that genre & language will be provided or a price reduction
                                equivalent to the a la carte rate of that channel will be provided from the date
                                of discontinuation.<br />
                                2.20 The Subscriber hereby agrees to allow the authorized representatives of the
                                LCO to enter upon the Installation Address for inspection, installation, removal,
                                replacement and repossession of the Hardware under the Terms hereof. This clause
                                survives the termination until the all the dues are paid and the Viewing Card (“VC”)
                                along with the STB is returned in satisfactory working condition.<br />
                                2.21 The Cable Service and the license to use the VC shall be for personal viewing
                                of the Subscriber/s and for his family members only. No assignment of VC shall be
                                valid unless the same is approved in writing by LCO. Subscriber shall not allow
                                public viewing or exploit the same for commercial benefit or otherwise. Breach of
                                this clause will result in termination of Service and the subscriber shall also
                                be liable to pay damages.<br />
                                2.22 The Subscriber acknowledges that the VC has been merely licensed to the Subscriber
                                to avail the Channels for one TV set only and shall at all times be the exclusive
                                property of Hathway and that he/she has been fully explained and accepts that any
                                unauthorized relay or retransmission of the signal will constitute infringement
                                of copyright of the content providers/owners/licensors thereof and will in addition
                                to the termination of Service, attract civil and/or criminal liability under the
                                law.<br />
                                2.23 The Subscriber undertakes not to use or cause to be used the VC with any other
                                set top box or device and/or STB with any other VC or device and shall ensure the
                                safety and security of the Hardware from unauthorized use, theft, misuse, damages,
                                loss etc.<br />
                                2.24 The subscriber undertakes that he shall neither by himself nor allow any other
                                person to modify, misuse or tamper with the Hardware or to add or remove any seal,
                                brand, logo, information, etc. which affects or may affect the integrity/ functionality/identity
                                of the Hardware or otherwise remove or replace any part thereof;
                                <br />
                                2.25 The subscriber undertakes not to do or allow any act or thing to be done as
                                a result of which the right of the LCO/ MSO/Distributor/Hathway in relation to the
                                Service and/or Hardware or of the channel providers/distributors/ in relation to
                                any Channel, may become restricted, extinguished or otherwise prejudiced thereby
                                or they or any of them may be held or alleged to be in breach of their obligation
                                under any agreement to which they are party or otherwise are so bound.<br />
                                2.26 The subscriber undertakes not to hypothecate, transfer or create or suffer
                                any charge, lien or any onerous liability in respect of the Hardware which is not
                                owned by the Subscriber.<br />
                                2.27 The subscriber undertakes not to relay, transmit or redistribute the signals/Service
                                to any Person or connect to any other device for any redistribution purpose.<br />
                                2.28 Commercial establishments will be governed by tariffs as laid down by the Authority
                                from time to time.<br />
                                2.29 All the terms and conditions including the provision related to the terms of
                                service, tariff, rebates, discount, refund shall be subject to the rule, regulation,
                                notification, guidelines as may be specified by the Authority or as may be applicable
                                from time to time.<br />
                                3. Payment Obligation:<br />
                                3.1 The subscriber shall ensure prompt payment of all the bills within 15 days of
                                the bill date. All payments shall be made either to Hathway or its LCO.<br />
                                3.2 Any payment made after 15 days will attract simple interest @15% per annum on
                                pro rata basis for the number of days delayed.<br />
                                3.3 Billing will be on a monthly basis.
                                <br />
                                3.4 Billing dispute if any will be resolved within 7 days.<br />
                                3.5 Refund, if any will be issued within 30 days of receipt of complaint.<br />
                                3.6 Customer under prepaid module need to renew their plan on or before the expiry
                                date. Customer would be notified regarding the renewal of services through B-mail
                                or SMS prior to the expiry date<br />
                                4. Suspensions/Termination of Service:<br />
                                4.1 The terms will commence from the date of installation of the Hardware and shall
                                remain in full force and effect unless terminated under the Terms.<br />
                                4.2 A 15 day notice period will be given if the LCO chooses to discontinue providing
                                a channel. the notice discontinuation shall be published in the local newspaper
                                circulating in the subscribers locality and shall also be displayed on the TV screen
                                as a scroll on the local cable channel.<br />
                                4.3 If the subscriber chooses to relocate, the subscriber shall submit its application
                                in advance to its LCO. After verification of the outstandings, the LCO shall provide
                                the services at the new location, provided it is technically and operationally feasible.
                                If not, the LCO will inform the subscriber likewise and the subscriber can opt to
                                surrender the STB and proceed to claim a refund as per the terms of the scheme under
                                which the subscriber has availed of the STB.<br />
                                4.4 If the services have been temporarily discontinued on the subscribers request,
                                no charges other than STB rentals will be payable by the subscriber.<br />
                                4.5 No suspension of services is possible if the period of suspension comprises
                                part of a calendar month.<br />
                                4.6 Suspension of services is possible for one calendar month or a multiple of calendar
                                month, but the period cannot exceed three calendar months.<br />
                                4.7 No reactivation charges are payable by the subscriber if the period of suspension
                                is under three calendar months. Thereafter a reconnection charge of Rs.50/- plus
                                service tax will be levied.<br />
                                4.8 If the subscriber submits its disconnection notice 15 days in advance, no charges
                                will be payable by the subscriber even LCO fails to disconnect the service.<br />
                                4.9 Any request for addition of channel/package will by default be done from the
                                next billing cycle, unless demanded as an immediate request. Disconnection of a
                                channel/package is possible only on a calendar month basis or on expiry of the term
                                of the contracted package.<br />
                                4.10 Notwithstanding the aforesaid, the cable service shall be liable to be terminated
                                or suspended at the option of LCO either wholly or partly, upon occurrence of any
                                of the following events i.e. (a) if the subscriber commits a payment default; (b)
                                in case of breach by the subscriber; (c) if the Rental Agreement is terminated;
                                (d) if the subscriber is declared bankrupt, or insolvency proceedings have been
                                initiated against the subscriber; (e) in order to comply with the Cable television
                                Networks (Regulation) Act, 1995 and/or the Rules made thereunder and all and any
                                other applicable laws, notifications, directions and Regulations of any statutory
                                or regulatory bodies; (f) if the Broadcaster/Channel Providers suspend or discontinue
                                to transmit any Channel/s for any reason not attributable to the LCO.<br />
                                4.11 In the event of suspension, the Subscriber will be liable to pay forthwith
                                upto the last day of the month of suspension/termination and to return forthwith
                                the VC, in working condition (reasonable wear and tear excepted).<br />
                                4.12 In the event of termination, the Subscriber will be liable to pay forthwith
                                upto the last day of the month of termination and to return forthwith the STB and
                                the VC, in working condition (reasonable wear and tear excepted).<br />
                                4.13 The cable Service may be restored upon receipt of all the dues, advance Subscription
                                or deposit, reconnection charges (if payable) and any other amount payable under
                                the Terms and on such other terms and condition as may be in force. If the Service
                                was suspended due to the Subscriber's default, the Subscriber shall also pay the
                                amount for the disconnected period as if the Service had continued.<br />
                                5. Redressal of Complaints:<br />
                                5.1 You can log in your complaint directly with LCO.
                                <br />
                                5.2 It will be the responsibility of the LCO to maintain the Quality Of Services
                                standards as laid down by the Regulator wherever it pertains to distribution of
                                signals from the node/amplifier of Hathway.<br />
                                6. Force Majeure:<br />
                                If at any time, during the continuance of Service, the Service is interrupted, discontinued
                                either whole or in part, by reason of war, warlike situation, civil commotion, theft,
                                willful destruction, terrorist attack, sabotage, fire, flood, earthquake, riots,
                                explosion, epidemic, quarantine, strikes, lock out, compliance with any acts or
                                directions of any judicial, statutory or regulatory authority or any others Acts
                                of God, or if any or more Channels are discontinued due to any technical or system
                                failure at any stage or for any other reasons beyond the reasonable control of the
                                LCO or Hathway, the Subscriber will not have any claim for any loss or damages against
                                the LCO.<br />
                                7. Disclaimer:<br />
                                The LCO will make reasonable efforts to render uninterrupted Service to the Subscriber
                                and make no representation and warranty other than those set forth in the Terms
                                and hereby expressly declaim all other warranties express or implied, including
                                but not limited to any implied warranty or merchantability or fitness for particular
                                purpose.<br />
                                8. Limitation of Liability:<br />
                                LCO, Distributors and Hathway and the employees thereof shall be not liable to the
                                Subscriber or to any other person for all or any indirect, special, incidental or
                                consequential damage arising out of or in connection with the provision of the Service
                                or inability to provide the same whether or not due to suspension, interruption
                                or termination of the Service or for nay inconvenience, disappointment due to deprival
                                of any programme or information whether attributable to any negligent act or omission
                                or otherwise. Provided however the maximum liability of LCO for any actual or alleged
                                breach shall not exceed the subscription paid in advance for such duration of Service,
                                for which the Subscriber had paid in advance but was deprived due to such breach.<br />
                                9. Indemnity:<br />
                                The Subscriber hereby indemnifies and hold harmless the LCO and Hathway from all
                                the loss, claims, demand, suits, proceedings, damages, costs, expenses, liabilities
                                (including, without limitation, reasonable legal fees) or cause of for use and misuse
                                of the Cable Service or for non-observance of the Terms by the Subscriber.<br />
                                10. Notice:<br />
                                Notice at the Installation Address shall be deemed to be sufficient and binding
                                on the Subscriber.<br />
                                11. Jurisdiction:<br />
                                All disputes and differences with respect to these Terms between the Subscriber
                                and LCO shall be shall be subject only to the jurisdiction of the courts at Mumbai.<br />
                                12. Miscellaneous:<br />
                                If any of the provisions of these Terms becomes or is declares illegal, invalid
                                or unenforceable for any reason, the other provisions shall remain in full force
                                and effect and no failure or delay to exercise any right or remedy hereunder shall
                                be construed or operate as a waiver thereof. Terms may be amended by the authority
                                from time to time and shall be binding on all.<br />
                                13. The terms and condition prescribed under the regulation issued by Authority
                                on 14th May 2012 are applicable herewith. Detailed information is available on the
                                authorized site of Telecom Regulatory Authority of India viz: www.trai.gov.in<br />
                            </div>
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
                    </td>
                </tr>
                <%--  <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="Label106" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>--%>
                <tr>
                    <td align="center" colspan="3">
                        <%--   <asp:HiddenField ID="HiddenField9" runat="server" />--%>
                        <br />
                        <asp:Button ID="btnTerms" runat="server" CssClass="button" Width="100px" Text="Back"
                            OnClick="btnTerms_Click" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="popallrenewal" runat="server" BehaviorID="mpeRenewPlan"
        TargetControlID="hdnrenewPlan" PopupControlID="pnlrenewalPlan">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnrenewPlan" runat="server" />
    <asp:Panel ID="pnlrenewalPlan" runat="server" CssClass="Popup" Style="width: 700px;
        display: none; height: 300px;">
        <%-- display: none; --%>
        <asp:Image ID="Image7" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closerenewalPlanPopup();" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3">
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblAllStatus" runat="server" ForeColor="#094791" Font-Bold="true"
                            Text="Renewal Pack Status"></asp:Label>
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
                    <td colspan="3" align="center">
                        <div class="plan_scroller" style="overflow: auto; max-height: 185px;">
                            <asp:GridView ID="Gridrenew" EmptyDataText="No Plan Found" CssClass="Grid" runat="server"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField HeaderText="Plan Name" DataField="PlanName" ItemStyle-Width="350px"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Renew Status" DataField="RenewStatus" ItemStyle-Width="700px"
                                        ItemStyle-HorizontalAlign="Left" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <table width="90%">
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="Button4" runat="server" CssClass="button" Text="OK" Visible="true"
                            Width="100px" OnClientClick="closerenewalPlanPopup();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <asp:Button ID="Button6" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="PopuPSearchDetails" runat="server" BehaviorID="mpepSearch"
        TargetControlID="Button6" PopupControlID="PanelSD" CancelControlID="Image5">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="PanelSD" runat="server" CssClass="Popup" Style="width: 410px; height: 310px;
        display: none;">
        <%-- display: none; --%>
        <asp:Image ID="Image5" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closepreview1();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%" style="height: 162px">
                <tr>
                    <td align="right">
                        Account No. :
                    </td>
                    <td align="left">
                        <asp:Label ID="lblSAccNo" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Plan Name :
                    </td>
                    <td align="left">
                        <asp:Label ID="lblSPlanName" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        MRP :
                    </td>
                    <td align="left">
                        <asp:Label ID="lblsMrp" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        STB ID<asp:Label ID="Label16" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtpairstb" runat="server" onkeyup="hideMACOnKeyPressSTB()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        VC ID<asp:Label ID="Label21" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtpairVC" runat="server" onkeyup="hideMACOnKeyPressVC()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                       OR  &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        MAC ID<asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtMacId" runat="server" onkeyup="hideVCOnKeyPressMAC()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <%--   <asp:HiddenField ID="HiddenField9" runat="server" />--%>
                        <br />
                        <asp:Button ID="btnPairChiltTV" runat="server" CssClass="button" Width="100px" Text="Submit"
                            OnClick="btnPairChiltTV_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <input type="button" class="button" value="Cancel" style="width: 100px;" onclick="closepreview1();" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblchildtverr" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <asp:Button ID="Button8" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="popchildtvconfirm" runat="server" BehaviorID="mpepchildtvconfirm"
        TargetControlID="Button8" PopupControlID="pnlchildtvconfirm" CancelControlID="Image6">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlchildtvconfirm" runat="server" CssClass="Popup" Style="width: 400px;
        height: 280px; display: none;">
        <%-- display: none; --%>
        <asp:Image ID="Image6" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closechildtvconfrim();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%" style="">
                <tr>
                    <td align="center">
                        Are you sure you want to Add Child TV with the following details?
                        <hr />
                    </td>
                </tr>
            </table>
            <table width="100%" style="height: 162px">
                <tr>
                    <td align="right" style="width: 100px;">
                        Account No. :
                    </td>
                    <td align="left" style="width: 174px;">
                        <asp:Label ID="lblaccnochildconfim" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Plan Name :
                    </td>
                    <td align="left">
                        <asp:Label ID="lblplannamechildconfim" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        MRP :
                    </td>
                    <td align="left">
                        <asp:Label ID="lblmrpchildconfim" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblSTBID" runat="server" Text="STB ID :" Visible="false"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblstbchild" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblVCID" runat="server" Text="VC ID:" Visible="false"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblvcidchild" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblMACID" runat="server" Text="MAC ID :" Visible="false"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblmacidchild" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnpairchildconfirm" runat="server" CssClass="button" Width="100px"
                            Text="Confirm" OnClick="btnpairchildconfirm_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="button" class="button" value="Close" style="width: 100px;" onclick="closechildtvconfrim();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="popchildfinalconfirm" runat="server" BehaviorID="mpechildfinalconfirm"
        TargetControlID="hdnchildfinalconfirm" PopupControlID="pnlchildfinalconfirm">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnchildfinalconfirm" runat="server" />
    <asp:Panel ID="pnlchildfinalconfirm" runat="server" CssClass="Popup" Style="width: 430px;
        display: none; height: 160px;">
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
                        <asp:Label ID="Label28" runat="server" Text="Are you sure?" Font-Bold="true"></asp:Label><br />
                        <br />
                        <asp:Label ID="lblchildfinalconfirm" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnchildfinalconfirm" runat="server" CssClass="button" Text="Yes"
                            Width="100px" OnClick="btnbtnchildfinalconfirm_Click" />
                        &nbsp;&nbsp;
                        <input id="Button12" class="button" runat="server" type="button" value="No" style="width: 100px;"
                            onclick="closebtnchildfinalconfirmPopup();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
</asp:Content>
