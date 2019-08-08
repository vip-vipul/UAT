<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransHwayBulkChangePlan.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwayBulkChangePlan" %>

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
            <button onclick="return back()" style="margin-right: 5px; margin-top: -15px;" class="button">
                Back</button>
        </div>
        <div class="delInfo">
            <asp:FileUpload ID="fupData" runat="server" />
            &nbsp
            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />&nbsp;
            <a href="../GeneratedFiles/DemoExcel/BulkChangePlan.xlsx">Download Template</a>
            <br />
            <asp:Label runat="server" ID="Label2" Text=""></asp:Label>
        </div>
        <div>
            <br />
            <h3>
                <asp:Label runat="server" ID="lblStatusHeading" Text=""></asp:Label>
            </h3>
            <asp:Label runat="server" ID="lblStatus" Text="" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <br />
             <asp:LinkButton ID="lnkfileStatus" runat="server" Text="View The Uploaded file Status"
                OnClick="lnkfileStatus_Click"></asp:LinkButton>
        </div>
        <asp:Panel ID="pnlErrData" runat="server" Visible="false">
           
            <table class="Grid">
                <tr>
                    <th>
                        Customer No
                    </th>
                    <th>
                        VC
                    </th>
                    <th>
                        LCO Code
                    </th>
                    <th>
                        Plan Name
                    </th>
                    <th>
                        Action
                    </th>
                    <th>
                        Error
                    </th>
                </tr>
                <asp:Repeater ID="rptErrData" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lblCustNo" runat="server" Text='<%#Eval("custno") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVC" runat="server" Text='<%#Eval("vc") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLcoCode" runat="server" Text='<%#Eval("lcocode") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPlan" runat="server" Text='<%#Eval("plan") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAction" runat="server" Text='<%#Eval("action") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("err") %>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </asp:Panel>
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
