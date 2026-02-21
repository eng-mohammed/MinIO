using Minio;
using MinIO.API.Middlewares.ExceptionMiddleware;
using MinIO.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IMinioClient>(sp =>
{
    return new MinioClient()
        .WithEndpoint("localhost:9000")
        .WithCredentials("minioadmin", "minioadmin")
        .WithSSL(false)
        .Build();
});

// configure with cloudflare R2
//builder.Services.AddSingleton<IMinioClient>(sp =>
//{
//    return new MinioClient()
//        .WithEndpoint("https://<your-account-id>.r2.cloudflarestorage.com")
//        .WithCredentials("<your-access-key>", "<your-secret-key>")
//        .WithSSL(true)
//        .Build();
//});

builder.Services.AddScoped<FileStorageService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
