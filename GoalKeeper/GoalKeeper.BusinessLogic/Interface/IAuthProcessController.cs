using GoalKeeper.Model.DTO;
using Model.DTO;

namespace GoalKeeper.BusinessLogic.Interface
{
    public interface IAuthProcessController<T>
    {
        public Task<T> Login(string mail, string password);
        public Task Register(RegisterRequest registerRequest);
    }
}
