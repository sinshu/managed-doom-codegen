using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class GenMobjInfoList
{
    private static readonly string srcFile = @"orig\info.c";

    private static readonly Regex getMobjType = new Regex(@"MT_.+");
    private static readonly Regex getValue = new Regex(@"");

    public static void Run()
    {
        Console.Write("GenMobjInfoList...");

        using (var writer = new StreamWriter("MobjInfoList.cs"))
        {
            foreach (var buf in Read())
            {
                var mobjType = "MobjType." + CToCs.MobjType(getMobjType.Match(buf[0]).Value);
                var doomEdNum = buf[1].Trim().Split(',')[0];
                var spawnState = "State." + CToCs.State(buf[2].Trim().Split(',')[0]);
                var spawnHealth = buf[3].Trim().Split(',')[0];
                var seeState = "State." + CToCs.State(buf[4].Trim().Split(',')[0]);
                var seeSound = "Sfx." + CToCs.Sfx(buf[5].Trim().Split(',')[0]);
                var reactionTime = buf[6].Trim().Split(',')[0];
                var attackSound = "Sfx." + CToCs.Sfx(buf[7].Trim().Split(',')[0]);
                var painState = "State." + CToCs.State(buf[8].Trim().Split(',')[0]);
                var painChance = buf[9].Trim().Split(',')[0];
                var painSound = "Sfx." + CToCs.Sfx(buf[10].Trim().Split(',')[0]);
                var meleeState = "State." + CToCs.State(buf[11].Trim().Split(',')[0]);
                var missileState = "State." + CToCs.State(buf[12].Trim().Split(',')[0]);
                var deathState = "State." + CToCs.State(buf[13].Trim().Split(',')[0]);
                var xdeathState = "State." + CToCs.State(buf[14].Trim().Split(',')[0]);
                var deathSound = "Sfx." + CToCs.Sfx(buf[15].Trim().Split(',')[0]);
                var speed = Speed(buf[16].Trim().Split(',')[0]);
                var radius = ToFixed(buf[17].Trim().Split(',')[0]);
                var height = ToFixed(buf[18].Trim().Split(',')[0]);
                var mass = buf[19].Trim().Split(',')[0];
                var damage = buf[20].Trim().Split(',')[0];
                var activeSound = "Sfx." + CToCs.Sfx(buf[21].Trim().Split(',')[0]);
                var flags = buf[22].Trim().Split(',')[0];
                flags = string.Join(" | ", flags.Split('|').Select(x => CToCs.MobjFlagF(x)));
                var raiseState = "State." + CToCs.State(buf[23].Split('/')[0].Trim());

                writer.WriteLine("            new MobjInfo( // " + mobjType);
                writer.WriteLine("                " + doomEdNum + ", // doomEdNum");
                writer.WriteLine("                " + spawnState + ", // spawnState");
                writer.WriteLine("                " + spawnHealth + ", // spawnHealth");
                writer.WriteLine("                " + seeState + ", // seeState");
                writer.WriteLine("                " + seeSound + ", // seeSound");
                writer.WriteLine("                " + reactionTime + ", // reactionTime");
                writer.WriteLine("                " + attackSound + ", // attackSound");
                writer.WriteLine("                " + painState + ", // painState");
                writer.WriteLine("                " + painChance + ", // painChance");
                writer.WriteLine("                " + painSound + ", // painSound");
                writer.WriteLine("                " + meleeState + ", // meleeState");
                writer.WriteLine("                " + missileState + ", // missileState");
                writer.WriteLine("                " + deathState + ", // deathState");
                writer.WriteLine("                " + xdeathState + ", // xdeathState");
                writer.WriteLine("                " + deathSound + ", // deathSound");
                writer.WriteLine("                " + speed + ", // speed");
                writer.WriteLine("                " + radius + ", // radius");
                writer.WriteLine("                " + height + ", // height");
                writer.WriteLine("                " + mass + ", // mass");
                writer.WriteLine("                " + damage + ", // damage");
                writer.WriteLine("                " + activeSound + ", // activeSound");
                writer.WriteLine("                " + flags + ", // flags");
                writer.WriteLine("                " + raiseState + "), // raiseState");
                writer.WriteLine("");
            }
        }

        Console.WriteLine("OK!");
    }

    private static string ToFixed(string value)
    {
        if (value.Contains("FRACUNIT"))
        {
            return "Fixed.FromInt(" + value.Split('*')[0].Trim() + ")";
        }
        else
        {
            return value;
        }
    }

    private static string Speed(string value)
    {
        if (value.EndsWith("FRACUNIT"))
        {
            return value.Split('*')[0].Trim() + " * Fixed.FracUnit";
        }
        else
        {
            return value;
        }
    }

    private static IEnumerable<IList<string>> Read()
    {
        return File.ReadLines(srcFile)
                   .SkipWhile(line => !line.Contains("mobjinfo_t mobjinfo[NUMMOBJTYPES] = {"))
                   .Skip(2)
                   .TakeWhile(line => line.Trim() != "};")
                   .Buffer(26)
                   .Where(buf => buf.Count == 26);
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
