using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper.Model.DTO
{
    public class RegisterRequest
    {        
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }        
    }
}
