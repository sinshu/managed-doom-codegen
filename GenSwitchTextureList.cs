using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenSwitchTextureList
{
    private static readonly string srcFile = @"orig\p_switch.c";

    public static void Run()
    {
        Console.Write("GenSwitchTextureList...");

        using (var writer = new StreamWriter("SwitchTextureList.cs"))
        {
            writer.WriteLine("        private static readonly Tuple<DoomString, DoomString>[] switchTextureList = new Tuple<DoomString, DoomString>[]");
            writer.WriteLine("        {");

            var values = Read().ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                var split = values[i].Split('\"');

                if (split.Length == 5)
                {
                    var value1 = split[1];
                    var value2 = split[3];
                    writer.Write("            Tuple.Create(new DoomString(\"" + value1 + "\"), new DoomString(\"" + value2 + "\"))");

                    if (i < values.Length - 1)
                    {
                        writer.WriteLine(",");
                    }
                    else
                    {
                        writer.WriteLine();
                    }
                }
            }

            writer.WriteLine("        };");
        }

        Console.WriteLine("OK!");
    }

    private static IEnumerable<string> Read()
    {
        return File.ReadLines(srcFile)
                   .SkipWhile(line => !line.Contains("SW1BRCOM"))
                   .TakeWhile(line => !line.Contains("\\0"))
                   .Select(line => line.Trim())
                   .Where(line => line.Length > 0);
    }
}
