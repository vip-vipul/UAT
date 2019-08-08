<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="rptBulkUpload.aspx.cs" Inherits="PrjUpassPl.Reports.rptBulkUpload" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
    function goBack() {
        window.location.href = "../Reports/rptnoncasreport.aspx";
        return false;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div class="maindive">
    <div style="float:right">
                <button onclick="return goBack()"  style="margin-right:5px;margin-top:-15px;"   class="button">Back</button>
                </div>
        <table width="100%">
         <tr>
                                    <td align="center">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="Label3" runat="server" Text="LCO Name"></asp:Label>
                                        &nbsp;:
                                        <asp:DropDownList ID="ddlLco" runat="server" AutoPostBack="true" Height="19px" 
                                            Style="resize: none;" Width="304px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
            <tr>
                <td align="center" class="cal_image_holder">
                    From Date :
                    <asp:TextBox runat="server" ID="txtFrom"></asp:TextBox>
                    <asp:Image runat="server" ID="imgFrom" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                    <cc1:CalendarExtender runat="server" ID="calFrom" TargetControlID="txtFrom" PopupButtonID="imgFrom"
                        Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td align="center" class="cal_image_holder">
                    &nbsp;&nbsp; To Date :
                    <asp:TextBox runat="server" ID="txtTo"></asp:TextBox>
                    <asp:Image runat="server" ID="imgTo" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                    <cc1:CalendarExtender runat="server" ID="calTo" TargetControlID="txtTo" PopupButtonID="imgTo"
                        Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="button" UseSubmitBehavior="false"
                        OnClick="btnSubmit_Click" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblSearchMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblSearchParams" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <div class="griddiv">
        <asp:GridView runat="server" ID="grdBulkUpload" CssClass="Grid" AutoGenerateColumns="false"
            Visible="false" OnRowCommand="grdBulkUpload_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sr. No." ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                        <asp:HiddenField ID="hdnUploadId" runat="server" Value='<%# Eval("uploadid")%>' />
                        <asp:HiddenField ID="hdnFileName" runat="server" Value='<%# Eval("filename")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="File Name" DataField="filename" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="left" FooterText="" />
                <asp:BoundField HeaderText="Status" DataField="status" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="left" FooterText="" />
                <asp:TemplateField HeaderText="Total Transaction" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbnTotal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"total")%>'
                            CommandName="total"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Success" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbnSuccess" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"success")%>'
                            CommandName="success"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Failed" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbnFail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"failed")%>'
                            CommandName="fail"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:BoundField HeaderText="Insert Date" DataField="insdt" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="left" FooterText="" />
            </Columns>
        </asp:GridView>
        </div>
    </div>
     </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>
