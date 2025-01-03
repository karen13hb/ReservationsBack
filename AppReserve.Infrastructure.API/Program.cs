using AppReserve.Application.EventHandlers;
using AppReserve.Application.Interfaces;
using AppReserve.Application.Services;
using AppReserve.Domain.Events;
using AppReserve.Domain.Interfaces;
using AppReserve.Domain.Interfaces.Events;
using AppReserve.Domain.Interfaces.Events.Handlers;
using AppReserve.Domain.Interfaces.Repositories;
using AppReserve.Infrastructure.Persistence;
using AppReserve.Infrastructure.Persistence.Context;
using AppReserve.Infrastructure.Persistence.Events;
using AppReserve.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddScoped<IEventHandler<ReservationCreatedEvent>, ReservationNotificationHandler>();
builder.Services.AddScoped<IEventHandler<ReservationCancelledEvent>, ReservationNotificationHandler>();
builder.Services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();

builder.Services.AddScoped<ISpaceRepository, SpaceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddScoped<IReservationService, ReservationService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();


app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
