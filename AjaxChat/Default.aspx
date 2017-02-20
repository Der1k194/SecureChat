<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SecureChat._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<script type="text/javascript" src="MessageCommunicator.js"></script>
<script type="text/javascript" src="js/jquery.min.js"></script>
<script type="text/javascript" src="js/jquery.min.1.4.js"></script>
<script type="text/javascript" src="js/jquery.tools.min.js"></script>

<script type="text/javascript" src="js/jquery.easing.1.3.js"></script>
<script type="text/javascript" src="js/jquery-ui.min.js"></script>
<script type="text/javascript" src="js/GeneralScripts.js"></script>
<script type="text/javascript" src="js/alertbox.js"></script>
<script type="text/javascript" src="js/Window.js"></script>

<link rel="stylesheet" type="text/css" media="all" href="js/jquery-ui.css"/>

<link rel="stylesheet" type="text/css" media="all" href="js/WindowStyles.css"/>
<link rel="stylesheet" type="text/css" media="all" href="js/style.css"/>

    <title> Secure Chat</title>
    <style type="text/css">
        #Recepient
        {
            width: 304px;
        }
    </style>
</head>
<body>
    <a href="#" title="Click here to Login" class="button" id="ShowLoginWindow" onclick="HideWindow('RegisterUserWindow', true);ShowWindow('LoginWindow',true);" style=" display:block; float:left; clear:right;" ><span class="Login">Login</span></a>
    <a href="#" title="Click here open List of online users" class="button" id="ShowOnlineUsersWindow" onclick="ShowWindow('OnlineUsersWindow',true)" style=" display:none;float:left; clear:right;" ><span class="OnlineUsers">Online Users</span></a>
    <a href="#" title="Click here if you are a new user" class="button" id="ShowRegisterUserWindow" onclick="HideWindow('LoginWindow', true);ShowWindow('RegisterUserWindow',true);" style=" display:block;float:left; clear:right;" ><span class="RegisterUser">Register</span></a>
    <a href="#" title="Click here to open logout window , then click logout button to Logout" class="button" id="ShowLogoutWindow" onclick="ShowWindow('LogoutWindow',true)" style=" display:none;float:left; clear:right;" ><span class="Logout">Logout</span></a>
    <a href="#" title="Click here to open Send message window" class="button" id="ShowSendMessageWindow" onclick="ShowWindow('SendMessageWindow',true)" style=" display:none;float:left; clear:right;" ><span class="SendMessage">Send Message</span></a>
    <a href="#" title="Click here to open Message history window " class="button" id="ShowMessageHistoryWindow" onclick="ShowWindow('MessageHistoryWindow',true)" style=" display:none;float:left; clear:none;" ><span class="MessageHistory">Message History</span></a>

    <form id="form1" runat="server" style = " height:100%;">
    
      
    <div id="OnlineUsersWindow" style="height:auto;width:230px; position:absolute;display:none;" >
        <div id="OnlineUsersWindowTitleBorderLeft" class="WindowTitleBorderLeft">
        <div id="OnlineUsersWindowTitleBorderRight" class="WindowTitleBorderRight">
        <div id="OnlineUsersWindowTitle" class="WindowTitle" >Online Users </div>
        </div>
        <a href="#" class="TitleCloseButton" onclick="HideWindow('OnlineUsersWindow',true)" ></a>
        </div>
        <div id="OnlineUsersWindowBorderLeft" class="WindowBorderLeft">
        <div id="OnlineUsersWindowBorderRight" class="WindowBorderRight">
        <div id="OnlineUsersWindowBody" class="WindowBody" >
	<br />
	<input type= "button" title="Click here to Re-load the Online Users List"  id="RefreshList" onclick="GetOnlineUsers()" style=" width:200px;" value="Refresh List Now" />
        <br />  
        <br />  
         <select title="Click on a particular user to send message"  id="OnlineUsersListDisplay" size="20" style=" width:200px; border:solid 0px #ffffff; overflow:auto;" onchange="SendMessageToSelecetdUser()" >
            
         </select>
        </div> 
        </div> 
        </div> 
    </div>
    <div id="RegisterUserWindow" style="height:auto;width:300px;position:absolute;display:none;" >
        <div id="RegisterUserWindowTitleBorderLeft" class="WindowTitleBorderLeft">
        <div id="RegisterUserWindowTitleBorderRight" class="WindowTitleBorderRight">
        <div id="RegisterUserWindowTitle" class="WindowTitle" > Register User </div>
        </div>
        <a href="#" class="TitleCloseButton" onclick="HideWindow('RegisterUserWindow',true)" ></a>
        </div>
        <div id="RegisterUserWindowBorderLeft" class="WindowBorderLeft">
        <div id="RegisterUserWindowBorderRight" class="WindowBorderRight">
        <div id="RegisterUserWindowBody" class="WindowBody" >
        <table>
        <tr>
        <td>User name : </td>
        <td><input type ="text" id="NewUserName" /></td>
        </tr>
        <tr>
        <td>Password : </td>
        <td> <input type="password" id="NewPassword" onkeypress="if(event.keyCode==13) document.getElementById('RegisterUser').click()" /></td>
        </tr>
        <tr >
        <td colspan="2"><input title="Click here to Register" type= "button" id="RegisterUser" onclick="Register()" value="Register"/> </td>
        </tr>
        </table>
        </div> 
        </div> 
        </div> 
    </div>
    <div id="LoginWindow" style="display:none;" >
        
        <div id="LoginWindowTitleBorderLeft" class="WindowTitleBorderLeft">
        <div id="LoginWindowTitleBorderRight" class="WindowTitleBorderRight">
        <div id="LoginWindowTitle" class="WindowTitle" > Login </div>
        </div>
        <a href="#" class="TitleCloseButton" onclick="HideWindow('LoginWindow',true)" ></a>
        </div>
        <div id="LoginWindowBorderLeft" class="WindowBorderLeft">
        <div id="LoginWindowBorderRight" class="WindowBorderRight">
        <div id="LoginWindowBody" class="WindowBody" >
        <table>
        <tr>
        <td>User name : </td>
        <td><input type ="text" id="UserName" /></td>
        </tr>
        <tr>
        <td>Password : </td>
        <td> <input type="password" id="Password" onkeypress="if(event.keyCode==13) document.getElementById('LoginButton').click()" /></td>
        </tr>
        <tr >
        <td colspan="2"><input type= "button" title="Click here to Login" id="LoginButton" onclick="Login()" value="Login"/> </td>
        </tr>
        </table>
        </div> 
        </div>
        <%--</div>--%>
        </div> 

    </div>
   
     <div id="SendMessageWindow" style="height:auto;width:300px;position:absolute;display:none;" >
        <div id="SendMessageWindowTitleBorderLeft" class="WindowTitleBorderLeft">
        <div id="SendMessageWindowTitleBorderRight" class="WindowTitleBorderRight">
        <div id="SendMessageWindowTitle" class="WindowTitle" >Send a Message </div>
        </div>
        <a href="#" class="TitleCloseButton" onclick="ClearSendMessageWindow();HideWindow('SendMessageWindow',true)" ></a>
        </div>
        <div id="SendMessageWindowBorderLeft" class="WindowBorderLeft">
        <div id="SendMessageWindowBorderRight" class="WindowBorderRight">
        <div id="SendMessageWindowBody" class="WindowBody" >
        <div id="RecepientConversation" style="height:250px;width:285px; overflow:auto;" >
        <table id="ConversationMessages">
        <tr>
        <td colspan="2"><center> </center></td>
        </tr>
        </table>
        </div>
        <table >
        <tr>
        <td>Recepient :</td>
        <td><input type ="text" id="Recepient" disabled="disabled" style="width:180px" class="UserNameDisplayField" /></td>
        </tr>
        <tr>
        <td colspan="2">Message : </td>
        </tr>
        <tr>
        <td colspan="2" title="Type in the message here"><textarea id="Message" style="height:80px;width:250px" > </textarea></td>
        </tr>
        <tr>
        <td colspan="2"><input type= "button" id="SendMessageButton" onclick="SendMessage()" title="Click here to Send" style="float:right;height:30px;width:80px" value="Send"/></td>
        </tr>
        </table>
        </div> 
        </div> 
        </div> 
    </div>
    <div id="MessageHistoryWindow" style="height:auto;width:300px;position:absolute;display:none;" >
        <div id="MessageHistoryWindowTitleBorderLeft" class="WindowTitleBorderLeft">
        <div id="MessageHistoryWindowTitleBorderRight" class="WindowTitleBorderRight">
        <div id="MessageHistoryWindowTitle" class="WindowTitle" >Recieved Messages </div>
        </div>
        <a href="#" class="TitleCloseButton" onclick="HideWindow('MessageHistoryWindow',true)" ></a>
        </div>
        <div id="MessageHistoryWindowBorderLeft" class="WindowBorderLeft">
        <div id="MessageHistoryWindowBorderRight" class="WindowBorderRight">
        <div id="MessageHistoryWindowBody" class="WindowBody" >
        <div id="MessageHistory" title="Messages recieved from other users" style="height:250px;width:270px; overflow:auto;"></div>
        </div> 
        </div> 
        </div> 
    </div>
    <div id="LogoutWindow" style="height:auto;width:300px;position:absolute;display:none;" >
        <%--<div id="LogoutWindowResizeContent">--%>
        <div id="LogoutWindowTitleBorderLeft" class="WindowTitleBorderLeft">
        <div id="LogoutWindowTitleBorderRight" class="WindowTitleBorderRight">
        <div id="LogoutWindowTitle" class="WindowTitle" > Logout </div>
        </div>
        <a href="#" class="TitleCloseButton" onclick="HideWindow('LogoutWindow',true)" ></a>
        </div>
        <div id="LogoutWindowBorderLeft" class="WindowBorderLeft">
        <div id="LogoutWindowBorderRight" class="WindowBorderRight">
        <div id="LogoutWindowBody" class="WindowBody" >
        <table>
        <tr>
        <td><b>Welcome</b> </td>
        <td><input type ="text" id="Sender" disabled="disabled" class="UserNameDisplayField" style="width:180px"  /></td>
        </tr>
        <tr>
        <td colspan="2"><input type= "button" id="LogoutButton" title="Click here to Logout" onclick="Logout()" style="float:right;" value="Logout"/> </td>
        </tr>
        </table>
        </div> 
        </div> 
        </div> 
        <%--</div>--%>
    </div>

    </form>

</body>
</html>
