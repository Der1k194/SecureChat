var UserLoggedIn = false;
var MessagePollingInterval = 3000 ;  // Interval of polling for message
var OLUsersPollingInterval = 9000;  // Interval of polling for online users
var CurrentReciepent = "";
var CurrentUser;
var MessagePollingTimer, OLUsersPollingTimer;
var EncryptionKey = 3;  //Encryption Key: 0 to disable encryption


function ValidateRegisterWindow() {
    var field1, field2;
    field1 = document.getElementById('NewUserName').value;
    field2 = document.getElementById('NewPassword').value;
    if (field1 !== "" && field2 !== "") {
        if ((field1.indexOf("|")) == -1) {
            return (true);
        }
        else {
            ShowErrorBox("Validation Error", "The Username cannot contain  | character");
            return (false);
        }
    }
    else {
        ShowErrorBox("Validation Error", "The Username or Password cannot be empty");
        return (false);
    }
}

function ClearRegisterWindow() {
    document.getElementById('NewUserName').value = "";
    document.getElementById('NewPassword').value = "";
}

function Register() {

    if (ValidateRegisterWindow()) {

        var url = "SecureChatServer.ashx";
        if (url == null) { alert("Request URL is Empty"); }
        else {
            username = Encrypt(document.getElementById('NewUserName').value, EncryptionKey);
            password = Encrypt(document.getElementById('NewPassword').value, EncryptionKey);
            AjaxRequest(ProcessRegisterResponse, url, "POST", '', '', { RequestCode: 'SC006', RUN: username, RUP: password });
        }    
    }
    
}


function ProcessRegisterResponse() {

    var ResponseStatus = GetHeader(ResponseHeaderJSON, 'ResponseStatus');
    if (ResponseStatus == "RS-OK") {

        ShowAlertMessage("Registration Sucessfull", "", "User Registered and Logged in Sucessfully");
        CurrentUser = document.getElementById('NewUserName').value;
        LoginEvents(CurrentUser,true);
        ClearRegisterWindow();
        

    }
    else if (ResponseStatus == "RS-Failed") {

    ShowErrorBox("Registration Error", "This username cannot be registered ,please try a different username");

    }
    else {

        ShowErrorBox("Unknown Error :Code-01 " + ResponseStatus, "Request cannot be processed ,please try again.");

    }
}





function ValidateLoginWindow() {
    var field1, field2;
    field1 = document.getElementById('UserName').value;
    field2 = document.getElementById('Password').value;
    if (field1 != "" && field2 != "") {
        return (true);
    }
    else {
        ShowErrorBox("Validation Error", "The Username or Password cannot be empty");
        return (false);
    }
}

function ClearLoginWindow() {
    document.getElementById('UserName').value = "";
    document.getElementById('Password').value = "";
}

function Login() {

    if (ValidateLoginWindow()) {

        var URL = "SecureChatServer.ashx";
        if (URL == null) { alert("Request URL is Empty"); }
        else {
            username = Encrypt(document.getElementById('UserName').value, EncryptionKey);
            password = Encrypt(document.getElementById('Password').value, EncryptionKey);
            AjaxRequest(ProcessLoginResponse, URL, "POST", '', '', { RequestCode: 'SC001', RUN: username, RUP: password });
        }
    }
}


function ProcessLoginResponse() {
    var ResponseStatus = GetHeader(ResponseHeaderJSON, 'ResponseStatus');
    if (ResponseStatus == "RS-OK") {

        // ShowAlertMessage("Login Sucess","", "Logged in Sucessfully");  // moved to login events
        LoginEvents(document.getElementById('UserName').value, false);
        ClearLoginWindow();
    }
    else if (ResponseStatus == "RS-Failed") {

        ShowErrorBox("Invalid Login", "Username or Password invalid");

    }
    else {

        ShowErrorBox("Unknoun Error: Code-02 " + ResponseStatus, "Request cannot be processed ,please try again.");

    }
}



function ClearLogoutWindow() {
    document.getElementById('Sender').value = "";
}

function Logout() {

    var URL = "SecureChatServer.ashx";
    if (URL == null) { alert("Request URL is Empty"); }
    else {
        AjaxRequest(ProcessLogoutResponse, URL, "POST", '', '', { RequestCode: 'SC002'});
    }
}


