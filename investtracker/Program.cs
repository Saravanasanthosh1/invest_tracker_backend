using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Enable CORS for Netlify + local dev
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetlifyAndLocal",
        policy => policy
            .WithOrigins(
                "https://YOUR_NETLIFY_APP.netlify.app", // replace with actual Netlify URL
                "http://localhost:3000" // local React dev server
            )
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add services to the container.
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// Use CORS before controllers
app.UseCors("AllowNetlifyAndLocal");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
