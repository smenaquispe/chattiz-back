namespace chattiz_back.Services;
using chattiz_back.Models;

public interface IUserService
{
    Task<UserModel?> GetUser(string id);
    Task<UserModel?> GetUser(string email, string password);

    Task<UserModel?> CreateUser(string username, string email, string password);

    Task<UserModel?> UpdateUser(string id, string username, string email, string password);

    Task<UserModel?> DeleteUser(string id);

}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserModel?> CreateUser(string username, string email, string password)
    {
        return await _userRepository.CreateUser(username, email, password);
    }

    public async Task<UserModel?> DeleteUser(string id)
    {
        return await _userRepository.DeleteUser(id);
    }

    public async Task<UserModel?> GetUser(string id)
    {
        return await _userRepository.GetUser(id);
    }

    public async Task<UserModel?> GetUser(string email, string password)
    {
        return await _userRepository.GetUser(email, password);
    }

    public async Task<UserModel?> UpdateUser(string id, string username, string email, string password)
    {
        return await _userRepository.UpdateUser(id, username, email, password);
    }

}
