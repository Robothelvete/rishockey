var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsEnvironment("Test") || app.Environment.IsEnvironment("Local")) {
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseForwardedHeaders(new ForwardedHeadersOptions {
	ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
