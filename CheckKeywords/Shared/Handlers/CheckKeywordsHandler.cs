using Newtonsoft.Json;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
                var assembly = Assembly.GetExecutingAssembly();
                var configStream = assembly.GetManifestResourceStream("Shared.keywords.json");
                var streamReader = new StreamReader(configStream);
                var str = streamReader.ReadToEnd();

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
