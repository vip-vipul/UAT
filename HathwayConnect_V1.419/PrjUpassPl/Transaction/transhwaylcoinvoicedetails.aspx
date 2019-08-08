<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="transhwaylcoinvoicedetails.aspx.cs"  MasterPageFile="~/MasterPage.Master" Inherits="PrjUpassPl.Transaction.transhwaylcoinvoicedetails" %>
<%@ mastertype virtualpath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .topHead
        {
            background: #E5E5E5;
            width: 75%;
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
    </style>
     <script type="text/javascript">
         function InProgress() {
             document.getElementById("imgrefresh").style.visibility = 'visible';
         }
         function onComplete() {
             document.getElementById("imgrefresh").style.visibility = 'hidden';
         }

         function goBack() {
             window.history.back();
         }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">

    <asp:UpdatePanel ID="upl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="maindive">
              <div style="float:right">
                <button onclick="goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
            <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>            
                <table cellspacing="0" cellpadding="0" align="center" border="0" width="80%">
                    <tr>
                        <td align="center">
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center">
                                        <div class="delInfo">
                                            <table cellpadding="2" width="96%">
                                              <tr>
                                            <td style="padding-left: 35px">
                                                    </td>
                                            <td style="padding-left: 35px" align="right">
                                                    <asp:Label ID="lblccno" runat="server" Text="LCO CODE"></asp:Label>                                                    
                                                    </td>
                                             <td>
                                             <asp:Label ID="Label1" runat="server" Text=":"></asp:Label>                                                    
                                                            </td>
                                                        <td align="left">
                                                         <asp:DropDownList
                                            ID="ddllco" runat="server" >
                                        </asp:DropDownList>
                                                            <%--<asp:TextBox ID="txtlcocode" runat="server" MaxLength="30" Width="90px" 
                                                                ReadOnly="True"></asp:TextBox>--%>
                                                            &nbsp;<asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="button" ValidationGroup="searchValidation"
                                                                UseSubmitBehavior="false" OnClick="btnSearch_Click" />
                                                        </td>
                                                        </tr>
                                                <tr id="Trbillno" runat="server">                                               
                                                    <td style="padding-left: 35px">
                                                    </td>
                                                    <td align="right" class="style70">
                                                        <asp:Label ID="lblbillno" runat="server" Visible="false" Text="Bill No"></asp:Label>                                                       
                                                    </td>
                                                    <td><asp:Label ID="Label2" runat="server" Text=":" Visible="False"></asp:Label>                                                    
                                                            </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlbillno" AutoPostBack="True" runat="server" Height="19px"
                                                            Style="resize: none;" Width="304px" 
                                                            Visible="False">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                
                                                 <tr id="Tr1" runat="server">                                               
                                                    <td style="padding-left: 35px;adding-top: 50px">
                                                    </td>                                                    
                                                    <td style="padding-top: 50px">&nbsp;</td>
                                                    <td align="left" style="padding-top: 50px">
                                                        
                                                   </td>
                                                   <td align="left" style="padding-top: 50px">
                                                   <asp:Button ID="btnpdf" runat="server" Visible="false"
                                                            Text="Download Pdf" UseSubmitBehavior="false" 
                                                            ValidationGroup="searchValidation"  CssClass="button" Width="180px" onclick="btnpdf_Click" />
                                                   </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </div>
                <div id="imgrefresh" class="loader transparent">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..."
                        ToolTip="Loading ..." Style="" />
                </div>
                  
         
        </ContentTemplate>  
         <Triggers>
            <asp:PostBackTrigger ControlID="btnpdf" />                           
        </Triggers>    
    </asp:UpdatePanel>
        
    <cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server" TargetControlID="upl">
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
