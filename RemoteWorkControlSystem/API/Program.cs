using AutoMapper;
using BLL;
using BLL.Interfaces;
using BLL.Services;
using DAL.Data;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowOrigin",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});


var connectionString = builder.Configuration.GetConnectionString("RemoteWorkControlSystemDB");
builder.Services.AddDbContext<RemoteWorkControlSystemDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var mapperConfiguration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutomapperProfile());
});
builder.Services.AddSingleton(mapperConfiguration.CreateMapper());

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IProjectMemberService, ProjectMemberService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWorkSessionService, WorkSessionService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["BlopContainer:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["BlopContainer:queue"], preferMsi: true);
});


var app = builder.Build();

app.UseCors("AllowOrigin");
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
