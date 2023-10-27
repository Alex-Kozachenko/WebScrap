using System.Text.Json.Nodes;

namespace WebScrap.API.Data;

public interface IScrapResult
{
    JsonArray AsJson();
}
