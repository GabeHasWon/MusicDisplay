using System;
using System.Collections.Generic;
using System.Text;
using Terraria.ID;
using Terraria.ModLoader;

namespace MusicDisplay;

internal class MusicDatabase : ILoadable
{
    Dictionary<short, MusicText> _tracksById = new();

    public void Load(Mod mod)
    {
        Type musicID = typeof(MusicID);
        
        foreach (var item in musicID.GetFields())
        {
            short id = (short)item.GetValue(null);
            string rawName = item.Name;

            if (id != MusicID.Count)
                _tracksById.Add(id, new(AddSpaces(rawName), "by Scott Lloyd Shelly", "Terraria"));
        }
    }

    private static string AddSpaces(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";

        StringBuilder newText = new(text.Length * 2);
        newText.Append(text[0]);

        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText.Append(' ');

            newText.Append(text[i]);
        }
        return newText.ToString();
    }

    public void Unload()
    {
        _tracksById = null;
    }

    internal static MusicText GetMusicText(short lastMusicSlot)
    {
        var db = ModContent.GetInstance<MusicDatabase>();

        if (!db._tracksById.ContainsKey(lastMusicSlot))
            return new MusicText("(Unknown Track)", "", "", true);

        return db._tracksById[lastMusicSlot];
    }

    internal static void AddMusic(object id, object name, object subTitle)
    {
        if (id is not short realID)
            throw new ArgumentException("id is not a short!");

        if (name is not string realName)
            throw new ArgumentException("name is not a string!");

        string realSub = subTitle is string ? (string)subTitle : string.Empty;

        var db = ModContent.GetInstance<MusicDatabase>();
        db._tracksById.Add(realID, new MusicText(realName, "", realSub));
    }

    internal static void AddMusic(object id, object name, object author, object subTitle)
    {
        if (id is not short realID)
            throw new ArgumentException("id is not a short!");

        if (author is not string realAuthor)
            throw new ArgumentException("id is not a short!");

        if (name is not string realName)
            throw new ArgumentException("name is not a string!");

        string realSub = subTitle is string ? (string)subTitle : string.Empty;

        var db = ModContent.GetInstance<MusicDatabase>();
        db._tracksById.Add(realID, new MusicText(realName, realAuthor, realSub));
    }
}
