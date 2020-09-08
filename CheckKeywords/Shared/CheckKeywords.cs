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
                JsonSerializer.Deserialize<object>(str, new JsonSerializerOptions
                {
                    // 允许注释和尾随逗号
                    // https://docs.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json-how-to#allow-comments-and-trailing-commas
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true

                    // 注：System.Text.Json 无法完全兼容 Newtonsoft.Json，例如不规范的写法会抛出异常，参见：
                    // https://docs.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to#json-strings-property-names-and-string-values
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
