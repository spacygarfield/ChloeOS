using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.Core.Models.OS;
using ChloeOS.DataAccess.Contexts;
using Jane;
using Microsoft.EntityFrameworkCore;

namespace ChloeOS.DataAccess.Repositories.OS;

public class UserRepository : IUserRepository {

    private readonly OperatingSystemContext _os;

    public UserRepository(OperatingSystemContext os) => _os = os;

    public async Task<IResult<User[]>> GetAllAsync() {
        try {
            User[] users = await _os.Users.ToArrayAsync();

            if (!users.Any()) {
                return Result.Failure<User[]>("No local users exist yet. Why not be the first one?");
            }

            return Result.Success(users);
        } catch {
            return Result.Failure<User[]>("Unable to retrieve all local users.");
        }
    }

    public async Task<IResult<User>> GetByUsernameAsync(string username) {
        try {
            User? user = await _os.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) {
                return Result.Failure<User>("No user with that username exist.");
            }

            return Result.Success(user);
        } catch {
            return Result.Failure<User>("Unable to retrieve requested user.");
        }
    }

    public async Task<IResult<User>> CreateAsync(User user) {
        await _os.Users.AddAsync(user);

        int savedCount = await _os.SaveChangesAsync();
        if (savedCount == 0) {
            return Result.Failure<User>("Unable to add the current user at the moment. Try again later.");
        }

        return Result.Success(user);
    }

    public async Task<IResult<User>> UpdateAsync(User user) {
        _os.Users.Update(user);

        int savedCount = await _os.SaveChangesAsync();
        if (savedCount == 0) {
            return Result.Failure<User>("Unable to update the user's information. Try again later.");
        }

        return Result.Success(user);
    }

}