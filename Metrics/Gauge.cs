public class Gauge : IGauge
{
    public string Name { get; }
    public string Description { get; }
    public IDictionary<string, string> Tags { get; }
    public double Value { get; private set; }

    public Gauge(string name, string description = "", IDictionary<string, string> tags = null)
    {
        this.Name = name;
        this.Description = description;
        this.Tags = tags ?? new Dictionary<string, string>();
    }

    public void Set(double value)
    {
        this.Value = value;
    }
}