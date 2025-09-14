namespace CleanJam.Application.Export
{


    public interface IExportServiceFactory
    {
        IExportService GetExporter(EnumExportFormat format);
    }

    public class ExportServiceFactory : IExportServiceFactory
    {
        private readonly Dictionary<EnumExportFormat, IExportService> _exporters;

        public ExportServiceFactory(IEnumerable<IExportService> exporters)
        {
            // Build lookup dictionary (case-insensitive)
            _exporters = exporters.ToDictionary(e => e.Format);
        }

        public IExportService GetExporter(EnumExportFormat format)
        {
            if (_exporters.TryGetValue(format, out var exporter))
                return exporter;

            throw new ArgumentException($"Unsupported format '{format}'. Supported: {string.Join(", ", _exporters.Keys)}");
        }
    }
}
