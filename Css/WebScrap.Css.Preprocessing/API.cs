using WebScrap.Css.Common;

namespace WebScrap.Css.Preprocessing;

public static class API
{
    public static CssToken[] Process(ReadOnlySpan<char> css)
        => CssTokensBuilder.Build(css);
}