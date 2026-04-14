using Scalar.AspNetCore;
using WhatsOn.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.AddServiceDefaults();
builder.AddRedisClient("cache");
builder.AddRedisOutputCache("cache");

builder.Services.AddOpenApi();
builder.Services.AddResponseCompression(o => o.EnableForHttps = true);
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("WhatsOnWebClient");
app.UseResponseCompression();
app.UseRateLimiter();
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

app.MapDefaultEndpoints();
app.MapEndpoints();
app.Run();
