using Scalar.AspNetCore;
using WhatsOn.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapEndpoints();
app.UseOutputCache();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference(options =>
	{
		options.WithTitle("WhatsOn API")
		.WithTheme(ScalarTheme.Mars)
		.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
	});
}

app.UseHttpsRedirection();
app.Run();