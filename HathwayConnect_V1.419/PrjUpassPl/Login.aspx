<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PrjUpassPl.Login1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/Usercontrol/GSTDOC.ascx" TagName="usercntrlgst" TagPrefix="uc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/StyleSheet2.css" rel="Stylesheet" type="text/css" />
    <link href="CSS/main.css" rel="stylesheet" type="text/css" />
 <style type="text/css">
     .back
     {
    
  -webkit-background-size: cover;
  -moz-background-size: cover;
   -o-background-size: cover;
    background-size: 130px 630px;
    background-position:center;
        overflow-x: hidden;
    overflow-y: hidden;
  position:fixed;
     }
     
     
     
     .test
{
    -webkit-background-size: cover;
  -moz-background-size: cover;
   -o-background-size: cover;
    background-size: 1367px 630px;
    background-position:center;
    background-attachment: fixed;
    overflow-x: hidden;
    overflow-y: hidden;
   
 
   
} 
     .promotion
 {
background-color: black;
height: 15%;
width: 50%;
position: absolute;
left: 25%;
top: 6.5%;
 }
 .Popup
        {
            display: inline-block;
            background-color: #FFFFFF;
            border: 2px solid #094791; /*box-shadow: 8px 8px 5px #888888;*/
            -moz-box-shadow: 3px 3px 4px #444;
            -webkit-box-shadow: 3px 3px 4px #444;
            box-shadow: 3px 3px 4px #444;
            -ms-filter: "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#444444')";
            filter: progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#444444');
        }
    </style>

   
    <script type="text/javascript" src="JS/2.3.0-crypto-md5.js"></script>
     <script language="javascript" type="text/javascript">
         var hash = "";
         var digest = "";
         var finaldigest = "";
         var hash2 = "";

         function dovalid() {
             debugger;
             if (document.getElementById("txtUsername").value == "") {
                 alert("Please Enter User Name");
                 document.getElementById("txtUsername").focus();
                 return false;

             }
             if (document.getElementById("txtPassword").value == "") {
                 alert("Please Enter Password");
                 document.getElementById("txtPassword").focus();
                 return false;
             }
             HelloWorld();
         }
                  function HelloWorld() {
                      debugger;
                      var result = GetSynchronousJSONResponse('<%= Page.ResolveUrl("Service1.asmx/HelloWorld") %>', '{"Name":"' + document.getElementById('txtUsername').value + '"}');

                      //var result = GetSynchronousJSONResponse('<%= Page.ResolveUrl("~/WebService.asmx/HelloWorld") %>', '{"Name":"' + document.getElementById('xTxtBxUserName').value + '"}');
                      result = eval('(' + result + ')');
                      digest = Crypto.MD5(document.getElementById("txtPassword").value);
                      finaldigest = digest.toUpperCase();
                      hash = finaldigest + result.d;
                      var finalvalue = Crypto.MD5(hash);
                      document.getElementById("txtPassword").value = finalvalue;
                  }


                  function GetSynchronousJSONResponse(url, postData) {
                      var xmlhttp = null;
                      if (window.XMLHttpRequest)
                          xmlhttp = new XMLHttpRequest();
                      else if (window.ActiveXObject) {
                          if (new ActiveXObject("Microsoft.XMLHTTP"))
                              xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
                          else
                              xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
                      }

                      url = url + "?rnd=" + Math.random(); // to be ensure non-cached version

                      xmlhttp.open("POST", url, false);
                      xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                      xmlhttp.send(postData);
                      var responseText = xmlhttp.responseText;
                      return responseText;
                  }
         //var _0xeec9=["","\x76\x61\x6C\x75\x65","\x78\x54\x78\x74\x42\x78\x55\x73\x65\x72\x4E\x61\x6D\x65","\x67\x65\x74\x45\x6C\x65\x6D\x65\x6E\x74\x42\x79\x49\x64","\x50\x6C\x65\x61\x73\x65\x20\x45\x6E\x74\x65\x72\x20\x55\x73\x65\x72\x20\x4E\x61\x6D\x65","\x66\x6F\x63\x75\x73","\x78\x54\x78\x74\x42\x78\x50\x61\x73\x73\x77\x6F\x72\x64","\x50\x6C\x65\x61\x73\x65\x20\x45\x6E\x74\x65\x72\x20\x50\x61\x73\x73\x77\x6F\x72\x64","\x7B\x22\x4E\x61\x6D\x65\x22\x3A\x22","\x22\x7D","\x28","\x29","\x74\x6F\x55\x70\x70\x65\x72\x43\x61\x73\x65","\x64","\x58\x4D\x4C\x48\x74\x74\x70\x52\x65\x71\x75\x65\x73\x74","\x41\x63\x74\x69\x76\x65\x58\x4F\x62\x6A\x65\x63\x74","\x4D\x69\x63\x72\x6F\x73\x6F\x66\x74\x2E\x58\x4D\x4C\x48\x54\x54\x50","\x4D\x73\x78\x6D\x6C\x32\x2E\x58\x4D\x4C\x48\x54\x54\x50","\x3F\x72\x6E\x64\x3D","\x72\x61\x6E\x64\x6F\x6D","\x50\x4F\x53\x54","\x6F\x70\x65\x6E","\x43\x6F\x6E\x74\x65\x6E\x74\x2D\x54\x79\x70\x65","\x61\x70\x70\x6C\x69\x63\x61\x74\x69\x6F\x6E\x2F\x6A\x73\x6F\x6E\x3B\x20\x63\x68\x61\x72\x73\x65\x74\x3D\x75\x74\x66\x2D\x38","\x73\x65\x74\x52\x65\x71\x75\x65\x73\x74\x48\x65\x61\x64\x65\x72","\x73\x65\x6E\x64","\x72\x65\x73\x70\x6F\x6E\x73\x65\x54\x65\x78\x74"];var hash=_0xeec9[0];var digest=_0xeec9[0];var finaldigest=_0xeec9[0];var hash2=_0xeec9[0];function dovalid(){if(document[_0xeec9[3]](_0xeec9[2])[_0xeec9[1]]==_0xeec9[0]){alert(_0xeec9[4]);document[_0xeec9[3]](_0xeec9[2])[_0xeec9[5]]();return false;} ;if(document[_0xeec9[3]](_0xeec9[6])[_0xeec9[1]]==_0xeec9[0]){alert(_0xeec9[7]);document[_0xeec9[3]](_0xeec9[6])[_0xeec9[5]]();return false;} ;HelloWorld();} ;function HelloWorld(){var _0xd0e7x7=GetSynchronousJSONResponse('<%= Page.ResolveUrl("~/WebService.asmx/HelloWorld") %>',_0xeec9[8]+document[_0xeec9[3]](_0xeec9[2])[_0xeec9[1]]+_0xeec9[9]);_0xd0e7x7=eval(_0xeec9[10]+_0xd0e7x7+_0xeec9[11]);digest=Crypto.MD5(document[_0xeec9[3]](_0xeec9[6])[_0xeec9[1]]);finaldigest=digest[_0xeec9[12]]();hash=finaldigest+_0xd0e7x7[_0xeec9[13]];var _0xd0e7x8=Crypto.MD5(hash);document[_0xeec9[3]](_0xeec9[6])[_0xeec9[1]]=_0xd0e7x8;} ;function GetSynchronousJSONResponse(_0xd0e7xa,_0xd0e7xb){var _0xd0e7xc=null;if(window[_0xeec9[14]]){_0xd0e7xc= new XMLHttpRequest();} else {if(window[_0xeec9[15]]){if( new ActiveXObject(_0xeec9[16])){_0xd0e7xc= new ActiveXObject(_0xeec9[16]);} else {_0xd0e7xc= new ActiveXObject(_0xeec9[17]);} ;} ;} ;_0xd0e7xa=_0xd0e7xa+_0xeec9[18]+Math[_0xeec9[19]]();_0xd0e7xc[_0xeec9[21]](_0xeec9[20],_0xd0e7xa,false);_0xd0e7xc[_0xeec9[24]](_0xeec9[22],_0xeec9[23]);_0xd0e7xc[_0xeec9[25]](_0xd0e7xb);var _0xd0e7xd=_0xd0e7xc[_0xeec9[26]];return _0xd0e7xd;} ;
     </script>
     <script type="text/javascript">
        function closeMsgPopup() {
            $find("mpeMsg").hide();
            return false;
        }
        </script>

