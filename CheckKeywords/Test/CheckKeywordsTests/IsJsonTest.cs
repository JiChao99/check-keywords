using NUnit.Framework;
using Shared;

namespace Test.CheckKeywordsTests
{
    public class IsJsonTest
    {
        [TestCase("{}")]
        [TestCase("[]")]
        [TestCase("{\"a\":{}}")]
        [TestCase("{\"a\": {}}")]
        [TestCase("{\"a\":null}")]
        [TestCase("{\"a\": null}")]
        [TestCase("{\"a\":{\"b\":123}}")]
        [TestCase("{\"a\":{\"b\": 123}}")]
        public void WhenInputJsonReturnTrue(string jsonStr)
        {
            var result = CheckKeywords.IsJson(jsonStr);

            Assert.IsTrue(result);
        }

        [TestCase("")]
        [TestCase("{")]
        [TestCase("{'}")]
        [TestCase("{'a':{}}")]
        [TestCase("{'a':''}")]
        [TestCase("{'a\": {}}")]
        [TestCase("{\"a': {}}")]
        [TestCase("{\"a\"}")]
        [TestCase("{123}")]
        public void WhenInputInvalidJsonReturnFalse(string jsonStr)
        {
            var result = CheckKeywords.IsJson(jsonStr);

            Assert.IsFalse(result);
        }
    }
}
