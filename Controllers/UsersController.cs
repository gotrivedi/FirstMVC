using Microsoft.AspNetCore.Mvc;
using UserManagment.Services;

namespace UserManagment.Controllers;

public class UsersController : Controller
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    public IActionResult Index()
    {
        var users = _userService.GetUsers();

        return View(users);
    }
}