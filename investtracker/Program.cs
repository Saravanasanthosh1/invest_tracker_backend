using investtracker.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure DbContext with Supabase/Postgres
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ CORS (Netlify + local dev)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetlifyAndLocal",
        policy => policy
            .WithOrigins(
                "https://sdinvest.netlify.app", // your Netlify app
                "http://localhost:3000"         // local dev
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// ✅ JWT auth (placeholder until Supabase auth wired in)
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

var app = builder.Build();


// ✅ Apply DB migrations automatically at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();   // create/update tables if needed
        Console.WriteLine("✅ Database migrated successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database migration failed: {ex.Message}");
    }
}

// ✅ Middleware pipeline
app.UseHttpsRedirection();
app.UseCors("AllowNetlifyAndLocal");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// ✅ Render needs to bind to assigned port
if (!app.Environment.IsDevelopment())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    app.Urls.Add($"http://0.0.0.0:{port}");
}

app.Run();
