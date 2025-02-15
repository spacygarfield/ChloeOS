using ChloeOS.Core.Models.OS;
using Jane;

namespace ChloeOS.Core.Contracts.DataAccess.OS;

public interface IUserRepository {

    Task<IResult<User[]>> GetAllAsync();
    Task<IResult<User>> GetByUsernameAsync(string username);
    Task<IResult<User>> CreateAsync(User user);
    Task<IResult<User>> UpdateAsync(User user);

}