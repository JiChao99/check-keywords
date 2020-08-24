using Newtonsoft.Json;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Shared.Handlers
{
    public abstract class CheckKeywordsHandler
    {
        public CheckResult Check(string str)
        {
            var result = new CheckResult();
            result.Items = new List<LanguageWordsItem>();
            var allKeywords = GetAllKeyWords();
            var words = BuildNeedCheckWords(str);

            var existsKeywords = allKeywords.Select(t => new
            {
                t.Language,
                t.Ref,
                keyWords = t.Keywords.Join(words, o => o, i => i, (o, i) => new { i }).Select(t => t.i).ToList()
            }).Where(t => t.keyWords.Any()).ToList();

            var existsSaveWords = allKeywords.Select(t => new
            {
                t.Language,
                t.Ref,
                SaveWords = t.SaveWords.Join(words, o => o, i => i, (o, i) => new { i }).Select(t => t.i).ToList()
            }).Where(t => t.SaveWords.Any()).ToList();

            if (!existsKeywords.Any() && !existsSaveWords.Any())
            {
                result.Result = (int)EnumCheckResult.Success;
            }
            else if (!existsKeywords.Any() && existsSaveWords.Any())
            {
                result.Result = (int)EnumCheckResult.Available;
                result.Items = existsSaveWords.Select(t => new LanguageWordsItem { Type = 2, Language = t.Language, Words = t.SaveWords, Ref = t.Ref }).ToList();
            }
            else
            {
                result.Result = (int)EnumCheckResult.Disable;
                result.Items.AddRange(existsKeywords.Select(t => new LanguageWordsItem
                {
                    Language = t.Language,
                    Type = 1,
                    Words = t.keyWords,
                    Ref = t.Ref
                }));
                if (existsSaveWords.Any())
                {
                    result.Items.AddRange(existsSaveWords.Select(t => new LanguageWordsItem
                    {
                        Language = t.Language,
                        Type = 2,
                        Words = t.SaveWords,
                        Ref = t.Ref
                    }));
                }
            }

            return result;
        }

        public abstract List<string> BuildNeedCheckWords(string str);

        public static List<KeyWords> GetAllKeyWords()
        {
            try
            {
                var str = "[{\"language\":\"C#\",\"ref\":\"https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/\",\"keywords\":[\"abstract\",\"as\",\"base\",\"bool\",\"break\",\"byte\",\"case\",\"catch\",\"char\",\"checked\",\"class\",\"const\",\"continue\",\"decimal\",\"default\",\"delegate\",\"do\",\"double\",\"else\",\"enum\",\"event\",\"explicit\",\"extern\",\"false\",\"finally\",\"fixed\",\"float\",\"for\",\"foreach\",\"goto\",\"if\",\"implicit\",\"in\",\"int\",\"interface\",\"internal\",\"is\",\"lock\",\"long\",\"namespace\",\"new\",\"null\",\"object\",\"operator\",\"out\",\"override\",\"params\",\"private\",\"protected\",\"public\",\"readonly\",\"ref\",\"return\",\"sbyte\",\"sealed\",\"short\",\"sizeof\",\"stackalloc\",\"static\",\"string\",\"struct\",\"switch\",\"this\",\"throw\",\"true\",\"try\",\"typeof\",\"uint\",\"ulong\",\"unchecked\",\"unsafe\",\"ushort\",\"using\",\"virtual\",\"void\",\"volatile\",\"while\"],\"saveWords\":[\"add\",\"alias\",\"ascending\",\"async\",\"await\",\"by\",\"descending\",\"dynamic\",\"equals\",\"from\",\"get\",\"global\",\"group\",\"into\",\"join\",\"let\",\"nameof\",\"on\",\"orderby\",\"partial\",\"remove\",\"select\",\"set\",\"unmanaged\",\"value\",\"var\",\"when\",\"where\",\"yield\"]},{\"language\":\"javascript\",\"ref\":\"https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Lexical_grammar#Keywords\",\"keywords\":[\"break\",\"case\",\"catch\",\"class\",\"const\",\"continue\",\"debugger\",\"default\",\"delete\",\"do\",\"else\",\"export\",\"extends\",\"finally\",\"for\",\"function\",\"if\",\"import\",\"in\",\"instanceof\",\"new\",\"return\",\"super\",\"switch\",\"this\",\"throw\",\"try\",\"typeof\",\"var\",\"void\",\"while\",\"with\",\"yield\"],\"saveWords\":[\"enum\",\"implements\",\"interface\",\"let\",\"package\",\"private\",\"protected\",\"public\",\"static\",\"await\",\"abstract\",\"boolean\",\"byte\",\"char\",\"double\",\"final\",\"float\",\"goto\",\"int\",\"long\",\"native\",\"short\",\"synchronized\",\"throws\",\"transient\",\"volatile\"]}]";
                return JsonConvert.DeserializeObject<List<KeyWords>>(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default;
            }

        }
    }
}
