using System.Security.Cryptography;

namespace SoccerManager.Importer.Source;

/// <summary>
/// A read-only, forward-only wrapper over another stream that counts the bytes read from it and feeds them into a
/// running SHA-256 hash, so a table's size and checksum can be computed while it streams through, without ever
/// buffering the whole table in memory or on disk first.
/// </summary>
public sealed class HashingReadStream : Stream
{
    private readonly Stream _inner;
    private readonly IncrementalHash _hash;

    /// <summary>
    /// Initializes a new instance of the <see cref="HashingReadStream"/> class.
    /// </summary>
    /// <param name="inner">The stream to read from and hash. This stream takes ownership and disposes it.</param>
    public HashingReadStream(Stream inner)
    {
        _inner = inner;
        _hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
    }

    /// <summary>The number of bytes read so far.</summary>
    public long BytesRead { get; private set; }

    /// <inheritdoc />
    public override bool CanRead => true;

    /// <inheritdoc />
    public override bool CanSeek => false;

    /// <inheritdoc />
    public override bool CanWrite => false;

    /// <inheritdoc />
    public override long Length => throw new NotSupportedException("The underlying response stream has no known length.");

    /// <inheritdoc />
    public override long Position
    {
        get => throw new NotSupportedException("This stream is forward-only.");
        set => throw new NotSupportedException("This stream is forward-only.");
    }

    /// <summary>
    /// Finishes the running hash and returns it as lowercase hex. Only meaningful once the stream has been read to
    /// its end; calling it again afterwards starts a fresh, empty hash.
    /// </summary>
    /// <returns>The SHA-256 hash of every byte read since the last call, as lowercase hex.</returns>
    public string GetHexHash()
    {
        return Convert.ToHexString(_hash.GetHashAndReset()).ToLowerInvariant();
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
    {
        var read = _inner.Read(buffer, offset, count);
        Track(buffer.AsSpan(offset, read));

        return read;
    }

    /// <inheritdoc />
    public override int Read(Span<byte> buffer)
    {
        var read = _inner.Read(buffer);
        Track(buffer[..read]);

        return read;
    }

    /// <inheritdoc />
    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        var read = await _inner.ReadAsync(buffer.AsMemory(offset, count), cancellationToken).ConfigureAwait(false);
        Track(buffer.AsSpan(offset, read));

        return read;
    }

    /// <inheritdoc />
    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        var read = await _inner.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
        Track(buffer.Span[..read]);

        return read;
    }

    /// <inheritdoc />
    public override void Flush()
    {
        // Nothing to flush: this stream is read-only.
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException("This stream is forward-only.");
    }

    /// <inheritdoc />
    public override void SetLength(long value)
    {
        throw new NotSupportedException("This stream is read-only.");
    }

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException("This stream is read-only.");
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _inner.Dispose();
            _hash.Dispose();
        }

        base.Dispose(disposing);
    }

    private void Track(ReadOnlySpan<byte> data)
    {
        if (data.Length == 0)
        {
            return;
        }

        _hash.AppendData(data);
        BytesRead += data.Length;
    }
}
