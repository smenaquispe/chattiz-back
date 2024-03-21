namespace chattiz_back.Controllers;

using Microsoft.AspNetCore.Mvc;
using chattiz_back.Models;
using chattiz_back.Services;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<UserModel>> GetUser(string id)
    {
        var user = await _userService.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserModel>> Login([FromBody] UserModel user)
    {
        var userFound = await _userService.GetUser(user.Email!, user.Password!);
        if (userFound == null)
        {
            return NotFound();
        }
        return Ok(userFound);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserModel>> Register([FromBody] UserModel user)
    {
        var userCreated = await _userService.CreateUser(user.Username!, user.Email!, user.Password!);
        if (userCreated == null)
        {
            return BadRequest();
        }
        return Ok(userCreated);
    }

    [HttpPut("update")]
    public async Task<ActionResult<UserModel>> Update([FromBody] UserModel user)
    {
        var userUpdated = await _userService.UpdateUser(user.Id!, user.Username!, user.Email!, user.Password!);
        if (userUpdated == null)
        {
            return BadRequest();
        }
        return Ok(userUpdated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserModel>> Delete(string id)
    {
        var userDeleted = await _userService.DeleteUser(id);
        if (userDeleted == null)
        {
            return NotFound();
        }
        return Ok(userDeleted);
    }
}