namespace WebScrap.API.Contracts;

public record struct Config(OutputFormatType OutputFormatType = OutputFormatType.Html);