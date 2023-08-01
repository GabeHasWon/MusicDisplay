using Terraria.Localization;

namespace MusicDisplay;

internal readonly struct MusicText
{
    public readonly LocalizedText MainText;
    public readonly LocalizedText Author;
    public readonly LocalizedText Subtitle;
    public readonly bool IsUnknown;

    public MusicText(LocalizedText mainText, LocalizedText author, LocalizedText subtitle, bool isUnknown = false)
    {
        MainText = mainText;
        Author = author;
        Subtitle = subtitle;
        IsUnknown = isUnknown;
    }
}
