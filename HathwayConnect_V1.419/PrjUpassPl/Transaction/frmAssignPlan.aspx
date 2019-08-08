<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmAssignPlan.aspx.cs" Inherits="PrjUpassPl.Transaction.frmAssignPlan" %>
        <%@ OutputCache Duration="5" VaryByParam="*" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JS/jquery.min.js" type="text/javascript"></script>
    <script src="../JS/jqueryui.min.js" type="text/javascript"></script>
    <link href="../CSS/jqueryui.css" rel="Stylesheet" type="text/css" />
    <link href="../CSS/jquery-ui.css" rel="stylesheet" />
    <link href="../CSS/jquery.sweet-dropdown.min.css" rel="stylesheet" type="text/css" />
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../JS/jquery-ui.js" type="text/javascript"></script>
    <script src="../JS/jquery.sweet-dropdown.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function select_one(control) {
            debugger;
            var currind = control.parentElement.parentElement.rowIndex;
            var GridView = document.getElementById('<%=GrdchangePlan.ClientID %>');
            for (i = 1; GridView.rows.length; i++) {

                var inputs = GridView.rows[i].getElementsByTagName('input');
                if (inputs[0].type = "checkbox") {
                   if (i != currind) {
                        inputs[0].checked = false;
                        continue;
                    }
                }
            }

        }
        function selectBasic_one(control) {
            debugger;
            var currind = control.parentElement.parentElement.rowIndex;
            var GridView = document.getElementById('<%=grdBasicADD.ClientID %>');
            for (i = 1; GridView.rows.length; i++) {

                var inputs = GridView.rows[i].getElementsByTagName('input');
                if (inputs[0].type = "checkbox") {
                    if (GridView.rows[i].cells[7].innerHTML.toString() == "B" && i != currind && GridView.rows[currind].cells[7].innerHTML.toString() == "B") {

                        inputs[0].checked = false;
                        continue;
                    }
                }
            }

        }
        function filter2(phrase, _id) {
            debugger;
            var words = phrase.value.toLowerCase().split(" ");
            var table = document.getElementById(_id);
            var ele;
            for (var r = 1; r < table.rows.length; r++) {
                var cellvalue = table.rows[r].cells[0].innerHTML;
                ele = cellvalue;
                var displayStyle = 'none';
                for (var i = 0; i < words.length; i++) {
                    if (ele.toLowerCase().indexOf(words[i]) >= 0)
                        displayStyle = '';
                    else {
                        displayStyle = 'none';
                        break;
                    }
                }

                table.rows[r].style.display = displayStyle;
            }
        }
        function filterBROD(phrase, _id) {
            debugger;
            var words = phrase.value.toLowerCase().split(" ");
            var table = document.getElementById('<%=grdPlanChan.ClientID %>');
            var ele;
            for (var r = 1; r < table.rows.length; r++) {
                var cellvalue = table.rows[r].cells[8].innerHTML;
                ele = cellvalue;
                var displayStyle = 'none';
                for (var i = 0; i < words.length; i++) {
                    if (ele.toLowerCase().indexOf(words[i]) >= 0)
                        displayStyle = '';
                    else {
                        displayStyle = 'none';
                        break;
                    }
                }

                table.rows[r].style.display = displayStyle;
            }
        }
        function filterSDHD(phrase, _id) {
            debugger;
            var words = phrase.value.toLowerCase().split(" ");
            var table = document.getElementById('<%=grdPlanChan.ClientID %>');
            var ele;
            for (var r = 1; r < table.rows.length; r++) {
                var cellvalue = table.rows[r].cells[9].innerHTML;
                ele = cellvalue;
                var displayStyle = 'none';

                for (var i = 0; i < words.length; i++) {
                    if (ele.toLowerCase().indexOf(words[i]) >= 0)
                        displayStyle = '';
                    else {
                        displayStyle = 'none';
                        break;
                    }
                }

                table.rows[r].style.display = displayStyle;
            }
        }
        function filterFREEORPAID(phrase, _id) {
            debugger;
            var words = phrase.value.toLowerCase().split(" ");

            if (words == 'free') {
                words = 'Y';
            }
            else {
                words = 'N';
            }
            var table = document.getElementById('<%=grdPlanChan.ClientID %>');
            var ele;
            for (var r = 1; r < table.rows.length; r++) {

                var cellvalue = table.rows[r].cells[10].innerHTML;
                ele = cellvalue;
                var displayStyle = 'none';
                for (var i = 0; i < words.length; i++) {
                    if (ele == words)
                        displayStyle = '';
                    else {
                        displayStyle = 'none';
                    }
                }

                table.rows[r].style.display = displayStyle;
            }
        }

        function filterGENER(phrase, _id) {
            debugger;
            var words = phrase.value.toLowerCase().split(" ");
            var table = document.getElementById('<%=grdPlanChan.ClientID %>');
            var ele;
            for (var r = 1; r < table.rows.length; r++) {
                var cellvalue = table.rows[r].cells[11].innerHTML;
                ele = cellvalue;
                var displayStyle = 'none';
                for (var i = 0; i < words.length; i++) {
                    if (ele.toLowerCase().indexOf(words[i]) >= 0)
                        displayStyle = '';
                    else {
                        displayStyle = 'none';
                        break;
                    }
                }

                table.rows[r].style.display = displayStyle;
            }
        }
        function filterallinone(phrase, _id) {
            debugger
            var wordBrod = $('#dropDownId :selected').text();
            var wordSDHD = $('#dropDownId :selected').text();
            var wordPAY = $('#dropDownId :selected').text();
            var wordGener = $('#dropDownId :selected').text();

            if (wordPAY == 'free') {
                wordPAY = 'Y';
            }
            else if (wordPAY == 'Pay') {
                wordPAY = 'N';
            }
            else {
                wordPAY = 'ALL';
            }
            var table = document.getElementById('<%=grdPlanChan.ClientID %>');
            var ele;
            for (var r = 1; r < table.rows.length; r++) {

                var cellvaluePay = table.rows[r].cells[10].innerHTML;
                var cellvalueBrod = table.rows[r].cells[8].innerHTML;
                var cellvalueSDHD = table.rows[r].cells[9].innerHTML;
                var cellvalueGener = table.rows[r].cells[11].innerHTML;
                ele = cellvalue;
                var displayStyle = 'none';
                var payvalidation = '';
                for (var i = 0; i < words.length; i++) {
                    if (payvalidation)
                        displayStyle = '';
                    else {
                        displayStyle = 'none';
                    }

                }

                table.rows[r].style.display = displayStyle;
            }

        }
        function calculateTotal(control) {
            var currind = control.parentElement.parentElement.rowIndex;
            var prevaltot = document.getElementById('<%=lbltotaladd.ClientID %>').innerHTML.toString();
            var prevalchcount = document.getElementById('<%=lblChannelcount.ClientID %>').innerHTML.toString();
            // alert("not checked");
            var GridView = document.getElementById('<%=grdPlanChan.ClientID %>');
            var state = 0;
            var radcheck = false;
            var i;
            // var cell;
            var cell1;
            var totalPrint2 = 0;
            var count = 0;
            var totalChannel = 0;
            var cell = 0;
            var SD = 0;
            var HD = 0;
            var ALPlanCount = 1;
            debugger;
            if (document.getElementById('<%=radPlanAll.ClientID%>').checked == true)
            { radcheck = true; }
            for (i = 1; GridView.rows.length; i++) {

                var inputs = GridView.rows[i].getElementsByTagName('input');
                if (inputs[0].type = "checkbox") {

                    if (inputs[0].checked) {
                        if (GridView.rows[i].cells[7].innerHTML.toString() == "B" && i != currind && GridView.rows[currind].cells[7].innerHTML.toString() == "B" || GridView.rows[i].cells[7].innerHTML.toString() == "HSP" && i != currind && GridView.rows[currind].cells[7].innerHTML.toString() == "HSP") {
                            inputs[0].checked = false;
                            continue;
                        }
                        if (GridView.rows[i].cells[7].innerHTML.toString() == "AL" && i != currind && GridView.rows[currind].cells[7].innerHTML.toString() == "AL") {
                            ALPlanCount++;
                            if (ALPlanCount > 20) {
                                inputs[0].checked = false;
                                alert("Alacarte Plan Max Selection 20 only.");
                                continue;
                            }
                        }
                        cell = GridView.rows[i].cells[2].innerHTML.toString();
                        SD = GridView.rows[i].cells[4].innerHTML.toString();
                        HD = GridView.rows[i].cells[5].innerHTML.toString();

                        var total = parseFloat(cell); //+ parseInt(cell1);
                        var totalSD = parseInt(SD, 10);
                        var totalHD = parseInt(HD, 10);
                        var HDCount = parseInt(totalHD) * 2;
                        channelcount = parseInt(totalSD) + parseInt(HDCount);
                        totalChannel = parseInt(channelcount) + parseInt(totalChannel);
                        totalPrint2 = parseFloat(total) + parseFloat(totalPrint2);
                        totalPrint2 = Math.round(totalPrint2 * 100) / 100;
                        document.getElementById('<%=lbltotaladd.ClientID %>').innerHTML = totalPrint2;
                        document.getElementById('<%=lblChannelcount.ClientID %>').innerHTML = totalChannel;

                    }

                    else {
                        count = count + 1;
                        if (count == GridView.rows.length - 1) {
                            document.getElementById('<%=lbltotaladd.ClientID %>').innerHTML = "0.00/-";
                            document.getElementById('<%=lblChannelcount.ClientID %>').innerHTML = "0";
                            break;

                        }
                    }

                }
            }
        }
  </script>
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

        function closealacartechangeconfirm() {
            $find("mpealacartebasechange").hide();
            return false;
        }

        function closealacrtebaseplan() {
            $find("mpealacrtebaseplan").hide();
            $find("mpeALFreePlan").show();
            return false;
        }

        function closeDiscount() {

            $find("mpediscount").hide();
            return false;
        }

        function CloseActiosPop() {
            $find("mpeActions").hide();
            return false;
        }

        function CloseActiosADDONPop() {
            $find("mpeActionsADDON").hide();
            return false;
        }
        function CloseActiosALPop() {
            $find("mpeActionsAL").hide();
            return false;
        }

        function closeFreePlanPopup() {
            $find("mpeFreePlan").hide();
            return false;
        }

        function closeALFreePlanPopup() {
            $find("mpeALFreePlan").hide();
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

        function closeServicePopupS() {

            $find("mpeServiceS").hide();
            return false;
        }

        function closeFOCMsgPopupALL() {
            $find("mpeFOCMsg").hide();
            $find("mpeFreePlan").show();
            return false;

        }

        function closeaddplanconfrimPopup() {
            $find("mpeaddplanConfirmation").hide();
            return false;
        }

        function closeMsgPopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeMsg").hide();
            return false;
        }

        function closeAlignDatePopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeAlignDate").hide();
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
        function closeRtractFinalConfPopup() {

            $find("mpeServceretrctConf").hide();
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

        function CloseSwapPop() {
            $find("mpeSwapAL").hide();
            return false;
        }
        function closeSwapConfirmPopup() {
            $find("mpeSwapModifyConfirm").hide();
            return false;
        }
        function CloseFaultyPop() {
            $find("mpeFaultyAL").hide();
            return false;
        }

        function CloseTERMINATEALPop() {
            $find("mpeTERMINATEAL").hide();
            return false;
        }
        function CloseTERMINATEALConfirmPopup() {
            $find("mpeTERMINATEALModifyConfirm").hide();
            return false;
        }
        function closeBulkCancelPopup() {

            $find("mpeBulkCancel").hide();
            return false;
        }
        function closeChangeConfPopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeChangeplanconf").hide();
            return false;
        }
        function closeBasicADDPopup() {
            //document.getElementById("<%= radPlanAD.ClientID %>").checked = true;
            $find("mpeBasicAdd").hide();
            return false;
        }
        function closepopupAlign() {
            $find("mpeAlign").hide();
            return false;
        }

    </script>


    <script type="text/javascript">
    function FunChkDisable(id) {

            if (id.checked == true) {
            debugger;
                var chkrow;
                document.getElementById("<%= btnRenSubmit.ClientID %>").style.display = "";
                for (k = 0; k < 6; k++) {
                    if (k == 0) {
                        var gv = document.getElementById("<%= grdBasicPlanDetails.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[9];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[9];

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
                    else if (k == 1) {
                        var gv = document.getElementById("<%= grdAddOnPlan.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[9];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[9];

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
                    else if (k == 2) {
                        var gv = document.getElementById("<%= grdAddOnPlanReg.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[7];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[7];

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
                    else if (k == 3) {
                        var gv = document.getElementById("<%= grdCarte.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[9];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[9];

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

                    else if (k == 4) {
                        var gv = document.getElementById("<%= Grdhathwayspecial.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[9];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[9];

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

                    else if (k == 5) {
                        var gv = document.getElementById("<%= grdCartefree.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[7];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[7];

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

                    if (gv == null) {
                        continue;
                    }



                }
            }
            else {
                var chkrow;
                document.getElementById("<%= btnRenSubmit.ClientID %>").style.display = "none";
                for (k = 0; k < 6; k++) {
                    if (k == 0) {
                        var gv = document.getElementById("<%= grdBasicPlanDetails.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[9];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[9];

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
                    else if (k == 1) {
                        var gv = document.getElementById("<%= grdAddOnPlan.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[9];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[9];

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
                    else if (k == 2) {
                        var gv = document.getElementById("<%= grdAddOnPlanReg.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[9];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[9];

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
                    else if (k == 3) {
                        var gv = document.getElementById("<%= grdCarte.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[9];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[9];

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

                    else if (k == 4) {
                        var gv = document.getElementById("<%= Grdhathwayspecial.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[9];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[9];

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

                    else if (k == 5) {
                        var gv = document.getElementById("<%= grdCartefree.ClientID %>");
                        if (gv != null) {
                            if (gv.rows.length > 0) {
                                var chk = gv.rows[0].cells[7];
                                for (i = 0; i < gv.rows.length; i++) {
                                    cell = gv.rows[i].cells[7];

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


                    if (gv == null) {
                        continue;
                    }


                }

            }
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
            for (k = 0; k < 6; k++) {

                if (k == 0) {
                    var grid = document.getElementById("<%= grdBasicPlanDetails.ClientID %>");
                }
                else if (k == 1) {
                    var grid = document.getElementById("<%= grdAddOnPlan.ClientID %>");
                }
                else if (k == 2) {
                    var grid = document.getElementById("<%= grdAddOnPlanReg.ClientID %>");
                }
                else if (k == 3) {
                    var grid = document.getElementById("<%= grdCarte.ClientID %>");
                }
                else if (k == 4) {
                    var grid = document.getElementById("<%= Grdhathwayspecial.ClientID %>");

                }
                else if (k == 5) {
                    var grid = document.getElementById("<%= grdCartefree.ClientID %>");
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
        
        
        }

//        function FunChkDisableauto(id) {

//            if (id.checked == true) {

//                var gv = document.getElementById("<%= grdAutoRenewal.ClientID %>");


//                var chkrow;
//                if (gv.rows.length > 0) {
//                    var chk = gv.rows[0].cells[1];
//                    for (i = 0; i < gv.rows.length; i++) {
//                        cell = gv.rows[i].cells[1];

//                        for (j = 0; j < cell.childNodes.length; j++) {
//                            if (cell.childNodes[j].type == "checkbox") {
//                                cell.childNodes[j].checked = true;
//                            }
//                        }
//                    }
//                }
//            }
//            else {

//                var gv = document.getElementById("<%= grdAutoRenewal.ClientID %>");

//                var chkrow;
//                if (gv.rows.length > 0) {
//                    var chk = gv.rows[0].cells[1];
//                    for (i = 0; i < gv.rows.length; i++) {
//                        cell = gv.rows[i].cells[1];

//                        for (j = 0; j < cell.childNodes.length; j++) {
//                            if (cell.childNodes[j].type == "checkbox") {
//                                cell.childNodes[j].checked = false;
//                            }
//                        }
//                    }
//                }
//            }
//        }


           function ChkcountBulkCancelStatus(action, id) {
               
               $('#<%=choBulkCancel.ClientID %>').prop('checked', false);

               if (id == "B") {
                   if ($(action).prop('checked') == true) {
                       $("#ctl00_MasterBody_grdBulkCancel tr").each(function () {
                           var this_row = $(this);
                           var plan_type = $.trim(this_row.find('td:eq(2)').html());
                           if (plan_type == "NCF" && plan_type != "B") {
                               //alert("ïn grd");
                               var $checkBox = $(this).find("input[type='checkbox']");
                               //  $checkBox.attr("checked", "checked");
                               $checkBox.prop('checked', true); 
                           }
                       });
                   }
                   else {
                       $("#ctl00_MasterBody_grdBulkCancel tr").each(function () {
                           var this_row = $(this);
                           var plan_type = $.trim(this_row.find('td:eq(2)').html());
                           if (plan_type == "NCF" && plan_type != "B") {
                               var $checkBox = $(this).find("input[type='checkbox']");
                             //  $checkBox.removeAttr("checked");
                               $checkBox.prop('checked', false); 
                           }

                       });
                   }


               }


           }



   function FunChkBulkCancel(id) {

               if ($(id).prop('checked') == true) {
                   $("#ctl00_MasterBody_grdBulkCancel tr").each(function () {
                       var this_row = $(this);

                       var $checkBox = $(this).find("input[type='checkbox']");
                       //  $checkBox.attr("checked", "checked");
                       $checkBox.prop('checked', true);

                   });
               }
               else {
                   $("#ctl00_MasterBody_grdBulkCancel tr").each(function () {
                       var this_row = $(this);

                       var $checkBox = $(this).find("input[type='checkbox']");
                       //  $checkBox.attr("checked", "checked");
                       $checkBox.prop('checked', false);

                   });
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
        .Hide { display:none; }
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
                                <td align="right" width="130px" id="avbal" runat="server">
                                    Available Balance:
                                </td>
                                <td align="left" width="67px" id="lblavbal" runat="server">
                                    <asp:Label ID="lblAvailBal" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table width="100%">
                        <tr>
                            <td align="left" valign="top" id="divCustHolder" runat="server">
                                <div class="delInfo" runat="server" id="divSearchHolder">
                                    <asp:Panel ID="Panel4" runat="server" DefaultButton="btnSearch">
                                   
                                    <table>
                                        <tr>
                                            <td style="width: 85px;">
                                                <asp:Label runat="server" ID="Label20" Text="Search By"></asp:Label>
                                                <asp:Label ID="Label59" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 285px;">
                                                <asp:RadioButtonList ID="rdoSearchParamType" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="0">VC/Mac ID/VM</asp:ListItem>
                                                    <asp:ListItem Value="1">Account No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 95px;">
                                                <asp:TextBox runat="server" Width="115px" ID="txtSearchParam" MaxLength="30"></asp:TextBox>
                                            </td>
                                            <td style="width: 80px;">
                                                <asp:Button runat="server" ID="btnSearch" Text="Search" ValidationGroup="searchValidation"
                                                    OnClick="btnSearch_Click" />
                                                &nbsp;&nbsp;
                                            </td>
                                            <td id="lcocodetd" runat="server" visible="false">
                                                <asp:Label ID="lbllcocode" runat="server" Text=""></asp:Label>
                                                &nbsp;&nbsp;
                                            </td>
                                            <td id="lconametd" runat="server" visible="false">
                                                <asp:Label ID="lbllconame" runat="server" Text=""></asp:Label>
                                                &nbsp;&nbsp;
                                            </td>
                                            <td id="lcocobalancetd" runat="server" visible="false">
                                                <asp:Label ID="lbllcobalance" runat="server" Text=""></asp:Label>
                                                &nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                     </asp:Panel>
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
                                        <tr id="tremail" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label ID="Label9" runat="server" Text="Email Id"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblemail" runat="server"></asp:Label>
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
                                        <tr id="trCustBOUQUET_ID" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label runat="server" ID="Label163_" Text="Bouquet ID"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label164_" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblCustBOUQUET_ID" Text=""></asp:Label>
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
                                                <asp:GridView ID="GridVC" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                                                    Height="100%" Width="60%" OnRowDataBound="GridVC_RowDataBound">
                                                    <Columns>
                                                    <asp:BoundField HeaderText="TV" DataField="TV" HeaderStyle-HorizontalAlign="Center"
                                                     Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:BoundField HeaderText="VC ID" DataField="VC_ID" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:BoundField HeaderText="STB NO" DataField="STB_NO" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:BoundField HeaderText="Status" DataField="Status" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:BoundField HeaderText="SUSPENSION DATE" DataField="SUSPENSION_DATE" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:BoundField HeaderText="BOX TYPE" DataField="BOX_TYPE" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="true" ItemStyle-HorizontalAlign="right" />
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                    <asp:Button ID="btnSwap" runat="server" Text="SWAP" Visible="false" OnClick="btnSwap_click" />  <%-- OnClick="lnkReceiptno_click"--%>
                                                    <asp:HiddenField ID="hdnVC_ID" runat="server" Value='<%# Eval("VC_ID").ToString()%>' />
                                                    <asp:HiddenField ID="hdnSTB_NO" runat="server" Value='<%# Eval("STB_NO").ToString()%>' />
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    </Columns>
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
                                                <asp:Button ID="btnModifyCust" runat="server" CssClass="button" Text="Modify Customer Detail"
                                                    Width="160px" OnClick="btnModifyCust_Click" />
                                                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="button" Target="_blank" Style="color: white;
                                                    padding: 3px; font-weight: bold; font-family: Trebuchet MS,Tahoma,Verdana,Arial,sans-serif;
                                                    font-size: 1em;">E-Caf Report</asp:HyperLink>
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
                                                    &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnTerminate" runat="server" Text="Terminate" OnClick="btnTerminate_Click" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="BtnRetract" runat="server" Text="Retrack" OnClick="BtnRetract_Click" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnFaulty" runat="server" Text="SWAP STB" OnClick="BtnFaulty_Click" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnVCSWAP" runat="server" Text="SWAP VC" OnClick="BtnVCSWAP_Click" Visible="false" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="BtnRequestfrm" runat="server" Text="Online Plan Request Form" OnClick="BtnRequestfrm_Click"  Visible="false" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnQuickpay" runat="server" Text="Quick Pay SMS Link" OnClick="btnQuickpay_Click"  />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID= "btnCustomerReceipt" runat="server" Text="Customer Receipt" OnClick="btnCustomerReceipt_Click"  Visible="false" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID= "btnBulkCancel" runat="server" Text="ALL Cancel" OnClick="btnBulkCancel_Click"  />
                                                &nbsp;&nbsp;
                                                <asp:HyperLink ID="lnkManageExPlan" runat="server" Target="_blank" NavigateUrl="~/Transaction/frmCancelPlans.aspx"
                                                Style="display: inline-block; width: 180px; text-decoration: none; background-color: #094791;
                                                color: white; text-align: center; height: 18px; ">Manage Expired Plans</asp:HyperLink>
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
                                                        <asp:Button ID="btnAutoRenewal" runat="server" Text="Auto Renew" OnClick="btnAutoRenewal_Click" />&nbsp;
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
                                                            <asp:Label ID="lblBasicPlan" runat="server" Text="Basic"></asp:Label>
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
                                                                    <%--<asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />--%>
                                                                        <asp:BoundField HeaderText="SD" DataField="SD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="HD" DataField="HD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="Total" DataField="Total_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="BC Share" DataField="BD_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                    <%--<asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-Width="100"
                                                                        ItemStyle-HorizontalAlign="Left" />--%>
                                                                    <asp:BoundField HeaderText="Valid Upto" DataField="EXPIRY" ItemStyle-Width="110"
                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField HeaderText="Grace" DataField="GRACE" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Left" Visible="false" />
                                                                    <asp:TemplateField HeaderText="Auto Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAutorenew" Text="Off" runat="server"></asp:Label>
                                                                            <asp:CheckBox ID="cbBAutorenew" Enabled="false" Style="visibility: hidden" runat="server"
                                                                                AutoPostBack="true" onclick="CheckOne(this)" OnCheckedChanged="cbBAutorenew_Clicked" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="cbBasicrenew" runat="server" AutoPostBack="false" onclick="Chkcount(this)" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="100" />
                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <%-- <asp:ImageButton ID="lnkpopOpen"  runat="server" OnClick="lnkpopOpen_Click" ImageUrl="~/Img/dot3.png"
                                                                            style="width:40px;height:25px"/>--%>
                                                                            <asp:Image ID="Imgbasicaction" runat="server" ImageUrl="~/Img/dot3.png" data-dropdown='<%# "#basic"+Container.DataItemIndex+1 %>'
                                                                                Style="width: 40px; height: 25px" ClientIDMode="Static" />
                                                                            <div class="dropdown-menu dropdown-anchor-bottom-right dropdown-has-anchor" id='<%# "basic"+Container.DataItemIndex+1 %>'>
                                                                                <ul>
                                                                                    <li><a href="#">
                                                                                        <asp:Button ID="lnkBRenew" runat="server" Text="RENEW" OnClick="lnkBRenew_Click"
                                                                                            Width="110" /></a></li>
                                                                                    <li><a href="#">
                                                                                        <asp:Button ID="lnkBCancel" runat="server" Text="CANCEL" OnClick="lnkBCancel_Click"
                                                                                            Width="110" /></a></li>
                                                                                    <li><a href="#">
                                                                                        <asp:Button ID="lnkBChange" runat="server" Text="CHANGE" OnClick="lnkBChange_Click"
                                                                                            Width="110" /></a></li>
                                                                                            <li><a href="#">
                                                                                        <asp:Button ID="lnkBAlign" runat="server" Text="Aling Expairy" OnClick="lnkBAlign_Click"
                                                                                            Width="110" /></a></li>
                                                                                    <li><a href="#">
                                                                                        <asp:Button ID="lnkAddFOCPack" runat="server" Text="FOC PACK" Visible="false" OnClick="lnkAddFOC_Click"
                                                                                            Width="110" /></a></li>
                                                                                    <li><a href="#">
                                                                                        <asp:Button ID="btnALPack" runat="server" Text="A-La-Carte Pack" Visible="false" OnClick="btnALPack_Click"
                                                                                            Width="110" /></a></li>
                                                                                    <li><a href="#">
                                                                                        <asp:Button ID="btnDiscnt" runat="server" Text="DISCOUNT" Visible="false" OnClick="btnDiscnt_Click"
                                                                                            Width="110" /></a></li>
                                                                                </ul>
                                                                            </div>
                                                                            <asp:HiddenField ID="hdnBasicPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicPlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBasicPlanType" runat="server" Value='<%# Eval("PLAN_TYPE").ToString()%>' /> 
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
                                                                            <asp:HiddenField ID="hdnbasicalacartebase" runat="server" Value='<%# Eval("alacartebase").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnbasicalacartebaseprice" runat="server" Value='<%# Eval("alacartebaseprice").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnBCPrice" runat="server" Value='<%# Eval("BD_PRICE").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnChannelCount" runat="server" Value='<%# Eval("Total_Count").ToString()%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <b>
                                                            <asp:Label ID="lblhathwayspecial" runat="server" Text="Hathway Bouquet"></asp:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                                        <div class="plan_scroller">
                                                                            <asp:GridView ID="Grdhathwayspecial" CssClass="Grid" Width="100%" runat="server"
                                                                                AutoGenerateColumns="false" OnRowDataBound="Grdhathwayspecial_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="350px"
                                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                                    <asp:BoundField HeaderText="SD" DataField="SD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="HD" DataField="HD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="Total" DataField="Total_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="BC Share" DataField="BD_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                                    <%--<asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />--%>
                                                                                    <asp:BoundField HeaderText="Valid Upto" DataField="EXPIRY" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />
                                                                                    <asp:BoundField HeaderText="Grace" DataField="GRACE" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" Visible="false" />
                                                                                    <asp:TemplateField HeaderText="Auto Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAutorenew" Text="Off" runat="server"></asp:Label>
                                                                                            <asp:CheckBox ID="cbAddonAutorenew" Enabled="false" Style="visibility: hidden" runat="server"
                                                                                                AutoPostBack="true" onclick="CheckOne(this)" OnCheckedChanged="cbAddonAutorenew_Clicked" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="cbAddonrenew" runat="server" AutoPostBack="false" onclick="Chkcount(this)" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="60" />
                                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:Image ID="Imgbasicaction" runat="server" ImageUrl="~/Img/dot3.png" data-dropdown='<%# "#HathSpecial"+Container.DataItemIndex+1 %>'
                                                                                                Style="width: 40px; height: 25px" ClientIDMode="Static" />
                                                                                            <div class="dropdown-menu dropdown-anchor-bottom-right dropdown-has-anchor" id='<%# "HathSpecial"+Container.DataItemIndex+1 %>'>
                                                                                                <ul>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lnkADRenew" runat="server" Text="RENEW" OnClick="lnkADRenew_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lnkADCancel" runat="server" Text="CANCEL" OnClick="lnkADCancel_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lnkADChange" runat="server" Text="CHANGE" Visible="false" OnClick="lnkADChange_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                            <li><a href="#">
                                                                                        <asp:Button ID="lnkBAlign" runat="server" Text="Aling Expairy" OnClick="lnkBAlign_Click"
                                                                                            Width="110" /></a></li>
                                                                                                </ul>
                                                                                            </div>
                                                                                            <%--     <asp:HiddenField ID="hdnADPlanId" runat="server" Value='<%# Eval("PLAN_ID").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnADPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                                           <asp:HiddenField ID="hdnADPlanType" runat="server" Value='<%# Eval("PLAN_TYPE").ToString()%>' /> 
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
                                                                                            <asp:HiddenField ID="hdnBCPrice" runat="server" Value='<%# Eval("BD_PRICE").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnChannelCount" runat="server" Value='<%# Eval("Total_Count").ToString()%>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                         
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <b>
                                                            <asp:Label ID="lblAddonPlan" runat="server" Text="Broadcaster Bouquet"></asp:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <cc1:Accordion ID="AddonAccordion" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                                                            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                                            FadeTransitions="true" SuppressHeaderPostbacks="true" TransitionDuration="250"
                                                            FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None">
                                                            <Panes>
                                                                <cc1:AccordionPane ID="AddonAccordionPane" runat="server">
                                                                    <Header>
                                                                        <a href="#" class="href" style="color: White;">Broadcaster Bouquet</a></Header>
                                                                    <Content>
                                                                        <div class="plan_scroller">
                                                                            <asp:GridView ID="grdAddOnPlan" CssClass="Grid" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                                OnRowDataBound="grdAddOnPlan_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="350px"
                                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                                    <asp:BoundField HeaderText="SD" DataField="SD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="HD" DataField="HD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="Total" DataField="Total_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="BC Share" DataField="BD_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                                    <%--<asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />--%>
                                                                                    <asp:BoundField HeaderText="Valid Upto" DataField="EXPIRY" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />
                                                                                    <asp:BoundField HeaderText="Grace" DataField="GRACE" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" Visible="false" />
                                                                                    <asp:TemplateField HeaderText="Auto Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAutorenew" Text="Off" runat="server"></asp:Label>
                                                                                            <asp:CheckBox ID="cbAddonAutorenew" Enabled="false" Style="visibility: hidden" runat="server"
                                                                                                AutoPostBack="true" onclick="CheckOne(this)" OnCheckedChanged="cbAddonAutorenew_Clicked" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="cbAddonrenew" runat="server" AutoPostBack="false" onclick="Chkcount(this)" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="60" />
                                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:Image ID="Imgbasicaction" runat="server" ImageUrl="~/Img/dot3.png" data-dropdown='<%# "#addon"+Container.DataItemIndex+1 %>'
                                                                                                Style="width: 40px; height: 25px" ClientIDMode="Static" />
                                                                                            <div class="dropdown-menu dropdown-anchor-bottom-right dropdown-has-anchor" id='<%# "addon"+Container.DataItemIndex+1 %>'>
                                                                                                <ul>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lnkADRenew" runat="server" Text="RENEW" OnClick="lnkADRenew_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lnkADCancel" runat="server" Text="CANCEL" OnClick="lnkADCancel_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lnkADChange" runat="server" Text="CHANGE" Visible="false" OnClick="lnkADChange_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                </ul>
                                                                                            </div>
                                                                                            <%--     <asp:HiddenField ID="hdnADPlanId" runat="server" Value='<%# Eval("PLAN_ID").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnADPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                                                <asp:HiddenField ID="hdnADPlanType" runat="server" Value='<%# Eval("PLAN_TYPE").ToString()%>' /> 
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
                                                                                            <asp:HiddenField ID="hdnBCPrice" runat="server" Value='<%# Eval("BD_PRICE").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnChannelCount" runat="server" Value='<%# Eval("Total_Count").ToString()%>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </Content>
                                                                </cc1:AccordionPane>
                                                            </Panes>
                                                        </cc1:Accordion>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <b>
                                                            <asp:Label ID="lblAddonPlanReg" Visible="false" runat="server" Text="Addon REG Bouquet"></asp:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <cc1:Accordion ID="AddonAccordionReg" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                                                            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                                            FadeTransitions="true" SuppressHeaderPostbacks="true" TransitionDuration="250"
                                                            FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None" Visible="false">
                                                            <Panes>
                                                                <cc1:AccordionPane ID="AddonAccordionPaneReg" runat="server" Visible="false">
                                                                    <Header>
                                                                        <a href="#" class="href" style="color: White;">Addon Plans</a></Header>
                                                                    <Content>
                                                                        <div class="plan_scroller">
                                                                            <asp:GridView ID="grdAddOnPlanReg" CssClass="Grid" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                                OnRowDataBound="grdAddOnPlanReg_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="350px"
                                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                                    <asp:BoundField HeaderText="SD" DataField="SD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="HD" DataField="HD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="Total" DataField="HD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="BC Price" DataField="BD_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                                    <%--<asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />--%>
                                                                                    <asp:BoundField HeaderText="Valid Upto" DataField="EXPIRY" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />
                                                                                    <asp:BoundField HeaderText="Grace" DataField="GRACE" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />
                                                                                    <asp:TemplateField HeaderText="Auto Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAutorenew" Text="Off" runat="server"></asp:Label>
                                                                                            <asp:CheckBox ID="cbAddonAutorenew" Enabled="false" Style="visibility: hidden" runat="server"
                                                                                                AutoPostBack="true" onclick="CheckOne(this)" OnCheckedChanged="cbAddonAutorenew_Clicked" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="cbAddonrenew" runat="server" AutoPostBack="false" onclick="Chkcount(this)" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="60" />
                                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:Image ID="Imgbasicaction" runat="server" ImageUrl="~/Img/dot3.png" data-dropdown='<%# "#addonreg"+Container.DataItemIndex+1 %>'
                                                                                                Style="width: 40px; height: 25px" ClientIDMode="Static" />
                                                                                            <div class="dropdown-menu dropdown-anchor-bottom-right dropdown-has-anchor" id='<%# "addonreg"+Container.DataItemIndex+1 %>'>
                                                                                                <ul>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lnkADRenew" runat="server" Text="RENEW" OnClick="lnkADRenew_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lnkADCancel" runat="server" Text="CANCEL" OnClick="lnkADCancel_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lnkADChange" runat="server" Text="CHANGE" OnClick="lnkADChange_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                </ul>
                                                                                            </div>
                                                                                            <%--     <asp:HiddenField ID="hdnADPlanId" runat="server" Value='<%# Eval("PLAN_ID").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnADPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                                          <asp:HiddenField ID="hdnADPlanType" runat="server" Value='<%# Eval("PLAN_TYPE").ToString()%>' />
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
                                                                                            <asp:HiddenField ID="hdnBCPrice" runat="server" Value='<%# Eval("BD_PRICE").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnADPlanActionFlag" runat="server" Value='<%# Eval("PLAN_ACTIONFLAG").ToString()%>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </Content>
                                                                </cc1:AccordionPane>
                                                            </Panes>
                                                        </cc1:Accordion>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <b>
                                                            <asp:Label ID="lblAlacartePlanFree" runat="server" Text="A-La-Carte FREE" Visible="false" ForeColor="White"></asp:Label>
                                                            <asp:HiddenField ID="hdnalacartebaseaddplanpoid" runat="server" />
                                                            <asp:HiddenField ID="hdntotaladdamount" runat="server" />
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <cc1:Accordion ID="AlacarteAccordionFree" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                                                            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                                            FadeTransitions="true" SuppressHeaderPostbacks="true" TransitionDuration="250"
                                                            FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None" Visible="false">
                                                            <Panes>
                                                                <cc1:AccordionPane ID="AlacartefreeAccordionPane" runat="server" Visible="false">
                                                                    <Header>
                                                                        <a href="#" class="href" style="color: White;">A-La-Carte FREE</a></Header>
                                                                    <Content>
                                                                        <div class="plan_scroller">
                                                                            <asp:GridView ID="grdCartefree" CssClass="Grid" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                                OnRowDataBound="grdCartefree_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="350px"
                                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                                    <asp:BoundField HeaderText="SD" DataField="SD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="HD" DataField="HD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="Total" DataField="HD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="BC Price" DataField="BD_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                                    <%--<asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />--%>
                                                                                    <asp:BoundField HeaderText="Valid Upto" DataField="EXPIRY" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />
                                                                                    <asp:BoundField HeaderText="Grace" DataField="GRACE" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />
                                                                                    <asp:TemplateField HeaderText="Auto Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAutorenew" Text="Off" runat="server"></asp:Label>
                                                                                            <asp:CheckBox ID="cbAlaAutorenew" Style="visibility: hidden" Enabled="false" runat="server"
                                                                                                onclick="CheckOne(this)" AutoPostBack="true" OnCheckedChanged="cbAlaAutorenew_Clicked" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkalRenew" runat="server" AutoPostBack="false" onclick="Chkcount(this)" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="60" />
                                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:Image ID="Imgbasicaction" runat="server" ImageUrl="~/Img/dot3.png" data-dropdown='<%# "#alacartefree"+Container.DataItemIndex+1 %>'
                                                                                                Style="width: 40px; height: 25px" ClientIDMode="Static" />
                                                                                            <div class="dropdown-menu dropdown-anchor-bottom-right dropdown-has-anchor" id='<%# "alacartefree"+Container.DataItemIndex+1 %>'>
                                                                                                <ul>
                                                                                                    <%--  <li><a href="#">
                                                                                                        <asp:Button ID="lbALRenewal" runat="server" Text="RENEW" OnClick="lnkALRenew_Click"
                                                                                                            Width="110" /></a></li>--%>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lbALCancel" runat="server" Text="CANCEL" OnClick="lnkALCancel_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                    <%-- <li><a href="#">
                                                                                                        <asp:Button ID="lbALChange" runat="server" Text="CHANGE" OnClick="lnkALChange_Click"
                                                                                                            Width="110" /></a></li>--%>
                                                                                                </ul>
                                                                                            </div>
                                                                                            <%--   <asp:HiddenField ID="hdnALPlanId" runat="server" Value='<%# Eval("PLAN_ID").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnALPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                                             <asp:HiddenField ID="hdnALPlanType" runat="server" Value='<%# Eval("PLAN_TYPE").ToString()%>' /> 
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
                                                                                            <asp:HiddenField ID="hdnBCPrice" runat="server" Value='<%# Eval("BD_PRICE").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALPlanActionFlag" runat="server" Value='<%# Eval("PLAN_ACTIONFLAG").ToString()%>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </Content>
                                                                </cc1:AccordionPane>
                                                            </Panes>
                                                        </cc1:Accordion>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <b>
                                                            <asp:Label ID="lblAlacartePlan" runat="server" Text="A-La-Carte" ></asp:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <cc1:Accordion ID="AlacarteAccordion" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                                                            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                                            FadeTransitions="true" SuppressHeaderPostbacks="true" TransitionDuration="250"
                                                            FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None">
                                                            <Panes>
                                                                <cc1:AccordionPane ID="AlacarteAccordionPane" runat="server">
                                                                    <Header>
                                                                        <a href="#" class="href" style="color: White;">A-La-Carte</a></Header>
                                                                    <Content>
                                                                        <div class="plan_scroller">
                                                                            <asp:GridView ID="grdCarte" CssClass="Grid" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                                OnRowDataBound="grdCarte_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="350px"
                                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                                    <asp:BoundField HeaderText="SD" DataField="SD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="HD" DataField="HD_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="Total" DataField="Total_Count" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="90" />
                                                                    <asp:BoundField HeaderText="BC Share" DataField="BD_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                        <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-Width="80" />
                                                                                    <%--<asp:BoundField HeaderText="Activation" DataField="ACTIVATION" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />--%>
                                                                                    <asp:BoundField HeaderText="Valid Upto" DataField="EXPIRY" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" />
                                                                                    <asp:BoundField HeaderText="Grace" DataField="GRACE" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="80" Visible="false" />
                                                                                    <asp:TemplateField HeaderText="Auto Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAutorenew" Text="Off" runat="server"></asp:Label>
                                                                                            <asp:CheckBox ID="cbAlaAutorenew" Style="visibility: hidden" Enabled="false" runat="server"
                                                                                                onclick="CheckOne(this)" AutoPostBack="true" OnCheckedChanged="cbAlaAutorenew_Clicked" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Renew" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkalRenew" runat="server" AutoPostBack="false" onclick="Chkcount(this)" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Status" DataField="PLAN_STATUS" ItemStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-Width="60" />
                                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:Image ID="Imgbasicaction" runat="server" ImageUrl="~/Img/dot3.png" data-dropdown='<%# "#alacarte"+Container.DataItemIndex+1 %>'
                                                                                                Style="width: 40px; height: 25px" ClientIDMode="Static" />
                                                                                            <div class="dropdown-menu dropdown-anchor-bottom-right dropdown-has-anchor" id='<%# "alacarte"+Container.DataItemIndex+1 %>'>
                                                                                                <ul>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lbALRenewal" runat="server" Text="RENEW" OnClick="lnkALRenew_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lbALCancel" runat="server" Text="CANCEL" OnClick="lnkALCancel_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                    <li><a href="#">
                                                                                                        <asp:Button ID="lbALChange" runat="server" Text="CHANGE" Visible="false" OnClick="lnkALChange_Click"
                                                                                                            Width="110" /></a></li>
                                                                                                </ul>
                                                                                            </div>
                                                                                            <%--   <asp:HiddenField ID="hdnALPlanId" runat="server" Value='<%# Eval("PLAN_ID").ToString()%>' /> --%>
                                                                                            <asp:HiddenField ID="hdnALPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnALPlanType" runat="server" Value='<%# Eval("PLAN_TYPE").ToString()%>' /> 
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
                                                                                            <asp:HiddenField ID="hdnBCPrice" runat="server" Value='<%# Eval("BD_PRICE").ToString()%>' />
                                                                                            <asp:HiddenField ID="hdnChannelCount" runat="server" Value='<%# Eval("Total_Count").ToString()%>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </Content>
                                                                </cc1:AccordionPane>
                                                            </Panes>
                                                        </cc1:Accordion>
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
                                    <div style="position: absolute;bottom: 12px;font-size: 12px;color: black;font-weight: bold;"><span style="color:Red">*</span>&nbsp;Taxes extra</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- ---------------------------------------------------ACTION POPUP-------------------------------------------------- --%>
                    <cc1:ModalPopupExtender ID="pop" runat="server" BehaviorID="mpeConfirmation" TargetControlID="hdnPop"
                        PopupControlID="pnlConfirmation">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnPop" runat="server" />
                    <asp:Panel ID="pnlConfirmation" runat="server" CssClass="Popup" Style="width: 750px;
                        display: none; height: auto;">
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
                            <table width="95%" ID="tblRenew" runat="server">
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
                                <tr runat ="server" id="trrenwallcoprice" visible="false">
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label111" runat="server" Text="LCO Price"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label112" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblpopuplcoamt" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr id="RenewDiscount" runat="server" visible="false">
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label107" runat="server" Text="Discount"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label108" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopupDiscount" runat="server" Text=""></asp:Label>
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
                                            <asp:Label ID="Label26" runat="server" Text="Valid Upto"></asp:Label></b>
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
                                            <asp:Label ID="Label42" runat="server" Text="Cust. Refund"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label43" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopupRefund" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>

                                <tr runat="server" id="trPopupCancellcoRefund" visible="false">
                                    <%-- visible="false" --%>
                                    <td align="left">
                                        <b>
                                            <asp:Label ID="Label15" runat="server" Text="LCO Refund"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label32" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPopuplcoRefund" runat="server" Text=""></asp:Label>
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
                            
                            <table>
                                <tr>
                                    <td colspan="3">
                                        <asp:Panel ID="Panel5" runat="server" Style="max-height: 300px; height: auto; overflow: auto;
                                            border: thin solid black;">
                                            <asp:GridView ID="GrdRenewConfrim" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                                                ShowFooter="true" >
                                                <Columns>
                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"
                                                        FooterText="Total :" FooterStyle-HorizontalAlign="Right" ItemStyle-Width="625" />
                                                    <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        
                                                        <asp:BoundField HeaderText="LCO Refund" DataField="refund_lcoamt" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        <asp:BoundField HeaderText="Cust Refund" DataField="refund_amt" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        <asp:BoundField HeaderText="Days Remaining" DataField="days_left" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                    <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="100" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                    <asp:BoundField HeaderText="Discount" DataField="discount" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                    <asp:BoundField HeaderText="Net BASE Price" DataField="netmrp" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                        <asp:BoundField HeaderText="Activation" DataField="Activation" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="100" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Valid Upto" ItemStyle-Width="100">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblValid_upto" runat="server" Text='<%# Eval("valid_upto").ToString()%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnplanrenewconfPlanPoid" runat="server" Value='<%# Eval("plan_poid").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanrenewconfDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanrenewconfplantype" runat="server" Value='<%# Eval("plan_type").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanrenewconfautorenew" runat="server" Value='<%# Eval("AutoRenew").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanaddLcoprice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>

                                </tr><tr runat="server" id="trPopupCancelReason" visible="false">
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
                    <%-----------------------------Confirmation autorenewal Added by Rushali ----------------------------------%>
                    <cc1:ModalPopupExtender ID="popupautorenewalconfirm" runat="server" BehaviorID="mpeautorenewalconfirm"
                        TargetControlID="hdnautorenewalconfirm" PopupControlID="pnlautorenewalconfirm">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnautorenewalconfirm" runat="server" />
                    <asp:Panel ID="pnlautorenewalconfirm" runat="server" CssClass="Popup" Style="width: 430px;
                        display: none; height: 160px;">
                        <%-- display: none; --%>
                        <%--<asp:Image ID="Image10" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                            margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closePopup();" />--%>
                        <asp:Image ID="Image10" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                            margin-top: -15px; margin-right: -15px;" onclick="closeAutoRenewalPopup();" ImageUrl="~/Images/closebtn.png" />
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
                                        <asp:Label ID="Lblautorenewconfirm" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Button ID="Btnautorenewsubmit" runat="server" CssClass="button" Text="Submit"
                                            Width="100px" OnClick="Btnautorenewsubmit_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnautorenewcancel" runat="server" CssClass="button" Text="Cancel"
                                            Width="100px" OnClick="btnautorenewcancel_Click" />
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                    <%-- ---------------------------------------------------ADDON POPUP-------------------------------------------------- --%>
                    <cc1:ModalPopupExtender ID="popAdd" runat="server" BehaviorID="mpeAdd" TargetControlID="btndummy"
                        PopupControlID="pnlAdd">
                    </cc1:ModalPopupExtender>
                    <asp:Button ID="btndummy" runat="server" Style="display: none" />
                    <asp:HiddenField ID="hdnPop3" runat="server" />
                    <asp:Panel ID="pnlAdd" runat="server" CssClass="Popup" Style="width: 750px; height: auto;
                        display: none;">
                        <%-- display: none; --%>
                        <asp:Image ID="imgClose3" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                            margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closeAddPopup();" />
                        <center>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnladdplan" runat="server">
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
                                        <table width="90%" runat="server" id="AddExpiredplan">
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
                                                        <asp:ListItem Text="30 Days" Value="1" Selected="True"></asp:ListItem>
                                                        <%--<asp:ListItem Text="3 Month" Value="3" ></asp:ListItem>
                                                        <asp:ListItem Text="6 Month" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="12 Month" Value="12"></asp:ListItem>--%>
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
                                                    <asp:RadioButton ID="radPlanBasic" runat="server" GroupName="RadPlanType" Text="Basic"
                                                        AutoPostBack="true" OnCheckedChanged="radPlanBasic_CheckedChanged"  />
                                                    <asp:RadioButton ID="radhwayspecial" runat="server" GroupName="RadPlanType" Text="Hathway Bouquet"
                                                        AutoPostBack="true" OnCheckedChanged="radhwayspecial_CheckedChanged"  />
                                                    <asp:RadioButton ID="radPlanAD" runat="server" GroupName="RadPlanType" Text="Broadcaster Bouquet"
                                                        OnCheckedChanged="radPlanAD_CheckedChanged" AutoPostBack="true" />
                                                    <asp:RadioButton ID="radPlanADreg" runat="server" GroupName="RadPlanType" Text="Addon REG Bouquet"
                                                        OnCheckedChanged="radPlanAD_CheckedChanged" AutoPostBack="true" Visible="false" />
                                                    <asp:RadioButton ID="radPlanAL" runat="server" GroupName="RadPlanType" Text="A-La-Carte"
                                                        OnCheckedChanged="radPlanAL_CheckedChanged" AutoPostBack="true" />
                                                        <asp:RadioButton ID="radPlanAll" runat="server" GroupName="RadPlanType" Text="All"
                                                            OnCheckedChanged="radPlanAll_CheckedChanged" AutoPostBack="true" />
                                                    <%-- <asp:DropDownList ID="DDLPlanType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLPlanType_SelectedIndexChanged"
                                        Width="200">
                                        <asp:ListItem Selected="True" Text="Basic" Value="1"></asp:ListItem>
                                        <asp:ListItem  Text="Addon" Value="2"></asp:ListItem>
                                        <asp:ListItem  Text="A-La-Carte" Value="3"></asp:ListItem>
                                        </asp:DropDownList>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button runat="server" ID="btnsearchplan" Style="display: none;" Text="Search"
                                                                OnClick="btnsearchplan_Click" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <%--ValidationGroup="AddPopup"--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="2" style="color: #094791; font-weight: bold;">
                                                    &nbsp;&nbsp;&nbsp;Plan Details
                                                </td>
                                                <td align="right" style="color: #094791; font-weight: bold;">
                                                Selected Channels Count : <asp:Label ID="lblChannelcount" runat="server" Text="0"></asp:Label>&nbsp;&nbsp;
                                                    Total : &nbsp;<asp:Label ID="lbltotaladd" runat="server" Text="0.00/-"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                            <td align="center" colspan="3" >
                                            <asp:Panel runat="server" ID="pnlBC" Visible="true">
                                            <table>
                                            <tr>
                                            <td>
                                            &nbsp;&nbsp;&nbsp; Search By :&nbsp;&nbsp;&nbsp;</td>
                                            <td>
                                           <input name="txtTerm" onkeyup="filter2(this, '<%=grdPlanChan.ClientID %>')" type="text">
                                           </td>
                                           <td>
                                            <asp:Label runat="server" ID="lblBC" Text="Broadcaster"></asp:Label>&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>
                                           <asp:DropDownList runat="server" ID="ddlBC" ></asp:DropDownList>
                                           </td>
                                             <td>
                                           <asp:Button ID="btnsearchfilter2" runat="server" Text="Search"  OnClick="btnsearchfilter_Click" />
                                           </td>
                                           </tr>
                                           </table>
                                           <%--<input name="txtBC" onkeyup="" type="text">--%>
                                            </asp:Panel>


                                           <asp:Panel runat="server" ID="pnlAL" Visible="false">
                                           <table>
                                           <tr>
                                           <td>
                                            <asp:Label runat="server" ID="Label139" Text="Broadcaster"></asp:Label>&nbsp;&nbsp;&nbsp;</td>
                                            <td>
                                           <asp:DropDownList runat="server" ID="ddlBC2" ></asp:DropDownList><%--onclick="filterBROD(this, '<%=grdPlanChan.ClientID %>')"--%>
                                           </td>
                                           <td>
                                           &nbsp;&nbsp;&nbsp; <asp:Label runat="server" ID="Label147" Text="SD/HD"></asp:Label>&nbsp;&nbsp;&nbsp;
                                           </td>
                                           <td>
                                           <asp:DropDownList runat="server" ID="ddlSDHD" ><%--onclick="filterSDHD(this, '<%=grdPlanChan.ClientID %>')">--%>
                                           <asp:ListItem Value="0">All</asp:ListItem>
                                           <asp:ListItem Value="HD">HD</asp:ListItem>
                                           <asp:ListItem Value="SD">SD</asp:ListItem>
                                           </asp:DropDownList>
                                           </td>
                                           <td></td>
                                           </tr>
                                           <tr>
                                           <td>
                                            <asp:Label runat="server" ID="Label148" Text="Pay/Free"></asp:Label>&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>
                                           <asp:DropDownList runat="server" ID="ddlPAYFREE"><%-- onchange="filterFREEORPAID(this, '<%=grdPlanChan.ClientID %>')">--%>
                                           <asp:ListItem Value="0">All</asp:ListItem>
                                           <asp:ListItem Value="Y">FREE</asp:ListItem>
                                           <asp:ListItem Value="N">PAY</asp:ListItem>
                                           </asp:DropDownList>
                                           </td>
                                           <td>
                                          &nbsp;&nbsp;&nbsp; <asp:Label runat="server" ID="Label149" Text="Genre"></asp:Label>&nbsp;&nbsp;&nbsp;
                                           </td>
                                           <td>
                                           <asp:DropDownList runat="server" ID="ddlGener" >
                                           
                                           </asp:DropDownList>
                                           </td>
                                           <td>
                                           <asp:Button ID="btnshearchfilter" runat="server" Text="Search"  OnClick="btnsearchfilter_Click" />
                                           </td>
                                           </tr>
                                           <%--<input name="txtBC" onkeyup="" type="text"> onclick="filterGENER(this, '<%=grdPlanChan.ClientID %>')"  --%>
                                           

                                           
                                            </table>
                                            </asp:Panel>
                                            
                                            </td>
                                            
                                         </tr>

                                        </table>
                                        <table width="90%">
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Panel ID="pnlnewlangrd" runat="server" Style="max-height: 300px; height: auto;
                                                        overflow: auto; border: thin solid black; text-align: center">
                                                        <asp:Label ID="Label7" runat="server" Text="No Plan Found" Visible="false"></asp:Label>
                                                      <asp:GridView ID="grdPlanChan" Width="100%"  CssClass="Grid" runat="server" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="625" />
                                                                <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" />
                                                                <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" />
                                                                    <asp:BoundField HeaderText="BC Price" DataField="BC_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" />
                                                                <asp:BoundField HeaderText="Device Type" DataField="var_plan_devicetype" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" Visible="false" />
                                                                    <asp:BoundField HeaderText="SD Count" DataField="num_plan_sd_cnt" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" />
                                                                    <asp:BoundField HeaderText="HD Count" DataField="num_plan_hd_cnt" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" />
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Select" ItemStyle-Width="50">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkPlanAdd" runat="server" OnClick="javascript:calculateTotal(this);" />
                                                                       <%-- <asp:CheckBox ID="ChkPlanAdd" runat="server" OnClick="javascript:calculateTotalN(this);" />--%>
                                                                        <asp:HiddenField ID="hdnplanaddPlanPoid" runat="server" Value='<%# Eval("plan_poid").ToString()%>' />
                                                                        <asp:HiddenField ID="hdnplanaddDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                                        <asp:HiddenField ID="hdnplanaddproducteId" runat="server" Value='<%# Eval("product_poid").ToString()%>' />
                                                                        <asp:HiddenField ID="hdnplanaddplantype" runat="server" Value='<%# Eval("plan_type").ToString()%>' />
                                                                        <asp:HiddenField ID="hdnplanaddplanLCOPrice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                                        <asp:HiddenField ID="hdnplanaddplanBCPrice" runat="server" Value='<%# Eval("BC_PRICE").ToString()%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="AutoRenew" ItemStyle-Width="50" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkPlanAddRenew" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:BoundField HeaderText="Plan Type" DataField="plan_type" 
                                                                    ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                                     <asp:BoundField HeaderText="broad_name" DataField="broad_name" 
                                                                    ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                                     <asp:BoundField HeaderText="devicetype" DataField="var_plan_devicetype" 
                                                                    ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                                     <asp:BoundField HeaderText="var_plan_freeflag" DataField="var_plan_freeflag" 
                                                                    ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                                    <asp:BoundField HeaderText="genre_type" DataField="genre_type" 
                                                                    ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center">
                                                    <asp:Label ID="lblresponse" ForeColor="RED" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Button ID="btnAddPlan" runat="server" Width="60px" Text="Add" OnClick="btnAddPlan1_Click"
                                                        CommandName="add" />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="BtnRest" runat="server" Width="60px" Text="Reset" OnClick="Button2_Click"
                                                        Visible="false" />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <input type="button" value="Cancel" class="button" onclick="closeAddPopup();" />
                                                    <%--<asp:Button ID="btnCloseAdd" runat="server" Width="60px" Text="Cancel" 
                                    onclick="btnCloseAdd_Click"/> --%>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </center>
                    </asp:Panel>
                    <%-- ---------------------------------------------------ADD POPUP-------------------------------------------------- --%>
                    <cc1:ModalPopupExtender ID="popaddplanconfirm" runat="server" BehaviorID="mpeaddplanConfirmation"
                        TargetControlID="hdnaddplanconfirm" PopupControlID="pnladdplanconfirm">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnaddplanconfirm" runat="server" />
                    <asp:Panel ID="pnladdplanconfirm" runat="server" CssClass="Popup" Style="width: 600px;
                        max-height: 550px; display: none; height: auto;">
                        <%-- display: none; --%>
                        <asp:Image ID="Image13" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                            margin-top: -15px; margin-right: -15px;" onclick="closeaddplanconfrimPopup();"
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
                                        <asp:Label ID="lbladdplanPopupText1" runat="server" Text="This will add the plan with following details."></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Label ID="lbladdplanPopupText2" runat="server" Text="Are you sure you want to add?"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel2" runat="server" Style="max-height: 300px; height: auto; overflow: auto;
                                            border: thin solid black;">
                                            <asp:GridView ID="GrdaddplanConfrim" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                                                ShowFooter="true" OnRowDataBound="GrdaddplanConfrim_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"
                                                        FooterText="Current Selection :" FooterStyle-HorizontalAlign="Right" ItemStyle-Width="625" />
                                                    <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        <asp:BoundField HeaderText="BC Price" DataField="BC_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                    <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="100" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                    <asp:BoundField HeaderText="Discount" DataField="discount" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                    <asp:BoundField HeaderText="Net BASE Price" DataField="netmrp" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                        <asp:BoundField HeaderText="Channels Count" DataField="ChannelCount" ItemStyle-HorizontalAlign="Right"
                                                                    ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField HeaderText="Message" DataField="Message" ItemStyle-HorizontalAlign="Right"
                                                                    ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" FooterStyle-CssClass="Hide">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblautorenew" runat="server" Text='<%# Eval("AutoRenew").ToString()%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnaddplanconfmessage" runat="server" Value='<%# Eval("plan_poid").ToString()%>' />
                                                            <asp:HiddenField ID="hdnaddplanconfcode" runat="server" Value='<%# Eval("plan_poid").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanaddconfPlanPoid" runat="server" Value='<%# Eval("plan_poid").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanaddconfDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanaddconfproducteId" runat="server" Value='<%# Eval("productid").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanaddconfplantype" runat="server" Value='<%# Eval("plan_type").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanaddconfautorenew" runat="server" Value='<%# Eval("AutoRenew").ToString()%>' />
                                                            <asp:HiddenField ID="hdnfoctype" runat="server" Value='<%# Eval("foctype").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanaddLcoprice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                            <asp:HiddenField ID="hdnCode" runat="server" Value='<%# Eval("Code").ToString()%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
                             <table width="100%" style="border: 1px solid black; border-collapse: collapse; ">
                                <tr style="border: 1px solid black; border-collapse: collapse; background-color: #f5c080;">
                                    <td style="border: 1px solid black; border-collapse: collapse; width: 305px;" align="right">  Existing Selection :&nbsp;&nbsp;</td>
                                    <td style="border: 1px solid black; border-collapse: collapse; width: 63px;" align="right"> <asp:Label ID="lblExistBaseTotal" runat="server"></asp:Label> </td>
                                    <td style="border: 1px solid black; border-collapse: collapse; width: 63px;"align="right"> <asp:Label ID="lblExistLCOTotal" runat="server"></asp:Label> </td>
                                    <td style="border: 1px solid black; border-collapse: collapse; width: 57px;"align="right"> <asp:Label ID="lblExistBCTotal" runat="server"></asp:Label> </td>
                                    <td style="border: 1px solid black; border-collapse: collapse; width: 63px;"align="right"> <asp:Label ID="lblExistChannelTotal" runat="server"></asp:Label> </td>
                                </tr>
                                <tr style="border: 1px solid black; border-collapse: collapse; background-color: #6477bb;">
                                    <td style="border: 1px solid black; border-collapse: collapse; " align="right">  Overall Total :&nbsp;&nbsp;</td>
                                    <td style="border: 1px solid black; border-collapse: collapse; "align="right"> <asp:Label ID="lblOverallBaseTotal" runat="server"></asp:Label> </td>
                                    <td style="border: 1px solid black; border-collapse: collapse; "align="right"> <asp:Label ID="lblOverallLCOTotal" runat="server"></asp:Label> </td>
                                    <td style="border: 1px solid black; border-collapse: collapse; "align="right"> <asp:Label ID="lblOverallBCTotal" runat="server"></asp:Label> </td>
                                    <td style="border: 1px solid black; border-collapse: collapse; "align="right"> <asp:Label ID="lblOverallChannelTotal" runat="server"></asp:Label> </td>
                                </tr>
                            </table>
                            <br />
             
                                        </asp:Panel>
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
                                        <input type="button" class="button" value="Cancel" onclick="closeaddplanconfrimPopup();" />
                                        &nbsp;
                                        <asp:Button ID="btnaddplanConfirm" class="button" runat="server" Text="Confirm" OnClick="btnaddplanConfirm_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                    <%-- ---------------------------------------------------Renew ALL POPUP-------------------------------------------------- --%>
                    <cc1:ModalPopupExtender ID="popupRenewAll" runat="server" BehaviorID="mpeServiceS"
                        TargetControlID="HiddenField8" PopupControlID="pnlServiceS" CancelControlID="imgClose4">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="HiddenField8" runat="server" />
                    <asp:Panel ID="pnlServiceS" runat="server" CssClass="Popup" Style="width: 600px;
                        height: auto; display: none;">
                        <%-- display: none; --%>
                        <asp:Image ID="Image12" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                            margin-top: -15px; margin-right: -15px;" onclick="closeServicePopupS();" ImageUrl="~/Images/closebtn.png" />
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
                                        <asp:Label ID="Label105" runat="server" Text="Are you sure you want to Renew All?"></asp:Label>
                                    </td>
                                </tr>

                                      <tr>
                                <td align="center" colspan="3">
                                     <asp:Panel ID="PnlAllRenewConfirm" runat="server" Visible="true" Style="max-height: 300px; height: auto; overflow: auto;
                                            border: thin solid black;">
                                            <asp:GridView ID="GrdAllRenewConfirm" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                                                ShowFooter="true" >
                                                <Columns>
                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"
                                                      FooterText="Total :"  FooterStyle-HorizontalAlign="Right" ItemStyle-Width="500" />
                                                    <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                    <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="100" FooterStyle-HorizontalAlign="Right"  />
                                                        <asp:BoundField HeaderText="Channel Count" DataField="Channel_Count" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="100" FooterStyle-HorizontalAlign="Right"  />
                                                        <asp:BoundField HeaderText="Activation" DataField="Activation" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="115" Visible="false" />
                                                    <asp:BoundField ItemStyle-HorizontalAlign="Center" HeaderText="Valid Upto" DataField="valid_upto" ItemStyle-Width="115" />
                                                        
                                                </Columns>
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
                                        </asp:Panel>
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
                                        <asp:Button ID="BtnAutoRenewAll" OnClick="BtnAutoRenewAll_Click" runat="server" CssClass="button"
                                            Width="100px" Text="Confirm" />
                                        &nbsp;&nbsp;&nbsp;
                                        <input type="button" class="button" value="Cancel" style="width: 100px;" onclick="closeServicePopupS();" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                    <%-- ---------------------------------------------------Renew ALl POPUP-------------------------------------------------- --%>
                  
                    <%-- ---------------------------------------------------Cancel ALl POPUP-------------------------------------------------- --%>
                   <cc1:ModalPopupExtender ID="popBulkCancel" runat="server" BehaviorID="mpeBulkCancel"
                    TargetControlID="hdnBulkCancel" PopupControlID="pnlBulkCancel">
                </cc1:ModalPopupExtender>

                <asp:HiddenField ID="hdnBulkCancel" runat="server" />

                <asp:Panel ID="pnlBulkCancel" runat="server" CssClass="Popup" Style="width: 700px; display: none; height: 300px;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image22" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closeBulkCancelPopup();" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label150" runat="server" ForeColor="#094791" Font-Bold="true" Text="Cancel All"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:CheckBox ID="choBulkCancel" runat="server" OnCheckedChanged="choBulkCancel_click" AutoPostBack="true" />
                                    <%--<asp:CheckBox ID="choBulkCancel" runat="server"  onclick="FunChkBulkCancel(this);" />--%>
                                    <asp:Label runat="server" ID="Label151" Text="Cancel All" ></asp:Label>
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
                        <asp:Label runat="server" ID="lblmutlcnlmsg" ForeColor="Red" ></asp:Label>
                        </td>
                        </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <div class="plan_scroller">
                                        <asp:UpdatePanel ID="updatepanel9" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdBulkCancel" EmptyDataText="No Plan Found" CssClass="Grid" runat="server"  
                                                    AutoGenerateColumns="false"  >
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="350px"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField HeaderText="Cancel" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkBulkCancel" runat="server"  OnCheckedChanged="lnkView_click" AutoPostBack="true" />
                                                                <%--<asp:CheckBox ID="chkBulkCancel" runat="server" chkType='<%# Eval("PLAN_TYPE").ToString()%>'  OnClick='<%# "ChkcountBulkCancelStatus(this,\"" + Eval("PLAN_TYPE") + "\"); " %>' />  --%>
                                                                <asp:HiddenField ID="hdnPlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                                <asp:HiddenField ID="hdnDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                                <asp:HiddenField ID="hdnCustPrice" runat="server" Value='<%# Eval("CUST_PRICE").ToString()%>' />
                                                                <asp:HiddenField ID="hdnLcoPrice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                                <asp:HiddenField ID="hdnActivation" runat="server" Value='<%# Eval("Activation").ToString()%>' />
                                                                <asp:HiddenField ID="hdnExpiry" runat="server" Value='<%# Eval("valid_upto").ToString()%>' />
                                                                <asp:HiddenField ID="hdnPackageId" runat="server" Value='<%# Eval("PackageId").ToString()%>' />
                                                                <asp:HiddenField ID="hdnPurchasePoid" runat="server" Value='<%# Eval("PurchasePoid").ToString()%>' />
                                                                <asp:HiddenField ID="hdnPlanType" runat="server" Value='<%# Eval("PLAN_TYPE").ToString()%>' />
                                                                 <asp:HiddenField ID="hdnPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                <asp:HiddenField ID="hdnChannelCount" runat="server" Value='<%# Eval("ChannelCount").ToString()%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="PlanType" DataField="PLAN_TYPE" ItemStyle-Width="350px"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
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
                                    <asp:Button ID="Button16" runat="server" CssClass="button" Text="OK" Visible="true"
                                        Width="100px" OnClick="btnShowCancel_Click" />&nbsp; &nbsp;&nbsp;
                                    <input id="Button17" class="button" runat="server" type="button" value="Cancel"
                                        style="width: 100px;" onclick="closeBulkCancelPopup();" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                 <%-- ---------------------------------------------------Cancel ALl POPUP-------------------------------------------------- --%>
                  
                  
                    <%-- ---------------------------------------------------RENEW NOW POPUP-------------------------------------------------- --%>
                    <cc1:ModalPopupExtender ID="PopUpRenewNow" runat="server" BehaviorID="mpeRenewNow"
                        TargetControlID="hdnRenewNow" PopupControlID="pnlRenewNow">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnRenewNow" runat="server" />
                    <asp:Panel ID="pnlRenewNow" runat="server" CssClass="Popup" Style="width: 400px;
                        display: none; height: 300px;">
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
                                            <asp:ListItem Text="Hathway Specal" Value="HSP"></asp:ListItem>
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
                                                        <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="left"
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
                                        <input id="Button4" class="button" runat="server" type="button" value="No" style="width: 100px;"
                                            onclick="closeRenewNowConfPopup();" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                    <%-- ---------------------------------------------------CHANGE PAYTERM POPUP-------------------------------------------------- --%>
                    <cc1:ModalPopupExtender ID="popChangePayTerm" runat="server" BehaviorID="mpeChangePayTerm"
                        TargetControlID="hdnPopChangePayTerm" PopupControlID="pnlChangePayTerm">
                    </cc1:ModalPopupExtender>
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

                            <%-- ---------------------------------------------------Basic Add POPUP-------------------------------------------------- --%>
                    <cc1:ModalPopupExtender ID="popBasicAdd" runat="server" BehaviorID="mpeBasicAdd" TargetControlID="hdnPopBasicAdd"
                        PopupControlID="pnlBasicAdd">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnPopBasicAdd" runat="server" />
                    <asp:Panel ID="pnlBasicAdd" runat="server" CssClass="Popup" Style="width: 700px; height: auto;max-height:550px;
                    display: none;">
                        <center>
                            <br />
                            <table width="100%">
                                <tr>
                                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                        &nbsp;&nbsp;&nbsp;Basic ADD Plan
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table width="95%">
                                <tr id="tr10" runat="server">
                                    <td align="left" width="100px">
                                        <asp:Label ID="Label163" Font-Bold="true" runat="server" Text="Plan Payterm"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="Label164" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="rbtnPayterm_SelectedIndexChanged">
                                            <asp:ListItem Text="30 Days" Value="1" Selected="True"></asp:ListItem>
                                            <%--<asp:ListItem Text="3 Month" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="6 Month" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="12 Month" Value="12"></asp:ListItem>--%>
                                        </asp:RadioButtonList>
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
                <table width="95%">
                <tr>
            <td colspan="3">
                <asp:Label ID="Label168" runat="server" Text="No Plan Found" Visible="false"></asp:Label>
                <asp:Panel ID="Panel8" runat="server" style="height:auto;max-height:300px;overflow:auto;">
               
            <asp:GridView ID="grdBasicADD" CssClass="Grid" runat="server" AutoGenerateColumns="false" ShowFooter="false">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="625" />
                                                                <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" />
                                                                <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" />
                                                                    <asp:BoundField HeaderText="BC Price" DataField="BC_PRICE" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" />
                                                                <asp:BoundField HeaderText="Device Type" DataField="var_plan_devicetype" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" Visible="false" />
                                                                    <asp:BoundField HeaderText="SD Count" DataField="num_plan_sd_cnt" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" />
                                                                    <asp:BoundField HeaderText="HD Count" DataField="num_plan_hd_cnt" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="80" />
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Select" ItemStyle-Width="50">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkPlanAdd" runat="server" OnClick="selectBasic_one(this);" />
                                                                       <%-- <asp:CheckBox ID="ChkPlanAdd" runat="server" OnClick="javascript:calculateTotalN(this);" />--%>
                                                                        <asp:HiddenField ID="hdnplanaddPlanPoid" runat="server" Value='<%# Eval("plan_poid").ToString()%>' />
                                                                        <asp:HiddenField ID="hdnplanaddDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                                        <asp:HiddenField ID="hdnplanaddproducteId" runat="server" Value='<%# Eval("product_poid").ToString()%>' />
                                                                        <asp:HiddenField ID="hdnplanaddplantype" runat="server" Value='<%# Eval("plan_type").ToString()%>' />
                                                                        <asp:HiddenField ID="hdnplanaddplanLCOPrice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                                        <asp:HiddenField ID="hdnplanaddplanBCPrice" runat="server" Value='<%# Eval("BC_PRICE").ToString()%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="AutoRenew" ItemStyle-Width="50" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkPlanAddRenew" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:BoundField HeaderText="Plan Type" DataField="plan_type" 
                                                                    ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                                     <asp:BoundField HeaderText="broad_name" DataField="broad_name" 
                                                                    ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                                     <asp:BoundField HeaderText="devicetype" DataField="var_plan_devicetype" 
                                                                    ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                                     <asp:BoundField HeaderText="var_plan_freeflag" DataField="var_plan_freeflag" 
                                                                    ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                                    <asp:BoundField HeaderText="genre_type" DataField="genre_type" 
                                                                    ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                                </Columns>
                                                                <FooterStyle CssClass="GridFooter" />
                                                            </asp:GridView>
                                                            <asp:HiddenField ID="HiddenField11" runat="server" />
                                                            <asp:HiddenField ID="HiddenField12" runat="server" />
                                                             </asp:Panel>
            </td>
            </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="Label169" ForeColor="RED" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="Button20" runat="server" Width="60px" Text="ADD" OnClick="btnBasicADDPlan_Click"
                                CommandName="Change" />
                            &nbsp;&nbsp;&nbsp;
                            <input type="button" value="Cancel" onclick="closeBasicADDPopup();" class="button" />
                            <%--<asp:Button ID="btnCloseAdd" runat="server" Width="60px" Text="Cancel" 
                                    onclick="btnCloseAdd_Click"/> --%>
                        </td>
                    </tr>
                </table>
                </center> </asp:Panel>


                     <%-- ---------------------------------------------------Lacarte Baseplan change CONFIRMATION POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="popalacartebasechange" runat="server" BehaviorID="mpealacartebasechange"
                    TargetControlID="hdnalacartebasechange" PopupControlID="pnlalacartebasechange">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnalacartebasechange" runat="server" />
                <asp:Panel ID="pnlalacartebasechange" runat="server" CssClass="Popup" Style="width: 430px;
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
                                    <asp:Label ID="Label8" runat="server" Text="You have chosen Ala-Carte pack, All Other Active Add-on packs will get cancel. Do want to continue."></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="btnalacartechangeconfirm" runat="server" CssClass="button" Text="Yes" Width="100px"
                                        OnClick="btnalacartechangeconfirm_Click" />
                                    &nbsp;&nbsp;
                                    <input id="Button8" class="button" runat="server" type="button" value="No" style="width: 100px;"
                                        onclick="closealacartechangeconfirm();" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>

                    <%-- ---------------------------------------------------CHANGE POPUP-------------------------------------------------- --%>
                    <cc1:ModalPopupExtender ID="popchange" runat="server" BehaviorID="mpeChange" TargetControlID="hdnPopChange"
                        PopupControlID="pnlChange">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnPopChange" runat="server" />
                    <asp:Panel ID="pnlChange" runat="server" CssClass="Popup" Style="width: 700px; height: auto;max-height:550px;
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
                                            <asp:ListItem Text="30 Days" Value="1"></asp:ListItem>
                                            <%--<asp:ListItem Text="3 Month" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="6 Month" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="12 Month" Value="12"></asp:ListItem>--%>
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
                                <%--<tr>
                                    <td align="left" valign="top" width="100px">
                                        <asp:Label ID="Label49" Font-Bold="true" runat="server" Text="Plan"></asp:Label><asp:Label
                                            ID="Label50" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    </td>
                                    <td width="5px">
                                        <asp:Label ID="LabelCol" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        &nbsp;&nbsp;<asp:DropDownList
                                            ID="ddlPlanChange" runat="server" OnSelectedIndexChanged="ddlPlanChange_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                        <br />
                                        </tr>--%>
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
            <td colspan="3">
                <asp:Label ID="lblchangeplannotfount" runat="server" Text="No Plan Found"></asp:Label>
                <asp:Panel ID="Panel3" runat="server" style="height:auto;max-height:300px;overflow:auto;">
               
            <asp:GridView ID="GrdchangePlan" CssClass="Grid" runat="server" AutoGenerateColumns="false" ShowFooter="false">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"  FooterStyle-HorizontalAlign="Right"
                                                                        ItemStyle-Width="625" />
                                                                     <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"
                                                                        ItemStyle-Width="80" />
                                                                    <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100" FooterStyle-HorizontalAlign="Right"/>
                                                                    <asp:BoundField HeaderText="Discount" DataField="discount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right"/>
                                                                    <asp:BoundField HeaderText="Net BASE Price" DataField="netmrp" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right"/>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Select" ItemStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="rbtnchangeplanselect" runat="server" OnClick = "select_one(this);"/>
                                                                            <asp:HiddenField ID="hdnplanaddconfPlanPoid" runat="server" Value='<%# Eval("plan_poid").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnplanaddconfDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                                            
                                                                            <asp:HiddenField ID="hdnplanaddconfplantype" runat="server" Value='<%# Eval("plan_type").ToString()%>' />
                                                                            <asp:HiddenField ID="hdnalacartechange" runat="server" Value='<%# Eval("ALACARTEBASE").ToString()%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                     <asp:BoundField HeaderText="Plan Type" DataField="plan_type" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"
                                                                        ItemStyle-Width="80" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                                                </Columns>
                                                                <FooterStyle CssClass="GridFooter" />
                                                            </asp:GridView>
                                                            <asp:HiddenField ID="Hidchangplan" runat="server" />
                                                            <asp:HiddenField ID="Hidchangplanlco" runat="server" />
                                                             </asp:Panel>
            </td>
            </tr>
                   <%-- <tr>
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
                        <td align="left" valign="top" width="65px">
                            <asp:Label ID="Label8" Font-Bold="true" runat="server" Text="LCO Price"></asp:Label>
                        </td>
                        <td width="5px">
                            :
                        </td>
                        <td align="left" width="220px">
                            <asp:Label ID="lblChangeplanLCO" runat="server" Text=""></asp:Label>
                            <asp:HiddenField ID="Hidchangplanlco" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="65px">
                            <asp:Label ID="Label109" Font-Bold="true" runat="server" Text="Discount"></asp:Label>
                        </td>
                        <td width="5px">
                            <asp:Label ID="Label110" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left" width="220px">
                            <asp:Label ID="lblchangediscount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>--%>
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
                <cc1:ModalPopupExtender ID="MPEConfirmation" runat="server" BehaviorID="mpeChangeConfirmation"
                    TargetControlID="hdnChangeConfirmPop" PopupControlID="pnlChangeConfirmation">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnChangeConfirmPop" runat="server" />
                <asp:Panel ID="pnlChangeConfirmation" runat="server" CssClass="Popup" Style="width: 560px;
                    display: none; height: auto;">
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
                        <table width="95%">
                            <tr>
                                <td align="left" width="200px">
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
                                        <asp:Label ID="Label152" runat="server" Text="Remaining Days"></asp:Label></b>
                                </td>
                                <td>
                                    <asp:Label ID="Label153" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblRemainingDays" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <b>
                                        <asp:Label ID="Label66" runat="server" Text="Customer Refund Amount"></asp:Label></b>
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
                                    <asp:Label ID="Label117" runat="server" Text="LCO Refund Amount"></asp:Label></b>
                            </td>
                            <td>
                                <asp:Label ID="Label119" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblConfirmRefundlcoAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                            <tr>
                                <td align="left">
                                    <b>
                                        <asp:Label ID="Label70" runat="server" Text="Customer to Pay"></asp:Label></b>
                                </td>
                                <td>
                                    <asp:Label ID="Label71" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="payamount" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                             <tr>
                            <td align="left">
                                <b>
                                    <asp:Label ID="Label120" runat="server" Text="LCO to Pay"></asp:Label></b>
                            </td>
                            <td>
                                <asp:Label ID="Label121" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="payamountLCO" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                         <td align="left">
                                    <b>
                                        <asp:Label ID="Label156" runat="server" Text="Pro-rata Customer Refund for NCF, BC and ALC Pack"></asp:Label></b>
                                </td>
                                <td>
                                    <asp:Label ID="Label157" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblOtherCustRefund" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            <td align="left">
                                <b>
                                    <asp:Label ID="Label159" runat="server" Text="Pro-rata LCO Refund for NCF, BC and ALC Pack"></asp:Label></b>
                            </td>
                            <td>
                                <asp:Label ID="Label160" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblOtherLCORefund" runat="server" Text=""></asp:Label>
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


                          <cc1:ModalPopupExtender ID="mpeChangeplan" runat="server" BehaviorID="mpeChangeplanconf"
                        TargetControlID="hdnChangeplanconfPOP" PopupControlID="pnlChangeplanconfPOP">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnChangeplanconfPOP" runat="server" />
                    <asp:Panel ID="pnlChangeplanconfPOP" runat="server" CssClass="Popup" Style="width: 430px;
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
                                        <asp:Label ID="Label154" runat="server" Text="Change Plan will invoke cancellation of existing Base Pack, NCF, Add-on & Ala-Carte packs. Pro-rata credit for balance period will be generated."></asp:Label>
                                        <br />
                                        <asp:Label ID="Label155" runat="server" Text="Do you want to Continue ?"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Button ID="btnChangePlanContinue" runat="server" CssClass="button" Text="Continue"
                                            Width="100px" OnClick="btnChangePlanContinue_click" />
                                        &nbsp;&nbsp;
                                    <input type="button" class="button" value="Cancel" onclick="closeChangeConfPopup();" />
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>

       
                <%-- ---------------------------------------------------MESSAGE POPUP-------------------------------------------------- --%>
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
                                    <asp:Button ID="btnRefreshForm" runat="server" CssClass="button" Text="OK" Visible="false"
                                        Width="100px" OnClick="btnRefreshForm_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <%-- ---------------------------------------------------MESSAGE POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="popFOCMsg" runat="server" BehaviorID="mpeFOCMsg" TargetControlID="HiddenField6"
                    PopupControlID="pnlFOCMessgae">
                </cc1:ModalPopupExtender>
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
                                        style="width: 100px;" onclick="closeFOCMsgPopupALL();" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <%-- ---------------------------------------------------SERVICE ACT/DEACT POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="popService" runat="server" BehaviorID="mpeService" TargetControlID="hdnPop4"
                    PopupControlID="pnlService" CancelControlID="imgClose4">
                </cc1:ModalPopupExtender>
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

                 <%-- ---------------------------------------------------DATE ALIGN POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="popAlignDate" runat="server" BehaviorID="mpeAlignDate" TargetControlID="hdnPopAlignDate"
                    PopupControlID="pnlAlignDate">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnPopAlignDate" runat="server" />
                <asp:Panel ID="pnlAlignDate" runat="server" CssClass="Popup" Style="width: 430px; height: 160px;
                    display: none;">
                    <%-- display: none; --%>
                    <asp:Image ID="imgAlignDate" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" onclick="closeAlignDatePopup();" ImageUrl="~/Images/closebtn.png" />
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
                                    <asp:Label ID="lblALignDate" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <%--<input id="Button23" class="button" runat="server" type="button" value="Close"
                                        style="width: 100px;" onclick="closeMsgPopup();" />--%>
                                    <asp:Button ID="btnYES" runat="server" CssClass="button" Text="YES" Visible="true"
                                        Width="100px"  OnClick="btnYes_Click"/>
                                         <asp:Button ID="btnNO" runat="server" CssClass="button" Text="NO" Visible="true"
                                        Width="100px" OnClick="btnNo_Click" /> <%--OnClick="btnRefreshForm_Click" --%>
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <%-- ---------------------------------------------------SERVICE ACT/DEACT INFORMATION POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="popServiceInfo" runat="server" BehaviorID="mpeServiceInfo"
                    TargetControlID="hdnPop5" PopupControlID="pnlServiceInfo" CancelControlID="imgClose5">
                </cc1:ModalPopupExtender>
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
                <cc1:ModalPopupExtender ID="popFinalConf" runat="server" BehaviorID="mpeFinalConf"
                    TargetControlID="hdnPop7" PopupControlID="pnlFinalConfirm">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnPop7" runat="server" />
                <asp:Panel ID="pnlFinalConfirm" runat="server" CssClass="Popup" Style="width: 750px;
                    display: none; height: auto;">
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
                                    <td>
                                        <asp:Panel ID="Panel6" runat="server" Style="max-height: 300px; height: auto; overflow: auto;
                                            border: thin solid black;">
                                            <asp:GridView ID="GrdRenewConfrim2" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                                                ShowFooter="true" >
                                                <Columns>
                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"
                                                        FooterText="Total :" FooterStyle-HorizontalAlign="Right" ItemStyle-Width="625" />
                                                    <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        
                                                        <asp:BoundField HeaderText="LCO Refund" DataField="refund_lcoamt" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        <asp:BoundField HeaderText="Cust Refund" DataField="refund_amt" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        <asp:BoundField HeaderText="Days Remaining" DataField="days_left" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                    <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="100" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                    <asp:BoundField HeaderText="Discount" DataField="discount" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                    <asp:BoundField HeaderText="Net BASE Price" DataField="netmrp" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                        <asp:BoundField HeaderText="Activation" DataField="Activation" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="100" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Valid Upto" ItemStyle-Width="100">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblValid_upto" runat="server" Text='<%# Eval("valid_upto").ToString()%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnplanrenewconfPlanPoid" runat="server" Value='<%# Eval("plan_poid").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanrenewconfDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanrenewconfplantype" runat="server" Value='<%# Eval("plan_type").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanrenewconfautorenew" runat="server" Value='<%# Eval("AutoRenew").ToString()%>' />
                                                            <asp:HiddenField ID="hdnplanaddLcoprice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
      
                        <table width="90%">
                            
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
                <cc1:ModalPopupExtender ID="popretrctservice" runat="server" BehaviorID="mpeServceretrctConf"
                    TargetControlID="hdnPop10" PopupControlID="Panel1" CancelControlID="imgClose4">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnPop10" runat="server" />
                <asp:Panel ID="Panel1" runat="server" CssClass="Popup" Style="width: 430px; height: 180px;
                    display: none;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image5" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" onclick="closeRtractFinalConfPopup();" ImageUrl="~/Images/closebtn.png" />
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

                 <%-- ---------------------------------------------------Lacarte Baseplan CONFIRMATION POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="popalacrtebaseplan" runat="server" BehaviorID="mpealacrtebaseplan"
                    TargetControlID="hdnalacrtebaseplan" PopupControlID="pnlalacrtebaseplan">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnalacrtebaseplan" runat="server" />
                <asp:Panel ID="pnlalacrtebaseplan" runat="server" CssClass="Popup" Style="width: 430px;
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
                                    <asp:Label ID="lblalacartetext" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    
                                    <input id="Button7" class="button" runat="server" type="button" value="Close" style="width: 100px;"
                                        onclick="closealacrtebaseplan();" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <%-- ---------------------------------------------------Free AL Plan POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="PopUpALFreePlan" runat="server" BehaviorID="mpeALFreePlan"
                    TargetControlID="hdnALFreePlan" PopupControlID="pnlALFreePlan">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnALFreePlan" runat="server" />
                <asp:Panel ID="pnlALFreePlan" runat="server" CssClass="Popup" Style="width: 650px;
                    display: none; height: 600px">
                    <asp:Image ID="Image19" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closeALFreePlanPopup();" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="A-La-Carte Free Channels Plan"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td align="left" style="color: #094791; font-weight: bold;">
                                    &nbsp;&nbsp;Plan Details
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="color: #094791; font-weight: bold;">
                                    Total Base Price : &nbsp;<asp:Label ID="lbltotalbaseprice" runat="server" Text="0.00/-"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td align="right" style="color: #094791; font-weight: bold;">
                                    Remaining Base Price : &nbsp;<asp:Label ID="lbltoatlalbaseremain" runat="server"
                                        Text="0.00/-"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td align="right" style="color: #094791; font-weight: bold;">
                                    Used Base Price : &nbsp;<asp:Label ID="lblALtotal" runat="server" Text="0.00/-"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <table width="90%">
                            <tr>
                                <td colspan="3" align="center">
                                    <div style="overflow: auto; max-height: 400px">
                                        <asp:UpdatePanel ID="updatepanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdALfree" EmptyDataText="No Plan Found" CssClass="Grid" runat="server"
                                                    AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-Width="500px"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="BASE Price" DataField="cust_price" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="LCO Price" DataField="lco_price" ItemStyle-Width="200px"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblAlFreePlan" runat="server" Text="Select Plan"></asp:Label>
                                                                <br />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbALFreePlan" runat="server" AutoPostBack="true" OnCheckedChanged="cbALFreePlan_Changed" />
                                                                <%--<asp:HiddenField ID="hdnfullname" runat="server" Value='<%# Eval("fullname")%>' />--%>
                                                                <itemstyle horizontalalign="Center" />
                                                                <%-- <asp:HiddenField ID="hdnFreePlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />--%>
                                                                <asp:HiddenField ID="hdnALFreePlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                                <asp:HiddenField ID="hdnALFreePlanbaseprice" runat="server" Value='<%# Eval("base_price").ToString()%>' />
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
                                    <asp:Button ID="btnalfreeadd" Visible="false" runat="server" Width="75px" Text="CONTINUE"
                                        CommandName="Add" OnClick="btnAddALFreePlan_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnalfreeclose" runat="server" Width="60px" Text="Cancel" Visible="false"
                                        OnClientClick="closeALFreePlanPopup();" />
                                    <asp:Label ID="Label14" runat="server" ForeColor="Red"></asp:Label>
                                    <%--<asp:Button ID="btnCloseAdd" runat="server" Width="60px" Text="Cancel" 
                                    onclick="btnCloseAdd_Click"/> --%>
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <%-- ---------------------------------------------------Free Plan POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="PopUpFreePlan" runat="server" BehaviorID="mpeFreePlan"
                    TargetControlID="hdnFreePlan" PopupControlID="pnlFreePlan">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnFreePlan" runat="server" />
                <asp:Panel ID="pnlFreePlan" runat="server" CssClass="Popup" Style="width: 400px;
                    display: none; height: 300px;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image6" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closeFreePlanPopup();" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lbleligfreeplan" runat="server" Text=""></asp:Label>
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
                                                    AutoGenerateColumns="false" >
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
                                                                <asp:HiddenField ID="hdnfreeplanlanguage" runat="server" Value='<%# Eval("language").ToString()%>' />
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
                                    <asp:TextBox runat="server" Width="150px" ID="txtModifyFirstName" MaxLength="100" 
                                    ></asp:TextBox>
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
                                    <asp:TextBox runat="server" Width="150px" ID="txtModifyMiddleName"  MaxLength="100"></asp:TextBox>
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
                                    <asp:TextBox runat="server" Width="150px" ID="txtModifylastName"   MaxLength="100"></asp:TextBox>
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
                                    <asp:Label ID="Lab2000" runat="server" Text="(*) Are mandatory field" ForeColor="Red"></asp:Label>
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
                <cc1:ModalPopupExtender ID="popallrenewal" runat="server" BehaviorID="mpeRenewPlan"
                    TargetControlID="hdnrenewPlan" PopupControlID="pnlrenewalPlan">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnrenewPlan" runat="server" />
                <asp:Panel ID="pnlrenewalPlan" runat="server" CssClass="Popup" Style="width: 700px;
                    display: none; height: auto;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image7" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px; display: none;" ImageUrl="~/Images/closebtn.png"
                        onclick="closerenewalPlanPopup();" />
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
                                <asp:Label ID="lblPlanStatus" runat="server" ForeColor="#094791" Font-Bold="true"
                                        Text=""></asp:Label>
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
                <%-- ---------------------------------------------------Cancel confirmatiom pop-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="popallCancel" runat="server" BehaviorID="mpeCancelPlan"
                    TargetControlID="HiddenField7" PopupControlID="pnlCancellPlan">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="HiddenField7" runat="server" />
                <asp:Panel ID="pnlCancellPlan" runat="server" CssClass="Popup" Style="width: 700px;
                    display: none; height: 300px;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image11" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="closecancelPlanPopup();" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label104" runat="server" ForeColor="#094791" Font-Bold="true" Text="Cancel Pack Status"></asp:Label>
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
                                        <asp:UpdatePanel ID="updatepanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdAllCancel" EmptyDataText="No Plan Found" CssClass="Grid" runat="server"
                                                    AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Vc Id" DataField="VCID" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Plan Name" DataField="PlanName" ItemStyle-Width="350px"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Cancel Status" DataField="Status" ItemStyle-Width="700px"
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
                                    <asp:Button ID="Button5" runat="server" CssClass="button" Text="OK" Visible="true"
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
                                    <asp:Label ID="Label87" runat="server" ForeColor="#094791" Font-Bold="true" Text="Auto Renewal"></asp:Label>
                                </td>
                                <td align="left"  colspan="3">
                                <asp:Label runat="server" ID="Label162" Text="Date :"></asp:Label>
                                <asp:TextBox ID="txtDate" runat="server" Width="100px"></asp:TextBox>
                                                            <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtDate" PopupButtonID="imgFrom"
                                        Format="dd-MMM-yyyy"  >
                                    </cc1:CalendarExtender>
                                </td>
                                <td align="right">
                                    <asp:CheckBox ID="choAutorenewAll" runat="server" onclick="FunChkDisableauto(this);" />
                                    <asp:Label runat="server" ID="Label88" Text="All Renewal"></asp:Label>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
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
                                                                <asp:CheckBox ID="chkAutoRenew" runat="server" onclick="ChkcountAuto(this)" />
                                                                <asp:HiddenField ID="hdnPlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                                <asp:HiddenField ID="hdnAutoStatus" runat="server" Value='<%# Eval("RenewStatus").ToString()%>' />
                                                                <asp:HiddenField ID="hdnExpiry" runat="server" Value='<%# Eval("EXPIRY").ToString()%>' />
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
                                        Width="100px" OnClick="btnAutoRenew_Click" />&nbsp; &nbsp;&nbsp;
                                    <input id="btnAutopopClose" class="button" runat="server" type="button" value="Cancel"
                                        style="width: 100px;" onclick="closeAutoRenewalPopup();" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
                <%-------------------------------------------------End Plans For AutoRenewal-----------------------------------%>
                <%-- ---------------------------------------------------Discount POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="mpeDiscnt" runat="server" BehaviorID="mpediscount" TargetControlID="HdnDiscount"
                    PopupControlID="pnlDiscount">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="HdnDiscount" runat="server" />
                <asp:Panel ID="pnlDiscount" runat="server" CssClass="Popup" Style="width: 750px;
                    height: 470px; display: none;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image14" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" onclick="closeDiscount();" ImageUrl="~/Images/closebtn.png" />
                    <table style="width: 100%;">
                        <tr>
                            <td align="left">
                                <b>Discount</b>
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table width="700px">
                        <tr id="tr2" runat="server">
                            <td align="center">
                                <div id="lcodet" class="delInfo" runat="server">
                                    <table width="100%" id="Table1" runat="server">
                                        <tr id="tr3" runat="server" visible="false">
                                            <td align="left">
                                                <asp:Label ID="Label122" runat="server" Text="Customer Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label123" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblCustName1" runat="server" Text=""></asp:Label>
                                            </td>
                                            <%-- <td align="left" rowspan="4" >
                                                <asp:GridView ID="GridVC" CssClass="Grid" runat="server" AutoGenerateColumns="true" Height="100%" Width="60%">
                                                             
                                                 </asp:GridView>
                                                               
                                            </td>--%>
                                        </tr>
                                        <tr id="Tr4" runat="server">
                                            <td align="left">
                                                <asp:Label ID="Label125" runat="server" Text="Customer A/C No."></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label126" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblCustNo1" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr5" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label ID="Label128" runat="server" Text="VC Id"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label129" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblVCID1" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr6" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label ID="Label131" runat="server" Text="STB No."></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label132" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblStbNo1" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr7" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label ID="Label134" runat="server" Text="Customer Mobile"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label135" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lbltxtmobno1" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr8" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label ID="Label137" runat="server" Text="Email Id"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label138" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblemail1" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr9" runat="server">
                                            <td align="left" width="130px">
                                                <asp:Label ID="Label124" runat="server" Text="Customer Address"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label127" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblCustAddr1" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table width="700px">
                        <tr>
                            <td align="center">
                                <div class="delInfo" id="div2" runat="server">
                                    <table runat="server" align="center" id="Table2" border="0">
                                        <tr>
                                            <td align="left" class="style67">
                                                <asp:Label ID="lblordernoo" runat="server" Text="Discount Amount"></asp:Label>
                                            </td>
                                            <td align="center" class="style68">
                                                <asp:Label ID="Label140" runat="server" Text=":"></asp:Label>
                                                <%--<asp:RegularExpressionValidator ID="Regex1" runat="server" ValidationExpression="((\d+)((\.\d{1,2})?))$"
ErrorMessage="Only 2 Digits Are Allowed After Decimal" ValidationGroup="Validate1"
ControlToValidate="txtAmount" />--%>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtdisamt" runat="server" MaxLength="4" onkeypress="return isNumber(event)"
                                                    Width="194px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="style67">
                                                <asp:Label ID="lblordernoo0" runat="server" Text="Discount Type"></asp:Label>
                                            </td>
                                            <td align="center" class="style68">
                                                <asp:Label ID="Label141" runat="server" Text=":"></asp:Label>
                                                <%--<asp:RegularExpressionValidator ID="Regex1" runat="server" ValidationExpression="((\d+)((\.\d{1,2})?))$"
ErrorMessage="Only 2 Digits Are Allowed After Decimal" ValidationGroup="Validate1"
ControlToValidate="txtAmount" />--%>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlbillno" runat="server" Height="19px" Visible="true" Width="304px">
                                                    <asp:ListItem Value="A">Amount</asp:ListItem>
                                                    <asp:ListItem Value="P">Percentage</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="style67">
                                                <asp:Label ID="lblbillrefnoo" runat="server" Text="Valid Upto"></asp:Label>
                                            </td>
                                            <td align="center" class="style68">
                                                &nbsp;<asp:Label ID="Label142" runat="server" Text=":"></asp:Label>
                                                <%--<asp:RegularExpressionValidator ID="Regex1" runat="server" ValidationExpression="((\d+)((\.\d{1,2})?))$"
ErrorMessage="Only 2 Digits Are Allowed After Decimal" ValidationGroup="Validate1"
ControlToValidate="txtAmount" />--%>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtexpdt" runat="server" Height="21px" Width="157px"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                    PopupButtonID="imgto" TargetControlID="txtexpdt">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgto" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    Width="16px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="style67">
                                                <asp:Label ID="Label143" runat="server" Text="Discount Reason"></asp:Label>
                                            </td>
                                            <td align="center" class="style68">
                                                <asp:Label ID="Label144" runat="server" Text=":"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtReason" runat="server" Height="57px" TextMode="MultiLine" Width="210px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Label ID="lblResponseMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table width="700px">
                        <tr>
                            <td align="center">
                                <div class="delInfo" id="div3" runat="server">
                                    <table runat="server" align="center" width="75%" id="Table3" border="0">
                                        <tr>
                                            <td align="right">
                                                <input id="Button2" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                                    onclick="closeDiscount();" />
                                            </td>
                                            <td align="left">
                                                &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                                    OnClientClick="return Confirm();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%-- ---------------------------------------------------Action POPUP Basic (renew cnacel change)-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="mpeActions" runat="server" BehaviorID="mpeActions" TargetControlID="hdnpnlActionn"
                    PopupControlID="pnlActionn">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnpnlActionn" runat="server" />
                <asp:Panel ID="pnlActionn" runat="server" CssClass="Popup" Style="width: auto; height: auto;
                    display: none;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image15" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" onclick="CloseActiosPop();" ImageUrl="~/Images/closebtn.png" />
                    <center>
                        <br />
                    </center>
                </asp:Panel>
                <%-- ---------------------------------------------------Action POPUP Basic (renew cnacel change)-------------------------------------------------- --%>
                <%-- ---------------------------------------------------Action POPUP ADDOn (renew cnacel change)-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="mpeActionsAddon" runat="server" BehaviorID="mpeActionsADDON"
                    TargetControlID="hdnpnlActionnADDOn" PopupControlID="pnlActionnADDON">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnpnlActionnADDOn" runat="server" />
                <asp:Panel ID="pnlActionnADDON" runat="server" CssClass="Popup" Style="width: auto;
                    height: auto; display: none;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image16" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" onclick="CloseActiosADDONPop();" ImageUrl="~/Images/closebtn.png" />
                    <center>
                        <br />
                    </center>
                </asp:Panel>
                <%-- ---------------------------------------------------Action POPUP AddON (renew cnacel change)-------------------------------------------------- --%>
                <%-- ---------------------------------------------------Action POPUP AL (renew cnacel change)-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="mpeActionsAL" runat="server" BehaviorID="mpeActionsAL"
                    TargetControlID="hdnpnlActionnAL" PopupControlID="pnlActionnAL">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnpnlActionnAL" runat="server" />
                <asp:Panel ID="pnlActionnAL" runat="server" CssClass="Popup" Style="width: auto;
                    height: auto; display: none;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image17" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" onclick="CloseActiosALPop();" ImageUrl="~/Images/closebtn.png" />
                    <center>
                        <br />
                    </center>
                </asp:Panel>

                <%-- ---------------------------------------------------SWAP pop-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="mpeSwapPop" runat="server" BehaviorID="mpeSwapAL"
                    TargetControlID="HdnpnlSwapAL" PopupControlID="pnlSwapAl">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="HdnpnlSwapAL" runat="server" />
                <asp:Panel ID="pnlSwapAl" runat="server" CssClass="Popup" Style="width: 650px;
                    display: none; height: 210px;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image18" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="CloseSwapPop();" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label46" runat="server" ForeColor="#094791" Font-Bold="true" Text="Swap"></asp:Label>
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
                                
                                <td align="right">
                                <asp:Label ID="Label1050" runat="server" Text="Main VC :"></asp:Label>
                                </td>
                                <td  align="left">
                                <asp:TextBox ID="txtswapMainTV" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td align="right">
                                <asp:Label ID="Label1049" runat="server" Text="Main STB :"></asp:Label>
                                 </td>
                                <td  align="left">
                                <asp:TextBox ID="txtswapMainSTB" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                            <td align="right">
                            <asp:Label ID="Label1048" runat="server" Text="Child VC/MAC :"></asp:Label>
                                 
                                </td>
                                <td align="left">
                                <asp:TextBox ID="txtSwapChildTV" runat="server" Visible="false"></asp:TextBox>
                                <asp:DropDownList ID="ddlSwapChildTV" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSwapChildTV_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td align="right">
                                <asp:Label ID="Label1047" runat="server" Text="Child STB/MAC :"></asp:Label>
                                 
                                </td>
                                <td align="left">
                                <asp:TextBox ID="txtSwapChildSTB" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                            <td align="right">
                            Reason :
                            </td>
                            <td align="left" colspan="3">
                            <asp:DropDownList ID="ddlSawpReason" runat="server" Width="350px">
                            <asp:ListItem  Text=" Select Reason" Value="0"></asp:ListItem>
                            <asp:ListItem  Text="Change of device due to faulty device" Value="Change of device due to faulty device"></asp:ListItem>
                            <asp:ListItem  Text="Change temporary current device to a permanemt device" Value="Change temporary current device to a permanemt device"></asp:ListItem>
                            </asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                            <td align="center" colspan="2">
                            <asp:Label ID="lblPopupResponse1" runat="server" Text="" ForeColor="Red"></asp:Label>
                            
                            </td>
                            </tr>
                            <tr align="center" colspan="2">
                            <td>
                            <asp:Label ID="lblActMain" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                            <asp:Label ID="lblActChild" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                            </tr>
                        </table>
                        <br />
                        <table width="90%">
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="btnswapConf" runat="server" CssClass="button" Text="Submit" Visible="true"
                                        Width="100px" OnClick="btnswapConf_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>

                  
            <cc1:ModalPopupExtender ID="mpSwapConfirm" runat="server" BehaviorID="mpeSwapModifyConfirm"
                TargetControlID="hdnSwapConfirm" PopupControlID="pnlSwapConfirm">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnSwapConfirm" runat="server" />
            <asp:Panel ID="pnlSwapConfirm" runat="server" CssClass="Popup" Style="width: 580px;
                display: none; height: 325px;">
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
                    <table>
                        <tr>
                            <td style=" width: 100px;" align="right">
                                <asp:Label ID="Label47" runat="server" Text="Child VC/MAC :"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblCofChildVC" runat="server"></asp:Label>
                            </td>
                        
                            <td style=" width: 100px;"  align="right">
                                <asp:Label ID="Label49" runat="server" Text="Main VC/MAC :"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblCofMainVC"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td style=" width: 110px;" align="right">
                                <asp:Label ID="Label52" runat="server" Text="Child STB/MAC :"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblCofChildSTB" runat="server"></asp:Label>
                            </td>
                        
                            <td style=" width: 110px;" align="right">
                                <asp:Label ID="Label54" runat="server" Text="Main STB/MAC :"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblCofMainSTB"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td    style=" width: 80px;" align="right">
                                <asp:Label ID="Label50" runat="server" Text="Reason :"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:Label runat="server" ID="lblCofReason"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                            Note :.
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4" align="left">
                                <asp:Label runat="server" ID="Label053" Text="A. This will change the Child TV as Main TV and Main TV as Child TV."></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label523" Text="B. Plans of Main TV & Child TV and respective end date of plans will continue to remain same."></asp:Label>
                            <br/>
                            <asp:Label runat="server" ID="Label113" Text="C. You can do SWAP STB in case both the plans of both STBs are inactive."></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label115" Text="D. However, SWAP will fail in case the plans of Parent and Child TV are different (eg if one is Royal & other is Prime)."></asp:Label>
                            <br/>
                            <asp:Label runat="server" ID="Label118" Text="E. In case any of the STB or both STBs are in suspended status, SWAP will fail."></asp:Label>
                            
	                        
                        </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Button ID="btnSawpConfirm" runat="server" CssClass="button" Text="Confirm"
                                    Width="100px" OnClick="btnModifyConfirm_click" /><%--  --%>
                                &nbsp;&nbsp;
                                <input id="btnSwapClose" class="button" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closeSwapConfirmPopup();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>

            <%-- --%>
            <cc1:ModalPopupExtender ID="MPESwapConfirm1" runat="server" BehaviorID="mpeSwapconfirm1"
                        TargetControlID="hdnswapconfirm1" PopupControlID="pnlSwapconfirm1">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnswapconfirm1" runat="server" />
                    <asp:Panel ID="pnlSwapconfirm1" runat="server" CssClass="Popup" Style="width: 430px;
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
                                        <asp:Label ID="Label133" runat="server" Text="Do You Want To Continue ?"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Button ID="btnSWAPConfirmation1" runat="server" CssClass="button" Text="Submit"
                                            Width="100px" OnClick="btnSWAPConfirmation1_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnSWAPConfirmationClose" runat="server" CssClass="button" Text="Cancel"
                                            Width="100px" OnClick="btnSWAPConfirmationClose_Click" />
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>


                <%-- ---------------------------------------------------Action POPUP AL (renew cnacel change)-------------------------------------------------- --%>
            <%-- ---------------------------------------------------MESSAGE POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="MPOPMsg" runat="server" BehaviorID="mpepopMsg1" TargetControlID="hdnPopMsg1"
                    PopupControlID="pnlppMessage">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hdnPopMsg1" runat="server" />
                <asp:Panel ID="pnlppMessage" runat="server" CssClass="Popup" Style="width: 430px; height: 160px;
                    display: none;">
                    <%-- display: none; --%>
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
                                    <asp:Label ID="lblPopupResponse4" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="Button12" runat="server" CssClass="button" Text="OK" Visible="true"
                                        Width="100px" OnClick="btnRefreshForm_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
            
                
                <%-- ---------------------------------------------------SWAP pop-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="POPFaulty" runat="server" BehaviorID="mpeFaultyAL"
                    TargetControlID="HdnpnlFalutyAL" PopupControlID="pnlFaultyAl">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="HdnpnlFalutyAL" runat="server" />
                <asp:Panel ID="pnlFaultyAl" runat="server" CssClass="Popup" Style="width: 660px;
                    display: none; height: 180px;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image20" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="CloseFaultyPop();" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label55" runat="server" ForeColor="#094791" Font-Bold="true" Text="Swap"></asp:Label>
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
                                <td align="right">
                                 
                                 <asp:Label ID="lblFaultySTB_ID" runat="server" Font-Bold="true" Text="STB ID :"></asp:Label>
                                </td>
                                <td align="left">
                                <asp:TextBox ID="txtfaultySTBID" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td align="right">
                                <asp:Label ID="lblFaultyNewSTB_ID" runat="server" Font-Bold="true" Text="New STB ID :"></asp:Label>
                                </td>
                                <td align="left">
                                
                                <asp:TextBox ID="txtfaultyNewSTBID" MaxLength="20" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            Reason :
                            </td>
                            <td colspan="3">
                            <asp:DropDownList ID="ddlFaultyReason" runat="server"     width="350px">
                            </asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                            <td colspan="2" align="center">
                            <asp:Label ID="lblPopupResponse2" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                            </tr>
                        </table>
                        <br />
                        <table width="90%">
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="Button9" runat="server" CssClass="button" Text="Submit" Visible="true"
                                        Width="100px" OnClick="btnFaultyConf_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>

          

  <%-- ---------------------------------------------------ALIGN POPUP-------------------------------------------------- --%>
                    <cc1:ModalPopupExtender ID="popupAlign" runat="server" BehaviorID="mpeAlign"
                        TargetControlID="HiddenField9" PopupControlID="pnlpopupAlign" CancelControlID="Image23">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="HiddenField9" runat="server" />
                    <asp:Panel ID="pnlpopupAlign" runat="server" CssClass="Popup" Style="width: 600px;
                        height: auto; display: none;">
                        <%-- display: none; --%>
                        <asp:Image ID="Image23" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                            margin-top: -15px; margin-right: -15px;" onclick="closepopupAlign();" ImageUrl="~/Images/closebtn.png" />
                        <center>
                            <br />
                            <table width="100%">
                                <tr>
                                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                        &nbsp;&nbsp;&nbsp;Align
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
                                        <asp:Label ID="lblAlign" runat="server" Text="Are you sure you want to Align?"></asp:Label>
                                    </td>
                                </tr>

                                      <tr>
                                <td align="center" colspan="3">
                                     <asp:Panel ID="PnlAlignConfirm" runat="server" Visible="true" Style="max-height: 300px; height: auto; overflow: auto;
                                            border: thin solid black;">
                                            <asp:GridView ID="GrdAlignConfirm" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                                                ShowFooter="true" >
                                                <Columns>
                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"
                                                      FooterText="Total :"  FooterStyle-HorizontalAlign="Right" ItemStyle-Width="500" />
                                                    <asp:BoundField HeaderText="Credit Amount" DataField="CREDIT_AMT" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                    <asp:BoundField HeaderText="Debit Amount" DataField="DEBIT_AMT" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="100" FooterStyle-HorizontalAlign="Right"  />
                                                        
                                                        
                                                </Columns>
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
                                        </asp:Panel>
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
                                        <asp:Button ID="BtnAlign" OnClick="BtnAlign_Click" runat="server" CssClass="button"
                                            Width="100px" Text="Confirm" />
                                        &nbsp;&nbsp;&nbsp;
                                        <input type="button" class="button" value="Cancel" style="width: 100px;" onclick="closepopupAlign();" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>

                  <%-- ---------------------------------------------------CANCEL CONFIRMATION POPUP-------------------------------------------------- --%>
                <cc1:ModalPopupExtender ID="mpeMultcnlconf" runat="server" BehaviorID="mpecanlConf"
                    TargetControlID="hndmultcnlpln" PopupControlID="pnlmultplnConfirm">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="hndmultcnlpln" runat="server" />
                <asp:Panel ID="pnlmultplnConfirm" runat="server" CssClass="Popup" Style="width: 750px;
                    display: none; height: auto;">
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
                        <table width="98%">
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Label ID="Label158" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="tr11">
                                <td align="center" colspan="3">
                                    <asp:Label ID="Label161" runat="server" Text="" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                </td>
                            </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel9" runat="server" Style="max-height: 300px; height: auto; overflow: auto;
                                            border: thin solid black;">
                                            <asp:GridView ID="grdMultCancelPlan" CssClass="Grid" runat="server" AutoGenerateColumns="false"
                                                ShowFooter="true" >
                                                <Columns>
                                                    <asp:BoundField HeaderText="Plan Name" DataField="PLAN_NAME" ItemStyle-HorizontalAlign="Left"
                                                        FooterText="Total :" FooterStyle-HorizontalAlign="Right" ItemStyle-Width="625" />
                                                    <asp:BoundField HeaderText="BASE Price" DataField="CUST_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        
                                                        <asp:BoundField HeaderText="LCO Refund" DataField="refund_lcoamt" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        <asp:BoundField HeaderText="Cust Refund" DataField="refund_amt" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                        <asp:BoundField HeaderText="Days Remaining" DataField="days_left" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="80" />
                                                    <asp:BoundField HeaderText="LCO Price" DataField="LCO_PRICE" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="100" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                    <asp:BoundField HeaderText="Discount" DataField="discount" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                    <asp:BoundField HeaderText="Net BASE Price" DataField="netmrp" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Width="80" FooterStyle-HorizontalAlign="Right" Visible="false" />
                                                        <asp:BoundField HeaderText="Activation" DataField="Activation" ItemStyle-HorizontalAlign="Right"
                                                        FooterStyle-HorizontalAlign="Right" ItemStyle-Width="100" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Valid Upto" ItemStyle-Width="100">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblValid_upto" runat="server" Text='<%# Eval("valid_upto").ToString()%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnPlanPoid" runat="server" Value='<%# Eval("PLAN_POID").ToString()%>' />
                                                                <asp:HiddenField ID="hdnDealPoid" runat="server" Value='<%# Eval("DEAL_POID").ToString()%>' />
                                                                <asp:HiddenField ID="hdnCustPrice" runat="server" Value='<%# Eval("CUST_PRICE").ToString()%>' />
                                                                <asp:HiddenField ID="hdnLcoPrice" runat="server" Value='<%# Eval("LCO_PRICE").ToString()%>' />
                                                                <asp:HiddenField ID="hdnActivation" runat="server" Value='<%# Eval("Activation").ToString()%>' />
                                                                <asp:HiddenField ID="hdnExpiry" runat="server" Value='<%# Eval("valid_upto").ToString()%>' />
                                                                <asp:HiddenField ID="hdnPackageId" runat="server" Value='<%# Eval("packageId").ToString()%>' />
                                                                <asp:HiddenField ID="hdnPurchasePoid" runat="server" Value='<%# Eval("purchasePoid").ToString()%>' />
                                                                <asp:HiddenField ID="hdnPlanType" runat="server" Value='<%# Eval("PLAN_TYPE").ToString()%>' />
                                                                 <asp:HiddenField ID="hdnPlanName" runat="server" Value='<%# Eval("PLAN_NAME").ToString()%>' />
                                                                <asp:HiddenField ID="hdnChannelCount" runat="server" Value='<%# Eval("ChannelCount").ToString()%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
      
                        <table width="90%">
                            
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="Button21" runat="server" CssClass="button" Text="Yes"
                                        Width="100px" OnClick="btnCancel_Click" />
                                    &nbsp;&nbsp;
                                    <input id="Button22" class="button" runat="server" type="button" value="No" style="width: 100px;"
                                        onclick="closemultplanConfirmPopup1();" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>
              

                    <cc1:ModalPopupExtender ID="mpeFaulty" runat="server" BehaviorID="mpeFaultyModifyConfirm1"
                TargetControlID="hdnFaultyConfirm1" PopupControlID="pnlFaultyConfirm1">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnFaultyConfirm1" runat="server" />
            <asp:Panel ID="pnlFaultyConfirm1" runat="server" CssClass="Popup" Style="width: 430px;
                display: none; height: 220px;">
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
                    <table>
                        <tr>
                            <td style=" width: 80px;">
                                <asp:Label ID="lblCofSTB_ID" runat="server" Text="STB ID"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label ID="lblSTBID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style=" width: 80px;">
                                <asp:Label ID="lblCofNEWSTB_ID" runat="server" Text="New STB ID"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblNewSTBID"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td    style=" width: 70px;">
                                <asp:Label ID="Label110" runat="server" Text="Reason"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblFaultyCofReason"></asp:Label>
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
                                <asp:Button ID="Button10" runat="server" CssClass="button" Text="Confirm"
                                    Width="100px" OnClick="btnFaultyModifyConfirm_click" /><%-- OnClick="btnModifyConfirm_click" --%>
                                &nbsp;&nbsp;
                               <asp:Button ID="btnFaultyclose" runat="server" Text="Cancel" CssClass="button" OnClick="btnFaultyclose_click"/>
                                   <%-- <button onclick="CloseFaulty1ConfirmPopup();"  style="margin-right:5px;margin-top:-15px; width: 100px;"   class="button">Cancel</button>--%>
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
        
            

                   <%-- --%>
            <cc1:ModalPopupExtender ID="mpeFaultyConfirm" runat="server" BehaviorID="mpeFaultyModifyConfirm2"
                        TargetControlID="hdnFaultyConfirm2" PopupControlID="pnlFaultyConfirm2">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnFaultyConfirm2" runat="server" />
                    <asp:Panel ID="pnlFaultyConfirm2" runat="server" CssClass="Popup" Style="width: 430px;
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
                                        <asp:Label ID="Label136" runat="server" Text="Do You Want To Continue ?"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Button ID="Button14" runat="server" CssClass="button" Text="Submit"
                                            Width="100px" OnClick="btnFaultyModifyConfirm1_click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="Button15" runat="server" CssClass="button" Text="Cancel"
                                            Width="100px" OnClick="btnFaultyConfirmationClose_Click" />
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>



            
               <cc1:ModalPopupExtender ID="PopTERMINATE" runat="server" BehaviorID="mpeTERMINATEAL"
                    TargetControlID="HdnpnlTERMINATEAL" PopupControlID="pnlTERMINATEAl">
                </cc1:ModalPopupExtender>
                <asp:HiddenField ID="HdnpnlTERMINATEAL" runat="server" />
                <asp:Panel ID="pnlTERMINATEAl" runat="server" CssClass="Popup" Style="width: 650px;
                    display: none; height: 180px;">
                    <%-- display: none; --%>
                    <asp:Image ID="Image21" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                        margin-top: -15px; margin-right: -15px;" ImageUrl="~/Images/closebtn.png" onclick="CloseTERMINATEALPop();" />
                    <center>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="3">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label82" runat="server" ForeColor="#094791" Font-Bold="true" Text="TERMINATE"></asp:Label>
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
                                
                                 <td align="right">
                                 <asp:Label ID="Label106" runat="server"  Font-Bold="true" Text="STB ID :"></asp:Label>
                                </td>
                                <td align="left">
                                <asp:Label runat="server" ID="lblTerminateSTBNO"></asp:Label>
                                </td>
                                <td align="right">
                                <asp:Label ID="Label109" runat="server" Font-Bold="true" Text="VC ID :"></asp:Label>
                                </td>
                                <td align="left">
                                
                                <asp:Label runat="server" ID="lblTerminateVCID"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            <td align="right">
                            Reason :
                            </td>
                            <td align="left" colspan="3">
                            <asp:DropDownList ID="ddlTERMINATEReason" runat="server" Width="320px">
                            </asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                            <td colspan="2" align="center">
                            <asp:Label ID="lblPopupResTerminate" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                            </tr>
                        </table>
                        <br />
                        <table width="90%">
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="Button11" runat="server" CssClass="button" Text="Submit" Visible="true"
                                        Width="100px" OnClick="btnTerminateConf_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>


                    <cc1:ModalPopupExtender ID="popConfTERMINATEAL" runat="server" BehaviorID="mpeTERMINATEALModifyConfirm"
                TargetControlID="hdnTERMINATEALConfirm" PopupControlID="pnlTERMINATEALConfirm">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnTERMINATEALConfirm" runat="server" />
            <asp:Panel ID="pnlTERMINATEALConfirm" runat="server" CssClass="Popup" Style="width: 430px;
                display: none; height: 220px;">
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
                    <table>
                        <tr>
                            <td style=" width: 90px;">
                                <asp:Label ID="Label114" runat="server" Text="STB ID"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblTerminate_STB"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style=" width: 90px;">
                                <asp:Label ID="Label116" runat="server" Text="VC ID"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblTerminate_VC"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td    style=" width: 90px;">
                                <asp:Label ID="Label130" runat="server" Text="Reason "></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td align="left">
                                <asp:Label runat="server" ID="lblTerminate_Reason"></asp:Label>
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
                                <asp:Button ID="Button13" runat="server" CssClass="button" Text="Confirm"
                                    Width="100px" OnClick="btnTerminateModifyConfirm_click" /><%-- OnClick="btnModifyConfirm_click" --%>
                                &nbsp;&nbsp;
                                
                                <asp:Button ID="btnTerminatClose" runat="server" Text="Cancel" CssClass="button" OnClick="btnTerminatClose_click"/>
                                    <%--<button onclick="CloseTERMINATEALConfirmPopup();"  style="margin-right:5px;margin-top:-15px; width: 100px;"   class="button">Cancel</button>--%>
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
        
            
            
            

                 <cc1:ModalPopupExtender ID="popConfTERMINATEAL2" runat="server" BehaviorID="mpeTERMINATEALModifyConfirm2"
                        TargetControlID="hdnTERMINATEALConfirm2" PopupControlID="pnlTERMINATEALConfirm2">
                    </cc1:ModalPopupExtender>
                    <asp:HiddenField ID="hdnTERMINATEALConfirm2" runat="server" />
                    <asp:Panel ID="pnlTERMINATEALConfirm2" runat="server" CssClass="Popup" Style="width: 430px;
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
                                        <asp:Label ID="Label145" runat="server" Text="Are You Sure?"></asp:Label>
                                        <br />
                                        <asp:Label ID="Label146" runat="server" Text="You Want To Terminate"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Button ID="Button18" runat="server" CssClass="button" Text="Submit"
                                            Width="100px" OnClick="btnTERMINATEALModifyConfirm2_click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="Button19" runat="server" CssClass="button" Text="Cancel"
                                            Width="100px" OnClick="btnTERMINATEALConfirmationClose1_Click" />
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>

            

                <%-- ----------------------------------------------------Loader------------------------------------------------------------------ --%>
                <div id="imgrefresh" class="loader transparent">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                        ToolTip="Loading ..." Style="" />
                </div>
        </ContentTemplate>
        <Triggers>
     <asp:PostBackTrigger ControlID="btnCustomerReceipt"  />
    </Triggers>
    </asp:UpdatePanel>
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
      <script type="text/javascript">
          function closeFaultyConfirmPopup1() {

              $find("mpeFaultyModifyConfirm1").hide();
              return false;
          }
          function closemultplanConfirmPopup1() {

              $find("mpecanlConf").hide();
              return false;
          }
          function FunChkDisableauto(id) {
              debugger;
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
        </script>
</asp:Content>
