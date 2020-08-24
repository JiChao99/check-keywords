using NUnit.Framework;
using Shared;
using System.Linq;

namespace Test
{
    public class Tests
    {

        [Test]
        public void Test1()
        {
            var result = CheckKeywords.Check("public,function");
            if (result.Result == 3 && result.Items.Any(t => t.Type == 1 && t.Language == "C#") 
                && result.Items.Any(t => t.Type == 2 && t.Language == "javascript")
                && result.Items.Any(t => t.Type == 1 && t.Language == "javascript"))
            {
                Assert.Pass("pass");
            }
            else
            {
                Assert.Fail("fail");
            }
        }
    }
}