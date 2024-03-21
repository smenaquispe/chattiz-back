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
    Task<ChatModel?> AddUserToChat(string chatId, string userId);
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
            Name = name
        };

        _context.Chats.Add(chat);
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
        await _context.SaveChangesAsync();

        return chat;
    }

    public async Task<ChatModel?> GetChat(string id)
    {
        return await _context.Chats.FindAsync(id);
    }

    public async Task<IEnumerable<ChatModel>> GetChats(string userId)
    {
        return await _context.Chats
            .Where(c => c.Participants!.Any(u => u == userId))
            .ToListAsync();
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

    public async Task<ChatModel?> AddUserToChat(string chatId, string userId)
    {
        var chat = await _context.Chats.FindAsync(chatId);
        if (chat == null)
        {
            return null;
        }

        chat.Participants = chat.Participants!.Append(userId).ToArray();
        await _context.SaveChangesAsync();

        return chat;
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
        var chat = await _context.Chats.FindAsync(chatId);
        if(chat == null)
        {
            return null;
        }

        chat.Participants = chat.Participants!.Where(u => u != userId).ToArray();
        await _context.SaveChangesAsync();

        return chat;
    }
}