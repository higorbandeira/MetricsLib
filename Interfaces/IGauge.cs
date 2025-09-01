public interface IGauge : IMetrics
{
    double Value { get; }
    void Set(double value);
}