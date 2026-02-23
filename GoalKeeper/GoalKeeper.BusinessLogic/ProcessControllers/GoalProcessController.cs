using GoalKeeper.BusinessLogic.Interface;
using GoalKeeper.Model.DTO;
using GoalKeeper.Model.Entity;
using GoalKeeper.Model.View;
using GoalKeeper.Repository.Interface;

namespace GoalKeeper.BusinessLogic.ProcessControllers
{
    public class GoalProcessController : IGoalProcessController
    {
        private readonly IGoalRepository _goalRepository;

        public GoalProcessController(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public async Task<IEnumerable<GoalDTO>> GetUserGoals(Guid ownerUserUID)
        {
            var goals = await _goalRepository.GetGoalsByOwnerUserUID(ownerUserUID);
            return goals.Select(g => new GoalDTO
            {
                UID = g.UID,
                Title = g.Title,
                Description = g.Description,
                Status = g.Status,
                Priority = g.Priority,
                OwnerUserUID = g.OwnerUserUID,
                DueDate = g.DueDate,
                CreatedAt = g.CreatedAt,
                UpdatedAt = g.UpdatedAt
            });
        }

        public async Task<GoalDTO> AddOrUpdateGoal(AddOrUpdateGoalRequest request, Guid ownerUserUID)
        {           
            var goal = new Goal
            {
                UID = request.GoalUID ?? Guid.Empty,
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                OwnerUserUID = ownerUserUID,
                DueDate = request.DueDate,  
                CreatedAt = request.CreatedAt
            };

            var savedGoal = await _goalRepository.AddOrUpdateGoal(goal);

            return new GoalDTO
            {
                UID = savedGoal.UID,
                Title = savedGoal.Title,
                Description = savedGoal.Description,
                Status = savedGoal.Status,
                Priority = savedGoal.Priority,
                OwnerUserUID = savedGoal.OwnerUserUID,
                DueDate = savedGoal.DueDate,
                CreatedAt = savedGoal.CreatedAt,
                UpdatedAt = savedGoal.UpdatedAt
            };
        }

        public async Task DeleteGoal(Guid goalUID, Guid ownerUserUID)
        {            
            var existingGoals = await _goalRepository.GetGoalsByOwnerUserUID(ownerUserUID);
            if (!existingGoals.Any(g => g.UID == goalUID))
            {
                throw new KeyNotFoundException("Goal not found or you do not have permission to delete this goal.");
            }
            
            await _goalRepository.DeleteGoal(goalUID);
        }

        public async Task<GoalConfigurationViewModel> GetGoalConfiguration()
        {
            var statuses = await _goalRepository.GetGoalStatuses();
            var priorities = await _goalRepository.GetGoalPriorities();

            return new GoalConfigurationViewModel
            {
                Statuses = statuses.ToList(),
                Priorities = priorities.ToList()
            };
        }
    }
}
