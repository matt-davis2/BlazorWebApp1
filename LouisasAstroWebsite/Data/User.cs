using MongoDB.Bson;

namespace LouisasAstroWebsite.Data;

public class User
{
    public ObjectId Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public Preferences Preferences { get; set; } = new Preferences();
}

public class Preferences
{
    public string Theme { get; set; } = "Light";
    public bool Notifications { get; set; } = true;
    public List<string> FavoriteColors { get; set; } = new List<string>();
}