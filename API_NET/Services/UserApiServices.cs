using API_NET.DAL;
using API_NET.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API_NET.Services
{
    public class UserApiServices : IUserApiServices
    {
        private string connectionString;
        private readonly ILogger<UserApiServices> log;

        public UserApiServices(DBAccess dbAccess, ILogger<UserApiServices> l)
        {
            connectionString = dbAccess.GetConnectionString;
            this.log = l;
        }

        private SqlConnection Conexion()
        {
            return new SqlConnection(connectionString);
        }

        public UserApi UserSeek(LoginApi loginApi)
        {
            SqlConnection cnn = Conexion();
            SqlCommand command = null;
            UserApi user = null;
            try
            {
                cnn.Open();
                command = cnn.CreateCommand();
                command.CommandText = "usp_Users_Seek";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserApi", loginApi.UserApi);
                command.Parameters.AddWithValue("@PassApi", loginApi.PassApi);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user = new UserApi
                    {
                        User = reader["UserAPi"].ToString(),
                        EmailUser = reader["UserEmail"].ToString()
                    };
                }

                return user;
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message.ToString());
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                command.Dispose();
                cnn.Close();
                cnn.Dispose();
            }
        }
    }
}
