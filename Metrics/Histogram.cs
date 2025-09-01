public class Histogram : IHistogram
{
    public string Name { get; }
    public string Description { get; }
    public IDictionary<string, string> Tags { get; }
    private readonly List<double> _values = new();
    public IEnumerable<double> Values => this._values;

    public Histogram(string name, string description = "", IDictionary<string, string>? tags = null)
    {
        this.Name = name;
        this.Description = description;
        this.Tags = tags ?? new Dictionary<string, string>();
    }

    public void Observe(double value)
    {
        this._values.Append(value);
    }
}