function ProcessLogoutResponse(ResponseText, ResponseStatus) {
    var ResponseStatus = GetHeader(ResponseHeaderJSON, 'ResponseStatus');
    if (ResponseStatus == "RS-OK") {

       // ShowAlertMessage("Logout Sucess","", "Logged out Sucessfully");  //moved to logout events
        LogoutEvents();

    }
    else if (ResponseStatus == "RS-Failed") {

        ShowErrorBox("Logout Failed", "Please Try Again");

    }
    else {

        ShowErrorBox("Unknown Error Code-03 ", "Request cannot be processed ,please try again.");

    }
}



function ValidateSendMessageWindow() {
    var field1, field2;
    field1 = document.getElementById('Message').value;
    field2 = document.getElementById('Recepient').value;
    if (field1 != "" && field2 != "") {
        return (true);
    }
    else {
        ShowErrorBox("Validation Error", "The Message or Recepient cannot be empty");
        return (false);
    }
}


function ClearSendMessageWindow() {
    document.getElementById('Message').value = "";
    document.getElementById('Recepient').value = "";
    CurrentReciepent = "";
    MessageHistoryContainer = document.getElementById("ConversationMessages");
    while (MessageHistoryContainer.hasChildNodes()) {
        MessageHistoryContainer.removeChild(MessageHistoryContainer.firstChild);
    }
}
function SendMessage() {

    if (ValidateSendMessageWindow()) {
        var URL = "SecureChatServer.ashx";
        var covert = "False";
        if (URL == null) { alert("Request URL is Empty"); }
        else {
            HTMLmessage = document.getElementById('Message').value.toString().replace(/\r\n?/g, '<br/>');
            message = Encrypt(HTMLmessage, EncryptionKey);
            recepient = Encrypt(document.getElementById('Recepient').value, EncryptionKey);
            AjaxRequest(ProcessSendMessageResponse, URL, "POST", {Message:message , Recepient:recepient}, '', { RequestCode: 'SC005'});
            CurrentReciepent = document.getElementById('Recepient').value;
            AppendMessageToMessageWindow("You", HTMLmessage);
            document.getElementById('Message').value = "";
        }
    }
}


function ProcessSendMessageResponse() {
    var ResponseStatus = GetHeader(ResponseHeaderJSON, 'ResponseStatus');
    if (ResponseStatus == "RS-OK") {

       
       //ShowAlertMessage("Message Sent", "", "Message Sent Sucessfully");
       // AppendInfoToMessageWindow("Message Sent Sucessfully");

    }
    else if (ResponseStatus == "RS-LoggedOut") {

        ShowErrorBox("Error", "You are not Logged in, Please Login to Send messages");
        LogoutEvents();

    }
    else if (ResponseStatus == "RS-Failed") {

    //ShowErrorBox("Error", "Message could not be sent ,please try again.");
    AppendInfoToMessageWindow("Message could not be sent ,please try again.");
    }
    
    else {

        ShowErrorBox("Unknown Error Code-04 ", "Request cannot be processed ,please try again.");
        
    }
}


function ClearMessageHistoryWindowWindow() {
    MessageHistoryDiv = document.getElementById("MessageHistory");
    MessageHistoryDiv.innerHTML = "";
}


function RecieveMessage() {

    var URL = "SecureChatServer.ashx";
    if (URL == null) { alert("Request URL is Empty"); }
    else {
            AjaxRequest(ProcessRecieveMessageResponse, URL, "POST",'', '', { RequestCode: 'SC003'});
            PollForMessages();
    }
}


function ProcessRecieveMessageResponse() {
    var ResponseStatus = GetHeader(ResponseHeaderJSON, 'ResponseStatus');
    var ResponseSender = GetHeader(ResponseHeaderJSON, 'Sender');
    if (ResponseStatus == "RS-OK") {
        Message = Decrypt(ResponseText, EncryptionKey);
        Sender = Decrypt(ResponseSender, EncryptionKey);
        AppendMessageToMessageWindow(Sender, Message);

    }
    else if (ResponseStatus == "RS-LoggedOut") {

    ShowErrorBox("Error", "You are not Logged in, Please Login to Recieve messages");
    LogoutEvents();

    }
    else if (ResponseStatus == "RS-Failed") {

       // Do nothing
    
    }
    else {

        ShowErrorBox("Unknown Error Code-05 ", "Request cannot be processed ,please try again.");

    }
}



