using System.Text.Json.Nodes;

namespace DevOvercome.WebScrap.Data;

public interface IScrapResult
{
    JsonArray AsJson();
}
