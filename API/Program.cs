using API.Data;
using API.Entities;
using API.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<UserDbContext>(
    opts =>
    {
        opts.UseSqlServer(
            "Server=localhost,1433;Database=BragaAuthentication;User Id=sa;Password=SqlServer2019!;TrustServerCertificate=True;");
    });
builder
    .Services
        .AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey("s5gly78n4g5t941ws5gly78n4g5t941w"u8.ToArray()),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy(
            nameof(MinimalAge), 
            policy => policy.AddRequirements(new MinimalAge(18)));
    });
builder.Services.AddSingleton<IAuthorizationHandler, AgeAuthorization>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<UserDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();   
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("*");
app.MapControllers();
app.Run();


