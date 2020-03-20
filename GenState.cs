using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenState
{
    private static readonly string srcFile = @"orig\info.h";

    public static void Run()
    {
        Console.Write("GenState...");

        using (var writer = new StreamWriter("State.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public enum State");
            writer.WriteLine("    {");

            var values = Read().ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];

                writer.Write("        " + CToCs.State(value));

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
                   .SkipWhile(line => line.Trim() != "S_NULL,")
                   .TakeWhile(line => line.Trim() != "NUMSTATES")
                   .Select(line => line.Trim().Replace(",", ""));
    }
}
