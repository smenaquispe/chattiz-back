namespace chattiz_back.Controllers;

using Microsoft.AspNetCore.Mvc;
using chattiz_back.Models;
using chattiz_back.Services;
using chattiz_back.Dto;


[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatRepository _chatService;

    public ChatController(IChatRepository chatService)
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
        var chat = await _chatService.CreateChat(chatCreateModel.Name!, chatCreateModel.UserIds!);

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
    public async Task<ActionResult<ChatModel>> UpdateStatusChat([FromBody] ChatUserModel chatUserModel)
    {
        var chat = await _chatService.UpdateStatusChat(chatUserModel.ChatId!, chatUserModel.UserId!, chatUserModel.Status ?? ChatStatus.None, chatUserModel.LastMessager!, chatUserModel.NumberOfMessages);

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

    [HttpPost("add-users")]
    public async Task<ActionResult<ChatModel>> AddUsersToChat([FromBody] ChatAddUsers chatAddUsers)
    {
        var chat = await _chatService.AddUsersToChat(chatAddUsers.ChatId!, chatAddUsers.UserIds!);

        if(chat == null)
        {
            return BadRequest();
        }

        return Ok(chat);
    }

    [HttpDelete("remove-user")]
    public async Task<ActionResult<ChatModel>> RemoveUserFromChat([FromBody] ChatRemoveUser chatRemoveUser)
    {
        var chat = await _chatService.RemoveUserFromChat(chatRemoveUser.ChatId!, chatRemoveUser.UserId!);

        if(chat == null)
        {
            return BadRequest();
        }

        return Ok(chat);
    }

}