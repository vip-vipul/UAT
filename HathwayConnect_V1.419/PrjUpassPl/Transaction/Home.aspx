<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="PrjUpassPl.Transaction.Home" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .blue
        {
            color: DarkBlue;
        }
        .red
        {
            color: Red;
        }
        .logo
        {
            font-size: 50px;
        }
        .subt
        {
            font-size: 25px;
        }
        .tdclass
        {
            padding-left: 30px;
            padding-bottom: 5px;
            padding-top: 5px;
        }
  a.tooltips {
  position: relative;
  display: inline;
}
a.tooltips span {
  position: absolute;
  width:140px;
  color: #C8302D;
  background: #FFFFFF;
  height: 30px;
  line-height: 30px;
  text-align: center;
  visibility: hidden;
  border-radius: 6px;
}
a.tooltips span:after {
  content: '';
  position: absolute;
  top: 100%;
  left: 50%;
  margin-left: -8px;
  width: 0; height: 0;
  border-top: 8px solid #FFFFFF;
  border-right: 8px solid transparent;
  border-left: 8px solid transparent;
}
a:hover.tooltips span {
  visibility: visible;
  bottom: 30px;
  left: 50%;
  margin-left: -70px;
  z-index: 999;
}

.notired
        {
            display: block;
background: #E43C03;
text-align: center;
color: #FFF;
font-size: 12px;
font-weight: bold;
-moz-border-radius: 6px;
-webkit-border-radius: 6px;
border-radius: 24px;
width: 15px;
z-index: 1;
margin: -19px 23px 0px 106px;
position: absolute;
padding: 12px;
height: 15px;
        }
    </style>
    <script type="text/javascript">
     function  lcoconfig() {
         
             window.location.href = "../Reports/EcafPages.aspx";

         }
         function closemsgbox() {

             $find("mpeMsgBox").hide();
         }
         function closemsgbox() {

             $find("mpeMsgBoxGLo").hide();
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <table border="0" align="center" style="margin-top: 90px;">
        <tr>
            <td colspan="4" class="tdclass">
               <%-- <a href="../Master/mstLCOAdminPages.aspx">
                    <img src="../Img/LCO Admin.jpg" alt="" border="0" height="130" width="130"></img></a>--%>
                      <a href="../Master/frmDashboard.aspx">
                    <img src="../Img/DashboardIcon.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
            <td colspan="4" class="tdclass">
                <a href="../Transaction/frmAssignPlan.aspx">
                    <img src="../Img/Pack Management.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
            <td colspan="4" class="tdclass">
                <a href="../Transaction/TransBalanceManagementPages.aspx">
                    <img src="../Img/Balance.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
            <td colspan="4" class="tdclass">
                <a href="../Transaction/TransBulkPages.aspx">
                    <img src="../Img/Bulk.jpg" alt="" border="0" height="130" width="130"></img></a>
            </td>
             <td colspan="4" class="tdclass">
                <span class="notired">
                    <asp:Label ID="lblinboxcount" runat="server" Text=""></asp:Label></span> <a href="../Master/frmMessenger.aspx" class="showfrindreq mesgnotfctn spritimg notifriend">
                    <img src="../Img/Messenger.jpg" alt="" border="0" height="130" width="130"></img>
                </a>
            </td>
        </tr>
        <tr>
         <td colspan="4" class="tdclass">
          <a href="../Reports/rptnoncasreport.aspx">
                    <img src="../Img/Report.jpg" alt="" border="0" height="130" width="130"></img></a>
              
            </td>
            <td colspan="4" class="tdclass">
            <a href="../Transaction/frmInventoryManu.aspx">
                <img src="../Img/Inventory_Management_Icon.jpg" alt="" border="0" height="130" width="130" ></img>
                </a>
            </td>
            <td colspan="4" class="tdclass">
                  <img src="../Img/E-CAF.jpg" alt="" border="0" height="130" width="130" onclick="lcoconfig();"></img>
                <asp:HiddenField ID="hdnlco" runat="server" ClientIDMode="Static"/>
            </td>
            <td colspan="4" class="tdclass">
               
                <a href="../Transaction/TransNotificationPages.aspx">
                    <img src="../Img/Notification.jpg" alt="" border="0" height="130" width="130" ></img></a>
            </td>
          
            <td colspan="4" class="tdclass">
          

               <a href="../Master/mstLCOAdminPages.aspx">
                    <img src="../Img/LCO Admin.jpg" alt="" border="0" height="130" width="130"></img></a>
              
            </td>
        </tr>
         <tr>
           <td colspan="4" class="tdclass">
            <a href="../Reports/rptnoncasreport.aspx">
              <asp:ImageButton ID="imgAndroid" ImageUrl="../Img/Android.png" Height="50" 
                 Width="130"  runat="server" onclick="imgAndroid_Click"/>
             </a>
             
            </td>
             <td colspan="4" class="tdclass">
             <a href="../Transaction/frmSelfcare.aspx">
                    <img src="../Img/Selfcare.jpg" alt="" border="0" height="50" width="130"></img></a>
            </td>
         <td colspan="4" class="tdclass">
         
            </td>
              <td colspan="4" class="tdclass">
             <a href="../Reports/rptHwaylcolegelDet.aspx">
                    <asp:ImageButton ID="BtnImgGlobalRenewal" ImageUrl="~/Img/GlobalAutoRenewal.jpg" Height="50"
                        Width="130" runat="server" OnClick="BtnImgGlobalRenewal_Click" />

            </td>
              <td colspan="4" class="tdclass">
               <a href="../Reports/rptHwaylcolegelDet.aspx">
                    <img src="../Img/LegaliconNew.jpg" alt="" border="0" height="50" width="130"></img></a>
             
            
            </td>
        </tr>
        <%--     <td colspan = 4 style="padding:10px 30px 10x 10px;padding-left:20px;"><img src="../Img/LCO Admin.jpg" alt="" border=0 height=100 width=100></img></td>
        <td colspan = 4 style="padding:10px 30px 10x 10px;padding-left:20px;"><img src="../Img/Pack Management.jpg" alt="" border=0 height=100 width=100></img></td>
        <td colspan = 4 style="padding:10px 30px 10x 10px;padding-left:20px;"><img src="../Img/Balance.jpg" alt="" border=0 height=100 width=100></img></td>
        <td colspan = 4 style="padding:10px 30px 10x 10px;padding-left:20px;"><img src="../Img/Bulk.jpg" alt="" border=0 height=100 width=100></img></td>
        <td colspan = 4 style="padding:10px 30px 10x 10px;padding-left:20px;"><img src="../Img/Customer Details.jpg" alt="" border=0 height=100 width=100></img></td></tr>
        --%>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <!---------------------------cnfmPopup------------------------>
            <cc1:ModalPopupExtender ID="popMsgBox" runat="server" BehaviorID="mpeMsgBox" TargetControlID="hdnMsgBox"
                PopupControlID="pnlMsgBox">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnMsgBox" runat="server" />
            <asp:Panel ID="pnlMsgBox" runat="server" CssClass="Popup" Style="width: 510px; height: 160px;
                display: none">
                <%-- display: none; --%>
                <center>
                    <br />
                    <table width="100%">
                    <tr>
                            <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                &nbsp;&nbsp;&nbsp;Should the Hathway connect mobile app download link be sent to :
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
                            <td align="left" >
                                <asp:CheckBox ID="ckOld" runat="server" Checked="true" OnCheckedChanged="ChckedChanged1" AutoPostBack="true"  Text="LCO Registered Mobile Number : "/>
                            </td>
                            <td>
                            <asp:TextBox runat ="server" ID="txtLCOExist" ReadOnly="true" Text="" ForeColor="Gray"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                               
                                <asp:CheckBox ID="ckNew" runat="server"  Text="Other Mobile Number : " OnCheckedChanged="ChckedChanged" AutoPostBack="true" />
                            </td>
                            <td>
                            <asp:TextBox runat ="server" ID="txtNewContactNo" Text=""></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtNewContactNo"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="btncnfmBlck" runat="server" Text="Confirm" class="button" OnClick="btncnfmBlck_Click" />
                                &nbsp;&nbsp;
                                <input id="Button3" class="button" runat="server" type="button" value="Cancel" style="width: 100px;"
                                    onclick="closemsgbox();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%--Global Auto renewal PoPup Start--%>
            <cc1:ModalPopupExtender ID="popglobalmsgbox" runat="server" BehaviorID="mpeMsgBoxGLo"
                TargetControlID="hdnglobal" PopupControlID="pnlGlobal">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnglobal" runat="server" />
            <asp:Panel ID="pnlGlobal" runat="server" CssClass="Popup" Style="width: 510px; height: 160px;
                display: none">
                <%-- display: none; --%>
                <center>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                                &nbsp;&nbsp;&nbsp;Global Auto Renewal
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
                            <td align="center">
                                <asp:Label Text="" ID="lblGlobalmsg" runat="server" Visible="false" />
                                <asp:Label Text="" ID="lblmsg" Style="text-align: center" runat="server" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                            <asp:Button ID="btnconfirm" runat="server" Text="Yes" class="button" OnClick="btnconfirm_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnsubmit" runat="server" Text="Confirm" class="button" OnClick="btnsubmimCon_Click" />
                                &nbsp;&nbsp;
                                <input id="btncancel" class="button" runat="server" type="button" value="Cancel"
                                    style="width: 100px;" onclick="closemsgbox();" />
                                <input id="btnclose" class="button" runat="server" type="button" value="Close" style="width: 100px;"
                                    onclick="closemsgbox();" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%--Global Auto renewal PoPup End--%>
            </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="ckNew" />
            <asp:PostBackTrigger ControlID="ckOld" />
        </Triggers>
            </asp:UpdatePanel>
</asp:Content>
