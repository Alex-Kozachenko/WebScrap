namespace WebScrap.Modules.Extracting.Html.Contracts.Data;

public record struct Table(string[] Header, string[][] ValueRows);
