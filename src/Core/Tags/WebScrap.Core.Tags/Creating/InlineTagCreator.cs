namespace WebScrap.Core.Tags.Creating;

internal readonly ref struct InlineTagCreator(ReadOnlySpan<char> html)
{
    private readonly ReadOnlySpan<char> html = html;

    internal int Proceed() 
        =>  1 + TagsNavigator.GetNextTagIndex(html[1..]);
}