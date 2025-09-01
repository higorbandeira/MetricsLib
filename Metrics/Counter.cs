public class Counter : ICounter
{
    public string Name { get; }
    public string Description { get; }
    public IDictionary<string, string> Tags { get; }
    public long Value { get; private set; }

    public Counter(string name, string description = "", IDictionary<string, string> tags = null)
    {
        this.Name = name;
        this.Description = description;
        this.Tags = tags ?? new Dictionary<string, string>();
    }

    public void Increment(long amount = 1)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Increment amount must be non-negative.");

        this.Value += amount;
    }
}