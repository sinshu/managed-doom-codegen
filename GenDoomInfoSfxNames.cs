using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenDoomInfoSfxSfxNames
{
    private static readonly string srcFile = @"orig\sounds.h";

    public static void Run()
    {
        Console.Write("GenDoomInfoSfxNames...");

        using (var writer = new StreamWriter("DoomInfo.SfxNames.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public static partial class DoomInfo");
            writer.WriteLine("    {");
            writer.WriteLine("        public static readonly DoomString[] SfxNames = new DoomString[]");
            writer.WriteLine("        {");

            var values = Read().ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];

                writer.Write("            new DoomString(\"" + CToCs.Sfx(value) + "\")");

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
                   .SkipWhile(line => line.Trim() != "sfx_None,")
                   .TakeWhile(line => line.Trim() != "NUMSFX")
                   .Select(line => line.Trim().Replace(",", ""));
    }
}
