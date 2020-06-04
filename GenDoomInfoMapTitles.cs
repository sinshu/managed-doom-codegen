using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenDoomInfoMapTitles
{
    public static void Run()
    {
        Console.Write("GenDoomInfoMapTitles...");

        using (var writer = new StreamWriter("DoomInfo.MapTitles.cs"))
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine();
            writer.WriteLine("namespace ManagedDoom");
            writer.WriteLine("{");
            writer.WriteLine("    public static partial class DoomInfo");
            writer.WriteLine("    {");
            writer.WriteLine("        public static class MapTitles");
            writer.WriteLine("        {");

            writer.WriteLine("            public static IReadOnlyList<IReadOnlyList<DoomString>> Doom = new DoomString[][]");
            writer.WriteLine("            {");
            for (var e = 0; e < 4; e++)
            {
                writer.WriteLine("                new DoomString[]");
                writer.WriteLine("                {");
                for (var m = 0; m < 9; m++)
                {
                    writer.Write("                    Strings.HUSTR_E" + (e + 1) + "M" + (m + 1));
                    if (m < 8)
                    {
                        writer.WriteLine(",");
                    }
                    else
                    {
                        writer.WriteLine();
                    }
                }
                writer.Write("                }");
                if (e < 3)
                {
                    writer.WriteLine(",");
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine();
                }
            }
            writer.WriteLine("            };");

            writer.WriteLine();

            writer.WriteLine("            public static IReadOnlyList<DoomString> Doom2 = new DoomString[]");
            writer.WriteLine("            {");
            for (var map = 0; map < 32; map++)
            {
                writer.Write("                Strings.HUSTR_" + (map + 1));
                if (map < 31)
                {
                    writer.WriteLine(",");
                }
                else
                {
                    writer.WriteLine();
                }
            }
            writer.WriteLine("            };");

            writer.WriteLine();

            writer.WriteLine("            public static IReadOnlyList<DoomString> Plutonia = new DoomString[]");
            writer.WriteLine("            {");
            for (var map = 0; map < 32; map++)
            {
                writer.Write("                Strings.PHUSTR_" + (map + 1));
                if (map < 31)
                {
                    writer.WriteLine(",");
                }
                else
                {
                    writer.WriteLine();
                }
            }
            writer.WriteLine("            };");

            writer.WriteLine();

            writer.WriteLine("            public static IReadOnlyList<DoomString> Tnt = new DoomString[]");
            writer.WriteLine("            {");
            for (var map = 0; map < 32; map++)
            {
                writer.Write("                Strings.THUSTR_" + (map + 1));
                if (map < 31)
                {
                    writer.WriteLine(",");
                }
                else
                {
                    writer.WriteLine();
                }
            }
            writer.WriteLine("            };");

            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        Console.WriteLine("OK");
    }
}
