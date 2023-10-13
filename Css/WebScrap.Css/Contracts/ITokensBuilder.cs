using WebScrap.Css.Data;

namespace WebScrap.Css.Contracts;

/// <summary>
/// Represents a builder 
/// which takes plain css 
/// and produces a set of css tokens object model.
/// </summary>
public interface ITokensBuilder
{
    public CssToken[] Build(ReadOnlySpan<char> css);
}