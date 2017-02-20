$(document).ready(function() {
    InitializeChatApplictionUI();

});

function InitializeChatApplictionUI() {
    csscody.initialize();
    InitializeWindow('RegisterUserWindow', 'RegisterUserWindowTitle', 'CoolTheme', true, true);
    InitializeWindow('LoginWindow', 'LoginWindowTitle', 'CoolTheme', true, true);
    InitializeWindow('LogoutWindow', 'LogoutWindowTitle', 'CoolTheme', true, true);
    InitializeWindow('SendMessageWindow', 'SendMessageWindowTitle', 'CoolTheme', true, true);
    InitializeWindow('MessageHistoryWindow', 'MessageHistoryWindowTitle', 'CoolTheme', true, true);
    InitializeWindow('OnlineUsersWindow', 'OnlineUsersWindowTitle', 'CoolTheme', true, true);
    $("[title]").tooltip({
                                    // tweak the position
                                    offset: [10, 2],
                                    // use the "slide" effect
                                    effect: 'slide'
                               // add dynamic plugin with optional configuration for bottom edge
                                    }).dynamic({ bottom: { direction: 'down', bounce: true } });
    CheckCurrentSession();
}

function ShowAlertMessage(Title, SubTitle, MessageText) {
    csscody.alert("<H1>" + Title + "</H1><EM>" + SubTitle + "</EM><BR><P>" + MessageText + " </P>");
}

function ShowErrorBox(Title, MessageText) {
    csscody.error("<H1>" + Title + "</H1><EM></EM><BR><P>" + MessageText + " </P>");
}

function ShowConfirmationBox(Title, MessageText) {
    csscody.confirm("<H1>" + Title + "</H1><EM></EM><BR><P>" + MessageText + " </P>");
}



//Nozel: Added to make any html element visible
function ShowElement(ElementID, animate) {

    if (ElementID == null) {
        alert("Invalid Arguments to function : ShowWindow ");
    }
    else {

        ElementSelector = "#" + ElementID;
        if (animate) {
            $(ElementSelector).fadeIn('fast')

        }
        else {
            $(ElementSelector).show('fast');
        }
    }
}

//Nozel: Added to make any html element disappear
function HideElement(ElementID, animate) {

    if (ElementID == null) {
        alert("Invalid Arguments to function : ShowWindow ");
    }
    else {

        ElementSelector = "#" + ElementID;
        if (animate) {
            $(ElementSelector).fadeOut('slow')
        }
        else {
            $(ElementSelector).hide();
        }
    }
}


function Encrypt(PlainText, Key) {
    var to_enc = PlainText.toString().replace(/^\n+/, "").replace(/\n+$/, ""); //.replace(String.fromCharCode(13),"");
 
	var xor_key=Key;
	var the_res="";//the result will be here
	for(i=0;i<to_enc.length;++i)
	{
	    ////the_res += String.fromCharCode((xor_key ^ to_enc.charCodeAt(i)));        //Nozel:  Xor Cipher . But encoded characters are not always allowed in http headers
	    
	    if (to_enc.charCodeAt(i) <= 32) {
	        //Keep c as it is
	        the_res += String.fromCharCode((to_enc.charCodeAt(i)));       //Nozel:   Bypass Invalid characters which are not supported in Http headers
	     }
	    else {

	        the_res += String.fromCharCode((to_enc.charCodeAt(i)) - Key);      //Nozel:   Normal substitution Cipher
	    }
	}
	return(the_res);
}

function Decrypt(CipherText, Key) {
    var to_dec = CipherText;
    var xor_key = Key;
    var PlainText = "";
    for (i = 0; i < to_dec.length; i++) {
      
       ///// PlainText += String.fromCharCode((xor_key ^ to_dec.charCodeAt(i)));   //Nozel:  Xor Cipher . But encoded characters are not always allowed in http headers

        if (to_dec.charCodeAt(i) <= 32) {
            //Keep c as it is
            PlainText += String.fromCharCode((to_dec.charCodeAt(i)));  //Nozel:   Bypass Invalid characters which are not supported in Http headers
        }
        else {

            PlainText += String.fromCharCode((to_dec.charCodeAt(i)) + Key);  //Nozel:   Normal substitution Cipher
        }

    }

    return (PlainText);
}

function AjaxRequest(ReadyHandler, URL, Method, Params, QueryString, HttpHeaders) {
    if (URL == null) { alert("Request URL is Empty"); }
    else {

        if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
            xmlhttp = new XMLHttpRequest();
        }
        else {// code for IE6, IE5
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }

        //An anonymous function is assigned to the event indicator. 
        xmlhttp.onreadystatechange = function() {

            //200 status means ok, otherwise some error code is returned, 404 for example
            //The 4 state means for the response is ready and sent by the server.  
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                ResponseText = xmlhttp.responseText;   //get text data in the response
                ResponseXML = xmlhttp.responseXML; //get xml data in the response
                ResponseHeaderJSON = xmlhttp.getResponseHeader("CustomHeaderJSON");  // Extract Data in http header
                ResponseHeaders = xmlhttp.getAllResponseHeaders();   //Get a string containing all http headers returned by server

                // Make all the results available in tha ReadyHandler via prototyping.
                ReadyHandler.prototype.ResponseText = ResponseText;
                ReadyHandler.prototype.ResponseHeaderJSON = ResponseHeaderJSON;
                ReadyHandler.prototype.ResponseXML = ResponseXML;
                ReadyHandler.prototype.ResponseHeaders = ResponseHeaders;
                // Execute function passed as ReadyHandelr
                ReadyHandler();
            }

        }

        //If querystring is provided Attach it to the url
        if (QueryString != "") {
            var QueryStringData = "";
            for (QueryStringAttribute in QueryString) {
                QueryStringData = QueryStringAttribute + "=" + QueryString[QueryStringAttribute] + "&" + QueryStringData;
            }
            QueryStringData = QueryStringData.substring(0, QueryStringData.lastIndexOf('&'));
            URL = URL + "?" + escape(QueryStringData);      //Here is where the query string ia attached to the request url.
        }

        //POST or GET URL of the script to execute.true for asynchronous (false for synchronous).
        xmlhttp.open(Method, URL, true);
        xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        if (HttpHeaders != "") {
            var HttpHeadersData = "";
            for (HttpHeaderName in HttpHeaders) {
                xmlhttp.setRequestHeader(HttpHeaderName, HttpHeaders[HttpHeaderName]);  // Here the custom headers are added
            }

        }

        //Post data provided then assemble it into single string to be posted to server
        if (Params != "") {
            var ParamsData = "";
            for (ParamName in Params) {
                ParamsData = ParamName + "=" + Params[ParamName] + "&" + ParamsData;
            }
            ParamsData = ParamsData.substring(0, ParamsData.lastIndexOf('&'));
        }
        
        xmlhttp.send(ParamsData); //Send the request with the post data
    }
}

function GetHeader(ResponseHeaderJSON,HeaderName) {

    eval("var CustomHeaders = { " + ResponseHeaderJSON + "};");
    var header;
    var HeaderValue = "";
    if (CustomHeaders != "" && HeaderName!= '') {
        for (header in CustomHeaders) {
            if (header.toString() == HeaderName) {
                HeaderValue = CustomHeaders[header];
                break;
            }
        }
    }
    return (HeaderValue);
}