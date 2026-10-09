using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class HomeController : Controller
{

    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public HomeController(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    // public IActionResult Index()
    // {
    //     // return Content(_configuration.GetConnectionString("DefaultConnection") ?? "Connection String Not Found");
    //     return Content(_configuration["MySecret"] ?? "Secret not found");
    // }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("http://jsonplaceholder.typicode.com/todos/1");
        var data = await response.Content.ReadAsStringAsync();
        return Content(data);
    }
}
