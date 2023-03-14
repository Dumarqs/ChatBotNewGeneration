using Domain.Core.SqlServer;
using Domain.Dtos;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUserFiltered(Filter filter);
        Task AddUser(UserDto roomDto);
        Task<UserDto> GetUser(Guid id);
        Task UpdateUser(UserDto userDto);
    }
}
