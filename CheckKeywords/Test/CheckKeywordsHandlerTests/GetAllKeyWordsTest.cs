using NUnit.Framework;
using Shared.Handlers;
using System.Linq;

namespace Test.CheckKeywordsHandlerTests
{
    public class GetAllKeyWordsTest
    {
        [Test]
        public void CanGetAllKeyWordsFromAssemblyTest()
        {
            var words = CheckKeywordsHandler.GetAllKeyWords();

            Assert.IsTrue(words.Any());

            var languages = new string[] { "C#", "javascript" };
            Assert.IsTrue(words.All(i => languages.Contains(i.Language)));

            var keywords = new string[] { "abstract", "object", "while", "break", "if", "yield" };
            Assert.IsTrue(keywords.Intersect(words.SelectMany(i => i.Keywords)).All(i => keywords.Contains(i)));

            var saveWords = new string[] { "add", "global", "var", "enum", "goto", "volatile" };
            Assert.IsTrue(keywords.Intersect(words.SelectMany(i => i.Keywords)).All(i => keywords.Contains(i)));
        }
    }
}
