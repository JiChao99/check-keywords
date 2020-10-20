using Shared.Model;
using System;
using System.Linq;
using System.Text.Json;

namespace Shared
{
    public class CheckKeywords
    {
        public static CheckResult Check(EnumCheckType checkType, string checkValue)
        {
            var hander = CheckKeywordsFactory.CreateHandler(checkType);

            return hander.Check(checkValue);
        }
        public static CheckResult Check(string checkValue)
        {
            return Check(GetCheckType(checkValue), checkValue);
        }

        public static EnumCheckType GetCheckType(string str)
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(str, new JsonDocumentOptions
                {
                    AllowTrailingCommas = true,
                    CommentHandling = JsonCommentHandling.Skip
                });
                if (jsonDocument.RootElement.ValueKind == JsonValueKind.Object)
                {
                    if (jsonDocument.RootElement.TryGetProperty("swagger", out _))
                    {
                        return EnumCheckType.Swagger;
                    }
                    else if (jsonDocument.RootElement.TryGetProperty("openapi", out _))
                    {
                        return EnumCheckType.OpenApi;
                    }
                }
                return EnumCheckType.Json;
            }
            catch
            {
                return EnumCheckType.Words;
            }
        }
    }
}
