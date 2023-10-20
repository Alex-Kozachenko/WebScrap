﻿using System.Text.Json.Nodes;
using WebScrap.Modules.Exporting.Json.Processors;

namespace WebScrap.Modules.Exporting.Json;

public class JsonApi
{
    public static JsonArray Export(IEnumerable<ReadOnlyMemory<char>> tags) 
        => new JsonProcessor().Process(tags);
}
