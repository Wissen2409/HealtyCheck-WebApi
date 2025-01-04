using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestSharp;

public class ApiHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    public ApiHealthCheck(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // interfaceden gelen metodun içerisini dolduralım!!

        RestSharp.RestClient cli = new RestSharp.RestClient("https://rickandmortyapi.com/api/character");
        RestSharp.RestRequest request = new RestSharp.RestRequest();
        request.Method = RestSharp.Method.Get;
        var response = cli.Execute(request);

        if (!string.IsNullOrEmpty(response.Content))
        {
            return HealthCheckResult.Healthy("Rick And Morty Api is Health");
        }
        else
        {
            return HealthCheckResult.Unhealthy("Rick And Morty Api is UnHealth");
        }
    }
}