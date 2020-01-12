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
            var count = 0;
            foreach (var value in Read())
            {
                writer.WriteLine("            " + ToStateDef(value, count < 100));
                count++;
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

    private static string ToStateDef(string value, bool isPlayerAction)
    {
        var body = getBody.Match(value).Groups[1].Value;
        var comment = getComment.Match(value).Groups[1].Value;

        var split = body.Split(',');

        var sprite = "Sprite." + split[0].Substring(4);
        var frame = split[1];
        var tics = split[2];
        string action;
        if (split[3] == "{NULL}")
        {
            action = "null";
        }
        else if (isPlayerAction)
        {
            action = "PlayerActions." + split[3].Replace("{A_", "").Replace("}", "");
        }
        else
        {
            action = "MobjActions." + split[3].Replace("{A_", "").Replace("}", "");
        }
        var next = CToCs.State(split[4]);
        var misc1 = split[5];
        var misc2 = split[6];

        if (isPlayerAction)
        {
            return "new StateDef("
                + sprite + ", "
                + frame + ", "
                + tics + ", "
                + action + ", "
                + "null, "
                + "State." + next + ", "
                + misc1 + ", "
                + misc2 + "), // " + "State." + CToCs.State(comment);
        }
        else
        {
            return "new StateDef("
                + sprite + ", "
                + frame + ", "
                + tics + ", "
                + "null, "
                + action + ", "
                + "State." + next + ", "
                + misc1 + ", "
                + misc2 + "), // " + "State." + CToCs.State(comment);
        }
    }
}
