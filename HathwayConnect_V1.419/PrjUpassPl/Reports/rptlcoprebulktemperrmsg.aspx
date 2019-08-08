<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptlcoprebulktemperrmsg.aspx.cs"
    Inherits="PrjUpassPl.Reports.rptlcoprebulktemperrmsg" MasterPageFile="~/MasterPage.Master" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function closePopup() {
            $find("mpeConfirmation").hide();
            return false;
        }
        function closeExpPopup() {
            $find("mpeExp").hide();
            return false;
        }
        
    </script>
    <style type="text/css">
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
        function doValid() {

            if (document.getElementById("<%=txtFrom1.ClientID %>").value== "") {
                alert("Please Enter Date!");
                document.getElementById("txtFrom1").select();
                document.getElementById("txtFrom1").focus();
                return false;
            }

            if (document.getElementById("<%=txthrs.ClientID %>").value == "") {
                alert("Please Enter From Hours!");
                document.getElementById("<%=txthrs.ClientID %>").select();
                document.getElementById("<%=txthrs.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtmin.ClientID %>").value == "") {
                alert("Please Enter From Mins!");
                document.getElementById("<%=txtmin.ClientID %>").select();
                document.getElementById("<%=txtmin.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txttohrs.ClientID %>").value == "") {
                alert("Please Enter To Hours!");
                document.getElementById("<%=txttohrs.ClientID %>").select();
                document.getElementById("<%=txttohrs.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txttomin.ClientID %>").value == "") {
                alert("Please Enter To Mins!");
                document.getElementById("<%=txttomin.ClientID %>").select();
                document.getElementById("<%=txttomin.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txthrs.ClientID %>").value > 24) {
                alert("Please Enter proper  From Hours!");
                document.getElementById("<%=txthrs.ClientID %>").select();
                document.getElementById("<%=txthrs.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txttohrs.ClientID %>").value > 24) {
                alert("Please Enter proper  To Hours!");
                document.getElementById("<%=txttohrs.ClientID %>").select();
                document.getElementById("<%=txttohrs.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtmin.ClientID %>").value > 60) {
                alert("Please Enter proper From Minute!");
                document.getElementById("<%=txtmin.ClientID %>").select();
                document.getElementById("<%=txtmin.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txttomin.ClientID %>").value > 60) {
                alert("Please enter proper  To Minute!");
                document.getElementById("<%=txttomin.ClientID %>").select();
                document.getElementById("<%=txttomin.ClientID %>").focus();
                return false;
            }

        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 46 || charCode > 57)) {
                alert("Enter Only Numeric Values..");
                return false;
            }
            return true;
        }


        function goBack()
         {
            window.history.back();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                <div style="float:right">
                <button onclick="goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
                    <div class="tblSearchItm" style="width: 30%;">
                        <table width="100%">
                    <tr>
                        <td align="center" td align="center" class="cal_image_holder">
                            &nbsp;<asp:Label ID="Label11" runat="server" Font-Bold="False" Font-Names="Tahoma"
                                Font-Size="Small">Date:</asp:Label>
                            <asp:TextBox ID="txtFrom1" runat="server" BorderWidth="1"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtFrom1_CalendarExtender" runat="server" Format="dd-MMM-yyyy"
                                PopupButtonID="imgFrom1" TargetControlID="txtFrom1">
                            </cc1:CalendarExtender>
                            <asp:Image ID="imgFrom1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="height: 24px">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label12" runat="server" Font-Bold="False" Font-Names="Tahoma" 
                                Font-Size="Small"> From Time:</asp:Label>
                            <asp:TextBox ID="txthrs" runat="server" 
                                Font-Names="Tahoma" Font-Size="8pt" MaxLength="2" 
                                onkeypress="return isNumber(event)" Width="28px" BorderWidth="1px"></asp:TextBox>
                            <asp:Label ID="Label13" runat="server" Font-Bold="False" Font-Names="Tahoma" 
                                Font-Size="Small">Hrs</asp:Label>
                            <asp:TextBox ID="txtmin" runat="server" Font-Names="Tahoma" Font-Size="8pt" 
                                MaxLength="2" onkeypress="return isNumber(event)" Width="32px" 
                                BorderWidth="1px"></asp:TextBox>
                            <asp:Label ID="Label14" runat="server" Font-Bold="False" Font-Names="Tahoma" 
                                Font-Size="Small">Min</asp:Label>
                        </td>
                        </tr>
                        <tr>
                        <td align="left">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Names="Tahoma" 
                                Font-Size="Small"> To Time:</asp:Label>
                            <asp:TextBox ID="txttohrs" runat="server" 
                                Font-Names="Tahoma" onkeypress="return isNumber(event)"
                                Font-Size="8pt" MaxLength="2" Width="28px" BorderWidth="1px"></asp:TextBox>
                            <asp:Label ID="Label9" runat="server" Font-Bold="False" Font-Names="Tahoma" 
                                Font-Size="Small">Hrs</asp:Label>
                            <asp:TextBox ID="txttomin" runat="server" 
                                Font-Names="Tahoma" Font-Size="8pt" MaxLength="2" 
                                onkeypress="return isNumber(event)" Width="32px" BorderWidth="1px"></asp:TextBox>
                            <asp:Label ID="Label10" runat="server" Font-Bold="False" Font-Names="Tahoma" 
                                Font-Size="Small">Min</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" style="height: 20px">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="xBtnSearch" runat="server" 
                                BorderStyle="Solid" BorderWidth="1" Font-Bold="True"
                                Font-Names="Tahoma" Font-Size="Small" Height="20px" OnClick="xBtnSearch_Click"
                                Text="Submit" Width="60" OnClientClick="return doValid();"  />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 1px">
                            &nbsp;
                            <asp:Label ID="xLblMessage" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="Small"
                                ForeColor="Red" Width="250px"></asp:Label>
                        </td>
                    </tr>
                    </table>
                     </div>
                    <%--OnClientClick="return doValid();"--%>
                    
                            <table>
                                      <tr>
                            <td align="left">
                                <div class="dGrid1">
                                                    <asp:DataGrid ID="xDGr" runat="server" AllowCustomPaging="True" AllowSorting="True"
                                                        AlternatingItemStyle-BackColor="white" AutoGenerateColumns="False" BorderColor="LightSteelBlue"
                                                        CellPadding="2" CssClass="Grid" Height="100%" OnItemDataBound="xDGr_ItemDataBound"
                                                        PageSize="14" ShowFooter="True" Width="100%" OnPageIndexChanged="xDGr_PageIndexChanged">
                                                        <FooterStyle CssClass="GridFooter" />
                                                        <HeaderStyle CssClass="GridFooter" />
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Sr. No.">
                                                                <HeaderStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" Width="50px" />
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%# Container.ItemIndex+1 %>.
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="var_lcopre_bulk_errormsg" HeaderText="Error Message">
                                                                <HeaderStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Left" />
                                                                <FooterStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Right" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="cnt" HeaderText="Count">
                                                                <HeaderStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Right" />
                                                                <FooterStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Right" />
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <PagerStyle CssClass="pStyle1" HorizontalAlign="Center" NextPageText="Next &amp;gt;"
                                                            PrevPageText="&amp;lt; Previous" />
                                                    </asp:DataGrid>
                                                     </div>
                                                </td>
                                            </tr>
                                        </table>                                                        
            </div>
    </asp:Panel>
</asp:Content>
