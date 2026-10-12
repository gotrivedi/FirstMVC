using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace UserManagment.Models;

public class ApiRequest<T>
{
    public string Action { get; set; } = string.Empty;
    public T Data { get; set; }




}