using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Revisao_ASP.NET_Web_API.Models;
using Revisao_ASP.NET_Web_API.Models.DTO;
using Revisao_ASP.NET_Web_API.Models.Entities;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// indicate the tipe of database <SQLSERVER> and the string connection
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// register the identity service
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//Add autentication based in cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login"; // login path
    options.AccessDeniedPath = "/auth/access-denied"; // login denied/access denied path
});

// Add admin role service (Admin Level)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

// register the DTO
builder.Services.AddScoped<IRepository, Repository>();

// implement the CORS services - CORS Domain
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin() // allow any front-end access the API - any origin
            .AllowAnyMethod() // allow any method to access this API <GET, POST, PUT, DELETE, ... >
            .AllowAnyHeader(); // allow any header to access this API
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true); // convert the route caracteres in lower case

var app = builder.Build();

// uses the service CORS
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add authentication
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// define the rule to aplicate the autorization roles
async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(); // define the manager roles

    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>(); // define UserManager to management the users operations

    string[] roleNames = { "Admin" }; // define the role names

    IdentityResult roleResult;

    foreach(var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);

        if (!roleExist)
        {
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName)); // create the new role in case of that don't exist
        }
    }

    var adminUser = await userManager.FindByEmailAsync("admin.novo@email.com"); // serach admin user for this application

    if (adminUser == null) // if the admin user dont exist create the admin user
    {
        var newAdmin = new AppUser // define a new admin user
        {
            UserName = "admin.novo@email.com",
            Email = "admin.novo@email.com"
        };

        var createPowerFull = await userManager.CreateAsync(newAdmin, "AdmiNovo@123"); // create a new admin user

        if (createPowerFull.Succeeded) 
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }

        var userRoles = await userManager.GetRolesAsync(newAdmin); // get the role defined to the new user admin

        Console.WriteLine($"Roles users {newAdmin.UserName} : {string.Join(",", userRoles)}"); // just console de admin role
    }
}

// create the service scope
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRoles(services);
}

app.Run();
