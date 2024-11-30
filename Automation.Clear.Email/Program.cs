using Automation.Clear.Email;
using Automation.Clear.Email.Services;
using Automation.Clear.Email.Services.Interfaces;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.Configure<GMailSettings>(builder.Configuration.GetSection(nameof(GMailSettings)));

builder.Services.AddScoped<IProcessClearEmailService, ProcessClearEmailService>();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
var host = builder.Build();
host.Run();
