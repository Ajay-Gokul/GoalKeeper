using GoalKeeper.Model.Entity;
using GoalKeeper.Model.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoalKeeper.Repository.Interface
{
    public interface IGoalRepository
    {
        Task<IEnumerable<Goal>> GetGoalsByOwnerUserUID(Guid ownerUserUID);
        Task<Goal> AddOrUpdateGoal(Goal goal);
        Task DeleteGoal(Guid goalUID);
        Task<IEnumerable<GoalStatusViewModel>> GetGoalStatuses();
        Task<IEnumerable<GoalPriorityViewModel>> GetGoalPriorities();
    }
}
