using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenSfx
{
    private static readonly string srcFile = @"orig\sounds.h";

    public static void Run()
    {
        Console.Write("GenSfx...");

        using (var writer = new StreamWriter("Sfx.cs"))
        {
            foreach (var value in Read())
            {
                writer.Write("        ");

                if (value != "NUMSFX")
                {
                    writer.WriteLine(CToCs.Sfx(value) + ",");
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
                   .SkipWhile(line => line.Trim() != "sfx_None,")
                   .TakeWhile(line => line.Trim() != "} sfxenum_t;")
                   .Select(line => line.Trim().Replace(",", ""));
    }
}
