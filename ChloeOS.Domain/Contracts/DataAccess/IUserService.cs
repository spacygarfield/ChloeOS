using ChloeOS.Domain.Models;
using Jane;

namespace ChloeOS.Domain.Contracts.DataAccess;

public interface IUserRepository {

    Task<Result<User[]>> GetAllAsync();
    Task<Result<User>> GetByUsernameAsync(string username);
    Task<Result<User>> CreateAsync(User user);
    Task<Result<User>> UpdateAsync(User user);

}