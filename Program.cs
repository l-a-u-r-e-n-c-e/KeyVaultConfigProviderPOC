using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["KeyVault:VaultUri"]!),
    new AzureCliCredential() // You can change this to DefaultAzureCredential() if it's easier
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/specialmessage", () =>
{
    var secret = app.Configuration["MessageCentre:Message"];
    var response = $"Don't actually print secrets like \"{secret}\" please";
    return response;
})
.WithName("GetSpecialMessage")
.WithOpenApi();

app.Run();
