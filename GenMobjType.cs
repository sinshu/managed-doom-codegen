using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenMobjType
{
    private static readonly string srcFile = @"orig\info.h";

    public static void Run()
    {
        Console.Write("GenMobjType...");

        using (var writer = new StreamWriter("MobjType.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public enum MobjType");
            writer.WriteLine("    {");

            var values = Read().ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];

                writer.Write("        " + CToCs.MobjType(value));

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
                   .SkipWhile(line => line.Trim() != "MT_PLAYER,")
                   .TakeWhile(line => line.Trim() != "NUMMOBJTYPES")
                   .Select(line => line.Trim().Replace(",", ""));
    }
}
