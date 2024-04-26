using ApiCubosAzureExamen.Data;
using ApiCubosAzureExamen.Helpers;
using ApiCubosAzureExamen.Repositories;
using ApiCubosAzureExamen.Services;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using NSwag;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
string connectionString = string.Empty;
string issuer = string.Empty;
string audience = string.Empty;
string credentials = string.Empty;
string urlContainer = string.Empty;

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>()!;
KeyVaultSecret secret = await secretClient.GetSecretAsync("sql-connection");
KeyVaultSecret secret1 = await secretClient.GetSecretAsync("issuer");
KeyVaultSecret secret2 = await secretClient.GetSecretAsync("audience");
KeyVaultSecret secret3 = await secretClient.GetSecretAsync("secretKey");
KeyVaultSecret secret4 = await secretClient.GetSecretAsync("azureStorage");

connectionString = secret.Value;
issuer = secret1.Value;
audience = secret2.Value;
credentials = secret3.Value;
urlContainer = secret4.Value;

HelperActionServicesOauth helper = new HelperActionServicesOauth(issuer, audience, credentials);
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Api Cubos";
    document.Description = "Api Cubos.";
    document.AddSecurity("JWT", Enumerable.Empty<string>(),
        new NSwag.OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Copia y pega el Token en el campo 'Value:' así: Bearer {Token JWT}."
        }
    );
    document.OperationProcessors.Add(
    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

BlobServiceClient blobServiceClient = new BlobServiceClient(urlContainer);
builder.Services.AddTransient<ServiceAzureStorage>();
builder.Services.AddTransient<BlobServiceClient>(service => blobServiceClient);

#region SqlServer
builder.Services.AddTransient<RepositoryCubosSqlServer>();
builder.Services.AddDbContext<CubosContext>(options =>
{
    options.UseSqlServer(connectionString);
});
#endregion

builder.Services.AddSingleton<HelperActionServicesOauth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticationSchema()).AddJwtBearer(helper.GetJwtBearerOptions());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        url: "/swagger/v1/swagger.json", name: "Api v1");

    options.RoutePrefix = "";
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


