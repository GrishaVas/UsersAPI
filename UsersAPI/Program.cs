using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UsersAPI.Models;
using UsersAPI.Models.Business;
using UsersAPI.Models.DatabaseContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddUsersSwagger();
builder.Services.AddUsersDBContext(builder.Configuration.GetConnectionString("Default"));
builder.Services.AddScoped<UsersService>();
builder.Services.AddUsersAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<UsersLogger>();
app.Run();
