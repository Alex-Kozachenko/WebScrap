using System.Text.Json.Nodes;

namespace ProSol.WebScrap.Data;

public interface IScrapResult
{
    JsonArray AsJson();
}
