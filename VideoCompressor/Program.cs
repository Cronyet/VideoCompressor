using System.Diagnostics;
using System.Text.RegularExpressions;
using VideoCompressor.Lib;

string Ask(string tip)
{
    Console.Write(tip);
    return Console.ReadLine() ?? string.Empty;
}

string AskPath(string tip) => Path.GetFullPath(Ask(tip));

bool? AskYesOrNo(string tip, string yes, string no)
{
    var rst = Ask(tip);
    if (rst.Equals(yes)) return true;
    if (rst.Equals(no)) return false;
    return null;
}

bool AskSureYesOrNo(string tip, string yes, string no)
{
    while (true)
    {
        var rst = AskYesOrNo(tip, yes, no);
        if (rst != null) return (bool)rst;
    }
}

void Debug(string obj) => Console.WriteLine($">>> {obj}");

Console.WriteLine("VideoCompressor v0.1.0");
Console.WriteLine("Copyright (c) Dynesshely 2022");

var scanner = new Scanner(AskPath("Input medias root path: "));
Console.WriteLine("Scanning ...");
var scanResult = scanner.Scan();

if (AskSureYesOrNo("Show Extensions? (y/n): ", "y", "n"))
{
    Console.WriteLine("Extensions: ");
    var index = 0;
    foreach (var extension in scanResult.Extensions)
    {
        ++index;
        Console.Write($"[.{extension}]{(index == scanResult.Extensions.Length ? "\n" : ", ")}");
    }
}

if (AskSureYesOrNo("Show Files? (y/n): ", "y", "n"))
{
    Console.WriteLine("Files: ");
    foreach (var file in scanResult.Files)
        Console.WriteLine(file);
}

Console.WriteLine($"Replace properties: \n" +
                  $"\t{{file}} -> File Path\n" +
                  $"\t{{ext}} -> Extension with dot\n" +
                  $"\t{{file.rep src, to}} -> File Path and Replace src with to\n");
var template = Ask("Input command template: ");
Console.WriteLine("Executing ...");

foreach (var file in scanResult.Files)
{
    var cmd = template;
    cmd = cmd.Replace("{file}", $"\"{file}\"");
    cmd = cmd.Replace("{ext}", Path.GetExtension(file));
    MatchEvaluator evaluator = new(
        match => $"{file.Replace(match.Groups[1].Value, match.Groups[2].Value)}"
    );
    cmd = new Regex(RegexStrings.FileReplace).Replace(cmd, evaluator);

    Console.WriteLine($"Executing: {cmd}");
    Process.Start(cmd);
}