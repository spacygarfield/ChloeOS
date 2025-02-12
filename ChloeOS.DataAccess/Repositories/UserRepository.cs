using ChloeOS.Client.Database;
using ChloeOS.Core.Contracts.DataAccess;
using ChloeOS.Core.Models;
using Jane;
using Microsoft.EntityFrameworkCore;

namespace ChloeOS.DataAccess.Repositories;

public class UserRepository : IUserRepository {

    private readonly OperatingSystemContext _context;

    public UserRepository(OperatingSystemContext context) => _context = context;

    public async Task<IResult<User[]>> GetAllAsync() {
        User[] users = await _context.Users.ToArrayAsync();

        if (!users.Any()) {
            return Result.Failure<User[]>("No local users exist yet. Why not be the first one?");
        }

        return Result.Success(users);
    }

    public async Task<IResult<User>> GetByUsernameAsync(string username) {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) {
            return Result.Failure<User>("No user with that username exist.");
        }

        return Result.Success(user);
    }

    public async Task<IResult<User>> CreateAsync(User user) {
        await _context.Users.AddAsync(user);

        int savedCount = await _context.SaveChangesAsync();
        if (savedCount != 1) {
            return Result.Failure<User>("Unable to add the current user at the moment. Try again later.");
        }

        return Result.Success(user);
    }

    public async Task<IResult<User>> UpdateAsync(User user) {
        _context.Users.Update(user);

        int savedCount = await _context.SaveChangesAsync();
        if (savedCount != 1) {
            return Result.Failure<User>("Unable to update the user's information. Try again later.");
        }

        return Result.Success(user);
    }

}