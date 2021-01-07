using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenDoomInfoBgmNames
{
    private static readonly string srcFile = @"orig\sounds.h";

    public static void Run()
    {
        Console.Write("GenDoomInfoBgmNames...");

        using (var writer = new StreamWriter("DoomInfo.BgmNames.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public static partial class DoomInfo");
            writer.WriteLine("    {");
            writer.WriteLine("        public static readonly DoomString[] BgmNames = new DoomString[]");
            writer.WriteLine("        {");

            var values = Read().ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];

                writer.Write("            new DoomString(\"" + CToCs.Bgm(value).ToLower() + "\")");

                if (i != values.Length - 1)
                {
                    writer.WriteLine(",");
                }
                else
                {
                    writer.WriteLine();
                }
            }

            writer.WriteLine("        };");
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        Console.WriteLine("OK!");
    }

    private static IEnumerable<string> Read()
    {
        return File.ReadLines(srcFile)
                   .SkipWhile(line => line.Trim() != "mus_None,")
                   .TakeWhile(line => line.Trim() != "NUMMUSIC")
                   .Select(line => line.Trim().Replace(",", ""));
    }
}
