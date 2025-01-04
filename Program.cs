using System.Text.Json;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);


// Bu paketi indiriniz : 
// dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks --version 9.0.0

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

// healty dependencyleri ekleyebilirsiniz!
// Web apinin bağlı olduğu tüm bağımlılıkların sağlık durumunu kontrol edebilirsiniz!!

// veri tabanı bağımlılığını ekleyelim!!


builder.Services.AddHealthChecksUI(option=>{

option.AddHealthCheckEndpoint("Health Check","/health");

}).AddInMemoryStorage();

builder.Services.AddHealthChecks()
.AddSqlServer(
    connectionString: "Server=db11596.public.databaseasp.net; Database=db11596; User Id=db11596; Password=i#5G!Tc2p6J+; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;",
    healthQuery: "select 1;",
    name: "Sql Server",
    tags: new[] { "db", "sql" })
.AddRedis(
    redisConnectionString: "redis-11810.c300.eu-central-1-1.ec2.redns.redis-cloud.com:11810",
    name: "Redis"
    )
.AddCheck<ApiHealthCheck>("Rick And Morty Api Health Check");
/*.AddCheck("Custom Check", () =>
{

    // saniye değerini 2 ye göre mod alıp random healty yada unhealty mesajı verelim
    return DateTime.Now.Second % 2 == 0 ?
     HealthCheckResult.Healthy("Custom Health succeeded") :
     HealthCheckResult.Unhealthy("Custon HealthCheck failed");

});*/

// healthcheck ekranını tasarımlaştırmak için!!
builder.Services.AddHealthChecksUI().AddInMemoryStorage();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapControllers();

app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    /*ResponseWriter =  async (context, report) =>
    {
        var value = report.Entries.Select(s => new HealthCheckModel
        {
            Key = s.Key,
            Value = s.Value.Status.ToString(),
             Exeption =  s.Value.Exception?.Message,
        });
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(value, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }*/
});
app.Run();

