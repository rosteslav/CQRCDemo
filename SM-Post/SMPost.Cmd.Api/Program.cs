using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Hadlers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;
using SQRS.Core.Domain;
using SQRS.Core.Handlers;
using SQRS.Core.Infrastucture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

//register command handler
var commandHadler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();
dispatcher.RegusterHandler<NewPostCommand>(commandHadler.HandleAsync);
dispatcher.RegusterHandler<EditMessageCommand>(commandHadler.HandleAsync);
dispatcher.RegusterHandler<LikePostCommand>(commandHadler.HandleAsync);
dispatcher.RegusterHandler<AddCommentCommand>(commandHadler.HandleAsync);
dispatcher.RegusterHandler<EditCommentCommand>(commandHadler.HandleAsync);
dispatcher.RegusterHandler<RemoveCommentCommand>(commandHadler.HandleAsync);
dispatcher.RegusterHandler<DeletePostCommand>(commandHadler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
