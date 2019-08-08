<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TransHwayInventryOnlinePayRes.aspx.cs" Inherits="PrjUpassPl.Transaction.TransHwayInventryOnlinePayRes" %>
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
            font-size: 18px;
            color: Blue;
            
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
<asp:Panel runat="server" ID="pnlRegisterLCO">
 <div class="maindive">
        <asp:Label ID="lblResponseMsg" ForeColor="Red" runat="server"></asp:Label>
        
         
            <center>
     
        <br />
        <br />
        <br />
        <br />
   
   <div runat="server" id="Divsts">
   <h4 class="subt" > <asp:Label ID="lbltype" runat="server" Text=""></asp:Label><asp:Label ID="Lblrefno" ForeColor="Black" runat="server"></asp:Label></h4>
         <h4 class="subt" >Current Balance :<asp:Label ID="LblCurruntBal" ForeColor="Black" runat="server"></asp:Label></h4>
         <br /><br /><br />
          <asp:Button runat="server" ID="btnBck" Text="Go to Balance Allocation" 
            Width="180px" CssClass="button" UseSubmitBehavior="false" 
            onclick="btnBck_Click" />
             <asp:Button runat="server" ID="BtngenexportPDF" Text="Download Payment Reciept" 
            Width="190px" CssClass="button" UseSubmitBehavior="false" onclick="BtngenexportPDF_Click" 
             />
         </div>
    </center>
       </div> 
    </asp:Panel>
</asp:Content>
