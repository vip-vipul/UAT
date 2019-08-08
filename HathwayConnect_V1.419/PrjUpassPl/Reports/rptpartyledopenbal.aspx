<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptpartyledopenbal.aspx.cs"
    Inherits="PrjUpassPl.Reports.rptpartyledopenbal" MasterPageFile="~/MasterPage.Master" %>

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
        .style67
        {
            height: 31px;
        }
        .delInfo
        {
            width: 95%;
            margin: 5px;
            padding: 10px;
            border: 1px solid #094791;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <script type="text/javascript">
        function goBack() {
            window.location.href = "../Reports/rptnoncasreport.aspx";
            return false;
        }
    </script>
    <script type="text/javascript">

        function InProgress() {

            document.getElementById("imgrefresh2").style.visibility = 'visible';
        }
        function onComplete() {

            document.getElementById("imgrefresh2").style.visibility = 'hidden';
        }         
    </script>
    <contenttemplate>        
        <div class="maindive"> <br />
            <asp:Panel runat="server" ID="pnlSearch">                
                 
                    <div class="tblSearchItm" style="width: 50%;">
                    <div class="delInfo" id="divsearchLco" runat="server">
                             <table width="120%">
                            <tr ID="tr4" runat="server" width="60%">
                                <td>
                                    <asp:Label ID="lblcity" runat="server" Text="City:" Visible="true"></asp:Label>
                                    </td>                                
                                <td align="left">
                                    <asp:DropDownList ID="ddladdcity" runat="server" AutoPostBack="True" Height="21px" 
                                        Visible="true" Width="312px" 
                                        onselectedindexchanged="ddladdcity_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>                                
                            </tr>
                            <tr>
                                <td align="center" class="style67">
                                     &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                                                                         
                                    </td>
                                <td align="left" class="style67">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnsubmit" runat="server" OnClick="btnsubmit_Click" 
                                        Text="Submit" Visible="true" />
                                </td>
                            </tr>
                           
                                 <tr>
                                     <td align="center">
                                         &nbsp;</td>
                                     <td align="left">
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                         <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                                     </td>
                                 </tr> </table>
                            </div>

                                  
                        </asp:Panel>
                        <div id="DivRoot" runat="server" align="left" style="width: 100%;display:none">
                        <div style="overflow: hidden;width:100%" id="DivHeaderRow">
                        </div>
                        <div style="overflow: scroll;width:100%" onscroll="OnScrollDiv(this)" id="DivMainContent" >


                                    <asp:GridView runat="server" ID="grdExpiry" CssClass="Grid" AutoGenerateColumns="false"
                                        ShowFooter="true" AllowPaging="true" PageSize="100"
                                        OnPageIndexChanging="grdExpiry_PageIndexChanging" Visible="False" 
                                        onselectedindexchanged="grdExpiry_SelectedIndexChanged" Width="100%">
                                        <%--OnRowCommand="grdLcoPartyLedger_RowCommand" OnRowDataBound="grdLcoPartyLedger_RowDataBound"
                        OnSorting="grdLcoPartyLedger_Sorting"--%>
                                        <FooterStyle CssClass="GridFooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="LCO Code" DataField="var_partled_lcocode" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" 
                                                FooterStyle-HorizontalAlign="Right" >
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField HeaderText="LCO Name" DataField="var_partled_lconame" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" 
                                                FooterStyle-HorizontalAlign="Right" >
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Opening Balance" DataField="num_partled_openinbal" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Date" DataField="partyleddate" HeaderStyle-HorizontalAlign="Center"
                                                Visible="true" ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           
                                        </Columns>
                                        <PagerSettings Mode="Numeric" />
                                    </asp:GridView>
                                

 </div>
                        <div id="DivFooterRow" style="overflow: hidden">
                        </div>
                    </div>                   
                   </div>
                    <div id="imgrefresh2" class="loader transparent">
                    <asp:ImageButton ID="imgUpdateProgress2"  runat="server" ImageUrl="~/Images/loader.GIF" AlternateText="Loading ..." ToolTip="Loading ..." OnClientClick="onComplete()"></asp:ImageButton>
                </div>
        </contenttemplate>
</asp:Content>
