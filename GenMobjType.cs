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
            foreach (var value in Read())
            {
                writer.Write("        ");

                if (value != "NUMMOBJTYPES")
                {
                    writer.WriteLine(CToCs.MobjType(value) + ",");
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
                   .SkipWhile(line => line.Trim() != "MT_PLAYER,")
                   .TakeWhile(line => line.Trim() != "")
                   .Select(line => line.Trim().Replace(",", ""));
    }
}
