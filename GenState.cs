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
            foreach (var value in Read())
            {
                writer.Write("        ");

                if (value != "NUMSTATES")
                {
                    writer.WriteLine(CToCs.State(value) + ",");
                }
                else
                {
                    //writer.WriteLine("Count");
                }
            }
        }

        Console.WriteLine("OK!");
    }

    private static IEnumerable<string> Read()
    {
        return File.ReadLines(srcFile)
                   .SkipWhile(line => line.Trim() != "S_NULL,")
                   .TakeWhile(line => line.Trim() != "} statenum_t;")
                   .Select(line => line.Trim().Replace(",", ""));
    }
}
