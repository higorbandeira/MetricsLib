using System.Text;

public class PrometheusExporter : IExporter
{
    public string Export(IEnumerable<IMetrics> metrics)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var metric in metrics)
        {
            switch (metric)
            {
                case ICounter counter:
                    sb.AppendLine($"# HELP {counter.Name} {counter.Description}");
                    sb.AppendLine($"# TYPE {counter.Name} counter");
                    sb.AppendLine($"{counter.Name} {counter.Value}");
                    break;
                case IGauge gauge:
                    sb.AppendLine($"# HELP {gauge.Name} {gauge.Description}");
                    sb.AppendLine($"# TYPE {gauge.Name} gauge");
                    sb.AppendLine($"{gauge.Name} {gauge.Value}");
                    break;
                case IHistogram histogram:
                    sb.AppendLine($"# HELP {histogram.Name} {histogram.Description}");
                    sb.AppendLine($"# TYPE {histogram.Name} histogram");
                    foreach (var value in histogram.Values)
                    {
                        sb.AppendLine($"{histogram.Name}_bucket {value}");
                    }
                    break;
                default:
                    throw new NotSupportedException($"Metric type {metric.GetType().Name} is not supported.");
            }
        }
        
        return sb.ToString();
    }
}