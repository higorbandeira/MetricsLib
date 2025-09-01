public interface ICounter : IMetrics
{
    long Value { get; }
    void Increment(long value = 1);
}