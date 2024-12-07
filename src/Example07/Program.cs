using Example07;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServices();
var host = builder.Build();
await host.RunAsync();