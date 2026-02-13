using GoalKeeper.BusinessLogic.Interface;
using GoalKeeper.Model.DTO;
using GoalKeeper.Repository.Interface;
using Model.DTO;
using Model.Entity;
using TokenService.Interface;

namespace GoalKeeper.BusinessLogic.ProcessControllers
{
    public class AuthProcessController : IAuthProcessController<UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerationService _tokenGenerationService;
        public AuthProcessController(IUserRepository userRepository, ITokenGenerationService tokenGenerationService) 
        {
            _userRepository = userRepository;
            _tokenGenerationService = tokenGenerationService;
        }

        public async Task<UserDTO> Login(string mail, string password) {
            var user = await _userRepository.GetUserByMail(mail);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new ArgumentException("Invalid password");

            var token = await _tokenGenerationService.GenerateToken(user);
            return new UserDTO
            {
                UID = user.UID,
                Name = user.Name,
                Mail = user.Mail,
                Token = token
            };
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            var user = new User
            {                
                Name = registerRequest.Name,
                Mail = registerRequest.Mail,
                PasswordHash = hash
            };
            await _userRepository.RegisterUser(user);
        }
    }
}
