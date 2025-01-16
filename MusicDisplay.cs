using System;
using Terraria.ModLoader;

namespace MusicDisplay;

public class MusicDisplay : Mod 
{
    public override object Call(params object[] args)
    {
        if (args[0] is not string str)
            throw new ArgumentException("args[0] is not a string!", nameof(args));

        str = str.ToLower();

        if (str == "addmusic")
        {
            if (args.Length <= 4)
                MusicDatabase.AddMusic(args[1], args[2], args[3]);
            if (args.Length == 5)
                MusicDatabase.AddMusic(args[1], args[2], args[3], args[4], null);
            if (args.Length == 6)
                MusicDatabase.AddMusic(args[1], args[2], args[3], args[4], args[5]);
        }

        return null;
    }
}