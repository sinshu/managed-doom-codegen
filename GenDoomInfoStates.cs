using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class GenDoomInfoStates
{
    private static readonly string srcFile = @"orig\info.c";

    private static readonly Regex getBody = new Regex(@"\{(.+)\}");
    private static readonly Regex getComment = new Regex(@"\/\/\s+(.+)");

    public static void Run()
    {
        Console.Write("GenDoomInfoStates...");

        using (var writer = new StreamWriter("DoomInfo.States.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public static partial class DoomInfo");
            writer.WriteLine("    {");
            writer.WriteLine("        private static PlayerActions pa = new PlayerActions();");
            writer.WriteLine("        private static MobjActions ma = new MobjActions();");
            writer.WriteLine();
            writer.WriteLine("        public static readonly StateDef[] States = new StateDef[]");
            writer.WriteLine("        {");

            var values = Read().ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];

                writer.WriteLine("            " + ToStateDef(i, value, i < 100, i == values.Length - 1));
            }

            writer.WriteLine("        };");
            writer.WriteLine("    }");
            writer.WriteLine("}");
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

    private static string ToStateDef(int number, string value, bool isPlayerAction, bool last)
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
            action = "pa." + split[3].Replace("{A_", "").Replace("}", "");
        }
        else
        {
            action = "ma." + split[3].Replace("{A_", "").Replace("}", "");
        }
        var next = CToCs.State(split[4]);
        var misc1 = split[5];
        var misc2 = split[6];

        if (isPlayerAction)
        {
            return "new StateDef("
                + number + ", "
                + sprite + ", "
                + frame + ", "
                + tics + ", "
                + action + ", "
                + "null, "
                + "State." + next + ", "
                + misc1 + ", "
                + misc2 + "), // " + "State." + CToCs.State(comment);
        }
        else if (!last)
        {
            return "new StateDef("
                + number + ", "
                + sprite + ", "
                + frame + ", "
                + tics + ", "
                + "null, "
                + action + ", "
                + "State." + next + ", "
                + misc1 + ", "
                + misc2 + "), // " + "State." + CToCs.State(comment);
        }
        else
        {
            return "new StateDef("
                + number + ", "
                + sprite + ", "
                + frame + ", "
                + tics + ", "
                + "null, "
                + action + ", "
                + "State." + next + ", "
                + misc1 + ", "
                + misc2 + ") // " + "State." + CToCs.State(comment);
        }
    }
}
