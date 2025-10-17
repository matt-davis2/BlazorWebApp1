using LouisasAstroWebsite.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace LouisasAstroWebsite.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IMongoCollection<User> users) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var userAccounts = await users.Find(_ => true).ToListAsync();
        return Ok(userAccounts);
    }
}