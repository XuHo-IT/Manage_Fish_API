using Fish_Manage.Models;
using Fish_Manage.Repository.DTO;

namespace Fish_Manage.Repository.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
