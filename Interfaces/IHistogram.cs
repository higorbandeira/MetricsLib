public interface IHistogram : IMetrics
{
    void Observe(double value);
    IEnumerable<double> Values { get; }
}