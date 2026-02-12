using System.Data;

namespace GoalKeeper.Repository.Interface
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
