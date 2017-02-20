using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Diagnostics;
using System.Text;

namespace SecureChat
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
   // [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SecureChatServer : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            int EncryptionKey = 3;      //Encryption Key: 0 to disable encryption

            string RequestCode,SessionID, UserIPAddress;
            RequestCode = context.Request.Headers["RequestCode"];
            SessionID = context.Session.SessionID.ToString();
            UserIPAddress = context.Request.UserHostAddress.ToString();
            Debug.WriteLine("SessionID : " + SessionID);
            switch (RequestCode)
            {
                #region Handle Login Request
                
                case "SC001":   //indicates request to login
                    {
                        string UserName, Password;
                        UserName = Decrypt(context.Request.Headers["RUN"],EncryptionKey);
                        Password = Decrypt(context.Request.Headers["RUP"],EncryptionKey);
                        Login userlogin = new Login();

                        try
                        {
                            if (userlogin.VerifyLogin(UserName, Password, SessionID, UserIPAddress))
                            {
                                context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-OK'");
                            }
                            else
                            {
                                context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Failed'");
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Failed Request SC001 : " + e.ToString());
                            context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Exception'");

                        }

                    }
                    break;

                #endregion

                #region Handle Logout Request
                case "SC002":  //indicates request to Logout
                    {

                        Login userlogin = new Login();

                        try
                        {
                            if (userlogin.Logout(SessionID, UserIPAddress))
                            {
                                context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-OK'");
                            }
                            else
                            {
                                context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Failed'");
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Failed Request SC002 : " + e.ToString());
                            context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Exception'");

                        }


                    }
                    break;

                #endregion

                #region  Handle Recieve Message Request
                case "SC003":  //indicates request to poll for incomming messages
                    {

                        string LoggedInUser,Message,Sender;
                        string[] MessageReadResult = new string[2];

                        //Recepient = context.Request.Headers["Recepient"];
                        try
                        {
                            Login userlogin = new Login();
                            LoggedInUser = userlogin.IsUserLoggedIn(SessionID, UserIPAddress);
                            
                            if (LoggedInUser != null)
                            {
                                Messages newMessage = new Messages();
                                MessageReadResult = newMessage.ReadMessage(LoggedInUser);

                                if (MessageReadResult != null && (MessageReadResult[0] != "" || MessageReadResult[1]!= ""))
                                {
                                    Sender = Encrypt(MessageReadResult[0],EncryptionKey);
                                    Message = Encrypt(MessageReadResult[1], EncryptionKey);
                                    context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-OK',Sender:'" + Sender + "'");
                                    context.Response.Write(Message);
                                    
                                }
                                else
                                {
                                    context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Failed'");
                                }
                            }
                            else
                            {
                                context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-LoggedOut'");

                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Failed Request SC003 : " + e.ToString());
                            context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Exception'");

                        }


                    }
                    break;

                #endregion

                #region Handle User List Request
                case "SC004":  //indicates request to get online users list
                    {
                        string UserList,LoggedInUser;
                        try
                        {
                            Login userlogin = new Login();
                            LoggedInUser = userlogin.IsUserLoggedIn(SessionID, UserIPAddress);
                            if (LoggedInUser != null)
                            {
                               UserList = userlogin.GetOnlineUsersList();

                               if (UserList != null || UserList!= "|")
                                {
                                    context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-OK'");
                                    context.Response.Write(UserList);
                                }
                                else
                                {
                                    context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Failed'");
                                }
                            }
                            else
                            {
                                context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-LoggedOut'");

                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Failed Request SC004 : " + e.ToString());
                            context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Exception'");

                        }

                    }
                    break;

                #endregion

                #region Handle Send Message Request
                case "SC005":  //indicates request to send message
                    {
                        string Recepient,Message,LoggedInUser;
                      
                        try
                            {
                                Login userlogin = new Login();
                                LoggedInUser = userlogin.IsUserLoggedIn(SessionID, UserIPAddress);
                                if (LoggedInUser != null)
                                {
                                    Messages newMessage = new Messages();

                                    Message = Decrypt(context.Request.Params["Message"], EncryptionKey);
                                    Recepient = Decrypt(context.Request.Params["Recepient"], EncryptionKey);

                                    if (newMessage.WriteMessage(LoggedInUser, Recepient, Message))
                                    {
                                        context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-OK'");
                                    }
                                    else
                                    {
                                        context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Failed'");
                                    }

                                }
                                else
                                {
                                    context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-LoggedOut'");

                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine("Failed Request SC005 : " + e.ToString());
                                context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Exception'");

                            }

                    }
                    break;

                #endregion

                #region Handle Add New User Request
                case "SC006":  //indicates request to add new user
                    {
                        string UserName, Password;
                        UserName = Decrypt(context.Request.Headers["RUN"],EncryptionKey);
                        Password = Decrypt(context.Request.Headers["RUP"],EncryptionKey);
                        Users newuser = new Users();
                        
                        try
                        {
                            if (newuser.AddUser(UserName, Password, SessionID, UserIPAddress))
                            {
                                context.Response.AddHeader("CustomHeaderJSON","ResponseStatus:'RS-OK'");
                            }
                            else
                            {
                                context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Failed'");
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Failed Request SC006 : " + e.ToString());
                            context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Exception'");

                        }
                    }
                    break;

                #endregion

                #region Handle Check loggedin Session  Request

                case "SC007":
                    {
                        string LoggedInUser;
                        try
                        {

                            Login userlogin = new Login();
                            LoggedInUser = userlogin.IsUserLoggedIn(SessionID, UserIPAddress);
                            if (LoggedInUser != null)
                            {
                                context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-OK',RUN:'" + LoggedInUser + "'");
                            }
                            else
                            {
                                context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Failed'");
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Failed Request SC007 : " + e.ToString());
                            context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Exception'");

                        }
                    }
                    break;
#endregion
                
                
                default:
                    {
                        context.Response.AddHeader("CustomHeaderJSON", "ResponseStatus:'RS-Failed'");
                    }
                    break;
            }
                
            
           // context.Response.Write("OK");
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        public static string Encrypt(string textToEncrypt ,int key)
        {
            StringBuilder inSb = new StringBuilder(textToEncrypt);
            StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
            char c;
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                c = inSb[i];
                //////c = (char)((c ^ key));        //Nozel:  Xor Cipher . But encoded characters are not always allowed in http headers
                if (c <= 32)
                {
                    //Keep c as it is              //Nozel:   Bypass Invalid characters which are not supported in Http headers
                }
                else
                {
                    c = (char)((c - key));           //Nozel:   Normal substitution Cipher
                }
                outSb.Append(c);
            }
            return outSb.ToString();
        }
        public static string Decrypt(string textToEncrypt, int key)
        {
            StringBuilder inSb = new StringBuilder(textToEncrypt);
            StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
            char c;
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                c = inSb[i];
                //////c = (char)((c ^ key));        //Nozel:  Xor Cipher . But encoded characters are not always allowed in http headers
               
                if (c <= 32)
                {
                    //Keep c as it is               //Nozel:   Bypass Invalid characters which are not supported in Http headers
                }
                else
                {
                    c = (char)((c + key));            //Nozel:   Normal substitution Cipher
                }
                outSb.Append(c);
            }
            return outSb.ToString();

           
        }   

    }
}
