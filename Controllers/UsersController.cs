using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using UserManagment.Models;
using UserManagment.Services;
using System.Text.Json;

namespace UserManagment.Controllers;

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

    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        string json = "\"hello\"";

        JsonDocument doc = JsonDocument.Parse(json);
        dynamic obj1 = new System.Dynamic.ExpandoObject();
        obj1.Name = "Vivek";
        obj1.Phone = "9966332255";

        Console.WriteLine(obj1.GetType());
        Console.WriteLine(doc.GetType());
        Console.WriteLine(doc.RootElement.ValueKind);
        Console.WriteLine(doc.RootElement.GetString());
        // _userService.AddUser(user);
        return Ok(User);
    }

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
