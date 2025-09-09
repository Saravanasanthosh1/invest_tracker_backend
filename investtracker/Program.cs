using investtracker.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with Supabase connection string
try
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}
catch (Exception ex)
{
    Console.WriteLine("⚠️ Failed to init DB: " + ex.Message);
}


// Enable CORS for Netlify + local dev
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetlifyAndLocal",
        policy => policy
            .WithOrigins(
                "https://sdinvest.netlify.app", // your Netlify URL
                "http://localhost:3000"         // local React dev server
            )
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add JWT auth (Supabase)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://YOUR-PROJECT.supabase.co/auth/v1";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://YOUR-PROJECT.supabase.co/auth/v1",
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!builder.Environment.IsDevelopment())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}


var app = builder.Build();

// ✅ Order of middlewares matters
if (app.Environment.IsDevelopment() || true) // enable always
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowNetlifyAndLocal");

app.UseAuthentication();  // ✅ Add this before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
