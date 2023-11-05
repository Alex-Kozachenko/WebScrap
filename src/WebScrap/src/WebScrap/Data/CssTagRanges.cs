using System.Collections.Immutable;

namespace ProSol.WebScrap.Data;

public record struct CssTagRanges(string Css, ImmutableArray<Range> TagRanges);
