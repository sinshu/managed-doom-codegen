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
                    writer.WriteLine(ToCsStyle(value) + ",");
                }
                else
                {
                    writer.WriteLine("Count");
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

    private static string ToCsStyle(string value)
    {
        var split = value.Split('_');
        var sb = new StringBuilder();
        foreach (var x in split.Skip(1))
        {
            sb.Append(ToUpperFirst(x));
        }
        for (var i = 0; i < sb.Length - 1; i++)
        {
            if ('0' <= sb[i] && sb[i] <= '9' && 'a' <= sb[i + 1] && sb[i + 1] <= 'z')
            {
                sb[i + 1] = (char)(sb[i + 1] - 'a' + 'A');
            }
        }
        return sb.ToString();
    }

    private static string ToUpperFirst(string value)
    {
        var upper = value.ToUpper();
        var lower = value.ToLower();
        return upper.Substring(0, 1) + lower.Substring(1);
    }
}
