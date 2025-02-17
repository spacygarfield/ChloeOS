using Ardalis.Result;
using ChloeOS.Core.Models.OS;

namespace ChloeOS.Core.Contracts.DataAccess.OS;

public interface IUserRepository {

    Task<Result<User[]>> GetAllAsync();
    Task<Result<User>> GetByUsernameAsync(string username);
    Task<Result<User>> CreateAsync(User user);
    Task<Result<User>> UpdateAsync(User user);

}