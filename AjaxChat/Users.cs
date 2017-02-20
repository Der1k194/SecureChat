using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Diagnostics;

namespace SecureChat
{
    public class Users
    {
        private string connectionString;
        /// <summary>
        /// Constructor
        /// </summary>
        public Users()
        {
            
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        }

 #region Methods

        /// <summary>
        /// Validate the username and password
        /// </summary>
        /// <param name="UserName">string username</param>
        /// <param name="Password">string password</param>
        /// <returns>true if valid and false if invalid</returns>
        public bool ValidateUser(string UserName, string Password)
        {
            SqlConnection sqlconn = new SqlConnection(connectionString);
            sqlconn.Open();
            SqlParameter userParam = new SqlParameter("@username",UserName);
            SqlParameter passParam = new SqlParameter("@password",Password);

            SqlCommand queryUser = new SqlCommand("Select id from Users where username=@username AND password=@password");
            
            queryUser.Connection = sqlconn;
            queryUser.Parameters.Add(userParam);
            queryUser.Parameters.Add(passParam);
            try
            {
                object verifyResult = queryUser.ExecuteScalar();
            if(verifyResult != null)
            {
                sqlconn.Close();
                return true;
            }
            else 
            {
                sqlconn.Close();
                return false;
            }
            }
            catch (Exception e)
            {

                sqlconn.Close();
                Debug.WriteLine("Failed Method ValidateUser : " + e.ToString());
                return false;
            }
            
        }

        /// <summary>
        /// Adds a new user in database and also sets it as loggeg in
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="SessionID"></param>
        /// <param name="UserIPAddress"></param>
        /// <returns>true if success else false</returns>
        public bool AddUser(string UserName, string Password, string SessionID, string UserIPAddress)
        {
            if (UserName != "" && Password != "")
            {
                string LoginDateTime = DateTime.Now.ToString();
                SqlConnection sqlconn = new SqlConnection(connectionString);
                sqlconn.Open();
                SqlParameter userParam = new SqlParameter("@username", UserName);
                SqlParameter passParam = new SqlParameter("@password", Password);
                SqlParameter sessionidParam = new SqlParameter("@sessionid", SessionID);
                SqlParameter useripaddressParam = new SqlParameter("@useripaddress", UserIPAddress);
                SqlParameter logindateParam = new SqlParameter("@logindate", LoginDateTime);
                logindateParam.DbType = DbType.DateTime;
                SqlCommand queryUser = new SqlCommand("insert into Users(username,password) values(@username,@password)");
                SqlCommand queryLogin = new SqlCommand("insert into Login(username,logindate,sessionid,useripaddress,loggedin) values(@username,@logindate,@sessionid,@useripaddress,'true')");
                
                queryUser.Connection = sqlconn;
                queryLogin.Connection = sqlconn;

                queryUser.Parameters.Add(userParam);
                queryUser.Parameters.Add(passParam);

                queryLogin.Parameters.Add((SqlParameter)((ICloneable)userParam).Clone());
                queryLogin.Parameters.Add((SqlParameter)((ICloneable)sessionidParam).Clone());
                queryLogin.Parameters.Add((SqlParameter)((ICloneable)useripaddressParam).Clone());
                queryLogin.Parameters.Add((SqlParameter)((ICloneable)logindateParam).Clone());
                try
                {
                    queryUser.ExecuteNonQuery();
                    queryLogin.ExecuteNonQuery();
                    sqlconn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    sqlconn.Close();
                    Debug.WriteLine("Failed Method AddUser : " + e.ToString());
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

 
#endregion
    }
}
