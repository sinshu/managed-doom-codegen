using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class GenPlayerActionStubs
{
    private static readonly string srcFile = @"orig\info.c";

    public static void Run()
    {
        Console.Write("GenPlayerActionStubs...");

        using (var writer = new StreamWriter("PlayerActions.cs"))
        {
            foreach (var value in Read().Select(x => x.Replace("void  A_", "").Replace("void A_", "").Replace("();", "")))
            {
                writer.WriteLine("        public static void " + value + "(Player player, PlayerSprite psp)");
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
                   .SkipWhile(line => !line.Contains("void  A_Light0();"))
                   .TakeWhile(line => !line.Contains("void A_BFGSpray();"))
                   .Select(line => line.Trim());
    }
}
