using System.Collections.Immutable;

namespace DevOvercome.WebScrap.Data;

public record struct CssTagRanges(string Css, ImmutableArray<Range> TagRanges);
