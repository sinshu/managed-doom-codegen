using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class GenDoomInfoSpriteNames
{
    private static readonly string srcFile = @"orig\info.c";

    private static readonly Regex regexSpriteName = new Regex(@"\""\w\w\w\w\""");

    public static void Run()
    {
        Console.Write("GenDoomInfoSpriteNames...");

        using (var writer = new StreamWriter("DoomInfo.SpriteNames.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public static partial class DoomInfo");
            writer.WriteLine("    {");
            writer.WriteLine("        public static readonly DoomString[] SpriteNames = new DoomString[]");
            writer.WriteLine("        {");

            var values = Read().ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];

                writer.Write("            new DoomString(\"" + value + "\")");

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
        foreach (var line in File.ReadLines(srcFile))
        {
            var matches = regexSpriteName.Matches(line);
            foreach (var match in matches)
            {
                yield return match.ToString().Substring(1, 4);
            }

        }
    }
}
