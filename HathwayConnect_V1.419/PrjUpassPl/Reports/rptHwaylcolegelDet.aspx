<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptHwaylcolegelDet.aspx.cs" Inherits="PrjUpassPl.Reports.rptHwaylcolegelDet" %>
    <%@ Register src="~/Usercontrol/MIADOC_NEW.ascx" tagname="MIADOC" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ mastertype virtualpath="~/MasterPage.Master" %>
    <style type="text/css">
        .l1
        {
            text-decoration: none;
            cursor: hand;
        }
        .l2
        {
            text-decoration: underline;
            cursor: hand;
        }
        table.list
        {
            font-family: Verdana;
            font-size: larger;
        }
        
        
        
        table.dList1
        {
            font-family: Verdana;
            font-size: large;
            width: 100%;
            background-color: black;
           
        }
        table.dList1 td
        {
            background-color: white;
        }
        table.dList1 td.srno
        {
            background-color: black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:Panel ID="pnlView" runat="server">
        <div class="maindive">
            <div class="griddiv">
            <div style="padding-left:20px;padding-right:20px" >

                <table  cellpadding="3" cellspacing="2" class="dList1" width="80%" height="30px">
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="excelOSD" CssClass="l1" NavigateUrl="../LegelContent/Consumer Complaint Redressal Regulation 2012.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Consumer Complaint Redressal Regulation 2012</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                       
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink1" CssClass="l1" NavigateUrl="../LegelContent/Interconnection (DAS) Regulations, 2012.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Interconnection (DAS) Regulations, 2012</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                       
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink2" CssClass="l1" NavigateUrl="../LegelContent/Interconnection (DAS) Regulations, Feb 2014.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Interconnection (DAS) Regulations, Feb 2014</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                       
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink3" CssClass="l1" NavigateUrl="../LegelContent/Interconnection (DAS) Regulations, July 2014.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Interconnection (DAS) Regulations, July 2014</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                       
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink4" CssClass="l1" NavigateUrl="../LegelContent/Interconnection Regulations (Eighth ammendement), July 2014.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Interconnection Regulations (Eighth ammendement), July 2014</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                     
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink5" CssClass="l1" NavigateUrl="../LegelContent/Interconnection Regulations (Fifth ammendement), Feb 2014.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Interconnection Regulations (Fifth ammendement), Feb 2014</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                      
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink6" CssClass="l1" NavigateUrl="../LegelContent/Tariff Order Addressable Systems July 2014.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Tariff Order Addressable Systems July 2014</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                     
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink7" CssClass="l1" NavigateUrl="../LegelContent/Tariff Order January 2015.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Tariff Order January 2015</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                        
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink8" CssClass="l1" NavigateUrl="../LegelContent/Cable TV Rules_1994.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Cable TV Rules_1994</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                      
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink9" CssClass="l1" NavigateUrl="../LegelContent/Cable TV_Amendment Act_2011.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Cable TV_Amendment Act_2011</b></font>
                                            
                            </asp:HyperLink>
                        </td>
             
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink10" CssClass="l1" NavigateUrl="../LegelContent/Consumer Complaint Redressal_Main Regulation_14.05.2012.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Consumer Complaint Redressal_Main Regulation_14.05.2012</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                        
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink11" CssClass="l1" NavigateUrl="../LegelContent/Amendment_25.03.2015.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Amendment_25.03.2015</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                    
                    </tr>
                      <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink12" CssClass="l1" NavigateUrl="../LegelContent/Main Regulation.pdf"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Main Regulation</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                      
                    </tr>

                     <tr id="trSIA" runat="server">
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink13" CssClass="l1" NavigateUrl="~/Reports/rptDownloadAgreement.aspx?Flag=SIA"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>SIA Agreement File</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                      
                    </tr>

                     <tr id="trMIA" runat="server">
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink14" CssClass="l1" NavigateUrl="~/Reports/rptDownloadAgreement.aspx?Flag=MIA"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>MIA Agreement File</b></font>
                                            
                            </asp:HyperLink>
                        </td>
                      
                    </tr>

                    <tr id="trMIAagree" runat="server">
                        <td align="left" valign="top" width="90%">
                            <asp:LinkButton ID="lnkmiaagreement" runat="server" OnClick="lnkmiaagreement_Click"> 
                            <font size="2"><b>Digitally Accepted MIA Agreement</b></font>
                            
                            </asp:LinkButton>

                             <asp:Panel ID="Panel1" runat="server" Style="width: 99%; height: 500px; overflow: auto;
                                                                margin-left: 1%;display:none;" >
                                                              <uc1:MIADOC ID="MIADOC2" runat="server" />
                                                            </asp:Panel>
                            
                        </td>
                      
                    </tr>
                     <tr>
                        <td align="left" valign="top" width="90%">
                            <asp:HyperLink ID="HyperLink15" CssClass="l1" NavigateUrl="../LegelContent/Nodal_Bank_Setup_Hathway.doc"
                                runat="server" onmouseout=" this.className='l1'" onmouseover="this.className='l2'">
                                            <font size="2"><b>Nodal Bank Set-up Form </b></font>
                                            
                            </asp:HyperLink>
                        </td>
                    
                    </tr>
                </table>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
