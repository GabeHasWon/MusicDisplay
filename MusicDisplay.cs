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
            if (args.Length == 7)
                MusicDatabase.AddMusic(args[1], args[2], args[3], args[4], args[5], args[6]);
        }
        else if (str == "getmusictext")
            return MusicDatabase.GetMusicText((short)args[1]);
        else if (str == "getmusicinfo")
            return MusicDatabase.GetMusicInfo((short)args[1]);
        else if (str == "trygetmusicinfo")
            return MusicDatabase.TryGetMusicInfo((short)args[1]);
        else if (str == "hasmusic")
            return MusicDatabase.HasMusic((short)args[1]);
        else if (str == "addpredraw")
            return MusicDatabase.AddPreDraw(args[1..]);

        return null;
    }
}