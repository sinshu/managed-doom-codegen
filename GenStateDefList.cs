using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class GenStateDefList
{
    private static readonly string srcFile = @"orig\info.c";

    private static readonly Regex getBody = new Regex(@"\{(.+)\}");
    private static readonly Regex getComment = new Regex(@"\/\/\s+(.+)");

    public static void Run()
    {
        Console.Write("GenStateDefList...");

        using (var writer = new StreamWriter("StateDefList.cs"))
        {
            foreach (var value in Read())
            {
                writer.WriteLine("            " + ToStateDef(value));
            }
        }

        Console.WriteLine("OK!");
    }

    private static IEnumerable<string> Read()
    {
        return File.ReadLines(srcFile)
                   .SkipWhile(line => !line.Contains("{SPR_TROO,0,-1,{NULL},S_NULL,0,0}"))
                   .TakeWhile(line => line.Trim() != "};")
                   .Select(line => line.Trim());
    }

    private static string ToStateDef(string value)
    {
        var body = getBody.Match(value).Groups[1].Value;
        var comment = getComment.Match(value).Groups[1].Value;

        var split = body.Split(',');

        var sprite = "Sprite." + split[0].Substring(4);
        var frame = split[1];
        var tics = split[2];
        var action = "null";
        var next = ToCsStyle(split[4]);
        var misc1 = split[5];
        var misc2 = split[6];

        return "new StateDef("
            + sprite + ", "
            + frame + ", "
            + tics + ", "
            + action + ", "
            + "State." + next + ", "
            + misc1 + ", "
            + misc2 + "), // " + "State." + ToCsStyle(comment);
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
