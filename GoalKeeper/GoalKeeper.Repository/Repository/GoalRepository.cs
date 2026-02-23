using GoalKeeper.Model.Entity;
using GoalKeeper.Model.View;
using GoalKeeper.Repository.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GoalKeeper.Repository.Repository
{
    public class GoalRepository : IGoalRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GoalRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Goal>> GetGoalsByOwnerUserUID(Guid ownerUserUID)
        {
            var goals = new List<Goal>();
            using (var connection = _connectionFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetGoalsByOwnerUserUID";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@OwnerUserUID", ownerUserUID));

                var sqlConnection = (SqlConnection)connection;
                var sqlCommand = (SqlCommand)command;
                await sqlConnection.OpenAsync();
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        goals.Add(new Goal
                        {
                            UID = reader.GetGuid(reader.GetOrdinal("UID")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            Priority = reader.GetString(reader.GetOrdinal("Priority")),
                            OwnerUserUID = reader.GetGuid(reader.GetOrdinal("OwnerUserUID")),
                            DueDate = reader.IsDBNull(reader.GetOrdinal("DueDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                        });
                    }
                }
            }
            return goals;
        }

        public async Task<Goal> AddOrUpdateGoal(Goal goal)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "AddOrUpdateGoal";
                command.CommandType = CommandType.StoredProcedure;

                
                if (goal.UID == Guid.Empty)
                {
                    goal.UID = Guid.NewGuid();
                    goal.CreatedAt = DateTime.UtcNow;
                }             
                goal.UpdatedAt = DateTime.UtcNow;

                command.Parameters.Add(new SqlParameter("@GoalUID", goal.UID));
                command.Parameters.Add(new SqlParameter("@Title", goal.Title));
                command.Parameters.Add(new SqlParameter("@Description", (object)goal.Description ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@Status", goal.Status));
                command.Parameters.Add(new SqlParameter("@Priority", goal.Priority));
                command.Parameters.Add(new SqlParameter("@OwnerUserUID", goal.OwnerUserUID));
                command.Parameters.Add(new SqlParameter("@DueDate", (object)goal.DueDate ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@CreatedAt", goal.CreatedAt));
                command.Parameters.Add(new SqlParameter("@UpdatedAt", goal.UpdatedAt));

                var sqlConnection = (SqlConnection)connection;
                var sqlCommand = (SqlCommand)command;
                await sqlConnection.OpenAsync();
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Goal
                        {
                            UID = reader.GetGuid(reader.GetOrdinal("UID")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            Priority = reader.GetString(reader.GetOrdinal("Priority")),
                            OwnerUserUID = reader.GetGuid(reader.GetOrdinal("OwnerUserUID")),
                            DueDate = reader.IsDBNull(reader.GetOrdinal("DueDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                        };
                    }
                }
                throw new InvalidOperationException("AddOrUpdateGoal did not return a goal.");
            }
        }

        public async Task DeleteGoal(Guid goalUID)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DeleteGoal";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@GoalUID", goalUID));

                var sqlConnection = (SqlConnection)connection;
                var sqlCommand = (SqlCommand)command;
                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<GoalStatusViewModel>> GetGoalStatuses()
        {
            var statuses = new List<GoalStatusViewModel>();
            using (var connection = _connectionFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetGoalStatuses";
                command.CommandType = CommandType.StoredProcedure;

                var sqlConnection = (SqlConnection)connection;
                var sqlCommand = (SqlCommand)command;
                await sqlConnection.OpenAsync();
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        statuses.Add(new GoalStatusViewModel
                        {
                            UID = reader.GetGuid(reader.GetOrdinal("UID")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        });
                    }
                }
            }
            return statuses;
        }

        public async Task<IEnumerable<GoalPriorityViewModel>> GetGoalPriorities()
        {
            var priorities = new List<GoalPriorityViewModel>();
            using (var connection = _connectionFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetGoalPriorities";
                command.CommandType = CommandType.StoredProcedure;

                var sqlConnection = (SqlConnection)connection;
                var sqlCommand = (SqlCommand)command;
                await sqlConnection.OpenAsync();
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        priorities.Add(new GoalPriorityViewModel
                        {
                            UID = reader.GetGuid(reader.GetOrdinal("UID")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        });
                    }
                }
            }
            return priorities;
        }
    }
}
