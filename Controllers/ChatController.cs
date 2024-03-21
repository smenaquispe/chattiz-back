namespace chattiz_back.Controllers;

using Microsoft.AspNetCore.Mvc;
using chattiz_back.Models;
using chattiz_back.Services;


[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ChatModel>> GetChat(string id)
    {
        var chat = await _chatService.GetChat(id);

        if (chat == null)
        {
            return NotFound();
        }

        return chat;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ChatModel>>> GetChats(string userId)
    {
        var chats = await _chatService.GetChats(userId);

        if (chats == null)
        {
            return NotFound();
        }

        return Ok(chats);
    }

    [HttpPost("create")]
    public async Task<ActionResult<ChatModel>> CreateChat([FromBody] ChatCreateModel chatCreateModel)
    {
        var chat = await _chatService.CreateChat(chatCreateModel.Name!, chatCreateModel.UserId!);

        if(chat == null)
        {
            return BadRequest();
        }

        return Ok(chat);
    }

    [HttpPut("update")]
    public async Task<ActionResult<ChatModel>> UpdateChat([FromBody] ChatModel chatModel)
    {
        var chat = await _chatService.UpdateChat(chatModel.Id!, chatModel.Name!);

        if(chat == null)
        {
            return BadRequest();
        }

        return Ok(chat);
    }

    [HttpPut("update-status")]
    public async Task<ActionResult<ChatModel>> UpdateStatusChat([FromBody] ChatModel chatModel)
    {
        var chat = await _chatService.UpdateStatusChat(chatModel.Id!, chatModel.Status ?? ChatStatus.None, chatModel.LastMessager!, chatModel.NumberOfMessages);

        if(chat == null)
        {
            return BadRequest();
        }

        return Ok(chat);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ChatModel>> DeleteChat(string id)
    {
        var chat = await _chatService.DeleteChat(id);

        if(chat == null)
        {
            return NotFound();
        }

        return Ok(chat);
    }

    [HttpPost("add-user")]
    public async Task<ActionResult<ChatModel>> AddUserToChat([FromBody] string chatId, string userId)
    {
        var chat = await _chatService.AddUserToChat(chatId, userId);

        if(chat == null)
        {
            return BadRequest();
        }

        return Ok(chat);
    }

    [HttpDelete("remove-user")]
    public async Task<ActionResult<ChatModel>> RemoveUserFromChat([FromBody] string chatId, string userId)
    {
        var chat = await _chatService.RemoveUserFromChat(chatId, userId);

        if(chat == null)
        {
            return BadRequest();
        }

        return Ok(chat);
    }

}