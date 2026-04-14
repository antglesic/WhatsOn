var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
	.WithRedisCommander(c =>
	{
		c.WithUrls(u => u.Urls.ForEach(url => url.DisplayText = "RedisCommander instance"));
	})
	.WithRedisInsight(c =>
	{
		c.WithUrls(u => u.Urls.ForEach(url => url.DisplayText = "RedisInsight instance"));
	});

var apiService = builder.AddProject<Projects.WhatsOn_Api>("api")
	.WithUrls(u =>
	{
		u.Urls.ForEach(u =>
		{
			u.Url = $"{u.Url.TrimEnd('/')}/scalar";
			u.DisplayText = "Scalar UI";
		});
	})
	.WithHttpHealthCheck("/health")
	.WithReference(cache)
	.WaitFor(cache);

builder.AddProject<Projects.WhatsOn_WebApplication>("web")
	.WithHttpHealthCheck("/health")
	.WithReference(apiService)
	.WaitFor(apiService)
	.WithHttpsEndpoint(port: 7066, name: "web-https"); ;

await builder
	.Build()
	.RunAsync();
