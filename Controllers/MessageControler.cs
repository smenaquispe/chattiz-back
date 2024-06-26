namespace chattiz_back.Controllers;

using Microsoft.AspNetCore.Mvc;
using chattiz_back.Models;
using chattiz_back.Services;


[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly  IMessageRepository _messageService;

    public MessageController(IMessageRepository messageService)
    {
        _messageService = messageService;
    }

    public async Task<ActionResult<MessageModel>> GetMessage(string id)
    {
        var message = await _messageService.GetMessage(id);

        if (message == null)
        {
            return NotFound();
        }

        return message;
    }

    public async Task<ActionResult<IEnumerable<MessageModel>>> GetMessages(string chatId)
    {
        var messages = await _messageService.GetMessages(chatId);

        if (messages == null)
        {
            return NotFound();
        }

        return Ok(messages);
    }

    public async Task<ActionResult<MessageModel>> CreateMessage([FromBody] MessageModel messageModel)
    {
        var message = await _messageService.CreateMessage(messageModel.ChatId!, messageModel.SenderId!, messageModel.Content!);

        if(message == null)
        {
            return BadRequest();
        }

        return Ok(message);
    }

    public async Task<ActionResult<MessageModel>> UpdateMessage([FromBody] MessageModel messageModel)
    {
        var message = await _messageService.UpdateMessage(messageModel.Id!, messageModel.Content!);

        if(message == null)
        {
            return BadRequest();
        }

        return Ok(message);
    }

    public async Task<ActionResult<MessageModel>> DeleteMessage(string id)
    {
        var message = await _messageService.DeleteMessage(id);

        if(message == null)
        {
            return BadRequest();
        }

        return Ok(message);
    }
}