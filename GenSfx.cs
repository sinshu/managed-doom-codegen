using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenSfx
{
    private static readonly string srcFile = @"orig\sounds.h";

    public static void Run()
    {
        Console.Write("GenSfx...");

        using (var writer = new StreamWriter("Sfx.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public enum Sfx");
            writer.WriteLine("    {");

            var values = Read().ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];

                writer.Write("        " + CToCs.Sfx(value));

                if (i != values.Length - 1)
                {
                    writer.WriteLine(",");
                }
                else
                {
                    writer.WriteLine();
                }
            }

            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        Console.WriteLine("OK!");
    }

    private static IEnumerable<string> Read()
    {
        return File.ReadLines(srcFile)
                   .SkipWhile(line => line.Trim() != "sfx_None,")
                   .TakeWhile(line => line.Trim() != "NUMSFX")
                   .Select(line => line.Trim().Replace(",", ""));
    }
}
