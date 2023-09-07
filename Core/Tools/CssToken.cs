namespace Core.Tools;

internal record struct CssToken(char? ChildSelector, ReadOnlyMemory<char> Css);