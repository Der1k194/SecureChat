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
    public class Messages
    {
        private string connectionString;
        public Messages()
        {
            
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }


        /// <summary>
        /// Puts message in the queue to be delivered
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Recepient"></param>
        /// <param name="Message"></param>
        /// <returns>true if message successfully sent else returns false</returns>
        public bool WriteMessage(string Sender,string Recepient,string Message)
        {
            if (Sender != null && Recepient != null)
            {
                string MessageDateTime = DateTime.Now.ToString();
                SqlConnection sqlconn = new SqlConnection(connectionString);
                sqlconn.Open();
                SqlParameter senderParam = new SqlParameter("@sender", Sender);
                SqlParameter recepient = new SqlParameter("@recepient", Recepient);
                SqlParameter senddateParam = new SqlParameter("@senddate", MessageDateTime);
                SqlParameter messageParam = new SqlParameter("@message", Message);
                senddateParam.DbType = DbType.DateTime;

                SqlCommand queryAddMessage = new SqlCommand("insert into Messages(sender,recepient,senddate,message) values(@sender,@recepient,@senddate,@message)");
                queryAddMessage.Connection = sqlconn;
                queryAddMessage.Parameters.Add(senderParam);
                queryAddMessage.Parameters.Add(recepient);
                queryAddMessage.Parameters.Add(senddateParam);
                queryAddMessage.Parameters.Add(messageParam);
                try
                {
                    queryAddMessage.ExecuteNonQuery();
                    sqlconn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    sqlconn.Close();
                    Debug.WriteLine("Failed Metod WriteMessage : " + e.ToString());
                    return false;
                }
            }
            else
            {
                Debug.WriteLine("Failed Metod WriteMessage sender or recepient null");
                return false;
            }
            

        }
        
        /// <summary>
        /// Checks if there is any message undelivered for specified user
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns>string[2] where string[1]=Message & string[1]= Sender if message present else null</returns>
        public string[] ReadMessage(string UserName)
        {
            if (UserName != null)
            {
                string Message,Sender;
                string[] MessageReadResult = new string[2];
                SqlConnection sqlconn = new SqlConnection(connectionString);
                sqlconn.Open();
                SqlParameter recepientParam = new SqlParameter("@recepient", UserName);
                SqlParameter MessageParam = new SqlParameter("@message","");
                SqlParameter SenderParam = new SqlParameter("@sender", "");
                
                MessageParam.Direction = ParameterDirection.Output;
                MessageParam.DbType = DbType.String;
                MessageParam.Size = 1073741823;
                SenderParam.Direction = ParameterDirection.Output;
                SenderParam.Size = 2147483647;

                SqlCommand queryAddMessage = new SqlCommand("ReadMessage");
                queryAddMessage.CommandType = CommandType.StoredProcedure;

                queryAddMessage.Connection = sqlconn;
                queryAddMessage.Parameters.Add(recepientParam);
                queryAddMessage.Parameters.Add(MessageParam);
                queryAddMessage.Parameters.Add(SenderParam);

                try
                {
                    queryAddMessage.ExecuteNonQuery();
                    sqlconn.Close();
                    Message = queryAddMessage.Parameters["@message"].Value.ToString();
                    Sender = queryAddMessage.Parameters["@sender"].Value.ToString();
                    MessageReadResult[0] = Sender;
                    MessageReadResult[1] = Message;
                    return MessageReadResult;
                }
                catch (Exception e)
                {
                    sqlconn.Close();
                    Debug.WriteLine("Failed Metod WriteMessage : " + e.ToString());
                    return null;
                }
            }
            else
            {
                Debug.WriteLine("Failed Metod ReadMessage sender or recepient null");
                return null;
            }


        }

    }
}
