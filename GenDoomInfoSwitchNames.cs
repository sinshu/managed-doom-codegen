using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenDoomInfoSwitchNames
{
    private static readonly string srcFile = @"orig\p_switch.c";

    public static void Run()
    {
        Console.Write("GenDoomInfoSwitchNames...");

        using (var writer = new StreamWriter("DoomInfo.SwitchNames.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public static partial class DoomInfo");
            writer.WriteLine("    {");
            writer.WriteLine("        public static readonly Tuple<DoomString, DoomString>[] SwitchNames = new Tuple<DoomString, DoomString>[]");
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
            writer.WriteLine("    }");
            writer.WriteLine("}");
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
