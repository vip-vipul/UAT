<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmAssignPlan.aspx.cs" Inherits="PrjUpassPl.Transaction.frmAssignPlan" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JS/jquery.min.js" type="text/javascript"></script>
    <script src="../JS/jqueryui.min.js" type="text/javascript"></script>
    <link href="../CSS/jqueryui.css" rel="Stylesheet" type="text/css" />
    <link href="http://code.jquery.com/ui/1.10.4/themes/ui-lightness/jquery-ui.css
         " rel="stylesheet" />
    <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
    <script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
   <script type="text/javascript">
       function AllvalidateConfirm() {
           var cboCheckModify = document.getElementById("<%=cboCheckModify.ClientID %>");
           if (cboCheckModify.checked == true) {
               return true;
           }
           else {
               alert('You must accept the terms');
               return false;
           }

       }

       function Allvalidate() {
           var ValidationSummary = "";
           ValidationSummary += NameValidation();
           ValidationSummary += EmailValidation();
           ValidationSummary += PhonenoValidation();
           ValidationSummary += EmailValidation_last();
           ValidationSummary += MiddleValidation();
           ValidationSummary += AddressValidation();
           if (ValidationSummary != "") {
               alert(ValidationSummary);
               return false;
           }
           else {

               $find("mpeModifyConfirm").show();
               $find("mpeModifyCust").hide();
               var txtModifyFirstName = document.getElementById("<%=txtModifyFirstName.ClientID %>");
               var txtModifyMiddleName = document.getElementById("<%=txtModifyMiddleName.ClientID %>");
               var txtModifylastName = document.getElementById("<%=txtModifylastName.ClientID %>");
               var txtmodifyAddress = document.getElementById("<%=txtmodifyAddress.ClientID %>");
               var txtModifyEmail = document.getElementById("<%=txtModifyEmail.ClientID %>");
               var txtModifymobile = document.getElementById("<%=txtModifymobile.ClientID %>");


               var lblModifyFirstName = document.getElementById("<%=lblModifyFirstName.ClientID %>");
               var lblModifyLastName = document.getElementById("<%=lblModifyLastName.ClientID %>");
               var lblmodifyMiddlename = document.getElementById("<%=lblmodifyMiddlename.ClientID %>");

               var lblModifyMobileNumber = document.getElementById("<%=lblModifyMobileNumber.ClientID %>");
               var lblModifyAddress = document.getElementById("<%=lblModifyAddress.ClientID %>");
               var lblModifyEmail = document.getElementById("<%=lblModifyEmail.ClientID %>");

               var firstname = txtModifyFirstName.value;
               var Middlename = txtModifyMiddleName.value;
               var LastName = txtModifylastName.value;
               var Address = txtmodifyAddress.value;
               var email = txtModifyEmail.value;
               var mobile = txtModifymobile.value;

               lblModifyFirstName.innerHTML = firstname;
               lblmodifyMiddlename.innerHTML = Middlename
               lblModifyLastName.innerHTML = LastName;
               lblModifyAddress.innerHTML = Address;
               lblModifyEmail.innerHTML = email;
               lblModifyMobileNumber.innerHTML = mobile;

               return false;
           }
       }
       function NameValidation() {
           var userid;
           var controlId = document.getElementById("<%=txtModifymobile.ClientID %>");
           userid = controlId.value;
           var val;
           val = /^[0-9]+$/;
           var digits = /\d(10)/;
           if (userid == "") {
               return ("Please Enter PhoneNo" + "\n");
           }
           if (!(userid.charAt(0) == "9" || userid.charAt(0) == "8" || userid.charAt(0) == "7")) {
               return ("Invalid Mobile Number ! " + "\n");

           }
           else if (userid.length < 10) {
               return ("Phone No should be 10 digits" + "\n");
           }
           else if (val.test(userid)) {
               return "";
           }


           else {
               return ("Phone No should be only in digits" + "\n");
           }
       }
       function EmailValidation() {
           var userid;
           var controlId = document.getElementById("<%=txtModifyFirstName.ClientID %>");
           userid = controlId.value;
           var val = /^[a-zA-Z ]+$/
           if (userid == "") {
               return ("Please Enter First Name" + "\n");
           }
           else if (val.test(userid)) {
               return "";

           }
           else {
               return ("First Name accepts only spaces and charcters" + "\n");
           }
       }
       function AddressValidation() {
           var userid;
           var controlId = document.getElementById("<%=txtmodifyAddress.ClientID %>");
           userid = controlId.value;
           var val = /^[a-zA-Z ]+$/
           if (userid == "") {
               return ("Please Enter Address" + "\n");
           }
           else {
               return "";
           }
       }
       function MiddleValidation() {
           var userid;
           var controlId = document.getElementById("<%=txtModifyMiddleName.ClientID %>");
           userid = controlId.value;
           var val = /^[a-zA-Z ]+$/
           if (userid == "") {
               return "";
           }
           else if (val.test(userid)) {
               return "";

           }
           else {
               return ("Middle Name accepts only spaces and charcters" + "\n");
           }
       }
       function EmailValidation_last() {
           var userid;
           var controlId = document.getElementById("<%=txtModifylastName.ClientID %>");
           userid = controlId.value;
           var val = /^[a-zA-Z ]+$/
           if (userid == "") {
               return ("Please Enter Last Name" + "\n");
           }
           else if (val.test(userid)) {
               return "";

           }
           else {
               return ("Last Name accepts only spaces and charcters" + "\n");
           }
       }
       function PhonenoValidation() {
           var userid;
           var controlId = document.getElementById("<%=txtModifyEmail.ClientID %>");
           userid = controlId.value;
           var val = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
           if (userid == "") {
               return "";
           }
           else if (val.test(userid)) {
               return "";
           }

           else {
               return ("Email should be in this form example: xyz@abc.com" + "\n");
           }
       }
    </script>
    <script>
        $(function () {
            $("#tabs-1").tabs();

        });


        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                //Dialog Code
                $("#tabs-1").tabs({
                    cookie: {
                        // store cookie for a day, without, it would be a session cookie
                        expires: 1
                    }
                });


            }
        }


        function clickre(objcta) {

            var btn = document.getElementById('<%=lnkatag1.ClientID%>');
            var hdntag = document.getElementById('<%=hdntag.ClientID%>');
            hdntag.value = objcta;

            btn.click();

        }
         
    </script>
    <style>
        #tabs-1
        {
            font-size: 16px;
            height: 10%;
        }
        
        .ui-widget-header
        {
            background: none;
            border: 1px solid #ffffff;
            color: #FFFFFF;
            font-weight: bold;
        }
        #tabs-1
        {
            padding: 0px;
            background: none;
            border-width: 0px;
        }
        #tabs-1 .ui-tabs-nav
        {
            padding-left: 0px;
            background: transparent;
            border-width: 0px 0px 1px 0px;
            -moz-border-radius: 0px;
            -webkit-border-radius: 0px;
            border-radius: 0px;
        }
    </style>
    <script type="text/javascript">


        function closeFreePlanPopup() {
            $find("mpeFreePlan").hide();
            return false;
        }

        function closerenewalPlanPopup() {
            $find("mpeRenewPlan").hide();
            return false;
        }


        function closePopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeConfirmation").hide();
            return false;
        }

        function closeRenewNowPopup() {
            $find("mpeRenewNow").hide();
            return false;
        }

        function closeRenewNowConfPopup() {
            $find("mpeRenewNowConf").hide();
            return false;

        }

        function closeMsgPopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeMsg").hide();
            return false;
        }
        function closeAddPopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeAdd").hide();
            return false;
        }
        function closeChangePopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeChange").hide();
            return false;
        }

        function closeChangePopupPayTerm() {
            $find("mpeChangePayTerm").hide();
            return false;
        }

        function closeChangeConfirmPopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeChangeConfirmation").hide();
            return false;
        }


        function closeServicePopup() {

            $find("mpeService").hide();
            return false;
        }
        function closeAutoRenewalPopup() {

            $find("mpeautoRenewPlan").hide();
            return false;
        }

        function closeServiceInfoPopup() {
            $find("mpeServiceInfo").hide();
            return false;
        }
        function closeFinalConfPopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeFinalConf").hide();
            return false;
        }
        /*function searchPlanData(sender, e) {
        document.getElementById("<%= btnsearchplan.ClientID %>").click();
        }*/
        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
            /*if ($("#<%= radPlanAD.ClientID %>").is(':checked')) {
            bindAutocomplete("AD");
            } else {
            bindAutocomplete("AL");
            }*/
        }

        function closeMsgPopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeMsg").hide();
            //$find("mpeFreePlan").show();
            return false;
        }

        function closeMsgPopupALL() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeFOCMsg").hide();
            $find("mpeFreePlan").hide();
            return false;
        }

        function closeModifyCustPopup() {
            $find("mpeModifyCust").hide();
            return false;

        }
        function closeModifyCustConfirmPopup() {
            $find("mpeModifyConfirm").hide();
            $find("mpeModifyCust").show();
            return false;
        }

        
    </script>
    <script type="text/javascript">
        function CheckOne(obj) {
            var grid = obj.parentNode.parentNode.parentNode;
            var inputs = grid.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                        inputs[i].checked = false;
                    }
                }
            }
        }





        function Chkcount(id) {

            var TottalRows = 0;
            var TotalTrueRows = 0;
            for (k = 0; k < 3; k++) {

                if (k == 0) {
                    var grid = document.getElementById("<%= grdBasicPlanDetails.ClientID %>");
                }
                else if (k == 1) {
                    var grid = document.getElementById("<%= grdAddOnPlan.ClientID %>");
                }
                else if (k == 2) {
                    var grid = document.getElementById("<%= grdCarte.ClientID %>");
                }

                if (grid == null) {
                    checkcount = 1;
                    continue;
                }


                if (grid.rows.length > 0) {
                    for (i = 1; i < grid.rows.length; i++) {
                        cell = grid.rows[i].cells[6];
                        for (j = 0; j < cell.childNodes.length; j++) {
                            if (cell.childNodes[j].type == "checkbox") {
                                TottalRows = TottalRows + 1;
                                if (cell.childNodes[j].checked) {
                                    TotalTrueRows = TotalTrueRows + 1;
                                }
                            }
                        }
                    }
                }
            }

            if (TotalTrueRows > 0) {
                document.getElementById("<%= btnRenSubmit.ClientID %>").style.display = "";

            }
            else {
                document.getElementById("<%= btnRenSubmit.ClientID %>").style.display = "none";
            }

            if (TottalRows == TotalTrueRows) {
                document.getElementById("<%= Challrenew.ClientID %>").checked = true;


            }
            else {
                document.getElementById("<%= Challrenew.ClientID %>").checked = false;

            }
        }

        function FunChkDisable(id) {

            if (id.checked == true) {
                document.getElementById("<%= btnRenSubmit.ClientID %>").style.display = "";
                for (k = 0; k < 3; k++) {
                    if (k == 0) {
                        var gv = document.getElementById("<%= grdBasicPlanDetails.ClientID %>");
                    }
                    else if (k == 1) {
                        var gv = document.getElementById("<%= grdAddOnPlan.ClientID %>");
                    }
                    else if (k == 2) {
                        var gv = document.getElementById("<%= grdCarte.ClientID %>");
                    }

                    if (gv == null) {
                        continue;
                    }

                    var chkrow;
                    if (gv.rows.length > 0) {
                        var chk = gv.rows[0].cells[6];
                        for (i = 0; i < gv.rows.length; i++) {
                            cell = gv.rows[i].cells[6];

                            for (j = 0; j < cell.childNodes.length; j++) {
                                //if childNode type is CheckBox                 
                                if (cell.childNodes[j].type == "checkbox") {
                                    cell.childNodes[j].checked = true;
                                }
                            }
                        }
                    }
                }
            }
            else {

                document.getElementById("<%= btnRenSubmit.ClientID %>").style.display = "none";
                for (k = 0; k < 3; k++) {
                    if (k == 0) {
                        var gv = document.getElementById("<%= grdBasicPlanDetails.ClientID %>");
                    }
                    else if (k == 1) {
                        var gv = document.getElementById("<%= grdAddOnPlan.ClientID %>");
                    }
                    else if (k == 2) {
                        var gv = document.getElementById("<%= grdCarte.ClientID %>");
                    }

                    if (gv == null) {
                        continue;
                    }

                    var chkrow;
                    if (gv.rows.length > 0) {
                        var chk = gv.rows[0].cells[6];
                        for (i = 0; i < gv.rows.length; i++) {
                            cell = gv.rows[i].cells[6];

                            for (j = 0; j < cell.childNodes.length; j++) {
                                //if childNode type is CheckBox                 
                                if (cell.childNodes[j].type == "checkbox") {
                                    cell.childNodes[j].checked = false;
                                }
                            }
                        }
                    }
                }

            }
        }

        function ChkcountAuto(id) {

            var TottalRows = 0;
            var TotalTrueRows = 0;

            var grid = document.getElementById("<%= grdAutoRenewal.ClientID %>");

            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[1];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            TottalRows = TottalRows + 1;
                            if (cell.childNodes[j].checked) {
                                TotalTrueRows = TotalTrueRows + 1;
                            }
                        }
                    }
                }

            }



            if (TottalRows == TotalTrueRows) {
                document.getElementById("<%= choAutorenewAll.ClientID %>").checked = true;


            }
            else {
                document.getElementById("<%= choAutorenewAll.ClientID %>").checked = false;

            }
        }

        function FunChkDisableauto(id) {

            if (id.checked == true) {

                var gv = document.getElementById("<%= grdAutoRenewal.ClientID %>");


                var chkrow;
                if (gv.rows.length > 0) {
                    var chk = gv.rows[0].cells[1];
                    for (i = 0; i < gv.rows.length; i++) {
                        cell = gv.rows[i].cells[1];

                        for (j = 0; j < cell.childNodes.length; j++) {
                            if (cell.childNodes[j].type == "checkbox") {
                                cell.childNodes[j].checked = true;
                            }
                        }
                    }
                }
            }
            else {

                var gv = document.getElementById("<%= grdAutoRenewal.ClientID %>");

                var chkrow;
                if (gv.rows.length > 0) {
                    var chk = gv.rows[0].cells[1];
                    for (i = 0; i < gv.rows.length; i++) {
                        cell = gv.rows[i].cells[1];

                        for (j = 0; j < cell.childNodes.length; j++) {
                            if (cell.childNodes[j].type == "checkbox") {
                                cell.childNodes[j].checked = false;
                            }
                        }
                    }
                }
            }
        }





    </script>
    <style type="text/css">
        .topHead
        {
            background: #E5E5E5;
            width: 96.5%;
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
        
        .tabody
        {
            padding: 10px;
            border: 1px solid #094791;
            background: #ffffff;
            width: 96.5%;
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
            height: 150px;
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
        .ui-autocomplete.ui-widget
        {
            font-family: Verdana,Arial,sans-serif;
            font-size: 10px;
        }
        
        
        .tabs
        {
            width: 100%;
            display: inline-block;
        }
        
        /*----- Tab Links -----*/
        /* Clearfix */
        .tab-links:after
        {
            display: block;
            clear: both;
            content: '';
        }
        
        .tab-links li
        {
            margin: 0px 5px;
            float: left;
            list-style: none;
        }
        
        .tab-links a
        {
            padding: 9px 15px;
            display: inline-block;
            border-radius: 3px 3px 0px 0px;
            background: red;
            font-size: 16px;
            font-weight: 600;
            color: #4c4c4c;
            transition: all linear 0.15s;
        }
        
        .tab-links a:hover
        {
            background: #a7cce5;
            text-decoration: none;
        }
        
        li.active a, li.active a:hover
        {
            background: #fff;
            color: #4c4c4c;
        }
        
        /*----- Content of Tabs -----*/
        .tab-content
        {
            padding: 15px;
            border-radius: 3px;
            box-shadow: -1px 1px 1px rgba(0,0,0,0.15);
            background: #fff;
        }
        
        .tab
        {
            display: none;
        }
        
        .tab.active
        {
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:HiddenField ID="HidValue" runat="server" />
    <asp:HiddenField ID="hdnBasicPoidAddResponse" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="maindive">
                <div class="griddiv">
                    <div class="delInfo topHead">
                        <table class='delInfoContent'>
                            <tr>
                                <td align="right" width="130px">
                                    Distributor Name:
                                </td>
                                <td align="left" width="200px">
                                    <asp:Label ID="lblDistName" runat="server"></asp:Label>
                                </td>
                                <td align="right" width="50px">
                                    User:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbluser" runat="server"></asp:Label>
                                </td>
                                <td align="right" width="130px">
                                    Available Balance:
                                </td>
                                <td align="left" width="67px">
                                    <asp:Label ID="lblAvailBal" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table width="100%">
                        <tr>
                            <td align="left" valign="top" id="divCustHolder" runat="server">
                                <div class="delInfo" runat="server" id="divSearchHolder">
                                    <table>
                                        <tr>
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
                                                    OnClick="btnSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Label runat="server" ID="lblSearchResponse" ForeColor="Red"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvSearchParam" runat="server" ValidationGroup="searchValidation"
                                    Display="Dynamic" ErrorMessage="Enter VCID or Account No" ControlToValidate="txtSearchParam"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <div id="dynamiclink" runat="server">
                        <div id="tabs-1">
                            <ul id="ulFiles" runat="server">
                                <li><a href="#Detailss" id="Details" style="display: none;" runat="server">Detail</a></li>
                                <li>
                                    <asp:LinkButton ID="lnkDetail" Width="125px" Height="12px" runat="server" Font-Bold="true"
                                        Text="Customer Details" OnClientClick="clickre('lnkDetail')" OnClick="lnkatag_Click"></asp:LinkButton>
                                </li>
                            </ul>
                            <asp:LinkButton ID="lnkatag1" runat="server" OnClick="lnkatag_Click"></asp:LinkButton>
                            <asp:HiddenField ID="hdntag" runat="server" />
                            <div class="tabody">
                                <div id="Detailss" runat="server">
                                    <table width="100%" id="tbl" runat="server">
                                        <tr id="trCustNo" runat="server">
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustNumber" Text="Customer A/C No."></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCustNo" Text=""></asp:Label>
                                            </td>
                                        </tr>

                                          <tr id="trcust1Name" runat="server">
                                            <td align="left">
                                                <asp:Label runat="server" ID="Label12" Text="Customer Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustName" Text=""></asp:Label>
                                            </td>
                                        </tr>

                                        <tr id="trVC" runat="server">
                                            <td align="left">
                                                <asp:Label runat="server" ID="Label21" Text="VC/Mac ID"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label72" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblVCID" Text="012548778742255"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="labelvc" runat="server" Text="STB/Mac ID"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                                                &nbsp;
                                                <asp:Label ID="lblStbNo" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnStbNo" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnVCNo" runat="server" Value="" />
                                            </td>
                                        </tr>
                                      
                                        <tr id="trCustMobileNo" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label runat="server" ID="Label3" Text="Customer Mobile"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label31" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lbltxtmobno" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trCustAdd" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label runat="server" ID="Label5" Text="Customer Address"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustAddr" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trvcdet" runat="server">
                                            <td align="left" width="130px">
                                                 <asp:Label runat="server" ID="Label73" Text="TV Details"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label74" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                              <asp:GridView ID="GridVC" CssClass="Grid" runat="server" AutoGenerateColumns="true" Height="100%" Width="60%">
                                                             
                                                 </asp:GridView>
                                            </td>
                                        </tr>
                                         <tr id="trModify" runat="server">
                                            <td align="left" width="130px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="left">
                                            <asp:Button ID="btnModifyCust" runat="server" CssClass="button" Text="Modify Customer Detail" Width="160px"
                                    OnClick="btnModifyCust_Click" />
                                            </td>
                                        </tr>
                                        <tr id="trServiceStatus" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label runat="server" ID="lblServiceStat" Text="Action Required"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lbactive" runat="server" Text="Inactive" Visible="false"></asp:Label>
                                                <asp:Label ID="lbdeactive" runat="server" Text="Active" Visible="false"></asp:Label>
                                                <asp:Button ID="btnAct" runat="server" Visible="false" Text="Activate" ToolTip="Activate this service"
                                                    OnClick="btnAct_Click" />
                                                <asp:Button ID="btnDeact" runat="server" Visible="false" Text="Deactivate" ToolTip="Deactivate this service"
                                                    OnClick="btnDeact_Click" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnRetract" runat="server" Text="Retrack" OnClick="BtnRetract_Click" />
                                                <%-- <asp:LinkButton ID="btnAct" runat="server" Visible="false" ToolTip="Activate this service"
                                                OnClick="btnAct_Click" Font-Underline="true">Activate</asp:LinkButton>
                                            <asp:LinkButton ID="btnDeact" runat="server" Visible="false" ToolTip="Deactivate this service"
                                                OnClick="btnDeact_Click" Font-Underline="true">Deactivate</asp:LinkButton>--%>
                                                <%--    <asp:LinkButton ID="BtnRetract" runat="server" Text="Retrack" 
                                                onclick="BtnRetract_Click" Font-Underline="true"></asp:LinkButton>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="planDetail" runat="server" visible="false">
                                    <asp:Panel runat="server" ID="pnlGridHolder" Visible="false">
                                        <div class="delInfo">
                                            <table style="border-collapse: collapse" width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <b>
                                                            <asp:Label runat="server" ID="lblPlanHeading" Text="Plan Details :"></asp:Label>
                                                        </b>
                                                    </td>
                                                    <td align="right">
                                                    <asp:Button ID="btnAutoRenewal" runat="server" Text="Auto Renew" 
                                                            OnClick="btnAutoRenewal_Click" />&nbsp;
                                                             <asp:Button ID="btnOpenAddPopup" runat="server" Text="Add Plan" OnClick="btnOpenAddPopup_Click" />
                                                        &nbsp;<asp:Button ID="btnOpenRenewNowPopup" runat="server" Text="Renew Now" Visible="false"
                                                            OnClick="btnOpenRenewNowPopup_Click" />&nbsp;&nbsp;
                                                        <asp:CheckBox ID="Challrenew" runat="server" onclick="FunChkDisable(this);" />
                                                        <asp:Label runat="server" ID="allrenewal" Text="All Renewal"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <hr />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2" width="95%">
                                                        <b>
                                                            <asp:Label ID="lblBasicPlan" runat="server" Text="Basic Plan"></asp:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="100%" colspan="2">
                                                        <div class="plan_scroller">
                                                            <asp:GridView ID="grdBasicPlanDetails" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                                                                OnRowDataBound="grdBasicPlanDetails_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="625" />
                                                                    <asp:BoundField HeaderText="MRP" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                    <asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-Width="100"
                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField HeaderText="Expiry" DataField="EXPIRY" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Left" />
                                                                     
                                                                    <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="100" />
                                                                     <asp:TemplateField HeaderText="Auto Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                        <ItemTemplate>
                                                                        <asp:Label ID="lblAutorenew" Text="Off" runat="server"></asp:Label>
                                                                            <asp:CheckBox ID="cbBAutorenew" Enabled="false" style="visibility:hidden" runat="server" AutoPostBack="true" onclick="CheckOne(this)"
                                                                                OnCheckedChanged="cbBAutorenew_Clicked" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="cbBasicrenew" runat="server" AutoPostBack="false" onclick="Chkcount(this)" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-Width="180">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBRenew" Font-Underline="true" runat="server" Text="Renew"
                                                                                OnClick="lnkBRenew_Click" CommandName="BRenew"></asp:LinkButton>
                                                                            <asp:LinkButton ID="lnkBCancel" Font-Underline="true" runat="server" Visible="false"
                                                                                Text="Cancel" OnClick="lnkBCancel_Click" CommandName="bCancel"></asp:LinkButton>
                                                                            <asp:LinkButton ID="lnkBChange" Font-Underline="true" runat="server" Text="Change"
                                                                                OnClick="lnkBChange_Click" CommandName="BChange"></asp:LinkButton>
                                                                            <asp:LinkButton ID="lnkAddFOCPack" Font-Underline="true" runat="server" Text="FOC Pack"
                                                                                OnClick="lnkAddFOC_Click" CommandName="AddFOCPack"></asp:LinkButton>
                                                                            <asp:HiddenField ID="hdnBasicPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicPlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicCustPrice" runat="server" Value='<%# Eval("CUST_PRICE").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicLcoPrice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicActivation" runat="server" Value='<%# Eval("ACTIVATION").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicExpiry" runat="server" Value='<%# Eval("EXPIRY").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicPackageId" runat="server" Value='<%# Eval("PACKAGE_ID").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicPurchasePoid" runat="server" Value='<%# Eval("PURCHASE_POID").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicPlanStatus" runat="server" Value='<%# Eval("PLAN_STATUS").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicRenewFlag" runat="server" Value='<%# Eval("PLAN_RENEWFLAG").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicChangeFlag" runat="server" Value='<%# Eval("PLAN_CHANGEFLAG").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicActionFlag" runat="server" Value='<%# Eval("PLAN_ACTIONFLAG").ToString()%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Grace" DataField="GRACE" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Left" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <b>
                                                            <asp:Label ID="lblAddonPlan" runat="server" Text="Addon Plans"></asp:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <cc1:accordion id="AddonAccordion" runat="server" selectedindex="-1" headercssclass="accordionHeader"
                                                            headerselectedcssclass="accordionHeaderSelected" contentcssclass="accordionContent"
                                                            fadetransitions="true" suppressheaderpostbacks="true" transitionduration="250"
                                                            framespersecond="40" requireopenedpane="false" autosize="None">
                                                            <Panes>
                                                                <cc1:AccordionPane ID="AddonAccordionPane" runat="server">
                                                                    <Header>
                                                                        <a href="#" class="href" style="color: White;">Addon Plans</a></Header>
                                                                    <Content>
                                                                        <div class="plan_scroller">
                                                                            <asp:GridView ID="grdAddOnPlan" CssClass="Grid" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                                OnRowDataBound="grdAddOnPlan_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="380px"
                                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                                    <asp:BoundField HeaderText="MRP" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="left"
                                                                                        ItemStyle-Width="55" />
                                                                                    <asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="90" />
                                                                                    <asp:BoundField HeaderText="Expiry" DataField="EXPIRY" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />
                                                                                        
                                                                                    <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="60" />
                                                                                    <%--   <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label30" runat="server" Text='<%# PlanStatusText(Eval("PLAN_STATUS")) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField> --%>
                                                                                    <asp:TemplateField HeaderText="Auto Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                        <asp:Label ID="lblAutorenew" Text="Off" runat="server"></asp:Label>
                                                                                            <asp:CheckBox ID="cbAddonAutorenew" Enabled="false" style="visibility:hidden" runat="server" AutoPostBack="true" onclick="CheckOne(this)"
                                                                                                OnCheckedChanged="cbAddonAutorenew_Clicked" />
                                                                                            <%--<asp:Label ID="Label30" runat="server" Text='<%# PlanStatusText(Eval("PLAN_STATUS")) %>'></asp:Label>--%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="cbAddonrenew" runat="server" AutoPostBack="false" onclick="Chkcount(this)" />
                                                                                            <%--<asp:Label ID="Label30" runat="server" Text='<%# PlanStatusText(Eval("PLAN_STATUS")) %>'></asp:Label>--%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkADRenew" runat="server" Font-Underline="true" Text="Renew"
                                                                                                OnClick="lnkADRenew_Click" CommandName="adRenew"></asp:LinkButton>
                                                                                            <asp:LinkButton ID="lnkADCancel" runat="server" Font-Underline="true" Text="Cancel"
                                                                                                OnClick="lnkADCancel_Click" CommandName="adCancel"></asp:LinkButton>
                                                                                            <asp:LinkButton ID="lnkADChange" runat="server" Font-Underline="true" Text="Change"
                                                                                                OnClick="lnkADChange_Click" CommandName="adChange"></asp:LinkButton>
                                                                                            <%--     <asp:HiddenField ID="hdnADPlanId" runat="server" Value='<%# Eval("PLAN_ID").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnADPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                                            <%--     <asp:HiddenField ID="hdnADPlanType" runat="server" Value='<%# Eval("PLAN_TYPE").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnADPlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                                                            <%--     <asp:HiddenField ID="hdnADProductPoid" runat="server" Value='<%# Eval("PRODUCT_POID").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnADCustPrice" runat="server" Value='<%# Eval("CUST_PRICE").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADLcoPrice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADActivation" runat="server" Value='<%# Eval("ACTIVATION").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADExpiry" runat="server" Value='<%# Eval("EXPIRY").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADPackageId" runat="server" Value='<%# Eval("PACKAGE_ID").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADPurchasePoid" runat="server" Value='<%# Eval("PURCHASE_POID").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADPlanStatus" runat="server" Value='<%# Eval("PLAN_STATUS").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADPlanRenewFlag" runat="server" Value='<%# Eval("PLAN_RENEWFLAG").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADPlanChangeFlag" runat="server" Value='<%# Eval("PLAN_CHANGEFLAG").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADPlanActionFlag" runat="server" Value='<%# Eval("PLAN_ACTIONFLAG").ToString()%>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Grace" DataField="GRACE" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </Content>
                                                                </cc1:AccordionPane>
                                                            </Panes>
                                                        </cc1:accordion>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <b>
                                                            <asp:Label ID="lblAlacartePlan" runat="server" Text="A-La-Carte" ForeColor="White"></asp:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <cc1:accordion id="AlacarteAccordion" runat="server" selectedindex="-1" headercssclass="accordionHeader"
                                                            headerselectedcssclass="accordionHeaderSelected" contentcssclass="accordionContent"
                                                            fadetransitions="true" suppressheaderpostbacks="true" transitionduration="250"
                                                            framespersecond="40" requireopenedpane="false" autosize="None">
                                                            <Panes>
                                                                <cc1:AccordionPane ID="AlacarteAccordionPane" runat="server">
                                                                    <Header>
                                                                        <a href="#" class="href" style="color: White;">A-La-Carte</a></Header>
                                                                    <Content>
                                                                        <div class="plan_scroller">
                                                                            <asp:GridView ID="grdCarte" CssClass="Grid" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                                OnRowDataBound="grdCarte_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="380px"
                                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                                    <asp:BoundField HeaderText="MRP" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="left"
                                                                                        ItemStyle-Width="55" />
                                                                                    <asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="90" />
                                                                                    <asp:BoundField HeaderText="Expiry" DataField="EXPIRY" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />
                                                                                        
                                                                                    <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="60" />
                                                                                    <%--<asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label30" runat="server" Text='<%# PlanStatusText(Eval("PLAN_STATUS")) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                                                                    <asp:TemplateField HeaderText="Auto Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                        <asp:Label ID="lblAutorenew" Text="Off" runat="server"></asp:Label>
                                                                                            <asp:CheckBox ID="cbAlaAutorenew" style="visibility:hidden" Enabled="false" runat="server" onclick="CheckOne(this)" AutoPostBack="true"
                                                                                                OnCheckedChanged="cbAlaAutorenew_Clicked" />
                                                                                            <%--<asp:Label ID="Label30" runat="server" Text='<%# PlanStatusText(Eval("PLAN_STATUS")) %>'></asp:Label>--%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkalRenew" runat="server" AutoPostBack="false" onclick="Chkcount(this)" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkALRenew" Font-Underline="true" runat="server" Text="Renew"
                                                                                                OnClick="lnkALRenew_Click" CommandName="alRenew"></asp:LinkButton>
                                                                                            <asp:LinkButton ID="lnkALCancel" Font-Underline="true" runat="server" Text="Cancel"
                                                                                                OnClick="lnkALCancel_Click" CommandName="alCancel"></asp:LinkButton>
                                                                                            <asp:LinkButton ID="lnkALChange" Font-Underline="true" runat="server" Text="Change"
                                                                                                OnClick="lnkALChange_Click" CommandName="alChange"></asp:LinkButton>
                                                                                            <%--   <asp:HiddenField ID="hdnALPlanId" runat="server" Value='<%# Eval("PLAN_ID").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnALPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                                            <%-- <asp:HiddenField ID="hdnALPlanType" runat="server" Value='<%# Eval("PLAN_TYPE").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnALPlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                                                            <%--  <asp:HiddenField ID="hdnALProductPoid" runat="server" Value='<%# Eval("PRODUCT_POID").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnALCustPrice" runat="server" Value='<%# Eval("CUST_PRICE").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALLcoPrice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALActivation" runat="server" Value='<%# Eval("ACTIVATION").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALExpiry" runat="server" Value='<%# Eval("EXPIRY").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALPackageId" runat="server" Value='<%# Eval("PACKAGE_ID").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALPurchasePoid" runat="server" Value='<%# Eval("PURCHASE_POID").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALPlanStatus" runat="server" Value='<%# Eval("PLAN_STATUS").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALPlanRenewFlag" runat="server" Value='<%# Eval("PLAN_RENEWFLAG").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALPlanChangeFlag" runat="server" Value='<%# Eval("PLAN_CHANGEFLAG").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALPlanActionFlag" runat="server" Value='<%# Eval("PLAN_ACTIONFLAG").ToString()%>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Grace" DataField="GRACE" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </Content>
                                                                </cc1:AccordionPane>
                                                            </Panes>
                                                        </cc1:accordion>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnRenSubmit" runat="server" Text="submit" OnClick="btnRenSubmit_Click"
                                                    Visible="true" Style="display: none;" />
                                                <asp:Button ID="btnReset" runat="server" Visible="false" Text="Reset" OnClick="btnReset_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- ---------------------------------------------------ACTION POPUP-------------------------------------------------- --%>
                    <cc1:modalpopupextender id="pop" runat="server" behaviorid="mpeConfirmation" targetcontrolid="hdnPop"
                        popupcontrolid="pnlConfirmation">
                    </cc1:modalpopupextender>
                    <asp:HiddenField ID="hdnPop" runat="server" />
                    <asp:Panel ID="pnlConfirmation" runat="server" CssClass="Popup" Style="width: 430px; display: none;
                        height: 340px;">
                        <%-- display: none; --%>
                        <asp:Image ID="imgClose" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                            margin-top: -15px; margin-right: -15px;" onclick="closePopup();" ImageUrl="~/Images/closebtn.png" />
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
                            <table width="85%">
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Label ID="lblPopupText1" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Label ID="lblPopupText2" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table width="85%">
                                <tr>
                                    <td align="left" width="120px">
                                        <b>
                                            <asp:Label ID="Label16" runat="server" Text="Customer No."></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopupCustNo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label18" runat="server" Text="Plan Name"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label19" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopupPlanName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label22" runat="server" Text="Amount"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopupAmount" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trAct">
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label24" runat="server" Text="Activation"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label25" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopupFrom" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trExp">
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label26" runat="server" Text="Expiry"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopupTo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trPopupCancelRefund" visible="false">
                                    <%-- visible="false" --%>
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label40" runat="server" Text="Remaining Days"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label41" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopupDaysleft" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trPopupCancelDaysLeft" visible="false">
                                    <%-- visible="false" --%>
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label42" runat="server" Text="Refund Amount"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label43" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopupRefund" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trPopupCancelReason" visible="false">
                                    <%-- visible="false" --%>
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label38" runat="server" Text="Reason"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label39" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlPopupReason" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr runat="server" id="trPopupAutorenew" visible="false">
                                    <%-- visible="false" --%>
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label44" runat="server" Text="AutoRenew"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label45" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopupAutorenew" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <input type="button" class="button" value="Cancel" onclick="closePopup();" />
                                        &nbsp;
                                        <asp:Button ID="btnConfirm" class="button" runat="server" Text="Confirm" OnClick="btnConfirm_Click" />
                                        <asp:HiddenField ID="hdnPopupAction" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnPopupType" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnPopupAutoRenew" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                    <%-- ---------------------------------------------------ADDON POPUP-------------------------------------------------- --%>
                    <cc1:modalpopupextender id="popAdd" runat="server" behaviorid="mpeAdd" targetcontrolid="hdnPop3"
                        popupcontrolid="pnlAdd">
                    </cc1:modalpopupextender>
                    <asp:HiddenField ID="hdnPop3" runat="server" />
                    <asp:Panel ID="pnlAdd" runat="server" CssClass="Popup" Style="width: 600px; height: 300px; display: none;">
                        <%-- display: none; --%>
                        <asp:Image ID="imgClose3" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                            margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closeAddPopup();" />
                        <center>
                            <br />
                            <table width="100%">
                                <tr>
                                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                        &nbsp;&nbsp;&nbsp;Add New Plan
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
                                        <asp:RadioButtonList ID="rdbplanpayterm" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="rdbplanpayterm_SelectedIndexChanged">
                                            <asp:ListItem Text="1 Month" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="3 Month" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="6 Month" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="12 Month" Value="12"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="90px">
                                        <asp:Label ID="Label28" Font-Bold="true" runat="server" Text="Type"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label29" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left" runat="server" id="tdRadioHolder">
                                        <%--<input id="radPlanAD" runat="server" name="RadPlanType" value="AD" checked="checked"
                                    onclick="bindAutocomplete('AD')" />
                                <input id="radPlanAL" runat="server" name="RadPlanType" value="AL" onclick="bindAutocomplete('AL')" />--%>
                                        <asp:RadioButton ID="radPlanBasic" runat="server" GroupName="RadPlanType" Text="Basic"
                                            AutoPostBack="true" OnCheckedChanged="radPlanBasic_CheckedChanged" />
                                        <asp:RadioButton ID="radPlanAD" runat="server" GroupName="RadPlanType" Text="Addon"
                                            OnCheckedChanged="radPlanAD_CheckedChanged" AutoPostBack="true" />
                                        <asp:RadioButton ID="radPlanAL" runat="server" GroupName="RadPlanType" Text="A-La-Carte"
                                            OnCheckedChanged="radPlanAL_CheckedChanged" AutoPostBack="true" />
                                        <%--<asp:RadioButton ID="radPlanAD" runat="server" GroupName="RadPlanType" AutoPostBack="true"
                                    Text="Addon" onclick="bindAutocomplete()" OnCheckedChanged="radPlanAD_CheckedChanged" />
                                <asp:RadioButton ID="radPlanAL" runat="server" GroupName="RadPlanType" AutoPostBack="true"
                                    Text="A-La-Carte" onclick="bindAutocomplete()" OnCheckedChanged="radPlanAL_CheckedChanged" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" width="100px">
                                        <asp:Label ID="Label7" Font-Bold="true" runat="server" Text="Plan"></asp:Label><asp:Label
                                            ID="Label15" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label9" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        &nbsp;&nbsp;<%--<asp:TextBox ID="txtPlanName" runat="server" AutoComplete="off"></asp:TextBox>--%><asp:DropDownList
                                            ID="ddlPlanChan" runat="server" OnSelectedIndexChanged="ddlPlanChan_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                        <br />
                                        <%--<asp:RequiredFieldValidator ID="rfvPlanVal" ValidationGroup="AddPopup" runat="server"
                                    Display="Dynamic" ControlToValidate="txtPlanName" ErrorMessage="Enter Plan"></asp:RequiredFieldValidator>--%>
                                        <%--<cc1:AutoCompleteExtender ID="AutoCompleteExtender1" BehaviorID="AutoCompleteExtender1"
                                    runat="server" CompletionInterval="100" CompletionListCssClass="autocomplete"
                                    OnClientItemSelected="searchPlanData" CompletionListHighlightedItemCssClass="autocompleteItemHover"
                                    CompletionListItemCssClass="autocompleteItem" CompletionSetCount="3" EnableCaching="true"
                                    FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchOperators"
                                    TargetControlID="txtPlanName" CompletionListElementID="autocompleteDiv">
                                </cc1:AutoCompleteExtender>
                                <div id="autocompleteDiv">--%>
                </div>
                <asp:HiddenField ID="hfplan" runat="server" />
                </td> </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Button runat="server" ID="btnsearchplan" Style="display: none;" Text="Search"
                            OnClick="btnsearchplan_Click" />
                        <%--ValidationGroup="AddPopup"--%>
                    </td>
                </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                            &nbsp;&nbsp;&nbsp;Plan Details
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
                        <td align="left" valign="top" width="100px">
                            <asp:Label ID="Label8" Font-Bold="true" runat="server" Text="Plan Name"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left" width="220px">
                            &nbsp;&nbsp;<asp:Label ID="lblplanname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label10" Font-Bold="true" runat="server" Text="MRP"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left" width="220px">
                            &nbsp;&nbsp;<asp:Label ID="lblplanamt" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trAddplanAutorenew" runat="server">
                        <td align="left">
                            <asp:Label ID="Label46" Font-Bold="true" runat="server" Text="AutoRenew"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label47" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left" width="220px">
                            &nbsp;&nbsp;<asp:CheckBox ID="cbAddPlanAutorenew" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lblresponse" ForeColor="RED" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="btnAddPlan" runat="server" Width="60px" Text="Add" OnClick="btnAddPlan_Click"
                                CommandName="add" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button2" runat="server" Width="60px" Text="Reset" OnClick="Button2_Click" />
                            &nbsp;&nbsp;&nbsp;
                            <input type="button" value="Cancel" class="button" onclick="closeAddPopup();" />
                            <%--<asp:Button ID="btnCloseAdd" runat="server" Width="60px" Text="Cancel" 
                                    onclick="btnCloseAdd_Click"/> --%>
                        </td>
                    </tr>
                </table>
                </center> </asp:Panel>
                <%-- ---------------------------------------------------RENEW NOW POPUP-------------------------------------------------- --%>
                <cc1:modalpopupextender id="PopUpRenewNow" runat="server" behaviorid="mpeRenewNow"
                    targetcontrolid="hdnRenewNow" popupcontrolid="pnlRenewNow">
                </cc1:modalpopupextender>
                <asp:HiddenField ID="hdnRenewNow" runat="server" />
                <asp:Panel ID="pnlRenewNow" runat="server" CssClass="Popup" Style="width: 400px; display: none;
                    height: 300px;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image4" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closeRenewNowPopup();" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                    &nbsp;&nbsp;&nbsp;Renew Now Plan
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
                                <td align="left" width="90px">
                                    <asp:Label ID="Label62" Font-Bold="true" runat="server" Text="Type"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label65" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" runat="server" id="td2">
                                    <asp:RadioButtonList ID="rbtnRenewNow" RepeatDirection="Horizontal" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="rbtnRenewNow_SelectedIndexChanged">
                                        <asp:ListItem Text="Basic" Value="B" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Addon" Value="AD"></asp:ListItem>
                                        <asp:ListItem Text="A-La-Carte" Value="AL"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:UpdatePanel ID="updatepanelrenewnow" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdRenewNow" CssClass="Grid" runat="server" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="350px"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField HeaderText="MRP" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="left"
                                                        ItemStyle-Width="70" />
                                                    <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                        ItemStyle-Width="60" />
                                                    <asp:TemplateField HeaderText="Renew Now" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkAutoRenew" runat="server" onclick="CheckOne(this)" />
                                                            <asp:HiddenField ID="hdnPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                            <asp:HiddenField ID="hdnPlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                            <asp:HiddenField ID="hdnDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                            <asp:HiddenField ID="hdnCustPrice" runat="server" Value='<%# Eval("CUST_PRICE").ToString()%>' />
                                                            <asp:HiddenField ID="hdnLcoPrice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                            <asp:HiddenField ID="hdnActivation" runat="server" Value='<%# Eval("ACTIVATION").ToString()%>' />
                                                            <asp:HiddenField ID="hdnExpiry" runat="server" Value='<%# Eval("EXPIRY").ToString()%>' />
                                                            <asp:HiddenField ID="hdnPackageId" runat="server" Value='<%# Eval("PACKAGE_ID").ToString()%>' />
                                                            <asp:HiddenField ID="hdnPurchasePoid" runat="server" Value='<%# Eval("PURCHASE_POID").ToString()%>' />
                                                            <asp:HiddenField ID="hdnPlanStatus" runat="server" Value='<%# Eval("PLAN_STATUS").ToString()%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="90%">
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="btnRenewNow" Visible="false" runat="server" Width="60px" Text="Renew Now"
                                        CommandName="Renew" OnClick="btnRenewNow_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnCancelRenew" runat="server" Width="60px" Text="Cancel" Visible="false"
                                        OnClientClick="closeRenewNowPopup();" />
                                    <%--<asp:Button ID="btnCloseAdd" runat="server" Width="60px" Text="Cancel" 
                                    onclick="btnCloseAdd_Click"/> --%>
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <%-- ---------------------------------------------------Renew Now CONFIRMATION POPUP-------------------------------------------------- --%>
                <cc1:modalpopupextender id="PopUpRenewNowConfirm" runat="server" behaviorid="mpeRenewNowConf"
                    targetcontrolid="hdnRenewNowConfirm" popupcontrolid="pnlRenewNowConfirm">
                </cc1:modalpopupextender>
                <asp:HiddenField ID="hdnRenewNowConfirm" runat="server" />
                <asp:Panel ID="pnlRenewNowConfirm" runat="server" CssClass="Popup" Style="width: 430px; display: none;
                    height: 160px;">
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
                                    <input id="Button4" class="button" runat="server" type="button" value="No" style="width: 100px;"
                                        onclick="closeRenewNowConfPopup();" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <%-- ---------------------------------------------------CHANGE PAYTERM POPUP-------------------------------------------------- --%>
                <cc1:modalpopupextender id="popChangePayTerm" runat="server" behaviorid="mpeChangePayTerm"
                    targetcontrolid="hdnPopChangePayTerm" popupcontrolid="pnlChangePayTerm">
                </cc1:modalpopupextender>
                <asp:HiddenField ID="hdnPopChangePayTerm" runat="server" />
                <asp:Panel ID="pnlChangePayTerm" runat="server" CssClass="Popup" Style="width: 350px;
                    height: 300px; display: none;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image3" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closeChangePopupPayTerm();" />
                    <center>
                        <br />
                        <%--<table width="100%">
                        <tr>
                            <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                &nbsp;&nbsp;&nbsp;Change Plan Pay Term
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                    </table>--%>
                        <%--<table width="90%">
                        <tr>
                            <td align="left" valign="top" width="100px">
                                 <asp:RadioButtonList ID="rbtnPayterm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnPayterm_SelectedIndexChanged">
                                    <asp:ListItem Text="1 Month" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="3 Month" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="6 Month" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="12 Month" Value="12"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>--%>
                    </center>
                </asp:Panel>
                <%-- ---------------------------------------------------CHANGE POPUP-------------------------------------------------- --%>
                <cc1:modalpopupextender id="popchange" runat="server" behaviorid="mpeChange" targetcontrolid="hdnPopChange"
                    popupcontrolid="pnlChange">
                </cc1:modalpopupextender>
                <asp:HiddenField ID="hdnPopChange" runat="server" />
                <asp:Panel ID="pnlChange" runat="server" CssClass="Popup" Style="width: 500px; height: 300px;
                    display: none;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image1" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closeChangePopup();" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                    &nbsp;&nbsp;&nbsp;Change Plan
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <table width="95%">
                            <tr id="trpayterm" runat="server">
                                <td align="left" width="100px">
                                    <asp:Label ID="Label69" Font-Bold="true" runat="server" Text="Plan Payterm"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label68" runat="server" Text=":"></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbtnPayterm" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rbtnPayterm_SelectedIndexChanged">
                                        <asp:ListItem Text="1 Month" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="3 Month" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="6 Month" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="12 Month" Value="12"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" width="100px">
                                    <asp:Label ID="Label48" Font-Bold="true" runat="server" Text="Old Plan"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="Label51" runat="server" Text=":"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOldPlan" Font-Bold="true" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" width="100px">
                                    <asp:Label ID="Label49" Font-Bold="true" runat="server" Text="Plan"></asp:Label><asp:Label
                                        ID="Label50" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td width="5px">
                                    <asp:Label ID="LabelCol" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    &nbsp;&nbsp;<%--<asp:TextBox ID="txtPlanName" runat="server" AutoComplete="off"></asp:TextBox>--%><asp:DropDownList
                                        ID="ddlPlanChange" runat="server" OnSelectedIndexChanged="ddlPlanChange_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <br />
            </div>
            <asp:HiddenField ID="HiddenField2" runat="server" />
            </td> </tr> </table>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Plan Details
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>
            </table>
            <table width="95%">
                <tr>
                    <td align="left" valign="top" width="65px">
                        <asp:Label ID="Label52" Font-Bold="true" runat="server" Text="Plan Name"></asp:Label>
                    </td>
                    <td width="5px">
                        <asp:Label ID="Label53" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left" width="220px">
                        <asp:Label ID="lblChangePlan" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top" width="65px">
                        <asp:Label ID="Label54" Font-Bold="true" runat="server" Text="MRP"></asp:Label>
                    </td>
                    <td width="5px">
                        <asp:Label ID="Label55" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left" width="220px">
                        <asp:Label ID="lblChangePlanMRP" runat="server" Text=""></asp:Label>
                        <asp:HiddenField ID="Hidchangplan" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Label ID="Label56" ForeColor="RED" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnChangePlan" runat="server" Width="60px" Text="Change" OnClick="btnChangePlan_Click"
                            CommandName="Change" />
                        &nbsp;&nbsp;&nbsp;
                        <input type="button" value="Cancel" onclick="closeChangePopup();" class="button" />
                        <%--<asp:Button ID="btnCloseAdd" runat="server" Width="60px" Text="Cancel" 
                                    onclick="btnCloseAdd_Click"/> --%>
                    </td>
                </tr>
            </table>
            </center> </asp:Panel>
            <%-- ---------------------------------------------------CHANGECONFIRM POPUP-------------------------------------------------- --%>
            <cc1:modalpopupextender id="MPEConfirmation" runat="server" behaviorid="mpeChangeConfirmation"
                targetcontrolid="hdnChangeConfirmPop" popupcontrolid="pnlChangeConfirmation">
            </cc1:modalpopupextender>
            <asp:HiddenField ID="hdnChangeConfirmPop" runat="server" />
            <asp:Panel ID="pnlChangeConfirmation" runat="server" CssClass="Popup" Style="width: 430px; display: none;
                height: 340px;">
                <%-- display: none; --%>
                <asp:Image ID="Image2" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" onclick="closeChangeConfirmPopup();"
                    ImageUrl="~/Images/closebtn.png" />
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
                    <table width="85%">
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="Label57" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="Label58" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="85%">
                        <tr>
                            <td align="left" width="120px">
                                <b>
                                    <asp:Label ID="Label60" runat="server" Text="Old Plan"></asp:Label></b>
                            </td>
                            <td>
                                <asp:Label ID="Label61" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblConfirmOldPlan" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <b>
                                    <asp:Label ID="Label63" runat="server" Text="New Plan"></asp:Label></b>
                            </td>
                            <td>
                                <asp:Label ID="Label64" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblConfirmNewPlan" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <b>
                                    <asp:Label ID="Label66" runat="server" Text="Refund Amount"></asp:Label></b>
                            </td>
                            <td>
                                <asp:Label ID="Label67" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblConfirmRefundAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <b>
                                    <asp:Label ID="Label70" runat="server" Text="Pay Amount"></asp:Label></b>
                            </td>
                            <td>
                                <asp:Label ID="Label71" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="payamount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <input type="button" class="button" value="Cancel" onclick="closeChangeConfirmPopup();" />
                                &nbsp;
                                <asp:Button ID="btnChangePlanConfirmation" class="button" runat="server" Text="Confirm"
                                    OnClick="btnChangePlanConfirmation_Click" />
                                <asp:HiddenField ID="HiddenField3" runat="server" Value="" />
                                <asp:HiddenField ID="HiddenField4" runat="server" Value="" />
                                <asp:HiddenField ID="HiddenField5" runat="server" Value="" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%-- ---------------------------------------------------MESSAGE POPUP-------------------------------------------------- --%>
            <cc1:modalpopupextender id="popMsg" runat="server" behaviorid="mpeMsg" targetcontrolid="hdnPop2"
                popupcontrolid="pnlMessage">
            </cc1:modalpopupextender>
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
                                <asp:Button ID="btnRefreshForm" runat="server" CssClass="button" Text="OK" Visible="false"
                                    Width="100px" OnClick="btnRefreshForm_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%-- ---------------------------------------------------MESSAGE POPUP-------------------------------------------------- --%>
            <cc1:modalpopupextender id="popFOCMsg" runat="server" behaviorid="mpeFOCMsg" targetcontrolid="HiddenField6"
                popupcontrolid="pnlFOCMessgae">
            </cc1:modalpopupextender>
            <asp:HiddenField ID="HiddenField6" runat="server" />
            <asp:Panel ID="pnlFOCMessgae" runat="server" CssClass="Popup" Style="width: 430px;
                height: 160px; display: none;">
                <%-- display: none; --%>
                <asp:Image ID="Image8" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" onclick="closeFOCMsgPopupALL();" ImageUrl="~/Images/closebtn.png" />
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
                                <asp:Label ID="lblFOCMsg" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <input id="btnFOCMsgClose" class="button" runat="server" type="button" value="Close"
                                    style="width: 100px;" onclick="closeFOCMsgPopup();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%-- ---------------------------------------------------SERVICE ACT/DEACT POPUP-------------------------------------------------- --%>
            <cc1:modalpopupextender id="popService" runat="server" behaviorid="mpeService" targetcontrolid="hdnPop4"
                popupcontrolid="pnlService" cancelcontrolid="imgClose4">
            </cc1:modalpopupextender>
            <asp:HiddenField ID="hdnPop4" runat="server" />
            <asp:Panel ID="pnlService" runat="server" CssClass="Popup" Style="width: 430px; height: 180px;
                display: none;">
                <%-- display: none; --%>
                <asp:Image ID="imgClose4" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" onclick="closeServicePopup();" ImageUrl="~/Images/closebtn.png" />
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
                                <asp:Label ID="lblPopupServiceMsg1" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="lblPopupServiceMsg2" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:HiddenField ID="hdnPopupServiceFlag" runat="server" />
                                <asp:Button ID="btnPopupServiceConfirm" runat="server" CssClass="button" Width="100px"
                                    Text="Confirm" OnClick="btnPopupServiceConfirm_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <input type="button" class="button" value="Cancel" style="width: 100px;" onclick="closeServicePopup();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%-- ---------------------------------------------------SERVICE ACT/DEACT INFORMATION POPUP-------------------------------------------------- --%>
            <cc1:modalpopupextender id="popServiceInfo" runat="server" behaviorid="mpeServiceInfo"
                targetcontrolid="hdnPop5" popupcontrolid="pnlServiceInfo" cancelcontrolid="imgClose5">
            </cc1:modalpopupextender>
            <asp:HiddenField ID="hdnPop5" runat="server" />
            <asp:Panel ID="pnlServiceInfo" runat="server" CssClass="Popup" Style="width: 430px;
                height: 200px; display: none;">
                <%-- display: none; --%>
                <asp:Image ID="imgClose5" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" onclick="closeServiceInfoPopup();" ImageUrl="~/Images/closebtn.png" />
                <center>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                &nbsp;&nbsp;&nbsp;Change Service Status
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table width="70%">
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label30" runat="server" Text="STB Number"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label34" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblServiceInfoPopupStbNo" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label33" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label35" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblServiceInfoPopupStatus" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label36" runat="server" Text="Reason"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label37" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlServiceInfoPopupActReason" Visible="false" runat="server">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlServiceInfoPopupDactReason" Visible="false" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:HiddenField ID="hdnServiceInfoPopupFlag" runat="server" />
                                <asp:Button ID="btnServiceInfoSubmit" runat="server" CssClass="button" Width="100px"
                                    Text="Confirm" OnClick="btnServiceInfoSubmit_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <input type="button" class="button" value="Cancel" style="width: 100px;" onclick="closeServiceInfoPopup();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%-- ---------------------------------------------------CANCEL CONFIRMATION POPUP-------------------------------------------------- --%>
            <cc1:modalpopupextender id="popFinalConf" runat="server" behaviorid="mpeFinalConf"
                targetcontrolid="hdnPop7" popupcontrolid="pnlFinalConfirm">
            </cc1:modalpopupextender>
            <asp:HiddenField ID="hdnPop7" runat="server" />
            <asp:Panel ID="pnlFinalConfirm" runat="server" CssClass="Popup" Style="width: 430px; display: none;
                height: 160px;">
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
                                <asp:Label ID="lblPopupFinalConfMsg" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" id="trpopFinalConfAutorenewMsg">
                            <td align="center" colspan="3">
                                <asp:Label ID="lblPopupAutoRenewMsg" runat="server" Text="" Font-Bold="true" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="btnPopupFinalConfYes" runat="server" CssClass="button" Text="Yes"
                                    Width="100px" OnClick="btnPopupFinalConfYes_Click" />
                                &nbsp;&nbsp;
                                <input id="Button1" class="button" runat="server" type="button" value="No" style="width: 100px;"
                                    onclick="closeFinalConfPopup();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%------------------------Retract Service---------------------%>
            <cc1:modalpopupextender id="popretrctservice" runat="server" behaviorid="mpeServceretrctConf"
                targetcontrolid="hdnPop10" popupcontrolid="Panel1" cancelcontrolid="imgClose4">
            </cc1:modalpopupextender>
            <asp:HiddenField ID="hdnPop10" runat="server" />
            <asp:Panel ID="Panel1" runat="server" CssClass="Popup" Style="width: 430px; height: 180px;
                display: none;">
                <%-- display: none; --%>
                <asp:Image ID="Image5" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" onclick="closeServicePopup();" ImageUrl="~/Images/closebtn.png" />
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
                                <asp:Label ID="Lblmsg1" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="Lblmsg2" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:Button ID="btnpopretracservice" runat="server" CssClass="button" Width="100px"
                                    Text="Confirm" OnClick="btnpopretracservice_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <input type="button" class="button" value="Cancel" style="width: 100px;" onclick="closeRtractFinalConfPopup();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%-- ---------------------------------------------------Free Plan POPUP-------------------------------------------------- --%>
            <cc1:modalpopupextender id="PopUpFreePlan" runat="server" behaviorid="mpeFreePlan"
                targetcontrolid="hdnFreePlan" popupcontrolid="pnlFreePlan">
            </cc1:modalpopupextender>
            <asp:HiddenField ID="hdnFreePlan" runat="server" />
            <asp:Panel ID="pnlFreePlan" runat="server" CssClass="Popup" Style="width: 400px; display: none;
                height: 300px;">
                <%-- display: none; --%>
                <asp:Image ID="Image6" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closeFreePlanPopup();" />
                <center>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                &nbsp;&nbsp;&nbsp;Eligible Free Regional Pack
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="center" colspan="6">
                                <asp:Label ID="lblNoOfFOCPlan" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="90%">
                        <tr>
                            <td colspan="3" align="center">
                                <div class="plan_scroller">
                                    <asp:UpdatePanel ID="updatepanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdFreePlan" EmptyDataText="No Plan Found" CssClass="Grid" runat="server"
                                                AutoGenerateColumns="false" OnRowDataBound="grdFreePlan_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="350px"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAllFreePlan" runat="server" Text="Select Plan"></asp:Label>
                                                            <br />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbFreePlan" runat="server" />
                                                            <%--<asp:HiddenField ID="hdnfullname" runat="server" Value='<%# Eval("fullname")%>' />--%>
                                                            <itemstyle horizontalalign="Center" />
                                                            <%-- <asp:HiddenField ID="hdnFreePlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />--%>
                                                            <asp:HiddenField ID="hdnFreePlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="90%">
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="btnAddFreePlan" Visible="false" runat="server" Width="60px" Text="Add"
                                    CommandName="Add" OnClick="btnAddFreePlan_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancelFreePlan" runat="server" Width="60px" Text="Cancel" Visible="false"
                                    OnClientClick="closeFreePlanPopup();" />
                                <asp:Label ID="lblFreePlan" runat="server" ForeColor="Red"></asp:Label>
                                <%--<asp:Button ID="btnCloseAdd" runat="server" Width="60px" Text="Cancel" 
                                    onclick="btnCloseAdd_Click"/> --%>
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%--------------------------------------end changes--------------------------%>

            
             <%--------------------------------------Customer Modification--------------------------%>
            <cc1:ModalPopupExtender ID="popupModifyCust" runat="server" BehaviorID="mpeModifyCust"
                TargetControlID="hdnmodifyCust" PopupControlID="pnlModifyCust">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnmodifyCust" runat="server" />
            <asp:Panel ID="pnlModifyCust" runat="server" CssClass="Popup" Style="width: 430px;
                display: none; height: 300px;">
                <%-- display: none; --%>
                <center>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                &nbsp;&nbsp;&nbsp;Customer Detail Modification
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
                            <td align="left">
                                <asp:Label ID="lblTesting" runat="server" Text="First Name *"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label77" runat="server" Text=": "></asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox runat="server" Width="150px" ID="txtModifyFirstName" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label100" runat="server" Text="Middle Name"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label101" runat="server" Text=": "></asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox runat="server" Width="150px" ID="txtModifyMiddleName" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label84" runat="server" Text="Last Name *"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label85" runat="server" Text=": "></asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox runat="server" Width="150px" ID="txtModifylastName" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label78" runat="server" Text="Mobile Number *"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label79" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox runat="server" Width="150px" ID="txtModifymobile" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label83" runat="server" Text="Email-Id"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label86" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox runat="server" Width="150px" ID="txtModifyEmail" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label80" runat="server" Text="Address *"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label81" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" colspan="2" rowspan="2">
                                <asp:TextBox runat="server" Width="150px" ID="txtmodifyAddress" TextMode="MultiLine"
                                    MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                       <tr>
                            <td align="left">
                                &nbsp;
                            </td>
                            <td align="center">
                                &nbsp;
                            </td>
                            <td align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>                           
                             <td align="left">                                
                                <asp:Label ID="Lab2000" runat="server" Text="(*) Are mandatory field" ForeColor="Red" ></asp:Label>                           
                            </td>
                            <td align="center">
                                &nbsp;
                            </td>
                            <td align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Label ID="lblModifyError" ForeColor="Red" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Button ID="btnModify" runat="server" CssClass="button" Text="Modify" Width="100px"
                                    OnClientClick="return Allvalidate();" OnClick="btnModify_Click" />
                                &nbsp;&nbsp;
                                <input id="btnModifycustCan" class="button" runat="server" type="button" value="Cancel"
                                    style="width: 100px;" onclick="closeModifyCustPopup();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%--------------------------------------End Customer Modification--------------------------%>


            <%-- ---------------------------------------------------Renew confirmatiom pop-------------------------------------------------- --%>
            <cc1:modalpopupextender id="popallrenewal" runat="server" behaviorid="mpeRenewPlan"
                targetcontrolid="hdnrenewPlan" popupcontrolid="pnlrenewalPlan">
            </cc1:modalpopupextender>
            <asp:HiddenField ID="hdnrenewPlan" runat="server" />
            <asp:Panel ID="pnlrenewalPlan" runat="server" CssClass="Popup" Style="width: 700px; display: none;
                height: 300px;">
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
                                <div class="plan_scroller">
                                    <asp:UpdatePanel ID="updatepanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="Gridrenew" EmptyDataText="No Plan Found" CssClass="Grid" runat="server"
                                                AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Plan Name" DataField="PlanName" ItemStyle-Width="350px"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField HeaderText="Renew Status" DataField="RenewStatus" ItemStyle-Width="700px"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="90%">
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="Button3" runat="server" CssClass="button" Text="OK" Visible="true"
                                    Width="100px" OnClick="btnRefreshForm_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%--------------------------------------end changes--------------------------%>

             <cc1:ModalPopupExtender ID="popupModifyConfirm" runat="server" BehaviorID="mpeModifyConfirm"
                TargetControlID="hdnModifyConfirm" PopupControlID="pnlModifyConfirm">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnModifyConfirm" runat="server" />
            <asp:Panel ID="pnlModifyConfirm" runat="server" CssClass="Popup" Style="width: 430px;
                display: none; height: 330px;">
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
                            <td align="left">
                                <asp:Label ID="Label89" runat="server" Text="First Name"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label90" runat="server" Text=": "></asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblModifyFirstName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label102" runat="server" Text="Middle Name"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label103" runat="server" Text=": "></asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:Label runat="server" ID="lblmodifyMiddlename"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label91" runat="server" Text="Last Name"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label92" runat="server" Text=": "></asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:Label runat="server" ID="lblModifyLastName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label93" runat="server" Text="Mobile Number"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label94" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:Label runat="server" ID="lblModifyMobileNumber"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label95" runat="server" Text="Email-Id"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label96" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:Label runat="server" ID="lblModifyEmail"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label97" runat="server" Text="Address"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label98" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" colspan="2" rowspan="2">
                                <asp:Label runat="server" ID="lblModifyAddress"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                &nbsp;
                            </td>
                            <td align="center">
                                &nbsp;
                            </td>
                            <td align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:CheckBox ID="cboCheckModify" runat="server" Checked="false" />
                                <asp:Label ID="Label99" runat="server" Text="I confirm that relevant documentation to make this change has been done and will be made available to Hathway"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                &nbsp;
                            </td>
                            <td align="center">
                                &nbsp;
                            </td>
                            <td align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Button ID="btnModifyConfirm" runat="server" CssClass="button" Text="Confirm"
                                    Width="100px" OnClick="btnModify_Click" OnClientClick="return AllvalidateConfirm();" />
                                &nbsp;&nbsp;
                                <input id="Button6" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closeModifyCustConfirmPopup();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>

            <%-------------------------------------------------Plans For AutoRenewal-----------------------------------%>
             <cc1:ModalPopupExtender ID="popAutoRenewal" runat="server" BehaviorID="mpeautoRenewPlan"
                TargetControlID="hdnAutoRenewal" PopupControlID="pnlautorenewalPlan">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnAutoRenewal" runat="server" />
            <asp:Panel ID="pnlautorenewalPlan" runat="server" CssClass="Popup" Style="width: 700px;
                display: none; height: 300px;">
                <%-- display: none; --%>
                <asp:Image ID="Image9" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                    margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closeAutoRenewalPopup();" />
                <center>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="3">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label87" runat="server" ForeColor="#094791" Font-Bold="true"
                                    Text="Auto Renewal"></asp:Label>
                                   
                            </td>
                            <td align="right">
                             <asp:CheckBox ID="choAutorenewAll" runat="server" onclick="FunChkDisableauto(this);" />
                                                        <asp:Label runat="server" ID="Label88" Text="All Renewal"></asp:Label>
                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            </td>

                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table width="90%">
                        <tr>
                            <td colspan="3" align="center">
                                <div class="plan_scroller">
                                    <asp:UpdatePanel ID="updatepanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdAutoRenewal" EmptyDataText="No Plan Found" CssClass="Grid" runat="server"
                                                AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Plan Name" DataField="PlanName" ItemStyle-Width="350px"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField HeaderText="Auto Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkAutoRenew" runat="server" onclick="ChkcountAuto(this)"    />
                                                            <asp:HiddenField ID="hdnPlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                             <asp:HiddenField ID="hdnAutoStatus" runat="server" Value='<%# Eval("RenewStatus").ToString()%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="90%">
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="btnAutoRenew" runat="server" CssClass="button" Text="OK" Visible="true"
                                    Width="100px" OnClick="btnAutoRenew_Click" />&nbsp;
                                    &nbsp;&nbsp;
                                    <input id="btnAutopopClose" class="button" runat="server" type="button" value="Cancel"
                                    style="width: 100px;" onclick="closeAutoRenewalPopup();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
              <%-------------------------------------------------End Plans For AutoRenewal-----------------------------------%>

            <%-- ----------------------------------------------------Loader------------------------------------------------------------------ --%>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." Style="" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:updatepanelanimationextender id="UpdatePanelAnimationExtender1" runat="server"
        targetcontrolid="UpdatePanel1">
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
    </cc1:updatepanelanimationextender>
</asp:Content>
