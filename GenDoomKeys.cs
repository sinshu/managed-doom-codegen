using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenDoomKeys
{
    private static readonly string srcFile = @"orig\KeyBoard.cs";

    public static void Run()
    {
        Console.Write("GenDoomKeys...");

        var values = Read().ToArray();

        using (var writer = new StreamWriter("DoomKeys.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public enum DoomKeys");
            writer.WriteLine("    {");

            for (var i = 0; i < values.Length; i++)
            {
                writer.WriteLine("        " + values[i] + ",");
            }

            writer.WriteLine("        Count");
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        Console.WriteLine("OK");
    }

    private static IEnumerable<string> Read()
    {
        return File.ReadLines(srcFile)
                   .SkipWhile(line => !line.Contains("Unknown"))
                   .TakeWhile(line => !line.Contains("KeyCount"))
                   .Select(line => line.Trim())
                   .Where(line => !line.StartsWith("///") && line.Length > 0)
                   .Select(line => line.Replace(",", ""));
    }
}
