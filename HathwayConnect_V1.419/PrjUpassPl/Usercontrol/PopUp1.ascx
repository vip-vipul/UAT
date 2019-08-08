<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopUp1.ascx.cs" Inherits="PrjUpassPl.Usercontrol.PopUp1" %>
<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>




<script type="text/javascript">

 function HideModalPopup() {
        $find("modal").hide();
        return false;
    }
    </script>
<style type="text/css">
    body
    {
        font-family: Arial;
        font-size: 10pt;
    }
    .modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=60);
        opacity: 0.1;
    }
    .modalPopup
    {
        
        width: 300px;
        
        padding: 0;
    }
    .modalPopup .header
    {
        
    }
    .modalPopup .body
    {
       
       
    }
    .modalPopup .footer
    {
        padding: 6px;
    }
    .modalPopup .yes, .modalPopup .no
    {
        
    }
    .modalPopup .yes
    {
       
    }
    .modalPopup .no
    {
        background-color: #9F9F9F;
        border: 1px solid #5C5C5C;
    }
</style>

<noscript>
    <div style="border: 1px solid purple; padding: 10px">
        <span style="color: red">JavaScript is not enabled!</span>
    </div>
</noscript>
<asp:HiddenField ID="msgAndTime" runat="server" />
<asp:LinkButton ID="lnk" runat="server"></asp:LinkButton>

<ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnk"
    OkControlID="btnok" BackgroundCssClass="modalBackground" BehaviorID="modal">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
  
  <div>
 
 
  </div>
    <div class="body">
        <img src="../Images/closebtn.png"  ID="btnok" style="margin-left:380px;cursor: pointer;" onclick="HideModalPopup()" />
   <img src="../Images/Final_HD_scheme_mailer.jpg"  height="570px"/>  
    </div>
   
</asp:Panel>