function GetOnlineUsers() {

    var URL = "SecureChatServer.ashx";
    if (URL == null) { alert("Request URL is Empty"); }
    else {
      
      AjaxRequest(ProcessGetOnlineUsersResponse, URL, "POST",'', '', { RequestCode: 'SC004'});
            
        PollForOnlineUsers();
    }
   
    
}


function ProcessGetOnlineUsersResponse() {
var ResponseStatus = GetHeader(ResponseHeaderJSON, 'ResponseStatus');
if (ResponseStatus == "RS-OK" && ResponseText != "") {

        PopulateOnlineUserList(ResponseText);

    }
    else if (ResponseStatus == "RS-Failed") {

        ShowErrorBox("Logout Failed", "Please Try Again");

    }
    else {

        ShowErrorBox("Unknown Error Code-06 ", "Request cannot be processed ,please try again.");

    }
}



function CheckCurrentSession() {

    var URL = "SecureChatServer.ashx";
    if (URL == null) { alert("Request URL is Empty"); }
    else {
            AjaxRequest(CheckCurrentSessionResponse, URL, "POST",'', '', { RequestCode: 'SC007'});
         }
}


function CheckCurrentSessionResponse(ResponseText, ResponseStatus, ResponseUserName) {
var ResponseStatus = GetHeader(ResponseHeaderJSON, 'ResponseStatus');
var ResponseUserName = GetHeader(ResponseHeaderJSON, 'RUN');
    if (ResponseStatus == "RS-OK") {

        LoginEvents(ResponseUserName,true);

    }
    else if (ResponseStatus == "RS-Failed") {

    // continue with a new session
    ShowWindow('LoginWindow', true);

    }
    else {

        ShowErrorBox("Unknown Error Code-07 ", "Request cannot be processed ,please try reloading page again.");

    }
}


function SendMessageToSelecetdUser() {
    var OnlineUsersDisplay = document.getElementById("OnlineUsersListDisplay");
    if (OnlineUsersDisplay != null) {
        var SelectedRecepientIndex = OnlineUsersDisplay.selectedIndex;
        if(SelectedRecepientIndex != null) {
            ClearSendMessageWindow();
            var RecepientUsername = OnlineUsersDisplay.options[SelectedRecepientIndex].value;
            //var RecepientUsername = OnlineUsersDisplay.options[SelectedRecepientIndex].text;  //can also use this to support a different Display name, but not needed now as both values are same
            if (RecepientUsername != "") {
                document.getElementById('Recepient').value = RecepientUsername;
                ShowWindow('SendMessageWindow', true);
            }
        }
    }
    
}


function PopulateOnlineUserList(UserList) {
    var x;
    var OnlineUsersArrayList = new Array();
    var OnlineUsersDisplay = document.getElementById("OnlineUsersListDisplay");
    OnlineUsersDisplay.innerHTML = "";
    //OnlineUsersDisplay.style.visiblity = "none";
     OnlineUsersArrayList = UserList.toString().split("|");
    var usernode;
    var uservalue;
    for (x in OnlineUsersArrayList) {
        if (OnlineUsersArrayList[x] != "") {
            usernode = document.createElement("option");
            uservalue = document.createAttribute("value");
            uservalue.value = OnlineUsersArrayList[x];
            usernode.attributes.setNamedItem(uservalue);
            usernode.innerHTML = OnlineUsersArrayList[x];
            OnlineUsersDisplay.appendChild(usernode);
            
        }
    }
    
}


function LoginEvents(UserName,IsRefresh) {

    document.getElementById("Sender").value = UserName;
    UserLoggedIn = true;
    CurrentUser = UserName;
    PollForMessages();
    GetOnlineUsers();

    if (!IsRefresh) {
        ShowAlertMessage("Login Sucess", "", "Logged in Sucessfully");
    }
    
    HideWindow('RegisterUserWindow', true);
    HideElement('ShowRegisterUserWindow', true);
    
    HideWindow('LoginWindow', true);
    HideElement('ShowLoginWindow', true);

    ShowWindow('OnlineUsersWindow', false);
    ShowElement('ShowOnlineUsersWindow', true);


    ShowWindowAt('MessageHistoryWindow', false, "50", "10");
    ShowElement('ShowMessageHistoryWindow', true);

    
    ShowElement('ShowSendMessageWindow', true);
    
    ShowWindowAt('LogoutWindow', false, '10', ($(window).width() - $('#LogoutWindow').outerWidth()));
    ShowElement('ShowLogoutWindow', true);
    
}

