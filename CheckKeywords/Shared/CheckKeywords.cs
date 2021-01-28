using Shared.Handlers;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Shared
{
    public class CheckKeywords
    {
        public static CheckResult Check(EnumCheckType checkType, string checkValue, List<string> LanguageNames)
        {
            var hander = CheckKeywordsFactory.CreateHandler(checkType);

            return hander.Check(checkValue, LanguageNames);
        }
        public static CheckResult Check(string checkValue, List<string> LanguageNames = null)
        {
            return Check(GetCheckType(checkValue), checkValue, LanguageNames);
        }
        public static List<string> GetAllProgramNames()
        {
            return CheckKeywordsHandler.GetAllKeyWords().Select(t => t.Language).ToList();
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
