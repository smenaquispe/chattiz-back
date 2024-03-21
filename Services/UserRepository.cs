namespace chattiz_back.Services;

using chattiz_back.Models;
using chattiz_back.Data;
using chattiz_back.Utils;
using Microsoft.EntityFrameworkCore;


public interface IUserRepository
{
    Task<UserModel?> GetUser(string id);
    Task<UserModel?> GetUser(string email, string password);

    Task<UserModel?> CreateUser(string username, string email, string password);

    Task<UserModel?> UpdateUser(string id, string username, string email, string password);

    Task<UserModel?> DeleteUser(string id);

}

public class UserRepository : IUserRepository
{

    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserModel?> CreateUser(string username, string email, string password)
    {
        var user = new UserModel
        {
            Id = Guid.NewGuid().ToString(),
            Username = username,
            Email = email,
            Password = HashCrypter.HashPassword(password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<UserModel?> DeleteUser(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return null;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<UserModel?> GetUser(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<UserModel?> GetUser(string email, string password)
    {
        var res = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if(res == null)
        {
            return null;
        }

        var isPasswordValid = HashCrypter.VerifyPassword(password, res.Password ?? "");
        if(!isPasswordValid)
        {
            return null;
        }

        return res;

    }

    public async Task<UserModel?> UpdateUser(string id, string username, string email, string password)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return null;
        }

        user.Username = username;
        user.Email = email;
        user.Password = HashCrypter.HashPassword(password);

        await _context.SaveChangesAsync();

        return user;
    }


};