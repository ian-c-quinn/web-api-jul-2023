using AutoMapper;
using EmployeesHrApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var employeesConnectionString = builder.Configuration.GetConnectionString("employees") ?? throw new Exception("Need a Connection String");

builder.Services.AddDbContext<EmployeeDataContext>(options =>
{
    options.UseSqlServer(employeesConnectionString);
});

var mapperConfig = new MapperConfiguration(opt =>
{
    opt.AddProfile<EmployeesHrApi.AutomapperProfiles.Employees>();
    opt.AddProfile<EmployeesHrApi.AutomapperProfiles.HiringRequestProfile>();
});

var mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton<IMapper>(mapper);
builder.Services.AddSingleton<MapperConfiguration>(mapperConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


app.MapControllers();

app.Run();