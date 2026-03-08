using LibraryAPI.Data;
using LibraryAPI.Data.Models;
using LibraryAPI.Data.Seeder;
using LibraryAPI.Service;
using LibraryAPI.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<PersonalLibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IAutherService, AutherService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IReadingProgressService, ReadingProgressService>();
builder.Services.AddScoped<IAdminBookService, AdminBookService>();
builder.Services.AddScoped<IUserLibraryService, UserLibraryService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()   
                   .AllowAnyMethod()   
                   .AllowAnyHeader();  
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddControllers();


builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,   
        Scheme = "bearer",                                         
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nhập token"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PersonalLibraryContext>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    await DatabaseSeeder.SeedAsync(context, config);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
// QUAN TRỌNG: Thứ tự middleware
app.UseAuthentication(); // Phải đặt trước UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task SeedAdminAsync(PersonalLibraryContext context, IConfiguration config)
{
    var adminConfig = config.GetSection("AdminAccount");
    var adminUsername = adminConfig["Username"]!;

    // Nếu admin đã tồn tại thì bỏ qua
    if (await context.Users.AnyAsync(u => u.Username == adminUsername))
        return;

    var adminUser = new User
    {
        Username = adminUsername,
        Email = adminConfig["Email"]!,
        FullName = adminConfig["FullName"]!,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminConfig["Password"]!),
        Role = "admin",         // thêm field Role vào User model
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
    };

    context.Users.Add(adminUser);
    await context.SaveChangesAsync();
    Console.WriteLine("✅ Admin account seeded successfully.");
}