function LogoutEvents() {

    UserLoggedIn = false;
    CurrentUser = "";
    document.getElementById("Sender").value = "";
    
    try {
        clearTimeout(MessagePollingTimer);  //Nozel : Stop timer for checking for messages
        clearTimeout(OLUsersPollingTimer);  //Nozel : Stop timer for checking ol users
        HideWindow('RegisterUserWindow', true);
        ShowElement('ShowRegisterUserWindow', true);

        HideWindow('LogoutWindow', true);
        ShowElement('ShowLoginWindow', true);

        HideWindow('MessageHistoryWindow', true, '50px', '10px');
        HideElement('ShowMessageHistoryWindow', true);

        HideWindow('OnlineUsersWindow', true);
        HideElement('ShowOnlineUsersWindow', true);

        HideElement('ShowLogoutWindow', true);
        
        HideWindow('SendMessageWindow', true);
        HideElement('ShowSendMessageWindow', true);

        ClearRegisterWindow();
        ClearLoginWindow();
        ClearLogoutWindow();
        ClearSendMessageWindow();
        ClearMessageHistoryWindowWindow();
        
        ShowAlertMessage("Logout Sucess", "", "Logged out Sucessfully");
    }
    catch (e) {
        AppendInfoToMessageWindow("Not a valid session, Please Login....");
    }
    
    
}

function PollForMessages() {
    if (UserLoggedIn) {
        MessagePollingTimer = setTimeout("RecieveMessage()", MessagePollingInterval);
    }

}
function PollForOnlineUsers() {
    if (UserLoggedIn) {
        OLUsersPollingTimer = setTimeout("GetOnlineUsers()", OLUsersPollingInterval);
    }

}



function AppendMessageToMessageWindow(MessageOwner ,MessageText) {

    if (MessageOwner != "" && MessageText != "") {

        if (MessageOwner == CurrentReciepent || MessageOwner == "You") {

            MessageHistoryContainerDiv = document.getElementById("RecepientConversation");
            MessageHistoryContainer = document.getElementById("ConversationMessages");
            MessageLine = document.createElement("tr");
            MessageOwnerTD = document.createElement("td");
            MessageOwnerTD.style.width='20%';
            MessageTD = document.createElement("td");
            if (MessageOwner == "You") {
                MessageOwnerTD.innerHTML = '<b style="color:#ff6500;font-weight: bold;">' + MessageOwner + '</b> : ';
            }
            else {
                MessageOwnerTD.innerHTML = '<b style="color:#0066cc;font-weight: bold;">' + MessageOwner + '</b> : ';
            }
            MessageTD.innerHTML = MessageText;
            MessageLine.appendChild(MessageOwnerTD);
            MessageLine.appendChild(MessageTD);
            MessageHistoryContainer.appendChild(MessageLine);
            MessageHistoryContainerDiv.scrollTop = MessageHistoryContainerDiv.scrollHeight;
        }
        else {
            MessageHistoryDiv = document.getElementById("MessageHistory");
            MessageLine = document.createElement("div");
            DisplayMessageHTML = "<b>" + MessageOwner + "</b> : " + MessageText;
            MessageLine.innerHTML = DisplayMessageHTML;
            MessageHistoryDiv.appendChild(MessageLine);
        }
    }

}
function AppendInfoToMessageWindow(InfoText) {

    MessageHistoryDiv = document.getElementById("MessageHistory");
    MessageLine = document.createElement("div");
    DisplayMessageHTML = "<center>---" + InfoText + "---</center> ";
    MessageLine.innerHTML = DisplayMessageHTML;
    MessageHistoryDiv.appendChild(MessageLine);
    MessageHistoryDiv.scrollTop = MessageHistoryDiv.scrollHeight;
}

function ToggleCovertMessageBlock() {
var CovertMessagingState = document.getElementById('CovertMessageExists').checked;
if (CovertMessagingState) {
    ShowElement('CovertMessaging', false);
}
else {
    HideElement('CovertMessaging', false);
}
}