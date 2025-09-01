using System.Text.Json;

public class DatadogExporter : IExporter
{
    private readonly string _apiKey;
    private readonly string _endpoint;
    private readonly HttpClient _httpClient;

    public DatadogExporter(string apiKey, string endpoint = "https://api.datadoghq.com")
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_endpoint)
        };
        _httpClient.DefaultRequestHeaders.Add("API_KEY", _apiKey);
    }

    //Interface needs a synchronous method
    public string Export(IEnumerable<IMetrics> metrics)
    {
        List<object> payload = new List<object>();

        foreach (var metric in metrics)
        {
            switch (metric)
            {
                case Gauge gauge:
                    payload.Add(new
                    {
                        metric = gauge.Name,
                        points = new[] { new[] { DateTimeOffset.UtcNow.ToUnixTimeSeconds(), gauge.Value } },
                        type = "gauge",
                        tags = ConvertTags(gauge.Tags)
                    });
                    break;

                case Counter counter:
                    payload.Add(new
                    {
                        metric = counter.Name,
                        points = new[] { new[] { DateTimeOffset.UtcNow.ToUnixTimeSeconds(), counter.Value } },
                        type = "count",
                        tags = ConvertTags(counter.Tags)
                    });
                    break;

                case Histogram histogram:
                    foreach (var value in histogram.Values)
                    {
                        payload.Add(new
                        {
                            metric = histogram.Name,
                            points = new[] { new[] { DateTimeOffset.UtcNow.ToUnixTimeSeconds(), value } },
                            type = "histogram",
                            tags = ConvertTags(histogram.Tags)
                        });
                    }
                    break;

                default:
                    throw new NotSupportedException($"Metric type {metric.GetType().Name} is not supported.");
            }
        }

        var content = new StringContent(JsonSerializer.Serialize(new { series = payload }), System.Text.Encoding.UTF8, "application/json");
        var response = _httpClient.PostAsync("/series", content).Result;

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = response.Content.ReadAsStringAsync().Result;
            throw new Exception($"Failed to send metrics to Datadog: {response.StatusCode} - {responseBody}");
        }

        return response.Content.ReadAsStringAsync().Result;
    }

    private static string[] ConvertTags(IDictionary<string, string> tags)
    {
        List<string> result = new List<string>();
        foreach (var tag in tags)
        {
            result.Add($"{tag.Key}:{tag.Value}");
        }
        return result.ToArray();
    }
}