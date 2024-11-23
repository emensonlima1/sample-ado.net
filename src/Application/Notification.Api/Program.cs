using Common.Interfaces;
using Domain.BusinessRules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient <ISmsRule, SmsRule>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();