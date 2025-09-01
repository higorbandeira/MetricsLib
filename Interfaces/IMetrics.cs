public interface IMetrics
{
    string Name { get; }
    string Description { get; }
    IDictionary<string, string> Tags { get; }
}