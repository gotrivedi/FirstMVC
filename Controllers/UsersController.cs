using Microsoft.AspNetCore.Mvc;
using UserManagment.Models;
using UserManagment.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace UserManagment.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<UsersController> _logger;


    public UsersController(UserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var users = _userService.GetUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetUser(string id)
    {

        var user = _userService.GetUsers().FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [Authorize(Policy = "Users.Create")]
    [HttpPost]
    public IActionResult CreateUser(User user)
    {

        _userService.AddUser(user);

        return Ok(user);
    }

    [Authorize(Policy = "Users.Update")]
    [HttpPut]
    public IActionResult EditUser(User user)
    {
        _logger.LogInformation(
        "EditUser received: Id={Id}, Name={Name}, Email={Email}",
        user.Id,
        user.Name,
        user.Email);
        _userService.EditUser(user);
        return Ok(user);
    }


    [Authorize(Policy = "Users.Delete")]
    [HttpDelete]
    public IActionResult DeleteUser(string id)
    {
        var result = _userService.DeleteUser(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("TestGeneric")]
    public IActionResult TestGeneric([FromBody] JsonElement data)
    {
        Console.WriteLine(data.ValueKind);

        return Ok();
    }
}
