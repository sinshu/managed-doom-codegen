using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class GenMobjActionStubs
{
    private static readonly string srcFile = @"orig\info.c";

    public static void Run()
    {
        Console.Write("GenMobjActionStubs...");

        using (var writer = new StreamWriter("MobjActions.cs"))
        {
            foreach (var value in Read().Select(x => x.Replace("void A_", "").Replace("();", "")))
            {
                writer.WriteLine("        public static void " + value + "(this Mobj mobj)");
                writer.WriteLine("        {");
                writer.WriteLine("        }");
                writer.WriteLine();
            }
        }

        Console.WriteLine("OK!");
    }

    private static IEnumerable<string> Read()
    {
        return File.ReadLines(srcFile)
                   .SkipWhile(line => !line.Contains("void A_BFGSpray();"))
                   .TakeWhile(line => line.Trim().Length > 0)
                   .Select(line => line.Trim());
    }
}
