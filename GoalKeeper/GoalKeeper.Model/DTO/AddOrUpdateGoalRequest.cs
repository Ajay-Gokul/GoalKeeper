using System;
using System.ComponentModel.DataAnnotations;

namespace GoalKeeper.Model.DTO
{
    public class AddOrUpdateGoalRequest
    {
        public Guid? GoalUID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
