public class MetricsManager
{
    private readonly Dictionary<string, IMetrics> _metrics = new();

    public T CreateMetrics<T>(string name, string description, IDictionary<string, string> tags = null) where T : IMetrics
    {
        if (_metrics.ContainsKey(name))
        {
            throw new InvalidOperationException($"Metrics with name {name} already exists.");
        }

        IMetrics metrics = typeof(T) switch
        {
            var t when t == typeof(ICounter) => new Counter(name, description, tags),
            var t when t == typeof(IGauge) => new Gauge(name, description, tags),
            var t when t == typeof(IHistogram) => new Histogram(name, description, tags),
            _ => throw new NotSupportedException($"Metrics type {typeof(T).Name} is not supported.")
        };

        _metrics[name] = metrics;
        return (T)metrics;
    }

    public IEnumerable<IMetrics> GetAllMetrics() => _metrics.Values;
    
    public T GetMetrics<T>(string name) where T : IMetrics
    {
        if (_metrics.TryGetValue(name, out var metrics))
        {
            return (T)metrics;
        }

        throw new KeyNotFoundException($"Metrics with name {name} not found.");
    }
}