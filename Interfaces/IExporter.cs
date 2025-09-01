public interface IExporter
{
    string Export(IEnumerable<IMetrics> metrics);
}