</head>
<%--<body scroll="no" style="background-image:url('start_img/HOME_a.png');background-size: 10% 10%; background-size: 150px; background-size: cover; height:"10%"; width:"100%";margin:-10; o">--%>
<body class="back">
<div class="promotion">
<img src="Images/bannerlogin.jpg" style="height:100%;width:100%"/>
 </div>
    <form id="form1" runat="server" style="height:770px;">
     <asp:ScriptManager ID="script1" runat="server">
    </asp:ScriptManager>
   <div ><asp:Image  ID="Image7" ImageUrl="~/start_img/HOME_a.png" runat="server" width="1350px" Height="650px"  /> </div>

 <%--    <div id="outer" style="width:100%;height:2- overflow-x:hidden; overflow:hidden">  
        <div id="inner" style="display: table;margin: 0 auto;width:1300px">
            
            <asp:Image  ID="Image7" ImageUrl="~/start_img/HOME_a.png" runat="server" width="1300px"   />
        </div>
    </div>--%>
          <p style="height:200px;font-size:larger;position: fixed;top: 0px;">
    <span style="color: white; font-family: Arial, Sans-Serif, halvatica;height:3px">
            <marquee style="height:200px;font-size:larger">
            <label style="height:200px;font-size:calc()" id ="lblbroadcast" runat="server"></label></marquee>
        </span>
        </p>

			<div id="signinwithyourhathwa"><asp:Image ID="Image1" ImageUrl="~/start_img/signinwithyourhathwa.png" runat="server"/></div>
			<div id="RoundedRectangle3"><asp:Image ID="Image2" ImageUrl="~/start_img/RoundedRectangle3.png" runat="server"/></div>
			<div id="Ellipse1"><asp:Image ID="Image3" ImageUrl="~/start_img/Ellipse1.png" runat="server"/></div>
			<div id="Ellipse1copy"><asp:Image ID="Image4" ImageUrl="~/start_img/Ellipse1copy.png" runat="server"/></div>
			<div id="UserName"><asp:Image ID="Image5" ImageUrl="~/start_img/UserName.png" runat="server"/></div>
			<div id="Layer4">
                <asp:TextBox ID="txtUsername" runat="server" Height="34px" Width="254px" 
                    TabIndex="1" ></asp:TextBox>
                
            </div>
			<div id="Password"><asp:Image ID="Image6" ImageUrl="~/start_img/Password.png" runat="server"/></div>
			<div id="Login">
                <asp:ImageButton ID="ibtLogIn" runat="server"
                    ImageUrl="~/start_img/Login.png"   OnClientClick="return dovalid();"  OnClick="ibtLogIn_Click" TabIndex="3" />
                    <asp:Label ID="lblLoginResult" runat="server"></asp:Label>
                    <asp:HiddenField ID="hdnWebUrl" runat="server" />

            </div>

			<div id="ForgotPassword">
                <asp:ImageButton ID="lnkforgot" runat="server" style="height: 16px;width: 97px;float: right !important;position: absolute;margin-left: 250px;"
                    ImageUrl="~/start_img/ForgotPassword.png"  OnClick="lnkforgot_Click1" TabIndex="4" />
                    <br />
                    <br />
                    <asp:Label ID="lblcomplaint" Width="400px" runat="server" ForeColor="White" Text="Kindly register your complaints on : hathwayconnect@hathway.net or Call on 7666450450 from 8:30 AM to 9:00 PM"></asp:Label> 
            </div>
			<div id="Layer4copy">
                <asp:TextBox ID="txtPassword" runat="server" Height="34px" Width="254px"
                    TextMode="Password" TabIndex="2"></asp:TextBox></div>
                        <div class="Capchta">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                     <asp:Image ID="imgCaptcha" runat="server" />
                <asp:ImageButton ID="imgcatcharefresh" runat="server" ImageUrl="~/IconImage/view_refresh.png" OnClick="imgcatcharefresh_Click"
                style="width: 24px;position: absolute;left: 85px;top: 3px;" ToolTip="Refresh"/>
                <asp:TextBox ID="txtcaptcha" runat="server" Height="23px" Width="133px" TabIndex="3" style="position: absolute;margin-left: 11px;"></asp:TextBox>
                </ContentTemplate>
                </asp:UpdatePanel>
                     
                     </div>

  <cc1:ModalPopupExtender ID="ModalPopupExtender1" Y="220" X="410" runat="server" TargetControlID="Label1"
            PopupControlID="pnlpassword" CancelControlID="imgclose">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="pnlpassword" runat="server" Style="display: inline-block; width: 330px;
            display: none; height: 150px; background-color: #99C2FF; padding-left: 2px; border: thin; box-shadow: 8px 8px 5px #888888;">
            <asp:Image ID="imgclose" ToolTip="Close" runat="server" Style="z-index: -1; float: right;
                margin-top: -15px; margin-right: -10px;" ImageUrl="Images/closebtn.png" />
         
             <table align="center" style="padding-top: 35px;">
                <tr>
                    <td align="right">
                        <asp:Label ID="lbluser" runat="server" Text="UserName"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtname" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr align="center">
                    <td colspan="3" align="center">
                        <br />
                        <asp:Button ID="btnreset" runat="server" Text="Reset" class="button" Width="60" Font-Bold="true"
                            OnClick="btnreset_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
            
                        <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
      
        </asp:Panel>
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                       <!---------------------------cnfmPopup------------------------>
               <cc1:ModalPopupExtender ID="popMsg" runat="server" BehaviorID="mpeMsg" TargetControlID="hdnPop2"
                PopupControlID="pnlMessage">
            </cc1:ModalPopupExtender>
            <asp:HiddenField ID="hdnPop2" runat="server" />
            <asp:Panel ID="pnlMessage" runat="server" CssClass="Popup" Style="width: 700px; height: auto; position: absolute;
                display: none;top: 20px !important;">
                <%-- display: none; --%>
                <center>
                    <br />
               <div style="width: 650px; height: 500px; padding-left: 5px; font-family: Verdana; font-size: small; font-weight: normal; ">
               <div>
                                <uc:usercntrlgst ID="userGST" runat="server" ></uc:usercntrlgst>
                           </div>
                           <div style="position: absolute; bottom: 7px; text-align: center; align-items: center; ">
                           <hr />
                        <asp:Button ID="btnGSTSet" runat="server" Text="I AGREE" class="button" Width="130" Font-Bold="true" OnClick="btnAccept_Click"></asp:Button>

                            
                            <%--<asp:Button ID="btnGSTSet" class="button" runat="server" Text="OK"style="width: 100px;" OnClick="btnGSTSet_Click" />--%>
                                <asp:Button ID="btnReject" runat="server" Text="UPDATE GSTN" class="button" Width="130" Font-Bold="true" OnClick="btnReject_Click"></asp:Button>
</div>
</div>
                </center>
            </asp:Panel>
    </form>
</body>
</html>