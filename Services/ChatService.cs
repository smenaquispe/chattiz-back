namespace chattiz_back.Services;
using chattiz_back.Models;

public interface IChatService
{
    Task<ChatModel?> GetChat(string id);
    Task<IEnumerable<ChatModel>> GetChats(string userId);
    Task<ChatModel?> CreateChat(string name, string userId);
    Task<ChatModel?> UpdateChat(string id, string name);
    Task<ChatModel?> UpdateStatusChat(string id, ChatStatus status, string lastMessager, int? numberOfMessages = null);

    Task<ChatModel?> DeleteChat(string id);
    Task<ChatUserModel?> AddUserToChat(string chatId, string userId);

    Task<ChatModel?> RemoveUserFromChat(string chatId, string userId);
}

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<ChatUserModel?> AddUserToChat(string chatId, string userId)
    {
        return await _chatRepository.AddUserToChat(chatId, userId);
    }

    public async Task<ChatModel?> CreateChat(string name, string userId)
    {
        return await _chatRepository.CreateChat(name, userId);
    }

    public async Task<ChatModel?> DeleteChat(string id)
    {
        return await _chatRepository.DeleteChat(id);
    }

    public async Task<ChatModel?> GetChat(string id)
    {
        return await _chatRepository.GetChat(id);
    }

    public async Task<IEnumerable<ChatModel>> GetChats(string userId)
    {
        return await _chatRepository.GetChats(userId);
    }

    public async Task<ChatModel?> RemoveUserFromChat(string chatId, string userId)
    {
        return await _chatRepository.RemoveUserFromChat(chatId, userId);
    }

    public async Task<ChatModel?> UpdateChat(string id, string name)
    {
        return await _chatRepository.UpdateChat(id, name);
    }

    public async Task<ChatModel?> UpdateStatusChat(string id, ChatStatus status, string lastMessager, int? numberOfMessages = null)
    {
        return await _chatRepository.UpdateStatusChat(id, status, lastMessager, numberOfMessages);
    }

    
}