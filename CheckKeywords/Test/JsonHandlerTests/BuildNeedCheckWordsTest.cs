using NUnit.Framework;
using Shared.Handlers;
using System.Linq;

namespace Test.JsonHandlerTests
{
    public class BuildNeedCheckWordsTest
    {
        private JsonHandler _jsonHandler;

        [SetUp]
        public void SetUp()
        {
            _jsonHandler = new JsonHandler();
        }

        [Test]
        public void InputEmptyObjectReturnEmptyList()
        {
            var result = _jsonHandler.BuildNeedCheckWords("{}");

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void InputOnlyOneArrayReturnEmptyList()
        {
            var result = _jsonHandler.BuildNeedCheckWords("[]");

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void InputOneObjectReturnOneItem()
        {
            const string KEY = "function";

            var result = _jsonHandler.BuildNeedCheckWords($"{{\"{KEY}\":\"\"}}");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(KEY, result.Single());
        }

        [Test]
        public void InputNestedObjects()
        {
            const string KEY_1 = "function";
            const string KEY_2 = "action";
            const string KEY_3 = "sub";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY_1}\": {{ \"{KEY_2}\": {{ \"{KEY_3}\": {{}} }} }} }}");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(KEY_1, result[0]);
            Assert.AreEqual(KEY_2, result[1]);
            Assert.AreEqual(KEY_3, result[2]);
        }

        [Test]
        public void InputObjectOnTheSameLevel()
        {
            const string KEY_1 = "function";
            const string KEY_2 = "action";
            const string KEY_3 = "sub";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY_1}\": {{}}, \"{KEY_2}\": {{}}, \"{KEY_3}\": {{}} }}");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(KEY_1, result[0]);
            Assert.AreEqual(KEY_2, result[1]);
            Assert.AreEqual(KEY_3, result[2]);
        }

        [Test]
        public void InputObjectOnTheSameLevelAndNestedObjects()
        {
            const string KEY_1 = "function";
            const string KEY_2 = "action";
            const string KEY_3 = "sub";
            const string KEY_4 = "b";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY_1}\": {{ \"{KEY_2}\": {{}}, \"{KEY_3}\": {{}} }}, \"{KEY_4}\": {{}} }}");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(KEY_1, result[0]);
            Assert.AreEqual(KEY_2, result[1]);
            Assert.AreEqual(KEY_3, result[2]);
            Assert.AreEqual(KEY_4, result[3]);
        }

        [Test]
        public void InputObjectOnTheSameLevelAndNestedObjectsAndArray()
        {
            const string KEY_1 = "function";
            const string KEY_2 = "action";
            const string KEY_3 = "sub";
            const string KEY_4 = "b";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY_1}\": {{ \"{KEY_2}\": [], \"{KEY_3}\": {{}} }}, \"{KEY_4}\": [] }}");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(KEY_1, result[0]);
            Assert.AreEqual(KEY_2, result[1]);
            Assert.AreEqual(KEY_3, result[2]);
            Assert.AreEqual(KEY_4, result[3]);
        }

        [Test]
        public void InputEverything()
        {
            const string KEY_1 = "function";
            const string KEY_2 = "action";
            const string KEY_3 = "sub";
            const string KEY_4 = "b";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY_1}\": {{ \"{KEY_2}\": null, \"{KEY_3}\": 123 }}, \"{KEY_4}\": [] }}");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(KEY_1, result[0]);
            Assert.AreEqual(KEY_2, result[1]);
            Assert.AreEqual(KEY_3, result[2]);
            Assert.AreEqual(KEY_4, result[3]);
        }

        [Test]
        public void InputDuplicateKey()
        {
            const string KEY = "function";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY}\": {{ \"{KEY}\": null, \"{KEY}\": 123 }}, \"{KEY}\": [] }}");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(KEY, result.Single());
        }
    }
}
