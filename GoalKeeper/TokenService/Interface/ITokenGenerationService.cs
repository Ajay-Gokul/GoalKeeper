using Model.Entity;

namespace TokenService.Interface
{
    public interface ITokenGenerationService
    {
        public Task <string> GenerateToken(User user);
    }
}
