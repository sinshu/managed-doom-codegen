﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenDoomKeysEx
{
    private static readonly string srcFile = @"orig\KeyBoard.cs";

    public static void Run()
    {
        Console.Write("GenDoomKeysEx...");

        var values = Read().ToArray();

        using (var writer = new StreamWriter("DoomKeysEx.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public static class DoomKeysEx");
            writer.WriteLine("    {");
            writer.WriteLine("        public static string ToString(DoomKeys key)");
            writer.WriteLine("        {");
            writer.WriteLine("            switch (key)");
            writer.WriteLine("            {");

            for (var i = 1; i < values.Length; i++)
            {
                writer.WriteLine("                case DoomKeys." + values[i] + ":");
                writer.WriteLine("                    return \"" + values[i].ToLower() + "\";");
            }

            writer.WriteLine("                default:");
            writer.WriteLine("                    return \"unknown\";");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine();

            writer.WriteLine("        public static DoomKeys Parse(string value)");
            writer.WriteLine("        {");
            writer.WriteLine("            switch (value)");
            writer.WriteLine("            {");

            for (var i = 0; i < values.Length; i++)
            {
                if (values[i].Contains("Unknown"))
                {
                    continue;
                }

                if (values[i].Contains("A = 0"))
                {
                    writer.WriteLine("                case \"a\":");
                    writer.WriteLine("                    return DoomKeys.A;");
                    continue;
                }

                writer.WriteLine("                case \"" + values[i].ToLower() + "\":");
                writer.WriteLine("                    return DoomKeys." + values[i] + ";");
            }

            writer.WriteLine("                default:");
            writer.WriteLine("                    return DoomKeys.Unknown;");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
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
                   .Select(line => line.Split('=')[0].Trim())
                   .Where(line => !line.StartsWith("///") && line.Length > 0)
                   .Select(line => line.Replace(",", ""));
    }
}
