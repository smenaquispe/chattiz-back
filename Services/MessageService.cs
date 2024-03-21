namespace chattiz_back.Services;
using chattiz_back.Models;

public interface IMessageService
{
    Task<MessageModel?> GetMessage(string id);
    Task<IEnumerable<MessageModel>> GetMessages(string chatId);
    Task<MessageModel?> CreateMessage(string chatId, string senderId, string content);
    Task<MessageModel?> UpdateMessage(string id, string content);
    Task<MessageModel?> DeleteMessage(string id);
}

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<MessageModel?> CreateMessage(string chatId, string senderId, string content)
    {
        return await _messageRepository.CreateMessage(chatId, senderId, content);
    }

    public async Task<MessageModel?> DeleteMessage(string id)
    {
        return await _messageRepository.DeleteMessage(id);
    }

    public async Task<MessageModel?> GetMessage(string id)
    {
        return await _messageRepository.GetMessage(id);
    }

    public async Task<IEnumerable<MessageModel>> GetMessages(string chatId)
    {
        return await _messageRepository.GetMessages(chatId);
    }

    public async Task<MessageModel?> UpdateMessage(string id, string content)
    {
        return await _messageRepository.UpdateMessage(id, content);
    }

}