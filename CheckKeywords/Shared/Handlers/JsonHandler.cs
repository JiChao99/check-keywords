using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Shared.Handlers
{
    public class JsonHandler : CheckKeywordsHandler
    {
        public override List<string> BuildNeedCheckWords(string str)
        {
            var jsonDocument = JsonDocument.Parse(str, new JsonDocumentOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            });

            var words = new List<string>();
            BuildJsonAllKey(jsonDocument.RootElement, words);

            return words.Distinct().ToList();
        }

        private static void BuildJsonAllKey(JsonElement jsonElement, List<string> keys)
        {
            if (jsonElement.ValueKind != JsonValueKind.Object)
            {
                return;
            }

            foreach (var jsonProperty in jsonElement.EnumerateObject())
            {
                keys.Add(jsonProperty.Name);
                BuildJsonAllKey(jsonProperty.Value, keys);
            }
        }
    }
}
