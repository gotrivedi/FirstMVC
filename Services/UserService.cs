using System.Text.Json;
using UserManagment.Models;

namespace UserManagment.Services;

public class UserService
{
    private readonly string _filePath = "Data/users.json";

    public List<User> GetUsers()
    {
        var json = File.ReadAllText(_filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var users = JsonSerializer.Deserialize<List<User>>(json, options);

        return users?.Take(50).ToList() ?? new List<User>();
    }
}