using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class GenDoomInfoMobjActions
{
    private static readonly string srcFile = @"orig\info.c";

    public static void Run()
    {
        Console.Write("GenDoomInfoMobjActions...");

        using (var writer = new StreamWriter("DoomInfo.MobjActions.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public static partial class DoomInfo");
            writer.WriteLine("    {");
            writer.WriteLine("        private class MobjActions");
            writer.WriteLine("        {");

            var values = Read().Select(x => x.Replace("void A_", "").Replace("();", "")).ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];

                writer.WriteLine("            public void " + value + "(World world, Mobj actor)");
                writer.WriteLine("            {");
                writer.WriteLine("            }");
                if (i != values.Length - 1)
                {
                    writer.WriteLine();
                }
            }

            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
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
