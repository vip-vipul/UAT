<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="mstComplaintRegistration.aspx.cs" Inherits="PrjUpassPl.Master.mstComplaintRegistration" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
     .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            margin: 10px;
            width: 75%;
        }
        .delInfoContent
        {
            width: 95%;
        }
 </style>
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

     <style type="text/css">
        .topHead
        {
            background: #E5E5E5;
        }
        .delInfo
        {
            padding: 10px;
            border: 1px solid #094791;
            margin: 10px;
            width: 75%;
        }
        .delInfoContent
        {
            width: 95%;
        }
        .scroller
        {
            overflow: auto;
            max-height: 250px;
        }
        .completionList
        {
            border: solid 1px Gray;
            margin: 0px;
            padding: 3px;
            height: 120px;
            overflow: auto;
            background-color: #FFFFFF;
        }
        .listItem
        {
            color: #191919;
        }
        .itemHighlighted
        {
            background-color: #ADD6FF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
 <asp:Label ID="lblSearchResponse" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
    <div style="Background-color:white;">
        <table cellspacing="0" cellpadding="0" align="center" border="0" width="80%">
            <tr>
                <td align="center">
                    <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="center">
                                <div class="delInfo">
                                    <table runat="server" align="center" width="100%" id="tbl1" border="0">
                                        <tr>
                                          
                                            <td align="left" class="style67">
                                                <asp:RadioButtonList ID="RadSearchby" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                                    OnSelectedIndexChanged="RadSearchby_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Selected="True">Register Ticket</asp:ListItem>
                                                    <asp:ListItem Value="1" >Ticket History</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                          
                                          <td>
                                           <div id="divcustomer" runat="server"> 
                                                <table runat="server" align="center" width="100%" id="Table1" border="0">
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    &nbsp;From Date :
                                    <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1"></asp:TextBox>
                                    <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="cal_image_holder">
                                    &nbsp;&nbsp;&nbsp; To Date :
                                    <asp:TextBox runat="server" ID="txtTo" BorderWidth="1"></asp:TextBox>
                                    <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                           
                            <tr>
                                <td align="center">
                                    <table style="width: 78%">
                                        <tr>
                                            <td align="center">
                                                Account No.
                                                 <asp:TextBox ID="txtsearchpara" runat="server"  Width="100px" MaxLength="10"
                                                    Height="15px" ></asp:TextBox>
                                                     <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                                    CssClass="button"  />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                                            </div>
                                           </td>
                                        </tr>
                                        <tr>
                                        <td colspan="2" align="center">
                                        <asp:Label ID="lblmsg" runat="server" ForeColor=Red></asp:Label>
                                        </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divdet" runat="server">
                                    <div class="delInfo">
                                        <table class="delInfoContent">
                                            
                                            <tr>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="Label21" Text="Name"></asp:Label>
                                                </td>
                                                <td width="10px">
                                                    <asp:Label runat="server" ID="Label11" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                   <asp:TextBox ID="txtname" runat="server" width="280px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="Label12" Text="Address"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label13" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtaddress" runat="server" TextMode="MultiLine" MaxLength="200" width="280px"></asp:TextBox> 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="Label15" Text="Mobile No."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label2" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtmobno" runat="server" MaxLength="10" width="280px"></asp:TextBox>  
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtmobno"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="Label1" Text="Email ID."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label3" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtemailid" runat="server" MaxLength="100" width="280px"></asp:TextBox>  
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="Label5" Text="Company Name."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label8" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtcompanyname" runat="server" MaxLength="100" width="280px"></asp:TextBox>  
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="Label9" Text="Alternate No."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label10" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtAlternateno" runat="server" MaxLength="10" width="280px"></asp:TextBox>  
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtAlternateno"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="130px">
                                                    <asp:Label runat="server" ID="Label7" Text="Complaint Type"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label19" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlcomplainttype" runat="server"  width="280px">
                                                    </asp:DropDownList>    
                                                  </td>
                                            </tr>
                                             <tr>
                                                <td align="left" width="130px">
                                                    <asp:Label runat="server" ID="Label4" Text="Complaint"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label6" Text=":"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtcomplaint" runat="server" TextMode="MultiLine"  width="280px" ></asp:TextBox>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td align="center" colspan="3" >
                                                    <asp:Button ID="btnsubmit"  runat="server" Font-Bold="True" Text="Submit"
                                                        class="button" Width="60" Height="20px" OnClick="btnSubmit_Click"></asp:Button>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btncnacel"  runat="server" Font-Bold="True" Text="Cancel" 
                                                        class="button" Width="60" Height="20px" OnClick="btnCancel_Click"></asp:Button>
                                                    
                                                   
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                  
                                </div>

                                               <div class="griddiv" runat="server" id="griddiv" style="width: 75%;">
                                  <asp:GridView ID="GridCompList" runat="server" AlternatingRowStyle-CssClass="GrdAltRow"
                            AutoGenerateColumns="false" CssClass="Grid" Width="98%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false"
                                    ItemStyle-Width="5px" ControlStyle-Font-Bold="false">
                                    <HeaderStyle Wrap="false" />
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ControlStyle Font-Bold="False" />
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="cmpno" HeaderText="Comp. No." ItemStyle-Width="80px" ItemStyle-HorizontalAlign="left"
                                    ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="custnm" HeaderText="Name" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="left"
                                    ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="custno" HeaderText="Mobile No" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="right" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="cmpdesc" HeaderText="Description" HeaderStyle-Width="240px"
                                    ItemStyle-Width="120px" ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="cmptype" HeaderText="Comp. Type" ItemStyle-Width="150px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="cmpsubtype" HeaderText="Sub Type" ItemStyle-Width="9px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="cmpstatus" HeaderText="Status" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="srvst" HeaderText="Service Status" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="regdt" HeaderText="Comp. Date" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="assgnuser" HeaderText="Assign User" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="userremark" HeaderText="User Remark" ItemStyle-Width="110px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="remarkdate" HeaderText="Remark Date" DataFormatString="{0:dd-MM-yyyy}"
                                    ItemStyle-Width="80px" ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="source" HeaderText="Source" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                    
                                       <asp:BoundField DataField="Flag" HeaderText="Flag" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                       <asp:BoundField DataField="lcocode" HeaderText="LCO Code" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                       <asp:BoundField DataField="companyname" HeaderText="Company Name" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="callername" HeaderText="Caller Name" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="callerno" HeaderText="Caller No" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="alternateno" HeaderText="Alternate No" ItemStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" />
                            </Columns>
                        </asp:GridView>
                                  <%--  <asp:GridView ID="grd" runat="server" CssClass="Grid" ShowFooter="true" Width="100%"
                                        AllowPaging="true" PageSize="100" OnPageIndexChanging="grd_PageIndexChanging">
                                        <FooterStyle CssClass="GridFooter" />
                                    </asp:GridView>--%>
                                </div>
                 
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
