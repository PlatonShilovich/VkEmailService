using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/{service}/{*path}")]
public class ProxyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Dictionary<string, string> _serviceUrls = new()
    {
        { "usersegmentation", "http://usersegmentation" },
        { "abtesting", "http://abtesting" },
        { "emailcampaign", "http://emailcampaign" },
        { "analytics", "http://analytics" }
    };

    public ProxyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet, HttpPost, HttpPut, HttpDelete, HttpPatch]
    [AllowAnonymous]
    public async Task<IActionResult> Proxy(string service, string path)
    {
        if (!_serviceUrls.TryGetValue(service.ToLower(), out var baseUrl))
            return NotFound($"Неизвестный сервис: {service}");
        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage
        {
            Method = new HttpMethod(Request.Method),
            RequestUri = new Uri($"{baseUrl}/api/{path}{Request.QueryString}")
        };
        if (Request.ContentLength > 0)
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            request.Content = new StringContent(body, System.Text.Encoding.UTF8, Request.ContentType ?? "application/json");
        }
        foreach (var header in Request.Headers)
            request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return new ContentResult
        {
            StatusCode = (int)response.StatusCode,
            Content = content,
            ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/json"
        };
    }
}