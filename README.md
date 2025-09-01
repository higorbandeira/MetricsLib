# MetricsLib

MetricsLib is a .NET library for collecting and exporting application metrics to popular analytics platforms.

## Features

- **Metrics Types:** Counter, Gauge, Histogram
- **Exporters:** Datadog, Prometheus
- **Tagging:** Add custom tags to metrics
- **Central Management:** Manage all metrics via `MetricsManager`

## Getting Started

### Installation

Add the source files to your project or reference the compiled DLL.

### Usage

#### 1. Create Metrics

Use [`Service.MetricsManager`](Service/MetricsManager.cs) to create and manage metrics:

```csharp
var manager = new MetricsManager();
var counter = manager.CreateMetrics<ICounter>("requests_total", "Total requests");
counter.Increment();

var gauge = manager.CreateMetrics<IGauge>("cpu_usage", "CPU Usage");
gauge.Set(42.5);

var histogram = manager.CreateMetrics<IHistogram>("response_time", "Response Time");
histogram.Observe(120.5);
```
