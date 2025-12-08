using System.Numerics;

namespace AOC2025.Extensions;

public class RingBufferFloat
{
    private readonly float[] _buffer;
    private int _head; // Index for the next write position
    private int _tail; // Index for the oldest element (read position)
    private int _count;

    public int Capacity => _buffer.Length;
    public int Count => _count;

    public RingBufferFloat(int capacity)
    {
        _buffer = new float[capacity];
        _head = 0;
        _tail = 0;
        _count = 0;
    }

    public void Write(float item)
    {
        _buffer[_head] = item;
        _head = (_head + 1) % Capacity;

        if (_count == Capacity)
        {
            // If the buffer was full, the tail must advance to discard the overwritten item
            _tail = (_tail + 1) % Capacity;
        }
        else
        {
            _count++;
        }
    }

    public float Read()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("Buffer is empty.");
        }

        float item = _buffer[_tail];
        _tail = (_tail + 1) % Capacity;
        _count--;
        return item;
    }

    public float Mean()
    {
        return _buffer.Sum() / (float) _count;
    }
}