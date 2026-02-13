using GoalKeeper.Repository.Interface;
using Microsoft.Data.SqlClient;
using Model.DTO;
using Model.Entity;
using System.Data;
using GoalKeeper.Model.Exceptions;

namespace GoalKeeper.Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> GetUserByMail(string mail)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetUserByMail";
                command.CommandType = CommandType.StoredProcedure;

                var mailParam = command.CreateParameter();
                mailParam.ParameterName = "@Mail";
                mailParam.Value = mail;
                command.Parameters.Add(mailParam);

                var sqlConnection = (SqlConnection)connection;
                var sqlCommand = (SqlCommand)command;
                await sqlConnection.OpenAsync();
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new User
                        {
                            UID = reader.GetGuid(reader.GetOrdinal("UID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Mail = reader.GetString(reader.GetOrdinal("Mail")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash"))
                        };
                    }
                }
            }
            return null;
        }

        public async Task RegisterUser(User user)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "RegisterUser";
                    command.CommandType = CommandType.StoredProcedure;        

                    var nameParam = command.CreateParameter();
                    nameParam.ParameterName = "@Name";
                    nameParam.Value = user.Name;
                    command.Parameters.Add(nameParam);

                    var mailParam = command.CreateParameter();
                    mailParam.ParameterName = "@Mail";
                    mailParam.Value = user.Mail;
                    command.Parameters.Add(mailParam);

                    var passwordParam = command.CreateParameter();
                    passwordParam.ParameterName = "@PasswordHash";
                    passwordParam.Value = user.PasswordHash;
                    command.Parameters.Add(passwordParam);

                    var createdAtParam = command.CreateParameter();
                    createdAtParam.ParameterName = "@CreatedAt";
                    createdAtParam.Value = DateTime.UtcNow;
                    command.Parameters.Add(createdAtParam);

                    var sqlConnection = (SqlConnection)connection;
                    var sqlCommand = (SqlCommand)command;
                    await sqlConnection.OpenAsync();
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {                
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    throw new DuplicateUserException("A user with this email already exists.", ex);
                }
                throw;
            }
        }
    }
}
