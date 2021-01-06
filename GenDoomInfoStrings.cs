using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenDoomInfoStrings
{
    private static readonly string srcFile = @"orig\d_englsh.h";

    private static readonly char[] sep = new char[] { ' ', '\t' };

    private static readonly string[] ignoreList = new string[]
    {
        "__D_ENGLSH__",
        "D_DEVSTR",
        "D_CDROM",
        "DETAILHI",
        "DETAILLO",
        "HUSTR_MSGU",
        "HUSTR_CHATMACR",
        "HUSTR_TALKTOSELF",
        "HUSTR_MESSAGESENT",
        "HUSTR_PLR",
        "HUSTR_KEY"
    };

    public static void Run()
    {
        Console.Write("GenDoomInfoStrings...");

        var values = Read().ToArray();

        using (var writer = new StreamWriter("DoomInfo.Strings.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public static partial class DoomInfo");
            writer.WriteLine("    {");
            writer.WriteLine("        public static class Strings");
            writer.WriteLine("        {");

            var tuples = Read().ToArray();

            var returned = false;
            foreach (var tuple in tuples)
            {
                if (tuple.Item2.Count == 1)
                {
                    var name = tuple.Item1;
                    var body = tuple.Item2[0];
                    writer.WriteLine("            public static readonly DoomString " + name + " = new DoomString(\"" + name + "\", " + body + ");");
                    returned = false;
                }
                else
                {
                    if (!returned)
                    {
                        writer.WriteLine();
                    }
                    var name = tuple.Item1;
                    var body = tuple.Item2;
                    writer.WriteLine("            public static readonly DoomString " + name + " = new DoomString(\"" + name + "\",");
                    for (var i = 0; i < body.Count - 1; i++)
                    {
                        writer.WriteLine("                " + body[i] + " +");
                    }
                    writer.WriteLine("                " + body.Last() + ");");
                    writer.WriteLine();
                    returned = true;
                }
            }

            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        Console.WriteLine("OK");
    }

    private static IEnumerable<Tuple<string, List<string>>> Read()
    {
        string name = null;
        List<string> list = null;
        var takeNext = false;
        foreach (var value in File.ReadLines(srcFile))
        {
            var line = value.Trim();

            var ignore = false;
            foreach (var test in ignoreList)
            {
                if (line.Contains(test))
                {
                    ignore = true;
                }
            }
            if (ignore)
            {
                continue;
            }

            if (line.StartsWith("#define"))
            {
                if (name != null)
                {
                    yield return Tuple.Create(name, list);
                }

                name = GetName(line);
                list = new List<string>();
                if (line.Last() == '\\')
                {
                    takeNext = true;
                }
                else
                {
                    list.Add(GetBody(line));
                    takeNext = false;
                }
            }
            else if (takeNext)
            {
                list.Add(GetBody(line));
                if (line.Last() == '\\')
                {
                    takeNext = true;
                }
                else
                {
                    takeNext = false;
                }
            }
        }
        yield return Tuple.Create(name, list);
    }

    private static string GetName(string line)
    {
        return line.Split(sep, StringSplitOptions.RemoveEmptyEntries)[1];
    }

    private static string GetBody(string line)
    {
        if (line.Last() == '\\')
        {
            line = line.Substring(0, line.Length - 1);
        }
        line = line.Trim();
        var start = line.IndexOf('\"');
        var body = line.Substring(start);
        body = body.Replace("PRESSKEY", " + PRESSKEY");
        body = body.Replace("PRESSYN", " + PRESSYN");
        return body;
    }
}
