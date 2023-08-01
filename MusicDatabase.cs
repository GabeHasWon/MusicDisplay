using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MusicDisplay;

internal class MusicDatabase : ModSystem
{
    private static MusicText UnknownTrack;

    Dictionary<short, MusicText> _tracksById = new();

    public override void PostAddRecipes()
    {
        for (short i = 1; i < MusicID.Count; ++i)
        {
            var trackName = Language.GetText("Mods.MusicDisplay.TrackNames.Vanilla." + i);
            var byLine = Language.GetText("Mods.MusicDisplay.TrackNames.VanillaByLine");
            var terraria = Language.GetText("Mods.MusicDisplay.TrackNames.TerrariaName");
            _tracksById.Add(i, new(trackName, byLine, terraria));
        }

        UnknownTrack = new MusicText(Language.GetText("Mods.MusicDisplay.TrackNames.UnknownTrack"), LocalizedText.Empty, LocalizedText.Empty);
    }

    public override void Unload() => _tracksById = null;

    internal static MusicText GetMusicText(short lastMusicSlot)
    {
        var db = ModContent.GetInstance<MusicDatabase>();

        if (!db._tracksById.ContainsKey(lastMusicSlot))
            return UnknownTrack;

        return db._tracksById[lastMusicSlot];
    }

    internal static void AddMusic(object id, object name, object subTitle)
    {
        throw new Exception("[MusicDisplay] Use the short, LocalizedText, LocalizedText, LocalizedText overload! This overload is outdated.");
    }

    internal static void AddMusic(object id, object name, object author, object subTitle)
    {
        if (id is not short realID)
            throw new ArgumentException("id is not a short!");

        if (author is not LocalizedText realAuthor)
        {
            if (author is string strAuth)
                realAuthor = Language.GetText(strAuth);
            else
                throw new ArgumentException("author is not a LocalizedText or a localization key!");
        }

        if (name is not LocalizedText realName)
        {
            if (name is string strName)
                realName = Language.GetText(strName);
            else
                throw new ArgumentException("name is not a LocalizedText or a localization key!");
        }

        if (subTitle is not LocalizedText realSub)
        {
            if (subTitle is string strSub)
                realSub = Language.GetText(strSub);
            else
                throw new ArgumentException("subTitle is not a LocalizedText or a localization key!");
        }

        var db = ModContent.GetInstance<MusicDatabase>();
        db._tracksById.Add(realID, new MusicText(realName, realAuthor, realSub));
    }
}
