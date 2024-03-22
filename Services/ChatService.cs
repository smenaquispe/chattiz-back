namespace chattiz_back.Services;

using chattiz_back.Models;
using chattiz_back.Data;
using Microsoft.EntityFrameworkCore;


public interface IChatRepository
{
    Task<ChatModel?> GetChat(string id);

    Task<IEnumerable<ChatModel>> GetChats(string userId);
    Task<ChatModel?> CreateChat(string name, string[] userIds);
    Task<ChatModel?> UpdateChat(string id, string name);
    Task<ChatUserModel?> UpdateStatusChat(string chatId, string userId, ChatStatus status, string lastMessager, int? numberOfMessages = null);

    Task<ChatModel?> DeleteChat(string id);
    Task<ChatUserModel[]?> AddUsersToChat(string chatId, string[] userIds);
    Task<ChatModel?> RemoveUserFromChat(string chatId, string userId);

    Task<UserModel[]?> GetUsersFromChat(string chatId);
}

public class ChatService : IChatRepository
{
    private readonly ApplicationDbContext _context;

    public ChatService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ChatModel?> CreateChat(string name, string[] userIds)
    {
        var chat = new ChatModel
        {
            Id = Guid.NewGuid().ToString(),
            Name = name
        };

        _context.Chats.Add(chat);
        foreach (var userId in userIds)
        {
            _context.ChatUsers.Add(new ChatUserModel
            {
                ChatId = chat.Id,
                UserId = userId
            });
        }
        
        await _context.SaveChangesAsync();

        return chat;
    }

    public async Task<ChatModel?> DeleteChat(string id)
    {
        var chat = await _context.Chats.FindAsync(id);
        if (chat == null)
        {
            return null;
        }

        _context.Chats.Remove(chat);

        _context.ChatUsers.RemoveRange(_context.ChatUsers.Where(cu => cu.ChatId == id));

        await _context.SaveChangesAsync();

        return chat;
    }

    public async Task<ChatModel?> GetChat(string id)
    {
        return await _context.Chats.FindAsync(id);
    }

    public async Task<IEnumerable<ChatModel>> GetChats(string userId)
    {
        return await _context.ChatUsers
            .Where(cu => cu.UserId == userId)
            .Join(_context.Chats, cu => cu.ChatId, c => c.Id, (cu, c) => c)
            .ToArrayAsync();
    }

    public async Task<ChatModel?> UpdateChat(string id, string name)
    {
        var chat = await _context.Chats.FindAsync(id);
        if (chat == null)
        {
            return null;
        }

        chat.Name = name;
        await _context.SaveChangesAsync();

        return chat;
    }

    public async Task<ChatUserModel[]?> AddUsersToChat(string chatId, string[] userId)
    {

        ChatUserModel[] chatUsers = [];

        foreach (var id in userId)
        {
            var chatUser = new ChatUserModel
            {
                ChatId = chatId,
                UserId = id
            };

            chatUsers.Append(chatUser);

            await _context.ChatUsers.AddAsync(chatUser);
        }
       
        await _context.SaveChangesAsync();

        return chatUsers;
    }

    public async Task<ChatUserModel?> UpdateStatusChat(string chatId, string userId, ChatStatus status, string lastMessager, int? numberOfMessages = null)
    {
        var chatUser = await _context.ChatUsers
            .Where(cu => cu.ChatId == chatId && cu.UserId == userId)
            .FirstOrDefaultAsync();
        

        if (chatUser == null)
        {
            return null;
        }

        chatUser.Status = status;
        chatUser.LastMessager = lastMessager;
        if (numberOfMessages != null)
        {
            chatUser.NumberOfMessages = numberOfMessages.Value;
        }

        await _context.SaveChangesAsync();
        
        return chatUser;
    }

    public async Task<ChatModel?> RemoveUserFromChat(string chatId, string userId)
    {

        var chatUser = await _context.ChatUsers
            .Where(cu => cu.ChatId == chatId && cu.UserId == userId)
            .FirstOrDefaultAsync();

        var chat = await _context.Chats.FindAsync(chatId);

        return chat;
    }

    public async Task<UserModel[]?> GetUsersFromChat(string chatId)
    {
        return await _context.ChatUsers
            .Where(cu => cu.ChatId == chatId)
            .Join(_context.Users, cu => cu.UserId, u => u.Id, (cu, u) => u)
            .ToArrayAsync();
    }
}