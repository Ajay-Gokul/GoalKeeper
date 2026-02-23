using System.Collections.Generic;

namespace GoalKeeper.Model.View
{
    public class GoalConfigurationViewModel
    {
        public List<GoalStatusViewModel> Statuses { get; set; }
        public List<GoalPriorityViewModel> Priorities { get; set; }
    }
}
