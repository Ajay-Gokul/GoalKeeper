using GoalKeeper.Model.DTO;
using GoalKeeper.Model.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoalKeeper.BusinessLogic.Interface
{
    public interface IGoalProcessController
    {
        Task<IEnumerable<GoalDTO>> GetUserGoals(Guid ownerUserUID);
        Task<GoalDTO> AddOrUpdateGoal(AddOrUpdateGoalRequest request, Guid ownerUserUID);
        Task DeleteGoal(Guid goalUID, Guid ownerUserUID);
        Task<GoalConfigurationViewModel> GetGoalConfiguration();
    }
}
