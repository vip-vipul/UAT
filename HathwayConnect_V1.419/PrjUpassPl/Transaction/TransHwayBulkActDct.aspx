<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TransHwayBulkActDct.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwayBulkActDct" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
  <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 750px;
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
        .loader h3
        {
            padding: 10px;
            position: fixed;
            top: 55%;
            left: 30%;
        }
    </style>
    <script type="text/javascript">

        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
        }
        function hideTrans(ctrl) {
            ctrl.style.visibility = 'hidden';
        }
        function back() {
            window.location.href = "../Transaction/TransBulkPages.aspx";
            return false;
        }
    </script>
    <div class="maindive">
    <div style="float: right">
            <button onclick="return back()" id="btnreturnBulkOperation" runat="server" style="margin-right: 5px;
                margin-top: -15px;" class="button">
                Back</button>
        </div>
    <h3>
        <asp:Label runat="server" ID="Label1" Text="Bulk activation and Deactivation File Upload" ForeColor="Black"></asp:Label>
    </h3>
    <div class="delInfo">
        <asp:FileUpload ID="fupData" runat="server" />
        &nbsp
        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
         &nbsp;
         <a href="~/GeneratedFiles/DemoExcel/Bulk_ACT_DCT.xlsx" id="bulktransaction" runat="server"> Download Template </a>
    </div>
    <br />
    <div class="delInfo" id="Operation" runat="server" visible="true" style="margin-top: 10px; text-align: left; 
            height: 280px">
            <center>
                <h2>
                    Steps to use the bulk upload file for bulk activation and Deactivation.</h2>
            </center>
            <div style="margin-left: 2%">
                <ol>
                    <li style="float: left">Download the template as available on the portal.</li><br />
                    <li style="float: left">Fill in all the relevant fields.</li><br />
                    <li style="float: left">VC ID field should have 12 characters i.e. please capture all the preceding zeroes.</li><br />
                    <li style="float: left">Reason in the field should correct, any incorrect Reason
                        will lead to the transaction failure.</li><br />
                        <li style="float: left">Use the Right Action Codes (RA:ACTIVATE, DA: DEACTIVATE).Please
                        note Action is not allowed in this option.</li><br />
                    <li style="float: left">Save the Excel file as Text Tab De-limited file on your PC. Except for input fields from column A to column E, please remove all other data including the headers in row 1.</li><br />
                    <li style="float: left">Upload this file from the path where you have stored and your transactions will be processed.</li><br />                    
                    <li style="float: left">You can monitor the progress using Bulk ACT & DEACT Report under Reports section.</li><br />
                    <li style="float: left">Any transaction that fails will be provided in the Service Activation Deactivation Report  along with the reason for failure.</li><br />
                </ol>
            </div>
            <br />
           
        </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h4>
                <asp:Label runat="server" ID="lblStatusHeading" Text="" ForeColor="Red"></asp:Label>
            </h4>
            <asp:Label runat="server" ID="lblStatus" Text="" ForeColor="Red"></asp:Label>
            <br />
            <%--  <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Image ID="img" runat="server" ImageUrl="../Images/spinner.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." Style="" />
                <br />
                <h3>
                    Transactions are in progress, Do not refresh or close the browser</h3>
            </div>
        </ContentTemplate>
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
    </div>
</asp:Content>
