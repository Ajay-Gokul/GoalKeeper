using GoalKeeper.BusinessLogic.Interface;
using GoalKeeper.Model.DTO;
using GoalKeeper.Model.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoalKeeper.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalProcessController _goalProcessController;

        public GoalController(IGoalProcessController goalProcessController)
        {
            _goalProcessController = goalProcessController;
        }

        private Guid GetOwnerUserUID()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var ownerUserUID))
            {                
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            return ownerUserUID;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoalDTO>>> GetUserGoals()
        {
            var ownerUserUID = GetOwnerUserUID();
            var goals = await _goalProcessController.GetUserGoals(ownerUserUID);
            return Ok(goals);
        }

        [HttpPut]
        public async Task<ActionResult<GoalDTO>> AddOrUpdateGoal([FromBody] AddOrUpdateGoalRequest request)
        {
            var ownerUserUID = GetOwnerUserUID();
            var savedGoal = await _goalProcessController.AddOrUpdateGoal(request, ownerUserUID);
            return Ok(savedGoal);
        }

        [HttpDelete("{goalUID}")]
        public async Task<IActionResult> DeleteGoal(Guid goalUID)
        {
            var ownerUserUID = GetOwnerUserUID();
            await _goalProcessController.DeleteGoal(goalUID, ownerUserUID);
            return Ok("Goal deleted successfully.");
        }

        [HttpGet("config")]
        public async Task<ActionResult<GoalConfigurationViewModel>> GetGoalConfiguration()
        {
            var config = await _goalProcessController.GetGoalConfiguration();
            return Ok(config);
        }
    }
}
