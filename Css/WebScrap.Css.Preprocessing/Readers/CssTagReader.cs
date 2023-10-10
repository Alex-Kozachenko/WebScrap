using WebScrap.Css.Common.Tags;

namespace WebScrap.Css.Preprocessing.Readers;

internal readonly ref struct CssTagReader
{
    private readonly char[] delimeters = [' ', '>'];
    private readonly ReadOnlySpan<char> css;

    public CssTagReader(ReadOnlySpan<char> css)
    {
        this.css = css.LastIndexOfAny(delimeters) switch 
        {
            -1 => css,
            var i => css[i..][1..]
        };
    }

    internal static int Read(ReadOnlySpan<char> css, out CssTagBase tag)
        => new CssTagReader(css).Read(out tag);

    private int Read(out CssTagBase tag)
    {
        if (css.Length == 1 && css[0] == '*')
        {
            tag = new AnyCssTag();
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
            0 => new AnyCssTag(),
            _ => new CssTag(css[^processed..].ToString()),
        };

        return processed;
    }
}