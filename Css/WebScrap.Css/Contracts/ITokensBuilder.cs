using WebScrap.Css.Data;

namespace WebScrap.Css.Contracts;

public interface ITokensBuilder
{
    public CssToken[] Build(ReadOnlySpan<char> css);
}