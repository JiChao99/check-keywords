using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Shared.Handlers
{
    public class OpenAPIHandler : CheckKeywordsHandler
    {
        public override List<string> BuildNeedCheckWords(string str)
        {
            var words = new List<string>();
            var jsonDocument = JsonDocument.Parse(str, new JsonDocumentOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            });

            if (jsonDocument.RootElement.TryGetProperty("paths", out JsonElement paths) && paths.ValueKind == JsonValueKind.Object)
            {
                foreach (var item in paths.EnumerateObject())
                {
                    if (item.Value.ValueKind == JsonValueKind.Object)
                    {
                        var path = item.Value.EnumerateObject();
                        foreach (var pathItem in path)
                        {
                            if (pathItem.Value.TryGetProperty("parameters", out JsonElement parameters) && parameters.ValueKind == JsonValueKind.Array)
                            {
                                var parameterEnumerateArray = parameters.EnumerateArray();
                                foreach (var paramItem in parameterEnumerateArray)
                                {
                                    if (paramItem.TryGetProperty("name", out JsonElement name))
                                    {
                                        words.Add(name.GetString());
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (jsonDocument.RootElement.TryGetProperty("components", out JsonElement definitions) && definitions.ValueKind == JsonValueKind.Object)
            {
                foreach (var item in definitions.EnumerateObject())
                {
                    if ("schemas".Equals(item.Name)) 
                    {
                        foreach (var schemasItem in item.Value.EnumerateObject())
                        {
                            if (schemasItem.Value.TryGetProperty("properties", out JsonElement properties) && properties.ValueKind == JsonValueKind.Object)
                            {
                                var propertiesEnumerateObject = properties.EnumerateObject();
                                foreach (var propertyItem in propertiesEnumerateObject)
                                {
                                    words.Add(propertyItem.Name);
                                }
                            }
                        }
                    }
                }
            }

            return words.Distinct().ToList();
        }
    }
}
