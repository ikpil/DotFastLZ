using System;
using System.Buffers;
using System.Diagnostics;

namespace DotFastLZ.Benchmark;

sealed class FixedArrayBufferWriter<T> : IBufferWriter<T>
{
    private const int ArrayMaxLength = 0x7FFFFFC7;
    private const int DefaultInitialBufferSize = 256;

    private readonly T[] _buffer;
    private int _index;

    public FixedArrayBufferWriter(T[] buffer)
    {
        if (buffer.Length <= 0)
            throw new ArgumentException(null, nameof(buffer));

        _buffer = buffer;
        _index = 0;
    }

    public ReadOnlyMemory<T> WrittenMemory => _buffer.AsMemory(0, _index);
    public ReadOnlySpan<T> WrittenSpan => _buffer.AsSpan(0, _index);
    public int WrittenCount => _index;
    public int Capacity => _buffer.Length;
    public int FreeCapacity => _buffer.Length - _index;

    public void Clear()
    {
        Debug.Assert(_buffer.Length >= _index);
        _buffer.AsSpan(0, _index).Clear();
        _index = 0;
    }

    public void Advance(int count)
    {
        if (count < 0)
            throw new ArgumentException(null, nameof(count));

        if (_index > _buffer.Length - count)
            ThrowInvalidOperationException_AdvancedTooFar(_buffer.Length);

        _index += count;
    }

    public Memory<T> GetMemory(int sizeHint = 0)
    {
        CheckAndResizeBuffer(sizeHint);
        Debug.Assert(_buffer.Length > _index);
        return _buffer.AsMemory(_index);
    }

    public Span<T> GetSpan(int sizeHint = 0)
    {
        CheckAndResizeBuffer(sizeHint);
        Debug.Assert(_buffer.Length > _index);
        return _buffer.AsSpan(_index);
    }

    private void CheckAndResizeBuffer(int sizeHint)
    {
        if (sizeHint < 0)
            throw new ArgumentException(nameof(sizeHint));

        if (sizeHint == 0)
        {
            sizeHint = 1;
        }

        if (sizeHint > FreeCapacity)
        {
            // Attempt to grow to ArrayMaxLength.
            int currentLength = _buffer.Length;
            uint needed = (uint)(currentLength - FreeCapacity + sizeHint);
            ThrowOutOfMemoryException(needed);
        }
    }

    private static void ThrowInvalidOperationException_AdvancedTooFar(int capacity)
    {
        throw new InvalidOperationException($"BufferWriterAdvancedTooFar, {capacity})");
    }

    private static void ThrowOutOfMemoryException(uint capacity)
    {
        throw new OutOfMemoryException($"BufferMaximumSizeExceeded, {capacity})");
    }
}