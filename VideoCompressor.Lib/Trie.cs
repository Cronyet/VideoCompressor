using System.Runtime.Intrinsics.X86;

namespace VideoCompressor.Lib;

public struct Trie
{
    private int[,]? _next;
    private int _count = 0;
    private bool[]? _exist;

    public Trie()
    {
    }

    public void Init(int capacity = 100000)
    {
        _next = new int[capacity, 26];
        _exist = new bool[capacity];
    }

    public void Insert(string s)
    {
        if (_next == null || _exist == null) return;
        var p = 0;
        foreach (var c in s.Select(t => t - 'a'))
        {
            if (_next[p, c] == 0) _next[p, c] = ++_count;
            p = _next[p, c];
        }

        _exist[p] = true;
    }

    public bool Find(string s)
    {
        if (_next == null || _exist == null) throw new Exception("This Struct didn't Init.");
        var p = 0;
        foreach (var c in s.Select(t => t - 'a'))
        {
            if (_next[p, c] == 0) return false;
            p = _next[p, c];
        }

        return _exist[p];
    }
}