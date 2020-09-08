using Shared.Model;
using System;
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
            if (IsJson(checkValue))
            {
                return Check(EnumCheckType.Json, checkValue);
            }
            else if (Uri.IsWellFormedUriString(checkValue, UriKind.Absolute))
            {
                return Check(EnumCheckType.SwaggerUrl, checkValue);
            }
            else
            {
                return Check(EnumCheckType.Word, checkValue);
            }
        }

        public static bool IsJson(string str)
        {
            try
            {
                JsonDocument.Parse(str, new JsonDocumentOptions
                {
                    AllowTrailingCommas = true,
                    CommentHandling = JsonCommentHandling.Skip
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
