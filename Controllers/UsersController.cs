using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using UserManagment.Models;
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

    public IActionResult Details(string id)
    {
        var users = _userService.GetUsers();
        var user = users.FirstOrDefault(u => u.Id == id);
        return View(user);

    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(User user)
    {
        _userService.AddUser(user);
        return RedirectToAction("Index");
    }

    public IActionResult Edit(string Id)
    {
        return Details(Id);
    }

    [HttpPost]
    public IActionResult Edit(User user)
    {
        _userService.EditUser(user);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(String id)
    {
        var success = _userService.DeleteUser(id);

        TempData[success ? "Message" : "Error"] =
        success ? "Deleted Successfully" : "Deletetion Failed";


        return RedirectToAction("Index");
    }
}