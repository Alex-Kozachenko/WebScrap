using WebScrap.Common.Css;

namespace WebScrap.Css.Preprocessing;

public static class PreprocessingAPI
{
    public static CssToken[] Process(ReadOnlySpan<char> css)
        => Builders.CssTokensBuilder.Build(css);
}