<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptEcafCustDetails.aspx.cs" Inherits="PrjUpassPl.Reports.rptEcafCustDetails" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*.GridFooter
        {
            border: 1px solid #094791;
            border-radius: 0px;
            color: White;
            background: #094791;
            width: 100px;
        }*/
        
        .cal_image_holder
        {
        }
        .Grid th a
        {
            color: #ffffff;
            cursor: pointer;
        }
        /*.style67
        {
            width: 142px;
        }
        .style68
        {
            width: 975px;
        }*/
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript">

        function InProgress() {

            document.getElementById("imgrefresh2").style.visibility = 'visible';
        }
        function onComplete() {

            document.getElementById("imgrefresh2").style.visibility = 'hidden';
        }
        function goBack() {
            window.location.href = "../Reports/EcafPages.aspx";
            return false;
        }
         
        
   
    </script>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                    <div style="float: right">
                        <button onclick=" return goBack()" style="margin-right: 5px; margin-top: -15px;"
                            class="button">
                            Back</button>
                    </div>
                    <asp:Panel ID="pnldetail" runat="server">
                    
                    <div class="tblSearchItm" >
                    
                        <table style="width: 60%;">
                            <tr>
                                <td  style="text-align:right;" class="cal_image_holder">
                                     <asp:CheckBox ID="chkdt"  runat="server"  AutoPostBack="True" oncheckedchanged="chkdt_CheckedChanged" 
                                         />
                                    From Date :
                                    <asp:TextBox ID="txtFrom" runat="server" BorderWidth="1"></asp:TextBox>
                                    <%--</td>
                    <td class="cal_image_holder" align="left">--%>
                                    <asp:Image ID="imgFrom" runat="server" 
                                        ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender ID="calFrom" runat="server" Format="dd-MMM-yyyy" 
                                        PopupButtonID="imgFrom" TargetControlID="txtFrom">
                                    </cc1:CalendarExtender>
                                </td>
                                <td style="text-align:left;" class="cal_image_holder">
                                    &nbsp;&nbsp; To Date :
                                    <asp:TextBox runat="server" ID="txtTo" BorderWidth="1"></asp:TextBox>
                                    <%--</td>
                    <td class="cal_image_holder" align="left">--%>
                                    <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                               
                            </tr>
                            <tr>
                                <td class="cal_image_holder" colspan="2" style="text-align:center;">
                                   OR</td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                 <asp:CheckBox ID="chkVC"  runat="server" 
                                        AutoPostBack="True" oncheckedchanged="chkVC_CheckedChanged" />     &nbsp;
                                    <asp:Label ID="Label110" runat="server" Text="Enter VC Number :"></asp:Label>
                                </td>
                                 <td style="text-align:left;">
                                    &nbsp;
                                     <asp:TextBox ID="txtVCid" runat="server" Height="25px" Width="180px" MaxLength="20"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                 <td>
                                    &nbsp;
                                </td>
                               
                            </tr>
                             <tr>
                                 <td>
                                     &nbsp;
                                 </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                     <table width="100%">
                            <tr>
                                <td style="padding-left: 50px" align="center">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button" UseSubmitBehavior="false"
                                        OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Label ID="Label1" ForeColor="Red" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    
                    </div>
                    </asp:Panel>





                    <div class="delInfo1" id="divData" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Button runat="server" ID="btnGenerateExcel" Text="Generate Excel" CssClass="button"
                                                Width="110px" Visible="true" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnGenerateExcel" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <div class="griddiv" id="dIVtOTgRIDD" runat="server">
                                        <asp:DataGrid ID="xDGr" runat="server" Width="100%" ShowFooter="True" BorderColor="LightSteelBlue"
                                            PageSize="14" AllowCustomPaging="True" AllowSorting="True" CellPadding="2" CssClass="Grid"
                                            AutoGenerateColumns="False" AlternatingItemStyle-BackColor="white" OnItemDataBound="xDGr_ItemDataBound"
                                            OnUpdateCommand="xDGr_UpdateCommand">
                                            <FooterStyle CssClass="GridFooter" />
                                            <HeaderStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Sr. No.">
                                                    <HeaderStyle Font-Size="Small" Font-Bold="True" HorizontalAlign="Center" Width="50px">
                                                    </HeaderStyle>
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <%# Container.ItemIndex+1 %>
                                                        .
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="owner" HeaderText="User Name" FooterText="Total">
                                                    <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Small"
                                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Size="Small" Font-Bold="True"></HeaderStyle>
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Total Customer Registered">
                                                    <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Small"
                                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" />
                                                    <HeaderStyle Font-Size="Small" Font-Bold="True"></HeaderStyle>
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk" runat="server" CommandName="Update" Text='<%# Eval("total") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                            </Columns>
                                            <PagerStyle NextPageText="Next &amp;gt;" PrevPageText="&amp;lt; Previous" CssClass="pStyle1">
                                            </PagerStyle>
                                        </asp:DataGrid>
                                    </div>
                                    <div class="griddiv" id="dIvGridReport" runat="server" visible="false">
                                    <asp:GridView ID="grd" runat="server" AllowPaging="true" 
                                        AutoGenerateColumns="false" CssClass="Grid" 
                                        PageSize="100" ShowFooter="true" Width="100%">
                                        <FooterStyle CssClass="GridFooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cafno" FooterText="" 
                                                HeaderStyle-HorizontalAlign="Center" HeaderText="Receipt No." 
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="accno" FooterText="" 
                                                HeaderStyle-HorizontalAlign="Center" HeaderText="Account No." 
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="name" FooterText="" 
                                                HeaderStyle-HorizontalAlign="Center" HeaderText="Name" 
                                                ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="mobile" FooterStyle-HorizontalAlign="Right" 
                                                HeaderStyle-HorizontalAlign="Center" HeaderText="Mobile No." 
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="landline" HeaderStyle-HorizontalAlign="Center" 
                                                HeaderText="Landline" ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="building" HeaderStyle-HorizontalAlign="Center" 
                                                HeaderText="Building" ItemStyle-HorizontalAlign="left" Visible="false" />
                                            <asp:BoundField DataField="street" HeaderStyle-HorizontalAlign="Center" 
                                                HeaderText="Street" ItemStyle-HorizontalAlign="left"  Visible="false" />
                                            <asp:BoundField DataField="area" HeaderStyle-HorizontalAlign="Center" 
                                                HeaderText="Area" ItemStyle-HorizontalAlign="left"  Visible="false"/>
                                            <asp:BoundField DataField="location" HeaderStyle-HorizontalAlign="Center" 
                                                HeaderText="Location" ItemStyle-HorizontalAlign="left"   Visible="false"/>
                                            <asp:BoundField DataField="zip" HeaderStyle-HorizontalAlign="Center" 
                                                HeaderText="Zip Code" ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="vcid" HeaderStyle-HorizontalAlign="Center" 
                                                HeaderText="VC Id./MAC Id." ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="stb" HeaderStyle-HorizontalAlign="Center" 
                                                HeaderText="STB No." ItemStyle-HorizontalAlign="left" />
                                                <asp:BoundField DataField="dt" HeaderStyle-HorizontalAlign="Center" 
                                                HeaderText="Date & Time" ItemStyle-HorizontalAlign="left" DataFormatString="{0:dd-MM-yyyy hh:mm:ss}"/>
                                            <asp:TemplateField HeaderText="CAF">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkPdf" runat="server" OnClick="lnkPdf_Click">View</asp:LinkButton>
                                                 
                                                    <asp:HiddenField ID="hdnaccno" runat="server" Value='<%# Eval("accno").ToString()%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="KYC">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkkyc" runat="server" OnClick="lnkkyc_Click">View</asp:LinkButton>
                                                  
                                                    <asp:HiddenField ID="hdnidproofpath" runat="server" Value='<%# Eval("IDPATH").ToString()%>'/>
                                                    <asp:HiddenField ID="hdnresiproofpath" runat="server" Value='<%# Eval("ADDPROOF").ToString()%>'/>
                                                    <asp:HiddenField ID="hdndocumentproofpath" runat="server" Value='<%# Eval("documentpath").ToString()%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
                <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                <asp:ImageButton ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()">
                </asp:ImageButton>
            </div>
      
</asp:Content>
