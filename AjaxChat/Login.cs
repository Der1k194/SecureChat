using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace SecureChat
{
    public class Login
    {
        private string connectionString;
        public Login()
        {
            
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }

        /// <summary>
        /// User login
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="SessionID"></param>
        /// <param name="UserIPAddress"></param>
        /// <returns>treu if login successful , false if invalid</returns>
        public bool VerifyLogin(string UserName, string Password, string SessionID, string UserIPAddress)
        {
            Users user = new Users();
            if (user.ValidateUser(UserName, Password))
            {
                string LoginDateTime = DateTime.Now.ToString();
                SqlConnection sqlconn = new SqlConnection(connectionString);
                sqlconn.Open();
                SqlParameter userParam = new SqlParameter("@username", UserName);
                SqlParameter sessionidParam = new SqlParameter("@sessionid", SessionID);
                SqlParameter useripaddressParam = new SqlParameter("@useripaddress", UserIPAddress);
                SqlParameter logindateParam = new SqlParameter("@logindate", LoginDateTime);
                SqlParameter lastmessagetimeParam = new SqlParameter("@lastmessagetime", LoginDateTime);
                logindateParam.DbType = DbType.DateTime;
                lastmessagetimeParam.DbType = DbType.DateTime;
                SqlCommand queryLogin = new SqlCommand("update Login set sessionid=@sessionid,logindate=@logindate,lastmessagetime=@lastmessagetime,useripaddress=@useripaddress,loggedin='true' where username=@username");
                
                queryLogin.Connection = sqlconn;
                queryLogin.Parameters.Add(userParam);
                queryLogin.Parameters.Add(sessionidParam);
                queryLogin.Parameters.Add(useripaddressParam);
                queryLogin.Parameters.Add(logindateParam);
                queryLogin.Parameters.Add(lastmessagetimeParam);
                try
                {
                    queryLogin.ExecuteNonQuery();
                    sqlconn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    sqlconn.Close();
                    Debug.WriteLine("Failed Method VerifyLogin : " + e.ToString());
                    return false;
                }
               
            }
            else
            {
                return false;  
            }

        }

        /// <summary>
        /// Checks if user is logged in.
        /// </summary>
        /// <param name="SessionID"></param>
        /// <param name="UserIPAddress"></param>
        /// <returns>username if logged in else returns null</returns>
        public string IsUserLoggedIn(string SessionID, string UserIPAddress)
        {
            SqlConnection sqlconn = new SqlConnection(connectionString);
            sqlconn.Open();
            SqlParameter sessionidParam = new SqlParameter("@sessionid", SessionID);
            SqlParameter useripaddressParam = new SqlParameter("@useripaddress", UserIPAddress);
            SqlCommand queryChekLogin = new SqlCommand("select username from Login where sessionid=@sessionid AND useripaddress=@useripaddress AND loggedin='true'");

            queryChekLogin.Connection = sqlconn;
            queryChekLogin.Parameters.Add(sessionidParam);
            queryChekLogin.Parameters.Add(useripaddressParam);
            
            try
            {
                object username = queryChekLogin.ExecuteScalar();
                if (username != null)
                {
                    sqlconn.Close();
                    return (username.ToString());
                }
                else
                {
                    sqlconn.Close();
                    return (null);
                }
                
            }
            catch (Exception e)
            {
                sqlconn.Close();
                Debug.WriteLine("Failed Method VerifyLogin : " + e.ToString());
                return (null);
            }
        }

        /// <summary>
        /// Logout operation
        /// </summary>
        /// <param name="SessionID"></param>
        /// <param name="UserIPAddress"></param>
        /// <returns>true if logout successful else false</returns>
        public bool Logout(string SessionID, string UserIPAddress)
        {
            SqlConnection sqlconn = new SqlConnection(connectionString);
            sqlconn.Open();
            SqlParameter sessionidParam = new SqlParameter("@sessionid", SessionID);
            SqlParameter useripaddressParam = new SqlParameter("@useripaddress", UserIPAddress);
            SqlCommand querylogout = new SqlCommand("update Login set loggedin='false' where sessionid=@sessionid AND useripaddress=@useripaddress ");

            querylogout.Connection = sqlconn;
            querylogout.Parameters.Add(sessionidParam);
            querylogout.Parameters.Add(useripaddressParam);

            try
            {
                querylogout.ExecuteNonQuery();
                sqlconn.Close();
                if(IsUserLoggedIn(SessionID, UserIPAddress)!= null)
                {
                    return false;
                }
                else {
                    return true;
                }
                
            }
            catch (Exception e)
            {
                sqlconn.Close();
                Debug.WriteLine("Failed Method Logout : " + e.ToString());
                return false;
            }
        }

        
        /// <summary>
        /// Logout if session expires
        /// </summary>
        /// <param name="SessionID">session id of the user</param>
        public void LogoutOnSessionEnd(string SessionID)
        {
            SqlConnection sqlconn = new SqlConnection(connectionString);
            sqlconn.Open();
            SqlParameter sessionidParam = new SqlParameter("@sessionid", SessionID);
            SqlCommand querylogout = new SqlCommand("update Login set loggedin='false' where sessionid=@sessionid ");

            querylogout.Connection = sqlconn;
            querylogout.Parameters.Add(sessionidParam);
           
            try
            {
                querylogout.ExecuteNonQuery();
                sqlconn.Close();
                
            }
            catch (Exception e)
            {
                sqlconn.Close();
                Debug.WriteLine("Failed Method Logout : " + e.ToString());
                
            }
        }



        /// <summary>
        /// Gets the pipeseperated list of online users
        /// </summary>
        /// <returns>pipeseperated list of online users or null</returns>
        public string GetOnlineUsersList()
        {
            string UserList = "";
            SqlConnection sqlconn = new SqlConnection(connectionString);
            sqlconn.Open();
            SqlCommand queryChekLogin = new SqlCommand("select username from Login where loggedin='true'");

            queryChekLogin.Connection = sqlconn;

            try
            {

                SqlDataReader reader = queryChekLogin.ExecuteReader();
                while (reader.Read())
                {
                    UserList = reader[0] + "|" + UserList;
                }
                return (UserList);
            }
            catch (Exception e)
            {
                sqlconn.Close();
                Debug.WriteLine("Failed Method VerifyLogin : " + e.ToString());
                return (null);
            }
        }
    }
}
