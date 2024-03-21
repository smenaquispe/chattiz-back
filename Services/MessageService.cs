namespace chattiz_back.Services;

using chattiz_back.Models;
using chattiz_back.Data;
using Microsoft.EntityFrameworkCore;

public interface IMessageRepository
{
    Task<MessageModel?> GetMessage(string id);
    Task<IEnumerable<MessageModel>> GetMessages(string chatId);
    Task<MessageModel?> CreateMessage(string chatId, string senderId, string content);
    Task<MessageModel?> UpdateMessage(string id, string content);
    Task<MessageModel?> DeleteMessage(string id);
}

public class MessageService : IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MessageModel?> CreateMessage(string chatId, string senderId, string content)
    {
        var message = new MessageModel
        {
            Id = Guid.NewGuid().ToString(),
            ChatId = chatId,
            SenderId = senderId,
            Content = content,
            SentAt = DateTime.Now
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<MessageModel?> DeleteMessage(string id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null)
        {
            return null;
        }

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<MessageModel?> GetMessage(string id)
    {
        return await _context.Messages.FindAsync(id);
    }

    public async Task<IEnumerable<MessageModel>> GetMessages(string chatId)
    {
        return await _context.Messages.Where(m => m.ChatId == chatId).ToListAsync();
    }

    public async Task<MessageModel?> UpdateMessage(string id, string content)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null)
        {
            return null;
        }

        message.Content = content;
        await _context.SaveChangesAsync();

        return message;
    }

}