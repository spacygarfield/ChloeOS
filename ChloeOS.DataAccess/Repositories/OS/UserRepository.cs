using Ardalis;
using Ardalis.Result;
using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.Core.Models.OS;
using ChloeOS.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChloeOS.DataAccess.Repositories.OS;

public class UserRepository : IUserRepository {

    private readonly OperatingSystemContext _os;

    public UserRepository(OperatingSystemContext os) => _os = os;

    public async Task<Result<User[]>> GetAllAsync() {
        try {
            User[] users = await _os.Users.ToArrayAsync();

            if (users.Length == 0) {
                return Result.NotFound("No local users exist yet. Why not be the first one?");
            }

            return Result.Success(users);
        } catch {
            return Result.Unavailable("Unable to retrieve all local users.");
        }
    }

    public async Task<Result<User>> GetByUsernameAsync(string username) {
        try {
            User? user = await _os.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) {
                return Result.NotFound("No user with that username exist.");
            }

            return Result.Success(user);
        } catch {
            return Result.Unavailable("Unable to retrieve requested user.");
        }
    }

    public async Task<Result<User>> CreateAsync(User user) {
        try {
            await _os.Users.AddAsync(user);

            int savedCount = await _os.SaveChangesAsync();
            if (savedCount == 0) { }

            return Result.Success(user);
        } catch {
            return Result.Error("Unable to add the current user at the moment. Try again later.");
        }
    }

    public async Task<Result<User>> UpdateAsync(User user) {
        try {
            _os.Users.Update(user);

            int savedCount = await _os.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }

            return Result.Success(user);
        } catch {
            return Result.Error("Unable to update the user's information. Try again later.");
        }
    }

}