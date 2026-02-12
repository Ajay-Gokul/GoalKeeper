using Model.DTO;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper.Repository.Interface
{
    public interface IUserRepository
    {
        public Task<User> GetUserByMail(string mail);
        public Task RegisterUser(User user);
    }
}
