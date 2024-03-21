namespace chattiz_back.Services;

using chattiz_back.Models;
using chattiz_back.Data;
using Microsoft.EntityFrameworkCore;


public interface IChatRepository
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

public class ChatRepository : IChatRepository
{
    private readonly ApplicationDbContext _context;

    public ChatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ChatModel?> CreateChat(string name, string userId)
    {
        var chat = new ChatModel
        {
            Id = Guid.NewGuid().ToString(),
            Status = ChatStatus.None
        };

        _context.Chats.Add(chat);
        _context.ChatUsers.Add(new ChatUserModel
        {
            ChatId = chat.Id,
            UserId = userId
        });

        await _context.SaveChangesAsync();

        await AddUserToChat(chat.Id, userId);

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

    public async Task<ChatUserModel?> AddUserToChat(string chatId, string userId)
    {
        var chatUser = new ChatUserModel
        {
            ChatId = chatId,
            UserId = userId
        };

        await _context.ChatUsers.AddAsync(chatUser);
        await _context.SaveChangesAsync();

        return chatUser;
    }

    public async Task<ChatModel?> UpdateStatusChat(string id, ChatStatus status, string lastMessager, int? numberOfMessages = null)
    {
        var chat = await _context.Chats.FindAsync(id);
        if (chat == null)
        {
            return null;
        }

        chat.Status = status;
        if (numberOfMessages != null)
        {
            chat.NumberOfMessages = numberOfMessages.Value;
        }

        chat.LastMessager = lastMessager;

        await _context.SaveChangesAsync();

        return chat;
    }

    public async Task<ChatModel?> RemoveUserFromChat(string chatId, string userId)
    {

        var chatUser = await _context.ChatUsers
            .Where(cu => cu.ChatId == chatId && cu.UserId == userId)
            .FirstOrDefaultAsync();

        var chat = await _context.Chats.FindAsync(chatId);

        return chat;
    }
}