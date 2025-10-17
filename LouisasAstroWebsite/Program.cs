using LouisasAstroWebsite.Components;
using LouisasAstroWebsite.Data;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//MongoDB
var mongoConn = builder.Configuration.GetConnectionString("MongoDb");
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConn));
builder.Services.AddScoped(x =>
{
    var client = x.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase("GirlfriendDB");

    // Check if collection exists, create if not
    var filter = new BsonDocument();
    var collections = database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter }).Result;
    var collectionNames = collections.ToList().Select(c => c["name"].AsString);
    if (!collectionNames.Contains("users"))
    {
        database.CreateCollection("users");
    }

    return database.GetCollection<User>("users");
});

// Add API controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();