<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rptAddPlanTransMSO_JV.aspx.cs" Inherits="PrjUpassPl.Reports.rptAddPlanTransMSO_JV" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
         
        .style68
        {
            width: 175px;
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
          window.location.href = "../Reports/rptnoncasreport.aspx";
          return false;
      }
      function checkvalid() {


          var rbtnvalue = $('#<%=RadSearchby.ClientID %> input[type=radio]:checked').val();

          if (rbtnvalue == 1) {

              if (document.getElementById("<%=txtsearchpara.ClientID%>").value == "") {
                  alert("Please Enter VC No.!!");
                  return false;
              }
          }
      }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="maindive">
                    <div style="float: right">
                        <button onclick="return goBack()" style="margin-right: 5px; margin-top: -15px;" class="button">
                            Back</button>
                    </div>
                    <div class="tblSearchItm" style="width: 65%;">
                        <table width="100%">
                            <tr>
                                <td align="right" class="style68">
                                    From Date:
                                    
                                    </td>
                                    
                                    <td  align="left">
                                    <asp:TextBox runat="server" ID="txtFrom" BorderWidth="1"></asp:TextBox>
                                    <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            
                                <td align="right" class="cal_image_holder"  >
                                    &nbsp;&nbsp; To Date :
                                    
                                   </td>
                                
                                 <td class="cal_image_holder" align="left" >
                                    <asp:TextBox runat="server" ID="txtTo" BorderWidth="1"></asp:TextBox>
                                    <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                                        Format="dd-MMM-yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                            <td align="right" class="style68">
                                Plan Type:
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPlantype" runat="server" AutoPostBack="True" Height="23px"
                                    Width="163px" >
                                    <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Basic" Value="B"></asp:ListItem>
                                    <asp:ListItem Text="Addon" Value="AD"></asp:ListItem>
                                    <asp:ListItem Text="A-La-Carte" Value="AL"></asp:ListItem>
                                    <asp:ListItem Text="Hathway Special Plan" Value="HSP"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="right">
                                Transaction Type :
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlTranType" runat="server" AutoPostBack="True" Height="23px"
                                    Width="163px" >
                                    <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Renewal" Value="'R'"></asp:ListItem>
                                    <asp:ListItem Text="Activation" Value="'A'"></asp:ListItem>
                                    <asp:ListItem Text="Cancellation" Value="'C','CH'"></asp:ListItem>
                                    <asp:ListItem Text="Failure Refund" Value="'RR','AR','CR','CHR'"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                              <td align="right">
                                Pay Term :
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPayTerm" runat="server" AutoPostBack="True" Height="23px"
                                    Width="163px" >
                                    <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                                <td align="right" class="style68">
                                    <asp:Label ID="lblUser" runat="server" Text="Search LCO By :"></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="RadSearchby" AutoPostBack="true" runat="server" 
                                        RepeatDirection="Horizontal" 
                                        onselectedindexchanged="RadSearchby_SelectedIndexChanged1">
                                        <asp:ListItem Value="0" style="display:none">Account No.</asp:ListItem>
                                        <asp:ListItem Value="1">VC Id</asp:ListItem>
                                        <asp:ListItem Value="2" style="display:none">LCO Code</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsearchpara" runat="server" Style="resize: none;" Width="180px"
                                         MaxLength=20 onkeydown="SetContextKey()" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                    
                                </td>
                            </tr>
                            <tr>
                            <td colspan="4" align="center">
                            <asp:Button runat="server" ID="btnSubmit" Text="Search" CssClass="button" 
                                        OnClick="btnSubmit_Click" OnClientClick="return checkvalid();"/>
                                         <asp:Button runat="server" ID="Button1" Text="Reset" CssClass="button" 
                                        OnClick="btnreset_Click"/>
                            </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblSearchMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table>
                        <table width="100%">
                            <tr>
                                <td style="width: 100px">
                                    <asp:Button runat="server" ID="btn_genExl" Text="Generate Excel" CssClass="button"
                                        Visible="false" align="left" OnClick="btn_genExl_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSearchParams" runat="server"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:Button runat="server" ID="btnAll" Text="ALL" CssClass="button" UseSubmitBehavior="false"
                                        Visible="false" OnClick="btnAll_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <div class="griddiv">
                                        <asp:GridView runat="server" ID="grdAddPlanSearch" CssClass="Grid" AutoGenerateColumns="false"
                                            ShowFooter="true" OnRowDataBound="grdAddPlanSearch_RowDataBound" AllowPaging="true"
                                            PageSize="5" OnSorting="grdAddPlanSearch_Sorting" OnRowCommand="grdAddPlanSearch_RowCommand"
                                            OnPageIndexChanging="grdAddPlanSearch_PageIndexChanging">
                                            <FooterStyle CssClass="GridFooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Company Name" ItemStyle-HorizontalAlign="Left" FooterText="Total">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"msoname")%>'
                                                            CommandName="MSOName"></asp:LinkButton>
                                                        <asp:Label ID="lblOperid1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"msoid")%>'
                                                            Visible="false"></asp:Label><asp:Label ID="lblolconame" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"msoname")%>'
                                                                Visible="false"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:BoundField DataField="lcocode" HeaderText="LCO Code" SortExpression="lcocode"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="65pt" />--%>
                                                <asp:BoundField DataField="amtdd" HeaderText="Amount Deducted" SortExpression="amtdd"
                                                    HeaderStyle-HorizontalAlign="Center" ControlStyle-Width="35pt" ItemStyle-HorizontalAlign="Right"
                                                    FooterStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="cnt" HeaderText="Count" SortExpression="cnt" HeaderStyle-HorizontalAlign="Center"
                                                    ControlStyle-Width="35pt" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                </div>
            </asp:Panel>
            <div id="imgrefresh2" class="loader transparent">
                <%--<asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." />--%>
                <asp:ImageButton ID="imgUpdateProgress2" runat="server" ImageUrl="~/Images/loader.GIF"
                    AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()">
                </asp:ImageButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:PostBackTrigger ControlID="btn_genExl" />
            <asp:PostBackTrigger ControlID="btnAll" />
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
</asp:Content>
