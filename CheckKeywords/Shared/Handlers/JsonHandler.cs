using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Handlers
{
    public class JsonHandler : CheckKeywordsHandler
    {
        public override List<string> BuildNeedCheckWords(string str)
        {
            var words = new List<string>();
            var jsonObject = JObject.Parse(str);
            BuildJsonAllKey(jsonObject, words);

            return words.Distinct().ToList();

        }

        private static void BuildJsonAllKey(JObject jObject, List<string> keys)
        {
            foreach (var item in jObject.Cast<KeyValuePair<string, JToken>>().ToList())
            {
                keys.Add(item.Key);

                if (item.Value is JObject itemValueJObject)
                {
                    BuildJsonAllKey(itemValueJObject, keys);
                    continue;
                }
            }
        }
    }
}
