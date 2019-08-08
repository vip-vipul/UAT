<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="transUserAccountMapping.aspx.cs" Inherits="PrjUpassPl.Transaction.transUserAccountMapping" %>
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
            window.location.href = "../Master/mstLCOAdminPages.aspx";
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
        <asp:Label runat="server" ID="Label1" Text="User Account Mapping" ForeColor="Black"></asp:Label>
    </h3>
    <div class="delInfo">
    <table >
    <tr>
    <td align="left">Mode</td><td>:</td>
    <td align="left">
    <asp:RadioButtonList ID="rblMode" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblMode_SelectedIndexChanged">
    <asp:ListItem Value="0" Text="Single" Selected="True"></asp:ListItem>
    <asp:ListItem Value="1" Text="Bulk"></asp:ListItem>
    </asp:RadioButtonList>
    </td>
    </tr>
    <tr runat="server" id="tr1">
    <td align="left"><asp:Label runat="server" ID="Label3" Text="User ID" ForeColor="Black"></asp:Label></td>
    <td>:</td>
    <td align="left">
         <%--<asp:TextBox runat="server" id="txtLcoCode" MaxLength="20"></asp:TextBox> --%>  
         <asp:DropDownList ID="ddlLco" runat="server" AutoPostBack="true" Height="19px" Style="resize: none;"
                        Width="304px" >
                    </asp:DropDownList>
    </td>
    </tr>
    <tr runat="server" id="tr2">
    <td align="left"> <asp:Label runat="server" ID="Label2" Text="Account No" ForeColor="Black"></asp:Label></td>
    <td>:</td>
    <td align="left">
        <asp:TextBox runat="server" id="txtAccountNo" MaxLength="10"></asp:TextBox>   
    </td>
    </tr>
    <tr runat="server" id="tr4" >
    <td align="left">Action</td><td>:</td>
    <td align="left">
    <asp:RadioButtonList ID="rblAction" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" >
    <asp:ListItem Value="Y" Text="Allocate" Selected="True"></asp:ListItem>
    <asp:ListItem Value="N" Text="Deallocate"></asp:ListItem>
    </asp:RadioButtonList>
    </td>
    </tr>
    <tr>
    <td colspan="3" runat="server" id="tr3" visible="false">
        <asp:FileUpload ID="fupData" runat="server" />
        </td>
        </tr>

        <tr>
        <td>
        <asp:Button ID="btnUpload" runat="server" Text="Submit" OnClick="btnUpload_Click" />
        </td>
        <td colspan="2" runat="server" id="td1" visible="false">
        
         <a href="~/GeneratedFiles/DemoExcel/User_AC_Mapping.xlsx" id="bulktransaction" runat="server"> Download Template </a>
         </td>
         </tr>
         </table>
          <h4>
                <asp:Label runat="server" ID="lblStatusHeading" Text="" ForeColor="Red"></asp:Label>
            </h4>
            <asp:Label runat="server" ID="lblStatus" Text="" ForeColor="Red"></asp:Label>
           
    </div>
    <br />
    <div class="delInfo" id="Operation" runat="server" visible="false" style="margin-top: 10px; text-align: left; 
            height: 280px">
            <center>
                <h2>
                    Steps to use the bulk upload file for bulk User Account Mapping.</h2>
            </center>
            <div style="margin-left: 2%">
                <ol>
                    <li style="float: left">Download the template as available on the portal.</li><br />
                    <li style="float: left">Fill in all the relevant fields.</li><br />
                    <li style="float: left">Account No field should have 12 characters i.e. please capture all the preceding zeroes.</li><br />
                    
                        <li style="float: left">Use the Right Action Flag Codes (Y:Active, N: Inactive).Please
                        note Action Flag is not allowed in this option.</li><br />
                    <li style="float: left">Save the Excel file as Text Tab De-limited file on your PC. Except for input fields from column A to column D, please remove all other data including the headers in row 1.</li><br />
                    <li style="float: left">Upload this file from the path where you have stored and your transactions will be processed.</li><br />                    
                 </ol>
            </div>
            <br />
           
        </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
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
