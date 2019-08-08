<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmMessenger.aspx.cs" Inherits="PrjUpassPl.Master.frmMessenger" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/jquery.sweet-dropdown.min.css" rel="stylesheet" type="text/css" />
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../JS/jquery-ui.js" type="text/javascript"></script>
    <script src="../JS/jquery.sweet-dropdown.min.js" type="text/javascript"></script>
    <link href="../CSS/chosen.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Search_Gridview(strKey, strGV) {
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById(strGV);
            var rowData;
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }
        function back() {

            window.location.href = "../Master/mstLCOAdminPages.aspx";
            return false;
        }

        function closeMailPopup() {

            $find("mpeMail").hide();
            return false;
        }
        function closeMailpnlPopOpenMail() {

            $find("mpePopOpenMail").hide();
            return false;
        }

        function closeconfirm() {
            $find("mpeConfirm").hide();
            return false;
        }

        function closemsgbox() {
            $find("mpeMsgBox").hide();
            return false;
        }



    </script>
    <style type="text/css">
        .Grid th
        {
            padding: 5px;
            background-color: #094791;
            font-size: 11px;
            height: 16px;
            color: #ffffff;
            font-weight: bold;
            text-align: center;
        }
        .Grid tr
        {
            cursor: pointer;
        }
        .chosen-choices
        {
            width: 400px !important;
        }
        .chosen-drop
        {
            width: 400px !important;
        }
        .default
        {
            width: 100px !important;
        }
        .delInfo
        {
            /*padding: 10px;
            border: 1px solid #094791;*/
            width: 95%;
            margin: 5px;
            padding: 10px;
            border: 1px solid #094791;
        }
        .delInfoContent
        {
            width: 95%;
        }
        .style68
        {
        }
        .style72
        {
            width: 96px;
        }
        ul
        {
            list-style: none;
        }
        ol
        {
            list-style: decimal;
            font-size: 24px;
            width: 400px;
            padding: 30px 0 0 60px;
            margin: 0 auto;
        }
        ul#navigation
        {
            height: 36px;
            padding: 20px 20px 0 30px;
            margin: 0 auto;
            position: relative;
            overflow: hidden;
        }
        
        ul#navigation li
        {
            -webkit-border-radius: 6px;
            -moz-border-radius: 6px;
            border-radius: 6px;
            float: left;
            width: 102px;
            margin: 0 10px 0 0;
            background-color: #2B477D;
            border: solid 1px #415F9D;
            position: relative;
            z-index: 1;
        }
        
        ul#navigation li.selected
        {
            z-index: 3;
        }
        
        ul#navigation li.shadow
        {
            width: 100%;
            height: 2px;
            position: absolute;
            bottom: -3px;
            left: 0;
            border: none;
            background: none;
            z-index: 2;
            -webkit-box-shadow: #111 0 -2px 6px;
            -moz-box-shadow: #111 0 -2px 6px;
            box-shadow: #111 0 -2px 6px;
        }
        
        ul#navigation li a:link, ul#navigation li a:visited
        {
            -webkit-border-radius: 6px;
            -moz-border-radius: 6px;
            border-radius: 6px;
            display: block;
            text-align: center;
            width: 100px;
            height: 40px;
            line-height: 36px;
            font-family: Arial, Helvetica, sans-serif;
            text-transform: uppercase;
            text-decoration: none;
            font-size: 13px;
            font-weight: bold;
            color: #fff;
            letter-spacing: 1px;
            outline: none;
            float: left;
            background: #2B477D;
            -webkit-transition: background-color 0.3s linear;
            -moz-transition: background-color 0.3s linear;
            -o-transition: background-color 0.3s linear;
        }
        
        
        ul#navigation li.selected a:link, ul#navigation li.selected a:visited
        {
            color: #2B477D;
            border: solid 1px #fff;
            -webkit-transition: background-color 0.2s linear;
            background: -moz-linear-gradient(top center, #d1d1d1, #f2f2f2 80%) repeat scroll 0 0 #f2f2f2;
            background: -webkit-gradient(linear,left bottom,left top,color-stop(.2, #f2f2f2),color-stop(.8, #d1d1d1));
            background-color: #f2f2f2;
        }
        
        
        .content
        {
            width: 100%;
            background: #f2f2f2;
            padding: 2px 0px 2px 0;
            margin: 0 auto;
        }
        .style73
        {
            width: 96px;
            height: 20px;
        }
        .style74
        {
            height: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MasterBody" runat="server">
    <asp:Panel runat="server" ID="pnlRegisterLCO">
        <div class="maindive">
            <div style="text-align: left; width: 90%">
                <div style="float: right; margin-right: 1px; margin-top: 15px;">
                    <asp:Button ID="btnNewMail" runat="server" Text="New Message" OnClick="btnNewMail_Click"
                        Height="40" />
                </div>
                <ul id="navigation">
                    <li class="one  selected" id="indoxli" runat="server">
                        <asp:LinkButton ID="lnkinbox" runat="server" OnClick="lnkinbox_Click">Inbox</asp:LinkButton></li>
                    <li class="two" id="sentli" runat="server">
                        <asp:LinkButton ID="lnksentitem" runat="server" OnClick="lnksentitem_Click">Sent Item</asp:LinkButton></li>
                    <b>Search By</b> 
                    <asp:Label ID="lbltextfrm" runat="server" Text="From :"></asp:Label>
                    <asp:TextBox ID="txtfrom" runat="server" Text=""></asp:TextBox>
                    Subject :
                    <asp:TextBox ID="txtsubjectsearch" runat="server" Text=""></asp:TextBox>&nbsp;&nbsp;
                    <asp:Button ID="btnsearch" runat="server" Text="Search" Height="20" OnClick="btnsearch_Click" />
                    <%--  <li class="three"  id="draftli" runat="server"><asp:LinkButton ID="lnkdraft" runat="server" OnClick="lnkdraft_Click">Draft</asp:LinkButton></li>
                            <li class="four"  id="trashli" runat="server"><asp:LinkButton ID="lnktrash" runat="server" OnClick="lnktrash_Click">Trash</asp:LinkButton></li>--%>
                </ul>
                <asp:Panel ID="inboxtab" runat="server" CssClass="content">
                    <asp:GridView ID="grdInbox" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                        ClientIDMode="Static" ShowFooter="false" Width="99%" AllowSorting="True" EnableModelValidation="True"
                        Style="margin-left: 0.5%; background-color: white;" OnRowDataBound="grdInbox_RowDataBound"
                        AllowPaging="true" PageSize="50" OnPageIndexChanging="grdInbox_OnPageIndexChanging"
                        OnRowCreated="grdInbox_RowCreated" OnSelectedIndexChanged="grdInbox_OnSelectedIndexChanged">
                        <RowStyle CssClass="cursor: pointer" />
                        <Columns>
                            <asp:BoundField HeaderText="From" DataField="mfrom" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <img src="../Images/attachment.png" style="width: 20px; height: 20px" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgAttach" runat="server" ImageUrl="~/Images/PINImage.png"
                                        Width="20" OnClick="imgAttach_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Subject" DataField="msub" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Date & Time" DataField="mdate" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MM-yyyy hh:mm tt}" />
                            <asp:BoundField HeaderText="Message Type" DataField="mtype" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Message" DataField="mmsg" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="500px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="Message" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblmsgs" runat="server" Text="Label"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Status" DataField="" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Image ID="Imgbasicaction" runat="server" ImageUrl="~/Images/dot3.png" data-dropdown='<%# "#Inbox"+Container.DataItemIndex+1 %>'
                                        Style="width: 40px; height: 25px" ClientIDMode="Static" />
                                    <div class="dropdown-menu dropdown-anchor-bottom-right dropdown-has-anchor" id='<%# "Inbox"+Container.DataItemIndex+1 %>'>
                                        <ul>
                                            <li><a href="#">
                                                <asp:Button ID="lnkReply" runat="server" Text="REPLY" OnClick="lnkReply_Click" Width="110" /></a></li>
                                            <li><a href="#">
                                                <asp:Button ID="lnkDelete" runat="server" Text="DELETE" OnClick="lnkDelete_Click"
                                                    Width="110" CommandName="Inbox" /></a></li>
                                            <li><a href="#">
                                                <asp:Button ID="lnkOpen" runat="server" Text="OPEN" OnClick="lnkOpen_Click" Width="110"
                                                    Visible="false" /></a></li>
                                        </ul>
                                    </div>
                                    <asp:HiddenField ID="hdnINBOXMsgId" runat="server" Value='<%# Eval("mid").ToString()%>' />
                                    <asp:HiddenField ID="hdnINBOXmsgsubId" runat="server" Value='<%# Eval("msubid").ToString()%>' />
                                    <asp:HiddenField ID="hdnINBOXMsgfile" runat="server" Value='<%# Eval("mfile").ToString()%>' />
                                    <asp:HiddenField ID="hdnINBOXMsgType" runat="server" Value='<%# Eval("mtype").ToString()%>' />
                                    <asp:HiddenField ID="hdnINBOXMsgTO" runat="server" Value='<%# Eval("mto").ToString()%>' />
                                    <asp:HiddenField ID="hdnmsgtoall" runat="server" Value='<%# Eval("msgtoall").ToString()%>' />
                                    <asp:HiddenField ID="hdnmsgtoallid" runat="server" Value='<%# Eval("msgtoallid").ToString()%>' />
                                    <asp:HiddenField ID="hdnmsgread" runat="server" Value='<%# Eval("readby").ToString()%>' />
                                    <asp:HiddenField ID="hdnmsgreply" runat="server" Value='<%# Eval("reply").ToString()%>' />
                                    <asp:HiddenField ID="hdnRead" runat="server" Value="N" />
                                    <asp:HiddenField ID="hdndelete" runat="server" Value='<%# Eval("INBOXDELETE").ToString()%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooter" />
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="sentitemtab" runat="server" CssClass="content" Visible="false">
                    <asp:GridView ID="grdSentitem" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                        Width="99%" AllowSorting="True" EnableModelValidation="True" Style="margin-left: 0.5%;
                        background-color: white;" OnRowDataBound="grdSentitem_RowDataBound" AllowPaging="true"
                        PageSize="50" OnPageIndexChanging="grdSentitem_OnPageIndexChanging" OnRowCreated="grdSentitem_RowCreated"
                        OnSelectedIndexChanged="grdSentitem_OnSelectedIndexChanged">
                        <Columns>
                            <asp:BoundField HeaderText="To" DataField="mto" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <img src="../Images/attachment.png" style="width: 20px; height: 20px" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgAttach" runat="server" ImageUrl="~/Images/PINImage.png"
                                        Width="20" OnClick="imgAttachSent_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Subject" DataField="msub" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Date & Time" DataField="mdate" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Message Type" DataField="mtype" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Message" DataField="mmsg" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="500px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="Message" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblmsgs" runat="server" Text="Label"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Image ID="Imgbasicaction" runat="server" ImageUrl="~/Images/dot3.png" data-dropdown='<%# "#Sent"+Container.DataItemIndex+1 %>'
                                        Style="width: 40px; height: 25px" ClientIDMode="Static" />
                                    <div class="dropdown-menu dropdown-anchor-bottom-right dropdown-has-anchor" id='<%# "Sent"+Container.DataItemIndex+1 %>'>
                                        <ul>
                                            <li><a href="#">
                                                <asp:Button ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" Text="Delete"
                                                    Width="110"></asp:Button></a></li>
                                            <li><a href="#">
                                                <asp:Button ID="lnkOpen" runat="server" OnClick="lnkOpenOutBox_Click" Text="Open"
                                                    Width="110" Visible="false"></asp:Button></a></li>
                                        </ul>
                                    </div>
                                    <asp:HiddenField ID="hdnOUTBOXMsgId" runat="server" Value='<%# Eval("mid").ToString()%>' />
                                    <asp:HiddenField ID="hdnOUTBOXmsgsubId" runat="server" Value='<%# Eval("msubid").ToString()%>' />
                                    <asp:HiddenField ID="hdnOUTBOXMsgfile" runat="server" Value='<%# Eval("mfile").ToString()%>' />
                                    <asp:HiddenField ID="hdnOTBOXMsgType" runat="server" Value='<%# Eval("mtype").ToString()%>' />
                                    <asp:HiddenField ID="hdnOUTBOXMsgTO" runat="server" Value='<%# Eval("mto").ToString()%>' />
                                    <asp:HiddenField ID="hdnOUTBOXdelete" runat="server" Value='<%# Eval("sentdelete").ToString()%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooter" />
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="drafttab" runat="server" CssClass="content" Visible="false">
                    Draft
                    <asp:GridView ID="grdDraft" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                        ShowFooter="false" Width="100%" AllowSorting="True" EnableModelValidation="True">
                        <Columns>
                            <asp:BoundField HeaderText="Date & Subject" DataField="" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Subject" DataField="" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="400px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="From" DataField="" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Message" DataField="" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="500px" ItemStyle-HorizontalAlign="Left" />
                            <%-- <asp:BoundField HeaderText="Attachment" DataField="" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />--%>
                            <asp:TemplateField HeaderText="Attachment" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgAttach" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkReply" runat="server">Reply</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server">Delete</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOpen" runat="server">Open</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooter" />
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="trashtab" runat="server" CssClass="content" Visible="false">
                    Trash
                    <asp:GridView ID="grdTrash" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                        ShowFooter="false" Width="100%" AllowSorting="True" EnableModelValidation="True">
                        <Columns>
                            <asp:BoundField HeaderText="Date & Subject" DataField="" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Subject" DataField="" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="400px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="From" DataField="" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Message" DataField="" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="500px" ItemStyle-HorizontalAlign="Left" />
                            <%-- <asp:BoundField HeaderText="Attachment" DataField="" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />--%>
                            <asp:TemplateField HeaderText="Attachment" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgAttach" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkReply" runat="server">Reply</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server">Delete</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOpen" runat="server">Open</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooter" />
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
    </asp:Panel>
    <!---------------mailComposePopup------------------------->
    <cc1:ModalPopupExtender ID="popMail" runat="server" BehaviorID="mpeMail" TargetControlID="hdnMail"
        PopupControlID="pnlMails" CancelControlID="imgClose4">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnMail" runat="server" />
    <asp:Panel ID="pnlMails" runat="server" CssClass="Popup" Style="width: 600px; height: auto;
        display: none;">
        <%-- display: none; --%>
        <asp:Image ID="imgClose4" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closeMailPopup();" ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;<asp:Label ID="lblheading" runat="server" Text="Compose Message"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>
            </table>
            <table width="95%">
                <tr>
                    <td align="left" class="style72">
                        To
                    </td>
                    <td align="center">
                        :
                    </td>
                    <td align="left" colspan="3">
                        <asp:DropDownList data-placeholder="Select User" ClientIDMode="Static" ID="multi"
                            CssClass="chosen-select" runat="server" Style="width: 400px;" multiple>
                            <%--<asp:ListItem Value="1" Text="Pawan"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Sanket"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Shiddesh"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Manoj"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Rahul"></asp:ListItem>
                            <asp:ListItem Value="6" Text="Rimesh"></asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdnsendto" runat="server" />
                        <asp:HiddenField ID="hdnreplyto" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style72">
                        Subject
                    </td>
                    <td align="center">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtSubject" runat="server" Height="22px" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style72">
                        file
                    </td>
                    <td align="center">
                        :
                    </td>
                    <td align="left" colspan="2">
                        <script language="JavaScript" type="text/javascript">

                            $(document).ready(function () {
                                var maxFileSize = 2097152; // 4MB -> 4 * 1024 * 1024
                                var fileUpload = $('#<%= attachfile.ClientID %>');
                                $("#<%= attachfile.ClientID %>").change(function () {
                                    if (fileUpload.val() == '') {
                                        return false;
                                    }
                                    else {
                                        if (fileUpload[0].files[0].size < maxFileSize) {

                                            var fileuploadval = fileUpload.val();
                                            var checkexe = fileuploadval.substr(fileuploadval.length - 4);
                                            if (checkexe == '.exe') {
                                                alert('Exe file not allowed');
                                                return false;
                                            }
                                            var fileinput = document.getElementById("<%= attachfile.ClientID %>");
                                            var textinput = document.getElementById("<%= txtFile.ClientID %>");
                                            textinput.value = fileinput.value;


                                        } else {
                                            alert('File must be Less than 2MB !')
                                            return false;
                                        }
                                    }

                                });
                            });
                            function HandleBrowseClick() {
                                var fileinput = document.getElementById("<%= attachfile.ClientID %>");
                                fileinput.click();

                            }
                        </script>
                        <asp:FileUpload ID="attachfile" runat="server" Style="display: none" />
                        <asp:TextBox ID="txtFile" runat="server" Height="22px" Width="200px"></asp:TextBox>
                        &nbsp;<input type="button" value="Attach File" class="button" id="fakeBrowse" onclick="HandleBrowseClick();" />
                        <%--<asp:Button ID="btnAttch" runat="server" Text="Attach File" />--%>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style72">
                        Message Type
                    </td>
                    <td align="center">
                        :
                    </td>
                    <td align="left" colspan="2">
                        <asp:DropDownList ID="ddlMessageType" runat="server" Height="22px" Width="200px">
                            <asp:ListItem Value="0" Text="Select Message Type" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Query" Text="Query"></asp:ListItem>
                            <asp:ListItem Value="Complaint" Text="Complaint"></asp:ListItem>
                            <asp:ListItem Value="Suggestion" Text="Suggestion"></asp:ListItem>
                            <asp:ListItem Value="Request" Text="Request"></asp:ListItem>
                            <asp:ListItem Value="Others" Text="Others"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table width="95%">
                <tr>
                    <td align="center" class="style68" colspan="4">
                        <asp:TextBox ID="txtMessageContent" runat="server" Height="204px" TextMode="MultiLine"
                            placeholder="Message Content" Width="489px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="95%">
                <tr id="trErr" runat="server" visible="false">
                    <td align="center" colspan="4">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblLeftCharecter" runat="server" Text="" CssClass="counter-container"></asp:Label>
                    </td>
                    <td colspan="2" style="float: right">
                        <asp:Button ID="btnSend" runat="server" Text="SEND" OnClick="btnSend_Click" />
                        <input id="Button2" class="button" runat="server" type="button" value="CLOSE" style="width: 100px;"
                            onclick="closeMailPopup();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/chosen.jquery.js" type="text/javascript"></script>
    <script src="../JS/MaxLength.min.js" type="text/javascript"></script>
    <script src="../JS/jquery.maxlength.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var config = {
            '.chosen-select': {},
            '.chosen-select-deselect': { allow_single_deselect: true },
            '.chosen-select-no-single': { disable_search_threshold: 10 },
            '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chosen-select-width': { width: "95%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var myLength = $("#<%= txtMessageContent.ClientID %>").val().length;

            myLength = myLength + 1000;

            $("#<%= txtMessageContent.ClientID %>").prop('maxLength', myLength);

            $("#<%= txtMessageContent.ClientID %>").maxlength({
                counterContainer: $(".counter-container")
            });

            setdata($("#<%=hdnsendto.ClientID %>").val());
            var Checkall = null;
            $('.chosen-choices').click(function () {
                if (Checkall == 0) {
                    $('.active-result').addClass('result-selected');
                    $('.active-result').removeClass('active-result');
                }
            });

            $(".chosen-select").change(function () {
                var arr = $(this).val();

                var res;
                if (arr != null) {
                    res = arr.indexOf('selectall');
                }
                if (res > -1) {
                    $('.active-result').addClass('result-selected');
                    $('.active-result').removeClass('active-result');
                    Checkall = 0;
                    setdata('selectall');
                    arr = 'selectall';
                }
                else {
                    Checkall = 1;
                }
                mySelect = $('.chosen-select');
                mySelect.trigger('chosen:updated');
                $("#<%=hdnsendto.ClientID %>").val(arr);
            })
        });


        function setdata(val) {
            if (val != "") {

                var splits = val.split(',');

                mySelect = $('.chosen-select');             //make values unique
                mySelect.val(null);                          //delete current options
                mySelect.val(splits);                                //add new options
                mySelect.trigger('chosen:updated');
            }
        }
    </script>
    <!------MailOpen-------------------->
    <cc1:ModalPopupExtender ID="PopOpenMail" runat="server" BehaviorID="mpePopOpenMail"
        TargetControlID="hdnPopOpenMail" PopupControlID="pnlPopOpenMail" CancelControlID="Image1">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnPopOpenMail" runat="server" />
    <asp:Panel ID="pnlPopOpenMail" runat="server" CssClass="Popup" Style="width: 600px;
        height: auto; display: none;">
        <%-- display: none; --%>
        <asp:Image ID="Image1" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
            margin-top: -15px; margin-right: -15px;" onclick="closeMailpnlPopOpenMail();"
            ImageUrl="~/Images/closebtn.png" />
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp; Message
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>
            </table>
            <table width="95%">
                <tr>
                    <td align="left" class="style73">
                        <asp:Label ID="lblfrmTO" runat="server" Text=""></asp:Label>
                    </td>
                    <td align="center" class="style74">
                        :
                    </td>
                    <td align="left" colspan="3" class="style74">
                        <asp:Literal ID="lblMsgfrom" runat="server"></asp:Literal><br />
                        <asp:Label ID="lblMsgdate" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style72">
                        Subject
                    </td>
                    <td align="center">
                        :
                    </td>
                    <td align="left">
                        <asp:Label ID="lblMsgsubject" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr id="trAttach" runat="server" visible="false">
                    <td align="left" class="style72">
                        Attachment
                    </td>
                    <td align="center">
                        :
                    </td>
                    <td align="left" colspan="2">
                        &nbsp;<asp:Label ID="lblMsgfileName" runat="server" Text=""></asp:Label>
                        <asp:Button ID="btnDownloadAttachmentPop" runat="server" Text="Download" OnClick="btnDownloadAttachmentPop_Click" />
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style72">
                        Message Type
                    </td>
                    <td align="center">
                        :
                    </td>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblMsgType" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="95%">
                <tr>
                    <td align="center" class="style68" colspan="4">
                        <asp:TextBox ID="txtMsgOContent" runat="server" Height="204px" TextMode="MultiLine"
                            placeholder="Message Content" Width="489px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="95%">
                <tr>
                    <td colspan="4" align="right">
                        <asp:Button ID="btnReply" runat="server" Text="Reply" OnClick="btnReply_Click" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <!------MailOpen-------------------->
    <%-- --------------------------------------------------- CONFIRMATION POPUP-------------------------------------------------- --%>
    <cc1:ModalPopupExtender ID="popConfirm" runat="server" BehaviorID="mpeConfirm" TargetControlID="hdnConfirm"
        PopupControlID="pnlConfirm">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnConfirm" runat="server" />
    <asp:Panel ID="pnlConfirm" runat="server" CssClass="Popup" Style="width: 430px; display: none;
        height: 160px;">
        <%-- display: none; --%>
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Confirmation
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
                    <td align="center" colspan="3">
                        <asp:Label ID="Label8" runat="server" Text="Are Sure You Want To Delete The Message ??"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnConfirmDelete" runat="server" CssClass="button" Text="Yes" Width="100px"
                            OnClick="btnConfirmDelete_Click" />
                        &nbsp;&nbsp;
                        <input id="Button8" class="button" runat="server" type="button" value="No" style="width: 100px;"
                            onclick="closeconfirm();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <%-- ---------------------------------------------------MsgBox POPUP-------------------------------------------------- --%>
    <cc1:ModalPopupExtender ID="popMsgBox" runat="server" BehaviorID="mpeMsgBox" TargetControlID="hdnMsgBox"
        PopupControlID="pnlMsgBox">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hdnMsgBox" runat="server" />
    <asp:Panel ID="pnlMsgBox" runat="server" CssClass="Popup" Style="width: 430px; display: none;
        height: 160px;">
        <%-- display: none; --%>
        <center>
            <br />
            <table width="100%">
                <tr>
                    <td align="left" colspan="3" style="color: #094791; font-weight: bold;">
                        &nbsp;&nbsp;&nbsp;Message
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
                    <td align="center" colspan="3">
                        <asp:Label ID="lblmsgbox" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        &nbsp;&nbsp;
                        <input id="Button3" class="button" runat="server" type="button" value="Ok" style="width: 100px;"
                            onclick="closemsgbox();" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
</asp:Content>
