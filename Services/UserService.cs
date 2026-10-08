using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using UserManagment.Controllers;
using UserManagment.Models;

namespace UserManagment.Services;

public class UserService
{
    private readonly string _filePath = "Data/users.json";
    private readonly ILogger<UserService> _logger;

    public UserService(ILogger<UserService> logger)
    {
        Console.WriteLine("🔥 USER SERVICE CONSTRUCTOR CALLED");

        Console.WriteLine($"Logger is null: {logger == null}");
        _logger = logger;
    }

    public List<User> GetAllUsers()
    {
        var json = File.ReadAllText(_filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var users = JsonSerializer.Deserialize<List<User>>(json, options);
        return users ?? new List<User>();
    }
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

    public void AddUser(User user)
    {
        var users = GetAllUsers();

        var nextId = users
    .Select(u => int.Parse(u.Id))
    .DefaultIfEmpty(0)
    .Max() + 1;
        user.Id = nextId.ToString();

        users.Add(user);

        var json = JsonSerializer.Serialize(users,
        new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
        _logger.LogInformation("User {Id} created successfully", user.Id);
    }

    public void EditUser(User user)
    {
        Console.WriteLine("In UserService");
        var users = GetAllUsers();
        var matchUser = users.FirstOrDefault(u => u.Id == user.Id);

        if (matchUser != null)
        {
            matchUser.Name = user.Name;
            matchUser.Email = user.Email;
        }


        var json = JsonSerializer.Serialize(users,
               new JsonSerializerOptions
               {
                   WriteIndented = true
               });

        File.WriteAllText(_filePath, json);
    }

    public bool DeleteUser(String Id)
    {
        var users = GetAllUsers();
        var user = users.FirstOrDefault(u => u.Id == Id);
        if (user != null)
        {
            users.Remove(user);
            writeJsonToFile(users);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool writeJsonToFile<T>(T data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data,
              new JsonSerializerOptions
              {
                  WriteIndented = true
              });

            File.WriteAllText(_filePath, json);
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}

