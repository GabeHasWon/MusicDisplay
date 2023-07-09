namespace MusicDisplay;

internal readonly struct MusicText
{
    public readonly string MainText;
    public readonly string Author;
    public readonly string Subtitle;
    public readonly bool IsUnknown;

    public MusicText(string mainText, string author, string subtitle, bool isUnknown = false)
    {
        MainText = mainText;
        Author = author;
        Subtitle = subtitle;
        IsUnknown = isUnknown;
    }
}
