using LouisasAstroWebsite.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LouisasAstroWebsite.Services;

public class UserService(IMongoCollection<User> users)
{
    private readonly IMongoCollection<User> _users = users ?? throw new ArgumentNullException(nameof(users));

    // Get all users
    public async Task<List<User>> GetUsersAsync() =>
        await _users.Find(_ => true).ToListAsync();

    // Get a user by ID
    public async Task<User> GetUserAsync(ObjectId id) =>
        await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

    // Create a new user
    public async Task CreateUserAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrEmpty(user.Email)) throw new ArgumentException("Email is required", nameof(user));
        await _users.InsertOneAsync(user);
    }

    // Update an existing user
    public async Task UpdateUserAsync(ObjectId id, User updatedUser)
    {
        if (updatedUser == null) throw new ArgumentNullException(nameof(updatedUser));
        await _users.ReplaceOneAsync(u => u.Id == id, updatedUser);
    }

    // Delete a user by ID
    public async Task DeleteUserAsync(ObjectId id) =>
        await _users.DeleteOneAsync(u => u.Id == id);
}