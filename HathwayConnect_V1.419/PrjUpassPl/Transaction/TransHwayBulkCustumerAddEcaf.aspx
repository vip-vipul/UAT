<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="TransHwayBulkCustumerAddEcaf.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwayBulkCustumerAddEcaf" %>

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
        .delInfoo
        {
            padding: 10px;
            border: 1px solid #094791;
            width: 700px;
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
    </script>
    <div class="maindive">
        <h3>
            <asp:Label runat="server" ID="Label1" Text="Bulk Ecaf File Upload" ForeColor="Black"></asp:Label>
        </h3>
        <div class="delInfo" id="divTabPanels" runat="server" visible="true">
            <div class="delInfoo" id="div2" runat="server">
                <table width="100%">
                    <tr id="tr3">
                        <td colspan="6" align="left">
                            <b>
                                <asp:Label ID="Label61" runat="server" Text="Address Details"></asp:Label>
                            </b>
                        </td>
                        <td colspan="5" align="right">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="8">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="style79">
                            <asp:Label ID="Label17" runat="server" Text="PinCode" Width="73px">
                                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label></asp:Label>
                        </td>
                        <td width="10px">
                            <asp:Label ID="Label18" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlpin" runat="server" AutoPostBack="True" Style="resize: none;
                                margin-bottom: 0px;" Width="180px" OnSelectedIndexChanged="ddlpin_SelectedIndexChanged">
                                <%----%>
                            </asp:DropDownList>
                        </td>
                        <td align="left" width="100px">
                            <asp:Label ID="Label19" runat="server" Text="Area" Width="83px"></asp:Label>
                        </td>
                        <td width="10px">
                            <asp:Label ID="Label20" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left" class="style81">
                            <asp:DropDownList ID="ddlarea" runat="server" AutoPostBack="True" Style="resize: none;
                                    margin-bottom: 0px;" Width="180px" OnSelectedIndexChanged="ddlarea_SelectedIndexChanged" ><%----%>
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="tr4">
                        <td align="left" class="style82">
                            <asp:Label ID="Label3" runat="server" Text="Street" Width="78px">
                                <asp:Label ID="Label69" runat="server" ForeColor="Red" Text="*"></asp:Label></asp:Label>
                        </td>
                        <td width="10px">
                            <asp:Label ID="Label10" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlstreet" runat="server" AutoPostBack="True" Style="resize: none;
                                    margin-bottom: 0px;" Width="180px" OnSelectedIndexChanged="ddlstreet_SelectedIndexChanged" ><%----%>
                                </asp:DropDownList>
                        </td>
                        <td align="left" class="style79">
                            <asp:Label ID="Label22" runat="server" Text="Location">
                                <asp:Label ID="Label113" runat="server" ForeColor="Red" Text="*"></asp:Label></asp:Label>
                        </td>
                        <td width="10px">
                            <asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddllocation" runat="server" AutoPostBack="True" Style="resize: none;
                                    margin-bottom: 0px;" Width="182px" OnSelectedIndexChanged="ddllocation_SelectedIndexChanged" ><%----%>
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="5">
                        <td align="left" width="80px">
                            <asp:Label ID="Label24" runat="server" Text="Building"></asp:Label>
                        </td>
                        <td width="10px">
                            <asp:Label ID="Label25" runat="server" Text=":"></asp:Label>
                        </td>
                        <td align="left" class="style81">
                            <asp:DropDownList ID="ddlbuilding" runat="server"  Style="resize: none;
                                    margin-bottom: 0px;" Width="182px" ><%----%>
                                </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
        </div>
        <br />
        <div class="delInfo">
            <asp:FileUpload ID="fupData" runat="server" />
            &nbsp
            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
            &nbsp;<a href="../GeneratedFiles/DemoExcel/Bulk_Cust_Upload_Ecaf.xlsx" id="bulktransaction"
                runat="server">Download Template</a>
        </div>
        <br />
        <div class="delInfo" id="Operation" runat="server" visible="true" style="margin-top: 10px;
            text-align: left; height: 280px">
            <center>
                <h2>Steps To Use The Bulk Upload File For Bulk Ecaf.</h2>
            </center>
            <div style="margin-left: 2%">
                <ol>
                    <li style="float: left">Download the template as available on the portal.</li><br />
                    <li style="float: left">Fill in all the relevant fields.</li><br />
                    <li style="float: left">VC / MAC ID : VC ID field should have 12 characters i.e. please
                        capture all the preceding zeroes.</li><br />
                    <li style="float: left">Discount rate flag ( column G) will be as amount or percentage
                        (A or P).</li><br />
                    <li style="float: left">DISCOUNT_AMT/PERCENTAGE: If specific amount is to be mentioned,
                        type in the amount and set the flag as A, or else enter percentage and set as P.
                        ( For eg. If discount to be given is 10 rupees, mention 10 in column C and set flag
                        in G as A; if its 2 percent, mention as 2 and set the flag to P. )</li><br />
                    <li style="float: left">Save the Excel file as Text Tab De-limited file on your PC.
                        Except for input fields from column A to column G, please remove all other data
                        including the headers in row 1.</li><br />
                    <li style="float: left">Upload this file from the path where you have stored and your
                        transactions will be processed.</li><br />
                </ol>
            </div>
            <br />
        </div>
        <br />
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
