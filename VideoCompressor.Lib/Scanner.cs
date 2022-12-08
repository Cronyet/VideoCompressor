namespace VideoCompressor.Lib;

public class Scanner
{
    public Scanner(string path)
    {
        RootPath = Path.GetFullPath(path);
    }

    public string RootPath { get; }

    public ScanResult Scan()
    {
        List<string> files = new();
        Trie extensions = new();

        Queue<DirectoryInfo> dirsToScan = new();
        dirsToScan.Enqueue(new DirectoryInfo(RootPath));

        while (dirsToScan.Count > 0)
        {
            var dir = dirsToScan.Dequeue();
            foreach (var subdivide in dir.GetDirectories())
                dirsToScan.Enqueue(subdivide);
            foreach (var file in dir.GetFiles())
            {
                var relatePath = Path.GetRelativePath(RootPath, file.FullName);
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