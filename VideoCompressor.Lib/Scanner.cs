namespace VideoCompressor.Lib;

public class Scanner
{
    private readonly string _path;

    public Scanner(string path)
    {
        _path = Path.GetFullPath(path);
    }

    public ScanResult Scan()
    {
        List<string> files = new();
        Trie extensions = new();

        Queue<DirectoryInfo> dirsToScan = new();
        dirsToScan.Enqueue(new DirectoryInfo(_path));

        while (dirsToScan.Count > 0)
        {
            var dir = dirsToScan.Dequeue();
            foreach (var subdivide in dir.GetDirectories())
                dirsToScan.Enqueue(subdivide);
            foreach (var file in dir.GetFiles())
            {
                var relatePath = Path.GetRelativePath(_path, file.FullName);
                var extension = file.Extension[1..];
                files.Add(relatePath);
                extensions.Insert(extension);
#if Debug
                Console.WriteLine($">>> Trie.cs(Scan): {relatePath}, {extension}");
#endif
            }
        }

        return new ScanResult(extensions.ToStringArray(), files);
    }

    //ToDo: 异步扫描版本
    // public async Task<ScanResult> ScanAsync()
    // {
    //     List<string> _files = new();
    // }

    public class ScanResult
    {
        public ScanResult(string[] extensions, List<string> files)
        {
            Extensions = extensions;
            Files = files;
        }

        public string[] Extensions { get; }

        public List<string> Files { get; }
    }
}