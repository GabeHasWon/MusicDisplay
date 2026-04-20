using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MusicDisplay;

#nullable enable

internal class MusicDatabase : ModSystem
{
    internal delegate bool PreDisplay(ref string nowText, ref string title, ref string author, ref string sub, ref float baseScale, Color[] colors, ref float delta, float defaultMaxDelta,
        ref float x, ref float y, ref Vector2 originMod, ref float baseAlpha, float? alwaysOn);

    internal static Dictionary<short, PreDisplay> PreDrawById = [];

    private static MusicText UnknownTrack;

    private Dictionary<short, MusicText> _tracksById = [];

    public override void PostAddRecipes()
    {
        for (short i = 1; i < MusicID.Count; ++i)
        {
            var trackName = Language.GetText("Mods.MusicDisplay.TrackNames.Vanilla." + i);
            var byLine = Language.GetText("Mods.MusicDisplay.TrackNames.VanillaByLine");
            var terraria = Language.GetText("Mods.MusicDisplay.TrackNames.TerrariaName");

            if (i is >= MusicID.OtherworldlyRain and <= MusicID.OtherworldlyHallow)
            {
                terraria = Language.GetText("Mods.MusicDisplay.TrackNames.OtherworldName");

                if (i == MusicID.OtherworldlyJungle)
                    byLine = Language.GetText("Mods.MusicDisplay.TrackNames.OtherworldTinaGuo");
                else if (i == MusicID.OtherworldlyUnderground)
                    byLine = Language.GetText("Mods.MusicDisplay.TrackNames.OtherworldJeffBall");
                else if (i is MusicID.OtherworldlyBoss2 or MusicID.OtherworldlyLunarBoss)
                    byLine = Language.GetText("Mods.MusicDisplay.TrackNames.OtherworldFrankKlepacki");
                else
                    byLine = Language.GetText("Mods.MusicDisplay.TrackNames.OtherworldByLine");
            }

            _tracksById.Add(i, new(trackName, byLine, terraria));
        }

        UnknownTrack = new MusicText(Language.GetText("Mods.MusicDisplay.TrackNames.UnknownTrack"), LocalizedText.Empty, LocalizedText.Empty, true);
    }

    public override void Unload()
    {
        _tracksById = null!;
        PreDrawById = null!;
    }

    internal static MusicText GetMusicText(short lastMusicSlot)
    {
        var db = ModContent.GetInstance<MusicDatabase>();
        return !db._tracksById.TryGetValue(lastMusicSlot, out MusicText value) ? UnknownTrack : value;
    }

    internal static void AddMusic(object id, object name, object subTitle) 
        => throw new Exception("[MusicDisplay] Use the short, LocalizedText, LocalizedText, LocalizedText overload! This overload is outdated.");

    internal static void AddMusic(object id, object name, object author, object subTitle, object displayCondition, object? color = null)
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

        Func<bool>? realDisplayCondition = null;

        if (displayCondition is not null)
        {
            if (displayCondition is Func<bool> condition)
                realDisplayCondition = condition;
            else
                throw new ArgumentException("displayCondition must be a Func<bool>!");
        }

        var db = ModContent.GetInstance<MusicDatabase>();
        db._tracksById.Add(realID, new MusicText(realName, realAuthor, realSub, shouldDisplay: realDisplayCondition, colors: (Color[]?)color));
    }

    internal static object GetMusicInfo(short id)
    {
        if (id == 0)
            throw new ArgumentException("A music ID of 0 isn't valid!");

        if (!ModContent.GetInstance<MusicDatabase>()._tracksById.TryGetValue(id, out var text) || text.IsUnknown)
            throw new ArgumentException($"Music ID {id} isn't registered, or is the placeholder \"Unknown\" track.");

        object[] info = [text.MainText, text.Author, text.Subtitle, text.ShouldDisplay];
        return info;
    }

    internal static object TryGetMusicInfo(short id)
    {
        if (id == 0)
            return (false, Array.Empty<object>(), "A music ID of 0 isn't valid!");

        if (!ModContent.GetInstance<MusicDatabase>()._tracksById.TryGetValue(id, out var text) || text.IsUnknown)
            return (false, Array.Empty<object>(), $"Music ID {id} isn't registered, or is the placeholder \"Unknown\" track.");

        object[] info = [text.MainText, text.Author, text.Subtitle, text.ShouldDisplay];
        return (true, info, "");
    }

    internal static bool HasMusic(short id) => id != 0 && ModContent.GetInstance<MusicDatabase>()._tracksById.ContainsKey(id);

    internal static object AddPreDraw(object[] objects)
    {
        if (objects.Length < 2)
            throw new ArgumentException("You must pass an appropriate delegate and either a short[] or short!");

        const string DelegateBindFailureMessage = "preDrawHook must be a delegate matching the signature " +
            "(ref string mainText, ref string author, ref string subTitle, ref string nowPlaying, Color[] colors)!";

        if (objects[0] is not Delegate preDraw)
            throw new ArgumentException(DelegateBindFailureMessage);

        if ((PreDisplay)Delegate.CreateDelegate(typeof(PreDisplay), preDraw.Method) is not PreDisplay displayDelegate)
            throw new ArgumentException(DelegateBindFailureMessage);

        short[] ids;

        if (objects[0] is short id)
            ids = [id];
        else if (objects[1] is short[] convIds)
            ids = convIds;
        else
            throw new ArgumentException("Second argument must be either a short[] or short!");

        foreach (short newId in ids)
            PreDrawById.Add(newId, displayDelegate);

        return true;
    }
}
