using Microsoft.IdentityModel.Tokens;
using Api.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001";

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "https://localhost:5001",
            ValidAudiences = new List<string>
            {
                "identity",
                "weather",
                "https://localhost:5001/resources",
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("staff", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", new List<string> { "api1","api2" });
    });

    options.AddPolicy("user", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api2");
    });
});
    

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
