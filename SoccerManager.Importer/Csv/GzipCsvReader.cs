using System.Globalization;
using System.IO.Compression;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace SoccerManager.Importer.Csv;

/// <summary>
/// Opens a gzip-compressed CSV stream for header-based reading.
/// </summary>
public static class GzipCsvReader
{
    /// <summary>
    /// Wraps the given stream in gzip decompression and opens it as a <see cref="CsvReader"/>, ready to read its
    /// header row.
    /// </summary>
    /// <param name="stream">
    /// The gzip-compressed stream to read from. Disposing the returned reader disposes this stream too.
    /// </param>
    /// <returns>
    /// A reader over the decompressed stream, using the invariant culture. Fields must be accessed by column name:
    /// the live dataset files' column order does not match the dataset's own generating code.
    /// </returns>
    public static CsvReader Open(Stream stream)
    {
        var gzipStream = new GZipStream(stream, CompressionMode.Decompress);
        var streamReader = new StreamReader(gzipStream, Encoding.UTF8);
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture);

        return new CsvReader(streamReader, configuration);
    }
}
