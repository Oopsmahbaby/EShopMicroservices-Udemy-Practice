using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddMarten(options =>
{
	options.Connection(builder.Configuration.GetConnectionString("Database")!);
	//options.AutoCreateSchemaObjects = AutoCreate.All;
}).UseLightweightSessions();

var app = builder.Build();

// 
//using (var scope = app.Services.CreateScope())
//{
//	var store = scope.ServiceProvider.GetRequiredService<IDocumentStore>();
//	await store.Advanced.Clean.CompletelyRemoveAllAsync(); // nếu muốn clear trước
//	await store.Storage.ApplyAllConfiguredChangesToDatabaseAsync();
//}

app.MapCarter();

app.Run();
