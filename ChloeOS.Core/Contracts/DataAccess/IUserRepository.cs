using ChloeOS.Core.Models;
using Jane;

namespace ChloeOS.Core.Contracts.DataAccess;

public interface IUserRepository {

    Task<IResult<User[]>> GetAllAsync();
    Task<IResult<User>> GetByUsernameAsync(string username);
    Task<IResult<User>> CreateAsync(User user);
    Task<IResult<User>> UpdateAsync(User user);

}