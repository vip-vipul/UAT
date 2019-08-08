﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ankit.aspx.cs" Inherits="PrjUpassPl.Transaction.ankit" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 820px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript">
        function back() {
            window.location.href = "../Transaction/TransBulkPages.aspx";
            return false;
        }

        function InProgress() {
            document.getElementById("imgrefresh").style.visibility = 'visible';
        }
        function onComplete() {
            document.getElementById("imgrefresh").style.visibility = 'hidden';
        }
        function hideTrans(ctrl) {
            ctrl.style.visibility = 'hidden';
        }
        
    </script>
    <div class="maindive">
        <div style="float: right">
            <button onclick="return back()" id="btnreturnBulkOperation" runat="server" style="margin-right: 5px; margin-top: -15px;" class="button">
                Back</button>
        </div>
    
        <div class="delInfo" style="margin-top: 11px;">
            <asp:FileUpload ID="fupData" runat="server" />
            &nbsp
            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />&nbsp;
             <a href="../GeneratedFiles/DemoExcel/BulkUpload.xlsx" id="bulktransaction" runat="server">
                Download Template</a> <a href="../GeneratedFiles/DemoExcel/BulkChange.xlsx" id="bulkchange"
                    runat="server">Download Template</a>
            <asp:Label runat="server" ID="Label2" Text=""></asp:Label>
        </div>
        <div>
            <h3>
                <asp:Label runat="server" ID="lblStatusHeading" Text=""></asp:Label>
            </h3>
            <asp:Label runat="server" ID="lblStatus" Text="" ForeColor="Red"></asp:Label>
            <br />
            <asp:LinkButton ID="lnkfileStatus" runat="server" Text="View The Uploaded file Status"
                ></asp:LinkButton>
        </div>
                <!--sanket-->
        <div class="delInfo" id="Operation" runat="server" visible="false" style="margin-top: 10px; text-align: left; 
            height: 280px">
            <center>
                <h2>
                    Steps to use the bulk upload file for bulk transaction</h2>
            </center>
            <div style="margin-left: 2%">
                <ol>
                    <li style="float: left">Download the template as available on the portal.</li><br />
                    <li style="float: left">Fill in the relevant fields.</li><br />
                    <li style="float: left">VC / MAC ID : VC ID field should have 12 characters i.e. please
                        capture all the preceding zeroes.</li><br />
                    <li style="float: left">Plan Name in the field should correct, any incorrect plan name
                        will lead to the transaction failure.</li><br />
                    <li style="float: left">Use the Right Action Codes (A: ADD, R: Renew, C: Cancel).Please
                        note base pack cancellation is not allowed in this option.</li><br />
                    <li style="float: left">Renewal Flag can be Y (Yes) or N (No). Blank input will be considered
                        as N by default.</li><br />
                    <li style="float: left">Save the Excel file as Text Tab De-limitedfile on your PC.Except
                        for input fields from column A to column F, please remove all other data including
                        the headers in row 1.</li><br />
                    <li style="float: left">Upload this file from the path where you have stored and your
                        transactions will be processed.</li><br />
                    <li style="float: left">You can monitor the progress using Bulk File Process Status
                        Report under Reports section.</li><br />
                    <li style="float: left">Any transaction that fails will be provided in the status report
                        along with the reason for failure.</li><br />
                    <li style="float: left">Please ensure that you have adequate balance under Allocated
                        Balance for processing transactions.</li><br />
                </ol>
            </div>
            <br />
           
        </div>


   

        <!--sanket-->
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="imgrefresh" class="loader transparent">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                    ToolTip="Loading ..." Style="" />
                <br />
                <h3>
                    Transactions are in progress, Do not refresh or close the browser</h3>
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
