using LouisasAstroWebsite.Components;
using LouisasAstroWebsite.Data;
using LouisasAstroWebsite.Services;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MongoDB
var mongoConn = builder.Configuration.GetConnectionString("MongoDb");
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConn));
builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase("GirlfriendDB");
    var filter = new BsonDocument();
    var collections = database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter }).Result;
    var collectionNames = collections.ToList().Select(c => c["name"].AsString);
    if (!collectionNames.Contains("users"))
    {
        database.CreateCollection("users");
    }
    return database.GetCollection<User>("users");
});

// Register UserService
builder.Services.AddScoped<UserService>();

// Add API controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(); // From Swashbuckle

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // From Swashbuckle
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LouisasAstroWebsite API v1"));
}

app.Run();