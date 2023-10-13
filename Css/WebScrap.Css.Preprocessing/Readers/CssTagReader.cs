using WebScrap.Css.Data.Tags;

namespace WebScrap.Css.Preprocessing.Readers;

internal readonly ref struct CssTagReader(ReadOnlySpan<char> css)
{
    private readonly ReadOnlySpan<char> css = css;

    internal static int Read(ReadOnlySpan<char> css, out CssTagBase tag)
        => new CssTagReader(css).Read(out tag);

    private int Read(out CssTagBase tag)
    {
        if (css.Length == 0)
        {
            tag = new WildcardCssTag();
            return 0;
        }
        
        if (css[^1] == '*')
        {
            tag = new WildcardCssTag();
            return 1;
        }

        var processed = 0;
        for (int i = css.Length - 1; i >= 0; i--)
        {
            if (char.IsLetter(css[i]))
            {
                processed++;
            }
            else 
            {
                break;
            }
        };

        tag = processed switch
        {
            0 => new WildcardCssTag(),
            _ => new CssTag(css[^processed..].ToString()),
        };

        return processed;
    }
}