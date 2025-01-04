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

builder.Services.AddHealthChecks()
.AddSqlServer(
    connectionString:"Server=db11596.public.databaseasp.net; Database=db11596; User Id=db11596; Password=i#5G!Tc2p6J+; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;",
    healthQuery:"select 1;",
    name:"Sql Server",
    tags:new[]{"db","sql"})
.AddRedis(
    redisConnectionString:"redis-11810.c300.eu-central-1-1.ec2.redns.redis-cloud.com:11810",
    name:"Redis"
    )
.AddCheck("Custom Check", () =>
{

    // saniye değerini 2 ye göre mod alıp random healty yada unhealty mesajı verelim
    return DateTime.Now.Second % 2 == 0 ?
     HealthCheckResult.Healthy("Custom Health succeeded") :
     HealthCheckResult.Unhealthy("Custon HealthCheck failed");

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapControllers();

app.MapHealthChecks("/healty");
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
