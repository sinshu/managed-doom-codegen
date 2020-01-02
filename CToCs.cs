using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class CToCs
{
    private static Dictionary<string, string> mobjFlagDic;

    static CToCs()
    {
        mobjFlagDic = new Dictionary<string, string>();
        mobjFlagDic.Add("MF_SPECIAL", "Special");
        mobjFlagDic.Add("MF_SOLID", "Solid");
        mobjFlagDic.Add("MF_SHOOTABLE", "Shootable");
        mobjFlagDic.Add("MF_NOSECTOR", "NoSector");
        mobjFlagDic.Add("MF_NOBLOCKMAP", "NoBlockmap");
        mobjFlagDic.Add("MF_AMBUSH", "Ambush");
        mobjFlagDic.Add("MF_JUSTHIT", "JustHit");
        mobjFlagDic.Add("MF_JUSTATTACKED", "JustAttacked");
        mobjFlagDic.Add("MF_SPAWNCEILING", "SpawnCeiling");
        mobjFlagDic.Add("MF_NOGRAVITY", "NoGravity");
        mobjFlagDic.Add("MF_DROPOFF", "DropOff");
        mobjFlagDic.Add("MF_PICKUP", "PickUp");
        mobjFlagDic.Add("MF_NOCLIP", "NoClip");
        mobjFlagDic.Add("MF_SLIDE", "Slide");
        mobjFlagDic.Add("MF_FLOAT", "Float");
        mobjFlagDic.Add("MF_TELEPORT", "Teleport");
        mobjFlagDic.Add("MF_MISSILE", "Missile");
        mobjFlagDic.Add("MF_DROPPED", "Dropped");
        mobjFlagDic.Add("MF_SHADOW", "Shadow");
        mobjFlagDic.Add("MF_NOBLOOD", "NoBlood");
        mobjFlagDic.Add("MF_CORPSE", "Corpse");
        mobjFlagDic.Add("MF_INFLOAT", "InFloat");
        mobjFlagDic.Add("MF_COUNTKILL", "CountKill");
        mobjFlagDic.Add("MF_COUNTITEM", "CountItem");
        mobjFlagDic.Add("MF_SKULLFLY", "SkullFly");
        mobjFlagDic.Add("MF_NOTDMATCH", "NotDmatch");
        mobjFlagDic.Add("MF_TRANSLATION", "Translation");
        mobjFlagDic.Add("MF_TRANSSHIFT", "TransShift");
    }

    public static string MobjFlag(string value)
    {
        if (value == "0")
        {
            return value;
        }

        return mobjFlagDic[value];
    }

    public static string MobjFlagF(string value)
    {
        if (value == "0")
        {
            return value;
        }

        return "MobjFlags." + mobjFlagDic[value];
    }

    public static string MobjType(string value)
    {
        var split = value.Split('_');
        return ToUpperFirst(split[1]);
    }

    public static string Sfx(string value)
    {
        if (value == "0")
        {
            return "NONE";
        }

        var split = value.Split('_');
        return split[1].ToUpper();
    }

    public static string State(string value)
    {
        if (value == "0")
        {
            return "Null";
        }

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
